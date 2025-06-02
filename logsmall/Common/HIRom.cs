namespace logsmall.Common;

class HiRom : Rom {
	public const int AddressOffset = 0xc00000;

	public override int AddressToPC(int address) => address - AddressOffset;

	public override int AddressToSNES(int address) => address + AddressOffset;
}

