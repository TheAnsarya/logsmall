namespace DQ4rLib.Tests;

using DQ4rLib;
using DQ4rLib.Models;

public class CutsceneManagerTests {
	[Fact]
	public void CutsceneManager_RegistersAndRetrievesCutscenes() {
		var manager = new CutsceneManager();

		var cutscene = new Cutscene {
			Id = 0x1000,
			Name = "Test Cutscene",
			Commands = [
				new CutsceneCommand { Opcode = CutsceneOpcode.FadeOut, Parameters = [30, 0, 0, 0] },
				new CutsceneCommand { Opcode = CutsceneOpcode.Wait, Parameters = [60, 0, 0, 0] },
				new CutsceneCommand { Opcode = CutsceneOpcode.FadeIn, Parameters = [30, 0, 0, 0] }
			]
		};

		manager.RegisterCutscene(cutscene);
		var retrieved = manager.GetCutscene(0x1000);

		Assert.NotNull(retrieved);
		Assert.Equal("Test Cutscene", retrieved.Name);
		Assert.Equal(3, retrieved.Commands.Count);
	}

	[Fact]
	public void CutsceneManager_StartsAndExecutesCutscene() {
		var manager = new CutsceneManager();
		var commandsExecuted = new List<CutsceneOpcode>();

		manager.CommandExecuted += (_, e) => commandsExecuted.Add(e.Command.Opcode);

		var cutscene = new Cutscene {
			Id = 0x0001,
			Name = "Simple Test",
			Commands = [
				new CutsceneCommand { Opcode = CutsceneOpcode.ShowText, Parameters = [1, 0, 0, 0] },
				new CutsceneCommand { Opcode = CutsceneOpcode.End }
			]
		};

		manager.RegisterCutscene(cutscene);
		manager.PlayCutscene(0x0001);

		Assert.True(manager.IsPlaying);

		// Update to execute commands
		manager.Update();
		manager.Update();

		Assert.Contains(CutsceneOpcode.ShowText, commandsExecuted);
	}

	[Fact]
	public void CutsceneManager_PauseAndResume() {
		var manager = new CutsceneManager();

		var cutscene = new Cutscene {
			Id = 0x0010,
			Commands = [
				new CutsceneCommand { Opcode = CutsceneOpcode.Wait, Parameters = [100, 0, 0, 0] }
			]
		};

		manager.RegisterCutscene(cutscene);
		manager.PlayCutscene(0x0010);

		Assert.True(manager.IsPlaying);
		Assert.False(manager.IsPaused);

		manager.Pause();
		Assert.True(manager.IsPaused);

		manager.Resume();
		Assert.False(manager.IsPaused);
	}

	[Fact]
	public void CutsceneManager_SkipStopsCutscene() {
		var manager = new CutsceneManager();
		bool completed = false;

		manager.CutsceneCompleted += (_, _) => completed = true;

		var cutscene = new Cutscene {
			Id = 0x0020,
			Skippable = true,
			Commands = [
				new CutsceneCommand { Opcode = CutsceneOpcode.Wait, Parameters = [1000, 0, 0, 0] }
			]
		};

		manager.RegisterCutscene(cutscene);
		manager.PlayCutscene(0x0020);

		manager.Skip();

		Assert.False(manager.IsPlaying);
		Assert.True(completed);
	}

	[Fact]
	public void CutsceneManager_UnskippableCutsceneCannotBeSkipped() {
		var manager = new CutsceneManager();

		var cutscene = new Cutscene {
			Id = 0x0030,
			Skippable = false,
			Commands = [
				new CutsceneCommand { Opcode = CutsceneOpcode.Wait, Parameters = [100, 0, 0, 0] }
			]
		};

		manager.RegisterCutscene(cutscene);
		manager.PlayCutscene(0x0030);

		manager.Skip();

		Assert.True(manager.IsPlaying); // Should still be playing
	}

	[Fact]
	public void CutsceneManager_HasChapterIntroCutscenes() {
		// Verify all 5 chapter intros are defined
		for (int chapter = 1; chapter <= 5; chapter++) {
			Assert.True(CutsceneManager.ChapterIntroCutscenes.ContainsKey(chapter));
			var cutscene = CutsceneManager.ChapterIntroCutscenes[chapter];
			Assert.NotNull(cutscene);
			Assert.Contains("Chapter", cutscene.Name);
		}
	}

	[Fact]
	public void CutsceneManager_PlayChapterIntro() {
		var manager = new CutsceneManager();
		bool started = false;

		manager.CutsceneStarted += (_, _) => started = true;

		var result = manager.PlayChapterIntro(1);

		Assert.True(result);
		Assert.True(started);
		Assert.True(manager.IsPlaying);
	}

	[Fact]
	public void CutsceneManager_EventsFireCorrectly() {
		var manager = new CutsceneManager();
		bool started = false;
		bool completed = false;

		manager.CutsceneStarted += (_, _) => started = true;
		manager.CutsceneCompleted += (_, _) => completed = true;

		var cutscene = new Cutscene {
			Id = 0x0040,
			Commands = [
				new CutsceneCommand { Opcode = CutsceneOpcode.End }
			]
		};

		manager.RegisterCutscene(cutscene);
		manager.PlayCutscene(0x0040);

		Assert.True(started);

		// Run until complete
		for (int i = 0; i < 10; i++) manager.Update();

		Assert.True(completed);
	}

	[Fact]
	public void Cutscene_SerializesCorrectly() {
		var cutscene = new Cutscene {
			Id = 0x1234,
			Name = "Serialization Test",
			Type = CutsceneType.ChapterIntro,
			ChapterId = 3,
			Skippable = true,
			Commands = [
				new CutsceneCommand { Opcode = CutsceneOpcode.FadeOut, Parameters = [30, 0, 0, 0] },
				new CutsceneCommand { Opcode = CutsceneOpcode.ShowText, Parameters = [5, 10, 0, 0] },
				new CutsceneCommand { Opcode = CutsceneOpcode.End }
			]
		};

		var bytes = cutscene.ToSnesBytes();
		var restored = Cutscene.FromSnesBytes(bytes);

		Assert.Equal(cutscene.Id, restored.Id);
		Assert.Equal(cutscene.Type, restored.Type);
		Assert.Equal(cutscene.ChapterId, restored.ChapterId);
		Assert.Equal(cutscene.Skippable, restored.Skippable);
		Assert.Equal(cutscene.Commands.Count, restored.Commands.Count);
	}

	[Fact]
	public void CutsceneCommand_SerializesCorrectly() {
		var command = new CutsceneCommand {
			Opcode = CutsceneOpcode.ChangeMap,
			Parameters = [0x100, 15, 20, 0]
		};

		var bytes = command.ToSnesBytes();
		var restored = CutsceneCommand.FromSnesBytes(bytes);

		Assert.Equal(command.Opcode, restored.Opcode);
		Assert.Equal(command.Parameters[0], restored.Parameters[0]);
		Assert.Equal(command.Parameters[1], restored.Parameters[1]);
		Assert.Equal(command.Parameters[2], restored.Parameters[2]);
	}

	[Fact]
	public void CutsceneManager_WaitCommandDelaysExecution() {
		var manager = new CutsceneManager();
		var showTextExecuted = false;

		manager.CommandExecuted += (_, e) => {
			if (e.Command.Opcode == CutsceneOpcode.ShowText)
				showTextExecuted = true;
		};

		var cutscene = new Cutscene {
			Id = 0x0050,
			Commands = [
				new CutsceneCommand { Opcode = CutsceneOpcode.Wait, Parameters = [5, 0, 0, 0] },
				new CutsceneCommand { Opcode = CutsceneOpcode.ShowText, Parameters = [1, 0, 0, 0] }
			]
		};

		manager.RegisterCutscene(cutscene);
		manager.PlayCutscene(0x0050);

		// During wait, ShowText should NOT have executed
		manager.Update();
		manager.Update();
		manager.Update();

		// ShowText command should not execute during wait period
		Assert.False(showTextExecuted);
	}

	[Fact]
	public void Cutscene_TotalDuration_CalculatesCorrectly() {
		var cutscene = new Cutscene {
			Id = 0x0060,
			Commands = [
				new CutsceneCommand { Opcode = CutsceneOpcode.Wait, Parameters = [30, 0, 0, 0], Duration = 30 },
				new CutsceneCommand { Opcode = CutsceneOpcode.FadeOut, Parameters = [60, 0, 0, 0], Duration = 60 },
				new CutsceneCommand { Opcode = CutsceneOpcode.End, Duration = 0 }
			]
		};

		Assert.Equal(90, cutscene.TotalDuration);
	}
}
