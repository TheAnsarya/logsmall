using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;

namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 town/dungeon entrance locations to DQ3r format.
/// </summary>
public static class EntranceToDQ3r {
	/// <summary>
	/// Default DW4 entrance locations on the main overworld.
	/// Based on MAP_LIST.md documentation.
	/// </summary>
	public static readonly EntranceLocation[] DW4MainEntrances = [
		// Chapter 1 locations
		new() { Name = "Burland", X = 0x38, Y = 0x58, DestMapId = 0x02, Type = MapType.Castle },
		new() { Name = "Izmit", X = 0x48, Y = 0x60, DestMapId = 0x12, Type = MapType.Town },

		// Chapter 2 locations
		new() { Name = "Santeem", X = 0x70, Y = 0x30, DestMapId = 0x01, Type = MapType.Castle },
		new() { Name = "Surene", X = 0x60, Y = 0x40, DestMapId = 0x13, Type = MapType.Town },
		new() { Name = "Tempe", X = 0x50, Y = 0x38, DestMapId = 0x0E, Type = MapType.Town },
		new() { Name = "Frenor", X = 0x68, Y = 0x48, DestMapId = 0x0F, Type = MapType.Town },

		// Chapter 3 locations
		new() { Name = "Lakanaba", X = 0x28, Y = 0x38, DestMapId = 0x16, Type = MapType.Town },
		new() { Name = "Endor", X = 0x40, Y = 0x40, DestMapId = 0x04, Type = MapType.Town },
		new() { Name = "Bonmalmo", X = 0x18, Y = 0x48, DestMapId = 0x05, Type = MapType.Town },

		// Chapter 4 locations
		new() { Name = "Monbaraba", X = 0x80, Y = 0x70, DestMapId = 0x15, Type = MapType.Town },
		new() { Name = "Kievs", X = 0x90, Y = 0x60, DestMapId = 0x17, Type = MapType.Town },
		new() { Name = "Haville", X = 0x78, Y = 0x50, DestMapId = 0x11, Type = MapType.Town },
		new() { Name = "Aneaux", X = 0x88, Y = 0x58, DestMapId = 0x10, Type = MapType.Town },
		new() { Name = "Hometown", X = 0x68, Y = 0x68, DestMapId = 0x14, Type = MapType.Town },

		// Chapter 5 additional locations
		new() { Name = "Branca", X = 0xA0, Y = 0x48, DestMapId = 0x06, Type = MapType.Town },
		new() { Name = "Soretta", X = 0xB0, Y = 0x50, DestMapId = 0x07, Type = MapType.Town },
		new() { Name = "Gardenbur", X = 0xC0, Y = 0x60, DestMapId = 0x08, Type = MapType.Castle },
		new() { Name = "Stancia", X = 0xD0, Y = 0x58, DestMapId = 0x09, Type = MapType.Castle },
		new() { Name = "Mintos", X = 0x98, Y = 0x40, DestMapId = 0x0D, Type = MapType.Town },
		new() { Name = "Riverton", X = 0xB8, Y = 0x38, DestMapId = 0x0B, Type = MapType.Town },

		// Caves and Dungeons
		new() { Name = "Lighthouse", X = 0x30, Y = 0x50, DestMapId = 0x24, Type = MapType.Tower },
		new() { Name = "Birdsong Tower", X = 0x58, Y = 0x28, DestMapId = 0x2A, Type = MapType.Tower },
		new() { Name = "Cascade Cave", X = 0x48, Y = 0x30, DestMapId = 0x2B, Type = MapType.Cave },
		new() { Name = "Cave West of Kievs", X = 0x88, Y = 0x68, DestMapId = 0x25, Type = MapType.Cave },
		new() { Name = "Aktemto Mine", X = 0xA8, Y = 0x70, DestMapId = 0x2D, Type = MapType.Dungeon },

		// Shrines
		new() { Name = "House of Prophecy", X = 0x78, Y = 0x28, DestMapId = 0x1D, Type = MapType.Shrine },
		new() { Name = "Shrine to Endor", X = 0x38, Y = 0x40, DestMapId = 0x1E, Type = MapType.Shrine },
		new() { Name = "Small Medal King", X = 0x50, Y = 0x60, DestMapId = 0x22, Type = MapType.Shrine },
	];

	/// <summary>
	/// Gottside overworld entrances (Chapter 5).
	/// </summary>
	public static readonly EntranceLocation[] DW4GottsideEntrances = [
		new() { Name = "Gottside", X = 0x40, Y = 0x40, DestMapId = 0x1A, Type = MapType.Town, Overworld = OverworldType.Gottside },
		new() { Name = "Rosaville", X = 0x50, Y = 0x38, DestMapId = 0x1B, Type = MapType.Town, Overworld = OverworldType.Gottside },
	];

	/// <summary>
	/// Underworld entrances (Chapter 5).
	/// </summary>
	public static readonly EntranceLocation[] DW4UnderworldEntrances = [
		new() { Name = "Cave of Betrayal", X = 0x30, Y = 0x50, DestMapId = 0x44, Type = MapType.Cave, Overworld = OverworldType.Underworld },
		new() { Name = "Necrosaro's Palace", X = 0x80, Y = 0x40, DestMapId = 0x45, Type = MapType.Dungeon, Overworld = OverworldType.Underworld },
	];

	/// <summary>
	/// Convert all DW4 entrances to DQ3r format.
	/// </summary>
	public static List<DQ3rWorldEntrance> ConvertAllEntrances(Dictionary<int, int> mapIdMapping) {
		var result = new List<DQ3rWorldEntrance>();

		// Convert main overworld entrances
		result.AddRange(DW4MainEntrances.Select(e => ConvertEntrance(e, mapIdMapping)));

		// Note: Gottside and Underworld would need separate handling
		// as DQ3r has a different world structure

		return result;
	}

	/// <summary>
	/// Convert single DW4 entrance to DQ3r entrance.
	/// </summary>
	public static DQ3rWorldEntrance ConvertEntrance(EntranceLocation dw4Entrance, Dictionary<int, int> mapIdMapping) {
		return new DQ3rWorldEntrance {
			Name = dw4Entrance.Name,
			X = ScaleCoordinate(dw4Entrance.X, DW4MapWidth, DQ3rMapWidth),
			Y = ScaleCoordinate(dw4Entrance.Y, DW4MapHeight, DQ3rMapHeight),
			DestMapId = mapIdMapping.TryGetValue(dw4Entrance.DestMapId, out int dq3rId) ? dq3rId : dw4Entrance.DestMapId,
			DestX = 8, // Default entrance position
			DestY = 8,
			Type = ConvertLocationType(dw4Entrance.Type),
			Visible = true
		};
	}

	/// <summary>
	/// DW4 map width/height (256 tiles).
	/// </summary>
	private const int DW4MapWidth = 256;
	private const int DW4MapHeight = 256;

	/// <summary>
	/// DQ3r map width/height (256 tiles).
	/// </summary>
	private const int DQ3rMapWidth = 256;
	private const int DQ3rMapHeight = 256;

	/// <summary>
	/// Scale coordinate from DW4 to DQ3r dimensions.
	/// </summary>
	private static ushort ScaleCoordinate(byte value, int sourceMax, int targetMax) {
		// If maps are same size, no scaling needed
		if (sourceMax == targetMax) return value;

		// Scale proportionally
		return (ushort)((value * targetMax) / sourceMax);
	}

	/// <summary>
	/// Convert DW4 map type to DQ3r location type.
	/// </summary>
	private static DQ3rLocationType ConvertLocationType(MapType dw4Type) => dw4Type switch {
		MapType.Town => DQ3rLocationType.Town,
		MapType.Castle => DQ3rLocationType.Castle,
		MapType.Dungeon => DQ3rLocationType.Cave,
		MapType.Tower => DQ3rLocationType.Tower,
		MapType.Cave => DQ3rLocationType.Cave,
		MapType.Shrine => DQ3rLocationType.Shrine,
		_ => DQ3rLocationType.Other
	};

	/// <summary>
	/// Generate map ID mapping from DW4 to DQ3r.
	/// This should be customized based on the target DQ3r hack.
	/// </summary>
	public static Dictionary<int, int> GenerateDefaultMapIdMapping() {
		var mapping = new Dictionary<int, int>();

		// Maps that have direct equivalents
		// DW4 -> DQ3r (these need to be adjusted for actual DQ3r map IDs)
		// For now, use identity mapping as placeholder

		for (int i = 0; i <= 0x48; i++) {
			mapping[i] = i + 0x100; // Offset to avoid conflicts
		}

		return mapping;
	}
}

/// <summary>
/// Result of entrance conversion.
/// </summary>
public class EntranceConversionResult {
	/// <summary>
	/// Converted world map entrances.
	/// </summary>
	public List<DQ3rWorldEntrance> Entrances { get; set; } = [];

	/// <summary>
	/// Map ID mapping used.
	/// </summary>
	public Dictionary<int, int> MapIdMapping { get; set; } = [];

	/// <summary>
	/// Warnings generated during conversion.
	/// </summary>
	public List<string> Warnings { get; set; } = [];

	/// <summary>
	/// Statistics about the conversion.
	/// </summary>
	public EntranceConversionStats Stats { get; set; } = new();
}

/// <summary>
/// Statistics for entrance conversion.
/// </summary>
public class EntranceConversionStats {
	/// <summary>
	/// Total entrances processed.
	/// </summary>
	public int TotalEntrances { get; set; }

	/// <summary>
	/// Number of towns converted.
	/// </summary>
	public int TownCount { get; set; }

	/// <summary>
	/// Number of castles converted.
	/// </summary>
	public int CastleCount { get; set; }

	/// <summary>
	/// Number of caves/dungeons converted.
	/// </summary>
	public int CaveCount { get; set; }

	/// <summary>
	/// Number of towers converted.
	/// </summary>
	public int TowerCount { get; set; }

	/// <summary>
	/// Number of shrines converted.
	/// </summary>
	public int ShrineCount { get; set; }
}
