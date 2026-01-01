using DW4Lib.DataStructures;

namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 save data to DQ3r format.
/// </summary>
public static class SaveStateConverter {
	/// <summary>
	/// DQ3r save slot size.
	/// </summary>
	public const int DQ3rSaveSlotSize = 2048;

	/// <summary>
	/// Convert DW4 save data to DQ3r format.
	/// </summary>
	public static DQ3rSaveData Convert(SaveData dw4Save) {
		var dq3Save = new DQ3rSaveData {
			Header = ConvertHeader(dw4Save.Header),
			Party = ConvertParty(dw4Save.Party),
			World = ConvertWorldState(dw4Save.World)
		};

		// Convert characters
		for (int i = 0; i < 8; i++) {
			dq3Save.Characters[i] = ConvertCharacter(dw4Save.Characters[i], dw4Save.Equipment[i], dw4Save.SpellsKnown[i]);
		}

		// Convert inventory
		dq3Save.Inventory = ConvertInventory(dw4Save.Inventory);

		return dq3Save;
	}

	/// <summary>
	/// Convert save header.
	/// </summary>
	private static DQ3rSaveHeader ConvertHeader(SaveHeader dw4Header) {
		return new DQ3rSaveHeader {
			// DQ3r uses scenario ID instead of chapter
			ScenarioId = (byte)(0x10 + dw4Header.Chapter), // DW4 scenarios start at 0x10
			Progress = dw4Header.SubChapterProgress,
			// Scale gold up by 1.5x for DQ3r economy
			Gold = (uint)(dw4Header.Gold * 1.5),
			CasinoCoins = (uint)dw4Header.CasinoCoins,
			PlayTimeSeconds = dw4Header.PlayTimeFrames / 60, // Convert frames to seconds
			// DQ3r day/night cycle uses 0-255 range
			DayNightCycle = (byte)(dw4Header.DayNightCycle / 2)
		};
	}

	/// <summary>
	/// Convert party configuration.
	/// </summary>
	private static DQ3rPartyConfig ConvertParty(PartyConfig dw4Party) {
		var party = new DQ3rPartyConfig {
			PartyCount = dw4Party.PartyCount,
			WagonCount = dw4Party.WagonCount,
			TacticsMode = 0x01 // Default AI tactics
		};

		// Convert character IDs to DQ3r format (add 0x100 offset)
		for (int i = 0; i < 4; i++) {
			party.ActiveParty[i] = dw4Party.ActiveParty[i] == 0xFF
				? (ushort)0xFFFF
				: (ushort)(0x100 + dw4Party.ActiveParty[i]);
			party.WagonParty[i] = dw4Party.WagonParty[i] == 0xFF
				? (ushort)0xFFFF
				: (ushort)(0x100 + dw4Party.WagonParty[i]);
		}

		return party;
	}

	/// <summary>
	/// Convert character data.
	/// </summary>
	private static DQ3rCharacterData ConvertCharacter(CharacterSaveData dw4Char, EquipmentData dw4Equip, SpellFlags dw4Spells) {
		return new DQ3rCharacterData {
			Name = dw4Char.Name,
			Level = dw4Char.Level,
			Experience = (uint)(dw4Char.Experience * 1.2), // DQ3r exp curve is slightly higher
			// Scale HP/MP by 1.5x
			CurrentHP = (ushort)(dw4Char.CurrentHP * 1.5),
			MaxHP = (ushort)(dw4Char.MaxHP * 1.5),
			CurrentMP = (ushort)(dw4Char.CurrentMP * 1.5),
			MaxMP = (ushort)(dw4Char.MaxMP * 1.5),
			// Scale stats by 1.2x
			Strength = (ushort)(dw4Char.Strength * 1.2),
			Agility = (ushort)(dw4Char.Agility * 1.2),
			Vitality = (ushort)(dw4Char.Vitality * 1.2),
			Intelligence = (ushort)(dw4Char.Intelligence * 1.2),
			Luck = (ushort)(dw4Char.Luck * 1.2),
			StatusEffects = dw4Char.StatusEffects,
			// Convert equipment IDs
			Weapon = ItemIdConverter.ConvertToDQ3r(dw4Equip.Weapon),
			Armor = ItemIdConverter.ConvertToDQ3r(dw4Equip.Armor),
			Shield = ItemIdConverter.ConvertToDQ3r(dw4Equip.Shield),
			Helmet = ItemIdConverter.ConvertToDQ3r(dw4Equip.Helmet),
			Accessory = ItemIdConverter.ConvertToDQ3r(dw4Equip.Accessory1),
			// Convert spell flags
			SpellsKnown = ConvertSpellFlags(dw4Spells)
		};
	}

	/// <summary>
	/// Convert spell flags to DQ3r format.
	/// </summary>
	private static ushort[] ConvertSpellFlags(SpellFlags dw4Spells) {
		// DQ3r uses 4 x 16-bit words for spell flags
		var spells = new ushort[4];

		for (int i = 0; i < 64; i++) {
			if (dw4Spells.HasSpell(i)) {
				// Convert spell ID to DQ3r equivalent
				int dq3SpellId = SpellIdConverter.ConvertToDQ3r(i);
				if (dq3SpellId >= 0 && dq3SpellId < 64) {
					spells[dq3SpellId / 16] |= (ushort)(1 << (dq3SpellId % 16));
				}
			}
		}

		return spells;
	}

	/// <summary>
	/// Convert inventory data.
	/// </summary>
	private static DQ3rInventoryData ConvertInventory(InventoryData dw4Inv) {
		var inv = new DQ3rInventoryData();

		// Convert bag items (64 slots)
		for (int i = 0; i < 64; i++) {
			inv.BagItems[i] = ItemIdConverter.ConvertToDQ3r(dw4Inv.BagItems[i]);
		}

		// Convert important/key items (16 slots)
		for (int i = 0; i < 16; i++) {
			inv.KeyItems[i] = ItemIdConverter.ConvertToDQ3r(dw4Inv.ImportantItems[i]);
		}

		return inv;
	}

	/// <summary>
	/// Convert world state.
	/// </summary>
	private static DQ3rWorldState ConvertWorldState(WorldState dw4World) {
		var world = new DQ3rWorldState();

		// Convert event flags with offset (DW4 scenarios use 0x200+ in DQ3r)
		// DQ3r has 1024 flags (0-1023), we reserve 0x200-0x3FF for DW4 flags
		for (int i = 0; i < 512; i++) {
			if (dw4World.GetEventFlag(i)) {
				int dq3FlagId = 0x200 + i;
				world.SetEventFlag(dq3FlagId);
			}
		}

		// Convert treasure chests with offset
		for (int i = 0; i < 512; i++) {
			if (dw4World.IsChestOpened(i)) {
				int dq3ChestId = 0x200 + i;
				world.SetChestOpened(dq3ChestId);
			}
		}

		return world;
	}
}

/// <summary>
/// DQ3r save data structure.
/// </summary>
public class DQ3rSaveData {
	public DQ3rSaveHeader Header { get; set; } = new();
	public DQ3rPartyConfig Party { get; set; } = new();
	public DQ3rCharacterData[] Characters { get; set; } = new DQ3rCharacterData[8];
	public DQ3rInventoryData Inventory { get; set; } = new();
	public DQ3rWorldState World { get; set; } = new();

	public DQ3rSaveData() {
		for (int i = 0; i < 8; i++) {
			Characters[i] = new DQ3rCharacterData();
		}
	}

	/// <summary>
	/// Serialize to DQ3r format bytes.
	/// </summary>
	public byte[] ToBytes() {
		var data = new byte[SaveStateConverter.DQ3rSaveSlotSize];

		// Header at 0x00
		Array.Copy(Header.ToBytes(), 0, data, 0, 32);

		// Party at 0x20
		Array.Copy(Party.ToBytes(), 0, data, 0x20, 32);

		// Characters at 0x40 (64 bytes each)
		for (int i = 0; i < 8; i++) {
			Array.Copy(Characters[i].ToBytes(), 0, data, 0x40 + (i * 64), 64);
		}

		// Inventory at 0x240
		Array.Copy(Inventory.ToBytes(), 0, data, 0x240, 160);

		// World at 0x2E0
		Array.Copy(World.ToBytes(), 0, data, 0x2E0, 256);

		return data;
	}
}

/// <summary>
/// DQ3r save header.
/// </summary>
public class DQ3rSaveHeader {
	public ushort Checksum { get; set; }
	public byte ScenarioId { get; set; }
	public byte Progress { get; set; }
	public uint Gold { get; set; }
	public uint CasinoCoins { get; set; }
	public uint PlayTimeSeconds { get; set; }
	public byte DayNightCycle { get; set; }

	public byte[] ToBytes() {
		var data = new byte[32];
		data[0] = (byte)(Checksum & 0xFF);
		data[1] = (byte)(Checksum >> 8);
		data[2] = ScenarioId;
		data[3] = Progress;
		BitConverter.GetBytes(Gold).CopyTo(data, 4);
		BitConverter.GetBytes(CasinoCoins).CopyTo(data, 8);
		BitConverter.GetBytes(PlayTimeSeconds).CopyTo(data, 12);
		data[16] = DayNightCycle;
		return data;
	}
}

/// <summary>
/// DQ3r party configuration.
/// </summary>
public class DQ3rPartyConfig {
	public ushort[] ActiveParty { get; set; } = [0xFFFF, 0xFFFF, 0xFFFF, 0xFFFF];
	public ushort[] WagonParty { get; set; } = [0xFFFF, 0xFFFF, 0xFFFF, 0xFFFF];
	public byte PartyCount { get; set; }
	public byte WagonCount { get; set; }
	public byte TacticsMode { get; set; }

	public byte[] ToBytes() {
		var data = new byte[32];
		for (int i = 0; i < 4; i++) {
			BitConverter.GetBytes(ActiveParty[i]).CopyTo(data, i * 2);
			BitConverter.GetBytes(WagonParty[i]).CopyTo(data, 8 + i * 2);
		}
		data[16] = PartyCount;
		data[17] = WagonCount;
		data[18] = TacticsMode;
		return data;
	}
}

/// <summary>
/// DQ3r character data.
/// </summary>
public class DQ3rCharacterData {
	public string Name { get; set; } = "";
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
	public ushort Weapon { get; set; }
	public ushort Armor { get; set; }
	public ushort Shield { get; set; }
	public ushort Helmet { get; set; }
	public ushort Accessory { get; set; }
	public ushort[] SpellsKnown { get; set; } = new ushort[4];

	public byte[] ToBytes() {
		var data = new byte[64];
		var nameBytes = System.Text.Encoding.UTF8.GetBytes(Name.PadRight(16, '\0')[..16]);
		Array.Copy(nameBytes, 0, data, 0, 16);
		data[16] = Level;
		BitConverter.GetBytes(Experience).CopyTo(data, 17);
		BitConverter.GetBytes(CurrentHP).CopyTo(data, 21);
		BitConverter.GetBytes(MaxHP).CopyTo(data, 23);
		BitConverter.GetBytes(CurrentMP).CopyTo(data, 25);
		BitConverter.GetBytes(MaxMP).CopyTo(data, 27);
		BitConverter.GetBytes(Strength).CopyTo(data, 29);
		BitConverter.GetBytes(Agility).CopyTo(data, 31);
		BitConverter.GetBytes(Vitality).CopyTo(data, 33);
		BitConverter.GetBytes(Intelligence).CopyTo(data, 35);
		BitConverter.GetBytes(Luck).CopyTo(data, 37);
		data[39] = StatusEffects;
		BitConverter.GetBytes(Weapon).CopyTo(data, 40);
		BitConverter.GetBytes(Armor).CopyTo(data, 42);
		BitConverter.GetBytes(Shield).CopyTo(data, 44);
		BitConverter.GetBytes(Helmet).CopyTo(data, 46);
		BitConverter.GetBytes(Accessory).CopyTo(data, 48);
		for (int i = 0; i < 4; i++) {
			BitConverter.GetBytes(SpellsKnown[i]).CopyTo(data, 50 + i * 2);
		}
		return data;
	}
}

/// <summary>
/// DQ3r inventory data.
/// </summary>
public class DQ3rInventoryData {
	public ushort[] BagItems { get; set; } = new ushort[64]; // 64 bag items
	public ushort[] KeyItems { get; set; } = new ushort[16]; // 16 key items

	public byte[] ToBytes() {
		var data = new byte[160];
		// 64 bag items × 2 bytes = 128 bytes
		for (int i = 0; i < 64; i++) {
			BitConverter.GetBytes(BagItems[i]).CopyTo(data, i * 2);
		}
		// 16 key items × 2 bytes = 32 bytes
		for (int i = 0; i < 16; i++) {
			BitConverter.GetBytes(KeyItems[i]).CopyTo(data, 128 + i * 2);
		}
		return data;
	}
}

/// <summary>
/// DQ3r world state.
/// </summary>
public class DQ3rWorldState {
	public byte[] EventFlags { get; set; } = new byte[128]; // 1024 flags
	public byte[] TreasureChests { get; set; } = new byte[128]; // 1024 chests

	public bool GetEventFlag(int flagId) =>
		flagId < 1024 && (EventFlags[flagId / 8] & (1 << (flagId % 8))) != 0;

	public void SetEventFlag(int flagId, bool value = true) {
		if (flagId >= 1024) return;
		if (value)
			EventFlags[flagId / 8] |= (byte)(1 << (flagId % 8));
		else
			EventFlags[flagId / 8] &= (byte)~(1 << (flagId % 8));
	}

	public bool IsChestOpened(int chestId) =>
		chestId < 1024 && (TreasureChests[chestId / 8] & (1 << (chestId % 8))) != 0;

	public void SetChestOpened(int chestId, bool opened = true) {
		if (chestId >= 1024) return;
		if (opened)
			TreasureChests[chestId / 8] |= (byte)(1 << (chestId % 8));
		else
			TreasureChests[chestId / 8] &= (byte)~(1 << (chestId % 8));
	}

	public byte[] ToBytes() {
		var data = new byte[256];
		Array.Copy(EventFlags, 0, data, 0, 128);
		Array.Copy(TreasureChests, 0, data, 128, 128);
		return data;
	}
}

/// <summary>
/// Spell ID converter for DW4 to DQ3r.
/// </summary>
public static class SpellIdConverter {
	private static readonly Dictionary<int, int> SpellMap = new() {
		// Healing spells
		{ 0x00, 0x00 }, // Heal
		{ 0x01, 0x01 }, // Healmore
		{ 0x02, 0x02 }, // Healall
		{ 0x03, 0x03 }, // HealUs
		{ 0x04, 0x04 }, // HealUsAll
		// Attack spells
		{ 0x10, 0x10 }, // Blaze
		{ 0x11, 0x11 }, // Blazemore
		{ 0x12, 0x12 }, // Blazemost
		{ 0x13, 0x13 }, // Firebal
		{ 0x14, 0x14 }, // Firebane
		{ 0x15, 0x15 }, // FireMost
		{ 0x16, 0x16 }, // Bang
		{ 0x17, 0x17 }, // Boom
		{ 0x18, 0x18 }, // Explodet
		{ 0x19, 0x19 }, // Zap
		{ 0x1A, 0x1A }, // Lightning
		{ 0x1B, 0x1B }, // ThunderBolt
		// Support spells
		{ 0x20, 0x20 }, // Upper
		{ 0x21, 0x21 }, // Bikill
		{ 0x22, 0x22 }, // Increase
		{ 0x23, 0x23 }, // SpeedUp
		{ 0x24, 0x24 }, // TwinHits
		// Status spells
		{ 0x30, 0x30 }, // Sleep
		{ 0x31, 0x31 }, // Surround
		{ 0x32, 0x32 }, // Stopspell
		{ 0x33, 0x33 }, // Beat
		{ 0x34, 0x34 }, // Defeat
		// Utility spells
		{ 0x40, 0x40 }, // Return
		{ 0x41, 0x41 }, // Outside
		{ 0x42, 0x42 }, // Repel
		{ 0x43, 0x43 }, // Radiant
		{ 0x44, 0x44 }, // StepGuard
	};

	public static int ConvertToDQ3r(int dw4SpellId) {
		return SpellMap.TryGetValue(dw4SpellId, out int dq3Id) ? dq3Id : -1;
	}
}
