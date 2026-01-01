namespace DW4Lib.Tests;

using DW4Lib.Converters;
using DW4Lib.Text;
using Xunit;

/// <summary>
/// Tests for FontToDQ3r converter.
/// </summary>
public class FontToDQ3rTests {
	[Fact]
	public void AsciiToTableCode_ContainsAllBasicCharacters() {
		// Verify all expected characters are mapped
		Assert.True(FontToDQ3r.AsciiToTableCode.ContainsKey(' '));
		Assert.True(FontToDQ3r.AsciiToTableCode.ContainsKey('A'));
		Assert.True(FontToDQ3r.AsciiToTableCode.ContainsKey('Z'));
		Assert.True(FontToDQ3r.AsciiToTableCode.ContainsKey('a'));
		Assert.True(FontToDQ3r.AsciiToTableCode.ContainsKey('z'));
		Assert.True(FontToDQ3r.AsciiToTableCode.ContainsKey('0'));
		Assert.True(FontToDQ3r.AsciiToTableCode.ContainsKey('9'));
		Assert.True(FontToDQ3r.AsciiToTableCode.ContainsKey('.'));
		Assert.True(FontToDQ3r.AsciiToTableCode.ContainsKey('!'));
		Assert.True(FontToDQ3r.AsciiToTableCode.ContainsKey('?'));
	}

	[Fact]
	public void AsciiToTableCode_HasCorrectMappings() {
		// Space should be first character
		Assert.Equal(0x0200, FontToDQ3r.AsciiToTableCode[' ']);

		// Numbers start at 0x0201
		Assert.Equal(0x0201, FontToDQ3r.AsciiToTableCode['0']);
		Assert.Equal(0x020A, FontToDQ3r.AsciiToTableCode['9']);

		// Uppercase starts at 0x020B
		Assert.Equal(0x020B, FontToDQ3r.AsciiToTableCode['A']);
		Assert.Equal(0x0224, FontToDQ3r.AsciiToTableCode['Z']);

		// Lowercase starts at 0x0225
		Assert.Equal(0x0225, FontToDQ3r.AsciiToTableCode['a']);
		Assert.Equal(0x023E, FontToDQ3r.AsciiToTableCode['z']);
	}

	[Fact]
	public void TableCodeToAscii_ReverseMapping() {
		// Verify reverse mapping works
		Assert.Equal(' ', FontToDQ3r.TableCodeToAscii[0x0200]);
		Assert.Equal('A', FontToDQ3r.TableCodeToAscii[0x020B]);
		Assert.Equal('z', FontToDQ3r.TableCodeToAscii[0x023E]);
	}

	[Fact]
	public void GetCharacterWidth_ReturnsCorrectWidths() {
		// Narrow characters
		Assert.Equal(2, FontToDQ3r.GetCharacterWidth('i'));
		Assert.Equal(2, FontToDQ3r.GetCharacterWidth('l'));
		Assert.Equal(2, FontToDQ3r.GetCharacterWidth('.'));

		// Wide characters
		Assert.Equal(8, FontToDQ3r.GetCharacterWidth('M'));
		Assert.Equal(8, FontToDQ3r.GetCharacterWidth('W'));
		Assert.Equal(8, FontToDQ3r.GetCharacterWidth('m'));
		Assert.Equal(8, FontToDQ3r.GetCharacterWidth('w'));

		// Standard width
		Assert.Equal(7, FontToDQ3r.GetCharacterWidth('A'));
		Assert.Equal(6, FontToDQ3r.GetCharacterWidth('a'));
	}

	[Fact]
	public void GetTableCode_ReturnsSpaceForUnknown() {
		// Unknown characters should map to space
		int spaceCode = FontToDQ3r.AsciiToTableCode[' '];
		Assert.Equal(spaceCode, FontToDQ3r.GetTableCode('©')); // Copyright symbol not mapped
	}

	[Fact]
	public void IsControlCode_IdentifiesControlCodes() {
		// Control codes are in 0x00AB-0x00FF range
		Assert.True(FontToDQ3r.IsControlCode(0x00AC)); // END STRING
		Assert.True(FontToDQ3r.IsControlCode(0x00AD)); // LINE
		Assert.True(FontToDQ3r.IsControlCode(0x00AF)); // WAIT
		Assert.True(FontToDQ3r.IsControlCode(0x00B0)); // HERO NAME

		// Regular characters should not be control codes
		Assert.False(FontToDQ3r.IsControlCode(0x0200)); // Space
		Assert.False(FontToDQ3r.IsControlCode(0x020B)); // A
	}

	[Fact]
	public void Create1bppGlyph_ReturnsCorrectSize() {
		// Each glyph should be 12 bytes (8x12 pixels, 1 byte per row)
		byte[] glyphA = FontToDQ3r.Create1bppGlyph('A');
		Assert.Equal(12, glyphA.Length);

		byte[] glyphSpace = FontToDQ3r.Create1bppGlyph(' ');
		Assert.Equal(12, glyphSpace.Length);

		byte[] glyphUnknown = FontToDQ3r.Create1bppGlyph('©');
		Assert.Equal(12, glyphUnknown.Length);
	}

	[Fact]
	public void Create1bppGlyph_SpaceIsEmpty() {
		byte[] glyphSpace = FontToDQ3r.Create1bppGlyph(' ');

		// Space should be all zeros
		foreach (byte b in glyphSpace) {
			Assert.Equal(0, b);
		}
	}

	[Fact]
	public void Create1bppGlyph_LettersHaveContent() {
		byte[] glyphA = FontToDQ3r.Create1bppGlyph('A');

		// Letter A should have some non-zero bytes
		Assert.Contains(glyphA, b => b != 0);
	}

	[Fact]
	public void Convert1bppTo4bpp_OutputsCorrectSize() {
		// 12-byte 1bpp glyph -> 2 tiles @ 32 bytes each = 64 bytes
		byte[] glyph1bpp = FontToDQ3r.Create1bppGlyph('A');
		byte[] glyph4bpp = FontToDQ3r.Convert1bppTo4bpp(glyph1bpp);

		// Should be 2 tiles (ceil(12/8) = 2)
		Assert.Equal(64, glyph4bpp.Length);
	}

	[Fact]
	public void GenerateEnglishFontTiles_GeneratesAllCharacters() {
		var tiles = FontToDQ3r.GenerateEnglishFontTiles();

		// Should have tiles for all mapped characters
		Assert.Equal(FontToDQ3r.AsciiToTableCode.Count, tiles.Count);

		// Verify some specific characters exist
		Assert.True(tiles.ContainsKey(' '));
		Assert.True(tiles.ContainsKey('A'));
		Assert.True(tiles.ContainsKey('z'));
		Assert.True(tiles.ContainsKey('0'));
		Assert.True(tiles.ContainsKey('.'));
	}

	[Fact]
	public void ControlCodes_HaveExpectedValues() {
		Assert.Equal(0x00AC, FontToDQ3r.ControlCodes.EndStringAC);
		Assert.Equal(0x00AD, FontToDQ3r.ControlCodes.NewLine);
		Assert.Equal(0x00AE, FontToDQ3r.ControlCodes.EndStringAE);
		Assert.Equal(0x00AF, FontToDQ3r.ControlCodes.Wait);
		Assert.Equal(0x00B0, FontToDQ3r.ControlCodes.HeroName);
	}
}
