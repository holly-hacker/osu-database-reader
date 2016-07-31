using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using osu_database_reader;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly string OsuPath;

        static UnitTest1() {
            OsuPath = $@"C:\Users\{Environment.UserName}\AppData\Local\osu!\";
        }

        [TestMethod]
        public void ReadOsuDb()
        {
            OsuDb db = OsuDb.Read(OsuPath + "osu!.db");
            Debug.WriteLine("Version: " + db.OsuVersion);
            Debug.WriteLine("Amount of beatmaps: " + db.AmountOfBeatmaps);
            Debug.WriteLine($"Account name: {db.AccountName} (account {(db.AccountUnlocked ? "unlocked" : "locked, unlocked at "+db.AccountUnlockDate)})");
            Debug.WriteLine("Account rank: " + db.AccountRank);
            for (int i = 0; i < Math.Min(10, db.AmountOfBeatmaps); i++) {   //print 10 at most
                var b = db.Beatmaps[i];
                Debug.WriteLine($"{b.Artist} - {b.Title} [{b.Difficulty}]");
            }
        }

        [TestMethod]
        public void ReadCollectionDb() {
            CollectionDb db = CollectionDb.Read(OsuPath + "collection.db");
            Debug.WriteLine("Version: " + db.OsuVersion);
            Debug.WriteLine("Amount of collections: " + db.AmountOfCollections);
            foreach (var c in db.Collections)
                Debug.WriteLine($" - Collection {c.Name} with {c.Md5Hashes.Count} item" + (c.Md5Hashes.Count == 1 ? "" : "s"));
        }
    }
}
