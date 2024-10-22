using System;
using System.Collections.Generic;
using System.IO;
using GameToFunLab.Core;
using GameToFunLab.Maps;
using GameToFunLab.Maps.Objects;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameToFunLab.Editor.MapEditor
{
    public class WarpExporter
    {
        private List<WarpData> warpDatas;
        private DefaultMap _defaultMap;
        private GameObject _player;

        public void Initialize(DefaultMap defaultMap, GameObject player)
        {
            _defaultMap = defaultMap;
            _player = player;
        }

        public void SetDefaultMap(DefaultMap defaultMap)
        {
            _defaultMap = defaultMap;
        }
        public void AddWarpToMap()
        {
            if (_defaultMap == null)
            {
                Debug.LogError("_defaultMap 이 없습니다.");
                return;
            }

            GameObject warpPrefab = Resources.Load<GameObject>(MapConstants.PathPrefabWarp);
            if (warpPrefab == null)
            {
                Debug.LogError("Warp prefab is null.");
                return;
            }

            GameObject warp = Object.Instantiate(warpPrefab, Vector3.zero, Quaternion.identity, _defaultMap.transform);
            if (_player != null)
            {
                warp.transform.position = _player.transform.position + new Vector3(1, 0, 0);
            }

            var objectWarp = warp.GetComponent<ObjectWarp>();
            if (objectWarp == null)
            {
                Debug.LogError("ObjectWarp script missing.");
                return;
            }

            Debug.Log("Warp added to the map.");
        }

        public void ExportWarpDataToJson(string filePath, string fileName, int mapUnum)
        {
            GameObject mapObject = GameObject.FindGameObjectWithTag("Map");
            WarpDataList warpDataList = new WarpDataList();

            foreach (Transform child in mapObject.transform)
            {
                if (child.CompareTag("MapObjectWarp"))
                {
                    var objectWarp = child.gameObject.GetComponent<ObjectWarp>();
                    if (objectWarp == null) continue;
                    warpDataList.DataList.Add(new WarpData(mapUnum,child.position,objectWarp.toMapUnum,objectWarp.toMapPlayerSpawnPosition,child.transform.eulerAngles,child.GetComponent<BoxCollider2D>().size));
                }
            }

            string json = JsonConvert.SerializeObject(warpDataList);
            string path = Path.Combine(filePath, fileName);
            File.WriteAllText(path, json);
            Debug.Log("Warp data exported to " + path);
        }
        
        public void LoadWarpData(string regenFileName)
        {
            // JSON 파일을 읽기
            try
            {
                TextAsset textFile = Resources.Load<TextAsset>($"{regenFileName}");
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
                FgLogger.LogError($"Error reading file {regenFileName}: {ex.Message}");
            }
        }
        private void SpawnWarps()
        {
            if (_defaultMap == null)
            {
                Debug.LogError("_defaultMap 이 없습니다.");
                return;
            }

            GameObject warpPrefab = Resources.Load<GameObject>(MapConstants.PathPrefabWarp);
            if (warpPrefab == null)
            {
                FgLogger.LogError("워프 프리팹이 없습니다. ");
                return;
            }
            foreach (WarpData warpData in warpDatas)
            {
                int toMapUnum = warpData.ToMapUnum;
                if (toMapUnum <= 0)
                {
                    FgLogger.LogError("이동할 맵이 셋팅 되지 않았습니다.");
                }
                GameObject warp = Object.Instantiate(warpPrefab, _defaultMap.gameObject.transform);
                
                // NPC의 속성을 설정하는 스크립트가 있을 경우 적용
                ObjectWarp npcScript = warp.GetComponent<ObjectWarp>();
                if (npcScript != null)
                {
                    // MapManager.cs:164 도 수정
                    npcScript.warpData = warpData;
                    npcScript.Initialize();
                }
            }

            Debug.Log("워프 spawned successfully.");
        }
    }
}
