using logsmall;
using System;
using System.IO;

namespace DQ3SFC {
	class Program {

		static void Main(string[] args) {
			var rom = new Rom();

			//-----------------------------------------------------
			var startAddressPC = Rom.AddressToPC(0xc545ee);
			var length = 0xe;
			var data = rom.Data.Span.Slice(startAddressPC, length);

			// $43d2,x	-	address
			// $43b2,x	-	databank
			var xx0 = data.Long(0);

			// $4452,x	-	address
			// $4432,x	-	databank
			var xx3 = data.Long(3);

			// $44b2,x
			var xx6 = data[6];

			// $44f2,x
			var xx7 = data.Word(7);

			// $4512,x
			var xx9 = data.Word(9);

			// $45f2,x
			var xxb = data.Word(0xb);

			// $4612,x
			var xxd = data[0xd] > 0x80 ? 0x8000 : data[0xd];

			//-----------------------------------------------------

			var startAddress2PC = Rom.AddressToPC(xx3);
			var graphicsAddressPC = Rom.AddressToPC(rom.Data.Span.Word(startAddress2PC));

			if (xx6 != 0x02) {
				// does some other stuff at $c79d2d
				// does some other stuff at $c79ea1 with graphicsAddress+2 (and the $dd89,X array)

				// not actual number tiles copied, for example the slime is 4 tiles:
				// endTileOffset is $12
				// write one tile, --, write one tile --
				// endTileOffset is $10
				// address/command is $ff0e so skip $0e (destination address is increased by $0e tiles, endTileOffset is decreased by $0e)
				// endTileOffset is $02
				// write one tile, --, write one tile --
				// endTileOffset is $00 so done
				var endTileOffset = rom.Data.Span.Word(graphicsAddressPC + 2);

				// around $c79d7a 
				var tilesAddressListPC = graphicsAddressPC + 4;

				// $c7c0b1
				// address of the "bank" of graphics
				var graphicsStartAddress = 0xce0080;

				// $c7c0e6
				//while (endTileOffset > 0) {
				var tileNumber = rom.Data.Span.Word(tilesAddressListPC);
				if (tileNumber >= 0xff00) {
					var skip = tileNumber & 0xff00;
					endTileOffset -= skip;
				} else {
					var tileAddress = graphicsStartAddress + (tileNumber << 5);



					endTileOffset--;
				}




			} else {
				// is at $c79db1 
			}


			Console.ReadKey();
		}

		// for copying slime grapgics to vram
		// tile numbers/offsets or skip destination commands
		// $ffxx means skip xx tiles in destination, used in "total tile count" thing
		class TileLayoutList {
			
		}

		interface ITileBank {

		}

		// Tiles are 8x8 4bpp planar composite 1-dimensional
		class TileBank {
			// Main graphics bank at $ce0080
			// NumberOfTiles = 0xafc4
			public static readonly TileBank Primary = new TileBank() {
				StartAddress = 0xce0080,
				// This is a guess based on looking in TileMolester
				// Non inclusive, last byte is at $e3f8ff
				EndAddress = 0xe3f900,
				// $20 bytes
				TileDataSize = 0x20
			};

			public int StartAddress { get; set; }

			public int EndAddress { get; set; }

			public int TileDataSize { get; set; }

			public int NumberOfTiles { get => (EndAddress - StartAddress) / TileDataSize; }

			public int AddressOf(int index) => StartAddress + (index << 5);

			public bool IsValidIndex(int index) => (0 <= index) && (index < NumberOfTiles);

			public Memory<byte> GetDataFor(int index) => Rom.Slice(AddressOf(index), TileDataSize);
		}
	}
}
