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
			NameIndex = monster.NameIndex,
			HP = monster.HP,
			Attack = monster.Attack,
			Defense = monster.Defense,
			Agility = monster.Agility,
			Experience = monster.Experience,
			Gold = monster.Gold,
			ItemDrop = monster.ItemDrop,
			DropRate = monster.DropRate,
			Spell1 = monster.Spell1,
			Spell2 = monster.Spell2,
			AIPattern = monster.AIPattern,
			Resistances = ParseResistances(monster.Resistances),
			SpriteID = monster.SpriteID
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
			NameIndex = json.NameIndex,
			HP = json.HP,
			Attack = json.Attack,
			Defense = json.Defense,
			Agility = json.Agility,
			Experience = json.Experience,
			Gold = json.Gold,
			ItemDrop = json.ItemDrop,
			DropRate = json.DropRate,
			Spell1 = json.Spell1,
			Spell2 = json.Spell2,
			AIPattern = json.AIPattern,
			Resistances = PackResistances(json.Resistances),
			SpriteID = json.SpriteID
		};
	}

	/// <summary>
	/// Parse resistance byte into named flags.
	/// </summary>
	private static ResistanceFlags ParseResistances(byte resistances) {
		return new ResistanceFlags {
			Fire = (resistances & 0x01) != 0,
			Ice = (resistances & 0x02) != 0,
			Wind = (resistances & 0x04) != 0,
			Lightning = (resistances & 0x08) != 0,
			Sleep = (resistances & 0x10) != 0,
			Stopspell = (resistances & 0x20) != 0,
			Death = (resistances & 0x40) != 0,
			Drain = (resistances & 0x80) != 0
		};
	}

	/// <summary>
	/// Pack resistance flags back into byte.
	/// </summary>
	private static byte PackResistances(ResistanceFlags flags) {
		byte result = 0;
		if (flags.Fire) result |= 0x01;
		if (flags.Ice) result |= 0x02;
		if (flags.Wind) result |= 0x04;
		if (flags.Lightning) result |= 0x08;
		if (flags.Sleep) result |= 0x10;
		if (flags.Stopspell) result |= 0x20;
		if (flags.Death) result |= 0x40;
		if (flags.Drain) result |= 0x80;
		return result;
	}
}

/// <summary>
/// JSON-friendly monster representation.
/// </summary>
public class MonsterJson {
	public int Index { get; set; }
	public string Name { get; set; } = "";
	public byte NameIndex { get; set; }
	public byte HP { get; set; }
	public byte Attack { get; set; }
	public byte Defense { get; set; }
	public byte Agility { get; set; }
	public ushort Experience { get; set; }
	public ushort Gold { get; set; }
	public byte ItemDrop { get; set; }
	public byte DropRate { get; set; }
	public byte Spell1 { get; set; }
	public byte Spell2 { get; set; }
	public byte AIPattern { get; set; }
	public ResistanceFlags Resistances { get; set; } = new();
	public byte SpriteID { get; set; }
}

/// <summary>
/// Named resistance flags for JSON readability.
/// </summary>
public class ResistanceFlags {
	public bool Fire { get; set; }
	public bool Ice { get; set; }
	public bool Wind { get; set; }
	public bool Lightning { get; set; }
	public bool Sleep { get; set; }
	public bool Stopspell { get; set; }
	public bool Death { get; set; }
	public bool Drain { get; set; }
}
