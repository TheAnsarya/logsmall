using System.Text.Json;
using DQ4rLib.Models;

namespace DQ4rLib.Converters;

/// <summary>
/// Converts text/dialog from DW4 NES format to SNES format
/// </summary>
public static class TextConverter {
	/// <summary>
	/// Convert extracted DW4 dialog JSON to SNES format
	/// </summary>
	public static SnesText ConvertDialogs(string jsonPath, string tablePath) {
		var snesText = new SnesText();

		// Load dialogs from JSON
		string json = File.ReadAllText(jsonPath);
		var dialogs = JsonSerializer.Deserialize<List<DialogJson>>(json);

		if (dialogs == null)
			return snesText;

		foreach (var dialog in dialogs) {
			snesText.Dialogs[dialog.Id] = new DialogEntry {
				Id = dialog.Id,
				Text = ConvertControlCodes(dialog.Text),
				Speaker = dialog.Speaker,
				Chapter = dialog.Chapter,
				Location = dialog.Location
			};
		}

		return snesText;
	}

	/// <summary>
	/// Convert DW4 NES control codes to SNES control codes
	/// </summary>
	private static string ConvertControlCodes(string text) {
		// Map NES control codes to SNES equivalents
		// These are based on the DW4Lib FontToDQ3r control code analysis
		var replacements = new Dictionary<string, string> {
			// Line/page control
			["[NEWLINE]"] = "[LINE]",
			["[NEWPAGE]"] = "[PAGE]",
			["[END]"] = "[END]",
			["[WAIT]"] = "[WAIT]",

			// Name substitution
			["[HERO]"] = "[NAME:0]",
			["[RAGNAR]"] = "[NAME:1]",
			["[ALENA]"] = "[NAME:2]",
			["[CHRISTO]"] = "[NAME:3]",
			["[BREY]"] = "[NAME:4]",
			["[TALOON]"] = "[NAME:5]",
			["[NARA]"] = "[NAME:6]",
			["[MARA]"] = "[NAME:7]",
			["[PARTY1]"] = "[PARTY:0]",
			["[PARTY2]"] = "[PARTY:1]",
			["[PARTY3]"] = "[PARTY:2]",
			["[PARTY4]"] = "[PARTY:3]",

			// Items/enemies
			["[ITEM]"] = "[ITEM]",
			["[ENEMY]"] = "[MONSTER]",
			["[NUMBER]"] = "[NUM]",
			["[GOLD]"] = "[GOLD]",

			// Formatting
			["[PAUSE]"] = "[DELAY:30]",
			["[CLEAR]"] = "[CLEAR]",
		};

		foreach (var (nes, snes) in replacements) {
			text = text.Replace(nes, snes, StringComparison.OrdinalIgnoreCase);
		}

		return text;
	}

	/// <summary>
	/// Export text to SNES assembly include files
	/// </summary>
	public static void ExportToAsm(SnesText text, string outputDir, TextEncoder encoder) {
		Directory.CreateDirectory(outputDir);

		// Export dialogs
		using (var writer = File.CreateText(Path.Combine(outputDir, "dialogs.inc"))) {
			writer.WriteLine("; DQ4r Dialog Data");
			writer.WriteLine("; Auto-generated from DW4 NES");
			writer.WriteLine();

			foreach (var (id, dialog) in text.Dialogs.OrderBy(d => d.Key)) {
				writer.WriteLine($"Dialog_{id:D4}:");
				byte[] bytes = dialog.ToBytes(encoder);
				WriteAsmBytes(writer, bytes);
				writer.WriteLine();
			}

			// Dialog pointer table
			writer.WriteLine();
			writer.WriteLine("DialogTable:");
			foreach (var id in text.Dialogs.Keys.OrderBy(k => k)) {
				writer.WriteLine($"\tdw Dialog_{id:D4}");
			}
		}

		// Export item names
		ExportStringTable(
			Path.Combine(outputDir, "items.inc"),
			"Item Names",
			"ItemName",
			text.ItemNames,
			encoder
		);

		// Export monster names
		ExportStringTable(
			Path.Combine(outputDir, "monsters.inc"),
			"Monster Names",
			"MonsterName",
			text.MonsterNames,
			encoder
		);

		// Export spell names
		ExportStringTable(
			Path.Combine(outputDir, "spells.inc"),
			"Spell Names",
			"SpellName",
			text.SpellNames,
			encoder
		);
	}

	private static void ExportStringTable(
		string path,
		string title,
		string prefix,
		List<string> strings,
		TextEncoder encoder) {
		using var writer = File.CreateText(path);
		writer.WriteLine($"; {title}");
		writer.WriteLine("; Auto-generated from DW4 NES");
		writer.WriteLine();

		for (int i = 0; i < strings.Count; i++) {
			writer.WriteLine($"{prefix}_{i:D3}:");
			byte[] bytes = encoder.Encode(strings[i] + "[END]");
			WriteAsmBytes(writer, bytes);
		}

		writer.WriteLine();
		writer.WriteLine($"{prefix}Table:");
		for (int i = 0; i < strings.Count; i++) {
			writer.WriteLine($"\tdw {prefix}_{i:D3}");
		}
	}

	private static void WriteAsmBytes(StreamWriter writer, byte[] bytes) {
		const int bytesPerLine = 16;
		for (int i = 0; i < bytes.Length; i += bytesPerLine) {
			int count = Math.Min(bytesPerLine, bytes.Length - i);
			string hex = string.Join(", ", bytes.Skip(i).Take(count).Select(b => $"${b:x2}"));
			writer.WriteLine($"\tdb {hex}");
		}
	}

	/// <summary>
	/// Create standard DQ3r-compatible text encoder
	/// </summary>
	public static TextEncoder CreateDq3rEncoder() {
		var encoder = new TextEncoder();

		// Register SNES control codes
		encoder.RegisterControlCode("END", 0x00);
		encoder.RegisterControlCode("LINE", 0x01);
		encoder.RegisterControlCode("PAGE", 0x03);
		encoder.RegisterControlCode("WAIT", 0x04);
		encoder.RegisterControlCode("CLEAR", 0x05);

		// Name substitution
		for (int i = 0; i < 8; i++) {
			encoder.RegisterControlCode($"NAME:{i}", 0x10, (byte)i);
		}
		for (int i = 0; i < 4; i++) {
			encoder.RegisterControlCode($"PARTY:{i}", 0x18, (byte)i);
		}

		// Variables
		encoder.RegisterControlCode("ITEM", 0x20);
		encoder.RegisterControlCode("MONSTER", 0x21);
		encoder.RegisterControlCode("NUM", 0x22);
		encoder.RegisterControlCode("GOLD", 0x23);

		// Delay
		encoder.RegisterControlCode("DELAY:10", 0x80, 10);
		encoder.RegisterControlCode("DELAY:30", 0x80, 30);
		encoder.RegisterControlCode("DELAY:60", 0x80, 60);

		return encoder;
	}
}

/// <summary>
/// JSON model for dialog import
/// </summary>
internal class DialogJson {
	public int Id { get; set; }
	public string Text { get; set; } = string.Empty;
	public string? Speaker { get; set; }
	public int Chapter { get; set; }
	public string? Location { get; set; }
}
