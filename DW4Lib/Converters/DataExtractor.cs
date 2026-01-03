using System.Text.Json;
using System.Text.Json.Serialization;
using DW4Lib.DataStructures;

namespace DW4Lib.Converters;

/// <summary>
/// Extracts game data from Dragon Warrior IV NES ROM and exports to JSON.
/// </summary>
public class DataExtractor {
	private readonly byte[] _romData;
	private const int HeaderSize = 16;

	/// <summary>
	/// Initialize extractor with ROM data.
	/// </summary>
	public DataExtractor(byte[] romData) {
		_romData = romData ?? throw new ArgumentNullException(nameof(romData));
	}

	/// <summary>
	/// Load ROM from file path.
	/// </summary>
	public static DataExtractor FromFile(string path) {
		return new DataExtractor(File.ReadAllBytes(path));
	}

	/// <summary>
	/// Convert bank number and CPU address to file offset.
	/// </summary>
	public static int BankAddressToFileOffset(int bank, int cpuAddress) {
		// CPU address $8000-$BFFF maps to the switchable bank
		// CPU address $C000-$FFFF maps to the fixed bank (bank $1F)
		int bankOffset = bank * 0x4000; // 16KB per bank
		int addressOffset = cpuAddress - 0x8000;
		return HeaderSize + bankOffset + addressOffset;
	}

	/// <summary>
	/// Extract all monsters from the ROM.
	/// </summary>
	public List<MonsterExport> ExtractMonsters(int count = 180) {
		var monsters = new List<MonsterExport>();

		// Monster table at Bank 6, $A2A2
		int fileOffset = BankAddressToFileOffset(6, 0xA2A2);

		for (int i = 0; i < count; i++) {
			int offset = fileOffset + (i * Monster.Size);
			if (offset + Monster.Size > _romData.Length) break;

			var monsterData = new byte[Monster.Size];
			Array.Copy(_romData, offset, monsterData, 0, Monster.Size);

			var monster = Monster.FromBytes(monsterData);
			monsters.Add(new MonsterExport {
				Id = i,
				RomOffset = $"0x{offset:x5}",
				Experience = monster.Experience,
				Gold = monster.Gold,
				HitPoints = monster.HitPoints,
				Attack = monster.Attack,
				Defense = monster.Defense,
				Agility = monster.Agility,
				ItemDropId = monster.ItemDropId,
				StatusFlags = monster.StatusVulnerability,
				IsMetal = monster.IsMetal,
				RawHex = BitConverter.ToString(monsterData).Replace("-", " ")
			});
		}

		return monsters;
	}

	/// <summary>
	/// Extract all items from the ROM.
	/// </summary>
	public List<ItemExport> ExtractItems(int count = 220) {
		var items = new List<ItemExport>();

		// Item table at Bank 7, $8000
		int fileOffset = BankAddressToFileOffset(7, 0x8000);
		const int itemSize = 8;

		for (int i = 0; i < count; i++) {
			int offset = fileOffset + (i * itemSize);
			if (offset + itemSize > _romData.Length) break;

			items.Add(new ItemExport {
				Id = i,
				RomOffset = $"0x{offset:x5}",
				Type = _romData[offset],
				Modifier = (sbyte)_romData[offset + 1],
				SpecialFlags = _romData[offset + 2],
				BuyPrice = (ushort)((_romData[offset + 3] << 8) | _romData[offset + 4]),
				SellPrice = (ushort)((_romData[offset + 5] << 8) | _romData[offset + 6]),
				EquipFlags = _romData[offset + 7],
				RawHex = BitConverter.ToString(_romData, offset, itemSize).Replace("-", " ")
			});
		}

		return items;
	}

	/// <summary>
	/// Extract experience tables for all characters.
	/// </summary>
	public Dictionary<string, List<int>> ExtractExpTables() {
		var tables = new Dictionary<string, List<int>>();
		string[] characters = ["Hero", "Ragnar", "Alena", "Cristo", "Brey", "Taloon", "Nara", "Mara"];

		// EXP table at Bank $27, $B6ED
		int fileOffset = BankAddressToFileOffset(0x27, 0xB6ED);
		const int levelsPerChar = 99;
		const int bytesPerEntry = 3;

		for (int c = 0; c < characters.Length; c++) {
			var expList = new List<int>();
			int charOffset = fileOffset + (c * levelsPerChar * bytesPerEntry);

			for (int level = 0; level < levelsPerChar; level++) {
				int entryOffset = charOffset + (level * bytesPerEntry);
				if (entryOffset + bytesPerEntry > _romData.Length) break;

				// 24-bit little-endian EXP value
				int exp = _romData[entryOffset]
					| (_romData[entryOffset + 1] << 8)
					| (_romData[entryOffset + 2] << 16);
				expList.Add(exp);
			}

			tables[characters[c]] = expList;
		}

		return tables;
	}

	/// <summary>
	/// Extract spells from the ROM.
	/// </summary>
	public List<SpellExport> ExtractSpells(int count = 64) {
		var spells = new List<SpellExport>();

		// Spell table at Bank 5, $8000 (estimated)
		int fileOffset = BankAddressToFileOffset(5, 0x8000);
		const int spellSize = 4;

		for (int i = 0; i < count; i++) {
			int offset = fileOffset + (i * spellSize);
			if (offset + spellSize > _romData.Length) break;

			spells.Add(new SpellExport {
				Id = i,
				RomOffset = $"0x{offset:x5}",
				EffectType = _romData[offset],
				Power = _romData[offset + 1],
				TargetType = _romData[offset + 2],
				MpCost = _romData[offset + 3],
				RawHex = BitConverter.ToString(_romData, offset, spellSize).Replace("-", " ")
			});
		}

		return spells;
	}

	/// <summary>
	/// Extract CHR graphics data (NES 2bpp format).
	/// </summary>
	public byte[] ExtractChrData() {
		// CHR-ROM starts after PRG-ROM
		// PRG-ROM = 512KB = 0x80000
		int chrOffset = HeaderSize + 0x80000;
		int chrSize = 0x40000; // 256KB

		if (chrOffset + chrSize > _romData.Length) {
			chrSize = _romData.Length - chrOffset;
		}

		var chrData = new byte[chrSize];
		Array.Copy(_romData, chrOffset, chrData, 0, chrSize);
		return chrData;
	}

	/// <summary>
	/// Export all data to JSON files in the specified directory.
	/// </summary>
	public void ExportAll(string outputDir) {
		Directory.CreateDirectory(outputDir);

		var options = new JsonSerializerOptions {
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
		};

		// Export monsters
		var monsters = ExtractMonsters();
		var monstersJson = JsonSerializer.Serialize(new {
			extraction_info = new {
				source = "Dragon Warrior IV (US) NES",
				bank = "0x06",
				address = "0xA2A2",
				count = monsters.Count,
				entry_size = Monster.Size
			},
			monsters
		}, options);
		File.WriteAllText(Path.Combine(outputDir, "monsters_extracted.json"), monstersJson);

		// Export items
		var items = ExtractItems();
		var itemsJson = JsonSerializer.Serialize(new {
			extraction_info = new {
				source = "Dragon Warrior IV (US) NES",
				bank = "0x07",
				address = "0x8000",
				count = items.Count,
				entry_size = 8
			},
			items
		}, options);
		File.WriteAllText(Path.Combine(outputDir, "items_extracted.json"), itemsJson);

		// Export EXP tables
		var expTables = ExtractExpTables();
		var expJson = JsonSerializer.Serialize(new {
			extraction_info = new {
				source = "Dragon Warrior IV (US) NES",
				bank = "0x27",
				address = "0xB6ED",
				levels_per_character = 99,
				bytes_per_entry = 3
			},
			characters = expTables
		}, options);
		File.WriteAllText(Path.Combine(outputDir, "exp_tables_extracted.json"), expJson);

		// Export spells
		var spells = ExtractSpells();
		var spellsJson = JsonSerializer.Serialize(new {
			extraction_info = new {
				source = "Dragon Warrior IV (US) NES",
				bank = "0x05",
				address = "0x8000",
				count = spells.Count,
				entry_size = 4
			},
			spells
		}, options);
		File.WriteAllText(Path.Combine(outputDir, "spells_extracted.json"), spellsJson);
	}
}

/// <summary>
/// Monster data export format.
/// </summary>
public class MonsterExport {
	public int Id { get; set; }
	public string? RomOffset { get; set; }
	public int Experience { get; set; }
	public int Gold { get; set; }
	public int HitPoints { get; set; }
	public int Attack { get; set; }
	public int Defense { get; set; }
	public int Agility { get; set; }
	public int ItemDropId { get; set; }
	public int StatusFlags { get; set; }
	public bool IsMetal { get; set; }
	public string? RawHex { get; set; }
}

/// <summary>
/// Item data export format.
/// </summary>
public class ItemExport {
	public int Id { get; set; }
	public string? RomOffset { get; set; }
	public int Type { get; set; }
	public int Modifier { get; set; }
	public int SpecialFlags { get; set; }
	public int BuyPrice { get; set; }
	public int SellPrice { get; set; }
	public int EquipFlags { get; set; }
	public string? RawHex { get; set; }
}

/// <summary>
/// Spell data export format.
/// </summary>
public class SpellExport {
	public int Id { get; set; }
	public string? RomOffset { get; set; }
	public int EffectType { get; set; }
	public int Power { get; set; }
	public int TargetType { get; set; }
	public int MpCost { get; set; }
	public string? RawHex { get; set; }
}
