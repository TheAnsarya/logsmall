using DW4Lib.Converters;

namespace DW4Lib.Graphics;

/// <summary>
/// Converts DW4 NES sprite data to DQ3r SNES format.
/// Handles character sprites, NPC sprites, and monster sprites.
/// </summary>
public static class SpriteToDQ3r {
	// ============================================================
	// DQ3r Sprite Constants
	// ============================================================

	/// <summary>DQ3r sprite width in pixels.</summary>
	public const int DQ3rSpriteWidth = 16;

	/// <summary>DQ3r sprite height in pixels.</summary>
	public const int DQ3rSpriteHeight = 24;

	/// <summary>SNES 4bpp tile size.</summary>
	public const int SnesTileSize = 32;

	/// <summary>Tiles per DQ3r sprite frame.</summary>
	public const int TilesPerDQ3rFrame = 6;

	/// <summary>Offset to add to DW4 sprite IDs for DQ3r.</summary>
	public const int SpriteIdOffset = 0x0400;

	// ============================================================
	// Conversion Methods
	// ============================================================

	/// <summary>
	/// Convert a DW4 sprite sheet to DQ3r format.
	/// </summary>
	public static DQ3rSpriteSheet ConvertSpriteSheet(SpriteSheet dw4Sprite) {
		var dq3rSprite = new DQ3rSpriteSheet {
			SpriteId = (ushort)(dw4Sprite.SpriteId + SpriteIdOffset),
			Name = dw4Sprite.Name,
			FrameCount = dw4Sprite.FrameCount,
			FrameWidth = dw4Sprite.FrameWidth,
			FrameHeight = dw4Sprite.FrameHeight,
			Animations = dw4Sprite.Animations
				.Select(a => ConvertAnimation(a))
				.ToArray()
		};

		// Convert tile data from NES 2bpp to SNES 4bpp
		if (dw4Sprite.TileData.Length > 0) {
			dq3rSprite.TileData = GraphicsToDQ3r.ConvertTileset2bppTo4bpp(dw4Sprite.TileData);
		}

		// Convert palette indices
		dq3rSprite.PaletteIndices = dw4Sprite.PaletteIndices.ToArray();

		return dq3rSprite;
	}

	/// <summary>
	/// Convert a sprite animation definition.
	/// </summary>
	public static DQ3rSpriteAnimation ConvertAnimation(SpriteAnimation dw4Anim) {
		return new DQ3rSpriteAnimation {
			SpriteId = (ushort)(dw4Anim.SpriteId + SpriteIdOffset),
			Name = dw4Anim.Name,
			FrameIndices = dw4Anim.FrameIndices.ToArray(),
			FrameDurations = dw4Anim.FrameDurations.ToArray(),
			Loops = dw4Anim.Loops
		};
	}

	/// <summary>
	/// Convert a sprite palette to SNES format.
	/// </summary>
	public static DQ3rSpritePalette ConvertPalette(SpritePalette dw4Palette) {
		var snesColors = new ushort[16];

		// Convert NES 4-color palette to SNES 16-color
		for (int i = 0; i < 4 && i < dw4Palette.Colors.Length; i++) {
			snesColors[i] = PaletteToDQ3r.NesColorToSnes(dw4Palette.Colors[i]);
		}

		// Color 0 is transparent
		snesColors[0] = 0x0000;

		// Fill remaining slots with copies for flexibility
		for (int i = 4; i < 16; i++) {
			snesColors[i] = snesColors[i % 4];
		}

		return new DQ3rSpritePalette {
			Index = dw4Palette.Index,
			Colors = snesColors,
			Name = dw4Palette.Name
		};
	}

	/// <summary>
	/// Convert all Chapter 1 character sprites.
	/// </summary>
	public static DQ3rSpriteSheet[] ConvertChapter1CharacterSprites() {
		return Chapter1Sprites.GetAllCharacterSprites()
			.Select(s => ConvertSpriteSheet(s))
			.ToArray();
	}

	/// <summary>
	/// Convert all Chapter 1 NPC sprites.
	/// </summary>
	public static DQ3rSpriteSheet[] ConvertChapter1NpcSprites() {
		return Chapter1Sprites.GetAllNpcSprites()
			.Select(s => ConvertSpriteSheet(s))
			.ToArray();
	}

	/// <summary>
	/// Convert Chapter 1 palettes.
	/// </summary>
	public static DQ3rSpritePalette[] ConvertChapter1Palettes() {
		return Chapter1Sprites.GetSpritePalettes()
			.Select(p => ConvertPalette(p))
			.ToArray();
	}

	/// <summary>
	/// Build a complete DQ3r sprite resource for Chapter 1.
	/// </summary>
	public static DQ3rSpriteResource BuildChapter1SpriteResource() {
		return new DQ3rSpriteResource {
			CharacterSprites = ConvertChapter1CharacterSprites(),
			NpcSprites = ConvertChapter1NpcSprites(),
			Palettes = ConvertChapter1Palettes(),
			Chapter = 1
		};
	}

	/// <summary>
	/// Convert DW4 sprite ID to DQ3r sprite ID.
	/// </summary>
	public static ushort ConvertSpriteId(byte dw4SpriteId) {
		return (ushort)(dw4SpriteId + SpriteIdOffset);
	}

	/// <summary>
	/// Convert DQ3r sprite ID back to DW4 sprite ID.
	/// </summary>
	public static byte ConvertSpriteIdBack(ushort dq3rSpriteId) {
		if (dq3rSpriteId < SpriteIdOffset) {
			return (byte)dq3rSpriteId;
		}
		return (byte)(dq3rSpriteId - SpriteIdOffset);
	}
}

/// <summary>
/// DQ3r SNES sprite sheet.
/// </summary>
public class DQ3rSpriteSheet {
	/// <summary>DQ3r sprite ID.</summary>
	public ushort SpriteId { get; set; }

	/// <summary>Sprite name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Number of frames.</summary>
	public int FrameCount { get; set; }

	/// <summary>Frame width in pixels.</summary>
	public int FrameWidth { get; set; }

	/// <summary>Frame height in pixels.</summary>
	public int FrameHeight { get; set; }

	/// <summary>SNES 4bpp tile data.</summary>
	public byte[] TileData { get; set; } = [];

	/// <summary>Palette indices for each frame.</summary>
	public byte[] PaletteIndices { get; set; } = [];

	/// <summary>Animation definitions.</summary>
	public DQ3rSpriteAnimation[] Animations { get; set; } = [];

	/// <summary>
	/// Get the size in bytes of tile data.
	/// </summary>
	public int TileDataSize => TileData.Length;

	/// <summary>
	/// Serialize to binary format for ROM injection.
	/// </summary>
	public byte[] ToBytes() {
		var ms = new MemoryStream();
		var writer = new BinaryWriter(ms);

		// Header
		writer.Write(SpriteId);
		writer.Write((byte)FrameCount);
		writer.Write((byte)FrameWidth);
		writer.Write((byte)FrameHeight);
		writer.Write((byte)Animations.Length);

		// Palette indices
		writer.Write((byte)PaletteIndices.Length);
		writer.Write(PaletteIndices);

		// Animation data
		foreach (var anim in Animations) {
			writer.Write((byte)anim.FrameIndices.Length);
			foreach (var idx in anim.FrameIndices) {
				writer.Write((byte)idx);
			}
			foreach (var dur in anim.FrameDurations) {
				writer.Write((byte)dur);
			}
			writer.Write(anim.Loops ? (byte)1 : (byte)0);
		}

		// Tile data
		writer.Write(TileData.Length);
		writer.Write(TileData);

		return ms.ToArray();
	}
}

/// <summary>
/// DQ3r sprite animation.
/// </summary>
public class DQ3rSpriteAnimation {
	/// <summary>Parent sprite ID.</summary>
	public ushort SpriteId { get; set; }

	/// <summary>Animation name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Frame indices.</summary>
	public int[] FrameIndices { get; set; } = [];

	/// <summary>Frame durations in ticks.</summary>
	public int[] FrameDurations { get; set; } = [];

	/// <summary>Whether animation loops.</summary>
	public bool Loops { get; set; }
}

/// <summary>
/// DQ3r SNES sprite palette.
/// </summary>
public class DQ3rSpritePalette {
	/// <summary>Palette index.</summary>
	public int Index { get; set; }

	/// <summary>16 SNES 15-bit colors (BGR555).</summary>
	public ushort[] Colors { get; set; } = new ushort[16];

	/// <summary>Palette name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Serialize palette to binary format.
	/// </summary>
	public byte[] ToBytes() {
		var bytes = new byte[32]; // 16 colors × 2 bytes
		for (int i = 0; i < 16; i++) {
			bytes[i * 2] = (byte)(Colors[i] & 0xFF);
			bytes[i * 2 + 1] = (byte)(Colors[i] >> 8);
		}
		return bytes;
	}

	/// <summary>
	/// Parse palette from binary data.
	/// </summary>
	public static DQ3rSpritePalette FromBytes(byte[] data, int offset = 0) {
		var palette = new DQ3rSpritePalette();
		for (int i = 0; i < 16; i++) {
			palette.Colors[i] = (ushort)(data[offset + i * 2] | (data[offset + i * 2 + 1] << 8));
		}
		return palette;
	}
}

/// <summary>
/// Complete DQ3r sprite resource for a chapter.
/// </summary>
public class DQ3rSpriteResource {
	/// <summary>Chapter number.</summary>
	public int Chapter { get; set; }

	/// <summary>Character sprites.</summary>
	public DQ3rSpriteSheet[] CharacterSprites { get; set; } = [];

	/// <summary>NPC sprites.</summary>
	public DQ3rSpriteSheet[] NpcSprites { get; set; } = [];

	/// <summary>Palettes.</summary>
	public DQ3rSpritePalette[] Palettes { get; set; } = [];

	/// <summary>
	/// Get all sprites.
	/// </summary>
	public IEnumerable<DQ3rSpriteSheet> GetAllSprites() {
		return CharacterSprites.Concat(NpcSprites);
	}

	/// <summary>
	/// Get total sprite count.
	/// </summary>
	public int TotalSpriteCount => CharacterSprites.Length + NpcSprites.Length;
}
