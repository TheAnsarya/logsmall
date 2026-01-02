namespace DQ4rLib.Casino;

/// <summary>
/// A standard 52-card deck with optional Jokers.
/// </summary>
public class CardDeck {
	private readonly List<Card> _cards = [];
	private readonly bool _includeJokers;
	private readonly Random _rng;
	private int _position;

	/// <summary>Number of cards remaining in the deck.</summary>
	public int Remaining => _cards.Count - _position;

	/// <summary>Whether the deck is empty.</summary>
	public bool IsEmpty => _position >= _cards.Count;

	/// <summary>
	/// Creates a new deck.
	/// </summary>
	/// <param name="includeJokers">Whether to include Joker cards (default true for DW4).</param>
	/// <param name="jokerCount">Number of Jokers to include (default 2).</param>
	/// <param name="rng">Random number generator (optional, uses new Random if null).</param>
	public CardDeck(bool includeJokers = true, int jokerCount = 2, Random? rng = null) {
		_includeJokers = includeJokers;
		_rng = rng ?? new Random();
		Reset(jokerCount);
	}

	/// <summary>
	/// Resets and rebuilds the deck.
	/// </summary>
	/// <param name="jokerCount">Number of Jokers to include.</param>
	public void Reset(int jokerCount = 2) {
		_cards.Clear();
		_position = 0;

		// Add standard 52 cards
		foreach (Suit suit in Enum.GetValues<Suit>()) {
			foreach (Rank rank in Enum.GetValues<Rank>()) {
				if (rank == Rank.Joker) continue;
				_cards.Add(new Card(rank, suit));
			}
		}

		// Add Jokers
		if (_includeJokers) {
			for (int i = 0; i < jokerCount; i++) {
				_cards.Add(Card.Joker);
			}
		}
	}

	/// <summary>
	/// Shuffles the deck using Fisher-Yates algorithm.
	/// </summary>
	public void Shuffle() {
		_position = 0;

		for (int i = _cards.Count - 1; i > 0; i--) {
			int j = _rng.Next(i + 1);
			(_cards[i], _cards[j]) = (_cards[j], _cards[i]);
		}
	}

	/// <summary>
	/// Draws the next card from the deck.
	/// </summary>
	/// <returns>The drawn card.</returns>
	/// <exception cref="InvalidOperationException">Deck is empty.</exception>
	public Card Draw() {
		if (IsEmpty) {
			throw new InvalidOperationException("Deck is empty.");
		}

		return _cards[_position++];
	}

	/// <summary>
	/// Draws multiple cards from the deck.
	/// </summary>
	/// <param name="count">Number of cards to draw.</param>
	/// <returns>Array of drawn cards.</returns>
	public Card[] Draw(int count) {
		var cards = new Card[count];
		for (int i = 0; i < count; i++) {
			cards[i] = Draw();
		}
		return cards;
	}

	/// <summary>
	/// Peeks at the next card without drawing it.
	/// </summary>
	/// <returns>The next card, or null if empty.</returns>
	public Card? Peek() {
		if (IsEmpty) return null;
		return _cards[_position];
	}

	/// <summary>
	/// Returns a card to the bottom of the deck.
	/// </summary>
	/// <param name="card">Card to return.</param>
	public void Return(Card card) {
		_cards.Add(card);
	}
}
