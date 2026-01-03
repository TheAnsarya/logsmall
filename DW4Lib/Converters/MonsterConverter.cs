using DW4Lib.DataStructures;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 Monster data to JSON format for editing and DQ3r conversion.
/// </summary>
public static class MonsterConverter {
	private static readonly JsonSerializerOptions JsonOptions = new() {
		WriteIndented = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = { new JsonStringEnumConverter() }
	};

	/// <summary>
	/// Export monster to a JSON-friendly object with all calculated properties.
	/// </summary>
	public static MonsterJson ToJson(Monster monster, int index, string? name = null) {
		return new MonsterJson {
			Index = index,
			Name = name ?? $"Monster_{index:D3}",
			Experience = monster.Experience,
			Gold = monster.Gold,
			HitPoints = monster.HitPoints,
			Attack = monster.Attack,
			Defense = monster.Defense,
			Agility = monster.Agility,
			ItemDrop = monster.ItemDropId,
			StatusFlags = monster.StatusVulnerability,
			IsMetal = monster.IsMetal,
			// Store raw bytes for unknown fields
			RawBytes = new MonsterRawBytes {
				SkillData = monster.SkillData,
				BehaviorData = monster.BehaviorData,
				Unknown20 = monster.Unknown20,
				Unknown21 = monster.Unknown21,
				MetalFlags = monster.MetalFlags,
				DropRateFlags = monster.DropRateFlags,
				Unknown25 = monster.Unknown25,
				Unknown26 = monster.Unknown26
			}
		};
	}

	/// <summary>
	/// Convert a list of monsters to JSON string.
	/// </summary>
	public static string ToJsonString(IEnumerable<Monster> monsters, IEnumerable<string>? names = null) {
		var nameList = names?.ToList();
		var jsonList = monsters.Select((m, i) =>
			ToJson(m, i, nameList != null && i < nameList.Count ? nameList[i] : null)).ToList();

		return JsonSerializer.Serialize(jsonList, JsonOptions);
	}

	/// <summary>
	/// Save monsters to a JSON file.
	/// </summary>
	public static void SaveToFile(IEnumerable<Monster> monsters, string filePath, IEnumerable<string>? names = null) {
		var json = ToJsonString(monsters, names);
		File.WriteAllText(filePath, json);
	}

	/// <summary>
	/// Load monsters from a JSON file.
	/// </summary>
	public static List<Monster> LoadFromFile(string filePath) {
		var json = File.ReadAllText(filePath);
		var jsonList = JsonSerializer.Deserialize<List<MonsterJson>>(json, JsonOptions)
			?? throw new InvalidOperationException("Failed to deserialize monster JSON");

		return jsonList.Select(FromJson).ToList();
	}

	/// <summary>
	/// Convert JSON object back to Monster.
	/// </summary>
	public static Monster FromJson(MonsterJson json) {
		var monster = new Monster {
			Experience = json.Experience,
			Gold = json.Gold,
			HitPoints = json.HitPoints,
			Attack = json.Attack,
			Defense = json.Defense,
			Agility = json.Agility,
			ItemDropId = json.ItemDrop,
			Unknown20 = json.RawBytes.Unknown20,
			Unknown21 = json.RawBytes.Unknown21,
			MetalFlags = json.RawBytes.MetalFlags,
			DropRateFlags = json.RawBytes.DropRateFlags,
			StatusVulnerability = json.StatusFlags,
			Unknown25 = json.RawBytes.Unknown25,
			Unknown26 = json.RawBytes.Unknown26
		};

		// Copy skill and behavior data if present
		if (json.RawBytes.SkillData != null) {
			Array.Copy(json.RawBytes.SkillData, monster.SkillData, Math.Min(6, json.RawBytes.SkillData.Length));
		}
		if (json.RawBytes.BehaviorData != null) {
			Array.Copy(json.RawBytes.BehaviorData, monster.BehaviorData, Math.Min(4, json.RawBytes.BehaviorData.Length));
		}

		return monster;
	}
}

/// <summary>
/// JSON-friendly monster representation.
/// </summary>
public class MonsterJson {
	public int Index { get; set; }
	public string Name { get; set; } = "";
	public ushort Experience { get; set; }
	public ushort Gold { get; set; }
	public ushort HitPoints { get; set; }
	public byte Attack { get; set; }
	public byte Defense { get; set; }
	public byte Agility { get; set; }
	public byte ItemDrop { get; set; }
	public byte StatusFlags { get; set; }
	public bool IsMetal { get; set; }
	public MonsterRawBytes RawBytes { get; set; } = new();
}

/// <summary>
/// Raw byte storage for monster data fields with research-documented names.
/// </summary>
public class MonsterRawBytes {
	public byte[] SkillData { get; set; } = new byte[6];
	public byte[] BehaviorData { get; set; } = new byte[4];
	public byte Unknown20 { get; set; }
	public byte Unknown21 { get; set; }
	public byte MetalFlags { get; set; }
	public byte DropRateFlags { get; set; }
	public byte Unknown25 { get; set; }
	public byte Unknown26 { get; set; }
}
