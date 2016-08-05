using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_database_reader
{
    public class CustomReader : BinaryReader
    {
        public CustomReader(Stream input) : base(input) {}
        public CustomReader(Stream input, Encoding encoding) : base(input, encoding) {}
        public CustomReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) {}

        public byte[] ReadBytes() {     // an overload to ReadBytes(int count)
            int length = ReadInt32();
            return length > 0 
                ? base.ReadBytes(length) 
                : null;
        }

        public override string ReadString() {
            byte b = ReadByte();
            if (b == 0x0B)
                return base.ReadString();
            else if (b == 0x00)
                return string.Empty;
            else
                throw new Exception($"Continuation byte is not 0x00 or 0x11, but is 0x{b.ToString("X2")} (position: {BaseStream.Position})");
        }

        public DateTime ReadDateTime() {
            long idk = ReadInt64();
            return new DateTime(idk, DateTimeKind.Utc);
        }

        public Dictionary<Mods, double> ReadModsDoubleDictionary() {
            int length = ReadInt32();
            Dictionary<Mods, double> dicks = new Dictionary<Mods, double>();
            for (int i = 0; i < length; i++) {
                ReadByte(); //type (0x08)
                int key = ReadInt32();
                ReadByte(); //type (0x0D)
                double value = ReadDouble();
                dicks.Add((Mods)key, value);
            }
            return dicks;
        }

        public List<TimingPoint> ReadTimingPointList() {
            List<TimingPoint> list = new List<TimingPoint>();
            int length = ReadInt32();
            for (int i = 0; i < length; i++) list.Add(ReadTimingPoint());
            return list;
        }

        private TimingPoint ReadTimingPoint() {
            TimingPoint t = new TimingPoint {
                Time = ReadDouble(),
                Speed = ReadDouble(),
                NotInherited = ReadByte()
            };
            return t;
        }
    }
}
