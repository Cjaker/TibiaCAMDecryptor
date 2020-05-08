using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaCAMDecryptor {
    public class Crypt {
        public static void DecryptPacket(ref byte[] buffer, uint time) {
            uint key = (uint)(buffer.Length + time) & 0xFF;
            for (int index = 0; index < buffer.Length; index++) {
                uint minus = (uint)(key + 33 * index + 2) & 0xFF;
                if ((minus & 0x80) == 0x80) {
                    while (((minus - 1) % 5) != 0) {
                        minus += 1;
                    }
                } else {
                    while ((minus % 5) != 0) {
                        minus += 1;
                    }
                }

                buffer[index] = (byte)((buffer[index] - minus) & 0xFF);
            }
        }
    }
}
