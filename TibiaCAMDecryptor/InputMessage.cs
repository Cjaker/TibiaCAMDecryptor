using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaCAMDecryptor {
    public class InputMessage {
        private byte[] buffer;
        private int position;

        public InputMessage(byte[] buffer) {
            this.buffer = buffer; // initialize buffer
            position = 0;
        }

        public Location getLocation() {
            return new Location(getU16(), getU16(), getByte());
        }

        public byte getByte() {
            return buffer[position++];
        }

        public ushort getU16() {
            ushort val = BitConverter.ToUInt16(buffer, position);
            position += 2;
            return val;
        }

        public uint getU32() {
            uint val = BitConverter.ToUInt32(buffer, position);
            position += 4;
            return val;
        }

        public ulong getU64() {
            ulong val = BitConverter.ToUInt64(buffer, position);
            position += 8;
            return val;
        }

        public ushort PeekU16() {
            return BitConverter.ToUInt16(buffer, position);
        }

        public string getString() {
            ushort stringLen = getU16();
            string val = Encoding.ASCII.GetString(buffer, position, stringLen);
            position += stringLen;
            return val;
        }

        public Outfit getOutfit() {
            var lookType = getByte();
            if (lookType != 0)
                return new Outfit(lookType, getByte(), getByte(), getByte(),
                    getByte());
            else
                return new Outfit(lookType, getU16());
        }

        public double getDouble() {
            byte precision = getByte();
            uint val = getU32();
            return 0; // not yet
        }

        public bool getBool() {
            return (getByte() == 1) ? true : false;
        }

        public int getLength() {
            return buffer.Length;
        }

        public List<byte> getBuffer() {
            return buffer.ToList();
        }

        public int getPosition() {
            return position;
        }

        public void skipBytes(int v) {
            position += v;
        }

        public void setPosition(int v) {
            position = v;
        }
    }
}
