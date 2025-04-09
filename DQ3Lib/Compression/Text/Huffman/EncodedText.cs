using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Compression.Text.Huffman;

internal class EncodedText(string Original, string Encoded, EncodingTable Encoding) {
	public string Original { get; } = Original;

	public string Encoded { get; } = Encoded;

	public EncodingTable Encoding { get; } = Encoding;

	public int[] AsData {
		get {
			int[] data = new int[Encoded.Length / 8 + 1];

			for (int i = 0; i < Encoded.Length; i += 8) {
				data[i / 8] = Convert.ToInt32(Encoded.Substring(i, Math.Min(8, Encoded.Length - i)), 2);
			}

			return data;
		}
	}
};
