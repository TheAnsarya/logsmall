using DW4Lib.Converters;
using DW4Lib.DataStructures;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for ExperienceTableToDQ3r converter.
/// </summary>
public class ExperienceTableToDQ3rTests {
	[Fact]
	public void GetVocationForCharacter_KnownCharacter_ReturnsCorrectVocation() {
		Assert.Equal(DQ3rVocation.Hero, ExperienceTableToDQ3r.GetVocationForCharacter("Hero"));
		Assert.Equal(DQ3rVocation.Soldier, ExperienceTableToDQ3r.GetVocationForCharacter("Ragnar"));
		Assert.Equal(DQ3rVocation.MartialArtist, ExperienceTableToDQ3r.GetVocationForCharacter("Alena"));
		Assert.Equal(DQ3rVocation.Priest, ExperienceTableToDQ3r.GetVocationForCharacter("Cristo"));
		Assert.Equal(DQ3rVocation.Mage, ExperienceTableToDQ3r.GetVocationForCharacter("Brey"));
		Assert.Equal(DQ3rVocation.Merchant, ExperienceTableToDQ3r.GetVocationForCharacter("Taloon"));
	}

	[Fact]
	public void GetVocationForCharacter_CaseInsensitive() {
		Assert.Equal(DQ3rVocation.Hero, ExperienceTableToDQ3r.GetVocationForCharacter("hero"));
		Assert.Equal(DQ3rVocation.Hero, ExperienceTableToDQ3r.GetVocationForCharacter("HERO"));
		Assert.Equal(DQ3rVocation.Soldier, ExperienceTableToDQ3r.GetVocationForCharacter("ragnar"));
	}

	[Fact]
	public void GetVocationForCharacter_UnknownCharacter_ReturnsSoldier() {
		Assert.Equal(DQ3rVocation.Soldier, ExperienceTableToDQ3r.GetVocationForCharacter("Unknown"));
		Assert.Equal(DQ3rVocation.Soldier, ExperienceTableToDQ3r.GetVocationForCharacter(""));
	}

	[Fact]
	public void ExtrapolateExpCurve_ShortCurve_ExtendsProperly() {
		// Arrange - DW4 has 50 levels
		var dw4Curve = new List<uint>();
		for (int i = 0; i < 50; i++) {
			dw4Curve.Add((uint)(i * 100 + i * i)); // Simple quadratic curve
		}

		// Act - extend to 99 levels
		var result = ExperienceTableToDQ3r.ExtrapolateExpCurve(dw4Curve, 99);

		// Assert
		Assert.Equal(99, result.Length);
		// First 50 values should match original
		for (int i = 0; i < 50; i++) {
			Assert.Equal(dw4Curve[i], result[i]);
		}
		// Extended values should be increasing
		for (int i = 50; i < 98; i++) {
			Assert.True(result[i + 1] > result[i], $"Level {i + 1} exp should be > level {i}");
		}
	}

	[Fact]
	public void ExtrapolateExpCurve_AlreadyLongEnough_NoChange() {
		// Arrange
		var dw4Curve = new List<uint>();
		for (int i = 0; i < 100; i++) {
			dw4Curve.Add((uint)(i * 100));
		}

		// Act
		var result = ExperienceTableToDQ3r.ExtrapolateExpCurve(dw4Curve, 99);

		// Assert
		Assert.Equal(99, result.Length);
		for (int i = 0; i < 99; i++) {
			Assert.Equal(dw4Curve[i], result[i]);
		}
	}

	[Fact]
	public void ConvertTable_ReturnsCorrectMaxLevel() {
		// Arrange
		var dw4Table = new ExperienceTable {
			CharacterId = 0,
			CharacterName = "Hero",
			ExpForLevel = new List<uint> { 0, 100, 300, 600, 1000 }
		};

		// Act
		var result = ExperienceTableToDQ3r.ConvertTable(dw4Table);

		// Assert
		Assert.Equal(ExperienceTableToDQ3r.DQ3rMaxLevel, result.MaxLevel);
		Assert.Equal(99, result.ExpRequired.Length);
	}

	[Fact]
	public void ConvertTable_PreservesSourceInfo() {
		// Arrange
		var dw4Table = new ExperienceTable {
			CharacterId = 1,
			CharacterName = "Ragnar",
			ExpForLevel = new List<uint> { 0, 100, 300, 600, 1000 }
		};

		// Act
		var result = ExperienceTableToDQ3r.ConvertTable(dw4Table);

		// Assert
		Assert.Equal(DQ3rVocation.Soldier, result.Vocation);
		Assert.Equal("Soldier", result.VocationName);
		Assert.Equal("Ragnar", result.SourceCharacter);
	}

	[Fact]
	public void ConvertAll_ProcessesAllTables() {
		// Arrange
		var collection = new ExperienceTableCollection();
		collection.Tables.Add(new ExperienceTable {
			CharacterId = 0,
			CharacterName = "Hero",
			ExpForLevel = new List<uint> { 0, 100, 300 }
		});
		collection.Tables.Add(new ExperienceTable {
			CharacterId = 1,
			CharacterName = "Ragnar",
			ExpForLevel = new List<uint> { 0, 150, 400 }
		});

		// Act
		var result = ExperienceTableToDQ3r.ConvertAll(collection);

		// Assert - should have original 2 + any generated vocations
		Assert.True(result.Count >= 2);
		Assert.Contains(result, t => t.SourceCharacter == "Hero");
		Assert.Contains(result, t => t.SourceCharacter == "Ragnar");
	}

	[Fact]
	public void CalculateDifferences_ReturnsPerLevelRequirements() {
		// Arrange
		uint[] totalExp = [0, 100, 300, 600, 1000];

		// Act
		var diffs = ExperienceTableToDQ3r.CalculateDifferences(totalExp);

		// Assert
		Assert.Equal(5, diffs.Length);
		Assert.Equal(0u, diffs[0]);   // Level 1 requires 0
		Assert.Equal(100u, diffs[1]); // Level 2 requires 100 more
		Assert.Equal(200u, diffs[2]); // Level 3 requires 200 more
		Assert.Equal(300u, diffs[3]); // Level 4 requires 300 more
		Assert.Equal(400u, diffs[4]); // Level 5 requires 400 more
	}

	[Fact]
	public void DQ3rExpTable_GetTotalExpForLevel_ReturnsCorrectValue() {
		// Arrange
		var table = new DQ3rExpTable {
			ExpRequired = [0, 100, 300, 600, 1000]
		};

		// Act & Assert
		Assert.Equal(0u, table.GetTotalExpForLevel(1));
		Assert.Equal(100u, table.GetTotalExpForLevel(2));
		Assert.Equal(300u, table.GetTotalExpForLevel(3));
		Assert.Equal(0u, table.GetTotalExpForLevel(0));  // Invalid
		Assert.Equal(0u, table.GetTotalExpForLevel(10)); // Out of range
	}

	[Fact]
	public void DQ3rExpTable_GetExpToNextLevel_ReturnsCorrectDifference() {
		// Arrange
		var table = new DQ3rExpTable {
			ExpRequired = [0, 100, 300, 600, 1000]
		};

		// Act & Assert
		Assert.Equal(100u, table.GetExpToNextLevel(1)); // 100 - 0
		Assert.Equal(200u, table.GetExpToNextLevel(2)); // 300 - 100
		Assert.Equal(300u, table.GetExpToNextLevel(3)); // 600 - 300
		Assert.Equal(400u, table.GetExpToNextLevel(4)); // 1000 - 600
		Assert.Equal(0u, table.GetExpToNextLevel(5));   // At max
	}
}
