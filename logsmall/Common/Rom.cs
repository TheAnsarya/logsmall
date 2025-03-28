using logsmall.DataStructures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Common {
	abstract class Rom {
		public string Filename { get; set; }

		abstract public int AddressToPC(int address);
		abstract public int AddressToSNES(int address);

		private byte[] _rom;
		public byte[] ROM {
			get {
				if (_rom == null) {
					var size = (int)new FileInfo(Filename).Length;
					_rom = new byte[size];
					using var romStream = File.OpenRead(Filename);
					romStream.ReadExactly(_rom, 0, size);
				}

				return _rom;
			}
		}

		public ByteArrayStream GetStream(int address) {
			return new ByteArrayStream(ROM, AddressToPC(address));
		}

		public string GetString(uint address, int length) {
			return GetString(AddressToPC((int)address), length);
		}

		public string GetString(int address, int length) {
			string segment = "";

			for (int i = 0; i < length; i++) {
				segment += ROM[address + i].ToString("x2", CultureInfo.InvariantCulture);
			}

			return segment;
		}

		/*public static byte Byte(uint address) {
			var addy = (int)(address - AddressOffset);
			return ROM[addy];
		}*/

		public byte Byte(int address) {
			var addy = AddressToPC(address);
			return ROM[addy];
		}

		/*public static ushort Word(uint address) {
			var addy = (int)(address - AddressOffset);
			return (ushort)((ushort)(ROM[addy + 1] << 8) + ROM[addy]);
		}*/

		public ushort Word(int address) {
			var addy = AddressToPC(address);
			return (ushort)((ushort)(ROM[addy + 1] << 8) + ROM[addy]);
		}

		/*public static uint Long(uint address) {
			var addy = (int)(address - AddressOffset);
			return (uint)((ROM[addy + 2] << 16) + (ROM[addy + 1] << 8) + ROM[addy]);
		}*/

		public uint Long(int address) {
			var addy = AddressToPC(address);
			return (uint)((ROM[addy + 2] << 16) + (ROM[addy + 1] << 8) + ROM[addy]);
		}

		public static RomByteArray ByteArray(int address) {
			return new RomByteArray { Address = address };
		}
	}
}
