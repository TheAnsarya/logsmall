using DW4Lib.DataStructures;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 Experience Tables to JSON format for editing.
/// </summary>
public static class ExperienceTableConverter {
	private static readonly JsonSerializerOptions JsonOptions = new() {
		WriteIndented = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = { new JsonStringEnumConverter() }
	};

	/// <summary>
	/// Convert an experience table to JSON-friendly format.
	/// </summary>
	public static ExpTableJson ToJson(ExperienceTable table) {
		var json = new ExpTableJson {
			CharacterId = table.CharacterId,
			CharacterName = table.CharacterName,
			MaxLevel = table.ExpForLevel.Count
		};

		for (int i = 0; i < table.ExpForLevel.Count; i++) {
			json.Levels.Add(new LevelEntry {
				Level = i + 1,
				TotalExp = table.ExpForLevel[i],
				ExpToReach = i == 0 ? table.ExpForLevel[0] : table.ExpForLevel[i] - table.ExpForLevel[i - 1]
			});
		}

		return json;
	}

	/// <summary>
	/// Convert all experience tables to JSON string.
	/// </summary>
	public static string ToJsonString(ExperienceTableCollection collection) {
		var jsonList = collection.Tables.Select(ToJson).ToList();
		return JsonSerializer.Serialize(jsonList, JsonOptions);
	}

	/// <summary>
	/// Save experience tables to a JSON file.
	/// </summary>
	public static void SaveToFile(ExperienceTableCollection collection, string filePath) {
		var json = ToJsonString(collection);
		File.WriteAllText(filePath, json);
	}

	/// <summary>
	/// Load experience tables from a JSON file.
	/// </summary>
	public static ExperienceTableCollection LoadFromFile(string filePath) {
		var json = File.ReadAllText(filePath);
		var jsonList = JsonSerializer.Deserialize<List<ExpTableJson>>(json, JsonOptions)
			?? throw new InvalidOperationException("Failed to deserialize experience table JSON");

		var collection = new ExperienceTableCollection();
		foreach (var jsonTable in jsonList) {
			collection.Tables.Add(FromJson(jsonTable));
		}

		return collection;
	}

	/// <summary>
	/// Convert JSON object back to ExperienceTable.
	/// </summary>
	public static ExperienceTable FromJson(ExpTableJson json) {
		var table = new ExperienceTable {
			CharacterId = json.CharacterId,
			CharacterName = json.CharacterName
		};

		foreach (var level in json.Levels) {
			table.ExpForLevel.Add(level.TotalExp);
		}

		return table;
	}
}

/// <summary>
/// JSON-friendly experience table representation.
/// </summary>
public class ExpTableJson {
	public int CharacterId { get; set; }
	public string CharacterName { get; set; } = "";
	public int MaxLevel { get; set; }
	public List<LevelEntry> Levels { get; set; } = new();
}

/// <summary>
/// Individual level entry in JSON format.
/// </summary>
public class LevelEntry {
	public int Level { get; set; }
	public uint TotalExp { get; set; }
	public uint ExpToReach { get; set; }
}
