using DW4Lib.DataStructures;
using Xunit;

namespace DW4Lib.Tests;

/// <summary>
/// Tests for SaveData and related save file structures.
/// </summary>
public class SaveDataTests {
	// ========================================
	// SaveData Basic Tests
	// ========================================

	[Fact]
	public void SaveSlotSize_Is752Bytes() {
		Assert.Equal(752, SaveData.SaveSlotSize);
	}

	[Fact]
	public void SaveSlotCount_Is3() {
		Assert.Equal(3, SaveData.SaveSlotCount);
	}

	[Fact]
	public void SramBase_Is0x6000() {
		Assert.Equal(0x6000, SaveData.SramBase);
	}

	[Fact]
	public void NewSaveData_HasInitializedArrays() {
		var save = new SaveData();
		Assert.NotNull(save.Characters);
		Assert.Equal(8, save.Characters.Length);
		Assert.NotNull(save.Equipment);
		Assert.Equal(8, save.Equipment.Length);
		Assert.NotNull(save.SpellsKnown);
		Assert.Equal(8, save.SpellsKnown.Length);
	}

	// ========================================
	// Chapter 1 Starting Save Tests
	// ========================================

	[Fact]
	public void CreateChapter1Start_HasCorrectChapter() {
		var save = SaveData.CreateChapter1Start();
		Assert.Equal(0x00, save.Header.Chapter);
	}

	[Fact]
	public void CreateChapter1Start_HasRagnarInParty() {
		var save = SaveData.CreateChapter1Start();
		Assert.Equal(0x06, save.Party.ActiveParty[0]); // Ragnar's ID
	}

	[Fact]
	public void CreateChapter1Start_HasSinglePartyMember() {
		var save = SaveData.CreateChapter1Start();
		Assert.Equal(1, save.Party.PartyCount);
		Assert.Equal(0xFF, save.Party.ActiveParty[1]); // Empty
		Assert.Equal(0xFF, save.Party.ActiveParty[2]); // Empty
		Assert.Equal(0xFF, save.Party.ActiveParty[3]); // Empty
	}

	[Fact]
	public void CreateChapter1Start_RagnarHasCorrectStats() {
		var save = SaveData.CreateChapter1Start();
		var ragnar = save.Characters[0];
		Assert.Equal("Ragnar", ragnar.Name.TrimEnd('\0'));
		Assert.Equal(1, ragnar.Level);
		Assert.Equal(30, ragnar.MaxHP);
		Assert.Equal(0, ragnar.MaxMP); // Ragnar has no magic
		Assert.Equal(12, ragnar.Strength);
	}

	[Fact]
	public void CreateChapter1Start_HasZeroGold() {
		var save = SaveData.CreateChapter1Start();
		Assert.Equal(0, save.Header.Gold);
	}

	[Fact]
	public void CreateChapter1Start_IsDaytime() {
		var save = SaveData.CreateChapter1Start();
		Assert.Equal(0x40, save.Header.DayNightCycle); // Day
	}

	[Fact]
	public void CreateChapter1Start_HasEventFlagSet() {
		var save = SaveData.CreateChapter1Start();
		Assert.True(save.World.GetEventFlag(0x0001)); // Chapter 1 started
	}

	// ========================================
	// Chapter 2 Starting Save Tests
	// ========================================

	[Fact]
	public void CreateChapter2Start_HasCorrectChapter() {
		var save = SaveData.CreateChapter2Start();
		Assert.Equal(0x01, save.Header.Chapter);
	}

	[Fact]
	public void CreateChapter2Start_HasThreePartyMembers() {
		var save = SaveData.CreateChapter2Start();
		Assert.Equal(3, save.Party.PartyCount);
	}

	[Fact]
	public void CreateChapter2Start_HasAlenaInLead() {
		var save = SaveData.CreateChapter2Start();
		Assert.Equal(0x07, save.Party.ActiveParty[0]); // Alena's ID
	}

	[Fact]
	public void CreateChapter2Start_AlenaHasCorrectStats() {
		var save = SaveData.CreateChapter2Start();
		var alena = save.Characters[0];
		Assert.Equal("Alena", alena.Name.TrimEnd('\0'));
		Assert.Equal(14, alena.Agility); // Alena is fast
		Assert.Equal(0, alena.MaxMP); // No magic
	}

	[Fact]
	public void CreateChapter2Start_CristoHasMP() {
		var save = SaveData.CreateChapter2Start();
		var cristo = save.Characters[1];
		Assert.Equal("Cristo", cristo.Name.TrimEnd('\0'));
		Assert.Equal(8, cristo.MaxMP); // Cristo is a healer
	}

	[Fact]
	public void CreateChapter2Start_BreyHasMostMP() {
		var save = SaveData.CreateChapter2Start();
		var brey = save.Characters[2];
		Assert.Equal("Brey", brey.Name.TrimEnd('\0'));
		Assert.Equal(12, brey.MaxMP); // Brey is a mage
	}

	// ========================================
	// SaveHeader Tests
	// ========================================

	[Fact]
	public void SaveHeader_ToBytes_IsCorrectLength() {
		var header = new SaveHeader();
		var bytes = header.ToBytes();
		Assert.Equal(16, bytes.Length);
	}

	[Fact]
	public void SaveHeader_RoundTrip_PreservesData() {
		var header = new SaveHeader {
			Checksum = 0x1234,
			Chapter = 0x03,
			SubChapterProgress = 0x05,
			Gold = 12345,
			CasinoCoins = 9999,
			PlayTimeFrames = 123456,
			DayNightCycle = 0x80
		};

		var bytes = header.ToBytes();
		var restored = SaveHeader.FromBytes(bytes);

		Assert.Equal(header.Checksum, restored.Checksum);
		Assert.Equal(header.Chapter, restored.Chapter);
		Assert.Equal(header.SubChapterProgress, restored.SubChapterProgress);
		Assert.Equal(header.Gold, restored.Gold);
		Assert.Equal(header.CasinoCoins, restored.CasinoCoins);
		Assert.Equal(header.PlayTimeFrames, restored.PlayTimeFrames);
		Assert.Equal(header.DayNightCycle, restored.DayNightCycle);
	}

	// ========================================
	// PartyConfig Tests
	// ========================================

	[Fact]
	public void PartyConfig_ToBytes_IsCorrectLength() {
		var party = new PartyConfig();
		var bytes = party.ToBytes();
		Assert.Equal(32, bytes.Length);
	}

	[Fact]
	public void PartyConfig_RoundTrip_PreservesData() {
		var party = new PartyConfig {
			ActiveParty = [0x06, 0x07, 0x01, 0x04],
			WagonParty = [0x00, 0x02, 0x03, 0x05],
			PartyCount = 4,
			WagonCount = 4
		};

		var bytes = party.ToBytes();
		var restored = PartyConfig.FromBytes(bytes);

		Assert.Equal(party.ActiveParty, restored.ActiveParty);
		Assert.Equal(party.WagonParty, restored.WagonParty);
		Assert.Equal(party.PartyCount, restored.PartyCount);
		Assert.Equal(party.WagonCount, restored.WagonCount);
	}

	// ========================================
	// CharacterSaveData Tests
	// ========================================

	[Fact]
	public void CharacterSaveData_ToBytes_IsCorrectLength() {
		var character = new CharacterSaveData();
		var bytes = character.ToBytes();
		Assert.Equal(32, bytes.Length);
	}

	[Fact]
	public void CharacterSaveData_RoundTrip_PreservesData() {
		var character = new CharacterSaveData {
			Name = "Hero",
			Level = 50,
			Experience = 999999,
			CurrentHP = 350,
			MaxHP = 400,
			CurrentMP = 100,
			MaxMP = 120,
			Strength = 200,
			Agility = 180,
			Vitality = 190,
			Intelligence = 160,
			Luck = 150,
			StatusEffects = 0x03
		};

		var bytes = character.ToBytes();
		var restored = CharacterSaveData.FromBytes(bytes);

		Assert.Equal("Hero", restored.Name);
		Assert.Equal(character.Level, restored.Level);
		Assert.Equal(character.Experience, restored.Experience);
		Assert.Equal(character.CurrentHP, restored.CurrentHP);
		Assert.Equal(character.MaxHP, restored.MaxHP);
		Assert.Equal(character.CurrentMP, restored.CurrentMP);
		Assert.Equal(character.MaxMP, restored.MaxMP);
		Assert.Equal(character.Strength, restored.Strength);
		Assert.Equal(character.Agility, restored.Agility);
		Assert.Equal(character.Vitality, restored.Vitality);
		Assert.Equal(character.Intelligence, restored.Intelligence);
		Assert.Equal(character.Luck, restored.Luck);
		Assert.Equal(character.StatusEffects, restored.StatusEffects);
	}

	// ========================================
	// EquipmentData Tests
	// ========================================

	[Fact]
	public void EquipmentData_ToBytes_IsCorrectLength() {
		var equipment = new EquipmentData();
		var bytes = equipment.ToBytes();
		Assert.Equal(8, bytes.Length);
	}

	[Fact]
	public void EquipmentData_RoundTrip_PreservesData() {
		var equipment = new EquipmentData {
			Weapon = 0x50,
			Armor = 0x60,
			Shield = 0x70,
			Helmet = 0x80,
			Accessory1 = 0x90,
			Accessory2 = 0xA0
		};

		var bytes = equipment.ToBytes();
		var restored = EquipmentData.FromBytes(bytes);

		Assert.Equal(equipment.Weapon, restored.Weapon);
		Assert.Equal(equipment.Armor, restored.Armor);
		Assert.Equal(equipment.Shield, restored.Shield);
		Assert.Equal(equipment.Helmet, restored.Helmet);
		Assert.Equal(equipment.Accessory1, restored.Accessory1);
		Assert.Equal(equipment.Accessory2, restored.Accessory2);
	}

	// ========================================
	// InventoryData Tests
	// ========================================

	[Fact]
	public void InventoryData_ToBytes_IsCorrectLength() {
		var inventory = new InventoryData();
		var bytes = inventory.ToBytes();
		Assert.Equal(96, bytes.Length);
	}

	[Fact]
	public void InventoryData_RoundTrip_PreservesData() {
		var inventory = new InventoryData();
		inventory.BagItems[0] = 0x01;
		inventory.BagItems[63] = 0xFF;
		inventory.ImportantItems[0] = 0x10;
		inventory.ImportantItems[31] = 0xEE;

		var bytes = inventory.ToBytes();
		var restored = InventoryData.FromBytes(bytes);

		Assert.Equal(inventory.BagItems[0], restored.BagItems[0]);
		Assert.Equal(inventory.BagItems[63], restored.BagItems[63]);
		Assert.Equal(inventory.ImportantItems[0], restored.ImportantItems[0]);
		Assert.Equal(inventory.ImportantItems[31], restored.ImportantItems[31]);
	}

	// ========================================
	// SpellFlags Tests
	// ========================================

	[Fact]
	public void SpellFlags_ToBytes_IsCorrectLength() {
		var spells = new SpellFlags();
		var bytes = spells.ToBytes();
		Assert.Equal(8, bytes.Length);
	}

	[Fact]
	public void SpellFlags_SetSpell_Works() {
		var spells = new SpellFlags();
		Assert.False(spells.HasSpell(0));
		spells.SetSpell(0);
		Assert.True(spells.HasSpell(0));
	}

	[Fact]
	public void SpellFlags_ClearSpell_Works() {
		var spells = new SpellFlags();
		spells.SetSpell(5);
		Assert.True(spells.HasSpell(5));
		spells.SetSpell(5, false);
		Assert.False(spells.HasSpell(5));
	}

	[Fact]
	public void SpellFlags_MultipleSpells_AreIndependent() {
		var spells = new SpellFlags();
		spells.SetSpell(0);
		spells.SetSpell(7);
		spells.SetSpell(8);
		spells.SetSpell(63);

		Assert.True(spells.HasSpell(0));
		Assert.False(spells.HasSpell(1));
		Assert.True(spells.HasSpell(7));
		Assert.True(spells.HasSpell(8));
		Assert.False(spells.HasSpell(9));
		Assert.True(spells.HasSpell(63));
	}

	[Fact]
	public void SpellFlags_IgnoresOutOfRange() {
		var spells = new SpellFlags();
		spells.SetSpell(64); // Out of range
		Assert.False(spells.HasSpell(64));
	}

	// ========================================
	// WorldState Tests
	// ========================================

	[Fact]
	public void WorldState_ToBytes_IsCorrectLength() {
		var world = new WorldState();
		var bytes = world.ToBytes();
		Assert.Equal(224, bytes.Length);
	}

	[Fact]
	public void WorldState_SetEventFlag_Works() {
		var world = new WorldState();
		Assert.False(world.GetEventFlag(0x0100));
		world.SetEventFlag(0x0100);
		Assert.True(world.GetEventFlag(0x0100));
	}

	[Fact]
	public void WorldState_ClearEventFlag_Works() {
		var world = new WorldState();
		world.SetEventFlag(0x0050);
		Assert.True(world.GetEventFlag(0x0050));
		world.SetEventFlag(0x0050, false);
		Assert.False(world.GetEventFlag(0x0050));
	}

	[Fact]
	public void WorldState_SetChestOpened_Works() {
		var world = new WorldState();
		Assert.False(world.IsChestOpened(0x10));
		world.SetChestOpened(0x10);
		Assert.True(world.IsChestOpened(0x10));
	}

	[Fact]
	public void WorldState_ClearChestOpened_Works() {
		var world = new WorldState();
		world.SetChestOpened(0x20);
		Assert.True(world.IsChestOpened(0x20));
		world.SetChestOpened(0x20, false);
		Assert.False(world.IsChestOpened(0x20));
	}

	[Fact]
	public void WorldState_RoundTrip_PreservesData() {
		var world = new WorldState();
		world.SetEventFlag(0x0001);
		world.SetEventFlag(0x0100);
		world.SetEventFlag(0x01FF);
		world.SetChestOpened(0x00);
		world.SetChestOpened(0x3F);
		world.MiscFlags[0] = 0xAA;
		world.MiscFlags[95] = 0x55;

		var bytes = world.ToBytes();
		var restored = WorldState.FromBytes(bytes);

		Assert.True(restored.GetEventFlag(0x0001));
		Assert.True(restored.GetEventFlag(0x0100));
		Assert.True(restored.GetEventFlag(0x01FF));
		Assert.True(restored.IsChestOpened(0x00));
		Assert.True(restored.IsChestOpened(0x3F));
		Assert.Equal(0xAA, restored.MiscFlags[0]);
		Assert.Equal(0x55, restored.MiscFlags[95]);
	}

	// ========================================
	// Full SaveData Serialization Tests
	// ========================================

	[Fact]
	public void SaveData_ToBytes_IsCorrectLength() {
		var save = new SaveData();
		var bytes = save.ToBytes();
		Assert.Equal(SaveData.SaveSlotSize, bytes.Length);
	}

	[Fact]
	public void SaveData_Chapter1_RoundTrip_PreservesData() {
		var original = SaveData.CreateChapter1Start();
		var bytes = original.ToBytes();
		var restored = SaveData.FromBytes(bytes);

		Assert.Equal(original.Header.Chapter, restored.Header.Chapter);
		Assert.Equal(original.Party.PartyCount, restored.Party.PartyCount);
		Assert.Equal(original.Party.ActiveParty[0], restored.Party.ActiveParty[0]);
		Assert.Equal(original.Characters[0].Name.TrimEnd('\0'), restored.Characters[0].Name);
		Assert.Equal(original.Characters[0].Level, restored.Characters[0].Level);
		Assert.Equal(original.Characters[0].MaxHP, restored.Characters[0].MaxHP);
	}

	[Fact]
	public void SaveData_Chapter2_RoundTrip_PreservesData() {
		var original = SaveData.CreateChapter2Start();
		var bytes = original.ToBytes();
		var restored = SaveData.FromBytes(bytes);

		Assert.Equal(original.Header.Chapter, restored.Header.Chapter);
		Assert.Equal(original.Party.PartyCount, restored.Party.PartyCount);
		Assert.Equal("Alena", restored.Characters[0].Name);
		Assert.Equal("Cristo", restored.Characters[1].Name);
		Assert.Equal("Brey", restored.Characters[2].Name);
	}

	[Fact]
	public void SaveData_FromBytes_ThrowsOnShortData() {
		var shortData = new byte[100];
		Assert.Throws<ArgumentException>(() => SaveData.FromBytes(shortData));
	}

	[Fact]
	public void SaveData_ToBytes_CalculatesChecksum() {
		var save = SaveData.CreateChapter1Start();
		var bytes = save.ToBytes();

		// Checksum is stored at offset 0-1
		ushort storedChecksum = (ushort)(bytes[0] | (bytes[1] << 8));
		Assert.NotEqual(0, storedChecksum);
	}
}
