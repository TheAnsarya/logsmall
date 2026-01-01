namespace DW4Lib.DataStructures;

/// <summary>
/// Dragon Warrior IV Chapter data structure.
/// Each chapter has different playable characters, mechanics, and story.
/// </summary>
public class Chapter {
	/// <summary>
	/// Chapter ID (0x00-0x04).
	/// </summary>
	public byte Id { get; set; }

	/// <summary>
	/// Chapter number (1-5).
	/// </summary>
	public int Number => Id + 1;

	/// <summary>
	/// Chapter name/title.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Protagonist character ID.
	/// </summary>
	public byte ProtagonistId { get; set; }

	/// <summary>
	/// Protagonist name.
	/// </summary>
	public string ProtagonistName { get; set; } = string.Empty;

	/// <summary>
	/// Party member IDs available in this chapter.
	/// </summary>
	public byte[] PartyMemberIds { get; set; } = [];

	/// <summary>
	/// NPC companion IDs (like Healie in Chapter 1).
	/// </summary>
	public byte[] CompanionIds { get; set; } = [];

	/// <summary>
	/// Location name where chapter starts.
	/// </summary>
	public string StartingLocation { get; set; } = string.Empty;

	/// <summary>
	/// Starting map ID.
	/// </summary>
	public int StartingMapId { get; set; }

	/// <summary>
	/// Starting X coordinate.
	/// </summary>
	public byte StartingX { get; set; }

	/// <summary>
	/// Starting Y coordinate.
	/// </summary>
	public byte StartingY { get; set; }

	/// <summary>
	/// Chapter objective/goal description.
	/// </summary>
	public string Objective { get; set; } = string.Empty;

	/// <summary>
	/// Description of the chapter for display/conversion.
	/// </summary>
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Starting location map ID (alias for StartingMapId for converter compatibility).
	/// </summary>
	public int StartLocationId => StartingMapId;

	/// <summary>
	/// Event flags that must be set to start this chapter.
	/// </summary>
	public int[] StartFlags { get; set; } = [];

	/// <summary>
	/// Event flags set when chapter ends.
	/// </summary>
	public int[] EndFlags { get; set; } = [];

	/// <summary>
	/// Event ID that triggers chapter end.
	/// </summary>
	public int EndEventId { get; set; }

	/// <summary>
	/// Array of playable character IDs (alias for PartyMemberIds for converter compatibility).
	/// </summary>
	public byte[] PlayableCharacters => PartyMemberIds;

	/// <summary>
	/// Whether wagon is available (Chapter 5 only).
	/// </summary>
	public bool HasWagon { get; set; }

	/// <summary>
	/// Whether tactics menu is available (Chapter 5 only).
	/// </summary>
	public bool HasTactics { get; set; }

	/// <summary>
	/// Special mechanics flags.
	/// </summary>
	public ChapterMechanics Mechanics { get; set; }
}

/// <summary>
/// Chapter-specific game mechanics flags.
/// </summary>
[Flags]
public enum ChapterMechanics : byte {
	None = 0x00,
	/// <summary>AI-controlled party members (Chapter 2, 4).</summary>
	AiPartyMembers = 0x01,
	/// <summary>Merchant mechanics (Chapter 3).</summary>
	MerchantAbilities = 0x02,
	/// <summary>Single character - solo protagonist (Chapter 1).</summary>
	SoloProtagonist = 0x04,
	/// <summary>Magic-focused party (Chapter 4).</summary>
	MagicFocus = 0x08,
	/// <summary>Wagon system available (Chapter 5).</summary>
	WagonParty = 0x10,
	/// <summary>Tactics menu available (Chapter 5).</summary>
	TacticsMenu = 0x20,
	/// <summary>NPC companion follows (Chapter 1 Healie).</summary>
	NpcCompanion = 0x40,
	/// <summary>Full player control over all party members.</summary>
	FullControl = 0x80
}

/// <summary>
/// Battle tactics available in Chapter 5.
/// </summary>
public enum BattleTactic : byte {
	/// <summary>Balanced approach.</summary>
	Normal = 0x00,
	/// <summary>Minimize magic use, preserve MP.</summary>
	SaveMP = 0x01,
	/// <summary>Maximum aggression, all-out attack.</summary>
	Offensive = 0x02,
	/// <summary>Protect party, defensive stance.</summary>
	Defensive = 0x03,
	/// <summary>Experimental AI behavior.</summary>
	TryOut = 0x04,
	/// <summary>Physical attacks only, no magic.</summary>
	UseNoMP = 0x05
}

/// <summary>
/// Day/Night cycle time period.
/// </summary>
public enum TimePeriod : byte {
	Dawn = 0,
	Day = 1,
	Dusk = 2,
	Night = 3
}

/// <summary>
/// Day/Night cycle state.
/// </summary>
public class DayNightCycle {
	/// <summary>
	/// Current time value (0x00-0xCB, wraps).
	/// </summary>
	public byte TimeValue { get; set; }

	/// <summary>
	/// Start of Dawn period.
	/// </summary>
	public int DawnStart => 0x00;

	/// <summary>
	/// Start of Day period.
	/// </summary>
	public int DayStart => 0x3F;

	/// <summary>
	/// Start of Dusk period.
	/// </summary>
	public int DuskStart => 0x8F;

	/// <summary>
	/// Start of Night period.
	/// </summary>
	public int NightStart => 0xA0;

	/// <summary>
	/// Maximum time value before wrap.
	/// </summary>
	public int MaxValue => 0xCB;

	/// <summary>
	/// Get the time period for a given time value.
	/// </summary>
	public TimePeriod GetPeriod(int timeValue) {
		return timeValue switch {
			< 0x3F => TimePeriod.Dawn,
			< 0x8F => TimePeriod.Day,
			< 0xA0 => TimePeriod.Dusk,
			_ => TimePeriod.Night
		};
	}

	/// <summary>
	/// Get the current time period.
	/// </summary>
	public TimePeriod GetPeriod() => GetPeriod(TimeValue);

	/// <summary>
	/// Check if it's currently night time.
	/// </summary>
	public bool IsNight => TimeValue >= NightStart && TimeValue <= MaxValue;

	/// <summary>
	/// Check if it's currently day time.
	/// </summary>
	public bool IsDay => TimeValue >= DayStart && TimeValue < DuskStart;
}

/// <summary>
/// Chapter-specific story event.
/// </summary>
public class ChapterEvent {
	/// <summary>
	/// Event ID.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Event name/description.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Chapter this event belongs to.
	/// </summary>
	public byte ChapterId { get; set; }

	/// <summary>
	/// Map where event triggers.
	/// </summary>
	public int MapId { get; set; }

	/// <summary>
	/// X coordinate trigger (or -1 for any).
	/// </summary>
	public int TriggerX { get; set; } = -1;

	/// <summary>
	/// Y coordinate trigger (or -1 for any).
	/// </summary>
	public int TriggerY { get; set; } = -1;

	/// <summary>
	/// Event flag that must be set.
	/// </summary>
	public int RequiredFlag { get; set; } = -1;

	/// <summary>
	/// Event flag that must NOT be set.
	/// </summary>
	public int BlockingFlag { get; set; } = -1;

	/// <summary>
	/// Event type.
	/// </summary>
	public ChapterEventType Type { get; set; }

	/// <summary>
	/// Dialog script ID to execute.
	/// </summary>
	public int DialogId { get; set; }

	/// <summary>
	/// Flag to set when event completes.
	/// </summary>
	public int SetFlag { get; set; } = -1;
}

/// <summary>
/// Types of chapter events.
/// </summary>
public enum ChapterEventType {
	Dialog,
	Battle,
	CharacterJoin,
	ItemReceive,
	ChapterTransition,
	CutScene,
	MapChange,
	ShopOpen,
	QuestUpdate
}

/// <summary>
/// Database of all chapters with their data.
/// </summary>
public static class ChapterDatabase {
	/// <summary>
	/// All 5 chapters in Dragon Warrior IV.
	/// </summary>
	public static readonly Chapter[] AllChapters = [
		new() {
			Id = 0x00,
			Name = "Chapter 1: The Royal Soldiers",
			Description = "Ragnar McRyan investigates missing children in Burland",
			ProtagonistId = 0x06,
			ProtagonistName = "Ragnar",
			PartyMemberIds = [0x06], // Ragnar only
			CompanionIds = [0xC5],   // Healie
			StartingLocation = "Burland Castle",
			StartingMapId = 0x02,
			StartingX = 0x08,
			StartingY = 0x0A,
			Objective = "Investigate missing children from Izmit village",
			StartFlags = [],
			EndFlags = [0x0100],
			EndEventId = 0x0100,
			HasWagon = false,
			HasTactics = false,
			Mechanics = ChapterMechanics.SoloProtagonist | ChapterMechanics.NpcCompanion
		},
		new() {
			Id = 0x01,
			Name = "Chapter 2: Princess Alena's Adventure",
			Description = "Princess Alena escapes Santeem Castle to prove herself",
			ProtagonistId = 0x07,
			ProtagonistName = "Alena",
			PartyMemberIds = [0x07, 0x01, 0x04], // Alena, Cristo, Brey
			CompanionIds = [],
			StartingLocation = "Santeem Castle",
			StartingMapId = 0x01,
			StartingX = 0x10,
			StartingY = 0x08,
			Objective = "Win the Endor tournament",
			StartFlags = [0x0100],
			EndFlags = [0x0200],
			EndEventId = 0x0200,
			HasWagon = false,
			HasTactics = false,
			Mechanics = ChapterMechanics.AiPartyMembers
		},
		new() {
			Id = 0x02,
			Name = "Chapter 3: Taloon the Arms Merchant",
			Description = "Taloon the merchant pursues his dream of opening a shop",
			ProtagonistId = 0x05,
			ProtagonistName = "Taloon",
			PartyMemberIds = [0x05], // Taloon only (hires NPCs)
			CompanionIds = [0xC7, 0xC8], // Laurent, Strom
			StartingLocation = "Lakanaba",
			StartingMapId = 0x16,
			StartingX = 0x0C,
			StartingY = 0x0E,
			Objective = "Open a shop in Endor",
			StartFlags = [0x0200],
			EndFlags = [0x0300],
			EndEventId = 0x0300,
			HasWagon = false,
			HasTactics = false,
			Mechanics = ChapterMechanics.SoloProtagonist | ChapterMechanics.MerchantAbilities
		},
		new() {
			Id = 0x03,
			Name = "Chapter 4: The Sisters of Monbaraba",
			Description = "Nara and Mara seek revenge for their father's murder",
			ProtagonistId = 0x02,
			ProtagonistName = "Nara",
			PartyMemberIds = [0x02, 0x03], // Nara, Mara
			CompanionIds = [0xC6], // Orin
			StartingLocation = "Monbaraba",
			StartingMapId = 0x15,
			StartingX = 0x08,
			StartingY = 0x0A,
			Objective = "Avenge their father, defeat Balzack",
			StartFlags = [0x0300],
			EndFlags = [0x0400],
			EndEventId = 0x0400,
			HasWagon = false,
			HasTactics = false,
			Mechanics = ChapterMechanics.AiPartyMembers | ChapterMechanics.MagicFocus
		},
		new() {
			Id = 0x04,
			Name = "Chapter 5: The Chosen Ones",
			Description = "The Hero unites all chosen ones to defeat the Lord of the Underworld",
			ProtagonistId = 0x00,
			ProtagonistName = "Hero",
			PartyMemberIds = [0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07],
			CompanionIds = [0xC9, 0xCA, 0xCB, 0xCC], // Hector, Panon, Lucia, Doran
			StartingLocation = "Hero's Village",
			StartingMapId = 0x14,
			StartingX = 0x08,
			StartingY = 0x0C,
			Objective = "Defeat the Lord of the Underworld",
			StartFlags = [0x0400],
			EndFlags = [0x0500],
			EndEventId = 0x0500,
			HasWagon = true,
			HasTactics = true,
			Mechanics = ChapterMechanics.WagonParty | ChapterMechanics.TacticsMenu | ChapterMechanics.FullControl
		}
	];

	/// <summary>
	/// Get chapter by ID.
	/// </summary>
	public static Chapter? GetChapter(byte id) {
		return id < AllChapters.Length ? AllChapters[id] : null;
	}

	/// <summary>
	/// Get chapter by number (1-5).
	/// </summary>
	public static Chapter? GetChapterByNumber(int number) {
		return number >= 1 && number <= 5 ? AllChapters[number - 1] : null;
	}
}
