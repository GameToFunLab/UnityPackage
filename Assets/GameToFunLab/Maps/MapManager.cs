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
    public class MapManager : MonoBehaviour
    {
        public enum Type
        {
            None,
            Common,
        }
        public enum SubType
        {
            None,
        }

        private enum State
        {
            None,
            FadeIn,               // 1. 검정색 스프라이트 페이드 인
            UnloadPreviousStage,  // 1. 이전 스테이지의 몬스터 인스턴스를 제거하고 메모리 정리를 진행
            LoadTilemapPrefab,    // 2. 맵에 필요한 타일맵 프리팹 불러오기
            LoadMonsterPrefabs,   // 3. 맵에 배치되는 몬스터 스파인 프리팹 불러오기
            FadeOut,              // 6. 완료
            Complete              // 6. 완료
        }

        private State currentState = State.None;

        public GameObject bgBlackForMapLoading;  // 페이드 인에 사용할 검정색 스프라이트 오브젝트
        public float fadeDuration = 2.0f;  // 페이드 인 지속 시간

        public List<GameObject> monsterPrefabs;
        public List<int> monsterVnums;

        public int loadMapVnum; // 현재 맵 vnum
        private DefaultMap currentDefaultMap; // 현재 맵 defaultMap 
        private int questVnum; // 맵에 연결된 퀘스트 vnum

        private SceneGame sceneGame;
        private SaveDataManager saveDataManager;
        
        private void Awake()
        {
            bgBlackForMapLoading.SetActive(false);
        }

        private void Start()
        {
            sceneGame = SceneGame.Instance;
            saveDataManager = sceneGame.saveDataManager;
        }

        private void Reset()
        {
            questVnum = 0;
        }
        public void LoadMap(int mapVnum = 0)
        {
            if (IsPossibleLoad() != true)
            {
                // FgLogger.LogError($"map state: {currentState}");
                return;
            }
            // FgLogger.Log("LoadMap start");
            Reset();
            currentState = State.FadeIn;
            loadMapVnum = mapVnum;
            StartCoroutine(UpdateState());
        }

        IEnumerator UpdateState()
        {
            while (currentState != State.Complete)
            {
                switch (currentState)
                {
                    case State.FadeIn:
                        yield return StartCoroutine(FadeIn());
                        currentState = State.UnloadPreviousStage;
                        break;

                    case State.UnloadPreviousStage:
                        yield return StartCoroutine(UnloadPreviousStage());
                        currentState = State.LoadTilemapPrefab;
                        break;
                    case State.LoadTilemapPrefab:
                        LoadTilemap();
                        currentState = State.LoadMonsterPrefabs;
                        break;

                    case State.LoadMonsterPrefabs:
                        LoadMonsters();
                        currentState = State.FadeOut;
                        break;
                    case State.FadeOut:
                        yield return StartCoroutine(FadeOut());
                        currentState = State.Complete;
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

            bgBlackForMapLoading.SetActive(true);
            SpriteRenderer spriteRenderer = bgBlackForMapLoading.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(0, 0, 0, 0);
            float elapsedTime = 0.0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeDuration);
                float alpha = Mathf.Lerp(0, 1, Easing.EaseOutQuintic(t));
                spriteRenderer.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            // Fade in이 완료된 후에 완전히 불투명하게 설정
            spriteRenderer.color = new Color(0, 0, 0, 1);

            // Logger.Log("Fade In 완료");
        }
        
        IEnumerator UnloadPreviousStage()
        {
            // 현재 씬에 있는 모든 몬스터 오브젝트를 삭제
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(sceneGame.tagMonster);
            foreach (var monster in enemies)
            {
                Destroy(monster);
            }
            monsterVnums.Clear();
            monsterPrefabs.Clear();

            // 잠시 대기하여 오브젝트가 완전히 삭제되도록 보장
            yield return null;

            // 사용되지 않는 메모리 해제
            yield return Resources.UnloadUnusedAssets();

            // 필요시 가비지 컬렉션을 강제로 실행
            System.GC.Collect();

            // FgLogger.Log("이전 스테이지의 몬스터 프리팹이 메모리에서 해제되었습니다.");
        }

        void LoadTilemap()
        {
            sceneGame.player?.GetComponent<Player>().Stop();

            (int vnum, string name, string tileMapPrefabName, Type type, SubType typeSub) resultChapterData;
            if (loadMapVnum == 0)
            {
                loadMapVnum = saveDataManager.CurrentChapter;
            }
            
            if (currentDefaultMap != null)
            {
                Destroy(currentDefaultMap.gameObject);
            }
            
            var result = currentDefaultMap.GetMapSize();

            // 로드된 맵에 맞게 맵 영역 사이즈 갱신하기 
            sceneGame.cameraManager.ChangeMapSize(result.width / 2, result.height / 2);
            
            // 플레이어 위치 0, 0 으로
            sceneGame.player?.GetComponent<Player>().MoveForce(0, 0);
            
            // Logger.Log("타일맵 프리팹 로드 완료");
        }

        protected virtual void LoadMonsters()
        {
            // string chapterMonsterVnums = tableLoaderManager.TableMap.GetMonsterVnum(loadMapVnum);
            // if (chapterMonsterVnums == "")
            // {
            //     // FgLogger.LogError("monster vnum is blink. map vnum: "+loadMapVnum);
            //     return;
            // }
            //
            // string[] vnums = chapterMonsterVnums.Split(",");
            // foreach(string vnum in vnums)
            // {
            //     string shapePath = tableLoaderManager.TableMonster.GetShapePath(int.Parse(vnum));
            //     if (shapePath == "") continue;
            //     GameObject monster = Resources.Load<GameObject>(shapePath);
            //     monsterPrefabs.Add(monster);
            //     monsterVnums.Add(int.Parse(vnum));
            // }
            // Logger.Log("몬스터 프리팹 로드 완료");
        }
        IEnumerator FadeOut()
        {
            if (bgBlackForMapLoading == null)
            {
                FgLogger.LogError("Fade Sprite가 설정되지 않았습니다.");
                yield break;
            }

            SpriteRenderer spriteRenderer = bgBlackForMapLoading.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(0, 0, 0, 1);
            float elapsedTime = 0.0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeDuration);
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
            
            // Logger.Log("맵 로드 완료");
        }

        public (GameObject prefab, int vnum) GetMonsterPrefabByRandom()
        {
            if (monsterPrefabs.Count <= 0) return (null, 0);
            int rand = Random.Range(0, monsterPrefabs.Count);
            return (monsterPrefabs[rand], monsterVnums[rand]);
        }
        private bool IsPossibleLoad()
        {
            return (currentState == State.Complete || currentState == State.None);
        }
    }
}
