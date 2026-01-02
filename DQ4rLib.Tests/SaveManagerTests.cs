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
	public void CharacterSaveData_NewFieldsSerializeCorrectly() {
		var character = new CharacterSaveData {
			Id = 3,
			ClassId = 2,
			Name = "CRISTO",
			Level = 30,
			CurrentHp = 180,
			MaxHp = 250,
			CurrentMp = 80,
			MaxMp = 100,
			Strength = 60,
			Agility = 50,
			Vitality = 70,
			Intelligence = 90,
			Luck = 40,
			Attack = 150,
			Defense = 120,
			Experience = 100000,
			WeaponId = 0x25,
			ArmorId = 0x30,
			ShieldId = 0x10,
			HelmetId = 0x15,
			AccessoryId = 0x05,
			Status = CharacterStatus.None,
			Tactic = BattleTactic.DontUseMagic,
			PartyPosition = 2
		};

		// Learn some spells
		character.LearnSpell(0);  // Heal
		character.LearnSpell(5);  // Midheal
		character.LearnSpell(10); // Fullheal
		character.LearnSpell(63); // Last spell slot

		var bytes = character.ToSnesBytes();
		Assert.Equal(48, bytes.Length);

		var restored = CharacterSaveData.FromSnesBytes(bytes);

		Assert.Equal(character.Id, restored.Id);
		Assert.Equal(character.ClassId, restored.ClassId);
		Assert.Equal(character.Name, restored.Name);
		Assert.Equal(character.Level, restored.Level);
		Assert.Equal(character.CurrentHp, restored.CurrentHp);
		Assert.Equal(character.MaxHp, restored.MaxHp);
		Assert.Equal(character.CurrentMp, restored.CurrentMp);
		Assert.Equal(character.MaxMp, restored.MaxMp);
		Assert.Equal(character.Strength, restored.Strength);
		Assert.Equal(character.Agility, restored.Agility);
		Assert.Equal(character.Vitality, restored.Vitality);
		Assert.Equal(character.Intelligence, restored.Intelligence);
		Assert.Equal(character.Luck, restored.Luck);
		Assert.Equal(character.Attack, restored.Attack);
		Assert.Equal(character.Defense, restored.Defense);
		Assert.Equal(character.Experience, restored.Experience);
		Assert.Equal(character.WeaponId, restored.WeaponId);
		Assert.Equal(character.ArmorId, restored.ArmorId);
		Assert.Equal(character.ShieldId, restored.ShieldId);
		Assert.Equal(character.HelmetId, restored.HelmetId);
		Assert.Equal(character.AccessoryId, restored.AccessoryId);
		Assert.Equal(character.Status, restored.Status);
		Assert.Equal(character.Tactic, restored.Tactic);
		Assert.Equal(character.PartyPosition, restored.PartyPosition);

		// Verify spells
		Assert.True(restored.KnowsSpell(0));
		Assert.True(restored.KnowsSpell(5));
		Assert.True(restored.KnowsSpell(10));
		Assert.True(restored.KnowsSpell(63));
		Assert.False(restored.KnowsSpell(1));
		Assert.False(restored.KnowsSpell(62));
	}

	[Fact]
	public void CharacterSaveData_SpellManagement() {
		var character = new CharacterSaveData();

		// Initially no spells
		Assert.False(character.KnowsSpell(0));
		Assert.Empty(character.GetKnownSpells());

		// Learn spells
		character.LearnSpell(0);
		character.LearnSpell(10);
		character.LearnSpell(20);

		Assert.True(character.KnowsSpell(0));
		Assert.True(character.KnowsSpell(10));
		Assert.True(character.KnowsSpell(20));
		Assert.False(character.KnowsSpell(1));
		Assert.Equal([0, 10, 20], character.GetKnownSpells());

		// Forget a spell
		character.ForgetSpell(10);
		Assert.False(character.KnowsSpell(10));
		Assert.Equal([0, 20], character.GetKnownSpells());

		// Invalid spell IDs should be ignored
		character.LearnSpell(64);
		character.LearnSpell(-1);
		Assert.False(character.KnowsSpell(64));
	}

	[Fact]
	public void CharacterSaveData_StatusHelpers() {
		var character = new CharacterSaveData {
			CurrentHp = 100,
			MaxHp = 200,
			Status = CharacterStatus.None
		};

		// Alive and can act
		Assert.True(character.IsAlive);
		Assert.True(character.CanAct);

		// Dead - not alive, can't act
		character.Status = CharacterStatus.Dead;
		Assert.False(character.IsAlive);
		Assert.False(character.CanAct);

		// Alive but asleep - can't act
		character.Status = CharacterStatus.Asleep;
		Assert.True(character.IsAlive);
		Assert.False(character.CanAct);

		// Alive but paralyzed - can't act
		character.Status = CharacterStatus.Paralyzed;
		Assert.True(character.IsAlive);
		Assert.False(character.CanAct);

		// Alive but confused - can't act (in terms of controlled actions)
		character.Status = CharacterStatus.Confused;
		Assert.True(character.IsAlive);
		Assert.False(character.CanAct);

		// Poisoned but can still act
		character.Status = CharacterStatus.Poisoned;
		Assert.True(character.IsAlive);
		Assert.True(character.CanAct);

		// Multiple status effects
		character.Status = CharacterStatus.Poisoned | CharacterStatus.DefenseUp;
		Assert.True(character.IsAlive);
		Assert.True(character.CanAct);

		// Zero HP means not alive even without Dead flag
		character.CurrentHp = 0;
		character.Status = CharacterStatus.None;
		Assert.False(character.IsAlive);
		Assert.False(character.CanAct);
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
