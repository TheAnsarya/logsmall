namespace DQ4rLib.Tests;

using DQ4rLib;
using DQ4rLib.Models;

public class EventEngineTests {
	private EventEngine CreateEngine() {
		var chapterManager = new ChapterManager(new ChapterState());
		var cutsceneManager = new CutsceneManager();
		return new EventEngine(chapterManager, cutsceneManager);
	}

	[Fact]
	public void EventEngine_RegistersAndRetrievesScripts() {
		var engine = CreateEngine();

		var script = new EventScript {
			Id = 0x1000,
			Name = "Test Script"
		};

		engine.RegisterScript(script);
		var retrieved = engine.GetScript(0x1000);

		Assert.NotNull(retrieved);
		Assert.Equal("Test Script", retrieved.Name);
	}

	[Fact]
	public void EventEngine_StartsScript() {
		var engine = CreateEngine();
		bool started = false;

		engine.ScriptStarted += (_, e) => {
			started = true;
			Assert.Equal((ushort)0x0001, e.ScriptId);
		};

		var script = new EventScript {
			Id = 0x0001,
			Instructions = [
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		bool result = engine.StartScript(0x0001);

		Assert.True(result);
		Assert.True(started);
		Assert.True(engine.IsExecuting);
	}

	[Fact]
	public void EventEngine_ExecutesSetFlagInstruction() {
		var engine = CreateEngine();

		var script = new EventScript {
			Id = 0x0002,
			Instructions = [
				ScriptInstruction.SetFlag(5),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x0002);

		// Execute instructions
		engine.Update();
		engine.Update();

		Assert.True(engine.Context.GetFlag(5));
	}

	[Fact]
	public void EventEngine_ExecutesClearFlagInstruction() {
		var engine = CreateEngine();

		engine.Context.SetFlag(10, true);
		Assert.True(engine.Context.GetFlag(10));

		var script = new EventScript {
			Id = 0x0003,
			Instructions = [
				ScriptInstruction.ClearFlag(10),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x0003);
		engine.Update();
		engine.Update();

		Assert.False(engine.Context.GetFlag(10));
	}

	[Fact]
	public void EventEngine_ExecutesJumpInstruction() {
		var engine = CreateEngine();

		var script = new EventScript {
			Id = 0x0004,
			Instructions = [
				new ScriptInstruction { Opcode = ScriptOpcode.Jump, Parameters = [2, 0, 0] }, // Jump to instruction 2
				ScriptInstruction.SetFlag(1), // This should be skipped
				ScriptInstruction.SetFlag(2), // This should execute
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x0004);

		for (int i = 0; i < 10; i++) engine.Update();

		Assert.False(engine.Context.GetFlag(1)); // Skipped
		Assert.True(engine.Context.GetFlag(2)); // Executed
	}

	[Fact]
	public void EventEngine_ExecutesJumpIfSetInstruction() {
		var engine = CreateEngine();

		engine.Context.SetFlag(0, true);

		var script = new EventScript {
			Id = 0x0005,
			Instructions = [
				new ScriptInstruction {
					Opcode = ScriptOpcode.JumpIfSet,
					Parameters = [3, 0, 0] // Jump to 3 if flag 0 set
				},
				ScriptInstruction.SetFlag(1), // Skipped
				ScriptInstruction.SetFlag(2), // Skipped
				ScriptInstruction.SetFlag(3), // Executed
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x0005);

		for (int i = 0; i < 10; i++) engine.Update();

		Assert.False(engine.Context.GetFlag(1));
		Assert.False(engine.Context.GetFlag(2));
		Assert.True(engine.Context.GetFlag(3));
	}

	[Fact]
	public void EventEngine_ExecutesWaitInstruction() {
		var engine = CreateEngine();

		var script = new EventScript {
			Id = 0x0006,
			Instructions = [
				new ScriptInstruction { Opcode = ScriptOpcode.Wait, Parameters = [5, 0, 0] },
				ScriptInstruction.SetFlag(1),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x0006);

		engine.Update(); // Start wait

		Assert.True(engine.IsWaiting);

		// Flag should not be set during wait
		for (int i = 0; i < 3; i++) {
			engine.Update();
			Assert.False(engine.Context.GetFlag(1));
		}

		// After wait completes
		for (int i = 0; i < 5; i++) engine.Update();

		Assert.True(engine.Context.GetFlag(1));
	}

	[Fact]
	public void EventEngine_FiresDialogRequestedEvent() {
		var engine = CreateEngine();
		int requestedDialogId = -1;

		engine.DialogRequested += (_, e) => requestedDialogId = e.DialogId;

		var script = new EventScript {
			Id = 0x0007,
			Instructions = [
				ScriptInstruction.ShowDialog(42),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x0007);
		engine.Update();
		engine.Update();

		Assert.Equal(42, requestedDialogId);
		Assert.True(engine.IsWaiting);
	}

	[Fact]
	public void EventEngine_ContinuesAfterDialogComplete() {
		var engine = CreateEngine();

		var script = new EventScript {
			Id = 0x0008,
			Instructions = [
				ScriptInstruction.ShowDialog(1),
				ScriptInstruction.SetFlag(1),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x0008);
		engine.Update();
		engine.Update();

		Assert.True(engine.IsWaiting);
		Assert.False(engine.Context.GetFlag(1));

		engine.OnDialogComplete();
		engine.Update();
		engine.Update();

		Assert.True(engine.Context.GetFlag(1));
	}

	[Fact]
	public void EventEngine_FiresBattleRequestedEvent() {
		var engine = CreateEngine();
		int battleId = -1;
		bool isBoss = false;

		engine.BattleRequested += (_, e) => {
			battleId = e.BattleId;
			isBoss = e.IsBoss;
		};

		var script = new EventScript {
			Id = 0x0009,
			Instructions = [
				ScriptInstruction.StartBattle(100),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x0009);
		engine.Update();
		engine.Update();

		Assert.Equal(100, battleId);
		Assert.False(isBoss);
		Assert.True(engine.IsWaiting);
	}

	[Fact]
	public void EventEngine_FiresItemGivenEvent() {
		var engine = CreateEngine();
		int itemId = -1;
		int count = 0;

		engine.ItemGiven += (_, e) => {
			itemId = e.ItemId;
			count = e.Count;
		};

		var script = new EventScript {
			Id = 0x000A,
			Instructions = [
				ScriptInstruction.GiveItem(50, 3),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x000A);
		engine.Update();
		engine.Update();

		Assert.Equal(50, itemId);
		Assert.Equal(3, count);
	}

	[Fact]
	public void EventEngine_FiresGoldChangedEvent() {
		var engine = CreateEngine();
		int amount = 0;
		bool isGiving = false;

		engine.GoldChanged += (_, e) => {
			amount = e.Amount;
			isGiving = e.IsGiving;
		};

		var script = new EventScript {
			Id = 0x000B,
			Instructions = [
				ScriptInstruction.GiveGold(1000),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x000B);
		engine.Update();
		engine.Update();

		Assert.Equal(1000, amount);
		Assert.True(isGiving);
	}

	[Fact]
	public void EventEngine_FiresTeleportRequestedEvent() {
		var engine = CreateEngine();
		int mapId = -1;
		int x = -1;
		int y = -1;

		engine.TeleportRequested += (_, e) => {
			mapId = e.MapId;
			x = e.X;
			y = e.Y;
		};

		var script = new EventScript {
			Id = 0x000C,
			Instructions = [
				ScriptInstruction.Teleport(0x50, 10, 20),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x000C);
		engine.Update();
		engine.Update();

		Assert.Equal(0x50, mapId);
		Assert.Equal(10, x);
		Assert.Equal(20, y);
	}

	[Fact]
	public void EventEngine_VariableOperations() {
		var engine = CreateEngine();

		var script = new EventScript {
			Id = 0x000D,
			Instructions = [
				new ScriptInstruction { Opcode = ScriptOpcode.SetVariable, Parameters = [0, 10, 0] },
				new ScriptInstruction { Opcode = ScriptOpcode.AddVariable, Parameters = [0, 5, 0] },
				new ScriptInstruction { Opcode = ScriptOpcode.MulVariable, Parameters = [0, 2, 0] },
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x000D);

		for (int i = 0; i < 10; i++) engine.Update();

		Assert.Equal(30, engine.Context.Variables[0]); // (10+5)*2
	}

	[Fact]
	public void EventScript_CanTrigger_ChecksChapter() {
		var script = new EventScript {
			ChapterId = 2,
			TriggerType = EventTriggerType.Auto
		};

		var context = new EventContext { CurrentChapterId = 2 };
		Assert.True(script.CanTrigger(context));

		context.CurrentChapterId = 1;
		Assert.False(script.CanTrigger(context));
	}

	[Fact]
	public void EventScript_CanTrigger_ChecksRequiredFlags() {
		var script = new EventScript {
			ChapterId = 0xff, // Any chapter
			TriggerType = EventTriggerType.Auto,
			RequiredFlags = [5, 10]
		};

		var context = new EventContext();
		Assert.False(script.CanTrigger(context));

		context.SetFlag(5);
		Assert.False(script.CanTrigger(context));

		context.SetFlag(10);
		Assert.True(script.CanTrigger(context));
	}

	[Fact]
	public void EventScript_CanTrigger_ChecksBlockingFlags() {
		var script = new EventScript {
			ChapterId = 0xff,
			TriggerType = EventTriggerType.Auto,
			BlockingFlags = [15]
		};

		var context = new EventContext();
		Assert.True(script.CanTrigger(context));

		context.SetFlag(15);
		Assert.False(script.CanTrigger(context));
	}

	[Fact]
	public void EventScript_SerializesCorrectly() {
		var script = new EventScript {
			Id = 0x1234,
			Name = "Test",
			ChapterId = 2,
			TriggerType = EventTriggerType.MapEnter,
			MapId = 0x50,
			TriggerX = 10,
			TriggerY = 20,
			Priority = 5,
			Repeatable = true,
			RequiredFlags = [1, 2],
			BlockingFlags = [10],
			Instructions = [
				ScriptInstruction.SetFlag(50),
				ScriptInstruction.ShowDialog(1),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		var bytes = script.ToSnesBytes();
		var restored = EventScript.FromSnesBytes(bytes);

		Assert.Equal(script.Id, restored.Id);
		Assert.Equal(script.ChapterId, restored.ChapterId);
		Assert.Equal(script.TriggerType, restored.TriggerType);
		Assert.Equal(script.MapId, restored.MapId);
		Assert.Equal(script.TriggerX, restored.TriggerX);
		Assert.Equal(script.TriggerY, restored.TriggerY);
		Assert.Equal(script.Priority, restored.Priority);
		Assert.Equal(script.Repeatable, restored.Repeatable);
		Assert.Equal(script.RequiredFlags.Count, restored.RequiredFlags.Count);
		Assert.Equal(script.BlockingFlags.Count, restored.BlockingFlags.Count);
		Assert.Equal(script.Instructions.Count, restored.Instructions.Count);
	}

	[Fact]
	public void ScriptInstruction_FactoryMethods() {
		var setFlag = ScriptInstruction.SetFlag(10);
		Assert.Equal(ScriptOpcode.SetFlag, setFlag.Opcode);
		Assert.Equal(10, setFlag.Parameters[0]);

		var giveItem = ScriptInstruction.GiveItem(50, 5);
		Assert.Equal(ScriptOpcode.GiveItem, giveItem.Opcode);
		Assert.Equal(50, giveItem.Parameters[0]);
		Assert.Equal(5, giveItem.Parameters[1]);

		var teleport = ScriptInstruction.Teleport(0x100, 5, 10);
		Assert.Equal(ScriptOpcode.Teleport, teleport.Opcode);
		Assert.Equal(0x100, teleport.Parameters[0]);
		Assert.Equal(5, teleport.Parameters[1]);
		Assert.Equal(10, teleport.Parameters[2]);
	}

	[Fact]
	public void EventEngine_CallScriptPushesState() {
		var engine = CreateEngine();
		var scriptsCompleted = new List<ushort>();

		engine.ScriptCompleted += (_, e) => scriptsCompleted.Add(e.ScriptId);

		var subScript = new EventScript {
			Id = 0x0100,
			Instructions = [
				ScriptInstruction.SetFlag(99),
				new ScriptInstruction { Opcode = ScriptOpcode.Return }
			]
		};

		var mainScript = new EventScript {
			Id = 0x0001,
			Instructions = [
				ScriptInstruction.SetFlag(1),
				ScriptInstruction.CallScript(0x0100),
				ScriptInstruction.SetFlag(2),
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(subScript);
		engine.RegisterScript(mainScript);
		engine.StartScript(0x0001);

		for (int i = 0; i < 20; i++) engine.Update();

		Assert.True(engine.Context.GetFlag(1));
		Assert.True(engine.Context.GetFlag(99)); // From subscript
		Assert.True(engine.Context.GetFlag(2));
		Assert.Contains((ushort)0x0100, scriptsCompleted);
		Assert.Contains((ushort)0x0001, scriptsCompleted);
	}

	[Fact]
	public void EventEngine_StopScriptClearsState() {
		var engine = CreateEngine();

		var script = new EventScript {
			Id = 0x0001,
			Instructions = [
				new ScriptInstruction { Opcode = ScriptOpcode.Wait, Parameters = [100, 0, 0] },
				new ScriptInstruction { Opcode = ScriptOpcode.End }
			]
		};

		engine.RegisterScript(script);
		engine.StartScript(0x0001);
		engine.Update();

		Assert.True(engine.IsExecuting);

		engine.StopScript();

		Assert.False(engine.IsExecuting);
		Assert.False(engine.IsWaiting);
	}

	[Fact]
	public void EventEngine_CreateChapterStartScripts() {
		var engine = CreateEngine();
		engine.CreateChapterStartScripts();

		// Verify scripts for all 5 chapters
		for (ushort i = 1; i <= 5; i++) {
			var script = engine.GetScript(i);
			Assert.NotNull(script);
			Assert.Contains("Chapter", script.Name);
			Assert.Equal(i, script.ChapterId);
			Assert.Equal(EventTriggerType.ChapterStart, script.TriggerType);
		}
	}
}
