using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Common {
	class LoRom : Common.Rom {
		public override int AddressToPC(int address) {
			if ((0x00ffff & address) < 0x8000) {
				throw new ArgumentException($"lower word of {nameof(address)} must be at least 0x8000");
			}

			var pc = (((0xff0000 & address) >> 16) * 0x8000) + ((0x00ffff & address) - 0x8000);
			return pc;
		}

		public override int AddressToSNES(int address) {
			var snes = ((address / 0x8000) * 0x10000) + (address % 0x8000) + 0x8000;
			return snes;
		}
	}
}
