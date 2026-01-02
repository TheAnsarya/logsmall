namespace DQ4rLib.Tests;

using DQ4rLib.Casino;

public class CardTests {
	[Fact]
	public void Card_ToString_ReturnsCorrectFormat() {
		var aceOfSpades = new Card(Rank.Ace, Suit.Spades);
		Assert.Equal("A♠", aceOfSpades.ToString());

		var kingOfHearts = new Card(Rank.King, Suit.Hearts);
		Assert.Equal("K♥", kingOfHearts.ToString());

		var tenOfDiamonds = new Card(Rank.Ten, Suit.Diamonds);
		Assert.Equal("10♦", tenOfDiamonds.ToString());
	}

	[Fact]
	public void Card_Joker_IsIdentified() {
		var joker = Card.Joker;
		Assert.True(joker.IsJoker);
		Assert.Equal("Joker", joker.ToString());
	}

	[Fact]
	public void Card_FromNesByte_DecodesCorrectly() {
		// Ace of Spades = 0x00
		var ace = Card.FromNesByte(0x00);
		Assert.Equal(Rank.Ace, ace.Rank);
		Assert.Equal(Suit.Spades, ace.Suit);

		// King of Hearts = 0x1C (suit 1 = Hearts, rank 12 = King)
		var king = Card.FromNesByte(0x1C);
		Assert.Equal(Rank.King, king.Rank);
		Assert.Equal(Suit.Hearts, king.Suit);

		// Joker = 0x40
		var joker = Card.FromNesByte(0x40);
		Assert.True(joker.IsJoker);
	}

	[Fact]
	public void Card_ToNesByte_EncodesCorrectly() {
		var ace = new Card(Rank.Ace, Suit.Spades);
		Assert.Equal(0x00, ace.ToNesByte());

		var king = new Card(Rank.King, Suit.Hearts);
		Assert.Equal(0x1C, king.ToNesByte());

		var joker = Card.Joker;
		Assert.Equal(0x40, joker.ToNesByte());
	}

	[Fact]
	public void Card_Value_ReturnsCorrectValue() {
		var ace = new Card(Rank.Ace, Suit.Spades);
		Assert.Equal(14, ace.Value); // Ace high

		var king = new Card(Rank.King, Suit.Hearts);
		Assert.Equal(13, king.Value);

		var two = new Card(Rank.Two, Suit.Clubs);
		Assert.Equal(2, two.Value);
	}

	[Fact]
	public void Card_Equality_Works() {
		var card1 = new Card(Rank.Ace, Suit.Spades);
		var card2 = new Card(Rank.Ace, Suit.Spades);
		var card3 = new Card(Rank.Ace, Suit.Hearts);

		Assert.Equal(card1, card2);
		Assert.NotEqual(card1, card3);
		Assert.True(card1 == card2);
		Assert.True(card1 != card3);
	}
}

public class CardDeckTests {
	[Fact]
	public void CardDeck_Reset_Has54Cards() {
		var deck = new CardDeck(includeJokers: true, jokerCount: 2);
		Assert.Equal(54, deck.Remaining);
	}

	[Fact]
	public void CardDeck_NoJokers_Has52Cards() {
		var deck = new CardDeck(includeJokers: false);
		Assert.Equal(52, deck.Remaining);
	}

	[Fact]
	public void CardDeck_Draw_ReducesRemaining() {
		var deck = new CardDeck();
		var initial = deck.Remaining;

		deck.Draw();
		Assert.Equal(initial - 1, deck.Remaining);

		deck.Draw(5);
		Assert.Equal(initial - 6, deck.Remaining);
	}

	[Fact]
	public void CardDeck_Shuffle_PreservesCardCount() {
		var deck = new CardDeck();
		var initial = deck.Remaining;

		deck.Shuffle();
		Assert.Equal(initial, deck.Remaining);
	}

	[Fact]
	public void CardDeck_Draw_ThrowsWhenEmpty() {
		var deck = new CardDeck(includeJokers: false);

		// Draw all cards
		for (int i = 0; i < 52; i++) {
			deck.Draw();
		}

		Assert.True(deck.IsEmpty);
		Assert.Throws<InvalidOperationException>(() => deck.Draw());
	}
}
