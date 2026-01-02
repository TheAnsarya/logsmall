namespace DQ4rLib;

using DQ4rLib.Models;

/// <summary>
/// Manages battle state and execution.
/// Handles turn order, action resolution, and battle flow.
/// </summary>
public class BattleManager {
	private readonly ChapterManager _chapterManager;
	private readonly Dictionary<ushort, MonsterData> _monsterData = [];
	private readonly Dictionary<ushort, SpellData> _spellData = [];
	private readonly Dictionary<ushort, BattleEncounter> _encounters = [];

	private BattleState? _currentBattle;
	private int _battleIdCounter;

	// Events for UI binding
	public event EventHandler<BattleStartedEventArgs>? BattleStarted;
	public event EventHandler<BattleEndedEventArgs>? BattleEnded;
	public event EventHandler<TurnStartedEventArgs>? TurnStarted;
	public event EventHandler<ActionExecutedEventArgs>? ActionExecuted;
	public event EventHandler<CombatantDefeatedEventArgs>? CombatantDefeated;
	public event EventHandler<LevelUpEventArgs>? LevelUp;
	public event EventHandler<InputRequestedEventArgs>? InputRequested;

	public BattleManager(ChapterManager chapterManager) {
		_chapterManager = chapterManager;
	}

	/// <summary>Current battle state.</summary>
	public BattleState? CurrentBattle => _currentBattle;

	/// <summary>Is battle active?</summary>
	public bool IsInBattle => _currentBattle != null && _currentBattle.Phase != BattlePhase.Ended;

	#region Data Loading

	/// <summary>Register monster data.</summary>
	public void RegisterMonster(MonsterData monster) {
		_monsterData[monster.Id] = monster;
	}

	/// <summary>Register spell data.</summary>
	public void RegisterSpell(SpellData spell) {
		_spellData[spell.Id] = spell;
	}

	/// <summary>Register encounter data.</summary>
	public void RegisterEncounter(BattleEncounter encounter) {
		_encounters[encounter.Id] = encounter;
	}

	/// <summary>Get monster data by ID.</summary>
	public MonsterData? GetMonster(ushort id) => _monsterData.GetValueOrDefault(id);

	/// <summary>Get spell data by ID.</summary>
	public SpellData? GetSpell(ushort id) => _spellData.GetValueOrDefault(id);

	/// <summary>Get encounter data by ID.</summary>
	public BattleEncounter? GetEncounter(ushort id) => _encounters.GetValueOrDefault(id);

	/// <summary>Load monster data from binary format.</summary>
	public void LoadMonsterData(byte[] data) {
		using var ms = new MemoryStream(data);
		using var br = new BinaryReader(ms);

		int count = br.ReadUInt16();
		for (int i = 0; i < count; i++) {
			var monster = new MonsterData {
				Id = br.ReadUInt16(),
				MaxHp = br.ReadUInt16(),
				MaxMp = br.ReadUInt16(),
				Attack = br.ReadUInt16(),
				Defense = br.ReadUInt16(),
				Agility = br.ReadByte(),
				Experience = br.ReadUInt32(),
				Gold = br.ReadUInt32(),
				DropItemId = br.ReadByte(),
				DropRate = br.ReadByte(),
				AiPatternId = br.ReadByte(),
				Resistances = br.ReadUInt32(),
				SpriteId = br.ReadUInt16(),
				IsBoss = br.ReadByte() != 0
			};

			// Read name (8 bytes, null-terminated)
			var nameBytes = br.ReadBytes(8);
			monster.Name = System.Text.Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');

			// Read actions
			int actionCount = br.ReadByte();
			for (int j = 0; j < actionCount; j++) {
				monster.Actions.Add(new MonsterAction {
					Type = (MonsterActionType)br.ReadByte(),
					SkillId = br.ReadUInt16(),
					Weight = br.ReadByte(),
					HpThreshold = br.ReadByte(),
					Target = (TargetType)br.ReadByte()
				});
			}

			_monsterData[monster.Id] = monster;
		}
	}

	/// <summary>Load spell data from binary format.</summary>
	public void LoadSpellData(byte[] data) {
		using var ms = new MemoryStream(data);
		using var br = new BinaryReader(ms);

		int count = br.ReadUInt16();
		for (int i = 0; i < count; i++) {
			var spell = new SpellData {
				Id = br.ReadUInt16(),
				MpCost = br.ReadByte(),
				Target = (TargetType)br.ReadByte(),
				Effect = (SpellEffect)br.ReadByte(),
				Power = br.ReadUInt16(),
				StatusEffect = (BattleStatus)br.ReadUInt16(),
				Element = (ElementType)br.ReadByte(),
				FieldUse = br.ReadByte() != 0,
				BattleUse = br.ReadByte() != 0
			};

			// Read name (12 bytes, null-terminated)
			var nameBytes = br.ReadBytes(12);
			spell.Name = System.Text.Encoding.ASCII.GetString(nameBytes).TrimEnd('\0');

			_spellData[spell.Id] = spell;
		}
	}

	#endregion

	#region Battle Flow

	/// <summary>Start a battle from an encounter ID.</summary>
	public bool StartBattle(ushort encounterId) {
		if (!_encounters.TryGetValue(encounterId, out var encounter))
			return false;

		return StartBattle(encounter);
	}

	/// <summary>Start a battle from an encounter definition.</summary>
	public bool StartBattle(BattleEncounter encounter) {
		if (IsInBattle) return false;

		_currentBattle = new BattleState {
			EncounterId = encounter.Id,
			IsBoss = encounter.IsBoss,
			CanFlee = encounter.CanFlee,
			Phase = BattlePhase.Starting
		};

		_battleIdCounter = 0;

		// Add party members
		AddPartyToBattle();

		// Add monsters
		AddMonstersToBattle(encounter);

		BattleStarted?.Invoke(this, new BattleStartedEventArgs {
			Encounter = encounter,
			PartyMembers = _currentBattle.Party.ToList(),
			Monsters = _currentBattle.Monsters.ToList()
		});

		// Transition to player input
		_currentBattle.Phase = BattlePhase.PlayerInput;
		RequestPlayerInput();

		return true;
	}

	/// <summary>Start a random encounter for the current map.</summary>
	public bool StartRandomEncounter(ushort mapId, byte encounterRate) {
		// Find encounters for this map
		var mapEncounters = _encounters.Values
			.Where(e => !e.IsBoss)
			.ToList();

		if (mapEncounters.Count == 0) return false;

		// Random selection
		var encounter = mapEncounters[Random.Shared.Next(mapEncounters.Count)];
		return StartBattle(encounter);
	}

	private void AddPartyToBattle() {
		if (_currentBattle == null) return;

		// Get active party from chapter manager
		var activeParty = _chapterManager.State?.GetActiveParty() ?? [];

		foreach (var charIndex in activeParty) {
			var charData = GetCharacterData(charIndex);
			if (charData == null || !charData.IsAlive) continue;

			var combatant = new BattleCombatant {
				BattleId = _battleIdCounter++,
				IsPartyMember = true,
				SlotIndex = charIndex,
				Character = charData,
				CurrentHp = charData.CurrentHp,
				MaxHp = charData.MaxHp,
				CurrentMp = charData.CurrentMp,
				MaxMp = charData.MaxMp,
				Attack = charData.Attack,
				Defense = charData.Defense,
				Agility = charData.Agility,
				Tactic = charData.Tactic
			};

			_currentBattle.Party.Add(combatant);
		}
	}

	private void AddMonstersToBattle(BattleEncounter encounter) {
		if (_currentBattle == null) return;

		foreach (var group in encounter.MonsterGroups) {
			if (!_monsterData.TryGetValue(group.MonsterId, out var monster)) continue;

			int count = Random.Shared.Next(group.MinCount, group.MaxCount + 1);
			for (int i = 0; i < count; i++) {
				var combatant = new BattleCombatant {
					BattleId = _battleIdCounter++,
					IsPartyMember = false,
					SlotIndex = _currentBattle.Monsters.Count,
					Monster = monster,
					CurrentHp = monster.MaxHp,
					MaxHp = monster.MaxHp,
					CurrentMp = monster.MaxMp,
					MaxMp = monster.MaxMp,
					Attack = monster.Attack,
					Defense = monster.Defense,
					Agility = monster.Agility
				};

				_currentBattle.Monsters.Add(combatant);
			}
		}
	}

	private CharacterSaveData? GetCharacterData(int index) {
		// This would integrate with SaveData system
		// For now, return null - caller should handle
		return null;
	}

	/// <summary>Request player input for party actions.</summary>
	private void RequestPlayerInput() {
		if (_currentBattle == null) return;

		// Find first party member needing input (following orders tactic)
		var needsInput = _currentBattle.Party
			.Where(p => p.IsAlive && p.CanAct && p.Tactic == BattleTactic.FollowOrders && p.QueuedAction == null)
			.FirstOrDefault();

		if (needsInput != null) {
			_currentBattle.CurrentInputIndex = needsInput.SlotIndex;
			InputRequested?.Invoke(this, new InputRequestedEventArgs {
				Combatant = needsInput,
				AvailableActions = GetAvailableActions(needsInput)
			});
		} else {
			// All player input done, queue AI actions
			QueueAiActions();
			StartTurnExecution();
		}
	}

	/// <summary>Set action for current combatant.</summary>
	public void SetAction(BattleAction action) {
		if (_currentBattle == null || _currentBattle.Phase != BattlePhase.PlayerInput) return;

		var combatant = _currentBattle.Party.FirstOrDefault(p => p.SlotIndex == _currentBattle.CurrentInputIndex);
		if (combatant != null) {
			combatant.QueuedAction = action;
			action.Actor = combatant;
		}

		// Request input for next party member
		RequestPlayerInput();
	}

	/// <summary>Queue AI actions for party members with tactics.</summary>
	private void QueueAiActions() {
		if (_currentBattle == null) return;

		// Party members with AI tactics
		foreach (var member in _currentBattle.Party.Where(p => p.IsAlive && p.CanAct && p.Tactic != BattleTactic.FollowOrders)) {
			member.QueuedAction = DecidePartyAction(member);
		}

		// Monster actions
		foreach (var monster in _currentBattle.Monsters.Where(m => m.IsAlive && m.CanAct)) {
			monster.QueuedAction = DecideMonsterAction(monster);
		}
	}

	/// <summary>Decide action for AI-controlled party member.</summary>
	private BattleAction DecidePartyAction(BattleCombatant combatant) {
		// Get targets
		var enemies = _currentBattle!.Monsters.Where(m => m.IsAlive).ToList();
		var allies = _currentBattle.Party.Where(p => p.IsAlive).ToList();

		switch (combatant.Tactic) {
			case BattleTactic.ShowNoMercy:
				// Balanced - attack weakest enemy or heal if needed
				if (allies.Any(a => a.CurrentHp < a.MaxHp / 4)) {
					return CreateHealAction(combatant, allies.OrderBy(a => a.CurrentHp).First());
				}
				return CreateAttackAction(combatant, enemies.OrderBy(e => e.CurrentHp).FirstOrDefault());

			case BattleTactic.GoAllOut:
				// Maximum offense
				return CreateAttackAction(combatant, enemies.OrderByDescending(e => e.Attack).FirstOrDefault());

			case BattleTactic.WatchMyMp:
				// Physical attacks only
				return CreateAttackAction(combatant, enemies.FirstOrDefault());

			case BattleTactic.DontUseMagic:
				// Defend or heal
				var needsHeal = allies.FirstOrDefault(a => a.CurrentHp < a.MaxHp / 2);
				if (needsHeal != null) {
					return CreateHealAction(combatant, needsHeal);
				}
				return CreateDefendAction(combatant);

			case BattleTactic.TryOut:
				// Random actions
				return CreateAttackAction(combatant, enemies[Random.Shared.Next(enemies.Count)]);

			default:
				return CreateAttackAction(combatant, enemies.FirstOrDefault());
		}
	}

	/// <summary>Decide action for monster.</summary>
	private BattleAction DecideMonsterAction(BattleCombatant combatant) {
		var monster = combatant.Monster;
		if (monster == null || monster.Actions.Count == 0) {
			// Default to attack
			return CreateAttackAction(combatant, _currentBattle!.Party.Where(p => p.IsAlive).FirstOrDefault());
		}

		// Calculate HP percentage
		int hpPercent = (combatant.CurrentHp * 100) / combatant.MaxHp;

		// Filter applicable actions
		var applicable = monster.Actions
			.Where(a => a.HpThreshold == 0 || hpPercent <= a.HpThreshold)
			.ToList();

		if (applicable.Count == 0) applicable = monster.Actions;

		// Weighted random selection
		int totalWeight = applicable.Sum(a => a.Weight);
		int roll = Random.Shared.Next(totalWeight);
		int cumulative = 0;

		MonsterAction? selected = applicable.Last();
		foreach (var action in applicable) {
			cumulative += action.Weight;
			if (roll < cumulative) {
				selected = action;
				break;
			}
		}

		return CreateMonsterAction(combatant, selected);
	}

	private BattleAction CreateAttackAction(BattleCombatant actor, BattleCombatant? target) {
		return new BattleAction {
			Type = BattleActionType.Attack,
			Actor = actor,
			Targets = target != null ? [target] : [],
			Priority = 0
		};
	}

	private BattleAction CreateDefendAction(BattleCombatant actor) {
		return new BattleAction {
			Type = BattleActionType.Defend,
			Actor = actor,
			Priority = 10 // Defend goes first
		};
	}

	private BattleAction CreateHealAction(BattleCombatant actor, BattleCombatant target) {
		// Find healing spell
		var healSpell = _spellData.Values.FirstOrDefault(s => s.Effect == SpellEffect.Heal && s.BattleUse);

		if (healSpell != null && actor.CurrentMp >= healSpell.MpCost) {
			return new BattleAction {
				Type = BattleActionType.Spell,
				Actor = actor,
				Targets = [target],
				ActionId = healSpell.Id,
				Priority = 5
			};
		}

		// Fallback to defend
		return CreateDefendAction(actor);
	}

	private BattleAction CreateMonsterAction(BattleCombatant actor, MonsterAction monsterAction) {
		var targets = SelectTargets(actor, monsterAction.Target);

		return monsterAction.Type switch {
			MonsterActionType.Attack => new BattleAction {
				Type = BattleActionType.Attack,
				Actor = actor,
				Targets = targets
			},
			MonsterActionType.Spell => new BattleAction {
				Type = BattleActionType.Spell,
				Actor = actor,
				Targets = targets,
				ActionId = monsterAction.SkillId
			},
			MonsterActionType.Skill => new BattleAction {
				Type = BattleActionType.Skill,
				Actor = actor,
				Targets = targets,
				ActionId = monsterAction.SkillId
			},
			MonsterActionType.Defend => CreateDefendAction(actor),
			MonsterActionType.Flee => new BattleAction {
				Type = BattleActionType.Flee,
				Actor = actor
			},
			_ => CreateAttackAction(actor, targets.FirstOrDefault())
		};
	}

	private List<BattleCombatant> SelectTargets(BattleCombatant actor, TargetType targetType) {
		var party = _currentBattle!.Party.Where(p => p.IsAlive).ToList();
		var monsters = _currentBattle.Monsters.Where(m => m.IsAlive).ToList();

		var allies = actor.IsPartyMember ? party : monsters;
		var enemies = actor.IsPartyMember ? monsters : party;

		return targetType switch {
			TargetType.SingleEnemy => enemies.Take(1).ToList(),
			TargetType.AllEnemies => enemies,
			TargetType.SingleAlly => allies.Take(1).ToList(),
			TargetType.AllAllies => allies,
			TargetType.Self => [actor],
			TargetType.RandomEnemy => [enemies[Random.Shared.Next(enemies.Count)]],
			TargetType.RandomAlly => [allies[Random.Shared.Next(allies.Count)]],
			TargetType.DeadAlly => (actor.IsPartyMember ? _currentBattle.Party : _currentBattle.Monsters)
				.Where(c => !c.IsAlive).Take(1).ToList(),
			_ => []
		};
	}

	/// <summary>Start executing queued actions.</summary>
	private void StartTurnExecution() {
		if (_currentBattle == null) return;

		_currentBattle.Phase = BattlePhase.TurnOrder;
		_currentBattle.TurnNumber++;

		// Calculate turn order based on agility + priority
		var allCombatants = _currentBattle.Party
			.Concat(_currentBattle.Monsters)
			.Where(c => c.IsAlive && c.QueuedAction != null)
			.OrderByDescending(c => c.Agility + (c.QueuedAction?.Priority ?? 0) + Random.Shared.Next(10))
			.ToList();

		_currentBattle.TurnOrder = new Queue<BattleCombatant>(allCombatants);
		_currentBattle.Phase = BattlePhase.Executing;

		TurnStarted?.Invoke(this, new TurnStartedEventArgs {
			TurnNumber = _currentBattle.TurnNumber,
			TurnOrder = allCombatants
		});

		// Execute first action
		ExecuteNextAction();
	}

	/// <summary>Execute the next action in turn order.</summary>
	public void ExecuteNextAction() {
		if (_currentBattle == null || _currentBattle.Phase != BattlePhase.Executing) return;

		if (_currentBattle.TurnOrder.Count == 0) {
			EndTurn();
			return;
		}

		var combatant = _currentBattle.TurnOrder.Dequeue();

		// Skip if no longer able to act
		if (!combatant.IsAlive || !combatant.CanAct || combatant.QueuedAction == null) {
			ExecuteNextAction();
			return;
		}

		var result = ExecuteAction(combatant.QueuedAction);

		ActionExecuted?.Invoke(this, new ActionExecutedEventArgs {
			Result = result
		});

		// Check for victory/defeat
		if (CheckBattleEnd()) return;

		// Continue to next action (could add delay here for animation)
		ExecuteNextAction();
	}

	/// <summary>Execute a battle action.</summary>
	private BattleActionResult ExecuteAction(BattleAction action) {
		var result = new BattleActionResult {
			Action = action,
			Success = true
		};

		if (action.Actor == null) return result;

		switch (action.Type) {
			case BattleActionType.Attack:
				foreach (var target in action.Targets.Where(t => t.IsAlive)) {
					var targetResult = CalculateAttackDamage(action.Actor, target);
					ApplyDamage(target, targetResult.Damage);
					result.TargetResults.Add(targetResult);
				}
				result.Message = $"{action.Actor.Name} attacks!";
				break;

			case BattleActionType.Defend:
				action.Actor.DefenseMultiplier = 2.0f;
				result.Message = $"{action.Actor.Name} defends.";
				break;

			case BattleActionType.Spell:
				if (_spellData.TryGetValue(action.ActionId, out var spell)) {
					if (action.Actor.CurrentMp >= spell.MpCost) {
						action.Actor.CurrentMp -= spell.MpCost;
						foreach (var target in action.Targets) {
							var targetResult = ExecuteSpell(action.Actor, target, spell);
							result.TargetResults.Add(targetResult);
						}
						result.Message = $"{action.Actor.Name} casts {spell.Name}!";
					} else {
						result.Success = false;
						result.Message = $"{action.Actor.Name} doesn't have enough MP!";
					}
				}
				break;

			case BattleActionType.Flee:
				if (TryFlee(action.Actor)) {
					result.Message = "Successfully fled!";
					_currentBattle!.Phase = BattlePhase.Fled;
				} else {
					result.Success = false;
					result.Message = "Couldn't escape!";
				}
				break;
		}

		return result;
	}

	private TargetResult CalculateAttackDamage(BattleCombatant attacker, BattleCombatant target) {
		var result = new TargetResult { Target = target };

		// Hit check (based on agility difference)
		int hitChance = 90 + (attacker.Agility - target.Agility) / 2;
		hitChance = Math.Clamp(hitChance, 10, 99);

		if (Random.Shared.Next(100) >= hitChance) {
			result.IsMiss = true;
			return result;
		}

		// Critical check (base 5%)
		result.IsCritical = Random.Shared.Next(100) < 5;

		// Damage calculation
		int attack = (int)(attacker.Attack * attacker.AttackMultiplier);
		int defense = (int)(target.Defense * target.DefenseMultiplier);

		int baseDamage = Math.Max(1, attack - defense / 2);
		baseDamage += Random.Shared.Next(-baseDamage / 8, baseDamage / 8 + 1);

		if (result.IsCritical) {
			baseDamage = (int)(baseDamage * 1.5);
		}

		result.Damage = Math.Max(1, baseDamage);
		result.Defeated = target.CurrentHp - result.Damage <= 0;

		return result;
	}

	private TargetResult ExecuteSpell(BattleCombatant caster, BattleCombatant target, SpellData spell) {
		var result = new TargetResult { Target = target };

		switch (spell.Effect) {
			case SpellEffect.Damage:
				// Check resistance
				int resistance = 0; // Would check target.Monster?.Resistances
				int damage = spell.Power - resistance;
				damage = Math.Max(1, damage);
				result.Damage = damage;
				result.Defeated = target.CurrentHp - damage <= 0;
				break;

			case SpellEffect.Heal:
				int healing = spell.Power;
				result.Damage = -healing; // Negative = healing
				break;

			case SpellEffect.HealStatus:
				result.StatusRemoved = spell.StatusEffect;
				target.Status &= ~spell.StatusEffect;
				break;

			case SpellEffect.Buff:
				result.StatusApplied = spell.StatusEffect;
				target.Status |= spell.StatusEffect;
				target.StatusDurations[spell.StatusEffect] = 3; // 3 turns
				break;

			case SpellEffect.Debuff:
				// Resistance check
				if (Random.Shared.Next(100) < 75) { // 75% base success
					result.StatusApplied = spell.StatusEffect;
					target.Status |= spell.StatusEffect;
					target.StatusDurations[spell.StatusEffect] = 3;
				}
				break;

			case SpellEffect.Revive:
				if (!target.IsAlive) {
					target.Status &= ~BattleStatus.Dead;
					target.CurrentHp = target.MaxHp / 2;
					result.StatusRemoved = BattleStatus.Dead;
				}
				break;
		}

		return result;
	}

	private void ApplyDamage(BattleCombatant target, int damage) {
		target.CurrentHp -= damage;
		if (target.CurrentHp <= 0) {
			target.CurrentHp = 0;
			target.Status |= BattleStatus.Dead;

			CombatantDefeated?.Invoke(this, new CombatantDefeatedEventArgs {
				Combatant = target
			});
		} else if (target.CurrentHp > target.MaxHp) {
			target.CurrentHp = target.MaxHp;
		}
	}

	private bool TryFlee(BattleCombatant actor) {
		if (_currentBattle == null || !_currentBattle.CanFlee) return false;

		// Flee calculation based on average agility
		int partyAgility = _currentBattle.Party.Where(p => p.IsAlive).Select(p => p.Agility).DefaultIfEmpty(0).Average().GetHashCode();
		int monsterAgility = _currentBattle.Monsters.Where(m => m.IsAlive).Select(m => m.Agility).DefaultIfEmpty(0).Average().GetHashCode();

		int fleeChance = 50 + (partyAgility - monsterAgility);
		fleeChance = Math.Clamp(fleeChance, 10, 90);

		return Random.Shared.Next(100) < fleeChance;
	}

	private void EndTurn() {
		if (_currentBattle == null) return;

		_currentBattle.Phase = BattlePhase.EndOfRound;

		// Process end of turn effects
		foreach (var combatant in _currentBattle.Party.Concat(_currentBattle.Monsters)) {
			// Reset defense multiplier
			combatant.DefenseMultiplier = 1.0f;

			// Clear queued actions
			combatant.QueuedAction = null;

			// Process status duration - decrement and find expired
			var expiredStatuses = new List<BattleStatus>();
			var keys = combatant.StatusDurations.Keys.ToList();
			foreach (var status in keys) {
				combatant.StatusDurations[status]--;
				if (combatant.StatusDurations[status] <= 0) {
					expiredStatuses.Add(status);
				}
			}

			foreach (var status in expiredStatuses) {
				combatant.Status &= ~status;
				combatant.StatusDurations.Remove(status);
			}

			// Poison damage
			if (combatant.Status.HasFlag(BattleStatus.Poisoned)) {
				ApplyDamage(combatant, combatant.MaxHp / 16);
			}

			// Regeneration
			if (combatant.Status.HasFlag(BattleStatus.Regenerating)) {
				ApplyDamage(combatant, -combatant.MaxHp / 10);
			}

			// Wake up chance
			if (combatant.Status.HasFlag(BattleStatus.Asleep) && Random.Shared.Next(100) < 33) {
				combatant.Status &= ~BattleStatus.Asleep;
			}
		}

		if (!CheckBattleEnd()) {
			// Start next turn
			_currentBattle.Phase = BattlePhase.PlayerInput;
			RequestPlayerInput();
		}
	}

	private bool CheckBattleEnd() {
		if (_currentBattle == null) return true;

		// Check for fled
		if (_currentBattle.Phase == BattlePhase.Fled) {
			EndBattle(BattleOutcome.Fled);
			return true;
		}

		// Check for defeat
		if (!_currentBattle.Party.Any(p => p.IsAlive)) {
			EndBattle(BattleOutcome.Defeat);
			return true;
		}

		// Check for victory
		if (!_currentBattle.Monsters.Any(m => m.IsAlive)) {
			EndBattle(BattleOutcome.Victory);
			return true;
		}

		return false;
	}

	private void EndBattle(BattleOutcome outcome) {
		if (_currentBattle == null) return;

		_currentBattle.Phase = outcome switch {
			BattleOutcome.Victory => BattlePhase.Victory,
			BattleOutcome.Defeat => BattlePhase.Defeat,
			BattleOutcome.Fled => BattlePhase.Fled,
			_ => BattlePhase.Ended
		};

		var result = new BattleResult {
			Outcome = outcome,
			TurnCount = _currentBattle.TurnNumber
		};

		if (outcome == BattleOutcome.Victory) {
			// Calculate rewards
			foreach (var monster in _currentBattle.Monsters) {
				if (monster.Monster != null) {
					result.ExperienceGained += monster.Monster.Experience;
					result.GoldGained += monster.Monster.Gold;

					// Drop check
					if (monster.Monster.DropItemId != 0 && Random.Shared.Next(256) < monster.Monster.DropRate) {
						result.ItemsDropped.Add(monster.Monster.DropItemId);
					}
				}
			}

			// Sync HP/MP back to character data
			SyncBattleStateToCharacters();
		}

		_currentBattle.Phase = BattlePhase.Ended;

		BattleEnded?.Invoke(this, new BattleEndedEventArgs {
			Result = result
		});

		_currentBattle = null;
	}

	private void SyncBattleStateToCharacters() {
		if (_currentBattle == null) return;

		foreach (var combatant in _currentBattle.Party) {
			if (combatant.Character != null) {
				combatant.Character.CurrentHp = (ushort)combatant.CurrentHp;
				combatant.Character.CurrentMp = (ushort)combatant.CurrentMp;
				combatant.Character.Status = (CharacterStatus)((int)combatant.Status & 0xFF);
			}
		}
	}

	#endregion

	#region Utility

	/// <summary>Get available actions for a combatant.</summary>
	public List<BattleActionType> GetAvailableActions(BattleCombatant combatant) {
		var actions = new List<BattleActionType> {
			BattleActionType.Attack,
			BattleActionType.Defend
		};

		// Check for spells
		if (combatant.Character != null && combatant.CurrentMp > 0) {
			var knownSpells = combatant.Character.GetKnownSpells();
			if (knownSpells.Any(id => _spellData.ContainsKey((ushort)id))) {
				actions.Add(BattleActionType.Spell);
			}
		}

		// Check for items (would need inventory access)
		actions.Add(BattleActionType.Item);

		// Flee option
		if (_currentBattle?.CanFlee == true) {
			actions.Add(BattleActionType.Flee);
		}

		return actions;
	}

	/// <summary>Get usable spells for a combatant.</summary>
	public List<SpellData> GetUsableSpells(BattleCombatant combatant) {
		if (combatant.Character == null) return [];

		return combatant.Character.GetKnownSpells()
			.Where(id => _spellData.ContainsKey((ushort)id))
			.Select(id => _spellData[(ushort)id])
			.Where(s => s.BattleUse && s.MpCost <= combatant.CurrentMp)
			.ToList();
	}

	#endregion
}

/// <summary>
/// Current state of a battle in progress.
/// </summary>
public class BattleState {
	public ushort EncounterId { get; set; }
	public BattlePhase Phase { get; set; }
	public bool IsBoss { get; set; }
	public bool CanFlee { get; set; }
	public int TurnNumber { get; set; }
	public int CurrentInputIndex { get; set; }
	public List<BattleCombatant> Party { get; set; } = [];
	public List<BattleCombatant> Monsters { get; set; } = [];
	public Queue<BattleCombatant> TurnOrder { get; set; } = [];
}

#region Event Args

public class BattleStartedEventArgs : EventArgs {
	public BattleEncounter? Encounter { get; set; }
	public List<BattleCombatant> PartyMembers { get; set; } = [];
	public List<BattleCombatant> Monsters { get; set; } = [];
}

public class BattleEndedEventArgs : EventArgs {
	public BattleResult? Result { get; set; }
}

public class TurnStartedEventArgs : EventArgs {
	public int TurnNumber { get; set; }
	public List<BattleCombatant> TurnOrder { get; set; } = [];
}

public class ActionExecutedEventArgs : EventArgs {
	public BattleActionResult? Result { get; set; }
}

public class CombatantDefeatedEventArgs : EventArgs {
	public BattleCombatant? Combatant { get; set; }
}

public class LevelUpEventArgs : EventArgs {
	public int CharacterIndex { get; set; }
	public int NewLevel { get; set; }
	public Dictionary<string, int> StatIncreases { get; set; } = [];
	public List<ushort> SpellsLearned { get; set; } = [];
}

public class InputRequestedEventArgs : EventArgs {
	public BattleCombatant? Combatant { get; set; }
	public List<BattleActionType> AvailableActions { get; set; } = [];
}

#endregion
