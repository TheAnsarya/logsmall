namespace DW4Lib.Tests;

using DW4Lib.DataStructures.Chapter2;
using DW4Lib.DataStructures.Chapter3;
using DW4Lib.DataStructures.Chapter4;
using DW4Lib.DataStructures.Chapter5;

/// <summary>
/// Tests for Chapter 2 data.
/// </summary>
public class Chapter2DataTests {
	[Fact]
	public void Chapter2Data_HasCorrectChapterId() {
		Assert.Equal(0x01, Chapter2Data.ChapterId);
	}

	[Fact]
	public void Chapter2Data_HasThreePartyMembers() {
		Assert.Equal(0x07, Chapter2Data.AlenaId);
		Assert.Equal(0x01, Chapter2Data.CristoId);
		Assert.Equal(0x04, Chapter2Data.BreyId);
	}

	[Fact]
	public void Chapter2Data_AlenaIsPhysicalFighter() {
		var stats = Chapter2Data.AlenaStartingStats;
		Assert.Equal(0, stats.MP);
		Assert.True(stats.Agility > stats.Intelligence);
	}

	[Fact]
	public void Chapter2Data_CristoHasMagic() {
		var stats = Chapter2Data.CristoStartingStats;
		Assert.True(stats.MP > 0);
	}

	[Fact]
	public void Chapter2Data_BreyIsMage() {
		var stats = Chapter2Data.BreyStartingStats;
		Assert.True(stats.Intelligence > stats.Strength);
		Assert.True(stats.MP > stats.HP / 2);
	}

	[Fact]
	public void Chapter2Data_MapsIncludeSanteemAndEndor() {
		var maps = Chapter2Data.Maps;
		Assert.Contains(maps, m => m.Name == "Santeem Castle");
		Assert.Contains(maps, m => m.Name == "Endor");
		Assert.Contains(maps, m => m.Name == "Colosseum");
	}

	[Fact]
	public void Chapter2Data_HasTournamentBattles() {
		var battles = Chapter2Data.TournamentBattles;
		Assert.Equal(5, battles.Length);
	}

	[Fact]
	public void Chapter2Data_TournamentDifficultyIncreases() {
		var battles = Chapter2Data.TournamentBattles;
		for (int i = 1; i < battles.Length; i++) {
			Assert.True(battles[i].OpponentHp > battles[i - 1].OpponentHp,
				$"Round {i + 1} should be harder than round {i}");
		}
	}

	[Fact]
	public void Chapter2Data_FinalTournamentOpponent() {
		var final = Chapter2Data.TournamentBattles[^1];
		Assert.Equal("Linguar", final.OpponentName);
		Assert.Equal(5, final.Round);
	}
}

/// <summary>
/// Tests for Chapter 3 data.
/// </summary>
public class Chapter3DataTests {
	[Fact]
	public void Chapter3Data_HasCorrectChapterId() {
		Assert.Equal(0x02, Chapter3Data.ChapterId);
	}

	[Fact]
	public void Chapter3Data_TaloonIsMerchant() {
		Assert.Equal(0x05, Chapter3Data.TaloonId);
		var stats = Chapter3Data.TaloonStartingStats;
		Assert.True(stats.Luck > 10); // Merchants have high luck
	}

	[Fact]
	public void Chapter3Data_TaloonHasNoMagic() {
		var stats = Chapter3Data.TaloonStartingStats;
		Assert.Equal(0, stats.MP);
	}

	[Fact]
	public void Chapter3Data_HasCompanionNPCs() {
		Assert.Equal(0xC7, Chapter3Data.LaurentId);
		Assert.Equal(0xC8, Chapter3Data.StromId);
	}

	[Fact]
	public void Chapter3Data_HasMerchantAbilities() {
		var abilities = Chapter3Data.MerchantAbilities;
		Assert.True(abilities.Length >= 5);
		Assert.Contains(abilities, a => a.Name == "Sell to Shop");
		Assert.Contains(abilities, a => a.Name == "Pick Up Gold");
	}

	[Fact]
	public void Chapter3Data_MerchantAbilitiesUnlockProgressively() {
		var abilities = Chapter3Data.MerchantAbilities;
		int lastLevel = 0;
		foreach (var ability in abilities) {
			Assert.True(ability.UnlockLevel >= lastLevel);
			lastLevel = ability.UnlockLevel;
		}
	}

	[Fact]
	public void Chapter3Data_HasShopWorkItems() {
		var items = Chapter3Data.ShopWorkItems;
		Assert.True(items.Length > 0);
		foreach (var item in items) {
			Assert.True(item.BuyPrice > item.SellPrice,
				"Buy price should be higher than sell price");
		}
	}

	[Fact]
	public void Chapter3Data_EndorShopCostIsReasonable() {
		Assert.Equal(35000, Chapter3Data.EndorShopCost);
	}

	[Fact]
	public void Chapter3Data_MapsIncludeKeyLocations() {
		var maps = Chapter3Data.Maps;
		Assert.Contains(maps, m => m.Name == "Lakanaba");
		Assert.Contains(maps, m => m.Name == "Endor");
		Assert.Contains(maps, m => m.Name == "Cave to Endor");
	}
}

/// <summary>
/// Tests for Chapter 4 data.
/// </summary>
public class Chapter4DataTests {
	[Fact]
	public void Chapter4Data_HasCorrectChapterId() {
		Assert.Equal(0x03, Chapter4Data.ChapterId);
	}

	[Fact]
	public void Chapter4Data_HasTwoSisters() {
		Assert.Equal(0x02, Chapter4Data.NaraId);
		Assert.Equal(0x03, Chapter4Data.MaraId);
	}

	[Fact]
	public void Chapter4Data_NaraIsPriest() {
		var stats = Chapter4Data.NaraStartingStats;
		Assert.True(stats.MP > 0);
		Assert.True(stats.Intelligence > stats.Strength);
	}

	[Fact]
	public void Chapter4Data_MaraIsMage() {
		var stats = Chapter4Data.MaraStartingStats;
		Assert.True(stats.MP > stats.HP / 2);
		Assert.True(stats.Intelligence > stats.Strength * 2);
	}

	[Fact]
	public void Chapter4Data_OrinIsTemporaryCompanion() {
		Assert.Equal(0xC6, Chapter4Data.OrinId);
		var stats = Chapter4Data.OrinStats;
		Assert.True(stats.Level > 1); // Joins at higher level
		Assert.Equal(0, stats.MP); // Warrior type
	}

	[Fact]
	public void Chapter4Data_BalzackBossExists() {
		var boss = Chapter4Data.BalzackStats;
		Assert.Equal("Balzack", boss.Name);
		Assert.True(boss.HP >= 500);
		Assert.Equal(0, boss.ExperienceReward); // Can't defeat in Chapter 4
	}

	[Fact]
	public void Chapter4Data_NaraLearnsHealingSpells() {
		var spells = Chapter4Data.NaraSpells;
		Assert.Contains(spells, s => s.SpellName == "Heal");
		Assert.Contains(spells, s => s.SpellName == "Healmore");
	}

	[Fact]
	public void Chapter4Data_MaraLearnsAttackSpells() {
		var spells = Chapter4Data.MaraSpells;
		Assert.Contains(spells, s => s.SpellName == "Blaze");
		Assert.Contains(spells, s => s.SpellName == "Bang");
		Assert.Contains(spells, s => s.SpellName == "Blazemore");
	}

	[Fact]
	public void Chapter4Data_MapsIncludeRevengeLocations() {
		var maps = Chapter4Data.Maps;
		Assert.Contains(maps, m => m.Name == "Monbaraba");
		Assert.Contains(maps, m => m.Name == "Keeleon Castle");
		Assert.Contains(maps, m => m.Name == "Aktemto Mine");
	}
}

/// <summary>
/// Tests for Chapter 5 data.
/// </summary>
public class Chapter5DataTests {
	[Fact]
	public void Chapter5Data_HasCorrectChapterId() {
		Assert.Equal(0x04, Chapter5Data.ChapterId);
	}

	[Fact]
	public void Chapter5Data_HeroIsProtagonist() {
		Assert.Equal(0x00, Chapter5Data.HeroId);
	}

	[Fact]
	public void Chapter5Data_HasAllEightCharacters() {
		Assert.Equal(8, Chapter5Data.AllCharacterIds.Length);
		Assert.Contains((byte)0x00, Chapter5Data.AllCharacterIds); // Hero
		Assert.Contains((byte)0x07, Chapter5Data.AllCharacterIds); // Alena
		Assert.Contains((byte)0x06, Chapter5Data.AllCharacterIds); // Ragnar
		Assert.Contains((byte)0x05, Chapter5Data.AllCharacterIds); // Taloon
	}

	[Fact]
	public void Chapter5Data_HasFourCompanions() {
		Assert.Equal(4, Chapter5Data.Chapter5Companions.Length);
	}

	[Fact]
	public void Chapter5Data_HeroIsBalanced() {
		var stats = Chapter5Data.HeroStartingStats;
		Assert.True(stats.HP > 0);
		Assert.True(stats.MP > 0); // Hero has magic
		// Balanced means no stat is extremely low
		Assert.True(stats.Strength >= 8);
		Assert.True(stats.Intelligence >= 8);
	}

	[Fact]
	public void Chapter5Data_HasMultipleRegions() {
		var regions = Chapter5Data.Regions;
		Assert.True(regions.Length >= 5);
		Assert.Contains(regions, r => r.Name.Contains("Hero"));
		Assert.Contains(regions, r => r.Name.Contains("Endor"));
		Assert.Contains(regions, r => r.Name.Contains("Zenithian"));
	}

	[Fact]
	public void Chapter5Data_HasMajorBosses() {
		var bosses = Chapter5Data.Bosses;
		Assert.True(bosses.Length >= 5);
		Assert.Contains(bosses, b => b.Name == "Necrosaro");
		Assert.Contains(bosses, b => b.Name == "Psaro the Manslayer");
	}

	[Fact]
	public void Chapter5Data_PsaroIsFinalBoss() {
		var psaro = Chapter5Data.Bosses.FirstOrDefault(b => b.Name.Contains("Psaro"));
		Assert.NotNull(psaro);
		Assert.True(psaro.HP >= 4000);
	}

	[Fact]
	public void Chapter5Data_HasZenithianEquipment() {
		var gear = Chapter5Data.ZenithianGear;
		Assert.Equal(4, gear.Length); // Sword, Armor, Shield, Helm
		Assert.Contains(gear, g => g.Name.Contains("Sword"));
		Assert.Contains(gear, g => g.Name.Contains("Armor"));
		Assert.Contains(gear, g => g.Name.Contains("Shield"));
		Assert.Contains(gear, g => g.Name.Contains("Helm"));
	}

	[Fact]
	public void Chapter5Data_HeroLearnsMultipleSpells() {
		var spells = Chapter5Data.HeroSpells;
		Assert.True(spells.Length >= 8);
		Assert.Contains(spells, s => s.SpellName == "Heal");
		Assert.Contains(spells, s => s.SpellName == "Kazap"); // Ultimate spell
	}

	[Fact]
	public void Chapter5Data_WagonCapacityIsValid() {
		Assert.Equal(4, Chapter5Data.MaxActiveParty);
		Assert.Equal(8, Chapter5Data.MaxWagonCapacity);
		Assert.True(Chapter5Data.MaxWagonCapacity >= Chapter5Data.AllCharacterIds.Length);
	}

	[Fact]
	public void Chapter5Data_DefaultTacticIsNormal() {
		Assert.Equal(BattleTactic.Normal, Chapter5Data.DefaultTactic);
	}
}
