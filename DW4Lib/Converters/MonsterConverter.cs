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
			Attack = monster.Attack,
			Defense = monster.Defense,
			Agility = monster.Agility,
			ItemDrop = monster.ItemDrop,
			StatusFlags = monster.StatusFlags,
			IsMetal = monster.IsMetal,
			// Store raw bytes for unknown fields
			RawBytes = new MonsterRawBytes {
				Byte4 = monster.Byte4,
				Byte5 = monster.Byte5,
				Byte9 = monster.Byte9,
				Byte10 = monster.Byte10,
				Byte11 = monster.Byte11,
				Byte12 = monster.Byte12,
				Byte13 = monster.Byte13,
				Byte14 = monster.Byte14,
				Byte15 = monster.Byte15,
				Byte16 = monster.Byte16,
				Byte17 = monster.Byte17,
				Byte18 = monster.Byte18,
				Byte19 = monster.Byte19,
				Byte20 = monster.Byte20,
				MetalFlag = monster.MetalFlag,
				Byte24 = monster.Byte24,
				Byte25 = monster.Byte25,
				Byte26 = monster.Byte26
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
		return new Monster {
			Experience = json.Experience,
			Gold = json.Gold,
			Attack = json.Attack,
			Defense = json.Defense,
			Agility = json.Agility,
			ItemDrop = json.ItemDrop,
			StatusFlags = json.StatusFlags,
			// Restore raw bytes
			Byte4 = json.RawBytes.Byte4,
			Byte5 = json.RawBytes.Byte5,
			Byte9 = json.RawBytes.Byte9,
			Byte10 = json.RawBytes.Byte10,
			Byte11 = json.RawBytes.Byte11,
			Byte12 = json.RawBytes.Byte12,
			Byte13 = json.RawBytes.Byte13,
			Byte14 = json.RawBytes.Byte14,
			Byte15 = json.RawBytes.Byte15,
			Byte16 = json.RawBytes.Byte16,
			Byte17 = json.RawBytes.Byte17,
			Byte18 = json.RawBytes.Byte18,
			Byte19 = json.RawBytes.Byte19,
			Byte20 = json.RawBytes.Byte20,
			MetalFlag = json.RawBytes.MetalFlag,
			Byte24 = json.RawBytes.Byte24,
			Byte25 = json.RawBytes.Byte25,
			Byte26 = json.RawBytes.Byte26
		};
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
	public byte Attack { get; set; }
	public byte Defense { get; set; }
	public byte Agility { get; set; }
	public byte ItemDrop { get; set; }
	public byte StatusFlags { get; set; }
	public bool IsMetal { get; set; }
	public MonsterRawBytes RawBytes { get; set; } = new();
}

/// <summary>
/// Raw byte storage for unknown monster data fields.
/// </summary>
public class MonsterRawBytes {
	public byte Byte4 { get; set; }
	public byte Byte5 { get; set; }
	public byte Byte9 { get; set; }
	public byte Byte10 { get; set; }
	public byte Byte11 { get; set; }
	public byte Byte12 { get; set; }
	public byte Byte13 { get; set; }
	public byte Byte14 { get; set; }
	public byte Byte15 { get; set; }
	public byte Byte16 { get; set; }
	public byte Byte17 { get; set; }
	public byte Byte18 { get; set; }
	public byte Byte19 { get; set; }
	public byte Byte20 { get; set; }
	public byte MetalFlag { get; set; }
	public byte Byte24 { get; set; }
	public byte Byte25 { get; set; }
	public byte Byte26 { get; set; }
}
