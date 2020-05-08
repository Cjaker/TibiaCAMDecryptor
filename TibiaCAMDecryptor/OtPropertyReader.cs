using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaCAMDecryptor {
    public class OtPropertyReader : BinaryReader {
        public OtPropertyReader(Stream stream)
            : base(stream) { }

        public string GetString() {
            var len = ReadUInt16();
            return Encoding.Default.GetString(ReadBytes(len));
        }

        public Location ReadLocation() {
            return new Location(ReadUInt16(), ReadUInt16(), ReadByte());
        }
    }
}
