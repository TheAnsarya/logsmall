namespace DQ4rLib.Tests;

using DQ4rLib.Casino;

public class PokerHandTests {
	[Fact]
	public void Evaluate_RoyalFlush_ReturnsRoyalFlush() {
		var cards = new Card[] {
			new(Rank.Ace, Suit.Spades),
			new(Rank.King, Suit.Spades),
			new(Rank.Queen, Suit.Spades),
			new(Rank.Jack, Suit.Spades),
			new(Rank.Ten, Suit.Spades)
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.RoyalFlush, result);
	}

	[Fact]
	public void Evaluate_FiveOfAKind_WithJoker() {
		var cards = new Card[] {
			new(Rank.Ace, Suit.Spades),
			new(Rank.Ace, Suit.Hearts),
			new(Rank.Ace, Suit.Diamonds),
			new(Rank.Ace, Suit.Clubs),
			Card.Joker
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.FiveOfAKind, result);
	}

	[Fact]
	public void Evaluate_StraightFlush_ReturnsStraightFlush() {
		var cards = new Card[] {
			new(Rank.Nine, Suit.Hearts),
			new(Rank.Eight, Suit.Hearts),
			new(Rank.Seven, Suit.Hearts),
			new(Rank.Six, Suit.Hearts),
			new(Rank.Five, Suit.Hearts)
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.StraightFlush, result);
	}

	[Fact]
	public void Evaluate_FourOfAKind_ReturnsFourOfAKind() {
		var cards = new Card[] {
			new(Rank.King, Suit.Spades),
			new(Rank.King, Suit.Hearts),
			new(Rank.King, Suit.Diamonds),
			new(Rank.King, Suit.Clubs),
			new(Rank.Two, Suit.Spades)
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.FourOfAKind, result);
	}

	[Fact]
	public void Evaluate_FullHouse_ReturnsFullHouse() {
		var cards = new Card[] {
			new(Rank.Queen, Suit.Spades),
			new(Rank.Queen, Suit.Hearts),
			new(Rank.Queen, Suit.Diamonds),
			new(Rank.Jack, Suit.Clubs),
			new(Rank.Jack, Suit.Spades)
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.FullHouse, result);
	}

	[Fact]
	public void Evaluate_Flush_ReturnsFlush() {
		var cards = new Card[] {
			new(Rank.Ace, Suit.Diamonds),
			new(Rank.Ten, Suit.Diamonds),
			new(Rank.Seven, Suit.Diamonds),
			new(Rank.Four, Suit.Diamonds),
			new(Rank.Two, Suit.Diamonds)
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.Flush, result);
	}

	[Fact]
	public void Evaluate_Straight_ReturnsStraight() {
		var cards = new Card[] {
			new(Rank.Nine, Suit.Spades),
			new(Rank.Eight, Suit.Hearts),
			new(Rank.Seven, Suit.Diamonds),
			new(Rank.Six, Suit.Clubs),
			new(Rank.Five, Suit.Spades)
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.Straight, result);
	}

	[Fact]
	public void Evaluate_AceHighStraight_ReturnsStraight() {
		var cards = new Card[] {
			new(Rank.Ace, Suit.Spades),
			new(Rank.King, Suit.Hearts),
			new(Rank.Queen, Suit.Diamonds),
			new(Rank.Jack, Suit.Clubs),
			new(Rank.Ten, Suit.Hearts)
		};

		var result = PokerHand.Evaluate(cards);
		// Ace-high straight of mixed suits is just a Straight, not Royal Flush
		Assert.Equal(PokerHandType.Straight, result);
	}

	[Fact]
	public void Evaluate_ThreeOfAKind_ReturnsThreeOfAKind() {
		var cards = new Card[] {
			new(Rank.Seven, Suit.Spades),
			new(Rank.Seven, Suit.Hearts),
			new(Rank.Seven, Suit.Diamonds),
			new(Rank.King, Suit.Clubs),
			new(Rank.Two, Suit.Spades)
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.ThreeOfAKind, result);
	}

	[Fact]
	public void Evaluate_TwoPairs_ReturnsTwoPairs() {
		var cards = new Card[] {
			new(Rank.Jack, Suit.Spades),
			new(Rank.Jack, Suit.Hearts),
			new(Rank.Three, Suit.Diamonds),
			new(Rank.Three, Suit.Clubs),
			new(Rank.King, Suit.Spades)
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.TwoPairs, result);
	}

	[Fact]
	public void Evaluate_OnePair_ReturnsOnePair() {
		var cards = new Card[] {
			new(Rank.Ten, Suit.Spades),
			new(Rank.Ten, Suit.Hearts),
			new(Rank.Ace, Suit.Diamonds),
			new(Rank.King, Suit.Clubs),
			new(Rank.Two, Suit.Spades)
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.OnePair, result);
	}

	[Fact]
	public void Evaluate_Nothing_ReturnsNothing() {
		var cards = new Card[] {
			new(Rank.Ace, Suit.Spades),
			new(Rank.Ten, Suit.Hearts),
			new(Rank.Seven, Suit.Diamonds),
			new(Rank.Four, Suit.Clubs),
			new(Rank.Two, Suit.Hearts)
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.Nothing, result);
	}

	[Fact]
	public void Evaluate_JokerMakesPair_ReturnsOnePair() {
		var cards = new Card[] {
			Card.Joker,
			new(Rank.Ace, Suit.Spades),
			new(Rank.King, Suit.Hearts),
			new(Rank.Seven, Suit.Diamonds),
			new(Rank.Two, Suit.Clubs)
		};

		var result = PokerHand.Evaluate(cards);
		Assert.Equal(PokerHandType.OnePair, result);
	}

	[Fact]
	public void Evaluate_ThrowsWithWrongCardCount() {
		var cards = new Card[] {
			new(Rank.Ace, Suit.Spades),
			new(Rank.King, Suit.Hearts),
			new(Rank.Queen, Suit.Diamonds)
		};

		Assert.Throws<ArgumentException>(() => PokerHand.Evaluate(cards));
	}

	[Fact]
	public void GetDescription_ReturnsCorrectStrings() {
		Assert.Equal("Royal Flush", PokerHand.GetDescription(PokerHandType.RoyalFlush));
		Assert.Equal("5 of a Kind", PokerHand.GetDescription(PokerHandType.FiveOfAKind));
		Assert.Equal("Full House", PokerHand.GetDescription(PokerHandType.FullHouse));
		Assert.Equal("Nothing", PokerHand.GetDescription(PokerHandType.Nothing));
	}
}

public class PokerPayoutsTests {
	[Theory]
	[InlineData(PokerHandType.Nothing, 10u, 0u)]
	[InlineData(PokerHandType.OnePair, 10u, 10u)]
	[InlineData(PokerHandType.TwoPairs, 10u, 20u)]
	[InlineData(PokerHandType.ThreeOfAKind, 10u, 30u)]
	[InlineData(PokerHandType.Straight, 10u, 50u)]
	[InlineData(PokerHandType.Flush, 10u, 100u)]
	[InlineData(PokerHandType.FullHouse, 10u, 200u)]
	[InlineData(PokerHandType.FourOfAKind, 10u, 500u)]
	[InlineData(PokerHandType.StraightFlush, 10u, 1000u)]
	[InlineData(PokerHandType.FiveOfAKind, 10u, 2500u)]
	[InlineData(PokerHandType.RoyalFlush, 10u, 5000u)]
	public void CalculatePayout_ReturnsCorrectAmount(PokerHandType hand, uint bet, uint expected) {
		var payout = PokerPayouts.CalculatePayout(hand, bet);
		Assert.Equal(expected, payout);
	}
}
