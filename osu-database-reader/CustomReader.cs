using System;
using System.Collections.Generic;
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
            if (b == 0x11)
                return base.ReadString();
            else if (b == 0)
                return string.Empty;
            else
                throw new Exception("Continuation byte is not 0x00 or 0x11");
        }

        public DateTime ReadDateTime() {
            long idk = ReadInt64();
            return new DateTime(idk, DateTimeKind.Utc);
        }

        public Dictionary<int, double> ReadIntDoubleDictionary() {
            int length = ReadInt32();
            Dictionary<int, double> dicks = new Dictionary<int, double>();
            for (int i = 0; i < length; i++) {
                int key = ReadInt32();
                int value = ReadInt32();
                dicks.Add(key, value);
            }
            return dicks;
        }

        public List<double> ReadDoubleList() {
            List<double> list = new List<double>();
            int length = ReadInt32();
            for (int i = 0; i < length; i++) list.Add(ReadDouble());
            return list;
        } 
    }
}
