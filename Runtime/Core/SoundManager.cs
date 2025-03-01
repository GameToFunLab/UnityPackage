using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameToFunLab.FgPlayerPrefs;
using UnityEngine;

namespace GameToFunLab.Core
{
    public class SoundManager : MonoBehaviour
    {
        public enum Type
        {
            None,
            Bgm,
            Sfx
        }
        public enum TypeSub
        {
            None,
            Player,
            Monster,
            UI,
            Skill
        }

        protected AudioSource AudioSourceDefaultGameBgm;
        protected AudioSource AudioSourceBgm2;
        public float bgmFadeDuration = 0.7f;
        protected const int UnumSoundBgmDefault = 2;

        protected AudioSource CurrentBgmAudioSource;
        protected AudioSource NextBgmAudioSource;

        private AudioClip[] bgms;

        protected readonly Dictionary<int, int> SoundPlayCount = new Dictionary<int, int>(); // Unum별 현재 재생 중인 사운드의 개수
        protected readonly Dictionary<int, int> MaxConcurrentPlays = new Dictionary<int, int>(); // Unum별 최대 동시 재생 개수

        protected readonly Dictionary<int, Queue<GameObject>> SoundSfxPoolDictionary = new Dictionary<int, Queue<GameObject>>();
        
        protected virtual void Awake()
        {
            // AudioSource 컴포넌트를 동적으로 추가
            AudioSourceDefaultGameBgm = gameObject.AddComponent<AudioSource>();
            AudioSourceBgm2 = gameObject.AddComponent<AudioSource>();
        }
        /// <summary>
        /// 배경음악 교체하기
        /// </summary>
        /// <param name="newClip"></param>
        protected void ChangeBackgroundMusic(AudioClip newClip)
        {
            if (newClip == null)
            {
                FgLogger.LogError("오디오 클립이 없습니다.");
                return;
            }
            StartCoroutine(BgmFadeOutAndIn(newClip));
        }
        /// <summary>
        /// 배경음악 교체시 fade in out
        /// </summary>
        /// <param name="newClip"></param>
        /// <returns></returns>
        private IEnumerator BgmFadeOutAndIn(AudioClip newClip)
        {
            // Fade out current audio
            float startVolume = SoundSettings.GetBGMVolume();
            while (CurrentBgmAudioSource.volume > 0)
            {
                CurrentBgmAudioSource.volume -= startVolume * Time.deltaTime / bgmFadeDuration;
                yield return null;
            }

            CurrentBgmAudioSource.Stop();
            CurrentBgmAudioSource.volume = startVolume;

            // Swap audio sources
            (CurrentBgmAudioSource, NextBgmAudioSource) = (NextBgmAudioSource, CurrentBgmAudioSource);

            // Set new clip and fade in
            CurrentBgmAudioSource.clip = newClip;
            CurrentBgmAudioSource.Play();
            CurrentBgmAudioSource.loop = true;

            CurrentBgmAudioSource.volume = 0;
            while (CurrentBgmAudioSource.volume < startVolume)
            {
                CurrentBgmAudioSource.volume += startVolume * Time.deltaTime / bgmFadeDuration;
                yield return null;
            }

            CurrentBgmAudioSource.volume = startVolume;
        }
        /// <summary>
        /// 효과음 재생하기 
        /// </summary>
        /// <param name="unum"></param>
        public void PlaySfxByUnum(int unum)
        {
            if (SoundSfxPoolDictionary.ContainsKey(unum))
            {
                GameObject soundObject = GetAvailableAudioSource(unum);
                if (soundObject != null)
                {
                    AudioSource audioSource = soundObject.GetComponent<AudioSource>();
                    soundObject.SetActive(true); // 활성화
                    audioSource.Play();
                    audioSource.volume = SoundSettings.GetSfxVolume();
                    StartCoroutine(DeactivateAfterPlay(soundObject, audioSource.clip.length));
                }
                else
                {
                    FgLogger.LogWarning("No available audio source in the pool for Unum: " + unum);
                }
            }
            else
            {
                FgLogger.LogWarning("Unum not found in the sound pool: " + unum);
            }
        }
        /// <summary>
        /// 재생이 끝난 sfx GameObject 를 비활성화 시켜준다 
        /// </summary>
        /// <param name="soundObject"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator DeactivateAfterPlay(GameObject soundObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            soundObject.SetActive(false); // 사운드 재생 후 비활성화
            SoundSfxPoolDictionary[int.Parse(soundObject.name)].Enqueue(soundObject); // 다시 풀에 추가
        }
        /// <summary>
        /// soundPoolDictionary 에서 재생 가능한 오디오 가져오기 
        /// </summary>
        /// <param name="unum"></param>
        /// <returns></returns>
        private GameObject GetAvailableAudioSource(int unum)
        {
            Queue<GameObject> pool = SoundSfxPoolDictionary[unum];
            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }
            return null; // 풀에 재생 가능한 오디오 소스가 없는 경우
        }
        /// <summary>
        /// 모든 사운드 on / off
        /// </summary>
        /// <param name="set"></param>
        public void MuteAllSound(bool set)
        {
            AudioListener.pause = set;
        }
        public void ChangeSoundVolumeBgm(float value)
        {
            if (CurrentBgmAudioSource == null) return;
            CurrentBgmAudioSource.volume = value;
            SoundSettings.SetBGMVolume(value);
        }
        public void ChangeSoundVolumeSfx(float value)
        {
            foreach (var unum in SoundSfxPoolDictionary.Keys)
            {
                SetSfxVolume(unum, value);
            }
            SoundSettings.SetSfxVolume(value);
        }
        /// <summary>
        /// sfx 볼륨 조절하기
        /// </summary>
        /// <param name="unum"></param>
        /// <param name="volume"></param>
        private void SetSfxVolume(int unum, float volume)
        {
            if (SoundSfxPoolDictionary.TryGetValue(unum, out var value))
            {
                foreach (var audioSource in value.Select(soundObject => soundObject.GetComponent<AudioSource>()).Where(audioSource => audioSource != null))
                {
                    audioSource.volume = volume;
                }
            }
            else
            {
                FgLogger.LogWarning("Unum not found in the sound pool: " + unum);
            }
        }

        public virtual void PlayByUnum(int unum)
        {
        }

        public virtual void PlayDefaultBgm()
        {
            
        }
    }
}
