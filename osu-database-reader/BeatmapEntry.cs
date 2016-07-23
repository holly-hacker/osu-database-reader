using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public byte RankedStatus;   //TODO: make enum
        public short CountHitCircles, CountSliders, CountSpinners;
        public DateTime LastModifiedTime;
        public float DiffAR, DiffCS, DiffHP, DiffOD;
        public double SliderVelocity;
        public List<Tuple<int, double>> DiffStarRatingStandard, DiffStarRatingTaiko, DiffStarRatingCtB, DiffStarRatingMania;    //TODO: Tuple<Mods, double>
        public int DrainTime;   //NOTE: in s
        public int TotalTime;   //NOTE: in ms
        public int AudioPreviewTime;    //NOTE: in ms
        public List<double> TimingPoints;   //TODO: check if double
        public int BeatmapId, BeatmapSetId, ThreadId;
        public byte GradeStandard, GradeTaiko, GradeCtB, GradeMania;    //TODO: make enum
        public short LocalOffset;
        public float StackLeniency;
        public byte GameMode;   //TODO: make enum
        public string SongSource;
        public short OnlineOffset;  //wtf
        public bool Unplayed;
        public DateTime LastPlayed;
        public bool IsOsz2;
        public string FolderName;
        public DateTime LastCheckAgainstOsuRepo;    //wtf
        public bool IgnoreBeatmapSounds, IgnoreBeatmapSkin, DisableStoryboard, DisableVideo, VisualOverride;
        public short Unknown1;
        public byte ManiaScrollSpeed;

        public static BeatmapEntry ReadFromStream(Stream stream, bool readLength = true) {
            using (CustomReader r = new CustomReader(stream)) {
                if (readLength) r.ReadInt32();
                throw new NotImplementedException();
            }
        }
    }
}
