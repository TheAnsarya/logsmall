namespace FFMQLib;

/// <summary>
/// FFMQ Spell data structure (12 bytes).
/// ROM address: $D50000+
/// </summary>
/// <remarks>
/// 16 total spells: 4 categories × 4 levels each
/// No MP cost in FFMQ - unlimited magic use
/// </remarks>
public record FfmqSpell {
	/// <summary>Spell ID (0-15)</summary>
	public byte Id { get; init; }

	/// <summary>Spell name</summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>Base power for damage/healing calculation</summary>
	public byte BasePower { get; init; }

	/// <summary>Element flags</summary>
	public FfmqElement Element { get; init; }

	/// <summary>Effect type: 0=damage, 1=heal, 2=status</summary>
	public byte EffectType { get; init; }

	/// <summary>Target type</summary>
	public FfmqTargetType TargetType { get; init; }

	/// <summary>Animation ID for battle effects</summary>
	public byte AnimationId { get; init; }

	/// <summary>Sound effect ID</summary>
	public byte SoundEffectId { get; init; }

	/// <summary>Status effect ID (for status spells)</summary>
	public byte StatusEffectId { get; init; }

	/// <summary>Status success chance (0-100%)</summary>
	public byte StatusChance { get; init; }

	/// <summary>Required story flag to unlock</summary>
	public byte RequiredStoryFlag { get; init; }

	/// <summary>Is this a damage spell?</summary>
	public bool IsDamageSpell => EffectType == 0;

	/// <summary>Is this a healing spell?</summary>
	public bool IsHealingSpell => EffectType == 1;

	/// <summary>Is this a status spell?</summary>
	public bool IsStatusSpell => EffectType == 2;

	/// <summary>Targets enemies?</summary>
	public bool TargetsEnemies => TargetType is FfmqTargetType.SingleEnemy or FfmqTargetType.AllEnemies;

	/// <summary>Targets allies?</summary>
	public bool TargetsAllies => TargetType is FfmqTargetType.SingleAlly or FfmqTargetType.AllAllies or FfmqTargetType.Self;
}

/// <summary>
/// Spell categories in FFMQ
/// </summary>
public enum FfmqSpellCategory {
	WhiteMagic,  // Cure, Life, Heal, Exit
	BlackMagic,  // Fire, Blizzard, Thunder, Quake
	WizardMagic, // Aero, Flare, Meteor, White
	Special      // Sleep, Confuse, Silence, Death
}

/// <summary>
/// Reads FFMQ spell data from ROM
/// </summary>
public class FfmqSpellReader {
	/// <summary>Spell data base address (SNES: $D50000)</summary>
	public const int SpellTableAddress = 0x150000; // PC address

	/// <summary>Number of spells</summary>
	public const int SpellCount = 16;

	/// <summary>Bytes per spell entry</summary>
	public const int SpellEntrySize = 12;

	private readonly byte[] _romData;
	private readonly FfmqTextDecoder _textDecoder;
	private readonly string[] _spellNames;

	public FfmqSpellReader(byte[] romData) {
		_romData = romData ?? throw new ArgumentNullException(nameof(romData));
		_textDecoder = new FfmqTextDecoder();
		_spellNames = _textDecoder.ReadTable(_romData, FfmqTextTables.SpellNames);
	}

	/// <summary>
	/// Read a single spell by ID
	/// </summary>
	public FfmqSpell ReadSpell(int id) {
		if (id < 0 || id >= SpellCount) {
			throw new ArgumentOutOfRangeException(nameof(id), $"Spell ID must be 0-{SpellCount - 1}");
		}

		int offset = SpellTableAddress + (id * SpellEntrySize);

		return new FfmqSpell {
			Id = (byte)id,
			Name = id < _spellNames.Length ? _spellNames[id] : $"Spell_{id:D2}",
			BasePower = _romData[offset + 0x00],
			Element = (FfmqElement)_romData[offset + 0x01],
			EffectType = _romData[offset + 0x02],
			TargetType = (FfmqTargetType)_romData[offset + 0x03],
			AnimationId = _romData[offset + 0x04],
			SoundEffectId = _romData[offset + 0x05],
			StatusEffectId = _romData[offset + 0x06],
			StatusChance = _romData[offset + 0x07],
			RequiredStoryFlag = _romData[offset + 0x08]
		};
	}

	/// <summary>
	/// Read all spells from ROM
	/// </summary>
	public IEnumerable<FfmqSpell> ReadAllSpells() {
		for (int i = 0; i < SpellCount; i++) {
			yield return ReadSpell(i);
		}
	}

	/// <summary>
	/// Get spells by category
	/// </summary>
	public IEnumerable<FfmqSpell> GetSpellsByCategory(FfmqSpellCategory category) {
		int startIndex = (int)category * 4;
		for (int i = startIndex; i < startIndex + 4; i++) {
			yield return ReadSpell(i);
		}
	}

	/// <summary>
	/// Get category for a spell ID
	/// </summary>
	public static FfmqSpellCategory GetSpellCategory(int spellId) {
		return (FfmqSpellCategory)(spellId / 4);
	}
}
