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
                try
                {
                    BeatmapFile bm = BeatmapFile.Read(SharedCode.GetRelativeFile($"Songs/{entry.FolderName}/{entry.BeatmapFileName}", true));
                    //BUG: this can still fail when maps use the hold note (used in some mania maps?)

                    Assert.IsTrue(bm.SectionGeneral.Count >= 2);       //disco prince only has 2
                    Assert.IsTrue(bm.SectionMetadata.Count >= 4);      //disco prince only has 4
                    Assert.IsTrue(bm.SectionDifficulty.Count >= 5);    //disco prince only has 5

                    Assert.AreEqual(entry.Artist, bm.Artist);
                    Assert.AreEqual(entry.Version, bm.Version);
                    Assert.AreEqual(entry.Creator, bm.Creator);
                    Assert.AreEqual(entry.Title, bm.Title);

                    //TODO: more, but check if the entries are present in the beatmap
                }
                catch (NotImplementedException e)
                {
                    Assert.AreEqual(e.Message, "Hold notes are not yet parsed.", "Unexpected exception thrown");
                }
            }
        }
    }
}
