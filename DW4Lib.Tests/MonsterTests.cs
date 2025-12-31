using DW4Lib.DataStructures;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for Monster data structure.
/// </summary>
public class MonsterTests {
	[Fact]
	public void FromBytes_WithValidData_ReturnsCorrectMonster() {
		// Arrange - Sample monster data (27 bytes)
		// EXP=1000 (0x03E8), Gold=500 (0x01F4), ATK=50, DEF=40, AGI=30
		var data = new byte[27];
		data[0] = 0xe8; // EXP low
		data[1] = 0x03; // EXP high
		data[2] = 0xf4; // Gold low
		data[3] = 0x01; // Gold high
		data[6] = 50;   // Attack
		data[7] = 40;   // Defense
		data[8] = 30;   // Agility
		data[21] = 1;   // Metal flag (set)
		data[22] = 15;  // Item drop
		data[23] = 0x0f; // Status flags

		// Act
		var monster = Monster.FromBytes(data);

		// Assert
		Assert.Equal(1000, (int)monster.Experience);
		Assert.Equal(500, (int)monster.Gold);
		Assert.Equal(50, monster.Attack);
		Assert.Equal(40, monster.Defense);
		Assert.Equal(30, monster.Agility);
		Assert.True(monster.IsMetal);
		Assert.Equal(15, monster.ItemDrop);
		Assert.Equal(0x0f, monster.StatusFlags);
	}

	[Fact]
	public void ToBytes_RoundTrip_PreservesData() {
		// Arrange - Create a monster with specific values
		var original = new Monster {
			Experience = 12345,
			Gold = 6789,
			Attack = 100,
			Defense = 80,
			Agility = 60,
			Byte4 = 0xaa,
			Byte5 = 0xbb,
			Byte9 = 0x11,
			MetalFlag = 0,
			ItemDrop = 42,
			StatusFlags = 0xff
		};

		// Act - Convert to bytes and back
		var bytes = original.ToBytes();
		var roundTrip = Monster.FromBytes(bytes);

		// Assert
		Assert.Equal(original.Experience, roundTrip.Experience);
		Assert.Equal(original.Gold, roundTrip.Gold);
		Assert.Equal(original.Attack, roundTrip.Attack);
		Assert.Equal(original.Defense, roundTrip.Defense);
		Assert.Equal(original.Agility, roundTrip.Agility);
		Assert.Equal(original.Byte4, roundTrip.Byte4);
		Assert.Equal(original.Byte5, roundTrip.Byte5);
		Assert.Equal(original.Byte9, roundTrip.Byte9);
		Assert.Equal(original.MetalFlag, roundTrip.MetalFlag);
		Assert.Equal(original.ItemDrop, roundTrip.ItemDrop);
		Assert.Equal(original.StatusFlags, roundTrip.StatusFlags);
	}

	[Fact]
	public void ToBytes_ReturnsCorrectSize() {
		// Arrange
		var monster = new Monster();

		// Act
		var bytes = monster.ToBytes();

		// Assert
		Assert.Equal(Monster.Size, bytes.Length);
		Assert.Equal(27, bytes.Length);
	}

	[Fact]
	public void FromBytes_WithOffset_ReadsCorrectData() {
		// Arrange - Put monster data at offset 10
		var data = new byte[50];
		data[10 + 0] = 0x64; // EXP low = 100
		data[10 + 1] = 0x00; // EXP high
		data[10 + 6] = 25;   // Attack

		// Act
		var monster = Monster.FromBytes(data, 10);

		// Assert
		Assert.Equal(100, (int)monster.Experience);
		Assert.Equal(25, monster.Attack);
	}

	[Fact]
	public void FromBytes_WithInsufficientData_ThrowsArgumentException() {
		// Arrange
		var data = new byte[10]; // Too small

		// Act & Assert
		Assert.Throws<ArgumentException>(() => Monster.FromBytes(data));
	}

	[Fact]
	public void IsMetal_WithMetalFlagSet_ReturnsTrue() {
		// Arrange
		var monster = new Monster { MetalFlag = 5 };

		// Assert
		Assert.True(monster.IsMetal);
	}

	[Fact]
	public void IsMetal_WithMetalFlagZero_ReturnsFalse() {
		// Arrange
		var monster = new Monster { MetalFlag = 0 };

		// Assert
		Assert.False(monster.IsMetal);
	}

	[Fact]
	public void Constants_HaveCorrectValues() {
		// Assert
		Assert.Equal(27, Monster.Size);
		Assert.Equal(6, Monster.Bank);
		Assert.Equal(0xA2A2, Monster.TableAddress);
	}
}
