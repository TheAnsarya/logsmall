namespace DW4Lib.DataStructures;

/// <summary>
/// Dragon Warrior IV NES Spell data structure (6 bytes).
/// </summary>
public class Spell {
	/// <summary>
	/// Size of a single spell record in bytes.
	/// </summary>
	public const int Size = 6;

	/// <summary>
	/// Spell name index for lookup in name table.
	/// </summary>
	public byte NameIndex { get; set; }

	/// <summary>
	/// MP cost to cast.
	/// </summary>
	public byte MPCost { get; set; }

	/// <summary>
	/// Base damage or healing amount.
	/// </summary>
	public byte BasePower { get; set; }

	/// <summary>
	/// Spell type and target flags.
	/// Bits 0-2: Target type (0=single enemy, 1=all enemies, 2=single ally, 3=all allies, 4=self)
	/// Bits 3-5: Spell type (0=damage, 1=heal, 2=buff, 3=debuff, 4=status, 5=utility)
	/// Bits 6-7: Element (0=none, 1=fire, 2=ice, 3=electric)
	/// </summary>
	public byte TypeFlags { get; set; }

	/// <summary>
	/// Secondary effect ID (0 = none).
	/// </summary>
	public byte SecondaryEffect { get; set; }

	/// <summary>
	/// Success rate modifier (for status spells).
	/// </summary>
	public byte SuccessRate { get; set; }

	/// <summary>
	/// Target type extracted from TypeFlags.
	/// </summary>
	public SpellTarget Target => (SpellTarget)(TypeFlags & 0x07);

	/// <summary>
	/// Spell type extracted from TypeFlags.
	/// </summary>
	public SpellType Type => (SpellType)((TypeFlags >> 3) & 0x07);

	/// <summary>
	/// Element type extracted from TypeFlags.
	/// </summary>
	public SpellElement Element => (SpellElement)((TypeFlags >> 6) & 0x03);

	/// <summary>
	/// Parse a Spell from a 6-byte array.
	/// </summary>
	public static Spell FromBytes(byte[] data, int offset = 0) {
		if (data.Length < offset + Size) {
			throw new ArgumentException($"Data must be at least {Size} bytes from offset");
		}

		return new Spell {
			NameIndex = data[offset + 0],
			MPCost = data[offset + 1],
			BasePower = data[offset + 2],
			TypeFlags = data[offset + 3],
			SecondaryEffect = data[offset + 4],
			SuccessRate = data[offset + 5]
		};
	}

	/// <summary>
	/// Convert Spell to 6-byte array.
	/// </summary>
	public byte[] ToBytes() {
		return [
			NameIndex,
			MPCost,
			BasePower,
			TypeFlags,
			SecondaryEffect,
			SuccessRate
		];
	}
}

/// <summary>
/// Spell target types.
/// </summary>
public enum SpellTarget : byte {
	SingleEnemy = 0,
	AllEnemies = 1,
	SingleAlly = 2,
	AllAllies = 3,
	Self = 4,
	Field = 5
}

/// <summary>
/// Spell type categories.
/// </summary>
public enum SpellType : byte {
	Damage = 0,
	Heal = 1,
	Buff = 2,
	Debuff = 3,
	Status = 4,
	Utility = 5,
	Transport = 6,
	Special = 7
}

/// <summary>
/// Spell element types.
/// </summary>
public enum SpellElement : byte {
	None = 0,
	Fire = 1,
	Ice = 2,
	Electric = 3
}
