using logsmall.Common;
using logsmall.Overworld;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3 {
	public static class OverworldMap2 {
		public static byte[,] GetTilemapData() {
			var layout = GetLayout();

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

			// fullmap[y, x]
			var fullmap = new byte[0x100, 0x100];
			for (int i = 0; i < 0x1000; i++) {
				// each 4 by 4 chunk
				var sourceIndex = layout[i * 2];
				var xStart = i % 64 * 4;
				var yStart = i / 64 * 4;

				for (int k = 0; k < 4; k++) {
					fullmap[yStart + 0, xStart + k] = tilemaps[0, k][sourceIndex];
					fullmap[yStart + 1, xStart + k] = tilemaps[1, k][sourceIndex];
					fullmap[yStart + 2, xStart + k] = tilemaps[2, k][sourceIndex];
					fullmap[yStart + 3, xStart + k] = tilemaps[3, k][sourceIndex];
				}
			}

			return fullmap;
		}

		public static void GetMapImage() {
			var fullmap = GetTilemapData();

			var sources =
				Enumerable.Range(0, 256)
					.ToDictionary(
						x => x,
						x => new Rectangle(x % 16 * 16, x / 16 * 16, 16, 16)
					);

			using var tiles = Image.FromFile(@"C:\working\dq3\overworld\dq3-tiles.png");
			using var img = new Bitmap(4096, 4096);
			using (var g = Graphics.FromImage(img)) {

				for (var row = 0; row < 256; row++) {
					for (var column = 0; column < 256; column++) {
						g.DrawImage(tiles, new Rectangle(column * 16, row * 16, 16, 16), sources[fullmap[row, column]], GraphicsUnit.Pixel);
					}
				}
			}

			img.Save(@"C:\working\dq3\overworld\overworld.png", ImageFormat.Png);
		}

		public static void MakeTilesImage() {
			var sourceA = new Rectangle(24, 15, 16, 16);
			var sourceB = new Rectangle(88, 15, 16, 16);
			var sourceC = new Rectangle(152, 15, 16, 16);
			var sourceD = new Rectangle(216, 15, 16, 16);

			using var tiles = new Bitmap(256, 256);
			using (var g = Graphics.FromImage(tiles)) {

				for (int i = 0; i < 64; i++) {
					var filename = $@"C:\working\dq3\overworld\screenshots\dq3-map test 1_{i:d3}.png";
					using var img = Image.FromFile(filename);

					var x = i % 4 * 64;
					var y = i / 4 * 16;

					g.DrawImage(img, new Rectangle(x + (16 * 0), y, 16, 16), sourceA, GraphicsUnit.Pixel);
					g.DrawImage(img, new Rectangle(x + (16 * 1), y, 16, 16), sourceB, GraphicsUnit.Pixel);
					g.DrawImage(img, new Rectangle(x + (16 * 2), y, 16, 16), sourceC, GraphicsUnit.Pixel);
					g.DrawImage(img, new Rectangle(x + (16 * 3), y, 16, 16), sourceD, GraphicsUnit.Pixel);
				}
			}

			tiles.Save(@"C:\working\dq3\overworld\dq3-tiles.png", ImageFormat.Png);
		}

		/*
		 Work area is ring buffer of $400 bytes, starts as zeros, destination starts at $3be
		 Reads from source data in order, writes to output and ring buffer in order
		 Command byte is 8 command bits:
			1 => read one byte (and write to output and ring buffer)
			0 => read two bytes
				10 bits are start address in ring buffer
				6 bits are number of bytes to copy (plus 3, so counter can be 3 to 66)
				copy bytes from ring buffer starting at source address,
				while writing to ring buffer destination address and output at same time)
		 So each command byte is followed by 8 to 16 bytes of data
			 
		*/
		// Output is ram $7f0000 - $7f1fff ($2000 bytes)
		// A => exitCounter
		// X => destinationAddress
		public static RamWordArray GetLayout() {
			// TODO: this can be greater simplified and won't require Ram class usage
			//(int A, int X) {
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
			int workDestination = 0x03be;

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
				TestData.OverworldLayout
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
				outputTemp.Add($"{generated[i]:x2}");
				expectedTemp.Add($"{b:x2}");

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
			Console.WriteLine($"Total: 0x{good + bad:x4}/0x{i:x4}");
			Console.ReadKey();
		}

		public static void TestTilemapToChunks() {
			var fullmap = GetTilemapData();
			var (Chunks, Map) = Chunk.TilemapToChunks(fullmap);
			var lines =
				Map
					.Chunk(8)
					.Select(x => string.Join(" ", x.Select(y => $"{y & 0xff:x2} {(y >> 8) & 0xff:x2}")));

			File.WriteAllLines(@"c:\working\get-layout-test-from-chunks.txt", lines);
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

		// Super fucking slow, but works
		public static void ProcessDQ4NesMap() {
			var inputFilename = @"C:\working\dq3\overworld\dq4-nes-map.png";
			var tilesFilename = @"C:\working\dq3\overworld\dq4-nes-tiles.png";
			var tilemapFilename = @"C:\working\dq3\overworld\dq4-nes-tilemap.txt";
			var tilemap = new List<byte>();
			var tiles = new List<TileEntry>();
			var tilesLookup = new Dictionary<string, TileEntry>();

			using (var inmap = new Bitmap(inputFilename)) {
				for (var row = 0; row < 0x100; row++) {
					for (var column = 0; column < 0x100; column++) {
						var rect = new Rectangle(column * 16, row * 16, 16, 16);
						var entry = new TileEntry {
							Tile = inmap.Clone(rect, PixelFormat.Format32bppArgb)
						};

						if (tilesLookup.ContainsKey(entry.Key)) {
							var index = tilesLookup[entry.Key].Index;
							tilemap.Add(index);
							entry.Tile.Dispose();
							Console.WriteLine($"{row}, {column} : FOUND : {index}");
						} else {
							entry.Index = (byte)tiles.Count;
							tiles.Add(entry);
							tilemap.Add(entry.Index);
							tilesLookup.Add(entry.Key, entry);
							Console.WriteLine($"{row}, {column} : new : {entry.Index}");
						}
					}
				}
			}

			if (tiles.Count > 256) {
				throw new Exception($"too many tiles {tiles.Count}/256");
			}

			using (var tilesImage = new Bitmap(256, 256)) {
				using var gTiles = Graphics.FromImage(tilesImage);
				for (int i = 0; i < tiles.Count; i++) {
					gTiles.DrawImage(tiles[i].Tile,
						new Rectangle(i % 16 * 16, i / 16 * 16, 16, 16),
						new Rectangle(0, 0, 16, 16),
						GraphicsUnit.Pixel);
				}

				tilesImage.Save(tilesFilename, ImageFormat.Png);
			}

			foreach (var tile in tiles) {
				tile.Tile.Dispose();
			}

			var lines =
				tilemap
					.Chunk(256)
					.Select(x => string.Join(" ", x.Select(y => $"{y:x2}")));

			File.WriteAllLines(tilemapFilename, lines);
		}

		public static string BitmapHash(Bitmap image) {
			var bytes = BitmapToByteArray(image);
			using var hasher = SHA1.Create();
			var hash = string.Join("", hasher.ComputeHash(bytes).Select(x => x.ToString("x2", CultureInfo.InvariantCulture)));
			return hash;
		}

		public static byte[] BitmapToByteArray(Bitmap image) {
			ArgumentNullException.ThrowIfNull(image, nameof(image));

			BitmapData bmpdata = null;

			try {
				bmpdata = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
				int size = bmpdata.Stride * image.Height;
				byte[] bytedata = new byte[size];
				IntPtr ptr = bmpdata.Scan0;

				Marshal.Copy(ptr, bytedata, 0, size);

				return bytedata;
			} finally {
				if (bmpdata != null) {
					image.UnlockBits(bmpdata);
				}
			}
		}

		class TileEntry {
			public byte Index { get; set; }
			public Bitmap Tile { get; set; }

			private string _key;
			public string Key {
				get {
					_key ??= BitmapHash(Tile);

					return _key;
				}
			}
		}
		public static readonly byte[] translate = new byte[] { 0x4b, 0x4b, 0x4b, 0x4b, 0x02, 0x4b, 0x4b, 0x4b, 0x4b, 0x4b, 0x4b, 0x4a, 0x4b, 0xc9, 0x64, 0x4b, 0x4b, 0x4b, 0x4b, 0x1c, 0x1d, 0x4b, 0xdd, 0x23, 0x24, 0x0d, 0x4b, 0x4b, 0xc9, 0x4b, 0x52, 0x61, 0x15, 0xff, 0x59, 0xdd, 0x9c, 0x4b, 0x0d, 0xdd, 0x0d, 0x0d, 0x4b, 0xdd, 0x4b, 0x4b, 0x4b, 0x9c, 0x4b, 0x4b, 0x4b, 0x4b, 0x4b, 0x02, 0x02, 0x4b, 0x4b, 0x9c, 0x4b, 0x4b, 0x4a, 0x4a, 0x02, 0x21, 0x4a, 0x4b, 0x4a, 0x4b, 0x4b, 0x4b, 0x4a, 0x21, 0x39, 0x4b, 0x04, 0x1a, 0x4b, 0x00, 0x00, 0x00 };

		public static void TranslateDQ4Map() {
			var inFilename = @"C:\working\dq3\overworld\dq4-nes-tilemap.txt";
			var outFilename = @"C:\working\dq3\overworld\dq4-snes-tilemap.txt";
			var data =
				File.ReadAllLines(inFilename)
					.Select(x =>
						string.Join(" ", x.Split(' ')
							.Select(y => $"{translate[Convert.ToByte(y, 16)]:x2}")));
			File.WriteAllLines(outFilename, data);
		}

		public static void DrawDQ4SnesMap() {
			var mapFilename = @"C:\working\dq3\overworld\dq4-snes-tilemap.txt";
			var outFilename = @"C:\working\dq3\overworld\dq4-snes-tilemap.png";
			var data =
				File.ReadAllLines(mapFilename)
					.Select(x => x.Split(' ').Select(y => Convert.ToByte(y, 16)).ToArray())
					.ToArray();

			var sources =
				Enumerable.Range(0, 256)
					.ToDictionary(
						x => x,
						x => new Rectangle(x % 16 * 16, x / 16 * 16, 16, 16)
					);

			using var tiles = Image.FromFile(@"C:\working\dq3\overworld\dq3-tiles.png");
			using var img = new Bitmap(4096, 4096);
			using (var g = Graphics.FromImage(img)) {

				for (var row = 0; row < 256; row++) {
					for (var column = 0; column < 256; column++) {
						g.DrawImage(tiles, new Rectangle(column * 16, row * 16, 16, 16), sources[data[row][column]], GraphicsUnit.Pixel);
					}
				}
			}

			img.Save(outFilename, ImageFormat.Png);
		}

		public static void EncodeLayout(byte[] target) {
			/*

  # Find identical sequence in already decoded stream (return length, offset)
  def search_slw(stream):
    MIN_LEN = 2
    MAX_LEN = 64
    MAX_OFFSET = 32767
    search_offset = max(0, stream.offset - MAX_OFFSET)
    forward_window = stream.bytes[stream.offset:stream.offset+MAX_LEN]
    if len(forward_window) < MIN_LEN or stream.offset - search_offset < MIN_LEN:
      return 0, 0

    length = MIN_LEN
    while True:
      if length > len(forward_window) or length > MAX_LEN:
        length -= 1
        break
      match_window = forward_window[0:length]
      search_window = stream.bytes[search_offset:stream.offset+length-2]
      if search_window[0:stream.offset+search_offset+length].count(match_window) > 0:
        length += 1
      else:
        length -= 1
        break

    if length >= MIN_LEN:
      return length, stream.offset - stream.bytes[search_offset:stream.offset+length].index(forward_window[0:length])
    return 0, 0

  # Get number of identical bytes in stream forward from current offset (max_len=4096)
  def search_rle(stream):
    MIN_LEN = 3
    MAX_LEN = 4096
    org_offset = stream.offset
    length = 1
    match_byte = stream.read()
    while stream.at_end() is not True:
      if length == MAX_LEN:
        break
      elif match_byte == stream.read():
        length += 1
      else:
        break
    if length < MIN_LEN:
      length = 0
    stream.offset = org_offset
    return length, match_byte

  # Get number of bytes until next RLE or SW match in stream forward from index (max 64 bytes)
  def get_literal_len(stream):
    org_offset = stream.offset
    length = 1
    while stream.at_end() is not True and search_rle(stream)[0] == 0 and search_slw(stream)[0] == 0:
      if length == 64:
        break
      length += 1
      stream.read()
    stream.offset = org_offset
    return length - 1


  # Encode
  while True:
    if in_bs.at_end():
      break

    # Look for Sliding Window and RLE matches
    slw_len, slw_offset = search_slw(in_bs)
    rle_len, rle_value = search_rle(in_bs)
    if rle_len >= slw_len: slw_len = 0

    # Append Sliding Window opcode
    if slw_len > 0:
      if verbose:
        print "{:06X}:{:06X} SLW sequence, 0x{:04X} bytes from offset 0x{:04X}".format(in_bs.offset, out_bs.length(), slw_len, slw_offset)
      if slw_len > 16 or slw_offset > 1023:
        # 3-byte SLW opcode, #$c0-#$df (%110xxxxx xyyyyyyy yyyyyyyy)
        out_bs.append(0xc0 | (((slw_len - 2) >> 1) & 0x1f))
        out_bs.append((((slw_len - 2) << 7) & 0x80) | ((slw_offset & 0x7f00) >> 8))
        out_bs.append(slw_offset & 0x00ff)
      else:
        # 2-byte SLW opcode, #$80-#$bf (%10xxxxyy yyyyyyyy)
        out_bs.append((0x80 | (slw_len - 2 & 0x0f) << 2) | ((slw_offset & 0x0300) >> 8))
        out_bs.append(slw_offset & 0xff)
      in_bs.fwd(slw_len)
      continue

    # Append RLE opcode
    if rle_len > 0:
      if verbose:
        print "{:06X}:{:06X} RLE sequence, 0x{:02X} * 0x{:X}".format(in_bs.offset, out_bs.length(), rle_value, rle_len)
      if rle_len > 9:
        # 3-byte RLE instruction, #$e0-#$ef (%1110xxxx xxxxxxxx yyyyyyyy)
        out_bs.append(0xe0 | ((rle_len - 3 >> 8) & 0x0f))
        out_bs.append((rle_len - 3) & 0xff)
        out_bs.append(rle_value)
      else:
        # 2-byte RLE instruction, #$f0-#$f7 (%11110xxx yyyyyyyy)
        out_bs.append(0xf0 | ((rle_len - 3) & 0x07))
        out_bs.append(rle_value)
      in_bs.fwd(rle_len)
      continue

    # Append literal
    lit_len = get_literal_len(in_bs)
    append_bytes = in_bs.get(lit_len, True)
    if verbose:
      print "{:06X}:{:06X} LIT sequence, 0x{:X} bytes".format(in_bs.offset, out_bs.length(), lit_len)
      #print "  Bytes:{}".format(bytearray_to_string(append_bytes))
    out_bs.append((lit_len - 1) & 0x3f)
    out_bs.append(append_bytes)
    in_bs.fwd(lit_len)


  # Done, terminate stream
  out_bs.append(0xff)
  if verbose:
    print "---"
    print "Original size     {:8d}B  (0x{:06X})".format(in_bs.length(), in_bs.length())
    print "Compressed size   {:8d}B  (0x{:06X})".format(out_bs.length(), out_bs.length())
    print "Compression ratio {:8.2f}%".format(100.0 * (float(out_bs.length()) / float(in_bs.length())))
  return out_bs.bytes
			 */
		}
	}
}
