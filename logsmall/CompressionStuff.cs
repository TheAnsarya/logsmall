namespace logsmall;

class CompressionStuff {


	// Overworld tilemap top-level grid layout
	public static byte[] GetLayout() {
		var source = Rom.GetStream(0xed8a00);
		return Compression.BasicRing400.Decompress(source, 0x2000);
	}
}

