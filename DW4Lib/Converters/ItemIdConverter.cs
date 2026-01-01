namespace DW4Lib.Converters;

/// <summary>
/// Item ID converter facade that provides equipment-type-specific conversions.
/// Wraps ItemToDQ3r for use by other converters.
/// </summary>
public static class ItemIdConverter {
	/// <summary>
	/// Base offset for DQ3r weapon IDs.
	/// </summary>
	private const int DQ3R_WEAPON_BASE = 0x000;

	/// <summary>
	/// Base offset for DQ3r armor IDs.
	/// </summary>
	private const int DQ3R_ARMOR_BASE = 0x080;

	/// <summary>
	/// Base offset for DQ3r shield IDs.
	/// </summary>
	private const int DQ3R_SHIELD_BASE = 0x0C0;

	/// <summary>
	/// Base offset for DQ3r helmet IDs.
	/// </summary>
	private const int DQ3R_HELMET_BASE = 0x0E0;

	/// <summary>
	/// Base offset for DQ3r accessory IDs.
	/// </summary>
	private const int DQ3R_ACCESSORY_BASE = 0x100;

	/// <summary>
	/// Base offset for DQ3r consumable item IDs.
	/// </summary>
	private const int DQ3R_CONSUMABLE_BASE = 0x140;

	/// <summary>
	/// Convert DW4 weapon ID to DQ3r format.
	/// </summary>
	public static int ConvertWeaponId(byte dw4WeaponId) {
		if (dw4WeaponId == 0) return 0; // No weapon

		// DW4 weapons are in lower ID range
		// Map to DQ3r weapon base range
		int baseId = ItemToDQ3r.ConvertItemId(dw4WeaponId);
		return DQ3R_WEAPON_BASE + baseId;
	}

	/// <summary>
	/// Convert DW4 armor ID to DQ3r format.
	/// </summary>
	public static int ConvertArmorId(byte dw4ArmorId) {
		if (dw4ArmorId == 0) return 0; // No armor

		// DW4 armor IDs are offset from weapon IDs
		// Typically in 0x10-0x30 range
		int baseId = ItemToDQ3r.ConvertItemId(dw4ArmorId);
		return DQ3R_ARMOR_BASE + (baseId - 0x10);
	}

	/// <summary>
	/// Convert DW4 shield ID to DQ3r format.
	/// </summary>
	public static int ConvertShieldId(byte dw4ShieldId) {
		if (dw4ShieldId == 0) return 0; // No shield

		// DW4 shields are in 0x30-0x40 range
		int baseId = ItemToDQ3r.ConvertItemId(dw4ShieldId);
		return DQ3R_SHIELD_BASE + (baseId - 0x30);
	}

	/// <summary>
	/// Convert DW4 helmet ID to DQ3r format.
	/// </summary>
	public static int ConvertHelmetId(byte dw4HelmetId) {
		if (dw4HelmetId == 0) return 0; // No helmet

		// DW4 helmets are in 0x40-0x50 range
		int baseId = ItemToDQ3r.ConvertItemId(dw4HelmetId);
		return DQ3R_HELMET_BASE + (baseId - 0x40);
	}

	/// <summary>
	/// Convert DW4 accessory ID to DQ3r format.
	/// </summary>
	public static int ConvertAccessoryId(byte dw4AccessoryId) {
		if (dw4AccessoryId == 0) return 0; // No accessory

		// DW4 accessories are in 0x50-0x60 range
		int baseId = ItemToDQ3r.ConvertItemId(dw4AccessoryId);
		return DQ3R_ACCESSORY_BASE + (baseId - 0x50);
	}

	/// <summary>
	/// Convert DW4 consumable/tool item ID to DQ3r format.
	/// </summary>
	public static int ConvertItemId(byte dw4ItemId) {
		if (dw4ItemId == 0) return 0;

		// General consumables are in higher ID ranges
		int baseId = ItemToDQ3r.ConvertItemId(dw4ItemId);

		// If it's clearly a consumable (0x50+), offset appropriately
		if (dw4ItemId >= 0x50) {
			return DQ3R_CONSUMABLE_BASE + (baseId - 0x50);
		}

		// Otherwise return the base converted ID
		return baseId;
	}

	/// <summary>
	/// Convert any DW4 item ID based on its category.
	/// </summary>
	public static int ConvertByCategory(byte dw4ItemId, DW4ItemCategory category) {
		return category switch {
			DW4ItemCategory.Weapon => ConvertWeaponId(dw4ItemId),
			DW4ItemCategory.Armor => ConvertArmorId(dw4ItemId),
			DW4ItemCategory.Shield => ConvertShieldId(dw4ItemId),
			DW4ItemCategory.Helmet => ConvertHelmetId(dw4ItemId),
			DW4ItemCategory.Accessory => ConvertAccessoryId(dw4ItemId),
			DW4ItemCategory.Tool => ConvertItemId(dw4ItemId),
			DW4ItemCategory.Important => ConvertItemId(dw4ItemId),
			_ => ConvertItemId(dw4ItemId)
		};
	}

	/// <summary>
	/// Batch convert an array of DW4 item IDs.
	/// </summary>
	public static int[] ConvertItemIds(byte[] dw4ItemIds) {
		return dw4ItemIds.Select(id => ConvertItemId(id)).ToArray();
	}

	/// <summary>
	/// Batch convert equipment array (weapon, armor, shield, helmet, accessory).
	/// </summary>
	public static int[] ConvertEquipmentSet(byte weapon, byte armor, byte shield, byte helmet, byte accessory = 0) {
		return [
			ConvertWeaponId(weapon),
			ConvertArmorId(armor),
			ConvertShieldId(shield),
			ConvertHelmetId(helmet),
			ConvertAccessoryId(accessory)
		];
	}
}
