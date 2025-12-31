namespace DW4Lib.DataStructures.Maps;

/// <summary>
/// Monster encounter zone data.
/// Defines enemy groups and encounter rates for map regions.
/// </summary>
public class EncounterZone {
	/// <summary>
	/// Zone index.
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// Map ID this zone applies to (or 0xFF for overworld).
	/// </summary>
	public int MapId { get; set; }

	/// <summary>
	/// Submap index (0xFF for entire map).
	/// </summary>
	public int SubmapIndex { get; set; }

	/// <summary>
	/// Base encounter rate (higher = more frequent).
	/// </summary>
	public byte EncounterRate { get; set; }

	/// <summary>
	/// Monster group IDs that can appear.
	/// </summary>
	public byte[] MonsterGroups { get; set; } = [];
}

/// <summary>
/// Monster group composition.
/// Defines which monsters appear together in battle.
/// </summary>
public class MonsterGroup {
	/// <summary>
	/// Group index.
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// Monster IDs in this group.
	/// </summary>
	public byte[] MonsterIds { get; set; } = [];

	/// <summary>
	/// Count range for each monster (min, max pairs).
	/// </summary>
	public (byte Min, byte Max)[] CountRanges { get; set; } = [];
}

/// <summary>
/// Overworld encounter region.
/// Divides overworld into zones with different enemy groups.
/// </summary>
public class OverworldEncounterRegion {
	/// <summary>
	/// Region index.
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// Region boundaries (X1, Y1, X2, Y2).
	/// </summary>
	public (byte X1, byte Y1, byte X2, byte Y2) Bounds { get; set; }

	/// <summary>
	/// Encounter zone ID for land tiles.
	/// </summary>
	public byte LandZoneId { get; set; }

	/// <summary>
	/// Encounter zone ID for water tiles.
	/// </summary>
	public byte WaterZoneId { get; set; }

	/// <summary>
	/// Chapter restrictions (null = all chapters).
	/// </summary>
	public int[]? Chapters { get; set; }
}

/// <summary>
/// Shop data structure.
/// </summary>
public class ShopData {
	/// <summary>
	/// Shop index.
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// Map ID where shop is located.
	/// </summary>
	public int MapId { get; set; }

	/// <summary>
	/// Shop type.
	/// </summary>
	public ShopType Type { get; set; }

	/// <summary>
	/// Item IDs available for purchase.
	/// </summary>
	public byte[] Items { get; set; } = [];
}

/// <summary>
/// Type of shop.
/// </summary>
public enum ShopType {
	Weapon,
	Armor,
	Item,
	Mixed
}

/// <summary>
/// Complete map data combining all elements.
/// </summary>
public class MapData {
	/// <summary>
	/// Map metadata (ID, name, etc.).
	/// </summary>
	public MapMetadata Metadata { get; set; } = new();

	/// <summary>
	/// Submap info entries.
	/// </summary>
	public MapInfo[] Submaps { get; set; } = [];

	/// <summary>
	/// Tileset used by this map.
	/// </summary>
	public Tileset? Tileset { get; set; }

	/// <summary>
	/// Tile layout for each submap.
	/// </summary>
	public byte[][]? TileLayouts { get; set; }

	/// <summary>
	/// Map dimensions per submap (width, height pairs).
	/// </summary>
	public (int Width, int Height)[] Dimensions { get; set; } = [];

	/// <summary>
	/// Events (NPCs, chests, warps) per submap.
	/// </summary>
	public MapEvent[][]? Events { get; set; }

	/// <summary>
	/// Encounter zones for this map.
	/// </summary>
	public EncounterZone[]? EncounterZones { get; set; }

	/// <summary>
	/// Shops in this map.
	/// </summary>
	public ShopData[]? Shops { get; set; }
}
