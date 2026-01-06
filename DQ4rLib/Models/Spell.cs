namespace DQ4rLib.Models;

/// <summary>
/// DQ4r Spell data structure (~12 bytes per entry).
/// ROM address: $E60000+ (estimated)
/// </summary>
/// <remarks>
/// Total: ~80 spells (party spells, monster spells, special abilities)
/// </remarks>
public record Dq4rSpell {
	/// <summary>Spell ID (0-79)</summary>
	public byte Id { get; init; }

	/// <summary>Spell name</summary>
	public string Name { get; init; } = string.Empty;

	/// <summary>MP cost</summary>
	public byte MpCost { get; init; }

	/// <summary>Base power for damage/healing calculation</summary>
	public byte BasePower { get; init; }

	/// <summary>Target type</summary>
	public Dq4rTargetType TargetType { get; init; }

	/// <summary>Element</summary>
	public Dq4rElement Element { get; init; }

	/// <summary>Effect type: 0=damage, 1=heal, 2=status, 3=buff, 4=field</summary>
	public byte EffectType { get; init; }

	/// <summary>Status effect inflicted (for status spells)</summary>
	public Dq4rStatus StatusEffect { get; init; }

	/// <summary>Status success rate (0-100%)</summary>
	public byte StatusChance { get; init; }

	/// <summary>Animation ID</summary>
	public byte AnimationId { get; init; }

	/// <summary>Sound effect ID</summary>
	public byte SoundEffectId { get; init; }

	/// <summary>Can be used in battle?</summary>
	public bool UsableInBattle { get; init; }

	/// <summary>Can be used in field?</summary>
	public bool UsableInField { get; init; }

	/// <summary>Is damage spell?</summary>
	public bool IsDamageSpell => EffectType == 0;

	/// <summary>Is healing spell?</summary>
	public bool IsHealingSpell => EffectType == 1;

	/// <summary>Is status spell?</summary>
	public bool IsStatusSpell => EffectType == 2;

	/// <summary>Is buff/debuff spell?</summary>
	public bool IsBuffSpell => EffectType == 3;

	/// <summary>Is field spell (outside battle)?</summary>
	public bool IsFieldSpell => EffectType == 4;
}

/// <summary>
/// Character spell learning entry
/// </summary>
public record Dq4rSpellLearning {
	/// <summary>Character who learns this</summary>
	public Dq4rCharacter Character { get; init; }

	/// <summary>Spell ID learned</summary>
	public byte SpellId { get; init; }

	/// <summary>Level at which spell is learned</summary>
	public byte LearnLevel { get; init; }
}

/// <summary>
/// Reads DQ4r spell data from ROM
/// </summary>
public class Dq4rSpellReader {
	/// <summary>Spell data base address (PC: estimated)</summary>
	public const int SpellTableAddress = 0x1E0000;

	/// <summary>Spell learning table base address</summary>
	public const int SpellLearningAddress = 0x1E2000;

	/// <summary>Number of spells</summary>
	public const int SpellCount = 80;

	/// <summary>Bytes per spell entry</summary>
	public const int SpellEntrySize = 12;

	private readonly byte[] _romData;

	public Dq4rSpellReader(byte[] romData) {
		_romData = romData ?? throw new ArgumentNullException(nameof(romData));
	}

	/// <summary>
	/// Read a single spell by ID
	/// </summary>
	public Dq4rSpell ReadSpell(int id) {
		if (id < 0 || id >= SpellCount) {
			throw new ArgumentOutOfRangeException(nameof(id), $"Spell ID must be 0-{SpellCount - 1}");
		}

		int offset = SpellTableAddress + (id * SpellEntrySize);

		return new Dq4rSpell {
			Id = (byte)id,
			Name = $"Spell_{id:D2}", // TODO: Implement text decoding
			MpCost = _romData[offset + 0x00],
			BasePower = _romData[offset + 0x01],
			TargetType = (Dq4rTargetType)_romData[offset + 0x02],
			Element = (Dq4rElement)_romData[offset + 0x03],
			EffectType = _romData[offset + 0x04],
			StatusEffect = (Dq4rStatus)BitConverter.ToUInt16(_romData, offset + 0x05),
			StatusChance = _romData[offset + 0x07],
			AnimationId = _romData[offset + 0x08],
			SoundEffectId = _romData[offset + 0x09],
			UsableInBattle = (_romData[offset + 0x0A] & 0x01) != 0,
			UsableInField = (_romData[offset + 0x0A] & 0x02) != 0
		};
	}

	/// <summary>
	/// Read all spells from ROM
	/// </summary>
	public IEnumerable<Dq4rSpell> ReadAllSpells() {
		for (int i = 0; i < SpellCount; i++) {
			yield return ReadSpell(i);
		}
	}

	/// <summary>
	/// Get spells by element
	/// </summary>
	public IEnumerable<Dq4rSpell> GetSpellsByElement(Dq4rElement element) {
		return ReadAllSpells().Where(s => s.Element.HasFlag(element));
	}

	/// <summary>
	/// Get damage spells
	/// </summary>
	public IEnumerable<Dq4rSpell> GetDamageSpells() {
		return ReadAllSpells().Where(s => s.IsDamageSpell);
	}

	/// <summary>
	/// Get healing spells
	/// </summary>
	public IEnumerable<Dq4rSpell> GetHealingSpells() {
		return ReadAllSpells().Where(s => s.IsHealingSpell);
	}

	/// <summary>
	/// Get spells learnable by a character (requires learning table implementation)
	/// </summary>
	public IEnumerable<Dq4rSpellLearning> GetSpellsForCharacter(Dq4rCharacter character) {
		// Learning table format: [CharacterIndex] [SpellId] [Level] × N entries
		// This is a placeholder - actual ROM parsing needed
		return [];
	}
}
