using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_database_reader
{
    public class CollectionDb
    {
        public int OsuVersion;
        public int AmountOfCollections => Collections.Count;
        public List<Collection> Collections = new List<Collection>();

        public static CollectionDb Read(string path) {
            var db = new CollectionDb();
            using (CustomReader r = new CustomReader(File.OpenRead(path))) {
                db.OsuVersion = r.ReadInt32();
                int amount = r.ReadInt32();

                for (int i = 0; i < amount; i++) {
                    var c = new Collection();
                    c.Md5Hashes = new List<string>();
                    c.Name = r.ReadString();
                    int amount2 = r.ReadInt32();
                    for (int j = 0; j < amount2; j++) c.Md5Hashes.Add(r.ReadString());

                    db.Collections.Add(c);
                }
            }

            return db;
        }
    }
}
