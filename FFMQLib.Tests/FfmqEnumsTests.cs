namespace FFMQLib.Tests;

/// <summary>
/// Tests for FfmqEnums flags and values
/// </summary>
public class FfmqEnumsTests {
	[Fact]
	public void FfmqElement_HasCorrectFlagValues() {
		Assert.Equal(0x00, (byte)FfmqElement.None);
		Assert.Equal(0x01, (byte)FfmqElement.Fire);
		Assert.Equal(0x02, (byte)FfmqElement.Ice);
		Assert.Equal(0x04, (byte)FfmqElement.Thunder);
		Assert.Equal(0x08, (byte)FfmqElement.Earth);
		Assert.Equal(0x10, (byte)FfmqElement.Water);
		Assert.Equal(0x20, (byte)FfmqElement.Wind);
		Assert.Equal(0x40, (byte)FfmqElement.Cure);
		Assert.Equal(0x80, (byte)FfmqElement.Fatal);
	}

	[Fact]
	public void FfmqElement_CanCombineFlags() {
		var fireAndIce = FfmqElement.Fire | FfmqElement.Ice;

		Assert.True(fireAndIce.HasFlag(FfmqElement.Fire));
		Assert.True(fireAndIce.HasFlag(FfmqElement.Ice));
		Assert.False(fireAndIce.HasFlag(FfmqElement.Thunder));
		Assert.Equal(0x03, (byte)fireAndIce);
	}

	[Fact]
	public void FfmqStatus_HasCorrectFlagValues() {
		Assert.Equal(0x00, (byte)FfmqStatus.None);
		Assert.Equal(0x01, (byte)FfmqStatus.Poison);
		Assert.Equal(0x02, (byte)FfmqStatus.Paralysis);
		Assert.Equal(0x04, (byte)FfmqStatus.Confusion);
		Assert.Equal(0x08, (byte)FfmqStatus.Sleep);
		Assert.Equal(0x10, (byte)FfmqStatus.Petrify);
		Assert.Equal(0x20, (byte)FfmqStatus.Blind);
		Assert.Equal(0x40, (byte)FfmqStatus.Mute);
		Assert.Equal(0x80, (byte)FfmqStatus.Dead);
	}

	[Fact]
	public void FfmqTargetType_HasCorrectValues() {
		Assert.Equal(0, (byte)FfmqTargetType.SingleAlly);
		Assert.Equal(1, (byte)FfmqTargetType.AllAllies);
		Assert.Equal(2, (byte)FfmqTargetType.Self);
		Assert.Equal(3, (byte)FfmqTargetType.SingleEnemy);
		Assert.Equal(4, (byte)FfmqTargetType.AllEnemies);
	}

	[Fact]
	public void FfmqWeaponSlot_HasCorrectValues() {
		Assert.Equal(0, (byte)FfmqWeaponSlot.Sword);
		Assert.Equal(1, (byte)FfmqWeaponSlot.Axe);
		Assert.Equal(2, (byte)FfmqWeaponSlot.Claw);
		Assert.Equal(3, (byte)FfmqWeaponSlot.Bomb);
	}

	[Fact]
	public void FfmqArmorSlot_HasCorrectValues() {
		Assert.Equal(0, (byte)FfmqArmorSlot.Helmet);
		Assert.Equal(1, (byte)FfmqArmorSlot.Armor);
		Assert.Equal(2, (byte)FfmqArmorSlot.Shield);
		Assert.Equal(3, (byte)FfmqArmorSlot.Accessory);
	}
}
