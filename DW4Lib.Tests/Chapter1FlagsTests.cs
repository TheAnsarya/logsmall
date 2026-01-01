using DW4Lib.DataStructures;
using DW4Lib.DataStructures.Chapter1;
using Xunit;

namespace DW4Lib.Tests;

/// <summary>
/// Tests for Chapter 1 event flags and save helpers.
/// </summary>
public class Chapter1FlagsTests {
	// ========================================
	// Story Flag Tests
	// ========================================

	[Fact]
	public void GetStoryFlags_Returns10Flags() {
		var flags = Chapter1Flags.GetStoryFlags();
		Assert.Equal(10, flags.Length);
	}

	[Fact]
	public void GetStoryFlags_StartsWithChapterStarted() {
		var flags = Chapter1Flags.GetStoryFlags();
		Assert.Equal(Chapter1Flags.ChapterStarted, flags[0]);
	}

	[Fact]
	public void GetStoryFlags_EndsWithChapterComplete() {
		var flags = Chapter1Flags.GetStoryFlags();
		Assert.Equal(Chapter1Flags.ChapterComplete, flags[^1]);
	}

	[Fact]
	public void GetTreasureFlags_Returns12Chests() {
		var flags = Chapter1Flags.GetTreasureFlags();
		Assert.Equal(12, flags.Length);
	}

	[Fact]
	public void IsStoryFlag_TrueForValidFlags() {
		Assert.True(Chapter1Flags.IsStoryFlag(0x0001));
		Assert.True(Chapter1Flags.IsStoryFlag(0x0005));
		Assert.True(Chapter1Flags.IsStoryFlag(0x000A));
	}

	[Fact]
	public void IsStoryFlag_FalseForOtherFlags() {
		Assert.False(Chapter1Flags.IsStoryFlag(0x0000));
		Assert.False(Chapter1Flags.IsStoryFlag(0x0020)); // Treasure
		Assert.False(Chapter1Flags.IsStoryFlag(0x0100));
	}

	[Fact]
	public void IsTreasureFlag_TrueForValidFlags() {
		Assert.True(Chapter1Flags.IsTreasureFlag(0x0020));
		Assert.True(Chapter1Flags.IsTreasureFlag(0x0025));
		Assert.True(Chapter1Flags.IsTreasureFlag(0x003F));
	}

	[Fact]
	public void IsTreasureFlag_FalseForOtherFlags() {
		Assert.False(Chapter1Flags.IsTreasureFlag(0x0001)); // Story
		Assert.False(Chapter1Flags.IsTreasureFlag(0x001F));
		Assert.False(Chapter1Flags.IsTreasureFlag(0x0040));
	}

	// ========================================
	// Story Progression Tests
	// ========================================

	[Fact]
	public void GetNextStoryFlag_ReturnsCorrectSequence() {
		Assert.Equal(Chapter1Flags.TalkedToIzmitVillagers,
			Chapter1Flags.GetNextStoryFlag(Chapter1Flags.ChapterStarted));
		Assert.Equal(Chapter1Flags.MetHealie,
			Chapter1Flags.GetNextStoryFlag(Chapter1Flags.HeardCaveRumor));
		Assert.Equal(Chapter1Flags.ChapterComplete,
			Chapter1Flags.GetNextStoryFlag(Chapter1Flags.ReportedToKing));
	}

	[Fact]
	public void GetNextStoryFlag_ReturnsZeroAtEnd() {
		Assert.Equal(0x0000, Chapter1Flags.GetNextStoryFlag(Chapter1Flags.ChapterComplete));
	}

	[Fact]
	public void GetFlagName_ReturnsCorrectNames() {
		Assert.Equal("Chapter Started", Chapter1Flags.GetFlagName(Chapter1Flags.ChapterStarted));
		Assert.Equal("Met Healie", Chapter1Flags.GetFlagName(Chapter1Flags.MetHealie));
		Assert.Equal("Defeated Boss", Chapter1Flags.GetFlagName(Chapter1Flags.DefeatedBoss));
	}

	[Fact]
	public void GetFlagName_HandlesUnknownFlags() {
		string name = Chapter1Flags.GetFlagName(0xFFFF);
		Assert.Contains("Unknown", name);
		Assert.Contains("FFFF", name);
	}

	// ========================================
	// Completion Tracking Tests
	// ========================================

	[Fact]
	public void GetStoryCompletion_ZeroAtStart() {
		var save = new SaveData();
		int completion = Chapter1Flags.GetStoryCompletion(save);
		Assert.Equal(0, completion);
	}

	[Fact]
	public void GetStoryCompletion_IncrementsWithFlags() {
		var save = new SaveData();
		save.World.SetEventFlag(Chapter1Flags.ChapterStarted);
		int completion = Chapter1Flags.GetStoryCompletion(save);
		Assert.Equal(10, completion); // 1 of 10 = 10%
	}

	[Fact]
	public void GetStoryCompletion_100AtFullCompletion() {
		var save = new SaveData();
		foreach (var flag in Chapter1Flags.GetStoryFlags()) {
			save.World.SetEventFlag(flag);
		}
		int completion = Chapter1Flags.GetStoryCompletion(save);
		Assert.Equal(100, completion);
	}

	[Fact]
	public void GetTreasureCompletion_ZeroAtStart() {
		var save = new SaveData();
		int completion = Chapter1Flags.GetTreasureCompletion(save);
		Assert.Equal(0, completion);
	}

	[Fact]
	public void GetTreasureCompletion_100WhenAllOpened() {
		var save = new SaveData();
		foreach (var flag in Chapter1Flags.GetTreasureFlags()) {
			save.World.SetChestOpened(flag);
		}
		int completion = Chapter1Flags.GetTreasureCompletion(save);
		Assert.Equal(100, completion);
	}

	// ========================================
	// Save Helper Tests
	// ========================================

	[Fact]
	public void CreateChapter1Start_HasChapterStartedFlag() {
		var save = Chapter1SaveHelper.CreateChapter1Start();
		Assert.True(save.World.GetEventFlag(Chapter1Flags.ChapterStarted));
	}

	[Fact]
	public void CreateChapter1Start_HasRagnarInParty() {
		var save = Chapter1SaveHelper.CreateChapter1Start();
		Assert.Equal(Chapter1Data.RagnarId, save.Party.ActiveParty[0]);
	}

	[Fact]
	public void CreatePreHealieJoin_HasCorrectFlags() {
		var save = Chapter1SaveHelper.CreatePreHealieJoin();
		Assert.True(save.World.GetEventFlag(Chapter1Flags.ChapterStarted));
		Assert.True(save.World.GetEventFlag(Chapter1Flags.TalkedToIzmitVillagers));
		Assert.True(save.World.GetEventFlag(Chapter1Flags.HeardCaveRumor));
		Assert.False(save.World.GetEventFlag(Chapter1Flags.MetHealie));
	}

	[Fact]
	public void CreatePreHealieJoin_HasLeveledRagnar() {
		var save = Chapter1SaveHelper.CreatePreHealieJoin();
		Assert.Equal(5, save.Characters[0].Level);
		Assert.True(save.Characters[0].Strength > 12); // Higher than starting
	}

	[Fact]
	public void CreatePreHealieJoin_HasSomeGold() {
		var save = Chapter1SaveHelper.CreatePreHealieJoin();
		Assert.Equal(200, save.Header.Gold);
	}

	[Fact]
	public void CreateWithHealie_HasHealieFlags() {
		var save = Chapter1SaveHelper.CreateWithHealie();
		Assert.True(save.World.GetEventFlag(Chapter1Flags.MetHealie));
		Assert.True(save.World.GetEventFlag(Chapter1Flags.LearnedLochTowerLocation));
	}

	[Fact]
	public void CreatePreBoss_HasReachedBasement() {
		var save = Chapter1SaveHelper.CreatePreBoss();
		Assert.True(save.World.GetEventFlag(Chapter1Flags.ReachedTowerBasement));
		Assert.False(save.World.GetEventFlag(Chapter1Flags.DefeatedBoss));
	}

	[Fact]
	public void CreatePreBoss_HasHigherLevel() {
		var save = Chapter1SaveHelper.CreatePreBoss();
		Assert.Equal(8, save.Characters[0].Level);
	}

	[Fact]
	public void CreatePreBoss_HasBetterEquipment() {
		var save = Chapter1SaveHelper.CreatePreBoss();
		Assert.NotEqual(0, save.Equipment[0].Weapon);
		Assert.NotEqual(0, save.Equipment[0].Armor);
		Assert.NotEqual(0, save.Equipment[0].Shield);
	}

	[Fact]
	public void CreatePostBoss_HasDefeatedBossFlag() {
		var save = Chapter1SaveHelper.CreatePostBoss();
		Assert.True(save.World.GetEventFlag(Chapter1Flags.DefeatedBoss));
		Assert.True(save.World.GetEventFlag(Chapter1Flags.RescuedChildren));
	}

	[Fact]
	public void CreatePostBoss_HasBossExperience() {
		var preBoss = Chapter1SaveHelper.CreatePreBoss();
		var postBoss = Chapter1SaveHelper.CreatePostBoss();
		Assert.True(postBoss.Characters[0].Experience > preBoss.Characters[0].Experience);
	}

	[Fact]
	public void CreateChapter1Complete_HasAllStoryFlags() {
		var save = Chapter1SaveHelper.CreateChapter1Complete();
		Assert.True(save.World.GetEventFlag(Chapter1Flags.ChapterStarted));
		Assert.True(save.World.GetEventFlag(Chapter1Flags.ReportedToKing));
		Assert.True(save.World.GetEventFlag(Chapter1Flags.ChapterComplete));
	}

	[Fact]
	public void CreateChapter1Complete_100PercentStory() {
		var save = Chapter1SaveHelper.CreateChapter1Complete();
		int completion = Chapter1Flags.GetStoryCompletion(save);
		Assert.Equal(100, completion);
	}

	[Fact]
	public void Create100PercentComplete_HasAllTreasures() {
		var save = Chapter1SaveHelper.Create100PercentComplete();
		int treasureCompletion = Chapter1Flags.GetTreasureCompletion(save);
		Assert.Equal(100, treasureCompletion);
	}

	[Fact]
	public void Create100PercentComplete_HasOptionalFlags() {
		var save = Chapter1SaveHelper.Create100PercentComplete();
		Assert.True(save.World.GetEventFlag(Chapter1Flags.FoundWingOfWyvern));
		Assert.True(save.World.GetEventFlag(Chapter1Flags.GotLeatherHat));
		Assert.True(save.World.GetEventFlag(Chapter1Flags.SleptAtIzmitInn));
	}

	// ========================================
	// Flag Value Tests
	// ========================================

	[Fact]
	public void StoryFlags_AreInCorrectRange() {
		foreach (var flag in Chapter1Flags.GetStoryFlags()) {
			Assert.InRange(flag, 0x0001, 0x000F);
		}
	}

	[Fact]
	public void TreasureFlags_AreInCorrectRange() {
		foreach (var flag in Chapter1Flags.GetTreasureFlags()) {
			Assert.InRange(flag, 0x0020, 0x003F);
		}
	}

	[Fact]
	public void StoryFlags_AreUnique() {
		var flags = Chapter1Flags.GetStoryFlags();
		var distinct = flags.Distinct().ToArray();
		Assert.Equal(flags.Length, distinct.Length);
	}

	[Fact]
	public void TreasureFlags_AreUnique() {
		var flags = Chapter1Flags.GetTreasureFlags();
		var distinct = flags.Distinct().ToArray();
		Assert.Equal(flags.Length, distinct.Length);
	}
}
