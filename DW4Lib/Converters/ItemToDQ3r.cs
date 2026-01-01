namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 item IDs to DQ3r item IDs.
/// </summary>
public static class ItemToDQ3r {
	/// <summary>
	/// DW4 to DQ3r item ID mapping table.
	/// Index is DW4 item ID, value is DQ3r item ID.
	/// </summary>
	private static readonly ushort[] ItemMapping = CreateItemMapping();

	/// <summary>
	/// Convert a DW4 item ID to DQ3r item ID.
	/// </summary>
	public static int ConvertItemId(int dw4ItemId) {
		if (dw4ItemId < 0 || dw4ItemId >= ItemMapping.Length) {
			return dw4ItemId; // Return as-is if out of range
		}
		return ItemMapping[dw4ItemId];
	}

	/// <summary>
	/// Create the item ID mapping table.
	/// Maps common items between games by function.
	/// </summary>
	private static ushort[] CreateItemMapping() {
		// DW4 has ~200 items, DQ3r has ~256
		// This mapping is based on item functions/purposes
		var mapping = new ushort[256];

		// Initialize with identity mapping
		for (int i = 0; i < 256; i++) {
			mapping[i] = (ushort)i;
		}

		// Weapons (approximate mappings by weapon type/tier)
		// DW4 weapon IDs -> DQ3r equivalent weapon IDs
		mapping[0x01] = 0x01; // Cypress Stick -> Cypress Stick
		mapping[0x02] = 0x02; // Club -> Club
		mapping[0x03] = 0x03; // Copper Sword -> Copper Sword
		mapping[0x04] = 0x04; // Boomerang -> Boomerang
		mapping[0x05] = 0x05; // Iron Claw -> similar
		mapping[0x06] = 0x06; // Thorn Whip
		mapping[0x07] = 0x07; // Chain Sickle
		mapping[0x08] = 0x08; // Iron Spear
		mapping[0x09] = 0x09; // Iron Fan
		mapping[0x0A] = 0x0A; // Poison Needle

		// Note: Full mapping requires research into both games' item lists
		// and matching by function/stats

		return mapping;
	}

	/// <summary>
	/// Get the DQ3r equivalent item category.
	/// </summary>
	public static ItemCategory ConvertItemCategory(DW4ItemCategory dw4Category) => dw4Category switch {
		DW4ItemCategory.Weapon => ItemCategory.Weapon,
		DW4ItemCategory.Armor => ItemCategory.Armor,
		DW4ItemCategory.Shield => ItemCategory.Shield,
		DW4ItemCategory.Helmet => ItemCategory.Helmet,
		DW4ItemCategory.Accessory => ItemCategory.Accessory,
		DW4ItemCategory.Tool => ItemCategory.Tool,
		DW4ItemCategory.Important => ItemCategory.Important,
		_ => ItemCategory.Tool
	};
}

/// <summary>
/// DW4 item categories.
/// </summary>
public enum DW4ItemCategory {
	Weapon,
	Armor,
	Shield,
	Helmet,
	Accessory,
	Tool,
	Important
}

/// <summary>
/// DQ3r item categories.
/// </summary>
public enum ItemCategory {
	Weapon,
	Armor,
	Shield,
	Helmet,
	Accessory,
	Tool,
	Important
}
