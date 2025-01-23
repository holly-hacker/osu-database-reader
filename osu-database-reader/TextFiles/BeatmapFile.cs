using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
            
            using var r = new Extensions.FileReadObject(stream);
            string nextLine = r.GetNextLineData();
            // beatmapID: 294291 has no osu file format header
            if (nextLine == null)
            {
                // just going to assume that its v14 i guess idk
                file.FileFormatVersion = 14;
            }
            else
            {
                // beatmapid: 548058 has a weird unicode character at the start
                nextLine = new Regex("[^a-zA-Z0-9 ]").Replace(nextLine, "");
                nextLine = nextLine.Trim();
                if (!int.TryParse(nextLine.Replace("osu file format v", string.Empty), out file.FileFormatVersion))
                {
                    // some beatmap files have typos in the "osu file format v<int>" line (somehow)
                
                    bool foundFormatVersion = false;
                    foreach (string word in nextLine.Split())
                    {
                        if (word.StartsWith("v"))
                        {
                            // Console.WriteLine($"{word}\n\n");
                            if (int.TryParse(word.Substring(1), out file.FileFormatVersion))
                            {
                                // Console.WriteLine($"{word.Split('v')[0]}\n\n");
                                foundFormatVersion = true;
                                break;
                            }
                        }
                    }

                    if (!foundFormatVersion) throw new Exception($"Not a valid beatmap, got header: {nextLine}");
                }
            }
            
            BeatmapSection? bs;
            while ((bs = r.ReadUntilSectionStart()) != null) {
                switch (bs.Value) {
                    case BeatmapSection.General:
                        // Console.WriteLine("general");
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
