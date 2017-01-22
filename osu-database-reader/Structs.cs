using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_database_reader
{
    public struct TimingPoint
    {
        public double Time, MsPerQuarter;
        public bool NotInherited;
    }

    public struct Collection
    {
        public string Name;
        public List<string> Md5Hashes;
    }
}
