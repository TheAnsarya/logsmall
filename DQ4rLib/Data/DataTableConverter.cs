using System.Text.Json;

namespace DQ4rLib.Data;

/// <summary>
/// Converts DW4 NES game data tables to SNES assembly format
/// </summary>
public static class DataTableConverter {
	/// <summary>
	/// Convert monster stats JSON to assembly include
	/// </summary>
	public static void ConvertMonsterStats(string jsonPath, string outputPath) {
		string json = File.ReadAllText(jsonPath);
		var monsters = JsonSerializer.Deserialize<List<MonsterStats>>(json);

		if (monsters == null)
			return;

		using var writer = File.CreateText(outputPath);
		writer.WriteLine("; DQ4r Monster Statistics");
		writer.WriteLine("; Auto-generated from DW4 NES");
		writer.WriteLine();

		// Stats table
		writer.WriteLine("MonsterStatsTable:");
		foreach (var monster in monsters) {
			writer.WriteLine($"\t; {monster.Name} (ID: {monster.Id})");
			writer.WriteLine($"\tdw {monster.Hp}\t\t; HP");
			writer.WriteLine($"\tdb {monster.Attack}\t\t; Attack");
			writer.WriteLine($"\tdb {monster.Defense}\t\t; Defense");
			writer.WriteLine($"\tdb {monster.Agility}\t\t; Agility");
			writer.WriteLine($"\tdw {monster.Exp}\t\t; Experience");
			writer.WriteLine($"\tdw {monster.Gold}\t\t; Gold");
			writer.WriteLine();
		}
	}

	/// <summary>
	/// Convert item data JSON to assembly include
	/// </summary>
	public static void ConvertItemData(string jsonPath, string outputPath) {
		string json = File.ReadAllText(jsonPath);
		var items = JsonSerializer.Deserialize<List<ItemData>>(json);

		if (items == null)
			return;

		using var writer = File.CreateText(outputPath);
		writer.WriteLine("; DQ4r Item Data");
		writer.WriteLine("; Auto-generated from DW4 NES");
		writer.WriteLine();

		writer.WriteLine("ItemDataTable:");
		foreach (var item in items) {
			writer.WriteLine($"\t; {item.Name} (ID: {item.Id})");
			writer.WriteLine($"\tdb {item.Type}\t\t; Type");
			writer.WriteLine($"\tdb {item.EquipFlags}\t\t; Equip flags");
			writer.WriteLine($"\tdw {item.Price}\t\t; Buy price");
			writer.WriteLine($"\tdb {item.Power}\t\t; Power/effect");
			writer.WriteLine($"\tdb {item.Special}\t\t; Special flags");
			writer.WriteLine();
		}
	}

	/// <summary>
	/// Convert spell data JSON to assembly include
	/// </summary>
	public static void ConvertSpellData(string jsonPath, string outputPath) {
		string json = File.ReadAllText(jsonPath);
		var spells = JsonSerializer.Deserialize<List<SpellData>>(json);

		if (spells == null)
			return;

		using var writer = File.CreateText(outputPath);
		writer.WriteLine("; DQ4r Spell Data");
		writer.WriteLine("; Auto-generated from DW4 NES");
		writer.WriteLine();

		writer.WriteLine("SpellDataTable:");
		foreach (var spell in spells) {
			writer.WriteLine($"\t; {spell.Name} (ID: {spell.Id})");
			writer.WriteLine($"\tdb {spell.MpCost}\t\t; MP Cost");
			writer.WriteLine($"\tdb {spell.Target}\t\t; Target type");
			writer.WriteLine($"\tdb {spell.Effect}\t\t; Effect type");
			writer.WriteLine($"\tdb {spell.Power}\t\t; Power");
			writer.WriteLine();
		}
	}

	/// <summary>
	/// Convert experience table to assembly
	/// </summary>
	public static void ConvertExpTable(int[] expValues, string outputPath) {
		using var writer = File.CreateText(outputPath);
		writer.WriteLine("; DQ4r Experience Table");
		writer.WriteLine("; Experience needed for each level");
		writer.WriteLine();

		writer.WriteLine("ExpTable:");
		for (int level = 1; level < expValues.Length; level++) {
			writer.WriteLine($"\tdl {expValues[level]}\t\t; Level {level + 1}");
		}
	}

	/// <summary>
	/// Convert all data tables from JSON directory
	/// </summary>
	public static void ConvertAllTables(string jsonDir, string outputDir) {
		Directory.CreateDirectory(outputDir);

		string monstersJson = Path.Combine(jsonDir, "monsters.json");
		if (File.Exists(monstersJson)) {
			ConvertMonsterStats(monstersJson, Path.Combine(outputDir, "monsters.inc"));
		}

		string itemsJson = Path.Combine(jsonDir, "items.json");
		if (File.Exists(itemsJson)) {
			ConvertItemData(itemsJson, Path.Combine(outputDir, "items.inc"));
		}

		string spellsJson = Path.Combine(jsonDir, "spells.json");
		if (File.Exists(spellsJson)) {
			ConvertSpellData(spellsJson, Path.Combine(outputDir, "spells.inc"));
		}
	}
}

// Data models for JSON deserialization

public class MonsterStats {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int Hp { get; set; }
	public int Attack { get; set; }
	public int Defense { get; set; }
	public int Agility { get; set; }
	public int Exp { get; set; }
	public int Gold { get; set; }
}

public class ItemData {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int Type { get; set; }
	public int EquipFlags { get; set; }
	public int Price { get; set; }
	public int Power { get; set; }
	public int Special { get; set; }
}

public class SpellData {
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int MpCost { get; set; }
	public int Target { get; set; }
	public int Effect { get; set; }
	public int Power { get; set; }
}
