namespace DQ3Lib.Text.Font.ScriptFont;

/// <summary>
/// Patches DQ3r SNES ROM with English font data.
/// Replaces Japanese script font with English characters.
/// </summary>
public static class EnglishFontPatcher {
	/// <summary>
	/// Configuration for font addresses.
	/// </summary>
	private static readonly EnglishConfiguration Config = new();

	/// <summary>
	/// Patch a DQ3r ROM with English font.
	/// </summary>
	/// <param name="romData">The ROM data to patch (modified in place).</param>
	/// <param name="fontTiles">Dictionary of character -> 4bpp tile data from FontToDQ3r.</param>
	/// <returns>True if successful.</returns>
	public static bool PatchRom(byte[] romData, Dictionary<char, byte[]> fontTiles) {
		if (romData == null || romData.Length < Config.BankAddress + 0x10000) {
			return false;
		}

		try {
			// Build font group table and tile data
			var groupTable = new List<byte>();
			var tileData = new List<byte>();

			int currentOffset = 0;

			foreach (string groupChars in EnglishConfiguration.CharacterGroups) {
				// Build tile data for this group
				var groupTileData = new List<byte>();

				foreach (char c in groupChars) {
					if (fontTiles.TryGetValue(c, out byte[]? tiles)) {
						groupTileData.AddRange(tiles);
					}
					else {
						// Use placeholder tile for missing characters
						groupTileData.AddRange(new byte[64]); // 8x12 @ 4bpp = ~64 bytes per char
					}
				}

				// Calculate group structure (5 bytes)
				int groupSize = groupTileData.Count;
				int width = 8; // Standard English font width
				int height = 12; // Standard English font height

				// Pack: size (12 bits), width (4 bits), height (4 bits), offset (16 bits)
				// Byte 0-1: size (12 bits) | width high (4 bits)
				// Byte 2: width low (4 bits) | height (4 bits)
				// Byte 3-4: offset (16 bits)
				byte b0 = (byte)(groupSize & 0xFF);
				byte b1 = (byte)(((groupSize >> 8) & 0x0F) | ((width & 0x0F) << 4));
				byte b2 = (byte)(height & 0xFF);
				byte b3 = (byte)(currentOffset & 0xFF);
				byte b4 = (byte)((currentOffset >> 8) & 0xFF);

				groupTable.Add(b0);
				groupTable.Add(b1);
				groupTable.Add(b2);
				groupTable.Add(b3);
				groupTable.Add(b4);

				tileData.AddRange(groupTileData);
				currentOffset += groupSize;
			}

			// Write group table to ROM
			int groupTableAddr = Config.GroupTableAddress;
			for (int i = 0; i < groupTable.Count && groupTableAddr + i < romData.Length; i++) {
				romData[groupTableAddr + i] = groupTable[i];
			}

			// Write tile data to ROM
			int tileDataAddr = Config.BankAddress;
			for (int i = 0; i < tileData.Count && tileDataAddr + i < romData.Length; i++) {
				romData[tileDataAddr + i] = tileData[i];
			}

			return true;
		}
		catch {
			return false;
		}
	}

	/// <summary>
	/// Extract current font data from ROM for analysis.
	/// </summary>
	public static FontData? ExtractFontData(byte[] romData) {
		if (romData == null || romData.Length < Config.GroupTableAddress + 250) {
			return null;
		}

		var result = new FontData {
			Groups = new List<FontGroupInfo>(),
		};

		int addr = Config.GroupTableAddress;

		for (int g = 0; g < Config.Groups; g++) {
			if (addr + 5 > romData.Length) break;

			// Read 5-byte group structure
			int b0 = romData[addr++];
			int b1 = romData[addr++];
			int b2 = romData[addr++];
			int b3 = romData[addr++];
			int b4 = romData[addr++];

			// Decode structure
			int size = b0 | ((b1 & 0x0F) << 8);
			int width = (b1 >> 4) & 0x0F;
			int height = b2 & 0x0F;
			int offset = b3 | (b4 << 8);

			result.Groups.Add(new FontGroupInfo {
				Index = g,
				Size = size,
				Width = width,
				Height = height,
				DataOffset = offset,
			});
		}

		return result;
	}

	/// <summary>
	/// Font data extracted from ROM.
	/// </summary>
	public class FontData {
		/// <summary>List of font groups.</summary>
		public List<FontGroupInfo> Groups { get; set; } = [];
	}

	/// <summary>
	/// Information about a font group.
	/// </summary>
	public class FontGroupInfo {
		/// <summary>Group index.</summary>
		public int Index { get; set; }

		/// <summary>Data size in bytes.</summary>
		public int Size { get; set; }

		/// <summary>Character width in pixels.</summary>
		public int Width { get; set; }

		/// <summary>Character height in pixels.</summary>
		public int Height { get; set; }

		/// <summary>Offset into font bank.</summary>
		public int DataOffset { get; set; }

		public override string ToString() =>
			$"Group {Index}: {Width}x{Height}, Size={Size}, Offset=0x{DataOffset:X4}";
	}
}
