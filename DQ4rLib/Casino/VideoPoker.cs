namespace DQ4rLib.Casino;

/// <summary>
/// Event arguments for poker game events.
/// </summary>
public class HandDealtEventArgs : EventArgs {
	public required Card[] Hand { get; init; }
}

public class HandEvaluatedEventArgs : EventArgs {
	public required Card[] Hand { get; init; }
	public required PokerHandType HandType { get; init; }
	public required uint Payout { get; init; }
}

public class DoubleUpEventArgs : EventArgs {
	public required Card DealerCard { get; init; }
	public required Card PlayerCard { get; init; }
	public required bool GuessedHigher { get; init; }
	public required bool Won { get; init; }
	public required uint NewWinnings { get; init; }
}

/// <summary>
/// Video Poker game implementation.
/// Port of DW4 NES Bank 23 poker game ($8052-$80AE main loop).
/// </summary>
public class VideoPoker {
	private readonly CardDeck _deck;
	private readonly Random _rng;
	private Card[] _hand = new Card[5];
	private bool[] _held = new bool[5];
	private PokerState _state = PokerState.Idle;

	/// <summary>Current bet amount in coins.</summary>
	public uint CurrentBet { get; private set; }

	/// <summary>Current winnings (for double up).</summary>
	public uint CurrentWinnings { get; private set; }

	/// <summary>Last evaluated hand type.</summary>
	public PokerHandType LastHandType { get; private set; }

	/// <summary>Current hand of cards.</summary>
	public IReadOnlyList<Card> Hand => _hand;

	/// <summary>Current hold states.</summary>
	public IReadOnlyList<bool> Held => _held;

	/// <summary>Current game state.</summary>
	public PokerState State => _state;

	/// <summary>Card shown by dealer in double up (when applicable).</summary>
	public Card? DoubleUpDealerCard { get; private set; }

	// Events
	public event EventHandler<HandDealtEventArgs>? HandDealt;
	public event EventHandler<HandEvaluatedEventArgs>? HandEvaluated;
	public event EventHandler<DoubleUpEventArgs>? DoubleUpResult;

	/// <summary>
	/// Creates a new Video Poker game.
	/// </summary>
	/// <param name="rng">Random number generator (optional).</param>
	public VideoPoker(Random? rng = null) {
		_rng = rng ?? new Random();
		_deck = new CardDeck(includeJokers: true, jokerCount: 2, rng: _rng);
	}

	/// <summary>
	/// Places a bet and starts a new round.
	/// </summary>
	/// <param name="bet">Bet amount (1-10 coins).</param>
	/// <exception cref="InvalidOperationException">Game not in idle state.</exception>
	/// <exception cref="ArgumentOutOfRangeException">Bet not in valid range.</exception>
	public void PlaceBet(uint bet) {
		if (_state != PokerState.Idle) {
			throw new InvalidOperationException("Cannot place bet during active game.");
		}

		if (bet < 1 || bet > 10) {
			throw new ArgumentOutOfRangeException(nameof(bet), "Bet must be between 1 and 10 coins.");
		}

		CurrentBet = bet;
		CurrentWinnings = 0;
		LastHandType = PokerHandType.Nothing;
		_state = PokerState.BetPlaced;
	}

	/// <summary>
	/// Deals the initial 5 cards.
	/// </summary>
	/// <exception cref="InvalidOperationException">Bet not placed.</exception>
	public void Deal() {
		if (_state != PokerState.BetPlaced) {
			throw new InvalidOperationException("Must place bet before dealing.");
		}

		// Reset deck and shuffle
		_deck.Reset();
		_deck.Shuffle();

		// Deal 5 cards
		_hand = _deck.Draw(5);
		_held = new bool[5];

		_state = PokerState.Dealt;

		HandDealt?.Invoke(this, new HandDealtEventArgs { Hand = [.. _hand] });
	}

	/// <summary>
	/// Toggles hold state for a card.
	/// </summary>
	/// <param name="index">Card index (0-4).</param>
	public void ToggleHold(int index) {
		if (_state != PokerState.Dealt) {
			throw new InvalidOperationException("Cannot hold cards in current state.");
		}

		if (index < 0 || index > 4) {
			throw new ArgumentOutOfRangeException(nameof(index), "Card index must be 0-4.");
		}

		_held[index] = !_held[index];
	}

	/// <summary>
	/// Sets hold state for a card.
	/// </summary>
	/// <param name="index">Card index (0-4).</param>
	/// <param name="held">Whether to hold the card.</param>
	public void SetHold(int index, bool held) {
		if (_state != PokerState.Dealt) {
			throw new InvalidOperationException("Cannot hold cards in current state.");
		}

		if (index < 0 || index > 4) {
			throw new ArgumentOutOfRangeException(nameof(index), "Card index must be 0-4.");
		}

		_held[index] = held;
	}

	/// <summary>
	/// Draws replacement cards for non-held positions.
	/// </summary>
	public void Draw() {
		if (_state != PokerState.Dealt) {
			throw new InvalidOperationException("Must deal before drawing.");
		}

		// Replace non-held cards
		for (int i = 0; i < 5; i++) {
			if (!_held[i]) {
				_hand[i] = _deck.Draw();
			}
		}

		// Evaluate hand
		LastHandType = PokerHand.Evaluate(_hand);
		CurrentWinnings = PokerPayouts.CalculatePayout(LastHandType, CurrentBet);

		if (CurrentWinnings > 0) {
			_state = PokerState.Won;
		} else {
			_state = PokerState.Lost;
		}

		HandEvaluated?.Invoke(this, new HandEvaluatedEventArgs {
			Hand = [.. _hand],
			HandType = LastHandType,
			Payout = CurrentWinnings
		});
	}

	/// <summary>
	/// Starts the double up mini-game.
	/// </summary>
	/// <exception cref="InvalidOperationException">No winnings to double.</exception>
	public void StartDoubleUp() {
		if (_state != PokerState.Won) {
			throw new InvalidOperationException("Can only double up after winning.");
		}

		// Generate dealer's card (matches NES RNG at $8654)
		_deck.Reset();
		_deck.Shuffle();
		DoubleUpDealerCard = _deck.Draw();

		_state = PokerState.DoubleUp;
	}

	/// <summary>
	/// Guesses higher in double up.
	/// </summary>
	/// <returns>True if won, false if lost.</returns>
	public bool GuessHigher() => DoDoubleUpGuess(higher: true);

	/// <summary>
	/// Guesses lower in double up.
	/// </summary>
	/// <returns>True if won, false if lost.</returns>
	public bool GuessLower() => DoDoubleUpGuess(higher: false);

	private bool DoDoubleUpGuess(bool higher) {
		if (_state != PokerState.DoubleUp || DoubleUpDealerCard == null) {
			throw new InvalidOperationException("Not in double up state.");
		}

		// Draw player's card
		var playerCard = _deck.Draw();

		// Compare values
		var dealerValue = DoubleUpDealerCard.Value.Value;
		var playerValue = playerCard.Value;

		bool won;
		if (playerValue == dealerValue) {
			// Tie - push (keep current winnings)
			won = true;
		} else if (higher) {
			won = playerValue > dealerValue;
		} else {
			won = playerValue < dealerValue;
		}

		if (won) {
			CurrentWinnings *= 2;
			_state = PokerState.Won; // Can double up again
		} else {
			CurrentWinnings = 0;
			_state = PokerState.Lost;
		}

		DoubleUpResult?.Invoke(this, new DoubleUpEventArgs {
			DealerCard = DoubleUpDealerCard.Value,
			PlayerCard = playerCard,
			GuessedHigher = higher,
			Won = won,
			NewWinnings = CurrentWinnings
		});

		DoubleUpDealerCard = null;
		return won;
	}

	/// <summary>
	/// Collects winnings and ends the round.
	/// </summary>
	/// <returns>Total winnings collected.</returns>
	public uint CollectWinnings() {
		if (_state != PokerState.Won && _state != PokerState.Lost) {
			throw new InvalidOperationException("Round not complete.");
		}

		var winnings = CurrentWinnings;
		CurrentWinnings = 0;
		CurrentBet = 0;
		LastHandType = PokerHandType.Nothing;
		_state = PokerState.Idle;

		return winnings;
	}

	/// <summary>
	/// Resets the game to idle state.
	/// </summary>
	public void Reset() {
		CurrentBet = 0;
		CurrentWinnings = 0;
		LastHandType = PokerHandType.Nothing;
		DoubleUpDealerCard = null;
		_hand = new Card[5];
		_held = new bool[5];
		_state = PokerState.Idle;
	}
}

/// <summary>
/// Video Poker game states.
/// </summary>
public enum PokerState {
	/// <summary>Waiting for bet.</summary>
	Idle,

	/// <summary>Bet placed, ready to deal.</summary>
	BetPlaced,

	/// <summary>Cards dealt, selecting holds.</summary>
	Dealt,

	/// <summary>Won, can collect or double up.</summary>
	Won,

	/// <summary>Lost, collect to continue.</summary>
	Lost,

	/// <summary>In double up mini-game.</summary>
	DoubleUp
}
