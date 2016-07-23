using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_database_reader
{
    public class OsuDb
    {
        public int OsuVersion;
        public int FolderCount;
        public bool AccountUnlocked;
        public DateTime AccountUnlockDate;
        public string PlayerName;
        public int AmountOfBeatmaps => Beatmaps.Count;
        public List<BeatmapEntry> Beatmaps;
        public int UserStatus;  //TODO: make enum, contains supporter and stuff (playerrank)

        public static OsuDb Read(string path) {
            OsuDb db = new OsuDb();
            using (CustomReader r = new CustomReader(File.OpenRead(path))) {
                db.OsuVersion = r.ReadInt32();
                db.FolderCount = r.ReadInt32();
                db.AccountUnlocked = r.ReadBoolean();
                r.ReadUInt64(); //TODO: read datetime
                db.PlayerName = r.ReadString();

                int length = r.ReadInt32();
                for (int i = 0; i < length; i++) {
                    int currentIndex = (int)r.BaseStream.Position;
                    int entryLength = r.ReadInt32();
                    try {
                        db.Beatmaps.Add(BeatmapEntry.ReadFromStream(r.BaseStream, false));
                    }
                    catch (Exception) {
                        //TODO: go to index+entryLength
                        throw;
                    }
                }
                db.UserStatus = r.ReadByte();   //TODO: cast as rank
            }
        }
    }
}
