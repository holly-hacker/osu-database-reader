using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace osu_database_reader.IO
{
    public class CustomStreamReader : StreamReader
    {
        //automatically generated constructors
        public CustomStreamReader(Stream stream) : base(stream) { }
        public CustomStreamReader(Stream stream, bool detectEncodingFromByteOrderMarks) : base(stream, detectEncodingFromByteOrderMarks) { }
        public CustomStreamReader(Stream stream, Encoding encoding) : base(stream, encoding) { }
        public CustomStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks) : base(stream, encoding, detectEncodingFromByteOrderMarks) { }
        public CustomStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize) : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize) { }
        public CustomStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen) : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen) { }
        public CustomStreamReader(string path) : base(path) { }
        public CustomStreamReader(string path, bool detectEncodingFromByteOrderMarks) : base(path, detectEncodingFromByteOrderMarks) { }
        public CustomStreamReader(string path, Encoding encoding) : base(path, encoding) { }
        public CustomStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks) : base(path, encoding, detectEncodingFromByteOrderMarks) { }
        public CustomStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize) : base(path, encoding, detectEncodingFromByteOrderMarks, bufferSize) { }

        public BeatmapSection ReadUntilSectionStart()
        {
            while (!EndOfStream) {
                string str = ReadLine();
                if (string.IsNullOrWhiteSpace(str)) continue;

                string stripped = str.TrimStart('[').TrimEnd(']');
                if (Enum.TryParse<BeatmapSection>(stripped, out var a)) {
                    return a;
                }
                else {  //oh shit
                    throw new Exception("Unrecognized beatmap section: " + stripped);
                }
            }

            //we reached an end of stream
            return BeatmapSection._EndOfFile;
        }

        public Dictionary<string, string> ReadBasicSection(bool extraSpaceAfterColon = true, bool extraSpaceBeforeColon = false)
        {
            var dic = new Dictionary<string, string>();

            string line;
            while (!string.IsNullOrWhiteSpace(line = ReadLine())) {
                if (!line.Contains(':'))
                    throw new Exception("Invalid key/value line: " + line);

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

        public void SkipSection()
        {
            while (!string.IsNullOrWhiteSpace(ReadLine())) { }
        }
    }
}
