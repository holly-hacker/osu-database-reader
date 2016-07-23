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
            throw new NotImplementedException();
        }
    }
}
