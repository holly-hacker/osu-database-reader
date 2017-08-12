using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
