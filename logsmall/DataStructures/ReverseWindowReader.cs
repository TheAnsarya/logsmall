using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DataStructures {
	class ReverseWindowReader {
		private byte[] Buffer { get; set; }

		private int Address { get; set; }

		public byte this[int address] => (address < 0) ? (byte)0 : Buffer[address];

		public ReverseWindowReader(byte[] buffer, int startAddress) {
			Buffer = buffer;
			Address = startAddress;
		}

		public ReverseWindowReader Branch(int address) {
			return new ReverseWindowReader(Buffer, address);
		}

		public byte Byte() {
			return this[Address--];
		}
	}
}
