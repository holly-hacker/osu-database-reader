using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using osu_database_reader.BinaryFiles;
using osu_database_reader.TextFiles;

namespace UnitTestProject
{
    [TestClass]
    public class TestsCombined
    {
        [TestInitialize]
        public void Init()
        {
            SharedCode.PreTestCheck();
        }

        [TestMethod]
        public void CheckBeatmapsAgainstDb()
        {
            OsuDb db = OsuDb.Read(SharedCode.GetRelativeFile("osu!.db"));

            for (var i = 0; i < Math.Min(db.Beatmaps.Count, 50); i++) {
                var entry = db.Beatmaps[i];

                Debug.WriteLine($"Going to read beatmap at /{entry.FolderName}/{entry.BeatmapFileName}");

                //read beatmap
                BeatmapFile bm = BeatmapFile.Read(SharedCode.GetRelativeFile($"Songs/{entry.FolderName}/{entry.BeatmapFileName}", true));
                //BUG: this can still fail when maps use the hold note (used in some mania maps?)

                Assert.AreEqual(entry.Artist, bm.SectionMetadata["Artist"]);
                Assert.AreEqual(entry.Difficulty, bm.SectionMetadata["Version"]);
                Assert.AreEqual(entry.Creator, bm.SectionMetadata["Creator"]);
                Assert.AreEqual(entry.Title, bm.SectionMetadata["Title"]);

                //TODO: more, but check if the entries are present in the beatmap
            }
        }
    }
}
