using DQ4rLib.Models;

namespace DQ4rLib.Converters;

/// <summary>
/// Converts NES 2bpp CHR graphics to SNES 4bpp format
/// </summary>
public static class GraphicsConverter {
	/// <summary>
	/// Convert NES 2bpp CHR tile data to SNES 4bpp format
	/// </summary>
	/// <param name="nesChr">NES CHR data (16 bytes per tile)</param>
	/// <returns>SNES 4bpp data (32 bytes per tile)</returns>
	public static byte[] NesToSnes4bpp(byte[] nesChr) {
		int tileCount = nesChr.Length / 16;
		byte[] snesData = new byte[tileCount * 32];

		for (int tile = 0; tile < tileCount; tile++) {
			ConvertTile2bppTo4bpp(
				nesChr.AsSpan(tile * 16, 16),
				snesData.AsSpan(tile * 32, 32)
			);
		}

		return snesData;
	}

	/// <summary>
	/// Convert a single NES 2bpp tile to SNES 4bpp
	/// NES format: 8 bytes plane 0, 8 bytes plane 1
	/// SNES 4bpp: interleaved bitplanes (row0-bp0, row0-bp1, row1-bp0, row1-bp1, ...)
	///            then (row0-bp2, row0-bp3, row1-bp2, row1-bp3, ...)
	/// </summary>
	private static void ConvertTile2bppTo4bpp(ReadOnlySpan<byte> nes, Span<byte> snes) {
		// First 16 bytes: bitplanes 0,1 interleaved by row
		for (int row = 0; row < 8; row++) {
			snes[row * 2] = nes[row];        // Bitplane 0
			snes[row * 2 + 1] = nes[row + 8]; // Bitplane 1
		}

		// Second 16 bytes: bitplanes 2,3 (all zeros for 2bpp source)
		for (int i = 16; i < 32; i++) {
			snes[i] = 0;
		}
	}

	/// <summary>
	/// Convert NES palette to SNES palette
	/// </summary>
	/// <param name="nesPalette">NES palette indices (4 colors)</param>
	/// <returns>SNES RGB555 palette (16 colors, first 4 from NES)</returns>
	public static SnesPalette ConvertPalette(byte[] nesPalette) {
		var palette = new SnesPalette();

		// NES master palette (simplified - using standard NES colors)
		// Full implementation would use proper NES palette
		for (int i = 0; i < Math.Min(4, nesPalette.Length); i++) {
			var (r, g, b) = NesColorToRgb(nesPalette[i]);
			palette.Colors[i] = SnesPalette.RgbToSnes(r, g, b);
		}

		return palette;
	}

	/// <summary>
	/// Convert NES palette index to RGB (simplified)
	/// </summary>
	private static (byte R, byte G, byte B) NesColorToRgb(byte nesColor) {
		// Simplified NES palette - for accurate colors, use full 64-color NES palette
		// This is a placeholder that generates grayscale based on luminance
		int lum = (nesColor & 0x30) >> 2; // 0, 8, 16, 24
		int gray = lum * 10 + 15;
		return ((byte)gray, (byte)gray, (byte)gray);
	}

	/// <summary>
	/// Create SNES graphic from converted data
	/// </summary>
	public static SnesGraphic CreateSnesGraphic(byte[] nesChr, int widthInTiles = 16) {
		byte[] snesData = NesToSnes4bpp(nesChr);
		return new SnesGraphic(snesData, widthInTiles);
	}

	/// <summary>
	/// Apply palette shift to 4bpp tile data
	/// </summary>
	/// <param name="data">SNES 4bpp data</param>
	/// <param name="shift">Palette index to add (0-7, multiplied by 16)</param>
	public static void ApplyPaletteShift(byte[] data, int shift) {
		// For tilemap entries, not raw tile data
		// This is used when creating tilemaps
	}

	/// <summary>
	/// Export SNES graphic to binary file for inclusion in ROM
	/// </summary>
	public static void ExportToBinary(SnesGraphic graphic, string outputPath) {
		File.WriteAllBytes(outputPath, graphic.TileData);
	}

	/// <summary>
	/// Export SNES palette to binary file
	/// </summary>
	public static void ExportPalette(SnesPalette palette, string outputPath) {
		byte[] data = new byte[palette.ColorCount * 2];
		for (int i = 0; i < palette.ColorCount; i++) {
			data[i * 2] = (byte)(palette.Colors[i] & 0xff);
			data[i * 2 + 1] = (byte)(palette.Colors[i] >> 8);
		}
		File.WriteAllBytes(outputPath, data);
	}
}
