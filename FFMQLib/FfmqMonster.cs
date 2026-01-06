namespace FFMQLib;

/// <summary>
/// FFMQ Monster data structure (variable size, ~16 bytes base stats).
/// ROM address: $D18000+ (stats), $D20000+ (names)
/// </summary>
/// <remarks>
/// Total: ~60 monsters (40 normal, 15 bosses, 4 Dark King phases)
/// </remarks>
public record FfmqMonster {
	/// <summary>Monster ID (0-59)</summary>
	public byte Id { get; init; }

	/// <summary>Monster name (ASCII)</summary>
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

	/// <summary>Experience reward (×10)</summary>
	public byte ExpReward { get; init; }

	/// <summary>Gold reward</summary>
	public ushort GoldReward { get; init; }

	/// <summary>Drop item ID</summary>
	public byte DropItemId { get; init; }

	/// <summary>Drop rate (1-255, 255 = guaranteed)</summary>
	public byte DropRate { get; init; }

	/// <summary>Elemental weaknesses</summary>
	public FfmqElement Weaknesses { get; init; }

	/// <summary>Elemental resistances</summary>
	public FfmqElement Resistances { get; init; }

	/// <summary>Status immunities</summary>
	public FfmqStatus StatusImmunities { get; init; }

	/// <summary>Graphics pointer (bank + offset)</summary>
	public int GraphicsPointer { get; init; }

	/// <summary>AI script ID</summary>
	public byte AiScriptId { get; init; }

	/// <summary>Is this a boss monster?</summary>
	public bool IsBoss => Id >= 45;

	/// <summary>Actual EXP value (ExpReward × 10)</summary>
	public int ActualExp => ExpReward * 10;
}

/// <summary>
/// Reads FFMQ monster data from ROM
/// </summary>
public class FfmqMonsterReader {
	/// <summary>Monster stats table base address (SNES: $D18000)</summary>
	public const int StatsTableAddress = 0x118000; // PC address

	/// <summary>Number of monsters in ROM</summary>
	public const int MonsterCount = 60;

	/// <summary>Bytes per monster stat entry</summary>
	public const int StatsEntrySize = 16;

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

		int offset = StatsTableAddress + (id * StatsEntrySize);

		return new FfmqMonster {
			Id = (byte)id,
			Name = id < _monsterNames.Length ? _monsterNames[id] : $"Monster_{id:D2}",
			Hp = BitConverter.ToUInt16(_romData, offset + 0x00),
			Attack = _romData[offset + 0x02],
			Defense = _romData[offset + 0x03],
			Speed = _romData[offset + 0x04],
			MagicAttack = _romData[offset + 0x05],
			MagicDefense = _romData[offset + 0x06],
			ExpReward = _romData[offset + 0x07],
			GoldReward = BitConverter.ToUInt16(_romData, offset + 0x08),
			DropItemId = _romData[offset + 0x0A],
			DropRate = _romData[offset + 0x0B],
			Weaknesses = (FfmqElement)_romData[offset + 0x0C],
			Resistances = (FfmqElement)_romData[offset + 0x0D],
			StatusImmunities = (FfmqStatus)_romData[offset + 0x0E],
			AiScriptId = _romData[offset + 0x0F]
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
