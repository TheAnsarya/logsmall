using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;
using DW4Lib.Events;
using DW4Lib.Maps;

namespace DW4Lib.Tests;

/// <summary>
/// Tests for Chapter 4 events and maps.
/// </summary>
public class Chapter4Tests {
	// ============================================================
	// Chapter 4 Events Tests
	// ============================================================

	[Fact]
	public void GetAllScripts_ReturnsAllChapter4Scripts() {
		var scripts = Chapter4Events.GetAllScripts();

		// 21 story scripts + 10 service scripts
		Assert.Equal(31, scripts.Count);
	}

	[Fact]
	public void IntroScript_HasCorrectId() {
		var scripts = Chapter4Events.GetAllScripts();
		var intro = scripts.First(s => s.Name == "Chapter 4 Intro");

		Assert.Equal(Chapter4Events.IntroScript, intro.Id);
		Assert.Equal(ScriptCategory.Cutscene, intro.Category);
		Assert.Equal(3, intro.ChapterId);
	}

	[Fact]
	public void AllScripts_HaveUniqueIds() {
		var scripts = Chapter4Events.GetAllScripts();
		var ids = scripts.Select(s => s.Id).ToList();

		Assert.Equal(ids.Count, ids.Distinct().Count());
	}

	[Fact]
	public void AllScripts_HaveNames() {
		var scripts = Chapter4Events.GetAllScripts();

		Assert.All(scripts, s => Assert.False(string.IsNullOrEmpty(s.Name)));
	}

	[Fact]
	public void AllScripts_HaveCommands() {
		var scripts = Chapter4Events.GetAllScripts();

		Assert.All(scripts, s => Assert.NotEmpty(s.Commands));
	}

	[Fact]
	public void BalzackEncounter_Exists() {
		var scripts = Chapter4Events.GetAllScripts();
		var balzack = scripts.FirstOrDefault(s => s.Name == "Balzack Encounter");

		Assert.NotNull(balzack);
		Assert.Equal(ScriptCategory.Cutscene, balzack.Category);
	}

	[Fact]
	public void BalzackBattle_IsBattleCategory() {
		var scripts = Chapter4Events.GetAllScripts();
		var battle = scripts.First(s => s.Name == "Balzack Battle");

		Assert.Equal(ScriptCategory.Battle, battle.Category);
	}

	[Fact]
	public void ServiceScripts_HaveCorrectCategories() {
		var scripts = Chapter4Events.GetAllScripts();
		var shopScripts = scripts.Where(s => s.Name.Contains("Shop")).ToList();
		var innScripts = scripts.Where(s => s.Name.Contains("Inn")).ToList();

		Assert.All(shopScripts, s => Assert.Equal(ScriptCategory.Shop, s.Category));
		Assert.All(innScripts, s => Assert.Equal(ScriptCategory.Inn, s.Category));
	}

	[Fact]
	public void ChurchScript_HasCorrectCategory() {
		var scripts = Chapter4Events.GetAllScripts();
		var church = scripts.First(s => s.Name == "Monbaraba Church");

		Assert.Equal(ScriptCategory.NPC, church.Category);
	}

	[Fact]
	public void ChapterCompleteScript_SetsCorrectChapter() {
		var script = Chapter4Events.BuildChapterCompleteScript();
		var setChapterCmd = script.Commands.FirstOrDefault(c => c.Opcode == ScriptOpcode.SetChapter);

		Assert.NotNull(setChapterCmd);
		Assert.Equal(4, setChapterCmd.Parameters[0]); // Should set to Chapter 5
	}

	[Fact]
	public void KeyItemScripts_Exist() {
		var scripts = Chapter4Events.GetAllScripts();
		var names = scripts.Select(s => s.Name).ToList();

		Assert.Contains("Find Sphere of Silence", names);
		Assert.Contains("Find Gunpowder", names);
	}

	[Fact]
	public void TheaterScripts_Exist() {
		var scripts = Chapter4Events.GetAllScripts();
		var names = scripts.Select(s => s.Name).ToList();

		Assert.Contains("Dance Performance", names);
		Assert.Contains("Fortune Telling", names);
	}

	[Fact]
	public void GetStoryScripts_ExcludesServiceScripts() {
		var storyScripts = Chapter4Events.GetStoryScripts();

		Assert.All(storyScripts, s =>
			Assert.True(s.Category != ScriptCategory.Inn &&
						s.Category != ScriptCategory.Shop));
	}

	[Fact]
	public void GetServiceScripts_OnlyIncludesServiceCategories() {
		var serviceScripts = Chapter4Events.GetServiceScripts();

		Assert.All(serviceScripts, s =>
			Assert.True(s.Category == ScriptCategory.Inn ||
						s.Category == ScriptCategory.Shop ||
						s.Category == ScriptCategory.NPC));
	}

	// ============================================================
	// Chapter 4 Maps Tests
	// ============================================================

	[Fact]
	public void GetAllMaps_ReturnsAllChapter4Maps() {
		var maps = Chapter4Maps.GetAllMaps();

		Assert.Equal(12, maps.Length);
	}

	[Fact]
	public void AllMaps_HaveValidMetadata() {
		var maps = Chapter4Maps.GetAllMaps();

		Assert.All(maps, m => {
			Assert.False(string.IsNullOrEmpty(m.Name));
			Assert.True(m.MapId > 0);
			Assert.NotNull(m.Chapters);
			Assert.Contains(3, m.Chapters);
		});
	}

	[Fact]
	public void MapMonbaraba_IsCorrectType() {
		var maps = Chapter4Maps.GetAllMaps();
		var monbaraba = maps.First(m => m.Name == "Monbaraba");

		Assert.Equal(MapType.Town, monbaraba.Type);
	}

	[Fact]
	public void KievsCastleMaps_AreCastleType() {
		var maps = Chapter4Maps.GetAllMaps();
		var castleMaps = maps.Where(m => m.Name.Contains("Kievs Castle"));

		Assert.All(castleMaps, m => Assert.Equal(MapType.Castle, m.Type));
	}

	[Fact]
	public void DungeonMaps_AreCaveType() {
		var maps = Chapter4Maps.GetAllMaps();
		var caveMaps = maps.Where(m => m.Name.Contains("Cave") || m.Name.Contains("Mine"));

		Assert.All(caveMaps, m => Assert.Equal(MapType.Cave, m.Type));
	}

	// ============================================================
	// Treasures Tests
	// ============================================================

	[Fact]
	public void GetAllTreasures_ReturnsExpectedCount() {
		var treasures = Chapter4Maps.GetAllTreasures();

		Assert.Equal(13, treasures.Length); // 13 including small medal
	}

	[Fact]
	public void AllTreasures_HaveValidData() {
		var treasures = Chapter4Maps.GetAllTreasures();

		Assert.All(treasures, t => {
			Assert.True(t.Index > 0);
			Assert.True(t.MapId > 0);
		});
	}

	[Fact]
	public void KeyItemTreasures_HaveCorrectItems() {
		var treasures = Chapter4Maps.GetAllTreasures();
		var sphereTreasure = treasures.First(t =>
			t.ContentsType == TreasureType.Item &&
			t.ContentsValue == Chapter4Events.ItemSphereOfSilence);
		var gunpowderTreasure = treasures.First(t =>
			t.ContentsType == TreasureType.Item &&
			t.ContentsValue == Chapter4Events.ItemGunpowder);

		Assert.Equal(Chapter4Maps.MapCaveMonbarabaF2, sphereTreasure.MapId);
		Assert.Equal(Chapter4Maps.MapMineLower, gunpowderTreasure.MapId);
	}

	[Fact]
	public void GoldTreasures_HavePositiveAmounts() {
		var treasures = Chapter4Maps.GetAllTreasures();
		var goldTreasures = treasures.Where(t => t.ContentsType == TreasureType.Gold);

		Assert.All(goldTreasures, t => Assert.True(t.ContentsValue > 0));
	}

	// ============================================================
	// Warps Tests
	// ============================================================

	[Fact]
	public void GetAllWarps_ReturnsExpectedCount() {
		var warps = Chapter4Maps.GetAllWarps();

		Assert.Equal(18, warps.Length);
	}

	[Fact]
	public void AllWarps_HaveValidSourceAndDest() {
		var warps = Chapter4Maps.GetAllWarps();

		Assert.All(warps, w => {
			Assert.True(w.SourceMapId >= 0);
			Assert.True(w.DestMapId >= 0);
		});
	}

	[Fact]
	public void TheaterWarps_ConnectProperly() {
		var warps = Chapter4Maps.GetAllWarps();
		var monbarabaToTheater = warps.First(w =>
			w.SourceMapId == Chapter4Maps.MapMonbaraba &&
			w.DestMapId == Chapter4Maps.MapTheater);
		var theaterToMonbaraba = warps.First(w =>
			w.SourceMapId == Chapter4Maps.MapTheater &&
			w.DestMapId == Chapter4Maps.MapMonbaraba);

		Assert.NotNull(monbarabaToTheater);
		Assert.NotNull(theaterToMonbaraba);
	}

	[Fact]
	public void SecretPassage_ExistsInKievsCastle() {
		var warps = Chapter4Maps.GetAllWarps();
		var secretEntry = warps.FirstOrDefault(w =>
			w.SourceMapId == Chapter4Maps.MapKievsCastleThrone &&
			w.DestMapId == Chapter4Maps.MapKievsCastleSecret);

		Assert.NotNull(secretEntry);
	}

	// ============================================================
	// NPCs Tests
	// ============================================================

	[Fact]
	public void GetAllNPCs_ReturnsExpectedCount() {
		var npcs = Chapter4Maps.GetAllNpcs();

		Assert.Equal(13, npcs.Length);
	}

	[Fact]
	public void AllNPCs_HaveValidData() {
		var npcs = Chapter4Maps.GetAllNpcs();

		Assert.All(npcs, n => {
			Assert.True(n.Index > 0);
			Assert.True(n.MapId > 0);
			Assert.False(string.IsNullOrEmpty(n.Name));
		});
	}

	[Fact]
	public void Balzack_IsInThroneRoom() {
		var npcs = Chapter4Maps.GetAllNpcs();
		var balzack = npcs.First(n => n.Name == "Balzack");

		Assert.Equal(Chapter4Maps.MapKievsCastleThrone, balzack.MapId);
		Assert.Equal(Chapter4Events.BalzackEncounter, balzack.DialogId);
	}

	[Fact]
	public void Orin_IsInMonbaraba() {
		var npcs = Chapter4Maps.GetAllNpcs();
		var orin = npcs.First(n => n.Name == "Orin");

		Assert.Equal(Chapter4Maps.MapMonbaraba, orin.MapId);
		Assert.Equal(Chapter4Events.MeetOrin, orin.DialogId);
	}

	[Fact]
	public void EachTown_HasInnkeeper() {
		var npcs = Chapter4Maps.GetAllNpcs();
		var innkeepers = npcs.Where(n => n.Name.Contains("Innkeeper"));

		// Monbaraba, Haville, Kievs
		Assert.Equal(3, innkeepers.Count());
	}

	// ============================================================
	// Encounter Zones Tests
	// ============================================================

	[Fact]
	public void GetAllEncounterZones_ReturnsExpectedCount() {
		var zones = Chapter4Maps.GetAllEncounterZones();

		Assert.Equal(5, zones.Length);
	}

	[Fact]
	public void AllEncounterZones_HaveValidData() {
		var zones = Chapter4Maps.GetAllEncounterZones();

		Assert.All(zones, z => {
			Assert.True(z.Index > 0);
			Assert.True(z.MapId >= 0);
			Assert.True(z.MonsterGroups.Length > 0);
			Assert.True(z.EncounterRate > 0);
		});
	}

	[Fact]
	public void DeeperDungeons_HaveHigherEncounterRates() {
		var zones = Chapter4Maps.GetAllEncounterZones();
		var caveF1 = zones.First(z => z.MapId == Chapter4Maps.MapCaveMonbarabaF1);
		var caveF2 = zones.First(z => z.MapId == Chapter4Maps.MapCaveMonbarabaF2);

		Assert.True(caveF2.EncounterRate >= caveF1.EncounterRate);
	}

	// ============================================================
	// DQ3r Conversion Tests
	// ============================================================

	[Fact]
	public void ConvertTreasures_ReturnsAllTreasures() {
		var dq3rTreasures = Chapter4Maps.ConvertTreasures();

		Assert.Equal(13, dq3rTreasures.Length); // Includes small medal
	}

	[Fact]
	public void ConvertWarps_ReturnsAllWarps() {
		var dq3rWarps = Chapter4Maps.ConvertWarps();

		Assert.Equal(18, dq3rWarps.Length);
	}

	[Fact]
	public void ConvertNpcs_ReturnsAllNpcs() {
		var dq3rNpcs = Chapter4Maps.ConvertNpcs();

		Assert.Equal(13, dq3rNpcs.Length);
	}

	[Fact]
	public void ConvertEncounterZones_ReturnsAllZones() {
		var dq3rZones = Chapter4Maps.ConvertEncounterZones();

		Assert.Equal(5, dq3rZones.Length);
	}

	[Fact]
	public void GetDQ3rMapIdMapping_MapsAllChapter4Maps() {
		var mapping = Chapter4Maps.GetDQ3rMapIdMapping();

		Assert.Equal(12, mapping.Count);
		Assert.True(mapping.ContainsKey(Chapter4Maps.MapMonbaraba));
		Assert.True(mapping.ContainsKey(Chapter4Maps.MapKievsCastleThrone));
	}

	// ============================================================
	// Story Progression Tests
	// ============================================================

	[Fact]
	public void StoryFlags_AreSequential() {
		// Verify key story flags are sequential for proper progression
		Assert.True(Chapter4Events.FlagIntro < Chapter4Events.FlagDancePerformed);
		Assert.True(Chapter4Events.FlagDancePerformed < Chapter4Events.FlagBalzackRumors);
		Assert.True(Chapter4Events.FlagBalzackRumors < Chapter4Events.FlagMetOrin);
		Assert.True(Chapter4Events.FlagMetOrin < Chapter4Events.FlagSphereOfSilence);
		Assert.True(Chapter4Events.FlagSphereOfSilence < Chapter4Events.FlagBalzackEncounter);
		Assert.True(Chapter4Events.FlagBalzackEncounter < Chapter4Events.FlagEscapedKievs);
		Assert.True(Chapter4Events.FlagEscapedKievs < Chapter4Events.FlagChapterComplete);
	}

	[Fact]
	public void AllScriptsForChapter_AreChapter3() {
		var scripts = Chapter4Events.GetAllScripts();

		// Note: Chapter 4 in story = ChapterId 3 (0-indexed)
		Assert.All(scripts, s => Assert.Equal(3, s.ChapterId));
	}

	[Fact]
	public void AllMapsForChapter_AreChapter3() {
		var maps = Chapter4Maps.GetAllMaps();

		// Note: Chapter 4 in story = Chapter 3 index in Chapters array
		Assert.All(maps, m => Assert.Contains(3, m.Chapters!));
	}
}
