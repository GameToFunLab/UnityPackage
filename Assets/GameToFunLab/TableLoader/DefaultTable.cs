using System.Collections.Generic;

namespace GameToFunLab.TableLoader
{
    public class DefaultTable
    {
        private readonly Dictionary<int, Dictionary<string, string>> table = new Dictionary<int, Dictionary<string, string>>();

        public virtual void LoadData(string content)
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

                int vnum = int.Parse(values[0]);
                table[vnum] = data;
            }
        }

        protected string CheckNone(string value)
        {
            return value == "None" ? "" : value;
        }
        public Dictionary<int, Dictionary<string, string>> GetDatas() => table;
        protected Dictionary<string, string> GetData(int vnum) => table.GetValueOrDefault(vnum);
        // protected string GetDataColumn(int vnum, string columnName) => table.TryGetValue(vnum, out var data) && data.TryGetValue(columnName, out var value) ? value : null;
        protected string GetDataColumn(int vnum, string columnName)
        {
            table.TryGetValue(vnum, out var data);
            if (data == null)
            {
                return null;
            }

            data.TryGetValue(columnName, out var value);
            return value == null ? null : CheckNone(value);
        }
    }
}