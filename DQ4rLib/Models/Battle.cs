namespace DQ4rLib.Models;

/// <summary>
/// Monster data loaded from game data files.
/// </summary>
public class MonsterData {
	/// <summary>Monster ID.</summary>
	public ushort Id { get; set; }

	/// <summary>Monster name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Base HP.</summary>
	public ushort MaxHp { get; set; }

	/// <summary>Base MP.</summary>
	public ushort MaxMp { get; set; }

	/// <summary>Attack power.</summary>
	public ushort Attack { get; set; }

	/// <summary>Defense power.</summary>
	public ushort Defense { get; set; }

	/// <summary>Agility (determines turn order).</summary>
	public byte Agility { get; set; }

	/// <summary>Experience reward.</summary>
	public uint Experience { get; set; }

	/// <summary>Gold reward.</summary>
	public uint Gold { get; set; }

	/// <summary>Item drop ID (0 = none).</summary>
	public byte DropItemId { get; set; }

	/// <summary>Item drop rate (0-255, where 255 = 100%).</summary>
	public byte DropRate { get; set; }

	/// <summary>Monster AI pattern ID.</summary>
	public byte AiPatternId { get; set; }

	/// <summary>Resistances bitmask.</summary>
	public uint Resistances { get; set; }

	/// <summary>Sprite ID for battle graphics.</summary>
	public ushort SpriteId { get; set; }

	/// <summary>Is boss monster (no flee, special defeat handling).</summary>
	public bool IsBoss { get; set; }

	/// <summary>Actions this monster can take.</summary>
	public List<MonsterAction> Actions { get; set; } = [];
}

/// <summary>
/// A single action a monster can take.
/// </summary>
public class MonsterAction {
	/// <summary>Action type.</summary>
	public MonsterActionType Type { get; set; }

	/// <summary>Skill/spell ID if applicable.</summary>
	public ushort SkillId { get; set; }

	/// <summary>Weight for random selection (0-255).</summary>
	public byte Weight { get; set; } = 100;

	/// <summary>HP threshold for action (e.g., use heal when below 50%).</summary>
	public byte HpThreshold { get; set; }

	/// <summary>Target selection for this action.</summary>
	public TargetType Target { get; set; }
}

/// <summary>
/// Monster action types.
/// </summary>
public enum MonsterActionType : byte {
	Attack = 0,
	Spell = 1,
	Skill = 2,
	Defend = 3,
	Flee = 4,
	CallForHelp = 5,
	DoNothing = 6
}

/// <summary>
/// Battle encounter definition.
/// </summary>
public class BattleEncounter {
	/// <summary>Encounter ID.</summary>
	public ushort Id { get; set; }

	/// <summary>Monster group (list of monster IDs and counts).</summary>
	public List<MonsterGroup> MonsterGroups { get; set; } = [];

	/// <summary>Background ID.</summary>
	public byte BackgroundId { get; set; }

	/// <summary>Music ID.</summary>
	public byte MusicId { get; set; }

	/// <summary>Can flee from this battle?</summary>
	public bool CanFlee { get; set; } = true;

	/// <summary>Is boss battle?</summary>
	public bool IsBoss { get; set; }

	/// <summary>Special event on victory.</summary>
	public ushort VictoryEventId { get; set; }

	/// <summary>Special event on defeat.</summary>
	public ushort DefeatEventId { get; set; }
}

/// <summary>
/// A group of monsters in a battle.
/// </summary>
public class MonsterGroup {
	/// <summary>Monster ID.</summary>
	public ushort MonsterId { get; set; }

	/// <summary>Minimum count.</summary>
	public byte MinCount { get; set; } = 1;

	/// <summary>Maximum count.</summary>
	public byte MaxCount { get; set; } = 1;
}

/// <summary>
/// A combatant in battle (party member or monster).
/// </summary>
public class BattleCombatant {
	/// <summary>Unique ID within battle.</summary>
	public int BattleId { get; set; }

	/// <summary>Is party member (vs monster).</summary>
	public bool IsPartyMember { get; set; }

	/// <summary>Party member index (0-7) or monster slot.</summary>
	public int SlotIndex { get; set; }

	/// <summary>Reference to CharacterSaveData if party member.</summary>
	public CharacterSaveData? Character { get; set; }

	/// <summary>Reference to MonsterData if monster.</summary>
	public MonsterData? Monster { get; set; }

	/// <summary>Display name.</summary>
	public string Name => IsPartyMember ? (Character?.Name ?? "???") : (Monster?.Name ?? "Monster");

	/// <summary>Current HP in battle.</summary>
	public int CurrentHp { get; set; }

	/// <summary>Max HP.</summary>
	public int MaxHp { get; set; }

	/// <summary>Current MP in battle.</summary>
	public int CurrentMp { get; set; }

	/// <summary>Max MP.</summary>
	public int MaxMp { get; set; }

	/// <summary>Attack power (with buffs/debuffs).</summary>
	public int Attack { get; set; }

	/// <summary>Defense power (with buffs/debuffs).</summary>
	public int Defense { get; set; }

	/// <summary>Agility for turn order.</summary>
	public int Agility { get; set; }

	/// <summary>Status effects during battle.</summary>
	public BattleStatus Status { get; set; }

	/// <summary>Tactic for AI-controlled party members.</summary>
	public BattleTactic Tactic { get; set; }

	/// <summary>Is alive?</summary>
	public bool IsAlive => CurrentHp > 0 && !Status.HasFlag(BattleStatus.Dead);

	/// <summary>Can take action?</summary>
	public bool CanAct => IsAlive && !Status.HasFlag(BattleStatus.Asleep)
		&& !Status.HasFlag(BattleStatus.Paralyzed) && !Status.HasFlag(BattleStatus.Stunned);

	/// <summary>Queued action for this turn.</summary>
	public BattleAction? QueuedAction { get; set; }

	/// <summary>Attack multiplier from buffs.</summary>
	public float AttackMultiplier { get; set; } = 1.0f;

	/// <summary>Defense multiplier from buffs.</summary>
	public float DefenseMultiplier { get; set; } = 1.0f;

	/// <summary>Turns remaining for status effects.</summary>
	public Dictionary<BattleStatus, int> StatusDurations { get; set; } = [];
}

/// <summary>
/// Battle status effects (can be combined).
/// </summary>
[Flags]
public enum BattleStatus : ushort {
	None = 0,
	Dead = 1 << 0,
	Poisoned = 1 << 1,
	Asleep = 1 << 2,
	Paralyzed = 1 << 3,
	Confused = 1 << 4,
	Silenced = 1 << 5,
	Blinded = 1 << 6,
	Stunned = 1 << 7,
	DefenseUp = 1 << 8,
	DefenseDown = 1 << 9,
	AttackUp = 1 << 10,
	AttackDown = 1 << 11,
	Reflecting = 1 << 12,
	Regenerating = 1 << 13,
	Berserk = 1 << 14
}

/// <summary>
/// Target selection type.
/// </summary>
public enum TargetType : byte {
	None = 0,
	SingleEnemy = 1,
	AllEnemies = 2,
	SingleAlly = 3,
	AllAllies = 4,
	Self = 5,
	RandomEnemy = 6,
	RandomAlly = 7,
	DeadAlly = 8
}

/// <summary>
/// A battle action to be executed.
/// </summary>
public class BattleAction {
	/// <summary>Type of action.</summary>
	public BattleActionType Type { get; set; }

	/// <summary>Actor performing the action.</summary>
	public BattleCombatant? Actor { get; set; }

	/// <summary>Target(s) of the action.</summary>
	public List<BattleCombatant> Targets { get; set; } = [];

	/// <summary>Skill/spell/item ID.</summary>
	public ushort ActionId { get; set; }

	/// <summary>Priority modifier for turn order.</summary>
	public int Priority { get; set; }
}

/// <summary>
/// Battle action types.
/// </summary>
public enum BattleActionType : byte {
	Attack = 0,
	Defend = 1,
	Spell = 2,
	Skill = 3,
	Item = 4,
	Flee = 5,
	Parry = 6,
	Swap = 7    // Swap party member
}

/// <summary>
/// Result of executing a battle action.
/// </summary>
public class BattleActionResult {
	/// <summary>The action that was executed.</summary>
	public BattleAction? Action { get; set; }

	/// <summary>Whether action succeeded.</summary>
	public bool Success { get; set; }

	/// <summary>Results for each target.</summary>
	public List<TargetResult> TargetResults { get; set; } = [];

	/// <summary>Message to display.</summary>
	public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Result of an action against a single target.
/// </summary>
public class TargetResult {
	/// <summary>Target combatant.</summary>
	public BattleCombatant? Target { get; set; }

	/// <summary>Damage dealt (negative for healing).</summary>
	public int Damage { get; set; }

	/// <summary>Was critical hit?</summary>
	public bool IsCritical { get; set; }

	/// <summary>Was miss?</summary>
	public bool IsMiss { get; set; }

	/// <summary>Status effect applied.</summary>
	public BattleStatus StatusApplied { get; set; }

	/// <summary>Status effect removed.</summary>
	public BattleStatus StatusRemoved { get; set; }

	/// <summary>Target was defeated.</summary>
	public bool Defeated { get; set; }
}

/// <summary>
/// Battle state phase.
/// </summary>
public enum BattlePhase {
	/// <summary>Battle starting, show encounter.</summary>
	Starting,

	/// <summary>Waiting for player input.</summary>
	PlayerInput,

	/// <summary>Calculating turn order.</summary>
	TurnOrder,

	/// <summary>Executing actions.</summary>
	Executing,

	/// <summary>Processing end of round effects.</summary>
	EndOfRound,

	/// <summary>Victory processing.</summary>
	Victory,

	/// <summary>Defeat processing.</summary>
	Defeat,

	/// <summary>Fled successfully.</summary>
	Fled,

	/// <summary>Battle ended.</summary>
	Ended
}

/// <summary>
/// Result of a completed battle.
/// </summary>
public class BattleResult {
	/// <summary>Outcome of the battle.</summary>
	public BattleOutcome Outcome { get; set; }

	/// <summary>Total experience gained.</summary>
	public uint ExperienceGained { get; set; }

	/// <summary>Total gold gained.</summary>
	public uint GoldGained { get; set; }

	/// <summary>Items dropped.</summary>
	public List<byte> ItemsDropped { get; set; } = [];

	/// <summary>Number of turns taken.</summary>
	public int TurnCount { get; set; }

	/// <summary>Party members who leveled up.</summary>
	public List<int> LeveledUp { get; set; } = [];
}

/// <summary>
/// Battle outcome.
/// </summary>
public enum BattleOutcome {
	Victory,
	Defeat,
	Fled,
	Cancelled  // For special cases
}

/// <summary>
/// Spell data.
/// </summary>
public class SpellData {
	/// <summary>Spell ID.</summary>
	public ushort Id { get; set; }

	/// <summary>Spell name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>MP cost.</summary>
	public byte MpCost { get; set; }

	/// <summary>Target type.</summary>
	public TargetType Target { get; set; }

	/// <summary>Effect type.</summary>
	public SpellEffect Effect { get; set; }

	/// <summary>Base power/healing amount.</summary>
	public ushort Power { get; set; }

	/// <summary>Status effect to apply (if any).</summary>
	public BattleStatus StatusEffect { get; set; }

	/// <summary>Element type.</summary>
	public ElementType Element { get; set; }

	/// <summary>Can use outside battle.</summary>
	public bool FieldUse { get; set; }

	/// <summary>Can use in battle.</summary>
	public bool BattleUse { get; set; } = true;
}

/// <summary>
/// Spell effect types.
/// </summary>
public enum SpellEffect : byte {
	Damage = 0,
	Heal = 1,
	HealStatus = 2,
	Buff = 3,
	Debuff = 4,
	Revive = 5,
	Escape = 6,
	Transform = 7
}

/// <summary>
/// Element types.
/// </summary>
public enum ElementType : byte {
	None = 0,
	Fire = 1,
	Ice = 2,
	Lightning = 3,
	Wind = 4,
	Earth = 5,
	Holy = 6,
	Dark = 7
}
