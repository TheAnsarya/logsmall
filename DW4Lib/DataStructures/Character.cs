namespace DW4Lib.DataStructures;

/// <summary>
/// Dragon Warrior IV NES Character/Party Member data structure (32 bytes).
/// Used for save data and RAM representation.
/// </summary>
public class Character {
	/// <summary>
	/// Size of a single character record in bytes.
	/// </summary>
	public const int Size = 32;

	/// <summary>
	/// Character name (8 characters max).
	/// </summary>
	public byte[] Name { get; set; } = new byte[8];

	/// <summary>
	/// Current level (1-99).
	/// </summary>
	public byte Level { get; set; }

	/// <summary>
	/// Character class/ID.
	/// </summary>
	public byte ClassID { get; set; }

	/// <summary>
	/// Current HP (low byte).
	/// </summary>
	public byte CurrentHPLow { get; set; }

	/// <summary>
	/// Current HP (high byte).
	/// </summary>
	public byte CurrentHPHigh { get; set; }

	/// <summary>
	/// Maximum HP (low byte).
	/// </summary>
	public byte MaxHPLow { get; set; }

	/// <summary>
	/// Maximum HP (high byte).
	/// </summary>
	public byte MaxHPHigh { get; set; }

	/// <summary>
	/// Current MP (low byte).
	/// </summary>
	public byte CurrentMPLow { get; set; }

	/// <summary>
	/// Current MP (high byte).
	/// </summary>
	public byte CurrentMPHigh { get; set; }

	/// <summary>
	/// Maximum MP (low byte).
	/// </summary>
	public byte MaxMPLow { get; set; }

	/// <summary>
	/// Maximum MP (high byte).
	/// </summary>
	public byte MaxMPHigh { get; set; }

	/// <summary>
	/// Strength stat.
	/// </summary>
	public byte Strength { get; set; }

	/// <summary>
	/// Agility stat.
	/// </summary>
	public byte Agility { get; set; }

	/// <summary>
	/// Vitality stat.
	/// </summary>
	public byte Vitality { get; set; }

	/// <summary>
	/// Intelligence stat.
	/// </summary>
	public byte Intelligence { get; set; }

	/// <summary>
	/// Luck stat.
	/// </summary>
	public byte Luck { get; set; }

	/// <summary>
	/// Experience points (4 bytes, little endian).
	/// </summary>
	public byte[] ExperienceBytes { get; set; } = new byte[4];

	/// <summary>
	/// Equipped weapon ID.
	/// </summary>
	public byte Weapon { get; set; }

	/// <summary>
	/// Equipped armor ID.
	/// </summary>
	public byte Armor { get; set; }

	/// <summary>
	/// Equipped shield ID.
	/// </summary>
	public byte Shield { get; set; }

	/// <summary>
	/// Equipped helmet ID.
	/// </summary>
	public byte Helmet { get; set; }

	/// <summary>
	/// Status effect flags.
	/// </summary>
	public byte StatusFlags { get; set; }

	/// <summary>
	/// Spells learned bitmask (low byte).
	/// </summary>
	public byte SpellsLow { get; set; }

	/// <summary>
	/// Spells learned bitmask (high byte).
	/// </summary>
	public byte SpellsHigh { get; set; }

	/// <summary>
	/// Combined 16-bit current HP.
	/// </summary>
	public ushort CurrentHP {
		get => (ushort)((CurrentHPHigh << 8) | CurrentHPLow);
		set {
			CurrentHPLow = (byte)(value & 0xff);
			CurrentHPHigh = (byte)((value >> 8) & 0xff);
		}
	}

	/// <summary>
	/// Combined 16-bit max HP.
	/// </summary>
	public ushort MaxHP {
		get => (ushort)((MaxHPHigh << 8) | MaxHPLow);
		set {
			MaxHPLow = (byte)(value & 0xff);
			MaxHPHigh = (byte)((value >> 8) & 0xff);
		}
	}

	/// <summary>
	/// Combined 16-bit current MP.
	/// </summary>
	public ushort CurrentMP {
		get => (ushort)((CurrentMPHigh << 8) | CurrentMPLow);
		set {
			CurrentMPLow = (byte)(value & 0xff);
			CurrentMPHigh = (byte)((value >> 8) & 0xff);
		}
	}

	/// <summary>
	/// Combined 16-bit max MP.
	/// </summary>
	public ushort MaxMP {
		get => (ushort)((MaxMPHigh << 8) | MaxMPLow);
		set {
			MaxMPLow = (byte)(value & 0xff);
			MaxMPHigh = (byte)((value >> 8) & 0xff);
		}
	}

	/// <summary>
	/// Combined 32-bit experience value.
	/// </summary>
	public uint Experience {
		get => (uint)(ExperienceBytes[0] | (ExperienceBytes[1] << 8) |
					  (ExperienceBytes[2] << 16) | (ExperienceBytes[3] << 24));
		set {
			ExperienceBytes[0] = (byte)(value & 0xff);
			ExperienceBytes[1] = (byte)((value >> 8) & 0xff);
			ExperienceBytes[2] = (byte)((value >> 16) & 0xff);
			ExperienceBytes[3] = (byte)((value >> 24) & 0xff);
		}
	}

	/// <summary>
	/// Parse a Character from a 32-byte array.
	/// </summary>
	public static Character FromBytes(byte[] data, int offset = 0) {
		if (data.Length < offset + Size) {
			throw new ArgumentException($"Data must be at least {Size} bytes from offset");
		}

		var character = new Character {
			Level = data[offset + 8],
			ClassID = data[offset + 9],
			CurrentHPLow = data[offset + 10],
			CurrentHPHigh = data[offset + 11],
			MaxHPLow = data[offset + 12],
			MaxHPHigh = data[offset + 13],
			CurrentMPLow = data[offset + 14],
			CurrentMPHigh = data[offset + 15],
			MaxMPLow = data[offset + 16],
			MaxMPHigh = data[offset + 17],
			Strength = data[offset + 18],
			Agility = data[offset + 19],
			Vitality = data[offset + 20],
			Intelligence = data[offset + 21],
			Luck = data[offset + 22],
			Weapon = data[offset + 27],
			Armor = data[offset + 28],
			Shield = data[offset + 29],
			Helmet = data[offset + 30],
			StatusFlags = data[offset + 31]
		};

		// Copy name bytes
		Array.Copy(data, offset, character.Name, 0, 8);

		// Copy experience bytes
		Array.Copy(data, offset + 23, character.ExperienceBytes, 0, 4);

		return character;
	}

	/// <summary>
	/// Convert Character to 32-byte array.
	/// </summary>
	public byte[] ToBytes() {
		var data = new byte[Size];

		Array.Copy(Name, 0, data, 0, 8);
		data[8] = Level;
		data[9] = ClassID;
		data[10] = CurrentHPLow;
		data[11] = CurrentHPHigh;
		data[12] = MaxHPLow;
		data[13] = MaxHPHigh;
		data[14] = CurrentMPLow;
		data[15] = CurrentMPHigh;
		data[16] = MaxMPLow;
		data[17] = MaxMPHigh;
		data[18] = Strength;
		data[19] = Agility;
		data[20] = Vitality;
		data[21] = Intelligence;
		data[22] = Luck;
		Array.Copy(ExperienceBytes, 0, data, 23, 4);
		data[27] = Weapon;
		data[28] = Armor;
		data[29] = Shield;
		data[30] = Helmet;
		data[31] = StatusFlags;

		return data;
	}
}
