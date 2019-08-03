using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall {
	class OverWorldMap {

		/*
c0539b lda $e36c,x
c0539e lsr a
c0539f lsr a
c053a0 lsr a
c053a1 sta $e3c4
c053a4 lda $e376,x
c053a7 lsr a
c053a8 lsr a
c053a9 lsr a
c053aa sta $e3c6
c053ad jsr $550f
c053b0 ldx #$0020
c053b3 lda #$0020
c053b6 jsl $c1697b
c053ba inc $e3c6
c053bd lda $e3c8
c053c0 clc
c053c1 adc #$0040
c053c4 sta $e3c8
c053c7 dex
c053c8 bne $53b3
c053ca jsl $c0578b
c053ce plx
c053cf rtl
		public byte[] c0539b(x, ) {

		}
		*/



		public class LocalRam {
			public Ram ram;

			// $00
			public int x00 {
				get => ram.Long(0x7e0000);
			}

			// $04
			public ushort x04 {
				get => ram.Word(0x7e0004);
				set => ram.Word(0x7e0004, value);
			}

			// $10
			public ushort x10 {
				get => ram.Word(0x7e0010);
				set => ram.Word(0x7e0010, value);
			}

			// $24
			public ushort x24 {
				get => ram.Word(0x7e0024);
				set => ram.Word(0x7e0024, value);
			}

			// $26
			public ushort x26 {
				get => ram.Word(0x7e0026);
				set => ram.Word(0x7e0026, value);
			}

			// $2a
			public ushort x2a {
				get => ram.Word(0x7e002a);
				set => ram.Word(0x7e002a, value);
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


			// $c886,y
			public ushort Tilebank_c886(int x) {
				return ram.Word(0x7fc886 + x);
			}

			// $d086,y
			public ushort Tilebank_d086(int x) {
				return ram.Word(0x7fd086 + x);
			}

			// $e3d0
			public ushort xe3d0 {
				get => ram.Word(0x7fe3d0);
				set => ram.Word(0x7fe3d0, value);
			}


			// $ee1f78,x
			public byte WorldmapDataA(int x) {
				return Rom.Byte(0xee1f78 + x);
			}

			// $ee29b5,x
			public byte WorldmapDataB(int x) {
				return Rom.Byte(0xee29b5 + x);
			}

			// $ee33f2,x
			public byte WorldmapDataC(int x) {
				return Rom.Byte(0xee33f2 + x);
			}

			// $ee3e2f,x
			public byte WorldmapDataD(int x) {
				return Rom.Byte(0xee3e2f + x);
			}
		}


		/*
c18981 lda $e3d0
c18984 pha						; save A
c18985 lsr a
c18986 lsr a
c18987 lsr a
c18988 asl a
c18989 and $26
c1898b sta $10					; $10 => $e3d0 >> 3 << 2 AND $26
c1898d tay						; Y => same
c1898e lda [$00],y				; get offset to source offset bases
c18990 tax
c18991 %setAto8bit()			; source offset bases:
c18993 stz $fc4f				; clear high bytes
c18996 stz $fc51
c18999 stz $fc53
c1899c stz $fc55
c1899f lda $ee1f78,x			; set low bytes
c189a3 sta $fc4e				; $fc4e => $ee1f78,x
c189a6 lda $ee29b5,x
c189aa sta $fc50				; $fc50 => $ee29b5,x
c189ad lda $ee33f2,x
c189b1 sta $fc52				; $fc52 => $ee33f2,x
c189b4 lda $ee3e2f,x
c189b8 sta $fc54				; $fc54 => $ee3e2f,x
c189bb %setAto16bit()
c189bd pla
c189be and #$0007
c189c1 asl a
c189c2 tax						; X => $e3d0 and #$0007 << 1
c189c3 jmp ($89c6,x)			; Jump using JumpTable_c189c6

; DATA: jump table $c189c6-$c189d5 ($10 bytes)
JumpTable_c189c6:
	dw $89d6	; Jump_c189d6
	dw $89db	; Jump_c189db
	dw $89e8	; Jump_c189e8
	dw $89ed	; Jump_c189ed
	dw $89fa	; Jump_c189fa
	dw $89ff	; Jump_c189ff
	dw $8a0c	; Jump_c18a0c missing
	dw $8a11	; Jump_c18a11

Jump_c189d6:
c189d6 ldx $04					; X => $04, destination address
c189d8 jmp Jump_c18a54

Jump_c189db:
c189db ldx $04					; X => $04, destination address
c189dd lda $fc4e
c189e0 clc
c189e1 adc $2a
c189e3 asl a
c189e4 tay						; Y => $fc4e + $2a << 1, source offset
c189e5 jmp Jump_c18a68

Jump_c189e8:
c189e8 ldx $04					; X => $04, destination address
c189ea jmp Jump_c18a74

Jump_c189ed:
c189ed ldx $04					; X => $04, destination address
c189ef lda $fc50
c189f2 clc
c189f3 adc $2a
c189f5 asl a
c189f6 tay						; Y => $fc50 + $2a << 1, source offset
c189f7 jmp Jump_c18a88

Jump_c189fa:
c189fa ldx $04					; X => $04, destination address
c189fc jmp Jump_c18a94

Jump_c189ff:
c189ff ldx $04					; X => $04, destination address
c18a01 lda $fc52
c18a04 clc
c18a05 adc $2a
c18a07 asl a
c18a08 tay						; Y => $fc52 + $2a << 1, source offset
c18a09 jmp Jump_c18aa8

Jump_c1890c:
c1890c ldx $04					; X => $04, destination address
c1890e jmp Jump_c18ab4

Jump_c18a11:
c18a11 ldx $04					; X => $04, destination address
c18a13 lda $fc54
c18a16 clc
c18a17 adc $2a
c18a19 asl a
c18a1a tay						; Y => $fc54 + $2a << 1, source offset
c18a1b jmp Jump_c18ac8
		 */
		public static void Whatever_c1(LocalRam data) {
			data.x10 = (ushort)(((data.xe3d0 >> 3) << 2) & data.x26);
			int offset = data.ram.Word(data.x00 + (data.x10));
			data.TilemapIndexA = data.WorldmapDataA(offset);
			data.TilemapIndexB = data.WorldmapDataB(offset);
			data.TilemapIndexC = data.WorldmapDataC(offset);
			data.TilemapIndexD = data.WorldmapDataD(offset);

			int step = (data.xe3d0 & 0x0007);
			int destinationAddress = data.x04 + 0x7f0000;
			switch (step) {
				case 1:
					// Jump_c189db
					offset = (data.TilemapIndexA + data.x2a) << 1;
					break;
				case 3:
					// Jump_c189ed
					offset = (data.TilemapIndexB + data.x2a) << 1;
					break;
				case 5:
					// Jump_c189ff
					offset = (data.TilemapIndexC + data.x2a) << 1;
					break;
				case 7:
					// Jump_c18a11
					offset = (data.TilemapIndexD + data.x2a) << 1;
					break;
				case 0:
				case 2:
				case 4:
				case 6:
					break;
			}

			do {
				switch (step) {
					case 0:
						// #Jump_c18a54:
						offset = (data.TilemapIndexA + data.x2a) << 1;
						data.ram.Word(destinationAddress, data.Tilebank_d086(offset));
						break;

					case 1:
						// #Jump_c18a68:
						data.ram.Word(destinationAddress, data.Tilebank_c886(offset));
						break;

					case 2:
						// #Jump_c18a74:
						offset = (data.TilemapIndexB + data.x2a) << 1;
						data.ram.Word(destinationAddress, data.Tilebank_d086(offset));
						break;

					case 3:
						// #Jump_c18a88:
						data.ram.Word(destinationAddress, data.Tilebank_c886(offset));
						break;

					case 4:
						// #Jump_c18a94
						offset = (data.TilemapIndexC + data.x2a) << 1;
						data.ram.Word(destinationAddress, data.Tilebank_d086(offset));
						break;

					case 5:
						// #Jump_c18aa8:
						data.ram.Word(destinationAddress, data.Tilebank_c886(offset));
						break;

					case 6:
						// #Jump_c18ab4
						offset = (data.TilemapIndexD + data.x2a) << 1;
						data.ram.Word(destinationAddress, data.Tilebank_d086(offset));
						break;

					case 7:
						// #Jump_c18ac8:
						data.ram.Word(destinationAddress, data.Tilebank_c886(offset));
						break;
				}

				destinationAddress += 2;
				data.x24--;
				step = (step + 1) % 8;

				if ((step == 0) && (data.x24 != 0)) {
					data.x10 += 2;
					// Jump_c18a1e:
					offset = data.ram.Word(data.x00 + (data.x10 & data.x26));
					data.TilemapIndexA = data.WorldmapDataA(offset);
					data.TilemapIndexB = data.WorldmapDataB(offset);
					data.TilemapIndexC = data.WorldmapDataC(offset);
					data.TilemapIndexD = data.WorldmapDataD(offset);
				}
			} while (data.x24 != 0);
		}
	}
}
