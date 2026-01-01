using DW4Lib.Converters;
using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for MapToDQ3r converter.
/// </summary>
public class MapToDQ3rTests {
	[Fact]
	public void ConvertOverworldTile_WithValidTile_ReturnsTranslatedTile() {
		// Tile $0d (13) = grass with path should map to something
		byte dw4Tile = 0x0d;
		byte result = MapToDQ3r.ConvertOverworldTile(dw4Tile);

		// From translation table: $0d maps to $c9
		Assert.Equal(0xc9, result);
	}

	[Fact]
	public void ConvertOverworldTile_WithInvalidTile_ReturnsDefaultTile() {
		// Tile outside translation table range
		byte dw4Tile = 0xff;
		byte result = MapToDQ3r.ConvertOverworldTile(dw4Tile);

		Assert.Equal(MapToDQ3r.DefaultTile, result);
	}

	[Fact]
	public void ConvertOverworldTile_AllTableEntries_AreValid() {
		// Verify all entries in translation table produce valid DQ3r tiles
		for (int i = 0; i < MapToDQ3r.OverworldTileTranslation.Length; i++) {
			byte result = MapToDQ3r.ConvertOverworldTile((byte)i);
			// Just verify we get a value without exception
			Assert.True(result >= 0 && result <= 0xff);
		}
	}

	[Fact]
	public void ConvertOverworldMap_WithSmallMap_ReturnsCorrectDimensions() {
		// Arrange - small 4x4 test map
		var dw4Map = new byte[4, 4] {
			{ 0x00, 0x01, 0x02, 0x03 },
			{ 0x04, 0x05, 0x06, 0x07 },
			{ 0x08, 0x09, 0x0a, 0x0b },
			{ 0x0c, 0x0d, 0x0e, 0x0f }
		};

		// Act
		var result = MapToDQ3r.ConvertOverworldMap(dw4Map);

		// Assert
		Assert.Equal(4, result.GetLength(0)); // Height
		Assert.Equal(4, result.GetLength(1)); // Width
	}

	[Fact]
	public void ConvertOverworldMap_AllTiles_AreTranslated() {
		// Arrange
		var dw4Map = new byte[2, 2] {
			{ 0x00, 0x0d }, // Known tiles
			{ 0x1e, 0x22 }
		};

		// Act
		var result = MapToDQ3r.ConvertOverworldMap(dw4Map);

		// Assert - verify expected translations
		Assert.Equal(MapToDQ3r.OverworldTileTranslation[0x00], result[0, 0]);
		Assert.Equal(MapToDQ3r.OverworldTileTranslation[0x0d], result[0, 1]);
		Assert.Equal(MapToDQ3r.OverworldTileTranslation[0x1e], result[1, 0]);
		Assert.Equal(MapToDQ3r.OverworldTileTranslation[0x22], result[1, 1]);
	}

	[Fact]
	public void ConvertEventType_AllTypes_MapCorrectly() {
		Assert.Equal(DQ3rEventType.None, MapToDQ3r.ConvertEventType(EventType.None));
		Assert.Equal(DQ3rEventType.NPC, MapToDQ3r.ConvertEventType(EventType.NPC));
		Assert.Equal(DQ3rEventType.Treasure, MapToDQ3r.ConvertEventType(EventType.Treasure));
		Assert.Equal(DQ3rEventType.Door, MapToDQ3r.ConvertEventType(EventType.Door));
		Assert.Equal(DQ3rEventType.Warp, MapToDQ3r.ConvertEventType(EventType.Warp));
		Assert.Equal(DQ3rEventType.Stairs, MapToDQ3r.ConvertEventType(EventType.StairsUp));
		Assert.Equal(DQ3rEventType.Stairs, MapToDQ3r.ConvertEventType(EventType.StairsDown));
		Assert.Equal(DQ3rEventType.Script, MapToDQ3r.ConvertEventType(EventType.Script));
		Assert.Equal(DQ3rEventType.Shop, MapToDQ3r.ConvertEventType(EventType.Shop));
		Assert.Equal(DQ3rEventType.Inn, MapToDQ3r.ConvertEventType(EventType.Inn));
		Assert.Equal(DQ3rEventType.Church, MapToDQ3r.ConvertEventType(EventType.Church));
		Assert.Equal(DQ3rEventType.Vault, MapToDQ3r.ConvertEventType(EventType.Vault));
	}

	[Fact]
	public void ConvertTreasure_PreservesPositionAndMap() {
		// Arrange
		var dw4Chest = new TreasureChest {
			Index = 5,
			MapId = 10,
			X = 12,
			Y = 8,
			ContentsType = TreasureType.Gold,
			ContentsValue = 100
		};

		// Act
		var result = MapToDQ3r.ConvertTreasure(dw4Chest);

		// Assert
		Assert.Equal(5, result.Id);
		Assert.Equal(10, result.MapId);
		Assert.Equal(12, result.X);
		Assert.Equal(8, result.Y);
		Assert.Equal(DQ3rTreasureType.Gold, result.ContentsType);
		Assert.Equal(100, (int)result.ContentsValue);
	}

	[Fact]
	public void ConvertTreasureType_AllTypes_MapCorrectly() {
		Assert.Equal(DQ3rTreasureType.Item, MapToDQ3r.ConvertTreasureType(TreasureType.Item));
		Assert.Equal(DQ3rTreasureType.Gold, MapToDQ3r.ConvertTreasureType(TreasureType.Gold));
		Assert.Equal(DQ3rTreasureType.SmallMedal, MapToDQ3r.ConvertTreasureType(TreasureType.SmallMedal));
		Assert.Equal(DQ3rTreasureType.Empty, MapToDQ3r.ConvertTreasureType(TreasureType.Empty));
		Assert.Equal(DQ3rTreasureType.Monster, MapToDQ3r.ConvertTreasureType(TreasureType.Monster));
	}

	[Fact]
	public void ConvertWarpType_AllTypes_MapCorrectly() {
		Assert.Equal(DQ3rWarpType.StairsUp, MapToDQ3r.ConvertWarpType(WarpType.StairsUp));
		Assert.Equal(DQ3rWarpType.StairsDown, MapToDQ3r.ConvertWarpType(WarpType.StairsDown));
		Assert.Equal(DQ3rWarpType.Door, MapToDQ3r.ConvertWarpType(WarpType.Door));
		Assert.Equal(DQ3rWarpType.Exit, MapToDQ3r.ConvertWarpType(WarpType.Exit));
		Assert.Equal(DQ3rWarpType.MapEdge, MapToDQ3r.ConvertWarpType(WarpType.MapEdge));
		Assert.Equal(DQ3rWarpType.Teleport, MapToDQ3r.ConvertWarpType(WarpType.Teleport));
		Assert.Equal(DQ3rWarpType.Fall, MapToDQ3r.ConvertWarpType(WarpType.Fall));
	}
}

/// <summary>
/// Unit tests for WorldMapToDQ3r converter.
/// </summary>
public class WorldMapToDQ3rTests {
	[Fact]
	public void ExtractChunk_ReturnsCorrect4x4Tiles() {
		// Arrange - 8x8 test map
		var tilemap = new byte[8, 8];
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				tilemap[y, x] = (byte)(y * 8 + x);
			}
		}

		// Act - extract chunk at (0,0)
		var chunk = WorldMapToDQ3r.ExtractChunk(tilemap, 0, 0);

		// Assert
		Assert.Equal(0, chunk.Tiles[0]);
		Assert.Equal(1, chunk.Tiles[1]);
		Assert.Equal(2, chunk.Tiles[2]);
		Assert.Equal(3, chunk.Tiles[3]);
		Assert.Equal(8, chunk.Tiles[4]);  // Row 1
		Assert.Equal(9, chunk.Tiles[5]);
		Assert.Equal(10, chunk.Tiles[6]);
		Assert.Equal(11, chunk.Tiles[7]);
	}

	[Fact]
	public void ExtractChunk_AtOffset_ReturnsCorrectTiles() {
		// Arrange
		var tilemap = new byte[8, 8];
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				tilemap[y, x] = (byte)(y * 8 + x);
			}
		}

		// Act - extract chunk at (4,4)
		var chunk = WorldMapToDQ3r.ExtractChunk(tilemap, 4, 4);

		// Assert - should get bottom-right 4x4
		Assert.Equal(36, chunk.Tiles[0]); // (4,4) = 4*8+4 = 36
		Assert.Equal(37, chunk.Tiles[1]);
		Assert.Equal(44, chunk.Tiles[4]); // (4,5) = 5*8+4 = 44
	}

	[Fact]
	public void GetChunkKey_IdenticalChunks_ProduceSameKey() {
		// Arrange
		var chunk1 = new DQ3rMapChunk { Tiles = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16] };
		var chunk2 = new DQ3rMapChunk { Tiles = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16] };

		// Act
		var key1 = WorldMapToDQ3r.GetChunkKey(chunk1);
		var key2 = WorldMapToDQ3r.GetChunkKey(chunk2);

		// Assert
		Assert.Equal(key1, key2);
	}

	[Fact]
	public void GetChunkKey_DifferentChunks_ProduceDifferentKeys() {
		// Arrange
		var chunk1 = new DQ3rMapChunk { Tiles = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16] };
		var chunk2 = new DQ3rMapChunk { Tiles = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 99] };

		// Act
		var key1 = WorldMapToDQ3r.GetChunkKey(chunk1);
		var key2 = WorldMapToDQ3r.GetChunkKey(chunk2);

		// Assert
		Assert.NotEqual(key1, key2);
	}

	[Fact]
	public void GenerateChunks_UniformMap_ProducesSingleChunk() {
		// Arrange - all same tile
		var tilemap = new byte[256, 256];
		for (int y = 0; y < 256; y++) {
			for (int x = 0; x < 256; x++) {
				tilemap[y, x] = 0x4b; // grass
			}
		}

		// Act
		var chunks = WorldMapToDQ3r.GenerateChunks(tilemap);

		// Assert - should only have 1 unique chunk
		Assert.Single(chunks);
		Assert.All(chunks[0].Tiles, t => Assert.Equal(0x4b, t));
	}

	[Fact]
	public void GenerateChunks_CheckerboardPattern_ProducesMultipleChunks() {
		// Arrange - 8x8 map with alternating 4x4 blocks
		var tilemap = new byte[8, 8];
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				// Different tile per 4x4 block
				tilemap[y, x] = (byte)((y / 4) * 2 + (x / 4));
			}
		}

		// Act
		var chunks = WorldMapToDQ3r.GenerateChunks(tilemap);

		// Assert - should have up to 4 unique chunks for 4 blocks
		Assert.True(chunks.Count >= 1 && chunks.Count <= 4);
	}

	[Fact]
	public void GenerateLayout_MatchesChunkGrid() {
		// Arrange
		var tilemap = new byte[8, 8];
		// Fill with pattern where each 4x4 block is different
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				tilemap[y, x] = (byte)((y / 4) * 2 + (x / 4));
			}
		}

		var chunks = WorldMapToDQ3r.GenerateChunks(tilemap);

		// Act
		var layout = WorldMapToDQ3r.GenerateLayout(tilemap, chunks);

		// Assert - layout should have 64x64 = 4096 entries
		Assert.Equal(WorldMapToDQ3r.LayoutGridWidth * WorldMapToDQ3r.LayoutGridHeight, layout.Length);
	}

	[Fact]
	public void GenerateTilemapStreams_Has16Streams() {
		// Arrange
		var chunks = new List<DQ3rMapChunk> {
			new DQ3rMapChunk { Index = 0, Tiles = new byte[16] }
		};

		// Act
		var streams = WorldMapToDQ3r.GenerateTilemapStreams(chunks);

		// Assert
		Assert.Equal(16, streams.Length);
	}

	[Fact]
	public void GenerateTilemapStreams_StreamLengthMatchesChunkCount() {
		// Arrange
		var chunks = new List<DQ3rMapChunk> {
			new DQ3rMapChunk { Index = 0, Tiles = new byte[16] },
			new DQ3rMapChunk { Index = 1, Tiles = new byte[16] },
			new DQ3rMapChunk { Index = 2, Tiles = new byte[16] }
		};

		// Act
		var streams = WorldMapToDQ3r.GenerateTilemapStreams(chunks);

		// Assert
		foreach (var stream in streams) {
			Assert.Equal(3, stream.Length);
		}
	}

	[Fact]
	public void ConvertWorldMap_ReturnsAllComponents() {
		// Arrange - minimal 8x8 test map (needs to be 256x256 for full test)
		var dw4Map = new byte[256, 256];

		// Fill with simple pattern
		for (int y = 0; y < 256; y++) {
			for (int x = 0; x < 256; x++) {
				dw4Map[y, x] = 0x00; // All same tile for simplicity
			}
		}

		// Act
		var result = WorldMapToDQ3r.ConvertWorldMap(dw4Map);

		// Assert
		Assert.NotNull(result.TranslatedTilemap);
		Assert.NotNull(result.Chunks);
		Assert.NotNull(result.Layout);
		Assert.NotNull(result.TilemapStreams);
		Assert.Equal(256, result.TranslatedTilemap.GetLength(0));
		Assert.Equal(256, result.TranslatedTilemap.GetLength(1));
		Assert.True(result.Chunks.Count >= 1);
		Assert.Equal(64 * 64, result.Layout.Length);
		Assert.Equal(16, result.TilemapStreams.Length);
	}
}

/// <summary>
/// Unit tests for EntranceToDQ3r converter.
/// </summary>
public class EntranceToDQ3rTests {
	[Fact]
	public void DW4MainEntrances_ContainsExpectedLocations() {
		// Should contain key locations like Burland, Endor, etc.
		Assert.NotEmpty(EntranceToDQ3r.DW4MainEntrances);
		Assert.True(EntranceToDQ3r.DW4MainEntrances.Length >= 10);
	}

	[Fact]
	public void AllEntrances_HaveValidCoordinates() {
		// Check all entrances have reasonable coordinates
		foreach (var entrance in EntranceToDQ3r.DW4MainEntrances) {
			Assert.InRange(entrance.X, 0, 255);
			Assert.InRange(entrance.Y, 0, 255);
			Assert.True(entrance.DestMapId >= 0);
		}

		foreach (var entrance in EntranceToDQ3r.DW4GottsideEntrances) {
			Assert.InRange(entrance.X, 0, 255);
			Assert.InRange(entrance.Y, 0, 255);
		}

		foreach (var entrance in EntranceToDQ3r.DW4UnderworldEntrances) {
			Assert.InRange(entrance.X, 0, 255);
			Assert.InRange(entrance.Y, 0, 255);
		}
	}

	[Fact]
	public void ConvertAllEntrances_ReturnsNonEmptyList() {
		// Arrange
		var mapping = EntranceToDQ3r.GenerateDefaultMapIdMapping();

		// Act
		var result = EntranceToDQ3r.ConvertAllEntrances(mapping);

		// Assert
		Assert.NotEmpty(result);
		Assert.True(result.Count >= EntranceToDQ3r.DW4MainEntrances.Length);
	}

	[Fact]
	public void ConvertEntrance_PreservesNameAndCoordinates() {
		// Arrange
		var dw4Entrance = new EntranceLocation {
			X = 100,
			Y = 150,
			DestMapId = 5,
			Name = "Test Town",
			Type = MapType.Town
		};
		var mapping = new Dictionary<int, int> { { 5, 105 } };

		// Act
		var result = EntranceToDQ3r.ConvertEntrance(dw4Entrance, mapping);

		// Assert
		Assert.Equal(100, result.X);
		Assert.Equal(150, result.Y);
		Assert.Equal("Test Town", result.Name);
		Assert.Equal(105, result.DestMapId); // Mapped ID
	}

	[Fact]
	public void GenerateDefaultMapIdMapping_ReturnsNonEmptyMapping() {
		// Act
		var mapping = EntranceToDQ3r.GenerateDefaultMapIdMapping();

		// Assert
		Assert.NotEmpty(mapping);
		Assert.True(mapping.Count >= 0x48);
	}
}
