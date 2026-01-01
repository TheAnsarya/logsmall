namespace DW4Lib.Converters;

/// <summary>
/// Converts NES 2bpp (2 bits per pixel) graphics to SNES 4bpp (4 bits per pixel) format.
/// NES tiles are 8x8 pixels, 16 bytes each (2 planes).
/// SNES tiles are 8x8 pixels, 32 bytes each (4 planes).
/// </summary>
public static class GraphicsToDQ3r {
	/// <summary>
	/// NES tile size in bytes (8x8 pixels, 2bpp = 16 bytes).
	/// </summary>
	public const int NesTileSize = 16;

	/// <summary>
	/// SNES tile size in bytes (8x8 pixels, 4bpp = 32 bytes).
	/// </summary>
	public const int SnesTileSize = 32;

	/// <summary>
	/// Convert a single NES 2bpp tile to SNES 4bpp format.
	/// </summary>
	/// <param name="nesTile">16-byte NES tile data</param>
	/// <param name="paletteShift">Bit shift for palette selection (0-3)</param>
	/// <returns>32-byte SNES tile data</returns>
	public static byte[] ConvertTile2bppTo4bpp(byte[] nesTile, int paletteShift = 0) {
		if (nesTile.Length != NesTileSize) {
			throw new ArgumentException($"NES tile must be {NesTileSize} bytes, got {nesTile.Length}");
		}

		// NES 2bpp format:
		// Bytes 0-7:  Bit plane 0 (low bit of each pixel)
		// Bytes 8-15: Bit plane 1 (high bit of each pixel)

		// SNES 4bpp format (interleaved):
		// Bytes 0,1:   Row 0 planes 0,1
		// Bytes 2,3:   Row 1 planes 0,1
		// ...
		// Bytes 14,15: Row 7 planes 0,1
		// Bytes 16,17: Row 0 planes 2,3
		// Bytes 18,19: Row 1 planes 2,3
		// ...
		// Bytes 30,31: Row 7 planes 2,3

		var snesTile = new byte[SnesTileSize];

		// Copy NES planes 0 and 1 to SNES first bitplane pair
		for (int row = 0; row < 8; row++) {
			// Plane 0 (low bit)
			snesTile[row * 2] = nesTile[row];
			// Plane 1 (next bit)
			snesTile[row * 2 + 1] = nesTile[row + 8];
		}

		// SNES planes 2 and 3 are set to 0 by default (all zeros)
		// This means the upper 2 bits of each pixel are 0
		// The paletteShift can be used to set which palette row to use

		// If paletteShift > 0, we need to set the upper planes
		// to select a specific palette row
		if (paletteShift > 0 && paletteShift <= 3) {
			// For palette selection via upper planes:
			// paletteShift 1: plane 2 = 0xFF (all pixels have bit 2 set)
			// paletteShift 2: plane 3 = 0xFF (all pixels have bit 3 set)
			// paletteShift 3: both planes 2 and 3 = 0xFF

			// This is a simplified approach - real usage would set individual pixels
			// based on the original NES palette attributes

			for (int row = 0; row < 8; row++) {
				if ((paletteShift & 1) != 0) {
					snesTile[16 + row * 2] = 0xFF;     // Plane 2
				}
				if ((paletteShift & 2) != 0) {
					snesTile[16 + row * 2 + 1] = 0xFF; // Plane 3
				}
			}
		}

		return snesTile;
	}

	/// <summary>
	/// Convert multiple NES tiles to SNES format.
	/// </summary>
	public static byte[] ConvertTileset2bppTo4bpp(byte[] nesTileset, int paletteShift = 0) {
		int tileCount = nesTileset.Length / NesTileSize;
		var snesTileset = new byte[tileCount * SnesTileSize];

		for (int i = 0; i < tileCount; i++) {
			var nesTile = new byte[NesTileSize];
			Array.Copy(nesTileset, i * NesTileSize, nesTile, 0, NesTileSize);

			var snesTile = ConvertTile2bppTo4bpp(nesTile, paletteShift);
			Array.Copy(snesTile, 0, snesTileset, i * SnesTileSize, SnesTileSize);
		}

		return snesTileset;
	}

	/// <summary>
	/// Convert NES tile with per-tile palette attribute.
	/// </summary>
	public static byte[] ConvertTileWithAttribute(byte[] nesTile, byte attributeByte, int quadrant) {
		// NES attributes are 2 bits per 16x16 pixel area (4 tiles)
		// quadrant: 0 = top-left, 1 = top-right, 2 = bottom-left, 3 = bottom-right
		int paletteIndex = (attributeByte >> (quadrant * 2)) & 0x03;
		return ConvertTile2bppTo4bpp(nesTile, paletteIndex);
	}

	/// <summary>
	/// Decode a single row of pixels from NES 2bpp to individual pixel values.
	/// </summary>
	public static byte[] DecodeNesRow(byte plane0, byte plane1) {
		var pixels = new byte[8];
		for (int x = 0; x < 8; x++) {
			int shift = 7 - x;
			int bit0 = (plane0 >> shift) & 1;
			int bit1 = (plane1 >> shift) & 1;
			pixels[x] = (byte)((bit1 << 1) | bit0);
		}
		return pixels;
	}

	/// <summary>
	/// Decode entire NES tile to 8x8 pixel array.
	/// </summary>
	public static byte[,] DecodeNesTile(byte[] nesTile) {
		var pixels = new byte[8, 8];
		for (int row = 0; row < 8; row++) {
			byte plane0 = nesTile[row];
			byte plane1 = nesTile[row + 8];
			for (int x = 0; x < 8; x++) {
				int shift = 7 - x;
				int bit0 = (plane0 >> shift) & 1;
				int bit1 = (plane1 >> shift) & 1;
				pixels[row, x] = (byte)((bit1 << 1) | bit0);
			}
		}
		return pixels;
	}

	/// <summary>
	/// Decode SNES 4bpp tile to 8x8 pixel array.
	/// </summary>
	public static byte[,] DecodeSnesTile(byte[] snesTile) {
		var pixels = new byte[8, 8];
		for (int row = 0; row < 8; row++) {
			byte plane0 = snesTile[row * 2];
			byte plane1 = snesTile[row * 2 + 1];
			byte plane2 = snesTile[16 + row * 2];
			byte plane3 = snesTile[16 + row * 2 + 1];

			for (int x = 0; x < 8; x++) {
				int shift = 7 - x;
				int bit0 = (plane0 >> shift) & 1;
				int bit1 = (plane1 >> shift) & 1;
				int bit2 = (plane2 >> shift) & 1;
				int bit3 = (plane3 >> shift) & 1;
				pixels[row, x] = (byte)((bit3 << 3) | (bit2 << 2) | (bit1 << 1) | bit0);
			}
		}
		return pixels;
	}

	/// <summary>
	/// Encode 8x8 pixel array to SNES 4bpp format.
	/// </summary>
	public static byte[] EncodeSnesTile(byte[,] pixels) {
		var snesTile = new byte[SnesTileSize];
		for (int row = 0; row < 8; row++) {
			for (int x = 0; x < 8; x++) {
				int shift = 7 - x;
				byte pixel = pixels[row, x];

				if ((pixel & 1) != 0) snesTile[row * 2] |= (byte)(1 << shift);
				if ((pixel & 2) != 0) snesTile[row * 2 + 1] |= (byte)(1 << shift);
				if ((pixel & 4) != 0) snesTile[16 + row * 2] |= (byte)(1 << shift);
				if ((pixel & 8) != 0) snesTile[16 + row * 2 + 1] |= (byte)(1 << shift);
			}
		}
		return snesTile;
	}

	/// <summary>
	/// Encode 8x8 pixel array to NES 2bpp format.
	/// </summary>
	public static byte[] EncodeNesTile(byte[,] pixels) {
		var nesTile = new byte[NesTileSize];
		for (int row = 0; row < 8; row++) {
			for (int x = 0; x < 8; x++) {
				int shift = 7 - x;
				byte pixel = (byte)(pixels[row, x] & 0x03); // Only low 2 bits

				if ((pixel & 1) != 0) nesTile[row] |= (byte)(1 << shift);
				if ((pixel & 2) != 0) nesTile[row + 8] |= (byte)(1 << shift);
			}
		}
		return nesTile;
	}
}

/// <summary>
/// NES/SNES palette conversion utilities.
/// </summary>
public static class PaletteToDQ3r {
	/// <summary>
	/// NES master palette (64 colors in RGB format).
	/// Standard NTSC NES palette values.
	/// </summary>
	public static readonly (byte R, byte G, byte B)[] NesPalette = [
		// 0x00-0x0F (grays and basic colors row 0)
		(0x62, 0x62, 0x62), // 00 Gray
		(0x00, 0x1F, 0xB2), // 01 Dark Blue
		(0x24, 0x04, 0xC8), // 02 Dark Purple
		(0x52, 0x00, 0xB2), // 03 Purple
		(0x73, 0x00, 0x76), // 04 Magenta
		(0x80, 0x00, 0x24), // 05 Dark Red
		(0x73, 0x0B, 0x00), // 06 Brown
		(0x52, 0x20, 0x00), // 07 Dark Brown
		(0x24, 0x38, 0x00), // 08 Dark Green
		(0x00, 0x49, 0x00), // 09 Green
		(0x00, 0x4F, 0x00), // 0A Dark Teal
		(0x00, 0x47, 0x24), // 0B Teal
		(0x00, 0x36, 0x62), // 0C Dark Cyan
		(0x00, 0x00, 0x00), // 0D Black
		(0x00, 0x00, 0x00), // 0E Black (mirror)
		(0x00, 0x00, 0x00), // 0F Black (mirror)

		// 0x10-0x1F (medium brightness row)
		(0xAB, 0xAB, 0xAB), // 10 Light Gray
		(0x0D, 0x57, 0xFF), // 11 Blue
		(0x4B, 0x30, 0xFF), // 12 Purple
		(0x8A, 0x13, 0xFF), // 13 Violet
		(0xBC, 0x08, 0xD6), // 14 Magenta
		(0xD2, 0x12, 0x69), // 15 Red
		(0xC7, 0x2E, 0x00), // 16 Orange
		(0x9D, 0x54, 0x00), // 17 Brown
		(0x60, 0x7B, 0x00), // 18 Olive
		(0x20, 0x98, 0x00), // 19 Green
		(0x00, 0xA3, 0x00), // 1A Bright Green
		(0x00, 0x99, 0x42), // 1B Sea Green
		(0x00, 0x82, 0x9F), // 1C Cyan
		(0x00, 0x00, 0x00), // 1D Black
		(0x00, 0x00, 0x00), // 1E Black (mirror)
		(0x00, 0x00, 0x00), // 1F Black (mirror)

		// 0x20-0x2F (bright colors row)
		(0xFF, 0xFF, 0xFF), // 20 White
		(0x53, 0xAE, 0xFF), // 21 Sky Blue
		(0x90, 0x85, 0xFF), // 22 Light Purple
		(0xD3, 0x65, 0xFF), // 23 Pink
		(0xFF, 0x57, 0xFF), // 24 Bright Pink
		(0xFF, 0x5D, 0xCF), // 25 Light Red
		(0xFF, 0x77, 0x57), // 26 Orange
		(0xFA, 0x9E, 0x00), // 27 Yellow-Orange
		(0xBD, 0xC7, 0x00), // 28 Yellow-Green
		(0x7A, 0xE7, 0x00), // 29 Lime
		(0x43, 0xF6, 0x11), // 2A Bright Green
		(0x26, 0xEF, 0x7E), // 2B Spring Green
		(0x2C, 0xD5, 0xF6), // 2C Light Cyan
		(0x4E, 0x4E, 0x4E), // 2D Dark Gray
		(0x00, 0x00, 0x00), // 2E Black (mirror)
		(0x00, 0x00, 0x00), // 2F Black (mirror)

		// 0x30-0x3F (brightest colors row)
		(0xFF, 0xFF, 0xFF), // 30 White
		(0xB6, 0xE1, 0xFF), // 31 Pale Blue
		(0xCE, 0xD1, 0xFF), // 32 Pale Purple
		(0xE9, 0xC3, 0xFF), // 33 Pale Pink
		(0xFF, 0xBC, 0xFF), // 34 Light Pink
		(0xFF, 0xBD, 0xF4), // 35 Pale Red
		(0xFF, 0xC6, 0xC3), // 36 Peach
		(0xFF, 0xD5, 0x9A), // 37 Pale Orange
		(0xE9, 0xE6, 0x81), // 38 Pale Yellow
		(0xCE, 0xF4, 0x81), // 39 Pale Green
		(0xB6, 0xFB, 0x9A), // 3A Light Green
		(0xA9, 0xFA, 0xC3), // 3B Pale Sea Green
		(0xA9, 0xF0, 0xF4), // 3C Pale Cyan
		(0xB8, 0xB8, 0xB8), // 3D Light Gray
		(0x00, 0x00, 0x00), // 3E Black (mirror)
		(0x00, 0x00, 0x00), // 3F Black (mirror)
	];

	/// <summary>
	/// Convert NES palette index to SNES 15-bit BGR color.
	/// </summary>
	public static ushort NesColorToSnes(byte nesColorIndex) {
		if (nesColorIndex >= NesPalette.Length) {
			nesColorIndex = 0x0F; // Black fallback
		}

		var (r, g, b) = NesPalette[nesColorIndex];

		// SNES uses 15-bit BGR: bbbbb ggggg rrrrr
		int snesR = r >> 3; // 5 bits
		int snesG = g >> 3; // 5 bits
		int snesB = b >> 3; // 5 bits

		return (ushort)((snesB << 10) | (snesG << 5) | snesR);
	}

	/// <summary>
	/// Convert a 4-color NES palette to 16-color SNES palette.
	/// NES uses 4 colors per palette, SNES 4bpp uses 16.
	/// </summary>
	public static ushort[] ConvertNesPaletteTo4bpp(byte[] nesPaletteIndices) {
		if (nesPaletteIndices.Length != 4) {
			throw new ArgumentException("NES palette must have exactly 4 color indices");
		}

		var snesPalette = new ushort[16];

		// NES 2bpp colors go to SNES positions 0-3
		for (int i = 0; i < 4; i++) {
			snesPalette[i] = NesColorToSnes(nesPaletteIndices[i]);
		}

		// Fill remaining 12 colors with gradients/copies
		// Position 0 is typically transparent
		snesPalette[0] = 0x0000; // Transparent (black)

		// Copy palette across to positions 4-15
		// This allows attribute-based palette selection
		for (int i = 4; i < 16; i++) {
			snesPalette[i] = snesPalette[i % 4];
		}

		return snesPalette;
	}

	/// <summary>
	/// Convert full NES sprite/BG palette set to SNES format.
	/// NES has 4 BG palettes + 4 sprite palettes (32 bytes total).
	/// </summary>
	public static ushort[] ConvertFullNesPalette(byte[] nesPaletteData) {
		if (nesPaletteData.Length != 32) {
			throw new ArgumentException("Full NES palette must be 32 bytes");
		}

		// SNES mode 1: 8 palettes × 16 colors = 256 words
		var snesPalette = new ushort[256];

		// Convert 4 BG palettes (indices 0-15 each)
		for (int pal = 0; pal < 4; pal++) {
			var nesPal = new byte[4];
			Array.Copy(nesPaletteData, pal * 4, nesPal, 0, 4);
			var snesPal = ConvertNesPaletteTo4bpp(nesPal);

			for (int c = 0; c < 16; c++) {
				snesPalette[pal * 16 + c] = snesPal[c];
			}
		}

		// Convert 4 sprite palettes (indices 128-191)
		for (int pal = 0; pal < 4; pal++) {
			var nesPal = new byte[4];
			Array.Copy(nesPaletteData, 16 + pal * 4, nesPal, 0, 4);
			var snesPal = ConvertNesPaletteTo4bpp(nesPal);

			for (int c = 0; c < 16; c++) {
				snesPalette[128 + pal * 16 + c] = snesPal[c];
			}
		}

		return snesPalette;
	}
}
