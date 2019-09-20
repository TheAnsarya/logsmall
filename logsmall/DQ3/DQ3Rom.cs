using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3 {
	class DQ3Rom : Common.Rom {
		public override string filename => @"c:\working\Dragon Quest III - Soshite Densetsu he... (J).smc";

		public const int AddressOffset = 0xc00000;

		public override int AddressToPC(int address) => address - AddressOffset;

		public override int AddressToSNES(int address) => address + AddressOffset;
	}
}
