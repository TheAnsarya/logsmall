using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ3Lib.Text.TableFile;

internal class TableFile : Dictionary<int, TableFileEntry> {
	public string? FileName { get; set; }

	public required Table Table { get; init; }

	public TableFile(string fileName) {
		if (!File.Exists(fileName)) {
			throw new FileNotFoundException($"File not found: {fileName}");
		}
		
		FileName = fileName;

		var lines = File.ReadAllLines(fileName);
		Table = new(lines);
	}
}
