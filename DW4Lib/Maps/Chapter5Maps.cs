using DW4Lib.Converters;
using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;
using DW4Lib.Events;

namespace DW4Lib.Maps;

/// <summary>
/// Chapter 5 map definitions and data.
/// Contains all map layouts, events, NPCs, and treasures for Chapter 5 (Hero's story).
/// This is the main chapter where all previous characters unite.
/// </summary>
public static class Chapter5Maps {
	// ============================================================
	// Map IDs
	// ============================================================

	/// <summary>Hero's village (destroyed).</summary>
	public const int MapHeroVillage = 0x50;

	/// <summary>Hero's house.</summary>
	public const int MapHeroHouse = 0x51;

	/// <summary>Secret passage from village.</summary>
	public const int MapSecretPassage = 0x52;

	/// <summary>Branca - first town.</summary>
	public const int MapBranca = 0x53;

	/// <summary>Endor - major city.</summary>
	public const int MapEndor = 0x54;

	/// <summary>Endor Castle.</summary>
	public const int MapEndorCastle = 0x55;

	/// <summary>Endor Casino.</summary>
	public const int MapEndorCasino = 0x56;

	/// <summary>Mintos - northern town.</summary>
	public const int MapMintos = 0x57;

	/// <summary>Zenithian Tower F1.</summary>
	public const int MapZenithianTowerF1 = 0x58;

	/// <summary>Zenithian Tower F2.</summary>
	public const int MapZenithianTowerF2 = 0x59;

	/// <summary>Zenithian Tower F3.</summary>
	public const int MapZenithianTowerF3 = 0x5A;

	/// <summary>Zenithia - floating castle.</summary>
	public const int MapZenithia = 0x5B;

	/// <summary>Zenithia Throne Room.</summary>
	public const int MapZenithiaThrone = 0x5C;

	/// <summary>Psaro's Castle entrance.</summary>
	public const int MapPsaroCastleEntrance = 0x5D;

	/// <summary>Psaro's Castle main hall.</summary>
	public const int MapPsaroCastleMain = 0x5E;

	/// <summary>Psaro's Castle throne room.</summary>
	public const int MapPsaroCastleThrone = 0x5F;

	/// <summary>Chapter 5 overworld.</summary>
	public const int MapChapter5Overworld = 0x05;

	// ============================================================
	// Tileset IDs
	// ============================================================

	/// <summary>Village tileset.</summary>
	public const byte TilesetVillage = 0x02;

	/// <summary>Town tileset.</summary>
	public const byte TilesetTown = 0x02;

	/// <summary>Castle tileset.</summary>
	public const byte TilesetCastle = 0x01;

	/// <summary>Tower tileset.</summary>
	public const byte TilesetTower = 0x50;

	/// <summary>Zenithia tileset.</summary>
	public const byte TilesetZenithia = 0x51;

	/// <summary>Dark castle tileset.</summary>
	public const byte TilesetDarkCastle = 0x52;

	// ============================================================
	// Map Metadata
	// ============================================================

	/// <summary>
	/// Get all Chapter 5 map metadata.
	/// </summary>
	public static MapMetadata[] GetAllMaps() => [
		new MapMetadata {
			MapId = MapHeroVillage,
			Name = "Hero's Village",
			Bank = 0x10,
			SubmapCount = 2,
			Type = MapType.Town,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapHeroHouse,
			Name = "Hero's House",
			Bank = 0x10,
			SubmapCount = 1,
			Type = MapType.Other,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapSecretPassage,
			Name = "Secret Passage",
			Bank = 0x10,
			SubmapCount = 1,
			Type = MapType.Cave,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapBranca,
			Name = "Branca",
			Bank = 0x10,
			SubmapCount = 4,
			Type = MapType.Town,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapEndor,
			Name = "Endor",
			Bank = 0x11,
			SubmapCount = 6,
			Type = MapType.Town,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapEndorCastle,
			Name = "Endor Castle",
			Bank = 0x11,
			SubmapCount = 3,
			Type = MapType.Castle,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapEndorCasino,
			Name = "Endor Casino",
			Bank = 0x11,
			SubmapCount = 2,
			Type = MapType.Other,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapMintos,
			Name = "Mintos",
			Bank = 0x11,
			SubmapCount = 3,
			Type = MapType.Town,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapZenithianTowerF1,
			Name = "Zenithian Tower F1",
			Bank = 0x12,
			SubmapCount = 1,
			Type = MapType.Cave,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapZenithianTowerF2,
			Name = "Zenithian Tower F2",
			Bank = 0x12,
			SubmapCount = 1,
			Type = MapType.Cave,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapZenithianTowerF3,
			Name = "Zenithian Tower F3",
			Bank = 0x12,
			SubmapCount = 1,
			Type = MapType.Cave,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapZenithia,
			Name = "Zenithia",
			Bank = 0x12,
			SubmapCount = 4,
			Type = MapType.Castle,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapZenithiaThrone,
			Name = "Zenithia Throne",
			Bank = 0x12,
			SubmapCount = 1,
			Type = MapType.Castle,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapPsaroCastleEntrance,
			Name = "Psaro Castle Entrance",
			Bank = 0x13,
			SubmapCount = 1,
			Type = MapType.Castle,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapPsaroCastleMain,
			Name = "Psaro Castle Main",
			Bank = 0x13,
			SubmapCount = 3,
			Type = MapType.Castle,
			Chapters = [4]
		},
		new MapMetadata {
			MapId = MapPsaroCastleThrone,
			Name = "Psaro Castle Throne",
			Bank = 0x13,
			SubmapCount = 1,
			Type = MapType.Castle,
			Chapters = [4]
		}
	];

	// ============================================================
	// Treasures
	// ============================================================

	/// <summary>
	/// Get all Chapter 5 treasure chests.
	/// </summary>
	public static TreasureChest[] GetAllTreasures() => [
		// Hero's House
		new TreasureChest {
			Index = 110,
			MapId = MapHeroHouse,
			SubmapIndex = 0,
			X = 4,
			Y = 2,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x05 // Medicinal Herb
		},
		// Secret Passage
		new TreasureChest {
			Index = 111,
			MapId = MapSecretPassage,
			SubmapIndex = 0,
			X = 10,
			Y = 25,
			ContentsType = TreasureType.Gold,
			ContentsValue = 50
		},
		// Branca
		new TreasureChest {
			Index = 112,
			MapId = MapBranca,
			SubmapIndex = 0,
			X = 8,
			Y = 12,
			ContentsType = TreasureType.Gold,
			ContentsValue = 100
		},
		// Endor Castle
		new TreasureChest {
			Index = 113,
			MapId = MapEndorCastle,
			SubmapIndex = 0,
			X = 20,
			Y = 8,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x50 // Silk Robe
		},
		new TreasureChest {
			Index = 114,
			MapId = MapEndorCastle,
			SubmapIndex = 1,
			X = 5,
			Y = 15,
			ContentsType = TreasureType.Gold,
			ContentsValue = 500
		},
		// Zenithian Tower F1
		new TreasureChest {
			Index = 115,
			MapId = MapZenithianTowerF1,
			SubmapIndex = 0,
			X = 12,
			Y = 20,
			ContentsType = TreasureType.Gold,
			ContentsValue = 200
		},
		new TreasureChest {
			Index = 116,
			MapId = MapZenithianTowerF1,
			SubmapIndex = 0,
			X = 25,
			Y = 5,
			ContentsType = TreasureType.SmallMedal,
			ContentsValue = 1
		},
		// Zenithian Tower F2
		new TreasureChest {
			Index = 117,
			MapId = MapZenithianTowerF2,
			SubmapIndex = 0,
			X = 8,
			Y = 18,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x60 // Dragon Shield
		},
		new TreasureChest {
			Index = 118,
			MapId = MapZenithianTowerF2,
			SubmapIndex = 0,
			X = 20,
			Y = 10,
			ContentsType = TreasureType.Gold,
			ContentsValue = 800
		},
		// Zenithian Tower F3 - Zenithian Sword
		new TreasureChest {
			Index = 119,
			MapId = MapZenithianTowerF3,
			SubmapIndex = 0,
			X = 16,
			Y = 8,
			ContentsType = TreasureType.Item,
			ContentsValue = Chapter5Events.ItemZenithianSword
		},
		// Zenithia
		new TreasureChest {
			Index = 120,
			MapId = MapZenithia,
			SubmapIndex = 0,
			X = 12,
			Y = 25,
			ContentsType = TreasureType.SmallMedal,
			ContentsValue = 1
		},
		new TreasureChest {
			Index = 121,
			MapId = MapZenithia,
			SubmapIndex = 1,
			X = 8,
			Y = 8,
			ContentsType = TreasureType.Item,
			ContentsValue = Chapter5Events.ItemZenithianHelm
		},
		// Psaro's Castle - Zenithian Shield
		new TreasureChest {
			Index = 122,
			MapId = MapPsaroCastleMain,
			SubmapIndex = 0,
			X = 25,
			Y = 15,
			ContentsType = TreasureType.Gold,
			ContentsValue = 1000
		},
		new TreasureChest {
			Index = 123,
			MapId = MapPsaroCastleMain,
			SubmapIndex = 1,
			X = 5,
			Y = 5,
			ContentsType = TreasureType.Item,
			ContentsValue = Chapter5Events.ItemZenithianShield
		},
		new TreasureChest {
			Index = 124,
			MapId = MapPsaroCastleMain,
			SubmapIndex = 2,
			X = 20,
			Y = 20,
			ContentsType = TreasureType.SmallMedal,
			ContentsValue = 1
		}
	];

	// ============================================================
	// Warps
	// ============================================================

	/// <summary>
	/// Get all Chapter 5 warp points.
	/// </summary>
	public static WarpPoint[] GetAllWarps() => [
		// Hero's Village to House
		new WarpPoint {
			SourceMapId = MapHeroVillage,
			SourceSubmapIndex = 0,
			SourceX = 10,
			SourceY = 12,
			DestMapId = MapHeroHouse,
			DestSubmapIndex = 0,
			DestX = 4,
			DestY = 7,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapHeroHouse,
			SourceSubmapIndex = 0,
			SourceX = 4,
			SourceY = 8,
			DestMapId = MapHeroVillage,
			DestSubmapIndex = 0,
			DestX = 10,
			DestY = 13,
			Type = WarpType.Exit
		},
		// Secret Passage
		new WarpPoint {
			SourceMapId = MapHeroHouse,
			SourceSubmapIndex = 0,
			SourceX = 2,
			SourceY = 2,
			DestMapId = MapSecretPassage,
			DestSubmapIndex = 0,
			DestX = 5,
			DestY = 2,
			Type = WarpType.StairsDown
		},
		new WarpPoint {
			SourceMapId = MapSecretPassage,
			SourceSubmapIndex = 0,
			SourceX = 30,
			SourceY = 30,
			DestMapId = MapChapter5Overworld,
			DestSubmapIndex = 0,
			DestX = 50,
			DestY = 60,
			Type = WarpType.Exit
		},
		// Branca entrances (generic)
		new WarpPoint {
			SourceMapId = MapChapter5Overworld,
			SourceSubmapIndex = 0,
			SourceX = 55,
			SourceY = 70,
			DestMapId = MapBranca,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 30,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapBranca,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 31,
			DestMapId = MapChapter5Overworld,
			DestSubmapIndex = 0,
			DestX = 55,
			DestY = 71,
			Type = WarpType.Exit
		},
		// Endor entrances
		new WarpPoint {
			SourceMapId = MapChapter5Overworld,
			SourceSubmapIndex = 0,
			SourceX = 80,
			SourceY = 90,
			DestMapId = MapEndor,
			DestSubmapIndex = 0,
			DestX = 20,
			DestY = 38,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapEndor,
			SourceSubmapIndex = 0,
			SourceX = 20,
			SourceY = 39,
			DestMapId = MapChapter5Overworld,
			DestSubmapIndex = 0,
			DestX = 80,
			DestY = 91,
			Type = WarpType.Exit
		},
		// Endor to Castle
		new WarpPoint {
			SourceMapId = MapEndor,
			SourceSubmapIndex = 0,
			SourceX = 20,
			SourceY = 5,
			DestMapId = MapEndorCastle,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 30,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapEndorCastle,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 31,
			DestMapId = MapEndor,
			DestSubmapIndex = 0,
			DestX = 20,
			DestY = 6,
			Type = WarpType.Exit
		},
		// Endor to Casino
		new WarpPoint {
			SourceMapId = MapEndor,
			SourceSubmapIndex = 0,
			SourceX = 30,
			SourceY = 15,
			DestMapId = MapEndorCasino,
			DestSubmapIndex = 0,
			DestX = 10,
			DestY = 18,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapEndorCasino,
			SourceSubmapIndex = 0,
			SourceX = 10,
			SourceY = 19,
			DestMapId = MapEndor,
			DestSubmapIndex = 0,
			DestX = 30,
			DestY = 16,
			Type = WarpType.Exit
		},
		// Mintos
		new WarpPoint {
			SourceMapId = MapChapter5Overworld,
			SourceSubmapIndex = 0,
			SourceX = 30,
			SourceY = 120,
			DestMapId = MapMintos,
			DestSubmapIndex = 0,
			DestX = 12,
			DestY = 25,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapMintos,
			SourceSubmapIndex = 0,
			SourceX = 12,
			SourceY = 26,
			DestMapId = MapChapter5Overworld,
			DestSubmapIndex = 0,
			DestX = 30,
			DestY = 121,
			Type = WarpType.Exit
		},
		// Zenithian Tower
		new WarpPoint {
			SourceMapId = MapChapter5Overworld,
			SourceSubmapIndex = 0,
			SourceX = 100,
			SourceY = 50,
			DestMapId = MapZenithianTowerF1,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 30,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapZenithianTowerF1,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 31,
			DestMapId = MapChapter5Overworld,
			DestSubmapIndex = 0,
			DestX = 100,
			DestY = 51,
			Type = WarpType.Exit
		},
		// Tower F1 to F2
		new WarpPoint {
			SourceMapId = MapZenithianTowerF1,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 5,
			DestMapId = MapZenithianTowerF2,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 25,
			Type = WarpType.StairsUp
		},
		new WarpPoint {
			SourceMapId = MapZenithianTowerF2,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 26,
			DestMapId = MapZenithianTowerF1,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 6,
			Type = WarpType.StairsDown
		},
		// Tower F2 to F3
		new WarpPoint {
			SourceMapId = MapZenithianTowerF2,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 5,
			DestMapId = MapZenithianTowerF3,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 25,
			Type = WarpType.StairsUp
		},
		new WarpPoint {
			SourceMapId = MapZenithianTowerF3,
			SourceSubmapIndex = 0,
			SourceX = 16,
			DestMapId = MapZenithianTowerF2,
			SourceY = 26,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 6,
			Type = WarpType.StairsDown
		},
		// Zenithia (from tower top or teleport)
		new WarpPoint {
			SourceMapId = MapZenithianTowerF3,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 2,
			DestMapId = MapZenithia,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 30,
			Type = WarpType.Teleport
		},
		new WarpPoint {
			SourceMapId = MapZenithia,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 31,
			DestMapId = MapZenithianTowerF3,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 3,
			Type = WarpType.Teleport
		},
		// Zenithia to Throne Room
		new WarpPoint {
			SourceMapId = MapZenithia,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 5,
			DestMapId = MapZenithiaThrone,
			DestSubmapIndex = 0,
			DestX = 10,
			DestY = 15,
			Type = WarpType.StairsUp
		},
		new WarpPoint {
			SourceMapId = MapZenithiaThrone,
			SourceSubmapIndex = 0,
			SourceX = 10,
			SourceY = 16,
			DestMapId = MapZenithia,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 6,
			Type = WarpType.StairsDown
		},
		// Psaro's Castle entrance
		new WarpPoint {
			SourceMapId = MapChapter5Overworld,
			SourceSubmapIndex = 0,
			SourceX = 200,
			SourceY = 200,
			DestMapId = MapPsaroCastleEntrance,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 30,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapPsaroCastleEntrance,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 31,
			DestMapId = MapChapter5Overworld,
			DestSubmapIndex = 0,
			DestX = 200,
			DestY = 201,
			Type = WarpType.Exit
		},
		// Psaro Castle Entrance to Main
		new WarpPoint {
			SourceMapId = MapPsaroCastleEntrance,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 5,
			DestMapId = MapPsaroCastleMain,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 30,
			Type = WarpType.StairsUp
		},
		new WarpPoint {
			SourceMapId = MapPsaroCastleMain,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 31,
			DestMapId = MapPsaroCastleEntrance,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 6,
			Type = WarpType.StairsDown
		},
		// Psaro Castle Main to Throne
		new WarpPoint {
			SourceMapId = MapPsaroCastleMain,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 5,
			DestMapId = MapPsaroCastleThrone,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 18,
			Type = WarpType.StairsUp
		},
		new WarpPoint {
			SourceMapId = MapPsaroCastleThrone,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 19,
			DestMapId = MapPsaroCastleMain,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 6,
			Type = WarpType.StairsDown
		}
	];

	// ============================================================
	// NPCs
	// ============================================================

	/// <summary>
	/// Get all Chapter 5 NPCs.
	/// </summary>
	public static NpcData[] GetAllNpcs() => [
		// Hero's House - Mother
		new NpcData {
			Index = 0x50,
			MapId = MapHeroHouse,
			SubmapIndex = 0,
			X = 5,
			Y = 4,
			SpriteId = 0x70,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.IntroScript,
			Flags = NpcFlags.None,
			Name = "Mother"
		},
		// Branca - Services
		new NpcData {
			Index = 0x51,
			MapId = MapBranca,
			SubmapIndex = 0,
			X = 10,
			Y = 15,
			SpriteId = 0x30,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.BrancaInn,
			Flags = NpcFlags.Inn,
			Name = "Branca Innkeeper"
		},
		new NpcData {
			Index = 0x52,
			MapId = MapBranca,
			SubmapIndex = 0,
			X = 20,
			Y = 12,
			SpriteId = 0x31,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.BrancaItemShop,
			Flags = NpcFlags.Shop,
			Name = "Branca Item Shopkeeper"
		},
		new NpcData {
			Index = 0x53,
			MapId = MapBranca,
			SubmapIndex = 0,
			X = 8,
			Y = 20,
			SpriteId = 0x32,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.BrancaWeaponShop,
			Flags = NpcFlags.Shop,
			Name = "Branca Weapon Shopkeeper"
		},
		new NpcData {
			Index = 0x54,
			MapId = MapBranca,
			SubmapIndex = 0,
			X = 25,
			Y = 25,
			SpriteId = 0x20,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.BrancaChurch,
			Flags = NpcFlags.Church,
			Name = "Branca Priest"
		},
		new NpcData {
			Index = 0x55,
			MapId = MapBranca,
			SubmapIndex = 1,
			X = 12,
			Y = 8,
			SpriteId = 0x60,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.ZenithianLegend,
			Flags = NpcFlags.None,
			Name = "Elder"
		},
		// Endor - Ragnar and services
		new NpcData {
			Index = 0x56,
			MapId = MapEndor,
			SubmapIndex = 0,
			X = 18,
			Y = 20,
			SpriteId = 0x01,
			Movement = NpcMovement.Random,
			Facing = 2,
			DialogId = Chapter5Events.MeetRagnar,
			Flags = NpcFlags.None,
			Name = "Ragnar"
		},
		new NpcData {
			Index = 0x57,
			MapId = MapEndor,
			SubmapIndex = 0,
			X = 12,
			Y = 30,
			SpriteId = 0x30,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.EndorInn,
			Flags = NpcFlags.Inn,
			Name = "Endor Innkeeper"
		},
		new NpcData {
			Index = 0x58,
			MapId = MapEndor,
			SubmapIndex = 0,
			X = 28,
			Y = 25,
			SpriteId = 0x31,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.EndorItemShop,
			Flags = NpcFlags.Shop,
			Name = "Endor Item Shopkeeper"
		},
		new NpcData {
			Index = 0x59,
			MapId = MapEndor,
			SubmapIndex = 0,
			X = 8,
			Y = 35,
			SpriteId = 0x32,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.EndorWeaponShop,
			Flags = NpcFlags.Shop,
			Name = "Endor Weapon Shopkeeper"
		},
		new NpcData {
			Index = 0x5A,
			MapId = MapEndor,
			SubmapIndex = 0,
			X = 35,
			Y = 10,
			SpriteId = 0x20,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.EndorChurch,
			Flags = NpcFlags.Church,
			Name = "Endor Priest"
		},
		// Endor Casino
		new NpcData {
			Index = 0x5B,
			MapId = MapEndorCasino,
			SubmapIndex = 0,
			X = 10,
			Y = 8,
			SpriteId = 0x71,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.CasinoNpc,
			Flags = NpcFlags.None,
			Name = "Casino Bunny"
		},
		// Mintos - Alena's group
		new NpcData {
			Index = 0x5C,
			MapId = MapMintos,
			SubmapIndex = 0,
			X = 15,
			Y = 12,
			SpriteId = 0x02,
			Movement = NpcMovement.Random,
			Facing = 2,
			DialogId = Chapter5Events.MeetAlenaGroup,
			Flags = NpcFlags.None,
			Name = "Alena"
		},
		// Zenithia - Master Dragon
		new NpcData {
			Index = 0x5D,
			MapId = MapZenithiaThrone,
			SubmapIndex = 0,
			X = 10,
			Y = 5,
			SpriteId = 0xF8,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.MeetMasterDragon,
			Flags = NpcFlags.None,
			Name = "Master Dragon"
		},
		// Vault Keeper
		new NpcData {
			Index = 0x5E,
			MapId = MapEndor,
			SubmapIndex = 2,
			X = 8,
			Y = 10,
			SpriteId = 0x33,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter5Events.VaultKeeper,
			Flags = NpcFlags.None,
			Name = "Vault Keeper"
		}
	];

	// ============================================================
	// Encounter Zones
	// ============================================================

	/// <summary>
	/// Get all Chapter 5 encounter zones.
	/// </summary>
	public static EncounterZone[] GetAllEncounterZones() => [
		// Secret Passage
		new EncounterZone {
			Index = 0x50,
			MapId = MapSecretPassage,
			SubmapIndex = 0,
			EncounterRate = 6,
			MonsterGroups = [0x60, 0x61, 0x62, 0x63]
		},
		// Zenithian Tower F1
		new EncounterZone {
			Index = 0x51,
			MapId = MapZenithianTowerF1,
			SubmapIndex = 0,
			EncounterRate = 10,
			MonsterGroups = [0x64, 0x65, 0x66, 0x67]
		},
		// Zenithian Tower F2
		new EncounterZone {
			Index = 0x52,
			MapId = MapZenithianTowerF2,
			SubmapIndex = 0,
			EncounterRate = 12,
			MonsterGroups = [0x68, 0x69, 0x6A, 0x6B]
		},
		// Zenithian Tower F3
		new EncounterZone {
			Index = 0x53,
			MapId = MapZenithianTowerF3,
			SubmapIndex = 0,
			EncounterRate = 14,
			MonsterGroups = [0x6C, 0x6D, 0x6E, 0x6F]
		},
		// Psaro's Castle Entrance
		new EncounterZone {
			Index = 0x54,
			MapId = MapPsaroCastleEntrance,
			SubmapIndex = 0,
			EncounterRate = 12,
			MonsterGroups = [0x70, 0x71, 0x72, 0x73]
		},
		// Psaro's Castle Main
		new EncounterZone {
			Index = 0x55,
			MapId = MapPsaroCastleMain,
			SubmapIndex = 0xFF,
			EncounterRate = 14,
			MonsterGroups = [0x74, 0x75, 0x76, 0x77]
		},
		// Psaro's Castle Throne (low rate near boss)
		new EncounterZone {
			Index = 0x56,
			MapId = MapPsaroCastleThrone,
			SubmapIndex = 0,
			EncounterRate = 4,
			MonsterGroups = [0x78, 0x79, 0x7A, 0x7B]
		},
		// Chapter 5 Overworld
		new EncounterZone {
			Index = 0x57,
			MapId = MapChapter5Overworld,
			SubmapIndex = 0xFF,
			EncounterRate = 8,
			MonsterGroups = [0x7C, 0x7D, 0x7E, 0x7F]
		}
	];

	// ============================================================
	// DQ3r Conversion
	// ============================================================

	/// <summary>
	/// DQ3r map ID base offset for Chapter 5.
	/// </summary>
	public const int DQ3rMapIdBase = 0x0500;

	/// <summary>
	/// Get DQ3r map ID mapping for Chapter 5.
	/// </summary>
	public static Dictionary<int, int> GetDQ3rMapIdMapping() => new() {
		{ MapHeroVillage, DQ3rMapIdBase + 0x00 },
		{ MapHeroHouse, DQ3rMapIdBase + 0x01 },
		{ MapSecretPassage, DQ3rMapIdBase + 0x02 },
		{ MapBranca, DQ3rMapIdBase + 0x03 },
		{ MapEndor, DQ3rMapIdBase + 0x04 },
		{ MapEndorCastle, DQ3rMapIdBase + 0x05 },
		{ MapEndorCasino, DQ3rMapIdBase + 0x06 },
		{ MapMintos, DQ3rMapIdBase + 0x07 },
		{ MapZenithianTowerF1, DQ3rMapIdBase + 0x08 },
		{ MapZenithianTowerF2, DQ3rMapIdBase + 0x09 },
		{ MapZenithianTowerF3, DQ3rMapIdBase + 0x0A },
		{ MapZenithia, DQ3rMapIdBase + 0x0B },
		{ MapZenithiaThrone, DQ3rMapIdBase + 0x0C },
		{ MapPsaroCastleEntrance, DQ3rMapIdBase + 0x0D },
		{ MapPsaroCastleMain, DQ3rMapIdBase + 0x0E },
		{ MapPsaroCastleThrone, DQ3rMapIdBase + 0x0F },
	};

	/// <summary>
	/// Convert all Chapter 5 treasures to DQ3r format.
	/// </summary>
	public static DQ3rTreasure[] ConvertTreasures() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllTreasures()
			.Select(t => MapToDQ3r.ConvertTreasure(t))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 5 warps to DQ3r format.
	/// </summary>
	public static DQ3rWarp[] ConvertWarps() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllWarps()
			.Select(w => MapToDQ3r.ConvertWarp(w, mapping))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 5 NPCs to DQ3r format.
	/// </summary>
	public static DQ3rNpc[] ConvertNpcs() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllNpcs()
			.Select(n => MapToDQ3r.ConvertNpc(n, MapToDQ3r.MapIdToDQ3r(n.MapId, mapping)))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 5 encounter zones to DQ3r format.
	/// </summary>
	public static DQ3rEncounterZone[] ConvertEncounterZones() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllEncounterZones()
			.Select(z => MapToDQ3r.ConvertEncounterZone(z, mapping))
			.ToArray();
	}
}
