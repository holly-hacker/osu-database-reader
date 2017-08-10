using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using osu_database_reader.BinaryFiles.Parts;

namespace osu_database_reader.IO
{
    public class CustomBinaryReader : BinaryReader
    {
        public CustomBinaryReader(Stream input) : base(input) {}
        public CustomBinaryReader(Stream input, Encoding encoding) : base(input, encoding) {}
        public CustomBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) {}

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
                return null;
            else
                throw new Exception($"Type byte is not 0x00 or 0x11, but is 0x{b:X2} (position: {BaseStream.Position})");
        }

        public DateTime ReadDateTime() {
            long ticks = ReadInt64();
            return new DateTime(ticks, DateTimeKind.Utc);
        }

        public Dictionary<Mods, double> ReadModsDoubleDictionary() {
            int length = ReadInt32();
            Dictionary<Mods, double> dicks = new Dictionary<Mods, double>();
            for (int i = 0; i < length; i++) {
                ReadByte(); //type (0x08, Int32)
                int key = ReadInt32();
                ReadByte(); //type (0x0D, Double)
                double value = ReadDouble();
                dicks.Add((Mods)key, value);
            }
            return dicks;
        }

        public List<TimingPoint> ReadTimingPointList() {
            List<TimingPoint> list = new List<TimingPoint>();
            int length = ReadInt32();
            for (int i = 0; i < length; i++) list.Add(TimingPoint.ReadFromReader(this));
            return list;
        }

        
    }
}
