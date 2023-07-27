using logsmall.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsmall.DQ3.OldStuff {
	class Routine_c60f98 {
		/*

C61ED5 $AF $60 $98 $7E LDA $7E9860                     A:01FF X:4000 Y:6000 S:0373 D:0000 DB:7F P:04 V:88  H:103
C61ED9 $8D $5A $FC     STA $FC5A [7FFC5A] = $0400      A:0400 X:4000 Y:6000 S:0373 D:0000 DB:7F P:04 V:88  H:113
C61EDC $20 $45 $21     JSR $2145 [C62145] = $00A2      A:0400 X:4000 Y:6000 S:0373 D:0000 DB:7F P:04 V:88  H:121
C62145 $A2 $00 $00     LDX #$0000                      A:0400 X:4000 Y:6000 S:0371 D:0000 DB:7F P:04 V:88  H:131
C62148 $9E $62 $FC     STZ $FC62,X [7FFC62] = $027B    A:0400 X:0000 Y:6000 S:0371 D:0000 DB:7F P:06 V:88  H:146
C6214B $9E $A2 $FC     STZ $FCA2,X [7FFCA2] = $027B    A:0400 X:0000 Y:6000 S:0371 D:0000 DB:7F P:06 V:88  H:156
C6214E $9E $E2 $FC     STZ $FCE2,X [7FFCE2] = $0000    A:0400 X:0000 Y:6000 S:0371 D:0000 DB:7F P:06 V:88  H:166
C62151 $E8             INX                             A:0400 X:0000 Y:6000 S:0371 D:0000 DB:7F P:06 V:88  H:176
C62152 $E8             INX                             A:0400 X:0001 Y:6000 S:0371 D:0000 DB:7F P:04 V:88  H:179
C62153 $E0 $40 $00     CPX #$0040                      A:0400 X:0002 Y:6000 S:0371 D:0000 DB:7F P:04 V:88  H:182
C62156 $90 $F0         BCC $C62148                     A:0400 X:0002 Y:6000 S:0371 D:0000 DB:7F P:84 V:88  H:186
C62148 $9E $62 $FC     STZ $FC62,X [7FFC64] = $027C    A:0400 X:0002 Y:6000 S:0371 D:0000 DB:7F P:84 V:88  H:191
C6214B $9E $A2 $FC     STZ $FCA2,X [7FFCA4] = $027C    A:0400 X:0002 Y:6000 S:0371 D:0000 DB:7F P:84 V:88  H:201
*/
		public class LocalRam {
			public Ram WRam { get; private set; }

			// $40
			public int X40 {
				get => WRam.Word(0x7e0040);
				set => WRam.Word(0x7e0040, (ushort)value);
			}
		}

		public static void Routine_c0512f(LocalRam data, int A) {
			Routine_c61ecf(data, A);
		}

		public static void Routine_c61ecf(LocalRam data, int A) {
			// $7ffc5a => $7e9860
			Routine_c62145(data);
			Routine_c6217d(data);


		}

		// Covers: $c62145 - $c6217c
		public static void Routine_c62145(LocalRam data) {
			data.WRam.Zero(0x7ffc62, 0xc0);
			data.WRam.Zero(0x7ea843, 0xb4);
			data.WRam.Zero(0x7eac7b, 0x80);
		}

		// Covers: $c6217d - $c6219f
		public static void Routine_c6217d(LocalRam data) {
			// A, X => $7e99bf	-- $03ca
			//Routine_c07d43(data, out bool carry);
			/*
c62182 jsl Routine_c07d43

.Branch_c62186
c62186 bcc .Branch_c62186

c62188 ldx #$fc62
c6218b jsr Routine_c621a0

c6218e lda $7e99c1
c62192 tax
c62193 jsl Routine_c07d43

.Branch_c62197
c62197 bcc .Branch_c62197

c62199 ldx #$fca2
c6219c jsr Routine_c621a0
*/
		}


		// $c07d43 - $c07d76
		public static void Routine_c07d43(LocalRam data, out bool carry, int X) {
			// bank => $7f

			Routine_c90572(data, 0xc07d53, X);
			//00 09 00 7D 18 C5 04 00 FF 03 00
			//wiggles the stack for return

			// tax
			Routine_c067fd(data, out carry);
		}

		public static void Routine_c067fd(LocalRam data, out bool carry) {
			throw new NotImplementedException();
		}

		public static void Routine_c90572(LocalRam data, int optionsAddress, int X) {
			data.X40 = X;
			var tmp = Rom.Byte(optionsAddress);
			if (tmp != 0) {
				// c90588 brl.Branch_c90610
			}

			var a = Rom.Word(optionsAddress + 1);
			X = 0x40;

			//c90593 jsl Routine_c01146
			//Routine_c01146(data, X, A);

			//........
		}

		private static void Routine_c01146(LocalRam data, int X, ushort A) {
			var tmpC0114C = data.WRam.Byte(0x7e0001 + X);

			var tmpC01152 = data.WRam.Byte(0x7e0000 + X);
			var m1 = tmpC0114C * tmpC01152;
			//data.ram.Word(0x7e0001 + X, )

			/*
C90593 $22 $46 $11 $C0 JSL $C01146                     A:0008 X:0040 Y:0002 S:034E D:0000 DB:C0 P:15 V:107 H:20 
C01146 $08             PHP                             A:0008 X:0040 Y:0002 S:034B D:0000 DB:C0 P:15 V:107 H:34 
C01147 $E2 $20         SEP #$20                        A:0008 X:0040 Y:0002 S:034A D:0000 DB:C0 P:15 V:107 H:39 
C01149 $48             PHA                             A:0008 X:0040 Y:0002 S:034A D:0000 DB:C0 P:35 V:107 H:43 
C0114A $EB             XBA                             A:0008 X:0040 Y:0002 S:0349 D:0000 DB:C0 P:35 V:107 H:48 
C0114B $48             PHA                             A:0800 X:0040 Y:0002 S:0349 D:0000 DB:C0 P:37 V:107 H:53 
C0114C $B5 $01         LDA $01,X [000041] = $00        A:0800 X:0040 Y:0002 S:0348 D:0000 DB:C0 P:37 V:107 H:58 
C0114E $48             PHA                             A:0800 X:0040 Y:0002 S:0348 D:0000 DB:C0 P:37 V:107 H:64 
C0114F $A3 $03         LDA $03,S [00034A] = $08        A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:37 V:107 H:69 
C01151 $EB             XBA                             A:0808 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:35 V:107 H:76 
C01152 $B5 $00         LDA $00,X [000040] = $47        A:0808 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:35 V:107 H:80 
C01154 $48             PHA                             A:0847 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:35 V:107 H:87 
C01155 $8F $02 $42 $00 STA $004202                     A:0847 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:92 
C01159 $EB             XBA                             A:0847 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:99 
C0115A $8F $03 $42 $00 STA $004203                     A:4708 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:104
C0115E $EA             NOP                             A:4708 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:111
C0115F $EA             NOP                             A:4708 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:114
C01160 $EA             NOP                             A:4708 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:117
C01161 $EA             NOP                             A:4708 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:120
C01162 $AF $16 $42 $00 LDA $004216                     A:4708 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:123
C01166 $95 $00         STA $00,X [000040] = $47        A:4738 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:131
C01168 $AF $17 $42 $00 LDA $004217                     A:4738 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:147
C0116C $95 $01         STA $01,X [000041] = $00        A:4702 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:155
C0116E $68             PLA                             A:4702 X:0040 Y:0002 S:0346 D:0000 DB:C0 P:35 V:107 H:161
C0116F $EB             XBA                             A:4747 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:35 V:107 H:168
C01170 $A3 $02         LDA $02,S [000349] = $00        A:4747 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:35 V:107 H:172
C01172 $8F $02 $42 $00 STA $004202                     A:4700 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:37 V:107 H:179
C01176 $EB             XBA                             A:4700 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:37 V:107 H:186
C01177 $8F $03 $42 $00 STA $004203                     A:0047 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:35 V:107 H:191
C0117B $EA             NOP                             A:0047 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:35 V:107 H:198
C0117C $EA             NOP                             A:0047 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:35 V:107 H:201
C0117D $EA             NOP                             A:0047 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:35 V:107 H:204
C0117E $EA             NOP                             A:0047 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:35 V:107 H:207
C0117F $AF $16 $42 $00 LDA $004216                     A:0047 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:35 V:107 H:210
C01183 $18             CLC                             A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:37 V:107 H:218
C01184 $75 $01         ADC $01,X [000041] = $02        A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:221
C01186 $95 $01         STA $01,X [000041] = $02        A:0002 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:34 V:107 H:227
C01188 $AF $17 $42 $00 LDA $004217                     A:0002 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:34 V:107 H:234
C0118C $69 $00         ADC #$00                        A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:241
C0118E $95 $02         STA $02,X [000042] = $C0        A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:244
C01190 $A3 $01         LDA $01,S [000348] = $00        A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:251
C01192 $EB             XBA                             A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:257
C01193 $A3 $03         LDA $03,S [00034A] = $08        A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:262
C01195 $8F $02 $42 $00 STA $004202                     A:0008 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:34 V:107 H:268
C01199 $EB             XBA                             A:0008 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:34 V:107 H:276
C0119A $8F $03 $42 $00 STA $004203                     A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:288
C0119E $EA             NOP                             A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:295
C0119F $EA             NOP                             A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:298
C011A0 $EA             NOP                             A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:301
C011A1 $EA             NOP                             A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:304
C011A2 $AF $16 $42 $00 LDA $004216                     A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:307
C011A6 $18             CLC                             A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:315
C011A7 $75 $01         ADC $01,X [000041] = $02        A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:318
C011A9 $95 $01         STA $01,X [000041] = $02        A:0802 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:34 V:107 H:324
C011AB $AF $17 $42 $00 LDA $004217                     A:0802 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:34 V:107 H:330
C011AF $75 $02         ADC $02,X [000042] = $00        A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:107 H:337
C011B1 $95 $02         STA $02,X [000042] = $00        A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:4  
C011B3 $A9 $00         LDA #$00                        A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:10 
C011B5 $2A             ROL                             A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:13 
C011B6 $95 $03         STA $03,X [000043] = $00        A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:16 
C011B8 $A3 $01         LDA $01,S [000348] = $00        A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:23 
C011BA $EB             XBA                             A:0800 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:29 
C011BB $A3 $02         LDA $02,S [000349] = $00        A:0008 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:34 V:108 H:34 
C011BD $8F $02 $42 $00 STA $004202                     A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:40 
C011C1 $EB             XBA                             A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:48 
C011C2 $8F $03 $42 $00 STA $004203                     A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:52 
C011C6 $EA             NOP                             A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:60 
C011C7 $EA             NOP                             A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:63 
C011C8 $EA             NOP                             A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:66 
C011C9 $EA             NOP                             A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:69 
C011CA $AF $16 $42 $00 LDA $004216                     A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:72 
C011CE $18             CLC                             A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:79 
C011CF $75 $02         ADC $02,X [000042] = $00        A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:82 
C011D1 $95 $02         STA $02,X [000042] = $00        A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:89 
C011D3 $AF $17 $42 $00 LDA $004217                     A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:95 
C011D7 $75 $03         ADC $03,X [000043] = $00        A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:103
C011D9 $95 $03         STA $03,X [000043] = $00        A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:109
C011DB $68             PLA                             A:0000 X:0040 Y:0002 S:0347 D:0000 DB:C0 P:36 V:108 H:116
C011DC $68             PLA                             A:0000 X:0040 Y:0002 S:0348 D:0000 DB:C0 P:36 V:108 H:122
C011DD $EB             XBA                             A:0000 X:0040 Y:0002 S:0349 D:0000 DB:C0 P:36 V:108 H:129
C011DE $68             PLA                             A:0000 X:0040 Y:0002 S:0349 D:0000 DB:C0 P:36 V:108 H:133
C011DF $28             PLP                             A:0008 X:0040 Y:0002 S:034A D:0000 DB:C0 P:34 V:108 H:150
C011E0 $6B             RTL                             A:0008 X:0040 Y:0002 S:034B D:0000 DB:C0 P:15 V:108 H:156
*/
		}
	}
}
