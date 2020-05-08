using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaCAMDecryptor {
    public class Tools {
        public const uint AdlerBase = 0xFFF1;
        public const uint AdlerStart = 0x0001;
        public static uint AdlerChecksum(uint length, byte[] buffer) {
            int v3 = 0;
            uint v4 = (uint)AdlerStart;
            uint i = 0;
            for (i = AdlerStart >> 16; v3 < length; i = (i + v4) % 0xFFF1)
                v4 = (uint)(v4 + buffer[v3++]) % 0xFFF1;

            return v4 + (i << 16);
        }
    }
}
