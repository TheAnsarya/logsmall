namespace DW4Lib.Converters;

using System.Text;
using System.Text.Json;

/// <summary>
/// Batch script export and conversion tool for DW4→DQ3r localization.
/// Exports DW4 text and converts it to DQ3r format for use in translations.
/// </summary>
public static class BatchScriptExporter {
	private static readonly JsonSerializerOptions JsonOptions = new() {
		WriteIndented = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};

	/// <summary>
	/// Export configuration.
	/// </summary>
	public class ExportConfig {
		/// <summary>Include raw hex bytes in output.</summary>
		public bool IncludeRawBytes { get; set; } = true;

		/// <summary>Include DQ3r converted bytes.</summary>
		public bool IncludeDQ3rBytes { get; set; } = true;

		/// <summary>Generate separate files per table.</summary>
		public bool SeparateFiles { get; set; } = true;

		/// <summary>Output format: "json", "txt", "csv", "tbl".</summary>
		public string Format { get; set; } = "json";
	}

	/// <summary>
	/// Export result for a single table.
	/// </summary>
	public class ExportedTable {
		/// <summary>Table name.</summary>
		public string Name { get; set; } = "";

		/// <summary>Original ROM bank.</summary>
		public int Bank { get; set; }

		/// <summary>Number of entries.</summary>
		public int EntryCount { get; set; }

		/// <summary>Exported entries.</summary>
		public List<ExportedEntry> Entries { get; set; } = [];
	}

	/// <summary>
	/// Single exported text entry.
	/// </summary>
	public class ExportedEntry {
		/// <summary>Entry index.</summary>
		public int Index { get; set; }

		/// <summary>Original ROM offset.</summary>
		public int RomOffset { get; set; }

		/// <summary>Original DW4 text.</summary>
		public string OriginalText { get; set; } = "";

		/// <summary>DQ3r converted text.</summary>
		public string DQ3rText { get; set; } = "";

		/// <summary>Raw DW4 bytes (hex).</summary>
		public string? RawHex { get; set; }

		/// <summary>DQ3r encoded bytes (hex).</summary>
		public string? DQ3rHex { get; set; }

		/// <summary>Translation notes/comments.</summary>
		public string Notes { get; set; } = "";
	}

	/// <summary>
	/// Full export result.
	/// </summary>
	public class ExportResult {
		/// <summary>Source ROM name.</summary>
		public string SourceRom { get; set; } = "";

		/// <summary>Export timestamp.</summary>
		public DateTime ExportedAt { get; set; }

		/// <summary>Total entries exported.</summary>
		public int TotalEntries { get; set; }

		/// <summary>All exported tables.</summary>
		public List<ExportedTable> Tables { get; set; } = [];
	}

	/// <summary>
	/// Export all text from DW4 ROM.
	/// </summary>
	public static ExportResult ExportAll(byte[] romData, ExportConfig? config = null) {
		config ??= new ExportConfig();

		var result = new ExportResult {
			SourceRom = "Dragon Warrior IV (NES)",
			ExportedAt = DateTime.UtcNow,
			Tables = [],
		};

		// Extract all text tables
		var extractedTables = DialogExtractor.ExtractAllTables(romData);

		foreach (var table in extractedTables) {
			var exportedTable = new ExportedTable {
				Name = table.Name,
				Bank = table.Bank,
				EntryCount = table.Entries.Count,
				Entries = [],
			};

			foreach (var entry in table.Entries) {
				// Convert to DQ3r format
				byte[] dq3rBytes = [];
				string dq3rText = "";

				try {
					// Use DialogExtractor encoding
					dq3rBytes = ConvertDW4ToDQ3r(entry.RawBytes);
					dq3rText = entry.DecodedText; // Original decoded is fine for display
				}
				catch {
					dq3rText = entry.DecodedText;
				}

				var exportedEntry = new ExportedEntry {
					Index = entry.Index,
					RomOffset = entry.TextOffset,
					OriginalText = entry.DecodedText,
					DQ3rText = dq3rText,
				};

				if (config.IncludeRawBytes) {
					exportedEntry.RawHex = BitConverter.ToString(entry.RawBytes).Replace("-", "");
				}

				if (config.IncludeDQ3rBytes && dq3rBytes.Length > 0) {
					exportedEntry.DQ3rHex = BitConverter.ToString(dq3rBytes).Replace("-", "");
				}

				exportedTable.Entries.Add(exportedEntry);
			}

			result.Tables.Add(exportedTable);
			result.TotalEntries += exportedTable.EntryCount;
		}

		return result;
	}

	/// <summary>
	/// Convert DW4 bytes to DQ3r format using DialogExtractor character tables.
	/// </summary>
	private static byte[] ConvertDW4ToDQ3r(byte[] dw4Bytes) {
		var result = new List<byte>();

		foreach (byte b in dw4Bytes) {
			// Control codes
			if (b == 0xFF) {
				// End marker
				result.Add(0x00);
				result.Add(0xAC);
				break;
			}
			else if (b == 0xFD) {
				// Newline
				result.Add(0x00);
				result.Add(0xAD);
			}
			else {
				// Try to decode character and map to DQ3r
				string decoded = DialogExtractor.DecodeByte(b);
				if (decoded.Length == 1 && !decoded.StartsWith("[")) {
					char c = decoded[0];
					int code = FontToDQ3r.GetTableCode(c);
					result.Add((byte)(code >> 8));
					result.Add((byte)(code & 0xFF));
				}
				else if (decoded.Length > 1 && !decoded.StartsWith("[")) {
					// Multi-char (DTE) - encode each character
					foreach (char c in decoded) {
						int code = FontToDQ3r.GetTableCode(c);
						result.Add((byte)(code >> 8));
						result.Add((byte)(code & 0xFF));
					}
				}
				else {
					// Unknown - use space
					result.Add(0x02);
					result.Add(0x00);
				}
			}
		}

		return [.. result];
	}

	/// <summary>
	/// Save export result to files.
	/// </summary>
	public static void SaveToFiles(ExportResult result, string outputDir, ExportConfig? config = null) {
		config ??= new ExportConfig();
		Directory.CreateDirectory(outputDir);

		switch (config.Format.ToLower()) {
			case "json":
				SaveAsJson(result, outputDir, config);
				break;
			case "txt":
				SaveAsText(result, outputDir, config);
				break;
			case "csv":
				SaveAsCsv(result, outputDir, config);
				break;
			case "tbl":
				SaveAsTableFiles(result, outputDir, config);
				break;
			default:
				SaveAsJson(result, outputDir, config);
				break;
		}
	}

	private static void SaveAsJson(ExportResult result, string outputDir, ExportConfig config) {
		// Save combined file
		string json = JsonSerializer.Serialize(result, JsonOptions);
		File.WriteAllText(Path.Combine(outputDir, "dw4_script_export.json"), json);

		// Save individual tables if requested
		if (config.SeparateFiles) {
			foreach (var table in result.Tables) {
				string tableJson = JsonSerializer.Serialize(table, JsonOptions);
				File.WriteAllText(Path.Combine(outputDir, $"{table.Name}.json"), tableJson);
			}
		}
	}

	private static void SaveAsText(ExportResult result, string outputDir, ExportConfig config) {
		var sb = new StringBuilder();
		sb.AppendLine("========================================");
		sb.AppendLine("DRAGON WARRIOR IV SCRIPT EXPORT");
		sb.AppendLine($"Exported: {result.ExportedAt:yyyy-MM-dd HH:mm:ss}");
		sb.AppendLine($"Total Entries: {result.TotalEntries}");
		sb.AppendLine("========================================");
		sb.AppendLine();

		foreach (var table in result.Tables) {
			sb.AppendLine($"## {table.Name}");
			sb.AppendLine($"## Bank: 0x{table.Bank:X2}, Entries: {table.EntryCount}");
			sb.AppendLine("----------------------------------------");

			foreach (var entry in table.Entries) {
				sb.AppendLine($"[{entry.Index:D4}] @0x{entry.RomOffset:X6}");
				sb.AppendLine($"  {entry.OriginalText}");
				if (!string.IsNullOrEmpty(entry.Notes)) {
					sb.AppendLine($"  # {entry.Notes}");
				}
				sb.AppendLine();
			}

			sb.AppendLine();
		}

		File.WriteAllText(Path.Combine(outputDir, "dw4_script_export.txt"), sb.ToString());

		// Save individual tables if requested
		if (config.SeparateFiles) {
			foreach (var table in result.Tables) {
				var tableSb = new StringBuilder();
				tableSb.AppendLine($"# {table.Name}");
				tableSb.AppendLine($"# Bank: 0x{table.Bank:X2}");
				tableSb.AppendLine();

				foreach (var entry in table.Entries) {
					tableSb.AppendLine($"[{entry.Index:D4}] {entry.OriginalText}");
				}

				File.WriteAllText(Path.Combine(outputDir, $"{table.Name}.txt"), tableSb.ToString());
			}
		}
	}

	private static void SaveAsCsv(ExportResult result, string outputDir, ExportConfig config) {
		var sb = new StringBuilder();
		sb.AppendLine("Table,Index,Offset,OriginalText,DQ3rText,RawHex,Notes");

		foreach (var table in result.Tables) {
			foreach (var entry in table.Entries) {
				string original = EscapeCsv(entry.OriginalText);
				string dq3r = EscapeCsv(entry.DQ3rText);
				string notes = EscapeCsv(entry.Notes);

				sb.AppendLine($"{table.Name},{entry.Index},0x{entry.RomOffset:X6},{original},{dq3r},{entry.RawHex ?? ""},{notes}");
			}
		}

		File.WriteAllText(Path.Combine(outputDir, "dw4_script_export.csv"), sb.ToString());
	}

	private static void SaveAsTableFiles(ExportResult result, string outputDir, ExportConfig config) {
		// Save as translation-ready table files
		foreach (var table in result.Tables) {
			var sb = new StringBuilder();
			sb.AppendLine($"; {table.Name} - Dragon Warrior IV Translation");
			sb.AppendLine($"; Bank: 0x{table.Bank:X2}");
			sb.AppendLine($"; Entries: {table.EntryCount}");
			sb.AppendLine(";");
			sb.AppendLine("; Format: INDEX=OriginalText|TranslatedText");
			sb.AppendLine();

			foreach (var entry in table.Entries) {
				string text = entry.OriginalText.Replace("\n", "\\n");
				sb.AppendLine($"{entry.Index:D4}={text}|{text}");
			}

			File.WriteAllText(Path.Combine(outputDir, $"{table.Name}.tbl"), sb.ToString());
		}
	}

	private static string EscapeCsv(string value) {
		if (string.IsNullOrEmpty(value)) return "";
		if (value.Contains(',') || value.Contains('"') || value.Contains('\n')) {
			return $"\"{value.Replace("\"", "\"\"")}\"";
		}
		return value;
	}

	/// <summary>
	/// Import translations from a table file.
	/// </summary>
	public static Dictionary<int, string> ImportTranslations(string filePath) {
		var translations = new Dictionary<int, string>();

		foreach (string line in File.ReadAllLines(filePath)) {
			if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";")) continue;

			int eqPos = line.IndexOf('=');
			if (eqPos < 0) continue;

			string indexStr = line[..eqPos].Trim();
			string valueStr = line[(eqPos + 1)..];

			if (!int.TryParse(indexStr, out int index)) continue;

			// Get translated text (after |) or original if no separator
			int pipePos = valueStr.IndexOf('|');
			string translation = pipePos >= 0 ? valueStr[(pipePos + 1)..] : valueStr;

			translations[index] = translation.Replace("\\n", "\n");
		}

		return translations;
	}

	/// <summary>
	/// Generate a DQ3r script file from translations.
	/// </summary>
	public static byte[] GenerateDQ3rScript(Dictionary<int, string> translations) {
		var result = new List<byte>();

		foreach (var kvp in translations.OrderBy(x => x.Key)) {
			byte[] encoded = ScriptToDQ3r.EncodeString(kvp.Value);
			result.AddRange(encoded);
		}

		return [.. result];
	}
}
