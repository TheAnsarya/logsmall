namespace DW4Lib.DQ3r;

/// <summary>
/// DQ3 Remake item format (SNES 16-bit).
/// </summary>
public class DQ3rItem {
	/// <summary>ID in the DQ3r item table.</summary>
	public int Id { get; set; }

	/// <summary>Item name.</summary>
	public string Name { get; set; } = "";

	/// <summary>Item type/category.</summary>
	public DQ3rItemType Type { get; set; }

	/// <summary>Buy price in gold.</summary>
	public int BuyPrice { get; set; }

	/// <summary>Sell price (typically half buy price).</summary>
	public int SellPrice { get; set; }

	/// <summary>Attack bonus (weapons).</summary>
	public int AttackBonus { get; set; }

	/// <summary>Defense bonus (armor/shields).</summary>
	public int DefenseBonus { get; set; }

	/// <summary>Agility modifier.</summary>
	public int AgilityMod { get; set; }

	/// <summary>Special effect ID.</summary>
	public int SpecialEffect { get; set; }

	/// <summary>Who can equip (bitmask).</summary>
	public int EquipFlags { get; set; }

	/// <summary>Icon/sprite ID.</summary>
	public int IconId { get; set; }

	/// <summary>Item description.</summary>
	public string Description { get; set; } = "";

	/// <summary>Is this item cursed?</summary>
	public bool IsCursed { get; set; }

	/// <summary>Can be used in battle?</summary>
	public bool UsableInBattle { get; set; }

	/// <summary>Can be used in field?</summary>
	public bool UsableInField { get; set; }

	/// <summary>Source DW4 item ID.</summary>
	public int SourceDW4Id { get; set; }

	/// <summary>Conversion notes.</summary>
	public string Notes { get; set; } = "";
}

/// <summary>
/// DQ3r item type categories.
/// </summary>
public enum DQ3rItemType {
	Consumable = 0,
	Weapon = 1,
	Armor = 2,
	Shield = 3,
	Helmet = 4,
	Accessory = 5,
	KeyItem = 6,
	Tool = 7
}
