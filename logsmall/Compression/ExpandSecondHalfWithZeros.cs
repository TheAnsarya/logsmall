using logsmall.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Compression {
	// Used in FFMQ alot
	// Writes 16 bytes, then then next 8 bytes are each followed by a zero
	// Used for 3bpp graphics to be displayed in 4bpp mode
	// Input must be in $18 byte chunks
	//  input: 50 51 52 53 54 55 56 57 58 59 5a 5b 5c 5d 5e 5f 60 61 62 63 64 65 66 67
	// output: 50 51 52 53 54 55 56 57 58 59 5a 5b 5c 5d 5e 5f 60 00 61 00 62 00 63 00 64 00 65 00 66 00 67 00
	class ExpandSecondHalfWithZeros {

		public static byte[] Decompress(byte[] source) {
			return Decompress(new ByteArrayStream(source), source.Length);
		}

		public static byte[] Decompress(ByteArrayStream source, int compressedSize) {
			if (compressedSize % 0x18 != 0) {
				throw new ArgumentException($"{nameof(compressedSize)} is wrong ({compressedSize:x2}), {nameof(source)} needs to be in full $18 byte chunks");
			}

			var output = new List<byte>(compressedSize * 4 / 3);

			for (int i = 0; i < compressedSize; i++) {
				output.Add(source.Byte());

				// skip a byte in destination
				if ((i % 0x18) >= 0x10) {
					output.Add(0);
				}
			}

			return output.ToArray();
		}


		public static byte[] Compress(byte[] target) {
			return Compress(new ByteArrayStream(target));
		}

		public static byte[] Compress(ByteArrayStream target) {
			if (target.Address != 0) {
				throw new ArgumentException($"{nameof(target)} address must be 0");
			}

			if (target.Size % 0x20 != 0) {
				throw new ArgumentException($"size is wrong ({target.Size:x2}), {nameof(target)} needs to be in full $20 byte chunks");
			}

			var output = new List<byte>(target.Size * 3 / 4);

			while (!target.AtEnd) {
				output.Add(target.Byte());

				if ((target.Address % 0x20) >= 0x10) {
					var zero = target.Byte();

					if (zero != 0) {
						throw new Exception($"byte should be zero. value: ${zero:x2}  address: ${target.Address - 1:x4}");
					}
				}
			}

			return output.ToArray();
		}

	}
}
