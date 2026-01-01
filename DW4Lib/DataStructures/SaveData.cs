namespace DW4Lib.DataStructures;

/// <summary>
/// Dragon Warrior IV save data structure.
/// Based on the 752-byte save slot format.
/// </summary>
public class SaveData {
	/// <summary>
	/// Save slot size in bytes.
	/// </summary>
	public const int SaveSlotSize = 752;

	/// <summary>
	/// Number of save slots.
	/// </summary>
	public const int SaveSlotCount = 3;

	/// <summary>
	/// SRAM start address.
	/// </summary>
	public const int SramBase = 0x6000;

	/// <summary>
	/// Save file header data.
	/// </summary>
	public SaveHeader Header { get; set; } = new();

	/// <summary>
	/// Party configuration.
	/// </summary>
	public PartyConfig Party { get; set; } = new();

	/// <summary>
	/// Character data (8 characters).
	/// </summary>
	public CharacterSaveData[] Characters { get; set; } = new CharacterSaveData[8];

	/// <summary>
	/// Equipment data (8 characters).
	/// </summary>
	public EquipmentData[] Equipment { get; set; } = new EquipmentData[8];

	/// <summary>
	/// Inventory items.
	/// </summary>
	public InventoryData Inventory { get; set; } = new();

	/// <summary>
	/// Spells known per character.
	/// </summary>
	public SpellFlags[] SpellsKnown { get; set; } = new SpellFlags[8];

	/// <summary>
	/// World state (event flags, chests).
	/// </summary>
	public WorldState World { get; set; } = new();

	/// <summary>
	/// Create a new save data with default values.
	/// </summary>
	public SaveData() {
		for (int i = 0; i < 8; i++) {
			Characters[i] = new CharacterSaveData();
			Equipment[i] = new EquipmentData();
			SpellsKnown[i] = new SpellFlags();
		}
	}

	/// <summary>
	/// Create Chapter 1 starting save.
	/// </summary>
	public static SaveData CreateChapter1Start() {
		var save = new SaveData {
			Header = new SaveHeader {
				Chapter = 0x00,
				SubChapterProgress = 0,
				Gold = 0,
				CasinoCoins = 0,
				PlayTimeFrames = 0,
				DayNightCycle = 0x40 // Day
			}
		};

		// Party: Just Ragnar
		save.Party = new PartyConfig {
			ActiveParty = [0x06, 0xFF, 0xFF, 0xFF], // Ragnar only
			WagonParty = [0xFF, 0xFF, 0xFF, 0xFF],
			PartyCount = 1,
			WagonCount = 0
		};

		// Ragnar starting stats
		save.Characters[0] = new CharacterSaveData {
			Name = "Ragnar\0\0",
			Level = 1,
			Experience = 0,
			CurrentHP = 30,
			MaxHP = 30,
			CurrentMP = 0,
			MaxMP = 0,
			Strength = 12,
			Agility = 6,
			Vitality = 10,
			Intelligence = 4,
			Luck = 5,
			StatusEffects = 0
		};

		// No equipment at start
		save.Equipment[0] = new EquipmentData();

		// No spells (Ragnar has no magic)
		save.SpellsKnown[0] = new SpellFlags();

		// Starting event flags (chapter 1 started)
		save.World = new WorldState();
		save.World.SetEventFlag(0x0001); // Chapter 1 started

		return save;
	}

	/// <summary>
	/// Create Chapter 2 starting save.
	/// </summary>
	public static SaveData CreateChapter2Start() {
		var save = new SaveData {
			Header = new SaveHeader {
				Chapter = 0x01,
				SubChapterProgress = 0,
				Gold = 0,
				CasinoCoins = 0,
				PlayTimeFrames = 0,
				DayNightCycle = 0x40
			}
		};

		// Party: Alena, Cristo, Brey
		save.Party = new PartyConfig {
			ActiveParty = [0x07, 0x01, 0x04, 0xFF], // Alena, Cristo, Brey
			WagonParty = [0xFF, 0xFF, 0xFF, 0xFF],
			PartyCount = 3,
			WagonCount = 0
		};

		// Alena (slot 0 for chapter 2)
		save.Characters[0] = new CharacterSaveData {
			Name = "Alena\0\0\0",
			Level = 1,
			Experience = 0,
			CurrentHP = 25,
			MaxHP = 25,
			CurrentMP = 0,
			MaxMP = 0,
			Strength = 10,
			Agility = 14,
			Vitality = 8,
			Intelligence = 6,
			Luck = 8,
			StatusEffects = 0
		};

		// Cristo (slot 1)
		save.Characters[1] = new CharacterSaveData {
			Name = "Cristo\0\0",
			Level = 1,
			Experience = 0,
			CurrentHP = 22,
			MaxHP = 22,
			CurrentMP = 8,
			MaxMP = 8,
			Strength = 8,
			Agility = 6,
			Vitality = 7,
			Intelligence = 10,
			Luck = 7,
			StatusEffects = 0
		};

		// Brey (slot 2)
		save.Characters[2] = new CharacterSaveData {
			Name = "Brey\0\0\0\0",
			Level = 1,
			Experience = 0,
			CurrentHP = 18,
			MaxHP = 18,
			CurrentMP = 12,
			MaxMP = 12,
			Strength = 4,
			Agility = 8,
			Vitality = 5,
			Intelligence = 15,
			Luck = 10,
			StatusEffects = 0
		};

		// Chapter 2 started
		save.World = new WorldState();
		save.World.SetEventFlag(0x0100); // Chapter 1 complete
		save.World.SetEventFlag(0x0201); // Chapter 2 started

		return save;
	}

	/// <summary>
	/// Write save data to byte array.
	/// </summary>
	public byte[] ToBytes() {
		var data = new byte[SaveSlotSize];

		// Header (0x00-0x0F)
		Array.Copy(Header.ToBytes(), 0, data, 0, 16);

		// Party (0x10-0x2F)
		Array.Copy(Party.ToBytes(), 0, data, 0x10, 32);

		// Characters (0x20-0x11F) - 8 × 32 bytes
		for (int i = 0; i < 8; i++) {
			Array.Copy(Characters[i].ToBytes(), 0, data, 0x20 + (i * 32), 32);
		}

		// Equipment (0x120-0x15F) - 8 × 8 bytes
		for (int i = 0; i < 8; i++) {
			Array.Copy(Equipment[i].ToBytes(), 0, data, 0x120 + (i * 8), 8);
		}

		// Inventory (0x160-0x1BF)
		Array.Copy(Inventory.ToBytes(), 0, data, 0x160, 96);

		// Spells (0x1C0-0x1FF) - 8 × 8 bytes
		for (int i = 0; i < 8; i++) {
			Array.Copy(SpellsKnown[i].ToBytes(), 0, data, 0x1C0 + (i * 8), 8);
		}

		// World state (0x200-0x2DF)
		Array.Copy(World.ToBytes(), 0, data, 0x200, 224);

		// Calculate and set checksum
		ushort checksum = CalculateChecksum(data);
		data[0] = (byte)(checksum & 0xFF);
		data[1] = (byte)(checksum >> 8);

		return data;
	}

	/// <summary>
	/// Read save data from byte array.
	/// </summary>
	public static SaveData FromBytes(byte[] data) {
		if (data.Length < SaveSlotSize) {
			throw new ArgumentException($"Data too short: {data.Length} < {SaveSlotSize}");
		}

		var save = new SaveData();

		// Header
		save.Header = SaveHeader.FromBytes(data[0..16]);

		// Party
		save.Party = PartyConfig.FromBytes(data[0x10..0x30]);

		// Characters
		for (int i = 0; i < 8; i++) {
			save.Characters[i] = CharacterSaveData.FromBytes(data[(0x20 + i * 32)..(0x20 + (i + 1) * 32)]);
		}

		// Equipment
		for (int i = 0; i < 8; i++) {
			save.Equipment[i] = EquipmentData.FromBytes(data[(0x120 + i * 8)..(0x120 + (i + 1) * 8)]);
		}

		// Inventory
		save.Inventory = InventoryData.FromBytes(data[0x160..0x1C0]);

		// Spells
		for (int i = 0; i < 8; i++) {
			save.SpellsKnown[i] = SpellFlags.FromBytes(data[(0x1C0 + i * 8)..(0x1C0 + (i + 1) * 8)]);
		}

		// World
		save.World = WorldState.FromBytes(data[0x200..0x2E0]);

		return save;
	}

	/// <summary>
	/// Calculate save file checksum.
	/// </summary>
	private static ushort CalculateChecksum(byte[] data) {
		ushort sum = 0;
		for (int i = 2; i < SaveSlotSize; i++) {
			sum += data[i];
		}
		return (ushort)(sum ^ 0xFFFF);
	}
}

/// <summary>
/// Save file header (16 bytes).
/// </summary>
public class SaveHeader {
	public ushort Checksum { get; set; }
	public byte Chapter { get; set; }
	public byte SubChapterProgress { get; set; }
	public ushort Gold { get; set; }
	public ushort CasinoCoins { get; set; }
	public uint PlayTimeFrames { get; set; }
	public ushort DayNightCycle { get; set; }

	public byte[] ToBytes() {
		var data = new byte[16];
		data[0] = (byte)(Checksum & 0xFF);
		data[1] = (byte)(Checksum >> 8);
		data[2] = Chapter;
		data[3] = SubChapterProgress;
		data[4] = (byte)(Gold & 0xFF);
		data[5] = (byte)(Gold >> 8);
		data[6] = (byte)(CasinoCoins & 0xFF);
		data[7] = (byte)(CasinoCoins >> 8);
		data[8] = (byte)(PlayTimeFrames & 0xFF);
		data[9] = (byte)((PlayTimeFrames >> 8) & 0xFF);
		data[10] = (byte)((PlayTimeFrames >> 16) & 0xFF);
		data[11] = (byte)((PlayTimeFrames >> 24) & 0xFF);
		data[12] = (byte)(DayNightCycle & 0xFF);
		data[13] = (byte)(DayNightCycle >> 8);
		return data;
	}

	public static SaveHeader FromBytes(byte[] data) => new() {
		Checksum = (ushort)(data[0] | (data[1] << 8)),
		Chapter = data[2],
		SubChapterProgress = data[3],
		Gold = (ushort)(data[4] | (data[5] << 8)),
		CasinoCoins = (ushort)(data[6] | (data[7] << 8)),
		PlayTimeFrames = (uint)(data[8] | (data[9] << 8) | (data[10] << 16) | (data[11] << 24)),
		DayNightCycle = (ushort)(data[12] | (data[13] << 8))
	};
}

/// <summary>
/// Party configuration (32 bytes).
/// </summary>
public class PartyConfig {
	public byte[] ActiveParty { get; set; } = [0xFF, 0xFF, 0xFF, 0xFF];
	public byte[] WagonParty { get; set; } = [0xFF, 0xFF, 0xFF, 0xFF];
	public byte PartyCount { get; set; }
	public byte WagonCount { get; set; }

	public byte[] ToBytes() {
		var data = new byte[32];
		Array.Copy(ActiveParty, 0, data, 0, 4);
		Array.Copy(WagonParty, 0, data, 4, 4);
		data[8] = PartyCount;
		data[9] = WagonCount;
		return data;
	}

	public static PartyConfig FromBytes(byte[] data) => new() {
		ActiveParty = [data[0], data[1], data[2], data[3]],
		WagonParty = [data[4], data[5], data[6], data[7]],
		PartyCount = data[8],
		WagonCount = data[9]
	};
}

/// <summary>
/// Character save data (32 bytes).
/// </summary>
public class CharacterSaveData {
	public string Name { get; set; } = "\0\0\0\0\0\0\0\0";
	public byte Level { get; set; }
	public uint Experience { get; set; }
	public ushort CurrentHP { get; set; }
	public ushort MaxHP { get; set; }
	public ushort CurrentMP { get; set; }
	public ushort MaxMP { get; set; }
	public ushort Strength { get; set; }
	public ushort Agility { get; set; }
	public ushort Vitality { get; set; }
	public ushort Intelligence { get; set; }
	public ushort Luck { get; set; }
	public byte StatusEffects { get; set; }

	public byte[] ToBytes() {
		var data = new byte[32];
		var nameBytes = System.Text.Encoding.ASCII.GetBytes(Name.PadRight(8, '\0')[..8]);
		Array.Copy(nameBytes, 0, data, 0, 8);
		data[8] = Level;
		data[9] = (byte)(Experience & 0xFF);
		data[10] = (byte)((Experience >> 8) & 0xFF);
		data[11] = (byte)((Experience >> 16) & 0xFF);
		data[12] = (byte)(CurrentHP & 0xFF);
		data[13] = (byte)(CurrentHP >> 8);
		data[14] = (byte)(MaxHP & 0xFF);
		data[15] = (byte)(MaxHP >> 8);
		data[16] = (byte)(CurrentMP & 0xFF);
		data[17] = (byte)(CurrentMP >> 8);
		data[18] = (byte)(MaxMP & 0xFF);
		data[19] = (byte)(MaxMP >> 8);
		data[20] = (byte)(Strength & 0xFF);
		data[21] = (byte)(Strength >> 8);
		data[22] = (byte)(Agility & 0xFF);
		data[23] = (byte)(Agility >> 8);
		data[24] = (byte)(Vitality & 0xFF);
		data[25] = (byte)(Vitality >> 8);
		data[26] = (byte)(Intelligence & 0xFF);
		data[27] = (byte)(Intelligence >> 8);
		data[28] = (byte)(Luck & 0xFF);
		data[29] = (byte)(Luck >> 8);
		data[30] = StatusEffects;
		return data;
	}

	public static CharacterSaveData FromBytes(byte[] data) => new() {
		Name = System.Text.Encoding.ASCII.GetString(data[0..8]).TrimEnd('\0'),
		Level = data[8],
		Experience = (uint)(data[9] | (data[10] << 8) | (data[11] << 16)),
		CurrentHP = (ushort)(data[12] | (data[13] << 8)),
		MaxHP = (ushort)(data[14] | (data[15] << 8)),
		CurrentMP = (ushort)(data[16] | (data[17] << 8)),
		MaxMP = (ushort)(data[18] | (data[19] << 8)),
		Strength = (ushort)(data[20] | (data[21] << 8)),
		Agility = (ushort)(data[22] | (data[23] << 8)),
		Vitality = (ushort)(data[24] | (data[25] << 8)),
		Intelligence = (ushort)(data[26] | (data[27] << 8)),
		Luck = (ushort)(data[28] | (data[29] << 8)),
		StatusEffects = data[30]
	};
}

/// <summary>
/// Equipment data (8 bytes per character).
/// </summary>
public class EquipmentData {
	public byte Weapon { get; set; }
	public byte Armor { get; set; }
	public byte Shield { get; set; }
	public byte Helmet { get; set; }
	public byte Accessory1 { get; set; }
	public byte Accessory2 { get; set; }

	public byte[] ToBytes() => [Weapon, Armor, Shield, Helmet, Accessory1, Accessory2, 0, 0];

	public static EquipmentData FromBytes(byte[] data) => new() {
		Weapon = data[0],
		Armor = data[1],
		Shield = data[2],
		Helmet = data[3],
		Accessory1 = data[4],
		Accessory2 = data[5]
	};
}

/// <summary>
/// Inventory data (96 bytes).
/// </summary>
public class InventoryData {
	public byte[] BagItems { get; set; } = new byte[64];
	public byte[] ImportantItems { get; set; } = new byte[32];

	public byte[] ToBytes() {
		var data = new byte[96];
		Array.Copy(BagItems, 0, data, 0, 64);
		Array.Copy(ImportantItems, 0, data, 64, 32);
		return data;
	}

	public static InventoryData FromBytes(byte[] data) {
		var inv = new InventoryData();
		Array.Copy(data, 0, inv.BagItems, 0, 64);
		Array.Copy(data, 64, inv.ImportantItems, 0, 32);
		return inv;
	}
}

/// <summary>
/// Spell flags (8 bytes per character, 64 spells as bits).
/// </summary>
public class SpellFlags {
	public byte[] Flags { get; set; } = new byte[8];

	public bool HasSpell(int spellId) => spellId < 64 && (Flags[spellId / 8] & (1 << (spellId % 8))) != 0;

	public void SetSpell(int spellId, bool learned = true) {
		if (spellId >= 64) return;
		if (learned)
			Flags[spellId / 8] |= (byte)(1 << (spellId % 8));
		else
			Flags[spellId / 8] &= (byte)~(1 << (spellId % 8));
	}

	public byte[] ToBytes() => (byte[])Flags.Clone();

	public static SpellFlags FromBytes(byte[] data) {
		var flags = new SpellFlags();
		Array.Copy(data, 0, flags.Flags, 0, 8);
		return flags;
	}
}

/// <summary>
/// World state (224 bytes).
/// </summary>
public class WorldState {
	public byte[] EventFlags { get; set; } = new byte[64];
	public byte[] TreasureChests { get; set; } = new byte[64];
	public byte[] MiscFlags { get; set; } = new byte[96];

	public bool GetEventFlag(int flagId) =>
		flagId < 512 && (EventFlags[flagId / 8] & (1 << (flagId % 8))) != 0;

	public void SetEventFlag(int flagId, bool value = true) {
		if (flagId >= 512) return;
		if (value)
			EventFlags[flagId / 8] |= (byte)(1 << (flagId % 8));
		else
			EventFlags[flagId / 8] &= (byte)~(1 << (flagId % 8));
	}

	public bool IsChestOpened(int chestId) =>
		chestId < 512 && (TreasureChests[chestId / 8] & (1 << (chestId % 8))) != 0;

	public void SetChestOpened(int chestId, bool opened = true) {
		if (chestId >= 512) return;
		if (opened)
			TreasureChests[chestId / 8] |= (byte)(1 << (chestId % 8));
		else
			TreasureChests[chestId / 8] &= (byte)~(1 << (chestId % 8));
	}

	public byte[] ToBytes() {
		var data = new byte[224];
		Array.Copy(EventFlags, 0, data, 0, 64);
		Array.Copy(TreasureChests, 0, data, 64, 64);
		Array.Copy(MiscFlags, 0, data, 128, 96);
		return data;
	}

	public static WorldState FromBytes(byte[] data) {
		var world = new WorldState();
		Array.Copy(data, 0, world.EventFlags, 0, 64);
		Array.Copy(data, 64, world.TreasureChests, 0, 64);
		Array.Copy(data, 128, world.MiscFlags, 0, 96);
		return world;
	}
}
