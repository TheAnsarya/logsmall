namespace DW4Lib.DataStructures.Chapter2;

/// <summary>
/// Chapter 2 "Princess Alena's Adventure" specific data.
/// Protagonist: Princess Alena
/// Party: Alena (controllable), Cristo, Brey (AI-controlled)
/// Location: Santeem Kingdom region
/// </summary>
public static class Chapter2Data {
	/// <summary>
	/// Chapter 2 ID.
	/// </summary>
	public const byte ChapterId = 0x01;

	/// <summary>
	/// Alena's character ID.
	/// </summary>
	public const byte AlenaId = 0x07;

	/// <summary>
	/// Cristo's character ID.
	/// </summary>
	public const byte CristoId = 0x01;

	/// <summary>
	/// Brey's character ID.
	/// </summary>
	public const byte BreyId = 0x04;

	/// <summary>
	/// Alena's starting stats at level 1.
	/// </summary>
	public static readonly CharacterStats AlenaStartingStats = new() {
		Level = 1,
		HP = 25,
		MP = 0,
		Strength = 10,
		Agility = 14,
		Vitality = 8,
		Intelligence = 6,
		Luck = 8
	};

	/// <summary>
	/// Cristo's starting stats.
	/// </summary>
	public static readonly CharacterStats CristoStartingStats = new() {
		Level = 1,
		HP = 22,
		MP = 8,
		Strength = 8,
		Agility = 6,
		Vitality = 7,
		Intelligence = 10,
		Luck = 7
	};

	/// <summary>
	/// Brey's starting stats.
	/// </summary>
	public static readonly CharacterStats BreyStartingStats = new() {
		Level = 1,
		HP = 18,
		MP = 12,
		Strength = 4,
		Agility = 8,
		Vitality = 5,
		Intelligence = 15,
		Luck = 10
	};

	/// <summary>
	/// Chapter 2 accessible maps.
	/// </summary>
	public static readonly Chapter2Map[] Maps = [
		new() {
			Id = 0x01,
			Name = "Santeem Castle",
			Type = MapLocationType.Castle,
			HasShop = true,
			HasInn = true,
			HasChurch = true
		},
		new() {
			Id = 0x10,
			Name = "Surene Village",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = true
		},
		new() {
			Id = 0x11,
			Name = "Tempe Village",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = false
		},
		new() {
			Id = 0x13,
			Name = "Frenor Town",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = true
		},
		new() {
			Id = 0x30,
			Name = "Cave of Tempe",
			Type = MapLocationType.Cave,
			HasShop = false,
			HasInn = false,
			HasChurch = false
		},
		new() {
			Id = 0x08,
			Name = "Endor",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = true
		},
		new() {
			Id = 0x09,
			Name = "Colosseum",
			Type = MapLocationType.Arena,
			HasShop = false,
			HasInn = false,
			HasChurch = false
		}
	];

	/// <summary>
	/// Chapter 2 story events in order.
	/// </summary>
	public static readonly Chapter2Event[] Events = [
		new() {
			Id = 0x0201,
			Name = "Escape from Santeem",
			Description = "Alena escapes through the castle window",
			MapId = 0x01
		},
		new() {
			Id = 0x0202,
			Name = "Cristo and Brey follow",
			Description = "The advisors catch up to Alena"
		},
		new() {
			Id = 0x0203,
			Name = "Tempe Golden Bracelet",
			Description = "Find the golden bracelet for the children",
			MapId = 0x11
		},
		new() {
			Id = 0x0204,
			Name = "Frenor Fake Princess",
			Description = "Deal with the imposter situation",
			MapId = 0x13
		},
		new() {
			Id = 0x0205,
			Name = "King's Dream",
			Description = "Learn about the tournament from the King",
			MapId = 0x01
		},
		new() {
			Id = 0x0206,
			Name = "Endor Tournament Entry",
			Description = "Register for the Colosseum tournament",
			MapId = 0x08
		},
		new() {
			Id = 0x0207,
			Name = "Tournament Battles",
			Description = "Fight through the tournament rounds",
			MapId = 0x09
		},
		new() {
			Id = 0x0208,
			Name = "Champion Victory",
			Description = "Defeat the tournament champion",
			MapId = 0x09
		},
		new() {
			Id = 0x0209,
			Name = "King Disappearance",
			Description = "Return to find the King has vanished",
			MapId = 0x01
		},
		new() {
			Id = 0x0210,
			Name = "Chapter 2 Complete",
			Description = "Chapter 2 ends with King's disappearance"
		}
	];

	/// <summary>
	/// Tournament battle sequence.
	/// </summary>
	public static readonly TournamentBattle[] TournamentBattles = [
		new() { Round = 1, OpponentName = "Hun", OpponentHp = 40, ExpReward = 25 },
		new() { Round = 2, OpponentName = "Roric", OpponentHp = 55, ExpReward = 35 },
		new() { Round = 3, OpponentName = "Vivian", OpponentHp = 70, ExpReward = 50 },
		new() { Round = 4, OpponentName = "Sampson", OpponentHp = 90, ExpReward = 80 },
		new() { Round = 5, OpponentName = "Linguar", OpponentHp = 120, ExpReward = 150 }
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
/// Chapter 2 map location.
/// </summary>
public class Chapter2Map {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public MapLocationType Type { get; set; }
	public bool HasShop { get; set; }
	public bool HasInn { get; set; }
	public bool HasChurch { get; set; }
}

/// <summary>
/// Map location types (extended for Chapter 2).
/// </summary>
public enum MapLocationType {
	Castle,
	Town,
	Cave,
	Dungeon,
	Tower,
	Shrine,
	Overworld,
	Arena
}

/// <summary>
/// Chapter 2 story event.
/// </summary>
public class Chapter2Event {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int MapId { get; set; }
}

/// <summary>
/// Tournament battle definition.
/// </summary>
public class TournamentBattle {
	public int Round { get; set; }
	public string OpponentName { get; set; } = string.Empty;
	public int OpponentHp { get; set; }
	public int ExpReward { get; set; }
}
