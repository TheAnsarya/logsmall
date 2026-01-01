namespace DW4Lib.Audio;

/// <summary>
/// DW4 NES Sound and Music data structures.
/// Music uses the 2A03 sound chip with 2 pulse, 1 triangle, 1 noise, and 1 DPCM channel.
/// Sound effects share channels with music.
/// </summary>
public class MusicTrack {
	/// <summary>
	/// Track ID (0x00-0xFF).
	/// </summary>
	public byte Id { get; set; }

	/// <summary>
	/// Track name/label.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Track category.
	/// </summary>
	public MusicCategory Category { get; set; }

	/// <summary>
	/// Associated chapter(s), null for global tracks.
	/// </summary>
	public int[]? Chapters { get; set; }

	/// <summary>
	/// ROM bank containing track data.
	/// </summary>
	public int Bank { get; set; }

	/// <summary>
	/// ROM address of track data.
	/// </summary>
	public int RomAddress { get; set; }

	/// <summary>
	/// Track tempo (BPM approximation).
	/// </summary>
	public int Tempo { get; set; }

	/// <summary>
	/// Whether track loops.
	/// </summary>
	public bool Loops { get; set; } = true;

	/// <summary>
	/// Channel data for each sound channel.
	/// </summary>
	public ChannelData[] Channels { get; set; } = new ChannelData[5];
}

/// <summary>
/// Music categories.
/// </summary>
public enum MusicCategory {
	/// <summary>Title screen and menu music.</summary>
	Title,
	/// <summary>Overworld exploration music.</summary>
	Overworld,
	/// <summary>Town and village themes.</summary>
	Town,
	/// <summary>Castle and palace themes.</summary>
	Castle,
	/// <summary>Dungeon and cave themes.</summary>
	Dungeon,
	/// <summary>Tower themes.</summary>
	Tower,
	/// <summary>Battle music.</summary>
	Battle,
	/// <summary>Boss battle music.</summary>
	BossBattle,
	/// <summary>Victory fanfares.</summary>
	Victory,
	/// <summary>Sad/melancholy themes.</summary>
	Sad,
	/// <summary>Dramatic/tension themes.</summary>
	Dramatic,
	/// <summary>Chapter-specific themes.</summary>
	Chapter,
	/// <summary>Ending and credits.</summary>
	Ending,
	/// <summary>Short jingles.</summary>
	Jingle
}

/// <summary>
/// Sound channel data.
/// </summary>
public class ChannelData {
	/// <summary>
	/// Channel type.
	/// </summary>
	public ChannelType Type { get; set; }

	/// <summary>
	/// Sequence data (note/duration pairs).
	/// </summary>
	public byte[] SequenceData { get; set; } = [];

	/// <summary>
	/// Instrument/duty cycle setting.
	/// </summary>
	public byte Instrument { get; set; }

	/// <summary>
	/// Volume envelope.
	/// </summary>
	public byte VolumeEnvelope { get; set; }
}

/// <summary>
/// NES APU channel types.
/// </summary>
public enum ChannelType {
	Pulse1 = 0,
	Pulse2 = 1,
	Triangle = 2,
	Noise = 3,
	DPCM = 4
}

/// <summary>
/// Sound effect definition.
/// </summary>
public class SoundEffect {
	/// <summary>
	/// Sound effect ID.
	/// </summary>
	public byte Id { get; set; }

	/// <summary>
	/// Sound name/label.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Sound category.
	/// </summary>
	public SoundCategory Category { get; set; }

	/// <summary>
	/// Priority level (higher priority interrupts lower).
	/// </summary>
	public byte Priority { get; set; }

	/// <summary>
	/// Duration in frames.
	/// </summary>
	public byte Duration { get; set; }

	/// <summary>
	/// Channel used for playback.
	/// </summary>
	public ChannelType Channel { get; set; }

	/// <summary>
	/// Sound data bytes.
	/// </summary>
	public byte[] Data { get; set; } = [];
}

/// <summary>
/// Sound effect categories.
/// </summary>
public enum SoundCategory {
	/// <summary>Menu navigation sounds.</summary>
	Menu,
	/// <summary>Battle sounds (attacks, spells).</summary>
	Battle,
	/// <summary>Item and treasure sounds.</summary>
	Item,
	/// <summary>Status effect sounds.</summary>
	Status,
	/// <summary>Environment sounds.</summary>
	Environment,
	/// <summary>Character sounds.</summary>
	Character,
	/// <summary>System sounds (save, level up).</summary>
	System
}

/// <summary>
/// DW4 Music and Sound database.
/// </summary>
public static class AudioDatabase {
	// ============================================================
	// Music Track IDs
	// ============================================================

	/// <summary>Title screen theme.</summary>
	public const byte MusicTitle = 0x01;

	/// <summary>Prologue theme.</summary>
	public const byte MusicPrologue = 0x02;

	/// <summary>Chapter 1 overworld.</summary>
	public const byte MusicChapter1Overworld = 0x10;

	/// <summary>Chapter 2 overworld.</summary>
	public const byte MusicChapter2Overworld = 0x11;

	/// <summary>Chapter 3 overworld.</summary>
	public const byte MusicChapter3Overworld = 0x12;

	/// <summary>Chapter 4 overworld.</summary>
	public const byte MusicChapter4Overworld = 0x13;

	/// <summary>Chapter 5 overworld.</summary>
	public const byte MusicChapter5Overworld = 0x14;

	/// <summary>Town theme.</summary>
	public const byte MusicTown = 0x20;

	/// <summary>Castle theme.</summary>
	public const byte MusicCastle = 0x21;

	/// <summary>Cave/dungeon theme.</summary>
	public const byte MusicDungeon = 0x22;

	/// <summary>Tower theme.</summary>
	public const byte MusicTower = 0x23;

	/// <summary>Shrine theme.</summary>
	public const byte MusicShrine = 0x24;

	/// <summary>Battle theme.</summary>
	public const byte MusicBattle = 0x30;

	/// <summary>Boss battle theme.</summary>
	public const byte MusicBossBattle = 0x31;

	/// <summary>Final boss theme.</summary>
	public const byte MusicFinalBoss = 0x32;

	/// <summary>Victory fanfare.</summary>
	public const byte MusicVictory = 0x38;

	/// <summary>Game over.</summary>
	public const byte MusicGameOver = 0x39;

	/// <summary>Level up jingle.</summary>
	public const byte MusicLevelUp = 0x3A;

	/// <summary>Item obtained jingle.</summary>
	public const byte MusicItemObtained = 0x3B;

	/// <summary>Inn rest music.</summary>
	public const byte MusicInn = 0x40;

	/// <summary>Church/save music.</summary>
	public const byte MusicChurch = 0x41;

	/// <summary>Casino music.</summary>
	public const byte MusicCasino = 0x42;

	/// <summary>Wagon/party select music.</summary>
	public const byte MusicWagon = 0x43;

	/// <summary>Sad theme.</summary>
	public const byte MusicSad = 0x50;

	/// <summary>Tension/dramatic theme.</summary>
	public const byte MusicTension = 0x51;

	/// <summary>Romance theme.</summary>
	public const byte MusicRomance = 0x52;

	/// <summary>Ending theme.</summary>
	public const byte MusicEnding = 0x60;

	/// <summary>Credits theme.</summary>
	public const byte MusicCredits = 0x61;

	// ============================================================
	// Sound Effect IDs
	// ============================================================

	/// <summary>Cursor move.</summary>
	public const byte SfxCursor = 0x01;

	/// <summary>Menu select/confirm.</summary>
	public const byte SfxConfirm = 0x02;

	/// <summary>Menu cancel.</summary>
	public const byte SfxCancel = 0x03;

	/// <summary>Error/buzzer.</summary>
	public const byte SfxError = 0x04;

	/// <summary>Attack hit.</summary>
	public const byte SfxHit = 0x10;

	/// <summary>Critical hit.</summary>
	public const byte SfxCritical = 0x11;

	/// <summary>Attack miss.</summary>
	public const byte SfxMiss = 0x12;

	/// <summary>Enemy defeated.</summary>
	public const byte SfxDefeat = 0x13;

	/// <summary>Party member death.</summary>
	public const byte SfxDeath = 0x14;

	/// <summary>Heal spell.</summary>
	public const byte SfxHeal = 0x20;

	/// <summary>Attack spell.</summary>
	public const byte SfxSpellAttack = 0x21;

	/// <summary>Buff spell.</summary>
	public const byte SfxBuff = 0x22;

	/// <summary>Debuff spell.</summary>
	public const byte SfxDebuff = 0x23;

	/// <summary>Revive spell.</summary>
	public const byte SfxRevive = 0x24;

	/// <summary>Warp/teleport.</summary>
	public const byte SfxWarp = 0x25;

	/// <summary>Treasure chest open.</summary>
	public const byte SfxChest = 0x30;

	/// <summary>Door open.</summary>
	public const byte SfxDoor = 0x31;

	/// <summary>Stairs.</summary>
	public const byte SfxStairs = 0x32;

	/// <summary>Item use.</summary>
	public const byte SfxItemUse = 0x33;

	/// <summary>Poison damage.</summary>
	public const byte SfxPoison = 0x40;

	/// <summary>Sleep.</summary>
	public const byte SfxSleep = 0x41;

	/// <summary>Confusion.</summary>
	public const byte SfxConfuse = 0x42;

	/// <summary>Save game.</summary>
	public const byte SfxSave = 0x50;

	/// <summary>Level up.</summary>
	public const byte SfxLevelUp = 0x51;

	/// <summary>Party join.</summary>
	public const byte SfxJoin = 0x52;

	// ============================================================
	// All Music Tracks
	// ============================================================

	/// <summary>
	/// Get all defined music tracks.
	/// </summary>
	public static MusicTrack[] GetAllTracks() => [
		// Title/System
		new() { Id = MusicTitle, Name = "Title Screen", Category = MusicCategory.Title, Loops = true },
		new() { Id = MusicPrologue, Name = "Prologue", Category = MusicCategory.Dramatic, Loops = false },

		// Chapter Overworld Themes
		new() { Id = MusicChapter1Overworld, Name = "Chapter 1 Overworld", Category = MusicCategory.Chapter, Chapters = [0] },
		new() { Id = MusicChapter2Overworld, Name = "Chapter 2 Overworld", Category = MusicCategory.Chapter, Chapters = [1] },
		new() { Id = MusicChapter3Overworld, Name = "Chapter 3 Overworld", Category = MusicCategory.Chapter, Chapters = [2] },
		new() { Id = MusicChapter4Overworld, Name = "Chapter 4 Overworld", Category = MusicCategory.Chapter, Chapters = [3] },
		new() { Id = MusicChapter5Overworld, Name = "Chapter 5 Overworld", Category = MusicCategory.Overworld, Chapters = [4] },

		// Location Themes
		new() { Id = MusicTown, Name = "Town Theme", Category = MusicCategory.Town },
		new() { Id = MusicCastle, Name = "Castle Theme", Category = MusicCategory.Castle },
		new() { Id = MusicDungeon, Name = "Dungeon Theme", Category = MusicCategory.Dungeon },
		new() { Id = MusicTower, Name = "Tower Theme", Category = MusicCategory.Tower },
		new() { Id = MusicShrine, Name = "Shrine Theme", Category = MusicCategory.Dungeon },

		// Battle Themes
		new() { Id = MusicBattle, Name = "Battle Theme", Category = MusicCategory.Battle },
		new() { Id = MusicBossBattle, Name = "Boss Battle", Category = MusicCategory.BossBattle },
		new() { Id = MusicFinalBoss, Name = "Final Boss", Category = MusicCategory.BossBattle },

		// Jingles
		new() { Id = MusicVictory, Name = "Victory Fanfare", Category = MusicCategory.Victory, Loops = false },
		new() { Id = MusicGameOver, Name = "Game Over", Category = MusicCategory.Sad, Loops = false },
		new() { Id = MusicLevelUp, Name = "Level Up", Category = MusicCategory.Jingle, Loops = false },
		new() { Id = MusicItemObtained, Name = "Item Obtained", Category = MusicCategory.Jingle, Loops = false },

		// Service Locations
		new() { Id = MusicInn, Name = "Inn", Category = MusicCategory.Town },
		new() { Id = MusicChurch, Name = "Church", Category = MusicCategory.Castle },
		new() { Id = MusicCasino, Name = "Casino", Category = MusicCategory.Town },
		new() { Id = MusicWagon, Name = "Wagon", Category = MusicCategory.Town },

		// Story/Mood
		new() { Id = MusicSad, Name = "Sad Theme", Category = MusicCategory.Sad },
		new() { Id = MusicTension, Name = "Tension", Category = MusicCategory.Dramatic },
		new() { Id = MusicRomance, Name = "Romance", Category = MusicCategory.Town },

		// Ending
		new() { Id = MusicEnding, Name = "Ending Theme", Category = MusicCategory.Ending },
		new() { Id = MusicCredits, Name = "Credits", Category = MusicCategory.Ending }
	];

	/// <summary>
	/// Get all defined sound effects.
	/// </summary>
	public static SoundEffect[] GetAllSoundEffects() => [
		// Menu
		new() { Id = SfxCursor, Name = "Cursor Move", Category = SoundCategory.Menu, Priority = 1 },
		new() { Id = SfxConfirm, Name = "Confirm", Category = SoundCategory.Menu, Priority = 2 },
		new() { Id = SfxCancel, Name = "Cancel", Category = SoundCategory.Menu, Priority = 2 },
		new() { Id = SfxError, Name = "Error", Category = SoundCategory.Menu, Priority = 3 },

		// Battle
		new() { Id = SfxHit, Name = "Attack Hit", Category = SoundCategory.Battle, Priority = 5 },
		new() { Id = SfxCritical, Name = "Critical Hit", Category = SoundCategory.Battle, Priority = 6 },
		new() { Id = SfxMiss, Name = "Miss", Category = SoundCategory.Battle, Priority = 4 },
		new() { Id = SfxDefeat, Name = "Enemy Defeated", Category = SoundCategory.Battle, Priority = 7 },
		new() { Id = SfxDeath, Name = "Party Death", Category = SoundCategory.Battle, Priority = 8 },

		// Spells
		new() { Id = SfxHeal, Name = "Heal", Category = SoundCategory.Battle, Priority = 5 },
		new() { Id = SfxSpellAttack, Name = "Attack Spell", Category = SoundCategory.Battle, Priority = 6 },
		new() { Id = SfxBuff, Name = "Buff", Category = SoundCategory.Battle, Priority = 5 },
		new() { Id = SfxDebuff, Name = "Debuff", Category = SoundCategory.Battle, Priority = 5 },
		new() { Id = SfxRevive, Name = "Revive", Category = SoundCategory.Battle, Priority = 7 },
		new() { Id = SfxWarp, Name = "Warp", Category = SoundCategory.Battle, Priority = 6 },

		// Items/Environment
		new() { Id = SfxChest, Name = "Treasure Chest", Category = SoundCategory.Item, Priority = 5 },
		new() { Id = SfxDoor, Name = "Door Open", Category = SoundCategory.Environment, Priority = 3 },
		new() { Id = SfxStairs, Name = "Stairs", Category = SoundCategory.Environment, Priority = 3 },
		new() { Id = SfxItemUse, Name = "Item Use", Category = SoundCategory.Item, Priority = 4 },

		// Status
		new() { Id = SfxPoison, Name = "Poison", Category = SoundCategory.Status, Priority = 4 },
		new() { Id = SfxSleep, Name = "Sleep", Category = SoundCategory.Status, Priority = 4 },
		new() { Id = SfxConfuse, Name = "Confusion", Category = SoundCategory.Status, Priority = 4 },

		// System
		new() { Id = SfxSave, Name = "Save Game", Category = SoundCategory.System, Priority = 8 },
		new() { Id = SfxLevelUp, Name = "Level Up", Category = SoundCategory.System, Priority = 9 },
		new() { Id = SfxJoin, Name = "Party Join", Category = SoundCategory.System, Priority = 8 }
	];

	/// <summary>
	/// Get music track for a chapter's overworld.
	/// </summary>
	public static byte GetChapterOverworldMusic(int chapterId) => chapterId switch {
		0 => MusicChapter1Overworld,
		1 => MusicChapter2Overworld,
		2 => MusicChapter3Overworld,
		3 => MusicChapter4Overworld,
		4 => MusicChapter5Overworld,
		_ => MusicChapter5Overworld
	};

	/// <summary>
	/// Get music track by ID.
	/// </summary>
	public static MusicTrack? GetTrack(byte id) {
		return GetAllTracks().FirstOrDefault(t => t.Id == id);
	}

	/// <summary>
	/// Get sound effect by ID.
	/// </summary>
	public static SoundEffect? GetSoundEffect(byte id) {
		return GetAllSoundEffects().FirstOrDefault(s => s.Id == id);
	}
}
