# DQ4rLib

A .NET library for Dragon Quest 4 SNES port (DQ4r) development, based on the DQ3r engine.

## Overview

DQ4rLib provides tools and systems for:
- Save/Load game management
- Cutscene playback
- Event scripting engine
- Chapter management
- Asset pipeline (graphics, audio, text)
- Data conversion utilities

## Systems

### Save/Load System

`SaveManager` and `SaveData` provide complete save game functionality:

```csharp
var saveManager = new SaveManager();

// Create and modify save
var saveData = saveManager.CreateNewSave(0, "HERO");
saveData.Gold = 1000;
saveData.AddItem(0x10); // Medicinal Herb

// Save to slot
saveManager.Save(0, saveData);

// Load from slot
var loaded = saveManager.Load(0);

// Export/Import SRAM
byte[] sram = saveManager.ExportSram();
saveManager.ImportSram(sramBytes);
```

**Features:**
- 4 save slots (2KB each, 8KB total SRAM)
- XOR checksum with 0x5a5a mask
- Character stats, inventory, gold, play time
- Chapter/story progress tracking
- 256 event flags
- Auto-save support

### Cutscene System

`CutsceneManager` and `Cutscene` handle in-game cutscenes:

```csharp
var cutsceneManager = new CutsceneManager();
cutsceneManager.CutsceneCompleted += (s, e) => Console.WriteLine("Done!");

var cutscene = new Cutscene("intro")
{
	Commands =
	[
		new CutsceneCommand(CutsceneOpcode.FadeOut),
		new CutsceneCommand(CutsceneOpcode.Dialog) { Parameters = [0x01] },
		new CutsceneCommand(CutsceneOpcode.Wait) { Parameters = [60] },
		new CutsceneCommand(CutsceneOpcode.FadeIn)
	]
};

cutsceneManager.Play(cutscene);
while (cutsceneManager.IsPlaying)
	cutsceneManager.Update();
```

**Opcodes:**
- Display: `Dialog`, `ShowPortrait`, `HidePortrait`
- Effects: `FadeIn`, `FadeOut`, `Flash`, `ShakeScreen`
- Audio: `PlaySound`, `PlayMusic`, `StopMusic`
- Control: `Wait`, `Jump`, `Call`, `Return`, `End`
- Camera: `MoveCameraTo`, `SetCameraTarget`
- Character: `MoveCharacter`, `SetCharacterPosition`

### Event Scripting Engine

`EventEngine` and `EventScript` provide map/NPC scripting:

```csharp
var eventEngine = new EventEngine();
eventEngine.ScriptCompleted += (s, e) => Console.WriteLine($"Script {e.ScriptId} done");

var script = new EventScript("npc_guard")
{
	Triggers = [TriggerType.Talk],
	Instructions =
	[
		new ScriptInstruction(ScriptOpcode.CheckFlag) { Parameters = [10] },
		new ScriptInstruction(ScriptOpcode.JumpIfFalse) { Parameters = [3] },
		new ScriptInstruction(ScriptOpcode.ShowMessage) { Parameters = [0x42] },
		new ScriptInstruction(ScriptOpcode.End),
		new ScriptInstruction(ScriptOpcode.ShowMessage) { Parameters = [0x41] },
		new ScriptInstruction(ScriptOpcode.End)
	]
};

eventEngine.LoadScript(script);
eventEngine.Execute("npc_guard");
while (eventEngine.IsExecuting)
	eventEngine.Step();
```

**Features:**
- ~50 opcodes covering all game scenarios
- 256 event flags (persistent state)
- 64 variables (temporary state)
- 16-level call stack for nested scripts
- 10 trigger types (MapEntry, Talk, Step, Item, etc.)
- Battle and cutscene integration

### Battle System

`BattleManager` provides full turn-based combat:

```csharp
var battleManager = new BattleManager(chapterManager);
battleManager.BattleStarted += (s, e) => Console.WriteLine("Battle!");
battleManager.BattleEnded += (s, e) => Console.WriteLine($"Result: {e.Result.Outcome}");

// Register monster data
battleManager.RegisterMonster(new MonsterData {
	Id = 1, Name = "Slime", MaxHp = 10, Attack = 5
});

// Register encounter
battleManager.RegisterEncounter(new BattleEncounter {
	Id = 1,
	MonsterGroups = [new MonsterGroup { MonsterId = 1, MinCount = 2, MaxCount = 4 }]
});

// Start battle
battleManager.StartBattle(encounterId: 1);

// Set player actions
battleManager.SetAction(new BattleAction {
	Type = BattleActionType.Attack,
	Targets = [enemy]
});
```

**Features:**
- Full turn-based combat with agility-based turn order
- DQ4 AI tactics (ShowNoMercy, GoAllOut, WatchMyMp, etc.)
- Spell system with damage, healing, buffs, debuffs
- Status effects with duration tracking
- Monster AI with weighted action selection
- Boss battles with flee restriction
- Experience/gold rewards and item drops

### Chapter Manager

`ChapterManager` handles the 5-chapter structure:

```csharp
var chapterManager = new ChapterManager();

chapterManager.StartChapter(1); // Ragnar
var currentChapter = chapterManager.CurrentChapter;
var partyMembers = chapterManager.GetChapterParty();
```

## Project Structure

```
DQ4rLib/
├── SaveManager.cs          # Save/load system
├── CutsceneManager.cs      # Cutscene playback
├── EventEngine.cs          # Event scripting
├── BattleManager.cs        # Turn-based combat
├── ChapterManager.cs       # Chapter management
├── AssetPipeline.cs        # Asset processing
├── Models/
│   ├── SaveData.cs         # Save file structure
│   ├── ChapterState.cs     # Chapter state
│   ├── Battle.cs           # Battle data models
│   └── ...
├── Graphics/               # Graphics conversion
├── Audio/                  # Audio conversion
├── Text/                   # Text/dialog handling
├── Data/                   # Game data definitions
└── Converters/             # Format converters
```

## Testing

```bash
cd DQ4rLib.Tests
dotnet test
```

177 tests covering all major systems.

## Requirements

- .NET 10.0
- C# 14

## Related Projects

- **logsmall** - Parent solution with DQ3 tools
- **dragon-warrior-4-info** - DW4 NES disassembly documentation
- **GameInfo** - ROM hacking tools and documentation

## License

See repository LICENSE file.
