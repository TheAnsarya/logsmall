using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	class OverworldMap2 {


		public static void GetMapImage() {
			var layout = GetLayout();

			var tilemaps = new RomByteArray[4, 4] {
				{
					Rom.ByteArray(0xeda49c),
					Rom.ByteArray(0xedaed9),
					Rom.ByteArray(0xedb916),
					Rom.ByteArray(0xedc353)
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
				{
					Rom.ByteArray(0xee1f78),
					Rom.ByteArray(0xee29b5),
					Rom.ByteArray(0xee33f2),
					Rom.ByteArray(0xee3e2f)
				}
			};

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
																  
			var sources =
			Enumerable.Range(0, 256)
			.ToDictionary(
				x => x,
				x => new Rectangle((x % 16) * 16, (x / 16) * 16, 16, 16)
			);

			using (var tiles = Image.FromFile(@"C:\working\dq3\overworld\dq3-tiles.png")) {
				using (var img = new Bitmap(4096, 4096)) {
					using (var g = Graphics.FromImage(img)) {

						for (var row = 0; row < 256; row++) {
							for (var column = 0; column < 256; column++) {
								g.DrawImage(tiles, new Rectangle(column * 16, row * 16, 16, 16), sources[fullmap[row, column]], GraphicsUnit.Pixel);
							}
						}

						//for (int i = 0; i < 64; i++) {
						//	for (int k = 0; k < 4; k++) {
						//		for (int j = 0; j < 48; j++) {
						//			var offset = (i * 48) + j;
						//			var y = ((i * 4) + k) * 16;
						//			g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 0), y, 16, 16), sources[tilemaps[k, 0][offset]], GraphicsUnit.Pixel);
						//			g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 1), y, 16, 16), sources[tilemaps[k, 1][offset]], GraphicsUnit.Pixel);
						//			g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 2), y, 16, 16), sources[tilemaps[k, 2][offset]], GraphicsUnit.Pixel);
						//			g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 3), y, 16, 16), sources[tilemaps[k, 3][offset]], GraphicsUnit.Pixel);
						//		}
						//	}
						//}

						//for (int i = 0; i < 256; i++) {
						//	var offset = 0;
						//	for (int j = 0; j < 64; j++) {
						//		offset = (i * 64 ) + j;
						//		if (offset == 0xa3d) {
						//			break;
						//		}
						//		/*Console.WriteLine((i * 64) + j);
						//		if (offset == 0x069C) {
						//			Console.WriteLine($"a: {tilemaps[i % 4, 0][offset].ToString("x2")}");
						//			Console.WriteLine($"b: {tilemaps[i % 4, 1][offset].ToString("x2")}");
						//			Console.WriteLine($"c: {tilemaps[i % 4, 2][offset].ToString("x2")}");
						//			Console.WriteLine($"d: {tilemaps[i % 4, 3][offset].ToString("x2")}");
						//			var fff = 0;
						//		}
						//		*/
						//		g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 0), (i * 16), 16, 16), sources[tilemaps[i % 4, 0][offset]], GraphicsUnit.Pixel);
						//		g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 1), (i * 16), 16, 16), sources[tilemaps[i % 4, 1][offset]], GraphicsUnit.Pixel);
						//		g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 2), (i * 16), 16, 16), sources[tilemaps[i % 4, 2][offset]], GraphicsUnit.Pixel);
						//		g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 3), (i * 16), 16, 16), sources[tilemaps[i % 4, 3][offset]], GraphicsUnit.Pixel);
						//	}

						//	if (offset == 0xa3d) {
						//		break;
						//	}
						//}
					}

					img.Save(@"C:\working\dq3\overworld\overworld.png", ImageFormat.Png);
				}
			}
			//Console.ReadKey();
		}

		public static void MakeTilesImage() {
			var sourceA = new Rectangle(24, 15, 16, 16);
			var sourceB = new Rectangle(88, 15, 16, 16);
			var sourceC = new Rectangle(152, 15, 16, 16);
			var sourceD = new Rectangle(216, 15, 16, 16);

			using (var tiles = new Bitmap(256, 256)) {
				using (var g = Graphics.FromImage(tiles)) {

					for (int i = 0; i < 64; i++) {
						var filename = $@"C:\working\dq3\overworld\screenshots\dq3-map test 1_{i.ToString("d3")}.png";
						using (var img = Image.FromFile(filename)) {

							var x = (i % 4) * 64;
							var y = (i / 4) * 16;

							g.DrawImage(img, new Rectangle(x + (16 * 0), y, 16, 16), sourceA, GraphicsUnit.Pixel);
							g.DrawImage(img, new Rectangle(x + (16 * 1), y, 16, 16), sourceB, GraphicsUnit.Pixel);
							g.DrawImage(img, new Rectangle(x + (16 * 2), y, 16, 16), sourceC, GraphicsUnit.Pixel);
							g.DrawImage(img, new Rectangle(x + (16 * 3), y, 16, 16), sourceD, GraphicsUnit.Pixel);
						}
					}
				}

				tiles.Save(@"C:\working\dq3\overworld\dq3-tiles.png", ImageFormat.Png);
			}
		}

		// Output is ram $7f0000 - $7f1fff ($2000 bytes)
		// A => exitCounter
		// X => destinationAddress
		public static RamWordArray GetLayout() { //(int A, int X) {
												 // I think this is map layout (when to read a block of sea or something or when to read map data)
												 // see debug log: trying to find water 02
												 // code start: c04923
												 // code end: c04a43
												 // code never hits: c049b3 (does hit when loading cave map)
												 // $ed8a00 - $ed9be2
			var ram = new Ram();
			var layoutSource = Rom.ByteArray(0xed8a00);
			var output = ram.WordArray(0x7f0000);
			var work = ram.ByteArray(0x7ff3c6);

			/*
			 Output at $7f0000 should be $2000 bytes
			 Each word represents 4 columns by 4 rows of tilemap data  --- this might not be right
			 So 256 x 256 is reduced to pointers for 64 x 64
			 This allows for easy repeat/reuse of blocks of sea etc.
			 */

			// $18-$19, initialized to 0
			// TODO: should resplit these
			// $19 is $00 (-1 so $ff, read $18 from source)
			//		$ff, $7f, $3f, $1f, $0f, $07, $03, $01, $00 = restart
			//		basically a counter to 8
			// regex @"ROR \$18 \[000018\] = \$.{62}C" shows all but first >> puts a 1 in $18 high bit (carry set because bit from $19 is 1)
			ushort command = 0;

			// $1c -- 16bit -- comes from outside (A)
			// each decrement is 2 bytes of output, so $2000 bytes
			// TODO: could be replaced with destination address offset? might be more than one setup (dest might not be zero in caves or something)
			int exitCounter = 0x1000;

			// $1e -- 16bit
			// masked with 0x03ff so $400 bytes of ring memory
			int workDestination = 0x03BE;

			// $1a -- 16bit
			// masked with 0x03ff so $400 bytes of ring memory
			int workSource;

			// $20 -- 16bit -- comes from outside (X)
			// $0000 to $1ffe by +=2 ($2000 bytes)
			int destinationAddress = 0;

			// $22 -- 8bit
			// overflows from $ff to $00
			// is really just high byte/low byte indicator
			byte columnIndex = 0;

			// don't need initial values
			// don't need to be differentiated, could just write byte by byte
			// $24
			byte lowValue = 0;
			// $25
			byte highValue = 0;

			// Y -- 16bit
			var sourceAddress = 0;

			while (true) {
				// decrement $19 basically, and rotate a one into the high bit of $18 (except first time in which $18 is thrown out anyways)
				command = (ushort)(command >> 1);

				// is $19 zero (happens at beginning of every cycle of 8)
				// Gets new command byte $18 (each bit says how next bytes are interpreted)
				if ((command & 0x0100) == 0) {
					//set $19 to $ff and read $18 from source
					command = (ushort)(((command - 0x0100) & 0xff00) + layoutSource[sourceAddress]);
					sourceAddress++;
				}

				// is $18 odd (bit 0 == 1)   --- command bit is 1
				if ((command & 0x0001) != 0) {
					// loads one byte directly

					if ((columnIndex & 0x01) == 0) {
						// load low byte direct

						lowValue = layoutSource[sourceAddress];
						work[workDestination] = layoutSource[sourceAddress];

						sourceAddress++;
						workDestination++;
						workDestination &= 0x03ff;

						columnIndex++;
					} else {
						// load high byte direct and write output

						highValue = layoutSource[sourceAddress];
						work[workDestination] = layoutSource[sourceAddress];

						sourceAddress++;
						workDestination++;
						workDestination &= 0x03ff;
						columnIndex++;

						output[destinationAddress] = (ushort)(lowValue + (highValue << 8));
						destinationAddress += 2;

						exitCounter--;
						if (exitCounter == 0) {
							return output;  // EXIT
						}
					}
				} else {
					// is $18 even (bit 0 == 0)   --- command bit is 0
					// copy part of work into output
					// next two source bytes are source address and number of bytes to copy
					// destination work buffer continues to fill

					// 10 bit number: low byte = ls[sa], high byte lower 2 bits = ls[sa+1] highest 2 bits
					// source address is absolute in buffer, not relative
					workSource = layoutSource[sourceAddress] + ((layoutSource[sourceAddress + 1] << 2) & 0x0300);

					// counter is lower 6 bits + 3
					// smallest value is (0 + 3) copy 3 bytes
					int counter = (layoutSource[sourceAddress + 1] & 0x3f) + 3;
					sourceAddress += 2;

					while (counter > 0) {
						counter--;

						if ((columnIndex & 0x01) == 0) {
							// copy low byte from work buffer

							lowValue = work[workSource];
							work[workDestination] = lowValue;

							workSource++;
							workSource &= 0x03ff;
							workDestination++;
							workDestination &= 0x03ff;

							columnIndex++;
						} else {
							// copy high byte from work buffer and write to output

							highValue = work[workSource];
							work[workDestination] = highValue;

							workSource++;
							workSource &= 0x03ff;
							workDestination++;
							workDestination &= 0x03ff;

							columnIndex++;
							var val = (ushort)(lowValue + (highValue << 8));
							output[destinationAddress] = val;
							destinationAddress += 2;

							exitCounter--;
							if (exitCounter == 0) {
								return output; // EXIT
							}
						}
					}
				}
			}
		}

		public static void TestGetLayout() {
			// Yes, this is terrible code, IDGAF
			var generated = GetLayout().Ram.ByteArray(0x7f0000);
			var expected =
				@"01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 02 00 03 00 04 00 05 00 06 00 07 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 08 00 09 00 0A 00 0B 00 0C 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 0D 00 0E 00 0F 00 10 00 11 00 12 00 13 00 14 00 15 00 01 00 16 00 17 00 01 00 01 00 01 00 01 00 01 00 18 00 19 00 1A 00 1B 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 1C 00 1D 00 1D 00 1D 00 1E 00 1F 00 20 00 21 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 22 00 23 00 24 00 25 00 26 00 27 00 28 00 29 00 2A 00 2B 00 2C 00 2D 00 2E 00 2F 00 30 00 31 00 32 00 33 00 34 00 35 00 36 00 37 00 38 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 39 00 3A 00 1D 00 1D 00 1D 00 1D 00 1D 00 1D 00 3B 00 3C 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 3D 00 3E 00 3F 00 40 00 41 00 42 00 43 00 44 00 45 00 46 00 47 00 48 00 49 00 4A 00 4B 00 4C 00 4D 00 4E 00 4F 00 50 00 51 00 52 00 53 00 54 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 55 00 1D 00 56 00 57 00 58 00 59 00 5A 00 5B 00 5C 00 5D 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 5E 00 5F 00 60 00 61 00 62 00 63 00 64 00 65 00 66 00 67 00 68 00 69 00 6A 00 6A 00 6B 00 6C 00 6D 00 6E 00 6F 00 70 00 71 00 72 00 73 00 74 00 75 00 38 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 76 00 77 00 78 00 79 00 7A 00 7B 00 7C 00 7D 00 7E 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 7F 00 80 00 01 00 81 00 82 00 63 00 01 00 83 00 84 00 85 00 86 00 87 00 88 00 89 00 8A 00 8B 00 8C 00 8D 00 8E 00 8F 00 90 00 91 00 92 00 93 00 94 00 95 00 96 00 97 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 98 00 99 00 63 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 9A 00 9B 00 01 00 01 00 9C 00 9D 00 9E 00 9F 00 A0 00 A1 00 A2 00 A3 00 A4 00 A5 00 A6 00 A7 00 A8 00 A9 00 AA 00 AB 00 AC 00 AD 00 AE 00 AF 00 B0 00 B1 00 B2 00 B3 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 B4 00 B5 00 B6 00 B7 00 01 00 01 00 B8 00 B9 00 BA 00 BB 00 BC 00 BD 00 BE 00 BF 00 C0 00 C1 00 C2 00 C3 00 C4 00 C5 00 C6 00 C7 00 C8 00 C9 00 CA 00 CB 00 6A 00 CC 00 CD 00 CE 00 CF 00 D0 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 D1 00 D2 00 D3 00 D4 00 D5 00 D6 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 D7 00 D8 00 D9 00 DA 00 01 00 01 00 01 00 DB 00 DC 00 DD 00 DE 00 DF 00 E0 00 E1 00 E2 00 E3 00 E4 00 E5 00 E6 00 E7 00 E8 00 E9 00 EA 00 EB 00 EC 00 ED 00 EE 00 EF 00 F0 00 F1 00 F2 00 F3 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 F4 00 F5 00 F6 00 F7 00 F8 00 F9 00 FA 00 FB 00 FC 00 FD 00 FE 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 FF 00 7D 00 00 01 01 00 0D 00 01 01 02 01 03 01 04 01 05 01 06 01 07 01 08 01 09 01 0A 01 0B 01 0C 01 0D 01 0E 01 0F 01 6A 00 10 01 11 01 12 01 13 01 14 01 15 01 16 01 17 01 18 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 19 01 1A 01 1B 01 1C 01 1D 01 1E 01 01 00 1F 01 20 01 21 01 22 01 23 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 24 01 93 00 25 01 26 01 27 01 28 01 29 01 2A 01 2B 01 2C 01 2D 01 2E 01 2F 01 30 01 31 01 3F 00 6A 00 32 01 33 01 34 01 35 01 36 01 37 01 38 01 39 01 3A 01 3B 01 D6 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 3C 01 6A 00 3D 01 3E 01 3F 01 40 01 41 01 42 01 43 01 44 01 45 01 46 01 47 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 48 01 49 01 4A 01 4B 01 4C 01 4D 01 4E 01 4F 01 50 01 51 01 52 01 53 01 54 01 55 01 56 01 57 01 58 01 59 01 5A 01 5B 01 5C 01 5D 01 5E 01 5F 01 6A 00 6A 00 6A 00 60 01 61 01 01 00 01 00 01 00 01 00 01 00 62 01 01 00 01 00 01 00 01 00 01 00 63 01 64 01 65 01 66 01 67 01 68 01 69 01 6A 01 6B 01 6C 01 6D 01 6E 01 6F 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 22 00 70 01 71 01 72 01 73 01 74 01 75 01 76 01 77 01 78 01 79 01 7A 01 7B 01 7C 01 7D 01 7E 01 7F 01 80 01 81 01 01 00 82 01 83 01 84 01 85 01 6A 00 6A 00 6A 00 6A 00 6A 00 86 01 87 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 88 01 89 01 8A 01 8B 01 8C 01 8D 01 8E 01 8F 01 90 01 91 01 92 01 93 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 94 01 95 01 96 01 97 01 98 01 99 01 9A 01 93 00 9B 01 9C 01 9D 01 9E 01 9F 01 01 00 A0 01 A1 01 A2 01 A3 01 A4 01 A5 01 A6 01 6A 00 6A 00 6A 00 6A 00 A7 01 A8 01 6A 00 6A 00 6A 00 A9 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 AA 01 AB 01 93 00 AC 01 AD 01 AE 01 AF 01 B0 01 B1 01 B2 01 B3 01 B4 01 B5 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 B6 01 B7 01 B8 01 B9 01 BA 01 BB 01 BC 01 BD 01 BE 01 BF 01 C0 01 C1 01 01 00 01 00 C2 01 C3 01 C4 01 C5 01 01 00 C6 01 C7 01 6A 00 6A 00 6A 00 6A 00 6A 00 6A 00 6A 00 6A 00 C8 01 C9 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 CA 01 CB 01 CC 01 CD 01 CE 01 CF 01 D0 01 D1 01 D2 01 D3 01 D4 01 D5 01 D6 01 D7 01 D8 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 D9 01 DA 01 DB 01 DC 01 DD 01 DE 01 DF 01 E0 01 6A 00 E1 01 E2 01 E3 01 E4 01 01 00 E5 01 A4 01 E6 01 E7 01 E8 01 01 00 E9 01 6A 00 6A 00 EA 01 6A 00 6A 00 6A 00 6A 00 EA 01 EB 01 EC 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 51 00 ED 01 6A 00 EE 01 6A 00 EF 01 F0 01 F1 01 F2 01 F3 01 F4 01 F5 01 F6 01 F7 01 F8 01 F9 01 FA 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 FB 01 FC 01 FD 01 FE 01 FF 01 00 02 01 02 02 02 03 02 04 02 05 02 06 02 07 02 08 02 09 02 51 00 0A 02 0B 02 0C 02 01 00 B8 00 0D 02 6A 00 0E 02 6A 00 6A 00 6A 00 6A 00 0E 02 6A 00 0F 02 10 02 01 00 01 00 01 00 01 00 01 00 01 00 11 02 12 02 13 02 14 02 15 02 16 02 17 02 18 02 19 02 1A 02 1B 02 1C 02 1D 02 1E 02 1F 02 20 02 21 02 22 02 01 00 01 00 01 00 01 00 01 00 01 00 0D 00 23 02 24 02 25 02 26 02 27 02 28 02 63 00 29 02 2A 02 2B 02 2C 02 2D 02 2E 02 2F 02 30 02 31 02 32 02 33 02 93 00 34 02 35 02 01 00 36 02 6A 00 6A 00 6A 00 6A 00 6A 00 6A 00 6A 00 6A 00 37 02 38 02 39 02 3A 02 01 00 01 00 01 00 08 00 3B 02 3C 02 3D 02 3E 02 3F 02 40 02 41 02 42 02 43 02 44 02 45 02 46 02 47 02 48 02 49 02 4A 02 4B 02 4C 02 4D 02 01 00 01 00 01 00 01 00 01 00 4E 02 4F 02 50 02 51 02 52 02 53 02 01 00 01 00 01 00 54 02 55 02 47 01 56 02 57 02 58 02 59 02 5A 02 5B 02 5C 02 5D 02 93 00 5E 02 5F 02 60 02 6A 00 6A 00 6A 00 A7 01 A8 01 6A 00 6A 00 6A 00 61 02 62 02 63 02 64 02 01 00 01 00 65 02 66 02 1D 00 67 02 68 02 69 02 6A 02 6B 02 6C 02 6D 02 6E 02 6F 02 70 02 71 02 72 02 73 02 74 02 75 02 76 02 77 02 78 02 01 00 01 00 01 00 01 00 01 00 79 02 7A 02 7B 02 7C 02 7D 02 7E 02 01 00 01 00 0D 00 7F 02 80 02 81 02 01 00 82 02 83 02 84 02 85 02 86 02 87 02 88 02 89 02 8A 02 8B 02 8C 02 8D 02 6A 00 6A 00 6A 00 6A 00 6A 00 6A 00 8E 02 8F 02 6A 00 90 02 91 02 92 02 93 02 94 02 95 02 1D 00 96 02 97 02 98 02 99 02 9A 02 9B 02 9C 02 9D 02 9E 02 9F 02 A0 02 A1 02 A2 02 A3 02 A4 02 A5 02 A6 02 A7 02 01 00 01 00 01 00 01 00 01 00 A8 02 A9 02 AA 02 AB 02 AC 02 63 00 01 00 01 00 AD 02 AE 02 AF 02 B0 02 01 00 B1 02 B2 02 B3 02 B4 02 B5 02 B6 02 B7 02 B8 02 B9 02 BA 02 BB 02 BC 02 BD 02 6A 00 6A 00 6A 00 6A 00 6A 00 BE 02 B0 01 BF 02 C0 02 C1 02 C2 02 C3 02 C4 02 C5 02 C6 02 C7 02 C8 02 C9 02 FF 00 CA 02 CB 02 CC 02 CD 02 CE 02 CF 02 D0 02 D1 02 D2 02 D3 02 D4 02 D5 02 D6 02 D7 02 01 00 01 00 01 00 01 00 01 00 D8 02 D9 02 DA 02 DB 02 DC 02 01 00 01 00 01 00 01 00 01 00 01 00 DD 02 DE 02 01 00 FF 00 DF 02 E0 02 6A 00 6A 00 E1 02 E2 02 E3 02 E4 02 E5 02 E6 02 E7 02 E8 02 6A 00 6A 00 6A 00 6A 00 6A 00 6A 00 E9 02 EA 02 EB 02 EC 02 ED 02 01 00 01 00 01 00 01 00 01 00 01 00 01 00 EE 02 EF 02 F0 02 F1 02 F2 02 F3 02 F4 02 F5 02 F6 02 F7 02 F8 02 F9 02 FA 02 01 00 01 00 01 00 01 00 01 00 01 00 01 00 FB 02 7D 00 FC 02 FD 02 01 00 01 00 01 00 01 00 01 00 FE 02 FF 02 00 03 01 00 01 00 01 03 02 03 03 03 6A 00 E1 01 04 03 05 03 06 03 07 03 08 03 09 03 0A 03 0B 03 0C 03 0D 03 6A 00 6A 00 0E 03 0F 03 10 03 26 02 11 03 12 03 13 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 14 03 15 03 16 03 17 03 18 03 19 03 1A 03 1B 03 1C 03 1D 03 1E 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 1F 03 20 03 21 03 22 03 01 00 01 00 01 00 23 03 24 03 01 00 01 00 01 00 25 03 26 03 27 03 28 03 29 03 2A 03 2B 03 2C 03 2D 03 2E 03 2F 03 30 03 31 03 32 03 33 03 34 03 35 03 36 03 37 03 38 03 93 00 39 03 3A 03 3B 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 3C 03 3D 03 3E 03 3F 03 40 03 41 03 42 03 43 03 44 03 45 03 46 03 47 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 6A 01 48 03 49 03 4A 03 4B 03 4C 03 4D 03 4E 03 4F 03 47 01 01 00 01 00 0D 00 50 03 51 03 93 00 52 03 53 03 54 03 55 03 56 03 57 03 58 03 59 03 30 03 5A 03 5B 03 5C 03 5D 03 5E 03 5F 03 60 03 61 03 62 03 63 03 64 03 65 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 56 02 66 03 67 03 68 03 69 03 6A 03 6B 03 6C 03 2C 02 6D 03 A4 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 6E 03 6F 03 70 03 71 03 72 03 72 03 72 03 73 03 74 03 75 03 76 03 77 03 78 03 79 03 7A 03 93 00 7B 03 7C 03 7D 03 7E 03 7F 03 80 03 81 03 82 03 30 03 83 03 84 03 85 03 86 03 87 03 88 03 89 03 8A 03 8B 03 8C 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 8D 03 8E 03 8F 03 90 03 91 03 47 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 92 03 93 03 94 03 72 03 95 03 72 03 72 03 72 03 72 03 96 03 97 03 98 03 99 03 9A 03 9B 03 9C 03 93 00 9D 03 9E 03 9F 03 A0 03 A1 03 A2 03 A3 03 A4 03 A5 03 A6 03 A7 03 A8 03 6A 00 A9 03 89 03 AA 03 AB 03 AC 03 AD 03 AE 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 AF 03 B0 03 B1 03 B2 03 B3 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 B4 03 B5 03 B6 03 B7 03 72 03 72 03 72 03 72 03 72 03 72 03 72 03 B8 03 B9 03 BA 03 BB 03 6A 00 3F 00 BC 03 BD 03 BE 03 BF 03 C0 03 C1 03 C2 03 C3 03 C4 03 C5 03 C6 03 C7 03 C8 03 6A 00 C9 03 CA 03 CB 03 CC 03 CD 03 CE 03 CF 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 D0 03 D1 03 D2 03 D3 03 D4 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 0D 00 D5 03 6A 00 D6 03 D7 03 72 03 72 03 72 03 72 03 72 03 72 03 72 03 72 03 72 03 D8 03 D9 03 DA 03 DB 03 DC 03 DD 03 DE 03 DF 03 E0 03 E1 03 E2 03 E3 03 30 03 E4 03 E5 03 E6 03 E7 03 E8 03 E9 03 EA 03 EB 03 EC 03 ED 03 EE 03 EF 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 F0 03 F1 03 F2 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 F3 03 F4 03 F5 03 F6 03 F7 03 F8 03 72 03 72 03 72 03 72 03 72 03 72 03 72 03 F9 03 FA 03 FB 03 FC 03 FD 03 FE 03 FF 03 00 04 01 04 02 04 89 03 89 03 03 04 30 03 04 04 05 04 06 04 07 04 08 04 09 04 0A 04 0B 04 0C 04 0D 04 0E 04 0F 04 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 6A 01 10 04 11 04 12 04 13 04 14 04 15 04 50 00 01 00 01 00 01 00 01 00 01 00 01 00 16 04 17 04 18 04 19 04 1A 04 1B 04 1C 04 1D 04 72 03 72 03 72 03 72 03 72 03 72 03 1E 04 1F 04 20 04 21 04 22 04 23 04 24 04 25 04 26 04 27 04 89 03 28 04 2F 03 29 04 2A 04 2B 04 2C 04 2D 04 2E 04 2F 04 30 04 31 04 32 04 33 04 34 04 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 35 04 36 04 37 04 38 04 39 04 3A 04 3B 04 3C 04 3D 04 3E 04 01 00 01 00 01 00 01 00 3F 04 40 04 41 04 42 04 43 04 44 04 45 04 46 04 47 04 72 03 72 03 72 03 72 03 48 04 49 04 4A 04 4B 04 4C 04 4D 04 4E 04 4F 04 50 04 51 04 52 04 53 04 54 04 55 04 56 04 57 04 58 04 59 04 5A 04 5B 04 5C 04 5D 04 5E 04 5F 04 60 04 61 04 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 62 04 63 04 64 04 65 04 66 04 67 04 68 04 69 04 6A 04 6B 04 6C 04 6D 04 01 00 01 00 01 00 6E 04 6F 04 6C 00 70 04 71 04 72 04 73 04 74 04 75 04 76 04 77 04 72 03 78 04 79 04 7A 04 7B 04 7C 04 7D 04 7E 04 7F 04 80 04 81 04 82 04 83 04 84 04 85 04 86 04 87 04 88 04 89 04 8A 04 8B 04 8C 04 8D 04 8E 04 8F 04 90 04 91 04 92 04 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 93 04 94 04 95 04 96 04 97 04 98 04 99 04 9A 04 9B 04 9C 04 27 03 9D 04 9E 04 01 00 01 00 01 00 01 00 9F 04 A0 04 6A 00 A1 04 8C 04 A2 04 A3 04 A4 04 A5 04 A6 04 A7 04 A8 04 A9 04 AA 04 AB 04 AC 04 AD 04 AE 04 AF 04 B0 04 B1 04 B2 04 B3 04 B4 04 B5 04 B6 04 B7 04 B8 04 B9 04 BA 04 BB 04 BC 04 BD 04 BE 04 BF 04 C0 04 C1 04 C2 04 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 C3 04 C4 04 C5 04 C6 04 C7 04 C8 04 C9 04 CA 04 CB 04 CC 04 CD 04 93 00 CE 04 CF 04 01 00 01 00 01 00 01 00 01 00 D0 04 D1 04 D2 04 D3 04 D4 04 78 01 D5 04 D6 04 D7 04 D8 04 D9 04 DA 04 DB 04 DC 04 DD 04 DE 04 DF 04 E0 04 E1 04 93 00 93 00 E2 04 4B 01 CE 03 E3 04 E4 04 E5 04 E6 04 6A 00 E7 04 E8 04 E9 04 EA 04 EB 04 EC 04 ED 04 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 EE 04 EF 04 F0 04 F1 04 F2 04 F3 04 F4 04 F5 04 F6 04 6A 00 F7 04 F8 04 F9 04 FA 04 01 00 01 00 01 00 01 00 01 00 01 00 AD 02 FB 04 FC 04 FD 04 FE 04 FF 04 93 00 00 05 01 05 02 05 03 05 04 05 05 05 06 05 07 05 08 05 09 05 0A 05 0B 05 0C 05 0D 05 0E 05 0F 05 10 05 11 03 11 05 12 05 13 05 14 05 93 00 15 05 16 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 17 05 18 05 19 05 1A 05 1B 05 1C 05 1D 05 1E 05 1F 05 20 05 21 05 22 05 23 05 24 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 25 05 26 05 27 05 28 05 29 05 2A 05 2B 05 2C 05 2D 05 2E 05 2F 05 30 05 31 05 32 05 33 05 72 03 34 05 35 05 36 05 37 05 38 05 39 05 3A 05 3B 05 3C 05 3D 05 3E 05 93 00 3F 05 40 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 41 05 42 05 43 05 44 05 45 05 46 05 47 05 48 05 49 05 4A 05 4B 05 4C 05 4D 05 4E 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 4F 05 50 05 51 05 52 05 53 05 54 05 55 05 56 05 57 05 58 05 59 05 78 01 5A 05 5B 05 5C 05 5D 05 5E 05 01 00 01 00 5F 05 60 05 61 05 EF 04 62 05 63 05 64 05 65 05 66 05 67 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 EE 02 68 05 69 05 6A 05 6B 05 78 01 6C 05 6D 05 6E 05 6F 05 70 05 71 05 72 05 73 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 74 05 75 05 76 05 77 05 78 05 79 05 7A 05 7B 05 7C 05 7D 05 7E 05 7F 05 80 05 81 05 01 00 01 00 01 00 01 00 01 00 01 00 82 05 83 05 84 05 9F 01 01 00 EE 02 85 05 86 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 87 05 88 05 89 05 8A 05 8B 05 8C 05 8D 05 8E 05 8F 05 90 05 91 05 92 05 93 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 94 05 95 05 96 05 97 05 98 05 99 05 9A 05 9B 05 9C 05 9D 05 30 03 9E 05 9F 05 01 00 01 00 01 00 01 00 01 00 01 00 FF 00 A0 05 A1 05 47 01 01 00 01 00 A2 05 A3 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 A4 05 A5 05 A6 05 A7 05 A8 05 A9 05 AA 05 AB 05 AC 05 AD 05 67 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 AE 05 AF 05 B0 05 B1 05 B2 05 B3 05 B4 05 B5 05 B6 05 B7 05 B8 05 B9 05 BA 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 BB 05 BC 05 01 00 01 00 FF 00 00 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 FF 00 BD 05 BE 05 BF 05 C0 05 C1 05 C2 05 3F 00 C3 05 C4 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 C5 05 C6 05 C7 05 C8 05 C9 05 CA 05 CB 05 CC 05 CD 05 CE 05 CF 05 93 00 D0 05 D1 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 D2 05 D3 05 D4 05 D5 05 13 05 D6 05 D7 05 D8 05 A4 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 D9 05 DA 05 89 03 DB 05 DC 05 DD 05 DE 05 DF 05 E0 05 E1 05 E2 05 E3 05 E4 05 63 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 FF 00 E5 05 E6 05 E7 05 E8 05 E9 05 EA 05 EB 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 EC 05 ED 05 EE 05 EF 05 F0 05 F1 05 F2 05 F3 05 F4 05 F5 05 F6 05 F7 05 F8 05 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 0D 00 03 00 F9 05 FA 05 FB 05 FC 05 FD 05 DE 02 01 00 01 00 01 00 01 00 01 00 01 00 01 00 FE 05 FF 05 00 06 01 06 02 06 03 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 04 06 05 06 06 06 07 06 08 06 09 06 0A 06 0B 06 0C 06 0D 06 0E 06 0F 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 10 06 11 06 12 06 93 00 13 06 14 06 15 06 16 06 17 06 18 06 01 00 01 00 01 00 01 00 01 00 01 00 19 06 1A 06 1B 06 1C 06 1D 06 63 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 1E 06 1F 06 20 06 21 06 22 06 23 06 24 06 0A 06 25 06 26 06 27 06 63 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 28 06 29 06 CE 03 2A 06 2B 06 2C 06 2D 06 2E 06 2F 06 6A 00 30 06 31 06 01 00 01 00 01 00 01 00 01 00 32 06 6A 00 6A 00 33 06 34 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 35 06 36 06 37 06 38 06 39 06 3A 06 3B 06 3C 06 3D 06 3E 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 3F 06 40 06 41 06 42 06 43 06 44 06 45 06 46 06 47 06 48 06 49 06 4A 06 01 00 01 00 01 00 01 00 01 00 4B 06 6A 00 6A 00 4C 06 63 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 FB 02 4D 06 4E 06 89 03 89 03 4F 06 50 06 51 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 A5 01 52 06 53 06 54 06 55 06 56 06 57 06 58 06 59 06 5A 06 5B 06 5C 06 5D 06 5E 06 01 00 01 00 01 00 01 00 5F 06 BA 04 60 06 61 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 FF 00 62 06 63 06 64 06 65 06 00 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 0D 00 66 06 67 06 68 06 69 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 6E 04 6A 06 6B 06 6C 06 6D 06 6E 06 6F 06 70 06 71 06 72 06 73 06 74 06 75 06 76 06 01 00 01 00 01 00 01 00 EE 02 77 06 78 06 79 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 7A 06 7B 06 7C 06 7D 06 7E 06 7F 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 80 06 81 06 82 06 02 03 83 06 84 06 85 06 86 06 87 06 88 06 89 06 8A 06 8B 06 01 00 01 00 01 00 01 00 01 00 8C 06 8D 06 8E 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 8F 06 90 06 91 06 92 06 93 06 94 06 95 06 96 06 01 00 01 00 01 00 01 00 01 00 01 00 97 06 98 06 99 06 9A 06 9B 06 9C 06 9D 06 9E 06 9F 06 A0 06 A1 06 A2 06 A3 06 01 00 01 00 01 00 01 00 01 00 EE 02 A4 06 A5 06 A6 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 A7 06 A8 06 A9 06 AA 06 AB 06 AC 06 AD 06 AE 06 01 00 01 00 01 00 01 00 01 00 01 00 AF 06 B0 06 B1 06 B2 06 B3 06 B4 06 B5 06 B6 06 B7 06 B8 06 B9 06 BA 06 BB 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 FB 02 00 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 BC 06 BD 06 BE 06 BF 06 C0 06 C1 06 C2 06 C3 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 C4 06 C5 06 C6 06 C7 06 C8 06 C9 06 CA 06 CB 06 CC 06 7D 00 F2 03 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 CD 06 00 01 01 00 CE 06 CF 06 D0 06 D1 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 D2 06 D3 06 D4 06 D5 06 D6 06 D7 06 D8 06 00 01 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 17 00 0C 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 D9 06 DA 06 1D 00 DB 06 DC 06 DD 06 01 00 DE 06 17 00 21 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 DF 06 E0 06 1D 00 1D 00 1D 00 E1 06 E2 06 3A 00 1D 00 E3 06 E4 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 E5 06 E6 06 E7 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 E8 06 E0 06 1D 00 1D 00 E9 06 1D 00 1D 00 1D 00 1D 00 EA 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 EE 02 EB 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 EC 06 C6 02 ED 06 EE 06 EE 06 EF 06 F0 06 F1 06 F2 06 F3 06 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00 01 00"
					.Split(' ')
					.Select(x => Convert.ToByte(x, 16));
			var i = 0;
			var good = 0;
			var bad = 0;
			var outputTemp = new List<string>();
			var expectedTemp = new List<string>();
			var outputLines = new List<string>();
			var expectedLines = new List<string>();
			foreach (var b in expected) {
				outputTemp.Add(generated[i].ToString("x2"));
				expectedTemp.Add(b.ToString("x2"));

				if (outputTemp.Count == 16) {
					outputLines.Add(string.Join(" ", outputTemp));
					expectedLines.Add(string.Join(" ", expectedTemp));

					outputTemp = new List<string>();
					expectedTemp = new List<string>();
				}

				if (generated[i] == b) {
					good++;
				} else {
					bad++;
				}
				i++;
			}

			if (outputTemp.Count > 0) {
				outputLines.Add(string.Join(" ", outputTemp));
				expectedLines.Add(string.Join(" ", expectedTemp));
			}

			File.WriteAllLines(@"c:\working\get-layout-test-output.txt", outputLines);
			File.WriteAllLines(@"c:\working\get-layout-test-expected.txt", expectedLines);

			Console.WriteLine($"Good: {good}");
			Console.WriteLine($"Bad: {bad}");
			Console.WriteLine($"Total: 0x{(good + bad).ToString("x4")}/0x{i.ToString("x4")}");
			Console.ReadKey();
		}

		public static Ram GetLayoutAnnotated() {
			var ram = new Ram();
			// I think this is map layout (when to read a block of sea or something or when to read map data)
			// see debug log: trying to find water 02
			// code start: c04923
			// code end: c04a43
			// code never hits: c049b3 (does hit when loading cave map)
			// $ed8a00 - $ed9be2
			var layoutSource = Rom.ByteArray(0xed8a00);
			var output = ram.WordArray(0x7f0000);
			var outputf3c6 = ram.ByteArray(0x7ff3c6);

			//c04923 php
			//c04924 %setAXYto16bit()
			//c04926 pha
			//c04927 phx
			//c04928 phy
			//c04929 sta $1c
			//c0492b stx $20
			//c0492d stz $18
			//c0492f stz $1a
			//c04931 lda #$03be
			//c04934 sta $1e
			//c04936 stz $22
			//c04938 ldx #$0000
			//c0493b stz $f3c6,x
			//c0493e inx
			//c0493f inx
			//c04940 cpx #$0400
			//c04943 bcc $493b
			//c04945 ldy #$0000
			//c04948 tya
			//c04949 %setAto8bit()


			// c04929
			ushort x18x19 = 0;
			int x1c = 0x1000;   // comes from outside (A)
			int x1e = 0x03BE;
			byte x1a = 0;
			byte x1b = 0;
			int x20 = 0;    // comes from outside (X)
			byte x22 = 0;

			// don't have initial values?
			byte x24 = 0;
			byte x25 = 0;

			// $c04945
			var y = 0;

			// A => 8bit
			// .Branch_c0494b -- top
			while (true) {
				//c0494b lsr $19
				//c0494d ror $18
				x18x19 = (ushort)(x18x19 >> 1);

				//c0494f lda $19
				//c04951 lsr
				//c04952 bcs .Branch_c0495b
				if ((x18x19 & 0x0100) == 0) {
					//c04954 lda [$00],y
					//c04956 iny
					//c04957 sta $18
					//c04959 dec $19
					x18x19 = (ushort)(((x18x19 - 0x0100) & 0xff00) + layoutSource[y]);
					y++;
				}

				//.Branch_c0495b
				//c0495b lda $18
				//c0495d lsr
				//c0495e bcc .Branch_c049b4
				if ((x18x19 & 0x0001) != 0) {

					//c04960 lda $22
					//c04962 lsr
					//c04963 bcs .Branch_c0497e
					if ((x22 & 0x01) == 0) {

						//c04965 lda [$00],y
						//c04967 iny
						//c04968 sta $24
						//c0496a ldx $1e
						//c0496c sta $f3c6,x
						x24 = layoutSource[y];
						outputf3c6[x1e] = layoutSource[y];
						y++;

						//c0496f inc $1e
						//c04971 bne .Branch_c0497a
						//c04973 lda $1f
						//c04975 inc
						//c04976 and #$03
						//c04978 sta $1f
						//.Branch_c0497a
						x1e++;
						x1e &= 0x03ff;

						//c0497a inc $22
						x22++;
						//c0497c bra .Branch_c0494b	// back to top
						continue;
					}
					//.Branch_c0497e

					//c0497e lda [$00],y
					//c04980 iny
					//c04981 sta $25
					//c04983 ldx $1e
					//c04985 sta $f3c6,x
					x25 = layoutSource[y];
					outputf3c6[x1e] = layoutSource[y];
					y++;

					//c04988 inc $1e
					//c0498a bne .Branch_c04993
					//c0498c lda $1f
					//c0498e inc
					//c0498f and #$03
					//c04991 sta $1f
					//.Branch_c04993
					//c04993 inc $22
					x1e++;
					x1e &= 0x03ff;
					x22++;

					//c04995 rep #$20
					// ---------------------------------A => 16bit
					//c04997 ldx $20
					//c04999 lda $24
					//c0499b sta $0000,x
					//c0499e inx
					//c0499f inx
					//c049a0 stx $20
					output[x20] = (ushort)(x24 + (x25 << 8));
					x20 += 2;

					//c049a2 dec $1c
					x1c--;
					//c049a4 beq .Branch_c049ad
					if (x1c != 0) {
						//c049a6 lda #$0000
						//c049a9 sep #$20
						// ---------------------------------A => 8bit
						//c049ab bra .Branch_c0494b	// back to top
						continue;
					} else {
						//.Branch_c049ad
						//c049ad %setAXYto16bit()
						//c049af ply
						//c049b0 plx
						//c049b1 pla
						//c049b2 plp
						//c049b3 rtl
						return ram;  // EXIT
					}
				}

				//.Branch_c049b4			// carry will be clear
				//c049b4 lda [$00],y
				//c049b6 iny
				//c049b7 sta $1a
				x1a = layoutSource[y];
				y++;

				//c049b9 lda [$00],y
				//c049bb rol
				//c049bc rol
				//c049bd rol
				//c049be and #$03
				//c049c0 sta $1b
				x1b = (byte)((layoutSource[y] >> 6) & 0x03);

				//c049c2 lda [$00],y
				//c049c4 iny
				//c049c5 and #$3f
				//c049c7 clc
				//c049c8 adc #$03
				//c049ca phy
				//c049cb tay
				byte counter = (byte)((layoutSource[y] & 0x3f) + 3);
				y++;

				//.Branch_c049cc
				while (true) {
					//c049cc dey
					counter--;
					//c049cd bmi .Branch_c04a3a
					if (SNES.IsNegative16(counter)) {
						break;
					}

					//c049cf lda $22
					//c049d1 lsr
					//c049d2 bcs .Branch_c049fc
					if ((x22 & 0x01) == 0) {

						//c049d4 ldx $1a
						//c049d6 lda $f3c6,x
						//c049d9 sta $24
						//c049db inc $1a
						x24 = outputf3c6[x1a];  // TODO: Wrong, needs to be 16bit x1a
						x1a++;

						//c049dd bne .Branch_c049e8
						if (x1a == 0) {
							//c049df lda $1b
							//c049e1 inc
							//c049e2 and #$03
							//c049e4 sta $1b
							x1b++;
							x1b &= 0x03;

							//c049e6 lda $24
						}
						//.Branch_c049e8

						//c049e8 ldx $1e
						//c049ea sta $f3c6,x
						outputf3c6[x1e] = x24;

						//c049ed inc $1e
						//c049ef bne .Branch_c049f8
						//c049f1 lda $1f
						//c049f3 inc
						//c049f4 and #$03
						//c049f6 sta $1f
						//.Branch_c049f8
						x1e++;
						x1e &= 0x03ff;

						//c049f8 inc $22
						x22++;
						//c049fa bra .Branch_c049cc
					} else {

						//.Branch_c049fc
						//c049fc ldx $1a
						//c049fe lda $f3c6,x
						//c04a01 sta $25
						x25 = outputf3c6[x1a];  // TODO: Wrong, needs to be 16bit x1a

						//c04a03 inc $1a
						x1a++;
						//c04a05 bne .Branch_c04a10

						if (x1a == 0) {
							//c04a07 lda $1b
							//c04a09 inc
							//c04a0a and #$03
							//c04a0c sta $1b
							x1b++;
							x1b &= 0x03;

							//c04a0e lda $25
						}
						//.Branch_c04a10

						//c04a10 ldx $1e
						//c04a12 sta $f3c6,x
						outputf3c6[x1e] = x25;

						//c04a15 inc $1e
						//c04a17 bne .Branch_c04a20
						//c04a19 lda $1f
						//c04a1b inc
						//c04a1c and #$03
						//c04a1e sta $1f
						//.Branch_c04a20
						x1e++;
						x1e &= 0x03ff;

						//c04a20 inc $22
						x22++;

						//c04a22 rep #$20
						//---------------------------------- A=> 16bit
						//c04a24 ldx $20
						//c04a26 lda $24
						//c04a28 sta $0000,x
						//c04a2b inx
						//c04a2c inx
						//c04a2d stx $20
						output[x20] = (ushort)(x24 + (x25 << 8));
						x20 += 2;

						//c04a2f dec $1c
						x1c--;
						//c04a31 beq .Branch_c04a3e
						if (x1c == 0) {
							return ram; // EXIT
						}


						//c04a33 lda #$0000
						//c04a36 sep #$20
						// ---------------------------------A => 8bit
						//c04a38 bra .Branch_c049cc
					}
				}

				//.Branch_c04a3a
				//c04a3a ply
				//c04a3b jmp Jump_c0494b	// back to top

			} // end of while

			//.Branch_c04a3e	-- EXIT
			//c04a3e ply
			//c04a3f ply
			//c04a40 plx
			//c04a41 pla
			//c04a42 plp
			//c04a43 rtl
		}
	}
}
