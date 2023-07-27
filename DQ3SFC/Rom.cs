using DQ3SFC.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DQ3SFC {
	class Rom {
		const string Filename = @"c:\working\Dragon Quest III - Soshite Densetsu he... (J).smc";
		const int AddressOffset = 0xc00000;

		public static int AddressToPC(int address) => (address >= AddressOffset) ? (address - AddressOffset) : address;

		public static int AddressToSNES(int address) => (address >= AddressOffset) ? address : (address + AddressOffset);

		public static Rom Active { get; set; } = new Rom();

		public static Memory<byte> All { get => Active.Data; }

		public static Memory<byte> Slice(int address) => Active.Data[AddressToPC(address)..];

		public static Memory<byte> Slice(int address, int length) => Active.Data.Slice(AddressToPC(address), length);

		public static ByteArrayStream StreamAt(int address) => new ByteArrayStream(Active.Data, AddressToPC(address));

		private readonly byte[] _RomBytes;

		public Memory<byte> Data { get; set; }

		public Rom() {
			_RomBytes = File.ReadAllBytes(Filename);
			Data = _RomBytes.AsMemory();
		}

	}
}
