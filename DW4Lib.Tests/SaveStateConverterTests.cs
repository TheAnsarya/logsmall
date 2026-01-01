using DW4Lib.Converters;
using DW4Lib.DataStructures;
using Xunit;

namespace DW4Lib.Tests;

/// <summary>
/// Tests for SaveStateConverter.
/// </summary>
public class SaveStateConverterTests {
	// ========================================
	// Basic Conversion Tests
	// ========================================

	[Fact]
	public void DQ3rSaveSlotSize_Is2048Bytes() {
		Assert.Equal(2048, SaveStateConverter.DQ3rSaveSlotSize);
	}

	[Fact]
	public void Convert_Chapter1Start_ReturnsValidSave() {
		var dw4Save = SaveData.CreateChapter1Start();
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.NotNull(dq3Save);
	}

	[Fact]
	public void Convert_Chapter1Start_HasCorrectScenarioId() {
		var dw4Save = SaveData.CreateChapter1Start();
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		// DW4 chapter 0x00 maps to DQ3r scenario 0x10
		Assert.Equal(0x10, dq3Save.Header.ScenarioId);
	}

	[Fact]
	public void Convert_Chapter2Start_HasCorrectScenarioId() {
		var dw4Save = SaveData.CreateChapter2Start();
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		// DW4 chapter 0x01 maps to DQ3r scenario 0x11
		Assert.Equal(0x11, dq3Save.Header.ScenarioId);
	}

	// ========================================
	// Gold Scaling Tests
	// ========================================

	[Fact]
	public void Convert_ScalesGoldBy1_5x() {
		var dw4Save = SaveData.CreateChapter1Start();
		dw4Save.Header.Gold = 1000;
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(1500u, dq3Save.Header.Gold);
	}

	[Fact]
	public void Convert_ZeroGold_StaysZero() {
		var dw4Save = SaveData.CreateChapter1Start();
		dw4Save.Header.Gold = 0;
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(0u, dq3Save.Header.Gold);
	}

	// ========================================
	// Party Conversion Tests
	// ========================================

	[Fact]
	public void Convert_RagnarId_HasOffset() {
		var dw4Save = SaveData.CreateChapter1Start();
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		// Ragnar 0x06 becomes 0x106 in DQ3r
		Assert.Equal(0x106, dq3Save.Party.ActiveParty[0]);
	}

	[Fact]
	public void Convert_EmptyPartySlot_Uses0xFFFF() {
		var dw4Save = SaveData.CreateChapter1Start();
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(0xFFFF, dq3Save.Party.ActiveParty[1]);
		Assert.Equal(0xFFFF, dq3Save.Party.ActiveParty[2]);
		Assert.Equal(0xFFFF, dq3Save.Party.ActiveParty[3]);
	}

	[Fact]
	public void Convert_Chapter2_HasThreePartyMembers() {
		var dw4Save = SaveData.CreateChapter2Start();
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(3, dq3Save.Party.PartyCount);
	}

	[Fact]
	public void Convert_Chapter2_AlenaHasOffset() {
		var dw4Save = SaveData.CreateChapter2Start();
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		// Alena 0x07 becomes 0x107 in DQ3r
		Assert.Equal(0x107, dq3Save.Party.ActiveParty[0]);
	}

	// ========================================
	// Character Stat Scaling Tests
	// ========================================

	[Fact]
	public void Convert_ScalesHPBy1_5x() {
		var dw4Save = SaveData.CreateChapter1Start();
		dw4Save.Characters[0].MaxHP = 100;
		dw4Save.Characters[0].CurrentHP = 50;
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(150, dq3Save.Characters[0].MaxHP);
		Assert.Equal(75, dq3Save.Characters[0].CurrentHP);
	}

	[Fact]
	public void Convert_ScalesMPBy1_5x() {
		var dw4Save = SaveData.CreateChapter2Start();
		dw4Save.Characters[1].MaxMP = 100;
		dw4Save.Characters[1].CurrentMP = 60;
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(150, dq3Save.Characters[1].MaxMP);
		Assert.Equal(90, dq3Save.Characters[1].CurrentMP);
	}

	[Fact]
	public void Convert_ScalesStrengthBy1_2x() {
		var dw4Save = SaveData.CreateChapter1Start();
		dw4Save.Characters[0].Strength = 100;
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(120, dq3Save.Characters[0].Strength);
	}

	[Fact]
	public void Convert_ScalesAgilityBy1_2x() {
		var dw4Save = SaveData.CreateChapter2Start();
		dw4Save.Characters[0].Agility = 100;
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(120, dq3Save.Characters[0].Agility);
	}

	[Fact]
	public void Convert_ScalesExperienceBy1_2x() {
		var dw4Save = SaveData.CreateChapter1Start();
		dw4Save.Characters[0].Experience = 10000;
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(12000u, dq3Save.Characters[0].Experience);
	}

	[Fact]
	public void Convert_PreservesCharacterName() {
		var dw4Save = SaveData.CreateChapter1Start();
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Contains("Ragnar", dq3Save.Characters[0].Name);
	}

	[Fact]
	public void Convert_PreservesLevel() {
		var dw4Save = SaveData.CreateChapter1Start();
		dw4Save.Characters[0].Level = 25;
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(25, dq3Save.Characters[0].Level);
	}

	// ========================================
	// PlayTime Conversion Tests
	// ========================================

	[Fact]
	public void Convert_PlayTimeFramesToSeconds() {
		var dw4Save = SaveData.CreateChapter1Start();
		dw4Save.Header.PlayTimeFrames = 3600; // 60 seconds of gameplay
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(60u, dq3Save.Header.PlayTimeSeconds);
	}

	[Fact]
	public void Convert_DayNightCycleScaled() {
		var dw4Save = SaveData.CreateChapter1Start();
		dw4Save.Header.DayNightCycle = 0x80;
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		Assert.Equal(0x40, dq3Save.Header.DayNightCycle);
	}

	// ========================================
	// World State Conversion Tests
	// ========================================

	[Fact]
	public void Convert_EventFlagsGetOffset() {
		var dw4Save = SaveData.CreateChapter1Start();
		dw4Save.World.SetEventFlag(0x0001);
		dw4Save.World.SetEventFlag(0x0050);
		var dq3Save = SaveStateConverter.Convert(dw4Save);

		// DW4 flag 0x0001 becomes DQ3r flag 0x0201
		Assert.True(dq3Save.World.GetEventFlag(0x201));
		// DW4 flag 0x0050 becomes DQ3r flag 0x0250
		Assert.True(dq3Save.World.GetEventFlag(0x250));
	}

	[Fact]
	public void Convert_TreasureChestsGetOffset() {
		var dw4Save = SaveData.CreateChapter1Start();
		dw4Save.World.SetChestOpened(0x0010);
		var dq3Save = SaveStateConverter.Convert(dw4Save);

		// DW4 chest 0x0010 becomes DQ3r chest 0x0210
		Assert.True(dq3Save.World.IsChestOpened(0x210));
	}

	// ========================================
	// DQ3rSaveData Serialization Tests
	// ========================================

	[Fact]
	public void DQ3rSaveData_ToBytes_HasCorrectSize() {
		var dw4Save = SaveData.CreateChapter1Start();
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		var bytes = dq3Save.ToBytes();
		Assert.Equal(SaveStateConverter.DQ3rSaveSlotSize, bytes.Length);
	}

	[Fact]
	public void DQ3rSaveData_ToBytes_ContainsScenarioId() {
		var dw4Save = SaveData.CreateChapter1Start();
		var dq3Save = SaveStateConverter.Convert(dw4Save);
		var bytes = dq3Save.ToBytes();
		// Scenario ID is at offset 2
		Assert.Equal(0x10, bytes[2]);
	}

	// ========================================
	// SpellIdConverter Tests
	// ========================================

	[Fact]
	public void SpellIdConverter_Heal_Maps() {
		Assert.Equal(0x00, SpellIdConverter.ConvertToDQ3r(0x00));
	}

	[Fact]
	public void SpellIdConverter_Blaze_Maps() {
		Assert.Equal(0x10, SpellIdConverter.ConvertToDQ3r(0x10));
	}

	[Fact]
	public void SpellIdConverter_Return_Maps() {
		Assert.Equal(0x40, SpellIdConverter.ConvertToDQ3r(0x40));
	}

	[Fact]
	public void SpellIdConverter_Unknown_ReturnsNegative() {
		Assert.Equal(-1, SpellIdConverter.ConvertToDQ3r(0xFF));
	}

	// ========================================
	// Component Structure Tests
	// ========================================

	[Fact]
	public void DQ3rSaveHeader_ToBytes_HasCorrectLength() {
		var header = new DQ3rSaveHeader();
		var bytes = header.ToBytes();
		Assert.Equal(32, bytes.Length);
	}

	[Fact]
	public void DQ3rPartyConfig_ToBytes_HasCorrectLength() {
		var party = new DQ3rPartyConfig();
		var bytes = party.ToBytes();
		Assert.Equal(32, bytes.Length);
	}

	[Fact]
	public void DQ3rCharacterData_ToBytes_HasCorrectLength() {
		var character = new DQ3rCharacterData();
		var bytes = character.ToBytes();
		Assert.Equal(64, bytes.Length);
	}

	[Fact]
	public void DQ3rInventoryData_ToBytes_HasCorrectLength() {
		var inventory = new DQ3rInventoryData();
		var bytes = inventory.ToBytes();
		Assert.Equal(160, bytes.Length);
	}

	[Fact]
	public void DQ3rWorldState_ToBytes_HasCorrectLength() {
		var world = new DQ3rWorldState();
		var bytes = world.ToBytes();
		Assert.Equal(256, bytes.Length);
	}
}
