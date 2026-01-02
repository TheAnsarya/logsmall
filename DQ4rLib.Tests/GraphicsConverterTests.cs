using DQ4rLib.Converters;
using DQ4rLib.Models;

namespace DQ4rLib.Tests;

public class GraphicsConverterTests {
	[Fact]
	public void NesToSnes4bpp_SingleTile_ConvertsCorrectly() {
		// Arrange - NES 2bpp tile (16 bytes)
		// Simple pattern: alternating bits for visual verification
		byte[] nesTile = [
			0b10101010, 0b01010101, 0b10101010, 0b01010101, // Plane 0, rows 0-3
			0b10101010, 0b01010101, 0b10101010, 0b01010101, // Plane 0, rows 4-7
			0b11001100, 0b00110011, 0b11001100, 0b00110011, // Plane 1, rows 0-3
			0b11001100, 0b00110011, 0b11001100, 0b00110011  // Plane 1, rows 4-7
		];

		// Act
		byte[] snesData = GraphicsConverter.NesToSnes4bpp(nesTile);

		// Assert
		Assert.Equal(32, snesData.Length); // 32 bytes per SNES 4bpp tile

		// First 16 bytes should be bitplanes 0,1 interleaved
		Assert.Equal(0b10101010, snesData[0]);  // Row 0, BP0
		Assert.Equal(0b11001100, snesData[1]);  // Row 0, BP1
		Assert.Equal(0b01010101, snesData[2]);  // Row 1, BP0
		Assert.Equal(0b00110011, snesData[3]);  // Row 1, BP1

		// Second 16 bytes should be zeros (BP2,3 for 2bpp source)
		for (int i = 16; i < 32; i++) {
			Assert.Equal(0, snesData[i]);
		}
	}

	[Fact]
	public void NesToSnes4bpp_MultipleTiles_ConvertsAll() {
		// Arrange - 4 tiles worth of NES data
		byte[] nesChr = new byte[64]; // 4 tiles × 16 bytes
		for (int i = 0; i < 64; i++) {
			nesChr[i] = (byte)(i % 256);
		}

		// Act
		byte[] snesData = GraphicsConverter.NesToSnes4bpp(nesChr);

		// Assert
		Assert.Equal(128, snesData.Length); // 4 tiles × 32 bytes
	}

	[Fact]
	public void NesToSnes4bpp_EmptyInput_ReturnsEmpty() {
		// Arrange
		byte[] emptyData = [];

		// Act
		byte[] result = GraphicsConverter.NesToSnes4bpp(emptyData);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void CreateSnesGraphic_SetsCorrectDimensions() {
		// Arrange
		byte[] nesChr = new byte[256]; // 16 tiles

		// Act
		var graphic = GraphicsConverter.CreateSnesGraphic(nesChr, widthInTiles: 4);

		// Assert
		Assert.Equal(16, graphic.TileCount);
		Assert.Equal(4, graphic.WidthInTiles);
		Assert.Equal(4, graphic.HeightInTiles);
		Assert.Equal(32, graphic.Width);
		Assert.Equal(32, graphic.Height);
	}

	[Fact]
	public void ConvertPalette_Creates16ColorPalette() {
		// Arrange
		byte[] nesPalette = [0x0F, 0x00, 0x10, 0x30]; // Black, gray, light, white-ish

		// Act
		var snesPalette = GraphicsConverter.ConvertPalette(nesPalette);

		// Assert
		Assert.Equal(16, snesPalette.ColorCount);
		// First 4 colors should be converted from NES
		// Remaining 12 should be grayscale defaults
	}
}
