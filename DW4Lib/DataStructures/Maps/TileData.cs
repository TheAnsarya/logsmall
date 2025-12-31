namespace DW4Lib.DataStructures.Maps;

/// <summary>
/// DW4 Tile data structure (2 bytes per tile entry).
/// Located in Bank $08, $8ADB-$979A (51 tilesets × 64 bytes).
/// </summary>
public class Tile {
	/// <summary>
	/// Tileset index this tile belongs to.
	/// </summary>
	public int TilesetIndex { get; set; }

	/// <summary>
	/// Tile index within the tileset (0-31).
	/// </summary>
	public int TileIndex { get; set; }

	/// <summary>
	/// Smoothing flags for front-facing tiles (3 bits).
	/// </summary>
	public byte SmoothingFlags { get; set; }

	/// <summary>
	/// Attribute data/color palette (2 bits).
	/// </summary>
	public byte Attribute { get; set; }

	/// <summary>
	/// Full tile number (11 bits: 3 high from byte 0 + 8 low from byte 1).
	/// </summary>
	public ushort TileNumber { get; set; }

	/// <summary>
	/// High bits of tile number (3 bits from byte 0).
	/// </summary>
	public byte TileNumberHigh => (byte)((TileNumber >> 8) & 0x07);

	/// <summary>
	/// Low bits of tile number (8 bits from byte 1).
	/// </summary>
	public byte TileNumberLow => (byte)(TileNumber & 0xFF);

	/// <summary>
	/// Parse tile from raw bytes.
	/// Byte 0: yyyAAXXX (yyy=smoothing, AA=attribute, XXX=tile high)
	/// Byte 1: xxxxxxxx (tile low)
	/// </summary>
	public static Tile Parse(byte byte0, byte byte1, int tilesetIndex, int tileIndex) {
		return new Tile {
			TilesetIndex = tilesetIndex,
			TileIndex = tileIndex,
			SmoothingFlags = (byte)((byte0 >> 5) & 0x07),
			Attribute = (byte)((byte0 >> 3) & 0x03),
			TileNumber = (ushort)(((byte0 & 0x07) << 8) | byte1)
		};
	}
}

/// <summary>
/// Extended tile data (3 bytes per entry).
/// Located at Bank $08, $A80D.
/// </summary>
public class TileData {
	/// <summary>
	/// Bank containing tile data table.
	/// </summary>
	public const int Bank = 0x08;

	/// <summary>
	/// CPU address of tile data table.
	/// </summary>
	public const int TableAddress = 0xA80D;

	/// <summary>
	/// Size of each entry in bytes.
	/// </summary>
	public const int EntrySize = 3;

	/// <summary>
	/// Physical tile number (10 bits: 8 low + 2 high).
	/// </summary>
	public ushort PhysicalTileNumber { get; set; }

	/// <summary>
	/// Tile increment pattern (4 bits).
	/// References table at $A1BB.
	/// </summary>
	public byte IncrementPattern { get; set; }

	/// <summary>
	/// Graphics page (2 bits).
	/// </summary>
	public byte GraphicsPage { get; set; }

	/// <summary>
	/// Tile behavior byte.
	/// </summary>
	public byte Behavior { get; set; }

	/// <summary>
	/// Parse tile data from raw bytes.
	/// Byte 0: Low byte of physical tile number
	/// Byte 1: AAAABBCC (AAAA=pattern, BB=page, CC=tile high)
	/// Byte 2: Tile Behavior
	/// </summary>
	public static TileData Parse(byte byte0, byte byte1, byte byte2) {
		return new TileData {
			PhysicalTileNumber = (ushort)(byte0 | ((byte1 & 0x03) << 8)),
			IncrementPattern = (byte)((byte1 >> 4) & 0x0F),
			GraphicsPage = (byte)((byte1 >> 2) & 0x03),
			Behavior = byte2
		};
	}
}

/// <summary>
/// Tileset data (64 bytes per tileset, 32 tiles).
/// Located at Bank $08, $8ADB-$979A (51 tilesets).
/// </summary>
public class Tileset {
	/// <summary>
	/// Bank containing tileset data.
	/// </summary>
	public const int Bank = 0x08;

	/// <summary>
	/// CPU address of tileset data start.
	/// </summary>
	public const int TableAddress = 0x8ADB;

	/// <summary>
	/// Size of each tileset in bytes.
	/// </summary>
	public const int TilesetSize = 64;

	/// <summary>
	/// Number of tiles per tileset.
	/// </summary>
	public const int TilesPerTileset = 32;

	/// <summary>
	/// Total number of tilesets.
	/// </summary>
	public const int TilesetCount = 51;

	/// <summary>
	/// Tileset index.
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// Tiles in this tileset (32 entries).
	/// </summary>
	public Tile[] Tiles { get; set; } = new Tile[TilesPerTileset];

	/// <summary>
	/// Parse tileset from raw bytes.
	/// </summary>
	public static Tileset Parse(byte[] data, int offset, int index) {
		var tileset = new Tileset { Index = index };

		for (int i = 0; i < TilesPerTileset; i++) {
			int tileOffset = offset + (i * 2);
			tileset.Tiles[i] = Tile.Parse(data[tileOffset], data[tileOffset + 1], index, i);
		}

		return tileset;
	}
}

/// <summary>
/// Tile increment pattern (3 bytes each).
/// Located at Bank $08, $A1BB (14 patterns).
/// </summary>
public class TileIncrementPattern {
	/// <summary>
	/// Bank containing pattern data.
	/// </summary>
	public const int Bank = 0x08;

	/// <summary>
	/// CPU address of pattern table.
	/// </summary>
	public const int TableAddress = 0xA1BB;

	/// <summary>
	/// Number of patterns.
	/// </summary>
	public const int PatternCount = 14;

	/// <summary>
	/// Pattern index.
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// Three signed displacement values from previous tile.
	/// </summary>
	public sbyte[] Displacements { get; set; } = new sbyte[3];

	/// <summary>
	/// Parse pattern from raw bytes.
	/// </summary>
	public static TileIncrementPattern Parse(byte[] data, int offset, int index) {
		return new TileIncrementPattern {
			Index = index,
			Displacements = [
				(sbyte)data[offset],
				(sbyte)data[offset + 1],
				(sbyte)data[offset + 2]
			]
		};
	}

	/// <summary>
	/// Standard increment patterns (from ROM).
	/// </summary>
	public static readonly TileIncrementPattern[] StandardPatterns = [
		new() { Index = 0x00, Displacements = [0x01, 0x01, 0x01] },
		new() { Index = 0x01, Displacements = [0x00, 0x00, 0x00] },
		new() { Index = 0x02, Displacements = [0x00, 0x01, 0x00] },
		new() { Index = 0x03, Displacements = [0x01, 0x00, -1] },
		new() { Index = 0x04, Displacements = [0x01, -1, 0x01] },
		new() { Index = 0x05, Displacements = [0x01, 0x01, 0x00] },
		new() { Index = 0x06, Displacements = [0x00, 0x01, 0x01] },
		new() { Index = 0x07, Displacements = [0x00, 0x02, -1] },
		new() { Index = 0x08, Displacements = [0x01, 0x01, 0x01] },
		new() { Index = 0x09, Displacements = [0x00, 0x01, 0x01] },
		new() { Index = 0x0A, Displacements = [0x03, 0x01, 0x01] },
		new() { Index = 0x0B, Displacements = [0x01, 0x03, 0x01] },
		new() { Index = 0x0C, Displacements = [-1, 0x05, -1] },
		new() { Index = 0x0D, Displacements = [-1, 0x03, -1] },
	];
}

/// <summary>
/// Tile behavior flags.
/// </summary>
[Flags]
public enum TileBehavior : byte {
	None = 0x00,
	Passable = 0x01,
	Damage = 0x02,
	Barrier = 0x04,
	Warp = 0x08,
	Encounter = 0x10,
	Water = 0x20,
	Stairs = 0x40,
	Door = 0x80
}
