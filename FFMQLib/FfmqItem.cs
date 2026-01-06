namespace FFMQLib;

/// <summary>
/// FFMQ Weapon data structure (16 bytes).
/// ROM address: 0x066000
/// </summary>
/// <remarks>
/// 15 weapons total
/// </remarks>
public record FfmqWeapon {
	/// <summary>Weapon ID (0-14)</summary>
	public byte Id { get; init; }

	/// <summary>Weapon name</summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>Attack power (0-255)</summary>
	public byte AttackPower { get; init; }

	/// <summary>Element flags</summary>
	public FfmqElement Element { get; init; }

	/// <summary>Special effect ID (0 = none)</summary>
	public byte SpecialEffectId { get; init; }
}

/// <summary>
/// FFMQ Armor data structure (16 bytes).
/// ROM address: 0x066100
/// </summary>
/// <remarks>
/// 7 armor pieces
/// </remarks>
public record FfmqArmor {
	/// <summary>Armor ID (0-6)</summary>
	public byte Id { get; init; }

	/// <summary>Armor name</summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>Physical defense power</summary>
	public byte DefensePower { get; init; }

	/// <summary>Magic defense / evade</summary>
	public byte MagicDefense { get; init; }

	/// <summary>Elemental resistances</summary>
	public FfmqElement ElementResistance { get; init; }

	/// <summary>Status resistances</summary>
	public FfmqStatus StatusResistance { get; init; }
}

/// <summary>
/// FFMQ Consumable item data structure (8 bytes).
/// ROM address: 0x066380
/// </summary>
public record FfmqItem {
	/// <summary>Item ID</summary>
	public byte Id { get; init; }

	/// <summary>Item name</summary>
	public string Name { get; init; } = string.Empty;
}

/// <summary>
/// Reads FFMQ item/weapon/armor data from ROM
/// </summary>
public class FfmqItemReader {
	/// <summary>Consumable items base address: 0x066380 (20 items, 8 bytes each)</summary>
	public const int ItemsTableAddress = 0x066380;

	/// <summary>Weapons table base address: 0x066000 (15 weapons, 16 bytes each)</summary>
	public const int WeaponsTableAddress = 0x066000;

	/// <summary>Armor table base address: 0x066100 (7 armors, 16 bytes each)</summary>
	public const int ArmorTableAddress = 0x066100;

	/// <summary>Helmets table base address: 0x066180 (7 helmets, 16 bytes each)</summary>
	public const int HelmetsTableAddress = 0x066180;

	/// <summary>Shields table base address: 0x066200 (7 shields, 16 bytes each)</summary>
	public const int ShieldsTableAddress = 0x066200;

	/// <summary>Accessories table base address: 0x066280 (11 accessories, 16 bytes each)</summary>
	public const int AccessoriesTableAddress = 0x066280;

	/// <summary>Number of consumable items</summary>
	public const int ItemCount = 20;

	/// <summary>Number of weapons</summary>
	public const int WeaponCount = 15;

	/// <summary>Number of armor pieces</summary>
	public const int ArmorCount = 7;

	/// <summary>Number of helmets</summary>
	public const int HelmetCount = 7;

	/// <summary>Number of shields</summary>
	public const int ShieldCount = 7;

	/// <summary>Number of accessories</summary>
	public const int AccessoryCount = 11;

	/// <summary>Bytes per weapon entry</summary>
	public const int WeaponEntrySize = 16;

	/// <summary>Bytes per armor/helmet/shield entry</summary>
	public const int ArmorEntrySize = 16;

	/// <summary>Bytes per accessory entry</summary>
	public const int AccessoryEntrySize = 16;

	/// <summary>Bytes per consumable entry</summary>
	public const int ConsumableEntrySize = 8;

	private readonly byte[] _romData;
	private readonly FfmqTextDecoder _textDecoder;
	private readonly string[] _weaponNames;
	private readonly string[] _armorNames;
	private readonly string[] _itemNames;

	public FfmqItemReader(byte[] romData) {
		_romData = romData ?? throw new ArgumentNullException(nameof(romData));
		_textDecoder = new FfmqTextDecoder();
		_weaponNames = _textDecoder.ReadTable(_romData, FfmqTextTables.WeaponNames);
		_armorNames = _textDecoder.ReadTable(_romData, FfmqTextTables.ArmorNames);
		_itemNames = _textDecoder.ReadTable(_romData, FfmqTextTables.ItemNames);
	}

	/// <summary>
	/// Read a single weapon by ID
	/// </summary>
	public FfmqWeapon ReadWeapon(int id) {
		if (id < 0 || id >= WeaponCount) {
			throw new ArgumentOutOfRangeException(nameof(id), $"Weapon ID must be 0-{WeaponCount - 1}");
		}

		int offset = WeaponsTableAddress + (id * WeaponEntrySize);

		// Weapon layout (16 bytes):
		// 0: Attack power
		// 1: Accuracy
		// 2: Element
		// 3: Special effects
		// 4: Character equip mask
		// 5-6: Buy price (16-bit)
		// 7-8: Sell price (16-bit)
		// 9: Flags
		// 10-15: Unused

		return new FfmqWeapon {
			Id = (byte)id,
			Name = id < _weaponNames.Length ? _weaponNames[id] : $"Weapon_{id:D2}",
			AttackPower = _romData[offset + 0],
			Element = (FfmqElement)_romData[offset + 2],
			SpecialEffectId = _romData[offset + 3],
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

		// Armor layout (16 bytes):
		// 0: Defense power
		// 1: Evade
		// 2: Element resistance
		// 3: Status resistance
		// 4: Character equip mask
		// 5-6: Buy price (16-bit)
		// 7-8: Sell price (16-bit)
		// 9: Flags
		// 10-15: Unused

		return new FfmqArmor {
			Id = (byte)id,
			Name = id < _armorNames.Length ? _armorNames[id] : $"Armor_{id:D2}",
			DefensePower = _romData[offset + 0],
			MagicDefense = _romData[offset + 1],
			ElementResistance = (FfmqElement)_romData[offset + 2],
			StatusResistance = (FfmqStatus)_romData[offset + 3],
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

	/// <summary>
	/// Read a single item by ID
	/// </summary>
	public FfmqItem ReadItem(int id) {
		if (id < 0 || id >= ItemCount) {
			throw new ArgumentOutOfRangeException(nameof(id), $"Item ID must be 0-{ItemCount - 1}");
		}

		return new FfmqItem {
			Id = (byte)id,
			Name = id < _itemNames.Length ? _itemNames[id] : $"Item_{id:D2}"
			// Note: Item stats would need additional ROM address research
		};
	}

	/// <summary>
	/// Read all items from ROM
	/// </summary>
	public IEnumerable<FfmqItem> ReadAllItems() {
		for (int i = 0; i < ItemCount; i++) {
			yield return ReadItem(i);
		}
	}

	/// <summary>
	/// Get all helmet names
	/// </summary>
	public string[] GetHelmetNames() => _textDecoder.ReadTable(_romData, FfmqTextTables.HelmetNames);

	/// <summary>
	/// Get all shield names
	/// </summary>
	public string[] GetShieldNames() => _textDecoder.ReadTable(_romData, FfmqTextTables.ShieldNames);

	/// <summary>
	/// Get all accessory names
	/// </summary>
	public string[] GetAccessoryNames() => _textDecoder.ReadTable(_romData, FfmqTextTables.AccessoryNames);

	/// <summary>
	/// Get all attack/ability names
	/// </summary>
	public string[] GetAttackNames() => _textDecoder.ReadTable(_romData, FfmqTextTables.AttackNames);

	/// <summary>
	/// Get all location names
	/// </summary>
	public string[] GetLocationNames() => _textDecoder.ReadTable(_romData, FfmqTextTables.LocationNames);
}
