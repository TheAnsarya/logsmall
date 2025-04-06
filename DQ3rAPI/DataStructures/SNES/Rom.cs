using DQ3rAPI.Domain.Hash;
using System.Diagnostics.CodeAnalysis;

namespace DQ3rAPI.DataStructures.SNES;

class Rom {
	// TODO: This offset is for DQ3r, but it should be configurable for other ROMs, like lorom, hirom, etc.
	const int AddressOffset = 0xc00000;

	public static int AddressToPC(int address) => address >= AddressOffset ? address - AddressOffset : address;

	public static int AddressToSNES(int address) => address >= AddressOffset ? address : address + AddressOffset;

	public Memory<byte> All { get => Data; }

	public Memory<byte> Slice(int address) => Data[AddressToPC(address)..];

	public Memory<byte> Slice(int address, int length) => Data.Slice(AddressToPC(address), length);

	public ByteArrayStream StreamAt(int address) => new(Data, AddressToPC(address));

	private readonly byte[] _RomBytes;

	public required Memory<byte> Data { get; set; }

	// TODO: SetsRequiredMembers doesn't actually check, but is here because Data is required and IS SET in the constructor
	// but error CS9035 is still appearing, so this is a workaround until the compiler starts correctly checking this situation in classes
	// if you add another required field, you will need to ensure it is set in the constructor
	// https://stackoverflow.com/questions/76909169/required-keyword-causes-error-even-if-member-initialized-in-constructor
	[SetsRequiredMembers]
	public Rom(string filename) {
		_RomBytes = File.ReadAllBytes(filename);
		_RomBytes ??= [];
		Data = _RomBytes.AsMemory();
	}
}
