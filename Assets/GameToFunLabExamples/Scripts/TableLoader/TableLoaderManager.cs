using System;
using System.Collections;
using FocusGame.Core.TableLoader;
using GameToFunLab.Core;
using Scripts.Scenes;
using UnityEngine;

namespace Scripts.TableLoader
{
    public class TableLoaderManager : MonoBehaviour
    {
        private static readonly string[] DataFiles = { "config", "animation", "monster", "item", "exp" };

        public static TableLoaderManager Instance { get; private set; }
        private float loadProgress;
        private SceneLoading sceneLoading;

        public TableConfig TableConfig { get; private set; } = new TableConfig();
        public TableAnimation TableAnimation { get; private set; } = new TableAnimation();
        public TableMonster TableMonster { get; private set; } = new TableMonster();
        public TableItem TableItem { get; private set; } = new TableItem();
        public TableExp TableExp { get; private set; } = new TableExp();

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
            SceneManager.LoadSceneByName("LoadingServer");
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
                                TableAnimation.LoadData(content);
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
        }
    }
}
