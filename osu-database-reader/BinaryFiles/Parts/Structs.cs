using System.Collections.Generic;

namespace osu_database_reader.BinaryFiles.Parts
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
