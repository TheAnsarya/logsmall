using DW4Lib.DataStructures;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for Item data structure.
/// </summary>
public class ItemTests {
	[Fact]
	public void FromBytes_WithValidData_ReturnsCorrectItem() {
		// Arrange - Sample item data (8 bytes)
		// TypeFlags=0x01 (weapon), StatModifier=50, SpecialFlags=0,
		// BuyPrice=0x0064 (100 gold, big-endian), SellPrice=0x0032 (50 gold), EquipFlags=0x05
		var data = new byte[] { 0x01, 50, 0, 0x00, 0x64, 0x00, 0x32, 0x05 };

		// Act
		var item = Item.FromBytes(data);

		// Assert
		Assert.Equal(0x01, item.TypeFlags);
		Assert.Equal(ItemType.Weapon, item.EquipmentSlot);
		Assert.Equal(50, item.StatModifier);
		Assert.Equal(0, item.SpecialFlags);
		Assert.Equal(100, (int)item.BuyPrice);
		Assert.Equal(50, (int)item.SellPrice);
		Assert.Equal(0x05, item.EquipFlags);
	}

	[Fact]
	public void ToBytes_RoundTrip_PreservesData() {
		// Arrange
		var original = new Item {
			TypeFlags = 0x02, // Armor
			StatModifier = 45,
			SpecialFlags = 10,
			BuyPrice = 336,
			SellPrice = 168,
			EquipFlags = 0x07
		};

		// Act
		var bytes = original.ToBytes();
		var roundTrip = Item.FromBytes(bytes);

		// Assert
		Assert.Equal(original.TypeFlags, roundTrip.TypeFlags);
		Assert.Equal(original.StatModifier, roundTrip.StatModifier);
		Assert.Equal(original.SpecialFlags, roundTrip.SpecialFlags);
		Assert.Equal(original.BuyPrice, roundTrip.BuyPrice);
		Assert.Equal(original.SellPrice, roundTrip.SellPrice);
		Assert.Equal(original.EquipFlags, roundTrip.EquipFlags);
	}

	[Fact]
	public void ToBytes_ReturnsCorrectSize() {
		// Arrange
		var item = new Item();

		// Act
		var bytes = item.ToBytes();

		// Assert
		Assert.Equal(Item.Size, bytes.Length);
		Assert.Equal(8, bytes.Length);
	}

	[Fact]
	public void FromBytes_WithOffset_ReadsCorrectData() {
		// Arrange - Put item data at offset 5
		var data = new byte[20];
		data[5 + 0] = 0x02; // Armor type
		data[5 + 1] = 75;   // StatModifier

		// Act
		var item = Item.FromBytes(data, 5);

		// Assert
		Assert.Equal(ItemType.Armor, item.EquipmentSlot);
		Assert.Equal(75, item.StatModifier);
	}

	[Fact]
	public void FromBytes_WithInsufficientData_ThrowsArgumentException() {
		// Arrange
		var data = new byte[5]; // Too small

		// Act & Assert
		Assert.Throws<ArgumentException>(() => Item.FromBytes(data));
	}

	[Fact]
	public void CanEquip_WithValidCharacter_ReturnsTrue() {
		// Arrange - Hero and Ragnar can equip (bits 0 and 1)
		var item = new Item { EquipFlags = 0x03 };

		// Assert
		Assert.True(item.CanEquip(CharacterID.Hero));
		Assert.True(item.CanEquip(CharacterID.Ragnar));
		Assert.False(item.CanEquip(CharacterID.Alena));
	}

	[Fact]
	public void BuyPrice_BigEndian_ParsesCorrectly() {
		// Arrange - Buy price 0x04D2 = 1234 in big-endian
		var data = new byte[] { 0x01, 0, 0, 0x04, 0xD2, 0x02, 0x69, 0xFF };

		// Act
		var item = Item.FromBytes(data);

		// Assert
		Assert.Equal(1234, (int)item.BuyPrice);
		Assert.Equal(617, (int)item.SellPrice); // 0x0269 = 617
	}

	[Fact]
	public void IsCursed_WithNegativeModifier_ReturnsTrue() {
		// Arrange - Cursed item with -5 stat modifier
		var item = new Item { StatModifier = -5 };

		// Assert
		Assert.True(item.IsCursed);
		Assert.Equal(-5, item.StatModifier);
	}

	[Fact]
	public void IsCursed_WithPositiveModifier_ReturnsFalse() {
		// Arrange - Normal item with +10 stat modifier
		var item = new Item { StatModifier = 10 };

		// Assert
		Assert.False(item.IsCursed);
	}

	[Fact]
	public void EquipableByString_ReturnsCorrectCharacters() {
		// Arrange - Hero, Ragnar, and Taloon can equip (bits 0, 1, 5)
		var item = new Item { EquipFlags = 0x23 }; // 0010 0011

		// Act
		var result = item.EquipableByString;

		// Assert
		Assert.Contains("Hero", result);
		Assert.Contains("Ragnar", result);
		Assert.Contains("Taloon", result);
		Assert.DoesNotContain("Alena", result);
	}

	[Fact]
	public void Constants_AreCorrect() {
		// Assert ROM location constants
		Assert.Equal(8, Item.Size);
		Assert.Equal(7, Item.Bank);
		Assert.Equal(0x8000, Item.TableAddress);
		Assert.Equal(0x1C010, Item.FileOffset);
		Assert.Equal(220, Item.TotalItems);
	}

	[Fact]
	public void ItemType_EnumValues_AreCorrect() {
		// Assert known item types
		Assert.Equal(0, (int)ItemType.Consumable);
		Assert.Equal(1, (int)ItemType.Weapon);
		Assert.Equal(2, (int)ItemType.Armor);
		Assert.Equal(3, (int)ItemType.Shield);
		Assert.Equal(4, (int)ItemType.Helmet);
		Assert.Equal(5, (int)ItemType.Accessory);
		Assert.Equal(6, (int)ItemType.KeyItem);
		Assert.Equal(7, (int)ItemType.Special);
	}
}
