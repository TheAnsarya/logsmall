namespace DW4Lib.DataStructures;

/// <summary>
/// Dragon Warrior IV NES Monster data structure (27 bytes).
/// Located in Bank 6 at $A2A2.
/// Format documented in monster_byte_structure.md with CDL verification.
/// </summary>
/// <remarks>
/// CDL Verification: Bank 0x06 has 99.8% coverage (verified 2026-01-03).
/// </remarks>
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
	/// Bytes 0-1: Experience points (16-bit little endian).
	/// </summary>
	public ushort Experience { get; set; }

	/// <summary>
	/// Bytes 2-3: Gold dropped (16-bit little endian).
	/// </summary>
	public ushort Gold { get; set; }

	/// <summary>
	/// Bytes 4-5: Hit Points (16-bit little endian).
	/// Research indicates this is HP, not unknown bytes.
	/// </summary>
	public ushort HitPoints { get; set; }

	/// <summary>
	/// Byte 6: Attack Power.
	/// </summary>
	public byte Attack { get; set; }

	/// <summary>
	/// Byte 7: Defense Power.
	/// </summary>
	public byte Defense { get; set; }

	/// <summary>
	/// Byte 8: Agility.
	/// </summary>
	public byte Agility { get; set; }

	/// <summary>
	/// Bytes 9-14: Skill/ability data (6 bytes).
	/// High bits encode action chance, action count, HP regeneration.
	/// </summary>
	public byte[] SkillData { get; set; } = new byte[6];

	/// <summary>
	/// Bytes 15-18: Unknown/AI behavior (4 bytes).
	/// Possibly AI patterns or behavior flags.
	/// </summary>
	public byte[] BehaviorData { get; set; } = new byte[4];

	/// <summary>
	/// Byte 19: Item drop ID.
	/// References the item table for what this monster can drop.
	/// </summary>
	public byte ItemDropId { get; set; }

	/// <summary>
	/// Byte 20: Unknown byte (Vaxherd: byte 17).
	/// </summary>
	public byte Unknown20 { get; set; }

	/// <summary>
	/// Byte 21: Unknown byte (Vaxherd: byte 18).
	/// </summary>
	public byte Unknown21 { get; set; }

	/// <summary>
	/// Byte 22: Metal monster flags (Vaxherd: byte 19).
	/// Bits 0-1: Metal flag ($03 mask) - if set, takes 0-1 damage.
	/// </summary>
	public byte MetalFlags { get; set; }

	/// <summary>
	/// Byte 23: Drop rate and flags (Vaxherd: byte 20).
	/// Bits 0-2: Drop rate (0=0%, 1=1/2, 2=1/4, 3=1/8, 4=1/16, 5=1/32, 6=1/256, 7=100%).
	/// </summary>
	public byte DropRateFlags { get; set; }

	/// <summary>
	/// Byte 24: Status vulnerability flags (Vaxherd: byte 21).
	/// Bit 6: Paralysis vulnerability.
	/// Bit 7: Confusion vulnerability or Bounce flag.
	/// </summary>
	public byte StatusVulnerability { get; set; }

	/// <summary>
	/// Byte 25: Unknown byte.
	/// </summary>
	public byte Unknown25 { get; set; }

	/// <summary>
	/// Byte 26: Unknown byte.
	/// </summary>
	public byte Unknown26 { get; set; }

	/// <summary>
	/// Whether this monster is a Metal type (takes 0-1 damage, flees often).
	/// </summary>
	public bool IsMetal => (MetalFlags & 0x03) != 0;

	/// <summary>
	/// Get the item drop rate as a fraction.
	/// </summary>
	public string DropRateString => (DropRateFlags & 0x07) switch {
		0 => "0%",
		1 => "1/2 (50%)",
		2 => "1/4 (25%)",
		3 => "1/8 (12.5%)",
		4 => "1/16 (6.25%)",
		5 => "1/32 (3.125%)",
		6 => "1/256 (0.39%)",
		7 => "100%",
		_ => "Unknown"
	};

	/// <summary>
	/// Whether this monster is vulnerable to paralysis.
	/// </summary>
	public bool VulnerableToParalysis => (StatusVulnerability & 0x40) != 0;

	/// <summary>
	/// Whether this monster is vulnerable to confusion or has Bounce.
	/// </summary>
	public bool VulnerableToConfusionOrBounce => (StatusVulnerability & 0x80) != 0;

	/// <summary>
	/// Parse a Monster from a 27-byte array.
	/// </summary>
	public static Monster FromBytes(byte[] data, int offset = 0) {
		if (data.Length < offset + Size) {
			throw new ArgumentException($"Data must be at least {Size} bytes from offset");
		}

		var monster = new Monster {
			Experience = (ushort)(data[offset + 0] | (data[offset + 1] << 8)),
			Gold = (ushort)(data[offset + 2] | (data[offset + 3] << 8)),
			HitPoints = (ushort)(data[offset + 4] | (data[offset + 5] << 8)),
			Attack = data[offset + 6],
			Defense = data[offset + 7],
			Agility = data[offset + 8],
			ItemDropId = data[offset + 19],
			Unknown20 = data[offset + 20],
			Unknown21 = data[offset + 21],
			MetalFlags = data[offset + 22],
			DropRateFlags = data[offset + 23],
			StatusVulnerability = data[offset + 24],
			Unknown25 = data[offset + 25],
			Unknown26 = data[offset + 26]
		};

		// Copy skill data (bytes 9-14)
		Array.Copy(data, offset + 9, monster.SkillData, 0, 6);

		// Copy behavior data (bytes 15-18)
		Array.Copy(data, offset + 15, monster.BehaviorData, 0, 4);

		return monster;
	}

	/// <summary>
	/// Convert Monster to 27-byte array.
	/// </summary>
	public byte[] ToBytes() {
		var data = new byte[Size];

		data[0] = (byte)(Experience & 0xff);
		data[1] = (byte)((Experience >> 8) & 0xff);
		data[2] = (byte)(Gold & 0xff);
		data[3] = (byte)((Gold >> 8) & 0xff);
		data[4] = (byte)(HitPoints & 0xff);
		data[5] = (byte)((HitPoints >> 8) & 0xff);
		data[6] = Attack;
		data[7] = Defense;
		data[8] = Agility;

		// Copy skill data (bytes 9-14)
		Array.Copy(SkillData, 0, data, 9, 6);

		// Copy behavior data (bytes 15-18)
		Array.Copy(BehaviorData, 0, data, 15, 4);

		data[19] = ItemDropId;
		data[20] = Unknown20;
		data[21] = Unknown21;
		data[22] = MetalFlags;
		data[23] = DropRateFlags;
		data[24] = StatusVulnerability;
		data[25] = Unknown25;
		data[26] = Unknown26;

		return data;
	}
}
