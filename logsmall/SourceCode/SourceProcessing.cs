using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logsmall.SourceCode {
	class SourceProcessing {

		public static List<string> GetMissingOutput(List<LineGroup> groups, Func<Line, string> lineToString) {
			var missing = new List<string>();
			LineGroup lastGroup = null;

			foreach (var group in groups) {
				if (lastGroup != null) {
					missing.AddRange(new List<string> {
						"",
						$"; Missing {lastGroup.NextAddress.ToString("x6", CultureInfo.InvariantCulture)} - {(group.StartAddress - 1).ToString("x6", CultureInfo.InvariantCulture)} ({lastGroup.BytesBetween(group)} bytes)",
						""
					});
				}

				missing.AddRange(
					group
						.Lines
						.Select(lineToString) //(x) => x.ToString())
						.ToList()
				);

				lastGroup = group;
			}

			return missing;
		}

		public static List<string> GetBadBytecodeConversions(IOrderedEnumerable<Line> lines) {
			var bytecodes =
				lines
					.Where(x => x.AddressRaw >= 0xC00000U)
					.Select(x =>
						new {
							Line = x,
							Converted = x.Bytecode,
							Rom = Rom.GetString(x.AddressRaw, x.ByteLength)
						}
					)
					.Where(x => x.Converted != x.Rom)
					.Select(x => $"{x.Line.ToString()}   rom={x.Rom}  converted={x.Converted}")
					.ToList();
			return bytecodes;
		}

		public static List<string> GetAllRaw(IOrderedEnumerable<Line> lines, Func<Line, string> lineToString) {
			var allraw =
				lines
					.Select(lineToString)
					.ToList();
			return allraw;
		}

		public static List<string> LabelCode(List<string> lines) {
			var branchRegex = new Regex(@"^([a-f0-9]{2})([a-f0-9]{4}) (bcc|bcs|beq|bmi|bne|bpl|bra|bvc|bvs) \$([a-f0-9]{4}|[a-f0-9]{6})$", RegexOptions.Compiled);
			var branches =
				lines.Where(x => branchRegex.IsMatch(x))
				.Select(x => branchRegex.Match(x))
				.Select(x => new {
					line = x.Value,
					address = $"{x.Groups[1].Value}{x.Groups[2].Value}",
					target = x.Groups[4].Value.Length == 4 ? $"{x.Groups[1].Value}{x.Groups[4].Value}" : x.Groups[4].Value,
					op = x.Groups[3].Value
				})
				.Select(
					x => new LabelMatch {
						line = x.line,
						newline = $"{x.address} {x.op} .Branch_{x.target}",
						label = $".Branch_{x.target}",
						targetaddress = x.target
					}
				);

			var longBranchRegex = new Regex(@"^([a-f0-9]{6}) brl \$([a-f0-9]{6})$", RegexOptions.Compiled);
			var longBranches =
				lines.Where(x => longBranchRegex.IsMatch(x))
				.Select(x => longBranchRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = $"{x.Groups[1].Value} brl .Branch_{x.Groups[2].Value}",
						label = $".Branch_{x.Groups[2].Value}",
						targetaddress = x.Groups[2].Value
					}
				);

			var jsrRegex = new Regex(@"^([a-f0-9]{2})([a-f0-9]{4}) jsr \$([a-f0-9]{4}|[a-f0-9]{6})$", RegexOptions.Compiled);
			var jsr =
				lines.Where(x => jsrRegex.IsMatch(x))
				.Select(x => jsrRegex.Match(x))
				.Select(x => new {
					line = x.Value,
					address = $"{x.Groups[1].Value}{x.Groups[2].Value}",
					target = x.Groups[3].Value.Length == 4 ? $"{x.Groups[1].Value}{x.Groups[3].Value}" : x.Groups[3].Value
				})
				.Select(
					x => new LabelMatch {
						line = x.line,
						newline = $"{x.address} jsr Routine_{x.target}",
						label = $"Routine_{x.target}:",
						targetaddress = x.target
					}
				);

			var jslRegex = new Regex(@"^([a-f0-9]{6}) jsl \$([a-f0-9]{6})$", RegexOptions.Compiled);
			var jsl =
				lines.Where(x => jslRegex.IsMatch(x))
				.Select(x => jslRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = $"{x.Groups[1].Value} jsl Routine_{x.Groups[2].Value}",
						label = $"Routine_{x.Groups[2].Value}:",
						targetaddress = x.Groups[2].Value
					}
				);

			var jmpRegex = new Regex(@"^([a-f0-9]{2})([a-f0-9]{4}) jmp \$([a-f0-9]{4}|[a-f0-9]{6})$", RegexOptions.Compiled);
			var jmp =
				lines.Where(x => jmpRegex.IsMatch(x))
				.Select(x => jmpRegex.Match(x))
				.Select(x => new {
					line = x.Value,
					address = $"{x.Groups[1].Value}{x.Groups[2].Value}",
					target = x.Groups[3].Value.Length == 4 ? $"{x.Groups[1].Value}{x.Groups[3].Value}" : x.Groups[3].Value
				})
				.Select(
					x => new LabelMatch {
						line = x.line,
						newline = $"{x.address} jmp Jump_{x.target}",
						label = $"Jump_{x.target}:",
						targetaddress = x.target
					}
				);

			var jmlRegex = new Regex(@"^([a-f0-9]{6}) jml \$([a-f0-9]{6})$", RegexOptions.Compiled);
			var jml =
				lines.Where(x => jmlRegex.IsMatch(x))
				.Select(x => jmlRegex.Match(x))
				.Select(
					x => new LabelMatch {
						line = x.Value,
						newline = $"{x.Groups[1].Value} jml Jump_{x.Groups[2].Value}",
						label = $"Jump_{x.Groups[2].Value}:",
						targetaddress = x.Groups[2].Value
					}
				);

			var all = branches.Concat(longBranches).Concat(jsr).Concat(jsl).Concat(jmp).Concat(jml);

			var labels =
				all.GroupBy(x => x.targetaddress)
				.ToDictionary(
					x => x.Key,
					x => x.Select(y => y.label).Distinct()
				);

			var dupekeys =
				all.GroupBy(x => x.line)
				.Where(x => x.Count() > 1);

			var emptylines = all.Count(x => string.IsNullOrEmpty(x.line));
			var oklines = all.Count(x => !string.IsNullOrEmpty(x.line));

			var replacements = all.ToDictionary(x => x.line, x => x.newline);

			var addressRegex = new Regex(@"^([a-f0-9]{6}) ", RegexOptions.Compiled);
			string last = null;

			var output = new List<string>();

			foreach (string line in lines) {
				var match = addressRegex.Match(line);
				var address = match.Success ? match.Groups[1].Value : null;
				if ((address != null) && labels.ContainsKey(address)) {
					output.Add("");
					output.AddRange(labels[address]);
					labels.Remove(address);
				}

				if (replacements.TryGetValue(line, out string newline)) {
					output.Add(newline);
					output.Add("");
					replacements.Remove(line);
				} else {
					output.Add(line);
					if ((line == "rts") || (line == "rtl") || (line == "rti")) {
						output.Add("");
					}
				}

				last = line;
			}

			return output;
		}
	}
}
