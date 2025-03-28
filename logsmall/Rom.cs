using logsmall.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	// TODO: Depreciate and remove
	class Rom {
		const string filename = @"c:\working\Dragon Quest III - Soshite Densetsu he... (J).smc";

		public const uint AddressOffset = 0xc00000U;

		private static byte[] _rom;
		public static byte[] ROM {
			get {
				if (_rom == null) {
					var size = (int)new FileInfo(filename).Length;
					_rom = new byte[size];
					using var romStream = File.OpenRead(filename);
					romStream.ReadExactly(_rom, 0, size);
				}

				return _rom;
			}
		}

		public static ByteArrayStream GetStream(int address) {
			return new ByteArrayStream(ROM, (int)(address - AddressOffset));
		}

		public static string GetString(uint address, int length) {
			return GetString((int)(address - AddressOffset), length);
		}

		public static string GetString(int address, int length) {
			string segment = "";

			for (int i = 0; i < length; i++) {
				segment += $"{ROM[address + i]:x2}";
			}

			return segment;
		}
		
		public static byte Byte(int address) {
			var addy = (int)(address - AddressOffset);
			return ROM[addy];
		}

		public static ushort Word(int address) {
			var addy = (int)(address - AddressOffset);
			return (ushort)((ushort)(ROM[addy + 1] << 8) + ROM[addy]);
		}


		public static RomByteArray ByteArray(int address) {
			return new RomByteArray { Address = address };
		}
	}
}
