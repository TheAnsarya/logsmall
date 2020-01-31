using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DQ3SFC {
	class Rom {
		const string RomFilename = @"c:\working\Dragon Quest III - Soshite Densetsu he... (J).smc";
		const int AddressOffset = 0xc00000;

		public static int AddressToPC(int address) => address - AddressOffset;

		public static int AddressToSNES(int address) => address + AddressOffset;

		public static Rom Active { get; set; } = new Rom();

		public static Memory<byte> All { get => Active.Data; }

		public static Memory<byte> Slice(int offset, int length) => Active.Data.Slice((offset >= AddressOffset) ? AddressToPC(offset) : offset, length);
		
		private readonly byte[] _RomBytes;

		public Memory<byte> Data { get; set; }

		public Rom() {
			_RomBytes = File.ReadAllBytes(RomFilename);
			Data = _RomBytes.AsMemory();
		}
	}
}
