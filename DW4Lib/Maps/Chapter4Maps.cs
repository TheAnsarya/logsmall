using DW4Lib.Converters;
using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;
using DW4Lib.Events;

namespace DW4Lib.Maps;

/// <summary>
/// Chapter 4 map definitions and data.
/// Contains all map layouts, events, NPCs, and treasures for Chapter 4 (Meena and Maya's story).
/// </summary>
public static class Chapter4Maps {
	// ============================================================
	// Map IDs
	// ============================================================

	/// <summary>Monbaraba - Entertainment town.</summary>
	public const int MapMonbaraba = 0x40;

	/// <summary>Monbaraba - Theater building.</summary>
	public const int MapTheater = 0x41;

	/// <summary>Monbaraba - Sisters' room.</summary>
	public const int MapSistersRoom = 0x42;

	/// <summary>Cave of Monbaraba - Floor 1.</summary>
	public const int MapCaveMonbarabaF1 = 0x43;

	/// <summary>Cave of Monbaraba - Floor 2.</summary>
	public const int MapCaveMonbarabaF2 = 0x44;

	/// <summary>Haville - Mining town.</summary>
	public const int MapHaville = 0x45;

	/// <summary>Haville Mine - Upper levels.</summary>
	public const int MapMineUpper = 0x46;

	/// <summary>Haville Mine - Lower levels.</summary>
	public const int MapMineLower = 0x47;

	/// <summary>Kievs - Town.</summary>
	public const int MapKievs = 0x48;

	/// <summary>Kievs Castle - Main floor.</summary>
	public const int MapKievsCastleMain = 0x49;

	/// <summary>Kievs Castle - Throne room.</summary>
	public const int MapKievsCastleThrone = 0x4A;

	/// <summary>Kievs Castle - Secret passage.</summary>
	public const int MapKievsCastleSecret = 0x4B;

	/// <summary>Chapter 4 region overworld.</summary>
	public const int MapChapter4Overworld = 0x04;

	// ============================================================
	// Tileset IDs
	// ============================================================

	/// <summary>Town tileset.</summary>
	public const byte TilesetTown = 0x02;

	/// <summary>Theater/entertainment tileset.</summary>
	public const byte TilesetTheater = 0x40;

	/// <summary>Cave tileset.</summary>
	public const byte TilesetCave = 0x05;

	/// <summary>Mine tileset.</summary>
	public const byte TilesetMine = 0x41;

	/// <summary>Castle tileset.</summary>
	public const byte TilesetCastle = 0x01;

	// ============================================================
	// Map Metadata
	// ============================================================

	/// <summary>
	/// Get all Chapter 4 map metadata.
	/// </summary>
	public static MapMetadata[] GetAllMaps() => [
		new MapMetadata {
			MapId = MapMonbaraba,
			Name = "Monbaraba",
			Bank = 0x0D,
			SubmapCount = 3,
			Type = MapType.Town,
			Chapters = [3]
		},
		new MapMetadata {
			MapId = MapTheater,
			Name = "Theater",
			Bank = 0x0D,
			SubmapCount = 2,
			Type = MapType.Other,
			Chapters = [3]
		},
		new MapMetadata {
			MapId = MapSistersRoom,
			Name = "Sisters Room",
			Bank = 0x0D,
			SubmapCount = 1,
			Type = MapType.Other,
			Chapters = [3]
		},
		new MapMetadata {
			MapId = MapCaveMonbarabaF1,
			Name = "Cave of Monbaraba F1",
			Bank = 0x0D,
			SubmapCount = 2,
			Type = MapType.Cave,
			Chapters = [3]
		},
		new MapMetadata {
			MapId = MapCaveMonbarabaF2,
			Name = "Cave of Monbaraba F2",
			Bank = 0x0D,
			SubmapCount = 1,
			Type = MapType.Cave,
			Chapters = [3]
		},
		new MapMetadata {
			MapId = MapHaville,
			Name = "Haville",
			Bank = 0x0E,
			SubmapCount = 2,
			Type = MapType.Town,
			Chapters = [3]
		},
		new MapMetadata {
			MapId = MapMineUpper,
			Name = "Haville Mine Upper",
			Bank = 0x0E,
			SubmapCount = 2,
			Type = MapType.Cave,
			Chapters = [3]
		},
		new MapMetadata {
			MapId = MapMineLower,
			Name = "Haville Mine Lower",
			Bank = 0x0E,
			SubmapCount = 2,
			Type = MapType.Cave,
			Chapters = [3]
		},
		new MapMetadata {
			MapId = MapKievs,
			Name = "Kievs",
			Bank = 0x0F,
			SubmapCount = 3,
			Type = MapType.Town,
			Chapters = [3]
		},
		new MapMetadata {
			MapId = MapKievsCastleMain,
			Name = "Kievs Castle Main",
			Bank = 0x0F,
			SubmapCount = 3,
			Type = MapType.Castle,
			Chapters = [3]
		},
		new MapMetadata {
			MapId = MapKievsCastleThrone,
			Name = "Kievs Castle Throne",
			Bank = 0x0F,
			SubmapCount = 2,
			Type = MapType.Castle,
			Chapters = [3]
		},
		new MapMetadata {
			MapId = MapKievsCastleSecret,
			Name = "Kievs Castle Secret",
			Bank = 0x0F,
			SubmapCount = 1,
			Type = MapType.Castle,
			Chapters = [3]
		}
	];

	// ============================================================
	// Treasures
	// ============================================================

	/// <summary>
	/// Get all Chapter 4 treasure chests.
	/// </summary>
	public static TreasureChest[] GetAllTreasures() => [
		// Cave of Monbaraba
		new TreasureChest {
			Index = 90,
			MapId = MapCaveMonbarabaF1,
			SubmapIndex = 0,
			X = 5,
			Y = 12,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x05 // Medicinal Herb
		},
		new TreasureChest {
			Index = 91,
			MapId = MapCaveMonbarabaF1,
			SubmapIndex = 0,
			X = 28,
			Y = 8,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x30 // Leather Shield
		},
		new TreasureChest {
			Index = 92,
			MapId = MapCaveMonbarabaF2,
			SubmapIndex = 0,
			X = 12,
			Y = 18,
			ContentsType = TreasureType.Gold,
			ContentsValue = 150
		},
		new TreasureChest {
			Index = 93,
			MapId = MapCaveMonbarabaF2,
			SubmapIndex = 0,
			X = 5,
			Y = 5,
			ContentsType = TreasureType.Item,
			ContentsValue = Chapter4Events.ItemSphereOfSilence // Sphere of Silence
		},
		// Haville Mine
		new TreasureChest {
			Index = 94,
			MapId = MapMineUpper,
			SubmapIndex = 0,
			X = 20,
			Y = 15,
			ContentsType = TreasureType.Gold,
			ContentsValue = 100
		},
		new TreasureChest {
			Index = 95,
			MapId = MapMineUpper,
			SubmapIndex = 0,
			X = 8,
			Y = 28,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x18 // Torch
		},
		new TreasureChest {
			Index = 96,
			MapId = MapMineLower,
			SubmapIndex = 0,
			X = 15,
			Y = 10,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x35 // Iron Armor
		},
		new TreasureChest {
			Index = 97,
			MapId = MapMineLower,
			SubmapIndex = 0,
			X = 22,
			Y = 20,
			ContentsType = TreasureType.Gold,
			ContentsValue = 200
		},
		new TreasureChest {
			Index = 98,
			MapId = MapMineLower,
			SubmapIndex = 0,
			X = 5,
			Y = 5,
			ContentsType = TreasureType.Item,
			ContentsValue = Chapter4Events.ItemGunpowder // Gunpowder Jar
		},
		// Kievs Castle
		new TreasureChest {
			Index = 99,
			MapId = MapKievsCastleMain,
			SubmapIndex = 0,
			X = 25,
			Y = 12,
			ContentsType = TreasureType.Gold,
			ContentsValue = 300
		},
		new TreasureChest {
			Index = 100,
			MapId = MapKievsCastleMain,
			SubmapIndex = 0,
			X = 8,
			Y = 28,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x50 // Silk Robe
		},
		new TreasureChest {
			Index = 101,
			MapId = MapKievsCastleSecret,
			SubmapIndex = 0,
			X = 8,
			Y = 25,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x20 // Chimaera Wing
		},
		// Small Medal in secret passage
		new TreasureChest {
			Index = 102,
			MapId = MapKievsCastleSecret,
			SubmapIndex = 0,
			X = 20,
			Y = 15,
			ContentsType = TreasureType.SmallMedal,
			ContentsValue = 1
		}
	];

	// ============================================================
	// Warps
	// ============================================================

	/// <summary>
	/// Get all Chapter 4 warp points.
	/// </summary>
	public static WarpPoint[] GetAllWarps() => [
		// Monbaraba - Theater entrance
		new WarpPoint {
			SourceMapId = MapMonbaraba,
			SourceSubmapIndex = 0,
			SourceX = 15,
			SourceY = 8,
			DestMapId = MapTheater,
			DestSubmapIndex = 0,
			DestX = 12,
			DestY = 18,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapTheater,
			SourceSubmapIndex = 0,
			SourceX = 12,
			SourceY = 19,
			DestMapId = MapMonbaraba,
			DestSubmapIndex = 0,
			DestX = 15,
			DestY = 9,
			Type = WarpType.Exit
		},
		// Theater - Sisters Room
		new WarpPoint {
			SourceMapId = MapTheater,
			SourceSubmapIndex = 0,
			SourceX = 20,
			SourceY = 5,
			DestMapId = MapSistersRoom,
			DestSubmapIndex = 0,
			DestX = 4,
			DestY = 6,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapSistersRoom,
			SourceSubmapIndex = 0,
			SourceX = 4,
			SourceY = 7,
			DestMapId = MapTheater,
			DestSubmapIndex = 0,
			DestX = 20,
			DestY = 6,
			Type = WarpType.Exit
		},
		// Cave of Monbaraba - Overworld entrance
		new WarpPoint {
			SourceMapId = MapChapter4Overworld,
			SourceSubmapIndex = 0,
			SourceX = 45,
			SourceY = 80,
			DestMapId = MapCaveMonbarabaF1,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 30,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapCaveMonbarabaF1,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 31,
			DestMapId = MapChapter4Overworld,
			DestSubmapIndex = 0,
			DestX = 45,
			DestY = 81,
			Type = WarpType.Exit
		},
		// Cave F1 to F2
		new WarpPoint {
			SourceMapId = MapCaveMonbarabaF1,
			SourceSubmapIndex = 0,
			SourceX = 20,
			SourceY = 5,
			DestMapId = MapCaveMonbarabaF2,
			DestSubmapIndex = 0,
			DestX = 12,
			DestY = 22,
			Type = WarpType.StairsDown
		},
		new WarpPoint {
			SourceMapId = MapCaveMonbarabaF2,
			SourceSubmapIndex = 0,
			SourceX = 12,
			SourceY = 23,
			DestMapId = MapCaveMonbarabaF1,
			DestSubmapIndex = 0,
			DestX = 20,
			DestY = 6,
			Type = WarpType.StairsUp
		},
		// Haville Mine entrance
		new WarpPoint {
			SourceMapId = MapHaville,
			SourceSubmapIndex = 0,
			SourceX = 18,
			SourceY = 5,
			DestMapId = MapMineUpper,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 30,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapMineUpper,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 31,
			DestMapId = MapHaville,
			DestSubmapIndex = 0,
			DestX = 18,
			DestY = 6,
			Type = WarpType.Exit
		},
		// Mine Upper to Lower
		new WarpPoint {
			SourceMapId = MapMineUpper,
			SourceSubmapIndex = 0,
			SourceX = 5,
			SourceY = 5,
			DestMapId = MapMineLower,
			DestSubmapIndex = 0,
			DestX = 12,
			DestY = 22,
			Type = WarpType.StairsDown
		},
		new WarpPoint {
			SourceMapId = MapMineLower,
			SourceSubmapIndex = 0,
			SourceX = 12,
			SourceY = 23,
			DestMapId = MapMineUpper,
			DestSubmapIndex = 0,
			DestX = 5,
			DestY = 6,
			Type = WarpType.StairsUp
		},
		// Kievs Castle entrance
		new WarpPoint {
			SourceMapId = MapKievs,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 5,
			DestMapId = MapKievsCastleMain,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 30,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapKievsCastleMain,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 31,
			DestMapId = MapKievs,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 6,
			Type = WarpType.Exit
		},
		// Castle Main to Throne
		new WarpPoint {
			SourceMapId = MapKievsCastleMain,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 5,
			DestMapId = MapKievsCastleThrone,
			DestSubmapIndex = 0,
			DestX = 12,
			DestY = 18,
			Type = WarpType.StairsUp
		},
		new WarpPoint {
			SourceMapId = MapKievsCastleThrone,
			SourceSubmapIndex = 0,
			SourceX = 12,
			SourceY = 19,
			DestMapId = MapKievsCastleMain,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 6,
			Type = WarpType.StairsDown
		},
		// Secret passage entrance (hidden)
		new WarpPoint {
			SourceMapId = MapKievsCastleThrone,
			SourceSubmapIndex = 0,
			SourceX = 2,
			SourceY = 10,
			DestMapId = MapKievsCastleSecret,
			DestSubmapIndex = 0,
			DestX = 8,
			DestY = 2,
			Type = WarpType.Teleport
		},
		// Secret passage exit to town
		new WarpPoint {
			SourceMapId = MapKievsCastleSecret,
			SourceSubmapIndex = 0,
			SourceX = 8,
			SourceY = 30,
			DestMapId = MapKievs,
			DestSubmapIndex = 0,
			DestX = 5,
			DestY = 28,
			Type = WarpType.Exit
		}
	];

	// ============================================================
	// NPCs
	// ============================================================

	/// <summary>
	/// Get all Chapter 4 NPCs.
	/// </summary>
	public static NpcData[] GetAllNpcs() => [
		// Monbaraba
		new NpcData {
			Index = 0x40,
			MapId = MapMonbaraba,
			SubmapIndex = 0,
			X = 10,
			Y = 15,
			SpriteId = 0x30,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter4Events.MonbarabaInn,
			Flags = NpcFlags.Inn,
			Name = "Innkeeper"
		},
		new NpcData {
			Index = 0x41,
			MapId = MapMonbaraba,
			SubmapIndex = 0,
			X = 20,
			Y = 12,
			SpriteId = 0x31,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter4Events.MonbarabaItemShop,
			Flags = NpcFlags.Shop,
			Name = "Item Shopkeeper"
		},
		new NpcData {
			Index = 0x42,
			MapId = MapMonbaraba,
			SubmapIndex = 0,
			X = 8,
			Y = 20,
			SpriteId = 0x32,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter4Events.MonbarabaWeaponShop,
			Flags = NpcFlags.Shop,
			Name = "Weapon Shopkeeper"
		},
		new NpcData {
			Index = 0x43,
			MapId = MapMonbaraba,
			SubmapIndex = 0,
			X = 25,
			Y = 25,
			SpriteId = 0x20,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter4Events.MonbarabaChurch,
			Flags = NpcFlags.Church,
			Name = "Priest"
		},
		new NpcData {
			Index = 0x44,
			MapId = MapTheater,
			SubmapIndex = 0,
			X = 12,
			Y = 8,
			SpriteId = 0x40,
			Movement = NpcMovement.Stationary,
			Facing = 0,
			DialogId = Chapter4Events.TheaterManager,
			Flags = NpcFlags.None,
			Name = "Theater Manager"
		},
		new NpcData {
			Index = 0x45,
			MapId = MapMonbaraba,
			SubmapIndex = 0,
			X = 5,
			Y = 10,
			SpriteId = 0x50,
			Movement = NpcMovement.Random,
			Facing = 0,
			DialogId = Chapter4Events.MeetOrin,
			Flags = NpcFlags.None,
			Name = "Orin"
		},
		// Haville
		new NpcData {
			Index = 0x46,
			MapId = MapHaville,
			SubmapIndex = 0,
			X = 12,
			Y = 15,
			SpriteId = 0x30,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter4Events.HavilleInn,
			Flags = NpcFlags.Inn,
			Name = "Haville Innkeeper"
		},
		new NpcData {
			Index = 0x47,
			MapId = MapHaville,
			SubmapIndex = 0,
			X = 18,
			Y = 10,
			SpriteId = 0x31,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter4Events.HavilleItemShop,
			Flags = NpcFlags.Shop,
			Name = "Haville Shopkeeper"
		},
		new NpcData {
			Index = 0x48,
			MapId = MapHaville,
			SubmapIndex = 0,
			X = 8,
			Y = 8,
			SpriteId = 0x60,
			Movement = NpcMovement.Random,
			Facing = 0,
			DialogId = Chapter4Events.AlchemyRumors,
			Flags = NpcFlags.None,
			Name = "Miner"
		},
		// Kievs
		new NpcData {
			Index = 0x49,
			MapId = MapKievs,
			SubmapIndex = 0,
			X = 15,
			Y = 20,
			SpriteId = 0x30,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter4Events.KievsInn,
			Flags = NpcFlags.Inn,
			Name = "Kievs Innkeeper"
		},
		new NpcData {
			Index = 0x4A,
			MapId = MapKievs,
			SubmapIndex = 0,
			X = 22,
			Y = 15,
			SpriteId = 0x31,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter4Events.KievsItemShop,
			Flags = NpcFlags.Shop,
			Name = "Kievs Shopkeeper"
		},
		new NpcData {
			Index = 0x4B,
			MapId = MapKievsCastleMain,
			SubmapIndex = 0,
			X = 16,
			Y = 15,
			SpriteId = 0x70,
			Movement = NpcMovement.Stationary,
			Facing = 0,
			DialogId = Chapter4Events.KievsCastle,
			Flags = NpcFlags.None,
			Name = "Castle Guard"
		},
		new NpcData {
			Index = 0x4C,
			MapId = MapKievsCastleThrone,
			SubmapIndex = 1,
			X = 12,
			Y = 5,
			SpriteId = 0xF4,
			Movement = NpcMovement.Stationary,
			Facing = 0,
			DialogId = Chapter4Events.BalzackEncounter,
			Flags = NpcFlags.None,
			Name = "Balzack"
		}
	];

	// ============================================================
	// Encounter Zones
	// ============================================================

	/// <summary>
	/// Get all Chapter 4 encounter zones.
	/// </summary>
	public static EncounterZone[] GetAllEncounterZones() => [
		// Cave of Monbaraba F1
		new EncounterZone {
			Index = 0x40,
			MapId = MapCaveMonbarabaF1,
			SubmapIndex = 0,
			EncounterRate = 8,
			MonsterGroups = [0x40, 0x41, 0x42, 0x43]
		},
		// Cave of Monbaraba F2
		new EncounterZone {
			Index = 0x41,
			MapId = MapCaveMonbarabaF2,
			SubmapIndex = 0,
			EncounterRate = 10,
			MonsterGroups = [0x44, 0x45, 0x46, 0x47]
		},
		// Haville Mine Upper
		new EncounterZone {
			Index = 0x42,
			MapId = MapMineUpper,
			SubmapIndex = 0,
			EncounterRate = 10,
			MonsterGroups = [0x48, 0x49, 0x4A, 0x4B]
		},
		// Haville Mine Lower
		new EncounterZone {
			Index = 0x43,
			MapId = MapMineLower,
			SubmapIndex = 0,
			EncounterRate = 12,
			MonsterGroups = [0x4C, 0x4D, 0x4E, 0x4F]
		},
		// Chapter 4 Overworld
		new EncounterZone {
			Index = 0x44,
			MapId = MapChapter4Overworld,
			SubmapIndex = 0xFF,
			EncounterRate = 6,
			MonsterGroups = [0x50, 0x51, 0x52, 0x53]
		}
	];

	// ============================================================
	// DQ3r Conversion
	// ============================================================

	/// <summary>
	/// DQ3r map ID base offset for Chapter 4.
	/// </summary>
	public const int DQ3rMapIdBase = 0x0140;

	/// <summary>
	/// Get DQ3r map ID mapping for Chapter 4.
	/// </summary>
	public static Dictionary<int, int> GetDQ3rMapIdMapping() => new() {
		{ MapMonbaraba, DQ3rMapIdBase + 0x00 },
		{ MapTheater, DQ3rMapIdBase + 0x01 },
		{ MapSistersRoom, DQ3rMapIdBase + 0x02 },
		{ MapCaveMonbarabaF1, DQ3rMapIdBase + 0x03 },
		{ MapCaveMonbarabaF2, DQ3rMapIdBase + 0x04 },
		{ MapHaville, DQ3rMapIdBase + 0x05 },
		{ MapMineUpper, DQ3rMapIdBase + 0x06 },
		{ MapMineLower, DQ3rMapIdBase + 0x07 },
		{ MapKievs, DQ3rMapIdBase + 0x08 },
		{ MapKievsCastleMain, DQ3rMapIdBase + 0x09 },
		{ MapKievsCastleThrone, DQ3rMapIdBase + 0x0A },
		{ MapKievsCastleSecret, DQ3rMapIdBase + 0x0B },
	};

	/// <summary>
	/// Convert all Chapter 4 treasures to DQ3r format.
	/// </summary>
	public static DQ3rTreasure[] ConvertTreasures() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllTreasures()
			.Select(t => MapToDQ3r.ConvertTreasure(t))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 4 warps to DQ3r format.
	/// </summary>
	public static DQ3rWarp[] ConvertWarps() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllWarps()
			.Select(w => MapToDQ3r.ConvertWarp(w, mapping))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 4 NPCs to DQ3r format.
	/// </summary>
	public static DQ3rNpc[] ConvertNpcs() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllNpcs()
			.Select(n => MapToDQ3r.ConvertNpc(n, MapToDQ3r.MapIdToDQ3r(n.MapId, mapping)))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 4 encounter zones to DQ3r format.
	/// </summary>
	public static DQ3rEncounterZone[] ConvertEncounterZones() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllEncounterZones()
			.Select(z => MapToDQ3r.ConvertEncounterZone(z, mapping))
			.ToArray();
	}
}
