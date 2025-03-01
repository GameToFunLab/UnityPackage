using System.Collections.Generic;
using GameToFunLab.Characters;
using GameToFunLab.Configs;
using GameToFunLab.Core;
using GameToFunLab.Maps;
using GameToFunLab.Maps.Objects;
using GameToFunLab.Runtime.Scripts.Maps;
using GameToFunLab.Runtime.Scripts.TableLoader;
using UnityEditor;
using UnityEngine;

namespace GameToFunLab.Editor.MapEditor
{
    public class MapExporter : EditorWindow
    {
        private List<NpcData> npcList;
        private List<WarpData> warpDatas;
        private static MapTiled _defaultMap;
        private static GameObject _gridTileMap;
        private static GameObject _player;
        
        private TableLoader tableLoader;
        private static TableMap _tableMap;
        private static TableNpc _tableNpc;
        private static TableSpine _tableSpine;

        private readonly string jsonFolderPath = Application.dataPath+"/Resources/"+MapConstants.ResourceMapPath;
        private string currentJsonFolderPath = "";

        private readonly string resourcesFolderPath = MapConstants.ResourceMapPath;
        // private string fileName = "regen_npc.json";
        // private string fileNameWarp = "warp.json";
        private string loadMapUnum = "1";
        private const string NameTempTableLoaderManager = "TempTableLoaderManager";

        private static readonly string FileNameTilemap = MapConstants.FileNameTilemap;
        private static readonly string FileNameRegen = MapConstants.FileNameRegenNpc;
        private static readonly string FileNameWarp = MapConstants.FileNameWarp;
        private static readonly string FileExt = MapConstants.FileExt;
        private readonly string jsonFileNameRegen = FileNameRegen+FileExt;
        private readonly string jsonFileNameWarp = FileNameWarp+FileExt;
        
        private NpcExporter npcExporter;
        private WarpExporter warpExporter;
        
        private static List<string> _npcNames; // NPC 이름 목록
        private int selectedNpcIndex;

        [MenuItem("GameToFunLab/Map 배치툴")]
        public static void ShowWindow()
        {
            GetWindow<MapExporter>("Map 배치툴");
        }

        private void OnEnable()
        {
            selectedNpcIndex = 0;
            npcExporter = new NpcExporter();
            warpExporter = new WarpExporter();
            tableLoader = new TableLoader();
            _tableMap = tableLoader.LoadMapTable();
            
            _gridTileMap = GameObject.Find(ConfigObjectName.GridTileMap);
            var defaultMap = GameObject.FindObjectOfType<DefaultMap>();
            var player = GameObject.FindWithTag(ConfigTags.Player);
            var tableNpc = tableLoader.LoadNpcTable();
            var tableSpine = tableLoader.LoadSpineTable();

            npcExporter.Initialize(tableNpc, tableSpine, defaultMap, player);
            warpExporter.Initialize(defaultMap, player);
             LoadNpcInfoData();
        }
         private void OnDestroy()
         {
             GameObject obj = GameObject.Find(NameTempTableLoaderManager);
             if (obj)
             {
                 DestroyImmediate(obj);
             }
             GameObject[] maps = GameObject.FindGameObjectsWithTag(ConfigTags.Map);
             foreach (GameObject map in maps)
             {
                 if (map == null) continue;
                 DestroyImmediate(map);
             }
         }
        private void OnGUI()
        {
            GUILayout.Label("* 맵 배치 불러오기", EditorStyles.whiteLargeLabel);
            // 파일 경로 및 파일명 입력
            loadMapUnum = EditorGUILayout.TextField("Map Unum", loadMapUnum);
            // 불러오기 버튼
            if (GUILayout.Button("불러오기"))
            {
                LoadJsonData();
            }
            
            GUILayout.Space(20);
            // NPC 추가 섹션
            GUILayout.Label("* NPC 추가", EditorStyles.whiteLargeLabel);
            // NPC 드롭다운
            selectedNpcIndex = EditorGUILayout.Popup("NPC 선택", selectedNpcIndex, _npcNames.ToArray());
            // NPC 추가 버튼
            if (GUILayout.Button("NPC 추가"))
            {
                npcExporter.AddNpcToMap(selectedNpcIndex);
            }
            
            GUILayout.Space(20);
            // 워프 추가 섹션
            GUILayout.Label("* 워프 추가", EditorStyles.whiteLargeLabel);
            // 워프 추가 버튼
            if (GUILayout.Button("워프 추가"))
            {
                warpExporter.AddWarpToMap();
            }
            
            GUILayout.Space(20);
            GUILayout.Label("* 맵 배치 저장하기", EditorStyles.whiteLargeLabel);
            currentJsonFolderPath = EditorGUILayout.TextField("저장 위치", currentJsonFolderPath);
            // _fileNameRegen = EditorGUILayout.TextField("리젠 파일 이름", _fileNameRegen);
            // _fileNameWarp = EditorGUILayout.TextField("Warp File Name", _fileNameWarp);
            if (GUILayout.Button("Json 으로 저장하기"))
            {
                ExportDataToJson();
            }
        }
        private void ExportDataToJson()
        {
            // 태그가 'Map'인 오브젝트를 찾습니다.
            GameObject mapObject = GameObject.FindGameObjectWithTag(ConfigTags.Map);
        
            if (mapObject == null)
            {
                Debug.LogWarning("No GameObject with the tag 'Map' found in the scene.");
                return;
            }
            
            int mapUnum = int.Parse(loadMapUnum);
            npcExporter.ExportNpcDataToJson(currentJsonFolderPath, jsonFileNameRegen, mapUnum);
            warpExporter.ExportWarpDataToJson(currentJsonFolderPath, jsonFileNameWarp, mapUnum);
            AssetDatabase.Refresh();
        }

        private void LoadJsonData()
        {
            int mapUnum = int.Parse(loadMapUnum);
            var mapData = _tableMap.GetMapData(mapUnum);
            
            LoadTileData();
            npcExporter.LoadNpcData(resourcesFolderPath + mapData.FolderName + "/" + FileNameRegen);
            warpExporter.LoadWarpData(resourcesFolderPath + mapData.FolderName + "/" + FileNameWarp);
        }
        /// <summary>
        /// MapManager.cs:25
        /// </summary>
        private void LoadTileData()
        {
            int mapUnum = int.Parse(loadMapUnum);
            var mapData = _tableMap.GetMapData(mapUnum);
            if (mapData.Unum <= 0)
            {
                FgLogger.LogError("맵 데이터가 없거나 리젠 파일명이 없습니다.");
                return;
            }
            
            if (_defaultMap != null)
            {
                DestroyImmediate(_defaultMap.gameObject);
            }

            string tilemapPath = resourcesFolderPath + mapData.FolderName + "/" + FileNameTilemap;
            GameObject prefab = Resources.Load<GameObject>(tilemapPath);
            if (prefab == null)
            {
                FgLogger.LogError("맵 프리팹이 없습니다. mapUnum : " + mapUnum);
                return;
            }

            if (_gridTileMap == null)
            {
                _gridTileMap = GameObject.Find(ConfigObjectName.GridTileMap);
            }
            currentJsonFolderPath = jsonFolderPath + mapData.FolderName + "/";
            GameObject currentMap = Instantiate(prefab, _gridTileMap.transform);
            _defaultMap = currentMap.GetComponent<MapTiled>();
            _defaultMap.InitializeByEditor(mapData.Unum, mapData.Name, mapData.Type, mapData.Subtype);
            npcExporter.SetDefaultMap(_defaultMap);
            warpExporter.SetDefaultMap(_defaultMap);
        }
        private void LoadNpcInfoData()
        {
            _tableNpc = tableLoader.LoadNpcTable();
             
            Dictionary<int, Dictionary<string, string>> npcDictionary = _tableNpc.GetDatas(); // NPC 데이터를 불러옵니다.
             
            _npcNames = new List<string>();
            // foreach 문을 사용하여 딕셔너리 내용을 출력
            foreach (KeyValuePair<int, Dictionary<string, string>> outerPair in npcDictionary)
            {
                var info = _tableNpc.GetNpcData(outerPair.Key);
                if (info.Unum <= 0) continue;
                _npcNames.Add($"{info.Unum} - {info.Name}");
            }
        }
    }
}
