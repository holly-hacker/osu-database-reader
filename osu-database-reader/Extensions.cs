using System.Collections.Generic;

namespace osu_database_reader
{
    internal static class Extensions
    {
        public static string GetValueOrNull(this Dictionary<string, string> dic, string key)
        {
            return dic.ContainsKey(key) 
                ? dic[key] 
                : null;
        }
    }
}
