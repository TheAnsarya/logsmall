namespace DQ4rLib.Casino;

/// <summary>
/// Represents a poker hand type.
/// Values match NES ROM hand evaluation order (best to worst).
/// Based on DW4 NES Bank 23 evaluation at $81F2.
/// </summary>
public enum PokerHandType {
	/// <summary>No winning combination.</summary>
	Nothing = 0,

	/// <summary>Two cards of the same rank.</summary>
	OnePair = 1,

	/// <summary>Two different pairs.</summary>
	TwoPairs = 2,

	/// <summary>Three cards of the same rank.</summary>
	ThreeOfAKind = 3,

	/// <summary>Five cards in sequence, any suits.</summary>
	Straight = 4,

	/// <summary>Five cards of the same suit.</summary>
	Flush = 5,

	/// <summary>Three of a kind plus a pair.</summary>
	FullHouse = 6,

	/// <summary>Four cards of the same rank.</summary>
	FourOfAKind = 7,

	/// <summary>Five cards in sequence, same suit.</summary>
	StraightFlush = 8,

	/// <summary>Five cards of same rank (using Joker as wild).</summary>
	FiveOfAKind = 9,

	/// <summary>10-J-Q-K-A of the same suit.</summary>
	RoyalFlush = 10
}

/// <summary>
/// Payout multipliers for each hand type.
/// From NES ROM table at $81C5.
/// </summary>
public static class PokerPayouts {
	/// <summary>Payout multipliers indexed by PokerHandType.</summary>
	public static readonly int[] Multipliers = [
		0,    // Nothing
		1,    // One Pair
		2,    // Two Pairs
		3,    // Three of a Kind
		5,    // Straight
		10,   // Flush
		20,   // Full House
		50,   // Four of a Kind
		100,  // Straight Flush
		250,  // Five of a Kind
		500   // Royal Flush
	];

	/// <summary>
	/// Gets the payout for a hand type.
	/// </summary>
	/// <param name="hand">The hand type.</param>
	/// <param name="bet">The bet amount.</param>
	/// <returns>Total payout (bet × multiplier).</returns>
	public static uint CalculatePayout(PokerHandType hand, uint bet) {
		var multiplier = Multipliers[(int)hand];
		return bet * (uint)multiplier;
	}
}
