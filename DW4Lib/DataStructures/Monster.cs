namespace DW4Lib.DataStructures;

/// <summary>
/// Dragon Warrior IV NES Monster data structure (27 bytes).
/// Located in Bank 6 at $A2A2.
/// Format based on research in bank6_monster_table.txt.
/// </summary>
public class Monster {
	/// <summary>
	/// Size of a single monster record in bytes.
	/// </summary>
	public const int Size = 27;

	/// <summary>
	/// ROM bank containing monster data.
	/// </summary>
	public const int Bank = 6;

	/// <summary>
	/// CPU address of monster table start.
	/// </summary>
	public const int TableAddress = 0xA2A2;

	/// <summary>
	/// Raw bytes 0-1: Experience points (16-bit little endian).
	/// </summary>
	public ushort Experience { get; set; }

	/// <summary>
	/// Raw bytes 2-3: Gold dropped (16-bit little endian).
	/// </summary>
	public ushort Gold { get; set; }

	/// <summary>
	/// Raw byte 4: Unknown byte 4.
	/// </summary>
	public byte Byte4 { get; set; }

	/// <summary>
	/// Raw byte 5: Unknown byte 5.
	/// </summary>
	public byte Byte5 { get; set; }

	/// <summary>
	/// Raw byte 6: Attack Power.
	/// </summary>
	public byte Attack { get; set; }

	/// <summary>
	/// Raw byte 7: Defense Power.
	/// </summary>
	public byte Defense { get; set; }

	/// <summary>
	/// Raw byte 8: Agility.
	/// </summary>
	public byte Agility { get; set; }

	/// <summary>
	/// Raw byte 9: Unknown byte 9.
	/// </summary>
	public byte Byte9 { get; set; }

	/// <summary>
	/// Raw byte 10: Unknown byte 10 (possibly second ATK).
	/// </summary>
	public byte Byte10 { get; set; }

	/// <summary>
	/// Raw byte 11: Unknown byte 11 (possibly second DEF).
	/// </summary>
	public byte Byte11 { get; set; }

	/// <summary>
	/// Raw byte 12: Unknown byte 12.
	/// </summary>
	public byte Byte12 { get; set; }

	/// <summary>
	/// Raw byte 13: Unknown byte 13.
	/// </summary>
	public byte Byte13 { get; set; }

	/// <summary>
	/// Raw byte 14: Unknown byte 14.
	/// </summary>
	public byte Byte14 { get; set; }

	/// <summary>
	/// Raw byte 15: Unknown byte 15 (possibly HP high).
	/// </summary>
	public byte Byte15 { get; set; }

	/// <summary>
	/// Raw byte 16: Unknown byte 16.
	/// </summary>
	public byte Byte16 { get; set; }

	/// <summary>
	/// Raw byte 17: Unknown byte 17.
	/// </summary>
	public byte Byte17 { get; set; }

	/// <summary>
	/// Raw byte 18: Unknown byte 18.
	/// </summary>
	public byte Byte18 { get; set; }

	/// <summary>
	/// Raw byte 19: Unknown byte 19.
	/// </summary>
	public byte Byte19 { get; set; }

	/// <summary>
	/// Raw byte 20: Unknown byte 20.
	/// </summary>
	public byte Byte20 { get; set; }

	/// <summary>
	/// Raw byte 21: Metal Monster flag (non-zero = metal).
	/// </summary>
	public byte MetalFlag { get; set; }

	/// <summary>
	/// Raw byte 22: Item drop ID.
	/// </summary>
	public byte ItemDrop { get; set; }

	/// <summary>
	/// Raw byte 23: Status/immunity flags.
	/// </summary>
	public byte StatusFlags { get; set; }

	/// <summary>
	/// Raw byte 24: Unknown byte 24.
	/// </summary>
	public byte Byte24 { get; set; }

	/// <summary>
	/// Raw byte 25: Unknown byte 25.
	/// </summary>
	public byte Byte25 { get; set; }

	/// <summary>
	/// Raw byte 26: Unknown byte 26.
	/// </summary>
	public byte Byte26 { get; set; }

	/// <summary>
	/// Whether this monster is a Metal type (takes 0-1 damage, flees often).
	/// </summary>
	public bool IsMetal => MetalFlag != 0;

	/// <summary>
	/// Parse a Monster from a 27-byte array.
	/// </summary>
	public static Monster FromBytes(byte[] data, int offset = 0) {
		if (data.Length < offset + Size) {
			throw new ArgumentException($"Data must be at least {Size} bytes from offset");
		}

		return new Monster {
			Experience = (ushort)(data[offset + 0] | (data[offset + 1] << 8)),
			Gold = (ushort)(data[offset + 2] | (data[offset + 3] << 8)),
			Byte4 = data[offset + 4],
			Byte5 = data[offset + 5],
			Attack = data[offset + 6],
			Defense = data[offset + 7],
			Agility = data[offset + 8],
			Byte9 = data[offset + 9],
			Byte10 = data[offset + 10],
			Byte11 = data[offset + 11],
			Byte12 = data[offset + 12],
			Byte13 = data[offset + 13],
			Byte14 = data[offset + 14],
			Byte15 = data[offset + 15],
			Byte16 = data[offset + 16],
			Byte17 = data[offset + 17],
			Byte18 = data[offset + 18],
			Byte19 = data[offset + 19],
			Byte20 = data[offset + 20],
			MetalFlag = data[offset + 21],
			ItemDrop = data[offset + 22],
			StatusFlags = data[offset + 23],
			Byte24 = data[offset + 24],
			Byte25 = data[offset + 25],
			Byte26 = data[offset + 26]
		};
	}

	/// <summary>
	/// Convert Monster to 27-byte array.
	/// </summary>
	public byte[] ToBytes() {
		return [
			(byte)(Experience & 0xff),
			(byte)((Experience >> 8) & 0xff),
			(byte)(Gold & 0xff),
			(byte)((Gold >> 8) & 0xff),
			Byte4,
			Byte5,
			Attack,
			Defense,
			Agility,
			Byte9,
			Byte10,
			Byte11,
			Byte12,
			Byte13,
			Byte14,
			Byte15,
			Byte16,
			Byte17,
			Byte18,
			Byte19,
			Byte20,
			MetalFlag,
			ItemDrop,
			StatusFlags,
			Byte24,
			Byte25,
			Byte26
		];
	}
}
