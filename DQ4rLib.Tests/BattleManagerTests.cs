namespace DQ4rLib.Tests;

using DQ4rLib;
using DQ4rLib.Models;

public class BattleManagerTests {
	private BattleManager CreateBattleManager() {
		var chapterState = new ChapterState { CurrentChapterId = 4 }; // Chapter 5
		var chapterManager = new ChapterManager(chapterState);
		return new BattleManager(chapterManager);
	}

	#region Data Loading Tests

	[Fact]
	public void BattleManager_RegisterMonster() {
		var manager = CreateBattleManager();
		var monster = new MonsterData {
			Id = 1,
			Name = "Slime",
			MaxHp = 10,
			Attack = 5,
			Defense = 2,
			Agility = 3,
			Experience = 1,
			Gold = 2
		};

		manager.RegisterMonster(monster);

		var retrieved = manager.GetMonster(1);
		Assert.NotNull(retrieved);
		Assert.Equal("Slime", retrieved.Name);
		Assert.Equal((ushort)10, retrieved.MaxHp);
	}

	[Fact]
	public void BattleManager_RegisterSpell() {
		var manager = CreateBattleManager();
		var spell = new SpellData {
			Id = 1,
			Name = "Heal",
			MpCost = 4,
			Target = TargetType.SingleAlly,
			Effect = SpellEffect.Heal,
			Power = 30,
			BattleUse = true,
			FieldUse = true
		};

		manager.RegisterSpell(spell);

		var retrieved = manager.GetSpell(1);
		Assert.NotNull(retrieved);
		Assert.Equal("Heal", retrieved.Name);
		Assert.Equal((byte)4, retrieved.MpCost);
	}

	[Fact]
	public void BattleManager_RegisterEncounter() {
		var manager = CreateBattleManager();
		var encounter = new BattleEncounter {
			Id = 1,
			CanFlee = true,
			MonsterGroups = [
				new MonsterGroup { MonsterId = 1, MinCount = 1, MaxCount = 3 }
			]
		};

		manager.RegisterEncounter(encounter);

		var retrieved = manager.GetEncounter(1);
		Assert.NotNull(retrieved);
		Assert.True(retrieved.CanFlee);
		Assert.Single(retrieved.MonsterGroups);
	}

	#endregion

	#region Monster Data Tests

	[Fact]
	public void MonsterData_Actions() {
		var monster = new MonsterData {
			Id = 10,
			Name = "Dragon",
			Actions = [
				new MonsterAction { Type = MonsterActionType.Attack, Weight = 100 },
				new MonsterAction { Type = MonsterActionType.Spell, SkillId = 5, Weight = 50, HpThreshold = 50 }
			]
		};

		Assert.Equal(2, monster.Actions.Count);
		Assert.Equal(MonsterActionType.Attack, monster.Actions[0].Type);
		Assert.Equal((ushort)5, monster.Actions[1].SkillId);
	}

	#endregion

	#region Battle Combatant Tests

	[Fact]
	public void BattleCombatant_IsAlive() {
		var combatant = new BattleCombatant {
			CurrentHp = 50,
			MaxHp = 100,
			Status = BattleStatus.None
		};

		Assert.True(combatant.IsAlive);

		combatant.CurrentHp = 0;
		Assert.False(combatant.IsAlive);

		combatant.CurrentHp = 50;
		combatant.Status = BattleStatus.Dead;
		Assert.False(combatant.IsAlive);
	}

	[Fact]
	public void BattleCombatant_CanAct() {
		var combatant = new BattleCombatant {
			CurrentHp = 50,
			MaxHp = 100,
			Status = BattleStatus.None
		};

		Assert.True(combatant.CanAct);

		combatant.Status = BattleStatus.Asleep;
		Assert.False(combatant.CanAct);

		combatant.Status = BattleStatus.Paralyzed;
		Assert.False(combatant.CanAct);

		combatant.Status = BattleStatus.Poisoned;
		Assert.True(combatant.CanAct); // Poison doesn't prevent action
	}

	[Fact]
	public void BattleCombatant_StatusDurations() {
		var combatant = new BattleCombatant {
			CurrentHp = 50,
			MaxHp = 100,
			Status = BattleStatus.DefenseUp,
			StatusDurations = new Dictionary<BattleStatus, int> {
				[BattleStatus.DefenseUp] = 3
			}
		};

		Assert.True(combatant.Status.HasFlag(BattleStatus.DefenseUp));
		Assert.Equal(3, combatant.StatusDurations[BattleStatus.DefenseUp]);
	}

	[Fact]
	public void BattleCombatant_Multipliers() {
		var combatant = new BattleCombatant {
			Attack = 100,
			Defense = 50,
			AttackMultiplier = 1.5f,
			DefenseMultiplier = 2.0f
		};

		Assert.Equal(1.5f, combatant.AttackMultiplier);
		Assert.Equal(2.0f, combatant.DefenseMultiplier);
	}

	#endregion

	#region Battle Action Tests

	[Fact]
	public void BattleAction_Attack() {
		var action = new BattleAction {
			Type = BattleActionType.Attack,
			Priority = 0
		};

		Assert.Equal(BattleActionType.Attack, action.Type);
		Assert.Equal(0, action.Priority);
	}

	[Fact]
	public void BattleAction_Spell() {
		var action = new BattleAction {
			Type = BattleActionType.Spell,
			ActionId = 5,
			Priority = 5
		};

		Assert.Equal(BattleActionType.Spell, action.Type);
		Assert.Equal((ushort)5, action.ActionId);
	}

	[Fact]
	public void BattleActionResult_Damage() {
		var target = new BattleCombatant { CurrentHp = 100, MaxHp = 100 };
		var result = new BattleActionResult {
			Success = true,
			TargetResults = [
				new TargetResult {
					Target = target,
					Damage = 25,
					IsCritical = true,
					IsMiss = false,
					Defeated = false
				}
			]
		};

		Assert.True(result.Success);
		Assert.Single(result.TargetResults);
		Assert.True(result.TargetResults[0].IsCritical);
		Assert.Equal(25, result.TargetResults[0].Damage);
	}

	#endregion

	#region Spell Data Tests

	[Fact]
	public void SpellData_DamageSpell() {
		var spell = new SpellData {
			Id = 10,
			Name = "Frizz",
			MpCost = 2,
			Target = TargetType.SingleEnemy,
			Effect = SpellEffect.Damage,
			Power = 15,
			Element = ElementType.Fire,
			BattleUse = true,
			FieldUse = false
		};

		Assert.Equal(SpellEffect.Damage, spell.Effect);
		Assert.Equal(ElementType.Fire, spell.Element);
		Assert.True(spell.BattleUse);
		Assert.False(spell.FieldUse);
	}

	[Fact]
	public void SpellData_HealSpell() {
		var spell = new SpellData {
			Id = 1,
			Name = "Heal",
			MpCost = 4,
			Target = TargetType.SingleAlly,
			Effect = SpellEffect.Heal,
			Power = 30,
			BattleUse = true,
			FieldUse = true
		};

		Assert.Equal(SpellEffect.Heal, spell.Effect);
		Assert.Equal(TargetType.SingleAlly, spell.Target);
	}

	[Fact]
	public void SpellData_BuffSpell() {
		var spell = new SpellData {
			Id = 20,
			Name = "Kabuff",
			MpCost = 3,
			Target = TargetType.AllAllies,
			Effect = SpellEffect.Buff,
			StatusEffect = BattleStatus.DefenseUp,
			BattleUse = true
		};

		Assert.Equal(SpellEffect.Buff, spell.Effect);
		Assert.Equal(BattleStatus.DefenseUp, spell.StatusEffect);
	}

	#endregion

	#region Battle State Tests

	[Fact]
	public void BattleState_Initialization() {
		var state = new BattleState {
			EncounterId = 5,
			Phase = BattlePhase.Starting,
			IsBoss = true,
			CanFlee = false
		};

		Assert.Equal((ushort)5, state.EncounterId);
		Assert.Equal(BattlePhase.Starting, state.Phase);
		Assert.True(state.IsBoss);
		Assert.False(state.CanFlee);
	}

	[Fact]
	public void BattleState_TurnOrder() {
		var combatant1 = new BattleCombatant { BattleId = 1, Agility = 50 };
		var combatant2 = new BattleCombatant { BattleId = 2, Agility = 30 };

		var state = new BattleState {
			TurnOrder = new Queue<BattleCombatant>([combatant1, combatant2])
		};

		Assert.Equal(2, state.TurnOrder.Count);
		var first = state.TurnOrder.Dequeue();
		Assert.Equal(1, first.BattleId);
	}

	#endregion

	#region Battle Result Tests

	[Fact]
	public void BattleResult_Victory() {
		var result = new BattleResult {
			Outcome = BattleOutcome.Victory,
			ExperienceGained = 100,
			GoldGained = 50,
			TurnCount = 5,
			ItemsDropped = [0x10, 0x15],
			LeveledUp = [0, 2]
		};

		Assert.Equal(BattleOutcome.Victory, result.Outcome);
		Assert.Equal(100u, result.ExperienceGained);
		Assert.Equal(50u, result.GoldGained);
		Assert.Equal(5, result.TurnCount);
		Assert.Equal(2, result.ItemsDropped.Count);
		Assert.Equal(2, result.LeveledUp.Count);
	}

	[Fact]
	public void BattleResult_Defeat() {
		var result = new BattleResult {
			Outcome = BattleOutcome.Defeat,
			TurnCount = 3
		};

		Assert.Equal(BattleOutcome.Defeat, result.Outcome);
		Assert.Equal(0u, result.ExperienceGained);
		Assert.Equal(0u, result.GoldGained);
	}

	[Fact]
	public void BattleResult_Fled() {
		var result = new BattleResult {
			Outcome = BattleOutcome.Fled,
			TurnCount = 1
		};

		Assert.Equal(BattleOutcome.Fled, result.Outcome);
	}

	#endregion

	#region Encounter Tests

	[Fact]
	public void BattleEncounter_MonsterGroups() {
		var encounter = new BattleEncounter {
			Id = 1,
			MonsterGroups = [
				new MonsterGroup { MonsterId = 1, MinCount = 2, MaxCount = 4 },
				new MonsterGroup { MonsterId = 5, MinCount = 1, MaxCount = 1 }
			],
			BackgroundId = 3,
			MusicId = 10,
			CanFlee = true
		};

		Assert.Equal(2, encounter.MonsterGroups.Count);
		Assert.Equal((byte)3, encounter.BackgroundId);
		Assert.Equal((byte)10, encounter.MusicId);
	}

	[Fact]
	public void BattleEncounter_BossBattle() {
		var encounter = new BattleEncounter {
			Id = 100,
			IsBoss = true,
			CanFlee = false,
			VictoryEventId = 500,
			DefeatEventId = 501
		};

		Assert.True(encounter.IsBoss);
		Assert.False(encounter.CanFlee);
		Assert.Equal((ushort)500, encounter.VictoryEventId);
		Assert.Equal((ushort)501, encounter.DefeatEventId);
	}

	#endregion

	#region Status Effect Tests

	[Fact]
	public void BattleStatus_CombineFlags() {
		var status = BattleStatus.Poisoned | BattleStatus.DefenseUp;

		Assert.True(status.HasFlag(BattleStatus.Poisoned));
		Assert.True(status.HasFlag(BattleStatus.DefenseUp));
		Assert.False(status.HasFlag(BattleStatus.Dead));
	}

	[Fact]
	public void BattleStatus_RemoveFlag() {
		var status = BattleStatus.Poisoned | BattleStatus.Asleep;
		status &= ~BattleStatus.Asleep;

		Assert.True(status.HasFlag(BattleStatus.Poisoned));
		Assert.False(status.HasFlag(BattleStatus.Asleep));
	}

	#endregion

	#region Available Actions Tests

	[Fact]
	public void BattleManager_GetAvailableActions_BasicActions() {
		var manager = CreateBattleManager();
		var combatant = new BattleCombatant {
			CurrentHp = 100,
			CurrentMp = 0,
			IsPartyMember = true
		};

		var actions = manager.GetAvailableActions(combatant);

		Assert.Contains(BattleActionType.Attack, actions);
		Assert.Contains(BattleActionType.Defend, actions);
		Assert.Contains(BattleActionType.Item, actions);
	}

	[Fact]
	public void BattleManager_GetUsableSpells_WithMp() {
		var manager = CreateBattleManager();
		manager.RegisterSpell(new SpellData {
			Id = 1,
			Name = "Heal",
			MpCost = 4,
			BattleUse = true
		});
		manager.RegisterSpell(new SpellData {
			Id = 2,
			Name = "Fullheal",
			MpCost = 20,
			BattleUse = true
		});

		var character = new CharacterSaveData { CurrentMp = 10 };
		character.LearnSpell(1);
		character.LearnSpell(2);

		var combatant = new BattleCombatant {
			CurrentMp = 10,
			Character = character
		};

		var usable = manager.GetUsableSpells(combatant);

		Assert.Single(usable); // Only Heal is affordable
		Assert.Equal("Heal", usable[0].Name);
	}

	#endregion
}
