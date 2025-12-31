namespace DW4Lib.DQ3r.Maps;

/// <summary>
/// DQ3r event and NPC data structures.
/// </summary>
public class DQ3rMapEvent {
	/// <summary>
	/// Event type.
	/// </summary>
	public DQ3rEventType Type { get; set; }

	/// <summary>
	/// X coordinate.
	/// </summary>
	public ushort X { get; set; }

	/// <summary>
	/// Y coordinate.
	/// </summary>
	public ushort Y { get; set; }

	/// <summary>
	/// Event-specific data.
	/// </summary>
	public byte[] Data { get; set; } = [];

	/// <summary>
	/// Script ID (for scripted events).
	/// </summary>
	public ushort ScriptId { get; set; }
}

/// <summary>
/// DQ3r event types.
/// </summary>
public enum DQ3rEventType {
	None = 0,
	NPC = 1,
	Treasure = 2,
	Door = 3,
	Warp = 4,
	Stairs = 5,
	Script = 6,
	Shop = 7,
	Inn = 8,
	Church = 9,
	Vault = 10,
	Vocation = 11,  // DQ3 specific - class change
	Monster = 12    // Boss/fixed encounter
}

/// <summary>
/// DQ3r treasure chest.
/// </summary>
public class DQ3rTreasure {
	/// <summary>
	/// Chest ID.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Map ID.
	/// </summary>
	public int MapId { get; set; }

	/// <summary>
	/// X coordinate.
	/// </summary>
	public ushort X { get; set; }

	/// <summary>
	/// Y coordinate.
	/// </summary>
	public ushort Y { get; set; }

	/// <summary>
	/// Contents type.
	/// </summary>
	public DQ3rTreasureType ContentsType { get; set; }

	/// <summary>
	/// Contents value (item ID, gold amount, etc.).
	/// </summary>
	public ushort ContentsValue { get; set; }
}

/// <summary>
/// DQ3r treasure types.
/// </summary>
public enum DQ3rTreasureType {
	Item = 0,
	Gold = 1,
	SmallMedal = 2,
	Empty = 3,
	Monster = 4,
	Event = 5  // Triggers script
}

/// <summary>
/// DQ3r warp/entrance data.
/// </summary>
public class DQ3rWarp {
	/// <summary>
	/// Source map ID.
	/// </summary>
	public int SourceMapId { get; set; }

	/// <summary>
	/// Source X coordinate.
	/// </summary>
	public ushort SourceX { get; set; }

	/// <summary>
	/// Source Y coordinate.
	/// </summary>
	public ushort SourceY { get; set; }

	/// <summary>
	/// Destination map ID.
	/// </summary>
	public int DestMapId { get; set; }

	/// <summary>
	/// Destination X coordinate.
	/// </summary>
	public ushort DestX { get; set; }

	/// <summary>
	/// Destination Y coordinate.
	/// </summary>
	public ushort DestY { get; set; }

	/// <summary>
	/// Warp type.
	/// </summary>
	public DQ3rWarpType Type { get; set; }

	/// <summary>
	/// Facing direction after warp.
	/// </summary>
	public DQ3rDirection Facing { get; set; }
}

/// <summary>
/// DQ3r warp types.
/// </summary>
public enum DQ3rWarpType {
	StairsUp,
	StairsDown,
	Door,
	Exit,
	MapEdge,
	Teleport,
	Fall,
	TravelGate  // DQ3 specific
}

/// <summary>
/// DQ3r directions.
/// </summary>
public enum DQ3rDirection : byte {
	Down = 0,
	Left = 1,
	Right = 2,
	Up = 3
}

/// <summary>
/// DQ3r NPC data.
/// </summary>
public class DQ3rNpc {
	/// <summary>
	/// NPC index.
	/// </summary>
	public int Index { get; set; }

	/// <summary>
	/// Map ID.
	/// </summary>
	public int MapId { get; set; }

	/// <summary>
	/// X coordinate.
	/// </summary>
	public ushort X { get; set; }

	/// <summary>
	/// Y coordinate.
	/// </summary>
	public ushort Y { get; set; }

	/// <summary>
	/// Sprite ID.
	/// </summary>
	public ushort SpriteId { get; set; }

	/// <summary>
	/// Movement pattern.
	/// </summary>
	public DQ3rNpcMovement Movement { get; set; }

	/// <summary>
	/// Facing direction.
	/// </summary>
	public DQ3rDirection Facing { get; set; }

	/// <summary>
	/// Dialog script ID.
	/// </summary>
	public ushort DialogId { get; set; }

	/// <summary>
	/// NPC flags.
	/// </summary>
	public DQ3rNpcFlags Flags { get; set; }
}

/// <summary>
/// DQ3r NPC movement types.
/// </summary>
public enum DQ3rNpcMovement : byte {
	Stationary = 0,
	Random = 1,
	Pace = 2,
	Circle = 3,
	Follow = 4,
	LookAtPlayer = 5,
	Custom = 6
}

/// <summary>
/// DQ3r NPC flags.
/// </summary>
[Flags]
public enum DQ3rNpcFlags : ushort {
	None = 0x0000,
	Shop = 0x0001,
	Inn = 0x0002,
	Church = 0x0004,
	Vault = 0x0008,
	VocationShrine = 0x0010,  // Class change
	Recruitable = 0x0020,
	Boss = 0x0040,
	Invisible = 0x0080,
	ScriptTrigger = 0x0100,
	Persistent = 0x0200,  // Doesn't reset
	DayOnly = 0x0400,
	NightOnly = 0x0800
}

/// <summary>
/// DQ3r encounter zone.
/// </summary>
public class DQ3rEncounterZone {
	/// <summary>
	/// Zone ID.
	/// </summary>
	public int ZoneId { get; set; }

	/// <summary>
	/// Map ID (or world map region).
	/// </summary>
	public int MapId { get; set; }

	/// <summary>
	/// Encounter rate.
	/// </summary>
	public byte EncounterRate { get; set; }

	/// <summary>
	/// Monster group IDs.
	/// </summary>
	public ushort[] MonsterGroups { get; set; } = [];
}

/// <summary>
/// DQ3r world map entrance.
/// </summary>
public class DQ3rWorldEntrance {
	/// <summary>
	/// Location name.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// X coordinate on world map.
	/// </summary>
	public ushort X { get; set; }

	/// <summary>
	/// Y coordinate on world map.
	/// </summary>
	public ushort Y { get; set; }

	/// <summary>
	/// Destination map ID.
	/// </summary>
	public int DestMapId { get; set; }

	/// <summary>
	/// Destination X coordinate.
	/// </summary>
	public ushort DestX { get; set; }

	/// <summary>
	/// Destination Y coordinate.
	/// </summary>
	public ushort DestY { get; set; }

	/// <summary>
	/// Location type.
	/// </summary>
	public DQ3rLocationType Type { get; set; }

	/// <summary>
	/// Whether location appears on world map.
	/// </summary>
	public bool Visible { get; set; }
}

/// <summary>
/// DQ3r location types.
/// </summary>
public enum DQ3rLocationType {
	Town,
	Castle,
	Cave,
	Tower,
	Shrine,
	TravelGate,
	Other
}
