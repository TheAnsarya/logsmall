using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Compression.Text.Huffman;

internal class HuffmanNode {
	public char Character { get; set; }

	public int Frequency { get; set; }

	public HuffmanNode? Left { get; set; }

	public HuffmanNode? Right { get; set; }

	public bool IsLeaf => Left is null && Right is null;
}
