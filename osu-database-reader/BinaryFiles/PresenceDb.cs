using System.Collections.Generic;
using System.IO;
using osu.Shared.Serialization;
using osu_database_reader.Components.Player;

namespace osu_database_reader.BinaryFiles
{
    public class PresenceDb : ISerializable
    {
        public int OsuVersion;
        public readonly List<PlayerPresence> Players = new();

        public static PresenceDb Read(string path) {
            var db = new PresenceDb();
            using var r = new SerializationReader(File.OpenRead(path));
            db.ReadFromStream(r);
            return db;
        }

        public void ReadFromStream(SerializationReader r)
        {
            OsuVersion = r.ReadInt32();

            int amount = r.ReadInt32();
            for (int i = 0; i < amount; i++)
                Players.Add(PlayerPresence.ReadFromReader(r));
        }

        public void WriteToStream(SerializationWriter w)
        {
            throw new System.NotImplementedException();
        }
    }
}
