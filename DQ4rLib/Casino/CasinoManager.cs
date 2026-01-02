namespace DQ4rLib.Casino;

/// <summary>
/// Main casino manager that coordinates all casino games.
/// Integrates with SaveManager for coin persistence.
/// </summary>
public class CasinoManager {
	private readonly ChapterManager _chapterManager;
	private readonly BattleManager _battleManager;
	private readonly Random _rng;

	/// <summary>Video Poker game instance.</summary>
	public VideoPoker Poker { get; }

	/// <summary>Monster Arena game instance.</summary>
	public MonsterArena Arena { get; }

	/// <summary>Current coin balance.</summary>
	public uint Coins { get; private set; }

	/// <summary>Maximum coins (24-bit limit from NES).</summary>
	public const uint MaxCoins = 16_777_215;

	/// <summary>Gold to coin exchange rate.</summary>
	public const uint GoldPerCoin = 20;

	/// <summary>
	/// Whether casino is accessible (Chapter 2+).
	/// </summary>
	public bool CanAccess => _chapterManager.CurrentChapter?.Number >= 2;

	// Events
	public event EventHandler<CoinsChangedEventArgs>? CoinsChanged;

	/// <summary>
	/// Creates a new casino manager.
	/// </summary>
	/// <param name="chapterManager">Chapter manager for access checks.</param>
	/// <param name="battleManager">Battle manager for arena fights.</param>
	/// <param name="initialCoins">Starting coin balance.</param>
	/// <param name="rng">Random number generator (optional).</param>
	public CasinoManager(
		ChapterManager chapterManager,
		BattleManager battleManager,
		uint initialCoins = 0,
		Random? rng = null) {
		_chapterManager = chapterManager;
		_battleManager = battleManager;
		_rng = rng ?? new Random();

		Coins = Math.Min(initialCoins, MaxCoins);

		Poker = new VideoPoker(_rng);
		Arena = new MonsterArena(_battleManager, _rng);

		// Hook up poker winnings
		Poker.HandEvaluated += OnPokerHandEvaluated;

		// Hook up arena payouts
		Arena.FightEnded += OnArenaFightEnded;
	}

	/// <summary>
	/// Adds coins to the balance.
	/// </summary>
	/// <param name="amount">Coins to add.</param>
	/// <returns>Actual amount added (may be less if at max).</returns>
	public uint AddCoins(uint amount) {
		var oldCoins = Coins;
		var newCoins = Math.Min(Coins + amount, MaxCoins);
		var added = newCoins - oldCoins;

		Coins = newCoins;

		if (added > 0) {
			CoinsChanged?.Invoke(this, new CoinsChangedEventArgs {
				OldBalance = oldCoins,
				NewBalance = newCoins,
				Change = (int)added
			});
		}

		return added;
	}

	/// <summary>
	/// Spends coins from the balance.
	/// </summary>
	/// <param name="amount">Coins to spend.</param>
	/// <returns>True if successful, false if insufficient coins.</returns>
	public bool SpendCoins(uint amount) {
		if (amount > Coins) {
			return false;
		}

		var oldCoins = Coins;
		Coins -= amount;

		CoinsChanged?.Invoke(this, new CoinsChangedEventArgs {
			OldBalance = oldCoins,
			NewBalance = Coins,
			Change = -(int)amount
		});

		return true;
	}

	/// <summary>
	/// Exchanges gold for coins.
	/// </summary>
	/// <param name="gold">Gold to exchange.</param>
	/// <returns>Coins received.</returns>
	public uint ExchangeGoldForCoins(uint gold) {
		var coins = gold / GoldPerCoin;
		AddCoins(coins);
		return coins;
	}

	/// <summary>
	/// Purchases a prize.
	/// </summary>
	/// <param name="prize">Prize to purchase.</param>
	/// <returns>True if successful.</returns>
	public bool PurchasePrize(CasinoPrize prize) {
		if (!CasinoPrizes.CanAfford(prize, Coins)) {
			return false;
		}

		SpendCoins(prize.Cost);
		// Item would be added to inventory by caller
		return true;
	}

	/// <summary>
	/// Starts a poker game with a bet.
	/// </summary>
	/// <param name="bet">Bet amount (1-10 coins).</param>
	/// <returns>True if bet was placed successfully.</returns>
	public bool StartPoker(uint bet) {
		if (!CanAccess) return false;
		if (bet > Coins) return false;

		SpendCoins(bet);
		Poker.PlaceBet(bet);
		Poker.Deal();
		return true;
	}

	/// <summary>
	/// Starts an arena bet.
	/// </summary>
	/// <param name="match">Match to bet on.</param>
	/// <param name="contenderIndex">Contender to bet on.</param>
	/// <param name="betIndex">Bet amount index.</param>
	/// <returns>True if bet was placed successfully.</returns>
	public bool StartArenaBet(ArenaMatch match, int contenderIndex, int betIndex) {
		if (!CanAccess) return false;

		var betAmount = MonsterArena.BetAmounts[betIndex];
		if (betAmount > Coins) return false;

		SpendCoins(betAmount);
		Arena.SelectMatch(match);
		Arena.SelectContender(contenderIndex);
		Arena.PlaceBet(betIndex);
		return true;
	}

	/// <summary>
	/// Loads coin balance from save data.
	/// </summary>
	/// <param name="coinLow">Low byte ($62AD).</param>
	/// <param name="coinMid">Middle byte ($62AE).</param>
	/// <param name="coinHigh">High byte ($62AF).</param>
	public void LoadFromSave(byte coinLow, byte coinMid, byte coinHigh) {
		Coins = (uint)(coinLow | (coinMid << 8) | (coinHigh << 16));
	}

	/// <summary>
	/// Saves coin balance to bytes.
	/// </summary>
	/// <returns>Tuple of (low, mid, high) bytes.</returns>
	public (byte Low, byte Mid, byte High) SaveToBytes() {
		return (
			(byte)(Coins & 0xFF),
			(byte)((Coins >> 8) & 0xFF),
			(byte)((Coins >> 16) & 0xFF)
		);
	}

	private void OnPokerHandEvaluated(object? sender, HandEvaluatedEventArgs e) {
		if (e.Payout > 0) {
			// Don't add yet - player may want to double up
		}
	}

	private void OnArenaFightEnded(object? sender, ArenaFightEndedEventArgs e) {
		if (e.Payout > 0) {
			AddCoins(e.Payout);
		}
	}

	/// <summary>
	/// Collects poker winnings after player chooses not to double up.
	/// </summary>
	/// <returns>Coins collected.</returns>
	public uint CollectPokerWinnings() {
		var winnings = Poker.CollectWinnings();
		if (winnings > 0) {
			AddCoins(winnings);
		}
		return winnings;
	}
}

/// <summary>
/// Event arguments for coin balance changes.
/// </summary>
public class CoinsChangedEventArgs : EventArgs {
	public uint OldBalance { get; init; }
	public uint NewBalance { get; init; }
	public int Change { get; init; }
}
