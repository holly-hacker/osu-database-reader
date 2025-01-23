using System;
using System.Collections.Generic;
using System.Linq;
using osu.Shared;
using osu.Shared.Serialization;

namespace osu_database_reader.Components.Beatmaps
{
    /// <summary>
    /// An entry in <see cref="OsuDb"/> with information about a beatmap.
    /// </summary>
    public class BeatmapEntry : ISerializable
    {
        public string Artist { get; set; }
        public string ArtistUnicode { get; set; }
        public string Title { get; set; }
        public string TitleUnicode { get; set; }
        public string Creator { get; set; }
        public string Version { get; set; }
        public string AudioFileName { get; set; }
        public string BeatmapChecksum { get; set; }
        public string BeatmapFileName { get; set; }
        public SubmissionStatus RankedStatus { get; set; }
        public ushort CountHitCircles { get; set; }
        public ushort CountSliders { get; set; }
        public ushort CountSpinners { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public float ApproachRate { get; set; }
        public float CircleSize { get; set; }
        public float HPDrainRate { get; set; }
        public float OveralDifficulty { get; set; }
        public double SliderVelocity { get; set; }
        public Dictionary<Mods,double> DiffStarRatingStandard { get; set; }
        public Dictionary<Mods,double> DiffStarRatingTaiko { get; set; }
        public Dictionary<Mods,double> DiffStarRatingCtB { get; set; }
        public Dictionary<Mods,double> DiffStarRatingMania { get; set; }
        public int DrainTimeSeconds { get; set; } //NOTE: in s
        public int TotalTime { get; set; }        //NOTE: in ms
        public int AudioPreviewTime { get; set; } //NOTE: in ms
        public List<TimingPoint> TimingPoints { get; set; }
        public int BeatmapId { get; set; }
        public int BeatmapSetId { get; set; }
        public int ThreadId { get; set; }
        public Ranking GradeStandard { get; set; }
        public Ranking GradeTaiko { get; set; }
        public Ranking GradeCtB { get; set; }
        public Ranking GradeMania { get; set; }
        public short OffsetLocal { get; set; }
        public float StackLeniency { get; set; }
        public GameMode GameMode { get; set; }
        public string SongSource { get; set; }
        public string SongTags { get; set; }
        public short OffsetOnline { get; set; }
        public string TitleFont { get; set; }
        public bool Unplayed { get; set; }
        public DateTime LastPlayed { get; set; }
        public bool IsOsz2 { get; set; }
        public string FolderName { get; set; }
        public DateTime LastCheckAgainstOsuRepo { get; set; }
        public bool IgnoreBeatmapSounds { get; set; }
        public bool IgnoreBeatmapSkin { get; set; }
        public bool DisableStoryBoard { get; set; }
        public bool DisableVideo { get; set; }
        public bool VisualOverride { get; set; }
        public short OldUnknown1 { get; set; }
        public int LastEditTime { get; set; }
        public byte ManiaScrollSpeed { get; set; }
        public int _version { get; set; }

        public static BeatmapEntry ReadFromReader(SerializationReader r, int version) {
            var e = new BeatmapEntry {
                _version = version,
            };

            e.ReadFromStream(r);

            return e;
        }

        public void ReadFromStream(SerializationReader r)
        {
            Artist = r.ReadString();
            if (_version >= OsuVersions.FirstOsz2)
                ArtistUnicode = r.ReadString();
            Title = r.ReadString();
            if (_version >= OsuVersions.FirstOsz2)
                TitleUnicode = r.ReadString();
            Creator = r.ReadString();
            Version = r.ReadString();
            AudioFileName = r.ReadString();
            BeatmapChecksum = r.ReadString();   //always 32 in length, so the 2 preceding bytes in the file are practically wasting space
            BeatmapFileName = r.ReadString();

            RankedStatus = (SubmissionStatus)r.ReadByte();
            CountHitCircles = r.ReadUInt16();
            CountSliders = r.ReadUInt16();
            CountSpinners = r.ReadUInt16();
            LastModifiedTime = r.ReadDateTime();

            if (_version >= OsuVersions.FloatDifficultyValues)
            {
                ApproachRate = r.ReadSingle();
                CircleSize = r.ReadSingle();
                HPDrainRate = r.ReadSingle();
                OveralDifficulty = r.ReadSingle();
            }
            else
            {
                ApproachRate = r.ReadByte();
                CircleSize = r.ReadByte();
                HPDrainRate = r.ReadByte();
                OveralDifficulty = r.ReadByte();
            }

            SliderVelocity = r.ReadDouble();

            if (_version >= OsuVersions.ReducedOsuDbSize)
            {
                DiffStarRatingStandard = r.ReadDictionary<Mods, float>().ToDictionary(kvp => kvp.Key, kvp => (double)kvp.Value);
                DiffStarRatingTaiko = r.ReadDictionary<Mods, float>().ToDictionary(kvp => kvp.Key, kvp => (double)kvp.Value);
                DiffStarRatingCtB = r.ReadDictionary<Mods, float>().ToDictionary(kvp => kvp.Key, kvp => (double)kvp.Value);
                DiffStarRatingMania = r.ReadDictionary<Mods, float>().ToDictionary(kvp => kvp.Key, kvp => (double)kvp.Value);
            }
            else if (_version >= OsuVersions.FloatDifficultyValues)
            {
                DiffStarRatingStandard = r.ReadDictionary<Mods, double>();
                DiffStarRatingTaiko = r.ReadDictionary<Mods, double>();
                DiffStarRatingCtB = r.ReadDictionary<Mods, double>();
                DiffStarRatingMania = r.ReadDictionary<Mods, double>();

                // TODO: there may be different reading behavior for versions before 20190204, 20200916, 20200504 and 20191024 here.
            }

            DrainTimeSeconds = r.ReadInt32();
            TotalTime = r.ReadInt32();
            AudioPreviewTime = r.ReadInt32();

            TimingPoints = r.ReadSerializableList<TimingPoint>();
            BeatmapId = r.ReadInt32();
            BeatmapSetId = r.ReadInt32();
            ThreadId = r.ReadInt32();

            GradeStandard = (Ranking)r.ReadByte();
            GradeTaiko = (Ranking)r.ReadByte();
            GradeCtB = (Ranking)r.ReadByte();
            GradeMania = (Ranking)r.ReadByte();

            OffsetLocal = r.ReadInt16();
            StackLeniency = r.ReadSingle();
            GameMode = (GameMode)r.ReadByte();

            SongSource = r.ReadString();
            SongTags = r.ReadString();
            OffsetOnline = r.ReadInt16();
            TitleFont = r.ReadString();
            Unplayed = r.ReadBoolean();
            LastPlayed = r.ReadDateTime();

            IsOsz2 = r.ReadBoolean();
            FolderName = r.ReadString();
            LastCheckAgainstOsuRepo = r.ReadDateTime();

            IgnoreBeatmapSounds = r.ReadBoolean();
            IgnoreBeatmapSkin = r.ReadBoolean();
            DisableStoryBoard = r.ReadBoolean();
            DisableVideo = r.ReadBoolean();
            VisualOverride = r.ReadBoolean();
            if (_version < OsuVersions.FloatDifficultyValues)
                OldUnknown1 = r.ReadInt16();
            LastEditTime = r.ReadInt32();
            ManiaScrollSpeed = r.ReadByte();
        }

        public void WriteToStream(SerializationWriter w)
        {
            w.Write(Artist);
            if (_version >= OsuVersions.FirstOsz2)
                w.Write(ArtistUnicode);
            w.Write(Title);
            if (_version >= OsuVersions.FirstOsz2)
                w.Write(TitleUnicode);
            w.Write(Creator);
            w.Write(Version);
            w.Write(AudioFileName);
            w.Write(BeatmapChecksum);
            w.Write(BeatmapFileName);

            w.Write((byte)RankedStatus);
            w.Write(CountHitCircles);
            w.Write(CountSliders);
            w.Write(CountSpinners);
            w.Write(LastModifiedTime);

            if (_version >= OsuVersions.FloatDifficultyValues)
            {
                w.Write(ApproachRate);
                w.Write(CircleSize);
                w.Write(HPDrainRate);
                w.Write(OveralDifficulty);
            }
            else
            {
                w.Write(ApproachRate);
                w.Write(CircleSize);
                w.Write(HPDrainRate);
                w.Write(OveralDifficulty);
            }

            w.Write(SliderVelocity);

            if (_version >= OsuVersions.ReducedOsuDbSize)
            {
                static Dictionary<int, float> ConvertToWritableDictionary(IDictionary<Mods, double> dic)
                    => dic.ToDictionary(pair => (int) pair.Key, pair => (float)pair.Value);

                w.Write(ConvertToWritableDictionary(DiffStarRatingStandard));
                w.Write(ConvertToWritableDictionary(DiffStarRatingTaiko));
                w.Write(ConvertToWritableDictionary(DiffStarRatingCtB));
                w.Write(ConvertToWritableDictionary(DiffStarRatingMania));
            }
            else if (_version >= OsuVersions.FloatDifficultyValues)
            {
                static Dictionary<int, double> ConvertToWritableDictionary(IDictionary<Mods, double> dic)
                    => dic.ToDictionary(pair => (int) pair.Key, pair => pair.Value);

                w.Write(ConvertToWritableDictionary(DiffStarRatingStandard));
                w.Write(ConvertToWritableDictionary(DiffStarRatingTaiko));
                w.Write(ConvertToWritableDictionary(DiffStarRatingCtB));
                w.Write(ConvertToWritableDictionary(DiffStarRatingMania));

                // TODO: there may be different reading behavior for versions before 20190204, 20200916, 20200504 and 20191024 here.
            }

            w.Write(DrainTimeSeconds);
            w.Write(TotalTime);
            w.Write(AudioPreviewTime);

            w.WriteSerializableList(TimingPoints);
            w.Write(BeatmapId);
            w.Write(BeatmapSetId);
            w.Write(ThreadId);

            w.Write((byte)GradeStandard);
            w.Write((byte)GradeTaiko);
            w.Write((byte)GradeCtB);
            w.Write((byte)GradeMania);

            w.Write(OffsetLocal);
            w.Write(StackLeniency);
            w.Write((byte)GameMode);

            w.Write(SongSource);
            w.Write(SongTags);
            w.Write(OffsetOnline);
            w.Write(TitleFont);
            w.Write(Unplayed);
            w.Write(LastPlayed);

            w.Write(IsOsz2);
            w.Write(FolderName);
            w.Write(LastCheckAgainstOsuRepo);

            w.Write(IgnoreBeatmapSounds);
            w.Write(IgnoreBeatmapSkin);
            w.Write(DisableStoryBoard);
            w.Write(DisableVideo);
            w.Write(VisualOverride);
            if (_version < OsuVersions.FloatDifficultyValues)
                w.Write(OldUnknown1);
            w.Write(LastEditTime);
            w.Write(ManiaScrollSpeed);
        }
    }
}