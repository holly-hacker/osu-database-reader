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
        public int OsuVersion;
        public int FolderCount;
        public bool AccountUnlocked;
        public DateTime AccountUnlockDate;
        public string AccountName;
        public List<BeatmapEntry> Beatmaps;
        public PlayerRank AccountRank;

        public static OsuDb Read(string path) {
            OsuDb db = new OsuDb();
            using (var r = new SerializationReader(File.OpenRead(path)))
                db.ReadFromStream(r);
            return db;
        }

        public void ReadFromStream(SerializationReader r)
        {
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
                
                // After version 20191107, the size of the beatmap entry is no longer present
                // https://github.com/ppy/osu-wiki/commit/7ce3b8988d9945fe5867029a65750b40d66a3820
                const int lengthOsuVersion = 20191107;
                
                if (OsuVersion < lengthOsuVersion)
                    entryLength = r.ReadInt32();

                Beatmaps.Add(BeatmapEntry.ReadFromReader(r, false, OsuVersion));

                if (OsuVersion < lengthOsuVersion && r.BaseStream.Position != currentIndex + entryLength + 4) {
                    Debug.Fail($"Length doesn't match, {r.BaseStream.Position} instead of expected {currentIndex + entryLength + 4}");
                }
            }
            
            AccountRank = (PlayerRank)r.ReadByte();
        }

        public void WriteToStream(SerializationWriter w)
        {
            throw new NotImplementedException();
        }
    }
}
