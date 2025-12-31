namespace DW4Lib.DataStructures;

/// <summary>
/// Dragon Warrior IV NES Monster data structure (16 bytes).
/// Located in Bank $06 at various addresses.
/// </summary>
public class Monster {
	/// <summary>
	/// Size of a single monster record in bytes.
	/// </summary>
	public const int Size = 16;

	/// <summary>
	/// Monster name index for lookup in name table.
	/// </summary>
	public byte NameIndex { get; set; }

	/// <summary>
	/// Hit Points (0-255).
	/// </summary>
	public byte HP { get; set; }

	/// <summary>
	/// Attack Power (0-255).
	/// </summary>
	public byte Attack { get; set; }

	/// <summary>
	/// Defense Power (0-255).
	/// </summary>
	public byte Defense { get; set; }

	/// <summary>
	/// Agility - determines turn order.
	/// </summary>
	public byte Agility { get; set; }

	/// <summary>
	/// Experience points awarded on defeat (low byte).
	/// </summary>
	public byte ExpLow { get; set; }

	/// <summary>
	/// Experience points awarded on defeat (high byte).
	/// </summary>
	public byte ExpHigh { get; set; }

	/// <summary>
	/// Gold dropped on defeat (low byte).
	/// </summary>
	public byte GoldLow { get; set; }

	/// <summary>
	/// Gold dropped on defeat (high byte).
	/// </summary>
	public byte GoldHigh { get; set; }

	/// <summary>
	/// Item drop ID (0 = no drop).
	/// </summary>
	public byte ItemDrop { get; set; }

	/// <summary>
	/// Item drop rate (1/N chance, higher = rarer).
	/// </summary>
	public byte DropRate { get; set; }

	/// <summary>
	/// Spell/Ability 1 ID.
	/// </summary>
	public byte Spell1 { get; set; }

	/// <summary>
	/// Spell/Ability 2 ID.
	/// </summary>
	public byte Spell2 { get; set; }

	/// <summary>
	/// AI pattern/behavior flags.
	/// </summary>
	public byte AIPattern { get; set; }

	/// <summary>
	/// Elemental resistances (bitfield).
	/// Bit 0: Fire, Bit 1: Ice, Bit 2: Wind, Bit 3: Lightning, etc.
	/// </summary>
	public byte Resistances { get; set; }

	/// <summary>
	/// Sprite/graphics ID for battle display.
	/// </summary>
	public byte SpriteID { get; set; }

	/// <summary>
	/// Combined 16-bit experience value.
	/// </summary>
	public ushort Experience {
		get => (ushort)((ExpHigh << 8) | ExpLow);
		set {
			ExpLow = (byte)(value & 0xff);
			ExpHigh = (byte)((value >> 8) & 0xff);
		}
	}

	/// <summary>
	/// Combined 16-bit gold value.
	/// </summary>
	public ushort Gold {
		get => (ushort)((GoldHigh << 8) | GoldLow);
		set {
			GoldLow = (byte)(value & 0xff);
			GoldHigh = (byte)((value >> 8) & 0xff);
		}
	}

	/// <summary>
	/// Parse a Monster from a 16-byte array.
	/// </summary>
	public static Monster FromBytes(byte[] data, int offset = 0) {
		if (data.Length < offset + Size) {
			throw new ArgumentException($"Data must be at least {Size} bytes from offset");
		}

		return new Monster {
			NameIndex = data[offset + 0],
			HP = data[offset + 1],
			Attack = data[offset + 2],
			Defense = data[offset + 3],
			Agility = data[offset + 4],
			ExpLow = data[offset + 5],
			ExpHigh = data[offset + 6],
			GoldLow = data[offset + 7],
			GoldHigh = data[offset + 8],
			ItemDrop = data[offset + 9],
			DropRate = data[offset + 10],
			Spell1 = data[offset + 11],
			Spell2 = data[offset + 12],
			AIPattern = data[offset + 13],
			Resistances = data[offset + 14],
			SpriteID = data[offset + 15]
		};
	}

	/// <summary>
	/// Convert Monster to 16-byte array.
	/// </summary>
	public byte[] ToBytes() {
		return [
			NameIndex,
			HP,
			Attack,
			Defense,
			Agility,
			ExpLow,
			ExpHigh,
			GoldLow,
			GoldHigh,
			ItemDrop,
			DropRate,
			Spell1,
			Spell2,
			AIPattern,
			Resistances,
			SpriteID
		];
	}
}
