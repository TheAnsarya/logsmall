using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace logsmall.SourceCode {
	class SimpleLine : Line {
		public static Regex CreateRegex = new Regex(@"^([0-9a-z]{6}) (...)(?: (\S*))?$", RegexOptions.Compiled);

		public static bool IsA(string line) {
			return CreateRegex.IsMatch(line);
		}

		public SimpleLine() { }

		public SimpleLine(string line) {
			var match = CreateRegex.Match(line);

			Address = match.Groups[1].Value.ToLowerInvariant();
			Op = match.Groups[2].Value.ToLowerInvariant();
			Parameters = match.Groups[3].Value.ToLowerInvariant();
		}

		public static List<Line> FromFile(string filename) {
			var rawlines = File.ReadAllLines(filename);

			var lines =
				rawlines
					.Where(x => IsA(x))
					.Select(x => (Line)new SimpleLine(x))
					.ToList();
			return lines;
		}
	}
}
