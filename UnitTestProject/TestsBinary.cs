using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using osu_database_reader;
using osu_database_reader.BinaryFiles;
using osu_database_reader.Components.Player;

namespace UnitTestProject
{
    [TestClass]
    public class TestsBinary
    {
        [TestInitialize]
        public void Init()
        {
            SharedCode.PreTestCheck();
        }

        [TestMethod]
        public void ReadOsuDb()
        {
            OsuDb db = OsuDb.Read(SharedCode.GetRelativeFile("osu!.db"));
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
        public void ReadCollectionDb()
        {
            CollectionDb db = CollectionDb.Read(SharedCode.GetRelativeFile("collection.db"));
            Debug.WriteLine("Version: " + db.OsuVersion);
            Debug.WriteLine("Amount of collections: " + db.Collections.Count);
            foreach (var c in db.Collections) {
                Debug.WriteLine($" - Collection {c.Name} with {c.Md5Hashes.Count} item" + (c.Md5Hashes.Count == 1 ? "" : "s"));
            }
        }

        [TestMethod]
        public void ReadScoresDb()
        {
            ScoresDb db = ScoresDb.Read(SharedCode.GetRelativeFile("scores.db"));
            Debug.WriteLine("Version: " + db.OsuVersion);
            Debug.WriteLine("Amount of beatmaps: " + db.Beatmaps.Count);
            Debug.WriteLine("Amount of scores: " + db.Scores.Count());

            string[] keys = db.Beatmaps.Keys.ToArray();
            for (int i = 0; i < Math.Min(25, keys.Length); i++) {   //print 25 at most
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
        public void ReadPresenceDb()
        {
            var db = PresenceDb.Read(SharedCode.GetRelativeFile("presence.db"));
            Debug.WriteLine("Version: " + db.OsuVersion);
            Debug.WriteLine("Amount of scores: " + db.Players.Count);

            for (int i = 0; i < Math.Min(db.Players.Count, 10); i++) {    //10 at most
                var p = db.Players[i];
                Debug.WriteLine($"Player {p.PlayerName} lives at long {p.Longitude} and lat {p.Latitude}. Some DateTime: {p.Unknown1}. Rank: {p.PlayerRank}. {p.GameMode}, #{p.GlobalRank}, id {p.PlayerId}");
            }
        }

        [TestMethod]
        public void ReadReplay()
        {
            //get random file
            string path = SharedCode.GetRelativeDirectory("Replays");
            string[] files = Directory.GetFiles(path);
            
            if (files == null || files.Length == 0)
                Assert.Inconclusive("No replays in your replay folder!");

            for (int i = 0; i < Math.Min(files.Length, 10); i++) {  //10 at most
                var r = Replay.Read(files[i]);
                Debug.WriteLine("Version: " + r.OsuVersion);
                Debug.WriteLine("Beatmap hash: " + r.BeatmapHash);
                Debug.WriteLine($"Replay by {r.PlayerName}, for {r.Score} score with {r.Combo}x combo. Played at {r.TimePlayed}");
                Debug.WriteLine("");
            }
        }
    }
}
