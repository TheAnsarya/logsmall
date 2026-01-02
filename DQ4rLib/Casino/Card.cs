namespace DQ4rLib.Casino;

/// <summary>
/// Represents a playing card rank (A-K + Joker).
/// </summary>
public enum Rank {
	Ace = 0,
	Two = 1,
	Three = 2,
	Four = 3,
	Five = 4,
	Six = 5,
	Seven = 6,
	Eight = 7,
	Nine = 8,
	Ten = 9,
	Jack = 10,
	Queen = 11,
	King = 12,
	Joker = 13
}

/// <summary>
/// Represents a playing card suit.
/// </summary>
public enum Suit {
	Spades = 0,
	Hearts = 1,
	Diamonds = 2,
	Clubs = 3
}

/// <summary>
/// Represents a playing card.
/// Based on NES encoding: bits 0-3 = rank, bits 4-5 = suit, bit 6 = joker flag.
/// </summary>
public readonly struct Card : IEquatable<Card> {
	/// <summary>The rank of the card.</summary>
	public Rank Rank { get; }

	/// <summary>The suit of the card.</summary>
	public Suit Suit { get; }

	/// <summary>Whether this card is a joker.</summary>
	public bool IsJoker => Rank == Rank.Joker;

	/// <summary>A joker card constant.</summary>
	public static readonly Card Joker = new(Rank.Joker, Suit.Spades);

	/// <summary>
	/// Creates a new card.
	/// </summary>
	public Card(Rank rank, Suit suit = Suit.Spades) {
		Rank = rank;
		Suit = IsJoker ? Suit.Spades : suit;
	}

	/// <summary>
	/// Creates a card from NES ROM encoding.
	/// </summary>
	/// <param name="value">NES byte value (bits 0-3: rank, 4-5: suit, 6: joker)</param>
	public static Card FromNesByte(byte value) {
		if ((value & 0x40) != 0) {
			return Joker;
		}

		var rank = (Rank)(value & 0x0F);
		var suit = (Suit)((value >> 4) & 0x03);
		return new Card(rank, suit);
	}

	/// <summary>
	/// Converts to NES ROM encoding.
	/// </summary>
	public byte ToNesByte() {
		if (IsJoker) {
			return 0x40;
		}

		return (byte)(((int)Suit << 4) | (int)Rank);
	}

	/// <summary>
	/// Gets the card value for comparison (1-13, Ace high).
	/// </summary>
	public int Value => IsJoker ? 0 : (Rank == Rank.Ace ? 14 : (int)Rank + 1);

	/// <summary>
	/// Gets the card value for straights (1-13, Ace can be 1 or 14).
	/// </summary>
	public int StraightValue => IsJoker ? 0 : ((int)Rank + 1);

	public override string ToString() {
		if (IsJoker) return "Joker";

		var rankStr = Rank switch {
			Rank.Ace => "A",
			Rank.Jack => "J",
			Rank.Queen => "Q",
			Rank.King => "K",
			_ => ((int)Rank + 1).ToString()
		};

		var suitStr = Suit switch {
			Suit.Spades => "♠",
			Suit.Hearts => "♥",
			Suit.Diamonds => "♦",
			Suit.Clubs => "♣",
			_ => "?"
		};

		return $"{rankStr}{suitStr}";
	}

	public bool Equals(Card other) => Rank == other.Rank && Suit == other.Suit;
	public override bool Equals(object? obj) => obj is Card card && Equals(card);
	public override int GetHashCode() => HashCode.Combine(Rank, Suit);

	public static bool operator ==(Card left, Card right) => left.Equals(right);
	public static bool operator !=(Card left, Card right) => !left.Equals(right);
}
