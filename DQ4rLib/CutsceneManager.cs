using DQ4rLib.Models;

namespace DQ4rLib;

/// <summary>
/// Manages cutscene playback and chapter transition sequences.
/// Handles the cinematic presentation of story events.
/// </summary>
public class CutsceneManager {
	/// <summary>All registered cutscenes.</summary>
	private readonly Dictionary<ushort, Cutscene> _cutscenes = new();

	/// <summary>Currently playing cutscene.</summary>
	public Cutscene? CurrentCutscene { get; private set; }

	/// <summary>Current command index.</summary>
	public int CurrentCommandIndex { get; private set; }

	/// <summary>Frames remaining on current command.</summary>
	public int CommandFramesRemaining { get; private set; }

	/// <summary>Whether a cutscene is currently playing.</summary>
	public bool IsPlaying => CurrentCutscene != null;

	/// <summary>Whether cutscene is paused.</summary>
	public bool IsPaused { get; private set; }

	/// <summary>Call stack for nested cutscenes.</summary>
	private readonly Stack<(Cutscene cutscene, int index)> _callStack = new();

	/// <summary>Event raised when cutscene command executes.</summary>
	public event EventHandler<CutsceneCommandEventArgs>? CommandExecuted;

	/// <summary>Event raised when cutscene completes.</summary>
	public event EventHandler<CutsceneEventArgs>? CutsceneCompleted;

	/// <summary>Event raised when cutscene starts.</summary>
	public event EventHandler<CutsceneEventArgs>? CutsceneStarted;

	/// <summary>
	/// Chapter transition cutscene definitions.
	/// </summary>
	public static readonly Dictionary<int, Cutscene> ChapterIntroCutscenes = new() {
		[1] = CreateChapter1Intro(),
		[2] = CreateChapter2Intro(),
		[3] = CreateChapter3Intro(),
		[4] = CreateChapter4Intro(),
		[5] = CreateChapter5Intro()
	};

	/// <summary>
	/// Register a cutscene.
	/// </summary>
	public void RegisterCutscene(Cutscene cutscene) {
		_cutscenes[cutscene.Id] = cutscene;
	}

	/// <summary>
	/// Get cutscene by ID.
	/// </summary>
	public Cutscene? GetCutscene(ushort id) {
		return _cutscenes.TryGetValue(id, out var cutscene) ? cutscene : null;
	}

	/// <summary>
	/// Start playing a cutscene by ID.
	/// </summary>
	public bool PlayCutscene(ushort id) {
		if (!_cutscenes.TryGetValue(id, out var cutscene))
			return false;

		return PlayCutscene(cutscene);
	}

	/// <summary>
	/// Start playing a cutscene.
	/// </summary>
	public bool PlayCutscene(Cutscene cutscene) {
		CurrentCutscene = cutscene;
		CurrentCommandIndex = 0;
		CommandFramesRemaining = 0;
		IsPaused = false;
		_callStack.Clear();

		CutsceneStarted?.Invoke(this, new CutsceneEventArgs { Cutscene = cutscene });

		// Execute first command
		ExecuteCurrentCommand();

		return true;
	}

	/// <summary>
	/// Play chapter intro cutscene.
	/// </summary>
	public bool PlayChapterIntro(int chapterNumber) {
		if (!ChapterIntroCutscenes.TryGetValue(chapterNumber, out var cutscene))
			return false;

		return PlayCutscene(cutscene);
	}

	/// <summary>
	/// Update cutscene playback (call once per frame).
	/// </summary>
	public void Update() {
		if (CurrentCutscene == null || IsPaused)
			return;

		// Decrement timer
		if (CommandFramesRemaining > 0) {
			CommandFramesRemaining--;
			return;
		}

		// Move to next command
		CurrentCommandIndex++;

		if (CurrentCommandIndex >= CurrentCutscene.Commands.Count) {
			// Check call stack
			if (_callStack.Count > 0) {
				var (parent, index) = _callStack.Pop();
				CurrentCutscene = parent;
				CurrentCommandIndex = index + 1;
			} else {
				// End cutscene
				EndCutscene();
				return;
			}
		}

		ExecuteCurrentCommand();
	}

	/// <summary>
	/// Skip current cutscene if allowed.
	/// </summary>
	public bool Skip() {
		if (CurrentCutscene == null)
			return false;

		if (!CurrentCutscene.Skippable)
			return false;

		EndCutscene();
		return true;
	}

	/// <summary>
	/// Pause cutscene playback.
	/// </summary>
	public void Pause() {
		IsPaused = true;
	}

	/// <summary>
	/// Resume cutscene playback.
	/// </summary>
	public void Resume() {
		IsPaused = false;
	}

	private void ExecuteCurrentCommand() {
		if (CurrentCutscene == null || CurrentCommandIndex >= CurrentCutscene.Commands.Count)
			return;

		var command = CurrentCutscene.Commands[CurrentCommandIndex];

		// Handle special opcodes
		switch (command.Opcode) {
			case CutsceneOpcode.Jump:
				CurrentCommandIndex = command.Parameters[0] - 1; // -1 because Update() will increment
				break;

			case CutsceneOpcode.Call:
				if (_cutscenes.TryGetValue((ushort)command.Parameters[0], out var subcutscene)) {
					_callStack.Push((CurrentCutscene, CurrentCommandIndex));
					CurrentCutscene = subcutscene;
					CurrentCommandIndex = -1; // Will be incremented to 0
				}
				break;

			case CutsceneOpcode.Return:
				if (_callStack.Count > 0) {
					var (parent, index) = _callStack.Pop();
					CurrentCutscene = parent;
					CurrentCommandIndex = index;
				}
				break;

			case CutsceneOpcode.Wait:
				// Wait command uses Parameters[0] as the wait duration
				CommandFramesRemaining = command.Parameters[0];
				CommandExecuted?.Invoke(this, new CutsceneCommandEventArgs {
					Command = command,
					Cutscene = CurrentCutscene
				});
				return; // Don't overwrite CommandFramesRemaining

			case CutsceneOpcode.End:
				EndCutscene();
				return;

			default:
				// Raise event for other commands
				CommandExecuted?.Invoke(this, new CutsceneCommandEventArgs {
					Command = command,
					Cutscene = CurrentCutscene
				});
				break;
		}

		CommandFramesRemaining = command.Duration;
	}

	private void EndCutscene() {
		var cutscene = CurrentCutscene;
		CurrentCutscene = null;
		CurrentCommandIndex = 0;
		CommandFramesRemaining = 0;
		_callStack.Clear();

		if (cutscene != null) {
			CutsceneCompleted?.Invoke(this, new CutsceneEventArgs { Cutscene = cutscene });
		}
	}

	/// <summary>
	/// Export all chapter intro cutscenes to binary.
	/// </summary>
	public static byte[] ExportChapterIntros() {
		using var ms = new MemoryStream();
		using var bw = new BinaryWriter(ms);

		bw.Write((byte)ChapterIntroCutscenes.Count);

		foreach (var kvp in ChapterIntroCutscenes.OrderBy(k => k.Key)) {
			byte[] data = kvp.Value.ToSnesBytes();
			bw.Write((ushort)data.Length);
			bw.Write(data);
		}

		return ms.ToArray();
	}

	#region Chapter Intro Cutscene Definitions

	private static Cutscene CreateChapter1Intro() {
		return new Cutscene {
			Id = 0x0100,
			Name = "Chapter 1 Intro - The Royal Soldiers",
			ChapterId = 0,
			Type = CutsceneType.ChapterIntro,
			MusicId = 0x10,
			Commands = [
				CutsceneCommand.Fade(fadeIn: false, 30),
				CutsceneCommand.PlayMusic(0x10),
				new() { Opcode = CutsceneOpcode.LoadBackground, Parameters = [0x01, 0, 0, 0] }, // Burland castle BG
				CutsceneCommand.Wait(30),
				CutsceneCommand.Fade(fadeIn: true, 60),
				new() { Opcode = CutsceneOpcode.ShowTitle, Duration = 180, Parameters = [1, 0, 0, 0] }, // "Chapter 1"
				CutsceneCommand.Wait(120),
				CutsceneCommand.ShowText(0x0001, 180), // "The Royal Soldiers"
				CutsceneCommand.Wait(60),
				CutsceneCommand.ShowText(0x0002, 240), // Intro narration
				CutsceneCommand.Wait(60),
				CutsceneCommand.Fade(fadeIn: false, 60),
				CutsceneCommand.ChangeMap(0x0002, 0x08, 0x0a), // Burland Castle
				CutsceneCommand.Fade(fadeIn: true, 60),
				new() { Opcode = CutsceneOpcode.End }
			]
		};
	}

	private static Cutscene CreateChapter2Intro() {
		return new Cutscene {
			Id = 0x0200,
			Name = "Chapter 2 Intro - Princess Alena's Adventure",
			ChapterId = 1,
			Type = CutsceneType.ChapterIntro,
			MusicId = 0x11,
			Commands = [
				CutsceneCommand.Fade(fadeIn: false, 30),
				CutsceneCommand.PlayMusic(0x11),
				new() { Opcode = CutsceneOpcode.LoadBackground, Parameters = [0x02, 0, 0, 0] }, // Santeem castle BG
				CutsceneCommand.Wait(30),
				CutsceneCommand.Fade(fadeIn: true, 60),
				new() { Opcode = CutsceneOpcode.ShowTitle, Duration = 180, Parameters = [2, 0, 0, 0] },
				CutsceneCommand.Wait(120),
				CutsceneCommand.ShowText(0x0010, 180), // "Princess Alena's Adventure"
				CutsceneCommand.Wait(60),
				CutsceneCommand.ShowText(0x0011, 240), // Intro narration
				CutsceneCommand.Wait(60),
				new() { Opcode = CutsceneOpcode.ShowPortrait, Parameters = [0x07, 0, 0, 0] }, // Alena portrait
				CutsceneCommand.Wait(120),
				new() { Opcode = CutsceneOpcode.HidePortrait },
				CutsceneCommand.Fade(fadeIn: false, 60),
				CutsceneCommand.ChangeMap(0x0001, 0x10, 0x08),
				CutsceneCommand.Fade(fadeIn: true, 60),
				new() { Opcode = CutsceneOpcode.End }
			]
		};
	}

	private static Cutscene CreateChapter3Intro() {
		return new Cutscene {
			Id = 0x0300,
			Name = "Chapter 3 Intro - Taloon the Arms Merchant",
			ChapterId = 2,
			Type = CutsceneType.ChapterIntro,
			MusicId = 0x12,
			Commands = [
				CutsceneCommand.Fade(fadeIn: false, 30),
				CutsceneCommand.PlayMusic(0x12),
				new() { Opcode = CutsceneOpcode.LoadBackground, Parameters = [0x03, 0, 0, 0] }, // Shop BG
				CutsceneCommand.Wait(30),
				CutsceneCommand.Fade(fadeIn: true, 60),
				new() { Opcode = CutsceneOpcode.ShowTitle, Duration = 180, Parameters = [3, 0, 0, 0] },
				CutsceneCommand.Wait(120),
				CutsceneCommand.ShowText(0x0020, 180), // "Taloon the Arms Merchant"
				CutsceneCommand.Wait(60),
				CutsceneCommand.ShowText(0x0021, 300), // Intro narration - Taloon's dream
				CutsceneCommand.Wait(60),
				new() { Opcode = CutsceneOpcode.ShowPortrait, Parameters = [0x05, 0, 0, 0] }, // Taloon portrait
				CutsceneCommand.Wait(120),
				new() { Opcode = CutsceneOpcode.HidePortrait },
				CutsceneCommand.Fade(fadeIn: false, 60),
				CutsceneCommand.ChangeMap(0x0016, 0x0c, 0x0e),
				CutsceneCommand.Fade(fadeIn: true, 60),
				new() { Opcode = CutsceneOpcode.End }
			]
		};
	}

	private static Cutscene CreateChapter4Intro() {
		return new Cutscene {
			Id = 0x0400,
			Name = "Chapter 4 Intro - The Sisters of Monbaraba",
			ChapterId = 3,
			Type = CutsceneType.ChapterIntro,
			MusicId = 0x13,
			Commands = [
				CutsceneCommand.Fade(fadeIn: false, 30),
				CutsceneCommand.PlayMusic(0x13),
				new() { Opcode = CutsceneOpcode.LoadBackground, Parameters = [0x04, 0, 0, 0] }, // Theatre BG
				CutsceneCommand.Wait(30),
				CutsceneCommand.Fade(fadeIn: true, 60),
				new() { Opcode = CutsceneOpcode.ShowTitle, Duration = 180, Parameters = [4, 0, 0, 0] },
				CutsceneCommand.Wait(120),
				CutsceneCommand.ShowText(0x0030, 180), // "The Sisters of Monbaraba"
				CutsceneCommand.Wait(60),
				CutsceneCommand.ShowText(0x0031, 300), // Intro narration - revenge for father
				CutsceneCommand.Wait(60),
				new() { Opcode = CutsceneOpcode.ShowPortrait, Parameters = [0x02, 0, 0, 0] }, // Nara portrait
				CutsceneCommand.Wait(90),
				new() { Opcode = CutsceneOpcode.ShowPortrait, Parameters = [0x03, 0, 0, 0] }, // Mara portrait
				CutsceneCommand.Wait(90),
				new() { Opcode = CutsceneOpcode.HidePortrait },
				CutsceneCommand.Fade(fadeIn: false, 60),
				CutsceneCommand.ChangeMap(0x0015, 0x08, 0x0a),
				CutsceneCommand.Fade(fadeIn: true, 60),
				new() { Opcode = CutsceneOpcode.End }
			]
		};
	}

	private static Cutscene CreateChapter5Intro() {
		return new Cutscene {
			Id = 0x0500,
			Name = "Chapter 5 Intro - The Chosen Ones",
			ChapterId = 4,
			Type = CutsceneType.ChapterIntro,
			MusicId = 0x14,
			Skippable = false, // Important story moment
			Commands = [
				CutsceneCommand.Fade(fadeIn: false, 30),
				CutsceneCommand.PlayMusic(0x14),
				new() { Opcode = CutsceneOpcode.LoadBackground, Parameters = [0x05, 0, 0, 0] }, // Village BG
				CutsceneCommand.Wait(30),
				CutsceneCommand.Fade(fadeIn: true, 60),
				new() { Opcode = CutsceneOpcode.ShowTitle, Duration = 240, Parameters = [5, 0, 0, 0] },
				CutsceneCommand.Wait(180),
				CutsceneCommand.ShowText(0x0040, 180), // "The Chosen Ones"
				CutsceneCommand.Wait(60),
				CutsceneCommand.ShowText(0x0041, 360), // Intro narration - Hero's destiny
				CutsceneCommand.Wait(60),
				CutsceneCommand.ShowText(0x0042, 300), // "The fate of the world..."
				CutsceneCommand.Wait(60),
				new() { Opcode = CutsceneOpcode.ShowPortrait, Parameters = [0x00, 0, 0, 0] }, // Hero portrait
				CutsceneCommand.Wait(150),
				new() { Opcode = CutsceneOpcode.HidePortrait },
				CutsceneCommand.Wait(30),
				// Show all previous heroes briefly
				CutsceneCommand.ShowText(0x0043, 180), // "The chosen ones shall unite..."
				CutsceneCommand.Wait(60),
				CutsceneCommand.Fade(fadeIn: false, 90),
				CutsceneCommand.ChangeMap(0x0014, 0x08, 0x0c),
				CutsceneCommand.Fade(fadeIn: true, 60),
				new() { Opcode = CutsceneOpcode.End }
			]
		};
	}

	#endregion
}

/// <summary>
/// Event args for cutscene events.
/// </summary>
public class CutsceneEventArgs : EventArgs {
	/// <summary>The cutscene.</summary>
	public required Cutscene Cutscene { get; init; }
}

/// <summary>
/// Event args for cutscene command execution.
/// </summary>
public class CutsceneCommandEventArgs : EventArgs {
	/// <summary>The command being executed.</summary>
	public required CutsceneCommand Command { get; init; }

	/// <summary>The cutscene containing the command.</summary>
	public required Cutscene Cutscene { get; init; }
}
