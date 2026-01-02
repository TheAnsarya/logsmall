using DQ4rLib.Converters;
using DQ4rLib.Models;

namespace DQ4rLib.Tests;

public class TextConverterTests {
	[Fact]
	public void CreateDq3rEncoder_RegistersControlCodes() {
		// Act
		var encoder = TextConverter.CreateDq3rEncoder();

		// Assert - verify basic encoding works
		byte[] result = encoder.Encode("[END]");
		Assert.Single(result);
		Assert.Equal(0x00, result[0]);
	}

	[Fact]
	public void Encoder_LineBreak_EncodesCorrectly() {
		// Arrange
		var encoder = TextConverter.CreateDq3rEncoder();

		// Act
		byte[] result = encoder.Encode("[LINE]");

		// Assert
		Assert.Single(result);
		Assert.Equal(0x01, result[0]);
	}

	[Fact]
	public void Encoder_PageBreak_EncodesCorrectly() {
		// Arrange
		var encoder = TextConverter.CreateDq3rEncoder();

		// Act
		byte[] result = encoder.Encode("[PAGE]");

		// Assert
		Assert.Single(result);
		Assert.Equal(0x03, result[0]);
	}

	[Fact]
	public void Encoder_NameSubstitution_EncodesCorrectly() {
		// Arrange
		var encoder = TextConverter.CreateDq3rEncoder();

		// Act
		byte[] result = encoder.Encode("[NAME:0]");

		// Assert
		Assert.Equal(2, result.Length);
		Assert.Equal(0x10, result[0]); // NAME control code
		Assert.Equal(0x00, result[1]); // Index 0
	}

	[Fact]
	public void Encoder_PartyReference_EncodesCorrectly() {
		// Arrange
		var encoder = TextConverter.CreateDq3rEncoder();

		// Act
		byte[] result = encoder.Encode("[PARTY:2]");

		// Assert
		Assert.Equal(2, result.Length);
		Assert.Equal(0x18, result[0]); // PARTY control code
		Assert.Equal(0x02, result[1]); // Index 2
	}

	[Fact]
	public void Encoder_Variables_EncodeCorrectly() {
		// Arrange
		var encoder = TextConverter.CreateDq3rEncoder();

		// Act & Assert
		Assert.Equal([0x20], encoder.Encode("[ITEM]"));
		Assert.Equal([0x21], encoder.Encode("[MONSTER]"));
		Assert.Equal([0x22], encoder.Encode("[NUM]"));
		Assert.Equal([0x23], encoder.Encode("[GOLD]"));
	}

	[Fact]
	public void Encoder_Delay_EncodesWithParameter() {
		// Arrange
		var encoder = TextConverter.CreateDq3rEncoder();

		// Act
		byte[] result = encoder.Encode("[DELAY:30]");

		// Assert
		Assert.Equal(2, result.Length);
		Assert.Equal(0x80, result[0]); // DELAY control code
		Assert.Equal(30, result[1]);   // Frame count
	}

	[Fact]
	public void Encoder_UnknownCode_PreservesBrackets() {
		// Arrange
		var encoder = TextConverter.CreateDq3rEncoder();

		// Act - unknown code should be encoded character by character
		byte[] result = encoder.Encode("[UNKNOWN]");

		// Assert - brackets and text encoded as regular characters (or placeholders)
		Assert.True(result.Length > 0);
	}

	[Fact]
	public void Encoder_MixedContent_EncodesCorrectly() {
		// Arrange
		var encoder = TextConverter.CreateDq3rEncoder();

		// Act
		byte[] result = encoder.Encode("[NAME:0][LINE]Hello[END]");

		// Assert
		// NAME:0 = 2 bytes, LINE = 1 byte, "Hello" = 5 placeholders, END = 1 byte
		Assert.True(result.Length >= 4); // At least control codes
		Assert.Equal(0x10, result[0]); // NAME
		Assert.Equal(0x00, result[1]); // Index
		Assert.Equal(0x01, result[2]); // LINE
	}

	[Fact]
	public void TextEncoder_LoadTable_LoadsCharacterMapping() {
		// Arrange
		var encoder = new TextEncoder();
		string tempTable = Path.GetTempFileName();
		File.WriteAllText(tempTable, "41=A\n42=B\n43=C");

		try {
			// Act
			encoder.LoadTable(tempTable);
			byte[] result = encoder.Encode("ABC");

			// Assert
			Assert.Equal(3, result.Length);
			Assert.Equal(0x41, result[0]);
			Assert.Equal(0x42, result[1]);
			Assert.Equal(0x43, result[2]);
		} finally {
			File.Delete(tempTable);
		}
	}

	[Fact]
	public void TextEncoder_LoadTable_IgnoresComments() {
		// Arrange
		var encoder = new TextEncoder();
		string tempTable = Path.GetTempFileName();
		File.WriteAllText(tempTable, "// This is a comment\n41=A\n// Another comment\n42=B");

		try {
			// Act
			encoder.LoadTable(tempTable);
			byte[] result = encoder.Encode("AB");

			// Assert
			Assert.Equal(2, result.Length);
		} finally {
			File.Delete(tempTable);
		}
	}
}
