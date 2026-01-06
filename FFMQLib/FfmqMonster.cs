namespace FFMQLib;

/// <summary>
/// FFMQ Monster data structure.
/// Stats table: 14 bytes per entry at PC 0x14275
/// Level table: 3 bytes per entry at PC 0x1417C
/// </summary>
/// <remarks>
/// Total: 83 monsters
/// </remarks>
public record FfmqMonster {
	/// <summary>Monster ID (0-82)</summary>
	public byte Id { get; init; }

	/// <summary>Monster name (from text table)</summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>Hit points (2 bytes, little-endian)</summary>
	public ushort Hp { get; init; }

	/// <summary>Physical attack power</summary>
	public byte Attack { get; init; }

	/// <summary>Physical defense</summary>
	public byte Defense { get; init; }

	/// <summary>Speed (affects turn order)</summary>
	public byte Speed { get; init; }

	/// <summary>Magic attack power</summary>
	public byte MagicAttack { get; init; }

	/// <summary>Magic defense</summary>
	public byte MagicDefense { get; init; }

	/// <summary>Experience multiplier</summary>
	public byte ExpReward { get; init; }

	/// <summary>Gold multiplier</summary>
	public byte GoldReward { get; init; }

	/// <summary>Elemental weaknesses (16-bit flags)</summary>
	public FfmqElement Weaknesses { get; init; }

	/// <summary>Elemental resistances (16-bit flags)</summary>
	public FfmqElement Resistances { get; init; }

	/// <summary>Is this a boss monster? (ID >= 63)</summary>
	public bool IsBoss => Id >= 63;
}

/// <summary>
/// Reads FFMQ monster data from ROM
/// </summary>
public class FfmqMonsterReader {
	/// <summary>
	/// Monster stats table base address.
	/// SNES: Bank $02, ROM $C275 -> PC: 0x02 * 0x8000 + (0xC275 - 0x8000) = 0x14275
	/// </summary>
	public const int StatsTableAddress = 0x14275;

	/// <summary>
	/// Monster level/multiplier table base address.
	/// SNES: Bank $02, ROM $C17C -> PC: 0x02 * 0x8000 + (0xC17C - 0x8000) = 0x1417C
	/// </summary>
	public const int LevelTableAddress = 0x1417C;

	/// <summary>Number of monsters in ROM</summary>
	public const int MonsterCount = 83;

	/// <summary>Bytes per monster stat entry (14 bytes)</summary>
	public const int StatsEntrySize = 14;

	/// <summary>Bytes per monster level entry (3 bytes)</summary>
	public const int LevelEntrySize = 3;

	private readonly byte[] _romData;
	private readonly FfmqTextDecoder _textDecoder;
	private readonly string[] _monsterNames;

	public FfmqMonsterReader(byte[] romData) {
		_romData = romData ?? throw new ArgumentNullException(nameof(romData));
		_textDecoder = new FfmqTextDecoder();
		_monsterNames = _textDecoder.ReadTable(_romData, FfmqTextTables.MonsterNames);
	}

	/// <summary>
	/// Read a single monster by ID
	/// </summary>
	public FfmqMonster ReadMonster(int id) {
		if (id < 0 || id >= MonsterCount) {
			throw new ArgumentOutOfRangeException(nameof(id), $"Monster ID must be 0-{MonsterCount - 1}");
		}

		int statsOffset = StatsTableAddress + (id * StatsEntrySize);
		int levelOffset = LevelTableAddress + (id * LevelEntrySize);

		// Stats layout (14 bytes):
		// 0-1: HP (16-bit)
		// 2: Attack
		// 3: Defense
		// 4: Speed
		// 5: Magic Attack
		// 6-7: Resistances (16-bit flags)
		// 8: Magic Defense
		// 9: Magic Evade
		// 10: Accuracy
		// 11: Evade
		// 12-13: Weaknesses (16-bit flags)

		// Level layout (3 bytes):
		// 0: Level
		// 1: XP multiplier
		// 2: GP multiplier

		return new FfmqMonster {
			Id = (byte)id,
			Name = id < _monsterNames.Length ? _monsterNames[id] : $"Monster_{id:D2}",
			Hp = BitConverter.ToUInt16(_romData, statsOffset + 0),
			Attack = _romData[statsOffset + 2],
			Defense = _romData[statsOffset + 3],
			Speed = _romData[statsOffset + 4],
			MagicAttack = _romData[statsOffset + 5],
			Resistances = (FfmqElement)BitConverter.ToUInt16(_romData, statsOffset + 6),
			MagicDefense = _romData[statsOffset + 8],
			Weaknesses = (FfmqElement)BitConverter.ToUInt16(_romData, statsOffset + 12),
			ExpReward = _romData[levelOffset + 1],  // XP multiplier
			GoldReward = _romData[levelOffset + 2], // GP multiplier
		};
	}

	/// <summary>
	/// Read all monsters from ROM
	/// </summary>
	public IEnumerable<FfmqMonster> ReadAllMonsters() {
		for (int i = 0; i < MonsterCount; i++) {
			yield return ReadMonster(i);
		}
	}
}
