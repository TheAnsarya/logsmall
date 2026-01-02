namespace DQ4rLib.Tests;

using DQ4rLib.Casino;

public class CasinoManagerTests {
	private CasinoManager CreateCasinoManager(uint initialCoins = 1000) {
		var chapterManager = new ChapterManager();
		var battleManager = new BattleManager(chapterManager);
		return new CasinoManager(chapterManager, battleManager, initialCoins);
	}

	[Fact]
	public void CasinoManager_InitializesWithCorrectCoins() {
		var casino = CreateCasinoManager(5000);
		Assert.Equal(5000u, casino.Coins);
	}

	[Fact]
	public void CasinoManager_CapsAtMaxCoins() {
		var casino = CreateCasinoManager(CasinoManager.MaxCoins + 1000);
		Assert.Equal(CasinoManager.MaxCoins, casino.Coins);
	}

	[Fact]
	public void AddCoins_IncreasesBalance() {
		var casino = CreateCasinoManager(1000);

		var added = casino.AddCoins(500);

		Assert.Equal(500u, added);
		Assert.Equal(1500u, casino.Coins);
	}

	[Fact]
	public void AddCoins_CapsAtMax() {
		var casino = CreateCasinoManager(CasinoManager.MaxCoins - 100);

		var added = casino.AddCoins(500);

		Assert.Equal(100u, added); // Only 100 could be added
		Assert.Equal(CasinoManager.MaxCoins, casino.Coins);
	}

	[Fact]
	public void SpendCoins_DecreasesBalance() {
		var casino = CreateCasinoManager(1000);

		var success = casino.SpendCoins(300);

		Assert.True(success);
		Assert.Equal(700u, casino.Coins);
	}

	[Fact]
	public void SpendCoins_FailsWithInsufficientFunds() {
		var casino = CreateCasinoManager(100);

		var success = casino.SpendCoins(500);

		Assert.False(success);
		Assert.Equal(100u, casino.Coins); // Unchanged
	}

	[Fact]
	public void ExchangeGoldForCoins_ConvertsProperly() {
		var casino = CreateCasinoManager(0);

		var coins = casino.ExchangeGoldForCoins(1000);

		Assert.Equal(50u, coins); // 1000 / 20 = 50
		Assert.Equal(50u, casino.Coins);
	}

	[Fact]
	public void CoinsChanged_FiresOnAdd() {
		var casino = CreateCasinoManager(100);
		var eventFired = false;
		CoinsChangedEventArgs? args = null;

		casino.CoinsChanged += (s, e) => {
			eventFired = true;
			args = e;
		};

		casino.AddCoins(50);

		Assert.True(eventFired);
		Assert.NotNull(args);
		Assert.Equal(100u, args.OldBalance);
		Assert.Equal(150u, args.NewBalance);
		Assert.Equal(50, args.Change);
	}

	[Fact]
	public void CoinsChanged_FiresOnSpend() {
		var casino = CreateCasinoManager(100);
		var eventFired = false;
		CoinsChangedEventArgs? args = null;

		casino.CoinsChanged += (s, e) => {
			eventFired = true;
			args = e;
		};

		casino.SpendCoins(30);

		Assert.True(eventFired);
		Assert.NotNull(args);
		Assert.Equal(100u, args.OldBalance);
		Assert.Equal(70u, args.NewBalance);
		Assert.Equal(-30, args.Change);
	}

	[Fact]
	public void PurchasePrize_SpendsCoinAndReturnsTrue() {
		var casino = CreateCasinoManager(1000);
		var prize = new CasinoPrize {
			ItemId = 0x90,
			Name = "Prayer Ring",
			Cost = 350
		};

		var success = casino.PurchasePrize(prize);

		Assert.True(success);
		Assert.Equal(650u, casino.Coins);
	}

	[Fact]
	public void PurchasePrize_FailsIfCantAfford() {
		var casino = CreateCasinoManager(100);
		var prize = new CasinoPrize {
			ItemId = 0x90,
			Name = "Prayer Ring",
			Cost = 350
		};

		var success = casino.PurchasePrize(prize);

		Assert.False(success);
		Assert.Equal(100u, casino.Coins); // Unchanged
	}

	[Fact]
	public void SaveToBytes_SerializesCorrectly() {
		var casino = CreateCasinoManager(0);
		casino.AddCoins(0x123456); // Test 24-bit value

		var (low, mid, high) = casino.SaveToBytes();

		Assert.Equal(0x56, low);
		Assert.Equal(0x34, mid);
		Assert.Equal(0x12, high);
	}

	[Fact]
	public void LoadFromSave_DeserializesCorrectly() {
		var casino = CreateCasinoManager(0);

		casino.LoadFromSave(0x56, 0x34, 0x12);

		Assert.Equal(0x123456u, casino.Coins);
	}

	[Fact]
	public void LoadSaveRoundTrip_PreservesCoins() {
		var casino1 = CreateCasinoManager(999_999);

		var (low, mid, high) = casino1.SaveToBytes();

		var casino2 = CreateCasinoManager(0);
		casino2.LoadFromSave(low, mid, high);

		Assert.Equal(casino1.Coins, casino2.Coins);
	}

	[Fact]
	public void Poker_AccessibleThroughManager() {
		var casino = CreateCasinoManager(1000);

		Assert.NotNull(casino.Poker);
		Assert.Equal(PokerState.Idle, casino.Poker.State);
	}

	[Fact]
	public void Arena_AccessibleThroughManager() {
		var casino = CreateCasinoManager(1000);

		Assert.NotNull(casino.Arena);
		Assert.Equal(ArenaState.Idle, casino.Arena.State);
	}
}

public class MonsterArenaTests {
	[Fact]
	public void GetTodaysMatches_ReturnsMatches() {
		var chapterManager = new ChapterManager();
		var battleManager = new BattleManager(chapterManager);
		var arena = new MonsterArena(battleManager);

		var matches = arena.GetTodaysMatches();

		Assert.NotEmpty(matches);
		Assert.All(matches, m => Assert.True(m.Contenders.Length >= 2));
	}

	[Fact]
	public void BetAmounts_ContainsExpectedValues() {
		uint[] expected = [10, 50, 100, 500];
		Assert.Equal(expected, MonsterArena.BetAmounts);
	}
}
