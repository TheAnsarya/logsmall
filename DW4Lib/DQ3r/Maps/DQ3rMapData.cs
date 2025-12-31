namespace DW4Lib.DQ3r.Maps;

/// <summary>
/// DQ3r SNES Map data structures and ROM addresses.
/// DQ3r uses HiROM addressing (6MB ROM).
/// </summary>
public static class DQ3rMapConstants {
	// ============================================================
	// Overworld Map Addresses
	// ============================================================

	/// <summary>
	/// World map layout data (LZSS compressed).
	/// Decompresses to $2000 bytes (4096 16-bit chunk indices).
	/// </summary>
	public const int WorldMapLayoutAddress = 0xed8a00;

	/// <summary>
	/// Metatile definitions for overworld.
	/// Located at $e54f38 - $e5569f.
	/// </summary>
	public const int MetatileDefinitionAddress = 0xe54f38;

	/// <summary>
	/// End of metatile definition data.
	/// </summary>
	public const int MetatileDefinitionEnd = 0xe5569f;

	/// <summary>
	/// Number of metatile entries (237: $00 to $EC).
	/// </summary>
	public const int MetatileCount = 237;

	// ============================================================
	// Tilemap Chunk Addresses (16 data streams for 4x4 grid)
	// ============================================================

	/// <summary>
	/// Top row tilemap chunk addresses.
	/// </summary>
	public static readonly int[] TilemapChunkRow0 = [
		0xeda49c,  // Top-left
		0xedaed9,
		0xedb916,
		0xedc353   // Top-right
	];

	/// <summary>
	/// Second row tilemap chunk addresses.
	/// </summary>
	public static readonly int[] TilemapChunkRow1 = [
		0xedcd90,
		0xedd7cd,
		0xede20a,
		0xedec47
	];

	/// <summary>
	/// Third row tilemap chunk addresses.
	/// </summary>
	public static readonly int[] TilemapChunkRow2 = [
		0xedf684,
		0xee00c1,
		0xee0afe,
		0xee153b
	];

	/// <summary>
	/// Bottom row tilemap chunk addresses.
	/// </summary>
	public static readonly int[] TilemapChunkRow3 = [
		0xee1f78,  // Bottom-left
		0xee29b5,
		0xee33f2,
		0xee3e2f   // Bottom-right
	];

	/// <summary>
	/// All tilemap chunk addresses as 2D array [row, column].
	/// </summary>
	public static readonly int[,] TilemapChunkAddresses = {
		{ 0xeda49c, 0xedaed9, 0xedb916, 0xedc353 },
		{ 0xedcd90, 0xedd7cd, 0xede20a, 0xedec47 },
		{ 0xedf684, 0xee00c1, 0xee0afe, 0xee153b },
		{ 0xee1f78, 0xee29b5, 0xee33f2, 0xee3e2f }
	};

	// ============================================================
	// Graphics Addresses
	// ============================================================

	/// <summary>
	/// World map tile graphics.
	/// </summary>
	public const int WorldMapTilesAddress = 0x180000;

	/// <summary>
	/// World map tile graphics size.
	/// </summary>
	public const int WorldMapTilesSize = 0x8000; // 32KB

	/// <summary>
	/// Town/dungeon tile graphics.
	/// </summary>
	public const int TownTilesAddress = 0x190000;

	// ============================================================
	// Map Decompression Constants
	// ============================================================

	/// <summary>
	/// Decompressed world map layout size.
	/// </summary>
	public const int DecompressedLayoutSize = 0x2000; // 8KB

	/// <summary>
	/// Number of layout chunk entries.
	/// </summary>
	public const int LayoutChunkCount = 0x1000; // 4096

	/// <summary>
	/// World map dimensions in tiles.
	/// </summary>
	public const int WorldMapWidth = 256;
	public const int WorldMapHeight = 256;

	/// <summary>
	/// Layout grid dimensions (in chunks).
	/// </summary>
	public const int LayoutGridWidth = 64;
	public const int LayoutGridHeight = 64;

	/// <summary>
	/// Tiles per chunk (4x4).
	/// </summary>
	public const int TilesPerChunk = 4;
}

/// <summary>
/// DQ3r Metatile definition (variable size).
/// </summary>
public class DQ3rMetatile {
	/// <summary>
	/// Metatile index ($00 to $EC).
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// Raw metatile data.
	/// </summary>
	public byte[] Data { get; set; } = [];

	/// <summary>
	/// Tile indices that make up this metatile (16x16 = 4x4 8x8 tiles).
	/// </summary>
	public ushort[] TileIndices { get; set; } = [];

	/// <summary>
	/// Palette index.
	/// </summary>
	public byte Palette { get; set; }

	/// <summary>
	/// Flip flags.
	/// </summary>
	public byte FlipFlags { get; set; }

	/// <summary>
	/// Priority flag.
	/// </summary>
	public bool Priority { get; set; }
}

/// <summary>
/// DQ3r map chunk (4x4 tile block).
/// </summary>
public class DQ3rMapChunk {
	/// <summary>
	/// Chunk index.
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// 16 tile indices (4 rows × 4 columns).
	/// </summary>
	public byte[] Tiles { get; set; } = new byte[16];

	/// <summary>
	/// Get tile at position.
	/// </summary>
	public byte GetTile(int row, int column) => Tiles[row * 4 + column];

	/// <summary>
	/// Set tile at position.
	/// </summary>
	public void SetTile(int row, int column, byte value) => Tiles[row * 4 + column] = value;
}

/// <summary>
/// DQ3r World map layout entry (2 bytes per entry).
/// </summary>
public class DQ3rLayoutEntry {
	/// <summary>
	/// Entry index (0-4095).
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// X position in layout grid (0-63).
	/// </summary>
	public int GridX => Index % DQ3rMapConstants.LayoutGridWidth;

	/// <summary>
	/// Y position in layout grid (0-63).
	/// </summary>
	public int GridY => Index / DQ3rMapConstants.LayoutGridWidth;

	/// <summary>
	/// Chunk index (low byte of 16-bit value).
	/// </summary>
	public byte ChunkIndex { get; set; }

	/// <summary>
	/// High byte (flags/extended index).
	/// </summary>
	public byte HighByte { get; set; }

	/// <summary>
	/// Full 16-bit value.
	/// </summary>
	public ushort Value => (ushort)(ChunkIndex | (HighByte << 8));
}

/// <summary>
/// Complete DQ3r world map data.
/// </summary>
public class DQ3rWorldMap {
	/// <summary>
	/// Layout entries (4096 entries).
	/// </summary>
	public DQ3rLayoutEntry[] Layout { get; set; } = new DQ3rLayoutEntry[DQ3rMapConstants.LayoutChunkCount];

	/// <summary>
	/// Map chunks.
	/// </summary>
	public DQ3rMapChunk[] Chunks { get; set; } = [];

	/// <summary>
	/// Full tilemap (256×256).
	/// </summary>
	public byte[,] Tilemap { get; set; } = new byte[DQ3rMapConstants.WorldMapHeight, DQ3rMapConstants.WorldMapWidth];

	/// <summary>
	/// Metatile definitions.
	/// </summary>
	public DQ3rMetatile[] Metatiles { get; set; } = new DQ3rMetatile[DQ3rMapConstants.MetatileCount];
}

/// <summary>
/// DQ3r town/dungeon map data.
/// </summary>
public class DQ3rTownMap {
	/// <summary>
	/// Map ID.
	/// </summary>
	public int MapId { get; set; }

	/// <summary>
	/// Map name.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Map width in tiles.
	/// </summary>
	public int Width { get; set; }

	/// <summary>
	/// Map height in tiles.
	/// </summary>
	public int Height { get; set; }

	/// <summary>
	/// Tile data (2 bytes per tile: tile index + attributes).
	/// </summary>
	public ushort[,]? Tiles { get; set; }

	/// <summary>
	/// Tileset index.
	/// </summary>
	public int TilesetIndex { get; set; }

	/// <summary>
	/// Compression type.
	/// </summary>
	public CompressionType Compression { get; set; }
}

/// <summary>
/// Compression types used in DQ3r.
/// </summary>
public enum CompressionType {
	/// <summary>
	/// No compression.
	/// </summary>
	None,

	/// <summary>
	/// LZSS compression (sliding window).
	/// </summary>
	LZSS,

	/// <summary>
	/// Ring buffer compression (Ring400).
	/// </summary>
	Ring400,

	/// <summary>
	/// RLE compression.
	/// </summary>
	RLE
}
