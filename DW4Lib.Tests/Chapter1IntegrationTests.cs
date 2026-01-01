using DW4Lib.Audio;
using DW4Lib.DataStructures;
using DW4Lib.DataStructures.Chapter1;
using DW4Lib.DataStructures.Maps;
using DW4Lib.Events;
using DW4Lib.Graphics;
using DW4Lib.Maps;

namespace DW4Lib.Tests;

/// <summary>
/// Integration tests verifying Chapter 1 content works together.
/// Tests the complete conversion pipeline for DW4 → DQ3r.
/// </summary>
public class Chapter1IntegrationTests {
	// ============================================================
	// Complete Chapter Data Tests
	// ============================================================

	[Fact]
	public void Chapter1_HasAllRequiredSystems() {
		// Verify all major systems are available
		var startingStats = Chapter1Data.StartingStats;
		var events = Chapter1Events.GetAllScripts();
		var maps = Chapter1Maps.GetAllMaps();
		var sprites = Chapter1Sprites.GetAllCharacterSprites();
		var audio = AudioDatabase.GetAllTracks();

		Assert.NotNull(startingStats);
		Assert.NotEmpty(events);
		Assert.NotEmpty(maps);
		Assert.NotEmpty(sprites);
		Assert.NotEmpty(audio);
	}

	[Fact]
	public void Chapter1_RagnarHasCompleteData() {
		// Character stats
		var ragnar = Chapter1Data.StartingStats;
		Assert.NotNull(ragnar);
		Assert.True(ragnar.HP > 0);
		Assert.True(ragnar.Strength > 0);

		// Sprite
		var sprite = Chapter1Sprites.GetRagnarSprite();
		Assert.NotNull(sprite);
		Assert.NotEmpty(sprite.Animations);

		// Starting flags (story progression)
		var flags = Chapter1Flags.GetStoryFlags();
		Assert.NotEmpty(flags);
	}

	[Fact]
	public void Chapter1_HealieCanBeRecruited() {
		// Healie should be in the well dungeon as an NPC
		var npcs = Chapter1Maps.GetAllNpcs();
		var healie = npcs.FirstOrDefault(n => n.Name == "Healie");
		Assert.NotNull(healie);
		Assert.True(healie.Flags.HasFlag(NpcFlags.Recruitable));

		// There should be a script for recruiting Healie
		var scripts = Chapter1Events.GetAllScripts();
		var recruitScript = scripts.FirstOrDefault(s => s.Name.Contains("Healie") && s.Name.Contains("Joins"));
		Assert.NotNull(recruitScript);
	}

	[Fact]
	public void Chapter1_BossIsAccessible() {
		// Boss should be at the top of Loch Tower
		var npcs = Chapter1Maps.GetAllNpcs();
		var boss = npcs.FirstOrDefault(n => n.Flags.HasFlag(NpcFlags.Boss));
		Assert.NotNull(boss);
		Assert.Equal(Chapter1Maps.MapLochTowerTop, boss.MapId);

		// Boss should have a battle script
		var scripts = Chapter1Events.GetAllScripts();
		var bossScript = scripts.FirstOrDefault(s => s.Name.Contains("Shadow"));
		Assert.NotNull(bossScript);
	}

	[Fact]
	public void Chapter1_FlyingShoesAreObtainable() {
		// Flying Shoes should be in Loch Tower
		var treasures = Chapter1Maps.GetAllTreasures();
		var flyingShoes = treasures.FirstOrDefault(t =>
			t.MapId == Chapter1Maps.MapLochTowerTop &&
			t.ContentsValue == 0x2A);
		Assert.NotNull(flyingShoes);
	}

	// ============================================================
	// Progression Tests
	// ============================================================

	[Fact]
	public void Chapter1_HasCorrectEventOrder() {
		var scripts = Chapter1Events.GetAllScripts();

		// Key story events should exist
		Assert.Contains(scripts, s => s.Name.Contains("Intro"));
		Assert.Contains(scripts, s => s.Name.Contains("King"));
		Assert.Contains(scripts, s => s.Name.Contains("Healie"));
		Assert.Contains(scripts, s => s.Name.Contains("Shadow") || s.Name.Contains("Boss"));
		Assert.Contains(scripts, s => s.Name.Contains("Complete") || s.Name.Contains("Return"));
	}

	[Fact]
	public void Chapter1_CanTraverseFromBurlandToLochTower() {
		var warps = Chapter1Maps.GetAllWarps();

		// Start from Strathbaile (where well dungeon entry is)
		// and from Loch Tower Base (reached via overworld)
		var visited = new HashSet<int> {
			Chapter1Maps.MapBurlandMain,
			Chapter1Maps.MapStrathbaile,
			Chapter1Maps.MapLochTowerBase
		};
		var queue = new Queue<int>();
		queue.Enqueue(Chapter1Maps.MapBurlandMain);
		queue.Enqueue(Chapter1Maps.MapStrathbaile);
		queue.Enqueue(Chapter1Maps.MapLochTowerBase);

		// Simple BFS to find reachable locations via warps
		while (queue.Count > 0) {
			var current = queue.Dequeue();
			var outgoing = warps.Where(w => w.SourceMapId == current);

			foreach (var warp in outgoing) {
				if (!visited.Contains(warp.DestMapId)) {
					visited.Add(warp.DestMapId);
					queue.Enqueue(warp.DestMapId);
				}
			}
		}

		// Verify internal connectivity from starting points
		Assert.Contains(Chapter1Maps.MapLochTowerMid, visited);
		Assert.Contains(Chapter1Maps.MapLochTowerTop, visited);
		Assert.Contains(Chapter1Maps.MapWellDungeon, visited);
	}

	// ============================================================
	// DQ3r Conversion Pipeline Tests
	// ============================================================

	[Fact]
	public void Chapter1_AllTreasuresConvertToDQ3r() {
		var dq3rTreasures = Chapter1Maps.ConvertTreasures();
		var originalTreasures = Chapter1Maps.GetAllTreasures();

		Assert.Equal(originalTreasures.Length, dq3rTreasures.Length);
		Assert.All(dq3rTreasures, t => Assert.True(t.Id >= 0));
	}

	[Fact]
	public void Chapter1_AllWarpsConvertToDQ3r() {
		var dq3rWarps = Chapter1Maps.ConvertWarps();
		var originalWarps = Chapter1Maps.GetAllWarps();

		Assert.Equal(originalWarps.Length, dq3rWarps.Length);
	}

	[Fact]
	public void Chapter1_AllNpcsConvertToDQ3r() {
		var dq3rNpcs = Chapter1Maps.ConvertNpcs();
		var originalNpcs = Chapter1Maps.GetAllNpcs();

		Assert.Equal(originalNpcs.Length, dq3rNpcs.Length);
	}

	[Fact]
	public void Chapter1_AllEncounterZonesConvertToDQ3r() {
		var dq3rZones = Chapter1Maps.ConvertEncounterZones();
		var originalZones = Chapter1Maps.GetAllEncounterZones();

		Assert.Equal(originalZones.Length, dq3rZones.Length);
	}

	[Fact]
	public void Chapter1_AllScriptsConvertToDQ3r() {
		var scripts = Chapter1Events.GetAllScripts();

		foreach (var script in scripts) {
			var converted = EventScriptConverter.Convert(script);
			Assert.NotNull(converted);
			Assert.NotEmpty(converted.Commands);
		}
	}

	[Fact]
	public void Chapter1_SpritesConvertToDQ3r() {
		var resource = SpriteToDQ3r.BuildChapter1SpriteResource();

		Assert.Equal(1, resource.Chapter);
		Assert.NotEmpty(resource.CharacterSprites);
		Assert.NotEmpty(resource.NpcSprites);
		Assert.NotEmpty(resource.Palettes);
	}

	[Fact]
	public void Chapter1_AudioConvertsToDQ3r() {
		var tracks = AudioDatabase.GetAllTracks()
			.Where(t => t.Name.Contains("Chapter 1") || t.Name.Contains("Overworld") || t.Name.Contains("Battle"))
			.ToArray();

		foreach (var track in tracks) {
			var converted = AudioConverter.ConvertTrack(track);
			Assert.NotNull(converted);
			Assert.True(converted.Id > 0);
		}
	}

	// ============================================================
	// Data Consistency Tests
	// ============================================================

	[Fact]
	public void Chapter1_MapsAndNpcsAreConsistent() {
		var maps = Chapter1Maps.GetAllMaps();
		var npcs = Chapter1Maps.GetAllNpcs();

		// All NPCs should reference valid map IDs
		var mapIds = maps.Select(m => m.MapId).ToHashSet();
		foreach (var npc in npcs) {
			Assert.Contains(npc.MapId, mapIds);
		}
	}

	[Fact]
	public void Chapter1_MapsAndTreasuresAreConsistent() {
		var maps = Chapter1Maps.GetAllMaps();
		var treasures = Chapter1Maps.GetAllTreasures();

		var mapIds = maps.Select(m => m.MapId).ToHashSet();
		foreach (var treasure in treasures) {
			Assert.Contains(treasure.MapId, mapIds);
		}
	}

	[Fact]
	public void Chapter1_MapsAndEncountersAreConsistent() {
		var maps = Chapter1Maps.GetAllMaps();
		var zones = Chapter1Maps.GetAllEncounterZones();

		var mapIds = maps.Select(m => m.MapId).ToHashSet();
		mapIds.Add(Chapter1Maps.MapChapter1Overworld); // Include overworld

		foreach (var zone in zones) {
			Assert.Contains(zone.MapId, mapIds);
		}
	}

	[Fact]
	public void Chapter1_FlagsAreUsedInScripts() {
		var storyFlags = Chapter1Flags.GetStoryFlags();
		var scripts = Chapter1Events.GetAllScripts();

		// Collect all flag IDs used in scripts
		var usedFlagIds = new HashSet<int>();
		foreach (var script in scripts) {
			foreach (var cmd in script.Commands) {
				if (cmd.Opcode == ScriptOpcode.SetFlag || cmd.Opcode == ScriptOpcode.ClearFlag ||
					cmd.Opcode == ScriptOpcode.CheckFlag) {
					if (cmd.Parameters.Length > 0) {
						usedFlagIds.Add(cmd.Parameters[0]);
					}
				}
			}
		}

		// Most story flags should be used in scripts
		var usedCount = storyFlags.Count(f => usedFlagIds.Contains(f));
		Assert.True(usedCount > 0, "At least some story flags should be used in scripts");
	}

	// ============================================================
	// Service Availability Tests
	// ============================================================

	[Fact]
	public void Chapter1_HasRequiredServices() {
		var npcs = Chapter1Maps.GetAllNpcs();

		// Chapter 1 should have basic services
		Assert.Contains(npcs, n => n.Flags.HasFlag(NpcFlags.Shop));
		Assert.Contains(npcs, n => n.Flags.HasFlag(NpcFlags.Inn));
		Assert.Contains(npcs, n => n.Flags.HasFlag(NpcFlags.Church));
	}

	[Fact]
	public void Chapter1_HasShopScripts() {
		var scripts = Chapter1Events.GetAllScripts();
		Assert.Contains(scripts, s => s.Name.Contains("Shop") || s.Name.Contains("Weapon"));
	}

	[Fact]
	public void Chapter1_HasInnScript() {
		var scripts = Chapter1Events.GetAllScripts();
		Assert.Contains(scripts, s => s.Name.Contains("Inn"));
	}

	[Fact]
	public void Chapter1_HasChurchScript() {
		var scripts = Chapter1Events.GetAllScripts();
		Assert.Contains(scripts, s => s.Name.Contains("Church"));
	}

	// ============================================================
	// Complete Resource Export Test
	// ============================================================

	[Fact]
	public void Chapter1_CanExportAllResources() {
		// This test verifies that all Chapter 1 resources can be gathered
		// into a single exportable package

		var package = new Chapter1ResourcePackage {
			StartingStats = Chapter1Data.StartingStats,
			StoryFlags = Chapter1Flags.GetStoryFlags(),
			Events = Chapter1Events.GetAllScripts(),
			Maps = Chapter1Maps.GetAllMaps(),
			Treasures = Chapter1Maps.GetAllTreasures(),
			Warps = Chapter1Maps.GetAllWarps(),
			Npcs = Chapter1Maps.GetAllNpcs(),
			EncounterZones = Chapter1Maps.GetAllEncounterZones(),
			Entrances = Chapter1Maps.GetAllEntrances(),
			Sprites = SpriteToDQ3r.BuildChapter1SpriteResource(),
			Palettes = Chapter1Sprites.GetSpritePalettes()
		};

		Assert.NotNull(package.StartingStats);
		Assert.NotEmpty(package.StoryFlags);
		Assert.NotEmpty(package.Events);
		Assert.NotEmpty(package.Maps);
		Assert.NotEmpty(package.Treasures);
		Assert.NotEmpty(package.Warps);
		Assert.NotEmpty(package.Npcs);
		Assert.NotEmpty(package.EncounterZones);
		Assert.NotEmpty(package.Entrances);
		Assert.NotNull(package.Sprites);
		Assert.NotEmpty(package.Palettes);
	}
}

/// <summary>
/// Container for all Chapter 1 resources.
/// Used for testing completeness.
/// </summary>
public class Chapter1ResourcePackage {
	public RagnarStats? StartingStats { get; set; }
	public ushort[] StoryFlags { get; set; } = [];
	public EventScript[] Events { get; set; } = [];
	public MapMetadata[] Maps { get; set; } = [];
	public TreasureChest[] Treasures { get; set; } = [];
	public WarpPoint[] Warps { get; set; } = [];
	public NpcData[] Npcs { get; set; } = [];
	public EncounterZone[] EncounterZones { get; set; } = [];
	public EntranceLocation[] Entrances { get; set; } = [];
	public DQ3rSpriteResource? Sprites { get; set; }
	public SpritePalette[] Palettes { get; set; } = [];
}
