using System.Collections.Generic;
using System.IO;
using osu_database_reader.BinaryFiles.Parts;
using osu_database_reader.IO;

namespace osu_database_reader.BinaryFiles
{
    public class CollectionDb
    {
        public int OsuVersion;
        public List<Collection> Collections = new List<Collection>();

        public static CollectionDb Read(string path) {
            var db = new CollectionDb();
            using (CustomBinaryReader r = new CustomBinaryReader(File.OpenRead(path))) {
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
