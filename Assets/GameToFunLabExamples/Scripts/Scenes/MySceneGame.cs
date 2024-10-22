using System.Collections;
using GameToFunLab.Characters;
using GameToFunLab.Core;
using GameToFunLab.Scenes;
using GameToFunLab.Utils;
using Scripts.TableLoader;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Scenes
{
    public class MySceneGame : SceneGame
    {
        public static MySceneGame MyInstance { get; private set; }
        
        [HideInInspector] public TableLoaderManager tableLoaderManager;
        public CharacterManager characterManager;
        public Canvas canvasInteractionButtons;

        private enum LoadState
        {
            None,
            UnloadPreviousStage,  // 
            LoadPlayer,    // 플레이어 생성
            LoadMap,       // 맵 생성
            LoadCamera,    // 카메라 생성
            FadeOut,
            Complete              // 6. 완료
        }
        private LoadState currentLoadState = LoadState.None;
        
        protected override void Awake()
        {
            if (TableLoaderManager.Instance == null)
            {
                SceneManager.LoadSceneByName("Intro");
                return;
            }
            base.Awake();
            if (MyInstance == null)
            {
                MyInstance = this;
                DontDestroyOnLoad(gameObject);
            }
            tableLoaderManager = TableLoaderManager.Instance;
            characterManager = new CharacterManager();
        }

        protected override void Start()
        {
            base.Start();
            mapManager.bgBlackForMapLoading.SetActive(true);
            currentLoadState = LoadState.UnloadPreviousStage;
            StartCoroutine(UpdateLoadState());
        }
        
        IEnumerator UpdateLoadState()
        {
            while (currentLoadState != LoadState.Complete)
            {
                switch (currentLoadState)
                {
                    case LoadState.UnloadPreviousStage:
                        yield return StartCoroutine(UnloadPreviousStage());
                        currentLoadState = LoadState.LoadPlayer;
                        break;
                    case LoadState.LoadPlayer:
                        yield return StartCoroutine(CreatePlayerCharacter());
                        currentLoadState = LoadState.LoadMap;
                        break;

                    case LoadState.LoadMap:
                        yield return StartCoroutine(LoadMap());
                        currentLoadState = LoadState.LoadCamera;
                        break;
                    case LoadState.LoadCamera:
                        yield return StartCoroutine(LoadCamera());
                        currentLoadState = LoadState.FadeOut;
                        break;
                    case LoadState.FadeOut:
                        yield return StartCoroutine(FadeOut());
                        currentLoadState = LoadState.Complete;
                        break;
                }

                yield return null;
            }

            OnLoadComplete();
        }

        IEnumerator FadeOut()
        {
            if (mapManager.bgBlackForMapLoading == null)
            {
                FgLogger.LogError("Fade Sprite가 설정되지 않았습니다.");
                yield break;
            }

            Image spriteRenderer = mapManager.bgBlackForMapLoading.GetComponent<Image>();
            spriteRenderer.color = new Color(0, 0, 0, 1);
            float elapsedTime = 0.0f;

            while (elapsedTime < mapManager.FadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / mapManager.FadeDuration);
                float alpha = Mathf.Lerp(1, 0, Easing.EaseInQuintic(t));
                spriteRenderer.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            // Fade in이 완료된 후에 완전히 불투명하게 설정
            spriteRenderer.color = new Color(0, 0, 0, 0);
            mapManager.bgBlackForMapLoading.SetActive(false);

            // Logger.Log("Fade Out 완료");
        }
        IEnumerator LoadCamera()
        {
            cameraManager.SetPlayer(player.transform);
            yield return null;
        }
        IEnumerator LoadMap()
        {
            mapManager.LoadMap(1);
            yield return null;
        }

        IEnumerator CreatePlayerCharacter()
        {
            CharacterData data = new CharacterData();
            data.Unum = 1;
            DefaultCharacter defaultCharacter = characterManager.CreateCharacter(CharacterManager.CharacterType.Player, data);
            player = defaultCharacter.gameObject;
            yield return null;
        }

        IEnumerator UnloadPreviousStage()
        {
            // 잠시 대기하여 오브젝트가 완전히 삭제되도록 보장
            yield return null;

            // 사용되지 않는 메모리 해제
            yield return Resources.UnloadUnusedAssets();

            // 필요시 가비지 컬렉션을 강제로 실행
            System.GC.Collect();
        }

        private void OnLoadComplete()
        {
            FgLogger.Log("게임 씬 로드 완료");
        }
        public override long GetMaxEnemyValue()
        {
            return 10;
        }
    }
}