using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Compression.Text.Huffman;

internal class HuffmanTreeAndTable {
	public required HuffmanNode Root { get; set; }

	public required EncodingTable EncodingTable { get; set; }
}
