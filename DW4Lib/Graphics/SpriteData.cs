namespace DW4Lib.Graphics;

/// <summary>
/// DW4 NES sprite data structures and extraction.
/// Sprites are stored in CHR-ROM banks 0x00-0x03.
/// </summary>
public static class SpriteData {
	// ============================================================
	// ROM Addresses
	// ============================================================

	/// <summary>CHR-ROM bank for character sprites.</summary>
	public const int CharacterSpriteBank = 0x00;

	/// <summary>CHR-ROM bank for monster sprites (lower).</summary>
	public const int MonsterSpriteBankLow = 0x01;

	/// <summary>CHR-ROM bank for monster sprites (upper).</summary>
	public const int MonsterSpriteBankHigh = 0x02;

	/// <summary>CHR-ROM bank for map/UI sprites.</summary>
	public const int MapSpriteBank = 0x03;

	/// <summary>PRG-ROM address of sprite pointer table.</summary>
	public const int SpritePointerTable = 0x1E000;

	/// <summary>PRG-ROM address of sprite metadata.</summary>
	public const int SpriteMetadataTable = 0x1E100;

	// ============================================================
	// Sprite Dimensions
	// ============================================================

	/// <summary>Width of a sprite frame in pixels.</summary>
	public const int SpriteWidth = 16;

	/// <summary>Height of a sprite frame in pixels.</summary>
	public const int SpriteHeight = 24;

	/// <summary>Tiles per sprite frame (2 wide × 3 tall).</summary>
	public const int TilesPerFrame = 6;

	/// <summary>Bytes per NES tile (8×8, 2bpp).</summary>
	public const int BytesPerNesTile = 16;

	/// <summary>Bytes per sprite frame.</summary>
	public const int BytesPerFrame = TilesPerFrame * BytesPerNesTile;

	/// <summary>Standard frames per walking animation.</summary>
	public const int WalkFrames = 2;

	/// <summary>Directions for character sprites.</summary>
	public const int DirectionCount = 4;

	// ============================================================
	// Character Sprite IDs
	// ============================================================

	/// <summary>Sprite ID for Ragnar (Chapter 1 hero).</summary>
	public const byte SpriteRagnar = 0x00;

	/// <summary>Sprite ID for Healie (Chapter 1 companion).</summary>
	public const byte SpriteHealie = 0xC5;

	/// <summary>Sprite ID for Alena.</summary>
	public const byte SpriteAlena = 0x01;

	/// <summary>Sprite ID for Cristo.</summary>
	public const byte SpriteCristo = 0x02;

	/// <summary>Sprite ID for Brey.</summary>
	public const byte SpriteBrey = 0x03;

	/// <summary>Sprite ID for Taloon.</summary>
	public const byte SpriteTaloon = 0x04;

	/// <summary>Sprite ID for Nara.</summary>
	public const byte SpriteNara = 0x05;

	/// <summary>Sprite ID for Mara.</summary>
	public const byte SpriteMara = 0x06;

	/// <summary>Sprite ID for Hero.</summary>
	public const byte SpriteHero = 0x07;

	// ============================================================
	// NPC Sprite IDs
	// ============================================================

	/// <summary>King sprite.</summary>
	public const byte SpriteKing = 0x10;

	/// <summary>Soldier/Guard sprite.</summary>
	public const byte SpriteSoldier = 0x11;

	/// <summary>Old man sprite.</summary>
	public const byte SpriteOldMan = 0x12;

	/// <summary>Woman sprite.</summary>
	public const byte SpriteWoman = 0x13;

	/// <summary>Child sprite.</summary>
	public const byte SpriteChild = 0x14;

	/// <summary>Merchant sprite.</summary>
	public const byte SpriteMerchant = 0x15;

	/// <summary>Priest sprite.</summary>
	public const byte SpritePriest = 0x16;

	/// <summary>Dancer sprite.</summary>
	public const byte SpriteDancer = 0x17;
}

/// <summary>
/// Sprite frame animation data.
/// </summary>
public class SpriteAnimation {
	/// <summary>Sprite ID.</summary>
	public byte SpriteId { get; set; }

	/// <summary>Animation name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Frame indices for this animation.</summary>
	public int[] FrameIndices { get; set; } = [];

	/// <summary>Frame durations in game ticks.</summary>
	public int[] FrameDurations { get; set; } = [];

	/// <summary>Whether animation loops.</summary>
	public bool Loops { get; set; } = true;
}

/// <summary>
/// Sprite sheet data extracted from ROM.
/// </summary>
public class SpriteSheet {
	/// <summary>Sprite ID.</summary>
	public byte SpriteId { get; set; }

	/// <summary>Sprite name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Number of frames in the sheet.</summary>
	public int FrameCount { get; set; }

	/// <summary>Frame width in pixels.</summary>
	public int FrameWidth { get; set; } = SpriteData.SpriteWidth;

	/// <summary>Frame height in pixels.</summary>
	public int FrameHeight { get; set; } = SpriteData.SpriteHeight;

	/// <summary>Raw tile data (NES 2bpp format).</summary>
	public byte[] TileData { get; set; } = [];

	/// <summary>Palette indices for each frame.</summary>
	public byte[] PaletteIndices { get; set; } = [];

	/// <summary>Animation definitions.</summary>
	public SpriteAnimation[] Animations { get; set; } = [];
}

/// <summary>
/// Palette data for sprites.
/// </summary>
public class SpritePalette {
	/// <summary>Palette index (0-3 for NES sprites).</summary>
	public int Index { get; set; }

	/// <summary>4 NES color indices.</summary>
	public byte[] Colors { get; set; } = new byte[4];

	/// <summary>Palette name/purpose.</summary>
	public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Chapter 1 sprite definitions.
/// </summary>
public static class Chapter1Sprites {
	/// <summary>
	/// Get Ragnar's sprite data.
	/// </summary>
	public static SpriteSheet GetRagnarSprite() => new() {
		SpriteId = SpriteData.SpriteRagnar,
		Name = "Ragnar McRyan",
		FrameCount = 8, // 4 directions × 2 walk frames
		Animations = [
			new() { SpriteId = SpriteData.SpriteRagnar, Name = "Walk Down", FrameIndices = [0, 1], FrameDurations = [8, 8], Loops = true },
			new() { SpriteId = SpriteData.SpriteRagnar, Name = "Walk Left", FrameIndices = [2, 3], FrameDurations = [8, 8], Loops = true },
			new() { SpriteId = SpriteData.SpriteRagnar, Name = "Walk Right", FrameIndices = [4, 5], FrameDurations = [8, 8], Loops = true },
			new() { SpriteId = SpriteData.SpriteRagnar, Name = "Walk Up", FrameIndices = [6, 7], FrameDurations = [8, 8], Loops = true }
		]
	};

	/// <summary>
	/// Get Healie's sprite data.
	/// </summary>
	public static SpriteSheet GetHealieSprite() => new() {
		SpriteId = SpriteData.SpriteHealie,
		Name = "Healie",
		FrameCount = 8,
		FrameWidth = 16,
		FrameHeight = 16, // Healie is smaller
		Animations = [
			new() { SpriteId = SpriteData.SpriteHealie, Name = "Bounce Down", FrameIndices = [0, 1], FrameDurations = [6, 6], Loops = true },
			new() { SpriteId = SpriteData.SpriteHealie, Name = "Bounce Left", FrameIndices = [2, 3], FrameDurations = [6, 6], Loops = true },
			new() { SpriteId = SpriteData.SpriteHealie, Name = "Bounce Right", FrameIndices = [4, 5], FrameDurations = [6, 6], Loops = true },
			new() { SpriteId = SpriteData.SpriteHealie, Name = "Bounce Up", FrameIndices = [6, 7], FrameDurations = [6, 6], Loops = true }
		]
	};

	/// <summary>
	/// Get all Chapter 1 character sprites.
	/// </summary>
	public static SpriteSheet[] GetAllCharacterSprites() => [
		GetRagnarSprite(),
		GetHealieSprite()
	];

	/// <summary>
	/// Get all Chapter 1 NPC sprites.
	/// </summary>
	public static SpriteSheet[] GetAllNpcSprites() => [
		new() { SpriteId = SpriteData.SpriteKing, Name = "King of Burland", FrameCount = 4 },
		new() { SpriteId = SpriteData.SpriteSoldier, Name = "Soldier", FrameCount = 8 },
		new() { SpriteId = SpriteData.SpriteOldMan, Name = "Old Man", FrameCount = 4 },
		new() { SpriteId = SpriteData.SpriteWoman, Name = "Woman", FrameCount = 8 },
		new() { SpriteId = SpriteData.SpriteChild, Name = "Child", FrameCount = 8 },
		new() { SpriteId = SpriteData.SpriteMerchant, Name = "Merchant", FrameCount = 4 },
		new() { SpriteId = SpriteData.SpritePriest, Name = "Priest", FrameCount = 4 }
	];

	/// <summary>
	/// Get Chapter 1 sprite palettes.
	/// </summary>
	public static SpritePalette[] GetSpritePalettes() => [
		new() { Index = 0, Colors = [0x0F, 0x30, 0x10, 0x00], Name = "Ragnar - Armor" },
		new() { Index = 1, Colors = [0x0F, 0x36, 0x16, 0x06], Name = "Ragnar - Skin/Cape" },
		new() { Index = 2, Colors = [0x0F, 0x30, 0x21, 0x11], Name = "Healie - Blue" },
		new() { Index = 3, Colors = [0x0F, 0x27, 0x17, 0x07], Name = "NPCs - Brown" }
	];
}
