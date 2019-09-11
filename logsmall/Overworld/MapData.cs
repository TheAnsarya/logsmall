using logsmall.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.Overworld {
	public class MapData {
		// Overworld tilemap top-level grid layout
		public static byte[] GetLayout() {
			var source = Rom.GetStream(0xed8a00);
			return BasicRing400.Decompress(source, 0x2000);
		}

		public static RomByteArray[,] GetOverworldTilemaps() {
			var tilemaps = new RomByteArray[4, 4] {
				// Top Row
				{
					Rom.ByteArray(0xeda49c),	// Top left tile
					Rom.ByteArray(0xedaed9),
					Rom.ByteArray(0xedb916),
					Rom.ByteArray(0xedc353)		// Top right tile
				},
				{
					Rom.ByteArray(0xedcd90),
					Rom.ByteArray(0xedd7cd),
					Rom.ByteArray(0xede20a),
					Rom.ByteArray(0xedec47)
				},
				{
					Rom.ByteArray(0xedf684),
					Rom.ByteArray(0xee00c1),
					Rom.ByteArray(0xee0afe),
					Rom.ByteArray(0xee153b)
				},
				// Bottom row
				{
					Rom.ByteArray(0xee1f78),	// Bottom left tile
					Rom.ByteArray(0xee29b5),
					Rom.ByteArray(0xee33f2),
					Rom.ByteArray(0xee3e2f)		// Bottom right tile
				}
			};

			return tilemaps;
		}

		public static byte[,] GetTilemapData() {
			var layout = GetLayout();
			var tilemaps = GetOverworldTilemaps();

			// fullmap[y, x]
			var fullmap = new byte[0x100, 0x100];
			for (int i = 0; i < 0x1000; i++) {
				// each 4 by 4 chunk
				var sourceIndex = layout[i * 2];
				var xStart = (i % 64) * 4;
				var yStart = (i / 64) * 4;

				for (int k = 0; k < 4; k++) {
					fullmap[yStart + 0, xStart + k] = tilemaps[0, k][sourceIndex];
					fullmap[yStart + 1, xStart + k] = tilemaps[1, k][sourceIndex];
					fullmap[yStart + 2, xStart + k] = tilemaps[2, k][sourceIndex];
					fullmap[yStart + 3, xStart + k] = tilemaps[3, k][sourceIndex];
				}
			}

			return fullmap;
		}
	}
}
