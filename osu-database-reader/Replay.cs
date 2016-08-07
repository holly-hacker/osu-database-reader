using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_database_reader
{
    public class Replay //used for both scores.db and .osr files
    {
        public GameMode GameMode;
        public int OsuVersion;
        public string BeatmapHash, PlayerName, ReplayHash;  //may the chickenmcnuggets be with you
        public ushort Count300, Count100, Count50, CountGeki, CountKatu, CountMiss;
        public int Score;
        public ushort Combo;
        public bool FullCombo;
        public Mods Mods;
        public string LifeGraph;    //not present in scores.db, TODO: parse this when implementing .osr
        public DateTime TimePlayed;
        public byte[] ReplayData;   //not present in scores.db
        public long ScoreId;

        public static Replay Read(string path) {
            Replay replay;
            using (CustomReader r = new CustomReader(File.OpenRead(path))) {
                replay = ReadFromReader(r); //scoreid should not be needed
            }
            return replay;
        }

        public static Replay ReadFromReader(CustomReader r, bool readScoreId = false) {
            Replay replay = new Replay {
                GameMode = (GameMode) r.ReadByte(),
                OsuVersion = r.ReadInt32(),
                BeatmapHash = r.ReadString(),
                PlayerName = r.ReadString(),
                ReplayHash = r.ReadString(),

                Count300 = r.ReadUInt16(),
                Count100 = r.ReadUInt16(),
                Count50 = r.ReadUInt16(),
                CountGeki = r.ReadUInt16(),
                CountKatu = r.ReadUInt16(),
                CountMiss = r.ReadUInt16(),

                Score = r.ReadInt32(),
                Combo = r.ReadUInt16(),
                FullCombo = r.ReadBoolean(),
                Mods = (Mods) r.ReadInt32(),
                LifeGraph = r.ReadString(),
                TimePlayed = r.ReadDateTime(),
                ReplayData = r.ReadBytes(),
                ScoreId = readScoreId ? r.ReadInt64() : -1
            };

            return replay;
        }
    }
}
