using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using osu.Shared;
using osu.Shared.Serialization;
using osu_database_reader.Components.Beatmaps;

namespace osu_database_reader.BinaryFiles
{
    public class OsuDb : ISerializable
    {
        public int OsuVersion { get; set; }
        public int FolderCount { get; set; }
        public bool AccountUnlocked { get; set; }
        public DateTime AccountUnlockDate { get; set; }
        public string AccountName { get; set; }
        public List<BeatmapEntry> Beatmaps { get; set; }
        public PlayerRank AccountRank { get; set; }

        public static OsuDb Read(string path)
        {
            using var stream = File.OpenRead(path);
            return Read(stream);
        }

        public static OsuDb Read(Stream stream) {
            var db = new OsuDb();
            using var r = new SerializationReader(stream);
            db.ReadFromStream(r);
            return db;
        }

        public void ReadFromStream(SerializationReader r)
        {
            bool hasEntryLength = OsuVersion
                is >= OsuVersions.EntryLengthInOsuDbMin
                and < OsuVersions.EntryLengthInOsuDbMax;

            OsuVersion = r.ReadInt32();
            FolderCount = r.ReadInt32();
            AccountUnlocked = r.ReadBoolean();
            AccountUnlockDate = r.ReadDateTime();
            AccountName = r.ReadString();

            Beatmaps = new List<BeatmapEntry>();

            int length = r.ReadInt32();

            for (int i = 0; i < length; i++) {
                int currentIndex = (int)r.BaseStream.Position;
                int entryLength = 0;

                if (hasEntryLength)
                    entryLength = r.ReadInt32();

                Beatmaps.Add(BeatmapEntry.ReadFromReader(r, OsuVersion));

                if (OsuVersion < OsuVersions.EntryLengthInOsuDbMax && r.BaseStream.Position != currentIndex + entryLength + 4) {
                    Debug.Fail($"Length doesn't match, {r.BaseStream.Position} instead of expected {currentIndex + entryLength + 4}");
                }
            }

            AccountRank = (PlayerRank)r.ReadByte();
        }

        public void WriteToStream(SerializationWriter w)
        {
            bool hasEntryLength = OsuVersion
                is >= OsuVersions.EntryLengthInOsuDbMin
                and < OsuVersions.EntryLengthInOsuDbMax;

            w.Write(OsuVersion);
            w.Write(FolderCount);
            w.Write(AccountUnlocked);
            w.Write(AccountUnlockDate);
            w.Write(AccountName);

            w.Write(Beatmaps.Count);

            foreach (var beatmap in Beatmaps)
            {
                if (hasEntryLength)
                {
                    using var ms = new MemoryStream();
                    using var w2 = new SerializationWriter(ms);
                    beatmap.WriteToStream(w2);

                    var bytes = ms.ToArray();
                    w.Write(bytes.Length);
                    w.WriteRaw(bytes);
                }
                else
                {
                    beatmap.WriteToStream(w);
                }
            }

            w.Write((byte)AccountRank);
            // when trying to use the db in osu it says that the database is invalid unless these 3 bytes are here
            w.Write((byte)0);
            w.Write((byte)0);
            w.Write((byte)0);
        }
    }
}
