using DW4Lib.DataStructures;

namespace DW4Lib.DQ3r;

/// <summary>
/// Converts DW4 NES items to DQ3r SNES format.
/// </summary>
public static class ItemToDQ3r {
	/// <summary>
	/// Price scaling factor for NES to SNES economy.
	/// </summary>
	public const double PriceScaling = 2.0;

	/// <summary>
	/// Stat bonus scaling for 8-bit to 16-bit.
	/// </summary>
	public const double StatScaling = 2.5;

	/// <summary>
	/// Map DW4 ItemType to DQ3r ItemType.
	/// </summary>
	private static readonly Dictionary<ItemType, DQ3rItemType> TypeMapping = new() {
		{ ItemType.Consumable, DQ3rItemType.Consumable },
		{ ItemType.Weapon, DQ3rItemType.Weapon },
		{ ItemType.Armor, DQ3rItemType.Armor },
		{ ItemType.Shield, DQ3rItemType.Shield },
		{ ItemType.Helmet, DQ3rItemType.Helmet },
		{ ItemType.Accessory, DQ3rItemType.Accessory },
		{ ItemType.KeyItem, DQ3rItemType.KeyItem },
		{ ItemType.Special, DQ3rItemType.Tool },
	};

	/// <summary>
	/// Convert a single DW4 item to DQ3r format.
	/// </summary>
	public static DQ3rItem Convert(Item dw4Item, int id, string name = "") {
		var type = MapItemType(dw4Item.EquipmentSlot);
		int buyPrice = (int)(dw4Item.Price * PriceScaling);

		return new DQ3rItem {
			Id = id,
			Name = string.IsNullOrEmpty(name) ? $"Item_{id:D3}" : name,
			Type = type,
			BuyPrice = buyPrice,
			SellPrice = buyPrice / 2,
			AttackBonus = type == DQ3rItemType.Weapon ? ScaleStat(dw4Item.StatBonus) : 0,
			DefenseBonus = IsDefensiveEquipment(type) ? ScaleStat(dw4Item.StatBonus) : 0,
			AgilityMod = 0, // DW4 doesn't have agility mods on items
			SpecialEffect = MapSpecialEffect(dw4Item.SpecialEffect),
			EquipFlags = ConvertEquipFlags(dw4Item.EquipFlags),
			IconId = dw4Item.IconID,
			Description = GenerateDescription(dw4Item, type),
			IsCursed = (dw4Item.SpecialEffect & 0x80) != 0, // Assuming high bit = cursed
			UsableInBattle = type == DQ3rItemType.Consumable || HasBattleUse(dw4Item.SpecialEffect),
			UsableInField = type == DQ3rItemType.Consumable || type == DQ3rItemType.Tool,
			SourceDW4Id = id,
			Notes = $"Converted from DW4 item {id}"
		};
	}

	/// <summary>
	/// Convert all items from DW4 data.
	/// </summary>
	public static List<DQ3rItem> ConvertAll(List<Item> dw4Items, List<string>? names = null) {
		var result = new List<DQ3rItem>();

		for (int i = 0; i < dw4Items.Count; i++) {
			string name = (names != null && i < names.Count) ? names[i] : "";
			result.Add(Convert(dw4Items[i], i, name));
		}

		return result;
	}

	private static DQ3rItemType MapItemType(ItemType dw4Type) {
		return TypeMapping.TryGetValue(dw4Type, out var dq3Type)
			? dq3Type
			: DQ3rItemType.Consumable;
	}

	private static int ScaleStat(byte value) {
		return (int)Math.Round(value * StatScaling);
	}

	private static bool IsDefensiveEquipment(DQ3rItemType type) {
		return type is DQ3rItemType.Armor or DQ3rItemType.Shield or DQ3rItemType.Helmet or DQ3rItemType.Accessory;
	}

	private static int MapSpecialEffect(byte dw4Effect) {
		// Map DW4 special effects to DQ3r equivalents
		// This would need a full mapping table based on research
		return dw4Effect switch {
			0x01 => 0x01, // Heal on use
			0x02 => 0x02, // Damage enemy
			0x10 => 0x10, // Cast spell when used
			0x20 => 0x20, // Stat boost
			_ => dw4Effect
		};
	}

	private static int ConvertEquipFlags(byte dw4Flags) {
		// DW4 and DQ3r may have different character orderings
		// This is a simple pass-through; expand as needed
		return dw4Flags;
	}

	private static bool HasBattleUse(byte specialEffect) {
		// Items with attack or buff effects are battle-usable
		return (specialEffect & 0x0F) > 0;
	}

	private static string GenerateDescription(Item item, DQ3rItemType type) {
		return type switch {
			DQ3rItemType.Weapon => $"ATK +{ScaleStat(item.StatBonus)}",
			DQ3rItemType.Armor => $"DEF +{ScaleStat(item.StatBonus)}",
			DQ3rItemType.Shield => $"DEF +{ScaleStat(item.StatBonus)}",
			DQ3rItemType.Helmet => $"DEF +{ScaleStat(item.StatBonus)}",
			DQ3rItemType.Accessory => $"Accessory",
			DQ3rItemType.Consumable => "Consumable item",
			DQ3rItemType.KeyItem => "Key item",
			DQ3rItemType.Tool => "Tool",
			_ => ""
		};
	}
}
