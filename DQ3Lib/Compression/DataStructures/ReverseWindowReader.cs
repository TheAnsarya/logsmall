using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Compression.DataStructures;

class ReverseWindowReader {
	private byte[] Buffer { get; set; }

	private int Address { get; set; }

	public byte this[int address] => address < 0 ? (byte)0 : Buffer[address];

	public ReverseWindowReader(byte[] buffer, int startAddress) {
		Buffer = buffer;
		Address = startAddress;
	}

	public ReverseWindowReader Branch(int address) => new(Buffer, address);

	public byte Byte() => this[Address--];
}
