using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaCAMDecryptor {
    public class Outfit {
        private ushort lookType;
        private ushort lookItem;
        private byte head;
        private byte body;
        private byte legs;
        private byte feet;
        private byte addons;
        private ushort mountId;

        public ushort LookType {
            get { return lookType; }
            set { lookType = value; }
        }
        public ushort LookItem {
            get { return lookItem; }
            set { lookItem = value; }
        }
        public byte Head {
            get { return head; }
            set { head = value; }
        }
        public byte Body {
            get { return body; }
            set { body = value; }
        }
        public byte Legs {
            get { return legs; }
            set { legs = value; }
        }
        public byte Feet {
            get { return feet; }
            set { feet = value; }
        }

        public Outfit(ushort looktype, ushort lookitem) {
            this.lookItem = lookitem;
            this.lookType = looktype;
        }

        public Outfit(ushort looktype, byte head, byte body, byte legs, byte feet) {
            this.lookType = looktype;
            this.head = head;
            this.body = body;
            this.legs = legs;
            this.feet = feet;
        }

        public override string ToString() {
            return "LookType: " + LookType.ToString();
        }

        public override bool Equals(object obj) {
            if (obj is Outfit)
                return Equals((Outfit)obj);

            return false;
        }

        public bool Equals(Outfit outfit) {
            return LookType == outfit.LookType && Head == outfit.Head && Body == outfit.Body
                && Legs == outfit.Legs && Feet == outfit.Feet ||
                LookType == outfit.LookType && LookItem == outfit.LookItem;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
