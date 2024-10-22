using System.Collections;
using System.Collections.Generic;
using GameToFunLab.Characters;
using GameToFunLab.Core;
using GameToFunLab.Scenes;
using GameToFunLab.Utils;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameToFunLab.Maps
{
    /// <summary>
    /// 맵 매니저
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        public MapConstants.State CurrentState { get; set; }
        public bool IsLoadComplete { get; set; }
        public int CurrentMapUnum { get; set; }
        public float FadeDuration { get; set; }
        public Vector3 PlaySpawnPosition { get; set; }

        public GameObject bgBlackForMapLoading;  // 페이드 인에 사용할 검정색 스프라이트 오브젝트

        public List<GameObject> monsterPrefabs;
        public List<int> monsterUnums;

        protected DefaultMap DefaultMap; // 현재 맵 defaultMap 

        protected SceneGame SceneGame;
        protected SaveDataManager SaveDataManager;
        
        private void Awake()
        {
            bgBlackForMapLoading.SetActive(true);
            Image spriteRenderer = bgBlackForMapLoading.GetComponent<Image>();
            spriteRenderer.color = new Color(0, 0, 0, 1);
            IsLoadComplete = false;
        }

        public virtual void Initialize()
        {
            SceneGame = SceneGame.Instance;
            SaveDataManager = SceneGame.saveDataManager;
        }

        protected virtual void Reset()
        {
            IsLoadComplete = false;
        }
        public void LoadMap(int mapUnum = 0)
        {
            if (IsPossibleLoad() != true)
            {
                // FgLogger.LogError($"map state: {CurrentState}");
                return;
            }
            // FgLogger.Log("LoadMap start");
            Reset();
            CurrentState = MapConstants.State.FadeIn;
            CurrentMapUnum = mapUnum;
            StartCoroutine(UpdateState());
        }

        IEnumerator UpdateState()
        {
            while (CurrentState != MapConstants.State.Complete)
            {
                switch (CurrentState)
                {
                    case MapConstants.State.FadeIn:
                        yield return StartCoroutine(FadeIn());
                        CurrentState = MapConstants.State.UnloadPreviousStage;
                        break;

                    case MapConstants.State.UnloadPreviousStage:
                        yield return StartCoroutine(UnloadPreviousStage());
                        CurrentState = MapConstants.State.LoadTilemapPrefab;
                        break;
                    case MapConstants.State.LoadTilemapPrefab:
                        yield return StartCoroutine(CreateMap());
                        CurrentState = MapConstants.State.LoadMonsterPrefabs;
                        break;

                    case MapConstants.State.LoadMonsterPrefabs:
                        yield return StartCoroutine(LoadMonsters());
                        CurrentState = MapConstants.State.LoadNpcPrefabs;
                        break;
                    case MapConstants.State.LoadNpcPrefabs:
                        yield return StartCoroutine(LoadNpcs());
                        CurrentState = MapConstants.State.LoadWarpPrefabs;
                        break;
                    case MapConstants.State.LoadWarpPrefabs:
                        yield return StartCoroutine(LoadWarps());
                        CurrentState = MapConstants.State.FadeOut;
                        break;
                    case MapConstants.State.FadeOut:
                        yield return StartCoroutine(FadeOut());
                        CurrentState = MapConstants.State.Complete;
                        break;
                }

                yield return null;
            }

            OnMapLoadComplete();
        }

        IEnumerator FadeIn()
        {
            if (bgBlackForMapLoading == null)
            {
                FgLogger.LogError("Fade Sprite가 설정되지 않았습니다.");
                yield break;
            }
            // 이미 활성화 되어있으면 (인게임 처음 시작했을때) 건너뛰기.
            if (bgBlackForMapLoading.activeSelf)
            {
                yield break;
            }

            bgBlackForMapLoading.SetActive(true);
            Image spriteRenderer = bgBlackForMapLoading.GetComponent<Image>();
            spriteRenderer.color = new Color(0, 0, 0, 0);
            float elapsedTime = 0.0f;

            while (elapsedTime < FadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / FadeDuration);
                float alpha = Mathf.Lerp(0, 1, Easing.EaseOutQuintic(t));
                spriteRenderer.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            // Fade in이 완료된 후에 완전히 불투명하게 설정
            spriteRenderer.color = new Color(0, 0, 0, 1);

            // Logger.Log("Fade In 완료");
        }
        protected void DestroyByTag(string tag)
        {
            GameObject[] mapObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject mapObject in mapObjects)
            {
                if (mapObject == null) continue;
                Destroy(mapObject);
            }
        }
        IEnumerator UnloadPreviousStage()
        {
            // 현재 씬에 있는 모든 몬스터 오브젝트를 삭제
            DestroyByTag(SceneGame.tagMonster);
            DestroyByTag(SceneGame.tagNpc);
            monsterUnums.Clear();
            monsterPrefabs.Clear();

            DestroyOthers();
            // 잠시 대기하여 오브젝트가 완전히 삭제되도록 보장
            yield return null;

            // 사용되지 않는 메모리 해제
            yield return Resources.UnloadUnusedAssets();

            // 필요시 가비지 컬렉션을 강제로 실행
            System.GC.Collect();

            // FgLogger.Log("이전 스테이지의 몬스터 프리팹이 메모리에서 해제되었습니다.");
        }

        protected virtual void DestroyOthers()
        {
        }

        protected virtual IEnumerator CreateMap()
        {
            SceneGame.player?.GetComponent<Player>().Stop();

            if (CurrentMapUnum == 0)
            {
                CurrentMapUnum = SaveDataManager.CurrentChapter;
            }
            
            if (DefaultMap != null)
            {
                Destroy(DefaultMap.gameObject);
            }
            // 맵 생성 코드 추가 해야 함
            
            // 플레이어 위치 0, 0 으로
            SceneGame.player?.GetComponent<Player>().MoveForce(0, 0);
            
            // Logger.Log("타일맵 프리팹 로드 완료");
            yield return null;
        }

        protected virtual IEnumerator LoadMonsters()
        {
            yield return null;
        }
        protected virtual IEnumerator LoadWarps()
        {
            yield return null;
        }
        protected virtual IEnumerator LoadNpcs()
        {
            // string chapterMonsterUnums = tableLoaderManager.TableMap.GetMonsterUnum(loadMapUnum);
            // if (chapterMonsterUnums == "")
            // {
            //     // FgLogger.LogError("monster unum is blink. map unum: "+loadMapUnum);
            //     return;
            // }
            //
            // string[] unums = chapterMonsterUnums.Split(",");
            // foreach(string unum in unums)
            // {
            //     string shapePath = tableLoaderManager.TableMonster.GetShapePath(int.Parse(unum));
            //     if (shapePath == "") continue;
            //     GameObject monster = Resources.Load<GameObject>(shapePath);
            //     monsterPrefabs.Add(monster);
            //     monsterUnums.Add(int.Parse(unum));
            // }
            // Logger.Log("몬스터 프리팹 로드 완료");
            yield return null;
        }
        IEnumerator FadeOut()
        {
            if (bgBlackForMapLoading == null)
            {
                FgLogger.LogError("Fade Sprite가 설정되지 않았습니다.");
                yield break;
            }

            Image spriteRenderer = bgBlackForMapLoading.GetComponent<Image>();
            spriteRenderer.color = new Color(0, 0, 0, 1);
            float elapsedTime = 0.0f;

            while (elapsedTime < FadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / FadeDuration);
                float alpha = Mathf.Lerp(1, 0, Easing.EaseInQuintic(t));
                spriteRenderer.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            // Fade in이 완료된 후에 완전히 불투명하게 설정
            spriteRenderer.color = new Color(0, 0, 0, 0);
            bgBlackForMapLoading.SetActive(false);

            // Logger.Log("Fade Out 완료");
        }

        void OnMapLoadComplete()
        {
            StopCoroutine(UpdateState());
            StopCoroutine(FadeOut());
            StopCoroutine(UnloadPreviousStage());
            StopCoroutine(FadeIn());

            SceneGame.saveDataManager.SetChapter(CurrentMapUnum);
            IsLoadComplete = true;
            PlaySpawnPosition = Vector3.zero;
            // Logger.Log("맵 로드 완료");
        }

        public (GameObject prefab, int unum) GetMonsterPrefabByRandom()
        {
            if (monsterPrefabs.Count <= 0) return (null, 0);
            int rand = Random.Range(0, monsterPrefabs.Count);
            return (monsterPrefabs[rand], monsterUnums[rand]);
        }
        private bool IsPossibleLoad()
        {
            return (CurrentState == MapConstants.State.Complete || CurrentState == MapConstants.State.None);
        }
        protected string GetPathTilemap(string folderName)
        {
            return MapConstants.ResourceMapPath + folderName + "/" + MapConstants.FileNameTilemap;
        }
        protected string GetPathRegen(string folderName)
        {
            return MapConstants.ResourceMapPath + folderName + "/" + MapConstants.FileNameRegenNpc;
        }
        protected string GetPathWarp(string folderName)
        {
            return MapConstants.ResourceMapPath + folderName + "/" + MapConstants.FileNameWarp;
        }

        public void SetPlaySpawnPosition(Vector3 position)
        {
            PlaySpawnPosition = position;
        }
        public Vector3 GetPlaySpawnPosition()
        {
            return PlaySpawnPosition;
        }
    }
}
