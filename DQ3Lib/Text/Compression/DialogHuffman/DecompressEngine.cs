using DQ3Lib.Streams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Text.Compression.DialogHuffman {
	internal class DecompressEngine {
		private readonly int[] LeftTree;

		private readonly int[] RightTree;

		public DecompressEngine(int[] leftTree, int[] rightTree) {
			this.LeftTree = leftTree;
			this.RightTree = rightTree;
		}

		public string[] Decompress(ByteArrayStream data) {
			var result = new List<string>();

			while (!data.AtEnd) {
				data
			}





			return [.. result];
		}
	}
}
