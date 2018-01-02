using System.Collections.Generic;
using osu.Shared.Serialization;

namespace osu_database_reader.Components.Beatmaps
{
    public struct Collection : ISerializable
    {
        public string Name;
        public List<string> BeatmapHashes;

        public void ReadFromStream(SerializationReader r)
        {
            Name = r.ReadString();

            BeatmapHashes = new List<string>();
            int amount = r.ReadInt32();
            for (int j = 0; j < amount; j++)
                BeatmapHashes.Add(r.ReadString());
        }

        public void WriteToStream(SerializationWriter w)
        {
            throw new System.NotImplementedException();
        }
    }
}
