using logsmall.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	class Compression {


		// Overworld tilemap top-level grid layout
		public static byte[] GetLayout() {
			var source = Rom.GetStream(0xed8a00);
			return Compression.BasicRing400Decompress(source, 0x1000);
		}

		public static byte[] BasicRing400Decompress(ByteArrayStream source, int outputSize) {
			var output = new ByteArrayStream(outputSize);
			var work = new ByteRingBuffer(0x400, 0x03be);
			Queue<bool> commands = source.Byte().ToBooleanQueue();

			while (output.HasSpace) {
				if (commands.Count == 0) {
					commands = source.Byte().ToBooleanQueue();
				}

				if (commands.Dequeue()) {
					var b = source.Byte();

					work.Byte(b);
					output.Byte(b);
				} else {
					var d1 = source.Byte();
					var d2 = source.Byte();

					var address = d1 + ((d2 << 2) & 0x0300);
					var counter = (d2 & 0x3f) + 3;
					var copySource = work.Branch(address);

					while (output.HasSpace && (counter != 0)) {
						var copy = copySource.Byte();
						counter--;

						work.Byte(copy);
						output.Byte(copy);
					}
				}
			}

			return output.Buffer;
		}

		public static byte[] BasicRing400Compress(byte[] target) {
			var commands = new List<Ring400Command>();

			throw new NotImplementedException();
		}

		private class Ring400Command {
			public bool Command { get; set; }
			public byte Value { get; set; }

			private int _address;
			public int Address {
				get {
					return _address;
				}
				set {
					if ((value < 0) || (value >= 0x400)) {
						throw new ArgumentOutOfRangeException($"{nameof(Address)} must be between 0x000 and 0x3ff: 0x{value.ToString("x3")}");
					}
					_address = value;
				}
			}

			private int _copySize;
			public int CopySize {
				get {
					return _copySize;
				}
				set {
					if ((value < 3) || (value > 66)) {
						// Range is (6 bits) + 3
						throw new ArgumentOutOfRangeException($"{nameof(CopySize)} must be between 3 and 66: {value}");
					}
					_copySize = value;
				}
			}

			public (byte H, byte L) GetCopyData() {
				var low = (byte)(Address & 0xff);
				var high = (byte)(((Address & 0x300) >> 2) + (CopySize - 3));

				return (high, low);
			}
		}
	}
}
