namespace DW4Lib.DataStructures.Chapter1;

/// <summary>
/// Chapter 1 dialog and NPC definitions.
/// Contains all story-critical dialog and NPC placement.
/// </summary>
public static class Chapter1Dialog {
	/// <summary>
	/// King of Burland - initial quest assignment.
	/// </summary>
	public static readonly DialogEntry KingInitialQuest = new() {
		Id = 0x0100,
		Speaker = "King of Burland",
		Lines = [
			"Ragnar, my loyal soldier!",
			"Children have been vanishing",
			"from villages across our land.",
			"I need you to investigate",
			"and bring them home safely.",
			"Begin in Izmit Village.",
			"May the Goddess protect you."
		]
	};

	/// <summary>
	/// Izmit villager - sets up cave clue.
	/// </summary>
	public static readonly DialogEntry IzmitVillagerClue = new() {
		Id = 0x0120,
		Speaker = "Worried Mother",
		Lines = [
			"My child... my poor child!",
			"A traveler spoke of strange",
			"lights in the cave west",
			"of our village at night.",
			"Could the children be there?"
		]
	};

	/// <summary>
	/// Healie introduction.
	/// </summary>
	public static readonly DialogEntry HealieIntro = new() {
		Id = 0x0130,
		Speaker = "Healie",
		Lines = [
			"*bounce bounce*",
			"Oh! A human! You came!",
			"I'm Healie! I was looking",
			"for someone brave to help!",
			"The children... they were",
			"taken to Loch Tower!",
			"I'll come with you!",
			"I can heal your wounds!"
		]
	};

	/// <summary>
	/// Healie explains the tower.
	/// </summary>
	public static readonly DialogEntry HealieTowerInfo = new() {
		Id = 0x0140,
		Speaker = "Healie",
		Lines = [
			"*bounce*",
			"Loch Tower is to the west.",
			"A monster made its lair",
			"in the basement. That's",
			"where the children are!",
			"Please, we must save them!"
		]
	};

	/// <summary>
	/// Boss encounter.
	/// </summary>
	public static readonly DialogEntry BossEncounter = new() {
		Id = 0x0155,
		Speaker = "Chameleon Humanoid",
		Lines = [
			"Sssso... a soldier from",
			"Burland, is it?",
			"These children are MINE!",
			"You cannot have them!",
			"Prepare to die, human!"
		]
	};

	/// <summary>
	/// Children rescued.
	/// </summary>
	public static readonly DialogEntry ChildrenRescued = new() {
		Id = 0x0160,
		Speaker = "Child",
		Lines = [
			"Thank you, mister soldier!",
			"We were so scared!",
			"The monster was going to",
			"turn us all into monsters!",
			"Can you take us home?"
		]
	};

	/// <summary>
	/// King's praise on return.
	/// </summary>
	public static readonly DialogEntry KingPraise = new() {
		Id = 0x0170,
		Speaker = "King of Burland",
		Lines = [
			"Ragnar! You've returned!",
			"And the children are safe!",
			"You are a true hero of",
			"Burland! I am so proud!",
			"But Ragnar... there is",
			"something troubling me.",
			"Whispers of a great evil",
			"gathering in distant lands.",
			"I sense your destiny lies",
			"beyond our borders..."
		]
	};

	/// <summary>
	/// Healie's farewell (becomes human in Chapter 5).
	/// </summary>
	public static readonly DialogEntry HealieFarewell = new() {
		Id = 0x0180,
		Speaker = "Healie",
		Lines = [
			"*bounce bounce*",
			"Ragnar! You did it!",
			"The children are saved!",
			"I... I want to be human",
			"someday, like you.",
			"Maybe we'll meet again!",
			"Until then... stay safe!",
			"*bounce away*"
		]
	};

	/// <summary>
	/// All Chapter 1 dialog entries.
	/// </summary>
	public static readonly DialogEntry[] AllDialog = [
		KingInitialQuest,
		IzmitVillagerClue,
		HealieIntro,
		HealieTowerInfo,
		BossEncounter,
		ChildrenRescued,
		KingPraise,
		HealieFarewell
	];
}

/// <summary>
/// Dialog entry structure.
/// </summary>
public class DialogEntry {
	public int Id { get; set; }
	public string Speaker { get; set; } = string.Empty;
	public string[] Lines { get; set; } = [];

	/// <summary>
	/// Get dialog as single string with line breaks.
	/// </summary>
	public string GetFullText() => string.Join("\n", Lines);

	/// <summary>
	/// Get total character count for ROM allocation.
	/// </summary>
	public int GetCharacterCount() => string.Join("", Lines).Length;
}

/// <summary>
/// Chapter 1 NPC definitions.
/// </summary>
public static class Chapter1NPCs {
	/// <summary>
	/// King of Burland.
	/// </summary>
	public static readonly NpcDefinition KingOfBurland = new() {
		Id = 0x01,
		Name = "King of Burland",
		SpriteId = 0x20,
		MapId = 0x02,
		X = 0x08,
		Y = 0x02,
		Facing = Direction.Down,
		DialogIds = [0x0100, 0x0170],
		IsStationary = true
	};

	/// <summary>
	/// Worried Mother in Izmit.
	/// </summary>
	public static readonly NpcDefinition WorriedMother = new() {
		Id = 0x10,
		Name = "Worried Mother",
		SpriteId = 0x30,
		MapId = 0x12,
		X = 0x06,
		Y = 0x08,
		Facing = Direction.Down,
		DialogIds = [0x0120],
		MovementPattern = MovementPattern.Wander
	};

	/// <summary>
	/// Healie the Healslime.
	/// </summary>
	public static readonly NpcDefinition Healie = new() {
		Id = Chapter1Data.HealieId,
		Name = "Healie",
		SpriteId = 0x45, // Healslime sprite
		MapId = 0x25,   // Cave
		X = 0x08,
		Y = 0x0A,
		Facing = Direction.Down,
		DialogIds = [0x0130, 0x0140],
		IsCompanion = true,
		CompanionBehavior = CompanionBehavior.FollowAndHeal
	};

	/// <summary>
	/// Chameleon Humanoid boss.
	/// </summary>
	public static readonly NpcDefinition ChameleonHumanoid = new() {
		Id = 0x80,
		Name = "Chameleon Humanoid",
		SpriteId = 0x60, // Boss sprite
		MapId = 0x26,    // Tower Basement
		X = 0x10,
		Y = 0x08,
		Facing = Direction.Down,
		DialogIds = [0x0155],
		IsBoss = true,
		BossStats = new BossStats {
			HP = 350,
			MP = 0,
			Attack = 35,
			Defense = 28,
			Agility = 20,
			ExperienceReward = 400,
			GoldReward = 150
		}
	};

	/// <summary>
	/// Captive children (group NPC).
	/// </summary>
	public static readonly NpcDefinition CaptiveChildren = new() {
		Id = 0x81,
		Name = "Captive Children",
		SpriteId = 0x32, // Child sprite
		MapId = 0x26,
		X = 0x10,
		Y = 0x0A,
		Facing = Direction.Up,
		DialogIds = [0x0160],
		IsStationary = true,
		AppearFlag = Chapter1Data.CompletionFlag - 1 // After boss defeat
	};

	/// <summary>
	/// Castle Guards (generic).
	/// </summary>
	public static readonly NpcDefinition[] BurlandGuards = [
		new() {
			Id = 0x02,
			Name = "Castle Guard",
			SpriteId = 0x21,
			MapId = 0x02,
			X = 0x04,
			Y = 0x06,
			Facing = Direction.Right,
			GenericDialog = "All hail Ragnar!|Good luck on your mission!",
			IsStationary = true
		},
		new() {
			Id = 0x03,
			Name = "Castle Guard",
			SpriteId = 0x21,
			MapId = 0x02,
			X = 0x0C,
			Y = 0x06,
			Facing = Direction.Left,
			GenericDialog = "The King is counting on you!",
			IsStationary = true
		}
	];

	/// <summary>
	/// All Chapter 1 NPCs.
	/// </summary>
	public static IEnumerable<NpcDefinition> GetAllNPCs() {
		yield return KingOfBurland;
		yield return WorriedMother;
		yield return Healie;
		yield return ChameleonHumanoid;
		yield return CaptiveChildren;
		foreach (var guard in BurlandGuards) {
			yield return guard;
		}
	}
}

/// <summary>
/// NPC definition.
/// </summary>
public class NpcDefinition {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int SpriteId { get; set; }
	public int MapId { get; set; }
	public byte X { get; set; }
	public byte Y { get; set; }
	public Direction Facing { get; set; }
	public int[] DialogIds { get; set; } = [];
	public string? GenericDialog { get; set; }
	public bool IsStationary { get; set; }
	public MovementPattern MovementPattern { get; set; } = MovementPattern.None;
	public bool IsCompanion { get; set; }
	public CompanionBehavior CompanionBehavior { get; set; }
	public bool IsBoss { get; set; }
	public BossStats? BossStats { get; set; }
	public int AppearFlag { get; set; } = -1;
	public int DisappearFlag { get; set; } = -1;
}

/// <summary>
/// Direction enum.
/// </summary>
public enum Direction {
	Up = 0,
	Right = 1,
	Down = 2,
	Left = 3
}

/// <summary>
/// NPC movement patterns.
/// </summary>
public enum MovementPattern {
	None,
	Wander,
	Patrol,
	Follow
}

/// <summary>
/// Companion behavior types.
/// </summary>
public enum CompanionBehavior {
	None,
	FollowOnly,
	FollowAndHeal,
	FollowAndFight
}

/// <summary>
/// Boss stats structure.
/// </summary>
public class BossStats {
	public int HP { get; set; }
	public int MP { get; set; }
	public int Attack { get; set; }
	public int Defense { get; set; }
	public int Agility { get; set; }
	public int ExperienceReward { get; set; }
	public int GoldReward { get; set; }
	public byte[] SpellList { get; set; } = [];
	public byte[] SkillList { get; set; } = [];
}
