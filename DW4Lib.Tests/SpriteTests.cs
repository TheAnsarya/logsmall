using DW4Lib.Graphics;

namespace DW4Lib.Tests;

/// <summary>
/// Tests for sprite data and conversion.
/// </summary>
public class SpriteTests {
	// ============================================================
	// SpriteData Constants Tests
	// ============================================================

	[Fact]
	public void SpriteData_ConstantsAreValid() {
		Assert.True(SpriteData.BytesPerNesTile == 16);
		Assert.True(SpriteData.BytesPerFrame == 96); // 6 tiles × 16 bytes
		Assert.True(SpriteData.TilesPerFrame == 6); // 2×3 tiles
	}

	[Fact]
	public void SpriteData_CharacterSpriteIdsAreUnique() {
		var ids = new[] {
			SpriteData.SpriteRagnar,
			SpriteData.SpriteAlena,
			SpriteData.SpriteCristo,
			SpriteData.SpriteBrey,
			SpriteData.SpriteTaloon,
			SpriteData.SpriteNara,
			SpriteData.SpriteMara,
			SpriteData.SpriteHero
		};
		Assert.Equal(ids.Length, ids.Distinct().Count());
	}

	[Fact]
	public void SpriteData_NpcSpriteIdsAreValid() {
		Assert.True(SpriteData.SpriteKing > 0);
		Assert.True(SpriteData.SpriteSoldier > 0);
		Assert.True(SpriteData.SpritePriest > 0);
	}

	// ============================================================
	// Chapter1Sprites Tests
	// ============================================================

	[Fact]
	public void Chapter1Sprites_GetRagnarSprite_HasCorrectData() {
		var sprite = Chapter1Sprites.GetRagnarSprite();
		Assert.Equal(SpriteData.SpriteRagnar, sprite.SpriteId);
		Assert.Equal("Ragnar McRyan", sprite.Name);
		Assert.Equal(8, sprite.FrameCount);
	}

	[Fact]
	public void Chapter1Sprites_GetRagnarSprite_HasAllDirections() {
		var sprite = Chapter1Sprites.GetRagnarSprite();
		Assert.Equal(4, sprite.Animations.Length);
		Assert.Contains(sprite.Animations, a => a.Name.Contains("Down"));
		Assert.Contains(sprite.Animations, a => a.Name.Contains("Left"));
		Assert.Contains(sprite.Animations, a => a.Name.Contains("Right"));
		Assert.Contains(sprite.Animations, a => a.Name.Contains("Up"));
	}

	[Fact]
	public void Chapter1Sprites_GetHealieSprite_HasCorrectData() {
		var sprite = Chapter1Sprites.GetHealieSprite();
		Assert.Equal(SpriteData.SpriteHealie, sprite.SpriteId);
		Assert.Equal("Healie", sprite.Name);
		Assert.Equal(16, sprite.FrameHeight); // Healie is smaller
	}

	[Fact]
	public void Chapter1Sprites_GetAllCharacterSprites_ReturnsChapter1Characters() {
		var sprites = Chapter1Sprites.GetAllCharacterSprites();
		Assert.Equal(2, sprites.Length); // Ragnar and Healie
		Assert.Contains(sprites, s => s.Name == "Ragnar McRyan");
		Assert.Contains(sprites, s => s.Name == "Healie");
	}

	[Fact]
	public void Chapter1Sprites_GetAllNpcSprites_ReturnsNpcs() {
		var sprites = Chapter1Sprites.GetAllNpcSprites();
		Assert.NotEmpty(sprites);
		Assert.Contains(sprites, s => s.Name == "King of Burland");
		Assert.Contains(sprites, s => s.Name == "Soldier");
	}

	[Fact]
	public void Chapter1Sprites_GetSpritePalettes_ReturnsPalettes() {
		var palettes = Chapter1Sprites.GetSpritePalettes();
		Assert.NotEmpty(palettes);
		Assert.True(palettes.Length >= 4); // At least 4 palettes

		foreach (var pal in palettes) {
			Assert.Equal(4, pal.Colors.Length);
			Assert.NotEmpty(pal.Name);
		}
	}

	[Fact]
	public void Chapter1Sprites_AllAnimationsHaveFrames() {
		var sprites = Chapter1Sprites.GetAllCharacterSprites();
		foreach (var sprite in sprites) {
			foreach (var anim in sprite.Animations) {
				Assert.NotEmpty(anim.FrameIndices);
				Assert.Equal(anim.FrameIndices.Length, anim.FrameDurations.Length);
			}
		}
	}

	// ============================================================
	// SpriteAnimation Tests
	// ============================================================

	[Fact]
	public void SpriteAnimation_WalkAnimationsLoop() {
		var ragnar = Chapter1Sprites.GetRagnarSprite();
		Assert.All(ragnar.Animations, a => Assert.True(a.Loops));
	}

	[Fact]
	public void SpriteAnimation_FrameDurationsAreValid() {
		var ragnar = Chapter1Sprites.GetRagnarSprite();
		foreach (var anim in ragnar.Animations) {
			Assert.All(anim.FrameDurations, d => Assert.True(d > 0));
		}
	}

	// ============================================================
	// SpriteToDQ3r Conversion Tests
	// ============================================================

	[Fact]
	public void SpriteToDQ3r_ConvertSpriteId_AddsOffset() {
		ushort converted = SpriteToDQ3r.ConvertSpriteId(SpriteData.SpriteRagnar);
		Assert.Equal((ushort)(SpriteData.SpriteRagnar + SpriteToDQ3r.SpriteIdOffset), converted);
	}

	[Fact]
	public void SpriteToDQ3r_ConvertSpriteIdBack_RemovesOffset() {
		ushort dq3rId = (ushort)(SpriteData.SpriteRagnar + SpriteToDQ3r.SpriteIdOffset);
		byte dw4Id = SpriteToDQ3r.ConvertSpriteIdBack(dq3rId);
		Assert.Equal(SpriteData.SpriteRagnar, dw4Id);
	}

	[Fact]
	public void SpriteToDQ3r_ConvertSpriteSheet_ConvertsCorrectly() {
		var dw4Sprite = Chapter1Sprites.GetRagnarSprite();
		var dq3rSprite = SpriteToDQ3r.ConvertSpriteSheet(dw4Sprite);

		Assert.Equal(dw4Sprite.SpriteId + SpriteToDQ3r.SpriteIdOffset, dq3rSprite.SpriteId);
		Assert.Equal(dw4Sprite.Name, dq3rSprite.Name);
		Assert.Equal(dw4Sprite.FrameCount, dq3rSprite.FrameCount);
		Assert.Equal(dw4Sprite.Animations.Length, dq3rSprite.Animations.Length);
	}

	[Fact]
	public void SpriteToDQ3r_ConvertAnimation_ConvertsCorrectly() {
		var dw4Anim = new SpriteAnimation {
			SpriteId = 0x10,
			Name = "Test Animation",
			FrameIndices = [0, 1, 2],
			FrameDurations = [8, 8, 8],
			Loops = true
		};

		var dq3rAnim = SpriteToDQ3r.ConvertAnimation(dw4Anim);

		Assert.Equal(dw4Anim.SpriteId + SpriteToDQ3r.SpriteIdOffset, dq3rAnim.SpriteId);
		Assert.Equal(dw4Anim.Name, dq3rAnim.Name);
		Assert.Equal(dw4Anim.FrameIndices, dq3rAnim.FrameIndices);
		Assert.Equal(dw4Anim.FrameDurations, dq3rAnim.FrameDurations);
		Assert.Equal(dw4Anim.Loops, dq3rAnim.Loops);
	}

	[Fact]
	public void SpriteToDQ3r_ConvertPalette_ConvertsToSnesFormat() {
		var dw4Palette = new SpritePalette {
			Index = 0,
			Colors = [0x0F, 0x30, 0x10, 0x00], // Black, white, gray, black
			Name = "Test Palette"
		};

		var dq3rPalette = SpriteToDQ3r.ConvertPalette(dw4Palette);

		Assert.Equal(dw4Palette.Index, dq3rPalette.Index);
		Assert.Equal(dw4Palette.Name, dq3rPalette.Name);
		Assert.Equal(16, dq3rPalette.Colors.Length);
		Assert.Equal((ushort)0x0000, dq3rPalette.Colors[0]); // Transparent
	}

	[Fact]
	public void SpriteToDQ3r_ConvertChapter1CharacterSprites_ReturnsConvertedSprites() {
		var converted = SpriteToDQ3r.ConvertChapter1CharacterSprites();
		Assert.Equal(2, converted.Length);
		Assert.All(converted, s => Assert.True(s.SpriteId >= SpriteToDQ3r.SpriteIdOffset));
	}

	[Fact]
	public void SpriteToDQ3r_ConvertChapter1NpcSprites_ReturnsConvertedSprites() {
		var converted = SpriteToDQ3r.ConvertChapter1NpcSprites();
		Assert.NotEmpty(converted);
		Assert.All(converted, s => Assert.True(s.SpriteId >= SpriteToDQ3r.SpriteIdOffset));
	}

	[Fact]
	public void SpriteToDQ3r_ConvertChapter1Palettes_ReturnsConvertedPalettes() {
		var converted = SpriteToDQ3r.ConvertChapter1Palettes();
		Assert.NotEmpty(converted);
		Assert.All(converted, p => Assert.Equal(16, p.Colors.Length));
	}

	[Fact]
	public void SpriteToDQ3r_BuildChapter1SpriteResource_ReturnsCompleteResource() {
		var resource = SpriteToDQ3r.BuildChapter1SpriteResource();

		Assert.Equal(1, resource.Chapter);
		Assert.NotEmpty(resource.CharacterSprites);
		Assert.NotEmpty(resource.NpcSprites);
		Assert.NotEmpty(resource.Palettes);
		Assert.True(resource.TotalSpriteCount > 0);
	}

	// ============================================================
	// DQ3rSpriteSheet Tests
	// ============================================================

	[Fact]
	public void DQ3rSpriteSheet_ToBytes_ProducesValidData() {
		var sheet = new DQ3rSpriteSheet {
			SpriteId = 0x0400,
			Name = "Test",
			FrameCount = 4,
			FrameWidth = 16,
			FrameHeight = 24,
			PaletteIndices = [0, 0, 0, 0],
			TileData = new byte[96],
			Animations = [
				new DQ3rSpriteAnimation {
					SpriteId = 0x0400,
					Name = "Walk",
					FrameIndices = [0, 1],
					FrameDurations = [8, 8],
					Loops = true
				}
			]
		};

		var bytes = sheet.ToBytes();
		Assert.NotEmpty(bytes);
		// First 2 bytes should be sprite ID
		Assert.Equal(0x00, bytes[0]);
		Assert.Equal(0x04, bytes[1]);
	}

	// ============================================================
	// DQ3rSpritePalette Tests
	// ============================================================

	[Fact]
	public void DQ3rSpritePalette_ToBytes_Produces32Bytes() {
		var palette = new DQ3rSpritePalette {
			Colors = new ushort[16]
		};

		var bytes = palette.ToBytes();
		Assert.Equal(32, bytes.Length);
	}

	[Fact]
	public void DQ3rSpritePalette_ToBytes_SerializesCorrectly() {
		var palette = new DQ3rSpritePalette {
			Colors = new ushort[16]
		};
		palette.Colors[0] = 0x7FFF; // White
		palette.Colors[1] = 0x001F; // Red

		var bytes = palette.ToBytes();
		Assert.Equal(0xFF, bytes[0]); // Low byte of white
		Assert.Equal(0x7F, bytes[1]); // High byte of white
		Assert.Equal(0x1F, bytes[2]); // Low byte of red
		Assert.Equal(0x00, bytes[3]); // High byte of red
	}

	[Fact]
	public void DQ3rSpritePalette_FromBytes_DeserializesCorrectly() {
		var originalPalette = new DQ3rSpritePalette {
			Colors = new ushort[16]
		};
		originalPalette.Colors[0] = 0x7FFF;
		originalPalette.Colors[1] = 0x001F;

		var bytes = originalPalette.ToBytes();
		var restored = DQ3rSpritePalette.FromBytes(bytes);

		Assert.Equal(originalPalette.Colors[0], restored.Colors[0]);
		Assert.Equal(originalPalette.Colors[1], restored.Colors[1]);
	}

	[Fact]
	public void DQ3rSpritePalette_RoundTrip_PreservesData() {
		var palettes = SpriteToDQ3r.ConvertChapter1Palettes();
		foreach (var palette in palettes) {
			var bytes = palette.ToBytes();
			var restored = DQ3rSpritePalette.FromBytes(bytes);

			for (int i = 0; i < 16; i++) {
				Assert.Equal(palette.Colors[i], restored.Colors[i]);
			}
		}
	}

	// ============================================================
	// DQ3rSpriteResource Tests
	// ============================================================

	[Fact]
	public void DQ3rSpriteResource_GetAllSprites_CombinesAllSprites() {
		var resource = SpriteToDQ3r.BuildChapter1SpriteResource();
		var allSprites = resource.GetAllSprites().ToList();

		Assert.Equal(resource.TotalSpriteCount, allSprites.Count);
		Assert.Equal(resource.CharacterSprites.Length + resource.NpcSprites.Length, allSprites.Count);
	}

	// ============================================================
	// Integration Tests
	// ============================================================

	[Fact]
	public void Chapter1_AllCharacterSpritesHaveAnimations() {
		var sprites = Chapter1Sprites.GetAllCharacterSprites();
		foreach (var sprite in sprites) {
			Assert.NotEmpty(sprite.Animations);
		}
	}

	[Fact]
	public void Chapter1_AllPalettesHave4Colors() {
		var palettes = Chapter1Sprites.GetSpritePalettes();
		Assert.All(palettes, p => Assert.Equal(4, p.Colors.Length));
	}

	[Fact]
	public void Chapter1_RagnarAndHealieHaveDistinctSpriteIds() {
		var ragnar = Chapter1Sprites.GetRagnarSprite();
		var healie = Chapter1Sprites.GetHealieSprite();

		Assert.NotEqual(ragnar.SpriteId, healie.SpriteId);
	}
}
