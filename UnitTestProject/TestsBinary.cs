using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using osu.Shared;
using osu_database_reader.BinaryFiles;
using osu_database_reader.Components.Player;
using Xunit;

namespace UnitTestProject
{
    public class TestsBinary
    {
        [Fact]
        public void ReadOsuDb()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.osu_.20210316.db");
            var db = OsuDb.Read(stream);

            db.OsuVersion.Should().Be(20210316);
            db.FolderCount.Should().Be(23);
            db.AccountUnlocked.Should().BeTrue();
            db.AccountUnlockDate.Should().Be(new DateTime(0, DateTimeKind.Utc));
            db.AccountName.Should().Be("Ilex");

            db.Beatmaps.Should().HaveCount(93);
            var bm = db.Beatmaps[31];
            bm.Artist.Should().Be("KIVA");
            bm.Title.Should().Be("The Whole Rest");
            bm.Creator.Should().Be("Ilex");
            bm.Version.Should().Be("Insane v1");
            bm.AudioFileName.Should().Be("audio.mp3");
            bm.BeatmapChecksum.Should().Be("f281f4cb1a1cf13f4456443a7725bff2");
            bm.BeatmapFileName.Should().Be("KIVA - The Whole Rest (Ilex) [Insane v1].osu");
            bm.RankedStatus.Should().Be(SubmissionStatus.NotSubmitted);
            bm.CountHitCircles.Should().Be(252);
            bm.CountSliders.Should().Be(83);
            bm.CountSpinners.Should().Be(0);
            bm.LastModifiedTime.Should().BeAfter(new DateTime(2021, 4, 10, 17, 41, 10, DateTimeKind.Utc))
                .And.BeBefore(new DateTime(2021, 4, 10, 17, 41, 11, DateTimeKind.Utc));

            bm.ApproachRate.Should().Be(9.4f);
            bm.CircleSize.Should().Be(4);
            bm.HPDrainRate.Should().Be(6);
            bm.OveralDifficulty.Should().Be(7.3f);
            bm.SliderVelocity.Should().Be(1.8);

            bm.DiffStarRatingStandard.Should().HaveCount(9);
            bm.DiffStarRatingStandard[Mods.None].Should().BeApproximately(5.20573182737814, 0.00000000000001);
            bm.DiffStarRatingStandard[Mods.DoubleTime].Should().BeApproximately(7.24497549018472, 0.00000000000001);
            bm.DiffStarRatingStandard[Mods.HalfTime].Should().BeApproximately(4.15562856331195, 0.00000000000001);
            bm.DiffStarRatingTaiko.Should().BeEmpty();
            bm.DiffStarRatingCtB.Should().BeEmpty();
            bm.DiffStarRatingMania.Should().BeEmpty();

            bm.DrainTimeSeconds.Should().Be(96);
            bm.TotalTime.Should().Be(111563);
            bm.AudioPreviewTime.Should().Be(64366);

            bm.TimingPoints.Should().HaveCount(7);
            bm.TimingPoints[0].MsPerQuarter.Should().Be(382.165605095541);
            bm.TimingPoints[0].Time.Should().Be(1500);
            bm.TimingPoints[0].TimingChange.Should().BeTrue();
            bm.TimingPoints[6].MsPerQuarter.Should().Be(-57.1428571428571);
            bm.TimingPoints[6].Time.Should().Be(82710);
            bm.TimingPoints[6].TimingChange.Should().BeFalse();

            bm.BeatmapId.Should().Be(2026717);
            bm.BeatmapSetId.Should().Be(968597);
            bm.ThreadId.Should().Be(0);

            bm.GradeStandard.Should().Be(Ranking.C);
            bm.GradeTaiko.Should().Be(Ranking.N);
            bm.GradeCtB.Should().Be(Ranking.N);
            bm.GradeMania.Should().Be(Ranking.N);

            bm.OffsetLocal.Should().Be(0);
            bm.StackLeniency.Should().Be(0.7f);
            bm.GameMode.Should().Be(GameMode.Standard);
            bm.SongSource.Should().Be("Cytus II");
            bm.SongTags.Should().Be("Cytus II 2 OST");
            bm.OffsetOnline.Should().Be(0);
            bm.TitleFont.Should().BeNullOrEmpty();
            bm.Unplayed.Should().BeTrue(); // not sure how this works
            bm.LastPlayed.Should().BeAfter(new DateTime(2021, 4, 10, 18, 13, 9, DateTimeKind.Utc))
                .And.BeBefore(new DateTime(2021, 4, 10, 18, 13, 10, DateTimeKind.Utc));

            bm.IsOsz2.Should().BeFalse();
            bm.FolderName.Should().Be("968597 KIVA - The Whole Rest");
            bm.LastCheckAgainstOsuRepo.Should().BeAfter(new DateTime(2021, 4, 10, 19, 41, 10, DateTimeKind.Utc))
                .And.BeBefore(new DateTime(2021, 4, 10, 19, 41, 11, DateTimeKind.Utc));

            bm.IgnoreBeatmapSounds.Should().BeFalse();
            bm.IgnoreBeatmapSkin.Should().BeFalse();
            bm.DisableStoryBoard.Should().BeFalse();
            bm.DisableVideo.Should().BeFalse();
            bm.VisualOverride.Should().BeFalse();

            bm.LastEditTime.Should().Be(0);
            bm.ManiaScrollSpeed.Should().Be(0);
        }

        [Fact]
        public void ReadOsuDb_20250108()
        {
            // test new osu!.db format introduced in v20250108.3

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.osu_.20250108.db");
            var db = OsuDb.Read(stream);

            db.Beatmaps.Should().HaveCount(71);

            var starRatings = db.Beatmaps.SelectMany(b => b.DiffStarRatingStandard).Select(b => b.Value).ToList();
            starRatings.Should().NotBeEmpty().And.NotContain(sr => sr <= 0);
        }

        [Fact]
        public void ReadCollectionDbEmpty()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Collection.20200811_empty.db");
            var db = CollectionDb.Read(stream);

            db.OsuVersion.Should().Be(20200811);
            db.Collections.Should().BeEmpty();
        }

        [Fact]
        public void ReadCollectionDb()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Collection.20210316.db");
            var db = CollectionDb.Read(stream);

            db.OsuVersion.Should().Be(20210316);
            db.Collections.Should().HaveCount(2);

            db.Collections[0].Name.Should().Be("Hard maps");
            db.Collections[0].BeatmapHashes.Should().HaveCount(1);
            db.Collections[0].BeatmapHashes[0].Should().Be("06b536749d5a59536983854be90504ee");

            db.Collections[1].Name.Should().Be("My Collection");
            db.Collections[1].BeatmapHashes.Should().HaveCount(2);
            db.Collections[1].BeatmapHashes[0].Should().Be("f281f4cb1a1cf13f4456443a7725bff2");
            db.Collections[1].BeatmapHashes[1].Should().Be("b0670c14ed8f9ac489941890ce9b212e");
        }

        [Fact]
        public void ReadScoresDb()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Scores.20210316.db");
            var db = ScoresDb.Read(stream);

            db.OsuVersion.Should().Be(20210316);
            db.Beatmaps.Should().HaveCount(8);

            db.Beatmaps.ContainsKey("f281f4cb1a1cf13f4456443a7725bff2").Should().BeTrue();
            db.Beatmaps.ContainsKey("a0f3d86d32caaf0f3ed7474365ef830d").Should().BeTrue();
            db.Beatmaps["f281f4cb1a1cf13f4456443a7725bff2"].Should().HaveCount(1);
            db.Beatmaps["a0f3d86d32caaf0f3ed7474365ef830d"].Should().HaveCount(2);

            var replay = db.Beatmaps["f281f4cb1a1cf13f4456443a7725bff2"][0];

            // Equivalent to replay header
            replay.GameMode.Should().Be(GameMode.Standard);
            replay.OsuVersion.Should().Be(20210316);
            replay.BeatmapHash.Should().Be("f281f4cb1a1cf13f4456443a7725bff2");
            replay.PlayerName.Should().Be("Ilex");
            replay.ReplayHash.Should().Be("cc94fbdcd78ad26ff14bf906bf62336c");

            replay.Count300.Should().Be(246);
            replay.Count100.Should().Be(66);
            replay.Count50.Should().Be(1);
            replay.CountGeki.Should().Be(49);
            replay.CountKatu.Should().Be(28);
            replay.CountMiss.Should().Be(22);
            replay.Combo.Should().Be(119);
            replay.Score.Should().Be(322376);
            replay.FullCombo.Should().BeFalse();
            replay.Mods.Should().Be(Mods.NoFail);
            replay.TimePlayed.Should().BeAfter(new DateTime(2021, 4, 10, 18, 15, 05, DateTimeKind.Utc))
                .And.BeBefore(new DateTime(2021, 4, 10, 18, 15, 06, DateTimeKind.Utc));
            replay.ScoreId.Should().Be(0);

            replay.ReplayData.Should().BeNull();
        }

        [Fact]
        public void ReadPresenceDbEmpty()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Presence.20210316_empty.db");
            var db = PresenceDb.Read(stream);

            db.OsuVersion.Should().Be(20210316);
            db.Players.Should().BeEmpty();
        }

        [Fact]
        public void ReadPresenceDb()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Presence.20210316.db");
            var db = PresenceDb.Read(stream);

            db.OsuVersion.Should().Be(20210316);
            db.Players.Should().HaveCount(7);

            db.Players[1].PlayerId.Should().Be(-3);
            db.Players[1].PlayerName.Should().Be("BanchoBot");

            db.Players[2].PlayerId.Should().Be(-2070907);
            db.Players[2].PlayerName.Should().Be("Tillerino");
            db.Players[2].UtcOffset.Should().Be(24);
            db.Players[2].CountryByte.Should().Be(56);
            db.Players[2].PlayerRank.Should().Be(PlayerRank.Default);
            db.Players[2].GameMode.Should().Be(GameMode.Standard);
            db.Players[2].Longitude.Should().BeApproximately(11.5850f, 0.0001f);
            db.Players[2].Latitude.Should().BeApproximately(48.1497f, 0.0001f);
            db.Players[2].GlobalRank.Should().Be(0);
            db.Players[2].LastUpdate.Should().BeAfter(new DateTime(2021, 04, 10, 20, 17, 17, DateTimeKind.Utc))
                .And.BeBefore(new DateTime(2021, 04, 10, 20, 17, 18, DateTimeKind.Utc));

            db.Players[5].GlobalRank.Should().Be(3873896);
        }

        [Fact]
        public void ReadReplay()
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Replays.20210316.osr");
            var replay = Replay.Read(stream);

            replay.GameMode.Should().Be(GameMode.Standard);
            replay.OsuVersion.Should().Be(20210316);
            replay.BeatmapHash.Should().Be("f281f4cb1a1cf13f4456443a7725bff2");
            replay.PlayerName.Should().Be("Ilex");
            replay.ReplayHash.Should().Be("cc94fbdcd78ad26ff14bf906bf62336c");

            replay.Count300.Should().Be(246);
            replay.Count100.Should().Be(66);
            replay.Count50.Should().Be(1);
            replay.CountGeki.Should().Be(49);
            replay.CountKatu.Should().Be(28);
            replay.CountMiss.Should().Be(22);
            replay.Combo.Should().Be(119);
            replay.Score.Should().Be(322376);
            replay.FullCombo.Should().BeFalse();
            replay.Mods.Should().Be(Mods.NoFail);

            replay.LifeGraphData.Should().HaveLength(430);
            replay.TimePlayed.Should().BeAfter(new DateTime(2021, 4, 10, 18, 15, 05, DateTimeKind.Utc))
                .And.BeBefore(new DateTime(2021, 4, 10, 18, 15, 06, DateTimeKind.Utc));
            replay.ReplayData.Length.Should().Be(35350);
            replay.ScoreId.Should().Be(0);
        }
    }
}
