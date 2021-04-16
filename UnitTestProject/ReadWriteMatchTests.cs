using System.IO;
using System.Reflection;
using FluentAssertions;
using osu.Shared.Serialization;
using osu_database_reader.BinaryFiles;
using osu_database_reader.Components.Player;
using Xunit;

namespace UnitTestProject
{
    public class ReadWriteMatchTests
    {
        [Fact]
        public void ReadWriteCollectionDb()
        {
            CollectionDb db;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Collection.20210316.db"))
                db = CollectionDb.Read(stream);

            using var ms = new MemoryStream();

            using var sw = new SerializationWriter(ms);
            db.WriteToStream(sw);

            ms.Position = 0;
            var read = CollectionDb.Read(ms);

            db.Should().BeEquivalentTo(read);
        }

        [Fact]
        public void ReadWriteOsuDb()
        {
            OsuDb db;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.osu_.20210316.db"))
                db = OsuDb.Read(stream);

            using var ms = new MemoryStream();

            using var sw = new SerializationWriter(ms);
            db.WriteToStream(sw);

            ms.Position = 0;
            var read = OsuDb.Read(ms);

            db.Should().BeEquivalentTo(read);
        }

        [Fact]
        public void ReadWritePresenceDb()
        {
            PresenceDb db;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Presence.20210316.db"))
                db = PresenceDb.Read(stream);

            using var ms = new MemoryStream();

            using var sw = new SerializationWriter(ms);
            db.WriteToStream(sw);

            ms.Position = 0;
            var read = PresenceDb.Read(ms);

            db.Should().BeEquivalentTo(read);
        }

        [Fact]
        public void ReadWriteScoresDb()
        {
            ScoresDb db;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Scores.20210316.db"))
                db = ScoresDb.Read(stream);

            using var ms = new MemoryStream();

            using var sw = new SerializationWriter(ms);
            db.WriteToStream(sw);

            ms.Position = 0;
            var read = ScoresDb.Read(ms);

            db.Should().BeEquivalentTo(read);
        }

        [Fact]
        public void ReadWriteReplay()
        {
            Replay db;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UnitTestProject.Data.Replays.20210316.osr"))
                db = Replay.Read(stream);

            using var ms = new MemoryStream();

            using var sw = new SerializationWriter(ms);
            db.WriteToStream(sw);

            ms.Position = 0;
            var read = Replay.Read(ms);

            db.Should().BeEquivalentTo(read);
        }
    }
}