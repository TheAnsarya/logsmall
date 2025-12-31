using DW4Lib.DataStructures;

namespace DW4Lib.DQ3r;

/// <summary>
/// Converts DW4 NES spells to DQ3r SNES format.
/// </summary>
public static class SpellToDQ3r {
	/// <summary>
	/// MP cost scaling factor for SNES economy.
	/// </summary>
	public const double MPScaling = 1.5;

	/// <summary>
	/// Power scaling factor for 16-bit range.
	/// </summary>
	public const double PowerScaling = 2.5;

	/// <summary>
	/// Map DW4 SpellTarget to DQ3r SpellTarget.
	/// </summary>
	private static readonly Dictionary<SpellTarget, DQ3rSpellTarget> TargetMapping = new() {
		{ SpellTarget.Self, DQ3rSpellTarget.Self },
		{ SpellTarget.SingleAlly, DQ3rSpellTarget.SingleAlly },
		{ SpellTarget.AllAllies, DQ3rSpellTarget.AllAllies },
		{ SpellTarget.SingleEnemy, DQ3rSpellTarget.SingleEnemy },
		{ SpellTarget.AllEnemies, DQ3rSpellTarget.AllEnemies },
		{ SpellTarget.Field, DQ3rSpellTarget.Self }, // Field spells target self
	};

	/// <summary>
	/// Map DW4 SpellType to DQ3r SpellCategory.
	/// </summary>
	private static readonly Dictionary<SpellType, DQ3rSpellCategory> CategoryMapping = new() {
		{ SpellType.Damage, DQ3rSpellCategory.Attack },
		{ SpellType.Heal, DQ3rSpellCategory.Healing },
		{ SpellType.Buff, DQ3rSpellCategory.Support },
		{ SpellType.Debuff, DQ3rSpellCategory.Debuff },
		{ SpellType.Status, DQ3rSpellCategory.Debuff },
		{ SpellType.Utility, DQ3rSpellCategory.Field },
		{ SpellType.Transport, DQ3rSpellCategory.Field },
		{ SpellType.Special, DQ3rSpellCategory.Special },
	};

	/// <summary>
	/// Map DW4 SpellElement to DQ3r SpellElement.
	/// </summary>
	private static readonly Dictionary<SpellElement, DQ3rSpellElement> ElementMapping = new() {
		{ SpellElement.None, DQ3rSpellElement.None },
		{ SpellElement.Fire, DQ3rSpellElement.Fire },
		{ SpellElement.Ice, DQ3rSpellElement.Ice },
		{ SpellElement.Electric, DQ3rSpellElement.Lightning },
	};

	/// <summary>
	/// Convert a single DW4 spell to DQ3r format.
	/// </summary>
	public static DQ3rSpell Convert(Spell dw4Spell, int id, string name = "") {
		// Get animation and sound IDs from mapping tables
		int animationId = DQ3rAnimationMappings.GetAnimationForDW4Spell(id);
		int soundId = DQ3rAnimationMappings.SpellSounds.TryGetValue(animationId, out int snd) ? snd : 0;

		return new DQ3rSpell {
			Id = id,
			Name = string.IsNullOrEmpty(name) ? $"Spell_{id:D3}" : name,
			MPCost = ScaleMPCost(dw4Spell.MPCost),
			BasePower = ScalePower(dw4Spell.BasePower),
			Category = MapCategory(dw4Spell.Type),
			Target = MapTarget(dw4Spell.Target),
			Element = MapElement(dw4Spell.Element),
			SuccessRate = dw4Spell.SuccessRate,
			LearnFlags = 0, // DW4 doesn't have class-based learning
			LearnLevel = 0,
			UsableInField = CanUseInField(dw4Spell.Type),
			UsableInBattle = CanUseInBattle(dw4Spell.Type),
			AnimationId = animationId,
			SoundId = soundId,
			Description = GenerateDescription(dw4Spell),
			SourceDW4Id = id,
			Notes = animationId == 0
				? $"Converted from DW4 spell {id} - no animation mapping"
				: $"Converted from DW4 spell {id} - animation 0x{animationId:X2}"
		};
	}

	/// <summary>
	/// Convert all spells from DW4 data.
	/// </summary>
	public static List<DQ3rSpell> ConvertAll(List<Spell> dw4Spells, List<string>? names = null) {
		var result = new List<DQ3rSpell>();

		for (int i = 0; i < dw4Spells.Count; i++) {
			string name = (names != null && i < names.Count) ? names[i] : "";
			result.Add(Convert(dw4Spells[i], i, name));
		}

		return result;
	}

	private static int ScaleMPCost(byte cost) {
		return (int)Math.Round(cost * MPScaling);
	}

	private static int ScalePower(byte power) {
		return (int)Math.Round(power * PowerScaling);
	}

	private static DQ3rSpellTarget MapTarget(SpellTarget dw4Target) {
		return TargetMapping.TryGetValue(dw4Target, out var target)
			? target
			: DQ3rSpellTarget.SingleEnemy;
	}

	private static DQ3rSpellCategory MapCategory(SpellType dw4Type) {
		return CategoryMapping.TryGetValue(dw4Type, out var category)
			? category
			: DQ3rSpellCategory.Special;
	}

	private static DQ3rSpellElement MapElement(SpellElement dw4Element) {
		return ElementMapping.TryGetValue(dw4Element, out var element)
			? element
			: DQ3rSpellElement.None;
	}

	private static bool CanUseInField(SpellType type) {
		return type is SpellType.Heal or SpellType.Utility or SpellType.Transport;
	}

	private static bool CanUseInBattle(SpellType type) {
		return type is not SpellType.Utility and not SpellType.Transport;
	}

	private static string GenerateDescription(Spell spell) {
		return spell.Type switch {
			SpellType.Damage => $"Deals {ScalePower(spell.BasePower)} {spell.Element} damage",
			SpellType.Heal => $"Restores {ScalePower(spell.BasePower)} HP",
			SpellType.Buff => "Enhances ally stats",
			SpellType.Debuff => "Weakens enemy stats",
			SpellType.Status => "Inflicts status effect",
			SpellType.Utility => "Field utility spell",
			SpellType.Transport => "Transportation spell",
			SpellType.Special => "Special effect",
			_ => "Unknown spell"
		};
	}

	/// <summary>
	/// DW4 to DQ3r spell ID mapping table.
	/// Maps DW4 spell IDs to their DQ3r equivalents.
	/// </summary>
	public static readonly Dictionary<int, int> SpellIdMapping = new() {
		// Healing spells
		{ 1, 1 },   // Heal → Heal
		{ 9, 9 },   // Healmore → Midheal
		{ 17, 17 }, // Healall → Fullheal

		// Attack spells
		{ 2, 2 },   // Hurt → Frizz
		{ 10, 10 }, // Hurtmore → Frizzle
		{ 18, 18 }, // Blaze → Sizz

		// Status spells
		{ 3, 3 },   // Sleep → Snooze
		{ 5, 5 },   // Stopspell → Fizzle
		{ 11, 11 }, // Surround → Dazzle

		// Field spells
		{ 4, 4 },   // Radiant → Glow
		{ 6, 6 },   // Outside → Evac
		{ 7, 7 },   // Return → Zoom
		{ 8, 8 },   // Repel → Holy Protection

		// Default: pass through
	};

	/// <summary>
	/// Get the DQ3r spell ID for a DW4 spell.
	/// </summary>
	public static int GetDQ3rSpellId(int dw4SpellId) {
		return SpellIdMapping.TryGetValue(dw4SpellId, out int dq3rId) ? dq3rId : dw4SpellId;
	}
}
