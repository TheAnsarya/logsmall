using logsmall.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.OldStuff {
	class OverWorldMap {
		public class LocalRam {
			public Ram ram { get; private set; }

			// $00
			// get as long address
			// set word, keep bank
			private int x00 {
				get => ram.Word(0x7e0000);
				set => ram.Word(0x7e0000, (ushort)value);
			}

			// $00
			public int WordmapIndexesAddress {
				get => ram.Long(0x7e0000);
				set => ram.Long(0x7e0000, value);
			}

			/*
			public int ShortWordmapIndexesAddress {
				get => x00;
				set => x00 = value;
			}*/

			// $02
			// WordmapIndexesAddressBank
			private ushort x02 {
				get => ram.Word(0x7e0002);
				set => ram.Word(0x7e0002, value);
			}

			// $04
			private ushort x04 {
				get => ram.Word(0x7e0004);
				set => ram.Word(0x7e0004, value);
			}

			// $04
			public int DestinationAddress {
				get => x04;
				set => x04 = (ushort)value;
			}

			// $08
			public int x08 {
				get => ram.Word(0x7e0008);
				set => ram.Word(0x7e0008, (ushort)value);
			}

			// $0a
			public int x0a {
				get => ram.Word(0x7e000a);
				set => ram.Word(0x7e000a, (ushort)value);
			}

			// $0c
			public int x0c {
				get => ram.Word(0x7e000c);
				set => ram.Word(0x7e000c, (ushort)value);
			}

			// $10
			private ushort x10 {
				get => ram.Word(0x7e0010);
				set => ram.Word(0x7e0010, value);
			}

			// $10
			public int WorldmapIndexesOffset {
				get => x10;
				set => x10 = (ushort)value;
			}

			// $18
			public int x18 {
				get => ram.Word(0x7e0018);
				set => ram.Word(0x7e0018, (ushort)value);
			}

			// $1e
			public int x1e {
				get => ram.Word(0x7e001e);
				set => ram.Word(0x7e001e, (ushort)value);
			}

			// $24
			public int Counter24 {
				get => ram.Word(0x7e0024);
				set => ram.Word(0x7e0024, (ushort)value);
			}

			// $26
			private ushort x26 {
				get => ram.Word(0x7e0026);
				set => ram.Word(0x7e0026, value);
			}

			// $26
			public int WorldmapIndexesMask {
				get => x26;
				set => x26 = (ushort)value;
			}

			// $28
			public int x28 {
				get => ram.Word(0x7e0028);
				set => ram.Word(0x7e0028, (ushort)value);
			}

			// $2a
			public ushort TilemapIndexOffset {
				get => ram.Word(0x7e002a);
				set => ram.Word(0x7e002a, value);
			}

			// $7e99d9
			public int x7e99d9 {
				get => ram.Word(0x7e99d9);
				set => ram.Word(0x7e99d9, (ushort)value);
			}

			// $7e7ff0,x
			private RamByteArray _x7e7ff0;
			public RamByteArray x7e7ff0 {
				get {
					if (_x7e7ff0 == null) {
						_x7e7ff0 = ram.ByteArray(0x7e7ff0);
					}
					return _x7e7ff0;
				}
			}


			// $e36c,x
			private RamWordArray _xe36c;
			public RamWordArray xe36c {
				get {
					if (_xe36c == null) {
						_xe36c = ram.WordArray(0x7fe36c);
					}
					return _xe36c;
				}
			}

			// $e376,x
			private RamWordArray _xe376;
			public RamWordArray xe376 {
				get {
					if (_xe376 == null) {
						_xe376 = ram.WordArray(0x7fe376);
					}
					return _xe376;
				}
			}

			// $e3c4
			public int xe3c4 {
				get => ram.Word(0x7fe3c4);
				set => ram.Word(0x7fe3c4, (ushort)value);
			}

			// $e3c6
			public int xe3c6 {
				get => ram.Word(0x7fe3c6);
				set => ram.Word(0x7fe3c6, (ushort)value);
			}

			// $e3c8
			public int DestinationAddressStart {
				get => ram.Word(0x7fe3c8);
				set => ram.Word(0x7fe3c8, (ushort)value);
			}

			// $e3d0
			public int xe3d0 {
				get => ram.Word(0x7fe3d0);
				set => ram.Word(0x7fe3d0, (ushort)value);
			}

			// $e3e2
			public int xe3e2 {
				get => ram.Word(0x7fe3e2);
				set => ram.Word(0x7fe3e2, (ushort)value);
			}

			// $e3e4
			public int xe3e4 {
				get => ram.Word(0x7fe3e4);
				set => ram.Word(0x7fe3e4, (ushort)value);
			}

			// $e3e6
			public int xe3e6 {
				get => ram.Word(0x7fe3e6);
				set => ram.Word(0x7fe3e6, (ushort)value);
			}

			// $e3e8
			public int xe3e8 {
				get => ram.Word(0x7fe3e8);
				set => ram.Word(0x7fe3e8, (ushort)value);
			}

			// $e3ea
			public int xe3ea {
				get => ram.Word(0x7fe3ea);
				set => ram.Word(0x7fe3ea, (ushort)value);
			}

			// $e3ec
			public int xe3ec {
				get => ram.Word(0x7fe3ec);
				set => ram.Word(0x7fe3ec, (ushort)value);
			}

			// $e3ee
			public int xe3ee {
				get => ram.Word(0x7fe3ee);
				set => ram.Word(0x7fe3ee, (ushort)value);
			}

			// $e3f0
			public int xe3f0 {
				get => ram.Word(0x7fe3f0);
				set => ram.Word(0x7fe3f0, (ushort)value);
			}

			// $e3f2,y
			private RamWordArray _xe3f2;
			public RamWordArray xe3f2 {
				get {
					if (_xe3f2 == null) {
						_xe3f2 = ram.WordArray(0x7fe3f2);
					}
					return _xe3f2;
				}
			}

			// $e402,y
			private RamWordArray _xe402;
			public RamWordArray xe402 {
				get {
					if (_xe402 == null) {
						_xe402 = ram.WordArray(0x7fe402);
					}
					return _xe402;
				}
			}

			// $e412,y
			private RamWordArray _xe412;
			public RamWordArray xe412 {
				get {
					if (_xe412 == null) {
						_xe412 = ram.WordArray(0x7fe412);
					}
					return _xe412;
				}
			}

			// $e422,y
			private RamWordArray _xe422;
			public RamWordArray xe422 {
				get {
					if (_xe422 == null) {
						_xe422 = ram.WordArray(0x7fe422);
					}
					return _xe422;
				}
			}

			// $e432,y
			private RamWordArray _xe432;
			public RamWordArray xe432 {
				get {
					if (_xe432 == null) {
						_xe432 = ram.WordArray(0x7fe432);
					}
					return _xe432;
				}
			}

			// $e442,y
			private RamWordArray _xe442;
			public RamWordArray xe442 {
				get {
					if (_xe442 == null) {
						_xe442 = ram.WordArray(0x7fe442);
					}
					return _xe442;
				}
			}

			// $e452,y
			private RamWordArray _xe452;
			public RamWordArray xe452 {
				get {
					if (_xe452 == null) {
						_xe452 = ram.WordArray(0x7fe452);
					}
					return _xe452;
				}
			}

			// $e462,y
			private RamWordArray _xe462;
			public RamWordArray xe462 {
				get {
					if (_xe462 == null) {
						_xe462 = ram.WordArray(0x7fe462);
					}
					return _xe462;
				}
			}

			// $e472
			public int xe472 {
				get => ram.Word(0x7fe472);
				set => ram.Word(0x7fe472, (ushort)value);
			}

			// $e474
			public int xe474 {
				get => ram.Word(0x7fe474);
				set => ram.Word(0x7fe474, (ushort)value);
			}

			// $e476
			public int xe476 {
				get => ram.Word(0x7fe476);
				set => ram.Word(0x7fe476, (ushort)value);
			}

			// $e478
			public int xe478 {
				get => ram.Word(0x7fe478);
				set => ram.Word(0x7fe478, (ushort)value);
			}

			// $e47a
			public int xe47a {
				get => ram.Word(0x7fe47a);
				set => ram.Word(0x7fe47a, (ushort)value);
			}

			// $e47c
			public int xe47c {
				get => ram.Word(0x7fe47c);
				set => ram.Word(0x7fe47c, (ushort)value);
			}

			// $e47e
			public int xe47e {
				get => ram.Word(0x7fe47e);
				set => ram.Word(0x7fe47e, (ushort)value);
			}

			// $e480
			public int xe480 {
				get => ram.Word(0x7fe480);
				set => ram.Word(0x7fe480, (ushort)value);
			}

			// $e4b2
			public int xe4b2 {
				get => ram.Word(0x7fe4b2);
				set => ram.Word(0x7fe4b2, (ushort)value);
			}

			// $e4b4
			public int xe4b4 {
				get => ram.Word(0x7fe4b4);
				set => ram.Word(0x7fe4b4, (ushort)value);
			}

			// $e4b6
			public int xe4b6 {
				get => ram.Word(0x7fe4b6);
				set => ram.Word(0x7fe4b6, (ushort)value);
			}

			// $e4b8
			public int xe4b8 {
				get => ram.Word(0x7fe4b8);
				set => ram.Word(0x7fe4b8, (ushort)value);
			}

			// $e4ba
			public int xe4ba {
				get => ram.Word(0x7fe4ba);
				set => ram.Word(0x7fe4ba, (ushort)value);
			}

			// $e4bc
			public int xe4bc {
				get => ram.Word(0x7fe4bc);
				set => ram.Word(0x7fe4bc, (ushort)value);
			}

			// $e4be
			public int xe4be {
				get => ram.Word(0x7fe4be);
				set => ram.Word(0x7fe4be, (ushort)value);
			}

			// $e4c0
			public int xe4c0 {
				get => ram.Word(0x7fe4c0);
				set => ram.Word(0x7fe4c0, (ushort)value);
			}

			// $e4c2,y
			private RamWordArray _xe4c2;
			public RamWordArray xe4c2 {
				get {
					if (_xe4c2 == null) {
						_xe4c2 = ram.WordArray(0x7fe4c2);
					}
					return _xe4c2;
				}
			}

			// $e5b4
			public int xe5b4 {
				get => ram.Word(0x7fe5b4);
				set => ram.Word(0x7fe5b4, (ushort)value);
			}

			// $e5b6
			public int xe5b6 {
				get => ram.Word(0x7fe5b6);
				set => ram.Word(0x7fe5b6, (ushort)value);
			}

			// $e5b8
			public int xe5b8 {
				get => ram.Word(0x7fe5b8);
				set => ram.Word(0x7fe5b8, (ushort)value);
			}

			// $e5ba
			public int xe5ba {
				get => ram.Word(0x7fe5ba);
				set => ram.Word(0x7fe5ba, (ushort)value);
			}

			// $e5bc
			public int xe5bc {
				get => ram.Word(0x7fe5bc);
				set => ram.Word(0x7fe5bc, (ushort)value);
			}

			// $e5be
			public int xe5be {
				get => ram.Word(0x7fe5be);
				set => ram.Word(0x7fe5be, (ushort)value);
			}

			// $e5c0
			public int xe5c0 {
				get => ram.Word(0x7fe5c0);
				set => ram.Word(0x7fe5c0, (ushort)value);
			}

			// $fc4e
			public ushort TilemapIndexA {
				get => ram.Word(0x7ffc4e);
				set => ram.Word(0x7ffc4e, value);
			}

			// $fc50
			public ushort TilemapIndexB {
				get => ram.Word(0x7ffc50);
				set => ram.Word(0x7ffc50, value);
			}

			// $fc52
			public ushort TilemapIndexC {
				get => ram.Word(0x7ffc52);
				set => ram.Word(0x7ffc52, value);
			}

			// $fc54
			public ushort TilemapIndexD {
				get => ram.Word(0x7ffc54);
				set => ram.Word(0x7ffc54, value);
			}

			// $c1a855,x
			public RomByteArray xc1a855 = Rom.ByteArray(0xc1a855);

			private RamWordArray[][] _tilebanks;
			public RamWordArray[][] Tilebanks {
				get {
					if (_tilebanks == null) {
						_tilebanks = new RamWordArray[][] {
							new RamWordArray[] {
								ram.WordArray(0x7fc086),
								ram.WordArray(0x7fd086),
							},
							new RamWordArray[] {
								ram.WordArray(0x7fb886),
								ram.WordArray(0x7fc886),
							}
						};
					}
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
			data.x18 = X;

			if (data.x7e99d9 == 0x000a) {
				// c05394 jsr $53d0
				throw new Exception("Unknown code path: c05394 jsr $53d0");
			} else {
				data.xe3c4 = data.xe36c[X] >> 3;
				data.xe3c6 = data.xe376[X] >> 3;
				ZeroOut800Bytes(data);

				for (int i = 0; i < 0x20; i++) {
					Routine_c1697b(data, 0x20);
					data.xe3c6 += 1;
					data.DestinationAddressStart += 0x0040;
				}
			}
		}

		// Covers: $c0550f - $c0557a
		public static void ZeroOut800Bytes(LocalRam data) {
			var address = data.DestinationAddressStart;
			data.ram.Zero(address, 0x800);
		}

		// Covers: $c1697b - $c16994
		public static void Routine_c1697b(LocalRam data, int A) {
			data.xe5c0 = A;
			Routine_c16f40(data);
		}

		// Covers: $c16f40 - $c17002
		public static void Routine_c16f40(LocalRam data) {
			var X = data.x18;

			if (!(IsPlus(data.xe3e2) || ((data.x18 >> 1) != data.xe472) || (data.xe4b2 == 0))) {
				jump_c17003(data, X, 0);
			} else if (!(IsPlus(data.xe3e4) || ((data.x18 >> 1) != data.xe474) || (data.xe4b4 == 0))) {
				jump_c17003(data, X, 2);
			} else if (!(IsPlus(data.xe3e6) || ((data.x18 >> 1) != data.xe476) || (data.xe4b6 == 0))) {
				jump_c17003(data, X, 4);
			} else if (!(IsPlus(data.xe3e8) || ((data.x18 >> 1) != data.xe478) || (data.xe4b8 == 0))) {
				jump_c17003(data, X, 6);
			} else if (!(IsPlus(data.xe3ea) || ((data.x18 >> 1) != data.xe47a) || (data.xe4ba == 0))) {
				jump_c17003(data, X, 8);
			} else if (!(IsPlus(data.xe3ec) || ((data.x18 >> 1) != data.xe47c) || (data.xe4bc == 0))) {
				jump_c17003(data, X, 0xa);
			} else if (!(IsPlus(data.xe3ee) || ((data.x18 >> 1) != data.xe47e) || (data.xe4be == 0))) {
				jump_c17003(data, X, 0xc);
			} else if (!(IsPlus(data.xe3f0) || ((data.x18 >> 1) != data.xe480) || (data.xe4c0 == 0))) {
				jump_c17003(data, X, 0xe);
			}
		}

		// Covers: $c17003 - $c1706b
		// Covers: $c1a703 - $c1a775
		public static void jump_c17003(LocalRam data, int X, int Y) {
			data.x08 = (X << 5) + 0xfc62;
			data.x0a = data.xe3c4;
			data.x0c = data.xe3c6;
			data.DestinationAddress = data.DestinationAddressStart;
			data.WordmapIndexesAddress = data.xe412[Y] + ((data.xe422[Y] & 0x00ff) << 0x10);

			// jsr $c1a703
			// Covers: $c1a703 - $c1a775
			var tmp = data.xe442[Y] - data.xe432[Y] + 1;
			data.xe5b4 = tmp;
			data.xe5b8 = tmp;
			tmp = data.xe462[Y] - data.xe452[Y] + 1;
			data.xe5b6 = tmp;
			data.xe5ba = tmp;
			data.xe5bc = data.x0a - data.xe432[Y];
			data.xe5be = data.x0c - data.xe452[Y];
			var which = data.xe3f2[Y];
			data.x1e = data.xc1a855[which];

			//jsr ($a73f,x)
			switch (which) {
				case 0:
				case 2:
				case 4:
					break;
				case 6:
				case 8:
				case 0xa:
					data.xe5b4 = data.xe5b4 >> 1;
					data.xe5b6 = data.xe5b6 >> 1;
					data.xe5bc = data.xe5bc >> 1;
					data.xe5be = data.xe5be >> 1;
					break;
				case 0xc:
				case 0xe:
				case 0x10:
					data.xe5b4 = data.xe5b4 >> 3;
					data.xe5b6 = data.xe5b6 >> 3;
					data.xe5bc = data.xe5bc >> 3;
					data.xe5be = data.xe5be >> 3;
					break;
				default:
					throw new Exception($"Invalid value -- {which}");
			}
			// end of -- jsr $c1a703

			if (data.xe402[Y] % 2 == 0) {
				// c17035 jmp $706c
				throw new Exception("Unknown code path: c17035 jmp $706c");
			}

			data.WorldmapIndexesMask = data.xe5b8 - 1;
			data.x28 = data.xe5ba - 1;
			data.x0c = ((data.x0c - data.xe452[Y]) & data.x28) + data.xe452[Y];
			data.xe3d0 = (data.x0a - data.xe432[Y]) & data.WorldmapIndexesMask;
			data.Counter24 = data.xe5c0;
			var which2 = data.xe3f2[Y];

			// jsr ($a81f,x)
			switch (which2) {
				case 0:
					r_c17faf(data, Y);
					break;
				default:
					throw new Exception("Unknown code path: jsr ($a81f,x) -- {which}");
			}
		}

		// $c17faf
		// Called from $c17067 jsr ($a81f,x) where X seems to always be $E
		public static void r_c17faf(LocalRam data, int Y) {
			data.TilemapIndexOffset = data.xe4c2[Y];
			data.WordmapIndexesAddress += (((data.x0c - data.xe452[Y]) >> 3) * data.xe5b4) << 1;
			data.WorldmapIndexesMask = data.WorldmapIndexesMask >> 3 << 1;
			var which = (data.x0c - data.xe452[Y]) & 0x0007;
			TilebankAndWorldmapDataToDestination(data, which);
		}

		// Covers: $c17ff4 - $c18adb
		// Called from $c17ff0
		public static void TilebankAndWorldmapDataToDestination(LocalRam data, int which) {
			var tilebankA = data.Tilebanks[which % 2][0];
			var tilebankB = data.Tilebanks[which % 2][1];
			var worldmapDataA = data.WorldmapData[(which / 2) % 4][0];
			var worldmapDataB = data.WorldmapData[(which / 2) % 4][1];
			var worldmapDataC = data.WorldmapData[(which / 2) % 4][2];
			var worldmapDataD = data.WorldmapData[(which / 2) % 4][3];

			data.WorldmapIndexesOffset = ((data.xe3d0 >> 3) << 2) & data.WorldmapIndexesMask;
			int WorldmapDataIndex = data.ram.Word(data.WordmapIndexesAddress + (data.WorldmapIndexesOffset));

			data.TilemapIndexA = worldmapDataA[WorldmapDataIndex];
			data.TilemapIndexB = worldmapDataB[WorldmapDataIndex];
			data.TilemapIndexC = worldmapDataC[WorldmapDataIndex];
			data.TilemapIndexD = worldmapDataD[WorldmapDataIndex];

			int step = (data.xe3d0 & 0x0007);
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
						data.ram.Word(destinationAddress, tilebankB[WorldmapDataIndex]);
						break;

					case 1:
						data.ram.Word(destinationAddress, tilebankA[WorldmapDataIndex]);
						break;

					case 2:
						WorldmapDataIndex = (data.TilemapIndexB + data.TilemapIndexOffset) << 1;
						data.ram.Word(destinationAddress, tilebankB[WorldmapDataIndex]);
						break;

					case 3:
						data.ram.Word(destinationAddress, tilebankA[WorldmapDataIndex]);
						break;

					case 4:
						WorldmapDataIndex = (data.TilemapIndexC + data.TilemapIndexOffset) << 1;
						data.ram.Word(destinationAddress, tilebankB[WorldmapDataIndex]);
						break;

					case 5:
						data.ram.Word(destinationAddress, tilebankA[WorldmapDataIndex]);
						break;

					case 6:
						WorldmapDataIndex = (data.TilemapIndexD + data.TilemapIndexOffset) << 1;
						data.ram.Word(destinationAddress, tilebankB[WorldmapDataIndex]);
						break;

					case 7:
						data.ram.Word(destinationAddress, tilebankA[WorldmapDataIndex]);
						break;
					default:
						throw new Exception($"Invalid step value: {step}");
				}

				destinationAddress += 2;
				data.Counter24--;
				step = (step + 1) % 8;

				if ((step == 0) && (data.Counter24 != 0)) {
					data.WorldmapIndexesOffset += 2;
					WorldmapDataIndex = data.ram.Word(data.WordmapIndexesAddress + (data.WorldmapIndexesOffset & data.WorldmapIndexesMask));
					data.TilemapIndexA = worldmapDataA[WorldmapDataIndex];
					data.TilemapIndexB = worldmapDataB[WorldmapDataIndex];
					data.TilemapIndexC = worldmapDataC[WorldmapDataIndex];
					data.TilemapIndexD = worldmapDataD[WorldmapDataIndex];
				}
			} while (data.Counter24 != 0);
		}
	}
}
