using System;
using System.Collections.Generic;
using osu_database_reader.Components.Beatmaps;
using osu_database_reader.Components.HitObjects;
using osu_database_reader.IO;

namespace osu_database_reader.TextFiles
{
    public class BeatmapFile
    {
        public int BeatmapVersion;

        public Dictionary<string, string> SectionGeneral;
        public Dictionary<string, string> SectionEditor;
        public Dictionary<string, string> SectionMetadata;
        public Dictionary<string, string> SectionDifficulty;
        public Dictionary<string, string> SectionColours;

        public List<TimingPoint> TimingPoints = new List<TimingPoint>();
        public List<HitObject> HitObjects = new List<HitObject>();

        public static BeatmapFile Read(string path)
        {
            var file = new BeatmapFile();

            using (var r = new CustomStreamReader(path)) {
                if (!int.TryParse(r.ReadLine()?.Replace("osu file format v", string.Empty), out file.BeatmapVersion))
                    throw new Exception("Not a valid beatmap"); //very simple check, could be better

                BeatmapSection bs;
                while ((bs = r.ReadUntilSectionStart()) != BeatmapSection._EndOfFile) {
                    switch (bs) {
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
                            //TODO
                            r.SkipSection();
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
            }

            return file;
        }
    }
}
