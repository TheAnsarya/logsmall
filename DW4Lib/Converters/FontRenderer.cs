namespace DW4Lib.Converters;

/// <summary>
/// Font rendering utility for testing English font display.
/// Generates preview images and validates font data.
/// </summary>
public static class FontRenderer {
	/// <summary>
	/// Render configuration.
	/// </summary>
	public class RenderConfig {
		/// <summary>Characters per row in preview.</summary>
		public int CharsPerRow { get; set; } = 16;

		/// <summary>Background color (ARGB).</summary>
		public uint BackgroundColor { get; set; } = 0xFF000080; // Dark blue

		/// <summary>Foreground color (ARGB).</summary>
		public uint ForegroundColor { get; set; } = 0xFFFFFFFF; // White

		/// <summary>Grid line color (ARGB).</summary>
		public uint GridColor { get; set; } = 0xFF404040; // Gray

		/// <summary>Show grid lines.</summary>
		public bool ShowGrid { get; set; } = true;

		/// <summary>Cell padding in pixels.</summary>
		public int CellPadding { get; set; } = 1;

		/// <summary>Scale factor (1x, 2x, 3x, etc.).</summary>
		public int Scale { get; set; } = 2;
	}

	/// <summary>
	/// Rendered font preview result.
	/// </summary>
	public class FontPreview {
		/// <summary>Preview image width.</summary>
		public int Width { get; set; }

		/// <summary>Preview image height.</summary>
		public int Height { get; set; }

		/// <summary>Raw pixel data (ARGB format).</summary>
		public uint[] Pixels { get; set; } = [];

		/// <summary>Character mapping info.</summary>
		public List<CharacterInfo> Characters { get; set; } = [];
	}

	/// <summary>
	/// Information about a rendered character.
	/// </summary>
	public class CharacterInfo {
		/// <summary>The character.</summary>
		public char Character { get; set; }

		/// <summary>Table code.</summary>
		public int TableCode { get; set; }

		/// <summary>X position in preview.</summary>
		public int X { get; set; }

		/// <summary>Y position in preview.</summary>
		public int Y { get; set; }

		/// <summary>Character width.</summary>
		public int Width { get; set; }

		/// <summary>Character height.</summary>
		public int Height { get; set; }
	}

	/// <summary>
	/// Generate a font preview image.
	/// </summary>
	public static FontPreview RenderFontPreview(RenderConfig? config = null) {
		config ??= new RenderConfig();

		// Character cell dimensions (before scaling)
		int cellWidth = 8 + config.CellPadding * 2;
		int cellHeight = 12 + config.CellPadding * 2;

		// Calculate image dimensions
		int charCount = FontToDQ3r.AsciiToTableCode.Count;
		int rows = (charCount + config.CharsPerRow - 1) / config.CharsPerRow;

		int imageWidth = config.CharsPerRow * cellWidth * config.Scale;
		int imageHeight = rows * cellHeight * config.Scale;

		var preview = new FontPreview {
			Width = imageWidth,
			Height = imageHeight,
			Pixels = new uint[imageWidth * imageHeight],
			Characters = [],
		};

		// Fill background
		Array.Fill(preview.Pixels, config.BackgroundColor);

		// Draw grid if enabled
		if (config.ShowGrid) {
			DrawGrid(preview, config, cellWidth, cellHeight, rows);
		}

		// Render each character
		int charIndex = 0;
		foreach (var kvp in FontToDQ3r.AsciiToTableCode.OrderBy(x => x.Value)) {
			int col = charIndex % config.CharsPerRow;
			int row = charIndex / config.CharsPerRow;

			int cellX = col * cellWidth * config.Scale;
			int cellY = row * cellHeight * config.Scale;

			// Get glyph
			byte[] glyph1bpp = FontToDQ3r.Create1bppGlyph(kvp.Key);

			// Render glyph
			RenderGlyph(preview, glyph1bpp, cellX + config.CellPadding * config.Scale,
				cellY + config.CellPadding * config.Scale, config);

			// Record character info
			preview.Characters.Add(new CharacterInfo {
				Character = kvp.Key,
				TableCode = kvp.Value,
				X = cellX,
				Y = cellY,
				Width = FontToDQ3r.GetCharacterWidth(kvp.Key),
				Height = 12,
			});

			charIndex++;
		}

		return preview;
	}

	private static void DrawGrid(FontPreview preview, RenderConfig config, int cellWidth, int cellHeight, int rows) {
		int scaledCellWidth = cellWidth * config.Scale;
		int scaledCellHeight = cellHeight * config.Scale;

		// Vertical lines
		for (int col = 0; col <= config.CharsPerRow; col++) {
			int x = col * scaledCellWidth;
			if (x >= preview.Width) continue;

			for (int y = 0; y < preview.Height; y++) {
				preview.Pixels[y * preview.Width + x] = config.GridColor;
			}
		}

		// Horizontal lines
		for (int row = 0; row <= rows; row++) {
			int y = row * scaledCellHeight;
			if (y >= preview.Height) continue;

			for (int x = 0; x < preview.Width; x++) {
				preview.Pixels[y * preview.Width + x] = config.GridColor;
			}
		}
	}

	private static void RenderGlyph(FontPreview preview, byte[] glyph1bpp, int startX, int startY, RenderConfig config) {
		for (int row = 0; row < 12 && row < glyph1bpp.Length; row++) {
			byte rowData = glyph1bpp[row];

			for (int bit = 0; bit < 8; bit++) {
				if ((rowData & (0x80 >> bit)) != 0) {
					// Draw scaled pixel
					for (int sy = 0; sy < config.Scale; sy++) {
						for (int sx = 0; sx < config.Scale; sx++) {
							int px = startX + bit * config.Scale + sx;
							int py = startY + row * config.Scale + sy;

							if (px >= 0 && px < preview.Width && py >= 0 && py < preview.Height) {
								preview.Pixels[py * preview.Width + px] = config.ForegroundColor;
							}
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Render a text string as a preview image.
	/// </summary>
	public static FontPreview RenderText(string text, RenderConfig? config = null) {
		config ??= new RenderConfig();

		// Calculate dimensions based on text
		int maxWidth = 0;
		int currentWidth = 0;
		int lines = 1;

		foreach (char c in text) {
			if (c == '\n') {
				maxWidth = Math.Max(maxWidth, currentWidth);
				currentWidth = 0;
				lines++;
			}
			else {
				currentWidth += FontToDQ3r.GetCharacterWidth(c) + 1; // +1 for spacing
			}
		}
		maxWidth = Math.Max(maxWidth, currentWidth);

		int imageWidth = (maxWidth + 4) * config.Scale;
		int imageHeight = (lines * 14 + 4) * config.Scale; // 12 height + 2 spacing

		var preview = new FontPreview {
			Width = imageWidth,
			Height = imageHeight,
			Pixels = new uint[imageWidth * imageHeight],
			Characters = [],
		};

		Array.Fill(preview.Pixels, config.BackgroundColor);

		// Render text
		int x = 2 * config.Scale;
		int y = 2 * config.Scale;

		foreach (char c in text) {
			if (c == '\n') {
				x = 2 * config.Scale;
				y += 14 * config.Scale;
				continue;
			}

			byte[] glyph = FontToDQ3r.Create1bppGlyph(c);
			RenderGlyph(preview, glyph, x, y, config);

			x += (FontToDQ3r.GetCharacterWidth(c) + 1) * config.Scale;
		}

		return preview;
	}

	/// <summary>
	/// Export preview to PPM format (simple image format).
	/// </summary>
	public static byte[] ToPpm(FontPreview preview) {
		using var ms = new MemoryStream();
		using var writer = new StreamWriter(ms, System.Text.Encoding.ASCII);

		// PPM header
		writer.WriteLine("P3");
		writer.WriteLine($"{preview.Width} {preview.Height}");
		writer.WriteLine("255");
		writer.Flush();

		// Pixel data
		var sb = new System.Text.StringBuilder();
		for (int y = 0; y < preview.Height; y++) {
			for (int x = 0; x < preview.Width; x++) {
				uint pixel = preview.Pixels[y * preview.Width + x];
				int r = (int)((pixel >> 16) & 0xFF);
				int g = (int)((pixel >> 8) & 0xFF);
				int b = (int)(pixel & 0xFF);
				sb.Append($"{r} {g} {b} ");
			}
			sb.AppendLine();
		}

		writer.Write(sb.ToString());
		writer.Flush();

		return ms.ToArray();
	}

	/// <summary>
	/// Export preview to raw bitmap data (32-bit BGRA for BMP).
	/// </summary>
	public static byte[] ToBitmapData(FontPreview preview) {
		// BMP uses bottom-up row order and BGRA format
		byte[] data = new byte[preview.Width * preview.Height * 4];

		for (int y = 0; y < preview.Height; y++) {
			int srcRow = preview.Height - 1 - y; // Flip vertically
			for (int x = 0; x < preview.Width; x++) {
				uint pixel = preview.Pixels[srcRow * preview.Width + x];

				int dstOffset = (y * preview.Width + x) * 4;
				data[dstOffset + 0] = (byte)(pixel & 0xFF);         // B
				data[dstOffset + 1] = (byte)((pixel >> 8) & 0xFF);  // G
				data[dstOffset + 2] = (byte)((pixel >> 16) & 0xFF); // R
				data[dstOffset + 3] = (byte)((pixel >> 24) & 0xFF); // A
			}
		}

		return data;
	}

	/// <summary>
	/// Export preview as a simple BMP file.
	/// </summary>
	public static byte[] ToBmp(FontPreview preview) {
		int rowSize = ((preview.Width * 32 + 31) / 32) * 4; // Row size padded to 4 bytes
		int pixelDataSize = rowSize * preview.Height;
		int fileSize = 54 + pixelDataSize; // Header + pixels

		using var ms = new MemoryStream();
		using var writer = new BinaryWriter(ms);

		// BMP File Header (14 bytes)
		writer.Write((byte)'B');
		writer.Write((byte)'M');
		writer.Write(fileSize);
		writer.Write((short)0); // Reserved
		writer.Write((short)0); // Reserved
		writer.Write(54); // Pixel data offset

		// DIB Header (40 bytes - BITMAPINFOHEADER)
		writer.Write(40); // Header size
		writer.Write(preview.Width);
		writer.Write(preview.Height);
		writer.Write((short)1); // Color planes
		writer.Write((short)32); // Bits per pixel
		writer.Write(0); // No compression
		writer.Write(pixelDataSize);
		writer.Write(2835); // Horizontal resolution (72 DPI)
		writer.Write(2835); // Vertical resolution
		writer.Write(0); // Colors in palette
		writer.Write(0); // Important colors

		// Pixel data (bottom-up, BGRA)
		for (int y = preview.Height - 1; y >= 0; y--) {
			for (int x = 0; x < preview.Width; x++) {
				uint pixel = preview.Pixels[y * preview.Width + x];
				writer.Write((byte)(pixel & 0xFF));         // B
				writer.Write((byte)((pixel >> 8) & 0xFF));  // G
				writer.Write((byte)((pixel >> 16) & 0xFF)); // R
				writer.Write((byte)((pixel >> 24) & 0xFF)); // A
			}

			// Row padding
			int padding = rowSize - preview.Width * 4;
			for (int p = 0; p < padding; p++) {
				writer.Write((byte)0);
			}
		}

		return ms.ToArray();
	}

	/// <summary>
	/// Save font preview to file.
	/// </summary>
	public static void SavePreview(FontPreview preview, string filePath) {
		string ext = Path.GetExtension(filePath).ToLower();
		byte[] data = ext switch {
			".ppm" => ToPpm(preview),
			".bmp" => ToBmp(preview),
			_ => ToBmp(preview),
		};

		File.WriteAllBytes(filePath, data);
	}

	/// <summary>
	/// Generate font table report.
	/// </summary>
	public static string GenerateReport() {
		var sb = new System.Text.StringBuilder();

		sb.AppendLine("DW4→DQ3r English Font Report");
		sb.AppendLine("============================");
		sb.AppendLine();
		sb.AppendLine($"Total Characters: {FontToDQ3r.AsciiToTableCode.Count}");
		sb.AppendLine();

		sb.AppendLine("Character Table:");
		sb.AppendLine("----------------");

		int col = 0;
		foreach (var kvp in FontToDQ3r.AsciiToTableCode.OrderBy(x => x.Value)) {
			char c = kvp.Key;
			string display = c switch {
				' ' => "SPC",
				'\t' => "TAB",
				'\n' => "LF",
				_ => c.ToString(),
			};

			sb.Append($"  {display,-4} 0x{kvp.Value:X4}  W={FontToDQ3r.GetCharacterWidth(c),2}");

			col++;
			if (col % 4 == 0) sb.AppendLine();
		}

		sb.AppendLine();
		sb.AppendLine();
		sb.AppendLine("Control Codes:");
		sb.AppendLine("--------------");
		sb.AppendLine($"  [END]     0x{FontToDQ3r.ControlCodes.EndStringAC:X4}");
		sb.AppendLine($"  [LINE]    0x{FontToDQ3r.ControlCodes.NewLine:X4}");
		sb.AppendLine($"  [WAIT]    0x{FontToDQ3r.ControlCodes.Wait:X4}");
		sb.AppendLine($"  [HERO]    0x{FontToDQ3r.ControlCodes.HeroName:X4}");
		sb.AppendLine($"  [ITEM]    0x{FontToDQ3r.ControlCodes.ItemName:X4}");
		sb.AppendLine($"  [NUM]     0x{FontToDQ3r.ControlCodes.Number:X4}");
		sb.AppendLine($"  [MONSTER] 0x{FontToDQ3r.ControlCodes.MonsterName:X4}");
		sb.AppendLine($"  [SPELL]   0x{FontToDQ3r.ControlCodes.SpellName:X4}");
		sb.AppendLine($"  [GOLD]    0x{FontToDQ3r.ControlCodes.Gold:X4}");
		sb.AppendLine($"  [EXP]     0x{FontToDQ3r.ControlCodes.Experience:X4}");
		sb.AppendLine($"  [HP]      0x{FontToDQ3r.ControlCodes.HitPoints:X4}");
		sb.AppendLine($"  [MP]      0x{FontToDQ3r.ControlCodes.MagicPoints:X4}");
		sb.AppendLine($"  [LV]      0x{FontToDQ3r.ControlCodes.Level:X4}");
		sb.AppendLine($"  [CLEAR]   0x{FontToDQ3r.ControlCodes.Clear:X4}");
		sb.AppendLine($"  [PAUSE]   0x{FontToDQ3r.ControlCodes.Pause:X4}");

		return sb.ToString();
	}
}
