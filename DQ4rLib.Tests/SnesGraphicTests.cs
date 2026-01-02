using DQ4rLib.Models;

namespace DQ4rLib.Tests;

public class SnesGraphicTests {
	[Fact]
	public void Constructor_Default_CreatesEmpty() {
		// Act
		var graphic = new SnesGraphic();

		// Assert
		Assert.Empty(graphic.TileData);
		Assert.Equal(0, graphic.TileCount);
	}

	[Fact]
	public void Constructor_WithData_SetsTileCount() {
		// Arrange - 4 tiles worth of 4bpp data
		byte[] data = new byte[128]; // 4 tiles × 32 bytes

		// Act
		var graphic = new SnesGraphic(data, widthInTiles: 2);

		// Assert
		Assert.Equal(4, graphic.TileCount);
		Assert.Equal(2, graphic.WidthInTiles);
		Assert.Equal(2, graphic.HeightInTiles);
	}

	[Fact]
	public void Width_CalculatesFromTiles() {
		// Arrange
		byte[] data = new byte[256]; // 8 tiles
		var graphic = new SnesGraphic(data, widthInTiles: 4, heightInTiles: 2);

		// Act & Assert
		Assert.Equal(32, graphic.Width);  // 4 tiles × 8 pixels
		Assert.Equal(16, graphic.Height); // 2 tiles × 8 pixels
	}

	[Fact]
	public void TileCount_CalculatesFromDataLength() {
		// Arrange
		byte[] data = new byte[320]; // 10 tiles

		// Act
		var graphic = new SnesGraphic(data);

		// Assert
		Assert.Equal(10, graphic.TileCount);
	}

	[Fact]
	public void HeightInTiles_AutoCalculatesFromWidth() {
		// Arrange - 16 tiles with width of 4
		byte[] data = new byte[512]; // 16 tiles

		// Act
		var graphic = new SnesGraphic(data, widthInTiles: 4, heightInTiles: 0);

		// Assert
		Assert.Equal(4, graphic.HeightInTiles); // 16 / 4 = 4
	}
}
