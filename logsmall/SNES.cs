using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logsmall {
	class SNES {

		public static bool IsNegative16(int value) => (value & 0x8000) == 0x8000;
		public static bool IsNegative8(int value) => (value & 0x80) == 0x80;


		public static string GetValue(Regex mode, string line) {
			var match = mode.Match(line);
			return match.Success
				? (match.Groups.Count > 2)
					? $"{match.Groups[1].Value}{match.Groups[2].Value}"
					: match.Groups[1].Value
				: null;
		}


		public static string OpToHex(string address, string op, string parameters) {
			var lookup = OpLookup.Find(op, parameters);

			if (lookup != null) {
				if (lookup.Mode == AddressingMode.PCRelative) {
					var dist =
						(
							(sbyte)
							(int.Parse(GetValue(lookup.Mode, parameters), NumberStyles.HexNumber)
								- int.Parse(address.Substring(2, 4), NumberStyles.HexNumber)
								- 2)
						)
						.ToString("X2")
						.ToLowerInvariant();
					return $"{lookup.Byte}{dist}";
				} else if (lookup.Mode == AddressingMode.PCRelative24) {
					var dist =
						(
							(sbyte)
							(int.Parse(GetValue(lookup.Mode, parameters), NumberStyles.HexNumber)
								- int.Parse(address, NumberStyles.HexNumber)
								- 2)
						)
						.ToString("X2")
						.ToLowerInvariant();
					return $"{lookup.Byte}{dist}";
				} else if (lookup.Mode == AddressingMode.PCRelativeLong) {
					var dist =
						(
							(short)
							(int.Parse(GetValue(lookup.Mode, parameters), NumberStyles.HexNumber)
								- int.Parse(address.Substring(2, 4), NumberStyles.HexNumber)
								- 3)
						)
						.ToString("X4")
						.ToLowerInvariant();
					return $"{lookup.Byte}{dist.FlipBytes()}";
				} else if (lookup.Mode == AddressingMode.PCRelativeLong24) {
					var dist =
						(
							(short)
							(int.Parse(GetValue(lookup.Mode, parameters), NumberStyles.HexNumber)
								- int.Parse(address, NumberStyles.HexNumber)
								- 3)
						)
						.ToString("X4")
						.ToLowerInvariant();
					return $"{lookup.Byte}{dist.FlipBytes()}";
				}
				if (lookup.Mode == AddressingMode.BlockMove) {
					return $"{lookup.Byte}{GetValue(lookup.Mode, parameters).FlipBytes()}";
				}
				return $"{lookup.Byte}{GetValue(lookup.Mode, parameters).FlipBytes()}";
			}

			return null;
		}
	}
}
