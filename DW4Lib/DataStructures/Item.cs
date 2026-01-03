namespace DW4Lib.DataStructures;

/// <summary>
/// Dragon Warrior IV NES Item data structure (8 bytes).
/// Located in Bank 7 at CPU $8000 (file offset $1C010).
/// </summary>
/// <remarks>
/// CDL Verification: Bank 0x07 has 78.1% coverage (verified 2026-01-03).
/// Note: Exact byte layout needs additional research/verification.
/// Current structure based on asset-map.json definition.
/// </remarks>
public class Item {
	/// <summary>
	/// Size of a single item record in bytes.
	/// </summary>
	public const int Size = 8;

	/// <summary>
	/// ROM bank containing item data.
	/// </summary>
	public const int Bank = 7;

	/// <summary>
	/// CPU address of item table start.
	/// </summary>
	public const int TableAddress = 0x8000;

	/// <summary>
	/// File offset (ROM offset including NES header) of item table.
	/// </summary>
	public const int FileOffset = 0x1C010;

	/// <summary>
	/// Total number of items in the game.
	/// </summary>
	public const int TotalItems = 220;

	/// <summary>
	/// Byte 0: Item type flags.
	/// Bits 0-3: Equipment slot (0=consumable, 1=weapon, 2=armor, 3=shield, 4=helmet, 5=accessory).
	/// Bits 4-7: Special flags (may include curse flag, usability flags, etc.).
	/// </summary>
	public byte TypeFlags { get; set; }

	/// <summary>
	/// Byte 1: Stat modifier.
	/// For weapons: Attack bonus. For armor/shields/helmets: Defense bonus.
	/// For accessories: May be agility or other stat bonus.
	/// Signed byte (-128 to +127) for cursed equipment with negative values.
	/// </summary>
	public sbyte StatModifier { get; set; }

	/// <summary>
	/// Byte 2: Special effect flags.
	/// May include elemental effects, status effects, or special abilities.
	/// </summary>
	public byte SpecialFlags { get; set; }

	/// <summary>
	/// Bytes 3-4: Buy price in gold (16-bit, big-endian per asset-map.json).
	/// Buy price is typically 2x sell price.
	/// </summary>
	public ushort BuyPrice { get; set; }

	/// <summary>
	/// Bytes 5-6: Sell price in gold (16-bit, big-endian per asset-map.json).
	/// Sell price is typically 1/2 buy price.
	/// </summary>
	public ushort SellPrice { get; set; }

	/// <summary>
	/// Byte 7: Who can equip this item (character bitmask).
	/// Bit 0 = Hero, Bit 1 = Ragnar, Bit 2 = Alena, Bit 3 = Cristo,
	/// Bit 4 = Brey, Bit 5 = Taloon, Bit 6 = Mara, Bit 7 = Nara.
	/// </summary>
	public byte EquipFlags { get; set; }

	/// <summary>
	/// Equipment slot extracted from TypeFlags (lower 4 bits).
	/// </summary>
	public ItemType EquipmentSlot => (ItemType)(TypeFlags & 0x0f);

	/// <summary>
	/// Returns true if this is a cursed item (negative stat modifier).
	/// </summary>
	public bool IsCursed => StatModifier < 0;

	/// <summary>
	/// Check if a character can equip this item.
	/// </summary>
	public bool CanEquip(CharacterID character) {
		return (EquipFlags & (1 << (int)character)) != 0;
	}

	/// <summary>
	/// Returns a string listing all characters who can equip this item.
	/// </summary>
	public string EquipableByString {
		get {
			var characters = new List<string>();
			foreach (CharacterID c in Enum.GetValues<CharacterID>()) {
				if (CanEquip(c)) {
					characters.Add(c.ToString());
				}
			}
			return characters.Count > 0 ? string.Join(", ", characters) : "Nobody";
		}
	}

	/// <summary>
	/// Parse an Item from an 8-byte array.
	/// </summary>
	public static Item FromBytes(byte[] data, int offset = 0) {
		if (data.Length < offset + Size) {
			throw new ArgumentException($"Data must be at least {Size} bytes from offset");
		}

		return new Item {
			TypeFlags = data[offset + 0],
			StatModifier = (sbyte)data[offset + 1],
			SpecialFlags = data[offset + 2],
			// Big-endian prices per asset-map.json
			BuyPrice = (ushort)((data[offset + 3] << 8) | data[offset + 4]),
			SellPrice = (ushort)((data[offset + 5] << 8) | data[offset + 6]),
			EquipFlags = data[offset + 7]
		};
	}

	/// <summary>
	/// Convert Item to 8-byte array.
	/// </summary>
	public byte[] ToBytes() {
		return [
			TypeFlags,
			(byte)StatModifier,
			SpecialFlags,
			(byte)((BuyPrice >> 8) & 0xff),
			(byte)(BuyPrice & 0xff),
			(byte)((SellPrice >> 8) & 0xff),
			(byte)(SellPrice & 0xff),
			EquipFlags
		];
	}
}

/// <summary>
/// Item type enumeration.
/// </summary>
public enum ItemType : byte {
	Consumable = 0,
	Weapon = 1,
	Armor = 2,
	Shield = 3,
	Helmet = 4,
	Accessory = 5,
	KeyItem = 6,
	Special = 7
}

/// <summary>
/// Character ID enumeration for equipment flags.
/// </summary>
public enum CharacterID : byte {
	Hero = 0,
	Ragnar = 1,
	Alena = 2,
	Cristo = 3,
	Brey = 4,
	Taloon = 5,
	Mara = 6,
	Nara = 7
}
