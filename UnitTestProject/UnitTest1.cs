using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        public void ReadOsuDb() {
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

        [TestMethod]
        public void ReadScoresDb() {
            ScoresDb db = ScoresDb.Read(OsuPath + "scores.db");
            Debug.WriteLine("Version: " + db.OsuVersion);
            Debug.WriteLine("Amount of beatmaps: " + db.AmountOfBeatmaps);
            Debug.WriteLine("Amount of scores: " + db.AmountOfScores);

            string[] keys = db.Beatmaps.Keys.ToArray();
            for (int i = 0; i < Math.Min(25, db.AmountOfBeatmaps); i++) {   //print 25 at most
                string md5 = keys[i];
                List<Replay> replays = db.Beatmaps[md5];

                Debug.WriteLine($"Beatmap with md5 {md5} has replays:");
                for (int j = 0; j < Math.Min(5, replays.Count); j++) {      //again, 5 at most
                    var r = replays[j];
                    Debug.WriteLine($"\tReplay by {r.PlayerName}, for {r.Score} score with {r.Combo}x combo. Played at {r.TimePlayed}");
                }
            }
        }

        [TestMethod]
        public void ReadPresenceDb() {
            var db = PresenceDb.Read(OsuPath + "presence.db");
            Debug.WriteLine("Version: " + db.OsuVersion);
            Debug.WriteLine("Amount of scores: " + db.AmountOfPlayers);

            for (int i = 0; i < Math.Min(db.AmountOfPlayers, 10); i++) {    //10 at most
                var p = db.Players[i];
                Debug.WriteLine($"Player {p.PlayerName} lives at long {p.Longitude} and lat {p.Latitude}. Some DateTime: {p.Unknown1}. Rank: {p.PlayerRank}. {p.GameMode}, #{p.GlobalRank}, id {p.PlayerId}");
            }
        }

        [TestMethod]
        public void ReadReplay() {
            //get random path
            string path = OsuPath + "Replays\\";
            string[] files = Directory.GetFiles(path);

            Assert.IsNotNull(files);
            Assert.AreNotEqual(files.Length, 0, "No replays in your replay folder!");

            for (int i = 0; i < Math.Max(files.Length, 10); i++) {  //10 at most
                var r = Replay.Read(files[i]);
                Debug.WriteLine("Version: " + r.OsuVersion);
                Debug.WriteLine("Beatmap hash: " + r.BeatmapHash);
                Debug.WriteLine($"Replay by {r.PlayerName}, for {r.Score} score with {r.Combo}x combo. Played at {r.TimePlayed}");
                Debug.WriteLine("");
            }
        }
    }
}
