using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Text.Compression.DialogHuffman {
	internal class Decompressor {
		public Configuration Configuration { get; init; }

		private int[] LeftTree { get; init; }
		private  int[] RightTree { get; init; }

		public Decompressor() {
			Configuration = new Configuration();
		}
		
		public Decompressor(Configuration configuration) {
			Configuration = configuration;
		}
	}
}
