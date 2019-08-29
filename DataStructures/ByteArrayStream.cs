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

		public ByteArrayStream Branch() => Branch(0);

		public ByteArrayStream Branch(int startAddress) {
			return new ByteRingBuffer(Buffer, startAddress);
		}

		public List<ByteArrayStream> FindAll(byte[] searchTerm) {
			var found = new List<ByteArrayStream>();

			var lastIndex = Buffer.Length - searchTerm.Length;
			for (int i = 0; i <= lastIndex; i++) {
				var matches = true;

				for (int j = 0; j < searchTerm.Length; j++) {
					if (Buffer[i + j] != searchTerm[j]) {
						matches = false;
						break;
					}
				}

				if (matches) {
					found.Add(this.Branch(i));
				}
			}

			return found;
		}

		public byte[] GetBytes(int length) => GetBytes(length, Address);

		public byte[] GetBytes(int length, int address) {
			if ((address + length) > Buffer.Length) {
				length = Buffer.Length - address;
			}

			var output = new byte[length];
			Array.Copy(Buffer, address, output, 0, length);

			return output;
		}
	}
}
