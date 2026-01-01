namespace DW4Lib.Tests;

using DW4Lib.Converters;
using Xunit;

/// <summary>
/// Tests for FontRenderer.
/// </summary>
public class FontRendererTests {
	[Fact]
	public void RenderConfig_HasDefaultValues() {
		var config = new FontRenderer.RenderConfig();

		Assert.Equal(16, config.CharsPerRow);
		Assert.Equal(0xFF000080u, config.BackgroundColor);
		Assert.Equal(0xFFFFFFFFu, config.ForegroundColor);
		Assert.Equal(0xFF404040u, config.GridColor);
		Assert.True(config.ShowGrid);
		Assert.Equal(1, config.CellPadding);
		Assert.Equal(2, config.Scale);
	}

	[Fact]
	public void RenderConfig_CanCustomize() {
		var config = new FontRenderer.RenderConfig {
			CharsPerRow = 32,
			BackgroundColor = 0xFF000000,
			ForegroundColor = 0xFF00FF00,
			ShowGrid = false,
			Scale = 4,
		};

		Assert.Equal(32, config.CharsPerRow);
		Assert.Equal(0xFF000000u, config.BackgroundColor);
		Assert.Equal(0xFF00FF00u, config.ForegroundColor);
		Assert.False(config.ShowGrid);
		Assert.Equal(4, config.Scale);
	}

	[Fact]
	public void RenderFontPreview_ReturnsPreview() {
		var preview = FontRenderer.RenderFontPreview();

		Assert.True(preview.Width > 0);
		Assert.True(preview.Height > 0);
		Assert.NotEmpty(preview.Pixels);
		Assert.NotEmpty(preview.Characters);
	}

	[Fact]
	public void RenderFontPreview_HasCorrectPixelCount() {
		var preview = FontRenderer.RenderFontPreview();

		Assert.Equal(preview.Width * preview.Height, preview.Pixels.Length);
	}

	[Fact]
	public void RenderFontPreview_ContainsAllCharacters() {
		var preview = FontRenderer.RenderFontPreview();

		// Should have entry for each mapped character
		Assert.Equal(FontToDQ3r.AsciiToTableCode.Count, preview.Characters.Count);
	}

	[Fact]
	public void RenderFontPreview_ScalesCorrectly() {
		var config1x = new FontRenderer.RenderConfig { Scale = 1 };
		var config2x = new FontRenderer.RenderConfig { Scale = 2 };

		var preview1x = FontRenderer.RenderFontPreview(config1x);
		var preview2x = FontRenderer.RenderFontPreview(config2x);

		// 2x should be twice the dimensions
		Assert.Equal(preview1x.Width * 2, preview2x.Width);
		Assert.Equal(preview1x.Height * 2, preview2x.Height);
	}

	[Fact]
	public void RenderText_ReturnsPreview() {
		var preview = FontRenderer.RenderText("Hello World");

		Assert.True(preview.Width > 0);
		Assert.True(preview.Height > 0);
		Assert.NotEmpty(preview.Pixels);
	}

	[Fact]
	public void RenderText_HandlesNewlines() {
		var singleLine = FontRenderer.RenderText("Hello");
		var multiLine = FontRenderer.RenderText("Hello\nWorld");

		// Multi-line should be taller
		Assert.True(multiLine.Height > singleLine.Height);
	}

	[Fact]
	public void RenderText_HandlesEmptyString() {
		var preview = FontRenderer.RenderText("");

		// Should still return valid preview (just padding)
		Assert.True(preview.Width >= 0);
		Assert.True(preview.Height >= 0);
	}

	[Fact]
	public void CharacterInfo_HasValidData() {
		var preview = FontRenderer.RenderFontPreview();

		foreach (var charInfo in preview.Characters) {
			Assert.True(charInfo.Width > 0);
			Assert.Equal(12, charInfo.Height);
			Assert.True(charInfo.X >= 0);
			Assert.True(charInfo.Y >= 0);
		}
	}

	[Fact]
	public void ToPpm_ReturnsValidData() {
		var preview = FontRenderer.RenderText("AB");
		byte[] ppmData = FontRenderer.ToPpm(preview);

		Assert.NotEmpty(ppmData);

		// PPM should start with P3 (ASCII format)
		string header = System.Text.Encoding.ASCII.GetString(ppmData, 0, 2);
		Assert.Equal("P3", header);
	}

	[Fact]
	public void ToBmp_ReturnsValidBitmapHeader() {
		var preview = FontRenderer.RenderText("AB");
		byte[] bmpData = FontRenderer.ToBmp(preview);

		Assert.NotEmpty(bmpData);

		// BMP should start with 'BM'
		Assert.Equal((byte)'B', bmpData[0]);
		Assert.Equal((byte)'M', bmpData[1]);

		// Header size is at offset 14, should be 40 (BITMAPINFOHEADER)
		int headerSize = BitConverter.ToInt32(bmpData, 14);
		Assert.Equal(40, headerSize);

		// Bits per pixel at offset 28, should be 32
		short bpp = BitConverter.ToInt16(bmpData, 28);
		Assert.Equal(32, bpp);
	}

	[Fact]
	public void ToBitmapData_ReturnsCorrectSize() {
		var preview = FontRenderer.RenderText("A");
		byte[] data = FontRenderer.ToBitmapData(preview);

		// 4 bytes per pixel (BGRA)
		Assert.Equal(preview.Width * preview.Height * 4, data.Length);
	}

	[Fact]
	public void GenerateReport_ReturnsReport() {
		string report = FontRenderer.GenerateReport();

		Assert.NotEmpty(report);
		Assert.Contains("DW4→DQ3r English Font Report", report);
		Assert.Contains("Total Characters:", report);
		Assert.Contains("Character Table:", report);
		Assert.Contains("Control Codes:", report);
	}

	[Fact]
	public void GenerateReport_ContainsAllSections() {
		string report = FontRenderer.GenerateReport();

		// Should list control codes
		Assert.Contains("[END]", report);
		Assert.Contains("[LINE]", report);
		Assert.Contains("[WAIT]", report);
		Assert.Contains("[HERO]", report);
	}
}
