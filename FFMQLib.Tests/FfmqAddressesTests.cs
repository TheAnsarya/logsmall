namespace FFMQLib.Tests;

/// <summary>
/// Tests for FfmqAddresses utility class
/// </summary>
public class FfmqAddressesTests {
	[Theory]
	[InlineData(0xC00000, 0x000000)] // Bank $C0 start
	[InlineData(0xC08000, 0x008000)] // Bank $C0 + $8000
	[InlineData(0xC10000, 0x010000)] // Bank $C1
	[InlineData(0xD18000, 0x118000)] // Monster stats typical address
	[InlineData(0xD20000, 0x120000)] // Another common address
	public void SnesLoRomToPc_ConvertsCorrectly(int snesAddress, int expectedPc) {
		var result = FfmqAddresses.SnesLoRomToPc(snesAddress);
		Assert.Equal(expectedPc, result);
	}

	[Theory]
	[InlineData(0x000000, 0x008000)] // PC 0 = SNES $00:8000
	[InlineData(0x008000, 0x018000)] // PC $8000 = SNES $01:8000
	[InlineData(0x010000, 0x028000)] // PC $10000 = SNES $02:8000
	public void PcToSnesLoRom_ConvertsCorrectly(int pcOffset, int expectedSnes) {
		var result = FfmqAddresses.PcToSnesLoRom(pcOffset);
		Assert.Equal(expectedSnes, result);
	}

	[Fact]
	public void RoundTrip_PcToSnesAndBack() {
		// For LoROM, PC addresses map to specific SNES addresses
		// Test that common addresses round-trip correctly
		int[] testAddresses = [0x000000, 0x008000, 0x064000, 0x100000];

		foreach (var pc in testAddresses) {
			var snes = FfmqAddresses.PcToSnesLoRom(pc);
			var backToPc = FfmqAddresses.SnesLoRomToPc(snes);
			Assert.Equal(pc, backToPc);
		}
	}

	[Fact]
	public void TextTableAddresses_AreCorrect() {
		// Verify key addresses match expected values
		Assert.Equal(0x064BA0, FfmqTextTables.MonsterNames.Address);
		Assert.Equal(0x064210, FfmqTextTables.SpellNames.Address);
		Assert.Equal(0x0642A0, FfmqTextTables.WeaponNames.Address);
		Assert.Equal(0x064120, FfmqTextTables.ItemNames.Address);
	}

	[Fact]
	public void TextTable_TotalBytes_CalculatesCorrectly() {
		var monsterTable = FfmqTextTables.MonsterNames;

		Assert.Equal(256 * 16, monsterTable.TotalBytes);
		Assert.Equal(0x064BA0 + (256 * 16), monsterTable.EndAddress);
	}
}
