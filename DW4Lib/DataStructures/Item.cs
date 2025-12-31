namespace DW4Lib.DataStructures;

/// <summary>
/// Dragon Warrior IV NES Item data structure (8 bytes).
/// </summary>
public class Item {
	/// <summary>
	/// Size of a single item record in bytes.
	/// </summary>
	public const int Size = 8;

	/// <summary>
	/// Item name index for lookup in name table.
	/// </summary>
	public byte NameIndex { get; set; }

	/// <summary>
	/// Item type flags.
	/// Bits 0-3: Equipment slot (0=consumable, 1=weapon, 2=armor, 3=shield, 4=helmet, 5=accessory)
	/// Bits 4-7: Special flags
	/// </summary>
	public byte TypeFlags { get; set; }

	/// <summary>
	/// Attack bonus (weapons) or Defense bonus (armor).
	/// </summary>
	public byte StatBonus { get; set; }

	/// <summary>
	/// Special effect ID (0 = none).
	/// </summary>
	public byte SpecialEffect { get; set; }

	/// <summary>
	/// Price in gold (low byte).
	/// </summary>
	public byte PriceLow { get; set; }

	/// <summary>
	/// Price in gold (high byte).
	/// </summary>
	public byte PriceHigh { get; set; }

	/// <summary>
	/// Who can equip this item (character bitmask).
	/// </summary>
	public byte EquipFlags { get; set; }

	/// <summary>
	/// Icon/sprite ID for menus.
	/// </summary>
	public byte IconID { get; set; }

	/// <summary>
	/// Equipment slot extracted from TypeFlags.
	/// </summary>
	public ItemType EquipmentSlot => (ItemType)(TypeFlags & 0x0f);

	/// <summary>
	/// Combined 16-bit price value.
	/// </summary>
	public ushort Price {
		get => (ushort)((PriceHigh << 8) | PriceLow);
		set {
			PriceLow = (byte)(value & 0xff);
			PriceHigh = (byte)((value >> 8) & 0xff);
		}
	}

	/// <summary>
	/// Check if a character can equip this item.
	/// </summary>
	public bool CanEquip(CharacterID character) {
		return (EquipFlags & (1 << (int)character)) != 0;
	}

	/// <summary>
	/// Parse an Item from an 8-byte array.
	/// </summary>
	public static Item FromBytes(byte[] data, int offset = 0) {
		if (data.Length < offset + Size) {
			throw new ArgumentException($"Data must be at least {Size} bytes from offset");
		}

		return new Item {
			NameIndex = data[offset + 0],
			TypeFlags = data[offset + 1],
			StatBonus = data[offset + 2],
			SpecialEffect = data[offset + 3],
			PriceLow = data[offset + 4],
			PriceHigh = data[offset + 5],
			EquipFlags = data[offset + 6],
			IconID = data[offset + 7]
		};
	}

	/// <summary>
	/// Convert Item to 8-byte array.
	/// </summary>
	public byte[] ToBytes() {
		return [
			NameIndex,
			TypeFlags,
			StatBonus,
			SpecialEffect,
			PriceLow,
			PriceHigh,
			EquipFlags,
			IconID
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
