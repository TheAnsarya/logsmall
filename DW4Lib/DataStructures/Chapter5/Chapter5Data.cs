namespace DW4Lib.DataStructures.Chapter5;

/// <summary>
/// Chapter 5 "The Chosen Ones" specific data.
/// Protagonist: The Hero
/// Party: All previous protagonists can join
/// Mechanics: Wagon system, full party control, tactics
/// </summary>
public static class Chapter5Data {
	/// <summary>
	/// Chapter 5 ID.
	/// </summary>
	public const byte ChapterId = 0x04;

	/// <summary>
	/// Hero's character ID.
	/// </summary>
	public const byte HeroId = 0x00;

	/// <summary>
	/// All character IDs in recruit order.
	/// </summary>
	public static readonly byte[] AllCharacterIds = [
		0x00, // Hero
		0x01, // Cristo
		0x02, // Nara
		0x03, // Mara
		0x04, // Brey
		0x05, // Taloon
		0x06, // Ragnar
		0x07  // Alena
	];

	/// <summary>
	/// NPC companions in Chapter 5.
	/// </summary>
	public static readonly byte[] Chapter5Companions = [
		0xC9, // Hector (dog)
		0xCA, // Panon (wagon bearer)
		0xCB, // Lucia
		0xCC  // Doran
	];

	/// <summary>
	/// Hero's starting stats at level 1.
	/// Balanced stats as "chosen one".
	/// </summary>
	public static readonly CharacterStats HeroStartingStats = new() {
		Level = 1,
		HP = 35,
		MP = 6,
		Strength = 10,
		Agility = 8,
		Vitality = 10,
		Intelligence = 10,
		Luck = 10
	};

	/// <summary>
	/// Main story regions/areas in Chapter 5.
	/// </summary>
	public static readonly Chapter5Region[] Regions = [
		new() {
			Name = "Hero's Village Region",
			Description = "Starting area with tutorial content",
			KeyLocations = ["Hero's Village", "Branca Village", "Border Cave"],
			UnlockRequirement = "None (starting area)"
		},
		new() {
			Name = "Endor Region",
			Description = "Central hub connecting all areas",
			KeyLocations = ["Endor", "Colosseum", "Taloon's Shop"],
			UnlockRequirement = "Clear Border Cave"
		},
		new() {
			Name = "Santeem Region",
			Description = "Alena's homeland - recruit Alena, Cristo, Brey",
			KeyLocations = ["Santeem Castle", "Surene", "Tempe"],
			UnlockRequirement = "Reach Endor"
		},
		new() {
			Name = "Monbaraba Region",
			Description = "Sisters' homeland - recruit Nara, Mara",
			KeyLocations = ["Monbaraba", "Haville", "Keeleon Castle"],
			UnlockRequirement = "Have Balloon"
		},
		new() {
			Name = "Burland Region",
			Description = "Ragnar's homeland - recruit Ragnar",
			KeyLocations = ["Burland Castle", "Izmit"],
			UnlockRequirement = "Have Ship"
		},
		new() {
			Name = "Riverton/Mintos Region",
			Description = "Access to El Ciclo and western lands",
			KeyLocations = ["Riverton", "Mintos", "Baron's Manor"],
			UnlockRequirement = "Have Ship"
		},
		new() {
			Name = "Zenithian Region",
			Description = "Sky castle and Zenithian equipment",
			KeyLocations = ["Zenithia", "World Tree"],
			UnlockRequirement = "Collect Zenithian Equipment"
		},
		new() {
			Name = "Final Region",
			Description = "Path to the Ruler of Evil",
			KeyLocations = ["Dark World", "Necrosaro's Palace"],
			UnlockRequirement = "Access Dark World"
		}
	];

	/// <summary>
	/// Major bosses in Chapter 5.
	/// </summary>
	public static readonly Chapter5Boss[] Bosses = [
		new() {
			Id = 0x83,
			Name = "Keeleon",
			HP = 700,
			Location = "Keeleon Castle",
			Notes = "Upgraded Balzack"
		},
		new() {
			Id = 0x84,
			Name = "Balzack (True Form)",
			HP = 1200,
			Location = "Keeleon Castle Depths",
			Notes = "Balzack's monster form"
		},
		new() {
			Id = 0x85,
			Name = "Esturk",
			HP = 2500,
			Location = "Esturk's Lair",
			Notes = "Optional boss, sleeping"
		},
		new() {
			Id = 0x86,
			Name = "Anderoug",
			HP = 800,
			Location = "Dire Palace",
			Notes = "Demon gate guardian"
		},
		new() {
			Id = 0x87,
			Name = "Gigademon",
			HP = 1500,
			Location = "Final Cave",
			Notes = "Powerful demon"
		},
		new() {
			Id = 0x88,
			Name = "Necrosaro",
			HP = 3000,
			Location = "Final Palace",
			Notes = "Main antagonist"
		},
		new() {
			Id = 0x89,
			Name = "Psaro the Manslayer",
			HP = 4500,
			Location = "Final Palace",
			Notes = "Necrosaro's true form, multi-phase"
		}
	];

	/// <summary>
	/// Zenithian equipment locations.
	/// </summary>
	public static readonly ZenithianEquipment[] ZenithianGear = [
		new() {
			ItemId = 0x40,
			Name = "Zenithian Sword",
			Location = "Zenithia Castle",
			Requirement = "All party members recruited"
		},
		new() {
			ItemId = 0x50,
			Name = "Zenithian Armor",
			Location = "Riverton/Colosseum",
			Requirement = "Win special tournament"
		},
		new() {
			ItemId = 0x60,
			Name = "Zenithian Shield",
			Location = "Gardenbur Castle",
			Requirement = "Complete Gardenbur quest"
		},
		new() {
			ItemId = 0x70,
			Name = "Zenithian Helm",
			Location = "Royal Crypt",
			Requirement = "Explore crypt depths"
		}
	];

	/// <summary>
	/// Hero's spell progression.
	/// </summary>
	public static readonly SpellLearn[] HeroSpells = [
		new() { SpellId = 0x01, SpellName = "Heal", LearnLevel = 1 },
		new() { SpellId = 0x30, SpellName = "Fireball", LearnLevel = 3 },
		new() { SpellId = 0x31, SpellName = "Return", LearnLevel = 5 },
		new() { SpellId = 0x32, SpellName = "Healmore", LearnLevel = 8 },
		new() { SpellId = 0x33, SpellName = "Firebal", LearnLevel = 11 },
		new() { SpellId = 0x34, SpellName = "Healall", LearnLevel = 15 },
		new() { SpellId = 0x35, SpellName = "Zap", LearnLevel = 18 },
		new() { SpellId = 0x36, SpellName = "Revive", LearnLevel = 22 },
		new() { SpellId = 0x37, SpellName = "Gigasword", LearnLevel = 26 },
		new() { SpellId = 0x38, SpellName = "Kazap", LearnLevel = 30 }
	];

	/// <summary>
	/// Wagon capacity (max party members).
	/// </summary>
	public const int MaxActiveParty = 4;

	/// <summary>
	/// Max total characters including wagon.
	/// </summary>
	public const int MaxWagonCapacity = 8;

	/// <summary>
	/// Default battle tactic.
	/// </summary>
	public const BattleTactic DefaultTactic = BattleTactic.Normal;
}

/// <summary>
/// Character stats structure.
/// </summary>
public class CharacterStats {
	public int Level { get; set; }
	public int HP { get; set; }
	public int MP { get; set; }
	public int Strength { get; set; }
	public int Agility { get; set; }
	public int Vitality { get; set; }
	public int Intelligence { get; set; }
	public int Luck { get; set; }
}

/// <summary>
/// Chapter 5 world region.
/// </summary>
public class Chapter5Region {
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string[] KeyLocations { get; set; } = [];
	public string UnlockRequirement { get; set; } = string.Empty;
}

/// <summary>
/// Chapter 5 boss definition.
/// </summary>
public class Chapter5Boss {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int HP { get; set; }
	public string Location { get; set; } = string.Empty;
	public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Zenithian equipment piece.
/// </summary>
public class ZenithianEquipment {
	public int ItemId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Location { get; set; } = string.Empty;
	public string Requirement { get; set; } = string.Empty;
}

/// <summary>
/// Spell learning entry.
/// </summary>
public class SpellLearn {
	public int SpellId { get; set; }
	public string SpellName { get; set; } = string.Empty;
	public int LearnLevel { get; set; }
}

/// <summary>
/// Battle tactics from Chapter.cs.
/// </summary>
public enum BattleTactic : byte {
	Normal = 0x00,
	SaveMP = 0x01,
	Offensive = 0x02,
	Defensive = 0x03,
	TryOut = 0x04,
	UseNoMP = 0x05
}
