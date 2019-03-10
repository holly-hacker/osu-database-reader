using System.IO;
using System.Linq;
using osu.Shared;
using osu_database_reader;
using osu_database_reader.Components.HitObjects;
using osu_database_reader.TextFiles;
using Xunit;

namespace UnitTestProject
{
    public class TestsText
    {
        public TestsText()
        {
            SharedCode.PreTestCheck();
        }

        [Fact]
        public void VerifyBigBlack()
        {
            //most people should have this map
            string beatmap = SharedCode.GetRelativeFile(@"Songs\41823 The Quick Brown Fox - The Big Black\The Quick Brown Fox - The Big Black (Blue Dragon) [WHO'S AFRAID OF THE BIG BLACK].osu");

            Skip.IfNot(File.Exists(beatmap), "Hardcoded path does not exist:  " + beatmap);
            Skip.IfNot(SharedCode.VerifyFileChecksum(beatmap, "2D687E5EE79F3862AD0C60651471CDCC"), "Beatmap was modified.");

            var bm = BeatmapFile.Read(beatmap);

            Assert.Equal(9, bm.FileFormatVersion);

            //check General
            Assert.Equal("02 The Big Black.mp3", bm.SectionGeneral["AudioFilename"]);
            Assert.Equal("0", bm.SectionGeneral["AudioLeadIn"]);
            Assert.Equal("18957", bm.SectionGeneral["PreviewTime"]);

            //check Metadata
            Assert.Equal("The Big Black", bm.Title);
            Assert.Equal("The Quick Brown Fox", bm.Artist);
            Assert.Equal("Blue Dragon", bm.Creator);
            Assert.Equal("WHO'S AFRAID OF THE BIG BLACK", bm.Version);
            Assert.Equal(bm.Source, string.Empty);
            Assert.Equal(bm.Tags, new[] { "Onosakihito", "speedcore", "renard", "lapfox" });

            //check Difficulty
            Assert.Equal(5f, bm.HPDrainRate);
            Assert.Equal(4f, bm.CircleSize);
            Assert.Equal(7f, bm.OverallDifficulty);
            Assert.Equal(10f, bm.ApproachRate);
            Assert.Equal(1.8f, bm.SliderMultiplier);
            Assert.Equal(2f, bm.SliderTickRate);

            //check Events
            //TODO

            //check TimingPoints
            Assert.Equal(5, bm.TimingPoints.Count);
            Assert.Equal(6966, bm.TimingPoints[0].Time);
            Assert.True(bm.TimingPoints[1].Kiai);
            Assert.Equal(bm.TimingPoints[2].MsPerQuarter, -100); //means no timing change
            Assert.False(bm.TimingPoints[3].TimingChange);

            //check Colours
            //Combo1 : 249,91,9
            //(...)
            //Combo5 : 255,255,128
            Assert.Equal("249,91,91", bm.SectionColours["Combo1"]);
            Assert.Equal("255,255,128", bm.SectionColours["Combo5"]);
            Assert.Equal(5, bm.SectionColours.Count);

            //check HitObjects
            Assert.Equal(746, bm.HitObjects.Count);
            Assert.Equal(410, bm.HitObjects.Count(a => a is HitObjectCircle));
            Assert.Equal(334, bm.HitObjects.Count(a => a is HitObjectSlider));
            Assert.Equal(2, bm.HitObjects.Count(a => a is HitObjectSpinner));

            //56,56,6966,1,4
            HitObjectCircle firstCircle = (HitObjectCircle) bm.HitObjects.First(a => a.Type.HasFlag(HitObjectType.Normal));
            Assert.Equal(56, firstCircle.X);
            Assert.Equal(56, firstCircle.Y);
            Assert.Equal(6966, firstCircle.Time);
            Assert.Equal(HitSound.Finish, firstCircle.HitSound);

            //178,50,7299,2,0,B|210:0|300:0|332:50,1,180,2|0
            HitObjectSlider firstSlider = (HitObjectSlider)bm.HitObjects.First(a => a.Type.HasFlag(HitObjectType.Slider));
            Assert.Equal(178, firstSlider.X);
            Assert.Equal(50, firstSlider.Y);
            Assert.Equal(7299, firstSlider.Time);
            Assert.Equal(HitSound.None, firstSlider.HitSound);
            Assert.Equal(CurveType.Bezier, firstSlider.CurveType);
            Assert.Equal(3, firstSlider.Points.Count);
            Assert.Equal(1, firstSlider.RepeatCount);
            Assert.Equal(180, firstSlider.Length);

            //256,192,60254,12,4,61587
            HitObjectSpinner firstSpinner = (HitObjectSpinner)bm.HitObjects.First(a => a.Type.HasFlag(HitObjectType.Spinner));
            Assert.Equal(256, firstSpinner.X);
            Assert.Equal(192, firstSpinner.Y);
            Assert.Equal(60254, firstSpinner.Time);
            Assert.Equal(HitSound.Finish, firstSpinner.HitSound);
            Assert.Equal(61587, firstSpinner.EndTime);
            Assert.True(firstSpinner.IsNewCombo);
        }
    }
}
