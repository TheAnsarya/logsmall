using DW4Lib.Converters;
using DW4Lib.DataStructures;
using System.Text.Json;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for MonsterConverter.
/// </summary>
public class MonsterConverterTests {
	[Fact]
	public void ToJson_CreatesCorrectJsonObject() {
		// Arrange
		var monster = new Monster {
			Experience = 1500,
			Gold = 750,
			Attack = 80,
			Defense = 60,
			Agility = 45,
			ItemDrop = 12,
			StatusFlags = 0x1f,
			MetalFlag = 0
		};

		// Act
		var json = MonsterConverter.ToJson(monster, 5, "Test Monster");

		// Assert
		Assert.Equal(5, json.Index);
		Assert.Equal("Test Monster", json.Name);
		Assert.Equal(1500, (int)json.Experience);
		Assert.Equal(750, (int)json.Gold);
		Assert.Equal(80, json.Attack);
		Assert.Equal(60, json.Defense);
		Assert.Equal(45, json.Agility);
		Assert.Equal(12, json.ItemDrop);
		Assert.Equal(0x1f, json.StatusFlags);
		Assert.False(json.IsMetal);
	}

	[Fact]
	public void ToJson_WithNoName_GeneratesDefaultName() {
		// Arrange
		var monster = new Monster();

		// Act
		var json = MonsterConverter.ToJson(monster, 42);

		// Assert
		Assert.Equal("Monster_042", json.Name);
	}

	[Fact]
	public void FromJson_CreatesCorrectMonster() {
		// Arrange
		var json = new MonsterJson {
			Index = 10,
			Name = "Slime",
			Experience = 2,
			Gold = 1,
			Attack = 5,
			Defense = 4,
			Agility = 3,
			ItemDrop = 0,
			StatusFlags = 0,
			IsMetal = false,
			RawBytes = new MonsterRawBytes()
		};

		// Act
		var monster = MonsterConverter.FromJson(json);

		// Assert
		Assert.Equal(2, (int)monster.Experience);
		Assert.Equal(1, (int)monster.Gold);
		Assert.Equal(5, monster.Attack);
		Assert.Equal(4, monster.Defense);
		Assert.Equal(3, monster.Agility);
	}

	[Fact]
	public void ToJsonString_CreatesValidJson() {
		// Arrange
		var monsters = new List<Monster> {
			new Monster { Experience = 100, Gold = 50, Attack = 10 },
			new Monster { Experience = 200, Gold = 100, Attack = 20 }
		};

		// Act
		var jsonString = MonsterConverter.ToJsonString(monsters);

		// Assert
		Assert.NotEmpty(jsonString);
		Assert.Contains("experience", jsonString); // camelCase
		Assert.Contains("100", jsonString);
		Assert.Contains("200", jsonString);
	}

	[Fact]
	public void ToJsonString_WithNames_AppliesNames() {
		// Arrange
		var monsters = new List<Monster> {
			new Monster { Experience = 1 },
			new Monster { Experience = 2 }
		};
		var names = new List<string> { "Slime", "Drakee" };

		// Act
		var jsonString = MonsterConverter.ToJsonString(monsters, names);

		// Assert
		Assert.Contains("Slime", jsonString);
		Assert.Contains("Drakee", jsonString);
	}

	[Fact]
	public void RoundTrip_ThroughJson_PreservesRawBytes() {
		// Arrange
		var original = new Monster {
			Byte4 = 0xaa,
			Byte5 = 0xbb,
			Byte9 = 0xcc,
			Byte10 = 0xdd,
			Byte24 = 0xee,
			Byte25 = 0xff,
			Byte26 = 0x12
		};
		var json = MonsterConverter.ToJson(original, 0);

		// Act
		var roundTrip = MonsterConverter.FromJson(json);

		// Assert
		Assert.Equal(original.Byte4, roundTrip.Byte4);
		Assert.Equal(original.Byte5, roundTrip.Byte5);
		Assert.Equal(original.Byte9, roundTrip.Byte9);
		Assert.Equal(original.Byte10, roundTrip.Byte10);
		Assert.Equal(original.Byte24, roundTrip.Byte24);
		Assert.Equal(original.Byte25, roundTrip.Byte25);
		Assert.Equal(original.Byte26, roundTrip.Byte26);
	}
}
