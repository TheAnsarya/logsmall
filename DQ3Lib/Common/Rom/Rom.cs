using DQ3Lib.Streams;
using System.Globalization;

namespace DQ3Lib.Common.Rom;

abstract class Rom {
	// You can pull all the spans you want from here, so for now I'll not add methods
	public required byte[] Data { get; init; }

	// Abstracted address conversion methods
	abstract public PCAddress ToPCAddress(int address);

	abstract public PCAddress ToPCAddress(SNESAddress address);

	abstract public SNESAddress ToSNESAddress(int address);

	abstract public SNESAddress ToSNESAddress(PCAddress address);

	// 
	public ByteArrayStream GetStream(PCAddress address) => new(Data, address.Address);

	public ByteArrayStream GetStream(SNESAddress address) => new(Data, ToPCAddress(address).Address);
}
