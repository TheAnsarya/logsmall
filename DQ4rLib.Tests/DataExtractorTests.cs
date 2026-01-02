using DW4Lib.Converters;
using Xunit;

namespace DQ4rLib.Tests;

/// <summary>
/// Tests for DW4Lib DataExtractor class.
/// </summary>
public class DataExtractorTests {
	[Fact]
	public void BankAddressToFileOffset_Bank0_ReturnsCorrectOffset() {
		// Bank 0 at $8000 should return offset 16 (header) + 0
		int offset = DataExtractor.BankAddressToFileOffset(0, 0x8000);
		Assert.Equal(16, offset);
	}

	[Fact]
	public void BankAddressToFileOffset_Bank6_MonsterTable_ReturnsCorrectOffset() {
		// Bank 6 at $A2A2 should return header + (6 * 16KB) + ($A2A2 - $8000)
		// = 16 + 0x18000 + 0x22A2 = 16 + 98304 + 8866 = 107186
		int offset = DataExtractor.BankAddressToFileOffset(6, 0xA2A2);
		int expected = 16 + (6 * 0x4000) + (0xA2A2 - 0x8000);
		Assert.Equal(expected, offset);
	}

	[Fact]
	public void BankAddressToFileOffset_Bank27_ExpTable_ReturnsCorrectOffset() {
		// Bank $27 (39) at $B6ED
		int offset = DataExtractor.BankAddressToFileOffset(0x27, 0xB6ED);
		int expected = 16 + (0x27 * 0x4000) + (0xB6ED - 0x8000);
		Assert.Equal(expected, offset);
	}

	[Fact]
	public void ExtractMonsters_WithMockData_ReturnsCorrectCount() {
		// Create minimal mock ROM data with monster table
		var romData = new byte[0x100000]; // 1MB for safety

		// Fill monster area with test pattern
		int monsterOffset = DataExtractor.BankAddressToFileOffset(6, 0xA2A2);
		for (int i = 0; i < 10 * 27; i++) {
			romData[monsterOffset + i] = (byte)(i % 256);
		}

		var extractor = new DataExtractor(romData);
		var monsters = extractor.ExtractMonsters(10);

		Assert.Equal(10, monsters.Count);
		Assert.Equal(0, monsters[0].Id);
		Assert.Equal(9, monsters[9].Id);
	}

	[Fact]
	public void ExtractMonsters_FirstMonster_HasCorrectRomOffset() {
		var romData = new byte[0x100000];
		var extractor = new DataExtractor(romData);
		var monsters = extractor.ExtractMonsters(1);

		int expectedOffset = DataExtractor.BankAddressToFileOffset(6, 0xA2A2);
		Assert.Equal($"0x{expectedOffset:x5}", monsters[0].RomOffset);
	}

	[Fact]
	public void ExtractItems_WithMockData_ReturnsCorrectCount() {
		var romData = new byte[0x100000];

		// Fill item area with test pattern
		int itemOffset = DataExtractor.BankAddressToFileOffset(7, 0x8000);
		for (int i = 0; i < 50 * 8; i++) {
			romData[itemOffset + i] = (byte)(i % 256);
		}

		var extractor = new DataExtractor(romData);
		var items = extractor.ExtractItems(50);

		Assert.Equal(50, items.Count);
	}

	[Fact]
	public void ExtractExpTables_ReturnsAllCharacters() {
		var romData = new byte[0x100000];
		var extractor = new DataExtractor(romData);
		var tables = extractor.ExtractExpTables();

		Assert.Equal(8, tables.Count);
		Assert.Contains("Hero", tables.Keys);
		Assert.Contains("Ragnar", tables.Keys);
		Assert.Contains("Alena", tables.Keys);
		Assert.Contains("Cristo", tables.Keys);
		Assert.Contains("Brey", tables.Keys);
		Assert.Contains("Taloon", tables.Keys);
		Assert.Contains("Nara", tables.Keys);
		Assert.Contains("Mara", tables.Keys);
	}

	[Fact]
	public void ExtractExpTables_EachCharacter_Has99Levels() {
		var romData = new byte[0x100000];
		var extractor = new DataExtractor(romData);
		var tables = extractor.ExtractExpTables();

		foreach (var character in tables) {
			Assert.Equal(99, character.Value.Count);
		}
	}

	[Fact]
	public void ExtractSpells_WithMockData_ReturnsCorrectCount() {
		var romData = new byte[0x100000];

		// Fill spell area with test pattern
		int spellOffset = DataExtractor.BankAddressToFileOffset(5, 0x8000);
		for (int i = 0; i < 64 * 4; i++) {
			romData[spellOffset + i] = (byte)(i % 256);
		}

		var extractor = new DataExtractor(romData);
		var spells = extractor.ExtractSpells(64);

		Assert.Equal(64, spells.Count);
	}

	[Fact]
	public void ExtractChrData_WithMockData_ReturnsCorrectSize() {
		// ROM with header + 512KB PRG + 256KB CHR
		var romData = new byte[16 + 0x80000 + 0x40000];

		// Fill CHR area with recognizable pattern
		int chrOffset = 16 + 0x80000;
		for (int i = 0; i < 0x40000; i++) {
			romData[chrOffset + i] = 0xAA;
		}

		var extractor = new DataExtractor(romData);
		var chrData = extractor.ExtractChrData();

		Assert.Equal(0x40000, chrData.Length);
		Assert.Equal(0xAA, chrData[0]);
		Assert.Equal(0xAA, chrData[chrData.Length - 1]);
	}

	[Fact]
	public void Monster_RawHex_FormattedCorrectly() {
		var romData = new byte[0x100000];

		// Set up a known pattern
		int offset = DataExtractor.BankAddressToFileOffset(6, 0xA2A2);
		for (int i = 0; i < 27; i++) {
			romData[offset + i] = (byte)i;
		}

		var extractor = new DataExtractor(romData);
		var monsters = extractor.ExtractMonsters(1);

		Assert.NotNull(monsters[0].RawHex);
		Assert.Contains("00 01 02", monsters[0].RawHex);
	}
}
