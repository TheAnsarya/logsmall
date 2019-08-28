using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DataStructures {
	class ByteArrayStream {
		public byte[] Buffer;
		public virtual int Address { get; set; }

		public int Size { get => Buffer.Length; }

		public bool HasSpace { get => Address < Size; }

		public ByteArrayStream(int size) : this(size, 0) { }

		public ByteArrayStream(int size, int startAddress) {
			Buffer = new byte[size];
			Address = startAddress;
		}

		public ByteArrayStream(byte[] buffer) : this(buffer, 0) { }

		public ByteArrayStream(byte[] buffer, int startAddress) {
			Buffer = buffer;
			Address = startAddress;
		}

		public byte Byte() {
			return Buffer[Address++];
		}

		public void Byte(byte value) {
			Buffer[Address++] = value;
		}

		public ushort Word() {
			byte a = Buffer[Address++];
			byte b = Buffer[Address++];
			return (ushort)((b << 8) + a);
		}
	}
}
