using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Compression.Text.Huffman;

internal class Frequencies : Dictionary<char, int> {
	public static Frequencies CalculateFrequencies(string text) {
		ArgumentNullException.ThrowIfNull(text, nameof(text));

		var frequencies = new Frequencies();

		foreach (var character in text) {
			if (!frequencies.ContainsKey(character)) {
				frequencies[character] = 0;
			}

			frequencies[character]++;
		}

		return frequencies;
	}
}
