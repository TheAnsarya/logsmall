namespace DQ4rLib.Tests;

using DQ4rLib.Casino;

public class VideoPokerTests {
	[Fact]
	public void VideoPoker_InitialState_IsIdle() {
		var poker = new VideoPoker();
		Assert.Equal(PokerState.Idle, poker.State);
		Assert.Equal(0u, poker.CurrentBet);
		Assert.Equal(0u, poker.CurrentWinnings);
	}

	[Fact]
	public void VideoPoker_PlaceBet_ChangesBetAndState() {
		var poker = new VideoPoker();
		poker.PlaceBet(5);

		Assert.Equal(5u, poker.CurrentBet);
		Assert.Equal(PokerState.BetPlaced, poker.State);
	}

	[Fact]
	public void VideoPoker_PlaceBet_ThrowsForInvalidAmount() {
		var poker = new VideoPoker();

		Assert.Throws<ArgumentOutOfRangeException>(() => poker.PlaceBet(0));
		Assert.Throws<ArgumentOutOfRangeException>(() => poker.PlaceBet(11));
	}

	[Fact]
	public void VideoPoker_Deal_Deals5Cards() {
		var poker = new VideoPoker();
		poker.PlaceBet(1);
		poker.Deal();

		Assert.Equal(5, poker.Hand.Count);
		Assert.Equal(PokerState.Dealt, poker.State);
	}

	[Fact]
	public void VideoPoker_Deal_ThrowsWithoutBet() {
		var poker = new VideoPoker();
		Assert.Throws<InvalidOperationException>(() => poker.Deal());
	}

	[Fact]
	public void VideoPoker_ToggleHold_ChangesHoldState() {
		var poker = new VideoPoker();
		poker.PlaceBet(1);
		poker.Deal();

		Assert.False(poker.Held[0]);
		poker.ToggleHold(0);
		Assert.True(poker.Held[0]);
		poker.ToggleHold(0);
		Assert.False(poker.Held[0]);
	}

	[Fact]
	public void VideoPoker_Draw_EvaluatesHand() {
		var poker = new VideoPoker();
		poker.PlaceBet(1);
		poker.Deal();

		// Hold all cards for consistent result
		for (int i = 0; i < 5; i++) {
			poker.SetHold(i, true);
		}

		poker.Draw();

		// State should be Won or Lost
		Assert.True(poker.State == PokerState.Won || poker.State == PokerState.Lost);
	}

	[Fact]
	public void VideoPoker_Draw_FiresHandEvaluatedEvent() {
		var poker = new VideoPoker();
		var eventFired = false;

		poker.HandEvaluated += (s, e) => {
			eventFired = true;
			Assert.NotNull(e.Hand);
			Assert.Equal(5, e.Hand.Length);
		};

		poker.PlaceBet(1);
		poker.Deal();
		poker.Draw();

		Assert.True(eventFired);
	}

	[Fact]
	public void VideoPoker_CollectWinnings_ResetsState() {
		var poker = new VideoPoker();
		poker.PlaceBet(1);
		poker.Deal();
		poker.Draw();

		poker.CollectWinnings();

		Assert.Equal(PokerState.Idle, poker.State);
		Assert.Equal(0u, poker.CurrentBet);
		Assert.Equal(0u, poker.CurrentWinnings);
	}

	[Fact]
	public void VideoPoker_Reset_ClearsAllState() {
		var poker = new VideoPoker();
		poker.PlaceBet(5);
		poker.Deal();

		poker.Reset();

		Assert.Equal(PokerState.Idle, poker.State);
		Assert.Equal(0u, poker.CurrentBet);
		Assert.Equal(0u, poker.CurrentWinnings);
		Assert.Null(poker.DoubleUpDealerCard);
	}
}

public class CasinoPrizesTests {
	[Fact]
	public void AllPrizes_ContainsFalconSword() {
		var falconSword = CasinoPrizes.GetByItemId(0x45);
		Assert.NotNull(falconSword);
		Assert.Equal("Falcon Sword", falconSword.Name);
		Assert.Equal(65_000u, falconSword.Cost);
	}

	[Fact]
	public void GetAffordable_ReturnsOnlyAffordableItems() {
		var affordable = CasinoPrizes.GetAffordable(1000).ToList();

		Assert.All(affordable, p => Assert.True(p.Cost <= 1000));
		Assert.Contains(affordable, p => p.Name == "Prayer Ring");
		Assert.Contains(affordable, p => p.Name == "Small Medal");
	}

	[Fact]
	public void GetByCategory_FiltersCorrectly() {
		var weapons = CasinoPrizes.GetByCategory(PrizeCategory.Weapon).ToList();

		Assert.All(weapons, p => Assert.Equal(PrizeCategory.Weapon, p.Category));
		Assert.True(weapons.Count >= 4); // At least 4 weapons
	}

	[Fact]
	public void ExchangeGoldToCoins_CalculatesCorrectly() {
		Assert.Equal(1u, CasinoPrizes.ExchangeGoldToCoins(20));
		Assert.Equal(50u, CasinoPrizes.ExchangeGoldToCoins(1000));
		Assert.Equal(0u, CasinoPrizes.ExchangeGoldToCoins(19)); // Rounds down
	}

	[Fact]
	public void GetGoldValue_CalculatesCorrectly() {
		var prize = new CasinoPrize {
			ItemId = 0x00,
			Name = "Test",
			Cost = 100
		};

		Assert.Equal(2000u, CasinoPrizes.GetGoldValue(prize));
	}
}
