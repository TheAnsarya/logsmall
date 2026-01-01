namespace DW4Lib.Converters;

using DW4Lib.DataStructures;
using DW4Lib.DataStructures.Chapter1;

/// <summary>
/// Converts DW4 Chapter data to DQ3 Remake format.
/// Handles chapter structure, events, NPCs, and dialog for DQ3r compatibility.
/// </summary>
public class ChapterConverter {
	/// <summary>
	/// DQ3r doesn't have a chapter system - all content is unified.
	/// This converts DW4 chapters to DQ3r "quest" or "scenario" format.
	/// </summary>
	public static Dq3rScenario ConvertChapter(Chapter chapter) {
		return new Dq3rScenario {
			ScenarioId = MapChapterToScenarioId(chapter.Id),
			Name = chapter.Name,
			Description = chapter.Description,
			StartingMapId = ConvertMapId(chapter.StartLocationId),
			RequiredFlags = ConvertEventFlags(chapter.StartFlags),
			CompletionFlags = ConvertEventFlags(chapter.EndFlags),
			IsMainStory = true
		};
	}

	/// <summary>
	/// Convert Chapter 1 specific data to DQ3r format.
	/// </summary>
	public static Dq3rChapter1Data ConvertChapter1() {
		var result = new Dq3rChapter1Data();

		// Convert Ragnar to DQ3r character format
		result.ProtagonistData = ConvertRagnarToHero();

		// Convert Healie companion
		result.CompanionData = ConvertHealieToNpc();

		// Convert maps
		foreach (var map in Chapter1Data.Maps) {
			result.Maps.Add(ConvertMap(map));
		}

		// Convert events to quest steps
		foreach (var evt in Chapter1Data.Events) {
			result.QuestSteps.Add(ConvertEvent(evt));
		}

		// Convert treasures
		foreach (var treasure in Chapter1Data.Treasures) {
			result.Treasures.Add(ConvertTreasure(treasure));
		}

		// Convert shops
		foreach (var shop in Chapter1Data.Shops) {
			result.Shops.Add(ConvertShop(shop));
		}

		// Convert encounters
		foreach (var zone in Chapter1Data.EncounterZones) {
			result.EncounterZones.Add(ConvertEncounterZone(zone));
		}

		// Convert dialog
		foreach (var dialog in Chapter1Dialog.AllDialog) {
			result.Dialog.Add(ConvertDialog(dialog));
		}

		// Convert NPCs
		foreach (var npc in Chapter1NPCs.GetAllNPCs()) {
			result.NPCs.Add(ConvertNpc(npc));
		}

		return result;
	}

	/// <summary>
	/// Convert Ragnar to DQ3r hero format.
	/// DQ3r uses a different stat scaling.
	/// </summary>
	private static Dq3rCharacterData ConvertRagnarToHero() {
		var stats = Chapter1Data.StartingStats;
		return new Dq3rCharacterData {
			CharacterId = 0x01, // DQ3r hero slot
			Name = "Ragnar",
			Class = Dq3rClass.Warrior, // Ragnar is a soldier/warrior
			Level = stats.Level,
			HP = ScaleHpToDq3r(stats.HP),
			MP = stats.MP, // Ragnar has no MP in Chapter 1
			Strength = ScaleStatToDq3r(stats.Strength),
			Agility = ScaleStatToDq3r(stats.Agility),
			Vitality = ScaleStatToDq3r(stats.Vitality),
			Intelligence = ScaleStatToDq3r(stats.Intelligence),
			Luck = ScaleStatToDq3r(stats.Luck),
			Equipment = [
				ItemIdConverter.ConvertWeaponId(stats.Weapon),
				ItemIdConverter.ConvertArmorId(stats.Armor),
				ItemIdConverter.ConvertShieldId(stats.Shield),
				ItemIdConverter.ConvertHelmetId(stats.Helmet)
			]
		};
	}

	/// <summary>
	/// Convert Healie to DQ3r NPC companion format.
	/// In DQ3r, Healie would be a healing slime NPC that follows the party.
	/// </summary>
	private static Dq3rNpcData ConvertHealieToNpc() {
		var healie = Chapter1NPCs.Healie;
		return new Dq3rNpcData {
			NpcId = 0xE0, // DQ3r companion NPC slot
			Name = "Healie",
			SpriteId = ConvertSpriteId(healie.SpriteId),
			IsCompanion = true,
			CompanionFlags = Dq3rCompanionFlags.CanHeal | Dq3rCompanionFlags.FollowsParty,
			HealSpellPower = 30 // Healie's heal effectiveness
		};
	}

	/// <summary>
	/// Convert DW4 map to DQ3r format.
	/// </summary>
	private static Dq3rMapData ConvertMap(Chapter1Map map) {
		return new Dq3rMapData {
			MapId = ConvertMapId(map.Id),
			Name = map.Name,
			MapType = ConvertMapType(map.Type),
			HasWeaponShop = map.HasShop,
			HasArmorShop = map.HasShop,
			HasItemShop = map.HasShop,
			HasInn = map.HasInn,
			HasChurch = map.HasChurch,
			WorldMapX = ScaleCoordinate(map.OverworldX),
			WorldMapY = ScaleCoordinate(map.OverworldY)
		};
	}

	/// <summary>
	/// Convert DW4 event to DQ3r quest step.
	/// </summary>
	private static Dq3rQuestStep ConvertEvent(Chapter1Event evt) {
		return new Dq3rQuestStep {
			StepId = evt.Id,
			Name = evt.Name,
			Description = evt.Description,
			MapId = ConvertMapId(evt.MapId),
			TriggerType = ConvertTriggerType(evt.TriggerType),
			TriggerX = evt.TriggerX >= 0 ? evt.TriggerX : null,
			TriggerY = evt.TriggerY >= 0 ? evt.TriggerY : null,
			RequiredFlag = evt.RequiredFlag >= 0 ? ConvertFlagId(evt.RequiredFlag) : null,
			CompletionFlag = evt.SetFlag >= 0 ? ConvertFlagId(evt.SetFlag) : null,
			DialogId = ConvertDialogId(evt.DialogId),
			BossEncounterId = evt.BossId >= 0 ? ConvertMonsterId(evt.BossId) : null,
			IsChapterEnd = evt.IsChapterEnd
		};
	}

	/// <summary>
	/// Convert DW4 treasure to DQ3r format.
	/// </summary>
	private static Dq3rTreasureData ConvertTreasure(Chapter1Treasure treasure) {
		return new Dq3rTreasureData {
			ChestId = treasure.Id,
			MapId = ConvertMapId(treasure.MapId),
			X = treasure.X,
			Y = treasure.Y,
			ContentsType = ConvertTreasureType(treasure.ContentsType),
			Contents = ConvertTreasureContents(treasure)
		};
	}

	/// <summary>
	/// Convert DW4 shop to DQ3r format.
	/// </summary>
	private static Dq3rShopData ConvertShop(Chapter1Shop shop) {
		var items = new List<int>();
		for (int i = 0; i < shop.Items.Length; i++) {
			int convertedId = shop.ShopType switch {
				ShopType.Weapon => ItemIdConverter.ConvertWeaponId(shop.Items[i]),
				ShopType.Armor => ItemIdConverter.ConvertArmorId(shop.Items[i]),
				ShopType.Item => ItemIdConverter.ConvertItemId(shop.Items[i]),
				_ => shop.Items[i]
			};
			items.Add(convertedId);
		}

		return new Dq3rShopData {
			MapId = ConvertMapId(shop.MapId),
			ShopType = ConvertShopType(shop.ShopType),
			Items = [.. items],
			Prices = ScalePrices(shop.Prices)
		};
	}

	/// <summary>
	/// Convert DW4 encounter zone to DQ3r format.
	/// </summary>
	private static Dq3rEncounterZone ConvertEncounterZone(Chapter1EncounterZone zone) {
		var monsterGroups = zone.MonsterGroups
			.Select(MonsterIdConverter.ConvertMonsterId)
			.ToArray();

		return new Dq3rEncounterZone {
			ZoneId = zone.ZoneId,
			Description = zone.Description,
			MonsterGroupIds = monsterGroups,
			EncounterRate = ScaleEncounterRate(zone.EncounterRate)
		};
	}

	/// <summary>
	/// Convert DW4 dialog to DQ3r format.
	/// </summary>
	private static Dq3rDialogData ConvertDialog(DialogEntry dialog) {
		return new Dq3rDialogData {
			DialogId = ConvertDialogId(dialog.Id),
			Speaker = dialog.Speaker,
			Lines = ConvertDialogLines(dialog.Lines)
		};
	}

	/// <summary>
	/// Convert DW4 NPC to DQ3r format.
	/// </summary>
	private static Dq3rNpcData ConvertNpc(NpcDefinition npc) {
		return new Dq3rNpcData {
			NpcId = npc.Id,
			Name = npc.Name,
			SpriteId = ConvertSpriteId(npc.SpriteId),
			MapId = ConvertMapId(npc.MapId),
			X = npc.X,
			Y = npc.Y,
			Facing = (int)npc.Facing,
			IsStationary = npc.IsStationary,
			IsBoss = npc.IsBoss,
			AppearFlag = npc.AppearFlag >= 0 ? ConvertFlagId(npc.AppearFlag) : null,
			DisappearFlag = npc.DisappearFlag >= 0 ? ConvertFlagId(npc.DisappearFlag) : null
		};
	}

	#region ID Conversion Methods

	/// <summary>
	/// Map DW4 chapter ID to DQ3r scenario ID.
	/// DQ3r scenario IDs start at 0x100 for custom content.
	/// </summary>
	private static int MapChapterToScenarioId(byte chapterId) {
		return 0x100 + chapterId;
	}

	/// <summary>
	/// Convert DW4 map ID to DQ3r map ID.
	/// DQ3r uses a different map numbering scheme.
	/// </summary>
	private static int ConvertMapId(int dw4MapId) {
		// DW4 maps 0x00-0x7F map to DQ3r 0x200+
		return 0x200 + dw4MapId;
	}

	/// <summary>
	/// Convert DW4 event flag to DQ3r flag ID.
	/// </summary>
	private static int ConvertFlagId(int dw4Flag) {
		// DW4 flags map to DQ3r custom flag range
		return 0x400 + dw4Flag;
	}

	/// <summary>
	/// Convert DW4 dialog ID to DQ3r dialog ID.
	/// </summary>
	private static int ConvertDialogId(int dw4DialogId) {
		return 0x1000 + dw4DialogId;
	}

	/// <summary>
	/// Convert DW4 monster ID to DQ3r monster ID.
	/// </summary>
	private static int ConvertMonsterId(int dw4MonsterId) {
		return MonsterIdConverter.ConvertMonsterId((byte)dw4MonsterId);
	}

	/// <summary>
	/// Convert DW4 sprite ID to DQ3r sprite ID.
	/// </summary>
	private static int ConvertSpriteId(int dw4SpriteId) {
		// Sprite mapping - DQ3r uses 4bpp sprites
		return 0x80 + dw4SpriteId;
	}

	/// <summary>
	/// Convert DW4 event flags array to DQ3r format.
	/// </summary>
	private static int[] ConvertEventFlags(int[] dw4Flags) {
		return dw4Flags.Select(ConvertFlagId).ToArray();
	}

	#endregion

	#region Type Conversion Methods

	/// <summary>
	/// Convert map location type.
	/// </summary>
	private static Dq3rMapType ConvertMapType(MapLocationType type) {
		return type switch {
			MapLocationType.Castle => Dq3rMapType.Castle,
			MapLocationType.Town => Dq3rMapType.Town,
			MapLocationType.Cave => Dq3rMapType.Cave,
			MapLocationType.Dungeon => Dq3rMapType.Dungeon,
			MapLocationType.Tower => Dq3rMapType.Tower,
			MapLocationType.Shrine => Dq3rMapType.Shrine,
			MapLocationType.Overworld => Dq3rMapType.Overworld,
			_ => Dq3rMapType.Other
		};
	}

	/// <summary>
	/// Convert event trigger type.
	/// </summary>
	private static Dq3rTriggerType ConvertTriggerType(EventTrigger trigger) {
		return trigger switch {
			EventTrigger.ChapterStart => Dq3rTriggerType.ScenarioStart,
			EventTrigger.EnterMap => Dq3rTriggerType.MapEntry,
			EventTrigger.EnterTile => Dq3rTriggerType.TileStep,
			EventTrigger.TalkToNPC => Dq3rTriggerType.NpcInteraction,
			EventTrigger.BossDefeated => Dq3rTriggerType.BattleVictory,
			EventTrigger.ItemUsed => Dq3rTriggerType.ItemUse,
			EventTrigger.ChapterEnd => Dq3rTriggerType.ScenarioEnd,
			_ => Dq3rTriggerType.None
		};
	}

	/// <summary>
	/// Convert treasure type.
	/// </summary>
	private static Dq3rTreasureType ConvertTreasureType(TreasureContents type) {
		return type switch {
			TreasureContents.Item => Dq3rTreasureType.Item,
			TreasureContents.Gold => Dq3rTreasureType.Gold,
			TreasureContents.SmallMedal => Dq3rTreasureType.MiniMedal,
			TreasureContents.Empty => Dq3rTreasureType.Empty,
			TreasureContents.Monster => Dq3rTreasureType.Monster,
			_ => Dq3rTreasureType.Empty
		};
	}

	/// <summary>
	/// Convert treasure contents value.
	/// </summary>
	private static int ConvertTreasureContents(Chapter1Treasure treasure) {
		return treasure.ContentsType switch {
			TreasureContents.Item => ItemIdConverter.ConvertItemId((byte)treasure.Value),
			TreasureContents.Gold => ScaleGold(treasure.Value),
			TreasureContents.SmallMedal => treasure.Value,
			TreasureContents.Monster => ConvertMonsterId(treasure.Value),
			_ => 0
		};
	}

	/// <summary>
	/// Convert shop type.
	/// </summary>
	private static Dq3rShopType ConvertShopType(ShopType type) {
		return type switch {
			ShopType.Weapon => Dq3rShopType.Weapon,
			ShopType.Armor => Dq3rShopType.Armor,
			ShopType.Item => Dq3rShopType.Item,
			ShopType.Inn => Dq3rShopType.Inn,
			ShopType.Church => Dq3rShopType.Church,
			_ => Dq3rShopType.Item
		};
	}

	#endregion

	#region Scaling Methods

	/// <summary>
	/// Scale HP from DW4 to DQ3r (DQ3r has ~1.5x HP values).
	/// </summary>
	private static int ScaleHpToDq3r(int dw4Hp) {
		return (int)(dw4Hp * 1.5);
	}

	/// <summary>
	/// Scale stat from DW4 to DQ3r.
	/// </summary>
	private static int ScaleStatToDq3r(int dw4Stat) {
		// DQ3r stats are roughly similar, minor adjustment
		return (int)(dw4Stat * 1.2);
	}

	/// <summary>
	/// Scale coordinate from DW4 to DQ3r world map.
	/// </summary>
	private static int ScaleCoordinate(byte dw4Coord) {
		// DQ3r has a larger world map
		return dw4Coord * 2;
	}

	/// <summary>
	/// Scale gold amount (inflation adjustment).
	/// </summary>
	private static int ScaleGold(int dw4Gold) {
		return (int)(dw4Gold * 1.5);
	}

	/// <summary>
	/// Scale shop prices.
	/// </summary>
	private static int[] ScalePrices(int[] dw4Prices) {
		return dw4Prices.Select(p => (int)(p * 1.5)).ToArray();
	}

	/// <summary>
	/// Scale encounter rate (DQ3r has different rate system).
	/// </summary>
	private static int ScaleEncounterRate(byte dw4Rate) {
		// DQ3r encounter rate is 0-255, DW4 is 0-31
		return Math.Min(255, dw4Rate * 8);
	}

	/// <summary>
	/// Convert dialog lines for DQ3r text system.
	/// </summary>
	private static string[] ConvertDialogLines(string[] lines) {
		// DQ3r uses different control codes
		return lines.Select(line =>
			line.Replace("*", "[") // Convert DW4 emphasis to DQ3r
			    .Replace("!", "]")
		).ToArray();
	}

	#endregion
}

#region DQ3r Data Structures

/// <summary>
/// DQ3r scenario (chapter equivalent).
/// </summary>
public class Dq3rScenario {
	public int ScenarioId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int StartingMapId { get; set; }
	public int[]? RequiredFlags { get; set; }
	public int[]? CompletionFlags { get; set; }
	public bool IsMainStory { get; set; }
}

/// <summary>
/// DQ3r Chapter 1 converted data container.
/// </summary>
public class Dq3rChapter1Data {
	public Dq3rCharacterData? ProtagonistData { get; set; }
	public Dq3rNpcData? CompanionData { get; set; }
	public List<Dq3rMapData> Maps { get; set; } = [];
	public List<Dq3rQuestStep> QuestSteps { get; set; } = [];
	public List<Dq3rTreasureData> Treasures { get; set; } = [];
	public List<Dq3rShopData> Shops { get; set; } = [];
	public List<Dq3rEncounterZone> EncounterZones { get; set; } = [];
	public List<Dq3rDialogData> Dialog { get; set; } = [];
	public List<Dq3rNpcData> NPCs { get; set; } = [];
}

/// <summary>
/// DQ3r character data.
/// </summary>
public class Dq3rCharacterData {
	public int CharacterId { get; set; }
	public string Name { get; set; } = string.Empty;
	public Dq3rClass Class { get; set; }
	public int Level { get; set; }
	public int HP { get; set; }
	public int MP { get; set; }
	public int Strength { get; set; }
	public int Agility { get; set; }
	public int Vitality { get; set; }
	public int Intelligence { get; set; }
	public int Luck { get; set; }
	public int[] Equipment { get; set; } = [];
}

/// <summary>
/// DQ3r classes.
/// </summary>
public enum Dq3rClass {
	Hero = 0,
	Warrior = 1,
	Mage = 2,
	Priest = 3,
	MartialArtist = 4,
	Merchant = 5,
	Gadabout = 6,
	Thief = 7,
	Sage = 8
}

/// <summary>
/// DQ3r NPC data.
/// </summary>
public class Dq3rNpcData {
	public int NpcId { get; set; }
	public string Name { get; set; } = string.Empty;
	public int SpriteId { get; set; }
	public int? MapId { get; set; }
	public byte X { get; set; }
	public byte Y { get; set; }
	public int Facing { get; set; }
	public bool IsStationary { get; set; }
	public bool IsBoss { get; set; }
	public bool IsCompanion { get; set; }
	public Dq3rCompanionFlags CompanionFlags { get; set; }
	public int HealSpellPower { get; set; }
	public int? AppearFlag { get; set; }
	public int? DisappearFlag { get; set; }
}

/// <summary>
/// DQ3r companion flags.
/// </summary>
[Flags]
public enum Dq3rCompanionFlags {
	None = 0,
	FollowsParty = 1,
	CanHeal = 2,
	CanFight = 4,
	CanCast = 8
}

/// <summary>
/// DQ3r map data.
/// </summary>
public class Dq3rMapData {
	public int MapId { get; set; }
	public string Name { get; set; } = string.Empty;
	public Dq3rMapType MapType { get; set; }
	public bool HasWeaponShop { get; set; }
	public bool HasArmorShop { get; set; }
	public bool HasItemShop { get; set; }
	public bool HasInn { get; set; }
	public bool HasChurch { get; set; }
	public int WorldMapX { get; set; }
	public int WorldMapY { get; set; }
}

/// <summary>
/// DQ3r map types.
/// </summary>
public enum Dq3rMapType {
	Overworld,
	Town,
	Castle,
	Cave,
	Dungeon,
	Tower,
	Shrine,
	Other
}

/// <summary>
/// DQ3r quest step.
/// </summary>
public class Dq3rQuestStep {
	public int StepId { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int MapId { get; set; }
	public Dq3rTriggerType TriggerType { get; set; }
	public int? TriggerX { get; set; }
	public int? TriggerY { get; set; }
	public int? RequiredFlag { get; set; }
	public int? CompletionFlag { get; set; }
	public int DialogId { get; set; }
	public int? BossEncounterId { get; set; }
	public bool IsChapterEnd { get; set; }
}

/// <summary>
/// DQ3r trigger types.
/// </summary>
public enum Dq3rTriggerType {
	None,
	ScenarioStart,
	MapEntry,
	TileStep,
	NpcInteraction,
	BattleVictory,
	ItemUse,
	ScenarioEnd
}

/// <summary>
/// DQ3r treasure data.
/// </summary>
public class Dq3rTreasureData {
	public int ChestId { get; set; }
	public int MapId { get; set; }
	public byte X { get; set; }
	public byte Y { get; set; }
	public Dq3rTreasureType ContentsType { get; set; }
	public int Contents { get; set; }
}

/// <summary>
/// DQ3r treasure types.
/// </summary>
public enum Dq3rTreasureType {
	Empty,
	Item,
	Gold,
	MiniMedal,
	Monster
}

/// <summary>
/// DQ3r shop data.
/// </summary>
public class Dq3rShopData {
	public int MapId { get; set; }
	public Dq3rShopType ShopType { get; set; }
	public int[] Items { get; set; } = [];
	public int[] Prices { get; set; } = [];
}

/// <summary>
/// DQ3r shop types.
/// </summary>
public enum Dq3rShopType {
	Weapon,
	Armor,
	Item,
	Inn,
	Church
}

/// <summary>
/// DQ3r encounter zone.
/// </summary>
public class Dq3rEncounterZone {
	public int ZoneId { get; set; }
	public string Description { get; set; } = string.Empty;
	public int[] MonsterGroupIds { get; set; } = [];
	public int EncounterRate { get; set; }
}

/// <summary>
/// DQ3r dialog data.
/// </summary>
public class Dq3rDialogData {
	public int DialogId { get; set; }
	public string Speaker { get; set; } = string.Empty;
	public string[] Lines { get; set; } = [];
}

#endregion
