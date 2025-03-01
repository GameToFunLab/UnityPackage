using System;
using System.Collections.Generic;
using System.IO;
using GameToFunLab.Characters;
using GameToFunLab.Configs;
using GameToFunLab.Core;
using GameToFunLab.Maps;
using Newtonsoft.Json;
using Scripts.TableLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameToFunLab.Editor.MapEditor
{
    public class NpcExporter
    {
        private List<NpcData> npcList;
        private TableNpc tableNpc;
        private TableSpine tableSpine;
        private DefaultMap defaultMap;
        private GameObject player;
        
        public void Initialize(TableNpc tableNpc, TableSpine tableSpine, DefaultMap defaultMap, GameObject player)
        {
            this.tableNpc = tableNpc;
            this.tableSpine = tableSpine;
            this.defaultMap = defaultMap;
            this.player = player;
        }
        public void SetDefaultMap(DefaultMap defaultMap)
        {
            this.defaultMap = defaultMap;
        }

        public void AddNpcToMap(int selectedNpcIndex)
        {
            if (defaultMap == null)
            {
                Debug.LogError("_defaultMap 이 없습니다.");
                return;
            }

            var npcDictionary = tableNpc.GetDatas();
            int index = 0;
            StruckTableNpc npcData = new StruckTableNpc();

            foreach (var outerPair in npcDictionary)
            {
                if (index == selectedNpcIndex)
                {
                    npcData = tableNpc.GetNpcData(outerPair.Key);
                    break;
                }
                index++;
            }

            if (npcData.Unum <= 0)
            {
                Debug.LogError("NPC 데이터가 없습니다.");
                return;
            }

            GameObject npcPrefab = tableSpine.GetPrefab(npcData.SpineUnum);
            if (npcPrefab == null)
            {
                Debug.LogError("NPC 프리팹을 찾을 수 없습니다.");
                return;
            }

            GameObject npc = Object.Instantiate(npcPrefab, Vector3.zero, Quaternion.identity, defaultMap.transform);
            if (player != null)
            {
                npc.transform.position = player.transform.position + new Vector3(1, 0, 0);
            }

            var npcScript = npc.GetComponent<Npc>();
            if (npcScript != null)
            {
                npcScript.unum = npcData.Unum;
            }

            Debug.Log($"{npcData.Name} NPC가 맵에 추가되었습니다.");
        }

        public void ExportNpcDataToJson(string filePath, string fileName, int mapUnum)
        {
            GameObject mapObject = GameObject.FindGameObjectWithTag(ConfigTags.Map);
            NpcDataList saveNpcList = new NpcDataList();

            foreach (Transform child in mapObject.transform)
            {
                if (child.CompareTag(ConfigTags.Npc))
                {
                    var npc = child.gameObject.GetComponent<Npc>();
                    if (npc == null) continue;
                    saveNpcList.DataList.Add(new NpcData(npc.unum, child.position, npc.IsFlip(), child.localScale, mapUnum));
                }
            }

            string json = JsonConvert.SerializeObject(saveNpcList);
            string path = Path.Combine(filePath, fileName);
            File.WriteAllText(path, json);
            Debug.Log("NPC data exported to " + path);
        }
        
        public void LoadNpcData(string regenFileName)
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
                        NpcDataList npcDataList = JsonConvert.DeserializeObject<NpcDataList>(content);
                        npcList = npcDataList.DataList;
                        SpawnNpc();
                    }
                }
            }
            catch (Exception ex)
            {
                FgLogger.LogError($"Error reading file {regenFileName}: {ex.Message}");
            }
        }

        private void SpawnNpc()
        {
            if (defaultMap == null)
            {
                Debug.LogError("_defaultMap 이 없습니다.");
                return;
            }

            foreach (NpcData npcData in npcList)
            {
                int unum = npcData.Unum;
                if (unum <= 0) continue;
                var info = tableNpc.GetNpcData(unum);
                if (info.Unum <= 0 || info.SpineUnum <= 0) continue;
                GameObject npcPrefab = tableSpine.GetPrefab(info.SpineUnum);
                if (npcPrefab == null)
                {
                    FgLogger.LogError("npc 프리팹이 없습니다. spine unum: " + info.SpineUnum);
                    continue;
                }
                GameObject npc = Object.Instantiate(npcPrefab, new Vector3(npcData.x, npcData.y, npcData.z), Quaternion.identity, defaultMap.gameObject.transform);
                
                // NPC의 속성을 설정하는 스크립트가 있을 경우 적용
                Npc npcScript = npc.GetComponent<Npc>();
                if (npcScript != null)
                {
                    // MapManager.cs:138 도 수정
                    npcScript.unum = npcData.Unum;
                    npcScript.NpcData = npcData;
                    npcScript.InitializeByEditor();
                    // 추가적인 속성 설정을 여기에서 수행할 수 있습니다.
                }
            }

            Debug.Log("NPCs spawned successfully.");
        }
    }
}
