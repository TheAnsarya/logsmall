namespace DW4Lib.DataStructures.Maps;

/// <summary>
/// DW4 Event/NPC data structure.
/// Events include NPCs, treasure chests, doors, warps, and scripted triggers.
/// </summary>
public class MapEvent {
	/// <summary>
	/// Event type.
	/// </summary>
	public EventType Type { get; set; }

	/// <summary>
	/// X coordinate on map.
	/// </summary>
	public byte X { get; set; }

	/// <summary>
	/// Y coordinate on map.
	/// </summary>
	public byte Y { get; set; }

	/// <summary>
	/// Event-specific data (sprite ID, treasure contents, warp destination, etc.).
	/// </summary>
	public byte[] Data { get; set; } = [];
}

/// <summary>
/// Types of map events.
/// </summary>
public enum EventType {
	None = 0,
	NPC = 1,
	Treasure = 2,
	Door = 3,
	Warp = 4,
	StairsUp = 5,
	StairsDown = 6,
	Script = 7,
	Shop = 8,
	Inn = 9,
	Church = 10,
	Vault = 11
}

/// <summary>
/// Treasure chest data.
/// </summary>
public class TreasureChest {
	/// <summary>
	/// RAM address for treasure flags: $625D-$6277.
	/// </summary>
	public const int FlagsAddress = 0x625D;

	/// <summary>
	/// Total bytes for treasure flags.
	/// </summary>
	public const int FlagsSize = 0x1B; // 27 bytes = 216 chests

	/// <summary>
	/// Chest index (global).
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// Map ID where chest is located.
	/// </summary>
	public int MapId { get; set; }

	/// <summary>
	/// Submap index.
	/// </summary>
	public int SubmapIndex { get; set; }

	/// <summary>
	/// X coordinate.
	/// </summary>
	public byte X { get; set; }

	/// <summary>
	/// Y coordinate.
	/// </summary>
	public byte Y { get; set; }

	/// <summary>
	/// Contents type.
	/// </summary>
	public TreasureType ContentsType { get; set; }

	/// <summary>
	/// Contents value (item ID, gold amount, etc.).
	/// </summary>
	public int ContentsValue { get; set; }
}

/// <summary>
/// Type of treasure contents.
/// </summary>
public enum TreasureType {
	Item = 0,
	Gold = 1,
	SmallMedal = 2,
	Empty = 3,
	Monster = 4
}

/// <summary>
/// Warp point data.
/// </summary>
public class WarpPoint {
	/// <summary>
	/// Source map ID.
	/// </summary>
	public int SourceMapId { get; set; }

	/// <summary>
	/// Source submap index.
	/// </summary>
	public int SourceSubmapIndex { get; set; }

	/// <summary>
	/// Source X coordinate.
	/// </summary>
	public byte SourceX { get; set; }

	/// <summary>
	/// Source Y coordinate.
	/// </summary>
	public byte SourceY { get; set; }

	/// <summary>
	/// Destination map ID.
	/// </summary>
	public int DestMapId { get; set; }

	/// <summary>
	/// Destination submap index.
	/// </summary>
	public int DestSubmapIndex { get; set; }

	/// <summary>
	/// Destination X coordinate.
	/// </summary>
	public byte DestX { get; set; }

	/// <summary>
	/// Destination Y coordinate.
	/// </summary>
	public byte DestY { get; set; }

	/// <summary>
	/// Warp type (stairs, door, edge, etc.).
	/// </summary>
	public WarpType Type { get; set; }
}

/// <summary>
/// Type of warp transition.
/// </summary>
public enum WarpType {
	StairsUp,
	StairsDown,
	Door,
	Exit,
	MapEdge,
	Teleport,
	Fall
}

/// <summary>
/// Overworld entrance location.
/// </summary>
public class EntranceLocation {
	/// <summary>
	/// Location name.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Overworld type (main, Gottside, underworld).
	/// </summary>
	public OverworldType Overworld { get; set; }

	/// <summary>
	/// X coordinate on overworld.
	/// </summary>
	public byte X { get; set; }

	/// <summary>
	/// Y coordinate on overworld.
	/// </summary>
	public byte Y { get; set; }

	/// <summary>
	/// Destination map ID.
	/// </summary>
	public int DestMapId { get; set; }

	/// <summary>
	/// Destination submap index.
	/// </summary>
	public int DestSubmapIndex { get; set; }

	/// <summary>
	/// Entrance type (town, castle, cave, shrine, etc.).
	/// </summary>
	public MapType Type { get; set; }

	/// <summary>
	/// Chapter availability (null = all chapters).
	/// </summary>
	public int[]? AvailableChapters { get; set; }
}

/// <summary>
/// NPC (Non-Player Character) data.
/// </summary>
public class NpcData {
	/// <summary>
	/// NPC index within map.
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// X coordinate.
	/// </summary>
	public byte X { get; set; }

	/// <summary>
	/// Y coordinate.
	/// </summary>
	public byte Y { get; set; }

	/// <summary>
	/// Sprite ID.
	/// </summary>
	public byte SpriteId { get; set; }

	/// <summary>
	/// Movement pattern.
	/// </summary>
	public NpcMovement Movement { get; set; }

	/// <summary>
	/// Facing direction.
	/// </summary>
	public Direction Facing { get; set; }

	/// <summary>
	/// Dialog ID or script pointer.
	/// </summary>
	public ushort DialogId { get; set; }

	/// <summary>
	/// NPC flags (shop, inn, church, recruitable, etc.).
	/// </summary>
	public NpcFlags Flags { get; set; }
}

/// <summary>
/// NPC movement patterns.
/// </summary>
public enum NpcMovement : byte {
	Stationary = 0,
	Random = 1,
	Pace = 2,
	Circle = 3,
	Follow = 4,
	Custom = 5
}

/// <summary>
/// Cardinal directions.
/// </summary>
public enum Direction : byte {
	Down = 0,
	Left = 1,
	Right = 2,
	Up = 3
}

/// <summary>
/// NPC behavior flags.
/// </summary>
[Flags]
public enum NpcFlags : byte {
	None = 0x00,
	Shop = 0x01,
	Inn = 0x02,
	Church = 0x04,
	Vault = 0x08,
	Recruitable = 0x10,
	Boss = 0x20,
	Invisible = 0x40,
	ScriptTrigger = 0x80
}
