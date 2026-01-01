using DW4Lib.Maps;
using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;

namespace DW4Lib.Tests;

/// <summary>
/// Tests for Chapter 1 map data and conversion.
/// </summary>
public class Chapter1MapTests {
	// ============================================================
	// Map Metadata Tests
	// ============================================================

	[Fact]
	public void GetAllMaps_ReturnsChapter1Maps() {
		var maps = Chapter1Maps.GetAllMaps();
		Assert.NotEmpty(maps);
		Assert.Contains(maps, m => m.Name == "Burland Castle");
		Assert.Contains(maps, m => m.Name == "Loch Tower - Top");
	}

	[Fact]
	public void GetAllMaps_ContainsCorrectMapTypes() {
		var maps = Chapter1Maps.GetAllMaps();
		Assert.Contains(maps, m => m.Type == MapType.Castle);
		Assert.Contains(maps, m => m.Type == MapType.Town);
		Assert.Contains(maps, m => m.Type == MapType.Tower);
		Assert.Contains(maps, m => m.Type == MapType.Cave);
	}

	[Fact]
	public void GetAllMaps_ChaptersSetForMainMaps() {
		var maps = Chapter1Maps.GetAllMaps();
		var burland = maps.First(m => m.MapId == Chapter1Maps.MapBurlandMain);
		Assert.NotNull(burland.Chapters);
		Assert.Contains(0, burland.Chapters!); // Chapter 0 = Chapter 1 index
	}

	// ============================================================
	// Treasure Tests
	// ============================================================

	[Fact]
	public void GetAllTreasures_ReturnsChapter1Treasures() {
		var treasures = Chapter1Maps.GetAllTreasures();
		Assert.NotEmpty(treasures);
		Assert.True(treasures.Length >= 10);
	}

	[Fact]
	public void GetAllTreasures_ContainsVariousTypes() {
		var treasures = Chapter1Maps.GetAllTreasures();
		Assert.Contains(treasures, t => t.ContentsType == TreasureType.Gold);
		Assert.Contains(treasures, t => t.ContentsType == TreasureType.Item);
		Assert.Contains(treasures, t => t.ContentsType == TreasureType.SmallMedal);
	}

	[Fact]
	public void GetAllTreasures_FlyingShoesInLochTower() {
		var treasures = Chapter1Maps.GetAllTreasures();
		var flyingShoes = treasures.FirstOrDefault(t =>
			t.MapId == Chapter1Maps.MapLochTowerTop && t.ContentsValue == 0x2A);
		Assert.NotNull(flyingShoes);
	}

	[Fact]
	public void GetAllTreasures_HaveValidCoordinates() {
		var treasures = Chapter1Maps.GetAllTreasures();
		foreach (var t in treasures) {
			Assert.True(t.X >= 0 && t.X < 64);
			Assert.True(t.Y >= 0 && t.Y < 64);
		}
	}

	[Fact]
	public void GetAllTreasures_UniqueIndexes() {
		var treasures = Chapter1Maps.GetAllTreasures();
		var indexes = treasures.Select(t => t.Index).ToList();
		Assert.Equal(indexes.Count, indexes.Distinct().Count());
	}

	// ============================================================
	// Warp Tests
	// ============================================================

	[Fact]
	public void GetAllWarps_ReturnsWarps() {
		var warps = Chapter1Maps.GetAllWarps();
		Assert.NotEmpty(warps);
	}

	[Fact]
	public void GetAllWarps_ContainsStairsAndExits() {
		var warps = Chapter1Maps.GetAllWarps();
		Assert.Contains(warps, w => w.Type == WarpType.StairsUp);
		Assert.Contains(warps, w => w.Type == WarpType.StairsDown);
		Assert.Contains(warps, w => w.Type == WarpType.Exit);
	}

	[Fact]
	public void GetAllWarps_LochTowerHasProgression() {
		var warps = Chapter1Maps.GetAllWarps();
		// Should have warps from base to mid, mid to mid, mid to top
		var baseToMid = warps.Any(w =>
			w.SourceMapId == Chapter1Maps.MapLochTowerBase &&
			w.DestMapId == Chapter1Maps.MapLochTowerMid);
		var midToTop = warps.Any(w =>
			w.SourceMapId == Chapter1Maps.MapLochTowerMid &&
			w.DestMapId == Chapter1Maps.MapLochTowerTop);
		Assert.True(baseToMid);
		Assert.True(midToTop);
	}

	[Fact]
	public void GetAllWarps_HaveValidCoordinates() {
		var warps = Chapter1Maps.GetAllWarps();
		foreach (var w in warps) {
			// Indoor maps are max 64x64, overworld is larger
			var srcMaxCoord = w.SourceMapId == Chapter1Maps.MapChapter1Overworld ? 256 : 64;
			var dstMaxCoord = w.DestMapId == Chapter1Maps.MapChapter1Overworld ? 256 : 64;

			Assert.True(w.SourceX >= 0 && w.SourceX < srcMaxCoord, $"SourceX {w.SourceX} invalid for map {w.SourceMapId}");
			Assert.True(w.SourceY >= 0 && w.SourceY < srcMaxCoord, $"SourceY {w.SourceY} invalid for map {w.SourceMapId}");
			Assert.True(w.DestX >= 0 && w.DestX < dstMaxCoord, $"DestX {w.DestX} invalid for map {w.DestMapId}");
			Assert.True(w.DestY >= 0 && w.DestY < dstMaxCoord, $"DestY {w.DestY} invalid for map {w.DestMapId}");
		}
	}

	// ============================================================
	// NPC Tests
	// ============================================================

	[Fact]
	public void GetAllNpcs_ReturnsNpcs() {
		var npcs = Chapter1Maps.GetAllNpcs();
		Assert.NotEmpty(npcs);
	}

	[Fact]
	public void GetAllNpcs_ContainsKingOfBurland() {
		var npcs = Chapter1Maps.GetAllNpcs();
		var king = npcs.FirstOrDefault(n => n.Name == "King of Burland");
		Assert.NotNull(king);
		Assert.Equal(Chapter1Maps.MapBurlandMain, king.MapId);
	}

	[Fact]
	public void GetAllNpcs_ContainsHealie() {
		var npcs = Chapter1Maps.GetAllNpcs();
		var healie = npcs.FirstOrDefault(n => n.Name == "Healie");
		Assert.NotNull(healie);
		Assert.Equal(Chapter1Maps.MapWellDungeon, healie.MapId);
		Assert.True(healie.Flags.HasFlag(NpcFlags.Recruitable));
	}

	[Fact]
	public void GetAllNpcs_ContainsBoss() {
		var npcs = Chapter1Maps.GetAllNpcs();
		var boss = npcs.FirstOrDefault(n => n.Name == "Saro's Shadow");
		Assert.NotNull(boss);
		Assert.Equal(Chapter1Maps.MapLochTowerTop, boss.MapId);
		Assert.True(boss.Flags.HasFlag(NpcFlags.Boss));
	}

	[Fact]
	public void GetAllNpcs_ContainsServices() {
		var npcs = Chapter1Maps.GetAllNpcs();
		Assert.Contains(npcs, n => n.Flags.HasFlag(NpcFlags.Shop));
		Assert.Contains(npcs, n => n.Flags.HasFlag(NpcFlags.Inn));
		Assert.Contains(npcs, n => n.Flags.HasFlag(NpcFlags.Church));
	}

	[Fact]
	public void GetAllNpcs_HaveValidCoordinates() {
		var npcs = Chapter1Maps.GetAllNpcs();
		foreach (var n in npcs) {
			Assert.True(n.X >= 0 && n.X < 64);
			Assert.True(n.Y >= 0 && n.Y < 64);
		}
	}

	// ============================================================
	// Encounter Zone Tests
	// ============================================================

	[Fact]
	public void GetAllEncounterZones_ReturnsZones() {
		var zones = Chapter1Maps.GetAllEncounterZones();
		Assert.NotEmpty(zones);
	}

	[Fact]
	public void GetAllEncounterZones_OverworldHasEncounters() {
		var zones = Chapter1Maps.GetAllEncounterZones();
		var overworld = zones.FirstOrDefault(z => z.MapId == Chapter1Maps.MapChapter1Overworld);
		Assert.NotNull(overworld);
		Assert.True(overworld.EncounterRate > 0);
		Assert.NotEmpty(overworld.MonsterGroups);
	}

	[Fact]
	public void GetAllEncounterZones_LochTowerTopNoRandomEncounters() {
		var zones = Chapter1Maps.GetAllEncounterZones();
		var top = zones.FirstOrDefault(z => z.MapId == Chapter1Maps.MapLochTowerTop);
		Assert.NotNull(top);
		Assert.Equal(0, top.EncounterRate); // Boss floor, no random
	}

	[Fact]
	public void GetAllEncounterZones_DungeonsHaveHigherRates() {
		var zones = Chapter1Maps.GetAllEncounterZones();
		var overworld = zones.First(z => z.MapId == Chapter1Maps.MapChapter1Overworld);
		var well = zones.First(z => z.MapId == Chapter1Maps.MapWellDungeon);
		Assert.True(well.EncounterRate > overworld.EncounterRate);
	}

	// ============================================================
	// Entrance Tests
	// ============================================================

	[Fact]
	public void GetAllEntrances_ReturnsEntrances() {
		var entrances = Chapter1Maps.GetAllEntrances();
		Assert.NotEmpty(entrances);
	}

	[Fact]
	public void GetAllEntrances_ContainsMajorLocations() {
		var entrances = Chapter1Maps.GetAllEntrances();
		Assert.Contains(entrances, e => e.Name == "Burland");
		Assert.Contains(entrances, e => e.Name == "Loch Tower");
	}

	[Fact]
	public void GetAllEntrances_HaveValidOverworldCoordinates() {
		var entrances = Chapter1Maps.GetAllEntrances();
		foreach (var e in entrances) {
			Assert.True(e.X >= 0);
			Assert.True(e.Y >= 0);
		}
	}

	// ============================================================
	// DQ3r Map ID Mapping Tests
	// ============================================================

	[Fact]
	public void GetDQ3rMapIdMapping_ContainsAllChapter1Maps() {
		var mapping = Chapter1Maps.GetDQ3rMapIdMapping();
		Assert.True(mapping.ContainsKey(Chapter1Maps.MapBurlandMain));
		Assert.True(mapping.ContainsKey(Chapter1Maps.MapWellDungeon));
		Assert.True(mapping.ContainsKey(Chapter1Maps.MapLochTowerBase));
		Assert.True(mapping.ContainsKey(Chapter1Maps.MapLochTowerTop));
	}

	[Fact]
	public void GetDQ3rMapIdMapping_DQ3rIdsAreValid() {
		var mapping = Chapter1Maps.GetDQ3rMapIdMapping();
		foreach (var kvp in mapping) {
			// DQ3r map IDs should be 16-bit values with offset
			Assert.True(kvp.Value > 0x100);
		}
	}

	// ============================================================
	// Conversion Tests
	// ============================================================

	[Fact]
	public void ConvertTreasures_ProducesValidDQ3rTreasures() {
		var converted = Chapter1Maps.ConvertTreasures();
		Assert.NotEmpty(converted);
		Assert.All(converted, t => Assert.True(t.Id >= 0));
	}

	[Fact]
	public void ConvertWarps_ProducesValidDQ3rWarps() {
		var converted = Chapter1Maps.ConvertWarps();
		Assert.NotEmpty(converted);
	}

	[Fact]
	public void ConvertNpcs_ProducesValidDQ3rNpcs() {
		var converted = Chapter1Maps.ConvertNpcs();
		Assert.NotEmpty(converted);
		Assert.All(converted, n => Assert.True(n.Index >= 0));
	}

	[Fact]
	public void ConvertEncounterZones_ProducesValidDQ3rZones() {
		var converted = Chapter1Maps.ConvertEncounterZones();
		Assert.NotEmpty(converted);
	}

	// ============================================================
	// Integration Tests
	// ============================================================

	[Fact]
	public void Chapter1_HasCompleteMapNetwork() {
		var warps = Chapter1Maps.GetAllWarps();
		var maps = Chapter1Maps.GetAllMaps();

		// Every dungeon/tower should be reachable (has at least one incoming warp)
		var dungeonAndTowers = maps.Where(m => m.Type == MapType.Tower || m.Type == MapType.Cave).ToList();

		// For Chapter 1, Loch Tower base is reached from overworld (not in warp table)
		// Well Dungeon is reached from Strathbaile
		// So only check tower middle/top floors have warps

		// Loch Tower Mid should be reachable from Base
		var midReachable = warps.Any(w =>
			w.SourceMapId == Chapter1Maps.MapLochTowerBase &&
			w.DestMapId == Chapter1Maps.MapLochTowerMid);
		Assert.True(midReachable, "Loch Tower Mid should be reachable from Base");

		// Loch Tower Top should be reachable from Mid
		var topReachable = warps.Any(w =>
			w.SourceMapId == Chapter1Maps.MapLochTowerMid &&
			w.DestMapId == Chapter1Maps.MapLochTowerTop);
		Assert.True(topReachable, "Loch Tower Top should be reachable from Mid");

		// Well Dungeon should be reachable from Strathbaile
		var wellReachable = warps.Any(w =>
			w.SourceMapId == Chapter1Maps.MapStrathbaile &&
			w.DestMapId == Chapter1Maps.MapWellDungeon);
		Assert.True(wellReachable, "Well Dungeon should be reachable from Strathbaile");
	}

	[Fact]
	public void Chapter1_BossIsReachable() {
		// Verify the boss NPC is in a reachable location
		var npcs = Chapter1Maps.GetAllNpcs();
		var warps = Chapter1Maps.GetAllWarps();

		var boss = npcs.First(n => n.Flags.HasFlag(NpcFlags.Boss));
		// Boss should be on Loch Tower top floor
		Assert.Equal(Chapter1Maps.MapLochTowerTop, boss.MapId);

		// There should be a warp leading to the boss floor
		var canReachBoss = warps.Any(w => w.DestMapId == Chapter1Maps.MapLochTowerTop);
		Assert.True(canReachBoss);
	}

	[Fact]
	public void Chapter1_HealieIsReachable() {
		var npcs = Chapter1Maps.GetAllNpcs();
		var warps = Chapter1Maps.GetAllWarps();

		var healie = npcs.First(n => n.Name == "Healie");
		// Healie should be in well dungeon
		Assert.Equal(Chapter1Maps.MapWellDungeon, healie.MapId);

		// Well dungeon should be reachable from Strathbaile
		var canReachWell = warps.Any(w =>
			w.SourceMapId == Chapter1Maps.MapStrathbaile &&
			w.DestMapId == Chapter1Maps.MapWellDungeon);
		Assert.True(canReachWell);
	}

	[Fact]
	public void Chapter1_AllNpcsHaveDialog() {
		var npcs = Chapter1Maps.GetAllNpcs();
		Assert.All(npcs, n => Assert.True(n.DialogId > 0));
	}

	[Fact]
	public void Chapter1_ServiceNpcsAreInTowns() {
		var npcs = Chapter1Maps.GetAllNpcs();
		var maps = Chapter1Maps.GetAllMaps();

		var serviceNpcs = npcs.Where(n =>
			n.Flags.HasFlag(NpcFlags.Shop) ||
			n.Flags.HasFlag(NpcFlags.Inn) ||
			n.Flags.HasFlag(NpcFlags.Church));

		foreach (var npc in serviceNpcs) {
			var mapMeta = maps.FirstOrDefault(m => m.MapId == npc.MapId);
			Assert.NotNull(mapMeta);
			Assert.True(mapMeta.Type == MapType.Castle || mapMeta.Type == MapType.Town,
				$"Service NPC {npc.Name} should be in town/castle, not {mapMeta.Type}");
		}
	}

	// ============================================================
	// Map ID Constants Tests
	// ============================================================

	[Fact]
	public void MapIdConstants_AreValid() {
		Assert.True(Chapter1Maps.MapBurlandMain >= 0);
		Assert.True(Chapter1Maps.MapIzmit >= 0);
		Assert.True(Chapter1Maps.MapStrathbaile >= 0);
		Assert.True(Chapter1Maps.MapWellDungeon >= 0);
		Assert.True(Chapter1Maps.MapLochTowerBase >= 0);
		Assert.True(Chapter1Maps.MapLochTowerMid >= 0);
		Assert.True(Chapter1Maps.MapLochTowerTop >= 0);
	}

	[Fact]
	public void TilesetConstants_AreValid() {
		Assert.True(Chapter1Maps.TilesetCastle > 0);
		Assert.True(Chapter1Maps.TilesetTown > 0);
		Assert.True(Chapter1Maps.TilesetDungeon > 0);
		Assert.True(Chapter1Maps.TilesetTower > 0);
		Assert.True(Chapter1Maps.TilesetCave > 0);
	}
}
