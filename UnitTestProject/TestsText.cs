﻿using System.Linq;
using System.Reflection;
using FluentAssertions;
using osu_database_reader;
using osu.Shared;
using osu_database_reader.Components.Events;
using osu_database_reader.Components.HitObjects;
using osu_database_reader.TextFiles;
using Xunit;

namespace UnitTestProject
{
    public class TestsText
    {
        [Fact]
        public void CheckBeatmapV14()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Beatmaps.beatmap_v14.osu");
            var bm = BeatmapFile.Read(stream);

            bm.FileFormatVersion.Should().Be(14);

            // general
            bm.AudioFilename.Should().Be("audio.mp3");
            bm.AudioLeadIn.Should().Be(0);
            bm.PreviewTime.Should().Be(64366);
            bm.Countdown.Should().BeTrue();
            // bm.SampleSet.Should().Be("audio.mp3");
            bm.StackLeniency.Should().Be(0.7f);
            bm.Mode.Should().Be(0);
            bm.LetterboxInBreaks.Should().BeFalse();
            bm.WidescreenStoryboard.Should().BeTrue();

            // editor
            bm.Bookmarks.Should().BeEquivalentTo(1500, 7614, 13729, 22901, 35130, 47359, 59589, 64175, 90162, 93219, 105449);
            bm.DistanceSpacing.Should().Be(0.3);
            bm.BeatDivisor.Should().Be(4);
            bm.GridSize.Should().Be(8);
            bm.TimelineZoom.Should().BeApproximately(1.950003f, 0.000001f);

            // metadata
            bm.Title.Should().Be("The Whole Rest");
            bm.TitleUnicode.Should().Be("The Whole Rest");
            bm.Artist.Should().Be("KIVA");
            bm.ArtistUnicode.Should().Be("KIVΛ");
            bm.Creator.Should().Be("Ilex");
            bm.Version.Should().Be("Insane v1");
            bm.Source.Should().Be("Cytus II");
            bm.Tags.Should().Be("Cytus II 2 OST");
            bm.BeatmapID.Should().Be(2026717);
            bm.BeatmapSetID.Should().Be(968597);

            // difficulty
            bm.HPDrainRate.Should().Be(6);
            bm.CircleSize.Should().Be(4);
            bm.OverallDifficulty.Should().Be(7.3f);
            bm.ApproachRate.Should().Be(9.4f);
            bm.SliderMultiplier.Should().Be(1.8);
            bm.SliderTickRate.Should().Be(1);

            // events
            bm.Events.Should().HaveCount(4);

            bm.Events[0].Should().BeOfType<BackgroundEvent>();
            ((BackgroundEvent) bm.Events[0]).Path.Should().Be("usedtobe.jpg");
            bm.Events[1].Should().BeOfType<VideoEvent>();
            ((VideoEvent) bm.Events[1]).Path.Should().Be("Cytus II Opening - The Whole Rest.mp4");
            ((VideoEvent) bm.Events[1]).Offset.Should().Be(0);
            bm.Events[2].Should().BeOfType<BreakEvent>();
            ((BreakEvent) bm.Events[2]).StartTime.Should().Be(13929);
            ((BreakEvent) bm.Events[2]).EndTime.Should().Be(22301);
            bm.Events[3].Should().BeOfType<BreakEvent>();
            ((BreakEvent) bm.Events[3]).StartTime.Should().Be(47559);
            ((BreakEvent) bm.Events[3]).EndTime.Should().Be(52874);

            // timing points
            bm.TimingPoints.Should().HaveCount(7);
            bm.TimingPoints[0].Time.Should().Be(1500);
            bm.TimingPoints[0].Kiai.Should().BeFalse();
            bm.TimingPoints[0].MsPerQuarter.Should().Be(382.165605095541);
            bm.TimingPoints[0].SampleSet.Should().Be(1);
            bm.TimingPoints[0].SampleVolume.Should().Be(100);
            bm.TimingPoints[0].CustomSampleSet.Should().Be(0);
            bm.TimingPoints[0].TimingChange.Should().BeTrue();
            bm.TimingPoints[0].TimingSignature.Should().Be(4);

            bm.TimingPoints[1].MsPerQuarter.Should().Be(-133.333333333333);

            // hit objects
            bm.HitObjects.Should().HaveCount(335);
            bm.HitObjects.Where(x => (x.Type & HitObjectType.Normal) != 0).Should().HaveCount(252);
            bm.HitObjects.Where(x => (x.Type & HitObjectType.Slider) != 0).Should().HaveCount(83);

            var firstNoteBase = bm.HitObjects[0];
            firstNoteBase.Should().BeOfType<HitObjectCircle>();
            var firstNote = (HitObjectCircle)firstNoteBase;
            firstNote.Time.Should().Be(1500);
            firstNote.X.Should().Be(136);
            firstNote.Y.Should().Be(87);
            firstNote.IsNewCombo.Should().BeTrue();
            firstNote.Type.Should().Be(HitObjectType.Normal | HitObjectType.NewCombo);
            firstNote.HitSound.Should().Be(HitSound.None);
            firstNote.SoundSampleData.Should().Be("0:0:0:0:");
        }

        [Fact]
        public void FloatingPointCoordinates()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Beatmaps.128.osu");
            var bm = BeatmapFile.Read(stream);

            bm.FileFormatVersion.Should().Be(3);
            bm.BeatmapID.Should().Be(128);
            bm.BeatmapSetID.Should().Be(46);

            // hit objects
            bm.HitObjects.Should().HaveCount(76);

            var noteWithFloat1 = bm.HitObjects[68];
            var note1 = noteWithFloat1.Should().BeOfType<HitObjectSlider>().Which;
            note1.Time.Should().Be(71034);
            note1.X.Should().Be(64);
            note1.Y.Should().Be(320);

            var noteWithFloat2 = bm.HitObjects[69];
            var note2 = noteWithFloat2.Should().BeOfType<HitObjectSlider>().Which;
            note2.Time.Should().Be(72812);
            note2.X.Should().Be(447);
            note2.Y.Should().Be(223);
        }

        [Fact]
        public void FloatingPointSliderCoordinates()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Beatmaps.4615284.osu");
            var bm = BeatmapFile.Read(stream);

            bm.FileFormatVersion.Should().Be(14);
            bm.BeatmapID.Should().Be(4615284);
            bm.BeatmapSetID.Should().Be(2182178);

            // hit objects
            bm.HitObjects.Should().HaveCount(389);

            var hitObject1 = bm.HitObjects[175];
            var slider1 = hitObject1.Should().BeOfType<HitObjectSlider>().Which;
            slider1.X.Should().Be(380);
            slider1.Y.Should().Be(108);
            slider1.Time.Should().Be(57137);
            slider1.CurveType.Should().Be(CurveType.Bezier);
            slider1.Points.Should().HaveCount(2);
            slider1.Points[0].X.Should().Be(362);
            slider1.Points[0].Y.Should().Be(151);
            slider1.Points[1].X.Should().Be(382);
            slider1.Points[1].Y.Should().Be(199);
        }

        [Fact]
        public void ManiaBeatmap()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Beatmaps.294291.osu");
            var bm = BeatmapFile.Read(stream);

            bm.FileFormatVersion.Should().Be(12);
            bm.BeatmapID.Should().Be(294291);
            bm.BeatmapSetID.Should().Be(113273);

            // hit objects
            bm.HitObjects.Should().HaveCount(1219);

            var hitObject1 = bm.HitObjects[10];
            var holdNote1 = hitObject1.Should().BeOfType<HitObjectHold>().Which;
            holdNote1.X.Should().Be(329);
            holdNote1.Y.Should().Be(192);
            holdNote1.Time.Should().Be(1574);
            holdNote1.EndTime.Should().Be(1890);
            holdNote1.SoundSampleData.Should().Be("0:0:0:0:");

            var hitObject2 = bm.HitObjects[121];
            var holdNote2 = hitObject2.Should().BeOfType<HitObjectHold>().Which;
            holdNote2.X.Should().Be(256);
            holdNote2.Y.Should().Be(192);
            holdNote2.Time.Should().Be(20363);
            holdNote2.EndTime.Should().Be(20679);
            holdNote2.SoundSampleData.Should().Be("0:0:0:70:normal-hitclap.wav");
        }

        [Fact]
        public void GarbageBetweenSections()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Beatmaps.3718961.osu");
            var bm = BeatmapFile.Read(stream);

            bm.FileFormatVersion.Should().Be(14);
            bm.BeatmapID.Should().Be(3718961);
            bm.BeatmapSetID.Should().Be(-1);
        }

        [Fact]
        public void InvalidMetadataLine()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Beatmaps.1391940.osu");
            var bm = BeatmapFile.Read(stream);

            bm.FileFormatVersion.Should().Be(14);
            bm.BeatmapID.Should().Be(1391940);
            bm.BeatmapSetID.Should().Be(654452);
        }
    }
}
