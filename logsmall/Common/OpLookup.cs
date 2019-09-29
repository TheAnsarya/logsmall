using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logsmall.Common {
	class OpLookup {
		public string Op { get; set; }
		public Regex Mode { get; set; }
		public string Byte { get; set; }

		public static OpLookup Find(string op, string parameters) {
			return Lookups.FirstOrDefault(x => (x.Op == op) && x.Mode.IsMatch(parameters));
		}

		public static OpLookup[] Lookups = new OpLookup[] {
			// adc ($00,x)
			new OpLookup{Op = "adc", Byte = "61", Mode = AddressingMode.DPIndexedIndirectX },
			// adc $00,s
			new OpLookup{Op = "adc", Byte = "63", Mode = AddressingMode.StackRelative },
			// adc $00
			new OpLookup{Op = "adc", Byte = "65", Mode = AddressingMode.DirectPage },
			// adc [$00]
			new OpLookup{Op = "adc", Byte = "67", Mode = AddressingMode.DPIndirectLong },
			// adc #$00
			// adc #$0000
			new OpLookup{Op = "adc", Byte = "69", Mode = AddressingMode.Immediate },
			// adc $0000
			new OpLookup{Op = "adc", Byte = "6d", Mode = AddressingMode.Absolute },
			// adc $000000
			new OpLookup{Op = "adc", Byte = "6f", Mode = AddressingMode.AbsoluteLong },
			// adc ($00),y
			new OpLookup{Op = "adc", Byte = "71", Mode = AddressingMode.DPIndirectIndexedY },
			// adc ($00)
			new OpLookup{Op = "adc", Byte = "72", Mode = AddressingMode.DPIndirect },
			// adc ($00,s),y
			new OpLookup{Op = "adc", Byte = "73", Mode = AddressingMode.SRIndirectIndexedY },
			// adc $00,x
			new OpLookup{Op = "adc", Byte = "75", Mode = AddressingMode.DPIndexedX },
			// adc [$00],y
			new OpLookup{Op = "adc", Byte = "77", Mode = AddressingMode.DPIndirectLongIndexedY },
			// adc $0000,y
			new OpLookup{Op = "adc", Byte = "79", Mode = AddressingMode.AbsoluteIndexedY },
			// adc $0000,x
			new OpLookup{Op = "adc", Byte = "7d", Mode = AddressingMode.AbsoluteIndexedX },
			// adc $000000,y
			new OpLookup{Op = "adc", Byte = "7f", Mode = AddressingMode.AbsoluteLongIndexedX },
			// and ($00,x)
			new OpLookup{Op = "and", Byte = "21", Mode = AddressingMode.DPIndexedIndirectX },
			// and $00,s
			new OpLookup{Op = "and", Byte = "23", Mode = AddressingMode.StackRelative },
			// and $00
			new OpLookup{Op = "and", Byte = "25", Mode = AddressingMode.DirectPage },
			// and [$00]
			new OpLookup{Op = "and", Byte = "27", Mode = AddressingMode.DPIndirectLong },
			// and #$00
			// and #$0000
			new OpLookup{Op = "and", Byte = "29", Mode = AddressingMode.Immediate },
			// and $0000
			new OpLookup{Op = "and", Byte = "2d", Mode = AddressingMode.Absolute },
			// and $000000
			new OpLookup{Op = "and", Byte = "2f", Mode = AddressingMode.AbsoluteLong },
			// and ($00),y
			new OpLookup{Op = "and", Byte = "31", Mode = AddressingMode.DPIndirectIndexedY },
			// and ($00)
			new OpLookup{Op = "and", Byte = "32", Mode = AddressingMode.DPIndirect },
			// and ($00,s),y
			new OpLookup{Op = "and", Byte = "33", Mode = AddressingMode.SRIndirectIndexedY },
			// and $00,x
			new OpLookup{Op = "and", Byte = "35", Mode = AddressingMode.DPIndexedX },
			// and [$00],y
			new OpLookup{Op = "and", Byte = "37", Mode = AddressingMode.DPIndirectLongIndexedY },
			// and $0000,y
			new OpLookup{Op = "and", Byte = "39", Mode = AddressingMode.AbsoluteIndexedY },
			// and $0000,x
			new OpLookup{Op = "and", Byte = "3d", Mode = AddressingMode.AbsoluteIndexedX },
			// and $000000,x
			new OpLookup{Op = "and", Byte = "3f", Mode = AddressingMode.AbsoluteLongIndexedX },
			// asl $00
			new OpLookup{Op = "asl", Byte = "06", Mode = AddressingMode.DirectPage },

			// asl a
			new OpLookup{Op = "asl", Byte = "0a", Mode = AddressingMode.Accumulator },
			// asl
			new OpLookup{Op = "asl", Byte = "0a", Mode = AddressingMode.Implied },

			// asl $0000
			new OpLookup{Op = "asl", Byte = "0e", Mode = AddressingMode.Absolute },
			// asl $00,x
			new OpLookup{Op = "asl", Byte = "16", Mode = AddressingMode.DPIndexedX },
			// asl $0000,x
			new OpLookup{Op = "asl", Byte = "1e", Mode = AddressingMode.AbsoluteIndexedX },
			// bcc $0000
			new OpLookup{Op = "bcc", Byte = "90", Mode = AddressingMode.PCRelative },
			// bcc $000000
			new OpLookup{Op = "bcc", Byte = "90", Mode = AddressingMode.PCRelative24 },
			// bcs $0000
			new OpLookup{Op = "bcs", Byte = "b0", Mode = AddressingMode.PCRelative },
			// bcs $000000
			new OpLookup{Op = "bcs", Byte = "b0", Mode = AddressingMode.PCRelative24 },
			// beq $0000
			new OpLookup{Op = "beq", Byte = "f0", Mode = AddressingMode.PCRelative },
			// beq $000000
			new OpLookup{Op = "beq", Byte = "f0", Mode = AddressingMode.PCRelative24 },
			// bit $00
			new OpLookup{Op = "bit", Byte = "24", Mode = AddressingMode.DirectPage },
			// bit $0000
			new OpLookup{Op = "bit", Byte = "2c", Mode = AddressingMode.Absolute },
			// bit $00,x
			new OpLookup{Op = "bit", Byte = "34", Mode = AddressingMode.DPIndexedX },
			// bit $0000,x
			new OpLookup{Op = "bit", Byte = "3c", Mode = AddressingMode.AbsoluteIndexedX },
			// bit #$00
			// bit #$0000
			new OpLookup{Op = "bit", Byte = "89", Mode = AddressingMode.Immediate },
			// bmi $0000
			new OpLookup{Op = "bmi", Byte = "30", Mode = AddressingMode.PCRelative },
			// bmi $000000
			new OpLookup{Op = "bmi", Byte = "30", Mode = AddressingMode.PCRelative24 },
			// bne $0000
			new OpLookup{Op = "bne", Byte = "d0", Mode = AddressingMode.PCRelative },
			// bne $000000
			new OpLookup{Op = "bne", Byte = "d0", Mode = AddressingMode.PCRelative24 },
			// bpl $0000
			new OpLookup{Op = "bpl", Byte = "10", Mode = AddressingMode.PCRelative },
			// bpl $000000
			new OpLookup{Op = "bpl", Byte = "10", Mode = AddressingMode.PCRelative24 },
			// bra $0000
			new OpLookup{Op = "bra", Byte = "80", Mode = AddressingMode.PCRelative },
			// bra $000000
			new OpLookup{Op = "bra", Byte = "80", Mode = AddressingMode.PCRelative24 },
			// brk
			new OpLookup{Op = "brk", Byte = "00", Mode = AddressingMode.ImmediateSignature },
			// brl $0000
			new OpLookup{Op = "brl", Byte = "82", Mode = AddressingMode.PCRelativeLong },
			// brl $000000
			new OpLookup{Op = "brl", Byte = "82", Mode = AddressingMode.PCRelativeLong24 },
			// bvc $0000
			new OpLookup{Op = "bvc", Byte = "50", Mode = AddressingMode.PCRelative },
			// bvc $000000
			new OpLookup{Op = "bvc", Byte = "50", Mode = AddressingMode.PCRelative24 },
			// bvs $0000
			new OpLookup{Op = "bvs", Byte = "70", Mode = AddressingMode.PCRelative },
			// bvs $000000
			new OpLookup{Op = "bvs", Byte = "70", Mode = AddressingMode.PCRelative24 },
			// clc
			new OpLookup{Op = "clc", Byte = "18", Mode = AddressingMode.Implied },
			// cld
			new OpLookup{Op = "cld", Byte = "d8", Mode = AddressingMode.Implied },
			// cli
			new OpLookup{Op = "cli", Byte = "58", Mode = AddressingMode.Implied },
			// clv
			new OpLookup{Op = "clv", Byte = "b8", Mode = AddressingMode.Implied },
			// cmp ($00,x)
			new OpLookup{Op = "cmp", Byte = "c1", Mode = AddressingMode.DPIndexedIndirectX },
			// cmp $00,s
			new OpLookup{Op = "cmp", Byte = "c3", Mode = AddressingMode.StackRelative },
			// cmp $00
			new OpLookup{Op = "cmp", Byte = "c5", Mode = AddressingMode.DirectPage },
			// cmp [$00]
			new OpLookup{Op = "cmp", Byte = "c7", Mode = AddressingMode.DPIndirectLong },
			// cmp #$00
			// cmp #$0000
			new OpLookup{Op = "cmp", Byte = "c9", Mode = AddressingMode.Immediate },
			// cmp $0000
			new OpLookup{Op = "cmp", Byte = "cd", Mode = AddressingMode.Absolute },
			// cmp $000000
			new OpLookup{Op = "cmp", Byte = "cf", Mode = AddressingMode.AbsoluteLong },
			// cmp ($00),y
			new OpLookup{Op = "cmp", Byte = "d1", Mode = AddressingMode.DPIndirectIndexedY },
			// cmp ($00)
			new OpLookup{Op = "cmp", Byte = "d2", Mode = AddressingMode.DPIndirect },
			// cmp ($00,s),y
			new OpLookup{Op = "cmp", Byte = "d3", Mode = AddressingMode.SRIndirectIndexedY },
			// cmp $00,x
			new OpLookup{Op = "cmp", Byte = "d5", Mode = AddressingMode.DPIndexedX },
			// cmp [$00],y
			new OpLookup{Op = "cmp", Byte = "d7", Mode = AddressingMode.DPIndirectLongIndexedY },
			// cmp $0000,y
			new OpLookup{Op = "cmp", Byte = "d9", Mode = AddressingMode.AbsoluteIndexedY },
			// cmp $0000,x
			new OpLookup{Op = "cmp", Byte = "dd", Mode = AddressingMode.AbsoluteIndexedX },
			// cmp $000000,x
			new OpLookup{Op = "cmp", Byte = "df", Mode = AddressingMode.AbsoluteLongIndexedX },
			// cop #$00
			new OpLookup{Op = "cop", Byte = "02", Mode = AddressingMode.ImmediateSignature },
			// cpx #$00
			// cpx #$0000
			new OpLookup{Op = "cpx", Byte = "e0", Mode = AddressingMode.Immediate },
			// cpx $00
			new OpLookup{Op = "cpx", Byte = "e4", Mode = AddressingMode.DirectPage },
			// cpx $0000
			new OpLookup{Op = "cpx", Byte = "ec", Mode = AddressingMode.Absolute },
			// cpy #$00
			// cpy #$0000
			new OpLookup{Op = "cpy", Byte = "c0", Mode = AddressingMode.Immediate },
			// cpy $00
			new OpLookup{Op = "cpy", Byte = "c4", Mode = AddressingMode.DirectPage },
			// cpy $0000
			new OpLookup{Op = "cpy", Byte = "cc", Mode = AddressingMode.Absolute },

			// dec a
			new OpLookup{Op = "dec", Byte = "3a", Mode = AddressingMode.Accumulator },
			// dec
			new OpLookup{Op = "dec", Byte = "3a", Mode = AddressingMode.Implied },

			// dec $00
			new OpLookup{Op = "dec", Byte = "c6", Mode = AddressingMode.DirectPage },
			// dec $0000
			new OpLookup{Op = "dec", Byte = "ce", Mode = AddressingMode.Absolute },
			// dec $00,x
			new OpLookup{Op = "dec", Byte = "d6", Mode = AddressingMode.DPIndexedX },
			// dec $0000,x
			new OpLookup{Op = "dec", Byte = "de", Mode = AddressingMode.AbsoluteIndexedX },
			// dex
			new OpLookup{Op = "dex", Byte = "ca", Mode = AddressingMode.Implied },
			// dey
			new OpLookup{Op = "dey", Byte = "88", Mode = AddressingMode.Implied },
			// eor ($00,x)
			new OpLookup{Op = "eor", Byte = "41", Mode = AddressingMode.DPIndexedIndirectX },
			// eor $00,s
			new OpLookup{Op = "eor", Byte = "43", Mode = AddressingMode.StackRelative },
			// eor $00
			new OpLookup{Op = "eor", Byte = "45", Mode = AddressingMode.DirectPage },
			// eor [$00]
			new OpLookup{Op = "eor", Byte = "47", Mode = AddressingMode.DPIndirectLong },
			// eor #$00
			// eor #$0000
			new OpLookup{Op = "eor", Byte = "49", Mode = AddressingMode.Immediate },
			// eor $0000
			new OpLookup{Op = "eor", Byte = "4d", Mode = AddressingMode.Absolute },
			// eor $000000
			new OpLookup{Op = "eor", Byte = "4f", Mode = AddressingMode.AbsoluteLong },
			// eor ($00),y
			new OpLookup{Op = "eor", Byte = "51", Mode = AddressingMode.DPIndirectIndexedY },
			// eor ($00)
			new OpLookup{Op = "eor", Byte = "52", Mode = AddressingMode.DPIndirect },
			// eor ($00,s),y
			new OpLookup{Op = "eor", Byte = "53", Mode = AddressingMode.SRIndirectIndexedY },
			// eor $00,x
			new OpLookup{Op = "eor", Byte = "55", Mode = AddressingMode.DPIndexedX },
			// eor [$00],y
			new OpLookup{Op = "eor", Byte = "57", Mode = AddressingMode.DPIndirectLongIndexedY },
			// eor $0000,y
			new OpLookup{Op = "eor", Byte = "59", Mode = AddressingMode.AbsoluteIndexedY },
			// eor $0000,x
			new OpLookup{Op = "eor", Byte = "5d", Mode = AddressingMode.AbsoluteIndexedX },
			// eor $000000,x
			new OpLookup{Op = "eor", Byte = "5f", Mode = AddressingMode.AbsoluteLongIndexedX },

			// inc a
			new OpLookup{Op = "inc", Byte = "1a", Mode = AddressingMode.Accumulator },
			// inc
			new OpLookup{Op = "inc", Byte = "1a", Mode = AddressingMode.Implied },

			// inc $00
			new OpLookup{Op = "inc", Byte = "e6", Mode = AddressingMode.DirectPage },
			// inc $0000
			new OpLookup{Op = "inc", Byte = "ee", Mode = AddressingMode.Absolute },
			// inc $00,x
			new OpLookup{Op = "inc", Byte = "f6", Mode = AddressingMode.DPIndexedX },
			// inc $0000,x
			new OpLookup{Op = "inc", Byte = "fe", Mode = AddressingMode.AbsoluteIndexedX },
			// inx
			new OpLookup{Op = "inx", Byte = "e8", Mode = AddressingMode.Implied },
			// iny
			new OpLookup{Op = "iny", Byte = "c8", Mode = AddressingMode.Implied },
			// jmp $0000
			new OpLookup{Op = "jmp", Byte = "4c", Mode = AddressingMode.Absolute },

			// jmp $000000
			new OpLookup{Op = "jmp", Byte = "5c", Mode = AddressingMode.AbsoluteLong },
			// jml $000000
			new OpLookup{Op = "jml", Byte = "5c", Mode = AddressingMode.AbsoluteLong },

			// jmp ($0000)
			new OpLookup{Op = "jmp", Byte = "6c", Mode = AddressingMode.AbsoluteIndirect },
			// jmp ($0000,x)
			new OpLookup{Op = "jmp", Byte = "7c", Mode = AddressingMode.AbsoluteIndexedIndirectX },
			// jmp [$0000]
			new OpLookup{Op = "jml", Byte = "dc", Mode = AddressingMode.AbsoluteIndirectLong },
			// jsr $0000
			new OpLookup{Op = "jsr", Byte = "20", Mode = AddressingMode.Absolute },

			// jsr $000000
			new OpLookup{Op = "jsr", Byte = "22", Mode = AddressingMode.AbsoluteLong },
			// jsl $000000
			new OpLookup{Op = "jsl", Byte = "22", Mode = AddressingMode.AbsoluteLong },

			// jsr ($0000,x)
			new OpLookup{Op = "jsr", Byte = "fc", Mode = AddressingMode.AbsoluteIndexedIndirectX },
			// lda ($00,x)
			new OpLookup{Op = "lda", Byte = "a1", Mode = AddressingMode.DPIndexedIndirectX },
			// lda $00,s
			new OpLookup{Op = "lda", Byte = "a3", Mode = AddressingMode.StackRelative },
			// lda $00
			new OpLookup{Op = "lda", Byte = "a5", Mode = AddressingMode.DirectPage },
			// lda [$00]
			new OpLookup{Op = "lda", Byte = "a7", Mode = AddressingMode.DPIndirectLong },
			// lda #$00
			// lda #$0000
			new OpLookup{Op = "lda", Byte = "a9", Mode = AddressingMode.Immediate },
			// lda $0000
			new OpLookup{Op = "lda", Byte = "ad", Mode = AddressingMode.Absolute },
			// lda $000000
			new OpLookup{Op = "lda", Byte = "af", Mode = AddressingMode.AbsoluteLong },
			// lda ($00),y
			new OpLookup{Op = "lda", Byte = "b1", Mode = AddressingMode.DPIndirectIndexedY },
			// lda ($00)
			new OpLookup{Op = "lda", Byte = "b2", Mode = AddressingMode.DPIndirect },
			// lda (sr,s),y
			new OpLookup{Op = "lda", Byte = "b3", Mode = AddressingMode.SRIndirectIndexedY },
			// lda $00,x
			new OpLookup{Op = "lda", Byte = "b5", Mode = AddressingMode.DPIndexedX },
			// lda [$00],y
			new OpLookup{Op = "lda", Byte = "b7", Mode = AddressingMode.DPIndirectLongIndexedY },
			// lda $0000,y
			new OpLookup{Op = "lda", Byte = "b9", Mode = AddressingMode.AbsoluteIndexedY },
			// lda $0000,x
			new OpLookup{Op = "lda", Byte = "bd", Mode = AddressingMode.AbsoluteIndexedX },
			// lda $000000,x
			new OpLookup{Op = "lda", Byte = "bf", Mode = AddressingMode.AbsoluteLongIndexedX },
			// ldx #$00
			// ldx #$0000
			new OpLookup{Op = "ldx", Byte = "a2", Mode = AddressingMode.Immediate },
			// ldx $00
			new OpLookup{Op = "ldx", Byte = "a6", Mode = AddressingMode.DirectPage },
			// ldx $0000
			new OpLookup{Op = "ldx", Byte = "ae", Mode = AddressingMode.Absolute },
			// ldx $00,y
			new OpLookup{Op = "ldx", Byte = "b6", Mode = AddressingMode.DPIndexedY },
			// ldx $0000,y
			new OpLookup{Op = "ldx", Byte = "be", Mode = AddressingMode.AbsoluteIndexedY },
			// ldy #$00
			// ldy #$0000
			new OpLookup{Op = "ldy", Byte = "a0", Mode = AddressingMode.Immediate },
			// ldy $00
			new OpLookup{Op = "ldy", Byte = "a4", Mode = AddressingMode.DirectPage },
			// ldy $0000
			new OpLookup{Op = "ldy", Byte = "ac", Mode = AddressingMode.Absolute },
			// ldy $00,x
			new OpLookup{Op = "ldy", Byte = "b4", Mode = AddressingMode.DPIndexedX },
			// ldy $0000,x
			new OpLookup{Op = "ldy", Byte = "bc", Mode = AddressingMode.AbsoluteIndexedX },
			// lsr $00
			new OpLookup{Op = "lsr", Byte = "46", Mode = AddressingMode.DirectPage },

			// lsr a
			new OpLookup{Op = "lsr", Byte = "4a", Mode = AddressingMode.Accumulator },
			// lsr
			new OpLookup{Op = "lsr", Byte = "4a", Mode = AddressingMode.Implied },

			// lsr $0000
			new OpLookup{Op = "lsr", Byte = "4e", Mode = AddressingMode.Absolute },
			// lsr $00,x
			new OpLookup{Op = "lsr", Byte = "56", Mode = AddressingMode.DPIndexedX },
			// lsr $0000,x
			new OpLookup{Op = "lsr", Byte = "5e", Mode = AddressingMode.AbsoluteIndexedX },

			// mvn $00,$00
			new OpLookup{Op = "mvn", Byte = "54", Mode = AddressingMode.BlockMove },
			// mvn $00 -> $00
			new OpLookup{Op = "mvn", Byte = "54", Mode = AddressingMode.BlockMoveArrow },

			// mvp $00,$00
			new OpLookup{Op = "mvp", Byte = "44", Mode = AddressingMode.BlockMove },
			// mvp $00 -> $00
			new OpLookup{Op = "mvp", Byte = "44", Mode = AddressingMode.BlockMoveArrow },

			// nop
			new OpLookup{Op = "nop", Byte = "ea", Mode = AddressingMode.Implied },
			// ora ($00,x)
			new OpLookup{Op = "ora", Byte = "01", Mode = AddressingMode.DPIndexedIndirectX },
			// ora $00,s
			new OpLookup{Op = "ora", Byte = "03", Mode = AddressingMode.StackRelative },
			// ora $00
			new OpLookup{Op = "ora", Byte = "05", Mode = AddressingMode.DirectPage },
			// ora [$00]
			new OpLookup{Op = "ora", Byte = "07", Mode = AddressingMode.DPIndirectLong },
			// ora #$00
			// ora #$0000
			new OpLookup{Op = "ora", Byte = "09", Mode = AddressingMode.Immediate },
			// ora $0000
			new OpLookup{Op = "ora", Byte = "0d", Mode = AddressingMode.Absolute },
			// ora $000000
			new OpLookup{Op = "ora", Byte = "0f", Mode = AddressingMode.AbsoluteLong },
			// ora ($00),y
			new OpLookup{Op = "ora", Byte = "11", Mode = AddressingMode.DPIndirectIndexedY },
			// ora ($00)
			new OpLookup{Op = "ora", Byte = "12", Mode = AddressingMode.DPIndirect },
			// ora ($00,s),y
			new OpLookup{Op = "ora", Byte = "13", Mode = AddressingMode.SRIndirectIndexedY },
			// ora $00,x
			new OpLookup{Op = "ora", Byte = "15", Mode = AddressingMode.DPIndexedX },
			// ora [$00],y
			new OpLookup{Op = "ora", Byte = "17", Mode = AddressingMode.DPIndirectLongIndexedY },
			// ora $0000,y
			new OpLookup{Op = "ora", Byte = "19", Mode = AddressingMode.AbsoluteIndexedY },
			// ora $0000,x
			new OpLookup{Op = "ora", Byte = "1d", Mode = AddressingMode.AbsoluteIndexedX },
			// ora $000000,x
			new OpLookup{Op = "ora", Byte = "1f", Mode = AddressingMode.AbsoluteLongIndexedX },

			// pea $0000
			new OpLookup{Op = "pea", Byte = "f4", Mode = AddressingMode.Absolute },
			// pea #$0000
			new OpLookup{Op = "pea", Byte = "f4", Mode = AddressingMode.Immediate },
			
			// pei ($00)
			new OpLookup{Op = "pei", Byte = "d4", Mode = AddressingMode.DPIndirect },
			// pei $00
			new OpLookup{Op = "pei", Byte = "d4", Mode = AddressingMode.DirectPage },

			// per $0000
			new OpLookup{Op = "per", Byte = "62", Mode = AddressingMode.PCRelativeLong },
			// per $000000
			new OpLookup{Op = "per", Byte = "62", Mode = AddressingMode.PCRelativeLong24 },
			// pha
			new OpLookup{Op = "pha", Byte = "48", Mode = AddressingMode.Implied },
			// phb
			new OpLookup{Op = "phb", Byte = "8b", Mode = AddressingMode.Implied },
			// phd
			new OpLookup{Op = "phd", Byte = "0b", Mode = AddressingMode.Implied },
			// phk
			new OpLookup{Op = "phk", Byte = "4b", Mode = AddressingMode.Implied },
			// php
			new OpLookup{Op = "php", Byte = "08", Mode = AddressingMode.Implied },
			// phx
			new OpLookup{Op = "phx", Byte = "da", Mode = AddressingMode.Implied },
			// phy
			new OpLookup{Op = "phy", Byte = "5a", Mode = AddressingMode.Implied },
			// pla
			new OpLookup{Op = "pla", Byte = "68", Mode = AddressingMode.Implied },
			// plb
			new OpLookup{Op = "plb", Byte = "ab", Mode = AddressingMode.Implied },
			// pld
			new OpLookup{Op = "pld", Byte = "2b", Mode = AddressingMode.Implied },
			// plp
			new OpLookup{Op = "plp", Byte = "28", Mode = AddressingMode.Implied },
			// plx
			new OpLookup{Op = "plx", Byte = "fa", Mode = AddressingMode.Implied },
			// ply
			new OpLookup{Op = "ply", Byte = "7a", Mode = AddressingMode.Implied },
			// rep #$00
			new OpLookup{Op = "rep", Byte = "c2", Mode = AddressingMode.Immediate },
			// rol $00
			new OpLookup{Op = "rol", Byte = "26", Mode = AddressingMode.DirectPage },
			// rol a
			new OpLookup{Op = "rol", Byte = "2a", Mode = AddressingMode.Accumulator },
			// rol $0000
			new OpLookup{Op = "rol", Byte = "2e", Mode = AddressingMode.Absolute },
			// rol $00,X
			new OpLookup{Op = "rol", Byte = "36", Mode = AddressingMode.DPIndexedX },
			// rol $0000,x
			new OpLookup{Op = "rol", Byte = "3e", Mode = AddressingMode.AbsoluteIndexedX },
			// ror $00
			new OpLookup{Op = "ror", Byte = "66", Mode = AddressingMode.DirectPage },
			// ror a
			new OpLookup{Op = "ror", Byte = "6a", Mode = AddressingMode.Accumulator },
			// ror $0000
			new OpLookup{Op = "ror", Byte = "6e", Mode = AddressingMode.Absolute },
			// ror $00,x
			new OpLookup{Op = "ror", Byte = "76", Mode = AddressingMode.DPIndexedX },
			// ror $0000,x
			new OpLookup{Op = "ror", Byte = "7e", Mode = AddressingMode.AbsoluteIndexedX },
			// rti
			new OpLookup{Op = "rti", Byte = "40", Mode = AddressingMode.Implied },
			// rtl
			new OpLookup{Op = "rtl", Byte = "6b", Mode = AddressingMode.Implied },
			// rts
			new OpLookup{Op = "rts", Byte = "60", Mode = AddressingMode.Implied },
			// sbc ($00,x)
			new OpLookup{Op = "sbc", Byte = "e1", Mode = AddressingMode.DPIndexedIndirectX },
			// sbc $00,s
			new OpLookup{Op = "sbc", Byte = "e3", Mode = AddressingMode.StackRelative },
			// sbc $00
			new OpLookup{Op = "sbc", Byte = "e5", Mode = AddressingMode.DirectPage },
			// sbc [$00]
			new OpLookup{Op = "sbc", Byte = "e7", Mode = AddressingMode.DPIndirectLong },
			// sbc #$00
			// sbc #$0000
			new OpLookup{Op = "sbc", Byte = "e9", Mode = AddressingMode.Immediate },
			// sbc $0000
			new OpLookup{Op = "sbc", Byte = "ed", Mode = AddressingMode.Absolute },
			// sbc $000000
			new OpLookup{Op = "sbc", Byte = "ef", Mode = AddressingMode.AbsoluteLong },
			// sbc ($00),y
			new OpLookup{Op = "sbc", Byte = "f1", Mode = AddressingMode.DPIndirectIndexedY },
			// sbc ($00)
			new OpLookup{Op = "sbc", Byte = "f2", Mode = AddressingMode.DPIndirect },
			// sbc ($00,s),y
			new OpLookup{Op = "sbc", Byte = "f3", Mode = AddressingMode.SRIndirectIndexedY },
			// sbc $00,x
			new OpLookup{Op = "sbc", Byte = "f5", Mode = AddressingMode.DPIndexedX },
			// sbc [$00],y
			new OpLookup{Op = "sbc", Byte = "f7", Mode = AddressingMode.DPIndirectLongIndexedY },
			// sbc $0000,y
			new OpLookup{Op = "sbc", Byte = "f9", Mode = AddressingMode.AbsoluteIndexedY },
			// sbc $0000,x
			new OpLookup{Op = "sbc", Byte = "fd", Mode = AddressingMode.AbsoluteIndexedX },
			// sbc $000000,x
			new OpLookup{Op = "sbc", Byte = "ff", Mode = AddressingMode.AbsoluteLongIndexedX },
			// sec
			new OpLookup{Op = "sec", Byte = "38", Mode = AddressingMode.Implied },
			// sed
			new OpLookup{Op = "sed", Byte = "f8", Mode = AddressingMode.Implied },
			// sei
			new OpLookup{Op = "sei", Byte = "78", Mode = AddressingMode.Implied },
			// sep #$00
			new OpLookup{Op = "sep", Byte = "e2", Mode = AddressingMode.Immediate },
			// sta ($00,x)
			new OpLookup{Op = "sta", Byte = "81", Mode = AddressingMode.DPIndexedIndirectX },
			// sta $00,s
			new OpLookup{Op = "sta", Byte = "83", Mode = AddressingMode.StackRelative },
			// sta $00
			new OpLookup{Op = "sta", Byte = "85", Mode = AddressingMode.DirectPage },
			// sta [$00]
			new OpLookup{Op = "sta", Byte = "87", Mode = AddressingMode.DPIndirectLong },
			// sta $0000
			new OpLookup{Op = "sta", Byte = "8d", Mode = AddressingMode.Absolute },
			// sta $000000
			new OpLookup{Op = "sta", Byte = "8f", Mode = AddressingMode.AbsoluteLong },
			// sta ($00),y
			new OpLookup{Op = "sta", Byte = "91", Mode = AddressingMode.DPIndirectIndexedY },
			// sta ($00)
			new OpLookup{Op = "sta", Byte = "92", Mode = AddressingMode.DPIndirect },
			// sta ($00,s),y
			new OpLookup{Op = "sta", Byte = "93", Mode = AddressingMode.SRIndirectIndexedY },
			// sta $00,x
			new OpLookup{Op = "sta", Byte = "95", Mode = AddressingMode.DPIndexedX },
			// sta [$00],y
			new OpLookup{Op = "sta", Byte = "97", Mode = AddressingMode.DPIndirectLongIndexedY },
			// sta $0000,y
			new OpLookup{Op = "sta", Byte = "99", Mode = AddressingMode.AbsoluteIndexedY },
			// sta $0000,x
			new OpLookup{Op = "sta", Byte = "9d", Mode = AddressingMode.AbsoluteIndexedX },
			// sta $000000,x
			new OpLookup{Op = "sta", Byte = "9f", Mode = AddressingMode.AbsoluteLongIndexedX },
			// stp
			new OpLookup{Op = "stp", Byte = "db", Mode = AddressingMode.Implied },	
			// stx $00
			new OpLookup{Op = "stx", Byte = "86", Mode = AddressingMode.DirectPage },
			// stx $0000
			new OpLookup{Op = "stx", Byte = "8e", Mode = AddressingMode.Absolute },
			// stx $00,y
			new OpLookup{Op = "stx", Byte = "96", Mode = AddressingMode.DPIndexedY },
			// sty $00
			new OpLookup{Op = "sty", Byte = "84", Mode = AddressingMode.DirectPage },
			// sty $0000
			new OpLookup{Op = "sty", Byte = "8c", Mode = AddressingMode.Absolute },
			// sty $00,x
			new OpLookup{Op = "sty", Byte = "94", Mode = AddressingMode.DPIndexedX },
			// stz $00
			new OpLookup{Op = "stz", Byte = "64", Mode = AddressingMode.DirectPage },
			// stz $00,x
			new OpLookup{Op = "stz", Byte = "74", Mode = AddressingMode.DPIndexedX },
			// stz $0000
			new OpLookup{Op = "stz", Byte = "9c", Mode = AddressingMode.Absolute },
			// stz $0000,x
			new OpLookup{Op = "stz", Byte = "9e", Mode = AddressingMode.AbsoluteIndexedX },
			// tax
			new OpLookup{Op = "tax", Byte = "aa", Mode = AddressingMode.Implied },
			// tay
			new OpLookup{Op = "tay", Byte = "a8", Mode = AddressingMode.Implied },
			// tcd
			new OpLookup{Op = "tcd", Byte = "5b", Mode = AddressingMode.Implied },
			// tcs
			new OpLookup{Op = "tcs", Byte = "1b", Mode = AddressingMode.Implied },
			// tdc
			new OpLookup{Op = "tdc", Byte = "7b", Mode = AddressingMode.Implied },
			// trb $00
			new OpLookup{Op = "trb", Byte = "14", Mode = AddressingMode.DirectPage },
			// trb $0000
			new OpLookup{Op = "trb", Byte = "1c", Mode = AddressingMode.Absolute },
			// tsb $00
			new OpLookup{Op = "tsb", Byte = "04", Mode = AddressingMode.DirectPage },
			// tsb $0000
			new OpLookup{Op = "tsb", Byte = "0c", Mode = AddressingMode.Absolute },
			// tsc
			new OpLookup{Op = "tsc", Byte = "3b", Mode = AddressingMode.Implied },
			// tsx
			new OpLookup{Op = "tsx", Byte = "ba", Mode = AddressingMode.Implied },
			// txa
			new OpLookup{Op = "txa", Byte = "8a", Mode = AddressingMode.Implied },
			// txs
			new OpLookup{Op = "txs", Byte = "9a", Mode = AddressingMode.Implied },
			// txy
			new OpLookup{Op = "txy", Byte = "9b", Mode = AddressingMode.Implied },
			// tya
			new OpLookup{Op = "tya", Byte = "98", Mode = AddressingMode.Implied },
			// tyx
			new OpLookup{Op = "tyx", Byte = "bb", Mode = AddressingMode.Implied },
			// wai
			new OpLookup{Op = "wai", Byte = "cb", Mode = AddressingMode.Implied },
/*
			// wdm
			new OpLookup{Op = "wdm", Byte = "42", Mode = AddressingMode. },
reserved opcode: wdm  -- 42 -- 2 bytes
*/
			// xba
			new OpLookup{Op = "xba", Byte = "eb", Mode = AddressingMode.Implied },
			// xce
			new OpLookup{Op = "xce", Byte = "fb", Mode = AddressingMode.Implied },
		};
	}
}
