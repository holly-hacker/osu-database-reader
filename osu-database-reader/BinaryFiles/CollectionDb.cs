using System.Collections.Generic;
using System.IO;
using osu.Shared.Serialization;
using osu_database_reader.Components.Beatmaps;

namespace osu_database_reader.BinaryFiles
{
    public class CollectionDb : ISerializable
    {
        public int OsuVersion;
        public readonly List<Collection> Collections = new();

        public static CollectionDb Read(string path)
        {
            using var stream = File.OpenRead(path);
            return Read(stream);
        }

        public static CollectionDb Read(Stream stream)
        {
            var db = new CollectionDb();
            using var r = new SerializationReader(stream);
            db.ReadFromStream(r);
            return db;
        }

        public void ReadFromStream(SerializationReader r)
        {
            OsuVersion = r.ReadInt32();
            int amount = r.ReadInt32();

            for (int i = 0; i < amount; i++) {
                var c = new Collection();
                c.ReadFromStream(r);
                Collections.Add(c);
            }
        }

        public void WriteToStream(SerializationWriter w)
        {
            throw new System.NotImplementedException();
        }
    }
}
