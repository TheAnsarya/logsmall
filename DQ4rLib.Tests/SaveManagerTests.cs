namespace DQ4rLib.Tests;

using DQ4rLib;
using DQ4rLib.Models;

public class SaveManagerTests {
	[Fact]
	public void SaveManager_InitializesWithEmptySlots() {
		var saveManager = new SaveManager();

		Assert.Equal(SaveManager.MaxSaveSlots, saveManager.Slots.Length);
		Assert.All(saveManager.Slots, slot => Assert.False(slot.IsUsed));
	}

	[Fact]
	public void SaveData_SerializesAndDeserializesCorrectly() {
		var saveData = new SaveData {
			Version = 1,
			SmallMedals = 10,
			CasinoCoins = 5000
		};

		// Set chapter state using settable properties
		saveData.ChapterState.CurrentChapterId = 1; // Chapter 2 (0-indexed)
		saveData.ChapterState.Gold = 1500;
		saveData.ChapterState.PlayTimeFrames = 216000; // 60 frames/sec * 60 min * 60 sec = 1 hour

		// Add a character
		saveData.Characters[0] = new CharacterSaveData {
			Id = 1,
			Name = "HERO",
			Level = 10,
			CurrentHp = 50,
			MaxHp = 75,
			Experience = 1234
		};

		var bytes = saveData.ToSnesBytes();
		var restored = SaveData.FromSnesBytes(bytes);

		Assert.Equal(saveData.Version, restored.Version);
		Assert.Equal(saveData.ChapterState.CurrentChapterId, restored.ChapterState.CurrentChapterId);
		Assert.Equal(saveData.ChapterState.CurrentChapter, restored.ChapterState.CurrentChapter); // Computed property
		Assert.Equal(saveData.ChapterState.PlayTimeFrames, restored.ChapterState.PlayTimeFrames);
		Assert.Equal(saveData.ChapterState.Gold, restored.ChapterState.Gold);
		Assert.Equal(saveData.SmallMedals, restored.SmallMedals);
		Assert.Equal(saveData.CasinoCoins, restored.CasinoCoins);
		Assert.Equal(saveData.Characters[0].Id, restored.Characters[0].Id);
		Assert.Equal(saveData.Characters[0].Level, restored.Characters[0].Level);
		Assert.Equal(saveData.Characters[0].CurrentHp, restored.Characters[0].CurrentHp);
	}

	[Fact]
	public void SaveData_ChecksumValidation() {
		var saveData = new SaveData {
			Version = 1
		};
		saveData.ChapterState.CurrentChapterId = 0; // Chapter 1

		var bytes = saveData.ToSnesBytes();
		var restored = SaveData.FromSnesBytes(bytes);

		Assert.True(restored.ValidateChecksum());

		// Corrupt the data
		bytes[100] ^= 0xff;
		var corrupted = SaveData.FromSnesBytes(bytes);
		Assert.False(corrupted.ValidateChecksum());
	}

	[Fact]
	public void CharacterSaveData_SerializesCorrectly() {
		var character = new CharacterSaveData {
			Id = 5,
			Name = "ALENA",
			Level = 25,
			CurrentHp = 150,
			MaxHp = 200,
			CurrentMp = 30,
			MaxMp = 50,
			Strength = 80,
			Agility = 120,
			Experience = 50000,
			Tactic = BattleTactic.ShowNoMercy
		};

		var bytes = character.ToSnesBytes();
		var restored = CharacterSaveData.FromSnesBytes(bytes);

		Assert.Equal(character.Id, restored.Id);
		Assert.Equal(character.Level, restored.Level);
		Assert.Equal(character.CurrentHp, restored.CurrentHp);
		Assert.Equal(character.MaxHp, restored.MaxHp);
		Assert.Equal(character.Strength, restored.Strength);
		Assert.Equal(character.Agility, restored.Agility);
		Assert.Equal(character.Experience, restored.Experience);
	}

	[Fact]
	public void InventoryData_SerializesCorrectly() {
		var inventory = new InventoryData();
		inventory.BagItems[0] = 0x10; // Medical herb
		inventory.BagItems[1] = 0x20; // Antidote
		inventory.SetImportantItem(1, true);
		inventory.SetImportantItem(5, true);

		var bytes = inventory.ToSnesBytes();
		var restored = InventoryData.FromSnesBytes(bytes);

		Assert.Equal(inventory.BagItems[0], restored.BagItems[0]);
		Assert.Equal(inventory.BagItems[1], restored.BagItems[1]);
		Assert.True(restored.HasImportantItem(1));
		Assert.True(restored.HasImportantItem(5));
		Assert.False(restored.HasImportantItem(0));
	}

	[Fact]
	public void SaveManager_SaveAndLoad() {
		var state = new ChapterState { CurrentChapterId = 1, Gold = 1000 }; // Chapter 2
		var chapterManager = new ChapterManager(state);
		var saveManager = new SaveManager();

		saveManager.Save(0, chapterManager);

		Assert.True(saveManager.Slots[0].IsUsed);
		Assert.NotNull(saveManager.Slots[0].Preview);
		Assert.Equal(2, saveManager.Slots[0].Preview!.ChapterNumber); // CurrentChapter = CurrentChapterId + 1
		Assert.Equal(1000u, saveManager.Slots[0].Preview!.Gold);

		var loaded = saveManager.Load(0);
		Assert.NotNull(loaded);
		Assert.Equal(2, loaded.ChapterState.CurrentChapter);
		Assert.Equal(1000u, loaded.ChapterState.Gold);
	}

	[Fact]
	public void SaveManager_DeleteSlot() {
		var state = new ChapterState { CurrentChapterId = 0 }; // Chapter 1
		var chapterManager = new ChapterManager(state);
		var saveManager = new SaveManager();

		saveManager.Save(1, chapterManager);
		Assert.True(saveManager.Slots[1].IsUsed);

		saveManager.Delete(1);
		Assert.False(saveManager.Slots[1].IsUsed);
	}

	[Fact]
	public void SaveManager_CopySlot() {
		var state = new ChapterState { CurrentChapterId = 2, Gold = 9999 }; // Chapter 3
		var chapterManager = new ChapterManager(state);
		var saveManager = new SaveManager();

		saveManager.Save(0, chapterManager);
		saveManager.Copy(0, 2);

		Assert.True(saveManager.Slots[2].IsUsed);
		Assert.Equal(3, saveManager.Slots[2].Preview!.ChapterNumber);
		Assert.Equal(9999u, saveManager.Slots[2].Preview!.Gold);
	}

	[Fact]
	public void SaveManager_GetSlotPreview() {
		var state = new ChapterState { CurrentChapterId = 3 }; // Chapter 4
		var chapterManager = new ChapterManager(state);
		var saveManager = new SaveManager();

		saveManager.Save(0, chapterManager);

		var preview = saveManager.GetSlotPreview(0);
		Assert.NotNull(preview);
		Assert.Equal(4, preview.ChapterNumber);

		// Empty slot should return null
		var emptyPreview = saveManager.GetSlotPreview(1);
		Assert.Null(emptyPreview);
	}

	[Fact]
	public void SaveManager_SramRoundTrip() {
		var state1 = new ChapterState { CurrentChapterId = 0, Gold = 100 }; // Chapter 1
		var state2 = new ChapterState { CurrentChapterId = 1, Gold = 200 }; // Chapter 2
		var chapterManager1 = new ChapterManager(state1);
		var chapterManager2 = new ChapterManager(state2);
		var saveManager = new SaveManager();

		saveManager.Save(0, chapterManager1);
		saveManager.Save(1, chapterManager2);

		// Full round trip
		var sram = saveManager.ToSramBytes();
		Assert.Equal(SaveManager.SramSize, sram.Length);

		var restored = SaveManager.FromSramBytes(sram);

		Assert.Equal(100u, restored.Slots[0].Preview!.Gold);
		Assert.Equal(200u, restored.Slots[1].Preview!.Gold);
	}

	[Fact]
	public void InventoryData_AddAndRemoveItem() {
		var inventory = new InventoryData();

		Assert.True(inventory.AddItem(0x10));
		Assert.True(inventory.AddItem(0x20));
		Assert.Equal((byte)0x10, inventory.BagItems[0]);
		Assert.Equal((byte)0x20, inventory.BagItems[1]);

		// After removing 0x10, items shift left so 0x20 moves to slot 0
		Assert.True(inventory.RemoveItem(0x10));
		Assert.Equal((byte)0x20, inventory.BagItems[0]);
		Assert.Equal((byte)0x00, inventory.BagItems[1]);
	}

	[Fact]
	public void ChapterState_EventFlags() {
		var state = new ChapterState();

		Assert.False(state.GetEventFlag(0));
		Assert.False(state.GetEventFlag(100));

		state.SetEventFlag(50);
		Assert.True(state.GetEventFlag(50));
		Assert.False(state.GetEventFlag(49));
		Assert.False(state.GetEventFlag(51));

		state.SetEventFlag(50, false);
		Assert.False(state.GetEventFlag(50));
	}

	[Fact]
	public void ChapterState_ChapterCompletion() {
		var state = new ChapterState();

		Assert.False(state.IsChapterCompleted(1));
		Assert.False(state.IsChapterCompleted(2));

		state.SetChapterCompleted(1);
		Assert.True(state.IsChapterCompleted(1));
		Assert.False(state.IsChapterCompleted(2));

		state.SetChapterCompleted(3);
		Assert.True(state.IsChapterCompleted(1));
		Assert.False(state.IsChapterCompleted(2));
		Assert.True(state.IsChapterCompleted(3));
	}

	[Fact]
	public void ChapterState_PlayTimeFormatted() {
		var state = new ChapterState();

		// 1 hour = 60 * 60 * 60 = 216000 frames (assuming 60 fps)
		state.PlayTimeFrames = 216000;
		Assert.Contains(":", state.PlayTimeFormatted);

		// 0 frames
		state.PlayTimeFrames = 0;
		Assert.Equal("00:00", state.PlayTimeFormatted);
	}
}
