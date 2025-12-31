using DW4Lib.DataStructures.Maps;
using DW4Lib.DQ3r.Maps;

namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 NES world/overworld map to DQ3r SNES format.
/// Handles the complete conversion pipeline including compression.
/// </summary>
public static class WorldMapToDQ3r {
	/// <summary>
	/// DW4 overworld map dimensions.
	/// </summary>
	public const int DW4MapWidth = 256;
	public const int DW4MapHeight = 256;

	/// <summary>
	/// DQ3r overworld map dimensions.
	/// </summary>
	public const int DQ3rMapWidth = 256;
	public const int DQ3rMapHeight = 256;

	/// <summary>
	/// DQ3r chunk size (4x4 tiles).
	/// </summary>
	public const int ChunkSize = 4;

	/// <summary>
	/// DQ3r layout grid dimensions.
	/// </summary>
	public const int LayoutGridWidth = 64;
	public const int LayoutGridHeight = 64;

	/// <summary>
	/// Convert complete DW4 overworld to DQ3r format.
	/// </summary>
	public static WorldMapConversionResult ConvertWorldMap(byte[,] dw4Map) {
		var result = new WorldMapConversionResult();

		// Step 1: Translate tiles
		result.TranslatedTilemap = MapToDQ3r.ConvertOverworldMap(dw4Map);

		// Step 2: Generate chunks from tilemap
		result.Chunks = GenerateChunks(result.TranslatedTilemap);

		// Step 3: Generate layout indices
		result.Layout = GenerateLayout(result.TranslatedTilemap, result.Chunks);

		// Step 4: Generate tilemap chunk data streams (16 streams)
		result.TilemapStreams = GenerateTilemapStreams(result.Chunks);

		// Step 5: Compress layout data
		result.CompressedLayout = CompressLayout(result.Layout);

		return result;
	}

	/// <summary>
	/// Generate unique chunks from tilemap.
	/// </summary>
	public static List<DQ3rMapChunk> GenerateChunks(byte[,] tilemap) {
		var chunks = new List<DQ3rMapChunk>();
		var chunkLookup = new Dictionary<string, int>();

		for (int gridY = 0; gridY < LayoutGridHeight; gridY++) {
			for (int gridX = 0; gridX < LayoutGridWidth; gridX++) {
				var chunk = ExtractChunk(tilemap, gridX * ChunkSize, gridY * ChunkSize);
				var key = GetChunkKey(chunk);

				if (!chunkLookup.ContainsKey(key)) {
					chunk.Index = chunks.Count;
					chunks.Add(chunk);
					chunkLookup[key] = chunk.Index;
				}
			}
		}

		return chunks;
	}

	/// <summary>
	/// Extract a 4x4 chunk from tilemap.
	/// </summary>
	public static DQ3rMapChunk ExtractChunk(byte[,] tilemap, int startX, int startY) {
		var chunk = new DQ3rMapChunk();

		for (int y = 0; y < ChunkSize; y++) {
			for (int x = 0; x < ChunkSize; x++) {
				int tileX = startX + x;
				int tileY = startY + y;

				// Handle wrap-around for edge cases
				if (tileX >= tilemap.GetLength(1)) tileX %= tilemap.GetLength(1);
				if (tileY >= tilemap.GetLength(0)) tileY %= tilemap.GetLength(0);

				chunk.Tiles[y * ChunkSize + x] = tilemap[tileY, tileX];
			}
		}

		return chunk;
	}

	/// <summary>
	/// Generate unique key for chunk comparison.
	/// </summary>
	public static string GetChunkKey(DQ3rMapChunk chunk) {
		return string.Join(",", chunk.Tiles);
	}

	/// <summary>
	/// Generate layout indices from tilemap and chunk list.
	/// </summary>
	public static DQ3rLayoutEntry[] GenerateLayout(byte[,] tilemap, List<DQ3rMapChunk> chunks) {
		var chunkLookup = new Dictionary<string, int>();
		for (int i = 0; i < chunks.Count; i++) {
			chunkLookup[GetChunkKey(chunks[i])] = i;
		}

		var layout = new DQ3rLayoutEntry[LayoutGridWidth * LayoutGridHeight];

		for (int gridY = 0; gridY < LayoutGridHeight; gridY++) {
			for (int gridX = 0; gridX < LayoutGridWidth; gridX++) {
				var chunk = ExtractChunk(tilemap, gridX * ChunkSize, gridY * ChunkSize);
				var key = GetChunkKey(chunk);
				int index = gridY * LayoutGridWidth + gridX;

				layout[index] = new DQ3rLayoutEntry {
					Index = index,
					ChunkIndex = (byte)chunkLookup[key],
					HighByte = 0
				};
			}
		}

		return layout;
	}

	/// <summary>
	/// Generate 16 tilemap data streams (4x4 grid of streams).
	/// Each stream contains one tile from each chunk.
	/// </summary>
	public static byte[][] GenerateTilemapStreams(List<DQ3rMapChunk> chunks) {
		var streams = new byte[16][];

		for (int i = 0; i < 16; i++) {
			streams[i] = new byte[chunks.Count];
		}

		for (int chunkIdx = 0; chunkIdx < chunks.Count; chunkIdx++) {
			for (int tileIdx = 0; tileIdx < 16; tileIdx++) {
				streams[tileIdx][chunkIdx] = chunks[chunkIdx].Tiles[tileIdx];
			}
		}

		return streams;
	}

	/// <summary>
	/// Compress layout data using Ring400 algorithm.
	/// Based on DQ3r decompression research.
	/// </summary>
	public static byte[] CompressLayout(DQ3rLayoutEntry[] layout) {
		var data = new byte[layout.Length * 2];
		for (int i = 0; i < layout.Length; i++) {
			data[i * 2] = layout[i].ChunkIndex;
			data[i * 2 + 1] = layout[i].HighByte;
		}

		return CompressRing400(data);
	}

	/// <summary>
	/// Ring400 compression (reverse of DQ3r decompression).
	/// </summary>
	public static byte[] CompressRing400(byte[] data) {
		var result = new List<byte>();
		var ring = new byte[0x400];
		int ringPos = 0x3BE;
		int i = 0;

		while (i < data.Length) {
			// Find best match in ring buffer
			var (matchLength, matchOffset) = FindBestMatch(data, i, ring, ringPos);

			if (matchLength >= 3) {
				// Encode as reference (2 bytes)
				// 10 bits = offset, 6 bits = length-3
				byte lowByte = (byte)(matchOffset & 0xFF);
				byte highByte = (byte)(((matchOffset >> 2) & 0xC0) | ((matchLength - 3) & 0x3F));

				result.Add(lowByte);
				result.Add(highByte);

				// Update ring buffer
				for (int j = 0; j < matchLength; j++) {
					ring[ringPos] = data[i + j];
					ringPos = (ringPos + 1) & 0x3FF;
				}
				i += matchLength;
			} else {
				// Encode as literal (command bit = 1)
				// For simplicity, encode as single literal
				result.Add(data[i]);
				ring[ringPos] = data[i];
				ringPos = (ringPos + 1) & 0x3FF;
				i++;
			}
		}

		return result.ToArray();
	}

	/// <summary>
	/// Find best match in ring buffer.
	/// </summary>
	private static (int Length, int Offset) FindBestMatch(byte[] data, int pos, byte[] ring, int ringPos) {
		int bestLength = 0;
		int bestOffset = 0;

		for (int offset = 0; offset < 0x400; offset++) {
			int length = 0;
			while (length < 66 && // Max length is 66 (6 bits + 3)
				   pos + length < data.Length &&
				   data[pos + length] == ring[(offset + length) & 0x3FF]) {
				length++;
			}

			if (length > bestLength) {
				bestLength = length;
				bestOffset = offset;
			}
		}

		return (bestLength, bestOffset);
	}
}

/// <summary>
/// Result of world map conversion.
/// </summary>
public class WorldMapConversionResult {
	/// <summary>
	/// Tilemap after tile translation (256x256).
	/// </summary>
	public byte[,] TranslatedTilemap { get; set; } = new byte[256, 256];

	/// <summary>
	/// Unique chunks extracted from tilemap.
	/// </summary>
	public List<DQ3rMapChunk> Chunks { get; set; } = [];

	/// <summary>
	/// Layout entries (4096 entries, 64x64 grid).
	/// </summary>
	public DQ3rLayoutEntry[] Layout { get; set; } = [];

	/// <summary>
	/// 16 tilemap data streams (one per tile position in chunk).
	/// </summary>
	public byte[][] TilemapStreams { get; set; } = [];

	/// <summary>
	/// Compressed layout data (Ring400 format).
	/// </summary>
	public byte[] CompressedLayout { get; set; } = [];

	/// <summary>
	/// Total unique chunks generated.
	/// </summary>
	public int UniqueChunkCount => Chunks.Count;

	/// <summary>
	/// Check if chunk count is within DQ3r limits (256).
	/// </summary>
	public bool IsValid => UniqueChunkCount <= 256;

	/// <summary>
	/// Compression ratio achieved.
	/// </summary>
	public double CompressionRatio =>
		(double)CompressedLayout.Length / (Layout.Length * 2);
}

/// <summary>
/// Extension methods for world map conversion.
/// </summary>
public static class WorldMapExtensions {
	/// <summary>
	/// Export tilemap to text file format.
	/// </summary>
	public static string[] ToTextLines(this byte[,] tilemap) {
		int height = tilemap.GetLength(0);
		int width = tilemap.GetLength(1);
		var lines = new string[height];

		for (int y = 0; y < height; y++) {
			var row = new byte[width];
			for (int x = 0; x < width; x++) {
				row[x] = tilemap[y, x];
			}
			lines[y] = string.Join(" ", row.Select(b => $"{b:x2}"));
		}

		return lines;
	}

	/// <summary>
	/// Load tilemap from text file format.
	/// </summary>
	public static byte[,] FromTextLines(this string[] lines) {
		int height = lines.Length;
		int width = lines[0].Split(' ').Length;
		var tilemap = new byte[height, width];

		for (int y = 0; y < height; y++) {
			var values = lines[y].Split(' ')
				.Select(s => Convert.ToByte(s, 16))
				.ToArray();
			for (int x = 0; x < width; x++) {
				tilemap[y, x] = values[x];
			}
		}

		return tilemap;
	}
}
