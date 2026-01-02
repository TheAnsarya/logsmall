namespace DQ4rLib.Casino;

/// <summary>
/// Represents an item available at the casino prize exchange.
/// </summary>
public class CasinoPrize {
	/// <summary>Item ID in the game's item table.</summary>
	public byte ItemId { get; init; }

	/// <summary>Display name of the prize.</summary>
	public required string Name { get; init; }

	/// <summary>Cost in casino coins.</summary>
	public uint Cost { get; init; }

	/// <summary>Item category for display purposes.</summary>
	public PrizeCategory Category { get; init; }

	/// <summary>Description/notes about the item.</summary>
	public string? Description { get; init; }
}

/// <summary>
/// Prize categories.
/// </summary>
public enum PrizeCategory {
	Weapon,
	Armor,
	Helmet,
	Shield,
	Accessory,
	Consumable,
	Special
}

/// <summary>
/// Casino prize exchange system.
/// Based on DW4 NES Endor Casino prize counter.
/// </summary>
public class CasinoPrizes {
	private static readonly List<CasinoPrize> _prizes =
	[
		// Weapons
		new() { ItemId = 0x45, Name = "Falcon Sword", Cost = 65_000, Category = PrizeCategory.Weapon,
			Description = "Attacks twice per round" },
		new() { ItemId = 0x50, Name = "Gringham Whip", Cost = 40_000, Category = PrizeCategory.Weapon,
			Description = "Hits all enemies" },
		new() { ItemId = 0x4A, Name = "Demon Hammer", Cost = 25_000, Category = PrizeCategory.Weapon,
			Description = "May instant kill" },
		new() { ItemId = 0x48, Name = "Metal Babble Sword", Cost = 50_000, Category = PrizeCategory.Weapon,
			Description = "Highest attack weapon" },
		new() { ItemId = 0x52, Name = "Sword of Miracles", Cost = 45_000, Category = PrizeCategory.Weapon,
			Description = "Heals HP on attack" },

		// Armor
		new() { ItemId = 0x89, Name = "Metal Babble Armor", Cost = 35_000, Category = PrizeCategory.Armor,
			Description = "Best armor in game" },
		new() { ItemId = 0x85, Name = "Magic Bikini", Cost = 15_000, Category = PrizeCategory.Armor,
			Description = "Females only" },
		new() { ItemId = 0x80, Name = "Dancer's Costume", Cost = 5_000, Category = PrizeCategory.Armor,
			Description = "Agility bonus" },

		// Helmets
		new() { ItemId = 0xA1, Name = "Metal Babble Helm", Cost = 30_000, Category = PrizeCategory.Helmet,
			Description = "Best helmet" },
		new() { ItemId = 0xB5, Name = "Hat of Happiness", Cost = 10_000, Category = PrizeCategory.Helmet,
			Description = "Heals while walking" },

		// Shields
		new() { ItemId = 0xC5, Name = "Aeolus' Shield", Cost = 20_000, Category = PrizeCategory.Shield,
			Description = "Fire/ice resistance" },

		// Accessories
		new() { ItemId = 0xC2, Name = "Meteorite Bracer", Cost = 50_000, Category = PrizeCategory.Accessory,
			Description = "Doubles agility" },
		new() { ItemId = 0xC8, Name = "Gospel Ring", Cost = 25_000, Category = PrizeCategory.Accessory,
			Description = "Prevents encounters" },

		// Consumables
		new() { ItemId = 0x20, Name = "Prayer Ring", Cost = 500, Category = PrizeCategory.Consumable,
			Description = "Restores MP (reusable)" },
		new() { ItemId = 0x15, Name = "Small Medal", Cost = 100, Category = PrizeCategory.Special,
			Description = "Medal King collection" },
		new() { ItemId = 0x30, Name = "Seed of Strength", Cost = 200, Category = PrizeCategory.Consumable,
			Description = "+1-3 STR permanently" },
		new() { ItemId = 0x31, Name = "Seed of Agility", Cost = 200, Category = PrizeCategory.Consumable,
			Description = "+1-3 AGI permanently" },
		new() { ItemId = 0x33, Name = "Seed of Luck", Cost = 200, Category = PrizeCategory.Consumable,
			Description = "+1-3 LCK permanently" },
	];

	/// <summary>All available prizes.</summary>
	public static IReadOnlyList<CasinoPrize> AllPrizes => _prizes;

	/// <summary>
	/// Gets prizes in a specific category.
	/// </summary>
	public static IEnumerable<CasinoPrize> GetByCategory(PrizeCategory category) =>
		_prizes.Where(p => p.Category == category);

	/// <summary>
	/// Gets prizes affordable with a given coin amount.
	/// </summary>
	public static IEnumerable<CasinoPrize> GetAffordable(uint coins) =>
		_prizes.Where(p => p.Cost <= coins).OrderBy(p => p.Cost);

	/// <summary>
	/// Gets a prize by item ID.
	/// </summary>
	public static CasinoPrize? GetByItemId(byte itemId) =>
		_prizes.FirstOrDefault(p => p.ItemId == itemId);

	/// <summary>
	/// Checks if a prize can be purchased.
	/// </summary>
	public static bool CanAfford(CasinoPrize prize, uint coins) =>
		coins >= prize.Cost;

	/// <summary>
	/// Gold to coin exchange rate (20 gold = 1 coin).
	/// </summary>
	public const int GoldPerCoin = 20;

	/// <summary>
	/// Calculates coins from gold exchange.
	/// </summary>
	/// <param name="gold">Amount of gold.</param>
	/// <returns>Number of coins received.</returns>
	public static uint ExchangeGoldToCoins(uint gold) =>
		gold / GoldPerCoin;

	/// <summary>
	/// Calculates effective gold cost of a prize.
	/// </summary>
	/// <param name="prize">The prize.</param>
	/// <returns>Equivalent gold value.</returns>
	public static uint GetGoldValue(CasinoPrize prize) =>
		prize.Cost * GoldPerCoin;
}
