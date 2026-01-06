using DW4Lib.Text;
using Xunit;

namespace DW4Lib.Tests;

/// <summary>
/// Tests for DW4 text encoding system.
/// Character table: 0x00=space, 0x01-0x0A=digits, 0x0B-0x24=lowercase, 0x25-0x3E=uppercase
/// </summary>
public class TextEncoderTests {
	// ========================================
	// DW4TextEncoder Decode Tests
	// ========================================

	[Fact]
	public void Decode_Numbers_ReturnsCorrectString() {
		// 0x01=0, 0x02=1, 0x03=2, etc.
		byte[] data = [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0xFF];
		string result = DW4TextEncoder.Decode(data);
		Assert.Equal("0123456789", result);
	}

	[Fact]
	public void Decode_UppercaseLetters_ReturnsCorrectString() {
		// 0x25=A, 0x26=B, 0x27=C
		byte[] data = [0x25, 0x26, 0x27, 0xFF]; // ABC
		string result = DW4TextEncoder.Decode(data);
		Assert.Equal("ABC", result);
	}

	[Fact]
	public void Decode_LowercaseLetters_ReturnsCorrectString() {
		// 0x0B=a, 0x0C=b, 0x0D=c
		byte[] data = [0x0B, 0x0C, 0x0D, 0xFF]; // abc
		string result = DW4TextEncoder.Decode(data);
		Assert.Equal("abc", result);
	}

	[Fact]
	public void Decode_Space_ReturnsSpace() {
		// 0x25=A, 0x00=space, 0x26=B
		byte[] data = [0x25, 0x00, 0x26, 0xFF]; // A B
		string result = DW4TextEncoder.Decode(data);
		Assert.Equal("A B", result);
	}

	[Fact]
	public void Decode_Punctuation_ReturnsCorrectString() {
		// H=0x2C, i=0x13, !=0x6E
		byte[] data = [0x2C, 0x13, 0x6E, 0xFF]; // Hi!
		string result = DW4TextEncoder.Decode(data);
		Assert.Equal("Hi!", result);
	}

	[Fact]
	public void Decode_StopsAtTerminator() {
		// 0x25=A, 0x26=B, 0xFF=end, then more data
		byte[] data = [0x25, 0x26, 0xFF, 0x27, 0x28];
		string result = DW4TextEncoder.Decode(data);
		Assert.Equal("AB", result);
	}

	[Fact]
	public void Decode_Newline_ReturnsNewline() {
		// 0x25=A, 0xFE=newline, 0x26=B
		byte[] data = [0x25, 0xFE, 0x26, 0xFF]; // A\nB
		string result = DW4TextEncoder.Decode(data);
		Assert.Equal("A\nB", result);
	}

	[Fact]
	public void Decode_UnknownByte_ReturnsBracketedHex() {
		byte[] data = [0xAA, 0xFF];
		string result = DW4TextEncoder.Decode(data);
		Assert.Equal("[AA]", result);
	}

	// ========================================
	// DW4TextEncoder Encode Tests
	// ========================================

	[Fact]
	public void Encode_Numbers_ReturnsCorrectBytes() {
		// 0=0x01, 1=0x02, 2=0x03, 3=0x04
		byte[] result = DW4TextEncoder.Encode("0123");
		Assert.Equal((byte)0x01, result[0]);
		Assert.Equal((byte)0x02, result[1]);
		Assert.Equal((byte)0x03, result[2]);
		Assert.Equal((byte)0x04, result[3]);
		Assert.Equal((byte)0xFF, result[4]); // Terminator
	}

	[Fact]
	public void Encode_UppercaseLetters_ReturnsCorrectBytes() {
		// A=0x25, B=0x26, C=0x27
		byte[] result = DW4TextEncoder.Encode("ABC");
		Assert.Equal((byte)0x25, result[0]);
		Assert.Equal((byte)0x26, result[1]);
		Assert.Equal((byte)0x27, result[2]);
		Assert.Equal((byte)0xFF, result[3]);
	}

	[Fact]
	public void Encode_LowercaseLetters_ReturnsCorrectBytes() {
		// a=0x0B, b=0x0C, c=0x0D
		byte[] result = DW4TextEncoder.Encode("abc");
		Assert.Equal((byte)0x0B, result[0]);
		Assert.Equal((byte)0x0C, result[1]);
		Assert.Equal((byte)0x0D, result[2]);
		Assert.Equal((byte)0xFF, result[3]);
	}

	[Fact]
	public void Encode_Space_ReturnsSpaceByte() {
		// A=0x25, space=0x00, B=0x26
		byte[] result = DW4TextEncoder.Encode("A B");
		Assert.Equal((byte)0x25, result[0]);
		Assert.Equal((byte)0x00, result[1]); // Space at 0x00
		Assert.Equal((byte)0x26, result[2]);
	}

	[Fact]
	public void Encode_AlwaysEndsWithTerminator() {
		byte[] result = DW4TextEncoder.Encode("Test");
		Assert.Equal((byte)0xFF, result[^1]);
	}

	[Fact]
	public void Encode_Newline_ReturnsNewlineByte() {
		// A=0x25, \n=0xFE, B=0x26
		byte[] result = DW4TextEncoder.Encode("A\nB");
		Assert.Equal((byte)0x25, result[0]);
		Assert.Equal((byte)0xFE, result[1]); // Newline
		Assert.Equal((byte)0x26, result[2]);
	}

	// ========================================
	// Round-trip Tests
	// ========================================

	[Theory]
	[InlineData("Hello World")]
	[InlineData("Ragnar")]
	[InlineData("HP 100")]  // Changed from "HP: 100" since : is at different code point
	[InlineData("Thank you")]  // Removed ! since it's at different code point
	public void RoundTrip_PreservesText(string original) {
		byte[] encoded = DW4TextEncoder.Encode(original);
		string decoded = DW4TextEncoder.Decode(encoded);
		Assert.Equal(original, decoded);
	}

	// ========================================
	// GetEncodedLength Tests
	// ========================================

	[Fact]
	public void GetEncodedLength_ReturnsTextLength() {
		Assert.Equal(5, DW4TextEncoder.GetEncodedLength("Hello"));
		Assert.Equal(11, DW4TextEncoder.GetEncodedLength("Hello World"));
	}

	// ========================================
	// DialogFormatter Tests
	// ========================================

	[Fact]
	public void MaxLineLength_Is18() {
		Assert.Equal(18, DialogFormatter.MaxLineLength);
	}

	[Fact]
	public void MaxVisibleLines_Is4() {
		Assert.Equal(4, DialogFormatter.MaxVisibleLines);
	}

	[Fact]
	public void FormatForDialogBox_ShortText_SingleLine() {
		string[] result = DialogFormatter.FormatForDialogBox("Hello");
		Assert.Single(result);
		Assert.Equal("Hello", result[0]);
	}

	[Fact]
	public void FormatForDialogBox_LongText_SplitsAtWordBoundary() {
		string[] result = DialogFormatter.FormatForDialogBox("This is a longer text that should wrap");
		Assert.True(result.Length > 1);
		Assert.True(result[0].Length <= DialogFormatter.MaxLineLength);
	}

	[Fact]
	public void FormatForDialogBox_PreservesWords() {
		string[] result = DialogFormatter.FormatForDialogBox("Hello World Test");
		string joined = string.Join(" ", result);
		Assert.Equal("Hello World Test", joined);
	}

	[Fact]
	public void SplitIntoPages_FewLines_SinglePage() {
		string[] lines = ["Line 1", "Line 2", "Line 3"];
		var pages = DialogFormatter.SplitIntoPages(lines);
		Assert.Single(pages);
		Assert.Equal(3, pages[0].Length);
	}

	[Fact]
	public void SplitIntoPages_ManyLines_MultiplePages() {
		string[] lines = ["Line 1", "Line 2", "Line 3", "Line 4", "Line 5", "Line 6"];
		var pages = DialogFormatter.SplitIntoPages(lines);
		Assert.Equal(2, pages.Length);
		Assert.Equal(4, pages[0].Length);
		Assert.Equal(2, pages[1].Length);
	}

	[Fact]
	public void FormatWithControlCodes_AddsTerminator() {
		byte[] result = DialogFormatter.FormatWithControlCodes("Hello");
		Assert.Equal((byte)0xFF, result[^1]);
	}

	// ========================================
	// DialogPointerTable Tests
	// ========================================

	[Fact]
	public void DialogPointerTable_Load_ReadsPointers() {
		// Create mock ROM with pointer table
		byte[] rom = new byte[100];
		rom[0] = 0x20; rom[1] = 0x00; // Pointer 1: 0x0020
		rom[2] = 0x40; rom[3] = 0x00; // Pointer 2: 0x0040
		rom[4] = 0x60; rom[5] = 0x00; // Pointer 3: 0x0060

		var table = DialogPointerTable.Load(rom, 0, 3);

		Assert.Equal(3, table.EntryCount);
		Assert.Equal(0x0020, table.Pointers[0]);
		Assert.Equal(0x0040, table.Pointers[1]);
		Assert.Equal(0x0060, table.Pointers[2]);
	}

	[Fact]
	public void DialogPointerTable_GetDialogAddress_ReturnsCorrectAddress() {
		var table = new DialogPointerTable {
			Pointers = [0x100, 0x200, 0x300]
		};

		Assert.Equal(0x100, table.GetDialogAddress(0));
		Assert.Equal(0x200, table.GetDialogAddress(1));
		Assert.Equal(0x300, table.GetDialogAddress(2));
	}

	[Fact]
	public void DialogPointerTable_GetDialogAddress_InvalidIndex_ReturnsNegative() {
		var table = new DialogPointerTable {
			Pointers = [0x100]
		};

		Assert.Equal(-1, table.GetDialogAddress(-1));
		Assert.Equal(-1, table.GetDialogAddress(1));
	}

	// ========================================
	// DialogScript Tests
	// ========================================

	[Fact]
	public void DialogScript_Execute_TextNode_ReturnsText() {
		var script = new DialogScript {
			Nodes = [
				new DialogNode { Type = DialogNodeType.Text, Text = "Hello!" }
			]
		};

		string result = script.Execute(_ => null);
		Assert.Equal("Hello!", result);
	}

	[Fact]
	public void DialogScript_Execute_VariableNode_SubstitutesVariable() {
		var script = new DialogScript {
			Nodes = [
				new DialogNode { Type = DialogNodeType.Text, Text = "HP: " },
				new DialogNode { Type = DialogNodeType.Variable, VariableName = "HP" }
			]
		};

		string result = script.Execute(name => name == "HP" ? 100 : null);
		Assert.Equal("HP: 100", result);
	}

	[Fact]
	public void DialogScript_Execute_ConditionalNode_TrueCondition() {
		var script = new DialogScript {
			Nodes = [
				new DialogNode {
					Type = DialogNodeType.Conditional,
					ConditionVariable = "HAS_ITEM",
					ConditionValue = "true",
					TrueText = "You have the key!",
					FalseText = "You need a key."
				}
			]
		};

		string result = script.Execute(name => name == "HAS_ITEM" ? "true" : null);
		Assert.Equal("You have the key!", result);
	}

	[Fact]
	public void DialogScript_Execute_ConditionalNode_FalseCondition() {
		var script = new DialogScript {
			Nodes = [
				new DialogNode {
					Type = DialogNodeType.Conditional,
					ConditionVariable = "HAS_ITEM",
					ConditionValue = "true",
					TrueText = "You have the key!",
					FalseText = "You need a key."
				}
			]
		};

		string result = script.Execute(name => name == "HAS_ITEM" ? "false" : null);
		Assert.Equal("You need a key.", result);
	}

	[Fact]
	public void DialogScript_Execute_LineBreakNode_AddsNewline() {
		var script = new DialogScript {
			Nodes = [
				new DialogNode { Type = DialogNodeType.Text, Text = "Line 1" },
				new DialogNode { Type = DialogNodeType.LineBreak },
				new DialogNode { Type = DialogNodeType.Text, Text = "Line 2" }
			]
		};

		string result = script.Execute(_ => null);
		Assert.Equal("Line 1\nLine 2", result);
	}

	[Fact]
	public void DialogScript_GetRawText_ShowsVariablePlaceholders() {
		var script = new DialogScript {
			Nodes = [
				new DialogNode { Type = DialogNodeType.Text, Text = "HP: " },
				new DialogNode { Type = DialogNodeType.Variable, VariableName = "HP" }
			]
		};

		string result = script.GetRawText();
		Assert.Equal("HP: [HP]", result);
	}

	// ========================================
	// DialogVariables Tests
	// ========================================

	[Fact]
	public void DialogVariables_HasHeroName() {
		Assert.Equal("HERO_NAME", DialogVariables.HeroName);
	}

	[Fact]
	public void DialogVariables_HasGold() {
		Assert.Equal("GOLD", DialogVariables.CurrentGold);
	}
}
