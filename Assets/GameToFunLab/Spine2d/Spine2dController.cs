using GameToFunLab.Core;
using GameToFunLab.Core.Spine2d;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace GameToFunLab.Spine2d
{
    /// <summary>
    /// 스파인 컨트롤러
    /// </summary>
    public class Spine2dController : MonoBehaviour
    {
        private protected SkeletonAnimation SkeletonAnimation;

        protected virtual void Awake() {
            // Spine 오브젝트의 SkeletonAnimation 컴포넌트 가져오기
            SkeletonAnimation = GetComponent<SkeletonAnimation>();

            if (SkeletonAnimation == null)
            {
                FgLogger.LogError("SkeletonAnimation component not found!");
            }
            SkeletonAnimation.AnimationState.Event += HandleEvent;
        }

        private void HandleEvent(TrackEntry trackEntry, Event e)
        {
            // Logger.Log("effect spine event: "+e.Data.Name);
            if (e.Data.Name == SpineEventManager.eventNameHit)
            {
                // FG_Logger.Log("hit event " + this.gameObject.name + " | json: " + e.String);
                OnSpineEventHit(e);
            }
            else if (e.Data.Name == SpineEventManager.eventNameSound)
            {
                OnSpineEventSound(e);
            }
            else if (e.Data.Name == SpineEventManager.eventNameShake)
            {
                if (e.Float <= 0) return;
                OnSpineEventShake(e);
            }
        }
        protected virtual void OnSpineEventShake(Event eEvent) 
        {
        
        }
        protected virtual void OnSpineEventHit(Event eEvent) 
        {
        
        }
        protected virtual void OnSpineEventSound(Event eEvent) 
        {
        }
        protected virtual void Start()
        {
        }
        /// <summary>
        /// 애니메이션 재생
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="loop"></param>
        protected virtual void PlayAnimation(string animationName, bool loop = false)
        {
            if (SkeletonAnimation == null) return;
            //  FG_Logger.Log("PlayAnimation gameobject: " + this.gameObject.name + " / animationName: " + animationName + " / " + loop);
            SkeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
        }
        /// <summary>
        /// 현재 재생 중인 애니메이션 이름 가져오기
        /// </summary>
        /// <returns></returns>
        protected string GetCurrentAnimation()
        {
            if (SkeletonAnimation == null || SkeletonAnimation.AnimationState == null) return null;
            TrackEntry currentEntry = SkeletonAnimation.AnimationState.GetCurrent(0);
            return currentEntry?.Animation.Name;
        }
        /// <summary>
        /// 애니메이션을 한 번 실행하고, 그 후에 다른 애니메이션을 loop로 실행
        /// </summary>
        /// <param name="animationName"></param>
        protected virtual void PlayAnimationOnceAndThenLoop(string animationName)
        {
            if (SkeletonAnimation == null) return;
            // FG_Logger.Log("PlayAnimationOnceAndThenLoop gameobject: " + this.gameObject.name + " / animationName: " + animationName );
            // 애니메이션 실행
            SkeletonAnimation.AnimationState.SetAnimation(0, animationName, false);

            // 애니메이션 이벤트 리스너 등록
            SkeletonAnimation.AnimationState.Complete += OnAnimationCompleteToIdle;
        }
        /// <summary>
        /// 애니메이션이 끝나면 호출되는 콜백 함수
        /// </summary>
        /// <param name="entry"></param>
        protected virtual void OnAnimationCompleteToIdle(TrackEntry entry)
        {
            if (SkeletonAnimation == null) return;
            // 애니메이션 이벤트 리스너 제거
            SkeletonAnimation.AnimationState.Complete -= OnAnimationCompleteToIdle;

            // 다른 애니메이션 loop로 실행
            SkeletonAnimation.AnimationState.SetAnimation(0, SpineCharacter.CharacterDefaultAnimationName["idle"], true);
        }
        protected virtual void AddCompleteEvent() 
        {
            SkeletonAnimation.AnimationState.Complete += OnAnimationCompleteToIdle;
        }
        protected virtual void RemoveCompleteEvent() 
        {
            if (SkeletonAnimation == null) return;
            SkeletonAnimation.AnimationState.Complete -= OnAnimationCompleteToIdle;
        }
        protected float GetAnimationDuration(string animationName, bool isMilliseconds = true)
        {
            var findAnimation = SkeletonAnimation.Skeleton.Data.FindAnimation(animationName);

            if (findAnimation == null)
            {
                FgLogger.LogWarning($"Animation '{animationName}' not found.");
                return 0;
            }

            float duration = findAnimation.Duration;
            return isMilliseconds ? duration * 1000 : duration;
        }
        protected void SetTrackNoEnd(int trackId = 0)
        {
            if (SkeletonAnimation == null) return;
            TrackEntry trackEntry = SkeletonAnimation.AnimationState.GetCurrent(trackId);
            if(trackEntry == null) return;
            trackEntry.AnimationEnd = 999999f;
        }

        protected void StopAnimation(int trackId = 0)
        {
            if (SkeletonAnimation == null) return;
            SkeletonAnimation.AnimationState.SetEmptyAnimation(trackId, 0);
            SkeletonAnimation.AnimationState.ClearTrack(trackId);
        }
        protected bool IsPlaying()
        {
            if (SkeletonAnimation == null) return false;
            var state = SkeletonAnimation.AnimationState;
            // 각 트랙에서 현재 애니메이션이 있는지 확인
            for (int i = 0; i < state.Tracks.Count; i++)
            {
                if (state.Tracks.Items[i] != null && state.Tracks.Items[i].Animation != null)
                {
                    return true; // 재생 중인 애니메이션이 존재함
                }
            }
            return false; // 재생 중인 애니메이션이 없음
        }
    }
}
