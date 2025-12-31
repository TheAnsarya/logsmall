namespace DW4Lib.DataStructures.Maps;

/// <summary>
/// DW4 Overworld map data.
/// Located at Bank $0B.
/// </summary>
public class OverworldMap {
	/// <summary>
	/// Bank containing overworld map data.
	/// </summary>
	public const int Bank = 0x0B;

	/// <summary>
	/// CPU address of main overworld map data.
	/// </summary>
	public const int MainOverworldAddress = 0x8CEE;

	/// <summary>
	/// CPU address of main overworld row pointers.
	/// </summary>
	public const int MainOverworldRowPointers = 0xA590;

	/// <summary>
	/// CPU address of Gottside overworld map data.
	/// </summary>
	public const int GottsideOverworldAddress = 0xA990;

	/// <summary>
	/// CPU address of Gottside overworld row pointers.
	/// </summary>
	public const int GottsideRowPointers = 0xAB65;

	/// <summary>
	/// CPU address of underworld map data.
	/// </summary>
	public const int UnderworldAddress = 0xAC65;

	/// <summary>
	/// CPU address of underworld row pointers.
	/// </summary>
	public const int UnderworldRowPointers = 0xAE89;

	/// <summary>
	/// Overworld type.
	/// </summary>
	public OverworldType Type { get; set; }

	/// <summary>
	/// Decompressed map data (256x256 tiles for full overworld).
	/// </summary>
	public byte[,] Tiles { get; set; } = new byte[256, 256];

	/// <summary>
	/// Row pointer entries.
	/// </summary>
	public OverworldRowPointer[] RowPointers { get; set; } = [];
}

/// <summary>
/// Type of overworld map.
/// </summary>
public enum OverworldType {
	Main,
	Gottside,
	Underworld
}

/// <summary>
/// Overworld row pointer structure (4 bytes).
/// </summary>
public class OverworldRowPointer {
	/// <summary>
	/// Row index.
	/// </summary>
	public int Row { get; set; }

	/// <summary>
	/// Pointer to map data (bytes 0-1, little-endian).
	/// </summary>
	public ushort DataPointer { get; set; }

	/// <summary>
	/// Compressed data size to X=128 (byte 2).
	/// </summary>
	public byte SizeToX128 { get; set; }

	/// <summary>
	/// Compressed data size to X=256 (byte 3).
	/// </summary>
	public byte SizeToX256 { get; set; }

	/// <summary>
	/// Parse row pointer from raw bytes.
	/// </summary>
	public static OverworldRowPointer Parse(byte[] data, int offset, int row) {
		return new OverworldRowPointer {
			Row = row,
			DataPointer = (ushort)(data[offset] | (data[offset + 1] << 8)),
			SizeToX128 = data[offset + 2],
			SizeToX256 = data[offset + 3]
		};
	}
}

/// <summary>
/// Overworld tile decompression utilities.
/// DW4 uses simple RLE compression for overworld maps.
/// </summary>
public static class OverworldCompression {
	/// <summary>
	/// Decompress a row of overworld data.
	/// Format: Bits 0-4 = length+1, Bits 5-7 = tile
	/// Special: If byte >= $E8 (Tile 7 + length >= 8), subtract $E0 for tile number.
	/// </summary>
	/// <param name="data">Compressed data</param>
	/// <param name="offset">Starting offset</param>
	/// <param name="targetWidth">Target decompressed width (128 or 256)</param>
	/// <returns>Decompressed row tiles</returns>
	public static byte[] DecompressRow(byte[] data, int offset, int targetWidth = 256) {
		var result = new List<byte>();
		int pos = offset;

		while (result.Count < targetWidth) {
			byte b = data[pos++];

			byte tile;
			int length;

			if (b >= 0xE8) {
				// Special case: tile number is (byte - $E0)
				tile = (byte)(b - 0xE0);
				length = 1;
			} else {
				// Normal case: bits 0-4 = length-1, bits 5-7 = tile
				length = (b & 0x1F) + 1;
				tile = (byte)((b >> 5) & 0x07);
			}

			for (int i = 0; i < length && result.Count < targetWidth; i++) {
				result.Add(tile);
			}
		}

		return result.ToArray();
	}

	/// <summary>
	/// Compress a row of overworld data.
	/// </summary>
	/// <param name="tiles">Uncompressed tile row</param>
	/// <returns>Compressed data</returns>
	public static byte[] CompressRow(byte[] tiles) {
		var result = new List<byte>();
		int i = 0;

		while (i < tiles.Length) {
			byte tile = tiles[i];
			int runLength = 1;

			// Count consecutive identical tiles
			while (i + runLength < tiles.Length &&
				   tiles[i + runLength] == tile &&
				   runLength < 32) {
				runLength++;
			}

			if (tile >= 0x08) {
				// Tiles >= 8 use special encoding
				for (int j = 0; j < runLength; j++) {
					result.Add((byte)(tile + 0xE0));
				}
			} else {
				// Normal RLE encoding
				byte encoded = (byte)(((tile & 0x07) << 5) | ((runLength - 1) & 0x1F));
				result.Add(encoded);
			}

			i += runLength;
		}

		return result.ToArray();
	}
}

/// <summary>
/// Overworld tile types for DW4.
/// </summary>
public static class OverworldTiles {
	public const byte Grass = 0;
	public const byte Forest = 1;
	public const byte Hills = 2;
	public const byte Desert = 3;
	public const byte Swamp = 4;
	public const byte Water = 5;
	public const byte Mountain = 6;
	public const byte Bridge = 7;

	/// <summary>
	/// Get tile name.
	/// </summary>
	public static string GetName(byte tile) => tile switch {
		0 => "Grass",
		1 => "Forest",
		2 => "Hills",
		3 => "Desert",
		4 => "Swamp",
		5 => "Water",
		6 => "Mountain",
		7 => "Bridge",
		_ => $"Tile_{tile:x2}"
	};

	/// <summary>
	/// Check if tile is passable on foot.
	/// </summary>
	public static bool IsPassable(byte tile) => tile switch {
		Water => false,
		Mountain => false,
		_ => true
	};

	/// <summary>
	/// Check if tile is water (requires ship).
	/// </summary>
	public static bool IsWater(byte tile) => tile == Water;

	/// <summary>
	/// Get encounter rate modifier for tile.
	/// </summary>
	public static int GetEncounterRate(byte tile) => tile switch {
		Grass => 16,
		Forest => 32,
		Hills => 24,
		Desert => 8,
		Swamp => 32,
		Water => 8,
		Mountain => 0,
		Bridge => 0,
		_ => 16
	};
}
