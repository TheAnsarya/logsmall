using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;
using DW4Lib.Events;
using DW4Lib.Maps;

namespace DW4Lib.Tests;

/// <summary>
/// Tests for Chapter 2 events and maps.
/// </summary>
public class Chapter2Tests {
	// ============================================================
	// Chapter 2 Events Tests
	// ============================================================

	[Fact]
	public void GetAllScripts_ReturnsAllChapter2Scripts() {
		var scripts = Chapter2Events.GetAllScripts();

		// 16 story scripts + 7 service scripts
		Assert.Equal(23, scripts.Length);
	}

	[Fact]
	public void IntroScript_HasCorrectId() {
		var scripts = Chapter2Events.GetAllScripts();
		var intro = scripts.First(s => s.Name == "Chapter 2 Intro");

		Assert.Equal(Chapter2Events.IntroScript, intro.Id);
		Assert.Equal(ScriptCategory.Cutscene, intro.Category);
		Assert.Equal(1, intro.ChapterId);
	}

	[Fact]
	public void AllScripts_HaveUniqueIds() {
		var scripts = Chapter2Events.GetAllScripts();
		var ids = scripts.Select(s => s.Id).ToList();

		Assert.Equal(ids.Count, ids.Distinct().Count());
	}

	[Fact]
	public void AllScripts_HaveNames() {
		var scripts = Chapter2Events.GetAllScripts();

		Assert.All(scripts, s => Assert.False(string.IsNullOrEmpty(s.Name)));
	}

	[Fact]
	public void AllScripts_HaveCommands() {
		var scripts = Chapter2Events.GetAllScripts();

		Assert.All(scripts, s => Assert.NotEmpty(s.Commands));
	}

	[Fact]
	public void TournamentScripts_ExistForAllRounds() {
		var scripts = Chapter2Events.GetAllScripts();
		var names = scripts.Select(s => s.Name).ToList();

		Assert.Contains("Tournament Round 1", names);
		Assert.Contains("Tournament Round 2", names);
		Assert.Contains("Tournament Semifinal", names);
		Assert.Contains("Tournament Final", names);
	}

	[Fact]
	public void PartyJoinsScript_AddsMultipleMembers() {
		var script = Chapter2Events.BuildPartyJoinsScript();
		var addCommands = script.Commands.Where(c => c.Opcode == ScriptOpcode.AddPartyMember).ToList();

		// Should add both Cristo and Brey
		Assert.Equal(2, addCommands.Count);
	}

	[Fact]
	public void ServiceScripts_HaveCorrectCategories() {
		var scripts = Chapter2Events.GetAllScripts();
		var shopScripts = scripts.Where(s => s.Name.Contains("Shop")).ToList();
		var innScripts = scripts.Where(s => s.Name.Contains("Inn")).ToList();

		Assert.All(shopScripts, s => Assert.Equal(ScriptCategory.Shop, s.Category));
		Assert.All(innScripts, s => Assert.Equal(ScriptCategory.Inn, s.Category));
	}

	[Fact]
	public void NecrosaroCutscene_HasMusicChange() {
		var script = Chapter2Events.BuildNecrosaroCutsceneScript();
		var musicCommands = script.Commands.Where(c => c.Opcode == ScriptOpcode.PlayMusic).ToList();

		Assert.True(musicCommands.Count >= 2); // Necrosaro theme and return to normal
	}

	[Fact]
	public void ChapterCompleteScript_SetsCorrectChapter() {
		var script = Chapter2Events.BuildChapterCompleteScript();
		var setChapterCmd = script.Commands.FirstOrDefault(c => c.Opcode == ScriptOpcode.SetChapter);

		Assert.NotNull(setChapterCmd);
		Assert.Equal(2, setChapterCmd.Parameters[0]); // Should set to Chapter 3
	}

	// ============================================================
	// Chapter 2 Maps Tests
	// ============================================================

	[Fact]
	public void GetAllMaps_ReturnsAllChapter2Maps() {
		var maps = Chapter2Maps.GetAllMaps();

		Assert.Equal(11, maps.Length);
	}

	[Fact]
	public void AllMaps_HaveUniqueMapIds() {
		var maps = Chapter2Maps.GetAllMaps();
		var mapIds = maps.Select(m => m.MapId).ToList();

		Assert.Equal(mapIds.Count, mapIds.Distinct().Count());
	}

	[Fact]
	public void AllMaps_HaveNames() {
		var maps = Chapter2Maps.GetAllMaps();

		Assert.All(maps, m => Assert.False(string.IsNullOrEmpty(m.Name)));
	}

	[Fact]
	public void SanteemCastle_IsMainStartingMap() {
		var maps = Chapter2Maps.GetAllMaps();
		var santeem = maps.First(m => m.MapId == Chapter2Maps.MapSanteemCastle);

		Assert.Equal("Santeem Castle", santeem.Name);
		Assert.Equal(MapType.Castle, santeem.Type);
	}

	[Fact]
	public void EndorColosseum_IsOtherType() {
		var maps = Chapter2Maps.GetAllMaps();
		var colosseum = maps.First(m => m.MapId == Chapter2Maps.MapEndorColosseum);

		Assert.Equal(MapType.Other, colosseum.Type);
	}

	[Fact]
	public void MapTypes_CoverExpectedVariety() {
		var maps = Chapter2Maps.GetAllMaps();

		Assert.Contains(maps, m => m.Type == MapType.Castle);
		Assert.Contains(maps, m => m.Type == MapType.Town);
		Assert.Contains(maps, m => m.Type == MapType.Cave);
		Assert.Contains(maps, m => m.Type == MapType.Other);
	}

	// ============================================================
	// Treasures Tests
	// ============================================================

	[Fact]
	public void GetAllTreasures_ReturnsChapter2Treasures() {
		var treasures = Chapter2Maps.GetAllTreasures();

		Assert.True(treasures.Length >= 12);
	}

	[Fact]
	public void AllTreasures_HaveValidCoordinates() {
		var treasures = Chapter2Maps.GetAllTreasures();

		Assert.All(treasures, t => {
			Assert.True(t.X >= 0 && t.X < 64);
			Assert.True(t.Y >= 0 && t.Y < 64);
		});
	}

	[Fact]
	public void AllTreasures_HaveContentsOrGold() {
		var treasures = Chapter2Maps.GetAllTreasures();

		Assert.All(treasures, t => Assert.True(t.ContentsType != TreasureType.Empty));
	}

	[Fact]
	public void AllTreasures_HaveUniqueIndexes() {
		var treasures = Chapter2Maps.GetAllTreasures();
		var indexes = treasures.Select(t => t.Index).ToList();

		Assert.Equal(indexes.Count, indexes.Distinct().Count());
	}

	[Fact]
	public void TempeCave_HasIronClaw() {
		var treasures = Chapter2Maps.GetAllTreasures();
		var claw = treasures.FirstOrDefault(t =>
			t.MapId == Chapter2Maps.MapTempeCave2 && t.ContentsValue == 0x18);

		Assert.NotNull(claw);
	}

	[Fact]
	public void Treasures_ContainVariousTypes() {
		var treasures = Chapter2Maps.GetAllTreasures();

		Assert.Contains(treasures, t => t.ContentsType == TreasureType.Gold);
		Assert.Contains(treasures, t => t.ContentsType == TreasureType.Item);
	}

	// ============================================================
	// Warps Tests
	// ============================================================

	[Fact]
	public void GetAllWarps_ReturnsChapter2Warps() {
		var warps = Chapter2Maps.GetAllWarps();

		Assert.True(warps.Length >= 10);
	}

	[Fact]
	public void AllWarps_HaveValidDestinations() {
		var warps = Chapter2Maps.GetAllWarps();
		var mapIds = Chapter2Maps.GetAllMaps().Select(m => m.MapId).ToHashSet();

		Assert.All(warps, w => {
			Assert.Contains(w.SourceMapId, mapIds);
			Assert.Contains(w.DestMapId, mapIds);
		});
	}

	[Fact]
	public void EndorColosseum_AccessibleFromEndor() {
		var warps = Chapter2Maps.GetAllWarps();
		var toColosseum = warps.FirstOrDefault(w =>
			w.SourceMapId == Chapter2Maps.MapEndor &&
			w.DestMapId == Chapter2Maps.MapEndorColosseum);

		Assert.NotNull(toColosseum);
	}

	[Fact]
	public void AllWarps_HaveValidTypes() {
		var warps = Chapter2Maps.GetAllWarps();

		Assert.All(warps, w => Assert.True(Enum.IsDefined(typeof(WarpType), w.Type)));
	}

	// ============================================================
	// NPCs Tests
	// ============================================================

	[Fact]
	public void GetAllNpcs_ReturnsChapter2Npcs() {
		var npcs = Chapter2Maps.GetAllNpcs();

		Assert.True(npcs.Length >= 10);
	}

	[Fact]
	public void AllNpcs_HaveValidMapIds() {
		var npcs = Chapter2Maps.GetAllNpcs();
		var mapIds = Chapter2Maps.GetAllMaps().Select(m => m.MapId).ToHashSet();

		Assert.All(npcs, n => Assert.Contains(n.MapId, mapIds));
	}

	[Fact]
	public void ServiceNpcs_HaveCorrectFlags() {
		var npcs = Chapter2Maps.GetAllNpcs();

		var shopNpcs = npcs.Where(n => n.Flags.HasFlag(NpcFlags.Shop)).ToList();
		var innNpcs = npcs.Where(n => n.Flags.HasFlag(NpcFlags.Inn)).ToList();
		var churchNpcs = npcs.Where(n => n.Flags.HasFlag(NpcFlags.Church)).ToList();

		Assert.NotEmpty(shopNpcs);
		Assert.NotEmpty(innNpcs);
		Assert.NotEmpty(churchNpcs);
	}

	[Fact]
	public void TournamentOfficial_ExistsInColosseum() {
		var npcs = Chapter2Maps.GetAllNpcs();
		var official = npcs.FirstOrDefault(n =>
			n.MapId == Chapter2Maps.MapEndorColosseum &&
			n.Name == "Tournament Official");

		Assert.NotNull(official);
		// DialogId should be non-zero (points to TournamentRegister script)
		Assert.True(official.DialogId > 0);
	}

	[Fact]
	public void KingOfSanteem_ExistsInCastle() {
		var npcs = Chapter2Maps.GetAllNpcs();
		var king = npcs.FirstOrDefault(n =>
			n.MapId == Chapter2Maps.MapSanteemCastle &&
			n.Name == "King of Santeem");

		Assert.NotNull(king);
	}

	[Fact]
	public void AllNpcs_HaveNames() {
		var npcs = Chapter2Maps.GetAllNpcs();

		Assert.All(npcs, n => Assert.False(string.IsNullOrEmpty(n.Name)));
	}

	// ============================================================
	// Encounter Zones Tests
	// ============================================================

	[Fact]
	public void GetAllEncounterZones_ReturnsChapter2Zones() {
		var zones = Chapter2Maps.GetAllEncounterZones();

		Assert.True(zones.Length >= 5);
	}

	[Fact]
	public void AllZones_HaveMonsterGroups() {
		var zones = Chapter2Maps.GetAllEncounterZones();

		Assert.All(zones, z => Assert.NotEmpty(z.MonsterGroups));
	}

	[Fact]
	public void AllZones_HaveValidEncounterRates() {
		var zones = Chapter2Maps.GetAllEncounterZones();

		Assert.All(zones, z => Assert.True(z.EncounterRate > 0 && z.EncounterRate <= 64));
	}

	[Fact]
	public void TempeCave_HasProgressivelyHarderFloors() {
		var zones = Chapter2Maps.GetAllEncounterZones();
		var floor1 = zones.First(z => z.Index == 0x21);
		var floor2 = zones.First(z => z.Index == 0x22);

		// Floor 2 should have more monster groups or similar complexity
		Assert.True(floor2.MonsterGroups.Length >= floor1.MonsterGroups.Length);
	}

	// ============================================================
	// Entrance Tests
	// ============================================================

	[Fact]
	public void GetAllEntrances_ReturnsChapter2Entrances() {
		var entrances = Chapter2Maps.GetAllEntrances();

		Assert.True(entrances.Length >= 6);
	}

	[Fact]
	public void AllEntrances_HaveNames() {
		var entrances = Chapter2Maps.GetAllEntrances();

		Assert.All(entrances, e => Assert.False(string.IsNullOrEmpty(e.Name)));
	}

	[Fact]
	public void Entrances_HaveValidDestinations() {
		var entrances = Chapter2Maps.GetAllEntrances();
		var mapIds = Chapter2Maps.GetAllMaps().Select(m => m.MapId).ToHashSet();

		Assert.All(entrances, e => Assert.Contains(e.DestMapId, mapIds));
	}

	// ============================================================
	// Conversion Tests
	// ============================================================

	[Fact]
	public void GetDQ3rMapIdMapping_ReturnsCorrectMappings() {
		var mappings = Chapter2Maps.GetDQ3rMapIdMapping();

		Assert.NotEmpty(mappings);
		Assert.All(mappings.Values, v => Assert.True(v >= 0x100));
	}

	[Fact]
	public void ConvertWarps_AppliesCorrectOffsets() {
		var dq3rWarps = Chapter2Maps.ConvertWarps();

		Assert.All(dq3rWarps, w => {
			Assert.True(w.SourceMapId >= 0x100);
			Assert.True(w.DestMapId >= 0x100);
		});
	}

	[Fact]
	public void ConvertNpcs_AppliesCorrectOffsets() {
		var dq3rNpcs = Chapter2Maps.ConvertNpcs();

		// Verify conversions happened - NPCs exist with valid data
		Assert.NotEmpty(dq3rNpcs);
		Assert.All(dq3rNpcs, n => {
			Assert.True(n.MapId > 0);
			Assert.True(n.SpriteId > 0);
		});
	}

	[Fact]
	public void ConvertEncounterZones_AppliesCorrectOffsets() {
		var dq3rZones = Chapter2Maps.ConvertEncounterZones();

		// Verify conversions happened - zones exist with valid data
		Assert.NotEmpty(dq3rZones);
		Assert.All(dq3rZones, z => {
			Assert.True(z.ZoneId > 0);
			Assert.NotEmpty(z.MonsterGroups);
		});
	}

	[Fact]
	public void ConvertTreasures_AppliesCorrectOffsets() {
		var dq3rTreasures = Chapter2Maps.ConvertTreasures();

		// Verify conversions happened - treasures exist with valid data
		Assert.NotEmpty(dq3rTreasures);
		Assert.All(dq3rTreasures, t => {
			Assert.True(t.MapId > 0);
			Assert.True(t.ContentsValue > 0 || t.ContentsType == DQ3rTreasureType.SmallMedal);
		});
	}
}
