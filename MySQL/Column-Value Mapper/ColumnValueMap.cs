using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    public class ColumnValueMap
    {
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();

        public void Add(string columnName, string value) => _values.Add(columnName, value);

        public string this[string columnName] => _values.TryGetValue(columnName, out var val) ? val : null;

        public void Clear() => _values.Clear();

        public IReadOnlyDictionary<string, string> Raw => _values;
    }

    public static class ColumnValueMapExtension
    {
        public static string GetValue<T>(this ColumnValueMap map, T Column) where T : Enum
            => map[Column.ToString()];
    }
}
