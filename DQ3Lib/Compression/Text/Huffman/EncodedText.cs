using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Compression.Text.Huffman;

internal class EncodedText(string Original, byte[] Encoded, EncodingTable Encoding) {
	public string Original { get; } = Original;

	public byte[] Encoded { get; } = Encoded;

	public EncodingTable Encoding { get; } = Encoding;
};
