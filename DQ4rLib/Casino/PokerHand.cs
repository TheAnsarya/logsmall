namespace DQ4rLib.Casino;

/// <summary>
/// Poker hand evaluation logic.
/// Port of DW4 NES Bank 23 evaluation routines at $81F2-$861F.
/// </summary>
public static class PokerHand {
	/// <summary>
	/// Evaluates a 5-card poker hand.
	/// Checks hands from best to worst, matching NES evaluation order.
	/// </summary>
	/// <param name="cards">Array of exactly 5 cards.</param>
	/// <returns>The best hand type found.</returns>
	/// <exception cref="ArgumentException">Cards array is not exactly 5 cards.</exception>
	public static PokerHandType Evaluate(Card[] cards) {
		if (cards.Length != 5) {
			throw new ArgumentException("Poker hand must have exactly 5 cards.", nameof(cards));
		}

		// Count jokers and non-joker cards
		var jokerCount = cards.Count(c => c.IsJoker);
		var normalCards = cards.Where(c => !c.IsJoker).ToArray();

		// Check hands in order (best to worst) - matches NES ROM $81F2
		if (IsRoyalFlush(normalCards, jokerCount)) return PokerHandType.RoyalFlush;
		if (IsFiveOfAKind(normalCards, jokerCount)) return PokerHandType.FiveOfAKind;
		if (IsStraightFlush(normalCards, jokerCount)) return PokerHandType.StraightFlush;
		if (IsFourOfAKind(normalCards, jokerCount)) return PokerHandType.FourOfAKind;
		if (IsFullHouse(normalCards, jokerCount)) return PokerHandType.FullHouse;
		if (IsFlush(normalCards, jokerCount)) return PokerHandType.Flush;
		if (IsStraight(normalCards, jokerCount)) return PokerHandType.Straight;
		if (IsThreeOfAKind(normalCards, jokerCount)) return PokerHandType.ThreeOfAKind;
		if (IsTwoPairs(normalCards, jokerCount)) return PokerHandType.TwoPairs;
		if (IsOnePair(normalCards, jokerCount)) return PokerHandType.OnePair;

		return PokerHandType.Nothing;
	}

	/// <summary>
	/// Gets the rank counts for a set of cards.
	/// </summary>
	private static int[] GetRankCounts(Card[] cards) {
		var counts = new int[13]; // A-K
		foreach (var card in cards) {
			if (!card.IsJoker) {
				counts[(int)card.Rank]++;
			}
		}
		return counts;
	}

	/// <summary>
	/// Gets the suit counts for a set of cards.
	/// </summary>
	private static int[] GetSuitCounts(Card[] cards) {
		var counts = new int[4]; // S, H, D, C
		foreach (var card in cards) {
			if (!card.IsJoker) {
				counts[(int)card.Suit]++;
			}
		}
		return counts;
	}

	/// <summary>
	/// Check for Royal Flush: 10-J-Q-K-A of same suit.
	/// NES: $84A0
	/// </summary>
	private static bool IsRoyalFlush(Card[] cards, int jokerCount) {
		if (!IsFlush(cards, jokerCount)) return false;

		var ranks = cards.Select(c => c.Rank).ToHashSet();
		var royalRanks = new[] { Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Ace };
		var missing = royalRanks.Count(r => !ranks.Contains(r));

		return missing <= jokerCount;
	}

	/// <summary>
	/// Check for 5 of a Kind (with Joker as wild).
	/// NES: $84CD
	/// </summary>
	private static bool IsFiveOfAKind(Card[] cards, int jokerCount) {
		if (jokerCount == 0) return false;

		var counts = GetRankCounts(cards);
		var maxOfKind = counts.Max();

		return maxOfKind + jokerCount >= 5;
	}

	/// <summary>
	/// Check for Straight Flush: 5 sequential cards of same suit.
	/// NES: $84EF
	/// </summary>
	private static bool IsStraightFlush(Card[] cards, int jokerCount) {
		return IsFlush(cards, jokerCount) && IsStraight(cards, jokerCount);
	}

	/// <summary>
	/// Check for 4 of a Kind.
	/// NES: $8510
	/// </summary>
	private static bool IsFourOfAKind(Card[] cards, int jokerCount) {
		var counts = GetRankCounts(cards);
		var maxOfKind = counts.Max();

		return maxOfKind + jokerCount >= 4;
	}

	/// <summary>
	/// Check for Full House: 3 of a kind + pair.
	/// NES: $8522
	/// </summary>
	private static bool IsFullHouse(Card[] cards, int jokerCount) {
		var counts = GetRankCounts(cards);
		var nonZeroCounts = counts.Where(c => c > 0).OrderDescending().ToArray();

		if (nonZeroCounts.Length == 0) return false;

		// With jokers, check if we can make full house
		if (jokerCount == 0) {
			return nonZeroCounts.Length == 2 && nonZeroCounts[0] == 3 && nonZeroCounts[1] == 2;
		}

		// With 1+ jokers, we need at least a pair and another card
		if (nonZeroCounts.Length >= 2) {
			var remaining = jokerCount;
			var first = nonZeroCounts[0];
			var second = nonZeroCounts[1];

			// Try to make 3 + 2
			var needFor3 = Math.Max(0, 3 - first);
			var needFor2 = Math.Max(0, 2 - second);

			return needFor3 + needFor2 <= remaining;
		}

		return jokerCount >= 2; // 2+ jokers can complete many hands
	}

	/// <summary>
	/// Check for Flush: 5 cards of same suit.
	/// NES: $8532
	/// </summary>
	private static bool IsFlush(Card[] cards, int jokerCount) {
		if (cards.Length == 0) return jokerCount >= 5;

		var counts = GetSuitCounts(cards);
		var maxSuit = counts.Max();

		return maxSuit + jokerCount >= 5;
	}

	/// <summary>
	/// Check for Straight: 5 sequential cards.
	/// NES: $8544
	/// </summary>
	private static bool IsStraight(Card[] cards, int jokerCount) {
		if (cards.Length == 0) return jokerCount >= 5;

		var values = cards.Select(c => c.StraightValue).Distinct().OrderBy(v => v).ToArray();

		// Check all possible 5-card sequences (A-5, 2-6, ..., 10-A)
		for (int start = 1; start <= 10; start++) {
			var range = Enumerable.Range(start, 5).ToHashSet();
			// Special case: A can be high (10-J-Q-K-A)
			if (start == 10) {
				range = new HashSet<int> { 10, 11, 12, 13, 1 }; // 10, J, Q, K, A (where A=1 or 14)
			}

			var present = values.Count(v => range.Contains(v) || (v == 1 && start == 10));
			var needed = 5 - present;

			if (needed <= jokerCount) return true;
		}

		// Also check A-high straight (10-J-Q-K-A)
		var aceHigh = new HashSet<int> { 1, 10, 11, 12, 13 };
		var aceHighPresent = values.Count(v => aceHigh.Contains(v));
		if (5 - aceHighPresent <= jokerCount) return true;

		return false;
	}

	/// <summary>
	/// Check for 3 of a Kind.
	/// NES: $8564
	/// </summary>
	private static bool IsThreeOfAKind(Card[] cards, int jokerCount) {
		var counts = GetRankCounts(cards);
		var maxOfKind = counts.Max();

		return maxOfKind + jokerCount >= 3;
	}

	/// <summary>
	/// Check for Two Pairs.
	/// NES: $8572
	/// </summary>
	private static bool IsTwoPairs(Card[] cards, int jokerCount) {
		var counts = GetRankCounts(cards);
		var pairs = counts.Count(c => c >= 2);

		if (pairs >= 2) return true;
		if (pairs == 1 && jokerCount >= 1) return true; // Joker makes second pair

		return false;
	}

	/// <summary>
	/// Check for One Pair.
	/// NES: $860C
	/// </summary>
	private static bool IsOnePair(Card[] cards, int jokerCount) {
		if (jokerCount > 0) return true; // Joker pairs with anything

		var counts = GetRankCounts(cards);
		return counts.Any(c => c >= 2);
	}

	/// <summary>
	/// Gets a human-readable description of a hand.
	/// </summary>
	public static string GetDescription(PokerHandType hand) {
		return hand switch {
			PokerHandType.RoyalFlush => "Royal Flush",
			PokerHandType.FiveOfAKind => "5 of a Kind",
			PokerHandType.StraightFlush => "Straight Flush",
			PokerHandType.FourOfAKind => "4 of a Kind",
			PokerHandType.FullHouse => "Full House",
			PokerHandType.Flush => "Flush",
			PokerHandType.Straight => "Straight",
			PokerHandType.ThreeOfAKind => "3 of a Kind",
			PokerHandType.TwoPairs => "2 Pairs",
			PokerHandType.OnePair => "1 Pair",
			_ => "Nothing"
		};
	}
}
