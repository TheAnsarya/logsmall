using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Compression.Text.Huffman;

internal class Encoder {

	public static EncodedText Encode(string text) {
		ArgumentNullException.ThrowIfNull(text, nameof(text));

		Frequencies frequencies = Frequencies.CalculateFrequencies(text);

		HuffmanNode root = HuffmanTree.BuildTree(frequencies);

		EncodingTable encoding = EncodingTable.Build(root);

		// TODO: this uses a lot of memory, but it is only temporary
		string encoded = string.Join("", text.Select(x => encoding[x]));

		return new EncodedText(text, encoded, encoding);
	}
}
