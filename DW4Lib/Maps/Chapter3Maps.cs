using DW4Lib.Converters;
using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;
using DW4Lib.Events;

namespace DW4Lib.Maps;

/// <summary>
/// Chapter 3 (Taloon/Torneko) map data.
/// Contains all map metadata, treasures, warps, NPCs, and encounter zones.
/// </summary>
public static class Chapter3Maps {
	// ============================================================
	// Map IDs
	// ============================================================

	/// <summary>Lakanaba town.</summary>
	public const int MapLakanaba = 0x30;

	/// <summary>Lakanaba weapon shop interior.</summary>
	public const int MapLakanabaWeaponShop = 0x31;

	/// <summary>Taloon's house.</summary>
	public const int MapTaloonHouse = 0x32;

	/// <summary>Cave east of Lakanaba.</summary>
	public const int MapEastCaveF1 = 0x33;

	/// <summary>Cave east of Lakanaba - floor 2.</summary>
	public const int MapEastCaveF2 = 0x34;

	/// <summary>Fox Village.</summary>
	public const int MapFoxVillage = 0x35;

	/// <summary>Bonmalmo town.</summary>
	public const int MapBonmalmo = 0x36;

	/// <summary>Bonmalmo castle/Prince Reed.</summary>
	public const int MapBonmalmoCastle = 0x37;

	/// <summary>Silver Statuette cave.</summary>
	public const int MapSilverCaveF1 = 0x38;

	/// <summary>Silver Statuette cave - floor 2.</summary>
	public const int MapSilverCaveF2 = 0x39;

	/// <summary>Ship to Endor.</summary>
	public const int MapShip = 0x3A;

	/// <summary>Endor (shared with Chapter 2).</summary>
	public const int MapEndor = 0x28;

	/// <summary>Endor tunnel construction.</summary>
	public const int MapTunnel = 0x3B;

	// ============================================================
	// Map Metadata
	// ============================================================

	/// <summary>
	/// Get all Chapter 3 map metadata.
	/// </summary>
	public static MapMetadata[] GetAllMaps() => [
		new MapMetadata {
			MapId = MapLakanaba,
			Name = "Lakanaba",
			Bank = 0x09,
			SubmapCount = 3,
			Type = MapType.Town,
			Chapters = [2]
		},
		new MapMetadata {
			MapId = MapLakanabaWeaponShop,
			Name = "Lakanaba Weapon Shop",
			Bank = 0x09,
			SubmapCount = 1,
			Type = MapType.Other,
			Chapters = [2]
		},
		new MapMetadata {
			MapId = MapTaloonHouse,
			Name = "Taloon's House",
			Bank = 0x09,
			SubmapCount = 1,
			Type = MapType.Other,
			Chapters = [2]
		},
		new MapMetadata {
			MapId = MapEastCaveF1,
			Name = "East Cave F1",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Cave,
			Chapters = [2]
		},
		new MapMetadata {
			MapId = MapEastCaveF2,
			Name = "East Cave F2",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Cave,
			Chapters = [2]
		},
		new MapMetadata {
			MapId = MapFoxVillage,
			Name = "Fox Village",
			Bank = 0x09,
			SubmapCount = 2,
			Type = MapType.Town,
			Chapters = [2]
		},
		new MapMetadata {
			MapId = MapBonmalmo,
			Name = "Bonmalmo",
			Bank = 0x09,
			SubmapCount = 2,
			Type = MapType.Town,
			Chapters = [2]
		},
		new MapMetadata {
			MapId = MapBonmalmoCastle,
			Name = "Bonmalmo Castle",
			Bank = 0x09,
			SubmapCount = 2,
			Type = MapType.Castle,
			Chapters = [2]
		},
		new MapMetadata {
			MapId = MapSilverCaveF1,
			Name = "Silver Cave F1",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Cave,
			Chapters = [2]
		},
		new MapMetadata {
			MapId = MapSilverCaveF2,
			Name = "Silver Cave F2",
			Bank = 0x0A,
			SubmapCount = 1,
			Type = MapType.Cave,
			Chapters = [2]
		},
		new MapMetadata {
			MapId = MapShip,
			Name = "Ship to Endor",
			Bank = 0x09,
			SubmapCount = 1,
			Type = MapType.Other,
			Chapters = [2]
		},
		new MapMetadata {
			MapId = MapTunnel,
			Name = "Endor Tunnel",
			Bank = 0x0A,
			SubmapCount = 2,
			Type = MapType.Cave,
			Chapters = [2]
		}
	];

	// ============================================================
	// Treasures
	// ============================================================

	/// <summary>
	/// Get all Chapter 3 treasure chests.
	/// </summary>
	public static TreasureChest[] GetAllTreasures() => [
		// Lakanaba
		new TreasureChest {
			Index = 70,
			MapId = MapLakanaba,
			SubmapIndex = 0,
			X = 8,
			Y = 15,
			ContentsType = TreasureType.Gold,
			ContentsValue = 30
		},
		// Taloon's House
		new TreasureChest {
			Index = 71,
			MapId = MapTaloonHouse,
			SubmapIndex = 0,
			X = 3,
			Y = 2,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x05 // Medical Herb
		},
		// East Cave F1
		new TreasureChest {
			Index = 72,
			MapId = MapEastCaveF1,
			SubmapIndex = 0,
			X = 12,
			Y = 8,
			ContentsType = TreasureType.Gold,
			ContentsValue = 80
		},
		new TreasureChest {
			Index = 73,
			MapId = MapEastCaveF1,
			SubmapIndex = 0,
			X = 20,
			Y = 15,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x06 // Antidote
		},
		// East Cave F2 - Steel Broadsword
		new TreasureChest {
			Index = 74,
			MapId = MapEastCaveF2,
			SubmapIndex = 0,
			X = 15,
			Y = 20,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x15 // Steel Broadsword
		},
		new TreasureChest {
			Index = 75,
			MapId = MapEastCaveF2,
			SubmapIndex = 0,
			X = 8,
			Y = 12,
			ContentsType = TreasureType.Gold,
			ContentsValue = 200
		},
		// Fox Village
		new TreasureChest {
			Index = 76,
			MapId = MapFoxVillage,
			SubmapIndex = 0,
			X = 5,
			Y = 10,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x20 // Fox Tail?
		},
		// Bonmalmo Castle
		new TreasureChest {
			Index = 77,
			MapId = MapBonmalmoCastle,
			SubmapIndex = 1,
			X = 22,
			Y = 8,
			ContentsType = TreasureType.Gold,
			ContentsValue = 500
		},
		// Silver Cave F1
		new TreasureChest {
			Index = 78,
			MapId = MapSilverCaveF1,
			SubmapIndex = 0,
			X = 10,
			Y = 18,
			ContentsType = TreasureType.Gold,
			ContentsValue = 150
		},
		new TreasureChest {
			Index = 79,
			MapId = MapSilverCaveF1,
			SubmapIndex = 0,
			X = 25,
			Y = 5,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x08 // Fairy Water
		},
		// Silver Cave F2 - Silver Statuette
		new TreasureChest {
			Index = 80,
			MapId = MapSilverCaveF2,
			SubmapIndex = 0,
			X = 16,
			Y = 16,
			ContentsType = TreasureType.Item,
			ContentsValue = 0x61 // Silver Statuette
		},
		// Tunnel
		new TreasureChest {
			Index = 81,
			MapId = MapTunnel,
			SubmapIndex = 0,
			X = 30,
			Y = 10,
			ContentsType = TreasureType.SmallMedal,
			ContentsValue = 1
		},
		new TreasureChest {
			Index = 82,
			MapId = MapTunnel,
			SubmapIndex = 1,
			X = 5,
			Y = 25,
			ContentsType = TreasureType.Gold,
			ContentsValue = 1000
		}
	];

	// ============================================================
	// Warps
	// ============================================================

	/// <summary>
	/// Get all Chapter 3 warp points.
	/// </summary>
	public static WarpPoint[] GetAllWarps() => [
		// Lakanaba entrances
		new WarpPoint {
			SourceMapId = MapLakanaba,
			SourceSubmapIndex = 0,
			SourceX = 15,
			SourceY = 10,
			DestMapId = MapLakanabaWeaponShop,
			DestSubmapIndex = 0,
			DestX = 7,
			DestY = 12,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapLakanabaWeaponShop,
			SourceSubmapIndex = 0,
			SourceX = 7,
			SourceY = 13,
			DestMapId = MapLakanaba,
			DestSubmapIndex = 0,
			DestX = 15,
			DestY = 11,
			Type = WarpType.Exit
		},
		new WarpPoint {
			SourceMapId = MapLakanaba,
			SourceSubmapIndex = 0,
			SourceX = 8,
			SourceY = 20,
			DestMapId = MapTaloonHouse,
			DestSubmapIndex = 0,
			DestX = 5,
			DestY = 8,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapTaloonHouse,
			SourceSubmapIndex = 0,
			SourceX = 5,
			SourceY = 9,
			DestMapId = MapLakanaba,
			DestSubmapIndex = 0,
			DestX = 8,
			DestY = 21,
			Type = WarpType.Exit
		},
		// East Cave
		new WarpPoint {
			SourceMapId = MapEastCaveF1,
			SourceSubmapIndex = 0,
			SourceX = 20,
			SourceY = 25,
			DestMapId = MapEastCaveF2,
			DestSubmapIndex = 0,
			DestX = 5,
			DestY = 5,
			Type = WarpType.StairsDown
		},
		new WarpPoint {
			SourceMapId = MapEastCaveF2,
			SourceSubmapIndex = 0,
			SourceX = 5,
			SourceY = 4,
			DestMapId = MapEastCaveF1,
			DestSubmapIndex = 0,
			DestX = 20,
			DestY = 24,
			Type = WarpType.StairsUp
		},
		// Bonmalmo to Castle
		new WarpPoint {
			SourceMapId = MapBonmalmo,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 5,
			DestMapId = MapBonmalmoCastle,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 28,
			Type = WarpType.Door
		},
		new WarpPoint {
			SourceMapId = MapBonmalmoCastle,
			SourceSubmapIndex = 0,
			SourceX = 16,
			SourceY = 29,
			DestMapId = MapBonmalmo,
			DestSubmapIndex = 0,
			DestX = 16,
			DestY = 6,
			Type = WarpType.Exit
		},
		// Silver Cave
		new WarpPoint {
			SourceMapId = MapSilverCaveF1,
			SourceSubmapIndex = 0,
			SourceX = 28,
			SourceY = 20,
			DestMapId = MapSilverCaveF2,
			DestSubmapIndex = 0,
			DestX = 3,
			DestY = 3,
			Type = WarpType.StairsDown
		},
		new WarpPoint {
			SourceMapId = MapSilverCaveF2,
			SourceSubmapIndex = 0,
			SourceX = 3,
			SourceY = 2,
			DestMapId = MapSilverCaveF1,
			DestSubmapIndex = 0,
			DestX = 28,
			DestY = 19,
			Type = WarpType.StairsUp
		},
		// Tunnel between areas
		new WarpPoint {
			SourceMapId = MapTunnel,
			SourceSubmapIndex = 0,
			SourceX = 2,
			SourceY = 15,
			DestMapId = MapTunnel,
			DestSubmapIndex = 1,
			DestX = 30,
			DestY = 15,
			Type = WarpType.StairsDown
		},
		new WarpPoint {
			SourceMapId = MapTunnel,
			SourceSubmapIndex = 1,
			SourceX = 31,
			SourceY = 15,
			DestMapId = MapTunnel,
			DestSubmapIndex = 0,
			DestX = 3,
			DestY = 15,
			Type = WarpType.StairsUp
		}
	];

	// ============================================================
	// NPCs
	// ============================================================

	/// <summary>
	/// Get all Chapter 3 NPCs.
	/// </summary>
	public static NpcData[] GetAllNpcs() => [
		// Taloon's House - Neta
		new NpcData {
			Index = 200,
			MapId = MapTaloonHouse,
			SubmapIndex = 0,
			X = 6,
			Y = 5,
			SpriteId = 0x20,
			Movement = NpcMovement.Random,
			Facing = 0,
			DialogId = Chapter3Events.MorningHome,
			Flags = NpcFlags.None,
			Name = "Neta"
		},
		// Lakanaba Weapon Shop - Owner
		new NpcData {
			Index = 201,
			MapId = MapLakanabaWeaponShop,
			SubmapIndex = 0,
			X = 4,
			Y = 4,
			SpriteId = 0x21,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter3Events.LakanabaWeaponShop,
			Flags = NpcFlags.Shop,
			Name = "Weapon Shop Owner"
		},
		// Lakanaba - Item Shop
		new NpcData {
			Index = 202,
			MapId = MapLakanaba,
			SubmapIndex = 1,
			X = 5,
			Y = 3,
			SpriteId = 0x22,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter3Events.LakanabaItemShop,
			Flags = NpcFlags.Shop,
			Name = "Item Shop Clerk"
		},
		// Lakanaba - Innkeeper
		new NpcData {
			Index = 203,
			MapId = MapLakanaba,
			SubmapIndex = 2,
			X = 3,
			Y = 5,
			SpriteId = 0x23,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter3Events.LakanabaInn,
			Flags = NpcFlags.Inn,
			Name = "Lakanaba Innkeeper"
		},
		// Lakanaba - Priest
		new NpcData {
			Index = 204,
			MapId = MapLakanaba,
			SubmapIndex = 0,
			X = 22,
			Y = 8,
			SpriteId = 0x24,
			Movement = NpcMovement.Stationary,
			Facing = 0,
			DialogId = Chapter3Events.LakanabaChurch,
			Flags = NpcFlags.Church,
			Name = "Lakanaba Priest"
		},
		// Lakanaba - Old Man (cave info)
		new NpcData {
			Index = 205,
			MapId = MapLakanaba,
			SubmapIndex = 0,
			X = 10,
			Y = 25,
			SpriteId = 0x30,
			Movement = NpcMovement.Pace,
			Facing = 0,
			DialogId = Chapter3Events.OldManCave,
			Flags = NpcFlags.None,
			Name = "Old Man"
		},
		// Fox Village - Fox Shopkeeper
		new NpcData {
			Index = 210,
			MapId = MapFoxVillage,
			SubmapIndex = 0,
			X = 8,
			Y = 8,
			SpriteId = 0x40,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter3Events.FoxvilleShop,
			Flags = NpcFlags.Shop,
			Name = "Fox Shopkeeper"
		},
		// Bonmalmo - Shopkeeper
		new NpcData {
			Index = 220,
			MapId = MapBonmalmo,
			SubmapIndex = 1,
			X = 6,
			Y = 4,
			SpriteId = 0x22,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter3Events.BonmalmoShop,
			Flags = NpcFlags.Shop,
			Name = "Bonmalmo Shopkeeper"
		},
		// Bonmalmo - Innkeeper
		new NpcData {
			Index = 221,
			MapId = MapBonmalmo,
			SubmapIndex = 1,
			X = 15,
			Y = 4,
			SpriteId = 0x23,
			Movement = NpcMovement.Stationary,
			Facing = 2,
			DialogId = Chapter3Events.BonmalmoInn,
			Flags = NpcFlags.Inn,
			Name = "Bonmalmo Innkeeper"
		},
		// Bonmalmo Castle - Prince Reed
		new NpcData {
			Index = 230,
			MapId = MapBonmalmoCastle,
			SubmapIndex = 1,
			X = 16,
			Y = 5,
			SpriteId = 0x10,
			Movement = NpcMovement.Stationary,
			Facing = 0,
			DialogId = Chapter3Events.PrinceReed,
			Flags = NpcFlags.None,
			Name = "Prince Reed"
		},
		// Ship - Captain
		new NpcData {
			Index = 240,
			MapId = MapShip,
			SubmapIndex = 0,
			X = 8,
			Y = 3,
			SpriteId = 0x50,
			Movement = NpcMovement.Stationary,
			Facing = 0,
			DialogId = Chapter3Events.ShipPassage,
			Flags = NpcFlags.None,
			Name = "Ship Captain"
		},
		// Tunnel - Worker
		new NpcData {
			Index = 250,
			MapId = MapTunnel,
			SubmapIndex = 0,
			X = 15,
			Y = 15,
			SpriteId = 0x60,
			Movement = NpcMovement.Random,
			Facing = 0,
			DialogId = Chapter3Events.TunnelStart,
			Flags = NpcFlags.None,
			Name = "Tunnel Worker"
		}
	];

	// ============================================================
	// Encounter Zones
	// ============================================================

	/// <summary>
	/// Get all Chapter 3 encounter zones.
	/// </summary>
	public static EncounterZone[] GetAllEncounterZones() => [
		// East Cave F1 - Easy
		new EncounterZone {
			Index = 0x30,
			MapId = MapEastCaveF1,
			SubmapIndex = 0,
			EncounterRate = 8,
			MonsterGroups = [0x20, 0x21, 0x22, 0x23]
		},
		// East Cave F2 - Medium
		new EncounterZone {
			Index = 0x31,
			MapId = MapEastCaveF2,
			SubmapIndex = 0,
			EncounterRate = 10,
			MonsterGroups = [0x24, 0x25, 0x26, 0x27]
		},
		// Silver Cave F1 - Medium
		new EncounterZone {
			Index = 0x32,
			MapId = MapSilverCaveF1,
			SubmapIndex = 0,
			EncounterRate = 10,
			MonsterGroups = [0x28, 0x29, 0x2A, 0x2B]
		},
		// Silver Cave F2 - Hard
		new EncounterZone {
			Index = 0x33,
			MapId = MapSilverCaveF2,
			SubmapIndex = 0,
			EncounterRate = 12,
			MonsterGroups = [0x2C, 0x2D, 0x2E, 0x2F]
		},
		// Tunnel - Medium-Hard
		new EncounterZone {
			Index = 0x34,
			MapId = MapTunnel,
			SubmapIndex = 0xFF,
			EncounterRate = 8,
			MonsterGroups = [0x30, 0x31, 0x32, 0x33]
		}
	];

	// ============================================================
	// Entrances (Overworld locations)
	// ============================================================

	/// <summary>
	/// Get all Chapter 3 overworld entrances.
	/// </summary>
	public static EntranceLocation[] GetAllEntrances() => [
		new EntranceLocation {
			Name = "Lakanaba",
			Overworld = OverworldType.Main,
			X = 45,
			Y = 120,
			DestMapId = MapLakanaba,
			DestSubmapIndex = 0,
			Type = MapType.Town,
			AvailableChapters = [2]
		},
		new EntranceLocation {
			Name = "East Cave",
			Overworld = OverworldType.Main,
			X = 60,
			Y = 115,
			DestMapId = MapEastCaveF1,
			DestSubmapIndex = 0,
			Type = MapType.Cave,
			AvailableChapters = [2]
		},
		new EntranceLocation {
			Name = "Fox Village",
			Overworld = OverworldType.Main,
			X = 55,
			Y = 100,
			DestMapId = MapFoxVillage,
			DestSubmapIndex = 0,
			Type = MapType.Town,
			AvailableChapters = [2]
		},
		new EntranceLocation {
			Name = "Bonmalmo",
			Overworld = OverworldType.Main,
			X = 70,
			Y = 95,
			DestMapId = MapBonmalmo,
			DestSubmapIndex = 0,
			Type = MapType.Town,
			AvailableChapters = [2]
		},
		new EntranceLocation {
			Name = "Bonmalmo Castle",
			Overworld = OverworldType.Main,
			X = 75,
			Y = 90,
			DestMapId = MapBonmalmoCastle,
			DestSubmapIndex = 0,
			Type = MapType.Castle,
			AvailableChapters = [2]
		},
		new EntranceLocation {
			Name = "Silver Cave",
			Overworld = OverworldType.Main,
			X = 80,
			Y = 85,
			DestMapId = MapSilverCaveF1,
			DestSubmapIndex = 0,
			Type = MapType.Cave,
			AvailableChapters = [2]
		},
		new EntranceLocation {
			Name = "Endor Tunnel",
			Overworld = OverworldType.Main,
			X = 100,
			Y = 130,
			DestMapId = MapTunnel,
			DestSubmapIndex = 0,
			Type = MapType.Cave,
			AvailableChapters = [2]
		}
	];

	// ============================================================
	// DQ3r Conversion
	// ============================================================

	/// <summary>
	/// DQ3r map ID base offset for Chapter 3.
	/// </summary>
	public const int DQ3rMapIdBase = 0x0130;

	/// <summary>
	/// Get DQ3r map ID mapping for Chapter 3.
	/// </summary>
	public static Dictionary<int, int> GetDQ3rMapIdMapping() => new() {
		{ MapLakanaba, DQ3rMapIdBase + 0x00 },
		{ MapLakanabaWeaponShop, DQ3rMapIdBase + 0x01 },
		{ MapTaloonHouse, DQ3rMapIdBase + 0x02 },
		{ MapEastCaveF1, DQ3rMapIdBase + 0x03 },
		{ MapEastCaveF2, DQ3rMapIdBase + 0x04 },
		{ MapFoxVillage, DQ3rMapIdBase + 0x05 },
		{ MapBonmalmo, DQ3rMapIdBase + 0x06 },
		{ MapBonmalmoCastle, DQ3rMapIdBase + 0x07 },
		{ MapSilverCaveF1, DQ3rMapIdBase + 0x08 },
		{ MapSilverCaveF2, DQ3rMapIdBase + 0x09 },
		{ MapShip, DQ3rMapIdBase + 0x0A },
		{ MapTunnel, DQ3rMapIdBase + 0x0B },
		{ MapEndor, 0x0128 } // Shared Endor from Chapter 2
	};

	/// <summary>
	/// Convert all Chapter 3 treasures to DQ3r format.
	/// </summary>
	public static DQ3rTreasure[] ConvertTreasures() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllTreasures()
			.Select(t => MapToDQ3r.ConvertTreasure(t))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 3 warps to DQ3r format.
	/// </summary>
	public static DQ3rWarp[] ConvertWarps() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllWarps()
			.Select(w => MapToDQ3r.ConvertWarp(w, mapping))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 3 NPCs to DQ3r format.
	/// </summary>
	public static DQ3rNpc[] ConvertNpcs() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllNpcs()
			.Select(n => MapToDQ3r.ConvertNpc(n, MapToDQ3r.MapIdToDQ3r(n.MapId, mapping)))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 3 encounter zones to DQ3r format.
	/// </summary>
	public static DQ3rEncounterZone[] ConvertEncounterZones() {
		var mapping = GetDQ3rMapIdMapping();
		return GetAllEncounterZones()
			.Select(z => MapToDQ3r.ConvertEncounterZone(z, mapping))
			.ToArray();
	}
}
