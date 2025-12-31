using DW4Lib.Converters;
using DW4Lib.ROM;

namespace DW4Extract;

/// <summary>
/// Command-line tool for extracting Dragon Warrior IV ROM data to JSON.
/// </summary>
public static class Program {
	public static int Main(string[] args) {
		Console.WriteLine("DW4Extract - Dragon Warrior IV Data Extraction Tool");
		Console.WriteLine();

		if (args.Length < 1) {
			PrintUsage();
			return 1;
		}

		string romPath = "";
		string outputPath = "output";
		string command = "all";

		// Parse arguments
		for (int i = 0; i < args.Length; i++) {
			switch (args[i]) {
				case "-r":
				case "--rom":
					if (i + 1 < args.Length) romPath = args[++i];
					break;
				case "-o":
				case "--output":
					if (i + 1 < args.Length) outputPath = args[++i];
					break;
				case "all":
				case "monsters":
				case "items":
				case "spells":
				case "text":
				case "exp":
				case "experience":
					command = args[i];
					break;
				case "-h":
				case "--help":
					PrintUsage();
					return 0;
				default:
					// If no flag, treat as ROM path
					if (!args[i].StartsWith("-") && string.IsNullOrEmpty(romPath)) {
						romPath = args[i];
					}
					break;
			}
		}

		if (string.IsNullOrEmpty(romPath)) {
			Console.Error.WriteLine("Error: ROM file path is required.");
			PrintUsage();
			return 1;
		}

		if (!File.Exists(romPath)) {
			Console.Error.WriteLine($"Error: ROM file not found: {romPath}");
			return 1;
		}

		Console.WriteLine($"ROM: {romPath}");
		Console.WriteLine($"Output: {outputPath}");
		Console.WriteLine($"Command: {command}");
		Console.WriteLine();

		Directory.CreateDirectory(outputPath);

		try {
			var rom = new DW4Rom(File.ReadAllBytes(romPath));

			switch (command) {
				case "all":
					ExtractMonsters(rom, outputPath);
					ExtractItems(rom, outputPath);
					ExtractSpells(rom, outputPath);
					ExtractText(rom, outputPath);
					ExtractExperienceTables(rom, outputPath);
					break;
				case "monsters":
					ExtractMonsters(rom, outputPath);
					break;
				case "items":
					ExtractItems(rom, outputPath);
					break;
				case "spells":
					ExtractSpells(rom, outputPath);
					break;
				case "text":
					ExtractText(rom, outputPath);
					break;
				case "exp":
				case "experience":
					ExtractExperienceTables(rom, outputPath);
					break;
			}

			Console.WriteLine();
			Console.WriteLine("Extraction complete!");
			return 0;
		} catch (Exception ex) {
			Console.Error.WriteLine($"Error: {ex.Message}");
			return 1;
		}
	}

	private static void PrintUsage() {
		Console.WriteLine("Usage: DW4Extract [command] -r <rom_path> [-o <output_dir>]");
		Console.WriteLine();
		Console.WriteLine("Commands:");
		Console.WriteLine("  all       Extract all game data (default)");
		Console.WriteLine("  monsters  Extract monster data");
		Console.WriteLine("  items     Extract item data");
		Console.WriteLine("  spells    Extract spell data");
		Console.WriteLine("  text      Extract text strings");
		Console.WriteLine("  exp       Extract experience tables");
		Console.WriteLine();
		Console.WriteLine("Options:");
		Console.WriteLine("  -r, --rom     Path to Dragon Warrior IV NES ROM");
		Console.WriteLine("  -o, --output  Output directory (default: output)");
		Console.WriteLine("  -h, --help    Show this help message");
		Console.WriteLine();
		Console.WriteLine("Examples:");
		Console.WriteLine("  DW4Extract -r \"Dragon Warrior IV.nes\"");
		Console.WriteLine("  DW4Extract monsters -r game.nes -o data");
	}

	private static void ExtractMonsters(DW4Rom rom, string outputPath) {
		Console.Write("Extracting monsters... ");
		try {
			var monsters = rom.ReadAllMonsters();
			var filePath = Path.Combine(outputPath, "monsters.json");
			MonsterConverter.SaveToFile(monsters, filePath);
			Console.WriteLine($"OK ({monsters.Count} monsters)");
		} catch (Exception ex) {
			Console.WriteLine($"FAILED: {ex.Message}");
		}
	}

	private static void ExtractItems(DW4Rom rom, string outputPath) {
		Console.Write("Extracting items... ");
		try {
			var items = rom.ReadAllItems();
			var filePath = Path.Combine(outputPath, "items.json");
			ItemConverter.SaveToFile(items, filePath);
			Console.WriteLine($"OK ({items.Count} items)");
		} catch (Exception ex) {
			Console.WriteLine($"FAILED: {ex.Message}");
		}
	}

	private static void ExtractSpells(DW4Rom rom, string outputPath) {
		Console.Write("Extracting spells... ");
		try {
			var spells = rom.ReadAllSpells();
			var filePath = Path.Combine(outputPath, "spells.json");
			SpellConverter.SaveToFile(spells, filePath);
			Console.WriteLine($"OK ({spells.Count} spells)");
		} catch (Exception ex) {
			Console.WriteLine($"FAILED: {ex.Message}");
		}
	}

	private static void ExtractText(DW4Rom rom, string outputPath) {
		Console.Write("Extracting text... ");
		try {
			var textBlocks = TextConverter.ExtractAll(rom);
			var filePath = Path.Combine(outputPath, "text.json");
			TextConverter.SaveToFile(textBlocks, filePath);

			// Also save individual text dumps
			foreach (var block in textBlocks) {
				var dumpPath = Path.Combine(outputPath, $"text_{block.Name.ToLower()}.txt");
				File.WriteAllText(dumpPath, TextConverter.GenerateTextDump(block));
			}

			Console.WriteLine($"OK ({textBlocks.Count} text blocks)");
		} catch (Exception ex) {
			Console.WriteLine($"FAILED: {ex.Message}");
		}
	}

	private static void ExtractExperienceTables(DW4Rom rom, string outputPath) {
		Console.Write("Extracting experience tables... ");
		try {
			var expTables = rom.ReadExperienceTables();
			var filePath = Path.Combine(outputPath, "experience_tables.json");
			ExperienceTableConverter.SaveToFile(expTables, filePath);
			Console.WriteLine($"OK ({expTables.Tables.Count} character tables)");
		} catch (Exception ex) {
			Console.WriteLine($"FAILED: {ex.Message}");
		}
	}
}
