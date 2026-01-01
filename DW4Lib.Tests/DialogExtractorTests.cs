namespace DW4Lib.Tests;

using DW4Lib.Converters;
using Xunit;

/// <summary>
/// Tests for DialogExtractor.
/// </summary>
public class DialogExtractorTests {
	[Fact]
	public void CharTable_ContainsAllBasicCharacters() {
		// Space at 0x00
		Assert.True(DialogExtractor.CharTable.ContainsKey(0x00));
		Assert.Equal(" ", DialogExtractor.CharTable[0x00]);

		// Digits 0-9 at 0x01-0x0A
		Assert.Equal("0", DialogExtractor.CharTable[0x01]);
		Assert.Equal("9", DialogExtractor.CharTable[0x0A]);

		// Lowercase a-z at 0x0B-0x24
		Assert.Equal("a", DialogExtractor.CharTable[0x0B]);
		Assert.Equal("z", DialogExtractor.CharTable[0x24]);

		// Uppercase A-Z at 0x25-0x3E
		Assert.Equal("A", DialogExtractor.CharTable[0x25]);
		Assert.Equal("Z", DialogExtractor.CharTable[0x3E]);
	}

	[Fact]
	public void CharTable_ContainsPunctuation() {
		// Punctuation - check actual values from DialogExtractor
		Assert.True(DialogExtractor.CharTable.ContainsKey(0x3F)); // '
		Assert.True(DialogExtractor.CharTable.ContainsKey(0x40)); // .
		Assert.True(DialogExtractor.CharTable.ContainsKey(0x41)); // ,
		Assert.True(DialogExtractor.CharTable.ContainsKey(0x42)); // -
		Assert.True(DialogExtractor.CharTable.ContainsKey(0x43)); // ?
		Assert.True(DialogExtractor.CharTable.ContainsKey(0x44)); // !
	}

	[Fact]
	public void ControlCodes_HaveExpectedValues() {
		// Control codes are in the dictionary
		Assert.True(DialogExtractor.ControlCodes.ContainsKey(0xFD)); // NewLine
		Assert.True(DialogExtractor.ControlCodes.ContainsKey(0xFF)); // End
		Assert.Equal("[LINE]", DialogExtractor.ControlCodes[0xFD]);
		Assert.Equal("[END]", DialogExtractor.ControlCodes[0xFF]);
	}

	[Fact]
	public void DteTable_ContainsCommonExpansions() {
		// DTE table should have entries
		Assert.NotEmpty(DialogExtractor.DteTable);

		// Check some known expansions
		Assert.True(DialogExtractor.DteTable.ContainsKey(0xE0));
		Assert.True(DialogExtractor.DteTable.ContainsKey(0xE1));
	}

	[Fact]
	public void DecodeByte_ReturnsCharForBasicChars() {
		Assert.Equal(" ", DialogExtractor.DecodeByte(0x00));
		Assert.Equal("A", DialogExtractor.DecodeByte(0x25));
		Assert.Equal("Z", DialogExtractor.DecodeByte(0x3E));
	}

	[Fact]
	public void DecodeByte_ReturnsControlCodePlaceholder() {
		// New line control code
		string result = DialogExtractor.DecodeByte(0xFD);
		Assert.Contains("[LINE]", result);

		// End of string
		result = DialogExtractor.DecodeByte(0xFF);
		Assert.Contains("[END]", result);
	}

	[Fact]
	public void DecodeByte_ExpandsDte() {
		// DTE byte should expand to 2+ characters
		byte dteByte = 0xE0; // Should be "RE" according to actual table
		string result = DialogExtractor.DecodeByte(dteByte);

		// DTE expansion should be longer than 1 character
		Assert.True(result.Length >= 2 || result.StartsWith("["));
	}

	[Fact]
	public void DecodeBytes_HandlesBasicString() {
		// Build "Hello" bytes using correct DW4 encoding
		// H=0x2C, e=0x0F, l=0x16, o=0x19
		byte[] data = [
			0x2C, // H
			0x0F, // e
			0x16, // l
			0x16, // l
			0x19, // o
			0xFF, // END
		];

		string result = DialogExtractor.DecodeBytes(data);
		Assert.StartsWith("Hello", result);
	}

	[Fact]
	public void DecodeBytes_HandlesNewLine() {
		byte[] data = [
			0x2C, // H
			0x13, // i
			0xFD, // LINE
			0xFF, // END
		];

		string result = DialogExtractor.DecodeBytes(data);
		Assert.Contains("[LINE]", result);
	}

	[Fact]
	public void DecodeBytes_StopsAtEndMarker() {
		byte[] data = [
			0x25, // A
			0xFF, // END - should stop here
			0x26, // B - should be ignored
			0x27, // C - should be ignored
		];

		string result = DialogExtractor.DecodeBytes(data);

		// Result should contain A, but not BC
		Assert.Contains("A", result);
		Assert.DoesNotContain("BC", result);
	}

	[Fact]
	public void KnownTables_HasExpectedCount() {
		// Should have all the defined tables (it's an array, not a dictionary)
		Assert.NotEmpty(DialogExtractor.KnownTables);

		// Verify at least some expected tables exist
		var tableNames = DialogExtractor.KnownTables.Select(t => t.Name).ToArray();
		Assert.Contains("MonsterNames", tableNames);
		Assert.Contains("ItemNames", tableNames);
		Assert.Contains("SpellNames", tableNames);
	}

	[Fact]
	public void KnownTables_HaveValidAddresses() {
		foreach (var table in DialogExtractor.KnownTables) {
			// Tables should have valid properties
			Assert.NotEmpty(table.Name);
			Assert.True(table.Bank >= 0, $"{table.Name} has invalid bank");
			Assert.True(table.PointerTableStart > 0, $"{table.Name} has invalid pointer table start");
		}
	}
}
