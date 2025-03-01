using System;
using GameToFunLab.Core;
using GameToFunLab.Runtime.Scripts.TableLoader;
using GameToFunLab.TableLoader;
using UnityEngine;

namespace GameToFunLab.Editor
{
    public class TableLoader
    {
        // 공통적인 로드 메서드로, 제네릭 타입과 파일명을 받아 로드
        private T LoadTable<T>(string fileName) where T : DefaultTable, new()
        {
            T tableData = null;
            try
            {
                TextAsset textFile = Resources.Load<TextAsset>($"Tables/{fileName}");
                if (textFile != null)
                {
                    string content = textFile.text;
                    if (!string.IsNullOrEmpty(content))
                    {
                        tableData = new T();
                        tableData.LoadData(content);
                    }
                    else
                    {
                        FgLogger.LogError($"Content is empty in file Tables/{fileName}");
                    }
                }
                else
                {
                    FgLogger.LogError($"File not found: Tables/{fileName}");
                }
            }
            catch (Exception ex)
            {
                FgLogger.LogError($"Error reading file Tables/{fileName}: {ex.Message}");
            }

            return tableData;
        }

        public TableMap LoadMapTable()
        {
            return LoadTable<TableMap>("map");
        }

        public TableNpc LoadNpcTable()
        {
            return LoadTable<TableNpc>("npc");
        }
        public TableSpine LoadSpineTable()
        {
            return LoadTable<TableSpine>("spine");
        }
    }
}