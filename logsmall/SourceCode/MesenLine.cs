using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logsmall.SourceCode {
	class MesenLine : Line {
		public static Regex CreateRegex = new Regex(@"^([0-9A-Z]{6}) ([\$0-9A-Z ]{15}) ([A-Z]{3}) (\S*) (?:\[([0-9A-Z]{6})\] = \$([0-9A-Z]+))?\s+A:([0-9A-Z]{4}) X:([0-9A-Z]{4}) Y:([0-9A-Z]{4}) S:([0-9A-Z]{4}) D:([0-9A-Z]{4}) DB:([0-9A-Z]{2}) P:([a-zA-Z]{8}|[0-9A-F]{2}) V:([0-9 ]{3}) H:([0-9 ]{3})$", RegexOptions.Compiled);

		public string BytecodeOriginal { get; set; }

		public string Target { get; set; }

		public string TargetValue { get; set; }

		public EmuState State { get; set; }

		public class EmuState {
			public string A { get; set; }

			public string X { get; set; }

			public string Y { get; set; }

			public string S { get; set; }

			public string D { get; set; }

			public string DB { get; set; }

			public ProcessorStatus P { get; set; }

			public int V { get; set; }

			public int H { get; set; }

			public EmuState(Match match) {
				this.A = match.Groups[7].Value.ToLowerInvariant();
				this.X = match.Groups[8].Value.ToLowerInvariant();
				this.Y = match.Groups[9].Value.ToLowerInvariant();
				this.S = match.Groups[10].Value.ToLowerInvariant();
				this.D = match.Groups[11].Value.ToLowerInvariant();
				this.DB = match.Groups[12].Value.ToLowerInvariant();
				this.P = new ProcessorStatus(match.Groups[13].Value);
				this.V = int.Parse(match.Groups[14].Value.Trim());
				this.H = int.Parse(match.Groups[15].Value.Trim());
			}
		}

		public class ProcessorStatus {
			private static Regex IsFlagsString = new Regex("^nvmxdizc$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

			// nvmxdizc
			public bool Negative { get; set; }
			public bool Overflow { get; set; }
			public bool Accumulator { get; set; }
			public bool Indexes { get; set; }
			public bool Decimal { get; set; }
			public bool IRQDisable { get; set; }
			public bool Zero { get; set; }
			public bool Carry { get; set; }

			public string Flags {
				get {
					return
						(Negative ? "N" : "n")
						+ (Overflow ? "N" : "n")
						+ (Accumulator ? "N" : "n")
						+ (Indexes ? "N" : "n")
						+ (Decimal ? "N" : "n")
						+ (IRQDisable ? "N" : "n")
						+ (Zero ? "N" : "n")
						+ (Carry ? "N" : "n");
				}
				set {
					if (value == null) {
						throw new ArgumentNullException(nameof(value));
					}

					if ((value.Length == 8) && IsFlagsString.IsMatch(value)) {
						Negative = value[0] == 'N';
						Overflow = value[1] == 'V';
						Accumulator = value[2] == 'M';
						Indexes = value[3] == 'X';
						Decimal = value[4] == 'D';
						IRQDisable = value[5] == 'I';
						Zero = value[6] == 'Z';
						Carry = value[7] == 'C';
					} else if ((value.Length == 2) && byte.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte flags)) {
						Negative = (flags & 0x80) != 0;
						Overflow = (flags & 0x40) != 0;
						Accumulator = (flags & 0x20) != 0;
						Indexes = (flags & 0x10) != 0;
						Decimal = (flags & 0x08) != 0;
						IRQDisable = (flags & 0x04) != 0;
						Zero = (flags & 0x02) != 0;
						Carry = (flags & 0x01) != 0;
					} else {
						throw new ArgumentException($"{nameof(value)} has invalid value: {value}");
					}
				}
			}

			public ProcessorStatus(string flags) {
				Flags = flags;
			}
		}

		public static bool IsA(string line) => CreateRegex.IsMatch(line);

		public MesenLine(string line) {
			var match = CreateRegex.Match(line);

			this.Address = match.Groups[1].Value.ToLowerInvariant();
			this.BytecodeOriginal = match.Groups[2].Value.ToLowerInvariant().Replace("$", "").Replace(" ", "");
			this.Op = match.Groups[3].Value.ToLowerInvariant();
			this.Parameters = match.Groups[4].Value.ToLowerInvariant();
			this.Target = match.Groups[5].Value.ToLowerInvariant();
			this.TargetValue = match.Groups[6].Value.ToLowerInvariant();
			this.State = new EmuState(match);
		}

		public override Line ToLine() =>
			new Line {
				Address = this.Address,
				Op = this.Op,
				Parameters = this.Parameters
			};

		public static Line MakeLine(string line) {
			var match = CreateRegex.Match(line);
			return new Line {
				Address = match.Groups[1].Value.ToLowerInvariant(),
				Op = match.Groups[3].Value.ToLowerInvariant(),
				Parameters = match.Groups[4].Value.ToLowerInvariant()
			};
		}

		public static List<MesenLine> FromFile(string filename) => ConvertTrace(File.ReadAllLines(filename).ToList());

		public static List<MesenLine> ConvertTrace(List<string> lines) =>
			lines.Where(x => IsA(x))
				.Select(x => new MesenLine(x))
				.ToList();
	}
}
