namespace DW4Lib.DQ3r;

/// <summary>
/// DQ3 Remake spell format (SNES 16-bit).
/// </summary>
public class DQ3rSpell {
	/// <summary>ID in the DQ3r spell table.</summary>
	public int Id { get; set; }

	/// <summary>Spell name.</summary>
	public string Name { get; set; } = "";

	/// <summary>MP cost to cast.</summary>
	public int MPCost { get; set; }

	/// <summary>Base power/effect value.</summary>
	public int BasePower { get; set; }

	/// <summary>Spell category.</summary>
	public DQ3rSpellCategory Category { get; set; }

	/// <summary>Target type.</summary>
	public DQ3rSpellTarget Target { get; set; }

	/// <summary>Element type.</summary>
	public DQ3rSpellElement Element { get; set; }

	/// <summary>Success rate (0-100%).</summary>
	public int SuccessRate { get; set; }

	/// <summary>Who can learn this spell (character class bitmask).</summary>
	public int LearnFlags { get; set; }

	/// <summary>Level at which spell is learned.</summary>
	public int LearnLevel { get; set; }

	/// <summary>Can be used outside battle?</summary>
	public bool UsableInField { get; set; }

	/// <summary>Can be used in battle?</summary>
	public bool UsableInBattle { get; set; }

	/// <summary>Animation ID.</summary>
	public int AnimationId { get; set; }

	/// <summary>Sound effect ID.</summary>
	public int SoundId { get; set; }

	/// <summary>Description text.</summary>
	public string Description { get; set; } = "";

	/// <summary>Source DW4 spell ID.</summary>
	public int SourceDW4Id { get; set; }

	/// <summary>Conversion notes.</summary>
	public string Notes { get; set; } = "";
}

/// <summary>
/// DQ3r spell category.
/// </summary>
public enum DQ3rSpellCategory {
	Attack = 0,
	Healing = 1,
	Support = 2,
	Debuff = 3,
	Field = 4,
	Special = 5
}

/// <summary>
/// DQ3r spell target type.
/// </summary>
public enum DQ3rSpellTarget {
	Self = 0,
	SingleAlly = 1,
	AllAllies = 2,
	SingleEnemy = 3,
	AllEnemies = 4,
	EnemyGroup = 5
}

/// <summary>
/// DQ3r spell element.
/// </summary>
public enum DQ3rSpellElement {
	None = 0,
	Fire = 1,
	Ice = 2,
	Lightning = 3,
	Wind = 4,
	Holy = 5,
	Dark = 6,
	Earth = 7
}
