namespace DW4Lib.DQ3r;

/// <summary>
/// Animation mappings for DQ3r spells.
/// Maps spell IDs to their visual effect animation IDs.
/// </summary>
/// <remarks>
/// Animation IDs are based on DQ3r SNES ROM analysis.
/// See: GameInfo/Games/SNES/Dragon Quest III (SNES)/Docs/reference/technical/spell-animations.md
/// 
/// ROM Layout (Verified):
/// - Spell Data:    $520000 (4KB)
/// - Spell Effects: $520800 (4KB estimated)
/// - Battle Effects: $260000 (24KB estimated)
/// 
/// VRAM Layout (Battle):
/// - $0000-$3FFF: Background tiles
/// - $4000-$5FFF: Monster graphics  
/// - $6000-$7FFF: Spell effect tiles (DMA loaded)
/// </remarks>
public static class DQ3rAnimationMappings {
	#region ROM Address Constants

	/// <summary>ROM address: Spell data table (4KB)</summary>
	public const int RomSpellData = 0x520000;

	/// <summary>ROM address: Spell effect data (4KB estimated)</summary>
	public const int RomSpellEffects = 0x520800;

	/// <summary>ROM address: Battle effect graphics (24KB estimated)</summary>
	public const int RomBattleEffects = 0x260000;

	/// <summary>VRAM address: Spell effect tiles destination</summary>
	public const int VramSpellEffects = 0x6000;

	#endregion

	/// <summary>
	/// Maps DQ3r spell ID to animation ID.
	/// </summary>
	/// <remarks>
	/// Animation IDs are based on spell table at $520000.
	/// Animation effect tiles DMA to VRAM $6000-$7FFF during casting.
	/// </remarks>
	public static readonly Dictionary<int, int> SpellAnimations = new() {
		// Attack Magic - Fire (メラ系)
		[0x01] = 0x10, // Frizz - Small fireball
		[0x02] = 0x11, // Frizzle - Medium fireball
		[0x03] = 0x12, // Kafrizzle - Large explosion

		// Attack Magic - Ice (ヒャド系)
		[0x04] = 0x20, // Crack - Ice spike
		[0x05] = 0x21, // Crackle - Ice shards
		[0x06] = 0x22, // Kacrack - Blizzard

		// Attack Magic - Electric (デイン系)
		[0x07] = 0x30, // Zap - Lightning bolt
		[0x08] = 0x31, // Kazap - Multiple lightning

		// Attack Magic - Explosion (イオ系)
		[0x09] = 0x40, // Bang - Small explosion
		[0x0A] = 0x41, // Boom - Medium explosion
		[0x0B] = 0x42, // Kaboom - Large explosion

		// Attack Magic - Wind (バギ系)
		[0x0C] = 0x50, // Woosh - Wind gust
		[0x0D] = 0x51, // Swoosh - Tornado
		[0x0E] = 0x52, // Kaswoosh - Cross wind

		// Healing Magic
		[0x10] = 0x60, // Heal - Sparkles
		[0x11] = 0x61, // Midheal - Larger sparkles
		[0x12] = 0x62, // Fullheal - Full glow
		[0x13] = 0x63, // Omniheal - Party glow
		[0x14] = 0x64, // Zing - Resurrection light
		[0x15] = 0x65, // Kazing - Full resurrection

		// Status Magic
		[0x20] = 0x70, // Snooze - Sleep bubbles
		[0x21] = 0x71, // Fuddle - Confusion swirls
		[0x22] = 0x72, // Fizzle - Silence effect
		[0x23] = 0x73, // Dazzle - Illusion stars

		// Support Magic
		[0x30] = 0x80, // Buff - Defense up
		[0x31] = 0x81, // Kabuff - Party defense up
		[0x32] = 0x82, // Accelerate - Speed up
		[0x33] = 0x83, // Oomph - Attack up
		[0x34] = 0x84, // Sap - Defense down
		[0x35] = 0x85, // Kasap - Group defense down
	};

	/// <summary>
	/// Maps DW4 spell ID to recommended DQ3r animation ID.
	/// </summary>
	/// <remarks>
	/// This mapping is used for the DW4 to DQ3r conversion pipeline.
	/// DW4 spells are mapped to their closest DQ3r equivalents.
	/// </remarks>
	public static readonly Dictionary<int, int> DW4SpellToAnimation = new() {
		// DW4 Attack spells -> DQ3r animations
		[0x00] = 0x10, // BLAZE -> Frizz animation
		[0x01] = 0x11, // BLAZEMORE -> Frizzle animation
		[0x02] = 0x12, // BLAZEMOST -> Kafrizzle animation
		[0x03] = 0x40, // FIREBANE -> Bang animation
		[0x04] = 0x41, // FIREVOLT -> Boom animation
		[0x05] = 0x20, // ICEBOLT -> Crack animation
		[0x06] = 0x21, // SNOWSTORM -> Crackle animation
		[0x07] = 0x30, // BOLT -> Zap animation
		[0x08] = 0x31, // ZAPPING -> Kazap animation

		// DW4 Healing spells
		[0x10] = 0x60, // HEAL -> Heal animation
		[0x11] = 0x61, // HEALMORE -> Midheal animation
		[0x12] = 0x62, // HEALALL -> Fullheal animation
		[0x13] = 0x63, // HEALUS -> Omniheal animation
		[0x14] = 0x64, // VIVIFY -> Zing animation
		[0x15] = 0x65, // REVIVE -> Kazing animation

		// DW4 Status spells
		[0x20] = 0x70, // SLEEP -> Snooze animation
		[0x21] = 0x71, // CHAOS -> Fuddle animation
		[0x22] = 0x72, // STOPSPELL -> Fizzle animation
		[0x23] = 0x73, // SURROUND -> Dazzle animation

		// DW4 Support spells
		[0x30] = 0x80, // INCREASE -> Buff animation
		[0x31] = 0x81, // BARRIER -> Kabuff animation
		[0x32] = 0x82, // SPEEDUP -> Accelerate animation
		[0x33] = 0x83, // BIKILL -> Oomph animation
		[0x34] = 0x84, // DECREASE -> Sap animation
	};

	/// <summary>
	/// Get the DQ3r animation ID for a DW4 spell.
	/// </summary>
	/// <param name="dw4SpellId">DW4 spell ID</param>
	/// <returns>DQ3r animation ID, or 0 if no mapping exists</returns>
	public static int GetAnimationForDW4Spell(int dw4SpellId) {
		return DW4SpellToAnimation.TryGetValue(dw4SpellId, out int animId) ? animId : 0;
	}

	/// <summary>
	/// Get the DQ3r animation ID for a DQ3r spell.
	/// </summary>
	/// <param name="dq3rSpellId">DQ3r spell ID</param>
	/// <returns>Animation ID, or 0 if no mapping exists</returns>
	public static int GetAnimation(int dq3rSpellId) {
		return SpellAnimations.TryGetValue(dq3rSpellId, out int animId) ? animId : 0;
	}

	/// <summary>
	/// Sound effect IDs corresponding to spell animations.
	/// </summary>
	public static readonly Dictionary<int, int> SpellSounds = new() {
		// Fire spells
		[0x10] = 0x20, // Frizz
		[0x11] = 0x21, // Frizzle
		[0x12] = 0x22, // Kafrizzle

		// Ice spells
		[0x20] = 0x30, // Crack
		[0x21] = 0x31, // Crackle
		[0x22] = 0x32, // Kacrack

		// Healing spells
		[0x60] = 0x50, // Heal
		[0x61] = 0x51, // Midheal
		[0x62] = 0x52, // Fullheal
	};
}

/// <summary>
/// Monster sprite mappings for DQ3r.
/// Maps monster IDs to their sprite graphics data.
/// </summary>
/// <remarks>
/// ROM Addresses (Verified):
/// - Monster Graphics: $220000 (128KB, SNES 4bpp)
/// - Monster Metadata: $3ed964-$3ee0db (1,896 bytes)
/// - Monster Stats:    $510000 (8KB)
/// - Monster AI:       $512000 (12KB)
/// 
/// Monster count: 155
/// Palette indices: 4-7 (battle sprites)
/// </remarks>
public static class DQ3rMonsterMappings {
	#region ROM Address Constants

	/// <summary>ROM address: Monster graphics (128KB, 4bpp)</summary>
	public const int RomMonsterGraphics = 0x220000;

	/// <summary>ROM address: Monster metadata table</summary>
	public const int RomMonsterMetadata = 0x3ed964;

	/// <summary>ROM address: Monster stats (8KB)</summary>
	public const int RomMonsterStats = 0x510000;

	/// <summary>ROM address: Monster AI patterns (12KB)</summary>
	public const int RomMonsterAI = 0x512000;

	/// <summary>Total monster count</summary>
	public const int MonsterCount = 155;

	/// <summary>VRAM address: Monster graphics destination</summary>
	public const int VramMonsterGraphics = 0x4000;

	#endregion

	/// <summary>
	/// Monster sprite size categories.
	/// </summary>
	public enum SpriteSize {
		Small,  // 16x16 (2x2 tiles)
		Medium, // 32x32 (4x4 tiles)
		Large,  // 48x48 (6x6 tiles)
		Boss    // 64x64 (8x8 tiles)
	}

	/// <summary>
	/// Monster sprite metadata.
	/// </summary>
	public record MonsterSprite(
		int MonsterId,
		int SpriteOffset,
		SpriteSize Size,
		int PaletteId,
		int TileCount
	);

	/// <summary>
	/// Maps DQ3r monster ID to sprite metadata.
	/// </summary>
	/// <remarks>
	/// ROM Addresses (Verified):
	/// - Monster Graphics: $220000 (128KB, 4bpp)
	/// - Monster Metadata: $3ed964-$3ee0db (1,896 bytes)
	/// - Monster Stats:    $510000 (8KB)
	/// - Monster AI:       $512000 (12KB)
	/// 
	/// Sprite offsets are relative to monster graphics bank at $220000.
	/// Palette indices 4-7 are used for monsters in battle.
	/// </remarks>
	public static readonly Dictionary<int, MonsterSprite> MonsterSprites = new() {
		// Small monsters (16x16)
		[0x01] = new(0x01, 0x0000, SpriteSize.Small, 0x00, 4),   // Slime
		[0x02] = new(0x02, 0x0080, SpriteSize.Small, 0x01, 4),   // Raven
		[0x07] = new(0x07, 0x0100, SpriteSize.Small, 0x02, 4),   // Bubble Slime
		[0x32] = new(0x32, 0x0180, SpriteSize.Small, 0x03, 4),   // Metal Slime
		[0x6D] = new(0x6D, 0x0200, SpriteSize.Small, 0x04, 4),   // Liquid Metal Slime

		// Medium monsters (32x32)
		[0x08] = new(0x08, 0x1000, SpriteSize.Medium, 0x10, 16), // Magician
		[0x23] = new(0x23, 0x1400, SpriteSize.Medium, 0x11, 16), // Druid

		// Large monsters (48x48)
		[0x5D] = new(0x5D, 0x4000, SpriteSize.Large, 0x20, 36),  // Troll
		[0x7D] = new(0x7D, 0x4900, SpriteSize.Large, 0x21, 36),  // Dragon

		// Boss monsters (64x64)
		[0x85] = new(0x85, 0x8000, SpriteSize.Boss, 0x30, 64),   // Baramos
		[0x86] = new(0x86, 0x9000, SpriteSize.Boss, 0x31, 64),   // Zoma
	};

	/// <summary>
	/// Maps DW4 monster ID to recommended DQ3r sprite.
	/// </summary>
	/// <remarks>
	/// Used for converting DW4 monsters to DQ3r format.
	/// Maps similar monsters where direct equivalents exist.
	/// </remarks>
	public static readonly Dictionary<int, int> DW4MonsterToSprite = new() {
		// Direct equivalents
		[0x00] = 0x01, // Slime -> Slime
		[0x01] = 0x07, // Bubble Slime -> Bubble Slime
		[0x32] = 0x32, // Metal Slime -> Metal Slime

		// Similar monsters
		[0x10] = 0x08, // Magician equivalent
	};

	/// <summary>
	/// Get sprite data for a DQ3r monster.
	/// </summary>
	public static MonsterSprite? GetSprite(int monsterId) {
		return MonsterSprites.TryGetValue(monsterId, out var sprite) ? sprite : null;
	}

	/// <summary>
	/// Get the DQ3r sprite ID for a DW4 monster.
	/// </summary>
	public static int? GetSpriteForDW4Monster(int dw4MonsterId) {
		return DW4MonsterToSprite.TryGetValue(dw4MonsterId, out int spriteId) ? spriteId : null;
	}
}
