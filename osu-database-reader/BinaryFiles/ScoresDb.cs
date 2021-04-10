using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Shared.Serialization;
using osu_database_reader.Components.Player;

namespace osu_database_reader.BinaryFiles
{
    public class ScoresDb : ISerializable
    {
        public int OsuVersion;
        public readonly Dictionary<string, List<Replay>> Beatmaps = new();
        public IEnumerable<Replay> Scores => Beatmaps.SelectMany(a => a.Value);

        public static ScoresDb Read(string path)
        {
            using var stream = File.OpenRead(path);
            return Read(stream);
        }

        public static ScoresDb Read(Stream stream)
        {
            var db = new ScoresDb();
            using var r = new SerializationReader(stream);
            db.ReadFromStream(r);

            return db;
        }

        public void ReadFromStream(SerializationReader r)
        {
            OsuVersion = r.ReadInt32();
            int amount = r.ReadInt32();

            for (int i = 0; i < amount; i++)
            {
                string md5 = r.ReadString();

                var tuple = new Tuple<string, List<Replay>>(md5, new List<Replay>());

                int amount2 = r.ReadInt32();
                for (int j = 0; j < amount2; j++)
                    tuple.Item2.Add(Replay.ReadFromReader(r, true));

                Beatmaps.Add(tuple.Item1, tuple.Item2);
            }
        }

        public void WriteToStream(SerializationWriter w)
        {
            throw new NotImplementedException();
        }
    }
}
