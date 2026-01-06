namespace FFMQLib.Tests;

/// <summary>
/// Tests for FfmqTextDecoder
/// </summary>
public class FfmqTextDecoderTests {
	[Fact]
	public void Decode_UppercaseLetters_ReturnsCorrectString() {
		var decoder = new FfmqTextDecoder();

		// "FIRE" in FFMQ encoding: F=0x9F, I=0xA2, R=0xAB, E=0x9E
		byte[] data = [0x9F, 0xA2, 0xAB, 0x9E, 0x00];

		var result = decoder.Decode(data, 0, 5);

		Assert.Equal("FIRE", result);
	}

	[Fact]
	public void Decode_LowercaseLetters_ReturnsCorrectString() {
		var decoder = new FfmqTextDecoder();

		// "fire" in FFMQ encoding: f=0xB9, i=0xBC, r=0xC5, e=0xB8
		byte[] data = [0xB9, 0xBC, 0xC5, 0xB8, 0x00];

		var result = decoder.Decode(data, 0, 5);

		Assert.Equal("fire", result);
	}

	[Fact]
	public void Decode_MixedCase_ReturnsCorrectString() {
		var decoder = new FfmqTextDecoder();

		// "Fire" in FFMQ encoding: F=0x9F, i=0xBC, r=0xC5, e=0xB8
		byte[] data = [0x9F, 0xBC, 0xC5, 0xB8, 0x00];

		var result = decoder.Decode(data, 0, 5);

		Assert.Equal("Fire", result);
	}

	[Fact]
	public void Decode_WithPadding_StripsTrailingPadding() {
		var decoder = new FfmqTextDecoder();

		// "Fire" + padding (0x03) to fill 12 bytes
		byte[] data = [0x9F, 0xBC, 0xC5, 0xB8, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x00];

		var result = decoder.Decode(data, 0, 12);

		Assert.Equal("Fire", result);
	}

	[Fact]
	public void Decode_WithTerminator_StopsAtTerminator() {
		var decoder = new FfmqTextDecoder();

		// "Hi" terminated early
		byte[] data = [0xA1, 0xBC, 0x00, 0x9F, 0xBC, 0xC5, 0xB8];

		var result = decoder.Decode(data, 0, 7);

		Assert.Equal("Hi", result);
	}

	[Fact]
	public void Decode_Digits_ReturnsCorrectNumbers() {
		var decoder = new FfmqTextDecoder();

		// "12345" in FFMQ encoding: 1=0x91, 2=0x92, etc.
		byte[] data = [0x91, 0x92, 0x93, 0x94, 0x95, 0x00];

		var result = decoder.Decode(data, 0, 6);

		Assert.Equal("12345", result);
	}

	[Fact]
	public void Decode_Punctuation_ReturnsCorrectCharacters() {
		var decoder = new FfmqTextDecoder();

		// ".!?" in FFMQ encoding
		byte[] data = [0xD0, 0xF7, 0xEB, 0x00];

		var result = decoder.Decode(data, 0, 4);

		Assert.Equal(".!?", result);
	}

	[Fact]
	public void Decode_UnknownBytes_ShowsHexPlaceholder() {
		var decoder = new FfmqTextDecoder();

		// Unknown byte 0x10 should show as <10>
		byte[] data = [0x9F, 0x10, 0xBC, 0x00];

		var result = decoder.Decode(data, 0, 4);

		Assert.Equal("F<10>i", result);
	}

	[Fact]
	public void Encode_SimpleText_ReturnsCorrectBytes() {
		var decoder = new FfmqTextDecoder();

		var result = decoder.Encode("Fire", 12);

		Assert.Equal(12, result.Length);
		Assert.Equal(0x9F, result[0]); // F
		Assert.Equal(0xBC, result[1]); // i
		Assert.Equal(0xC5, result[2]); // r
		Assert.Equal(0xB8, result[3]); // e
		Assert.Equal(0x00, result[4]); // Terminator
	}

	[Fact]
	public void Encode_FillsWithPadding() {
		var decoder = new FfmqTextDecoder();

		var result = decoder.Encode("Hi", 8, FfmqTextDecoder.PaddingByte);

		Assert.Equal(8, result.Length);
		Assert.Equal(0xA1, result[0]); // H
		Assert.Equal(0xBC, result[1]); // i
		Assert.Equal(0x00, result[2]); // Terminator
		// Rest should be padding
		for (int i = 3; i < 8; i++) {
			Assert.Equal(FfmqTextDecoder.PaddingByte, result[i]);
		}
	}

	[Fact]
	public void ReadTable_ReturnsCorrectCount() {
		var decoder = new FfmqTextDecoder();

		// Create fake ROM data with 3 entries of 4 bytes each
		byte[] rom = new byte[100];
		// Entry 0: "AB"
		rom[0] = 0x9A; rom[1] = 0x9B; rom[2] = 0x00; rom[3] = 0x03;
		// Entry 1: "CD"
		rom[4] = 0x9C; rom[5] = 0x9D; rom[6] = 0x00; rom[7] = 0x03;
		// Entry 2: "EF"
		rom[8] = 0x9E; rom[9] = 0x9F; rom[10] = 0x00; rom[11] = 0x03;

		var table = new FfmqTextTable("test", 0, 3, 4);
		var results = decoder.ReadTable(rom, table);

		Assert.Equal(3, results.Length);
		Assert.Equal("AB", results[0]);
		Assert.Equal("CD", results[1]);
		Assert.Equal("EF", results[2]);
	}

	[Fact]
	public void ReadEntry_ReturnsCorrectEntry() {
		var decoder = new FfmqTextDecoder();

		// Create fake ROM data
		byte[] rom = new byte[100];
		// Entry at index 2 (offset 8): "XY"
		rom[8] = 0xB1; rom[9] = 0xB2; rom[10] = 0x00; rom[11] = 0x03;

		var table = new FfmqTextTable("test", 0, 5, 4);
		var result = decoder.ReadEntry(rom, table, 2);

		Assert.Equal("XY", result);
	}

	[Fact]
	public void ReadEntry_InvalidIndex_ThrowsException() {
		var decoder = new FfmqTextDecoder();
		byte[] rom = new byte[100];
		var table = new FfmqTextTable("test", 0, 5, 4);

		Assert.Throws<ArgumentOutOfRangeException>(() => decoder.ReadEntry(rom, table, 10));
	}
}
