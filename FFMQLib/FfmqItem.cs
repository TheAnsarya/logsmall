namespace FFMQLib;

/// <summary>
/// FFMQ Weapon data structure (8 bytes).
/// ROM address: $D2A000+
/// </summary>
/// <remarks>
/// 16 total weapons: 4 categories × 4 levels each
/// </remarks>
public record FfmqWeapon {
	/// <summary>Weapon ID (0-15)</summary>
	public byte Id { get; init; }

	/// <summary>Weapon name</summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>Attack power (0-255)</summary>
	public byte AttackPower { get; init; }

	/// <summary>Element flags</summary>
	public FfmqElement Element { get; init; }

	/// <summary>Special effect ID (0 = none)</summary>
	public byte SpecialEffectId { get; init; }

	/// <summary>Menu icon ID</summary>
	public byte IconId { get; init; }

	/// <summary>Equipment slot category</summary>
	public FfmqWeaponSlot Slot { get; init; }

	/// <summary>Required level to use (0 = always available)</summary>
	public byte RequiredLevel { get; init; }

	/// <summary>Weapon tier within category (0-3)</summary>
	public int Tier => Id % 4;

	/// <summary>Display name based on slot and tier</summary>
	public string CategoryName => Slot switch {
		FfmqWeaponSlot.Sword => Tier switch {
			0 => "Steel Sword",
			1 => "Knight Sword",
			2 => "Excalibur",
			3 => "Dragon Claw",
			_ => "Unknown Sword"
		},
		FfmqWeaponSlot.Axe => Tier switch {
			0 => "Battleaxe",
			1 => "Great Axe",
			2 => "Giant Axe",
			3 => "Zeus Axe",
			_ => "Unknown Axe"
		},
		FfmqWeaponSlot.Claw => Tier switch {
			0 => "Cat Claw",
			1 => "Charm Claw",
			2 => "Dragon Claw",
			3 => "Gemini Claw",
			_ => "Unknown Claw"
		},
		FfmqWeaponSlot.Bomb => Tier switch {
			0 => "Bombs",
			1 => "Mega Grenades",
			2 => "Jumbo Bombs",
			3 => "Super Bombs",
			_ => "Unknown Bomb"
		},
		_ => "Unknown"
	};
}

/// <summary>
/// FFMQ Armor data structure (6 bytes).
/// ROM address: $D2C000+
/// </summary>
/// <remarks>
/// 16 total armor pieces: 4 slots × 4 levels each
/// </remarks>
public record FfmqArmor {
	/// <summary>Armor ID (0-15)</summary>
	public byte Id { get; init; }

	/// <summary>Armor name</summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>Physical defense power</summary>
	public byte DefensePower { get; init; }

	/// <summary>Magic defense power</summary>
	public byte MagicDefense { get; init; }

	/// <summary>Elemental resistances</summary>
	public FfmqElement ElementResistance { get; init; }

	/// <summary>Status resistances</summary>
	public FfmqStatus StatusResistance { get; init; }

	/// <summary>Equipment slot</summary>
	public FfmqArmorSlot Slot { get; init; }
}

/// <summary>
/// FFMQ Consumable item data structure.
/// ROM address: $D28000+
/// </summary>
public record FfmqItem {
	/// <summary>Item ID</summary>
	public byte Id { get; init; }

	/// <summary>Item name</summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>Effect type (heal, cure status, etc.)</summary>
	public byte EffectType { get; init; }

	/// <summary>Effect power/amount</summary>
	public byte EffectPower { get; init; }

	/// <summary>Target type</summary>
	public FfmqTargetType TargetType { get; init; }

	/// <summary>Menu icon ID</summary>
	public byte IconId { get; init; }
}

/// <summary>
/// Reads FFMQ item/weapon/armor data from ROM
/// </summary>
public class FfmqItemReader {
	/// <summary>Consumable items base address (SNES: $D28000)</summary>
	public const int ItemsTableAddress = 0x128000; // PC address

	/// <summary>Weapons table base address (SNES: $D2A000)</summary>
	public const int WeaponsTableAddress = 0x12A000; // PC address

	/// <summary>Armor table base address (SNES: $D2C000)</summary>
	public const int ArmorTableAddress = 0x12C000; // PC address

	/// <summary>Number of consumable items</summary>
	public const int ItemCount = 15;

	/// <summary>Number of weapons</summary>
	public const int WeaponCount = 16;

	/// <summary>Number of armor pieces</summary>
	public const int ArmorCount = 16;

	/// <summary>Bytes per weapon entry</summary>
	public const int WeaponEntrySize = 8;

	/// <summary>Bytes per armor entry</summary>
	public const int ArmorEntrySize = 6;

	private readonly byte[] _romData;

	public FfmqItemReader(byte[] romData) {
		_romData = romData ?? throw new ArgumentNullException(nameof(romData));
	}

	/// <summary>
	/// Read a single weapon by ID
	/// </summary>
	public FfmqWeapon ReadWeapon(int id) {
		if (id < 0 || id >= WeaponCount) {
			throw new ArgumentOutOfRangeException(nameof(id), $"Weapon ID must be 0-{WeaponCount - 1}");
		}

		int offset = WeaponsTableAddress + (id * WeaponEntrySize);

		return new FfmqWeapon {
			Id = (byte)id,
			AttackPower = _romData[offset + 0x00],
			Element = (FfmqElement)_romData[offset + 0x01],
			SpecialEffectId = _romData[offset + 0x02],
			IconId = _romData[offset + 0x03],
			Slot = (FfmqWeaponSlot)_romData[offset + 0x04],
			RequiredLevel = _romData[offset + 0x05]
		};
	}

	/// <summary>
	/// Read all weapons from ROM
	/// </summary>
	public IEnumerable<FfmqWeapon> ReadAllWeapons() {
		for (int i = 0; i < WeaponCount; i++) {
			yield return ReadWeapon(i);
		}
	}

	/// <summary>
	/// Read a single armor piece by ID
	/// </summary>
	public FfmqArmor ReadArmor(int id) {
		if (id < 0 || id >= ArmorCount) {
			throw new ArgumentOutOfRangeException(nameof(id), $"Armor ID must be 0-{ArmorCount - 1}");
		}

		int offset = ArmorTableAddress + (id * ArmorEntrySize);

		return new FfmqArmor {
			Id = (byte)id,
			DefensePower = _romData[offset + 0x00],
			MagicDefense = _romData[offset + 0x01],
			ElementResistance = (FfmqElement)_romData[offset + 0x02],
			StatusResistance = (FfmqStatus)_romData[offset + 0x03],
			Slot = (FfmqArmorSlot)_romData[offset + 0x04]
		};
	}

	/// <summary>
	/// Read all armor pieces from ROM
	/// </summary>
	public IEnumerable<FfmqArmor> ReadAllArmor() {
		for (int i = 0; i < ArmorCount; i++) {
			yield return ReadArmor(i);
		}
	}
}
