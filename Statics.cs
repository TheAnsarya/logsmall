using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	static class Statics {
		public static IEnumerable<string> Split(this string str, int chunkSize) {
			return Enumerable.Range(0, str.Length / chunkSize)
				.Select(x => str.Substring(x * chunkSize, chunkSize));
		}

		public static string FlipBytes(this string value) {
			return string.Join("", value.Split(2).Reverse());
		}
		/*
		public static int LengthWithTabs(this string value) {
			if (value)

		}
		*/

		public static byte H(this ushort value) => (byte)(value >> 8);
		public static ushort H(this ushort value, byte b) => (ushort)((value & 0x00ff) + (b << 8));
		public static byte L(this ushort value) => (byte)value;
		public static ushort L(this ushort value, byte b) => (ushort)((value & 0xff00) + b);
	}
}
