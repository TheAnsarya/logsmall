namespace DW4Lib.DataStructures.Chapter3;

/// <summary>
/// Chapter 3 "Taloon the Arms Merchant" specific data.
/// Protagonist: Taloon
/// Mechanics: Merchant abilities, hired NPCs, shop ownership
/// Location: Lakanaba/Endor region
/// </summary>
public static class Chapter3Data {
	/// <summary>
	/// Chapter 3 ID.
	/// </summary>
	public const byte ChapterId = 0x02;

	/// <summary>
	/// Taloon's character ID.
	/// </summary>
	public const byte TaloonId = 0x05;

	/// <summary>
	/// Laurent's NPC companion ID.
	/// </summary>
	public const byte LaurentId = 0xC7;

	/// <summary>
	/// Strom's NPC companion ID.
	/// </summary>
	public const byte StromId = 0xC8;

	/// <summary>
	/// Taloon's starting stats at level 1.
	/// </summary>
	public static readonly CharacterStats TaloonStartingStats = new() {
		Level = 1,
		HP = 28,
		MP = 0,
		Strength = 9,
		Agility = 4,
		Vitality = 10,
		Intelligence = 8,
		Luck = 15 // High luck for merchant
	};

	/// <summary>
	/// Chapter 3 accessible maps.
	/// </summary>
	public static readonly Chapter3Map[] Maps = [
		new() {
			Id = 0x16,
			Name = "Lakanaba",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = true,
			Notes = "Starting location, Taloon's home"
		},
		new() {
			Id = 0x17,
			Name = "Lakanaba Weapon Shop",
			Type = MapLocationType.Shop,
			HasShop = true,
			HasInn = false,
			HasChurch = false,
			Notes = "Where Taloon works initially"
		},
		new() {
			Id = 0x18,
			Name = "Bonmalmo",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = true,
			Notes = "Eastern town with cave access"
		},
		new() {
			Id = 0x35,
			Name = "Cave to Endor",
			Type = MapLocationType.Cave,
			HasShop = false,
			HasInn = false,
			HasChurch = false,
			Notes = "Connects Lakanaba region to Endor"
		},
		new() {
			Id = 0x08,
			Name = "Endor",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = true,
			Notes = "Chapter 3 destination"
		},
		new() {
			Id = 0x36,
			Name = "Iron Safe Cave",
			Type = MapLocationType.Cave,
			HasShop = false,
			HasInn = false,
			HasChurch = false,
			Notes = "Contains the Iron Safe"
		},
		new() {
			Id = 0x19,
			Name = "Foxville",
			Type = MapLocationType.Town,
			HasShop = true,
			HasInn = true,
			HasChurch = false,
			Notes = "Fox transformation town"
		}
	];

	/// <summary>
	/// Chapter 3 story events in order.
	/// </summary>
	public static readonly Chapter3Event[] Events = [
		new() {
			Id = 0x0301,
			Name = "Working at the Shop",
			Description = "Taloon starts as a shop clerk",
			MapId = 0x17
		},
		new() {
			Id = 0x0302,
			Name = "Earn 60000 Gold",
			Description = "Save up money working and adventuring",
			GoldRequired = 60000
		},
		new() {
			Id = 0x0303,
			Name = "Royal Letter",
			Description = "Receive letter from the King",
			MapId = 0x16
		},
		new() {
			Id = 0x0304,
			Name = "Cave Shortcut",
			Description = "Get workers to tunnel through",
			MapId = 0x35
		},
		new() {
			Id = 0x0305,
			Name = "Iron Safe",
			Description = "Retrieve the Iron Safe for storage",
			MapId = 0x36
		},
		new() {
			Id = 0x0306,
			Name = "Open Endor Shop",
			Description = "Finally open shop in Endor",
			MapId = 0x08
		},
		new() {
			Id = 0x0307,
			Name = "Ship Departure",
			Description = "Prepare to sail with Tom and his son",
			MapId = 0x08
		},
		new() {
			Id = 0x0308,
			Name = "Chapter 3 Complete",
			Description = "Chapter 3 ends as ship sails"
		}
	];

	/// <summary>
	/// Merchant special abilities available to Taloon.
	/// </summary>
	public static readonly MerchantAbility[] MerchantAbilities = [
		new() {
			Name = "Sell to Shop",
			Description = "Can sell items at any weapon/armor shop",
			UnlockLevel = 1
		},
		new() {
			Name = "Evaluate Item",
			Description = "Appraise item's true value",
			UnlockLevel = 3
		},
		new() {
			Name = "Call for Help",
			Description = "Sometimes summons wandering merchant in battle",
			UnlockLevel = 5
		},
		new() {
			Name = "Toss Powder",
			Description = "May throw blinding powder at enemies",
			UnlockLevel = 8
		},
		new() {
			Name = "Pick Up Gold",
			Description = "Sometimes finds extra gold after battle",
			UnlockLevel = 10
		},
		new() {
			Name = "Whistle",
			Description = "Call horse and wagon",
			UnlockLevel = 15
		}
	];

	/// <summary>
	/// Work at shop: item appearances and prices.
	/// </summary>
	public static readonly ShopWorkItem[] ShopWorkItems = [
		new() { ItemId = 0x02, ItemName = "Club", BuyPrice = 30, SellPrice = 15 },
		new() { ItemId = 0x03, ItemName = "Copper Sword", BuyPrice = 100, SellPrice = 50 },
		new() { ItemId = 0x04, ItemName = "Boomerang", BuyPrice = 350, SellPrice = 175 },
		new() { ItemId = 0x11, ItemName = "Leather Armor", BuyPrice = 180, SellPrice = 90 }
	];

	/// <summary>
	/// Gold threshold for shop purchase.
	/// </summary>
	public const int EndorShopCost = 35000;
}

/// <summary>
/// Chapter 3 character stats.
/// </summary>
public class CharacterStats {
	public int Level { get; set; }
	public int HP { get; set; }
	public int MP { get; set; }
	public int Strength { get; set; }
	public int Agility { get; set; }
	public int Vitality { get; set; }
	public int Intelligence { get; set; }
	public int Luck { get; set; }
}

/// <summary>
/// Chapter 3 map location.
/// </summary>
public class Chapter3Map {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public MapLocationType Type { get; set; }
	public bool HasShop { get; set; }
	public bool HasInn { get; set; }
	public bool HasChurch { get; set; }
	public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Map location types (extended for Chapter 3).
/// </summary>
public enum MapLocationType {
	Castle,
	Town,
	Cave,
	Dungeon,
	Tower,
	Shrine,
	Overworld,
	Shop
}

/// <summary>
/// Chapter 3 story event.
/// </summary>
public class Chapter3Event {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int MapId { get; set; }
	public int GoldRequired { get; set; }
}

/// <summary>
/// Merchant special ability.
/// </summary>
public class MerchantAbility {
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int UnlockLevel { get; set; }
}

/// <summary>
/// Item available for sale when working at shop.
/// </summary>
public class ShopWorkItem {
	public int ItemId { get; set; }
	public string ItemName { get; set; } = string.Empty;
	public int BuyPrice { get; set; }
	public int SellPrice { get; set; }
}
