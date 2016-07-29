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
        public byte RankedStatus;   //TODO: make enum, automatically convert (subtract 3?)
        public ushort CountHitCircles, CountSliders, CountSpinners;
        public DateTime LastModifiedTime;
        public float DiffAR, DiffCS, DiffHP, DiffOD;
        public double SliderVelocity;
        public Dictionary<int, double> DiffStarRatingStandard, DiffStarRatingTaiko, DiffStarRatingCtB, DiffStarRatingMania;    //TODO: Dictionary<Mods, double>
        public int DrainTime;   //NOTE: in s
        public int TotalTime;   //NOTE: in ms
        public int AudioPreviewTime;    //NOTE: in ms
        public List<double> TimingPoints;   //TODO: check if double
        public int BeatmapId, BeatmapSetId, ThreadId;
        public byte GradeStandard, GradeTaiko, GradeCtB, GradeMania;    //TODO: make enum
        public short LocalOffset;
        public float StackLeniency;
        public byte GameMode;   //TODO: make enum
        public string SongSource, SongTags;
        public short OnlineOffset;  //wtf
        public string TitleFont;
        public bool Unplayed;
        public DateTime LastPlayed;
        public bool IsOsz2;
        public string FolderName;
        public DateTime LastCheckAgainstOsuRepo;    //wtf
        public bool IgnoreBeatmapSounds, IgnoreBeatmapSkin, DisableStoryBoard, DisableVideo, VisualOverride;
        public short Unknown1;
        public int Unknown2;
        public byte ManiaScrollSpeed;

        public static BeatmapEntry ReadFromStream(Stream stream, bool readLength = true, int version = 20160729) {
            BeatmapEntry e = new BeatmapEntry();
            using (CustomReader r = new CustomReader(stream)) {
                int length = 0;
                if (readLength) length = r.ReadInt32();
                int startPosition = (int)stream.Position;

                e.Artist = r.ReadString();
                e.ArtistUnicode = r.ReadString();
                e.Title = r.ReadString();
                e.TitleUnicode = r.ReadString();
                e.Creator = r.ReadString();
                e.Difficulty = r.ReadString();
                e.AudioFileName = r.ReadString();
                e.BeatmapChecksum = r.ReadString(); //always 32 in length, so the 2 preceding bytes in the file are practically wasting space
                e.BeatmapFileName = r.ReadString();

                e.RankedStatus = r.ReadByte();  //TODO: to enum
                e.CountHitCircles = r.ReadUInt16();
                e.CountSliders = r.ReadUInt16();
                e.CountSpinners = r.ReadUInt16();
                e.LastModifiedTime = r.ReadDateTime();

                if (version >= 20140609) {
                    e.DiffAR = r.ReadSingle();
                    e.DiffCS = r.ReadSingle();
                    e.DiffHP = r.ReadSingle();
                    e.DiffOD = r.ReadSingle();
                } else {
                    e.DiffAR = r.ReadByte();
                    e.DiffCS = r.ReadByte();
                    e.DiffHP = r.ReadByte();
                    e.DiffOD = r.ReadByte();
                }

                e.SliderVelocity = r.ReadDouble();

                if (version >= 20140609) {
                    e.DiffStarRatingStandard = r.ReadIntDoubleDictionary();
                    e.DiffStarRatingTaiko = r.ReadIntDoubleDictionary();
                    e.DiffStarRatingCtB = r.ReadIntDoubleDictionary();
                    e.DiffStarRatingMania = r.ReadIntDoubleDictionary();
                }

                e.DrainTime = r.ReadInt32();
                e.TotalTime = r.ReadInt32();
                e.AudioPreviewTime = r.ReadInt32();

                e.TimingPoints = r.ReadDoubleList();
                e.BeatmapId = r.ReadInt32();
                e.BeatmapSetId = r.ReadInt32();
                e.ThreadId = r.ReadInt32();     //no idea what this is

                e.GradeStandard = r.ReadByte(); //TODO: cast these as mods
                e.GradeTaiko = r.ReadByte();
                e.GradeCtB = r.ReadByte();
                e.GradeMania = r.ReadByte();

                e.LocalOffset = r.ReadInt16();
                e.StackLeniency = r.ReadSingle();
                e.GameMode = r.ReadByte();  //TODO: cast as mode
                e.SongSource = r.ReadString();
                e.SongTags = r.ReadString();
                e.OnlineOffset = r.ReadInt16();
                e.TitleFont = r.ReadString();
                e.Unplayed = r.ReadBoolean();
                e.LastPlayed = r.ReadDateTime();
                e.IgnoreBeatmapSounds = r.ReadBoolean();
                e.IgnoreBeatmapSkin = r.ReadBoolean();
                e.DisableStoryBoard = r.ReadBoolean();
                e.DisableVideo = r.ReadBoolean();
                e.VisualOverride = r.ReadBoolean();
                if (version >= 20140609)
                    e.Unknown1 = r.ReadInt16();
                e.Unknown2 = r.ReadInt32();
                e.ManiaScrollSpeed = r.ReadByte();

                int endPosition = (int)stream.Position;

                Debug.Assert(!readLength || length == endPosition-startPosition);   //TODO: could throw error here
            }

            return e;
        }
    }
}
