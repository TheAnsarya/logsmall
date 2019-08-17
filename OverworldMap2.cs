using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	class OverworldMap2 {


		public static void GetMapImage() {

			// I think this is map layout (when to read a block of sea or something or when to read map data)
			// see debug log: trying to find water 02
			// code start: c04923
			// code end: c04a43
			// code never hits: c049b3 (does hit when loading cave map)
			// $ed8a00 - $ed9be2
			var layout = Rom.ByteArray(0xed8a00);

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

			var sources =
				Enumerable.Range(0, 256)
				.ToDictionary(
					x => x,
					x => new Rectangle((x % 16) * 16, (x / 16) * 16, 16, 16)
				);

			using (var tiles = Image.FromFile(@"C:\working\dq3\overworld\dq3-tiles.png")) {
				using (var img = new Bitmap(4096, 4096)) {
					using (var g = Graphics.FromImage(img)) {

						for (int i = 0; i < 64; i++) {
							for (int k = 0; k < 4; k++) {
								for (int j = 0; j < 48; j++) {
									var offset = (i * 48) + j;
									var y = ((i * 4) + k) * 16;
									g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 0), y, 16, 16), sources[tilemaps[k, 0][offset]], GraphicsUnit.Pixel);
									g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 1), y, 16, 16), sources[tilemaps[k, 1][offset]], GraphicsUnit.Pixel);
									g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 2), y, 16, 16), sources[tilemaps[k, 2][offset]], GraphicsUnit.Pixel);
									g.DrawImage(tiles, new Rectangle((j * 64) + (16 * 3), y, 16, 16), sources[tilemaps[k, 3][offset]], GraphicsUnit.Pixel);
								}
							}
						}

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

		public static void GetLayoutAnnotated() {
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

			// c04929
			ushort x18x19 = 0;
			int x1c = 0x1000;
			int x1e = 0x03BE;
			byte x1a = 0;
			byte x1b = 0;
			int x20 = 0;
			byte x22 = 0;

			// don't have initial values?
			byte x24 = 0;
			byte x25 = 0;

			// $c04945
			var y = 0;
			var a = 0;
			var x = 0;
			int saveY;

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
				if ((x18x19 & 0x0100) != 0) {

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
						a = 0;
						continue;
					} else {
						//.Branch_c049ad
						//c049ad %setAXYto16bit()
						//c049af ply
						//c049b0 plx
						//c049b1 pla
						//c049b2 plp
						//c049b3 rtl
						break;  // EXIT
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
				a = (layoutSource[y] & 0x3f) + 3;
				y++;
				//c049ca phy
				saveY = y;
				//c049cb tay
				y = a;

				//.Branch_c049cc
				while (true) {
					//c049cc dey
					y--;
					//c049cd bmi .Branch_c04a3a
					if (SNES.IsNegative16(y)) {
						y = saveY;
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
						x24 = outputf3c6[x1a];
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
						x25 = outputf3c6[x1a];

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
							break; // EXIT
						}


						//c04a33 lda #$0000
						//c04a36 sep #$20
						// ---------------------------------A => 8bit
						//c04a38 bra .Branch_c049cc
					}
				}

				/*
				.Branch_c04a3a
				c04a3a ply
							 * */

				//c04a3b jmp Jump_c0494b	// back to top
			} // end of while

			//.Branch_c04a3e	-- EXIT
		}
	}
}
