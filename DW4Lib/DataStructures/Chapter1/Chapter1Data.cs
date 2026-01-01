namespace DW4Lib.DataStructures.Chapter1;

/// <summary>
/// Chapter 1 "The Royal Soldiers" specific data.
/// Protagonist: Ragnar McRyan
/// Companion: Healie (NPC)
/// Location: Burland Kingdom region
/// </summary>
public static class Chapter1Data {
	/// <summary>
	/// Chapter 1 ID.
	/// </summary>
	public const byte ChapterId = 0x00;

	/// <summary>
	/// Ragnar's character ID.
	/// </summary>
	public const byte RagnarId = 0x06;

	/// <summary>
	/// Healie's NPC ID.
	/// </summary>
	public const byte HealieId = 0xC5;

	/// <summary>
	/// Ragnar's starting stats at level 1.
	/// </summary>
	public static readonly RagnarStats StartingStats = new() {
		Level = 1,
		HP = 30,
		MP = 0,
		Strength = 12,
		Agility = 6,
		Vitality = 10,
		Intelligence = 4,
		Luck = 5,
		Weapon = 0x00, // None
		Armor = 0x00,  // None
		Shield = 0x00, // None
		Helmet = 0x00  // None
	};

	/// <summary>
	/// Chapter 1 accessible maps.
	/// </summary>
	public static readonly Chapter1Map[] Maps = [
		new() {
			Id = 0x02,
			Name = "Burland Castle",
			Type = MapLocationType.Castle,
			HasShop = true,
			HasInn = true,
			HasChurch = true,
			OverworldX = 0x38,
			OverworldY = 0x58
		},
		new() {
			Id = 0x12,
			Name = "Izmit Village",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = true,
			OverworldX = 0x48,
			OverworldY = 0x60
		},
		new() {
			Id = 0x24,
			Name = "Loch Tower (Lighthouse)",
			Type = MapLocationType.Dungeon,
			HasShop = false,
			HasInn = false,
			HasChurch = false,
			OverworldX = 0x30,
			OverworldY = 0x50
		},
		new() {
			Id = 0x25,
			Name = "Cave West of Izmit",
			Type = MapLocationType.Cave,
			HasShop = false,
			HasInn = false,
			HasChurch = false,
			OverworldX = 0x40,
			OverworldY = 0x68
		},
		new() {
			Id = 0x26,
			Name = "Loch Tower Basement",
			Type = MapLocationType.Dungeon,
			HasShop = false,
			HasInn = false,
			HasChurch = false,
			OverworldX = 0x30,
			OverworldY = 0x50
		}
	];

	/// <summary>
	/// Chapter 1 story events in order.
	/// </summary>
	public static readonly Chapter1Event[] Events = [
		new() {
			Id = 0x0001,
			Name = "Chapter 1 Start - King's Orders",
			Description = "Ragnar receives orders from the King of Burland",
			MapId = 0x02, // Burland Castle
			TriggerType = EventTrigger.ChapterStart,
			DialogId = 0x0100,
			SetFlag = 0x0001
		},
		new() {
			Id = 0x0002,
			Name = "Izmit Investigation",
			Description = "Talk to villagers about missing children",
			MapId = 0x12, // Izmit
			TriggerType = EventTrigger.EnterMap,
			RequiredFlag = 0x0001,
			DialogId = 0x0110,
			SetFlag = 0x0002
		},
		new() {
			Id = 0x0003,
			Name = "Cave Rumor",
			Description = "Learn about suspicious cave west of village",
			MapId = 0x12, // Izmit
			TriggerType = EventTrigger.TalkToNPC,
			NpcId = 0x10, // Villager
			RequiredFlag = 0x0002,
			DialogId = 0x0120,
			SetFlag = 0x0003
		},
		new() {
			Id = 0x0004,
			Name = "Find Healie",
			Description = "Meet Healie in the cave",
			MapId = 0x25, // Cave West of Izmit
			TriggerType = EventTrigger.EnterTile,
			TriggerX = 0x08,
			TriggerY = 0x0A,
			RequiredFlag = 0x0003,
			DialogId = 0x0130,
			SetFlag = 0x0004,
			JoinCharacterId = HealieId
		},
		new() {
			Id = 0x0005,
			Name = "Loch Tower Discovery",
			Description = "Learn children were taken to Loch Tower",
			MapId = 0x25,
			TriggerType = EventTrigger.TalkToNPC,
			NpcId = HealieId,
			RequiredFlag = 0x0004,
			DialogId = 0x0140,
			SetFlag = 0x0005
		},
		new() {
			Id = 0x0006,
			Name = "Reach Loch Tower Basement",
			Description = "Navigate to the tower basement",
			MapId = 0x26, // Basement
			TriggerType = EventTrigger.EnterMap,
			RequiredFlag = 0x0005,
			DialogId = 0x0150,
			SetFlag = 0x0006
		},
		new() {
			Id = 0x0007,
			Name = "Boss: Chameleon Humanoid",
			Description = "Fight the boss holding the children",
			MapId = 0x26,
			TriggerType = EventTrigger.EnterTile,
			TriggerX = 0x10,
			TriggerY = 0x08,
			RequiredFlag = 0x0006,
			BossId = 0x80, // Chameleon Humanoid
			SetFlag = 0x0007
		},
		new() {
			Id = 0x0008,
			Name = "Children Rescued",
			Description = "Free the children from imprisonment",
			MapId = 0x26,
			TriggerType = EventTrigger.BossDefeated,
			RequiredFlag = 0x0007,
			DialogId = 0x0160,
			SetFlag = 0x0008
		},
		new() {
			Id = 0x0009,
			Name = "Return to Burland",
			Description = "Report success to the King",
			MapId = 0x02, // Burland Castle
			TriggerType = EventTrigger.TalkToNPC,
			NpcId = 0x01, // King
			RequiredFlag = 0x0008,
			DialogId = 0x0170,
			SetFlag = 0x0009
		},
		new() {
			Id = 0x0100,
			Name = "Chapter 1 Complete",
			Description = "Chapter 1 ends, transition to Chapter 2",
			MapId = 0x02,
			TriggerType = EventTrigger.ChapterEnd,
			RequiredFlag = 0x0009,
			DialogId = 0x0180,
			SetFlag = 0x0100,
			IsChapterEnd = true
		}
	];

	/// <summary>
	/// Chapter 1 treasure chest locations.
	/// </summary>
	public static readonly Chapter1Treasure[] Treasures = [
		// Burland Castle
		new() { Id = 0, MapId = 0x02, X = 0x05, Y = 0x03, ContentsType = TreasureContents.Gold, Value = 50 },
		new() { Id = 1, MapId = 0x02, X = 0x0A, Y = 0x03, ContentsType = TreasureContents.Item, Value = 0x52 }, // Herb

		// Izmit Village
		new() { Id = 2, MapId = 0x12, X = 0x04, Y = 0x08, ContentsType = TreasureContents.Gold, Value = 30 },
		new() { Id = 3, MapId = 0x12, X = 0x0C, Y = 0x02, ContentsType = TreasureContents.Item, Value = 0x53 }, // Antidote

		// Cave West of Izmit
		new() { Id = 4, MapId = 0x25, X = 0x06, Y = 0x0E, ContentsType = TreasureContents.Item, Value = 0x02 }, // Club
		new() { Id = 5, MapId = 0x25, X = 0x0C, Y = 0x04, ContentsType = TreasureContents.Gold, Value = 120 },
		new() { Id = 6, MapId = 0x25, X = 0x08, Y = 0x0C, ContentsType = TreasureContents.Item, Value = 0x11 }, // Leather Armor

		// Loch Tower
		new() { Id = 7, MapId = 0x24, X = 0x03, Y = 0x05, ContentsType = TreasureContents.Item, Value = 0x03 }, // Copper Sword
		new() { Id = 8, MapId = 0x24, X = 0x0D, Y = 0x0A, ContentsType = TreasureContents.Gold, Value = 200 },
		new() { Id = 9, MapId = 0x24, X = 0x07, Y = 0x02, ContentsType = TreasureContents.Item, Value = 0x20 }, // Leather Shield

		// Loch Tower Basement
		new() { Id = 10, MapId = 0x26, X = 0x02, Y = 0x0E, ContentsType = TreasureContents.Item, Value = 0x31 }, // Iron Helmet
		new() { Id = 11, MapId = 0x26, X = 0x0E, Y = 0x02, ContentsType = TreasureContents.SmallMedal, Value = 1 }
	];

	/// <summary>
	/// Chapter 1 shop inventories.
	/// </summary>
	public static readonly Chapter1Shop[] Shops = [
		new() {
			MapId = 0x02, // Burland
			ShopType = ShopType.Weapon,
			Items = [0x02, 0x03, 0x04], // Club, Copper Sword, Boomerang
			Prices = [30, 100, 350]
		},
		new() {
			MapId = 0x02, // Burland
			ShopType = ShopType.Armor,
			Items = [0x10, 0x11, 0x20], // Clothes, Leather Armor, Leather Shield
			Prices = [10, 70, 90]
		},
		new() {
			MapId = 0x02, // Burland
			ShopType = ShopType.Item,
			Items = [0x52, 0x53, 0x54, 0x55], // Herb, Antidote, Wing, Torch
			Prices = [8, 10, 25, 8]
		},
		new() {
			MapId = 0x12, // Izmit
			ShopType = ShopType.Weapon,
			Items = [0x02, 0x03], // Club, Copper Sword
			Prices = [30, 100]
		},
		new() {
			MapId = 0x12, // Izmit
			ShopType = ShopType.Item,
			Items = [0x52, 0x53, 0x54], // Herb, Antidote, Wing
			Prices = [8, 10, 25]
		}
	];

	/// <summary>
	/// Chapter 1 enemy encounter zones.
	/// </summary>
	public static readonly Chapter1EncounterZone[] EncounterZones = [
		// Overworld - near Burland
		new() {
			ZoneId = 0x01,
			Description = "Burland area overworld",
			MonsterGroups = [0x01, 0x02, 0x03], // Slime, Red Slime, Dracky
			EncounterRate = 8
		},
		// Overworld - near Izmit
		new() {
			ZoneId = 0x02,
			Description = "Izmit area overworld",
			MonsterGroups = [0x02, 0x03, 0x04, 0x05], // Red Slime, Dracky, Stump, Imp
			EncounterRate = 10
		},
		// Cave West of Izmit
		new() {
			ZoneId = 0x03,
			Description = "Cave B1",
			MonsterGroups = [0x04, 0x05, 0x06], // Stump, Imp, Demon Stump
			EncounterRate = 12
		},
		// Loch Tower
		new() {
			ZoneId = 0x04,
			Description = "Loch Tower floors",
			MonsterGroups = [0x05, 0x06, 0x07, 0x08], // Imp, Demon Stump, Wyvern, Shadow
			EncounterRate = 14
		},
		// Loch Tower Basement
		new() {
			ZoneId = 0x05,
			Description = "Loch Tower Basement",
			MonsterGroups = [0x07, 0x08, 0x09], // Wyvern, Shadow, Skeleton
			EncounterRate = 16
		}
	];

	/// <summary>
	/// Get the total number of Chapter 1 event flags needed.
	/// </summary>
	public const int TotalEventFlags = 16;

	/// <summary>
	/// Chapter 1 completion flag.
	/// </summary>
	public const int CompletionFlag = 0x0100;
}

/// <summary>
/// Ragnar's stats structure.
/// </summary>
public class RagnarStats {
	public int Level { get; set; }
	public int HP { get; set; }
	public int MP { get; set; }
	public int Strength { get; set; }
	public int Agility { get; set; }
	public int Vitality { get; set; }
	public int Intelligence { get; set; }
	public int Luck { get; set; }
	public byte Weapon { get; set; }
	public byte Armor { get; set; }
	public byte Shield { get; set; }
	public byte Helmet { get; set; }
}

/// <summary>
/// Chapter 1 map location.
/// </summary>
public class Chapter1Map {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public MapLocationType Type { get; set; }
	public bool HasShop { get; set; }
	public bool HasInn { get; set; }
	public bool HasChurch { get; set; }
	public byte OverworldX { get; set; }
	public byte OverworldY { get; set; }
}

/// <summary>
/// Map location types.
/// </summary>
public enum MapLocationType {
	Castle,
	Town,
	Cave,
	Dungeon,
	Tower,
	Shrine,
	Overworld
}

/// <summary>
/// Chapter 1 story event.
/// </summary>
public class Chapter1Event {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int MapId { get; set; }
	public EventTrigger TriggerType { get; set; }
	public int TriggerX { get; set; } = -1;
	public int TriggerY { get; set; } = -1;
	public int NpcId { get; set; } = -1;
	public int RequiredFlag { get; set; } = -1;
	public int DialogId { get; set; }
	public int SetFlag { get; set; } = -1;
	public int BossId { get; set; } = -1;
	public byte JoinCharacterId { get; set; } = 0xFF;
	public bool IsChapterEnd { get; set; }
}

/// <summary>
/// Event trigger types.
/// </summary>
public enum EventTrigger {
	ChapterStart,
	EnterMap,
	EnterTile,
	TalkToNPC,
	BossDefeated,
	ItemUsed,
	ChapterEnd
}

/// <summary>
/// Chapter 1 treasure chest.
/// </summary>
public class Chapter1Treasure {
	public int Id { get; set; }
	public int MapId { get; set; }
	public byte X { get; set; }
	public byte Y { get; set; }
	public TreasureContents ContentsType { get; set; }
	public int Value { get; set; }
}

/// <summary>
/// Treasure contents type.
/// </summary>
public enum TreasureContents {
	Item,
	Gold,
	SmallMedal,
	Empty,
	Monster
}

/// <summary>
/// Chapter 1 shop.
/// </summary>
public class Chapter1Shop {
	public int MapId { get; set; }
	public ShopType ShopType { get; set; }
	public byte[] Items { get; set; } = [];
	public int[] Prices { get; set; } = [];
}

/// <summary>
/// Shop types.
/// </summary>
public enum ShopType {
	Weapon,
	Armor,
	Item,
	Inn,
	Church
}

/// <summary>
/// Chapter 1 encounter zone.
/// </summary>
public class Chapter1EncounterZone {
	public int ZoneId { get; set; }
	public string Description { get; set; } = string.Empty;
	public byte[] MonsterGroups { get; set; } = [];
	public byte EncounterRate { get; set; }
}
