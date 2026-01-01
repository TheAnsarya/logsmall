using DW4Lib.Converters;
using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;

namespace DW4Lib.Maps;

/// <summary>
/// Chapter 1 map definitions and data.
/// Contains all map layouts, events, NPCs, and treasures for Chapter 1 (Ragnar's story).
/// </summary>
public static class Chapter1Maps {
	// ============================================================
	// Map IDs
	// ============================================================

	/// <summary>Burland Castle - Main floor (map ID from MapDatabase).</summary>
	public const int MapBurlandMain = 0x02;

	/// <summary>Burland Castle - Throne room.</summary>
	public const int MapBurlandThrone = 0x02;

	/// <summary>Burland Castle - Basement.</summary>
	public const int MapBurlandBasement = 0x02;

	/// <summary>Burland Town - Outside castle.</summary>
	public const int MapBurlandTown = 0x02;

	/// <summary>Izmit Village.</summary>
	public const int MapIzmit = 0x0E;

	/// <summary>Strathbaile (children's village).</summary>
	public const int MapStrathbaile = 0x0F;

	/// <summary>Well dungeon (where Healie is found).</summary>
	public const int MapWellDungeon = 0x10;

	/// <summary>Loch Tower - Base.</summary>
	public const int MapLochTowerBase = 0x11;

	/// <summary>Loch Tower - Mid floors.</summary>
	public const int MapLochTowerMid = 0x12;

	/// <summary>Loch Tower - Top floor (boss).</summary>
	public const int MapLochTowerTop = 0x13;

	/// <summary>Chapter 1 overworld region.</summary>
	public const int MapChapter1Overworld = 0x00;

	// ============================================================
	// Tileset IDs
	// ============================================================

	/// <summary>Castle interior tileset.</summary>
	public const byte TilesetCastle = 0x01;

	/// <summary>Town/village tileset.</summary>
	public const byte TilesetTown = 0x02;

	/// <summary>Dungeon tileset.</summary>
	public const byte TilesetDungeon = 0x03;

	/// <summary>Tower tileset.</summary>
	public const byte TilesetTower = 0x04;

	/// <summary>Cave/underground tileset.</summary>
	public const byte TilesetCave = 0x05;

	// ============================================================
	// Map Metadata
	// ============================================================

	/// <summary>
	/// Get all Chapter 1 map metadata.
	/// </summary>
	public static MapMetadata[] GetAllMaps() => [
		new() {
			MapId = MapBurlandMain,
			Name = "Burland Castle",
			Bank = 0x09,
			SubmapCount = 4,
			Type = MapType.Castle,
			Chapters = [0, 4]
		},
		new() {
			MapId = MapIzmit,
			Name = "Izmit Village",
			Bank = 0x09,
			SubmapCount = 1,
			Type = MapType.Town,
			Chapters = [0]
		},
		new() {
			MapId = MapStrathbaile,
			Name = "Strathbaile",
			Bank = 0x09,
			SubmapCount = 2,
			Type = MapType.Town,
			Chapters = [0]
		},
		new() {
			MapId = MapWellDungeon,
			Name = "Well Dungeon",
			Bank = 0x0A,
			SubmapCount = 2,
			Type = MapType.Cave,
			Chapters = [0]
		},
		new() {
			MapId = MapLochTowerBase,
			Name = "Loch Tower - Base",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Tower,
			Chapters = [0]
		},
		new() {
			MapId = MapLochTowerMid,
			Name = "Loch Tower - Middle",
			Bank = 0x0A,
			SubmapCount = 3,
			Type = MapType.Tower,
			Chapters = [0]
		},
		new() {
			MapId = MapLochTowerTop,
			Name = "Loch Tower - Top",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Tower,
			Chapters = [0]
		}
	];

	// ============================================================
	// Treasure Chests
	// ============================================================

	/// <summary>
	/// Get all Chapter 1 treasure chests.
	/// </summary>
	public static TreasureChest[] GetAllTreasures() => [
		// Burland Castle
		new() { Index = 0, MapId = MapBurlandMain, SubmapIndex = 0, X = 5, Y = 3, ContentsType = TreasureType.Gold, ContentsValue = 50 },
		new() { Index = 1, MapId = MapBurlandMain, SubmapIndex = 1, X = 12, Y = 8, ContentsType = TreasureType.Item, ContentsValue = 0x01 }, // Medical Herb
		new() { Index = 2, MapId = MapBurlandMain, SubmapIndex = 2, X = 3, Y = 5, ContentsType = TreasureType.Item, ContentsValue = 0x02 }, // Antidote

		// Izmit Village
		new() { Index = 3, MapId = MapIzmit, SubmapIndex = 0, X = 8, Y = 10, ContentsType = TreasureType.Gold, ContentsValue = 30 },

		// Strathbaile
		new() { Index = 4, MapId = MapStrathbaile, SubmapIndex = 0, X = 6, Y = 4, ContentsType = TreasureType.Item, ContentsValue = 0x03 }, // Chimaera Wing

		// Well Dungeon
		new() { Index = 5, MapId = MapWellDungeon, SubmapIndex = 0, X = 10, Y = 5, ContentsType = TreasureType.Gold, ContentsValue = 100 },
		new() { Index = 6, MapId = MapWellDungeon, SubmapIndex = 1, X = 3, Y = 8, ContentsType = TreasureType.Item, ContentsValue = 0x0A }, // Copper Sword

		// Loch Tower
		new() { Index = 7, MapId = MapLochTowerBase, SubmapIndex = 0, X = 7, Y = 3, ContentsType = TreasureType.Gold, ContentsValue = 200 },
		new() { Index = 8, MapId = MapLochTowerMid, SubmapIndex = 0, X = 5, Y = 5, ContentsType = TreasureType.Item, ContentsValue = 0x15 }, // Scale Armor
		new() { Index = 9, MapId = MapLochTowerMid, SubmapIndex = 1, X = 12, Y = 2, ContentsType = TreasureType.Item, ContentsValue = 0x20 }, // Iron Shield
		new() { Index = 10, MapId = MapLochTowerMid, SubmapIndex = 2, X = 3, Y = 10, ContentsType = TreasureType.SmallMedal, ContentsValue = 1 },
		new() { Index = 11, MapId = MapLochTowerTop, SubmapIndex = 0, X = 8, Y = 8, ContentsType = TreasureType.Item, ContentsValue = 0x2A } // Flying Shoes
	];

	// ============================================================
	// Warp Points
	// ============================================================

	/// <summary>
	/// Get all Chapter 1 warp points.
	/// </summary>
	public static WarpPoint[] GetAllWarps() => [
		// Burland Castle entrances/exits
		new() {
			SourceMapId = MapBurlandMain, SourceSubmapIndex = 0, SourceX = 8, SourceY = 15,
			DestMapId = MapChapter1Overworld, DestSubmapIndex = 0, DestX = 100, DestY = 50,
			Type = WarpType.Exit
		},
		new() {
			SourceMapId = MapBurlandMain, SourceSubmapIndex = 0, SourceX = 5, SourceY = 0,
			DestMapId = MapBurlandMain, DestSubmapIndex = 1, DestX = 5, DestY = 14,
			Type = WarpType.StairsUp
		},

		// Well Dungeon
		new() {
			SourceMapId = MapStrathbaile, SourceSubmapIndex = 0, SourceX = 10, SourceY = 5,
			DestMapId = MapWellDungeon, DestSubmapIndex = 0, DestX = 8, DestY = 2,
			Type = WarpType.StairsDown
		},
		new() {
			SourceMapId = MapWellDungeon, SourceSubmapIndex = 0, SourceX = 3, SourceY = 10,
			DestMapId = MapWellDungeon, DestSubmapIndex = 1, DestX = 3, DestY = 2,
			Type = WarpType.StairsDown
		},

		// Loch Tower
		new() {
			SourceMapId = MapLochTowerBase, SourceSubmapIndex = 0, SourceX = 8, SourceY = 1,
			DestMapId = MapLochTowerMid, DestSubmapIndex = 0, DestX = 8, DestY = 14,
			Type = WarpType.StairsUp
		},
		new() {
			SourceMapId = MapLochTowerMid, SourceSubmapIndex = 0, SourceX = 12, SourceY = 0,
			DestMapId = MapLochTowerMid, DestSubmapIndex = 1, DestX = 3, DestY = 14,
			Type = WarpType.StairsUp
		},
		new() {
			SourceMapId = MapLochTowerMid, SourceSubmapIndex = 1, SourceX = 8, SourceY = 0,
			DestMapId = MapLochTowerMid, DestSubmapIndex = 2, DestX = 8, DestY = 14,
			Type = WarpType.StairsUp
		},
		new() {
			SourceMapId = MapLochTowerMid, SourceSubmapIndex = 2, SourceX = 5, SourceY = 0,
			DestMapId = MapLochTowerTop, DestSubmapIndex = 0, DestX = 8, DestY = 14,
			Type = WarpType.StairsUp
		}
	];

	// ============================================================
	// NPCs
	// ============================================================

	/// <summary>
	/// Get all Chapter 1 NPCs.
	/// </summary>
	public static NpcData[] GetAllNpcs() => [
		// Burland Castle
		new() {
			Index = 0, MapId = MapBurlandMain, X = 8, Y = 3, SpriteId = 0x10,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0001,
			Flags = NpcFlags.None, Name = "King of Burland"
		},
		new() {
			Index = 1, MapId = MapBurlandMain, X = 6, Y = 5, SpriteId = 0x11,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0002,
			Flags = NpcFlags.None, Name = "Chancellor"
		},
		new() {
			Index = 2, MapId = MapBurlandMain, X = 10, Y = 5, SpriteId = 0x12,
			Movement = NpcMovement.Random, Facing = 0, DialogId = 0x0003,
			Flags = NpcFlags.None, Name = "Guard"
		},
		new() {
			Index = 3, MapId = MapBurlandMain, X = 3, Y = 10, SpriteId = 0x20,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0010,
			Flags = NpcFlags.Shop, Name = "Weapon Shop"
		},
		new() {
			Index = 4, MapId = MapBurlandMain, X = 13, Y = 10, SpriteId = 0x21,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0011,
			Flags = NpcFlags.Shop, Name = "Armor Shop"
		},
		new() {
			Index = 5, MapId = MapBurlandMain, X = 8, Y = 12, SpriteId = 0x22,
			Movement = NpcMovement.Stationary, Facing = 0, DialogId = 0x0012,
			Flags = NpcFlags.Inn, Name = "Innkeeper"
		},
		new() {
			Index = 6, MapId = MapBurlandMain, X = 5, Y = 8, SpriteId = 0x23,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0013,
			Flags = NpcFlags.Church, Name = "Priest"
		},

		// Strathbaile
		new() {
			Index = 10, MapId = MapStrathbaile, X = 5, Y = 8, SpriteId = 0x30,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0020,
			Flags = NpcFlags.None, Name = "Village Elder"
		},
		new() {
			Index = 11, MapId = MapStrathbaile, X = 10, Y = 6, SpriteId = 0x31,
			Movement = NpcMovement.Random, Facing = 0, DialogId = 0x0021,
			Flags = NpcFlags.None, Name = "Worried Mother"
		},

		// Well Dungeon - Healie
		new() {
			Index = 20, MapId = MapWellDungeon, X = 5, Y = 5, SpriteId = 0xC5,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0030,
			Flags = NpcFlags.Recruitable | NpcFlags.ScriptTrigger, Name = "Healie"
		},

		// Loch Tower - Boss
		new() {
			Index = 30, MapId = MapLochTowerTop, X = 8, Y = 3, SpriteId = 0x80,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0040,
			Flags = NpcFlags.Boss | NpcFlags.ScriptTrigger, Name = "Saro's Shadow"
		}
	];

	// ============================================================
	// Encounter Zones
	// ============================================================

	/// <summary>
	/// Get all Chapter 1 encounter zones.
	/// </summary>
	public static EncounterZone[] GetAllEncounterZones() => [
		// Chapter 1 Overworld - weak monsters
		new() {
			Index = 0, MapId = MapChapter1Overworld, EncounterRate = 10,
			MonsterGroups = [0x01, 0x02, 0x03] // Slimes, etc.
		},

		// Well Dungeon
		new() {
			Index = 1, MapId = MapWellDungeon, EncounterRate = 15,
			MonsterGroups = [0x04, 0x05, 0x06] // Bats, rats
		},

		// Loch Tower - Base
		new() {
			Index = 2, MapId = MapLochTowerBase, EncounterRate = 20,
			MonsterGroups = [0x07, 0x08, 0x09] // Stronger monsters
		},

		// Loch Tower - Mid
		new() {
			Index = 3, MapId = MapLochTowerMid, EncounterRate = 20,
			MonsterGroups = [0x0A, 0x0B, 0x0C]
		},

		// Loch Tower - Top (no random encounters, just boss)
		new() {
			Index = 4, MapId = MapLochTowerTop, EncounterRate = 0,
			MonsterGroups = []
		}
	];

	// ============================================================
	// Entrance Locations
	// ============================================================

	/// <summary>
	/// Get all Chapter 1 overworld entrance locations.
	/// </summary>
	public static EntranceLocation[] GetAllEntrances() => [
		new() { Name = "Burland", Overworld = OverworldType.Main, X = 100, Y = 50, DestMapId = MapBurlandMain, Type = MapType.Castle },
		new() { Name = "Izmit", Overworld = OverworldType.Main, X = 90, Y = 60, DestMapId = MapIzmit, Type = MapType.Town },
		new() { Name = "Strathbaile", Overworld = OverworldType.Main, X = 110, Y = 45, DestMapId = MapStrathbaile, Type = MapType.Town },
		new() { Name = "Loch Tower", Overworld = OverworldType.Main, X = 120, Y = 40, DestMapId = MapLochTowerBase, Type = MapType.Tower }
	];

	// ============================================================
	// Map Conversion
	// ============================================================

	/// <summary>
	/// Get DQ3r map ID mapping for Chapter 1.
	/// </summary>
	public static Dictionary<int, int> GetDQ3rMapIdMapping() => new() {
		{ MapBurlandMain, 0x202 },
		{ MapIzmit, 0x20E },
		{ MapStrathbaile, 0x20F },
		{ MapWellDungeon, 0x210 },
		{ MapLochTowerBase, 0x211 },
		{ MapLochTowerMid, 0x212 },
		{ MapLochTowerTop, 0x213 }
	};

	/// <summary>
	/// Convert all Chapter 1 treasures to DQ3r format.
	/// </summary>
	public static DQ3rTreasure[] ConvertTreasures() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllTreasures()
			.Select(t => MapToDQ3r.ConvertTreasure(t))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 1 warps to DQ3r format.
	/// </summary>
	public static DQ3rWarp[] ConvertWarps() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllWarps()
			.Select(w => MapToDQ3r.ConvertWarp(w, mapping))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 1 NPCs to DQ3r format.
	/// </summary>
	public static DQ3rNpc[] ConvertNpcs() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllNpcs()
			.Select(n => MapToDQ3r.ConvertNpc(n, MapToDQ3r.MapIdToDQ3r(n.MapId, mapping)))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 1 encounter zones to DQ3r format.
	/// </summary>
	public static DQ3rEncounterZone[] ConvertEncounterZones() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllEncounterZones()
			.Select(z => MapToDQ3r.ConvertEncounterZone(z, mapping))
			.ToArray();
	}
}
