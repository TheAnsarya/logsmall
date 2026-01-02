using DQ4rLib;
using DQ4rLib.Models;
using Xunit;

namespace DQ4rLib.Tests;

/// <summary>
/// Unit tests for ChapterManager and chapter system.
/// </summary>
public class ChapterManagerTests {
	[Fact]
	public void Chapters_HasFiveChapters() {
		Assert.Equal(5, ChapterManager.Chapters.Length);
	}

	[Theory]
	[InlineData(0, "Chapter 1: The Royal Soldiers", 0x06)]
	[InlineData(1, "Chapter 2: Princess Alena's Adventure", 0x07)]
	[InlineData(2, "Chapter 3: Taloon the Arms Merchant", 0x05)]
	[InlineData(3, "Chapter 4: The Sisters of Monbaraba", 0x02)]
	[InlineData(4, "Chapter 5: The Chosen Ones", 0x00)]
	public void Chapters_HaveCorrectTitlesAndProtagonists(int id, string title, byte protagonistId) {
		var chapter = ChapterManager.Chapters[id];
		Assert.Equal(title, chapter.Title);
		Assert.Equal(protagonistId, chapter.ProtagonistId);
	}

	[Fact]
	public void Chapter1_HasCorrectMechanics() {
		var chapter = ChapterManager.Chapters[0];
		Assert.True((chapter.Mechanics & ChapterMechanics.SoloProtagonist) != 0);
		Assert.True((chapter.Mechanics & ChapterMechanics.NpcCompanion) != 0);
		Assert.False(chapter.WagonEnabled);
		Assert.False(chapter.TacticsEnabled);
	}

	[Fact]
	public void Chapter5_HasWagonAndTactics() {
		var chapter = ChapterManager.Chapters[4];
		Assert.True(chapter.WagonEnabled);
		Assert.True(chapter.TacticsEnabled);
		Assert.True((chapter.Mechanics & ChapterMechanics.WagonParty) != 0);
		Assert.True((chapter.Mechanics & ChapterMechanics.TacticsMenu) != 0);
		Assert.True((chapter.Mechanics & ChapterMechanics.FullControl) != 0);
	}

	[Fact]
	public void Chapter5_HasAllEightPartyMembers() {
		var chapter = ChapterManager.Chapters[4];
		Assert.Equal(8, chapter.PartyMemberIds.Length);
		Assert.Contains((byte)0x00, chapter.PartyMemberIds); // Hero
		Assert.Contains((byte)0x07, chapter.PartyMemberIds); // Alena
	}

	[Fact]
	public void StartNewGame_InitializesToChapter1() {
		var manager = new ChapterManager();
		manager.StartNewGame();

		Assert.Equal(0, manager.State.CurrentChapterId);
		Assert.Equal(1, manager.State.CurrentChapter);
		Assert.Equal(0x0002, manager.State.CurrentMapId); // Burland Castle
	}

	[Fact]
	public void StartNewGame_InitializesPartyCorrectly() {
		var manager = new ChapterManager();
		manager.StartNewGame();

		// Chapter 1 has only Ragnar (0x06)
		Assert.Equal(0x06, manager.State.ActiveParty[0]);
	}

	[Fact]
	public void GetChapter_ReturnsCorrectChapter() {
		var chapter = ChapterManager.GetChapter(2);
		Assert.NotNull(chapter);
		Assert.Equal("Chapter 3: Taloon the Arms Merchant", chapter.Title);
	}

	[Fact]
	public void GetChapter_ReturnsNullForInvalidId() {
		var chapter = ChapterManager.GetChapter(10);
		Assert.Null(chapter);
	}

	[Fact]
	public void GetChapterByNumber_ReturnsCorrectChapter() {
		var chapter = ChapterManager.GetChapterByNumber(4);
		Assert.NotNull(chapter);
		Assert.Equal("Chapter 4: The Sisters of Monbaraba", chapter.Title);
	}

	[Fact]
	public void TransitionToChapter_UpdatesState() {
		var manager = new ChapterManager();
		manager.StartNewGame();

		// Set prerequisite flag for Chapter 2 (flag 1 = Ch1 complete)
		manager.State.SetEventFlag(1);

		var result = manager.TransitionToChapter(1);

		Assert.True(result);
		Assert.Equal(1, manager.State.CurrentChapterId);
		Assert.Equal(0x0001, manager.State.CurrentMapId); // Santeem Castle
	}

	[Fact]
	public void TransitionToChapter_FailsWithoutPrerequisites() {
		var manager = new ChapterManager();
		manager.StartNewGame();

		// Don't set prerequisite flags
		var result = manager.TransitionToChapter(1);

		Assert.False(result);
		Assert.Equal(0, manager.State.CurrentChapterId);
	}

	[Fact]
	public void TransitionToChapter_MarksChapterComplete() {
		var manager = new ChapterManager();
		manager.StartNewGame();

		manager.State.SetEventFlag(1);  // Ch1 complete flag
		manager.TransitionToChapter(1);

		Assert.True(manager.State.IsChapterCompleted(1));
	}

	[Fact]
	public void TransitionToChapter5_InitializesWagonParty() {
		var manager = new ChapterManager();
		manager.StartNewGame();

		// Set all prerequisite flags (1-4 for chapters 1-4 complete)
		manager.State.SetEventFlag(1);
		manager.State.SetEventFlag(2);
		manager.State.SetEventFlag(3);
		manager.State.SetEventFlag(4);

		manager.TransitionToChapter(4);

		// Chapter 5 party: Hero(0), Cristo(1), Nara(2), Mara(3) active
		// Brey(4), Taloon(5), Ragnar(6), Alena(7) in wagon
		Assert.Equal(0x00, manager.State.ActiveParty[0]);  // Hero
		Assert.Equal(0x04, manager.State.WagonParty[0]);   // Brey
	}

	[Fact]
	public void SwapPartyMember_WorksInChapter5() {
		var manager = new ChapterManager();
		manager.StartNewGame();

		// Go to Chapter 5
		manager.State.SetEventFlag(1);
		manager.State.SetEventFlag(2);
		manager.State.SetEventFlag(3);
		manager.State.SetEventFlag(4);
		manager.TransitionToChapter(4);

		byte active0 = manager.State.ActiveParty[0];
		byte wagon0 = manager.State.WagonParty[0];

		var result = manager.SwapPartyMember(0, 0);

		Assert.True(result);
		Assert.Equal(wagon0, manager.State.ActiveParty[0]);
		Assert.Equal(active0, manager.State.WagonParty[0]);
	}

	[Fact]
	public void SwapPartyMember_FailsBeforeChapter5() {
		var manager = new ChapterManager();
		manager.StartNewGame();

		var result = manager.SwapPartyMember(0, 0);

		Assert.False(result);
	}

	[Fact]
	public void SetTactic_WorksInChapter5() {
		var manager = new ChapterManager();
		manager.StartNewGame();

		manager.State.SetEventFlag(1);
		manager.State.SetEventFlag(2);
		manager.State.SetEventFlag(3);
		manager.State.SetEventFlag(4);
		manager.TransitionToChapter(4);

		var result = manager.SetTactic(BattleTactic.GoAllOut);

		Assert.True(result);
		Assert.Equal(BattleTactic.GoAllOut, manager.State.CurrentTactic);
	}

	[Fact]
	public void SetTactic_FailsBeforeChapter5() {
		var manager = new ChapterManager();
		manager.StartNewGame();

		var result = manager.SetTactic(BattleTactic.GoAllOut);

		Assert.False(result);
	}

	[Fact]
	public void HasMechanic_ReturnsCorrectFlags() {
		var manager = new ChapterManager();
		manager.StartNewGame();

		Assert.True(manager.HasMechanic(ChapterMechanics.SoloProtagonist));
		Assert.True(manager.HasMechanic(ChapterMechanics.NpcCompanion));
		Assert.False(manager.HasMechanic(ChapterMechanics.WagonParty));
	}

	[Fact]
	public void ExportAllChapterData_GeneratesBinaryData() {
		var data = ChapterManager.ExportAllChapterData();

		Assert.NotNull(data);
		Assert.True(data.Length > 0);
		Assert.Equal(5, data[0]); // Chapter count
	}

	[Fact]
	public void GenerateAssemblyInclude_GeneratesValidAssembly() {
		var asm = ChapterManager.GenerateAssemblyInclude();

		Assert.NotNull(asm);
		Assert.Contains("CHAPTER_COUNT", asm);
		Assert.Contains("CHAPTER_1_RAGNAR", asm);
		Assert.Contains("CHAPTER_5_HERO", asm);
		Assert.Contains("ChapterDataTable:", asm);
	}
}

/// <summary>
/// Unit tests for Chapter model.
/// </summary>
public class ChapterTests {
	[Fact]
	public void ToSnesBytes_GeneratesCorrectSize() {
		var chapter = ChapterManager.Chapters[0];
		var bytes = chapter.ToSnesBytes();

		Assert.Equal(28, bytes.Length);
	}

	[Fact]
	public void ToSnesBytes_FromSnesBytes_Roundtrip() {
		var original = ChapterManager.Chapters[0];
		var bytes = original.ToSnesBytes();
		var restored = Chapter.FromSnesBytes(bytes);

		Assert.Equal(original.Id, restored.Id);
		Assert.Equal(original.ProtagonistId, restored.ProtagonistId);
		Assert.Equal(original.StartMapId, restored.StartMapId);
		Assert.Equal(original.StartX, restored.StartX);
		Assert.Equal(original.StartY, restored.StartY);
		Assert.Equal(original.Mechanics, restored.Mechanics);
		Assert.Equal(original.WagonEnabled, restored.WagonEnabled);
		Assert.Equal(original.TacticsEnabled, restored.TacticsEnabled);
	}

	[Fact]
	public void MaxPartySize_CorrectForChapters() {
		Assert.Equal(1, ChapterManager.Chapters[0].MaxPartySize); // Ragnar solo
		Assert.Equal(3, ChapterManager.Chapters[1].MaxPartySize); // Alena party
		Assert.Equal(4, ChapterManager.Chapters[4].MaxPartySize); // Chapter 5 (wagon)
	}
}

/// <summary>
/// Unit tests for ChapterState model.
/// </summary>
public class ChapterStateTests {
	[Fact]
	public void EventFlags_SetAndGet() {
		var state = new ChapterState();

		state.SetEventFlag(0);
		state.SetEventFlag(7);
		state.SetEventFlag(100);

		Assert.True(state.GetEventFlag(0));
		Assert.True(state.GetEventFlag(7));
		Assert.True(state.GetEventFlag(100));
		Assert.False(state.GetEventFlag(1));
		Assert.False(state.GetEventFlag(50));
	}

	[Fact]
	public void EventFlags_ClearFlag() {
		var state = new ChapterState();

		state.SetEventFlag(50);
		Assert.True(state.GetEventFlag(50));

		state.SetEventFlag(50, false);
		Assert.False(state.GetEventFlag(50));
	}

	[Fact]
	public void ChaptersCompleted_TracksBitmask() {
		var state = new ChapterState();

		state.SetChapterCompleted(1);
		state.SetChapterCompleted(3);

		Assert.True(state.IsChapterCompleted(1));
		Assert.False(state.IsChapterCompleted(2));
		Assert.True(state.IsChapterCompleted(3));
	}

	[Fact]
	public void PlayTimeFormatted_FormatsCorrectly() {
		var state = new ChapterState {
			PlayTimeFrames = 60 * 60 * 90 // 90 minutes
		};

		Assert.Equal("01:30", state.PlayTimeFormatted);
	}

	[Fact]
	public void ToSnesBytes_GeneratesCorrectSize() {
		var state = new ChapterState();
		var bytes = state.ToSnesBytes();

		Assert.Equal(64, bytes.Length);
	}

	[Fact]
	public void ToSnesBytes_FromSnesBytes_Roundtrip() {
		var original = new ChapterState {
			CurrentChapterId = 2,
			ChaptersCompleted = 0x03,
			CurrentMapId = 0x0016,
			CurrentX = 0x0c,
			CurrentY = 0x0e,
			Gold = 12345,
			StepCount = 500,
			CurrentTactic = BattleTactic.WatchMyMp,
			PlayTimeFrames = 100000
		};
		original.ActiveParty = [0x05, 0xff, 0xff, 0xff];
		original.SetEventFlag(10);
		original.SetEventFlag(200);

		var bytes = original.ToSnesBytes();
		var restored = ChapterState.FromSnesBytes(bytes);

		Assert.Equal(original.CurrentChapterId, restored.CurrentChapterId);
		Assert.Equal(original.ChaptersCompleted, restored.ChaptersCompleted);
		Assert.Equal(original.CurrentMapId, restored.CurrentMapId);
		Assert.Equal(original.CurrentX, restored.CurrentX);
		Assert.Equal(original.CurrentY, restored.CurrentY);
		Assert.Equal(original.Gold, restored.Gold);
		Assert.Equal(original.StepCount, restored.StepCount);
		Assert.Equal(original.CurrentTactic, restored.CurrentTactic);
		Assert.Equal(original.PlayTimeFrames, restored.PlayTimeFrames);
		Assert.True(restored.GetEventFlag(10));
		Assert.True(restored.GetEventFlag(200));
	}
}
