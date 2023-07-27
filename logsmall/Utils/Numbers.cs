using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Utils {
	internal static class Numbers {
		public static string AsLittleEndianHexStringByte(this int value) {
			var bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			return $"{bytes[0]:x2}";
		}
		public static string AsLittleEndianHexStringWord(this int value) {
			var bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			return $"{bytes[0]:x4}";
		}
		public static string AsLittleEndianHexStringLong(this int value) {
			var bytes = BitConverter.GetBytes(value);
			Array.Reverse(bytes);
			return $"{bytes[0]:x6}";
		}
	}
}
