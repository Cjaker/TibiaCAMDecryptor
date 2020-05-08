﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaCAMDecryptor {
    public class OtFileNode {
        public const byte NODE_START = 0xFE;
        public const byte NODE_END = 0xFF;
        public const byte ESCAPE = 0xFD;

        public long Start { get; set; }
        public long PropsSize { get; set; }
        public long Type { get; set; }
        public OtFileNode Next { get; set; }
        public OtFileNode Child { get; set; }
    }
}
