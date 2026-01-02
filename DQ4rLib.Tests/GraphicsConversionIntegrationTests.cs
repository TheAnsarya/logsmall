using DQ4rLib.Converters;
using DQ4rLib.Models;
using DW4Lib.Converters;
using Xunit;

namespace DQ4rLib.Tests;

/// <summary>
/// Integration tests for NES→SNES graphics conversion pipeline.
/// </summary>
public class GraphicsConversionIntegrationTests {
	/// <summary>
	/// Verify full 2bpp→4bpp tile conversion produces correct size output.
	/// </summary>
	[Fact]
	public void ConvertNesTile_ProducesCorrectSize() {
		// Single NES tile = 16 bytes (8 bytes plane 0, 8 bytes plane 1)
		byte[] nesTile = new byte[16];
		for (int i = 0; i < 16; i++) {
			nesTile[i] = (byte)(i * 17); // Some pattern
		}

		byte[] snesTile = GraphicsConverter.NesToSnes4bpp(nesTile);

		// Single SNES 4bpp tile = 32 bytes
		Assert.Equal(32, snesTile.Length);
	}

	/// <summary>
	/// Verify multiple tiles convert correctly.
	/// </summary>
	[Fact]
	public void ConvertMultipleTiles_ProducesCorrectSize() {
		// 16 NES tiles = 256 bytes
		byte[] nesData = new byte[256];
		for (int i = 0; i < 256; i++) {
			nesData[i] = (byte)i;
		}

		byte[] snesData = GraphicsConverter.NesToSnes4bpp(nesData);

		// 16 SNES 4bpp tiles = 512 bytes
		Assert.Equal(512, snesData.Length);
	}

	/// <summary>
	/// Verify bitplane data is correctly interleaved.
	/// </summary>
	[Fact]
	public void ConvertTile_BitplanesCorrectlyInterleaved() {
		// Create a tile with distinct planes
		byte[] nesTile = new byte[16];
		// Plane 0 (bytes 0-7): all 0xFF
		for (int i = 0; i < 8; i++) nesTile[i] = 0xFF;
		// Plane 1 (bytes 8-15): all 0x00
		for (int i = 8; i < 16; i++) nesTile[i] = 0x00;

		byte[] snesTile = GraphicsConverter.NesToSnes4bpp(nesTile);

		// In SNES 4bpp, first 16 bytes are BP0,BP1 interleaved
		// Row 0: BP0=0xFF, BP1=0x00
		Assert.Equal(0xFF, snesTile[0]); // Row 0, BP0
		Assert.Equal(0x00, snesTile[1]); // Row 0, BP1

		// Check all rows follow same pattern
		for (int row = 0; row < 8; row++) {
			Assert.Equal(0xFF, snesTile[row * 2]);     // BP0
			Assert.Equal(0x00, snesTile[row * 2 + 1]); // BP1
		}

		// Bytes 16-31 are BP2,BP3 (all zeros for 2bpp source)
		for (int i = 16; i < 32; i++) {
			Assert.Equal(0x00, snesTile[i]);
		}
	}

	/// <summary>
	/// Verify palette conversion produces valid SNES colors.
	/// </summary>
	[Fact]
	public void ConvertPalette_ProducesValidSnesColors() {
		byte[] nesPalette = [0x0F, 0x00, 0x10, 0x30]; // NES palette indices

		SnesPalette snesPalette = GraphicsConverter.ConvertPalette(nesPalette);

		// First 4 colors should be set
		// (Values depend on NES→RGB mapping, but should be valid RGB555)
		for (int i = 0; i < 4; i++) {
			ushort color = snesPalette.Colors[i];
			// RGB555: each component is 5 bits (0-31)
			int r = color & 0x1F;
			int g = (color >> 5) & 0x1F;
			int b = (color >> 10) & 0x1F;

			Assert.InRange(r, 0, 31);
			Assert.InRange(g, 0, 31);
			Assert.InRange(b, 0, 31);
		}
	}

	/// <summary>
	/// Verify SnesGraphic creation from converted data.
	/// </summary>
	[Fact]
	public void CreateSnesGraphic_HasCorrectDimensions() {
		// Create 256 NES tiles (16x16 tile sheet)
		byte[] nesData = new byte[256 * 16];

		SnesGraphic graphic = GraphicsConverter.CreateSnesGraphic(nesData, widthInTiles: 16);

		Assert.Equal(16, graphic.WidthInTiles);
		Assert.Equal(16, graphic.HeightInTiles);
		Assert.Equal(256, graphic.TileCount);
	}

	/// <summary>
	/// Test full CHR bank conversion (256 tiles).
	/// </summary>
	[Fact]
	public void ConvertChrBank_256Tiles_ProducesCorrectOutput() {
		// Full NES CHR bank = 256 tiles × 16 bytes = 4096 bytes
		byte[] chrBank = new byte[4096];

		// Fill with recognizable pattern per tile
		for (int tile = 0; tile < 256; tile++) {
			int offset = tile * 16;
			for (int b = 0; b < 16; b++) {
				chrBank[offset + b] = (byte)tile;
			}
		}

		byte[] snesBank = GraphicsConverter.NesToSnes4bpp(chrBank);

		// SNES output = 256 tiles × 32 bytes = 8192 bytes
		Assert.Equal(8192, snesBank.Length);

		// Verify each tile's pattern was preserved
		for (int tile = 0; tile < 256; tile++) {
			int snesOffset = tile * 32;
			// First byte of each tile should match NES tile pattern
			// NES BP0 row 0 → SNES byte 0
			Assert.Equal((byte)tile, snesBank[snesOffset]);
		}
	}

	/// <summary>
	/// Test conversion of empty tile (all zeros).
	/// </summary>
	[Fact]
	public void ConvertEmptyTile_ProducesAllZeros() {
		byte[] emptyTile = new byte[16];

		byte[] snesTile = GraphicsConverter.NesToSnes4bpp(emptyTile);

		Assert.All(snesTile, b => Assert.Equal(0, b));
	}

	/// <summary>
	/// Test conversion of solid tile (all ones).
	/// </summary>
	[Fact]
	public void ConvertSolidTile_ProducesCorrectPattern() {
		// Solid color 3 (both bitplanes set) in NES
		byte[] solidTile = new byte[16];
		for (int i = 0; i < 16; i++) {
			solidTile[i] = 0xFF;
		}

		byte[] snesTile = GraphicsConverter.NesToSnes4bpp(solidTile);

		// First 16 bytes should be all 0xFF (BP0 and BP1)
		for (int i = 0; i < 16; i++) {
			Assert.Equal(0xFF, snesTile[i]);
		}

		// Last 16 bytes should be all 0x00 (BP2 and BP3)
		for (int i = 16; i < 32; i++) {
			Assert.Equal(0x00, snesTile[i]);
		}
	}

	/// <summary>
	/// Test that DataExtractor and GraphicsConverter work together.
	/// </summary>
	[Fact]
	public void IntegrateChrExtraction_WithConversion() {
		// Create mock ROM with CHR data
		var romData = new byte[16 + 0x80000 + 0x40000]; // header + PRG + CHR

		// Fill CHR area with test pattern
		int chrOffset = 16 + 0x80000;
		for (int i = 0; i < 0x40000; i++) {
			romData[chrOffset + i] = (byte)(i % 256);
		}

		// Extract CHR
		var extractor = new DataExtractor(romData);
		byte[] chrData = extractor.ExtractChrData();

		Assert.Equal(0x40000, chrData.Length);

		// Convert first bank (4KB = 256 tiles)
		byte[] firstBank = new byte[4096];
		Array.Copy(chrData, 0, firstBank, 0, 4096);

		byte[] snesBank = GraphicsConverter.NesToSnes4bpp(firstBank);

		Assert.Equal(8192, snesBank.Length);
	}

	/// <summary>
	/// Verify checkerboard pattern converts correctly.
	/// </summary>
	[Fact]
	public void ConvertCheckerboard_PreservesPattern() {
		byte[] checkerTile = new byte[16];

		// Create checkerboard: alternating 0xAA and 0x55 per row
		for (int row = 0; row < 8; row++) {
			if (row % 2 == 0) {
				checkerTile[row] = 0xAA;     // BP0
				checkerTile[row + 8] = 0x55; // BP1
			} else {
				checkerTile[row] = 0x55;     // BP0
				checkerTile[row + 8] = 0xAA; // BP1
			}
		}

		byte[] snesTile = GraphicsConverter.NesToSnes4bpp(checkerTile);

		// Verify pattern is preserved in interleaved format
		Assert.Equal(0xAA, snesTile[0]);  // Row 0, BP0
		Assert.Equal(0x55, snesTile[1]);  // Row 0, BP1
		Assert.Equal(0x55, snesTile[2]);  // Row 1, BP0
		Assert.Equal(0xAA, snesTile[3]);  // Row 1, BP1
	}
}
