using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;
using DW4Lib.Events;
using DW4Lib.Maps;

namespace DW4Lib.Tests;

/// <summary>
/// Tests for Chapter 3 events and maps.
/// </summary>
public class Chapter3Tests {
	// ============================================================
	// Chapter 3 Events Tests
	// ============================================================

	[Fact]
	public void GetAllScripts_ReturnsAllChapter3Scripts() {
		var scripts = Chapter3Events.GetAllScripts();

		// 21 story scripts + 9 service scripts
		Assert.Equal(30, scripts.Length);
	}

	[Fact]
	public void IntroScript_HasCorrectId() {
		var scripts = Chapter3Events.GetAllScripts();
		var intro = scripts.First(s => s.Name == "Chapter 3 Intro");

		Assert.Equal(Chapter3Events.IntroScript, intro.Id);
		Assert.Equal(ScriptCategory.Cutscene, intro.Category);
		Assert.Equal(2, intro.ChapterId);
	}

	[Fact]
	public void AllScripts_HaveUniqueIds() {
		var scripts = Chapter3Events.GetAllScripts();
		var ids = scripts.Select(s => s.Id).ToList();

		Assert.Equal(ids.Count, ids.Distinct().Count());
	}

	[Fact]
	public void AllScripts_HaveNames() {
		var scripts = Chapter3Events.GetAllScripts();

		Assert.All(scripts, s => Assert.False(string.IsNullOrEmpty(s.Name)));
	}

	[Fact]
	public void AllScripts_HaveCommands() {
		var scripts = Chapter3Events.GetAllScripts();

		Assert.All(scripts, s => Assert.NotEmpty(s.Commands));
	}

	[Fact]
	public void TutorialScripts_Exist() {
		var scripts = Chapter3Events.GetAllScripts();
		var names = scripts.Select(s => s.Name).ToList();

		Assert.Contains("Shop Tutorial Buy", names);
		Assert.Contains("Shop Tutorial Sell", names);
	}

	[Fact]
	public void QuestScripts_CoverMainQuests() {
		var scripts = Chapter3Events.GetAllScripts();
		var names = scripts.Select(s => s.Name).ToList();

		Assert.Contains("Find Steel Sword", names);
		Assert.Contains("Prince Reed", names);
		Assert.Contains("Silver Statuette Cave", names);
		Assert.Contains("Deliver Statuette", names);
	}

	[Fact]
	public void ServiceScripts_HaveCorrectCategories() {
		var scripts = Chapter3Events.GetAllScripts();
		var shopScripts = scripts.Where(s => s.Name.Contains("Shop")).ToList();
		var innScripts = scripts.Where(s => s.Name.Contains("Inn")).ToList();

		Assert.All(shopScripts, s => Assert.Equal(ScriptCategory.Shop, s.Category));
		Assert.All(innScripts, s => Assert.Equal(ScriptCategory.Inn, s.Category));
	}

	[Fact]
	public void ChurchScript_HasCorrectCategory() {
		var scripts = Chapter3Events.GetAllScripts();
		var church = scripts.First(s => s.Name == "Lakanaba Church");

		Assert.Equal(ScriptCategory.NPC, church.Category);
	}

	[Fact]
	public void ChapterCompleteScript_SetsCorrectChapter() {
		var script = Chapter3Events.BuildChapterCompleteScript();
		var setChapterCmd = script.Commands.FirstOrDefault(c => c.Opcode == ScriptOpcode.SetChapter);

		Assert.NotNull(setChapterCmd);
		Assert.Equal(3, setChapterCmd.Parameters[0]); // Should set to Chapter 4
	}

	// ============================================================
	// Chapter 3 Maps Tests
	// ============================================================

	[Fact]
	public void GetAllMaps_ReturnsAllChapter3Maps() {
		var maps = Chapter3Maps.GetAllMaps();

		Assert.Equal(12, maps.Length);
	}

	[Fact]
	public void AllMaps_HaveUniqueMapIds() {
		var maps = Chapter3Maps.GetAllMaps();
		var mapIds = maps.Select(m => m.MapId).ToList();

		Assert.Equal(mapIds.Count, mapIds.Distinct().Count());
	}

	[Fact]
	public void AllMaps_HaveNames() {
		var maps = Chapter3Maps.GetAllMaps();

		Assert.All(maps, m => Assert.False(string.IsNullOrEmpty(m.Name)));
	}

	[Fact]
	public void Lakanaba_IsMainStartingMap() {
		var maps = Chapter3Maps.GetAllMaps();
		var lakanaba = maps.First(m => m.MapId == Chapter3Maps.MapLakanaba);

		Assert.Equal("Lakanaba", lakanaba.Name);
		Assert.Equal(MapType.Town, lakanaba.Type);
	}

	[Fact]
	public void TaloonHouse_IsOtherType() {
		var maps = Chapter3Maps.GetAllMaps();
		var house = maps.First(m => m.MapId == Chapter3Maps.MapTaloonHouse);

		Assert.Equal(MapType.Other, house.Type);
	}

	[Fact]
	public void MapTypes_CoverExpectedVariety() {
		var maps = Chapter3Maps.GetAllMaps();

		Assert.Contains(maps, m => m.Type == MapType.Town);
		Assert.Contains(maps, m => m.Type == MapType.Castle);
		Assert.Contains(maps, m => m.Type == MapType.Cave);
		Assert.Contains(maps, m => m.Type == MapType.Other);
	}

	[Fact]
	public void FoxVillage_ExistsAsUniqueTown() {
		var maps = Chapter3Maps.GetAllMaps();
		var foxVillage = maps.First(m => m.MapId == Chapter3Maps.MapFoxVillage);

		Assert.Equal("Fox Village", foxVillage.Name);
		Assert.Equal(MapType.Town, foxVillage.Type);
	}

	// ============================================================
	// Treasures Tests
	// ============================================================

	[Fact]
	public void GetAllTreasures_ReturnsChapter3Treasures() {
		var treasures = Chapter3Maps.GetAllTreasures();

		Assert.True(treasures.Length >= 13);
	}

	[Fact]
	public void AllTreasures_HaveValidCoordinates() {
		var treasures = Chapter3Maps.GetAllTreasures();

		Assert.All(treasures, t => {
			Assert.True(t.X >= 0 && t.X < 64);
			Assert.True(t.Y >= 0 && t.Y < 64);
		});
	}

	[Fact]
	public void AllTreasures_HaveContents() {
		var treasures = Chapter3Maps.GetAllTreasures();

		Assert.All(treasures, t => Assert.True(t.ContentsType != TreasureType.Empty));
	}

	[Fact]
	public void AllTreasures_HaveUniqueIndexes() {
		var treasures = Chapter3Maps.GetAllTreasures();
		var indexes = treasures.Select(t => t.Index).ToList();

		Assert.Equal(indexes.Count, indexes.Distinct().Count());
	}

	[Fact]
	public void SteelBroadsword_InEastCave() {
		var treasures = Chapter3Maps.GetAllTreasures();
		var sword = treasures.FirstOrDefault(t =>
			t.MapId == Chapter3Maps.MapEastCaveF2 && t.ContentsValue == 0x15);

		Assert.NotNull(sword);
	}

	[Fact]
	public void SilverStatuette_InSilverCave() {
		var treasures = Chapter3Maps.GetAllTreasures();
		var statuette = treasures.FirstOrDefault(t =>
			t.MapId == Chapter3Maps.MapSilverCaveF2 && t.ContentsValue == 0x61);

		Assert.NotNull(statuette);
	}

	[Fact]
	public void Treasures_ContainVariousTypes() {
		var treasures = Chapter3Maps.GetAllTreasures();

		Assert.Contains(treasures, t => t.ContentsType == TreasureType.Gold);
		Assert.Contains(treasures, t => t.ContentsType == TreasureType.Item);
		Assert.Contains(treasures, t => t.ContentsType == TreasureType.SmallMedal);
	}

	// ============================================================
	// Warps Tests
	// ============================================================

	[Fact]
	public void GetAllWarps_ReturnsChapter3Warps() {
		var warps = Chapter3Maps.GetAllWarps();

		Assert.True(warps.Length >= 12);
	}

	[Fact]
	public void AllWarps_HaveValidTypes() {
		var warps = Chapter3Maps.GetAllWarps();

		Assert.All(warps, w => Assert.True(Enum.IsDefined(typeof(WarpType), w.Type)));
	}

	[Fact]
	public void WeaponShop_AccessibleFromLakanaba() {
		var warps = Chapter3Maps.GetAllWarps();
		var toShop = warps.FirstOrDefault(w =>
			w.SourceMapId == Chapter3Maps.MapLakanaba &&
			w.DestMapId == Chapter3Maps.MapLakanabaWeaponShop);

		Assert.NotNull(toShop);
	}

	[Fact]
	public void TaloonHouse_AccessibleFromLakanaba() {
		var warps = Chapter3Maps.GetAllWarps();
		var toHouse = warps.FirstOrDefault(w =>
			w.SourceMapId == Chapter3Maps.MapLakanaba &&
			w.DestMapId == Chapter3Maps.MapTaloonHouse);

		Assert.NotNull(toHouse);
	}

	[Fact]
	public void CaveWarps_HaveStairTypes() {
		var warps = Chapter3Maps.GetAllWarps();
		var caveWarps = warps.Where(w =>
			w.SourceMapId == Chapter3Maps.MapEastCaveF1 ||
			w.SourceMapId == Chapter3Maps.MapEastCaveF2).ToList();

		Assert.Contains(caveWarps, w => w.Type == WarpType.StairsDown);
		Assert.Contains(caveWarps, w => w.Type == WarpType.StairsUp);
	}

	// ============================================================
	// NPCs Tests
	// ============================================================

	[Fact]
	public void GetAllNpcs_ReturnsChapter3Npcs() {
		var npcs = Chapter3Maps.GetAllNpcs();

		Assert.True(npcs.Length >= 12);
	}

	[Fact]
	public void AllNpcs_HaveValidMapIds() {
		var npcs = Chapter3Maps.GetAllNpcs();
		var mapIds = Chapter3Maps.GetAllMaps().Select(m => m.MapId).ToHashSet();

		Assert.All(npcs, n => Assert.Contains(n.MapId, mapIds));
	}

	[Fact]
	public void ServiceNpcs_HaveCorrectFlags() {
		var npcs = Chapter3Maps.GetAllNpcs();

		var shopNpcs = npcs.Where(n => n.Flags.HasFlag(NpcFlags.Shop)).ToList();
		var innNpcs = npcs.Where(n => n.Flags.HasFlag(NpcFlags.Inn)).ToList();
		var churchNpcs = npcs.Where(n => n.Flags.HasFlag(NpcFlags.Church)).ToList();

		Assert.NotEmpty(shopNpcs);
		Assert.NotEmpty(innNpcs);
		Assert.NotEmpty(churchNpcs);
	}

	[Fact]
	public void Neta_ExistsInTaloonHouse() {
		var npcs = Chapter3Maps.GetAllNpcs();
		var neta = npcs.FirstOrDefault(n =>
			n.MapId == Chapter3Maps.MapTaloonHouse &&
			n.Name == "Neta");

		Assert.NotNull(neta);
	}

	[Fact]
	public void PrinceReed_ExistsInBonmalmoCastle() {
		var npcs = Chapter3Maps.GetAllNpcs();
		var prince = npcs.FirstOrDefault(n =>
			n.MapId == Chapter3Maps.MapBonmalmoCastle &&
			n.Name == "Prince Reed");

		Assert.NotNull(prince);
	}

	[Fact]
	public void FoxShopkeeper_ExistsInFoxVillage() {
		var npcs = Chapter3Maps.GetAllNpcs();
		var fox = npcs.FirstOrDefault(n =>
			n.MapId == Chapter3Maps.MapFoxVillage &&
			n.Name == "Fox Shopkeeper");

		Assert.NotNull(fox);
		Assert.True(fox.Flags.HasFlag(NpcFlags.Shop));
	}

	[Fact]
	public void AllNpcs_HaveNames() {
		var npcs = Chapter3Maps.GetAllNpcs();

		Assert.All(npcs, n => Assert.False(string.IsNullOrEmpty(n.Name)));
	}

	// ============================================================
	// Encounter Zones Tests
	// ============================================================

	[Fact]
	public void GetAllEncounterZones_ReturnsChapter3Zones() {
		var zones = Chapter3Maps.GetAllEncounterZones();

		Assert.True(zones.Length >= 5);
	}

	[Fact]
	public void AllZones_HaveMonsterGroups() {
		var zones = Chapter3Maps.GetAllEncounterZones();

		Assert.All(zones, z => Assert.NotEmpty(z.MonsterGroups));
	}

	[Fact]
	public void AllZones_HaveValidEncounterRates() {
		var zones = Chapter3Maps.GetAllEncounterZones();

		Assert.All(zones, z => Assert.True(z.EncounterRate > 0 && z.EncounterRate <= 64));
	}

	[Fact]
	public void SilverCave_HasProgressiveDifficulty() {
		var zones = Chapter3Maps.GetAllEncounterZones();
		var floor1 = zones.First(z => z.MapId == Chapter3Maps.MapSilverCaveF1);
		var floor2 = zones.First(z => z.MapId == Chapter3Maps.MapSilverCaveF2);

		// Floor 2 should have higher encounter rate (harder)
		Assert.True(floor2.EncounterRate >= floor1.EncounterRate);
	}

	// ============================================================
	// Entrance Tests
	// ============================================================

	[Fact]
	public void GetAllEntrances_ReturnsChapter3Entrances() {
		var entrances = Chapter3Maps.GetAllEntrances();

		Assert.True(entrances.Length >= 7);
	}

	[Fact]
	public void AllEntrances_HaveNames() {
		var entrances = Chapter3Maps.GetAllEntrances();

		Assert.All(entrances, e => Assert.False(string.IsNullOrEmpty(e.Name)));
	}

	[Fact]
	public void Entrances_HaveValidDestinations() {
		var entrances = Chapter3Maps.GetAllEntrances();
		var mapIds = Chapter3Maps.GetAllMaps().Select(m => m.MapId).ToHashSet();

		Assert.All(entrances, e => Assert.Contains(e.DestMapId, mapIds));
	}

	// ============================================================
	// Conversion Tests
	// ============================================================

	[Fact]
	public void GetDQ3rMapIdMapping_ReturnsCorrectMappings() {
		var mappings = Chapter3Maps.GetDQ3rMapIdMapping();

		Assert.NotEmpty(mappings);
		// Chapter 3 maps should be in 0x130+ range
		Assert.True(mappings.Values.Count(v => v >= 0x130) > 0);
	}

	[Fact]
	public void ConvertWarps_ProducesValidWarps() {
		var dq3rWarps = Chapter3Maps.ConvertWarps();

		Assert.NotEmpty(dq3rWarps);
		Assert.All(dq3rWarps, w => {
			Assert.True(w.SourceMapId > 0);
			Assert.True(w.DestMapId > 0);
		});
	}

	[Fact]
	public void ConvertNpcs_ProducesValidNpcs() {
		var dq3rNpcs = Chapter3Maps.ConvertNpcs();

		Assert.NotEmpty(dq3rNpcs);
		Assert.All(dq3rNpcs, n => {
			Assert.True(n.MapId > 0);
			Assert.True(n.SpriteId > 0);
		});
	}

	[Fact]
	public void ConvertEncounterZones_ProducesValidZones() {
		var dq3rZones = Chapter3Maps.ConvertEncounterZones();

		Assert.NotEmpty(dq3rZones);
		Assert.All(dq3rZones, z => {
			Assert.True(z.ZoneId > 0);
			Assert.NotEmpty(z.MonsterGroups);
		});
	}

	[Fact]
	public void ConvertTreasures_ProducesValidTreasures() {
		var dq3rTreasures = Chapter3Maps.ConvertTreasures();

		Assert.NotEmpty(dq3rTreasures);
		Assert.All(dq3rTreasures, t => {
			Assert.True(t.MapId > 0);
		});
	}
}
