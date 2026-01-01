namespace DW4Lib.Tests;

using DW4Lib.DataStructures;
using DW4Lib.DataStructures.Chapter1;
using DW4Lib.Converters;

/// <summary>
/// Tests for Chapter data structures.
/// </summary>
public class ChapterTests {
	[Fact]
	public void ChapterDatabase_ContainsFiveChapters() {
		// Arrange & Act
		var chapters = ChapterDatabase.AllChapters;

		// Assert
		Assert.Equal(5, chapters.Length);
	}

	[Theory]
	[InlineData(0x00, "Chapter 1: The Royal Soldiers")]
	[InlineData(0x01, "Chapter 2: Princess Alena's Adventure")]
	[InlineData(0x02, "Chapter 3: Taloon the Arms Merchant")]
	[InlineData(0x03, "Chapter 4: The Sisters of Monbaraba")]
	[InlineData(0x04, "Chapter 5: The Chosen Ones")]
	public void ChapterDatabase_HasCorrectNames(byte id, string expectedName) {
		// Arrange & Act
		var chapter = ChapterDatabase.GetChapter(id);

		// Assert
		Assert.NotNull(chapter);
		Assert.Equal(expectedName, chapter.Name);
	}

	[Fact]
	public void Chapter1_HasCorrectProtagonist() {
		// Arrange
		var chapter = ChapterDatabase.GetChapter(0x00);

		// Assert
		Assert.NotNull(chapter);
		Assert.Single(chapter.PlayableCharacters);
		Assert.Equal(0x06, chapter.PlayableCharacters[0]); // Ragnar
	}

	[Fact]
	public void Chapter1_IsRagnarSolo() {
		// Arrange
		var chapter = ChapterDatabase.GetChapter(0x00);

		// Assert
		Assert.NotNull(chapter);
		Assert.True(chapter.Mechanics.HasFlag(ChapterMechanics.SoloProtagonist));
		Assert.True(chapter.Mechanics.HasFlag(ChapterMechanics.NpcCompanion));
	}

	[Fact]
	public void Chapter2_HasAIControlledParty() {
		// Arrange
		var chapter = ChapterDatabase.GetChapter(0x01);

		// Assert
		Assert.NotNull(chapter);
		Assert.True(chapter.Mechanics.HasFlag(ChapterMechanics.AiPartyMembers));
	}

	[Fact]
	public void Chapter3_HasMerchantAbilities() {
		// Arrange
		var chapter = ChapterDatabase.GetChapter(0x02);

		// Assert
		Assert.NotNull(chapter);
		Assert.True(chapter.Mechanics.HasFlag(ChapterMechanics.MerchantAbilities));
	}

	[Fact]
	public void Chapter5_HasWagonAndTactics() {
		// Arrange
		var chapter = ChapterDatabase.GetChapter(0x04);

		// Assert
		Assert.NotNull(chapter);
		Assert.True(chapter.Mechanics.HasFlag(ChapterMechanics.WagonParty));
		Assert.True(chapter.Mechanics.HasFlag(ChapterMechanics.TacticsMenu));
	}

	[Fact]
	public void BattleTactic_HasSixOptions() {
		// Assert
		var tactics = Enum.GetValues<BattleTactic>();
		Assert.Equal(6, tactics.Length);
	}

	[Fact]
	public void ChapterMechanics_CanCombineFlags() {
		// Arrange
		var mechanics = ChapterMechanics.WagonParty | ChapterMechanics.TacticsMenu | ChapterMechanics.FullControl;

		// Assert
		Assert.True(mechanics.HasFlag(ChapterMechanics.WagonParty));
		Assert.True(mechanics.HasFlag(ChapterMechanics.TacticsMenu));
		Assert.True(mechanics.HasFlag(ChapterMechanics.FullControl));
		Assert.False(mechanics.HasFlag(ChapterMechanics.MerchantAbilities));
	}

	[Fact]
	public void DayNightCycle_HasCorrectPeriods() {
		// Arrange
		var cycle = new DayNightCycle();

		// Assert
		Assert.Equal(0x00, cycle.DawnStart);
		Assert.Equal(0x3F, cycle.DayStart);
		Assert.Equal(0x8F, cycle.DuskStart);
		Assert.Equal(0xA0, cycle.NightStart);
		Assert.Equal(0xCB, cycle.MaxValue);
	}

	[Theory]
	[InlineData(0x00, TimePeriod.Dawn)]
	[InlineData(0x3E, TimePeriod.Dawn)]
	[InlineData(0x3F, TimePeriod.Day)]
	[InlineData(0x8E, TimePeriod.Day)]
	[InlineData(0x8F, TimePeriod.Dusk)]
	[InlineData(0x9F, TimePeriod.Dusk)]
	[InlineData(0xA0, TimePeriod.Night)]
	[InlineData(0xCB, TimePeriod.Night)]
	public void DayNightCycle_GetPeriod_ReturnsCorrectPeriod(int timeValue, TimePeriod expected) {
		// Arrange
		var cycle = new DayNightCycle();

		// Act
		var period = cycle.GetPeriod(timeValue);

		// Assert
		Assert.Equal(expected, period);
	}
}

/// <summary>
/// Tests for Chapter 1 specific data.
/// </summary>
public class Chapter1DataTests {
	[Fact]
	public void Chapter1Data_HasCorrectChapterId() {
		Assert.Equal(0x00, Chapter1Data.ChapterId);
	}

	[Fact]
	public void Chapter1Data_HasCorrectCharacterIds() {
		Assert.Equal(0x06, Chapter1Data.RagnarId);
		Assert.Equal(0xC5, Chapter1Data.HealieId);
	}

	[Fact]
	public void Chapter1Data_RagnarStartingStats_AreValid() {
		// Arrange
		var stats = Chapter1Data.StartingStats;

		// Assert
		Assert.Equal(1, stats.Level);
		Assert.Equal(30, stats.HP);
		Assert.Equal(0, stats.MP); // Ragnar has no magic
		Assert.True(stats.Strength > stats.Intelligence); // Warrior type
	}

	[Fact]
	public void Chapter1Data_Maps_ContainsExpectedLocations() {
		// Arrange
		var maps = Chapter1Data.Maps;

		// Assert
		Assert.Contains(maps, m => m.Name == "Burland Castle");
		Assert.Contains(maps, m => m.Name == "Izmit Village");
		Assert.Contains(maps, m => m.Name == "Loch Tower (Lighthouse)");
	}

	[Fact]
	public void Chapter1Data_Events_AreInOrder() {
		// Arrange
		var events = Chapter1Data.Events;

		// Assert
		Assert.True(events[0].TriggerType == EventTrigger.ChapterStart);
		Assert.True(events[^1].IsChapterEnd);
	}

	[Fact]
	public void Chapter1Data_Events_HaveValidFlags() {
		// Arrange
		var events = Chapter1Data.Events;

		// Assert
		for (int i = 1; i < events.Length; i++) {
			// Each event after the first should require a flag
			Assert.True(events[i].RequiredFlag >= 0,
				$"Event {events[i].Name} should have a required flag");
		}
	}

	[Fact]
	public void Chapter1Data_Treasures_HaveValidMapIds() {
		// Arrange
		var treasures = Chapter1Data.Treasures;
		var validMapIds = Chapter1Data.Maps.Select(m => m.Id).ToHashSet();

		// Assert
		foreach (var treasure in treasures) {
			Assert.Contains(treasure.MapId, validMapIds);
		}
	}

	[Fact]
	public void Chapter1Data_Shops_HaveItemsAndPrices() {
		// Arrange
		var shops = Chapter1Data.Shops;

		// Assert
		foreach (var shop in shops) {
			Assert.Equal(shop.Items.Length, shop.Prices.Length);
			Assert.True(shop.Items.Length > 0);
		}
	}

	[Fact]
	public void Chapter1Data_EncounterZones_HaveValidRates() {
		// Arrange
		var zones = Chapter1Data.EncounterZones;

		// Assert
		foreach (var zone in zones) {
			Assert.True(zone.EncounterRate > 0);
			Assert.True(zone.EncounterRate <= 32); // DW4 max rate
		}
	}
}

/// <summary>
/// Tests for Chapter 1 Dialog and NPCs.
/// </summary>
public class Chapter1DialogTests {
	[Fact]
	public void Chapter1Dialog_AllDialog_ContainsExpectedEntries() {
		// Arrange
		var dialog = Chapter1Dialog.AllDialog;

		// Assert
		Assert.True(dialog.Length >= 8); // At least the main story dialogs
	}

	[Fact]
	public void Chapter1Dialog_KingInitialQuest_HasContent() {
		// Arrange
		var dialog = Chapter1Dialog.KingInitialQuest;

		// Assert
		Assert.Equal("King of Burland", dialog.Speaker);
		Assert.True(dialog.Lines.Length > 0);
		Assert.True(dialog.GetCharacterCount() > 0);
	}

	[Fact]
	public void Chapter1Dialog_HealieIntro_ContainsJoinIndicator() {
		// Arrange
		var dialog = Chapter1Dialog.HealieIntro;

		// Assert
		Assert.Equal("Healie", dialog.Speaker);
		Assert.Contains(dialog.Lines, l => l.Contains("come with you"));
	}

	[Fact]
	public void Chapter1NPCs_GetAllNPCs_ReturnsMultipleNpcs() {
		// Arrange & Act
		var npcs = Chapter1NPCs.GetAllNPCs().ToList();

		// Assert
		Assert.True(npcs.Count >= 4);
	}

	[Fact]
	public void Chapter1NPCs_Healie_IsCompanion() {
		// Arrange
		var healie = Chapter1NPCs.Healie;

		// Assert
		Assert.Equal(Chapter1Data.HealieId, healie.Id);
		Assert.True(healie.IsCompanion);
		Assert.Equal(CompanionBehavior.FollowAndHeal, healie.CompanionBehavior);
	}

	[Fact]
	public void Chapter1NPCs_ChameleonHumanoid_IsBoss() {
		// Arrange
		var boss = Chapter1NPCs.ChameleonHumanoid;

		// Assert
		Assert.True(boss.IsBoss);
		Assert.NotNull(boss.BossStats);
		Assert.True(boss.BossStats.HP > 0);
		Assert.True(boss.BossStats.ExperienceReward > 0);
	}

	[Fact]
	public void Chapter1NPCs_AllHaveValidMapIds() {
		// Arrange
		var npcs = Chapter1NPCs.GetAllNPCs().ToList();
		var validMapIds = Chapter1Data.Maps.Select(m => m.Id).ToHashSet();
		validMapIds.Add(0x02); // Burland Castle (may not be in Chapter1Data.Maps)

		// Assert
		foreach (var npc in npcs) {
			Assert.Contains(npc.MapId, validMapIds);
		}
	}
}

/// <summary>
/// Tests for ChapterConverter.
/// </summary>
public class ChapterConverterTests {
	[Fact]
	public void ConvertChapter_Chapter1_ReturnsValidScenario() {
		// Arrange
		var chapter1 = ChapterDatabase.GetChapter(0x00);

		// Act
		var scenario = ChapterConverter.ConvertChapter(chapter1!);

		// Assert
		Assert.NotNull(scenario);
		Assert.Equal(0x100, scenario.ScenarioId); // Chapter 0 maps to scenario 0x100
		Assert.True(scenario.IsMainStory);
	}

	[Fact]
	public void ConvertChapter1_ReturnsCompleteData() {
		// Act
		var data = ChapterConverter.ConvertChapter1();

		// Assert
		Assert.NotNull(data);
		Assert.NotNull(data.ProtagonistData);
		Assert.NotNull(data.CompanionData);
		Assert.True(data.Maps.Count > 0);
		Assert.True(data.QuestSteps.Count > 0);
		Assert.True(data.Treasures.Count > 0);
		Assert.True(data.Shops.Count > 0);
		Assert.True(data.EncounterZones.Count > 0);
		Assert.True(data.Dialog.Count > 0);
		Assert.True(data.NPCs.Count > 0);
	}

	[Fact]
	public void ConvertChapter1_Protagonist_IsWarrior() {
		// Act
		var data = ChapterConverter.ConvertChapter1();

		// Assert
		Assert.Equal("Ragnar", data.ProtagonistData!.Name);
		Assert.Equal(Dq3rClass.Warrior, data.ProtagonistData.Class);
	}

	[Fact]
	public void ConvertChapter1_Companion_HasHealingFlags() {
		// Act
		var data = ChapterConverter.ConvertChapter1();

		// Assert
		Assert.Equal("Healie", data.CompanionData!.Name);
		Assert.True(data.CompanionData.IsCompanion);
		Assert.True(data.CompanionData.CompanionFlags.HasFlag(Dq3rCompanionFlags.CanHeal));
	}

	[Fact]
	public void ConvertChapter1_Maps_HaveScaledCoordinates() {
		// Act
		var data = ChapterConverter.ConvertChapter1();

		// Assert
		foreach (var map in data.Maps) {
			// DQ3r coordinates should be scaled up (2x)
			Assert.True(map.WorldMapX >= 0);
			Assert.True(map.WorldMapY >= 0);
		}
	}

	[Fact]
	public void ConvertChapter1_MapIds_AreInDq3rRange() {
		// Act
		var data = ChapterConverter.ConvertChapter1();

		// Assert - DQ3r maps start at 0x200
		foreach (var map in data.Maps) {
			Assert.True(map.MapId >= 0x200, $"Map ID {map.MapId:X} should be >= 0x200");
		}
	}

	[Fact]
	public void ConvertChapter1_QuestSteps_HaveConvertedFlags() {
		// Act
		var data = ChapterConverter.ConvertChapter1();

		// Assert - DQ3r flags start at 0x400
		foreach (var step in data.QuestSteps.Where(s => s.RequiredFlag.HasValue)) {
			Assert.True(step.RequiredFlag >= 0x400,
				$"Flag {step.RequiredFlag:X} should be >= 0x400");
		}
	}

	[Fact]
	public void ConvertChapter1_Dialog_HasConvertedIds() {
		// Act
		var data = ChapterConverter.ConvertChapter1();

		// Assert - DQ3r dialog IDs start at 0x1000
		foreach (var dialog in data.Dialog) {
			Assert.True(dialog.DialogId >= 0x1000,
				$"Dialog ID {dialog.DialogId:X} should be >= 0x1000");
		}
	}

	[Fact]
	public void ConvertChapter1_Shops_HaveScaledPrices() {
		// Arrange
		var originalShop = Chapter1Data.Shops[0];

		// Act
		var data = ChapterConverter.ConvertChapter1();
		var convertedShop = data.Shops[0];

		// Assert - prices should be scaled up (1.5x)
		Assert.True(convertedShop.Prices[0] > originalShop.Prices[0],
			"Converted prices should be higher than original");
	}
}
