using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	class Ram {
		public const int AddressOffset = 0x7e0000;

		public byte[] wram = new byte[0x2_0000];

		// TODO: Make sure these are all right (run tests)
		public byte Byte(int address) {
			var addy = address - AddressOffset;
			return wram[addy];
		}

		public void Byte(int address, byte value) {
			var addy = address - AddressOffset;
			wram[addy] = value;
		}

		public ushort Word(int address) {
			var addy = address - AddressOffset;
			return (ushort)((wram[addy + 1] << 8) + wram[addy]);
		}

		public void Word(int address, ushort value) {
			var addy = address - AddressOffset;
			wram[addy] = (byte)(value & 0x00FF);
			wram[addy+1] = (byte)(value & 0xFF00 >> 8);
		}
		
		public int Long(int address) {
			var addy = address - AddressOffset;
			return (wram[addy + 2] << 16) + (wram[addy + 1] << 8) + wram[addy];
		}
	}
}
