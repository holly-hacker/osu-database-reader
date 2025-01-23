using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using osu_database_reader.Components.Beatmaps;
using osu_database_reader.Components.Events;
using osu_database_reader.Components.HitObjects;
using osu_database_reader.TextFiles;

namespace osu_database_reader
{
    internal static class Extensions
    {
        public static string GetValueOrNull(this Dictionary<string, string> dic, string key)
        {
            return dic.ContainsKey(key)
                ? dic[key]
                : null;
        }
        
        // i do this so that we can seek to previous lines and stuff because some beatmaps have no newlines between
        // each group and some of them have multiple and its all a mess oh my god
        public class FileReadObject : IDisposable
        {
            public List<string> Lines = new ();
            public int Index = 0;
            public StreamReader Sr;

            public FileReadObject(Stream stream)
            {
                Sr = new StreamReader(stream);
            }

            void IDisposable.Dispose()
            {
                
            }

            public string ReadLine()
            {
                Index++;
                
                if (Lines.Count > Index)
                {
                    return Lines[Index];
                }

                return GetNewLine();
            }

            private string GetNewLine()
            {
                string newLine = Sr.ReadLine();
                Lines.Add(newLine);
                // Console.WriteLine(newLine);
                return newLine;
            }

            public void AddNewLineAfterIndex(string line)
            {
                Lines.Insert(Index + 1, line);
            }
        }

        //for StreamReader
        public static BeatmapSection? ReadUntilSectionStart(this FileReadObject sr)
        {
            while (!sr.Sr.EndOfStream)
            {
                string str = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(str)) continue;
                // beatmapID: 3718961 has a -- at start of file as a comment
                if (str.StartsWith("--")) continue;

                // beatmapID: 1255026 has the line [TimingPoints]0,500,4,1,0,100,1,0
                string afterClosedBracket = str.Trim().Split(']').Last().Trim();
                string stripped;
                if (afterClosedBracket != "")
                {
                    sr.AddNewLineAfterIndex(afterClosedBracket);
                    stripped = str.TrimStart('[').Split(']').First().Trim();
                }
                else
                {
                    stripped = str.TrimStart('[').TrimEnd(']');
                }
                
                if (!Enum.TryParse(stripped, out BeatmapSection a))
                    throw new Exception("Unrecognized beatmap section: " + stripped);
                return a;
            }

            //we reached an end of stream
            return null;
        }

        public static Dictionary<string, string> ReadBasicSection(this FileReadObject sr, bool extraSpaceAfterColon = true, bool extraSpaceBeforeColon = false)
        {
            var dic = new Dictionary<string, string>();

            string line;
            while ((line = sr.GetNextLineData()) != null)
            {
                // Console.WriteLine(line);
                
                // BeatmapID: 1391940 has a weird thing where a line is just a repeated tag? i dont know but 
                //                    this rule isnt true for all valid beatmap files
                if (!line.Contains(':'))
                    continue;
                    // throw new Exception("Invalid key/value line: " + line);

                int i = line.IndexOf(':');

                string key = line.Substring(0, i);
                string value = line.Substring(i + 1);

                //This is just so we can recreate files properly in the future.
                //It is very likely not needed at all, but it makes me sleep
                //better at night knowing everything is 100% correct.
                if (extraSpaceBeforeColon && key.EndsWith(" ")) key = key.Substring(0, key.Length - 1);
                if (extraSpaceAfterColon && value.StartsWith(" ")) value = value.Substring(1);

                dic.Add(key, value);
            }

            return dic;
        }

        public static string? GetNextLineData(this FileReadObject sr)
        {
            int goBackwards = 0;
            while (sr.ReadLine() is { } line)
            {
                // beatmapID: 3718961 has a -- at start of file as a comment
                if (line.StartsWith("--")) continue;
                goBackwards++;
                
                if (sr.Sr.EndOfStream && line.Trim() == "")
                {
                    return null;
                }
                if (line.Trim() != "")
                {
                    if (line.Trim().StartsWith("["))
                    {
                        sr.Index -= goBackwards;
                        return null;
                    }
                    return line;
                }
            }

            return null;
        }

        public static IEnumerable<HitObject> ReadHitObjects(this FileReadObject sr)
        {
            string line;
            while ((line = sr.GetNextLineData()) != null)
                yield return HitObject.FromString(line);
        }

        public static IEnumerable<TimingPoint> ReadTimingPoints(this FileReadObject sr)
        {
            string line;
            while ((line = sr.GetNextLineData()) != null)
                yield return TimingPoint.FromString(line);
        }

        public static IEnumerable<EventBase> ReadEvents(this FileReadObject sr)
        {
            string line;
            while ((line = sr.GetNextLineData()) != null)
            {
                // idk which beatmap exactly but there are many that use // as comments
                if (!line.StartsWith("//")) yield return EventBase.FromString(line);
            }
        }
    }
}
