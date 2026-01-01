namespace DQ4rLib.Models;

/// <summary>
/// Represents a SNES 4bpp graphic tile or tileset
/// </summary>
public class SnesGraphic {
	/// <summary>
	/// Raw 4bpp tile data (32 bytes per 8x8 tile)
	/// </summary>
	public byte[] TileData { get; set; } = [];

	/// <summary>
	/// Number of 8x8 tiles in this graphic
	/// </summary>
	public int TileCount => TileData.Length / 32;

	/// <summary>
	/// Width in tiles (for arranged graphics)
	/// </summary>
	public int WidthInTiles { get; set; } = 1;

	/// <summary>
	/// Height in tiles (for arranged graphics)
	/// </summary>
	public int HeightInTiles { get; set; } = 1;

	/// <summary>
	/// Width in pixels
	/// </summary>
	public int Width => WidthInTiles * 8;

	/// <summary>
	/// Height in pixels
	/// </summary>
	public int Height => HeightInTiles * 8;

	/// <summary>
	/// Creates a new empty SNES graphic
	/// </summary>
	public SnesGraphic() { }

	/// <summary>
	/// Creates a SNES graphic from raw 4bpp data
	/// </summary>
	public SnesGraphic(byte[] data, int widthInTiles = 16, int heightInTiles = 0) {
		TileData = data;
		WidthInTiles = widthInTiles;
		HeightInTiles = heightInTiles > 0 ? heightInTiles : TileCount / widthInTiles;
	}
}

/// <summary>
/// Represents a SNES 15-bit color palette
/// </summary>
public class SnesPalette {
	/// <summary>
	/// Colors in SNES RGB555 format (2 bytes per color)
	/// </summary>
	public ushort[] Colors { get; set; } = new ushort[16];

	/// <summary>
	/// Number of colors in the palette
	/// </summary>
	public int ColorCount => Colors.Length;

	/// <summary>
	/// Creates a grayscale default palette
	/// </summary>
	public SnesPalette() {
		for (int i = 0; i < 16; i++) {
			int gray = i * 2; // 0-30
			Colors[i] = (ushort)(gray | (gray << 5) | (gray << 10));
		}
	}

	/// <summary>
	/// Creates a palette from raw SNES color data
	/// </summary>
	public SnesPalette(byte[] data) {
		int colorCount = data.Length / 2;
		Colors = new ushort[colorCount];
		for (int i = 0; i < colorCount; i++) {
			Colors[i] = (ushort)(data[i * 2] | (data[i * 2 + 1] << 8));
		}
	}

	/// <summary>
	/// Converts a SNES RGB555 color to 24-bit RGB
	/// </summary>
	public static (byte R, byte G, byte B) SnesTo24Bit(ushort snesColor) {
		int r = (snesColor & 0x1f) << 3;
		int g = ((snesColor >> 5) & 0x1f) << 3;
		int b = ((snesColor >> 10) & 0x1f) << 3;
		return ((byte)r, (byte)g, (byte)b);
	}

	/// <summary>
	/// Converts a 24-bit RGB color to SNES RGB555
	/// </summary>
	public static ushort RgbToSnes(byte r, byte g, byte b) {
		return (ushort)((r >> 3) | ((g >> 3) << 5) | ((b >> 3) << 10));
	}
}
