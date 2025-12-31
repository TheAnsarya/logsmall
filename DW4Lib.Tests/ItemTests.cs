using DW4Lib.DataStructures;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for Item data structure.
/// </summary>
public class ItemTests {
	[Fact]
	public void FromBytes_WithValidData_ReturnsCorrectItem() {
		// Arrange - Sample item data (8 bytes)
		// NameIndex=0x10, TypeFlags=0x01 (weapon), StatBonus=50, SpecialEffect=0,
		// PriceLow=0x64, PriceHigh=0x00 (100 gold), EquipFlags=0x05, IconID=2
		var data = new byte[] { 0x10, 0x01, 50, 0, 0x64, 0x00, 0x05, 2 };

		// Act
		var item = Item.FromBytes(data);

		// Assert
		Assert.Equal(0x10, item.NameIndex);
		Assert.Equal(0x01, item.TypeFlags);
		Assert.Equal(ItemType.Weapon, item.EquipmentSlot);
		Assert.Equal(50, item.StatBonus);
		Assert.Equal(0, item.SpecialEffect);
		Assert.Equal(100, (int)item.Price);
		Assert.Equal(0x05, item.EquipFlags);
		Assert.Equal(2, item.IconID);
	}

	[Fact]
	public void ToBytes_RoundTrip_PreservesData() {
		// Arrange
		var original = new Item {
			NameIndex = 25,
			TypeFlags = 0x02, // Armor
			StatBonus = 45,
			SpecialEffect = 10,
			PriceLow = 0x50,
			PriceHigh = 0x01, // 336 gold
			EquipFlags = 0x07,
			IconID = 5
		};

		// Act
		var bytes = original.ToBytes();
		var roundTrip = Item.FromBytes(bytes);

		// Assert
		Assert.Equal(original.NameIndex, roundTrip.NameIndex);
		Assert.Equal(original.TypeFlags, roundTrip.TypeFlags);
		Assert.Equal(original.StatBonus, roundTrip.StatBonus);
		Assert.Equal(original.SpecialEffect, roundTrip.SpecialEffect);
		Assert.Equal(original.Price, roundTrip.Price);
		Assert.Equal(original.EquipFlags, roundTrip.EquipFlags);
		Assert.Equal(original.IconID, roundTrip.IconID);
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
		data[5 + 1] = 0x02; // Armor type
		data[5 + 2] = 75;   // StatBonus

		// Act
		var item = Item.FromBytes(data, 5);

		// Assert
		Assert.Equal(ItemType.Armor, item.EquipmentSlot);
		Assert.Equal(75, item.StatBonus);
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
	public void Price_SetAndGet_WorksCorrectly() {
		// Arrange
		var item = new Item();

		// Act
		item.Price = 1234;

		// Assert
		Assert.Equal(1234, (int)item.Price);
		Assert.Equal(0xD2, item.PriceLow);  // 1234 & 0xFF = 210 = 0xD2
		Assert.Equal(0x04, item.PriceHigh); // 1234 >> 8 = 4
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
