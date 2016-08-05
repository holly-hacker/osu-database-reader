using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_database_reader
{
    public class PresenceDb
    {
        public int OsuVersion;
        public int AmountOfPlayers => Players.Count;
        public List<Player> Players = new List<Player>();

        public static PresenceDb Read(string path) {
            var db = new PresenceDb();
            using (CustomReader r = new CustomReader(File.OpenRead(path))) {
                db.OsuVersion = r.ReadInt32();
                int amount = r.ReadInt32();

                for (int i = 0; i < amount; i++) {
                    db.Players.Add(Player.ReadFromReader(r));
                }
            }

            return db;
        }
    }
}
