using System.Collections.Generic;
using System.IO;
using osu_database_reader.Components.Player;
using osu_database_reader.IO;

namespace osu_database_reader.BinaryFiles
{
    public class PresenceDb
    {
        public int OsuVersion;
        public List<PlayerPresence> Players = new List<PlayerPresence>();

        public static PresenceDb Read(string path) {
            var db = new PresenceDb();
            using (CustomBinaryReader r = new CustomBinaryReader(File.OpenRead(path))) {
                db.OsuVersion = r.ReadInt32();
                int amount = r.ReadInt32();

                for (int i = 0; i < amount; i++) {
                    db.Players.Add(PlayerPresence.ReadFromReader(r));
                }
            }

            return db;
        }
    }
}
