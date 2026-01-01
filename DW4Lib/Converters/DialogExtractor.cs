namespace DW4Lib.Converters;

using DW4Lib.ROM;
using System.Text;
using System.Text.Json;

/// <summary>
/// Comprehensive dialog extractor for Dragon Warrior IV NES.
/// Extracts all text from the ROM including dialog, menus, and names.
/// </summary>
public static class DialogExtractor {
	private static readonly JsonSerializerOptions JsonOptions = new() {
		WriteIndented = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};

	/// <summary>
	/// DW4 NES character encoding table.
	/// Based on analysis of ROM data and the TBL file.
	/// </summary>
	public static readonly Dictionary<byte, string> CharTable = new() {
		// Whitespace
		[0x00] = " ",

		// Digits 0-9
		[0x01] = "0", [0x02] = "1", [0x03] = "2", [0x04] = "3", [0x05] = "4",
		[0x06] = "5", [0x07] = "6", [0x08] = "7", [0x09] = "8", [0x0A] = "9",

		// Lowercase a-z
		[0x0B] = "a", [0x0C] = "b", [0x0D] = "c", [0x0E] = "d", [0x0F] = "e",
		[0x10] = "f", [0x11] = "g", [0x12] = "h", [0x13] = "i", [0x14] = "j",
		[0x15] = "k", [0x16] = "l", [0x17] = "m", [0x18] = "n", [0x19] = "o",
		[0x1A] = "p", [0x1B] = "q", [0x1C] = "r", [0x1D] = "s", [0x1E] = "t",
		[0x1F] = "u", [0x20] = "v", [0x21] = "w", [0x22] = "x", [0x23] = "y",
		[0x24] = "z",

		// Uppercase A-Z
		[0x25] = "A", [0x26] = "B", [0x27] = "C", [0x28] = "D", [0x29] = "E",
		[0x2A] = "F", [0x2B] = "G", [0x2C] = "H", [0x2D] = "I", [0x2E] = "J",
		[0x2F] = "K", [0x30] = "L", [0x31] = "M", [0x32] = "N", [0x33] = "O",
		[0x34] = "P", [0x35] = "Q", [0x36] = "R", [0x37] = "S", [0x38] = "T",
		[0x39] = "U", [0x3A] = "V", [0x3B] = "W", [0x3C] = "X", [0x3D] = "Y",
		[0x3E] = "Z",

		// Primary punctuation
		[0x3F] = "'", [0x40] = ".", [0x41] = ",", [0x42] = "-", [0x43] = "?",
		[0x44] = "!", [0x45] = "(", [0x46] = ")", [0x47] = "/", [0x48] = ":",
		[0x49] = "*", [0x4A] = "\"",

		// Alternate punctuation (used in some contexts)
		[0x65] = "'", [0x66] = ".", [0x67] = ",", [0x68] = "~", [0x69] = "?",
		[0x6A] = "!", [0x6B] = "(", [0x6C] = ")", [0x6D] = "/", [0x6E] = ":",
		[0x6F] = "*", [0x70] = "\"", [0x71] = "-", [0x78] = ".",
	};

	/// <summary>
	/// DTE (Dual Tile Encoding) table - single bytes that expand to multiple characters.
	/// </summary>
	public static readonly Dictionary<byte, string> DteTable = new() {
		[0x80] = "64", [0x81] = "2", [0x82] = "sp", [0x83] = "ee", [0x84] = "d",
		[0x85] = "yo", [0x86] = "u", [0x87] = "li", [0x88] = "ke", [0x89] = ".",
		[0x8A] = "A", [0x8B] = "QU", [0x8C] = "ES", [0x8D] = "TC", [0x8E] = "ON",
		[0x8F] = "TI", [0x90] = "NU", [0x92] = "CH", [0x93] = "AN", [0x94] = "GE",
		[0x95] = " M", [0x96] = "ES", [0x97] = "SA", [0x98] = "GE", [0x99] = " S",
		[0x9A] = "PE", [0x9B] = "ED", [0x9C] = "BE", [0x9D] = "GI", [0x9E] = "N",
		[0x9F] = "A", [0xA0] = "NE", [0xA1] = "W", [0xA2] = "QU", [0xA3] = "ES",
		[0xA4] = "TC", [0xA5] = "OP", [0xA7] = "ER", [0xA8] = "AS", [0xAA] = "NA",
		[0xAB] = "ME", [0xAC] = "Do", [0xAD] = " y", [0xAE] = "ou", [0xAF] = " w",
		[0xB0] = "an", [0xB1] = "t", [0xB2] = "to", [0xB3] = "er", [0xB4] = "as",
		[0xB5] = "e", [0xB6] = "th", [0xB7] = "is", [0xB8] = "qu", [0xB9] = "es",
		[0xBA] = "t?", [0xBB] = "Se", [0xBC] = "le", [0xBD] = "ct", [0xBE] = " t",
		[0xBF] = "he", [0xC0] = " m", [0xC1] = "es", [0xC2] = "sa", [0xC3] = "ge",
		[0xC4] = " 1", [0xC5] = " 2", [0xC6] = " 3", [0xC7] = " 4", [0xC8] = " 5",
		[0xC9] = " 6", [0xCA] = " 7", [0xCB] = " 8", [0xCC] = "Fa", [0xCD] = "st",
		[0xD2] = "Sl", [0xD3] = "ow", [0xD4] = " C", [0xD5] = "ha", [0xD6] = "pt",
		[0xD7] = "er", [0xD8] = " L", [0xD9] = "EV", [0xDA] = "EL", [0xDC] = ". A",
		[0xDD] = "DV", [0xDE] = "EN", [0xDF] = "TU", [0xE0] = "RE", [0xE1] = " L",
		[0xE2] = "OG", [0xE3] = "FI", [0xE4] = "GH", [0xE5] = "TT", [0xE6] = "AC",
		[0xE7] = "TI", [0xE8] = "CS", [0xE9] = "ME", [0xEA] = "MB", [0xEB] = "ER",
		[0xEC] = "RU", [0xED] = "NS", [0xEE] = "PE", [0xEF] = "LL", [0xF0] = "IT",
		[0xF1] = "EM", [0xF2] = "AT", [0xF3] = "TA", [0xF4] = "CK", [0xF5] = "PA",
		[0xF6] = "RR", [0xF7] = "YC", [0xF8] = " 0", [0xF9] = "00", [0xFA] = "00",
		[0xFB] = "0S", [0xFC] = "EE", [0xFE] = "SS",
	};

	/// <summary>
	/// Control codes used in dialog.
	/// </summary>
	public static readonly Dictionary<byte, string> ControlCodes = new() {
		[0xFD] = "[LINE]",
		[0xFF] = "[END]",
	};

	/// <summary>
	/// Known text pointer tables in DW4 NES ROM.
	/// </summary>
	public static readonly TextPointerTable[] KnownTables = [
		// Chapter 1 Dialog
		new() { Name = "Chapter1Dialog", Bank = 0x0C, PointerTableStart = 0x8000, PointerTableEnd = 0x8100 },
		new() { Name = "Chapter1Dialog2", Bank = 0x0C, PointerTableStart = 0x8100, PointerTableEnd = 0x8200 },

		// Chapter 2 Dialog
		new() { Name = "Chapter2Dialog", Bank = 0x0D, PointerTableStart = 0x8000, PointerTableEnd = 0x8100 },
		new() { Name = "Chapter2Dialog2", Bank = 0x0D, PointerTableStart = 0x8100, PointerTableEnd = 0x8200 },

		// Chapter 3 Dialog
		new() { Name = "Chapter3Dialog", Bank = 0x0E, PointerTableStart = 0x8000, PointerTableEnd = 0x8100 },
		new() { Name = "Chapter3Dialog2", Bank = 0x0E, PointerTableStart = 0x8100, PointerTableEnd = 0x8200 },

		// Chapter 4 Dialog
		new() { Name = "Chapter4Dialog", Bank = 0x0F, PointerTableStart = 0x8000, PointerTableEnd = 0x8100 },
		new() { Name = "Chapter4Dialog2", Bank = 0x0F, PointerTableStart = 0x8100, PointerTableEnd = 0x8200 },

		// Chapter 5 Dialog
		new() { Name = "Chapter5Dialog", Bank = 0x10, PointerTableStart = 0x8000, PointerTableEnd = 0x8100 },
		new() { Name = "Chapter5Dialog2", Bank = 0x10, PointerTableStart = 0x8100, PointerTableEnd = 0x8200 },

		// Menu Text
		new() { Name = "MenuText", Bank = 0x1F, PointerTableStart = 0x8000, PointerTableEnd = 0x8040 },

		// Battle Text
		new() { Name = "BattleText", Bank = 0x1E, PointerTableStart = 0x8000, PointerTableEnd = 0x8080 },

		// Item Names
		new() { Name = "ItemNames", Bank = 0x25, PointerTableStart = 0x8000, PointerTableEnd = 0x8100 },

		// Monster Names
		new() { Name = "MonsterNames", Bank = 0x24, PointerTableStart = 0x8000, PointerTableEnd = 0x8140 },

		// Spell Names
		new() { Name = "SpellNames", Bank = 0x25, PointerTableStart = 0x8100, PointerTableEnd = 0x8180 },

		// Location Names
		new() { Name = "LocationNames", Bank = 0x22, PointerTableStart = 0xBE00, PointerTableEnd = 0xBF00 },
	];

	/// <summary>
	/// Decode a single byte to its string representation.
	/// </summary>
	public static string DecodeByte(byte b) {
		if (ControlCodes.TryGetValue(b, out string? ctrl)) return ctrl;
		if (CharTable.TryGetValue(b, out string? ch)) return ch;
		if (DteTable.TryGetValue(b, out string? dte)) return dte;
		return $"[${b:X2}]";
	}

	/// <summary>
	/// Decode a byte array to string.
	/// </summary>
	public static string DecodeBytes(byte[] data, int offset = 0, int maxLength = -1) {
		var result = new StringBuilder();
		int length = maxLength > 0 ? Math.Min(maxLength, data.Length - offset) : data.Length - offset;

		for (int i = 0; i < length; i++) {
			byte b = data[offset + i];
			if (b == 0xFF) break; // End marker

			if (ControlCodes.TryGetValue(b, out string? ctrl)) {
				result.Append(ctrl);
				if (b == 0xFD) result.Append('\n');
			}
			else if (CharTable.TryGetValue(b, out string? ch)) {
				result.Append(ch);
			}
			else if (DteTable.TryGetValue(b, out string? dte)) {
				result.Append(dte);
			}
			else {
				result.Append($"[${b:X2}]");
			}
		}

		return result.ToString();
	}

	/// <summary>
	/// Extract text from a pointer table.
	/// </summary>
	public static ExtractedTextTable ExtractPointerTable(byte[] rom, TextPointerTable table) {
		var result = new ExtractedTextTable {
			Name = table.Name,
			Bank = table.Bank,
			PointerTableStart = table.PointerTableStart,
			Entries = [],
		};

		int bankOffset = table.Bank * 0x4000; // NES PRG bank offset (16KB banks)
		int pointerCount = (table.PointerTableEnd - table.PointerTableStart) / 2;

		for (int i = 0; i < pointerCount; i++) {
			int pointerAddr = bankOffset + (table.PointerTableStart - 0x8000) + i * 2;

			if (pointerAddr + 1 >= rom.Length) break;

			// Read little-endian pointer
			int textPtr = rom[pointerAddr] | (rom[pointerAddr + 1] << 8);

			// Convert CPU address to file offset
			int textOffset = bankOffset + (textPtr - 0x8000);

			if (textOffset < 0 || textOffset >= rom.Length) continue;

			// Read text until end marker
			var textBytes = new List<byte>();
			for (int j = 0; j < 512 && textOffset + j < rom.Length; j++) {
				byte b = rom[textOffset + j];
				textBytes.Add(b);
				if (b == 0xFF) break;
			}

			result.Entries.Add(new ExtractedTextEntry {
				Index = i,
				PointerOffset = pointerAddr,
				TextOffset = textOffset,
				CpuAddress = textPtr,
				RawBytes = [.. textBytes],
				DecodedText = DecodeBytes([.. textBytes]),
			});
		}

		return result;
	}

	/// <summary>
	/// Extract all known text tables from ROM.
	/// </summary>
	public static List<ExtractedTextTable> ExtractAllTables(byte[] rom) {
		var results = new List<ExtractedTextTable>();

		foreach (var table in KnownTables) {
			try {
				var extracted = ExtractPointerTable(rom, table);
				if (extracted.Entries.Count > 0) {
					results.Add(extracted);
				}
			}
			catch {
				// Skip tables that fail to extract
			}
		}

		return results;
	}

	/// <summary>
	/// Scan ROM for potential text strings.
	/// </summary>
	public static List<FoundTextString> ScanForText(byte[] rom, int minLength = 5) {
		var results = new List<FoundTextString>();
		int textStart = -1;
		var currentText = new List<byte>();

		for (int i = 0; i < rom.Length; i++) {
			byte b = rom[i];

			bool isText = CharTable.ContainsKey(b) || DteTable.ContainsKey(b) || b == 0xFD;

			if (isText) {
				if (textStart < 0) textStart = i;
				currentText.Add(b);
			}
			else if (b == 0xFF && currentText.Count >= minLength) {
				// End of string
				currentText.Add(b);
				string decoded = DecodeBytes([.. currentText]);

				// Filter out garbage (should have mostly printable chars)
				int printableCount = decoded.Count(c => char.IsLetterOrDigit(c) || char.IsPunctuation(c) || c == ' ');
				if (printableCount > decoded.Length * 0.6) {
					results.Add(new FoundTextString {
						Offset = textStart,
						RawBytes = [.. currentText],
						DecodedText = decoded,
					});
				}

				textStart = -1;
				currentText.Clear();
			}
			else {
				textStart = -1;
				currentText.Clear();
			}
		}

		return results;
	}

	/// <summary>
	/// Export extracted text to JSON.
	/// </summary>
	public static string ToJson(List<ExtractedTextTable> tables) {
		return JsonSerializer.Serialize(tables, JsonOptions);
	}

	/// <summary>
	/// Export extracted text to plain text format.
	/// </summary>
	public static string ToPlainText(List<ExtractedTextTable> tables) {
		var sb = new StringBuilder();
		sb.AppendLine("DRAGON WARRIOR IV - EXTRACTED TEXT");
		sb.AppendLine("===================================");
		sb.AppendLine();

		foreach (var table in tables) {
			sb.AppendLine($"## {table.Name}");
			sb.AppendLine($"Bank: 0x{table.Bank:X2}");
			sb.AppendLine($"Pointer Table: 0x{table.PointerTableStart:X4}");
			sb.AppendLine($"Entry Count: {table.Entries.Count}");
			sb.AppendLine();

			foreach (var entry in table.Entries) {
				sb.AppendLine($"  [{entry.Index:D3}] @0x{entry.TextOffset:X5}: {entry.DecodedText}");
			}

			sb.AppendLine();
		}

		return sb.ToString();
	}

	/// <summary>
	/// Save extracted text to files.
	/// </summary>
	public static void SaveToFiles(List<ExtractedTextTable> tables, string outputDir) {
		Directory.CreateDirectory(outputDir);

		// Save combined JSON
		string json = ToJson(tables);
		File.WriteAllText(Path.Combine(outputDir, "all_text.json"), json);

		// Save combined plain text
		string plainText = ToPlainText(tables);
		File.WriteAllText(Path.Combine(outputDir, "all_text.txt"), plainText);

		// Save individual table files
		foreach (var table in tables) {
			string tableJson = JsonSerializer.Serialize(table, JsonOptions);
			File.WriteAllText(Path.Combine(outputDir, $"{table.Name}.json"), tableJson);
		}
	}

	/// <summary>
	/// Text pointer table definition.
	/// </summary>
	public class TextPointerTable {
		public string Name { get; set; } = "";
		public int Bank { get; set; }
		public int PointerTableStart { get; set; }
		public int PointerTableEnd { get; set; }
	}

	/// <summary>
	/// Extracted text table with all entries.
	/// </summary>
	public class ExtractedTextTable {
		public string Name { get; set; } = "";
		public int Bank { get; set; }
		public int PointerTableStart { get; set; }
		public List<ExtractedTextEntry> Entries { get; set; } = [];
	}

	/// <summary>
	/// Single extracted text entry.
	/// </summary>
	public class ExtractedTextEntry {
		public int Index { get; set; }
		public int PointerOffset { get; set; }
		public int TextOffset { get; set; }
		public int CpuAddress { get; set; }
		public byte[] RawBytes { get; set; } = [];
		public string DecodedText { get; set; } = "";

		public string RawHex => BitConverter.ToString(RawBytes).Replace("-", " ");
	}

	/// <summary>
	/// Text string found during ROM scan.
	/// </summary>
	public class FoundTextString {
		public int Offset { get; set; }
		public byte[] RawBytes { get; set; } = [];
		public string DecodedText { get; set; } = "";
	}
}
