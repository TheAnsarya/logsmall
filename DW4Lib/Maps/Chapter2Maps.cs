using DW4Lib.Converters;
using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;

namespace DW4Lib.Maps;

/// <summary>
/// Chapter 2 map definitions and data.
/// Contains all map layouts, events, NPCs, and treasures for Chapter 2 (Alena's story).
/// </summary>
public static class Chapter2Maps {
	// ============================================================
	// Map IDs
	// ============================================================

	/// <summary>Santeem Castle - Main floor.</summary>
	public const int MapSanteemCastle = 0x20;

	/// <summary>Santeem Castle - Basement (secret escape route).</summary>
	public const int MapSanteemBasement = 0x21;

	/// <summary>Santeem Castle - Tower.</summary>
	public const int MapSanteemTower = 0x22;

	/// <summary>Surene Village.</summary>
	public const int MapSurene = 0x23;

	/// <summary>Tempe Village.</summary>
	public const int MapTempe = 0x24;

	/// <summary>Tempe Cave - Floor 1.</summary>
	public const int MapTempeCave1 = 0x25;

	/// <summary>Tempe Cave - Floor 2.</summary>
	public const int MapTempeCave2 = 0x26;

	/// <summary>Frenor Town.</summary>
	public const int MapFrenor = 0x27;

	/// <summary>Endor Town.</summary>
	public const int MapEndor = 0x28;

	/// <summary>Endor Colosseum.</summary>
	public const int MapEndorColosseum = 0x29;

	/// <summary>Endor Castle.</summary>
	public const int MapEndorCastle = 0x2A;

	/// <summary>Chapter 2 overworld region.</summary>
	public const int MapChapter2Overworld = 0x00;

	// ============================================================
	// Tileset IDs
	// ============================================================

	/// <summary>Castle interior tileset.</summary>
	public const byte TilesetCastle = 0x01;

	/// <summary>Town/village tileset.</summary>
	public const byte TilesetTown = 0x02;

	/// <summary>Dungeon/cave tileset.</summary>
	public const byte TilesetDungeon = 0x03;

	/// <summary>Arena tileset.</summary>
	public const byte TilesetArena = 0x06;

	// ============================================================
	// Map Metadata
	// ============================================================

	/// <summary>
	/// Get all Chapter 2 map metadata.
	/// </summary>
	public static MapMetadata[] GetAllMaps() => [
		new() {
			MapId = MapSanteemCastle,
			Name = "Santeem Castle",
			Bank = 0x0A,
			SubmapCount = 4,
			Type = MapType.Castle,
			Chapters = [1, 4]
		},
		new() {
			MapId = MapSanteemBasement,
			Name = "Santeem Basement",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Dungeon,
			Chapters = [1]
		},
		new() {
			MapId = MapSanteemTower,
			Name = "Santeem Tower",
			Bank = 0x0A,
			SubmapCount = 2,
			Type = MapType.Tower,
			Chapters = [1, 4]
		},
		new() {
			MapId = MapSurene,
			Name = "Surene",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Town,
			Chapters = [1]
		},
		new() {
			MapId = MapTempe,
			Name = "Tempe",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Town,
			Chapters = [1]
		},
		new() {
			MapId = MapTempeCave1,
			Name = "Tempe Cave B1",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Cave,
			Chapters = [1]
		},
		new() {
			MapId = MapTempeCave2,
			Name = "Tempe Cave B2",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Cave,
			Chapters = [1]
		},
		new() {
			MapId = MapFrenor,
			Name = "Frenor",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Town,
			Chapters = [1, 4]
		},
		new() {
			MapId = MapEndor,
			Name = "Endor",
			Bank = 0x0A,
			SubmapCount = 2,
			Type = MapType.Town,
			Chapters = [1, 2, 3, 4]
		},
		new() {
			MapId = MapEndorColosseum,
			Name = "Endor Colosseum",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Other,
			Chapters = [1, 4]
		},
		new() {
			MapId = MapEndorCastle,
			Name = "Endor Castle",
			Bank = 0x0A,
			SubmapCount = 3,
			Type = MapType.Castle,
			Chapters = [1, 2, 3, 4]
		}
	];

	// ============================================================
	// Treasure Chests
	// ============================================================

	/// <summary>
	/// Get all Chapter 2 treasure chests.
	/// </summary>
	public static TreasureChest[] GetAllTreasures() => [
		// Santeem Castle
		new() {
			Index = 50, MapId = MapSanteemCastle, SubmapIndex = 1, X = 5, Y = 10,
			ContentsType = TreasureType.Item, ContentsValue = 0x15 // Feather Hat
		},
		new() {
			Index = 51, MapId = MapSanteemCastle, SubmapIndex = 2, X = 27, Y = 8,
			ContentsType = TreasureType.Gold, ContentsValue = 120
		},
		// Santeem Basement
		new() {
			Index = 52, MapId = MapSanteemBasement, SubmapIndex = 0, X = 8, Y = 12,
			ContentsType = TreasureType.Item, ContentsValue = 0x0A // Medical Herb
		},
		new() {
			Index = 53, MapId = MapSanteemBasement, SubmapIndex = 0, X = 3, Y = 3,
			ContentsType = TreasureType.Item, ContentsValue = 0x20 // Scale Shield
		},
		// Tempe Cave
		new() {
			Index = 54, MapId = MapTempeCave1, SubmapIndex = 0, X = 18, Y = 5,
			ContentsType = TreasureType.Gold, ContentsValue = 80
		},
		new() {
			Index = 55, MapId = MapTempeCave1, SubmapIndex = 0, X = 6, Y = 20,
			ContentsType = TreasureType.Item, ContentsValue = 0x0B // Antidote Herb
		},
		new() {
			Index = 56, MapId = MapTempeCave2, SubmapIndex = 0, X = 12, Y = 12,
			ContentsType = TreasureType.Item, ContentsValue = 0x18 // Iron Claw
		},
		new() {
			Index = 57, MapId = MapTempeCave2, SubmapIndex = 0, X = 20, Y = 18,
			ContentsType = TreasureType.Gold, ContentsValue = 200
		},
		// Frenor
		new() {
			Index = 58, MapId = MapFrenor, SubmapIndex = 0, X = 15, Y = 8,
			ContentsType = TreasureType.Item, ContentsValue = 0x0C // Wing of Wyvern
		},
		// Endor
		new() {
			Index = 59, MapId = MapEndor, SubmapIndex = 0, X = 28, Y = 25,
			ContentsType = TreasureType.Gold, ContentsValue = 150
		},
		// Endor Castle
		new() {
			Index = 60, MapId = MapEndorCastle, SubmapIndex = 0, X = 10, Y = 5,
			ContentsType = TreasureType.SmallMedal, ContentsValue = 1
		},
		new() {
			Index = 61, MapId = MapEndorCastle, SubmapIndex = 1, X = 25, Y = 28,
			ContentsType = TreasureType.Gold, ContentsValue = 500
		}
	];

	// ============================================================
	// Warp Points
	// ============================================================

	/// <summary>
	/// Get all Chapter 2 warp points.
	/// </summary>
	public static WarpPoint[] GetAllWarps() => [
		// Santeem Castle ↔ Basement
		new() {
			SourceMapId = MapSanteemCastle, SourceSubmapIndex = 0, SourceX = 16, SourceY = 25,
			DestMapId = MapSanteemBasement, DestSubmapIndex = 0, DestX = 8, DestY = 2,
			Type = WarpType.StairsDown
		},
		new() {
			SourceMapId = MapSanteemBasement, SourceSubmapIndex = 0, SourceX = 8, SourceY = 1,
			DestMapId = MapSanteemCastle, DestSubmapIndex = 0, DestX = 16, DestY = 24,
			Type = WarpType.StairsUp
		},
		// Santeem Castle ↔ Tower
		new() {
			SourceMapId = MapSanteemCastle, SourceSubmapIndex = 1, SourceX = 8, SourceY = 5,
			DestMapId = MapSanteemTower, DestSubmapIndex = 0, DestX = 6, DestY = 10,
			Type = WarpType.StairsUp
		},
		new() {
			SourceMapId = MapSanteemTower, SourceSubmapIndex = 0, SourceX = 6, SourceY = 11,
			DestMapId = MapSanteemCastle, DestSubmapIndex = 1, DestX = 8, DestY = 6,
			Type = WarpType.StairsDown
		},
		// Tempe Cave floors
		new() {
			SourceMapId = MapTempeCave1, SourceSubmapIndex = 0, SourceX = 12, SourceY = 20,
			DestMapId = MapTempeCave2, DestSubmapIndex = 0, DestX = 12, DestY = 2,
			Type = WarpType.StairsDown
		},
		new() {
			SourceMapId = MapTempeCave2, SourceSubmapIndex = 0, SourceX = 12, SourceY = 1,
			DestMapId = MapTempeCave1, DestSubmapIndex = 0, DestX = 12, DestY = 19,
			Type = WarpType.StairsUp
		},
		// Endor ↔ Colosseum
		new() {
			SourceMapId = MapEndor, SourceSubmapIndex = 0, SourceX = 20, SourceY = 10,
			DestMapId = MapEndorColosseum, DestSubmapIndex = 0, DestX = 12, DestY = 22,
			Type = WarpType.Door
		},
		new() {
			SourceMapId = MapEndorColosseum, SourceSubmapIndex = 0, SourceX = 12, SourceY = 23,
			DestMapId = MapEndor, DestSubmapIndex = 0, DestX = 20, DestY = 11,
			Type = WarpType.Exit
		},
		// Endor ↔ Castle
		new() {
			SourceMapId = MapEndor, SourceSubmapIndex = 0, SourceX = 16, SourceY = 2,
			DestMapId = MapEndorCastle, DestSubmapIndex = 0, DestX = 16, DestY = 30,
			Type = WarpType.Door
		},
		new() {
			SourceMapId = MapEndorCastle, SourceSubmapIndex = 0, SourceX = 16, SourceY = 31,
			DestMapId = MapEndor, DestSubmapIndex = 0, DestX = 16, DestY = 3,
			Type = WarpType.Exit
		}
	];

	// ============================================================
	// NPCs
	// ============================================================

	/// <summary>
	/// Get all Chapter 2 NPCs.
	/// </summary>
	public static NpcData[] GetAllNpcs() => [
		// Santeem Castle NPCs
		new() {
			Index = 100, MapId = MapSanteemCastle, X = 16, Y = 5, SpriteId = 0x50,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0210,
			Flags = NpcFlags.None, Name = "King of Santeem"
		},
		new() {
			Index = 101, MapId = MapSanteemCastle, X = 10, Y = 15, SpriteId = 0x61,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0230,
			Flags = NpcFlags.Shop, Name = "Weapon Shopkeeper"
		},
		new() {
			Index = 102, MapId = MapSanteemCastle, X = 22, Y = 15, SpriteId = 0x62,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0233,
			Flags = NpcFlags.Inn, Name = "Castle Innkeeper"
		},
		new() {
			Index = 103, MapId = MapSanteemCastle, X = 16, Y = 20, SpriteId = 0x63,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0234,
			Flags = NpcFlags.Church, Name = "Castle Priest"
		},
		// Tempe NPCs
		new() {
			Index = 110, MapId = MapTempe, X = 10, Y = 8, SpriteId = 0x70,
			Movement = NpcMovement.Random, Facing = 0, DialogId = 0x0240,
			Flags = NpcFlags.None, Name = "Worried Child"
		},
		new() {
			Index = 111, MapId = MapTempe, X = 12, Y = 10, SpriteId = 0x71,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0245,
			Flags = NpcFlags.None, Name = "Tempe Elder"
		},
		// Endor NPCs
		new() {
			Index = 120, MapId = MapEndor, X = 20, Y = 12, SpriteId = 0x80,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0275,
			Flags = NpcFlags.None, Name = "Colosseum Guard"
		},
		new() {
			Index = 121, MapId = MapEndor, X = 8, Y = 20, SpriteId = 0x61,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0235,
			Flags = NpcFlags.Shop, Name = "Endor Weapon Shop"
		},
		new() {
			Index = 122, MapId = MapEndor, X = 24, Y = 20, SpriteId = 0x62,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0236,
			Flags = NpcFlags.Inn, Name = "Endor Innkeeper"
		},
		// Colosseum NPCs
		new() {
			Index = 130, MapId = MapEndorColosseum, X = 12, Y = 18, SpriteId = 0x81,
			Movement = NpcMovement.Stationary, Facing = 2, DialogId = 0x0280,
			Flags = NpcFlags.None, Name = "Tournament Official"
		}
	];

	// ============================================================
	// Encounter Zones
	// ============================================================

	/// <summary>
	/// Get all Chapter 2 encounter zones.
	/// </summary>
	public static EncounterZone[] GetAllEncounterZones() => [
		// Santeem region (outside castle)
		new() {
			MapId = MapChapter2Overworld,
			Index = 0x20,
			EncounterRate = 8,
			MonsterGroups = [0x10, 0x11, 0x12, 0x13]
		},
		// Tempe Cave B1
		new() {
			MapId = MapTempeCave1,
			Index = 0x21,
			EncounterRate = 10,
			MonsterGroups = [0x14, 0x15, 0x16, 0x17]
		},
		// Tempe Cave B2
		new() {
			MapId = MapTempeCave2,
			Index = 0x22,
			EncounterRate = 10,
			MonsterGroups = [0x18, 0x19, 0x1A, 0x1B]
		},
		// Frenor region
		new() {
			MapId = MapChapter2Overworld,
			Index = 0x23,
			EncounterRate = 8,
			MonsterGroups = [0x1C, 0x1D, 0x1E, 0x1F]
		},
		// Endor region
		new() {
			MapId = MapChapter2Overworld,
			Index = 0x24,
			EncounterRate = 7,
			MonsterGroups = [0x20, 0x21, 0x22, 0x23]
		}
	];

	// ============================================================
	// Entrances
	// ============================================================

	/// <summary>
	/// Get all Chapter 2 map entrances (from overworld).
	/// </summary>
	public static EntranceLocation[] GetAllEntrances() => [
		// Santeem Castle entrance
		new() {
			Name = "Santeem Castle",
			Overworld = OverworldType.Main,
			X = 50, Y = 80,
			DestMapId = MapSanteemCastle,
			DestSubmapIndex = 0,
			Type = MapType.Castle,
			AvailableChapters = [1, 4]
		},
		// Surene entrance
		new() {
			Name = "Surene",
			Overworld = OverworldType.Main,
			X = 45, Y = 70,
			DestMapId = MapSurene,
			DestSubmapIndex = 0,
			Type = MapType.Town,
			AvailableChapters = [1]
		},
		// Tempe entrance
		new() {
			Name = "Tempe",
			Overworld = OverworldType.Main,
			X = 60, Y = 75,
			DestMapId = MapTempe,
			DestSubmapIndex = 0,
			Type = MapType.Town,
			AvailableChapters = [1]
		},
		// Tempe Cave entrance
		new() {
			Name = "Tempe Cave",
			Overworld = OverworldType.Main,
			X = 65, Y = 70,
			DestMapId = MapTempeCave1,
			DestSubmapIndex = 0,
			Type = MapType.Cave,
			AvailableChapters = [1]
		},
		// Frenor entrance
		new() {
			Name = "Frenor",
			Overworld = OverworldType.Main,
			X = 80, Y = 60,
			DestMapId = MapFrenor,
			DestSubmapIndex = 0,
			Type = MapType.Town,
			AvailableChapters = [1, 4]
		},
		// Endor entrance
		new() {
			Name = "Endor",
			Overworld = OverworldType.Main,
			X = 100, Y = 50,
			DestMapId = MapEndor,
			DestSubmapIndex = 0,
			Type = MapType.Town,
			AvailableChapters = [1, 2, 3, 4]
		}
	];

	// ============================================================
	// Conversion Helpers
	// ============================================================

	/// <summary>
	/// Get DQ3r map ID mapping for Chapter 2.
	/// </summary>
	public static Dictionary<int, int> GetDQ3rMapIdMapping() => new() {
		{ MapSanteemCastle, 0x0120 },
		{ MapSanteemBasement, 0x0121 },
		{ MapSanteemTower, 0x0122 },
		{ MapSurene, 0x0123 },
		{ MapTempe, 0x0124 },
		{ MapTempeCave1, 0x0125 },
		{ MapTempeCave2, 0x0126 },
		{ MapFrenor, 0x0127 },
		{ MapEndor, 0x0128 },
		{ MapEndorColosseum, 0x0129 },
		{ MapEndorCastle, 0x012A }
	};

	/// <summary>
	/// Convert all Chapter 2 treasures to DQ3r format.
	/// </summary>
	public static DQ3rTreasure[] ConvertTreasures() {
		return GetAllTreasures()
			.Select(t => MapToDQ3r.ConvertTreasure(t))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 2 warps to DQ3r format.
	/// </summary>
	public static DQ3rWarp[] ConvertWarps() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllWarps()
			.Select(w => MapToDQ3r.ConvertWarp(w, mapping))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 2 NPCs to DQ3r format.
	/// </summary>
	public static DQ3rNpc[] ConvertNpcs() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllNpcs()
			.Select(n => MapToDQ3r.ConvertNpc(n, MapToDQ3r.MapIdToDQ3r(n.MapId, mapping)))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 2 encounter zones to DQ3r format.
	/// </summary>
	public static DQ3rEncounterZone[] ConvertEncounterZones() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllEncounterZones()
			.Select(z => MapToDQ3r.ConvertEncounterZone(z, mapping))
			.ToArray();
	}
}
