#if UNITY_EDITOR
using System.Collections.Generic;

namespace RedOwl.Engine
{
    public static class AddressableDatabase
    {
        public static Dictionary<string, IList<string>> Table;

        public static void Add(string guid, IEnumerable<string> keys)
        {
            if (Table == null) Table = new Dictionary<string, IList<string>>();
            if (!Table.TryGetValue(guid, out var current))
            {
                current = new List<string>();
                Table[guid] = current;
            }
            foreach (string key in keys)
            {
                current.Add(key);
            }
        }
    }
}
#endif
