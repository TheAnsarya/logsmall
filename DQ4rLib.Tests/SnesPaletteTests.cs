using DQ4rLib.Models;

namespace DQ4rLib.Tests;

public class SnesPaletteTests {
	[Fact]
	public void Constructor_CreatesGrayscalePalette() {
		// Act
		var palette = new SnesPalette();

		// Assert
		Assert.Equal(16, palette.ColorCount);
		// First color should be black (0,0,0 in RGB555)
		Assert.Equal(0, palette.Colors[0]);
	}

	[Fact]
	public void Constructor_WithData_ParsesCorrectly() {
		// Arrange - RGB555 colors in little-endian
		byte[] data = [
			0x00, 0x00, // Color 0: Black
			0xFF, 0x7F, // Color 1: White (max RGB555)
			0x1F, 0x00, // Color 2: Pure red
			0xE0, 0x03, // Color 3: Pure green
			0x00, 0x7C, // Color 4: Pure blue
		];

		// Act
		var palette = new SnesPalette(data);

		// Assert
		Assert.Equal(5, palette.ColorCount);
		Assert.Equal(0x0000, palette.Colors[0]); // Black
		Assert.Equal(0x7FFF, palette.Colors[1]); // White
		Assert.Equal(0x001F, palette.Colors[2]); // Red
		Assert.Equal(0x03E0, palette.Colors[3]); // Green
		Assert.Equal(0x7C00, palette.Colors[4]); // Blue
	}

	[Theory]
	[InlineData(0x0000, 0, 0, 0)]       // Black
	[InlineData(0x7FFF, 248, 248, 248)] // White (max shifted)
	[InlineData(0x001F, 248, 0, 0)]     // Pure red
	[InlineData(0x03E0, 0, 248, 0)]     // Pure green
	[InlineData(0x7C00, 0, 0, 248)]     // Pure blue
	public void SnesTo24Bit_ConvertsCorrectly(ushort snesColor, byte expectedR, byte expectedG, byte expectedB) {
		// Act
		var (r, g, b) = SnesPalette.SnesTo24Bit(snesColor);

		// Assert
		Assert.Equal(expectedR, r);
		Assert.Equal(expectedG, g);
		Assert.Equal(expectedB, b);
	}

	[Theory]
	[InlineData(0, 0, 0, 0x0000)]       // Black
	[InlineData(255, 255, 255, 0x7FFF)] // White
	[InlineData(255, 0, 0, 0x001F)]     // Red
	[InlineData(0, 255, 0, 0x03E0)]     // Green
	[InlineData(0, 0, 255, 0x7C00)]     // Blue
	public void RgbToSnes_ConvertsCorrectly(byte r, byte g, byte b, ushort expectedSnes) {
		// Act
		ushort result = SnesPalette.RgbToSnes(r, g, b);

		// Assert
		Assert.Equal(expectedSnes, result);
	}

	[Fact]
	public void RoundTrip_RgbToSnesTo24Bit_PreservesColor() {
		// Arrange - use values that survive the 5-bit quantization
		byte r = 128, g = 64, b = 192;

		// Act
		ushort snes = SnesPalette.RgbToSnes(r, g, b);
		var (r2, g2, b2) = SnesPalette.SnesTo24Bit(snes);

		// Assert - values are quantized to 5 bits then expanded, so allow some loss
		Assert.InRange(r2, r - 8, r + 8);
		Assert.InRange(g2, g - 8, g + 8);
		Assert.InRange(b2, b - 8, b + 8);
	}
}
