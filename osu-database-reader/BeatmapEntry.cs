using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace osu_database_reader
{
    public class BeatmapEntry
    {
        public string Artist, ArtistUnicode;
        public string Title, TitleUnicode;
        public string Creator;  //mapper
        public string Difficulty;   //called "Version" in the .osu format
        public string AudioFileName;
        public string BeatmapChecksum;
        public string BeatmapFileName;
        public SubmissionStatus RankedStatus;
        public ushort CountHitCircles, CountSliders, CountSpinners;
        public DateTime LastModifiedTime;
        public float DiffAR, DiffCS, DiffHP, DiffOD;
        public double SliderVelocity;
        public Dictionary<Mods, double> DiffStarRatingStandard, DiffStarRatingTaiko, DiffStarRatingCtB, DiffStarRatingMania;
        public int DrainTimeSeconds;   //NOTE: in s
        public int TotalTime;   //NOTE: in ms
        public int AudioPreviewTime;    //NOTE: in ms
        public List<TimingPoint> TimingPoints;
        public int BeatmapId, BeatmapSetId, ThreadId;
        public Ranking GradeStandard, GradeTaiko, GradeCtB, GradeMania;
        public short OffsetLocal;
        public float StackLeniency;
        public GameMode GameMode;
        public string SongSource, SongTags;
        public short OffsetOnline;
        public string TitleFont;
        public bool Unplayed;
        public DateTime LastPlayed;
        public bool IsOsz2;
        public string FolderName;
        public DateTime LastCheckAgainstOsuRepo;    //wtf
        public bool IgnoreBeatmapSounds, IgnoreBeatmapSkin, DisableStoryBoard, DisableVideo, VisualOverride;
        public short OldUnknown1;   //unused
        public int Unknown2;
        public byte ManiaScrollSpeed;

        public static BeatmapEntry ReadFromReader(CustomReader r, bool readLength = true, int version = 20160729) {
            BeatmapEntry e = new BeatmapEntry();

            int length = 0;
            if (readLength) length = r.ReadInt32();
            int startPosition = (int) r.BaseStream.Position;

            e.Artist = r.ReadString();
            e.ArtistUnicode = r.ReadString();
            e.Title = r.ReadString();
            e.TitleUnicode = r.ReadString();
            e.Creator = r.ReadString();
            e.Difficulty = r.ReadString();
            e.AudioFileName = r.ReadString();
            e.BeatmapChecksum = r.ReadString(); //always 32 in length, so the 2 preceding bytes in the file are practically wasting space
            e.BeatmapFileName = r.ReadString();

            //Debug.WriteLine($"{e.Artist} - {e.Title} [{e.Difficulty}]");

            e.RankedStatus = (SubmissionStatus)r.ReadByte();
            e.CountHitCircles = r.ReadUInt16();
            e.CountSliders = r.ReadUInt16();
            e.CountSpinners = r.ReadUInt16();
            e.LastModifiedTime = r.ReadDateTime();

            //Debug.WriteLine("Last modified: " + e.LastModifiedTime + ", ranked status is " + e.RankedStatus);

            if (version >= 20140609) {
                e.DiffAR = r.ReadSingle();
                e.DiffCS = r.ReadSingle();
                e.DiffHP = r.ReadSingle();
                e.DiffOD = r.ReadSingle();
            }
            else {
                e.DiffAR = r.ReadByte();
                e.DiffCS = r.ReadByte();
                e.DiffHP = r.ReadByte();
                e.DiffOD = r.ReadByte();
            }

            //Debug.WriteLine($"AR: {e.DiffAR} CS: {e.DiffCS} HP: {e.DiffHP} OD: {e.DiffOD}");

            e.SliderVelocity = r.ReadDouble();

            if (version >= 20140609) {
                e.DiffStarRatingStandard = r.ReadModsDoubleDictionary();
                e.DiffStarRatingTaiko = r.ReadModsDoubleDictionary();
                e.DiffStarRatingCtB = r.ReadModsDoubleDictionary();
                e.DiffStarRatingMania = r.ReadModsDoubleDictionary();
            }

            e.DrainTimeSeconds = r.ReadInt32();
            e.TotalTime = r.ReadInt32();
            e.AudioPreviewTime = r.ReadInt32();

            e.TimingPoints = r.ReadTimingPointList();
            e.BeatmapId = r.ReadInt32();
            e.BeatmapSetId = r.ReadInt32();
            e.ThreadId = r.ReadInt32(); //no idea what this is

            e.GradeStandard = (Ranking)r.ReadByte();
            e.GradeTaiko = (Ranking)r.ReadByte();
            e.GradeCtB = (Ranking)r.ReadByte();
            e.GradeMania = (Ranking)r.ReadByte();

            e.OffsetLocal = r.ReadInt16();
            e.StackLeniency = r.ReadSingle();
            e.GameMode = (GameMode)r.ReadByte();

            //Debug.WriteLine("gamemode: " + e.GameMode);

            e.SongSource = r.ReadString();
            e.SongTags = r.ReadString();
            e.OffsetOnline = r.ReadInt16();
            e.TitleFont = r.ReadString();
            e.Unplayed = r.ReadBoolean();
            e.LastPlayed = r.ReadDateTime();

            //Debug.WriteLine("Last played: " + e.LastPlayed);

            e.IsOsz2 = r.ReadBoolean();
            e.FolderName = r.ReadString();
            e.LastCheckAgainstOsuRepo = r.ReadDateTime();

            //Debug.WriteLine("Last osu! repo check: " + e.LastCheckAgainstOsuRepo);

            e.IgnoreBeatmapSounds = r.ReadBoolean();
            e.IgnoreBeatmapSkin = r.ReadBoolean();
            e.DisableStoryBoard = r.ReadBoolean();
            e.DisableVideo = r.ReadBoolean();
            e.VisualOverride = r.ReadBoolean();
            if (version < 20140609)
                e.OldUnknown1 = r.ReadInt16();
            e.Unknown2 = r.ReadInt32();
            e.ManiaScrollSpeed = r.ReadByte();

            //Debug.WriteLine("Mania scroll speed: " + e.ManiaScrollSpeed);

            int endPosition = (int) r.BaseStream.Position;
            Debug.Assert(!readLength || length == endPosition - startPosition); //TODO: could throw error here
            //Debug.WriteLine("---");

            return e;
        }
    }
}
