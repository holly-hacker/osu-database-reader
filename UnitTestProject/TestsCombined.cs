using System;
using System.Diagnostics;
using System.IO;
using osu_database_reader.BinaryFiles;
using osu_database_reader.TextFiles;
using Xunit;

namespace UnitTestProject
{
    public class TestsCombined
    {
        public TestsCombined()
        {
            SharedCode.PreTestCheck();
        }

        [Fact]
        public void CheckBeatmapsAgainstDb()
        {
            OsuDb db = OsuDb.Read(SharedCode.GetRelativeFile("osu!.db"));

            for (var i = 0; i < Math.Min(db.Beatmaps.Count, 50); i++) {
                var entry = db.Beatmaps[i];

                Debug.WriteLine($"Going to read beatmap at /{entry.FolderName}/{entry.BeatmapFileName}");

                //just make sure the songs folder exists
                SharedCode.GetRelativeDirectory("Songs");

                //read beatmap
                try
                {
                    BeatmapFile bm = BeatmapFile.Read(SharedCode.GetRelativeFile(Path.Combine("Songs", entry.FolderName, entry.BeatmapFileName), true));
                    //BUG: this can still fail when maps use the hold note (used in some mania maps?)

                    Assert.True(bm.SectionGeneral.Count >= 2);       //disco prince only has 2
                    Assert.True(bm.SectionMetadata.Count >= 4);      //disco prince only has 4
                    Assert.True(bm.SectionDifficulty.Count >= 5);    //disco prince only has 5

                    Assert.Equal(entry.Artist, bm.Artist);
                    Assert.Equal(entry.Version, bm.Version);
                    Assert.Equal(entry.Creator, bm.Creator);
                    Assert.Equal(entry.Title, bm.Title);

                    //TODO: more, but check if the entries are present in the beatmap
                }
                catch (NotImplementedException e)
                {
                    Assert.Equal("Hold notes are not yet parsed.", e.Message);  //Unexpected exception thrown
                }
            }
        }
    }
}
