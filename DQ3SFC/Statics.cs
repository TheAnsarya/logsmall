using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	static class Statics {

		// offset is in bytes
		// throws out of bound exceptions
		public static int Word(this Span<byte> span, int offset) => (ushort)(span[offset] + (span[offset + 1] << 8));
		public static int Long(this Span<byte> span, int offset) => span[offset] + (span[offset + 1] << 8) + (span[offset + 2] << 16);

		//public static byte Bank(this int value) => (byte)((value & 0xff0000) >> 16);
	}
}
