using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Text {
	internal class DialogTableFile {
		// Examples of lines in the table file. Last line is blank:
		// 0201=愛
		// 00AB=[CODE AB]
		// Commands start with 00, characters start with 02-06
		// 00AC is <end of string>

		public string? OriginalFileName { get; set; }

		public Dictionary<int, TableFileEntry> Entries { get; init; }

		public DialogTableFile() {
			Entries = [];
		}

		public DialogTableFile(string fileName) {
			var lines = File.ReadAllLines(fileName);
			Entries = DictionaryFromLines(lines);
		}

		public DialogTableFile(string[] lines) {
			Entries = DictionaryFromLines(lines);
		}

		private static Dictionary<int, TableFileEntry> DictionaryFromLines(string[] lines) {
			return lines
				.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(x => new TableFileEntry(int.Parse(x[..4], System.Globalization.NumberStyles.HexNumber), x[..4], x[5..]))
				.ToDictionary(x => x.Key, y => y);
		}
	}
}
