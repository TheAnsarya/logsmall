using DQ4rLib.Models;

namespace DQ4rLib;

/// <summary>
/// Manages DQ4r chapter system - transitions, state, and chapter-specific logic.
/// Dragon Quest IV's unique 5-chapter narrative structure where each chapter
/// follows different protagonists before uniting in Chapter 5.
/// </summary>
public class ChapterManager {
	/// <summary>
	/// All chapter definitions.
	/// Event flags use IDs 1-5 for chapter completion (flag ID = chapter number).
	/// </summary>
	public static readonly Chapter[] Chapters = [
		new() {
			Id = 0x00,
			Title = "Chapter 1: The Royal Soldiers",
			Description = "Ragnar McRyan investigates missing children",
			ProtagonistId = 0x06,
			PartyMemberIds = [0x06],
			CompanionIds = [0xc5],  // Healie
			StartMapId = 0x0002,    // Burland Castle
			StartX = 0x08,
			StartY = 0x0a,
			PrerequisiteFlags = [],
			CompletionFlags = [1],  // Flag 1 = Ch1 complete
			CompletionEventId = 0x0100,
			WagonEnabled = false,
			TacticsEnabled = false,
			Mechanics = ChapterMechanics.SoloProtagonist | ChapterMechanics.NpcCompanion,
			IntroMusicId = 0x10,
			OverworldMusicId = 0x08
		},
		new() {
			Id = 0x01,
			Title = "Chapter 2: Princess Alena's Adventure",
			Description = "Princess Alena escapes to prove herself",
			ProtagonistId = 0x07,
			PartyMemberIds = [0x07, 0x01, 0x04],  // Alena, Cristo, Brey
			CompanionIds = [],
			StartMapId = 0x0001,    // Santeem Castle
			StartX = 0x10,
			StartY = 0x08,
			PrerequisiteFlags = [1],  // Requires Ch1 complete
			CompletionFlags = [2],    // Flag 2 = Ch2 complete
			CompletionEventId = 0x0200,
			WagonEnabled = false,
			TacticsEnabled = false,
			Mechanics = ChapterMechanics.AiPartyMembers,
			IntroMusicId = 0x11,
			OverworldMusicId = 0x09
		},
		new() {
			Id = 0x02,
			Title = "Chapter 3: Taloon the Arms Merchant",
			Description = "Taloon pursues his dream of opening a shop",
			ProtagonistId = 0x05,
			PartyMemberIds = [0x05],
			CompanionIds = [0xc7, 0xc8],  // Laurent, Strom
			StartMapId = 0x0016,    // Lakanaba
			StartX = 0x0c,
			StartY = 0x0e,
			PrerequisiteFlags = [2],  // Requires Ch2 complete
			CompletionFlags = [3],    // Flag 3 = Ch3 complete
			CompletionEventId = 0x0300,
			WagonEnabled = false,
			TacticsEnabled = false,
			Mechanics = ChapterMechanics.SoloProtagonist | ChapterMechanics.MerchantAbilities,
			IntroMusicId = 0x12,
			OverworldMusicId = 0x0a
		},
		new() {
			Id = 0x03,
			Title = "Chapter 4: The Sisters of Monbaraba",
			Description = "Nara and Mara seek revenge for their father",
			ProtagonistId = 0x02,
			PartyMemberIds = [0x02, 0x03],  // Nara, Mara
			CompanionIds = [0xc6],  // Orin
			StartMapId = 0x0015,    // Monbaraba
			StartX = 0x08,
			StartY = 0x0a,
			PrerequisiteFlags = [3],  // Requires Ch3 complete
			CompletionFlags = [4],    // Flag 4 = Ch4 complete
			CompletionEventId = 0x0400,
			WagonEnabled = false,
			TacticsEnabled = false,
			Mechanics = ChapterMechanics.AiPartyMembers | ChapterMechanics.MagicFocus,
			IntroMusicId = 0x13,
			OverworldMusicId = 0x0b
		},
		new() {
			Id = 0x04,
			Title = "Chapter 5: The Chosen Ones",
			Description = "The Hero unites all chosen ones to save the world",
			ProtagonistId = 0x00,
			PartyMemberIds = [0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07],
			CompanionIds = [0xc9, 0xca, 0xcb, 0xcc],  // Hector, Panon, Lucia, Doran
			StartMapId = 0x0014,    // Hero's Village
			StartX = 0x08,
			StartY = 0x0c,
			PrerequisiteFlags = [4],  // Requires Ch4 complete
			CompletionFlags = [5],    // Flag 5 = game complete
			CompletionEventId = 0x0500,
			WagonEnabled = true,
			TacticsEnabled = true,
			Mechanics = ChapterMechanics.WagonParty | ChapterMechanics.TacticsMenu | ChapterMechanics.FullControl,
			IntroMusicId = 0x14,
			OverworldMusicId = 0x0c
		}
	];

	/// <summary>
	/// Current chapter state.
	/// </summary>
	public ChapterState State { get; private set; }

	/// <summary>
	/// Current chapter definition.
	/// </summary>
	public Chapter CurrentChapter => Chapters[State.CurrentChapterId];

	/// <summary>
	/// Event raised when chapter transition begins.
	/// </summary>
	public event EventHandler<ChapterTransitionEventArgs>? ChapterTransitionStarted;

	/// <summary>
	/// Event raised when chapter transition completes.
	/// </summary>
	public event EventHandler<ChapterTransitionEventArgs>? ChapterTransitionCompleted;

	/// <summary>
	/// Create a new ChapterManager with initial state.
	/// </summary>
	public ChapterManager() {
		State = new ChapterState();
	}

	/// <summary>
	/// Create ChapterManager with existing state.
	/// </summary>
	public ChapterManager(ChapterState state) {
		State = state;
	}

	/// <summary>
	/// Initialize a new game at Chapter 1.
	/// </summary>
	public void StartNewGame() {
		State = new ChapterState {
			CurrentChapterId = 0,
			ChaptersCompleted = 0,
			CurrentMapId = Chapters[0].StartMapId,
			CurrentX = Chapters[0].StartX,
			CurrentY = Chapters[0].StartY,
			Gold = 0,
			StepCount = 0,
			CurrentTactic = BattleTactic.ShowNoMercy
		};

		// Set initial party
		InitializePartyForChapter(0);
	}

	/// <summary>
	/// Get chapter by ID.
	/// </summary>
	public static Chapter? GetChapter(byte id) =>
		id < Chapters.Length ? Chapters[id] : null;

	/// <summary>
	/// Get chapter by number (1-5).
	/// </summary>
	public static Chapter? GetChapterByNumber(int number) =>
		number >= 1 && number <= 5 ? Chapters[number - 1] : null;

	/// <summary>
	/// Check if chapter can be transitioned to.
	/// </summary>
	public bool CanTransitionTo(int chapterId) {
		if (chapterId < 0 || chapterId >= Chapters.Length)
			return false;

		var chapter = Chapters[chapterId];

		// Check prerequisites
		foreach (var flag in chapter.PrerequisiteFlags) {
			if (!State.GetEventFlag(flag))
				return false;
		}

		return true;
	}

	/// <summary>
	/// Transition to a new chapter.
	/// </summary>
	public bool TransitionToChapter(int chapterId) {
		if (!CanTransitionTo(chapterId))
			return false;

		var previousChapter = State.CurrentChapterId;
		var targetChapter = Chapters[chapterId];

		// Raise transition started event
		ChapterTransitionStarted?.Invoke(this, new ChapterTransitionEventArgs {
			FromChapterId = previousChapter,
			ToChapterId = (byte)chapterId
		});

		// Mark previous chapter as complete
		if (previousChapter < 5) {
			State.SetChapterCompleted(previousChapter + 1);
		}

		// Update state
		State.CurrentChapterId = (byte)chapterId;
		State.CurrentMapId = targetChapter.StartMapId;
		State.CurrentX = targetChapter.StartX;
		State.CurrentY = targetChapter.StartY;

		// Reset certain state for new chapter
		if (chapterId != 4) {  // Chapter 5 keeps gold
			State.Gold = 0;
		}
		State.StepCount = 0;

		// Initialize party
		InitializePartyForChapter(chapterId);

		// Raise transition completed event
		ChapterTransitionCompleted?.Invoke(this, new ChapterTransitionEventArgs {
			FromChapterId = previousChapter,
			ToChapterId = (byte)chapterId
		});

		return true;
	}

	/// <summary>
	/// Initialize party members for a chapter.
	/// </summary>
	private void InitializePartyForChapter(int chapterId) {
		var chapter = Chapters[chapterId];
		State.ActiveParty = new byte[4];
		State.WagonParty = new byte[4];

		// Set active party (up to 4)
		for (int i = 0; i < Math.Min(4, chapter.PartyMemberIds.Length); i++) {
			State.ActiveParty[i] = chapter.PartyMemberIds[i];
		}

		// Chapter 5: Initialize wagon with remaining party
		if (chapter.WagonEnabled && chapter.PartyMemberIds.Length > 4) {
			for (int i = 4; i < Math.Min(8, chapter.PartyMemberIds.Length); i++) {
				State.WagonParty[i - 4] = chapter.PartyMemberIds[i];
			}
		}

		// Set tactics for Chapter 5
		if (chapter.TacticsEnabled) {
			State.CurrentTactic = BattleTactic.ShowNoMercy;
		}
	}

	/// <summary>
	/// Complete the current chapter.
	/// </summary>
	public void CompleteCurrentChapter() {
		var chapter = CurrentChapter;

		// Set completion flags
		foreach (var flag in chapter.CompletionFlags) {
			State.SetEventFlag(flag);
		}

		// Mark chapter complete
		State.SetChapterCompleted(chapter.Number);

		// Auto-transition to next chapter if not Chapter 5
		if (chapter.Id < 4) {
			TransitionToChapter(chapter.Id + 1);
		}
	}

	/// <summary>
	/// Check if current chapter mechanics include a flag.
	/// </summary>
	public bool HasMechanic(ChapterMechanics mechanic) =>
		(CurrentChapter.Mechanics & mechanic) != 0;

	/// <summary>
	/// Check if wagon is available.
	/// </summary>
	public bool IsWagonAvailable => CurrentChapter.WagonEnabled;

	/// <summary>
	/// Check if tactics menu is available.
	/// </summary>
	public bool IsTacticsAvailable => CurrentChapter.TacticsEnabled;

	/// <summary>
	/// Swap party member between active and wagon (Chapter 5 only).
	/// </summary>
	public bool SwapPartyMember(int activeSlot, int wagonSlot) {
		if (!IsWagonAvailable)
			return false;

		if (activeSlot < 0 || activeSlot >= 4 || wagonSlot < 0 || wagonSlot >= 4)
			return false;

		(State.ActiveParty[activeSlot], State.WagonParty[wagonSlot]) =
			(State.WagonParty[wagonSlot], State.ActiveParty[activeSlot]);

		return true;
	}

	/// <summary>
	/// Set current AI battle tactic.
	/// </summary>
	public bool SetTactic(BattleTactic tactic) {
		if (!IsTacticsAvailable)
			return false;

		State.CurrentTactic = tactic;
		return true;
	}

	/// <summary>
	/// Export all chapter data to SNES binary format.
	/// Returns combined chapter data for ROM inclusion.
	/// </summary>
	public static byte[] ExportAllChapterData() {
		using var ms = new MemoryStream();

		// Write chapter count
		ms.WriteByte((byte)Chapters.Length);

		// Write each chapter
		foreach (var chapter in Chapters) {
			byte[] chapterData = chapter.ToSnesBytes();
			ms.WriteByte((byte)chapterData.Length);
			ms.Write(chapterData, 0, chapterData.Length);
		}

		return ms.ToArray();
	}

	/// <summary>
	/// Generate SNES assembly include file for chapter data.
	/// </summary>
	public static string GenerateAssemblyInclude() {
		var sb = new System.Text.StringBuilder();

		sb.AppendLine(";==============================================================================");
		sb.AppendLine("; DQ4r Chapter Data - Auto-generated by ChapterManager");
		sb.AppendLine(";==============================================================================");
		sb.AppendLine();
		sb.AppendLine(".DEFINE CHAPTER_COUNT 5");
		sb.AppendLine();

		// Chapter IDs
		sb.AppendLine("; Chapter IDs");
		sb.AppendLine(".DEFINE CHAPTER_1_RAGNAR    $00");
		sb.AppendLine(".DEFINE CHAPTER_2_ALENA     $01");
		sb.AppendLine(".DEFINE CHAPTER_3_TALOON    $02");
		sb.AppendLine(".DEFINE CHAPTER_4_SISTERS   $03");
		sb.AppendLine(".DEFINE CHAPTER_5_HERO      $04");
		sb.AppendLine();

		// Mechanics flags
		sb.AppendLine("; Chapter Mechanics Flags");
		sb.AppendLine(".DEFINE MECH_AI_PARTY       $01");
		sb.AppendLine(".DEFINE MECH_MERCHANT       $02");
		sb.AppendLine(".DEFINE MECH_SOLO           $04");
		sb.AppendLine(".DEFINE MECH_MAGIC          $08");
		sb.AppendLine(".DEFINE MECH_WAGON          $10");
		sb.AppendLine(".DEFINE MECH_TACTICS        $20");
		sb.AppendLine(".DEFINE MECH_NPC_COMPANION  $40");
		sb.AppendLine(".DEFINE MECH_FULL_CONTROL   $80");
		sb.AppendLine();

		// Battle tactics
		sb.AppendLine("; Battle Tactics");
		sb.AppendLine(".DEFINE TACTIC_SHOW_NO_MERCY  $00");
		sb.AppendLine(".DEFINE TACTIC_WATCH_MY_MP    $01");
		sb.AppendLine(".DEFINE TACTIC_GO_ALL_OUT     $02");
		sb.AppendLine(".DEFINE TACTIC_DONT_USE_MAGIC $03");
		sb.AppendLine(".DEFINE TACTIC_TRY_OUT        $04");
		sb.AppendLine(".DEFINE TACTIC_FOLLOW_ORDERS  $05");
		sb.AppendLine();

		// Chapter data table
		sb.AppendLine("; Chapter Data Table");
		sb.AppendLine("ChapterDataTable:");

		for (int i = 0; i < Chapters.Length; i++) {
			var ch = Chapters[i];
			sb.AppendLine($"    .DW Chapter{i + 1}Data");
		}
		sb.AppendLine();

		// Individual chapter data
		for (int i = 0; i < Chapters.Length; i++) {
			var ch = Chapters[i];
			sb.AppendLine($"Chapter{i + 1}Data:");
			sb.AppendLine($"    .DB ${ch.Id:x2}                ; Chapter ID");
			sb.AppendLine($"    .DB ${ch.ProtagonistId:x2}                ; Protagonist ID");
			sb.AppendLine($"    .DB ${ch.PartyMemberIds.Length:x2}                ; Party count");
			sb.AppendLine($"    .DB ${ch.CompanionIds.Length:x2}                ; Companion count");
			sb.AppendLine($"    .DW ${ch.StartMapId:x4}            ; Start map");
			sb.AppendLine($"    .DB ${ch.StartX:x2}                ; Start X");
			sb.AppendLine($"    .DB ${ch.StartY:x2}                ; Start Y");
			sb.AppendLine($"    .DB ${(byte)ch.Mechanics:x2}                ; Mechanics flags");
			sb.AppendLine($"    .DB ${ch.MaxPartySize:x2}                ; Max party size");
			sb.AppendLine($"    .DB ${ch.IntroMusicId:x2}                ; Intro music");
			sb.AppendLine($"    .DB ${ch.OverworldMusicId:x2}                ; Overworld music");
			sb.AppendLine($"    .DW ${ch.CompletionEventId:x4}            ; Completion event");
			sb.AppendLine($"    .DB ${(ch.WagonEnabled ? 1 : 0):x2}                ; Wagon enabled");
			sb.AppendLine($"    .DB ${(ch.TacticsEnabled ? 1 : 0):x2}                ; Tactics enabled");

			// Party member IDs
			sb.Append("    .DB ");
			for (int j = 0; j < 8; j++) {
				if (j > 0) sb.Append(", ");
				sb.Append(j < ch.PartyMemberIds.Length ? $"${ch.PartyMemberIds[j]:x2}" : "$ff");
			}
			sb.AppendLine("  ; Party IDs");

			// Companion IDs
			sb.Append("    .DB ");
			for (int j = 0; j < 4; j++) {
				if (j > 0) sb.Append(", ");
				sb.Append(j < ch.CompanionIds.Length ? $"${ch.CompanionIds[j]:x2}" : "$ff");
			}
			sb.AppendLine("      ; Companion IDs");
			sb.AppendLine();
		}

		return sb.ToString();
	}
}

/// <summary>
/// Event args for chapter transitions.
/// </summary>
public class ChapterTransitionEventArgs : EventArgs {
	/// <summary>
	/// Chapter transitioning from.
	/// </summary>
	public byte FromChapterId { get; set; }

	/// <summary>
	/// Chapter transitioning to.
	/// </summary>
	public byte ToChapterId { get; set; }

	/// <summary>
	/// From chapter number (1-5).
	/// </summary>
	public int FromChapter => FromChapterId + 1;

	/// <summary>
	/// To chapter number (1-5).
	/// </summary>
	public int ToChapter => ToChapterId + 1;
}
