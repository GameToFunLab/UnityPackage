using System;
using System.Collections;
using System.Collections.Generic;
using GameToFunLab.Characters;
using GameToFunLab.Configs;
using GameToFunLab.Core;
using GameToFunLab.Maps;
using GameToFunLab.Maps.Objects;
using GameToFunLab.Runtime.Scripts.Characters;
using GameToFunLab.Runtime.Scripts.Scenes;
using GameToFunLab.Runtime.Scripts.TableLoader;
using Newtonsoft.Json;
using UnityEngine;

namespace GameToFunLab.Runtime.Scripts.Maps
{
    public class MyMapManager : MapManager
    {
        private List<NpcData> npcList;
        private List<WarpData> warpDatas;
        private StruckTableMap resultChapterData;
        public GameObject gridTileMap;
        private MapTiled mapTiled;
        private MySceneGame mySceneGame;

        private void Awake()
        {
            FadeDuration = MapConstants.FadeDuration;
        }
        private void Start()
        {
            mySceneGame = MySceneGame.MyInstance;
        }
        protected override void DestroyOthers()
        {
            DestroyByTag(ConfigTags.Map);
            DestroyByTag(ConfigTags.ButtonNpcQuest);
        }
        /// <summary>
        /// MapEditor.cs:152
        /// </summary>
        // protected override IEnumerator CreateMap()
        // {
        //     mySceneGame.player?.GetComponent<MyPlayer>().Stop();
        //
        //     if (CurrentMapUnum == 0)
        //     {
        //         CurrentMapUnum = SaveDataManager.CurrentChapter;
        //     }
        //     resultChapterData = TableLoaderManager.Instance.TableMap.GetMapData(CurrentMapUnum);
        //     string path = GetPathTilemap(resultChapterData.FolderName);
        //     GameObject prefab = Resources.Load<GameObject>(path);
        //     if (prefab == null)
        //     {
        //         FgLogger.Log($"dont exist prefab path. path: {path} / chapterUnum: {CurrentMapUnum}");
        //         yield break;
        //     }
        //     if (mapTiled != null)
        //     {
        //         Destroy(mapTiled.gameObject);
        //     }
        //     GameObject currentMap = Instantiate(prefab, gridTileMap.transform);
        //     mapTiled = currentMap.GetComponent<MapTiled>();
        //     mapTiled.Unum = CurrentMapUnum;
        //     
        //     // 플레이어 위치
        //     Vector3 spawnPosition = resultChapterData.PlayerSpawnPosition;
        //     if (PlaySpawnPosition != Vector3.zero)
        //     {
        //         spawnPosition = PlaySpawnPosition;
        //     }
        //     mySceneGame.player?.GetComponent<MyPlayer>().MoveForce(spawnPosition.x, spawnPosition.y);
        //     
        //     // Logger.Log("타일맵 프리팹 로드 완료");
        //     yield return null;
        // }
        protected override IEnumerator LoadNpcs()
        {
            string regenFileName = GetPathRegen(resultChapterData.FolderName);

            try
            {
                TextAsset textFile = Resources.Load<TextAsset>($"{regenFileName}");
                if (textFile != null)
                {
                    string content = textFile.text;
                    if (!string.IsNullOrEmpty(content))
                    {
                        NpcDataList npcDataList = JsonConvert.DeserializeObject<NpcDataList>(content);
                        npcList = npcDataList.DataList;
                        SpawnNpc();
                    }
                }
            }
            catch (Exception ex)
            {
                FgLogger.LogError($"Error reading file {regenFileName}: {ex.Message}");
                yield break;
            }

            yield return null;
        }
        protected override IEnumerator LoadWarps()
        {
            string rPathWarp = GetPathWarp(resultChapterData.FolderName);

            try
            {
                TextAsset textFile = Resources.Load<TextAsset>($"{rPathWarp}");
                if (textFile != null)
                {
                    string content = textFile.text;
                    if (!string.IsNullOrEmpty(content))
                    {
                        WarpDataList warpDataList = JsonConvert.DeserializeObject<WarpDataList>(content);
                        warpDatas = warpDataList.DataList;
                        SpawnWarps();
                    }
                }
            }
            catch (Exception ex)
            {
                FgLogger.LogError($"Error reading file {rPathWarp}: {ex.Message}");
                yield break;
            }
            yield return null;
        }
        
        private void SpawnNpc()
        {
            TableNpc tableNpc = TableLoaderManager.Instance.TableNpc;
            TableSpine tableSpine = TableLoaderManager.Instance.TableSpine;
            
            GameObject npcPrefab = null;
            foreach (NpcData npcData in npcList)
            {
                int unum = npcData.Unum;
                if (unum <= 0) continue;
                var info = tableNpc.GetNpcData(unum);
                if (info.Unum <= 0 || info.SpineUnum <= 0) continue;
                npcPrefab = tableSpine.GetPrefab(info.SpineUnum);
                if (npcPrefab == null)
                {
                    FgLogger.LogError("프리팹이 없습니다. spine unum: " + info.SpineUnum);
                    continue;
                }
                GameObject npc = Instantiate(npcPrefab, new Vector3(npcData.x, npcData.y, npcData.z), Quaternion.identity, mapTiled.gameObject.transform);
            
                // NPC의 이름과 기타 속성 설정
                MyNpc myNpcScript = npc.GetComponent<MyNpc>();
                if (myNpcScript != null)
                {
                    // npcExporter.cs:158 도 수정
                    myNpcScript.unum = npcData.Unum;
                    myNpcScript.NpcData = npcData;
                    
                    mapTiled.AddNpc(npc);
                }
            }
        }
        private void SpawnWarps()
        {
            GameObject warpPrefab = Resources.Load<GameObject>(MapConstants.PathPrefabWarp);
            if (warpPrefab == null)
            {
                FgLogger.LogError("워프 프리팹이 없습니다. path:"+MapConstants.PathPrefabWarp);
                return;
            }
            foreach (WarpData warpData in warpDatas)
            {
                int ToMapUnum = warpData.ToMapUnum;
                if (ToMapUnum <= 0) continue;
                GameObject warp = Instantiate(warpPrefab, new Vector3(warpData.x, warpData.y, warpData.z), Quaternion.identity, mapTiled.gameObject.transform);
            
                // NPC의 이름과 기타 속성 설정
                ObjectWarp objectWarp = warp.GetComponent<ObjectWarp>();
                if (objectWarp != null)
                {
                    // warpExporter.cs:128 도 수정
                    objectWarp.warpData = warpData;
                }
            }
        }
    }
}
