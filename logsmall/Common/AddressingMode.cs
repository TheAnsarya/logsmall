using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logsmall.Common {
	class AddressingMode {
		// adc $0000
		public static Regex Absolute = new Regex(@"^\$([0-9a-z]{4})$", RegexOptions.Compiled);
		// jmp ($0000,x)
		public static Regex AbsoluteIndexedIndirectX = new Regex(@"^\(\$([0-9a-z]{4}),x\)$", RegexOptions.Compiled);
		// adc $0000,x
		public static Regex AbsoluteIndexedX = new Regex(@"^\$([0-9a-z]{4}),x$", RegexOptions.Compiled);
		// adc $0000,y
		public static Regex AbsoluteIndexedY = new Regex(@"^\$([0-9a-z]{4}),y$", RegexOptions.Compiled);
		// jmp ($0000)
		public static Regex AbsoluteIndirect = new Regex(@"^\(\$([0-9a-z]{4})\)$", RegexOptions.Compiled);
		// jmp [$0000]
		public static Regex AbsoluteIndirectLong = new Regex(@"^\[\$([0-9a-z]{4})\]$", RegexOptions.Compiled);
		// adc $000000
		public static Regex AbsoluteLong = new Regex(@"^\$([0-9a-z]{6})$", RegexOptions.Compiled);
		// adc $000000,y
		public static Regex AbsoluteLongIndexedX = new Regex(@"^\$([0-9a-z]{6}),x$", RegexOptions.Compiled);
		// ror a
		//public static Regex Accumulator = new Regex(@"^a$", RegexOptions.Compiled);
		// Mesen is using "rol" instead of "rol a"
		public static Regex Accumulator = new Regex(@"^$", RegexOptions.Compiled);
		// adc $00
		public static Regex DirectPage = new Regex(@"^\$([0-9a-z]{2})$", RegexOptions.Compiled);
		// adc ($00,x)
		public static Regex DPIndexedIndirectX = new Regex(@"^\(\$([0-9a-z]{2}),x\)$", RegexOptions.Compiled);
		// adc $00,x
		public static Regex DPIndexedX = new Regex(@"^\$([0-9a-z]{2}),x$", RegexOptions.Compiled);
		// stx $00,y
		public static Regex DPIndexedY = new Regex(@"^\$([0-9a-z]{2}),y$", RegexOptions.Compiled);
		// adc ($00)
		public static Regex DPIndirect = new Regex(@"^\(\$([0-9a-z]{2})\)$", RegexOptions.Compiled);
		// adc ($00),y
		public static Regex DPIndirectIndexedY = new Regex(@"^\(\$([0-9a-z]{2})\),y$", RegexOptions.Compiled);
		// adc [$00]
		public static Regex DPIndirectLong = new Regex(@"^\[\$([0-9a-z]{2})\]$", RegexOptions.Compiled);
		// adc [$00],y
		public static Regex DPIndirectLongIndexedY = new Regex(@"^\[\$([0-9a-z]{2})\],y$", RegexOptions.Compiled);
		// adc #$00
		// adc #$0000
		public static Regex Immediate = new Regex(@"^#\$([0-9a-z]{2}|[0-9a-z]{4})$", RegexOptions.Compiled);
		// cop #$00
		public static Regex ImmediateSignature = new Regex(@"^#\$([0-9a-z]{2})$", RegexOptions.Compiled);
		// php
		public static Regex Implied = new Regex(@"^$", RegexOptions.Compiled);
		// adc $00,s
		public static Regex StackRelative = new Regex(@"^\$([0-9a-z]{2}),s$", RegexOptions.Compiled);
		// adc ($00,s),y
		public static Regex SRIndirectIndexedY = new Regex(@"^\(\$([0-9a-z]{2}),s\),y$", RegexOptions.Compiled);

		// Special

		// bra $0000
		public static Regex PCRelative = new Regex(@"^\$([0-9a-z]{4})$", RegexOptions.Compiled);
		// per $0000
		public static Regex PCRelativeLong = new Regex(@"^\$([0-9a-z]{4})$", RegexOptions.Compiled);
		// beq $000000
		public static Regex PCRelative24 = new Regex(@"^\$([0-9a-z]{6})$", RegexOptions.Compiled);
		// brl $000000
		public static Regex PCRelativeLong24 = new Regex(@"^\$([0-9a-z]{6})$", RegexOptions.Compiled);
		// mvn $00,$00
		public static Regex BlockMove = new Regex(@"^\$([0-9a-z]{2}),\$([0-9a-z]{2})$", RegexOptions.Compiled);
		// mvn $08 -> $7f
		public static Regex BlockMoveArrow = new Regex(@"^\$([0-9a-z]{2}) -> \$([0-9a-z]{2})$", RegexOptions.Compiled);
	}
}
