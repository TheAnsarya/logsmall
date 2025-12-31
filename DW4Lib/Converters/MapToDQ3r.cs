using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;

namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 NES map data to DQ3r SNES format.
/// </summary>
public static class MapToDQ3r {
	/// <summary>
	/// DW4 NES tile to DQ3r SNES tile translation table.
	/// 80 entries mapping DW4 overworld tiles to DQ3r tiles.
	/// From OverworldMap2.cs research.
	/// </summary>
	public static readonly byte[] OverworldTileTranslation = [
		0x4b, 0x4b, 0x4b, 0x4b, 0x02, 0x4b, 0x4b, 0x4b,  // $00-$07
		0x4b, 0x4b, 0x4b, 0x4a, 0x4b, 0xc9, 0x64, 0x4b,  // $08-$0f
		0x4b, 0x4b, 0x4b, 0x1c, 0x1d, 0x4b, 0xdd, 0x23,  // $10-$17
		0x24, 0x0d, 0x4b, 0x4b, 0xc9, 0x4b, 0x52, 0x61,  // $18-$1f
		0x15, 0xff, 0x59, 0xdd, 0x9c, 0x4b, 0x0d, 0xdd,  // $20-$27
		0x0d, 0x0d, 0x4b, 0xdd, 0x4b, 0x4b, 0x4b, 0x9c,  // $28-$2f
		0x4b, 0x4b, 0x4b, 0x4b, 0x4b, 0x02, 0x02, 0x4b,  // $30-$37
		0x4b, 0x9c, 0x4b, 0x4b, 0x4a, 0x4a, 0x02, 0x21,  // $38-$3f
		0x4a, 0x4b, 0x4a, 0x4b, 0x4b, 0x4b, 0x4a, 0x21,  // $40-$47
		0x39, 0x4b, 0x04, 0x1a, 0x4b, 0x00, 0x00, 0x00   // $48-$4f
	];

	/// <summary>
	/// Default DQ3r tile for unmapped DW4 tiles.
	/// </summary>
	public const byte DefaultTile = 0x4b; // Grass/generic

	/// <summary>
	/// Convert a single DW4 overworld tile to DQ3r tile.
	/// </summary>
	public static byte ConvertOverworldTile(byte dw4Tile) {
		if (dw4Tile < OverworldTileTranslation.Length) {
			return OverworldTileTranslation[dw4Tile];
		}
		return DefaultTile;
	}

	/// <summary>
	/// Convert DW4 overworld map to DQ3r format.
	/// </summary>
	public static byte[,] ConvertOverworldMap(byte[,] dw4Map) {
		int width = dw4Map.GetLength(1);
		int height = dw4Map.GetLength(0);
		var dq3rMap = new byte[height, width];

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				dq3rMap[y, x] = ConvertOverworldTile(dw4Map[y, x]);
			}
		}

		return dq3rMap;
	}

	/// <summary>
	/// Convert DW4 event type to DQ3r event type.
	/// </summary>
	public static DQ3rEventType ConvertEventType(EventType dw4Type) => dw4Type switch {
		EventType.None => DQ3rEventType.None,
		EventType.NPC => DQ3rEventType.NPC,
		EventType.Treasure => DQ3rEventType.Treasure,
		EventType.Door => DQ3rEventType.Door,
		EventType.Warp => DQ3rEventType.Warp,
		EventType.StairsUp => DQ3rEventType.Stairs,
		EventType.StairsDown => DQ3rEventType.Stairs,
		EventType.Script => DQ3rEventType.Script,
		EventType.Shop => DQ3rEventType.Shop,
		EventType.Inn => DQ3rEventType.Inn,
		EventType.Church => DQ3rEventType.Church,
		EventType.Vault => DQ3rEventType.Vault,
		_ => DQ3rEventType.None
	};

	/// <summary>
	/// Convert DW4 treasure to DQ3r treasure.
	/// </summary>
	public static DQ3rTreasure ConvertTreasure(TreasureChest dw4Chest) {
		return new DQ3rTreasure {
			Id = dw4Chest.Index,
			MapId = dw4Chest.MapId,
			X = dw4Chest.X,
			Y = dw4Chest.Y,
			ContentsType = ConvertTreasureType(dw4Chest.ContentsType),
			ContentsValue = (ushort)ConvertTreasureContents(dw4Chest)
		};
	}

	/// <summary>
	/// Convert DW4 treasure type to DQ3r treasure type.
	/// </summary>
	public static DQ3rTreasureType ConvertTreasureType(TreasureType dw4Type) => dw4Type switch {
		TreasureType.Item => DQ3rTreasureType.Item,
		TreasureType.Gold => DQ3rTreasureType.Gold,
		TreasureType.SmallMedal => DQ3rTreasureType.SmallMedal,
		TreasureType.Empty => DQ3rTreasureType.Empty,
		TreasureType.Monster => DQ3rTreasureType.Monster,
		_ => DQ3rTreasureType.Empty
	};

	/// <summary>
	/// Convert treasure contents (item/gold) to DQ3r equivalent.
	/// </summary>
	public static int ConvertTreasureContents(TreasureChest chest) {
		return chest.ContentsType switch {
			TreasureType.Item => ItemToDQ3r.ConvertItemId(chest.ContentsValue),
			TreasureType.Gold => chest.ContentsValue, // Gold amount stays same
			TreasureType.SmallMedal => 1, // Small medals are consistent
			_ => 0
		};
	}

	/// <summary>
	/// Convert DW4 warp to DQ3r warp.
	/// </summary>
	public static DQ3rWarp ConvertWarp(WarpPoint dw4Warp, Dictionary<int, int> mapIdMapping) {
		return new DQ3rWarp {
			SourceMapId = MapIdToDQ3r(dw4Warp.SourceMapId, mapIdMapping),
			SourceX = dw4Warp.SourceX,
			SourceY = dw4Warp.SourceY,
			DestMapId = MapIdToDQ3r(dw4Warp.DestMapId, mapIdMapping),
			DestX = dw4Warp.DestX,
			DestY = dw4Warp.DestY,
			Type = ConvertWarpType(dw4Warp.Type),
			Facing = (DQ3rDirection)dw4Warp.SourceX // Placeholder
		};
	}

	/// <summary>
	/// Convert DW4 warp type to DQ3r warp type.
	/// </summary>
	public static DQ3rWarpType ConvertWarpType(WarpType dw4Type) => dw4Type switch {
		WarpType.StairsUp => DQ3rWarpType.StairsUp,
		WarpType.StairsDown => DQ3rWarpType.StairsDown,
		WarpType.Door => DQ3rWarpType.Door,
		WarpType.Exit => DQ3rWarpType.Exit,
		WarpType.MapEdge => DQ3rWarpType.MapEdge,
		WarpType.Teleport => DQ3rWarpType.Teleport,
		WarpType.Fall => DQ3rWarpType.Fall,
		_ => DQ3rWarpType.Door
	};

	/// <summary>
	/// Convert DW4 map ID to DQ3r map ID.
	/// Requires mapping dictionary.
	/// </summary>
	public static int MapIdToDQ3r(int dw4MapId, Dictionary<int, int> mapIdMapping) {
		if (mapIdMapping.TryGetValue(dw4MapId, out int dq3rMapId)) {
			return dq3rMapId;
		}
		return dw4MapId; // Fallback to same ID
	}

	/// <summary>
	/// Convert DW4 map type to DQ3r location type.
	/// </summary>
	public static DQ3rLocationType ConvertMapType(MapType dw4Type) => dw4Type switch {
		MapType.Town => DQ3rLocationType.Town,
		MapType.Castle => DQ3rLocationType.Castle,
		MapType.Dungeon => DQ3rLocationType.Cave,
		MapType.Tower => DQ3rLocationType.Tower,
		MapType.Cave => DQ3rLocationType.Cave,
		MapType.Shrine => DQ3rLocationType.Shrine,
		MapType.Overworld => DQ3rLocationType.Other,
		MapType.Other => DQ3rLocationType.Other,
		_ => DQ3rLocationType.Other
	};

	/// <summary>
	/// Convert DW4 NPC to DQ3r NPC.
	/// </summary>
	public static DQ3rNpc ConvertNpc(NpcData dw4Npc, int mapId) {
		return new DQ3rNpc {
			Index = dw4Npc.Index,
			MapId = mapId,
			X = dw4Npc.X,
			Y = dw4Npc.Y,
			SpriteId = ConvertSpriteId(dw4Npc.SpriteId),
			Movement = ConvertNpcMovement(dw4Npc.Movement),
			Facing = (DQ3rDirection)dw4Npc.Facing,
			DialogId = dw4Npc.DialogId, // Dialog needs separate conversion
			Flags = ConvertNpcFlags(dw4Npc.Flags)
		};
	}

	/// <summary>
	/// Convert DW4 sprite ID to DQ3r sprite ID.
	/// </summary>
	public static ushort ConvertSpriteId(byte dw4SpriteId) {
		// TODO: Build full sprite mapping
		return dw4SpriteId; // Placeholder
	}

	/// <summary>
	/// Convert DW4 NPC movement to DQ3r movement.
	/// </summary>
	public static DQ3rNpcMovement ConvertNpcMovement(NpcMovement dw4Movement) => dw4Movement switch {
		NpcMovement.Stationary => DQ3rNpcMovement.Stationary,
		NpcMovement.Random => DQ3rNpcMovement.Random,
		NpcMovement.Pace => DQ3rNpcMovement.Pace,
		NpcMovement.Circle => DQ3rNpcMovement.Circle,
		NpcMovement.Follow => DQ3rNpcMovement.Follow,
		NpcMovement.Custom => DQ3rNpcMovement.Custom,
		_ => DQ3rNpcMovement.Stationary
	};

	/// <summary>
	/// Convert DW4 NPC flags to DQ3r NPC flags.
	/// </summary>
	public static DQ3rNpcFlags ConvertNpcFlags(NpcFlags dw4Flags) {
		DQ3rNpcFlags result = DQ3rNpcFlags.None;

		if ((dw4Flags & NpcFlags.Shop) != 0) result |= DQ3rNpcFlags.Shop;
		if ((dw4Flags & NpcFlags.Inn) != 0) result |= DQ3rNpcFlags.Inn;
		if ((dw4Flags & NpcFlags.Church) != 0) result |= DQ3rNpcFlags.Church;
		if ((dw4Flags & NpcFlags.Vault) != 0) result |= DQ3rNpcFlags.Vault;
		if ((dw4Flags & NpcFlags.Recruitable) != 0) result |= DQ3rNpcFlags.Recruitable;
		if ((dw4Flags & NpcFlags.Boss) != 0) result |= DQ3rNpcFlags.Boss;
		if ((dw4Flags & NpcFlags.Invisible) != 0) result |= DQ3rNpcFlags.Invisible;
		if ((dw4Flags & NpcFlags.ScriptTrigger) != 0) result |= DQ3rNpcFlags.ScriptTrigger;

		return result;
	}

	/// <summary>
	/// Convert DW4 entrance to DQ3r world entrance.
	/// </summary>
	public static DQ3rWorldEntrance ConvertEntrance(EntranceLocation dw4Entrance, Dictionary<int, int> mapIdMapping) {
		return new DQ3rWorldEntrance {
			Name = dw4Entrance.Name,
			X = dw4Entrance.X,
			Y = dw4Entrance.Y,
			DestMapId = MapIdToDQ3r(dw4Entrance.DestMapId, mapIdMapping),
			DestX = 0, // Center of destination map
			DestY = 0,
			Type = ConvertMapType(dw4Entrance.Type),
			Visible = true
		};
	}

	/// <summary>
	/// Convert DW4 encounter zone to DQ3r encounter zone.
	/// </summary>
	public static DQ3rEncounterZone ConvertEncounterZone(EncounterZone dw4Zone, Dictionary<int, int> mapIdMapping) {
		return new DQ3rEncounterZone {
			ZoneId = dw4Zone.Index,
			MapId = dw4Zone.MapId == 0xFF ? 0 : MapIdToDQ3r(dw4Zone.MapId, mapIdMapping),
			EncounterRate = dw4Zone.EncounterRate,
			MonsterGroups = dw4Zone.MonsterGroups.Select(g => (ushort)MonsterToDQ3r.ConvertMonsterId(g)).ToArray()
		};
	}
}
