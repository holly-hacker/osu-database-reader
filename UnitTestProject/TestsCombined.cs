using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void CheckBeatmapsAgainstDatabase()
        {
            string pathSongs = SharedCode.GetRelativeDirectory("Songs");
            OsuDb db = OsuDb.Read(SharedCode.GetRelativeFile("osu!.db"));

            for (var i = 0; i < Math.Max(db.Beatmaps.Count, 25); i++) {
                var entry = db.Beatmaps[i];

                //read beatmap
                BeatmapFile bm = BeatmapFile.Read(SharedCode.GetRelativeFile($"Songs/{entry.FolderName}/{entry.BeatmapFileName}", true));
                
                Assert.AreEqual(entry.Artist, bm.SectionMetadata["Artist"]);
                Assert.AreEqual(entry.Difficulty, bm.SectionMetadata["Version"]);
                Assert.AreEqual(entry.Creator, bm.SectionMetadata["Creator"]);
                Assert.AreEqual(entry.Title, bm.SectionMetadata["Title"]);

                //TODO: more, but check if the entries are present in the beatmap
            }
        }
    }
}
