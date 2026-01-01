using DW4Lib.Converters;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for GraphicsToDQ3r converter.
/// </summary>
public class GraphicsToDQ3rTests {
	[Fact]
	public void ConvertTile2bppTo4bpp_ValidTile_ReturnsCorrectSize() {
		// Arrange
		var nesTile = new byte[16]; // Valid 2bpp tile (all zeros)

		// Act
		var result = GraphicsToDQ3r.ConvertTile2bppTo4bpp(nesTile);

		// Assert
		Assert.Equal(32, result.Length); // SNES 4bpp = 32 bytes
	}

	[Fact]
	public void ConvertTile2bppTo4bpp_InvalidSize_ThrowsException() {
		// Arrange
		var invalidTile = new byte[15]; // Wrong size

		// Act & Assert
		Assert.Throws<ArgumentException>(() =>
			GraphicsToDQ3r.ConvertTile2bppTo4bpp(invalidTile));
	}

	[Fact]
	public void ConvertTile2bppTo4bpp_PreservesLowerBitplanes() {
		// Arrange - NES tile with pattern in plane 0
		var nesTile = new byte[16];
		nesTile[0] = 0xFF; // Row 0, plane 0 = all pixels have bit 0 set
		nesTile[8] = 0x00; // Row 0, plane 1 = none have bit 1

		// Act
		var result = GraphicsToDQ3r.ConvertTile2bppTo4bpp(nesTile);

		// Assert - SNES interleaved format
		Assert.Equal(0xFF, result[0]); // Row 0, plane 0
		Assert.Equal(0x00, result[1]); // Row 0, plane 1
	}

	[Fact]
	public void ConvertTile2bppTo4bpp_UpperBitplanes_DefaultToZero() {
		// Arrange
		var nesTile = new byte[16];
		nesTile[0] = 0xFF;
		nesTile[8] = 0xFF;

		// Act
		var result = GraphicsToDQ3r.ConvertTile2bppTo4bpp(nesTile);

		// Assert - upper bitplanes (16-31) should be zero by default
		for (int i = 16; i < 32; i++) {
			Assert.Equal(0x00, result[i]);
		}
	}

	[Fact]
	public void ConvertTile2bppTo4bpp_WithPaletteShift_SetsUpperPlanes() {
		// Arrange
		var nesTile = new byte[16];

		// Act - palette shift 1 sets plane 2
		var result = GraphicsToDQ3r.ConvertTile2bppTo4bpp(nesTile, paletteShift: 1);

		// Assert - plane 2 should be 0xFF for all rows
		for (int row = 0; row < 8; row++) {
			Assert.Equal(0xFF, result[16 + row * 2]); // Plane 2
			Assert.Equal(0x00, result[16 + row * 2 + 1]); // Plane 3
		}
	}

	[Fact]
	public void ConvertTileset2bppTo4bpp_MultiTile_ConvertsAll() {
		// Arrange - 3 tiles
		var nesTileset = new byte[48]; // 3 tiles × 16 bytes

		// Act
		var result = GraphicsToDQ3r.ConvertTileset2bppTo4bpp(nesTileset);

		// Assert
		Assert.Equal(96, result.Length); // 3 tiles × 32 bytes
	}

	[Fact]
	public void DecodeNesTile_ReturnsCorrectPixelValues() {
		// Arrange - tile where row 0 has specific bit patterns
		// NES format: plane 0 is low bit, plane 1 is high bit
		// Pixel value = (plane1_bit << 1) | plane0_bit
		var nesTile = new byte[16];
		nesTile[0] = 0b10101010; // Plane 0: pixels 0,2,4,6 have bit0=0; pixels 1,3,5,7 have bit0=1
		nesTile[8] = 0b11001100; // Plane 1: pixels 0,1,4,5 have bit1=1; pixels 2,3,6,7 have bit1=0

		// Calculation for each pixel (reading left to right, bit 7 to bit 0):
		// pixel 0: plane0_bit7=1, plane1_bit7=1 → (1<<1)|1 = 3
		// pixel 1: plane0_bit6=0, plane1_bit6=1 → (1<<1)|0 = 2
		// pixel 2: plane0_bit5=1, plane1_bit5=0 → (0<<1)|1 = 1
		// pixel 3: plane0_bit4=0, plane1_bit4=0 → (0<<1)|0 = 0
		// pixel 4: plane0_bit3=1, plane1_bit3=1 → (1<<1)|1 = 3
		// pixel 5: plane0_bit2=0, plane1_bit2=1 → (1<<1)|0 = 2
		// pixel 6: plane0_bit1=1, plane1_bit1=0 → (0<<1)|1 = 1
		// pixel 7: plane0_bit0=0, plane1_bit0=0 → (0<<1)|0 = 0

		// Act
		var pixels = GraphicsToDQ3r.DecodeNesTile(nesTile);

		// Assert - row 0
		Assert.Equal(3, pixels[0, 0]); // bit0=1, bit1=1
		Assert.Equal(2, pixels[0, 1]); // bit0=0, bit1=1
		Assert.Equal(1, pixels[0, 2]); // bit0=1, bit1=0
		Assert.Equal(0, pixels[0, 3]); // bit0=0, bit1=0
		Assert.Equal(3, pixels[0, 4]); // bit0=1, bit1=1
		Assert.Equal(2, pixels[0, 5]); // bit0=0, bit1=1
		Assert.Equal(1, pixels[0, 6]); // bit0=1, bit1=0
		Assert.Equal(0, pixels[0, 7]); // bit0=0, bit1=0
	}

	[Fact]
	public void DecodeSnesTile_ReturnsCorrectPixelValues() {
		// Arrange - SNES 4bpp tile
		var snesTile = new byte[32];
		snesTile[0] = 0b10000000;  // Plane 0, row 0: pixel 0 has bit 0
		snesTile[1] = 0b10000000;  // Plane 1, row 0: pixel 0 has bit 1
		snesTile[16] = 0b10000000; // Plane 2, row 0: pixel 0 has bit 2
		snesTile[17] = 0b10000000; // Plane 3, row 0: pixel 0 has bit 3

		// Act
		var pixels = GraphicsToDQ3r.DecodeSnesTile(snesTile);

		// Assert - pixel 0 should have all 4 bits = 15
		Assert.Equal(15, pixels[0, 0]);
		// Other pixels should be 0
		Assert.Equal(0, pixels[0, 1]);
	}

	[Fact]
	public void EncodeSnesTile_RoundTrip_PreservesData() {
		// Arrange - create test pixel data
		var originalPixels = new byte[8, 8];
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				originalPixels[y, x] = (byte)((y + x) % 16);
			}
		}

		// Act - encode then decode
		var encoded = GraphicsToDQ3r.EncodeSnesTile(originalPixels);
		var decoded = GraphicsToDQ3r.DecodeSnesTile(encoded);

		// Assert - should match original
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				Assert.Equal(originalPixels[y, x], decoded[y, x]);
			}
		}
	}

	[Fact]
	public void EncodeNesTile_RoundTrip_PreservesData() {
		// Arrange - create test pixel data (2bpp = 0-3 values only)
		var originalPixels = new byte[8, 8];
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				originalPixels[y, x] = (byte)((y + x) % 4);
			}
		}

		// Act - encode then decode
		var encoded = GraphicsToDQ3r.EncodeNesTile(originalPixels);
		var decoded = GraphicsToDQ3r.DecodeNesTile(encoded);

		// Assert - should match original (masked to 2 bits)
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				Assert.Equal(originalPixels[y, x] & 0x03, decoded[y, x]);
			}
		}
	}

	[Fact]
	public void ConvertAndDecode_PreservesNesPixels() {
		// Arrange - NES tile with all possible values
		var originalPixels = new byte[8, 8];
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				originalPixels[y, x] = (byte)((y * 2 + x) % 4);
			}
		}
		var nesTile = GraphicsToDQ3r.EncodeNesTile(originalPixels);

		// Act - convert to SNES and decode
		var snesTile = GraphicsToDQ3r.ConvertTile2bppTo4bpp(nesTile);
		var snesPixels = GraphicsToDQ3r.DecodeSnesTile(snesTile);

		// Assert - lower 2 bits should match
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				Assert.Equal(originalPixels[y, x] & 0x03, snesPixels[y, x] & 0x03);
			}
		}
	}
}

/// <summary>
/// Unit tests for PaletteToDQ3r converter.
/// </summary>
public class PaletteToDQ3rTests {
	[Fact]
	public void NesPalette_Has64Colors() {
		Assert.Equal(64, PaletteToDQ3r.NesPalette.Length);
	}

	[Fact]
	public void NesColorToSnes_BlackColor_ReturnsZero() {
		// NES color 0x0F is black (0,0,0)
		var result = PaletteToDQ3r.NesColorToSnes(0x0F);
		Assert.Equal(0, result);
	}

	[Fact]
	public void NesColorToSnes_WhiteColor_ReturnsMaxBGR() {
		// NES color 0x20 or 0x30 is white (255,255,255)
		var result = PaletteToDQ3r.NesColorToSnes(0x20);

		// SNES 15-bit: should have max values (31,31,31) in BGR
		// bbbbb ggggg rrrrr = 11111 11111 11111 = 0x7FFF
		Assert.Equal(0x7FFF, result);
	}

	[Fact]
	public void NesColorToSnes_OutOfRange_ReturnsBlack() {
		// Invalid index should return black
		var result = PaletteToDQ3r.NesColorToSnes(0xFF);
		// Should map to fallback black (index 0x0F)
		Assert.Equal(PaletteToDQ3r.NesColorToSnes(0x0F), result);
	}

	[Fact]
	public void ConvertNesPaletteTo4bpp_Returns16Colors() {
		// Arrange
		var nesPalette = new byte[] { 0x0F, 0x00, 0x10, 0x30 }; // 4 NES colors

		// Act
		var result = PaletteToDQ3r.ConvertNesPaletteTo4bpp(nesPalette);

		// Assert
		Assert.Equal(16, result.Length);
	}

	[Fact]
	public void ConvertNesPaletteTo4bpp_InvalidSize_ThrowsException() {
		// Arrange
		var invalidPalette = new byte[] { 0x00, 0x10 }; // Wrong size

		// Act & Assert
		Assert.Throws<ArgumentException>(() =>
			PaletteToDQ3r.ConvertNesPaletteTo4bpp(invalidPalette));
	}

	[Fact]
	public void ConvertNesPaletteTo4bpp_FirstColorIsTransparent() {
		// Arrange
		var nesPalette = new byte[] { 0x10, 0x20, 0x30, 0x0F };

		// Act
		var result = PaletteToDQ3r.ConvertNesPaletteTo4bpp(nesPalette);

		// Assert - position 0 should be transparent (0)
		Assert.Equal(0, result[0]);
	}

	[Fact]
	public void ConvertFullNesPalette_Returns256Colors() {
		// Arrange - 32 bytes (4 BG palettes + 4 sprite palettes)
		var nesPaletteData = new byte[32];

		// Act
		var result = PaletteToDQ3r.ConvertFullNesPalette(nesPaletteData);

		// Assert
		Assert.Equal(256, result.Length);
	}

	[Fact]
	public void ConvertFullNesPalette_InvalidSize_ThrowsException() {
		// Arrange
		var invalidData = new byte[16]; // Wrong size

		// Act & Assert
		Assert.Throws<ArgumentException>(() =>
			PaletteToDQ3r.ConvertFullNesPalette(invalidData));
	}
}
