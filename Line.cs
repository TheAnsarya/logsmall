using MoreLinq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logsmall {
	class Line {
		public string Address { get; set; }
		// TODO: rename which address is what
		public uint AddressRaw { get => uint.Parse(Address, NumberStyles.HexNumber); }
		public string Op { get; set; }
		public string Parameters { get; set; }
		public string Bytecode { get => SNES.OpToHex(this.Address, this.Op, this.Parameters); }
		public int ByteLength { get => Bytecode.Length / 2; }
		public override string ToString() => $"{Address} {Op} {Parameters}".Trim();
		public string ToLongString() => $"{Address} {string.Join(" ", Bytecode.Split(2).Select(x => $"${x}")).PadRight(15)} {Op} {Parameters}".Trim();

		// Simplify from a more complex type
		public virtual Line ToLine() {
			return
				this.GetType() == typeof(Line)
					? this
					: new Line { Address = this.Address, Op = this.Op, Parameters = this.Parameters };
		}

		public static IOrderedEnumerable<Line> ToLines(IEnumerable<Line> lines) {
			return
				lines
					.Select(x => x.ToLine())
					.DistinctBy(x => x.Address)
					.OrderBy(x => x.Address);
		}

		protected static Regex IsBranchRegex = new Regex(@"^(bcc|bcs|beq|bmi|bne|bpl|bra|bvc|bvs|brl)$", RegexOptions.Compiled);
		public bool IsBranch {
			get {
				return IsBranchRegex.IsMatch(this.Op);
			}
		}

		protected static Regex IsCallRegex = new Regex(@"^(jsr|jsl)$", RegexOptions.Compiled);
		public bool IsCall {
			get {
				return IsCallRegex.IsMatch(this.Op);
			}
		}

		protected static Regex IsJumpRegex = new Regex(@"^(jmp|jml)$", RegexOptions.Compiled);
		public bool IsJump {
			get {
				return IsJumpRegex.IsMatch(this.Op);
			}
		}
	}
}
