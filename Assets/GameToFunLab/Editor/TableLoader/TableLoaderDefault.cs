using System;
using System.Collections.Generic;
using GameToFunLab.Core;
using UnityEngine;

namespace GameToFunLab.Editor.TableLoader
{
    public class TableLoaderDefault
    {
        private readonly Dictionary<int, Dictionary<string, string>> table = new Dictionary<int, Dictionary<string, string>>();

        public virtual void LoadData(string fileName)
        {
            try
            {
                TextAsset textFile = Resources.Load<TextAsset>($"Tables/{fileName}");
                if (textFile != null)
                {
                    string content = textFile.text;
                    if (!string.IsNullOrEmpty(content))
                    {
                        string[] lines = content.Split('\n');
                        string[] headers = lines[0].Trim().Split('\t');

                        for (int i = 1; i < lines.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(lines[i]) || lines[i].StartsWith("#")) continue;
                            string[] values = lines[i].Split('\t');
                            var data = new Dictionary<string, string>();

                            for (int j = 0; j < headers.Length; j++)
                            {
                                data[headers[j].Trim()] = values[j].Trim().Replace(@"\n", "\n");
                            }

                            int unum = int.Parse(values[0]);
                            table[unum] = data;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FgLogger.LogError($"Error reading file {fileName}: {ex.Message}");
            }
        }

        protected string CheckNone(string value)
        {
            return value == "None" ? "" : value;
        }
        public Dictionary<int, Dictionary<string, string>> GetDatas() => table;
        protected Dictionary<string, string> GetData(int unum) => table.GetValueOrDefault(unum);
        protected string GetDataColumn(int unum, string columnName)
        {
            table.TryGetValue(unum, out var data);
            if (data == null)
            {
                return null;
            }

            data.TryGetValue(columnName, out var value);
            return value == null ? null : CheckNone(value);
        }
    }
}