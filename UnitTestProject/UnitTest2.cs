using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using osu_database_reader.TextFiles;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest2
    {
        private static readonly string OsuPath;

        static UnitTest2()
        {
            OsuPath = $@"C:\Users\{Environment.UserName}\AppData\Local\osu!\";
        }

        [TestMethod]
        public void ReadBeatmap()
        {
            //most people should have this map
            string beatmap = OsuPath + @"Songs\41823 The Quick Brown Fox - The Big Black\The Quick Brown Fox - The Big Black (Blue Dragon) [WHO'S AFRAID OF THE BIG BLACK].osu";

            if (!File.Exists(beatmap))
                Assert.Inconclusive("Hardcoded beatmap not found at location " + beatmap);

            var bm = BeatmapFile.Read(beatmap);
            Debug.WriteLine("Beatmap version: " + bm.BeatmapVersion);

            Assert.IsTrue(bm.SectionGeneral.Count >= 2);   //disco prince only has 2
            Assert.IsTrue(bm.SectionMetadata.Count >= 4);  //disco prince only has 4
            Assert.IsTrue(bm.SectionDifficulty.Count >= 5);//disco prince only has 5

            try {
                string[] required = {"Title", "Artist", "Creator", "Version"};
                foreach (var item in required) {
                    if (!bm.SectionMetadata.ContainsKey(item))
                        Assert.Fail("Important key missing: " + item);
                    Debug.WriteLine($"{item}: {bm.SectionMetadata[item] ?? "(null)"}"); //null is fine
                }
            } catch (Exception e) {
                Assert.Fail("Exception while enumerating important fields: " + e.Message);
            }
        }
    }
}
