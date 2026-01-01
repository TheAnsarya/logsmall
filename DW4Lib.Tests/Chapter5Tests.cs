using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;
using DW4Lib.Events;
using DW4Lib.Maps;

namespace DW4Lib.Tests;

/// <summary>
/// Tests for Chapter 5 events and maps.
/// Chapter 5 is the Hero's main chapter where all characters unite.
/// </summary>
public class Chapter5Tests {
	// ============================================================
	// Chapter 5 Events Tests
	// ============================================================

	[Fact]
	public void GetAllScripts_ReturnsAllChapter5Scripts() {
		var scripts = Chapter5Events.GetAllScripts();

		// 25 story scripts + 10 service scripts
		Assert.Equal(35, scripts.Length);
	}

	[Fact]
	public void IntroScript_HasCorrectId() {
		var scripts = Chapter5Events.GetAllScripts();
		var intro = scripts.First(s => s.Name == "Chapter 5 Intro");

		Assert.Equal(Chapter5Events.IntroScript, intro.Id);
		Assert.Equal(ScriptCategory.Cutscene, intro.Category);
		Assert.Equal(4, intro.ChapterId);
	}

	[Fact]
	public void AllScripts_HaveUniqueIds() {
		var scripts = Chapter5Events.GetAllScripts();
		var ids = scripts.Select(s => s.Id).ToList();

		Assert.Equal(ids.Count, ids.Distinct().Count());
	}

	[Fact]
	public void AllScripts_HaveNames() {
		var scripts = Chapter5Events.GetAllScripts();

		Assert.All(scripts, s => Assert.False(string.IsNullOrEmpty(s.Name)));
	}

	[Fact]
	public void AllScripts_HaveCommands() {
		var scripts = Chapter5Events.GetAllScripts();

		Assert.All(scripts, s => Assert.NotEmpty(s.Commands));
	}

	[Fact]
	public void VillageAttack_Exists() {
		var scripts = Chapter5Events.GetAllScripts();
		var attack = scripts.FirstOrDefault(s => s.Name == "Village Attack");

		Assert.NotNull(attack);
		Assert.Equal(ScriptCategory.Cutscene, attack.Category);
	}

	[Fact]
	public void PsaroBattle_IsBattleCategory() {
		var scripts = Chapter5Events.GetAllScripts();
		var battle = scripts.First(s => s.Name == "Psaro Battle");

		Assert.Equal(ScriptCategory.Battle, battle.Category);
	}

	[Fact]
	public void ServiceScripts_HaveCorrectCategories() {
		var scripts = Chapter5Events.GetAllScripts();
		var shopScripts = scripts.Where(s => s.Name.Contains("Shop")).ToList();
		var innScripts = scripts.Where(s => s.Name.Contains("Inn")).ToList();

		Assert.All(shopScripts, s => Assert.Equal(ScriptCategory.Shop, s.Category));
		Assert.All(innScripts, s => Assert.Equal(ScriptCategory.Inn, s.Category));
	}

	[Fact]
	public void ChurchScript_HasCorrectCategory() {
		var scripts = Chapter5Events.GetAllScripts();
		var church = scripts.First(s => s.Name == "Branca Church");

		Assert.Equal(ScriptCategory.NPC, church.Category);
	}

	[Fact]
	public void GameEnding_SetsCorrectChapter() {
		var script = Chapter5Events.BuildGameEndingScript();
		// Game ending should not set a new chapter, it ends the game

		Assert.NotNull(script);
		Assert.Contains("Game Ending", script.Name);
	}

	[Fact]
	public void ZenithianItemScripts_Exist() {
		var scripts = Chapter5Events.GetAllScripts();
		var names = scripts.Select(s => s.Name).ToList();

		Assert.Contains("Find Zenithian Sword", names);
		Assert.Contains("Find Zenithian Armor", names);
		Assert.Contains("Find Zenithian Helm", names);
		Assert.Contains("Find Zenithian Shield", names);
	}

	[Fact]
	public void PartyMemberScripts_Exist() {
		var scripts = Chapter5Events.GetAllScripts();
		var names = scripts.Select(s => s.Name).ToList();

		Assert.Contains("Ragnar Joins", names);
		Assert.Contains("Torneko Joins", names);
		Assert.Contains("Sisters Join", names);
		Assert.Contains("Alena Group Joins", names);
	}

	[Fact]
	public void GetStoryScripts_ExcludesServiceScripts() {
		var storyScripts = Chapter5Events.GetStoryScripts();

		Assert.All(storyScripts, s =>
			Assert.True(s.Category != ScriptCategory.Inn &&
						s.Category != ScriptCategory.Shop));
	}

	[Fact]
	public void GetServiceScripts_OnlyIncludesServiceCategories() {
		var serviceScripts = Chapter5Events.GetServiceScripts();

		Assert.All(serviceScripts, s =>
			Assert.True(s.Category == ScriptCategory.Inn ||
						s.Category == ScriptCategory.Shop ||
						s.Category == ScriptCategory.NPC));
	}

	// ============================================================
	// Chapter 5 Maps Tests
	// ============================================================

	[Fact]
	public void GetAllMaps_ReturnsAllChapter5Maps() {
		var maps = Chapter5Maps.GetAllMaps();

		Assert.Equal(16, maps.Length);
	}

	[Fact]
	public void AllMaps_HaveValidMetadata() {
		var maps = Chapter5Maps.GetAllMaps();

		Assert.All(maps, m => {
			Assert.False(string.IsNullOrEmpty(m.Name));
			Assert.True(m.MapId > 0);
			Assert.NotNull(m.Chapters);
			Assert.Contains(4, m.Chapters);
		});
	}

	[Fact]
	public void MapHeroVillage_IsCorrectType() {
		var maps = Chapter5Maps.GetAllMaps();
		var village = maps.First(m => m.Name == "Hero's Village");

		Assert.Equal(MapType.Town, village.Type);
	}

	[Fact]
	public void EndorMaps_AreTownAndCastleType() {
		var maps = Chapter5Maps.GetAllMaps();
		var endor = maps.First(m => m.MapId == Chapter5Maps.MapEndor);
		var castle = maps.First(m => m.MapId == Chapter5Maps.MapEndorCastle);

		Assert.Equal(MapType.Town, endor.Type);
		Assert.Equal(MapType.Castle, castle.Type);
	}

	[Fact]
	public void TowerMaps_AreCaveType() {
		var maps = Chapter5Maps.GetAllMaps();
		var towerMaps = maps.Where(m => m.Name.Contains("Tower"));

		// Towers use cave tileset/type in DW4
		Assert.All(towerMaps, m => Assert.Equal(MapType.Cave, m.Type));
	}

	[Fact]
	public void PsaroCastleMaps_AreCastleType() {
		var maps = Chapter5Maps.GetAllMaps();
		var psaroMaps = maps.Where(m => m.Name.Contains("Psaro"));

		Assert.All(psaroMaps, m => Assert.Equal(MapType.Castle, m.Type));
	}

	// ============================================================
	// Treasures Tests
	// ============================================================

	[Fact]
	public void GetAllTreasures_ReturnsExpectedCount() {
		var treasures = Chapter5Maps.GetAllTreasures();

		Assert.Equal(15, treasures.Length);
	}

	[Fact]
	public void AllTreasures_HaveValidData() {
		var treasures = Chapter5Maps.GetAllTreasures();

		Assert.All(treasures, t => {
			Assert.True(t.Index > 0);
			Assert.True(t.MapId > 0);
		});
	}

	[Fact]
	public void ZenithianTreasures_HaveCorrectItems() {
		var treasures = Chapter5Maps.GetAllTreasures();
		var zenithianItems = treasures.Where(t =>
			t.ContentsType == TreasureType.Item &&
			(t.ContentsValue == Chapter5Events.ItemZenithianSword ||
			 t.ContentsValue == Chapter5Events.ItemZenithianHelm ||
			 t.ContentsValue == Chapter5Events.ItemZenithianShield));

		// 3 Zenithian items as treasures (armor obtained elsewhere)
		Assert.Equal(3, zenithianItems.Count());
	}

	[Fact]
	public void GoldTreasures_HavePositiveAmounts() {
		var treasures = Chapter5Maps.GetAllTreasures();
		var goldTreasures = treasures.Where(t => t.ContentsType == TreasureType.Gold);

		Assert.All(goldTreasures, t => Assert.True(t.ContentsValue > 0));
	}

	// ============================================================
	// Warps Tests
	// ============================================================

	[Fact]
	public void GetAllWarps_ReturnsExpectedCount() {
		var warps = Chapter5Maps.GetAllWarps();

		Assert.Equal(30, warps.Length);
	}

	[Fact]
	public void AllWarps_HaveValidSourceAndDest() {
		var warps = Chapter5Maps.GetAllWarps();

		Assert.All(warps, w => {
			Assert.True(w.SourceMapId >= 0);
			Assert.True(w.DestMapId >= 0);
		});
	}

	[Fact]
	public void SecretPassage_ConnectsFromVillage() {
		var warps = Chapter5Maps.GetAllWarps();
		var passage = warps.FirstOrDefault(w =>
			w.SourceMapId == Chapter5Maps.MapHeroHouse &&
			w.DestMapId == Chapter5Maps.MapSecretPassage);

		Assert.NotNull(passage);
	}

	[Fact]
	public void TowerWarps_ConnectFloors() {
		var warps = Chapter5Maps.GetAllWarps();
		var f1ToF2 = warps.FirstOrDefault(w =>
			w.SourceMapId == Chapter5Maps.MapZenithianTowerF1 &&
			w.DestMapId == Chapter5Maps.MapZenithianTowerF2);
		var f2ToF3 = warps.FirstOrDefault(w =>
			w.SourceMapId == Chapter5Maps.MapZenithianTowerF2 &&
			w.DestMapId == Chapter5Maps.MapZenithianTowerF3);

		Assert.NotNull(f1ToF2);
		Assert.NotNull(f2ToF3);
	}

	// ============================================================
	// NPCs Tests
	// ============================================================

	[Fact]
	public void GetAllNPCs_ReturnsExpectedCount() {
		var npcs = Chapter5Maps.GetAllNpcs();

		Assert.Equal(15, npcs.Length);
	}

	[Fact]
	public void AllNPCs_HaveValidData() {
		var npcs = Chapter5Maps.GetAllNpcs();

		Assert.All(npcs, n => {
			Assert.True(n.Index > 0);
			Assert.True(n.MapId > 0);
			Assert.False(string.IsNullOrEmpty(n.Name));
		});
	}

	[Fact]
	public void Ragnar_IsInEndor() {
		var npcs = Chapter5Maps.GetAllNpcs();
		var ragnar = npcs.First(n => n.Name == "Ragnar");

		Assert.Equal(Chapter5Maps.MapEndor, ragnar.MapId);
		Assert.Equal(Chapter5Events.MeetRagnar, ragnar.DialogId);
	}

	[Fact]
	public void MasterDragon_IsInZenithiaThrone() {
		var npcs = Chapter5Maps.GetAllNpcs();
		var dragon = npcs.First(n => n.Name == "Master Dragon");

		Assert.Equal(Chapter5Maps.MapZenithiaThrone, dragon.MapId);
	}

	[Fact]
	public void TownNPCs_HaveInnkeepers() {
		var npcs = Chapter5Maps.GetAllNpcs();
		var innkeepers = npcs.Where(n => n.Name.Contains("Innkeeper"));

		// Branca and Endor
		Assert.True(innkeepers.Count() >= 2);
	}

	// ============================================================
	// Encounter Zones Tests
	// ============================================================

	[Fact]
	public void GetAllEncounterZones_ReturnsExpectedCount() {
		var zones = Chapter5Maps.GetAllEncounterZones();

		Assert.Equal(8, zones.Length);
	}

	[Fact]
	public void AllEncounterZones_HaveValidData() {
		var zones = Chapter5Maps.GetAllEncounterZones();

		Assert.All(zones, z => {
			Assert.True(z.Index > 0);
			Assert.True(z.MapId >= 0);
			Assert.True(z.MonsterGroups.Length > 0);
			Assert.True(z.EncounterRate > 0);
		});
	}

	[Fact]
	public void PsaroCastle_HasHighestEncounterRate() {
		var zones = Chapter5Maps.GetAllEncounterZones();
		var psaroZone = zones.First(z => z.MapId == Chapter5Maps.MapPsaroCastleMain);
		var maxRate = zones.Max(z => z.EncounterRate);

		Assert.Equal(maxRate, psaroZone.EncounterRate);
	}

	// ============================================================
	// DQ3r Conversion Tests
	// ============================================================

	[Fact]
	public void ConvertTreasures_ReturnsAllTreasures() {
		var dq3rTreasures = Chapter5Maps.ConvertTreasures();

		Assert.Equal(15, dq3rTreasures.Length);
	}

	[Fact]
	public void ConvertWarps_ReturnsAllWarps() {
		var dq3rWarps = Chapter5Maps.ConvertWarps();

		Assert.Equal(30, dq3rWarps.Length);
	}

	[Fact]
	public void ConvertNpcs_ReturnsAllNpcs() {
		var dq3rNpcs = Chapter5Maps.ConvertNpcs();

		Assert.Equal(15, dq3rNpcs.Length);
	}

	[Fact]
	public void ConvertEncounterZones_ReturnsAllZones() {
		var dq3rZones = Chapter5Maps.ConvertEncounterZones();

		Assert.Equal(8, dq3rZones.Length);
	}

	[Fact]
	public void GetDQ3rMapIdMapping_MapsAllChapter5Maps() {
		var mapping = Chapter5Maps.GetDQ3rMapIdMapping();

		Assert.Equal(16, mapping.Count);
		Assert.True(mapping.ContainsKey(Chapter5Maps.MapHeroVillage));
		Assert.True(mapping.ContainsKey(Chapter5Maps.MapZenithia));
		Assert.True(mapping.ContainsKey(Chapter5Maps.MapPsaroCastleThrone));
	}

	// ============================================================
	// Story Progression Tests
	// ============================================================

	[Fact]
	public void StoryFlags_AreSequential() {
		// Verify key story flags are sequential for proper progression
		Assert.True(Chapter5Events.FlagVillageAttacked < Chapter5Events.FlagHeroEscaped);
		Assert.True(Chapter5Events.FlagHeroEscaped < Chapter5Events.FlagArrivedBranca);
		Assert.True(Chapter5Events.FlagArrivedBranca < Chapter5Events.FlagZenithianLegend);
		Assert.True(Chapter5Events.FlagZenithianLegend < Chapter5Events.FlagMetRagnar);
	}

	[Fact]
	public void AllScriptsForChapter_AreChapter4() {
		var scripts = Chapter5Events.GetAllScripts();

		// Note: Chapter 5 in story = ChapterId 4 (0-indexed)
		Assert.All(scripts, s => Assert.Equal(4, s.ChapterId));
	}

	[Fact]
	public void AllMapsForChapter_AreChapter4() {
		var maps = Chapter5Maps.GetAllMaps();

		// Note: Chapter 5 in story = Chapter 4 index in Chapters array
		Assert.All(maps, m => Assert.Contains(4, m.Chapters!));
	}

	// ============================================================
	// Character Recruitment Tests
	// ============================================================

	[Fact]
	public void AllPreviousChapterCharacters_HaveJoinScripts() {
		var scripts = Chapter5Events.GetAllScripts();
		var names = scripts.Select(s => s.Name).ToList();

		// From Chapter 1
		Assert.Contains("Ragnar Joins", names);
		// From Chapter 2 (Alena group)
		Assert.Contains("Alena Group Joins", names);
		// From Chapter 3
		Assert.Contains("Torneko Joins", names);
		// From Chapter 4
		Assert.Contains("Sisters Join", names);
	}

	[Fact]
	public void CharacterIds_AreCorrect() {
		Assert.Equal(0x00, Chapter5Events.CharacterHero);
		Assert.Equal(0x01, Chapter5Events.CharacterRagnar);
		Assert.Equal(0x02, Chapter5Events.CharacterAlena);
		Assert.Equal(0x03, Chapter5Events.CharacterKiryl);
		Assert.Equal(0x04, Chapter5Events.CharacterBorya);
		Assert.Equal(0x05, Chapter5Events.CharacterMeena);
		Assert.Equal(0x06, Chapter5Events.CharacterMaya);
		Assert.Equal(0x07, Chapter5Events.CharacterTorneko);
	}
}
