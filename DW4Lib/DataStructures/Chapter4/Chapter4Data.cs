namespace DW4Lib.DataStructures.Chapter4;

/// <summary>
/// Chapter 4 "The Sisters of Monbaraba" specific data.
/// Protagonists: Nara (controllable), Mara (AI)
/// Companion: Orin (NPC, temporary)
/// Location: Monbaraba/Keeleon region
/// </summary>
public static class Chapter4Data {
	/// <summary>
	/// Chapter 4 ID.
	/// </summary>
	public const byte ChapterId = 0x03;

	/// <summary>
	/// Nara's character ID.
	/// </summary>
	public const byte NaraId = 0x02;

	/// <summary>
	/// Mara's character ID.
	/// </summary>
	public const byte MaraId = 0x03;

	/// <summary>
	/// Orin's NPC companion ID.
	/// </summary>
	public const byte OrinId = 0xC6;

	/// <summary>
	/// Nara's starting stats at level 1.
	/// Fortune teller/Priest type.
	/// </summary>
	public static readonly CharacterStats NaraStartingStats = new() {
		Level = 1,
		HP = 20,
		MP = 12,
		Strength = 6,
		Agility = 7,
		Vitality = 6,
		Intelligence = 12,
		Luck = 8
	};

	/// <summary>
	/// Mara's starting stats at level 1.
	/// Dancer/Mage type.
	/// </summary>
	public static readonly CharacterStats MaraStartingStats = new() {
		Level = 1,
		HP = 18,
		MP = 15,
		Strength = 5,
		Agility = 10,
		Vitality = 5,
		Intelligence = 14,
		Luck = 6
	};

	/// <summary>
	/// Orin's NPC stats (temporary companion).
	/// </summary>
	public static readonly CharacterStats OrinStats = new() {
		Level = 10,
		HP = 80,
		MP = 0,
		Strength = 35,
		Agility = 25,
		Vitality = 30,
		Intelligence = 10,
		Luck = 10
	};

	/// <summary>
	/// Chapter 4 accessible maps.
	/// </summary>
	public static readonly Chapter4Map[] Maps = [
		new() {
			Id = 0x15,
			Name = "Monbaraba",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = true,
			Notes = "Starting location, sisters' home"
		},
		new() {
			Id = 0x20,
			Name = "Cave South of Monbaraba",
			Type = MapLocationType.Cave,
			HasShop = false,
			HasInn = false,
			HasChurch = false,
			Notes = "Shortcut through mountains"
		},
		new() {
			Id = 0x21,
			Name = "Haville",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = true,
			Notes = "Mining town"
		},
		new() {
			Id = 0x22,
			Name = "Aktemto Mine",
			Type = MapLocationType.Dungeon,
			HasShop = false,
			HasInn = false,
			HasChurch = false,
			Notes = "Poison gas filled mine"
		},
		new() {
			Id = 0x23,
			Name = "Keeleon Castle",
			Type = MapLocationType.Castle,
			HasShop = false,
			HasInn = false,
			HasChurch = false,
			Notes = "Balzack's stronghold"
		},
		new() {
			Id = 0x24,
			Name = "Gardenbur",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = true,
			Notes = "Women-only town (initially)"
		}
	];

	/// <summary>
	/// Chapter 4 story events in order.
	/// </summary>
	public static readonly Chapter4Event[] Events = [
		new() {
			Id = 0x0401,
			Name = "Father's Murder",
			Description = "Learn of Edgar's death at Balzack's hands",
			MapId = 0x15
		},
		new() {
			Id = 0x0402,
			Name = "Seek Revenge",
			Description = "Sisters vow to avenge their father",
			MapId = 0x15
		},
		new() {
			Id = 0x0403,
			Name = "Meet Orin",
			Description = "Orin joins to help the sisters",
			MapId = 0x20
		},
		new() {
			Id = 0x0404,
			Name = "Symbol of Faith",
			Description = "Receive holy symbol for protection",
			MapId = 0x15
		},
		new() {
			Id = 0x0405,
			Name = "Aktemto Gas Mask",
			Description = "Get gas mask to survive the mine",
			MapId = 0x21
		},
		new() {
			Id = 0x0406,
			Name = "Through the Mine",
			Description = "Navigate the poisoned Aktemto Mine",
			MapId = 0x22
		},
		new() {
			Id = 0x0407,
			Name = "Keeleon Infiltration",
			Description = "Sneak into Keeleon Castle",
			MapId = 0x23
		},
		new() {
			Id = 0x0408,
			Name = "Boss: Balzack",
			Description = "Confront Balzack",
			MapId = 0x23,
			BossId = 0x82
		},
		new() {
			Id = 0x0409,
			Name = "Balzack Transforms",
			Description = "Balzack reveals his true form",
			MapId = 0x23
		},
		new() {
			Id = 0x0410,
			Name = "Escape",
			Description = "Flee from the transformed Balzack",
			MapId = 0x23
		},
		new() {
			Id = 0x0411,
			Name = "Chapter 4 Complete",
			Description = "Chapter ends with unfinished revenge"
		}
	];

	/// <summary>
	/// Balzack boss stats (first form).
	/// </summary>
	public static readonly BossStats BalzackStats = new() {
		Name = "Balzack",
		HP = 500,
		MP = 30,
		Attack = 65,
		Defense = 50,
		Agility = 35,
		ExperienceReward = 0, // Can't actually defeat
		GoldReward = 0,
		Abilities = ["Attack", "Blazemore", "Heal"]
	};

	/// <summary>
	/// Nara's spell progression.
	/// </summary>
	public static readonly SpellLearn[] NaraSpells = [
		new() { SpellId = 0x01, SpellName = "Heal", LearnLevel = 1 },
		new() { SpellId = 0x10, SpellName = "Antidote", LearnLevel = 2 },
		new() { SpellId = 0x02, SpellName = "Hurt", LearnLevel = 3 },
		new() { SpellId = 0x11, SpellName = "Outside", LearnLevel = 5 },
		new() { SpellId = 0x03, SpellName = "Healmore", LearnLevel = 8 },
		new() { SpellId = 0x12, SpellName = "Surround", LearnLevel = 10 }
	];

	/// <summary>
	/// Mara's spell progression.
	/// </summary>
	public static readonly SpellLearn[] MaraSpells = [
		new() { SpellId = 0x20, SpellName = "Blaze", LearnLevel = 1 },
		new() { SpellId = 0x21, SpellName = "Sap", LearnLevel = 2 },
		new() { SpellId = 0x22, SpellName = "Bang", LearnLevel = 4 },
		new() { SpellId = 0x23, SpellName = "Blazemore", LearnLevel = 7 },
		new() { SpellId = 0x24, SpellName = "Snowstorm", LearnLevel = 10 },
		new() { SpellId = 0x25, SpellName = "Bikill", LearnLevel = 12 }
	];
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
/// Chapter 4 map location.
/// </summary>
public class Chapter4Map {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public MapLocationType Type { get; set; }
	public bool HasShop { get; set; }
	public bool HasInn { get; set; }
	public bool HasChurch { get; set; }
	public string Notes { get; set; } = string.Empty;
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
/// Chapter 4 story event.
/// </summary>
public class Chapter4Event {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int MapId { get; set; }
	public int BossId { get; set; }
}

/// <summary>
/// Boss stats structure.
/// </summary>
public class BossStats {
	public string Name { get; set; } = string.Empty;
	public int HP { get; set; }
	public int MP { get; set; }
	public int Attack { get; set; }
	public int Defense { get; set; }
	public int Agility { get; set; }
	public int ExperienceReward { get; set; }
	public int GoldReward { get; set; }
	public string[] Abilities { get; set; } = [];
}

/// <summary>
/// Spell learning entry.
/// </summary>
public class SpellLearn {
	public int SpellId { get; set; }
	public string SpellName { get; set; } = string.Empty;
	public int LearnLevel { get; set; }
}
