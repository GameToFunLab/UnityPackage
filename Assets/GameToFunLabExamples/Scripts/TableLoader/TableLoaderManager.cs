using System;
using System.Collections;
using GameToFunLab.Core;
using Scripts.Scenes;
using UnityEngine;

namespace Scripts.TableLoader
{
    public class TableLoaderManager : MonoBehaviour
    {
        private static readonly string[] DataFiles = { "config", "animation", "monster", "item", "exp", "map", "map_regen" };

        public static TableLoaderManager Instance { get; private set; }
        private float loadProgress;
        private SceneLoading sceneLoading;

        public TableConfig TableConfig { get; private set; } = new TableConfig();
        public TableSpine TableSpine { get; private set; } = new TableSpine();
        public TableMonster TableMonster { get; private set; } = new TableMonster();
        public TableItem TableItem { get; private set; } = new TableItem();
        public TableExp TableExp { get; private set; } = new TableExp();
        public TableMap TableMap { get; private set; } = new TableMap();
        public TableMapRegen TableMapRegen { get; private set; } = new TableMapRegen();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            sceneLoading = GameObject.Find("SceneLoading").GetComponent<SceneLoading>();
            loadProgress = 0f;
        }

        private void Start()
        {
            StartCoroutine(LoadAllDataFiles());
        }

        private IEnumerator LoadAllDataFiles()
        {
            int fileCount = DataFiles.Length;
            for (int i = 0; i < fileCount; i++)
            {
                LoadDataFile(DataFiles[i]);
                loadProgress = (float)(i + 1) / fileCount * 100f;
                sceneLoading?.SetTextLoadingPercent(loadProgress);
                yield return new WaitForSeconds(0.1f);
            }

            OnEndLoad();
        }
        private void LoadDataFile(string fileName)
        {
            try
            {
                TextAsset textFile = Resources.Load<TextAsset>($"Tables/{fileName}");
                if (textFile != null)
                {
                    string content = textFile.text;
                    if (!string.IsNullOrEmpty(content))
                    {
                        switch (fileName)
                        {
                            case "config":
                                TableConfig.LoadData(content);
                                break;
                            case "animation":
                                TableSpine.LoadData(content);
                                break;
                            case "monster":
                                TableMonster.LoadData(content);
                                break;
                            case "item":
                                TableItem.LoadData(content);
                                break;
                            case "exp":
                                TableExp.LoadData(content);
                                break;
                            case "map":
                                TableMap.LoadData(content);
                                break;
                            case "map_regen":
                                TableMapRegen.LoadData(content);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FgLogger.LogError($"Error reading file {fileName}: {ex.Message}");
            }
        }

        private static void OnEndLoad()
        {
            // 로드 완료 후의 로직 추가
            SceneManager.LoadSceneByName("Game");
        }
    }
}
