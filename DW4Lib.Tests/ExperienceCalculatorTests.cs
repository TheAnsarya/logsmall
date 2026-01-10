using DW4Lib.DataStructures;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for ExperienceCalculator - the EXP formula implementation.
/// Tests verify against known values from the game.
/// </summary>
public class ExperienceCalculatorTests {
	[Theory]
	[InlineData(2, "Alena", 20u)]      // L2 base value - slot 2
	[InlineData(5, "Taloon", 10u)]     // L2 base value - slot 5
	[InlineData(0, "Hero", 19u)]       // L2 base value - slot 0
	public void ComputeExp_Level2_ReturnsInitialValue(int slot, string name, uint expected) {
		// Act
		uint result = ExperienceCalculator.ComputeExp(slot, 2);

		// Assert
		Assert.Equal(expected, result);
		Assert.Equal(name, ExperienceCalculator.CharacterNames[slot]);
	}

	[Theory]
	[InlineData(2, 3, 60u)]    // Alena L3
	[InlineData(2, 4, 140u)]   // Alena L4
	[InlineData(2, 5, 260u)]   // Alena L5
	[InlineData(2, 10, 2632u)] // Alena L10
	public void ComputeExp_Alena_MatchesKnownValues(int slot, int level, uint expected) {
		// Act
		uint result = ExperienceCalculator.ComputeExp(slot, level);

		// Assert
		Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData(5, 2, 10u)]    // Taloon L2
	[InlineData(5, 3, 30u)]    // Taloon L3
	[InlineData(5, 4, 70u)]    // Taloon L4
	[InlineData(5, 5, 130u)]   // Taloon L5
	[InlineData(5, 10, 1314u)] // Taloon L10
	public void ComputeExp_Taloon_MatchesKnownValues(int slot, int level, uint expected) {
		// Act
		uint result = ExperienceCalculator.ComputeExp(slot, level);

		// Assert
		Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData(0, 2, 19u)]    // Hero L2
	[InlineData(0, 3, 57u)]    // Hero L3
	[InlineData(0, 4, 133u)]   // Hero L4
	[InlineData(0, 5, 285u)]   // Hero L5
	public void ComputeExp_Hero_MatchesKnownValues(int slot, int level, uint expected) {
		// Act
		uint result = ExperienceCalculator.ComputeExp(slot, level);

		// Assert
		Assert.Equal(expected, result);
	}

	[Fact]
	public void ComputeExp_Level1_ReturnsZero() {
		for (int slot = 0; slot < 8; slot++) {
			Assert.Equal(0u, ExperienceCalculator.ComputeExp(slot, 1));
		}
	}

	[Fact]
	public void ComputeExp_ByName_Works() {
		// Act
		uint result = ExperienceCalculator.ComputeExp("Alena", 10);

		// Assert
		Assert.Equal(2632u, result);
	}

	[Fact]
	public void ComputeExp_ByName_CaseInsensitive() {
		// Act
		uint result1 = ExperienceCalculator.ComputeExp("alena", 5);
		uint result2 = ExperienceCalculator.ComputeExp("ALENA", 5);
		uint result3 = ExperienceCalculator.ComputeExp("Alena", 5);

		// Assert
		Assert.Equal(260u, result1);
		Assert.Equal(260u, result2);
		Assert.Equal(260u, result3);
	}

	[Fact]
	public void ComputeExp_InvalidSlot_Throws() {
		Assert.Throws<ArgumentOutOfRangeException>(() => ExperienceCalculator.ComputeExp(-1, 10));
		Assert.Throws<ArgumentOutOfRangeException>(() => ExperienceCalculator.ComputeExp(8, 10));
	}

	[Fact]
	public void ComputeExp_InvalidLevel_Throws() {
		Assert.Throws<ArgumentOutOfRangeException>(() => ExperienceCalculator.ComputeExp(0, 0));
		Assert.Throws<ArgumentOutOfRangeException>(() => ExperienceCalculator.ComputeExp(0, 51));
	}

	[Fact]
	public void ComputeExp_InvalidName_Throws() {
		Assert.Throws<ArgumentException>(() => ExperienceCalculator.ComputeExp("NotACharacter", 10));
	}

	[Fact]
	public void GenerateTable_CreatesCorrectTable() {
		// Act
		var table = ExperienceCalculator.GenerateTable(2); // Alena

		// Assert
		Assert.Equal("Alena", table.CharacterName);
		Assert.Equal(2, table.CharacterId);
		Assert.Equal(50, table.ExpForLevel.Count);
		Assert.Equal(0u, table.ExpForLevel[0]);   // L1
		Assert.Equal(20u, table.ExpForLevel[1]);  // L2
		Assert.Equal(60u, table.ExpForLevel[2]);  // L3
		Assert.Equal(2632u, table.ExpForLevel[9]); // L10
	}

	[Fact]
	public void GenerateAllTables_Creates8Tables() {
		// Act
		var collection = ExperienceCalculator.GenerateAllTables();

		// Assert
		Assert.Equal(8, collection.Tables.Count);
		Assert.NotNull(collection.GetTable("Hero"));
		Assert.NotNull(collection.GetTable("Ragnar"));
		Assert.NotNull(collection.GetTable("Alena"));
		Assert.NotNull(collection.GetTable("Cristo"));
		Assert.NotNull(collection.GetTable("Brey"));
		Assert.NotNull(collection.GetTable("Taloon"));
		Assert.NotNull(collection.GetTable("Nara"));
		Assert.NotNull(collection.GetTable("Mara"));
	}

	[Fact]
	public void GetCharacterParams_ReturnsCorrectValues() {
		// Act
		var (initial, rateIdx, thresholds) = ExperienceCalculator.GetCharacterParams(2); // Alena

		// Assert
		Assert.Equal(20, initial);
		Assert.Equal(0, rateIdx);
		Assert.Equal([4, 12, 18, 43, 99], thresholds);
	}

	[Fact]
	public void GetCharacterData_ReturnsCorrectBytes() {
		// Act
		byte[] data = ExperienceCalculator.GetCharacterData(2); // Alena

		// Assert
		Assert.Equal(new byte[] { 0x04, 0x0c, 0x92, 0x2b, 0xe3 }, data);
	}

	[Theory]
	[InlineData(0)]  // Hero
	[InlineData(1)]  // Ragnar
	[InlineData(2)]  // Alena
	[InlineData(3)]  // Cristo
	[InlineData(4)]  // Brey
	[InlineData(5)]  // Taloon
	[InlineData(6)]  // Nara
	[InlineData(7)]  // Mara
	public void ComputeExp_AllCharacters_IncreasingValues(int slot) {
		// Verify EXP always increases with level
		uint prev = 0;
		for (int level = 1; level <= 50; level++) {
			uint current = ExperienceCalculator.ComputeExp(slot, level);
			Assert.True(current >= prev, $"EXP should increase: L{level - 1}={prev}, L{level}={current}");
			prev = current;
		}
	}
}
