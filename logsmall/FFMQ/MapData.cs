using logsmall.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.FFMQ {
	class MapData {


		public static void Go() {
			var rom = FFMQ.Game.Rom;
			//0B81A5 $AD $91 $0E     LDA $0E91 [010E91] = $0D        A:400D X:182E Y:0080 S:1FC8 D:0000 DB:01 P:nvMxdizc V:30  H:166
			var start = 0x0d; // byte
			start *= 2;

			Console.WriteLine($"{nameof(start)} == {start.ToString("x2")}");
			//0B81AF $BF $3B $AF $07 LDA $07AF3B,X[07AF55] = $0234  A: 001A X:001A Y:0080 S: 1FC8 D:0000 DB: 01 P: nvmxdizc V:30  H: 192
			var lookup_07af3b = rom.GetStream(0x07af3b);
			var offset = lookup_07af3b.WordAt(start);

			Console.WriteLine($"{nameof(offset)} == {offset.ToString("x4")}");

			//0B81BC $BF $13 $B0 $07 LDA $07B013,X[07B247] = $07    A: 0234 X: 0234 Y: 0000 S: 1FC8 D:0000 DB: 01 P: nvMxdiZc V:30  H: 229
			//0B81C0 $99 $10 $19     STA $1910,Y[011910] = $00      A: 0207 X: 0234 Y: 0000 S: 1FC8 D:0000 DB: 01 P: nvMxdizc V:30  H: 239
			//0B81C3 $E8 INX                             A: 0207 X: 0234 Y: 0000 S: 1FC8 D:0000 DB: 01 P: nvMxdizc V:30  H: 249
			//0B81C4 $C8 INY                             A: 0207 X: 0235 Y: 0000 S: 1FC8 D:0000 DB: 01 P: nvMxdizc V:30  H: 252
			//0B81C5 $C0 $07 $00     CPY #$0007                      A:0207 X:0235 Y:0001 S:1FC8 D:0000 DB:01 P:nvMxdizc V:30  H:256
			var optionsLookup = rom.GetStream(0x07b013);

			// $1910 - $1916
			// $1910 is index into tilemap data
			// $1911 is index into bg tile graphics data
			// $1912 is index into color palettes
			var options = optionsLookup.GetBytesAt(7, offset);

			Console.WriteLine($"{nameof(options)} == {options.ToHexString()}");



			//; multiply(lower six bits of $1910) * 3
			//0b850e sep #$20
			//0b8510 lda $1910
			//0b8513 and #$3f
			//0b8515 sta $4202
			//0b8518 lda #$03
			//0b851a sta $4203

			//; get multiply result
			//0b8526 ldx $4216

			var mapDataOffsetIndex = (options[0] & 0x3f) * 3;

			Console.WriteLine($"{nameof(mapDataOffsetIndex)} == {mapDataOffsetIndex.ToString("x4")}");

			var mapDataOffsetLookup = rom.GetStream(0x0b8735);


			//; store source data lookup address, long, at $0900 => value at $0b8735,x
			//0b8529 rep #$20
			//0b852b lda $0b8735,x
			//0b852f sta $0900
			//0b8532 sep #$20
			//0b8534 lda $0b8737,x
			//0b8538 sta $0902

			var mapDataOffset = mapDataOffsetLookup.LongAt(mapDataOffsetIndex);

			Console.WriteLine($"{nameof(mapDataOffset)} == {mapDataOffset.ToString("x4")}");

			// $1000 seems to be the largest map, but using $2000 for now
			var (tilemapcomp, tilemapdecomp) = SimpleTailWindowCompression.DecompressFull(rom.GetStream(mapDataOffset), 0x2000);


			Utilities.WriteBytesToFile(tilemapdecomp, @"c:\working\ffmq\~go! -- decomp.txt");




			// $19b7
			var bgGraphicsOffsetsIndex = options[1] * 0x0a;
			var lookup_0b8cd9 = rom.GetStream(0x0b8cd9);

			// $1918 - $1921
			// $1918 and $1919 are ???
			// $191a to $1921 are bg tile graphics offsets
			var x1918 = lookup_0b8cd9.GetBytesAt(0x0a, bgGraphicsOffsetsIndex);








			Console.ReadKey();
		}
	}
}
