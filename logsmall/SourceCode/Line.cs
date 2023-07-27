using logsmall.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logsmall.SourceCode {
	class Line {
		public string Address { get; set; }
		// TODO: rename which address is what
		public uint AddressRaw { get => uint.Parse(Address, NumberStyles.HexNumber, CultureInfo.InvariantCulture); }
		public string Op { get; set; }
		public string Parameters { get; set; }
		public string Bytecode { get => SNES.OpToHex(Address, Op, Parameters); }
		public int ByteLength { get => Bytecode.Length / 2; }
		public override string ToString() => $"{Address} {Op} {Parameters}".Trim();
		public string ToLongString() {
			string v = string.Join(" ", Bytecode.Split(2).Select(x => $"${x}"));
			return $"{Address} {v,-15} {Op} {Parameters}".Trim();
		}

		// Simplify from a more complex type
		public virtual Line ToLine() {
			return
				GetType() == typeof(Line)
					? this
					: new Line { Address = Address, Op = Op, Parameters = Parameters };
		}

		public static IOrderedEnumerable<Line> ToLines(IEnumerable<Line> lines)
		{
			return lines
					.Select(x => x.ToLine())
					.DistinctBy(x => x.Address)
					.OrderBy(x => x.Address);
		}

		protected static Regex IsBranchRegex = new Regex(@"^(bcc|bcs|beq|bmi|bne|bpl|bra|bvc|bvs|brl)$", RegexOptions.Compiled);
		public bool IsBranch {
			get {
				return IsBranchRegex.IsMatch(Op);
			}
		}

		protected static Regex IsCallRegex = new Regex(@"^(jsr|jsl)$", RegexOptions.Compiled);
		public bool IsCall {
			get {
				return IsCallRegex.IsMatch(Op);
			}
		}

		protected static Regex IsJumpRegex = new Regex(@"^(jmp|jml)$", RegexOptions.Compiled);
		public bool IsJump {
			get {
				return IsJumpRegex.IsMatch(Op);
			}
		}
	}
}
