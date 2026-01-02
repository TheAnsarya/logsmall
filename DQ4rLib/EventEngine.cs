namespace DQ4rLib;

using DQ4rLib.Models;

/// <summary>
/// Event scripting engine for story progression and game logic.
/// Executes scripts based on triggers and conditions.
/// </summary>
public class EventEngine {
	private readonly Dictionary<ushort, EventScript> _scripts = [];
	private readonly Stack<ScriptState> _callStack = [];
	private readonly ChapterManager _chapterManager;
	private readonly CutsceneManager _cutsceneManager;

	private ScriptState? _currentState;
	private EventContext _context = new();
	private bool _isExecuting;
	private bool _waitingForDialog;
	private bool _waitingForBattle;
	private bool _waitingForCutscene;
	private int _waitFrames;

	// Events
	public event EventHandler<ScriptEventArgs>? ScriptStarted;
	public event EventHandler<ScriptEventArgs>? ScriptCompleted;
	public event EventHandler<DialogEventArgs>? DialogRequested;
	public event EventHandler<ChoiceEventArgs>? ChoiceRequested;
	public event EventHandler<TeleportEventArgs>? TeleportRequested;
	public event EventHandler<BattleEventArgs>? BattleRequested;
	public event EventHandler<ItemEventArgs>? ItemGiven;
	public event EventHandler<GoldEventArgs>? GoldChanged;
	public event EventHandler<PartyEventArgs>? PartyChanged;
	public event EventHandler<AudioEventArgs>? AudioRequested;
	public event EventHandler<ScreenEffectEventArgs>? ScreenEffectRequested;
	public event EventHandler<ChapterTransitionEventArgs>? ChapterTransitionRequested;

	public EventEngine(ChapterManager chapterManager, CutsceneManager cutsceneManager) {
		_chapterManager = chapterManager;
		_cutsceneManager = cutsceneManager;

		// Hook cutscene completion
		_cutsceneManager.CutsceneCompleted += (_, _) => {
			_waitingForCutscene = false;
		};
	}

	/// <summary>Current execution context.</summary>
	public EventContext Context => _context;

	/// <summary>Whether a script is currently executing.</summary>
	public bool IsExecuting => _isExecuting;

	/// <summary>Whether waiting for external input.</summary>
	public bool IsWaiting => _waitingForDialog || _waitingForBattle || _waitingForCutscene || _waitFrames > 0;

	/// <summary>Register a script.</summary>
	public void RegisterScript(EventScript script) {
		_scripts[script.Id] = script;
	}

	/// <summary>Remove a script.</summary>
	public void UnregisterScript(ushort scriptId) {
		_scripts.Remove(scriptId);
	}

	/// <summary>Get a script by ID.</summary>
	public EventScript? GetScript(ushort scriptId) {
		return _scripts.GetValueOrDefault(scriptId);
	}

	/// <summary>Update context with current game state.</summary>
	public void UpdateContext(byte chapterId, ushort mapId, short playerX, short playerY) {
		_context.CurrentChapterId = chapterId;
		_context.CurrentMapId = mapId;
		_context.PlayerX = playerX;
		_context.PlayerY = playerY;

		// Sync flags from chapter state
		if (_chapterManager.State != null) {
			Array.Copy(_chapterManager.State.EventFlags, _context.EventFlags, Math.Min(_chapterManager.State.EventFlags.Length, _context.EventFlags.Length));
		}
	}

	/// <summary>Check and trigger scripts based on trigger type.</summary>
	public void CheckTriggers(EventTriggerType triggerType) {
		if (_isExecuting) return;

		// Find matching scripts sorted by priority
		var matching = _scripts.Values
			.Where(s => s.TriggerType == triggerType && s.CanTrigger(_context))
			.OrderByDescending(s => s.Priority)
			.ToList();

		if (matching.Count > 0) {
			StartScript(matching[0].Id);
		}
	}

	/// <summary>Start executing a script by ID.</summary>
	public bool StartScript(ushort scriptId) {
		if (!_scripts.TryGetValue(scriptId, out var script))
			return false;

		if (_isExecuting) {
			// Push current state and start new script
			if (_currentState != null) {
				_callStack.Push(_currentState);
			}
		}

		_currentState = new ScriptState {
			ScriptId = scriptId,
			InstructionIndex = 0
		};
		_isExecuting = true;

		ScriptStarted?.Invoke(this, new ScriptEventArgs { ScriptId = scriptId, Script = script });

		return true;
	}

	/// <summary>Stop script execution.</summary>
	public void StopScript() {
		if (!_isExecuting) return;

		_isExecuting = false;
		_currentState = null;
		_callStack.Clear();
		_waitingForDialog = false;
		_waitingForBattle = false;
		_waitingForCutscene = false;
		_waitFrames = 0;
	}

	/// <summary>Update script execution (call each frame).</summary>
	public void Update() {
		if (!_isExecuting || _currentState == null) return;

		// Handle wait timer
		if (_waitFrames > 0) {
			_waitFrames--;
			return;
		}

		// Waiting for external input
		if (_waitingForDialog || _waitingForBattle || _waitingForCutscene)
			return;

		// Execute instructions
		while (_isExecuting && _currentState != null && !IsWaiting) {
			if (!ExecuteNextInstruction())
				break;
		}
	}

	/// <summary>Execute the next instruction.</summary>
	private bool ExecuteNextInstruction() {
		if (_currentState == null) return false;

		if (!_scripts.TryGetValue(_currentState.ScriptId, out var script))
			return false;

		if (_currentState.InstructionIndex >= script.Instructions.Count) {
			// Script complete
			CompleteCurrentScript(script);
			return true;
		}

		var instruction = script.Instructions[_currentState.InstructionIndex];
		_currentState.InstructionIndex++;

		return ExecuteInstruction(instruction);
	}

	/// <summary>Execute a single instruction.</summary>
	private bool ExecuteInstruction(ScriptInstruction inst) {
		switch (inst.Opcode) {
			case ScriptOpcode.Nop:
				return true;

			case ScriptOpcode.End:
				CompleteCurrentScript(_scripts[_currentState!.ScriptId]);
				return false;

			case ScriptOpcode.Wait:
				_waitFrames = inst.Parameters[0];
				return true;

			// Flag operations
			case ScriptOpcode.SetFlag:
				_context.SetFlag(inst.Parameters[0], true);
				SyncFlagsToChapter();
				return true;

			case ScriptOpcode.ClearFlag:
				_context.SetFlag(inst.Parameters[0], false);
				SyncFlagsToChapter();
				return true;

			case ScriptOpcode.ToggleFlag:
				_context.SetFlag(inst.Parameters[0], !_context.GetFlag(inst.Parameters[0]));
				SyncFlagsToChapter();
				return true;

			case ScriptOpcode.CopyFlag:
				_context.SetFlag(inst.Parameters[1], _context.GetFlag(inst.Parameters[0]));
				SyncFlagsToChapter();
				return true;

			// Flow control
			case ScriptOpcode.Jump:
				_currentState!.InstructionIndex = inst.Parameters[0];
				return true;

			case ScriptOpcode.JumpIfSet:
				if (_context.GetFlag(inst.Parameters[1]))
					_currentState!.InstructionIndex = inst.Parameters[0];
				return true;

			case ScriptOpcode.JumpIfClear:
				if (!_context.GetFlag(inst.Parameters[1]))
					_currentState!.InstructionIndex = inst.Parameters[0];
				return true;

			case ScriptOpcode.JumpIfEqual:
				if (_context.Variables[inst.Parameters[1]] == inst.Parameters[2])
					_currentState!.InstructionIndex = inst.Parameters[0];
				return true;

			case ScriptOpcode.JumpIfNotEqual:
				if (_context.Variables[inst.Parameters[1]] != inst.Parameters[2])
					_currentState!.InstructionIndex = inst.Parameters[0];
				return true;

			case ScriptOpcode.JumpIfGreater:
				if (_context.Variables[inst.Parameters[1]] > inst.Parameters[2])
					_currentState!.InstructionIndex = inst.Parameters[0];
				return true;

			case ScriptOpcode.JumpIfLess:
				if (_context.Variables[inst.Parameters[1]] < inst.Parameters[2])
					_currentState!.InstructionIndex = inst.Parameters[0];
				return true;

			case ScriptOpcode.CallScript:
				StartScript((ushort)inst.Parameters[0]);
				return true;

			case ScriptOpcode.Return:
				// Fire completion event for the returning script
				ScriptCompleted?.Invoke(this, new ScriptEventArgs {
					ScriptId = _currentState!.ScriptId,
					Script = _scripts[_currentState.ScriptId]
				});
				// Return to caller
				if (_callStack.Count > 0) {
					_currentState = _callStack.Pop();
				} else {
					_isExecuting = false;
					_currentState = null;
				}
				return true;

			// Dialog
			case ScriptOpcode.ShowDialog:
				_waitingForDialog = true;
				DialogRequested?.Invoke(this, new DialogEventArgs { DialogId = inst.Parameters[0] });
				return true;

			case ScriptOpcode.ShowChoice:
				_waitingForDialog = true;
				ChoiceRequested?.Invoke(this, new ChoiceEventArgs {
					DialogId = inst.Parameters[0],
					ChoiceCount = inst.Parameters[1]
				});
				return true;

			// Items and gold
			case ScriptOpcode.GiveItem:
				ItemGiven?.Invoke(this, new ItemEventArgs {
					ItemId = inst.Parameters[0],
					Count = inst.Parameters[1],
					IsGiving = true
				});
				return true;

			case ScriptOpcode.TakeItem:
				ItemGiven?.Invoke(this, new ItemEventArgs {
					ItemId = inst.Parameters[0],
					Count = inst.Parameters[1],
					IsGiving = false
				});
				return true;

			case ScriptOpcode.CheckItem:
				// Store result in variable
				// Implementation depends on inventory system
				return true;

			case ScriptOpcode.GiveGold:
				GoldChanged?.Invoke(this, new GoldEventArgs {
					Amount = inst.Parameters[0],
					IsGiving = true
				});
				return true;

			case ScriptOpcode.TakeGold:
				GoldChanged?.Invoke(this, new GoldEventArgs {
					Amount = inst.Parameters[0],
					IsGiving = false
				});
				return true;

			// Party
			case ScriptOpcode.AddPartyMember:
				PartyChanged?.Invoke(this, new PartyEventArgs {
					CharacterId = inst.Parameters[0],
					IsAdding = true
				});
				return true;

			case ScriptOpcode.RemovePartyMember:
				PartyChanged?.Invoke(this, new PartyEventArgs {
					CharacterId = inst.Parameters[0],
					IsAdding = false
				});
				return true;

			case ScriptOpcode.HealParty:
				PartyChanged?.Invoke(this, new PartyEventArgs {
					CharacterId = -1, // All
					HealAmount = inst.Parameters[0]
				});
				return true;

			// Movement
			case ScriptOpcode.Teleport:
				TeleportRequested?.Invoke(this, new TeleportEventArgs {
					MapId = inst.Parameters[0],
					X = inst.Parameters[1],
					Y = inst.Parameters[2]
				});
				return true;

			case ScriptOpcode.LockMovement:
				// Signal movement lock
				return true;

			case ScriptOpcode.UnlockMovement:
				// Signal movement unlock
				return true;

			// Battle
			case ScriptOpcode.StartBattle:
				_waitingForBattle = true;
				BattleRequested?.Invoke(this, new BattleEventArgs {
					BattleId = inst.Parameters[0],
					IsBoss = false
				});
				return true;

			case ScriptOpcode.StartBossBattle:
				_waitingForBattle = true;
				BattleRequested?.Invoke(this, new BattleEventArgs {
					BattleId = inst.Parameters[0],
					IsBoss = true
				});
				return true;

			// Audio
			case ScriptOpcode.PlayMusic:
				AudioRequested?.Invoke(this, new AudioEventArgs {
					AudioId = inst.Parameters[0],
					AudioType = AudioType.Music,
					FadeFrames = inst.Parameters[1]
				});
				return true;

			case ScriptOpcode.StopMusic:
				AudioRequested?.Invoke(this, new AudioEventArgs {
					AudioId = -1,
					AudioType = AudioType.Music,
					FadeFrames = inst.Parameters[0]
				});
				return true;

			case ScriptOpcode.PlaySound:
				AudioRequested?.Invoke(this, new AudioEventArgs {
					AudioId = inst.Parameters[0],
					AudioType = AudioType.Sound
				});
				return true;

			// Screen effects
			case ScriptOpcode.FadeScreen:
				ScreenEffectRequested?.Invoke(this, new ScreenEffectEventArgs {
					EffectType = ScreenEffectType.Fade,
					Parameter1 = inst.Parameters[0],
					Parameter2 = inst.Parameters[1]
				});
				return true;

			case ScriptOpcode.FlashScreen:
				ScreenEffectRequested?.Invoke(this, new ScreenEffectEventArgs {
					EffectType = ScreenEffectType.Flash,
					Parameter1 = inst.Parameters[0],
					Parameter2 = inst.Parameters[1]
				});
				return true;

			case ScriptOpcode.ShakeScreen:
				ScreenEffectRequested?.Invoke(this, new ScreenEffectEventArgs {
					EffectType = ScreenEffectType.Shake,
					Parameter1 = inst.Parameters[0],
					Parameter2 = inst.Parameters[1]
				});
				return true;

			// Cutscene and chapter
			case ScriptOpcode.PlayCutscene:
				_waitingForCutscene = true;
				_cutsceneManager.PlayCutscene((ushort)inst.Parameters[0]);
				return true;

			case ScriptOpcode.ChapterTransition:
				ChapterTransitionRequested?.Invoke(this, new ChapterTransitionEventArgs {
					ToChapterId = (byte)inst.Parameters[0]
				});
				return true;

			case ScriptOpcode.ShowChapterTitle:
				// Show chapter title card
				return true;

			case ScriptOpcode.ShowLocationName:
				// Show location name
				return true;

			// Variables
			case ScriptOpcode.SetVariable:
				if (inst.Parameters[0] < _context.Variables.Length)
					_context.Variables[inst.Parameters[0]] = inst.Parameters[1];
				return true;

			case ScriptOpcode.AddVariable:
				if (inst.Parameters[0] < _context.Variables.Length)
					_context.Variables[inst.Parameters[0]] += inst.Parameters[1];
				return true;

			case ScriptOpcode.SubVariable:
				if (inst.Parameters[0] < _context.Variables.Length)
					_context.Variables[inst.Parameters[0]] -= inst.Parameters[1];
				return true;

			case ScriptOpcode.MulVariable:
				if (inst.Parameters[0] < _context.Variables.Length)
					_context.Variables[inst.Parameters[0]] *= inst.Parameters[1];
				return true;

			case ScriptOpcode.DivVariable:
				if (inst.Parameters[0] < _context.Variables.Length && inst.Parameters[1] != 0)
					_context.Variables[inst.Parameters[0]] /= inst.Parameters[1];
				return true;

			case ScriptOpcode.RandomVariable:
				if (inst.Parameters[0] < _context.Variables.Length)
					_context.Variables[inst.Parameters[0]] = Random.Shared.Next(inst.Parameters[1], inst.Parameters[2] + 1);
				return true;

			// Shops and services
			case ScriptOpcode.OpenShop:
			case ScriptOpcode.OpenInn:
			case ScriptOpcode.OpenChurch:
			case ScriptOpcode.OpenBank:
			case ScriptOpcode.OpenCasino:
				// These would open respective menus
				return true;

			// Debug
			case ScriptOpcode.DebugPrint:
#if DEBUG
				Console.WriteLine($"[Script Debug] Var[{inst.Parameters[0]}] = {_context.Variables[inst.Parameters[0]]}");
#endif
				return true;

			case ScriptOpcode.DebugBreak:
#if DEBUG
				System.Diagnostics.Debugger.Break();
#endif
				return true;

			default:
				return true;
		}
	}

	/// <summary>Complete the current script.</summary>
	private void CompleteCurrentScript(EventScript script) {
		if (_currentState == null) return;

		// Mark non-repeatable scripts as executed
		if (!script.Repeatable) {
			// Could track in a separate set
		}

		ScriptCompleted?.Invoke(this, new ScriptEventArgs {
			ScriptId = _currentState.ScriptId,
			Script = script
		});

		// Return to caller if any
		if (_callStack.Count > 0) {
			_currentState = _callStack.Pop();
		} else {
			_isExecuting = false;
			_currentState = null;
		}
	}

	/// <summary>Sync event flags back to chapter state.</summary>
	private void SyncFlagsToChapter() {
		if (_chapterManager.State != null) {
			Array.Copy(_context.EventFlags, _chapterManager.State.EventFlags, Math.Min(_context.EventFlags.Length, _chapterManager.State.EventFlags.Length));
		}
	}

	/// <summary>Called when dialog is dismissed.</summary>
	public void OnDialogComplete(int result = 0) {
		_waitingForDialog = false;
		_context.LastChoiceResult = result;
	}

	/// <summary>Called when battle is complete.</summary>
	public void OnBattleComplete(int result) {
		_waitingForBattle = false;
		_context.LastBattleResult = result;
	}

	/// <summary>Load scripts from binary data.</summary>
	public void LoadScripts(byte[] data) {
		_scripts.Clear();

		using var ms = new MemoryStream(data);
		using var br = new BinaryReader(ms);

		// Header
		int count = br.ReadUInt16();

		// Script table
		var offsets = new uint[count];
		for (int i = 0; i < count; i++) {
			offsets[i] = br.ReadUInt32();
		}

		// Load each script
		for (int i = 0; i < count; i++) {
			ms.Position = offsets[i];
			int size = (i < count - 1) ? (int)(offsets[i + 1] - offsets[i]) : (int)(data.Length - offsets[i]);
			byte[] scriptData = br.ReadBytes(size);
			var script = EventScript.FromSnesBytes(scriptData);
			_scripts[script.Id] = script;
		}
	}

	/// <summary>Save all scripts to binary data.</summary>
	public byte[] SaveScripts() {
		using var ms = new MemoryStream();
		using var bw = new BinaryWriter(ms);

		var scripts = _scripts.Values.OrderBy(s => s.Id).ToList();

		// Header
		bw.Write((ushort)scripts.Count);

		// Reserve space for offset table
		long tablePos = ms.Position;
		for (int i = 0; i < scripts.Count; i++) {
			bw.Write((uint)0);
		}

		// Write scripts and record offsets
		var offsets = new uint[scripts.Count];
		for (int i = 0; i < scripts.Count; i++) {
			offsets[i] = (uint)ms.Position;
			bw.Write(scripts[i].ToSnesBytes());
		}

		// Go back and write offsets
		ms.Position = tablePos;
		foreach (var offset in offsets) {
			bw.Write(offset);
		}

		return ms.ToArray();
	}

	/// <summary>Create chapter start scripts for all chapters.</summary>
	public void CreateChapterStartScripts() {
		// Chapter 1 - Ragnar's Story
		RegisterScript(new EventScript {
			Id = 0x0001,
			Name = "Chapter 1 Start",
			ChapterId = 1,
			TriggerType = EventTriggerType.ChapterStart,
			Instructions = [
				ScriptInstruction.PlayCutscene(0x0100), // Chapter 1 intro cutscene
				ScriptInstruction.SetFlag(1), // Mark chapter 1 started
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		});

		// Chapter 2 - Alena's Story
		RegisterScript(new EventScript {
			Id = 0x0002,
			Name = "Chapter 2 Start",
			ChapterId = 2,
			TriggerType = EventTriggerType.ChapterStart,
			Instructions = [
				ScriptInstruction.PlayCutscene(0x0200),
				ScriptInstruction.SetFlag(32), // Chapter 2 flags start at 32
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		});

		// Chapter 3 - Torneko's Story
		RegisterScript(new EventScript {
			Id = 0x0003,
			Name = "Chapter 3 Start",
			ChapterId = 3,
			TriggerType = EventTriggerType.ChapterStart,
			Instructions = [
				ScriptInstruction.PlayCutscene(0x0300),
				ScriptInstruction.SetFlag(64),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		});

		// Chapter 4 - Sisters' Story
		RegisterScript(new EventScript {
			Id = 0x0004,
			Name = "Chapter 4 Start",
			ChapterId = 4,
			TriggerType = EventTriggerType.ChapterStart,
			Instructions = [
				ScriptInstruction.PlayCutscene(0x0400),
				ScriptInstruction.SetFlag(96),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		});

		// Chapter 5 - Hero's Story
		RegisterScript(new EventScript {
			Id = 0x0005,
			Name = "Chapter 5 Start",
			ChapterId = 5,
			TriggerType = EventTriggerType.ChapterStart,
			Instructions = [
				ScriptInstruction.PlayCutscene(0x0500),
				ScriptInstruction.SetFlag(128),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		});
	}
}

/// <summary>
/// Internal script execution state.
/// </summary>
internal class ScriptState {
	public ushort ScriptId { get; set; }
	public int InstructionIndex { get; set; }
}

// Event argument classes
public class ScriptEventArgs : EventArgs {
	public ushort ScriptId { get; set; }
	public EventScript? Script { get; set; }
}

public class DialogEventArgs : EventArgs {
	public int DialogId { get; set; }
}

public class ChoiceEventArgs : EventArgs {
	public int DialogId { get; set; }
	public int ChoiceCount { get; set; }
}

public class TeleportEventArgs : EventArgs {
	public int MapId { get; set; }
	public int X { get; set; }
	public int Y { get; set; }
}

public class BattleEventArgs : EventArgs {
	public int BattleId { get; set; }
	public bool IsBoss { get; set; }
}

public class ItemEventArgs : EventArgs {
	public int ItemId { get; set; }
	public int Count { get; set; }
	public bool IsGiving { get; set; }
}

public class GoldEventArgs : EventArgs {
	public int Amount { get; set; }
	public bool IsGiving { get; set; }
}

public class PartyEventArgs : EventArgs {
	public int CharacterId { get; set; }
	public bool IsAdding { get; set; }
	public int HealAmount { get; set; }
}

public class AudioEventArgs : EventArgs {
	public int AudioId { get; set; }
	public AudioType AudioType { get; set; }
	public int FadeFrames { get; set; }
}

public class ScreenEffectEventArgs : EventArgs {
	public ScreenEffectType EffectType { get; set; }
	public int Parameter1 { get; set; }
	public int Parameter2 { get; set; }
}

public enum AudioType {
	Music,
	Sound
}

public enum ScreenEffectType {
	Fade,
	Flash,
	Shake
}
