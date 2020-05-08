using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaCAMDecryptor {
    public class Packet {
        public byte[] PacketData { get; set; }
        public uint PacketLength { get; set; }
        public uint PacketTime { get; set; }
        public Packet(byte[] packetData, uint packetLen, uint packetTime) {
            PacketData = packetData;
            PacketLength = packetLen;
            // get length from packetData
            PacketLength = BitConverter.ToUInt16(packetData, 0);
            PacketTime = packetTime;
        }

        // skip the 2 bytes of length
        public byte[] GetPacketData() {
            byte[] newPacketData = new byte[PacketLength];
            Array.Copy(PacketData, 2, newPacketData, 0, PacketLength);
            return newPacketData;
        }
    }
}
