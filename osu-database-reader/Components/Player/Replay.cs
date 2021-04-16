using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using osu.Shared;
using osu.Shared.Serialization;

namespace osu_database_reader.Components.Player
{
    public class Replay : ISerializable //used for both scores.db and .osr files
    {
        public GameMode GameMode;
        public int OsuVersion;
        public string BeatmapHash, PlayerName, ReplayHash;
        public ushort Count300, Count100, Count50, CountGeki, CountKatu, CountMiss;
        public int Score;
        public ushort Combo;
        public bool FullCombo;
        public Mods Mods;
        public string LifeGraphData;    //null in scores.db, TODO: parse this when implementing .osr
        public DateTime TimePlayed;

        public byte[] ReplayData
        {
            get => _replayData;
            set {
                if (_replayData != value) {
                    _frames = null;
                    _replayData = value;
                }
            }
        }

        public ReplayFrame[] ReplayFrames
        {
            get {
                if (_replayData == null) return null;
                if (_frames != null)
                    return _frames;
                else {
                    byte[] decomp = LZMACoder.Decompress(_replayData);
                    string str = Encoding.ASCII.GetString(decomp); //ascii should be faster than UTF8, though not that it matters
                    return _frames = ReplayFrame.FromStrings(ref str);
                }
            }
            set {
                _frames = value;
                var clearFrames = string.Join(";", value.Select(x => x.ToString()));
                var decomp = Encoding.ASCII.GetBytes(clearFrames);
                _replayData = LZMACoder.Compress(decomp);
            }
        }

        public long? ScoreId;

        private byte[] _replayData; //null in scores.db
        private ReplayFrame[] _frames;

        public static Replay Read(string path)
        {
            using var stream = File.OpenRead(path);
            return Read(stream);
        }

        public static Replay Read(Stream stream) {
            using var r = new SerializationReader(stream);
            return ReadFromReader(r);
        }

        public static Replay ReadFromReader(SerializationReader r) {
            var replay = new Replay();
            replay.ReadFromStream(r);
            return replay;
        }

        /// <summary>
        /// Set the replay hash for a given rank
        /// </summary>
        /// <param name="rank">A rank such as SS, S, A, etc</param>
        /// <returns></returns>
        public void SetReplayHash(Ranking rank)
        {
            byte[] bytes = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes($"{Combo}osu{PlayerName}{BeatmapHash}{Score}{rank}"));
            ReplayHash = string.Join(string.Empty, bytes.Select(x => x.ToString("X2"))).ToLower();
        }

        public void ReadFromStream(SerializationReader r)
        {
            GameMode = (GameMode) r.ReadByte();
            OsuVersion = r.ReadInt32();
            BeatmapHash = r.ReadString();
            PlayerName = r.ReadString();
            ReplayHash = r.ReadString();

            Count300 = r.ReadUInt16();
            Count100 = r.ReadUInt16();
            Count50 = r.ReadUInt16();
            CountGeki = r.ReadUInt16();
            CountKatu = r.ReadUInt16();
            CountMiss = r.ReadUInt16();

            Score = r.ReadInt32();
            Combo = r.ReadUInt16();
            FullCombo = r.ReadBoolean();
            Mods = (Mods) r.ReadInt32();
            LifeGraphData = r.ReadString();
            TimePlayed = r.ReadDateTime();
            _replayData = r.ReadBytes();

            ScoreId = OsuVersion switch
            {
                >= OsuVersions.ReplayScoreId64Bit => r.ReadInt64(),
                >= OsuVersions.FirstOsz2 => r.ReadInt32(),
                _ => null,
            };
        }

        public void WriteToStream(SerializationWriter w)
        {
            w.Write((byte)GameMode);
            w.Write(OsuVersion);
            w.Write(BeatmapHash);
            w.Write(PlayerName);
            w.Write(ReplayHash);

            w.Write(Count300);
            w.Write(Count100);
            w.Write(Count50);
            w.Write(CountGeki);
            w.Write(CountKatu);
            w.Write(CountMiss);

            w.Write(Score);
            w.Write(Combo);
            w.Write(FullCombo);
            w.Write((int)Mods);
            w.Write(LifeGraphData);
            w.Write(TimePlayed);
            w.Write(_replayData);

            switch (OsuVersion)
            {
                case >= OsuVersions.ReplayScoreId64Bit:
                    w.Write(ScoreId ?? 0L);
                    break;
                case >= OsuVersions.FirstOsz2:
                    w.Write((int) (ScoreId ?? 0));
                    break;
            }
        }
    }
}
