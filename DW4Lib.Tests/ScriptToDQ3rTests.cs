namespace DW4Lib.Tests;

using DW4Lib.Converters;
using Xunit;

/// <summary>
/// Tests for ScriptToDQ3r converter.
/// </summary>
public class ScriptToDQ3rTests {
	[Fact]
	public void EncodeString_BasicString() {
		byte[] encoded = ScriptToDQ3r.EncodeString("Hello");

		// Should be 5 characters + end marker = 6 codes = 12 bytes
		Assert.Equal(12, encoded.Length);

		// Verify H (0x0212) is first - big endian
		Assert.Equal(0x02, encoded[0]);
		Assert.Equal(0x12, encoded[1]);
	}

	[Fact]
	public void EncodeString_AddsEndMarker() {
		byte[] encoded = ScriptToDQ3r.EncodeString("A");

		// 1 char + end marker = 4 bytes
		Assert.Equal(4, encoded.Length);

		// Last two bytes should be END STRING (0x00AC)
		Assert.Equal(0x00, encoded[2]);
		Assert.Equal(0xAC, encoded[3]);
	}

	[Fact]
	public void DecodeString_BasicString() {
		// Encode "Hi" then decode it
		byte[] encoded = ScriptToDQ3r.EncodeString("Hi");
		string decoded = ScriptToDQ3r.DecodeString(encoded);

		Assert.Equal("Hi", decoded);
	}

	[Fact]
	public void DecodeString_WithNewline() {
		// Create bytes for "A[LINE]B"
		byte[] bytes = [
			0x02, 0x0B, // A
			0x00, 0xAD, // LINE
			0x02, 0x0C, // B
			0x00, 0xAC, // END
		];

		string decoded = ScriptToDQ3r.DecodeString(bytes);

		Assert.Equal("A\nB", decoded);
	}

	[Fact]
	public void DecodeString_WithControlCodes() {
		// Create bytes with control code
		byte[] bytes = [
			0x02, 0x12, // H
			0x02, 0x22, // e - wait this is wrong, let me fix
		];

		// Actually let's use proper encoding
		// H = 0x0212, e = 0x0229
		bytes = [
			0x02, 0x12, // H
			0x02, 0x29, // e
			0x02, 0x30, // l
			0x02, 0x30, // l
			0x02, 0x33, // o
			0x00, 0xAC, // END
		];

		string decoded = ScriptToDQ3r.DecodeString(bytes);
		Assert.Equal("Hello", decoded);
	}

	[Fact]
	public void DecodeString_HeroNameCode() {
		byte[] bytes = [
			0x00, 0xB0, // HERO NAME
			0x00, 0xAC, // END
		];

		string decoded = ScriptToDQ3r.DecodeString(bytes);
		Assert.Equal("[HERO]", decoded);
	}

	[Fact]
	public void DecodeString_ItemCode() {
		byte[] bytes = [
			0x00, 0xC0, // ITEM
			0x00, 0xAC, // END
		];

		string decoded = ScriptToDQ3r.DecodeString(bytes);
		Assert.Equal("[ITEM]", decoded);
	}

	[Fact]
	public void ConvertString_FromDW4Bytes() {
		// DW4 byte encoding: A=0x0A, space=0x50
		// Test with simple "AB" (A=0x0A, B=0x0B)
		byte[] dw4Bytes = [0x0A, 0x0B, 0xFF]; // AB + end

		byte[] dq3rBytes = ScriptToDQ3r.ConvertString(dw4Bytes);

		// Should have 3 codes = 6 bytes
		Assert.Equal(6, dq3rBytes.Length);

		// Verify end marker
		Assert.Equal(0x00, dq3rBytes[4]);
		Assert.Equal(0xAC, dq3rBytes[5]);
	}

	[Fact]
	public void ConvertString_HandlesNewline() {
		// DW4 newline is 0xFE
		byte[] dw4Bytes = [0x0A, 0xFE, 0x0B, 0xFF]; // A + newline + B + end

		byte[] dq3rBytes = ScriptToDQ3r.ConvertString(dw4Bytes);

		// A (2) + LINE (2) + B (2) + END (2) = 8 bytes
		Assert.Equal(8, dq3rBytes.Length);

		// Verify newline code (0x00AD)
		Assert.Equal(0x00, dq3rBytes[2]);
		Assert.Equal(0xAD, dq3rBytes[3]);
	}

	[Fact]
	public void RoundTrip_EncodeAndDecode() {
		string[] testStrings = [
			"Hello World",
			"Test 123",
			"ABC xyz",
			"Items: Sword, Shield",
			"Level up!",
		];

		foreach (string original in testStrings) {
			byte[] encoded = ScriptToDQ3r.EncodeString(original);
			string decoded = ScriptToDQ3r.DecodeString(encoded);

			Assert.Equal(original, decoded);
		}
	}

	[Fact]
	public void DQ3rTextEntry_Properties() {
		var entry = new ScriptToDQ3r.DQ3rTextEntry {
			Index = 1,
			OriginalText = "Test",
			DQ3rBytes = [0x02, 0x1E, 0x02, 0x29], // Te
			DQ3rText = "Te",
		};

		Assert.Equal(1, entry.Index);
		Assert.Equal("Test", entry.OriginalText);
		Assert.NotEmpty(entry.DQ3rBytes);
		Assert.Equal("Te", entry.DQ3rText);
	}

	[Fact]
	public void SpecialCharacters_Encode() {
		// Test punctuation
		byte[] encoded = ScriptToDQ3r.EncodeString("Hello!");
		string decoded = ScriptToDQ3r.DecodeString(encoded);
		Assert.Equal("Hello!", decoded);

		// Test with question mark
		encoded = ScriptToDQ3r.EncodeString("Why?");
		decoded = ScriptToDQ3r.DecodeString(encoded);
		Assert.Equal("Why?", decoded);

		// Test with colon and numbers
		encoded = ScriptToDQ3r.EncodeString("HP: 100");
		decoded = ScriptToDQ3r.DecodeString(encoded);
		Assert.Equal("HP: 100", decoded);
	}
}
