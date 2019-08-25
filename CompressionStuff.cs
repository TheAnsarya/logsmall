using logsmall.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	class CompressionStuff {


		// Overworld tilemap top-level grid layout
		public static byte[] GetLayout() {
			var source = Rom.GetStream(0xed8a00);
			return Compression.BasicRing400.Decompress(source, 0x2000);
		}
	}
}
