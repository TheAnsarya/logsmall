using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DataStructures {
	class ReverseReader {
		private byte[] Buffer { get; set; }

		private int Address { get; set; }

		public byte this[int address] {
			get {
				if (address < 0) {
					throw new IndexOutOfRangeException($"{nameof(address)} cannot be less than zero");
				}

				return Buffer[address];
			}
		}

		public ReverseReader(byte[] buffer, int startAddress) {
			Buffer = buffer;
			Address = startAddress;
		}

		public ReverseReader Branch(int address) {
			return new ReverseReader(Buffer, address);
		}

		public byte Byte() {
			return this[Address--];
		}
	}
}
