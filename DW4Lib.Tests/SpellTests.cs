using DW4Lib.DataStructures;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for Spell data structure.
/// </summary>
public class SpellTests {
	[Fact]
	public void FromBytes_WithValidData_ReturnsCorrectSpell() {
		// Arrange - Sample spell data (6 bytes)
		// NameIndex=5, MPCost=10, BasePower=25, TypeFlags, SecondaryEffect=0, SuccessRate=100
		// TypeFlags: Target=1 (AllEnemies), Type=0 (Damage), Element=1 (Fire)
		// TypeFlags = 0x41 = 01000001 = Fire(01) + Damage(000) + AllEnemies(001)
		var data = new byte[] { 5, 10, 25, 0x41, 0, 100 };

		// Act
		var spell = Spell.FromBytes(data);

		// Assert
		Assert.Equal(5, spell.NameIndex);
		Assert.Equal(10, spell.MPCost);
		Assert.Equal(25, spell.BasePower);
		Assert.Equal(SpellTarget.AllEnemies, spell.Target);
		Assert.Equal(SpellType.Damage, spell.Type);
		Assert.Equal(SpellElement.Fire, spell.Element);
		Assert.Equal(0, spell.SecondaryEffect);
		Assert.Equal(100, spell.SuccessRate);
	}

	[Fact]
	public void ToBytes_RoundTrip_PreservesData() {
		// Arrange
		var original = new Spell {
			NameIndex = 10,
			MPCost = 15,
			BasePower = 60,
			TypeFlags = 0x8B, // Ice(10) + Heal(001) + AllAllies(011)
			SecondaryEffect = 5,
			SuccessRate = 80
		};

		// Act
		var bytes = original.ToBytes();
		var roundTrip = Spell.FromBytes(bytes);

		// Assert
		Assert.Equal(original.NameIndex, roundTrip.NameIndex);
		Assert.Equal(original.MPCost, roundTrip.MPCost);
		Assert.Equal(original.BasePower, roundTrip.BasePower);
		Assert.Equal(original.TypeFlags, roundTrip.TypeFlags);
		Assert.Equal(original.SecondaryEffect, roundTrip.SecondaryEffect);
		Assert.Equal(original.SuccessRate, roundTrip.SuccessRate);
	}

	[Fact]
	public void ToBytes_ReturnsCorrectSize() {
		// Arrange
		var spell = new Spell();

		// Act
		var bytes = spell.ToBytes();

		// Assert
		Assert.Equal(Spell.Size, bytes.Length);
		Assert.Equal(6, bytes.Length);
	}

	[Fact]
	public void FromBytes_WithOffset_ReadsCorrectData() {
		// Arrange - Put spell data at offset 3
		var data = new byte[15];
		data[3 + 1] = 20;   // MP Cost
		data[3 + 2] = 80;   // BasePower

		// Act
		var spell = Spell.FromBytes(data, 3);

		// Assert
		Assert.Equal(20, spell.MPCost);
		Assert.Equal(80, spell.BasePower);
	}

	[Fact]
	public void FromBytes_WithInsufficientData_ThrowsArgumentException() {
		// Arrange
		var data = new byte[3]; // Too small

		// Act & Assert
		Assert.Throws<ArgumentException>(() => Spell.FromBytes(data));
	}

	[Fact]
	public void SpellTarget_EnumValues_AreCorrect() {
		Assert.Equal(0, (int)SpellTarget.SingleEnemy);
		Assert.Equal(1, (int)SpellTarget.AllEnemies);
		Assert.Equal(2, (int)SpellTarget.SingleAlly);
		Assert.Equal(3, (int)SpellTarget.AllAllies);
		Assert.Equal(4, (int)SpellTarget.Self);
		Assert.Equal(5, (int)SpellTarget.Field);
	}

	[Fact]
	public void SpellType_EnumValues_AreCorrect() {
		Assert.Equal(0, (int)SpellType.Damage);
		Assert.Equal(1, (int)SpellType.Heal);
		Assert.Equal(2, (int)SpellType.Buff);
		Assert.Equal(3, (int)SpellType.Debuff);
		Assert.Equal(4, (int)SpellType.Status);
		Assert.Equal(5, (int)SpellType.Utility);
		Assert.Equal(6, (int)SpellType.Transport);
		Assert.Equal(7, (int)SpellType.Special);
	}

	[Fact]
	public void SpellElement_EnumValues_AreCorrect() {
		Assert.Equal(0, (int)SpellElement.None);
		Assert.Equal(1, (int)SpellElement.Fire);
		Assert.Equal(2, (int)SpellElement.Ice);
		Assert.Equal(3, (int)SpellElement.Electric);
	}

	[Fact]
	public void Target_ExtractsCorrectlyFromTypeFlags() {
		// Arrange - TypeFlags with different targets
		var spellSingleEnemy = new Spell { TypeFlags = 0x00 };
		var spellAllEnemies = new Spell { TypeFlags = 0x01 };
		var spellAllAllies = new Spell { TypeFlags = 0x03 };
		var spellSelf = new Spell { TypeFlags = 0x04 };

		// Assert
		Assert.Equal(SpellTarget.SingleEnemy, spellSingleEnemy.Target);
		Assert.Equal(SpellTarget.AllEnemies, spellAllEnemies.Target);
		Assert.Equal(SpellTarget.AllAllies, spellAllAllies.Target);
		Assert.Equal(SpellTarget.Self, spellSelf.Target);
	}

	[Fact]
	public void Element_ExtractsCorrectlyFromTypeFlags() {
		// Arrange - TypeFlags with different elements (bits 6-7)
		var spellNone = new Spell { TypeFlags = 0x00 };
		var spellFire = new Spell { TypeFlags = 0x40 };
		var spellIce = new Spell { TypeFlags = 0x80 };
		var spellElectric = new Spell { TypeFlags = 0xC0 };

		// Assert
		Assert.Equal(SpellElement.None, spellNone.Element);
		Assert.Equal(SpellElement.Fire, spellFire.Element);
		Assert.Equal(SpellElement.Ice, spellIce.Element);
		Assert.Equal(SpellElement.Electric, spellElectric.Element);
	}
}
