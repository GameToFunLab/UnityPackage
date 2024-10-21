using System.Collections;
using System.Collections.Generic;
using GameToFunLab.Scenes;
using GameToFunLab.Utils;
using Scripts.Maps;
using Scripts.Scenes;
using Scripts.TableLoader;
using Spine.Unity;
using TMPro;
using UnityEngine;

namespace Scripts.Characters
{
    /// <summary>
    /// 몬스터 기본 클레스
    /// </summary>
    public class MyNpc : GameToFunLab.Characters.Npc
    {
        public string npcName;
        public bool isFlip;
        public List<int> questVnums;
        private MySceneGame sceneGameCustom;

        public GameObject buttonPrefab; // 버튼 프리팹
        public int buttonCount = 3; // 생성할 버튼의 수
        public float buttonSpacing = 160f; // 버튼 간격
        public float verticalSpacing = 40f; // 버튼 간격 (수직)
        private List<GameObject> interactionButtons = new List<GameObject>();
        private Canvas canvasInteractionButtons; // 버튼이 배치될 Canvas
        private Camera mainCamera; // 메인 카메라 (월드 좌표 변환에 필요)
        private SkeletonAnimation skeletonAnimation;
        private bool isStartFade;
        public NPCData npcData;

        protected override void Awake()
        {
            base.Awake();
            questVnums = new List<int>();
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            isStartFade = false;
            npcData = null;
        }
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            sceneGameCustom = MySceneGame.MyInstance;
            canvasInteractionButtons = sceneGameCustom.canvasInteractionButtons;
            mainCamera = sceneGameCustom.mainCamera;

            if (questVnums.Count > 0)
            {
                CreateButtons();
            }
        }

        /// <summary>
        /// 몬스터 정보 초기화.
        /// </summary>
        protected override void InitializationStat() 
        {
            base.InitializationStat();
            if (vnum <= 0) return;
            TableLoaderManager tableLoaderManager = TableLoaderManager.Instance;
            int mapVnum = MySceneGame.MyInstance.saveDataManager.CurrentChapter;
            var info = tableLoaderManager.TableNpc.GetNpcData(vnum);
            // FG_Logger.Log("InitializationStat vnum: "+vnum+" / info.vnum: "+info.vnum+" / StatMoveSpeed: "+info.statMoveSpeed);
            if (info.Vnum > 0)
            {
                StatAtk = info.StatAtk;
                CurrentAtk = (long)StatAtk;
                StatMoveSpeed = info.StatMoveSpeed;
                StatHp = info.StatHp;
                CurrentHp = (long)StatHp;
                CurrentMoveSpeed = StatMoveSpeed;
                float scale = info.Scale;
                transform.localScale = new Vector3(scale, scale, 0);
                OriginalScaleX = scale;
            }

            SetFlip(isFlip);
            
            // 맵 배치툴로 저장한 정보가 있을 경우 
            if (npcData != null)
            {
                // SetFlip(npcData.IsFlip);
                // List<int> questVnums = tableLoaderManager.TableQuest.GetQuestsByNpcVnum(npcData.MapVnum, npcData.Vnum);
                // transform.localScale = new Vector3(npcData.ScaleX, npcData.ScaleY, npcData.ScaleZ);
                // originalScaleX = npcData.ScaleX;
                // SetQuestVnums(questVnums);
            }
        }
        /// <summary>    
        /// 몬스터의 hit area 에 플레이어가 있을경우 공격하기 
        /// <para>
        /// monsterAi.cs 에서 충돌 검사중
        /// </para>
        /// </summary>
        protected override bool DownAttack()
        {
            if (!base.DownAttack()) return false;
        
            // PlayAnimationOnceAndThenLoop(attackAniNames[attackNameIndex]);
            // fG_Spine2DController.PlayAnimation(attackAniNames[attackNameIndex], true);
            return true;
        }
        /// <summary>
        /// 몬스터가 죽었을때 처리 
        /// </summary>
        private void OnDead()
        {
        }

        protected override void SetByDirection(bool set) 
        {
            base.SetByDirection(set);
            if (set == false)
            {
                Run();
            }
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(sceneGame.tagPlayer))
            {
                IsAttacking = true;
                SetStatusIdle();
                // StartCoroutine(AttackCoroutine(collision.gameObject.GetComponent<Player>()));
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(sceneGame.tagPlayer))
            {
                IsAttacking = false;
                SetStatusIdle();
                Invoke(nameof(Run), 0.3f);
            }
        }
        /// <summary>
        /// 보스전 시작할때 일반 몬스터 죽이기 
        /// </summary>
        public void DestroyByChallengeBoss()
        {
            if (sceneGame == null) return;
            SetStatusDead();
            Destroy(this.gameObject);
        }
        /// <summary>
        /// pc 가 근처에서 f 키를 눌렀을때 처리 
        /// </summary>
        public void OnTriggerTalk()
        {
            // kdh
            // 퀘스트가 하나라고 했을때 처리 
            if (questVnums.Count <= 0) return;
            int questVnum = questVnums[0];
            if (questVnum <= 0) return;
            // sceneGameCustom.questDialogueManager.StartDialogueQuest(questVnum);
        }
        
        protected override void Update()
        {
            // base.Update();
            if (interactionButtons.Count > 0)
            {
                UpdateButtonPositions();
            }
        }
        /// <summary>
        /// 맵 로드할때 npc를 생성하고 할당된 quest 가 있으면 넣어주기 
        /// </summary>
        /// <param name="mQuestVnums"></param>
        public void SetQuestVnums(List<int> mQuestVnums)
        {
            questVnums.Clear();
            RemoveButtons();
            if (mQuestVnums.Count <= 0) return;
            foreach(int questVnum in mQuestVnums)
            {
                questVnums.Add(questVnum);
            }
        }
        void CreateButtons()
        {
            // 버튼 생성 및 리스트에 추가
            foreach(int questVnum in questVnums)
            {
                GameObject newButton = Instantiate(buttonPrefab, canvasInteractionButtons.transform);
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{questVnum}";
                interactionButtons.Add(newButton);
            }
        }
        
        void RemoveButtons()
        {
            // 생성된 버튼들을 모두 삭제
            foreach (GameObject button in interactionButtons)
            {
                Destroy(button);
            }
            interactionButtons.Clear();
        }

        void UpdateButtonPositions()
        {
            // NPC의 스켈레톤 바운딩 박스에서 높이를 계산
            float npcHeight = GetNPCHeight();

            // NPC 머리 위 월드 좌표 설정
            Vector3 npcHeadWorldPosition = transform.position + new Vector3(0, npcHeight, 0);

            // 월드 좌표를 화면 좌표로 변환
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(npcHeadWorldPosition);

            // 화면 좌표를 Canvas의 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasInteractionButtons.transform as RectTransform, screenPosition, canvasInteractionButtons.worldCamera, out Vector2 localPosition);

            // 버튼을 수평 및 수직으로 정렬하여 배치
            float totalWidth = (buttonCount - 1) * buttonSpacing;
            Vector2 startOffset = new Vector2(-totalWidth / 2, 0); // 버튼의 시작 위치 오프셋

            for (int i = 0; i < interactionButtons.Count; i++)
            {
                Vector2 offset = startOffset + new Vector2(i * buttonSpacing, i * verticalSpacing);
                interactionButtons[i].GetComponent<RectTransform>().anchoredPosition = localPosition + offset;
            }
        }
        float GetNPCHeight()
        {
            // Skeleton에서 바운딩 박스 계산
            float[] vertexBuffer = new float[8];
            skeletonAnimation.Skeleton.GetBounds(out float x, out float y, out float width, out float height, ref vertexBuffer);
            return height; // NPC의 높이를 반환
        }

        private void OnDisable()
        {
            if (interactionButtons.Count <= 0) return;
            foreach (var interactionButton in interactionButtons)
            {
                if (interactionButton == null) continue;
                interactionButton.SetActive(false);
            }
        }
        private void OnEnable()
        {
            if (interactionButtons.Count <= 0) return;
            foreach (var interactionButton in interactionButtons)
            {
                if (interactionButton == null) continue;
                interactionButton.SetActive(true);
            }
        }
        public void StartFadeIn()
        {
            if (isStartFade) return;
            isStartFade = true;
            gameObject.SetActive(true);
            StartCoroutine(FadeIn(0.7f)); // 0.5초 동안 페이드 인
        }
        public void StartFadeOut()
        {
            if (isStartFade) return;
            isStartFade = true;
            StartCoroutine(FadeOut(0.7f)); // 0.5초 동안 페이드 아웃
        }
        // 페이드 인 효과
        private IEnumerator FadeIn(float duration)
        {
            float elapsedTime = 0.0f;
            Color color = skeletonAnimation.Skeleton.GetColor();
            color.a = 0; // 초기 알파 값을 0으로 설정 (투명)

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                color.a = Easing.EaseOutQuintic(t);
                skeletonAnimation.Skeleton.SetColor(color);
                yield return null;
            }
            color.a = 1; // 최종 알파 값은 1로 설정 (불투명)
            skeletonAnimation.Skeleton.SetColor(color);
            isStartFade = false;
        }

        // 페이드 아웃 효과
        private IEnumerator FadeOut(float duration)
        {
            float elapsedTime = 0.0f;
            Color color = skeletonAnimation.Skeleton.GetColor();
            color.a = 1; // 초기 알파 값을 0으로 설정 (투명)
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                color.a = Easing.EaseInQuintic(1.0f - t);
                skeletonAnimation.Skeleton.SetColor(color);
                yield return null;
            }
            color.a = 0; // 최종 알파 값은 0으로 설정 (투명)
            skeletonAnimation.Skeleton.SetColor(color);
            // 페이드 아웃 완료 후 비활성화
            gameObject.SetActive(false);
            isStartFade = false;
        }
    }
}
