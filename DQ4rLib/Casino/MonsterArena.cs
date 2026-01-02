namespace DQ4rLib.Casino;

/// <summary>
/// Represents an arena match with contending monster groups.
/// </summary>
public class ArenaMatch {
	/// <summary>Match ID.</summary>
	public int Id { get; init; }

	/// <summary>Contender groups.</summary>
	public required ArenaContender[] Contenders { get; init; }

	/// <summary>Current bet amounts per contender.</summary>
	public uint[] Bets { get; init; } = new uint[4];

	/// <summary>Winner index after fight (-1 if not fought).</summary>
	public int WinnerIndex { get; set; } = -1;
}

/// <summary>
/// Represents a contender group in the arena.
/// </summary>
public class ArenaContender {
	/// <summary>Display name.</summary>
	public required string Name { get; init; }

	/// <summary>Monster ID(s) in the group.</summary>
	public required ushort[] MonsterIds { get; init; }

	/// <summary>Number of monsters in the group.</summary>
	public int Count { get; init; } = 1;

	/// <summary>Betting odds (e.g., 2 for 2:1).</summary>
	public int Odds { get; init; } = 2;

	/// <summary>Power rating for determining odds.</summary>
	public int PowerRating { get; init; }
}

/// <summary>
/// Event arguments for arena events.
/// </summary>
public class ArenaFightStartedEventArgs : EventArgs {
	public required ArenaMatch Match { get; init; }
}

public class ArenaFightEndedEventArgs : EventArgs {
	public required ArenaMatch Match { get; init; }
	public required int WinnerIndex { get; init; }
	public required uint Payout { get; init; }
}

/// <summary>
/// Monster Arena betting game.
/// Based on DW4 NES Endor Colosseum.
/// </summary>
public class MonsterArena {
	private readonly BattleManager _battleManager;
	private readonly Random _rng;
	private ArenaMatch? _currentMatch;
	private int _selectedContender = -1;
	private uint _currentBet;

	/// <summary>Available bet amounts.</summary>
	public static readonly uint[] BetAmounts = [10, 50, 100, 500];

	/// <summary>Current match.</summary>
	public ArenaMatch? CurrentMatch => _currentMatch;

	/// <summary>Selected contender index.</summary>
	public int SelectedContender => _selectedContender;

	/// <summary>Current bet amount.</summary>
	public uint CurrentBet => _currentBet;

	/// <summary>Current arena state.</summary>
	public ArenaState State { get; private set; } = ArenaState.Idle;

	// Events
	public event EventHandler<ArenaFightStartedEventArgs>? FightStarted;
	public event EventHandler<ArenaFightEndedEventArgs>? FightEnded;

	/// <summary>
	/// Creates a new Monster Arena.
	/// </summary>
	/// <param name="battleManager">Battle manager for running fights.</param>
	/// <param name="rng">Random number generator.</param>
	public MonsterArena(BattleManager battleManager, Random? rng = null) {
		_battleManager = battleManager;
		_rng = rng ?? new Random();
	}

	/// <summary>
	/// Gets today's matches (random selection).
	/// </summary>
	/// <returns>Array of available matches.</returns>
	public ArenaMatch[] GetTodaysMatches() {
		// Generate 3-5 matches
		var matchCount = _rng.Next(3, 6);
		var matches = new ArenaMatch[matchCount];

		for (int i = 0; i < matchCount; i++) {
			matches[i] = GenerateMatch(i);
		}

		return matches;
	}

	/// <summary>
	/// Generates a random match.
	/// </summary>
	private ArenaMatch GenerateMatch(int id) {
		// Sample matchups (would load from data in full implementation)
		var contenderPool = new ArenaContender[][]
		{
			[
				new() { Name = "Slimes", MonsterIds = [0x01], Count = 4, PowerRating = 10 },
				new() { Name = "Drakees", MonsterIds = [0x02], Count = 3, PowerRating = 12 },
				new() { Name = "Babbles", MonsterIds = [0x03], Count = 2, PowerRating = 15 },
			],
			[
				new() { Name = "Metal Slime", MonsterIds = [0x10], Count = 1, PowerRating = 50 },
				new() { Name = "Healers", MonsterIds = [0x08], Count = 3, PowerRating = 40 },
				new() { Name = "Magicians", MonsterIds = [0x09], Count = 2, PowerRating = 45 },
			],
			[
				new() { Name = "Wyvern", MonsterIds = [0x20], Count = 1, PowerRating = 80 },
				new() { Name = "Lionhead", MonsterIds = [0x21], Count = 2, PowerRating = 75 },
				new() { Name = "Vampdog", MonsterIds = [0x22], Count = 3, PowerRating = 70 },
			],
		};

		// Pick a tier based on match ID
		var tier = Math.Min(id, contenderPool.Length - 1);
		var contenders = contenderPool[tier].ToArray();

		// Calculate odds based on power ratings
		var totalPower = contenders.Sum(c => c.PowerRating);
		for (int i = 0; i < contenders.Length; i++) {
			var winChance = (double)contenders[i].PowerRating / totalPower;
			var odds = Math.Max(2, (int)(1.0 / winChance));
			contenders[i] = new ArenaContender {
				Name = contenders[i].Name,
				MonsterIds = contenders[i].MonsterIds,
				Count = contenders[i].Count,
				PowerRating = contenders[i].PowerRating,
				Odds = odds
			};
		}

		return new ArenaMatch {
			Id = id,
			Contenders = contenders
		};
	}

	/// <summary>
	/// Selects a match for betting.
	/// </summary>
	public void SelectMatch(ArenaMatch match) {
		if (State != ArenaState.Idle) {
			throw new InvalidOperationException("Cannot select match during active game.");
		}

		_currentMatch = match;
		_selectedContender = -1;
		_currentBet = 0;
		State = ArenaState.MatchSelected;
	}

	/// <summary>
	/// Selects a contender to bet on.
	/// </summary>
	/// <param name="index">Contender index.</param>
	public void SelectContender(int index) {
		if (State != ArenaState.MatchSelected && State != ArenaState.ContenderSelected) {
			throw new InvalidOperationException("No match selected.");
		}

		if (_currentMatch == null || index < 0 || index >= _currentMatch.Contenders.Length) {
			throw new ArgumentOutOfRangeException(nameof(index));
		}

		_selectedContender = index;
		State = ArenaState.ContenderSelected;
	}

	/// <summary>
	/// Places a bet on the selected contender.
	/// </summary>
	/// <param name="betIndex">Index into BetAmounts array.</param>
	public void PlaceBet(int betIndex) {
		if (State != ArenaState.ContenderSelected) {
			throw new InvalidOperationException("No contender selected.");
		}

		if (betIndex < 0 || betIndex >= BetAmounts.Length) {
			throw new ArgumentOutOfRangeException(nameof(betIndex));
		}

		_currentBet = BetAmounts[betIndex];
		State = ArenaState.BetPlaced;
	}

	/// <summary>
	/// Starts the arena fight.
	/// </summary>
	/// <returns>Winner index.</returns>
	public int StartFight() {
		if (State != ArenaState.BetPlaced || _currentMatch == null) {
			throw new InvalidOperationException("Bet not placed.");
		}

		State = ArenaState.Fighting;
		FightStarted?.Invoke(this, new ArenaFightStartedEventArgs { Match = _currentMatch });

		// Simulate fight (simplified - would use BattleManager in full implementation)
		var winner = SimulateFight(_currentMatch);
		_currentMatch.WinnerIndex = winner;

		// Calculate payout
		uint payout = 0;
		if (winner == _selectedContender) {
			var odds = _currentMatch.Contenders[winner].Odds;
			payout = _currentBet * (uint)odds;
		}

		State = ArenaState.Ended;
		FightEnded?.Invoke(this, new ArenaFightEndedEventArgs {
			Match = _currentMatch,
			WinnerIndex = winner,
			Payout = payout
		});

		return winner;
	}

	/// <summary>
	/// Simulates a fight between contenders.
	/// </summary>
	private int SimulateFight(ArenaMatch match) {
		// Weighted random based on power ratings
		var totalPower = match.Contenders.Sum(c => c.PowerRating);
		var roll = _rng.Next(totalPower);

		var cumulative = 0;
		for (int i = 0; i < match.Contenders.Length; i++) {
			cumulative += match.Contenders[i].PowerRating;
			if (roll < cumulative) {
				return i;
			}
		}

		return match.Contenders.Length - 1;
	}

	/// <summary>
	/// Gets the payout if the selected contender wins.
	/// </summary>
	public uint GetPotentialPayout() {
		if (_currentMatch == null || _selectedContender < 0) {
			return 0;
		}

		var odds = _currentMatch.Contenders[_selectedContender].Odds;
		return _currentBet * (uint)odds;
	}

	/// <summary>
	/// Resets the arena to idle state.
	/// </summary>
	public void Reset() {
		_currentMatch = null;
		_selectedContender = -1;
		_currentBet = 0;
		State = ArenaState.Idle;
	}
}

/// <summary>
/// Monster Arena states.
/// </summary>
public enum ArenaState {
	/// <summary>No match selected.</summary>
	Idle,

	/// <summary>Match selected, choosing contender.</summary>
	MatchSelected,

	/// <summary>Contender selected, placing bet.</summary>
	ContenderSelected,

	/// <summary>Bet placed, ready to fight.</summary>
	BetPlaced,

	/// <summary>Fight in progress.</summary>
	Fighting,

	/// <summary>Fight ended.</summary>
	Ended
}
