namespace DQ4rLib.Models;

/// <summary>
/// DQ4r Item data structure (~16 bytes per entry).
/// ROM address: $E40000+ (estimated)
/// </summary>
/// <remarks>
/// Total: ~250 items (weapons, armor, accessories, consumables, key items)
/// </remarks>
public record Dq4rItem {
	/// <summary>Item ID (0-249)</summary>
	public byte Id { get; init; }

	/// <summary>Item name</summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>Item category</summary>
	public Dq4rItemCategory Category { get; init; }

	/// <summary>Attack power (weapons only)</summary>
	public byte Attack { get; init; }

	/// <summary>Defense power (armor/shields only)</summary>
	public byte Defense { get; init; }

	/// <summary>Agility modifier</summary>
	public sbyte AgilityMod { get; init; }

	/// <summary>Luck modifier</summary>
	public sbyte LuckMod { get; init; }

	/// <summary>Buy price in gold (0 = cannot buy)</summary>
	public ushort BuyPrice { get; init; }

	/// <summary>Sell price in gold (typically BuyPrice/2)</summary>
	public ushort SellPrice => (ushort)(BuyPrice / 2);

	/// <summary>Who can equip (bitmask of Dq4rCharacter)</summary>
	public Dq4rCharacter EquipFlags { get; init; }

	/// <summary>Element for weapon damage or armor resistance</summary>
	public Dq4rElement Element { get; init; }

	/// <summary>Status effect when used/equipped</summary>
	public Dq4rStatus StatusEffect { get; init; }

	/// <summary>Spell cast when used as item (0 = none)</summary>
	public byte UsableSpellId { get; init; }

	/// <summary>Is cursed equipment?</summary>
	public bool IsCursed { get; init; }

	/// <summary>Can be used in battle?</summary>
	public bool UsableInBattle { get; init; }

	/// <summary>Can be used in field?</summary>
	public bool UsableInField { get; init; }

	/// <summary>Is weapon?</summary>
	public bool IsWeapon => Category == Dq4rItemCategory.Weapon;

	/// <summary>Is armor/shield/helmet/accessory?</summary>
	public bool IsEquipment => Category is Dq4rItemCategory.Weapon or Dq4rItemCategory.Shield
		or Dq4rItemCategory.Armor or Dq4rItemCategory.Helmet or Dq4rItemCategory.Accessory;

	/// <summary>Is consumable?</summary>
	public bool IsConsumable => Category == Dq4rItemCategory.Consumable;

	/// <summary>Check if character can equip this item</summary>
	public bool CanEquip(Dq4rCharacter character) => EquipFlags.HasFlag(character);
}

/// <summary>
/// Reads DQ4r item data from ROM
/// </summary>
public class Dq4rItemReader {
	/// <summary>Item data base address (PC: estimated)</summary>
	public const int ItemTableAddress = 0x1C0000;

	/// <summary>Item names base address (PC: estimated)</summary>
	public const int ItemNamesAddress = 0x1C4000;

	/// <summary>Number of items</summary>
	public const int ItemCount = 250;

	/// <summary>Bytes per item entry</summary>
	public const int ItemEntrySize = 16;

	private readonly byte[] _romData;

	public Dq4rItemReader(byte[] romData) {
		_romData = romData ?? throw new ArgumentNullException(nameof(romData));
	}

	/// <summary>
	/// Read a single item by ID
	/// </summary>
	public Dq4rItem ReadItem(int id) {
		if (id < 0 || id >= ItemCount) {
			throw new ArgumentOutOfRangeException(nameof(id), $"Item ID must be 0-{ItemCount - 1}");
		}

		int offset = ItemTableAddress + (id * ItemEntrySize);

		return new Dq4rItem {
			Id = (byte)id,
			Name = $"Item_{id:D3}", // TODO: Implement text decoding
			Category = (Dq4rItemCategory)(_romData[offset + 0x00] & 0x07),
			Attack = _romData[offset + 0x01],
			Defense = _romData[offset + 0x02],
			AgilityMod = (sbyte)_romData[offset + 0x03],
			LuckMod = (sbyte)_romData[offset + 0x04],
			BuyPrice = BitConverter.ToUInt16(_romData, offset + 0x05),
			EquipFlags = (Dq4rCharacter)_romData[offset + 0x07],
			Element = (Dq4rElement)_romData[offset + 0x08],
			StatusEffect = (Dq4rStatus)BitConverter.ToUInt16(_romData, offset + 0x09),
			UsableSpellId = _romData[offset + 0x0B],
			IsCursed = (_romData[offset + 0x0C] & 0x01) != 0,
			UsableInBattle = (_romData[offset + 0x0C] & 0x02) != 0,
			UsableInField = (_romData[offset + 0x0C] & 0x04) != 0
		};
	}

	/// <summary>
	/// Read all items from ROM
	/// </summary>
	public IEnumerable<Dq4rItem> ReadAllItems() {
		for (int i = 0; i < ItemCount; i++) {
			yield return ReadItem(i);
		}
	}

	/// <summary>
	/// Get items by category
	/// </summary>
	public IEnumerable<Dq4rItem> GetItemsByCategory(Dq4rItemCategory category) {
		return ReadAllItems().Where(i => i.Category == category);
	}

	/// <summary>
	/// Get items equippable by a specific character
	/// </summary>
	public IEnumerable<Dq4rItem> GetEquippableItems(Dq4rCharacter character) {
		return ReadAllItems().Where(i => i.IsEquipment && i.CanEquip(character));
	}
}
