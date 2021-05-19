using System;
using System.Collections.Generic;
using System.IO;
using osu_database_reader.Components.Beatmaps;
using osu_database_reader.Components.Events;
using osu_database_reader.Components.HitObjects;

namespace osu_database_reader.TextFiles
{
    public partial class BeatmapFile
    {
        public int FileFormatVersion;

        public Dictionary<string, string> SectionGeneral;
        public Dictionary<string, string> SectionEditor;
        public Dictionary<string, string> SectionMetadata;
        public Dictionary<string, string> SectionDifficulty;
        public Dictionary<string, string> SectionColours;

        public readonly List<EventBase> Events = new();
        public readonly List<TimingPoint> TimingPoints = new();
        public readonly List<HitObject> HitObjects = new();

        public static BeatmapFile Read(string path)
        {
            using var fs = File.OpenRead(path);
            return Read(fs);
        }

        public static BeatmapFile Read(Stream stream)
        {
            var file = new BeatmapFile();

            using var r = new StreamReader(stream);
            if (!int.TryParse(r.ReadLine()?.Replace("osu file format v", string.Empty), out file.FileFormatVersion))
                throw new Exception("Not a valid beatmap"); //very simple check, could be better

            BeatmapSection? bs;
            while ((bs = r.ReadUntilSectionStart()) != null) {
                switch (bs.Value) {
                    case BeatmapSection.General:
                        file.SectionGeneral = r.ReadBasicSection();
                        break;
                    case BeatmapSection.Editor:
                        file.SectionEditor = r.ReadBasicSection();
                        break;
                    case BeatmapSection.Metadata:
                        file.SectionMetadata = r.ReadBasicSection(false);
                        break;
                    case BeatmapSection.Difficulty:
                        file.SectionDifficulty = r.ReadBasicSection(false);
                        break;
                    case BeatmapSection.Events:
                        file.Events.AddRange(r.ReadEvents());
                        break;
                    case BeatmapSection.TimingPoints:
                        file.TimingPoints.AddRange(r.ReadTimingPoints());
                        break;
                    case BeatmapSection.Colours:
                        file.SectionColours = r.ReadBasicSection(true, true);
                        break;
                    case BeatmapSection.HitObjects:
                        file.HitObjects.AddRange(r.ReadHitObjects());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return file;
        }
    }
}
