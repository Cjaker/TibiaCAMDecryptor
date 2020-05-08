using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaCAMDecryptor {
    public class Recording {
        public string FilePath { get; set; }
        public byte[] FileBuffer { get; set; }
        public bool IsValid { get; set; }
        public Location Location { get; internal set; }
        public bool HasProblem { get; internal set; }

        public List<Packet> Packets = new List<Packet>();
        public Recording(string filePath) {
            FilePath = filePath;
            IsValid = true;
            HasProblem = false;
        }

        public void Parse() {
            FileBuffer = File.ReadAllBytes(FilePath);
            BinaryReader br = new BinaryReader(new MemoryStream(FileBuffer));
            byte fileVersionByte = br.ReadByte();
            if (fileVersionByte != 3) {
                Console.WriteLine("Wrong file version! [" + FilePath + "]");
                Console.ReadLine();
                IsValid = false;
                HasProblem = true;
                return;
            }

            byte fileCompressionVersionByte = br.ReadByte();
            if (fileCompressionVersionByte != 1 && fileCompressionVersionByte != 2) {
                Console.WriteLine("Unknown compression! [" + FilePath + "]");
                Console.ReadLine();
                IsValid = false;
                HasProblem = true;
                return;
            }

            uint packetCount = br.ReadUInt32();
            if (fileCompressionVersionByte == 2)
                packetCount -= 57;

            if (packetCount == 0) {
                Console.WriteLine("Empty recording file! [" + FilePath + "]");
                Console.ReadLine();
                IsValid = false;
                HasProblem = true;
                return;
            }

            try {
                for (int index = 0; index < packetCount; index++) {
                    uint packetLength = 0;
                    if (fileCompressionVersionByte == 1)
                        packetLength = br.ReadUInt32();
                    else if (fileCompressionVersionByte == 2)
                        packetLength = br.ReadUInt16();

                    if (packetLength == 0) {
                        Console.WriteLine("Invalid packet length! PacketID: " + index + " [" + FilePath + "]");
                        Packets.Clear();
                        IsValid = false;
                        HasProblem = true;
                        break;
                    }

                    uint packetTime = br.ReadUInt32();

                    // get packet data
                    byte[] packetData = br.ReadBytes((int)packetLength);

                    // adler checksum
                    uint packetDataChecksum = Tools.AdlerChecksum((uint)packetData.Length, packetData);
                    uint packetChecksumFile = br.ReadUInt32();

                    if (packetDataChecksum != packetChecksumFile) {
                        Console.WriteLine("Invalid checksum! PacketID: " + index + " [" + FilePath + "]");
                        Packets.Clear();
                        IsValid = false;
                        HasProblem = true;
                        break;
                    }

                    // decrypt packet
                    Decrypt(ref packetData, packetTime);
                    Packets.Add(new Packet(packetData, packetLength, packetTime));
                }
            } catch (Exception ex) {
                IsValid = false;
                HasProblem = true;
            }

            /*Console.WriteLine("Recording: " + FilePath);
            Console.WriteLine("Status: Done");
            Console.WriteLine("Packets Count: " + Packets.Count);*/
        }

        public void Decrypt(ref byte[] packetData, uint packetTime) {
            Crypt.DecryptPacket(ref packetData, packetTime);
        }
    }
}
