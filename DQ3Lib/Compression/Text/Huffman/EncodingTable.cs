using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Compression.Text.Huffman;

internal class EncodingTable : Dictionary<char, string> {
	public static EncodingTable Build(HuffmanNode node) {
		ArgumentNullException.ThrowIfNull(node, nameof(node));

		EncodingTable encoding = [];

		BuildRecursive(encoding, node, string.Empty);

		return encoding;
	}

	// TODO: remove/flatten recursive calls, although it shouldn't go more than 5 or 6 levels deep it is a lot of method calls (or make local method?)
	private static void BuildRecursive(EncodingTable encoding, HuffmanNode node, string prefix) {
		if (node.IsLeaf) {
			encoding[node.Character] = prefix;
			return;
		}

		if (node.Left != null) {
			BuildRecursive(encoding, node.Left, prefix + "0");
		}

		if (node.Right != null) {
			BuildRecursive(encoding, node.Right, prefix + "1");
		}
	}
}
