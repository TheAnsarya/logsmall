using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logsmall {
	class MesenLine : Line, ILine {
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
				//this.P = new ProcessorStatus(match.Groups[13].Value);
				this.V = int.Parse(match.Groups[14].Value.Trim());
				this.H = int.Parse(match.Groups[15].Value.Trim());
			}
		}

		public class ProcessorStatus {
			public string Flags;

			public ProcessorStatus(string flags) {
				this.Flags = flags;
			}

			//nvmxdIzc
			public bool Negative { get { return Flags[0] == 'N'; } }
			public bool Overflow { get { return Flags[1] == 'V'; } }
			public bool Accumulator { get { return Flags[2] == 'M'; } }
			public bool Indexes { get { return Flags[3] == 'X'; } }
			public bool Decimal { get { return Flags[4] == 'D'; } }
			public bool IRQDisable { get { return Flags[5] == 'I'; } }
			public bool Zero { get { return Flags[6] == 'Z'; } }
			public bool Carry { get { return Flags[7] == 'C'; } }
		}

		public static bool IsA(string line) {
			return CreateRegex.IsMatch(line);
		}

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

		public override ILine ToLine() {
			return new Line {
				Address = this.Address,
				Op = this.Op,
				Parameters = this.Parameters
			};
		}

		public static ILine MakeLine(string line) {
			var match = CreateRegex.Match(line);
			return new Line {
				Address = match.Groups[1].Value.ToLowerInvariant(),
				Op = match.Groups[3].Value.ToLowerInvariant(),
				Parameters = match.Groups[4].Value.ToLowerInvariant()
			};
		}

		public static List<MesenLine> FromFile(string filename) {
			var rawlines = File.ReadAllLines(filename);
			
			var lines =
				rawlines
					.Where(x => IsA(x))
					.Select(x => new MesenLine(x))
					.ToList();
			return lines;
		}
	}
}
