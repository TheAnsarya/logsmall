namespace DW4Lib.DataStructures.Chapter1;

/// <summary>
/// Chapter 1 event flags for "The Royal Soldiers".
/// Each flag represents a story/game state milestone.
/// </summary>
public static class Chapter1Flags {
	// ========================================
	// Chapter 1 Event Flag Definitions
	// ========================================

	// Range: 0x0001 - 0x00FF (Chapter 1 story flags)

	/// <summary>
	/// Chapter 1 has started - Ragnar received orders from King.
	/// </summary>
	public const ushort ChapterStarted = 0x0001;

	/// <summary>
	/// Spoke to villagers in Izmit about missing children.
	/// </summary>
	public const ushort TalkedToIzmitVillagers = 0x0002;

	/// <summary>
	/// Learned about suspicious cave west of Izmit.
	/// </summary>
	public const ushort HeardCaveRumor = 0x0003;

	/// <summary>
	/// Found and recruited Healie in the cave.
	/// </summary>
	public const ushort MetHealie = 0x0004;

	/// <summary>
	/// Healie told Ragnar about Loch Tower.
	/// </summary>
	public const ushort LearnedLochTowerLocation = 0x0005;

	/// <summary>
	/// Entered Loch Tower basement area.
	/// </summary>
	public const ushort ReachedTowerBasement = 0x0006;

	/// <summary>
	/// Defeated the Chameleon Humanoid boss.
	/// </summary>
	public const ushort DefeatedBoss = 0x0007;

	/// <summary>
	/// Freed the kidnapped children.
	/// </summary>
	public const ushort RescuedChildren = 0x0008;

	/// <summary>
	/// Reported success to the King of Burland.
	/// </summary>
	public const ushort ReportedToKing = 0x0009;

	/// <summary>
	/// Chapter 1 complete - King mentioned hero prophecy.
	/// </summary>
	public const ushort ChapterComplete = 0x000A;

	// ========================================
	// Optional/Side Content Flags
	// ========================================

	// Range: 0x0010 - 0x001F (optional events)

	/// <summary>
	/// Found the Wing of Wyvern in Loch Tower.
	/// </summary>
	public const ushort FoundWingOfWyvern = 0x0010;

	/// <summary>
	/// Opened chest with Leather Hat in Izmit.
	/// </summary>
	public const ushort GotLeatherHat = 0x0011;

	/// <summary>
	/// Opened chest with Medical Herb stash.
	/// </summary>
	public const ushort GotHerbStash = 0x0012;

	/// <summary>
	/// Slept at Izmit Inn at least once.
	/// </summary>
	public const ushort SleptAtIzmitInn = 0x0013;

	/// <summary>
	/// Saved game at Burland Church.
	/// </summary>
	public const ushort SavedAtBurlandChurch = 0x0014;

	/// <summary>
	/// Bought equipment from Burland weapon shop.
	/// </summary>
	public const ushort ShoppedAtBurland = 0x0015;

	// ========================================
	// Treasure Chest Flags
	// ========================================

	// Range: 0x0020 - 0x003F (treasure chests)

	/// <summary>
	/// Burland Castle treasure: Medical Herb.
	/// </summary>
	public const ushort ChestBurland01 = 0x0020;

	/// <summary>
	/// Burland Castle treasure: 50 Gold.
	/// </summary>
	public const ushort ChestBurland02 = 0x0021;

	/// <summary>
	/// Izmit Village treasure: Leather Hat.
	/// </summary>
	public const ushort ChestIzmit01 = 0x0022;

	/// <summary>
	/// Izmit Village treasure: Antidote Herb.
	/// </summary>
	public const ushort ChestIzmit02 = 0x0023;

	/// <summary>
	/// Cave treasure: Iron Shield.
	/// </summary>
	public const ushort ChestCave01 = 0x0024;

	/// <summary>
	/// Cave treasure: 120 Gold.
	/// </summary>
	public const ushort ChestCave02 = 0x0025;

	/// <summary>
	/// Loch Tower 1F: Medical Herb.
	/// </summary>
	public const ushort ChestTower1F_01 = 0x0026;

	/// <summary>
	/// Loch Tower 2F: Chain Sickle.
	/// </summary>
	public const ushort ChestTower2F_01 = 0x0027;

	/// <summary>
	/// Loch Tower basement: Wing of Wyvern.
	/// </summary>
	public const ushort ChestTowerB1_01 = 0x0028;

	/// <summary>
	/// Loch Tower basement: 200 Gold.
	/// </summary>
	public const ushort ChestTowerB1_02 = 0x0029;

	/// <summary>
	/// Loch Tower basement: Leather Armor.
	/// </summary>
	public const ushort ChestTowerB1_03 = 0x002A;

	/// <summary>
	/// Loch Tower boss room: Sword of Malice (post-boss).
	/// </summary>
	public const ushort ChestTowerBoss = 0x002B;

	// ========================================
	// Flag Utility Methods
	// ========================================

	/// <summary>
	/// Get all story flags in order.
	/// </summary>
	public static ushort[] GetStoryFlags() => [
		ChapterStarted,
		TalkedToIzmitVillagers,
		HeardCaveRumor,
		MetHealie,
		LearnedLochTowerLocation,
		ReachedTowerBasement,
		DefeatedBoss,
		RescuedChildren,
		ReportedToKing,
		ChapterComplete
	];

	/// <summary>
	/// Get all treasure chest flags.
	/// </summary>
	public static ushort[] GetTreasureFlags() => [
		ChestBurland01, ChestBurland02,
		ChestIzmit01, ChestIzmit02,
		ChestCave01, ChestCave02,
		ChestTower1F_01, ChestTower2F_01,
		ChestTowerB1_01, ChestTowerB1_02, ChestTowerB1_03,
		ChestTowerBoss
	];

	/// <summary>
	/// Check if a flag is a story progress flag.
	/// </summary>
	public static bool IsStoryFlag(ushort flag) =>
		flag >= 0x0001 && flag <= 0x000F;

	/// <summary>
	/// Check if a flag is a treasure chest flag.
	/// </summary>
	public static bool IsTreasureFlag(ushort flag) =>
		flag >= 0x0020 && flag <= 0x003F;

	/// <summary>
	/// Get the next story flag after completing the given one.
	/// </summary>
	public static ushort GetNextStoryFlag(ushort currentFlag) {
		return currentFlag switch {
			ChapterStarted => TalkedToIzmitVillagers,
			TalkedToIzmitVillagers => HeardCaveRumor,
			HeardCaveRumor => MetHealie,
			MetHealie => LearnedLochTowerLocation,
			LearnedLochTowerLocation => ReachedTowerBasement,
			ReachedTowerBasement => DefeatedBoss,
			DefeatedBoss => RescuedChildren,
			RescuedChildren => ReportedToKing,
			ReportedToKing => ChapterComplete,
			_ => 0x0000
		};
	}

	/// <summary>
	/// Get descriptive name for a flag.
	/// </summary>
	public static string GetFlagName(ushort flag) {
		return flag switch {
			ChapterStarted => "Chapter Started",
			TalkedToIzmitVillagers => "Talked to Izmit Villagers",
			HeardCaveRumor => "Heard Cave Rumor",
			MetHealie => "Met Healie",
			LearnedLochTowerLocation => "Learned Loch Tower Location",
			ReachedTowerBasement => "Reached Tower Basement",
			DefeatedBoss => "Defeated Boss",
			RescuedChildren => "Rescued Children",
			ReportedToKing => "Reported to King",
			ChapterComplete => "Chapter Complete",
			FoundWingOfWyvern => "Found Wing of Wyvern",
			GotLeatherHat => "Got Leather Hat",
			GotHerbStash => "Got Herb Stash",
			_ when IsTreasureFlag(flag) => $"Treasure Chest 0x{flag:X4}",
			_ => $"Unknown Flag 0x{flag:X4}"
		};
	}

	/// <summary>
	/// Get percentage completion based on story flags.
	/// </summary>
	public static int GetStoryCompletion(SaveData save) {
		var storyFlags = GetStoryFlags();
		int set = 0;
		foreach (var flag in storyFlags) {
			if (save.World.GetEventFlag(flag)) set++;
		}
		return (set * 100) / storyFlags.Length;
	}

	/// <summary>
	/// Get percentage of treasure chests opened.
	/// </summary>
	public static int GetTreasureCompletion(SaveData save) {
		var treasureFlags = GetTreasureFlags();
		int opened = 0;
		foreach (var flag in treasureFlags) {
			if (save.World.IsChestOpened(flag)) opened++;
		}
		return (opened * 100) / treasureFlags.Length;
	}
}

/// <summary>
/// Chapter 1 save state helper for creating test saves.
/// </summary>
public static class Chapter1SaveHelper {
	/// <summary>
	/// Create a fresh Chapter 1 start save.
	/// </summary>
	public static SaveData CreateChapter1Start() {
		var save = SaveData.CreateChapter1Start();
		save.World.SetEventFlag(Chapter1Flags.ChapterStarted);
		return save;
	}

	/// <summary>
	/// Create a save at the point where Healie joins.
	/// </summary>
	public static SaveData CreatePreHealieJoin() {
		var save = CreateChapter1Start();
		save.World.SetEventFlag(Chapter1Flags.TalkedToIzmitVillagers);
		save.World.SetEventFlag(Chapter1Flags.HeardCaveRumor);
		// Give Ragnar some levels and gear
		save.Characters[0].Level = 5;
		save.Characters[0].Experience = 300;
		save.Characters[0].CurrentHP = 50;
		save.Characters[0].MaxHP = 50;
		save.Characters[0].Strength = 20;
		save.Equipment[0].Weapon = 0x02; // Copper Sword
		save.Equipment[0].Armor = 0x10; // Leather Armor
		save.Header.Gold = 200;
		return save;
	}

	/// <summary>
	/// Create a save with Healie in party.
	/// </summary>
	public static SaveData CreateWithHealie() {
		var save = CreatePreHealieJoin();
		save.World.SetEventFlag(Chapter1Flags.MetHealie);
		save.World.SetEventFlag(Chapter1Flags.LearnedLochTowerLocation);
		// Healie is an NPC companion, not in standard party array
		return save;
	}

	/// <summary>
	/// Create a save just before the boss fight.
	/// </summary>
	public static SaveData CreatePreBoss() {
		var save = CreateWithHealie();
		save.World.SetEventFlag(Chapter1Flags.ReachedTowerBasement);
		// Level up for boss
		save.Characters[0].Level = 8;
		save.Characters[0].Experience = 1200;
		save.Characters[0].CurrentHP = 75;
		save.Characters[0].MaxHP = 75;
		save.Characters[0].Strength = 30;
		save.Equipment[0].Weapon = 0x04; // Chain Sickle
		save.Equipment[0].Armor = 0x12; // Chain Mail
		save.Equipment[0].Shield = 0x20; // Iron Shield
		save.Header.Gold = 500;
		return save;
	}

	/// <summary>
	/// Create a save after defeating the boss.
	/// </summary>
	public static SaveData CreatePostBoss() {
		var save = CreatePreBoss();
		save.World.SetEventFlag(Chapter1Flags.DefeatedBoss);
		save.World.SetEventFlag(Chapter1Flags.RescuedChildren);
		save.Characters[0].Experience += 850; // Boss exp
		return save;
	}

	/// <summary>
	/// Create a Chapter 1 complete save.
	/// </summary>
	public static SaveData CreateChapter1Complete() {
		var save = CreatePostBoss();
		save.World.SetEventFlag(Chapter1Flags.ReportedToKing);
		save.World.SetEventFlag(Chapter1Flags.ChapterComplete);
		save.Characters[0].Level = 10;
		save.Characters[0].Experience = 2500;
		save.Header.Gold = 800;
		return save;
	}

	/// <summary>
	/// Create a 100% complete Chapter 1 save (all chests, etc).
	/// </summary>
	public static SaveData Create100PercentComplete() {
		var save = CreateChapter1Complete();

		// Open all treasure chests
		foreach (var flag in Chapter1Flags.GetTreasureFlags()) {
			save.World.SetChestOpened(flag);
		}

		// Set optional flags
		save.World.SetEventFlag(Chapter1Flags.FoundWingOfWyvern);
		save.World.SetEventFlag(Chapter1Flags.GotLeatherHat);
		save.World.SetEventFlag(Chapter1Flags.GotHerbStash);
		save.World.SetEventFlag(Chapter1Flags.SleptAtIzmitInn);
		save.World.SetEventFlag(Chapter1Flags.SavedAtBurlandChurch);
		save.World.SetEventFlag(Chapter1Flags.ShoppedAtBurland);

		return save;
	}
}
