namespace DW4Lib.DataStructures.Maps;

/// <summary>
/// Dragon Warrior IV NES Map Information (3 bytes per submap).
/// Located at Bank $17, $B121-$B4AE.
/// </summary>
public class MapInfo {
	/// <summary>
	/// Size of a single map info record in bytes.
	/// </summary>
	public const int Size = 3;

	/// <summary>
	/// ROM bank containing map pointer table.
	/// </summary>
	public const int PointerTableBank = 0x17;

	/// <summary>
	/// CPU address of map pointer table (73 entries, 2 bytes each).
	/// </summary>
	public const int PointerTableAddress = 0xB08D;

	/// <summary>
	/// CPU address of map information data start.
	/// </summary>
	public const int MapInfoDataAddress = 0xB121;

	/// <summary>
	/// Total number of maps in DW4.
	/// </summary>
	public const int TotalMaps = 73;

	/// <summary>
	/// Map ID (0x00 to 0x48).
	/// </summary>
	public int MapId { get; set; }

	/// <summary>
	/// Submap index within this map.
	/// </summary>
	public int SubmapIndex { get; set; }

	/// <summary>
	/// Tileset number (byte 0 of map info).
	/// References tileset data in Bank $08.
	/// </summary>
	public byte TilesetNumber { get; set; }

	/// <summary>
	/// Map data address in ROM (bytes 1-2, little-endian).
	/// </summary>
	public ushort MapDataAddress { get; set; }

	/// <summary>
	/// Bank number where map data is located.
	/// Derived from function at Bank $0F, $E9AD.
	/// </summary>
	public int DataBank { get; set; }

	/// <summary>
	/// Calculate file offset for map data.
	/// </summary>
	public int FileOffset => 0x10 + (DataBank * 0x4000) + (MapDataAddress - 0x8000);

	/// <summary>
	/// Parse map info from ROM bytes.
	/// </summary>
	public static MapInfo Parse(byte[] data, int offset, int mapId, int submapIndex, int dataBank) {
		return new MapInfo {
			MapId = mapId,
			SubmapIndex = submapIndex,
			TilesetNumber = data[offset],
			MapDataAddress = (ushort)(data[offset + 1] | (data[offset + 2] << 8)),
			DataBank = dataBank
		};
	}
}

/// <summary>
/// Map metadata including name and submap count.
/// </summary>
public class MapMetadata {
	/// <summary>
	/// Map ID (0x00 to 0x48).
	/// </summary>
	public int MapId { get; set; }

	/// <summary>
	/// Map name (e.g., "Burland", "Endor").
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Primary bank containing this map's data.
	/// </summary>
	public int Bank { get; set; }

	/// <summary>
	/// Number of submaps in this location.
	/// </summary>
	public int SubmapCount { get; set; }

	/// <summary>
	/// Map type (town, dungeon, castle, overworld, etc.).
	/// </summary>
	public MapType Type { get; set; }

	/// <summary>
	/// Chapter(s) this map appears in.
	/// </summary>
	public int[]? Chapters { get; set; }
}

/// <summary>
/// Type of map location.
/// </summary>
public enum MapType {
	Town,
	Castle,
	Dungeon,
	Tower,
	Cave,
	Shrine,
	Overworld,
	Other
}

/// <summary>
/// Static data for all DW4 maps.
/// </summary>
public static class MapDatabase {
	/// <summary>
	/// All 73 DW4 maps with metadata.
	/// </summary>
	public static readonly MapMetadata[] Maps = [
		new() { MapId = 0x00, Name = "Keeleon", Bank = 0x09, SubmapCount = 5, Type = MapType.Castle },
		new() { MapId = 0x01, Name = "Santeem", Bank = 0x09, SubmapCount = 3, Type = MapType.Castle, Chapters = [4, 5] },
		new() { MapId = 0x02, Name = "Burland", Bank = 0x09, SubmapCount = 4, Type = MapType.Castle, Chapters = [1, 5] },
		new() { MapId = 0x03, Name = "Dire Palace", Bank = 0x09, SubmapCount = 4, Type = MapType.Dungeon },
		new() { MapId = 0x04, Name = "Endor", Bank = 0x09, SubmapCount = 15, Type = MapType.Town, Chapters = [2, 3, 5] },
		new() { MapId = 0x05, Name = "Bonmalmo", Bank = 0x09, SubmapCount = 2, Type = MapType.Town },
		new() { MapId = 0x06, Name = "Branca", Bank = 0x09, SubmapCount = 4, Type = MapType.Town },
		new() { MapId = 0x07, Name = "Soretta", Bank = 0x09, SubmapCount = 2, Type = MapType.Town },
		new() { MapId = 0x08, Name = "Gardenbur", Bank = 0x09, SubmapCount = 5, Type = MapType.Castle },
		new() { MapId = 0x09, Name = "Stancia", Bank = 0x09, SubmapCount = 8, Type = MapType.Castle },
		new() { MapId = 0x0A, Name = "Aktemto Town", Bank = 0x09, SubmapCount = 1, Type = MapType.Town },
		new() { MapId = 0x0B, Name = "Riverton", Bank = 0x09, SubmapCount = 2, Type = MapType.Town },
		new() { MapId = 0x0C, Name = "Bazaar", Bank = 0x09, SubmapCount = 1, Type = MapType.Town },
		new() { MapId = 0x0D, Name = "Mintos", Bank = 0x09, SubmapCount = 3, Type = MapType.Town },
		new() { MapId = 0x0E, Name = "Tempe", Bank = 0x09, SubmapCount = 1, Type = MapType.Town },
		new() { MapId = 0x0F, Name = "Frenor", Bank = 0x09, SubmapCount = 2, Type = MapType.Town },
		new() { MapId = 0x10, Name = "Aneaux", Bank = 0x09, SubmapCount = 4, Type = MapType.Town },
		new() { MapId = 0x11, Name = "Haville", Bank = 0x09, SubmapCount = 6, Type = MapType.Town },
		new() { MapId = 0x12, Name = "Izmit", Bank = 0x09, SubmapCount = 3, Type = MapType.Town },
		new() { MapId = 0x13, Name = "Surene", Bank = 0x09, SubmapCount = 5, Type = MapType.Town },
		new() { MapId = 0x14, Name = "Hometown", Bank = 0x09, SubmapCount = 3, Type = MapType.Town, Chapters = [4, 5] },
		new() { MapId = 0x15, Name = "Monbaraba", Bank = 0x09, SubmapCount = 4, Type = MapType.Town },
		new() { MapId = 0x16, Name = "Lakanaba", Bank = 0x09, SubmapCount = 3, Type = MapType.Town },
		new() { MapId = 0x17, Name = "Kievs", Bank = 0x09, SubmapCount = 2, Type = MapType.Town },
		new() { MapId = 0x18, Name = "Foxville", Bank = 0x09, SubmapCount = 2, Type = MapType.Town },
		new() { MapId = 0x19, Name = "Seaside Village", Bank = 0x09, SubmapCount = 3, Type = MapType.Town },
		new() { MapId = 0x1A, Name = "Gottside", Bank = 0x09, SubmapCount = 2, Type = MapType.Town },
		new() { MapId = 0x1B, Name = "Rosaville", Bank = 0x09, SubmapCount = 5, Type = MapType.Town },
		new() { MapId = 0x1C, Name = "Secret Playground", Bank = 0x09, SubmapCount = 2, Type = MapType.Other },
		new() { MapId = 0x1D, Name = "House of Prophecy", Bank = 0x09, SubmapCount = 1, Type = MapType.Shrine },
		new() { MapId = 0x1E, Name = "Shrine to Endor", Bank = 0x09, SubmapCount = 1, Type = MapType.Shrine },
		new() { MapId = 0x1F, Name = "Inn Shrine", Bank = 0x09, SubmapCount = 1, Type = MapType.Shrine },
		new() { MapId = 0x20, Name = "Woodsman's Shack", Bank = 0x09, SubmapCount = 1, Type = MapType.Other },
		new() { MapId = 0x21, Name = "Desert Inn", Bank = 0x09, SubmapCount = 1, Type = MapType.Shrine },
		new() { MapId = 0x22, Name = "Small Medal King", Bank = 0x09, SubmapCount = 2, Type = MapType.Shrine },
		new() { MapId = 0x23, Name = "Colossus", Bank = 0x09, SubmapCount = 1, Type = MapType.Shrine },
		new() { MapId = 0x24, Name = "Lighthouse", Bank = 0x09, SubmapCount = 4, Type = MapType.Tower },
		new() { MapId = 0x25, Name = "Cave West of Kievs", Bank = 0x09, SubmapCount = 4, Type = MapType.Cave },
		new() { MapId = 0x26, Name = "Cave South of Lakanaba", Bank = 0x09, SubmapCount = 1, Type = MapType.Cave },
		new() { MapId = 0x27, Name = "Cave to Haville", Bank = 0x09, SubmapCount = 2, Type = MapType.Cave },
		new() { MapId = 0x28, Name = "Cave to Riverton", Bank = 0x09, SubmapCount = 2, Type = MapType.Cave },
		new() { MapId = 0x29, Name = "Cave North of Lakanaba", Bank = 0x09, SubmapCount = 2, Type = MapType.Cave },
		new() { MapId = 0x2A, Name = "Birdsong Tower", Bank = 0x09, SubmapCount = 4, Type = MapType.Tower },
		new() { MapId = 0x2B, Name = "Cascade Cave", Bank = 0x09, SubmapCount = 5, Type = MapType.Cave },
		new() { MapId = 0x2C, Name = "Silver Statuette Cave", Bank = 0x09, SubmapCount = 4, Type = MapType.Cave },
		new() { MapId = 0x2D, Name = "Aktemto Mine", Bank = 0x0A, SubmapCount = 10, Type = MapType.Dungeon },
		new() { MapId = 0x2E, Name = "Shrine of Breaking Waves", Bank = 0x0A, SubmapCount = 2, Type = MapType.Shrine },
		new() { MapId = 0x2F, Name = "Cave to Aktemto Mine", Bank = 0x0A, SubmapCount = 4, Type = MapType.Cave },
		new() { MapId = 0x30, Name = "Cave to Gardenbur", Bank = 0x0A, SubmapCount = 4, Type = MapType.Cave },
		new() { MapId = 0x31, Name = "Cave to Stancia", Bank = 0x0A, SubmapCount = 2, Type = MapType.Cave },
		new() { MapId = 0x32, Name = "Cave to Surene", Bank = 0x0A, SubmapCount = 2, Type = MapType.Cave },
		new() { MapId = 0x33, Name = "Royal Crypt", Bank = 0x0A, SubmapCount = 4, Type = MapType.Dungeon },
		new() { MapId = 0x34, Name = "Metal Babble Cave", Bank = 0x0A, SubmapCount = 2, Type = MapType.Cave },
		new() { MapId = 0x35, Name = "Cave to Izmit", Bank = 0x0A, SubmapCount = 2, Type = MapType.Cave },
		new() { MapId = 0x36, Name = "Cave to Konenber", Bank = 0x0A, SubmapCount = 3, Type = MapType.Cave },
		new() { MapId = 0x37, Name = "Cave to Branca", Bank = 0x0A, SubmapCount = 2, Type = MapType.Cave },
		new() { MapId = 0x38, Name = "Cave to Soretta", Bank = 0x0A, SubmapCount = 2, Type = MapType.Cave },
		new() { MapId = 0x39, Name = "Konenber", Bank = 0x0A, SubmapCount = 2, Type = MapType.Town },
		new() { MapId = 0x3A, Name = "Tower South of Keeleon", Bank = 0x0A, SubmapCount = 4, Type = MapType.Tower },
		new() { MapId = 0x3B, Name = "Esturk's Tomb", Bank = 0x0A, SubmapCount = 5, Type = MapType.Dungeon },
		new() { MapId = 0x3C, Name = "World Tree", Bank = 0x0A, SubmapCount = 3, Type = MapType.Dungeon },
		new() { MapId = 0x3D, Name = "Shrine SW of Endor", Bank = 0x0A, SubmapCount = 1, Type = MapType.Shrine },
		new() { MapId = 0x3E, Name = "Shrine East of Konenber", Bank = 0x0A, SubmapCount = 1, Type = MapType.Shrine },
		new() { MapId = 0x3F, Name = "Zenethia Castle", Bank = 0x0A, SubmapCount = 5, Type = MapType.Castle },
		new() { MapId = 0x40, Name = "Batten Tower", Bank = 0x0A, SubmapCount = 4, Type = MapType.Tower },
		new() { MapId = 0x41, Name = "Tower of Zenethia", Bank = 0x0A, SubmapCount = 5, Type = MapType.Tower },
		new() { MapId = 0x42, Name = "Cave Near Gottside", Bank = 0x0A, SubmapCount = 2, Type = MapType.Cave },
		new() { MapId = 0x43, Name = "Royal Crypt Entrance", Bank = 0x0A, SubmapCount = 2, Type = MapType.Dungeon },
		new() { MapId = 0x44, Name = "Cave of Betrayal", Bank = 0x0A, SubmapCount = 3, Type = MapType.Cave },
		new() { MapId = 0x45, Name = "Necrosaro's Palace", Bank = 0x0A, SubmapCount = 8, Type = MapType.Dungeon },
		new() { MapId = 0x46, Name = "Zenethia", Bank = 0x0B, SubmapCount = 4, Type = MapType.Town },
		new() { MapId = 0x47, Name = "Shrine of the Horn", Bank = 0x0B, SubmapCount = 2, Type = MapType.Shrine },
		new() { MapId = 0x48, Name = "Shrine of Colossus", Bank = 0x0B, SubmapCount = 10, Type = MapType.Shrine },
	];
}
