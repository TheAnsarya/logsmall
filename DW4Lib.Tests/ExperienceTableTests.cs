using DW4Lib.DataStructures;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for ExperienceTable data structure.
/// </summary>
public class ExperienceTableTests {
	[Fact]
	public void GetLevelForExp_ReturnsCorrectLevel() {
		// Arrange
		var table = new ExperienceTable {
			CharacterName = "Test",
			ExpForLevel = new List<uint> { 0, 100, 300, 600, 1000 }
		};

		// Act & Assert
		Assert.Equal(1, table.GetLevelForExp(0));
		Assert.Equal(1, table.GetLevelForExp(50));
		Assert.Equal(2, table.GetLevelForExp(100));
		Assert.Equal(2, table.GetLevelForExp(250));
		Assert.Equal(3, table.GetLevelForExp(300));
		Assert.Equal(5, table.GetLevelForExp(1500));
	}

	[Fact]
	public void ExpToNextLevel_ReturnsCorrectDifference() {
		// Arrange
		var table = new ExperienceTable {
			ExpForLevel = new List<uint> { 0, 100, 300, 600, 1000 }
		};

		// Act & Assert
		Assert.Equal(100u, table.ExpToNextLevel(1)); // 100 - 0
		Assert.Equal(200u, table.ExpToNextLevel(2)); // 300 - 100
		Assert.Equal(300u, table.ExpToNextLevel(3)); // 600 - 300
		Assert.Equal(400u, table.ExpToNextLevel(4)); // 1000 - 600
		Assert.Equal(0u, table.ExpToNextLevel(5));   // No level 6
	}

	[Fact]
	public void ExpToNextLevel_WithInvalidLevel_ReturnsZero() {
		// Arrange
		var table = new ExperienceTable {
			ExpForLevel = new List<uint> { 0, 100, 300 }
		};

		// Act & Assert
		Assert.Equal(0u, table.ExpToNextLevel(0));  // Invalid
		Assert.Equal(0u, table.ExpToNextLevel(-1)); // Invalid
		Assert.Equal(0u, table.ExpToNextLevel(10)); // Out of range
	}
}
