namespace DQ4rLib.Models;

/// <summary>
/// Character status effect flags.
/// </summary>
[Flags]
public enum CharacterStatus : byte {
	/// <summary>No status effects.</summary>
	None = 0,

	/// <summary>Character is dead.</summary>
	Dead = 1 << 0,

	/// <summary>Character is poisoned.</summary>
	Poisoned = 1 << 1,

	/// <summary>Character is asleep.</summary>
	Asleep = 1 << 2,

	/// <summary>Character is paralyzed.</summary>
	Paralyzed = 1 << 3,

	/// <summary>Character is confused.</summary>
	Confused = 1 << 4,

	/// <summary>Character is silenced (can't cast spells).</summary>
	Silenced = 1 << 5,

	/// <summary>Character has defense buff.</summary>
	DefenseUp = 1 << 6,

	/// <summary>Character has defense debuff.</summary>
	DefenseDown = 1 << 7
}

/// <summary>
/// Complete save data for a single save slot.
/// Contains all game state needed to restore a saved game.
/// </summary>
public class SaveData {
	/// <summary>
	/// Save format version for compatibility checking.
	/// </summary>
	public const byte CurrentVersion = 1;

	/// <summary>
	/// Save data version.
	/// </summary>
	public byte Version { get; set; } = CurrentVersion;

	/// <summary>
	/// Checksum for data validation.
	/// </summary>
	public ushort Checksum { get; set; }

	/// <summary>
	/// Chapter state (position, flags, party).
	/// </summary>
	public ChapterState ChapterState { get; set; } = new();

	/// <summary>
	/// All character data (8 playable + NPCs).
	/// </summary>
	public CharacterSaveData[] Characters { get; set; } = new CharacterSaveData[16];

	/// <summary>
	/// Inventory items.
	/// </summary>
	public InventoryData Inventory { get; set; } = new();

	/// <summary>
	/// Chapter-specific gold values (preserved between chapters).
	/// </summary>
	public uint[] ChapterGold { get; set; } = new uint[5];

	/// <summary>
	/// Chapter-specific inventories (preserved between chapters).
	/// </summary>
	public byte[][] ChapterInventories { get; set; } = new byte[5][];

	/// <summary>
	/// Small medal collection count.
	/// </summary>
	public byte SmallMedals { get; set; }

	/// <summary>
	/// Casino coins.
	/// </summary>
	public uint CasinoCoins { get; set; }

	/// <summary>
	/// Monster encyclopedia completion flags.
	/// </summary>
	public byte[] MonsterEncyclopedia { get; set; } = new byte[32];

	/// <summary>
	/// Save timestamp (SNES format).
	/// </summary>
	public uint SaveTimestamp { get; set; }

	/// <summary>
	/// Calculate checksum for save data validation.
	/// </summary>
	public ushort CalculateChecksum() {
		ushort sum = 0;
		var bytes = ToSnesBytesWithoutChecksum();

		// Skip first 4 bytes (version + checksum)
		for (int i = 4; i < bytes.Length; i++) {
			sum += bytes[i];
		}

		return (ushort)(sum ^ 0x5a5a);
	}

	/// <summary>
	/// Validate save data checksum.
	/// </summary>
	public bool ValidateChecksum() {
		return Checksum == CalculateChecksum();
	}

	/// <summary>
	/// Total size of save data in bytes.
	/// </summary>
	public const int SaveSize = 0x800; // 2KB per save slot

	/// <summary>
	/// Serialize save data to SNES SRAM format (without checksum calculation).
	/// Internal method used by both ToSnesBytes and CalculateChecksum.
	/// </summary>
	/// <remarks>
	/// Memory layout (2KB = 0x800 bytes):
	/// 0x000-0x00F: Header (version, checksum, reserved)
	/// 0x010-0x04F: Chapter state (64 bytes)
	/// 0x050-0x34F: Characters (16 × 48 bytes = 768 bytes)
	/// 0x350-0x3CF: Inventory (128 bytes)
	/// 0x3D0-0x3E3: Chapter gold (5 × 4 bytes = 20 bytes)
	/// 0x3E4-0x3EF: Small medals, casino coins (12 bytes)
	/// 0x400-0x41F: Monster encyclopedia (32 bytes)
	/// 0x420-0x423: Save timestamp (4 bytes)
	/// 0x424-0x7FF: Reserved
	/// </remarks>
	private byte[] ToSnesBytesWithoutChecksum() {
		byte[] data = new byte[SaveSize];

		// Header
		data[0x00] = Version;
		data[0x01] = 0x00; // Reserved
		// Checksum at 0x02-0x03 (set separately)

		// Chapter state (64 bytes at 0x10)
		byte[] chapterData = ChapterState.ToSnesBytes();
		Array.Copy(chapterData, 0, data, 0x10, Math.Min(64, chapterData.Length));

		// Character data (16 chars × 48 bytes = 768 bytes at 0x50)
		for (int i = 0; i < 16; i++) {
			if (Characters[i] != null) {
				byte[] charData = Characters[i].ToSnesBytes();
				Array.Copy(charData, 0, data, 0x50 + (i * 48), Math.Min(48, charData.Length));
			}
		}

		// Inventory (128 bytes at 0x350)
		byte[] invData = Inventory.ToSnesBytes();
		Array.Copy(invData, 0, data, 0x350, Math.Min(128, invData.Length));

		// Chapter gold (5 × 4 bytes = 20 bytes at 0x3D0)
		for (int i = 0; i < 5; i++) {
			data[0x3D0 + (i * 4)] = (byte)(ChapterGold[i] & 0xff);
			data[0x3D1 + (i * 4)] = (byte)((ChapterGold[i] >> 8) & 0xff);
			data[0x3D2 + (i * 4)] = (byte)((ChapterGold[i] >> 16) & 0xff);
			data[0x3D3 + (i * 4)] = (byte)((ChapterGold[i] >> 24) & 0xff);
		}

		// Small medals and casino (at 0x3E4)
		data[0x3E4] = SmallMedals;
		data[0x3E8] = (byte)(CasinoCoins & 0xff);
		data[0x3E9] = (byte)((CasinoCoins >> 8) & 0xff);
		data[0x3EA] = (byte)((CasinoCoins >> 16) & 0xff);
		data[0x3EB] = (byte)((CasinoCoins >> 24) & 0xff);

		// Monster encyclopedia (32 bytes at 0x400)
		Array.Copy(MonsterEncyclopedia, 0, data, 0x400, 32);

		// Save timestamp (4 bytes at 0x420)
		data[0x420] = (byte)(SaveTimestamp & 0xff);
		data[0x421] = (byte)((SaveTimestamp >> 8) & 0xff);
		data[0x422] = (byte)((SaveTimestamp >> 16) & 0xff);
		data[0x423] = (byte)((SaveTimestamp >> 24) & 0xff);

		return data;
	}

	/// <summary>
	/// Serialize save data to SNES SRAM format.
	/// </summary>
	public byte[] ToSnesBytes() {
		byte[] data = ToSnesBytesWithoutChecksum();

		// Calculate and set checksum
		Checksum = CalculateChecksum();
		data[0x02] = (byte)(Checksum & 0xff);
		data[0x03] = (byte)(Checksum >> 8);

		return data;
	}

	/// <summary>
	/// Deserialize save data from SNES SRAM format.
	/// </summary>
	public static SaveData FromSnesBytes(byte[] data) {
		var save = new SaveData {
			Version = data[0x00],
			Checksum = (ushort)(data[0x02] | (data[0x03] << 8))
		};

		// Chapter state (64 bytes at 0x10)
		byte[] chapterData = new byte[64];
		Array.Copy(data, 0x10, chapterData, 0, 64);
		save.ChapterState = ChapterState.FromSnesBytes(chapterData);

		// Character data (16 chars × 48 bytes at 0x50)
		for (int i = 0; i < 16; i++) {
			byte[] charData = new byte[48];
			Array.Copy(data, 0x50 + (i * 48), charData, 0, 48);
			save.Characters[i] = CharacterSaveData.FromSnesBytes(charData);
		}

		// Inventory (128 bytes at 0x350)
		byte[] invData = new byte[128];
		Array.Copy(data, 0x350, invData, 0, 128);
		save.Inventory = InventoryData.FromSnesBytes(invData);

		// Chapter gold (5 × 4 bytes at 0x3D0)
		for (int i = 0; i < 5; i++) {
			save.ChapterGold[i] = (uint)(
				data[0x3D0 + (i * 4)] |
				(data[0x3D1 + (i * 4)] << 8) |
				(data[0x3D2 + (i * 4)] << 16) |
				(data[0x3D3 + (i * 4)] << 24));
		}

		// Small medals and casino (at 0x3E4)
		save.SmallMedals = data[0x3E4];
		save.CasinoCoins = (uint)(
			data[0x3E8] |
			(data[0x3E9] << 8) |
			(data[0x3EA] << 16) |
			(data[0x3EB] << 24));

		// Monster encyclopedia (32 bytes at 0x400)
		Array.Copy(data, 0x400, save.MonsterEncyclopedia, 0, 32);

		// Save timestamp (4 bytes at 0x420)
		save.SaveTimestamp = (uint)(
			data[0x420] |
			(data[0x421] << 8) |
			(data[0x422] << 16) |
			(data[0x423] << 24));

		return save;
	}

	/// <summary>
	/// Create a new save from chapter manager state.
	/// </summary>
	public static SaveData CreateFromChapterManager(ChapterManager manager) {
		var save = new SaveData {
			ChapterState = manager.State
		};
		// Calculate checksum for validation on load
		save.Checksum = save.CalculateChecksum();
		return save;
	}
}

/// <summary>
/// Character save data structure.
/// Contains all persistent data for a single character including stats,
/// equipment, spells, and status.
/// </summary>
public class CharacterSaveData {
	/// <summary>Character ID (0-7 for party, 8+ for NPCs).</summary>
	public byte Id { get; set; }

	/// <summary>Character class/job ID.</summary>
	public byte ClassId { get; set; }

	/// <summary>Character name (8 characters max).</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Current level (1-99).</summary>
	public byte Level { get; set; }

	/// <summary>Current HP.</summary>
	public ushort CurrentHp { get; set; }

	/// <summary>Maximum HP.</summary>
	public ushort MaxHp { get; set; }

	/// <summary>Current MP.</summary>
	public ushort CurrentMp { get; set; }

	/// <summary>Maximum MP.</summary>
	public ushort MaxMp { get; set; }

	/// <summary>Strength stat.</summary>
	public byte Strength { get; set; }

	/// <summary>Agility stat.</summary>
	public byte Agility { get; set; }

	/// <summary>Vitality stat.</summary>
	public byte Vitality { get; set; }

	/// <summary>Intelligence stat.</summary>
	public byte Intelligence { get; set; }

	/// <summary>Luck stat.</summary>
	public byte Luck { get; set; }

	/// <summary>Attack power (calculated from strength + equipment).</summary>
	public ushort Attack { get; set; }

	/// <summary>Defense power (calculated from vitality + equipment).</summary>
	public ushort Defense { get; set; }

	/// <summary>Experience points.</summary>
	public uint Experience { get; set; }

	/// <summary>Equipped weapon ID (0 = none).</summary>
	public byte WeaponId { get; set; }

	/// <summary>Equipped armor ID (0 = none).</summary>
	public byte ArmorId { get; set; }

	/// <summary>Equipped shield ID (0 = none).</summary>
	public byte ShieldId { get; set; }

	/// <summary>Equipped helmet ID (0 = none).</summary>
	public byte HelmetId { get; set; }

	/// <summary>Equipped accessory ID (0 = none).</summary>
	public byte AccessoryId { get; set; }

	/// <summary>Status effects bitmask.</summary>
	public CharacterStatus Status { get; set; }

	/// <summary>Learned spells bitmask (up to 64 spells).</summary>
	public ulong LearnedSpells { get; set; }

	/// <summary>Individual tactic for this character (Chapter 5).</summary>
	public BattleTactic Tactic { get; set; }

	/// <summary>Position in party (0-3 active, 4-7 wagon).</summary>
	public byte PartyPosition { get; set; }

	/// <summary>Check if character knows a spell.</summary>
	public bool KnowsSpell(int spellId) => spellId < 64 && (LearnedSpells & (1UL << spellId)) != 0;

	/// <summary>Learn a spell.</summary>
	public void LearnSpell(int spellId) {
		if (spellId < 64)
			LearnedSpells |= 1UL << spellId;
	}

	/// <summary>Forget a spell.</summary>
	public void ForgetSpell(int spellId) {
		if (spellId < 64)
			LearnedSpells &= ~(1UL << spellId);
	}

	/// <summary>Get list of known spell IDs.</summary>
	public List<int> GetKnownSpells() {
		var spells = new List<int>();
		for (int i = 0; i < 64; i++) {
			if (KnowsSpell(i))
				spells.Add(i);
		}
		return spells;
	}

	/// <summary>Check if character is alive.</summary>
	public bool IsAlive => CurrentHp > 0 && !Status.HasFlag(CharacterStatus.Dead);

	/// <summary>Check if character can act in battle.</summary>
	public bool CanAct => IsAlive && !Status.HasFlag(CharacterStatus.Asleep)
		&& !Status.HasFlag(CharacterStatus.Paralyzed) && !Status.HasFlag(CharacterStatus.Confused);

	/// <summary>Serialize to 48 bytes.</summary>
	public byte[] ToSnesBytes() {
		byte[] data = new byte[48];

		data[0x00] = Id;
		data[0x01] = ClassId;
		data[0x02] = Level;
		data[0x03] = (byte)Status;
		data[0x04] = (byte)(CurrentHp & 0xff);
		data[0x05] = (byte)(CurrentHp >> 8);
		data[0x06] = (byte)(MaxHp & 0xff);
		data[0x07] = (byte)(MaxHp >> 8);
		data[0x08] = (byte)(CurrentMp & 0xff);
		data[0x09] = (byte)(CurrentMp >> 8);
		data[0x0A] = (byte)(MaxMp & 0xff);
		data[0x0B] = (byte)(MaxMp >> 8);
		data[0x0C] = Strength;
		data[0x0D] = Agility;
		data[0x0E] = Vitality;
		data[0x0F] = Intelligence;
		data[0x10] = Luck;
		data[0x11] = (byte)(Attack & 0xff);
		data[0x12] = (byte)(Attack >> 8);
		data[0x13] = (byte)(Defense & 0xff);
		data[0x14] = (byte)(Defense >> 8);
		data[0x15] = (byte)(Experience & 0xff);
		data[0x16] = (byte)((Experience >> 8) & 0xff);
		data[0x17] = (byte)((Experience >> 16) & 0xff);
		data[0x18] = (byte)((Experience >> 24) & 0xff);
		data[0x19] = WeaponId;
		data[0x1A] = ArmorId;
		data[0x1B] = ShieldId;
		data[0x1C] = HelmetId;
		data[0x1D] = AccessoryId;
		data[0x1E] = (byte)Tactic;
		data[0x1F] = PartyPosition;
		// Learned spells (8 bytes at 0x20)
		for (int i = 0; i < 8; i++) {
			data[0x20 + i] = (byte)((LearnedSpells >> (i * 8)) & 0xff);
		}
		// Name (8 bytes at 0x28)
		for (int i = 0; i < 8 && i < Name.Length; i++) {
			data[0x28 + i] = (byte)Name[i];
		}

		return data;
	}

	/// <summary>Deserialize from 48 bytes.</summary>
	public static CharacterSaveData FromSnesBytes(byte[] data) {
		var chr = new CharacterSaveData {
			Id = data[0x00],
			ClassId = data[0x01],
			Level = data[0x02],
			Status = (CharacterStatus)data[0x03],
			CurrentHp = (ushort)(data[0x04] | (data[0x05] << 8)),
			MaxHp = (ushort)(data[0x06] | (data[0x07] << 8)),
			CurrentMp = (ushort)(data[0x08] | (data[0x09] << 8)),
			MaxMp = (ushort)(data[0x0A] | (data[0x0B] << 8)),
			Strength = data[0x0C],
			Agility = data[0x0D],
			Vitality = data[0x0E],
			Intelligence = data[0x0F],
			Luck = data[0x10],
			Attack = (ushort)(data[0x11] | (data[0x12] << 8)),
			Defense = (ushort)(data[0x13] | (data[0x14] << 8)),
			Experience = (uint)(data[0x15] | (data[0x16] << 8) | (data[0x17] << 16) | (data[0x18] << 24)),
			WeaponId = data[0x19],
			ArmorId = data[0x1A],
			ShieldId = data[0x1B],
			HelmetId = data[0x1C],
			AccessoryId = data[0x1D],
			Tactic = (BattleTactic)data[0x1E],
			PartyPosition = data[0x1F]
		};

		// Learned spells
		for (int i = 0; i < 8; i++) {
			chr.LearnedSpells |= (ulong)data[0x20 + i] << (i * 8);
		}

		// Decode name
		char[] nameChars = new char[8];
		int nameLen = 0;
		for (int i = 0; i < 8; i++) {
			if (data[0x28 + i] != 0) {
				nameChars[i] = (char)data[0x28 + i];
				nameLen = i + 1;
			}
		}
		chr.Name = new string(nameChars, 0, nameLen);

		return chr;
	}
}

/// <summary>
/// Inventory data structure.
/// </summary>
public class InventoryData {
	/// <summary>Maximum bag slots.</summary>
	public const int MaxBagSlots = 64;

	/// <summary>Maximum important items.</summary>
	public const int MaxImportantItems = 32;

	/// <summary>Bag item IDs.</summary>
	public byte[] BagItems { get; set; } = new byte[MaxBagSlots];

	/// <summary>Important/Key items bitmask.</summary>
	public byte[] ImportantItems { get; set; } = new byte[4]; // 32 bits

	/// <summary>Check if important item is owned.</summary>
	public bool HasImportantItem(int itemId) {
		int byteIndex = itemId / 8;
		int bitIndex = itemId % 8;
		return byteIndex < ImportantItems.Length && (ImportantItems[byteIndex] & (1 << bitIndex)) != 0;
	}

	/// <summary>Set important item as owned.</summary>
	public void SetImportantItem(int itemId, bool owned = true) {
		int byteIndex = itemId / 8;
		int bitIndex = itemId % 8;
		if (byteIndex < ImportantItems.Length) {
			if (owned)
				ImportantItems[byteIndex] |= (byte)(1 << bitIndex);
			else
				ImportantItems[byteIndex] &= (byte)~(1 << bitIndex);
		}
	}

	/// <summary>Add item to bag.</summary>
	public bool AddItem(byte itemId) {
		for (int i = 0; i < MaxBagSlots; i++) {
			if (BagItems[i] == 0) {
				BagItems[i] = itemId;
				return true;
			}
		}
		return false;
	}

	/// <summary>Remove item from bag.</summary>
	public bool RemoveItem(byte itemId) {
		for (int i = 0; i < MaxBagSlots; i++) {
			if (BagItems[i] == itemId) {
				// Shift remaining items
				for (int j = i; j < MaxBagSlots - 1; j++) {
					BagItems[j] = BagItems[j + 1];
				}
				BagItems[MaxBagSlots - 1] = 0;
				return true;
			}
		}
		return false;
	}

	/// <summary>Serialize to 128 bytes.</summary>
	public byte[] ToSnesBytes() {
		byte[] data = new byte[128];
		Array.Copy(BagItems, 0, data, 0, MaxBagSlots);
		Array.Copy(ImportantItems, 0, data, MaxBagSlots, 4);
		return data;
	}

	/// <summary>Deserialize from 128 bytes.</summary>
	public static InventoryData FromSnesBytes(byte[] data) {
		var inv = new InventoryData();
		Array.Copy(data, 0, inv.BagItems, 0, MaxBagSlots);
		Array.Copy(data, MaxBagSlots, inv.ImportantItems, 0, 4);
		return inv;
	}
}
