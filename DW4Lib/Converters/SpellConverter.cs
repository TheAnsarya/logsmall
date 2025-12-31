using DW4Lib.DataStructures;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 Spell data to JSON format for editing and DQ3r conversion.
/// </summary>
public static class SpellConverter {
	private static readonly JsonSerializerOptions JsonOptions = new() {
		WriteIndented = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = { new JsonStringEnumConverter() }
	};

	/// <summary>
	/// Export spell to a JSON-friendly object with all calculated properties.
	/// </summary>
	public static SpellJson ToJson(Spell spell, int index, string? name = null) {
		return new SpellJson {
			Index = index,
			Name = name ?? $"Spell_{index:D3}",
			NameIndex = spell.NameIndex,
			MPCost = spell.MPCost,
			BasePower = spell.BasePower,
			Target = spell.Target,
			Type = spell.Type,
			Element = spell.Element,
			SecondaryEffect = spell.SecondaryEffect,
			SuccessRate = spell.SuccessRate
		};
	}

	/// <summary>
	/// Convert a list of spells to JSON string.
	/// </summary>
	public static string ToJsonString(IEnumerable<Spell> spells, IEnumerable<string>? names = null) {
		var nameList = names?.ToList();
		var jsonList = spells.Select((spell, i) =>
			ToJson(spell, i, nameList != null && i < nameList.Count ? nameList[i] : null)).ToList();

		return JsonSerializer.Serialize(jsonList, JsonOptions);
	}

	/// <summary>
	/// Save spells to a JSON file.
	/// </summary>
	public static void SaveToFile(IEnumerable<Spell> spells, string filePath, IEnumerable<string>? names = null) {
		var json = ToJsonString(spells, names);
		File.WriteAllText(filePath, json);
	}

	/// <summary>
	/// Load spells from a JSON file.
	/// </summary>
	public static List<Spell> LoadFromFile(string filePath) {
		var json = File.ReadAllText(filePath);
		var jsonList = JsonSerializer.Deserialize<List<SpellJson>>(json, JsonOptions)
			?? throw new InvalidOperationException("Failed to deserialize spell JSON");

		return jsonList.Select(FromJson).ToList();
	}

	/// <summary>
	/// Convert JSON object back to Spell.
	/// </summary>
	public static Spell FromJson(SpellJson json) {
		byte typeFlags = (byte)(
			((byte)json.Target & 0x07) |
			(((byte)json.Type & 0x07) << 3) |
			(((byte)json.Element & 0x03) << 6)
		);

		return new Spell {
			NameIndex = json.NameIndex,
			MPCost = json.MPCost,
			BasePower = json.BasePower,
			TypeFlags = typeFlags,
			SecondaryEffect = json.SecondaryEffect,
			SuccessRate = json.SuccessRate
		};
	}
}

/// <summary>
/// JSON-friendly spell representation.
/// </summary>
public class SpellJson {
	public int Index { get; set; }
	public string Name { get; set; } = "";
	public byte NameIndex { get; set; }
	public byte MPCost { get; set; }
	public byte BasePower { get; set; }
	public SpellTarget Target { get; set; }
	public SpellType Type { get; set; }
	public SpellElement Element { get; set; }
	public byte SecondaryEffect { get; set; }
	public byte SuccessRate { get; set; }
}
