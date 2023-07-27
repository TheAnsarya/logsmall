using logsmall.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.OldStuff {
	class OverWorldMap {
		public class LocalRam {
			public Ram WRam { get; private set; }

			// $00
			// get as long address
			// set word, keep bank
			private int X00 {
				get => WRam.Word(0x7e0000);
				set => WRam.Word(0x7e0000, (ushort)value);
			}

			// $00
			public int WordmapIndexesAddress {
				get => WRam.Long(0x7e0000);
				set => WRam.Long(0x7e0000, value);
			}

			/*
			public int ShortWordmapIndexesAddress {
				get => x00;
				set => x00 = value;
			}*/

			// $02
			// WordmapIndexesAddressBank
			private ushort X02 {
				get => WRam.Word(0x7e0002);
				set => WRam.Word(0x7e0002, value);
			}

			// $04
			private ushort X04 {
				get => WRam.Word(0x7e0004);
				set => WRam.Word(0x7e0004, value);
			}

			// $04
			public int DestinationAddress {
				get => X04;
				set => X04 = (ushort)value;
			}

			// $08
			public int X08 {
				get => WRam.Word(0x7e0008);
				set => WRam.Word(0x7e0008, (ushort)value);
			}

			// $0a
			public int X0a {
				get => WRam.Word(0x7e000a);
				set => WRam.Word(0x7e000a, (ushort)value);
			}

			// $0c
			public int X0c {
				get => WRam.Word(0x7e000c);
				set => WRam.Word(0x7e000c, (ushort)value);
			}

			// $10
			private ushort X10 {
				get => WRam.Word(0x7e0010);
				set => WRam.Word(0x7e0010, value);
			}

			// $10
			public int WorldmapIndexesOffset {
				get => X10;
				set => X10 = (ushort)value;
			}

			// $18
			public int X18 {
				get => WRam.Word(0x7e0018);
				set => WRam.Word(0x7e0018, (ushort)value);
			}

			// $1e
			public int X1e {
				get => WRam.Word(0x7e001e);
				set => WRam.Word(0x7e001e, (ushort)value);
			}

			// $24
			public int Counter24 {
				get => WRam.Word(0x7e0024);
				set => WRam.Word(0x7e0024, (ushort)value);
			}

			// $26
			private ushort X26 {
				get => WRam.Word(0x7e0026);
				set => WRam.Word(0x7e0026, value);
			}

			// $26
			public int WorldmapIndexesMask {
				get => X26;
				set => X26 = (ushort)value;
			}

			// $28
			public int X28 {
				get => WRam.Word(0x7e0028);
				set => WRam.Word(0x7e0028, (ushort)value);
			}

			// $2a
			public ushort TilemapIndexOffset {
				get => WRam.Word(0x7e002a);
				set => WRam.Word(0x7e002a, value);
			}

			// $7e99d9
			public int X7e99d9 {
				get => WRam.Word(0x7e99d9);
				set => WRam.Word(0x7e99d9, (ushort)value);
			}

			// $7e7ff0,x
			private RamByteArray _x7e7ff0;
			public RamByteArray X7e7ff0 {
				get {
					_x7e7ff0 ??= WRam.ByteArray(0x7e7ff0);

					return _x7e7ff0;
				}
			}


			// $e36c,x
			private RamWordArray _xe36c;
			public RamWordArray Xe36c {
				get {
					_xe36c ??= WRam.WordArray(0x7fe36c);

					return _xe36c;
				}
			}

			// $e376,x
			private RamWordArray _xe376;
			public RamWordArray Xe376 {
				get {
					_xe376 ??= WRam.WordArray(0x7fe376);

					return _xe376;
				}
			}

			// $e3c4
			public int Xe3c4 {
				get => WRam.Word(0x7fe3c4);
				set => WRam.Word(0x7fe3c4, (ushort)value);
			}

			// $e3c6
			public int Xe3c6 {
				get => WRam.Word(0x7fe3c6);
				set => WRam.Word(0x7fe3c6, (ushort)value);
			}

			// $e3c8
			public int DestinationAddressStart {
				get => WRam.Word(0x7fe3c8);
				set => WRam.Word(0x7fe3c8, (ushort)value);
			}

			// $e3d0
			public int Xe3d0 {
				get => WRam.Word(0x7fe3d0);
				set => WRam.Word(0x7fe3d0, (ushort)value);
			}

			// $e3e2
			public int Xe3e2 {
				get => WRam.Word(0x7fe3e2);
				set => WRam.Word(0x7fe3e2, (ushort)value);
			}

			// $e3e4
			public int Xe3e4 {
				get => WRam.Word(0x7fe3e4);
				set => WRam.Word(0x7fe3e4, (ushort)value);
			}

			// $e3e6
			public int Xe3e6 {
				get => WRam.Word(0x7fe3e6);
				set => WRam.Word(0x7fe3e6, (ushort)value);
			}

			// $e3e8
			public int Xe3e8 {
				get => WRam.Word(0x7fe3e8);
				set => WRam.Word(0x7fe3e8, (ushort)value);
			}

			// $e3ea
			public int Xe3ea {
				get => WRam.Word(0x7fe3ea);
				set => WRam.Word(0x7fe3ea, (ushort)value);
			}

			// $e3ec
			public int Xe3ec {
				get => WRam.Word(0x7fe3ec);
				set => WRam.Word(0x7fe3ec, (ushort)value);
			}

			// $e3ee
			public int Xe3ee {
				get => WRam.Word(0x7fe3ee);
				set => WRam.Word(0x7fe3ee, (ushort)value);
			}

			// $e3f0
			public int Xe3f0 {
				get => WRam.Word(0x7fe3f0);
				set => WRam.Word(0x7fe3f0, (ushort)value);
			}

			// $e3f2,y
			private RamWordArray _xe3f2;
			public RamWordArray Xe3f2 {
				get {
					_xe3f2 ??= WRam.WordArray(0x7fe3f2);

					return _xe3f2;
				}
			}

			// $e402,y
			private RamWordArray _xe402;
			public RamWordArray Xe402 {
				get {
					_xe402 ??= WRam.WordArray(0x7fe402);

					return _xe402;
				}
			}

			// $e412,y
			private RamWordArray _xe412;
			public RamWordArray Xe412 {
				get {
					_xe412 ??= WRam.WordArray(0x7fe412);

					return _xe412;
				}
			}

			// $e422,y
			private RamWordArray _xe422;
			public RamWordArray Xe422 {
				get {
					_xe422 ??= WRam.WordArray(0x7fe422);

					return _xe422;
				}
			}

			// $e432,y
			private RamWordArray _xe432;
			public RamWordArray Xe432 {
				get {
					_xe432 ??= WRam.WordArray(0x7fe432);

					return _xe432;
				}
			}

			// $e442,y
			private RamWordArray _xe442;
			public RamWordArray Xe442 {
				get {
					_xe442 ??= WRam.WordArray(0x7fe442);

					return _xe442;
				}
			}

			// $e452,y
			private RamWordArray _xe452;
			public RamWordArray Xe452 {
				get {
					_xe452 ??= WRam.WordArray(0x7fe452);

					return _xe452;
				}
			}

			// $e462,y
			private RamWordArray _xe462;
			public RamWordArray Xe462 {
				get {
					_xe462 ??= WRam.WordArray(0x7fe462);

					return _xe462;
				}
			}

			// $e472
			public int Xe472 {
				get => WRam.Word(0x7fe472);
				set => WRam.Word(0x7fe472, (ushort)value);
			}

			// $e474
			public int Xe474 {
				get => WRam.Word(0x7fe474);
				set => WRam.Word(0x7fe474, (ushort)value);
			}

			// $e476
			public int Xe476 {
				get => WRam.Word(0x7fe476);
				set => WRam.Word(0x7fe476, (ushort)value);
			}

			// $e478
			public int Xe478 {
				get => WRam.Word(0x7fe478);
				set => WRam.Word(0x7fe478, (ushort)value);
			}

			// $e47a
			public int Xe47a {
				get => WRam.Word(0x7fe47a);
				set => WRam.Word(0x7fe47a, (ushort)value);
			}

			// $e47c
			public int Xe47c {
				get => WRam.Word(0x7fe47c);
				set => WRam.Word(0x7fe47c, (ushort)value);
			}

			// $e47e
			public int Xe47e {
				get => WRam.Word(0x7fe47e);
				set => WRam.Word(0x7fe47e, (ushort)value);
			}

			// $e480
			public int Xe480 {
				get => WRam.Word(0x7fe480);
				set => WRam.Word(0x7fe480, (ushort)value);
			}

			// $e4b2
			public int Xe4b2 {
				get => WRam.Word(0x7fe4b2);
				set => WRam.Word(0x7fe4b2, (ushort)value);
			}

			// $e4b4
			public int Xe4b4 {
				get => WRam.Word(0x7fe4b4);
				set => WRam.Word(0x7fe4b4, (ushort)value);
			}

			// $e4b6
			public int Xe4b6 {
				get => WRam.Word(0x7fe4b6);
				set => WRam.Word(0x7fe4b6, (ushort)value);
			}

			// $e4b8
			public int Xe4b8 {
				get => WRam.Word(0x7fe4b8);
				set => WRam.Word(0x7fe4b8, (ushort)value);
			}

			// $e4ba
			public int Xe4ba {
				get => WRam.Word(0x7fe4ba);
				set => WRam.Word(0x7fe4ba, (ushort)value);
			}

			// $e4bc
			public int Xe4bc {
				get => WRam.Word(0x7fe4bc);
				set => WRam.Word(0x7fe4bc, (ushort)value);
			}

			// $e4be
			public int Xe4be {
				get => WRam.Word(0x7fe4be);
				set => WRam.Word(0x7fe4be, (ushort)value);
			}

			// $e4c0
			public int Xe4c0 {
				get => WRam.Word(0x7fe4c0);
				set => WRam.Word(0x7fe4c0, (ushort)value);
			}

			// $e4c2,y
			private RamWordArray _xe4c2;
			public RamWordArray Xe4c2 {
				get {
					_xe4c2 ??= WRam.WordArray(0x7fe4c2);

					return _xe4c2;
				}
			}

			// $e5b4
			public int Xe5b4 {
				get => WRam.Word(0x7fe5b4);
				set => WRam.Word(0x7fe5b4, (ushort)value);
			}

			// $e5b6
			public int Xe5b6 {
				get => WRam.Word(0x7fe5b6);
				set => WRam.Word(0x7fe5b6, (ushort)value);
			}

			// $e5b8
			public int Xe5b8 {
				get => WRam.Word(0x7fe5b8);
				set => WRam.Word(0x7fe5b8, (ushort)value);
			}

			// $e5ba
			public int Xe5ba {
				get => WRam.Word(0x7fe5ba);
				set => WRam.Word(0x7fe5ba, (ushort)value);
			}

			// $e5bc
			public int Xe5bc {
				get => WRam.Word(0x7fe5bc);
				set => WRam.Word(0x7fe5bc, (ushort)value);
			}

			// $e5be
			public int Xe5be {
				get => WRam.Word(0x7fe5be);
				set => WRam.Word(0x7fe5be, (ushort)value);
			}

			// $e5c0
			public int Xe5c0 {
				get => WRam.Word(0x7fe5c0);
				set => WRam.Word(0x7fe5c0, (ushort)value);
			}

			// $fc4e
			public ushort TilemapIndexA {
				get => WRam.Word(0x7ffc4e);
				set => WRam.Word(0x7ffc4e, value);
			}

			// $fc50
			public ushort TilemapIndexB {
				get => WRam.Word(0x7ffc50);
				set => WRam.Word(0x7ffc50, value);
			}

			// $fc52
			public ushort TilemapIndexC {
				get => WRam.Word(0x7ffc52);
				set => WRam.Word(0x7ffc52, value);
			}

			// $fc54
			public ushort TilemapIndexD {
				get => WRam.Word(0x7ffc54);
				set => WRam.Word(0x7ffc54, value);
			}

			// $c1a855,x
			public RomByteArray xc1a855 = Rom.ByteArray(0xc1a855);

			private RamWordArray[][] _tilebanks;
			public RamWordArray[][] Tilebanks {
				get {
					_tilebanks ??= new RamWordArray[][] {
							new RamWordArray[] {
								WRam.WordArray(0x7fc086),
								WRam.WordArray(0x7fd086),
							},
							new RamWordArray[] {
								WRam.WordArray(0x7fb886),
								WRam.WordArray(0x7fc886),
							}
						};

					return _tilebanks;
				}
			}

			public RomByteArray[][] WorldmapData = new RomByteArray[][] {
				new RomByteArray[] {
					Rom.ByteArray(0xeda49c), // a3d
					Rom.ByteArray(0xedaed9), // a3d
					Rom.ByteArray(0xedb916), // a3d
					Rom.ByteArray(0xedc353),
				},
				new RomByteArray[] {
					Rom.ByteArray(0xedcd90),
					Rom.ByteArray(0xedd7cd),
					Rom.ByteArray(0xede20a),
					Rom.ByteArray(0xedec47),
				},
				new RomByteArray[] {
					Rom.ByteArray(0xedf684),
					Rom.ByteArray(0xee00c1),
					Rom.ByteArray(0xee0afe),
					Rom.ByteArray(0xee153b),
				},
				new RomByteArray[] {
					Rom.ByteArray(0xee1f78),
					Rom.ByteArray(0xee29b5),
					Rom.ByteArray(0xee33f2),
					Rom.ByteArray(0xee3e2f),
				},
			};
		}

		// value is word
		public static bool IsPlus(int value) {
			return value < 0x8000;
		}



//		c77e62 %setAXYto16bit()
//c77e64 pea $7e7e
//c77e67 plb
//c77e68 plb
//c77e69 ldy $daa5
//c77e6c jmp $7e93




		// $c0512f - $c05167
		//  -- A is 0, 1, or 2
		public static void Routine_c0512f(LocalRam data, int A) {
			// BG 1
			Routine_c05242(data, 0);
			// c0513a jsl $c02892 -- wait for vblank so data can be dma'd into vram and then continue

			// BG2
			Routine_c05242(data, 1);
			// c05145 jsl $c02892 -- wait for vblank so data can be dma'd into vram and then continue

			// BG3
			Routine_c05242(data, 2);
			// c05150 jsl $c02892 -- wait for vblank so data can be dma'd into vram and then continue

			//data.x7eee5d = data.x7e7fa0;
			//data.x7eee5f = data.x7e7fa8;
		}

		// $c05242 - $c05280
		//  -- A is 0, 1, or 2
		public static void Routine_c05242(LocalRam data, int A) {
			var x = (A & 0x0003) << 1;
			Routine_c05387(data, 0xecc6, x);
			/*
			Routine_c0557b(data);
			// c0525a jsl $c0557b
			if ((data.x7e99d9 == 0x000a) && (x == 0)) {
				// c0526a jsl $c055a4
				throw new Exception("Unknown code path: c0526a jsl $c055a4");
			}
			data.x7e7ff0[x >> 1] = 0x18;
			*/
		}

		// Covers: $c05387 - $c053cf
		public static void Routine_c05387(LocalRam data, int A, int X) {
			data.DestinationAddressStart = A;
			data.X18 = X;

			if (data.X7e99d9 == 0x000a) {
				// c05394 jsr $53d0
				throw new Exception("Unknown code path: c05394 jsr $53d0");
			} else {
				data.Xe3c4 = data.Xe36c[X] >> 3;
				data.Xe3c6 = data.Xe376[X] >> 3;
				ZeroOut800Bytes(data);

				for (int i = 0; i < 0x20; i++) {
					Routine_c1697b(data, 0x20);
					data.Xe3c6 += 1;
					data.DestinationAddressStart += 0x0040;
				}
			}
		}

		// Covers: $c0550f - $c0557a
		public static void ZeroOut800Bytes(LocalRam data) {
			var address = data.DestinationAddressStart;
			data.WRam.Zero(address, 0x800);
		}

		// Covers: $c1697b - $c16994
		public static void Routine_c1697b(LocalRam data, int A) {
			data.Xe5c0 = A;
			Routine_c16f40(data);
		}

		// Covers: $c16f40 - $c17002
		public static void Routine_c16f40(LocalRam data) {
			var X = data.X18;

			if (!(IsPlus(data.Xe3e2) || ((data.X18 >> 1) != data.Xe472) || (data.Xe4b2 == 0))) {
				Jump_c17003(data, X, 0);
			} else if (!(IsPlus(data.Xe3e4) || ((data.X18 >> 1) != data.Xe474) || (data.Xe4b4 == 0))) {
				Jump_c17003(data, X, 2);
			} else if (!(IsPlus(data.Xe3e6) || ((data.X18 >> 1) != data.Xe476) || (data.Xe4b6 == 0))) {
				Jump_c17003(data, X, 4);
			} else if (!(IsPlus(data.Xe3e8) || ((data.X18 >> 1) != data.Xe478) || (data.Xe4b8 == 0))) {
				Jump_c17003(data, X, 6);
			} else if (!(IsPlus(data.Xe3ea) || ((data.X18 >> 1) != data.Xe47a) || (data.Xe4ba == 0))) {
				Jump_c17003(data, X, 8);
			} else if (!(IsPlus(data.Xe3ec) || ((data.X18 >> 1) != data.Xe47c) || (data.Xe4bc == 0))) {
				Jump_c17003(data, X, 0xa);
			} else if (!(IsPlus(data.Xe3ee) || ((data.X18 >> 1) != data.Xe47e) || (data.Xe4be == 0))) {
				Jump_c17003(data, X, 0xc);
			} else if (!(IsPlus(data.Xe3f0) || ((data.X18 >> 1) != data.Xe480) || (data.Xe4c0 == 0))) {
				Jump_c17003(data, X, 0xe);
			}
		}

		// Covers: $c17003 - $c1706b
		// Covers: $c1a703 - $c1a775
		public static void Jump_c17003(LocalRam data, int X, int Y) {
			data.X08 = (X << 5) + 0xfc62;
			data.X0a = data.Xe3c4;
			data.X0c = data.Xe3c6;
			data.DestinationAddress = data.DestinationAddressStart;
			data.WordmapIndexesAddress = data.Xe412[Y] + ((data.Xe422[Y] & 0x00ff) << 0x10);

			// jsr $c1a703
			// Covers: $c1a703 - $c1a775
			var tmp = data.Xe442[Y] - data.Xe432[Y] + 1;
			data.Xe5b4 = tmp;
			data.Xe5b8 = tmp;
			tmp = data.Xe462[Y] - data.Xe452[Y] + 1;
			data.Xe5b6 = tmp;
			data.Xe5ba = tmp;
			data.Xe5bc = data.X0a - data.Xe432[Y];
			data.Xe5be = data.X0c - data.Xe452[Y];
			var which = data.Xe3f2[Y];
			data.X1e = data.xc1a855[which];

			//jsr ($a73f,x)
			switch (which) {
				case 0:
				case 2:
				case 4:
					break;
				case 6:
				case 8:
				case 0xa:
					data.Xe5b4 >>= 1;
					data.Xe5b6 >>= 1;
					data.Xe5bc >>= 1;
					data.Xe5be >>= 1;
					break;
				case 0xc:
				case 0xe:
				case 0x10:
					data.Xe5b4 >>= 3;
					data.Xe5b6 >>= 3;
					data.Xe5bc >>= 3;
					data.Xe5be >>= 3;
					break;
				default:
					throw new Exception($"Invalid value -- {which}");
			}
			// end of -- jsr $c1a703

			if (data.Xe402[Y] % 2 == 0) {
				// c17035 jmp $706c
				throw new Exception("Unknown code path: c17035 jmp $706c");
			}

			data.WorldmapIndexesMask = data.Xe5b8 - 1;
			data.X28 = data.Xe5ba - 1;
			data.X0c = ((data.X0c - data.Xe452[Y]) & data.X28) + data.Xe452[Y];
			data.Xe3d0 = (data.X0a - data.Xe432[Y]) & data.WorldmapIndexesMask;
			data.Counter24 = data.Xe5c0;
			var which2 = data.Xe3f2[Y];

			// jsr ($a81f,x)
			switch (which2) {
				case 0:
					R_c17faf(data, Y);
					break;
				default:
					throw new Exception("Unknown code path: jsr ($a81f,x) -- {which}");
			}
		}

		// $c17faf
		// Called from $c17067 jsr ($a81f,x) where X seems to always be $E
		public static void R_c17faf(LocalRam data, int Y) {
			data.TilemapIndexOffset = data.Xe4c2[Y];
			data.WordmapIndexesAddress += (((data.X0c - data.Xe452[Y]) >> 3) * data.Xe5b4) << 1;
			data.WorldmapIndexesMask = data.WorldmapIndexesMask >> 3 << 1;
			var which = (data.X0c - data.Xe452[Y]) & 0x0007;
			TilebankAndWorldmapDataToDestination(data, which);
		}

		// Covers: $c17ff4 - $c18adb
		// Called from $c17ff0
		public static void TilebankAndWorldmapDataToDestination(LocalRam data, int which) {
			var tilebankA = data.Tilebanks[which % 2][0];
			var tilebankB = data.Tilebanks[which % 2][1];
			var worldmapDataA = data.WorldmapData[which / 2 % 4][0];
			var worldmapDataB = data.WorldmapData[which / 2 % 4][1];
			var worldmapDataC = data.WorldmapData[which / 2 % 4][2];
			var worldmapDataD = data.WorldmapData[which / 2 % 4][3];

			data.WorldmapIndexesOffset = ((data.Xe3d0 >> 3) << 2) & data.WorldmapIndexesMask;
			int WorldmapDataIndex = data.WRam.Word(data.WordmapIndexesAddress + data.WorldmapIndexesOffset);

			data.TilemapIndexA = worldmapDataA[WorldmapDataIndex];
			data.TilemapIndexB = worldmapDataB[WorldmapDataIndex];
			data.TilemapIndexC = worldmapDataC[WorldmapDataIndex];
			data.TilemapIndexD = worldmapDataD[WorldmapDataIndex];

			int step = data.Xe3d0 & 0x0007;
			int destinationAddress = data.DestinationAddress + 0x7f0000;
			switch (step) {
				case 1:
					WorldmapDataIndex = (data.TilemapIndexA + data.TilemapIndexOffset) << 1;
					break;
				case 3:
					WorldmapDataIndex = (data.TilemapIndexB + data.TilemapIndexOffset) << 1;
					break;
				case 5:
					WorldmapDataIndex = (data.TilemapIndexC + data.TilemapIndexOffset) << 1;
					break;
				case 7:
					WorldmapDataIndex = (data.TilemapIndexD + data.TilemapIndexOffset) << 1;
					break;
				case 0:
				case 2:
				case 4:
				case 6:
					break;
				default:
					throw new Exception($"Invalid step value: {step}");
			}

			do {
				switch (step) {
					case 0:
						WorldmapDataIndex = (data.TilemapIndexA + data.TilemapIndexOffset) << 1;
						data.WRam.Word(destinationAddress, tilebankB[WorldmapDataIndex]);
						break;

					case 1:
						data.WRam.Word(destinationAddress, tilebankA[WorldmapDataIndex]);
						break;

					case 2:
						WorldmapDataIndex = (data.TilemapIndexB + data.TilemapIndexOffset) << 1;
						data.WRam.Word(destinationAddress, tilebankB[WorldmapDataIndex]);
						break;

					case 3:
						data.WRam.Word(destinationAddress, tilebankA[WorldmapDataIndex]);
						break;

					case 4:
						WorldmapDataIndex = (data.TilemapIndexC + data.TilemapIndexOffset) << 1;
						data.WRam.Word(destinationAddress, tilebankB[WorldmapDataIndex]);
						break;

					case 5:
						data.WRam.Word(destinationAddress, tilebankA[WorldmapDataIndex]);
						break;

					case 6:
						WorldmapDataIndex = (data.TilemapIndexD + data.TilemapIndexOffset) << 1;
						data.WRam.Word(destinationAddress, tilebankB[WorldmapDataIndex]);
						break;

					case 7:
						data.WRam.Word(destinationAddress, tilebankA[WorldmapDataIndex]);
						break;
					default:
						throw new Exception($"Invalid step value: {step}");
				}

				destinationAddress += 2;
				data.Counter24--;
				step = (step + 1) % 8;

				if ((step == 0) && (data.Counter24 != 0)) {
					data.WorldmapIndexesOffset += 2;
					WorldmapDataIndex = data.WRam.Word(data.WordmapIndexesAddress + (data.WorldmapIndexesOffset & data.WorldmapIndexesMask));
					data.TilemapIndexA = worldmapDataA[WorldmapDataIndex];
					data.TilemapIndexB = worldmapDataB[WorldmapDataIndex];
					data.TilemapIndexC = worldmapDataC[WorldmapDataIndex];
					data.TilemapIndexD = worldmapDataD[WorldmapDataIndex];
				}
			} while (data.Counter24 != 0);
		}
	}
}
