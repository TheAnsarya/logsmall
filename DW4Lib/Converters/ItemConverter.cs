using DW4Lib.DataStructures;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 Item data to JSON format for editing and DQ3r conversion.
/// </summary>
public static class ItemConverter {
	private static readonly JsonSerializerOptions JsonOptions = new() {
		WriteIndented = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		Converters = { new JsonStringEnumConverter() }
	};

	/// <summary>
	/// Export item to a JSON-friendly object with all calculated properties.
	/// </summary>
	public static ItemJson ToJson(Item item, int index, string? name = null) {
		return new ItemJson {
			Index = index,
			Name = name ?? $"Item_{index:D3}",
			NameIndex = item.NameIndex,
			Type = item.EquipmentSlot,
			StatBonus = item.StatBonus,
			SpecialEffect = item.SpecialEffect,
			Price = item.Price,
			EquippableBy = ParseEquipFlags(item.EquipFlags),
			IconID = item.IconID
		};
	}

	/// <summary>
	/// Convert a list of items to JSON string.
	/// </summary>
	public static string ToJsonString(IEnumerable<Item> items, IEnumerable<string>? names = null) {
		var nameList = names?.ToList();
		var jsonList = items.Select((item, i) =>
			ToJson(item, i, nameList != null && i < nameList.Count ? nameList[i] : null)).ToList();

		return JsonSerializer.Serialize(jsonList, JsonOptions);
	}

	/// <summary>
	/// Save items to a JSON file.
	/// </summary>
	public static void SaveToFile(IEnumerable<Item> items, string filePath, IEnumerable<string>? names = null) {
		var json = ToJsonString(items, names);
		File.WriteAllText(filePath, json);
	}

	/// <summary>
	/// Load items from a JSON file.
	/// </summary>
	public static List<Item> LoadFromFile(string filePath) {
		var json = File.ReadAllText(filePath);
		var jsonList = JsonSerializer.Deserialize<List<ItemJson>>(json, JsonOptions)
			?? throw new InvalidOperationException("Failed to deserialize item JSON");

		return jsonList.Select(FromJson).ToList();
	}

	/// <summary>
	/// Convert JSON object back to Item.
	/// </summary>
	public static Item FromJson(ItemJson json) {
		return new Item {
			NameIndex = json.NameIndex,
			TypeFlags = (byte)json.Type,
			StatBonus = json.StatBonus,
			SpecialEffect = json.SpecialEffect,
			Price = json.Price,
			EquipFlags = PackEquipFlags(json.EquippableBy),
			IconID = json.IconID
		};
	}

	/// <summary>
	/// Parse equip flags into named character list.
	/// </summary>
	private static EquipFlags ParseEquipFlags(byte flags) {
		return new EquipFlags {
			Hero = (flags & 0x01) != 0,
			Ragnar = (flags & 0x02) != 0,
			Alena = (flags & 0x04) != 0,
			Cristo = (flags & 0x08) != 0,
			Brey = (flags & 0x10) != 0,
			Taloon = (flags & 0x20) != 0,
			Mara = (flags & 0x40) != 0,
			Nara = (flags & 0x80) != 0
		};
	}

	/// <summary>
	/// Pack equip flags back into byte.
	/// </summary>
	private static byte PackEquipFlags(EquipFlags flags) {
		byte result = 0;
		if (flags.Hero) result |= 0x01;
		if (flags.Ragnar) result |= 0x02;
		if (flags.Alena) result |= 0x04;
		if (flags.Cristo) result |= 0x08;
		if (flags.Brey) result |= 0x10;
		if (flags.Taloon) result |= 0x20;
		if (flags.Mara) result |= 0x40;
		if (flags.Nara) result |= 0x80;
		return result;
	}
}

/// <summary>
/// JSON-friendly item representation.
/// </summary>
public class ItemJson {
	public int Index { get; set; }
	public string Name { get; set; } = "";
	public byte NameIndex { get; set; }
	public ItemType Type { get; set; }
	public byte StatBonus { get; set; }
	public byte SpecialEffect { get; set; }
	public ushort Price { get; set; }
	public EquipFlags EquippableBy { get; set; } = new();
	public byte IconID { get; set; }
}

/// <summary>
/// Named equip flags for JSON readability.
/// </summary>
public class EquipFlags {
	public bool Hero { get; set; }
	public bool Ragnar { get; set; }
	public bool Alena { get; set; }
	public bool Cristo { get; set; }
	public bool Brey { get; set; }
	public bool Taloon { get; set; }
	public bool Mara { get; set; }
	public bool Nara { get; set; }
}
