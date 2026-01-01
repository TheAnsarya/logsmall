using DW4Lib.Events;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for Event Scripting System.
/// </summary>
public class EventScriptTests {
	// ============================================================
	// EventScript Tests
	// ============================================================

	[Fact]
	public void EventScript_DefaultValues_AreCorrect() {
		var script = new EventScript();

		Assert.Equal((ushort)0, script.Id);
		Assert.Equal(string.Empty, script.Name);
		Assert.Equal(ScriptCategory.Dialog, script.Category);
		Assert.Null(script.ChapterId);
		Assert.Empty(script.Commands);
	}

	[Fact]
	public void EventScript_Parse_ExtractsCommands() {
		// Arrange - simple script: ShowDialog, SetFlag, End
		byte[] data = [0x10, 0x01, 0x00, 0x20, 0x05, 0x00, 0x00];

		// Act
		var script = EventScript.Parse(data, 0, 0x0001);

		// Assert
		Assert.Equal((ushort)0x0001, script.Id);
		Assert.Equal(3, script.Commands.Count);
		Assert.Equal(ScriptOpcode.ShowDialog, script.Commands[0].Opcode);
		Assert.Equal(ScriptOpcode.SetFlag, script.Commands[1].Opcode);
		Assert.Equal(ScriptOpcode.End, script.Commands[2].Opcode);
	}

	[Fact]
	public void EventScript_ToBytes_SerializesCorrectly() {
		// Arrange
		var script = new EventScript {
			Commands = [
				new ScriptCommand { Opcode = ScriptOpcode.ShowDialog, Parameters = [0x01, 0x00] },
				new ScriptCommand { Opcode = ScriptOpcode.End, Parameters = [] }
			]
		};

		// Act
		var bytes = script.ToBytes();

		// Assert
		Assert.Equal(4, bytes.Length);
		Assert.Equal(0x10, bytes[0]); // ShowDialog
		Assert.Equal(0x01, bytes[1]); // Dialog ID low
		Assert.Equal(0x00, bytes[2]); // Dialog ID high
		Assert.Equal(0x00, bytes[3]); // End
	}

	// ============================================================
	// ScriptCommand Tests
	// ============================================================

	[Fact]
	public void ScriptCommand_Parse_ExtractsOpcodeAndParams() {
		byte[] data = [0x30, 0x15]; // GiveItem, item ID 0x15

		var cmd = ScriptCommand.Parse(data, 0);

		Assert.Equal(ScriptOpcode.GiveItem, cmd.Opcode);
		Assert.Single(cmd.Parameters);
		Assert.Equal(0x15, cmd.Parameters[0]);
	}

	[Fact]
	public void ScriptCommand_Size_IncludesOpcodeAndParams() {
		var cmd = new ScriptCommand {
			Opcode = ScriptOpcode.Warp,
			Parameters = [0x10, 5, 5, 0]
		};

		Assert.Equal(5, cmd.Size); // 1 opcode + 4 params
	}

	[Fact]
	public void ScriptCommand_ToBytes_SerializesCorrectly() {
		var cmd = new ScriptCommand {
			Opcode = ScriptOpcode.GiveGold,
			Parameters = [0x64, 0x00] // 100 gold
		};

		var bytes = cmd.ToBytes();

		Assert.Equal(3, bytes.Length);
		Assert.Equal(0x40, bytes[0]); // GiveGold opcode
		Assert.Equal(0x64, bytes[1]); // 100 low byte
		Assert.Equal(0x00, bytes[2]); // 100 high byte
	}

	[Theory]
	[InlineData(ScriptOpcode.End, 0)]
	[InlineData(ScriptOpcode.Return, 0)]
	[InlineData(ScriptOpcode.Nop, 0)]
	[InlineData(ScriptOpcode.ShowDialog, 2)]
	[InlineData(ScriptOpcode.ShowChoice, 3)]
	[InlineData(ScriptOpcode.SetFlag, 2)]
	[InlineData(ScriptOpcode.CheckFlag, 4)]
	[InlineData(ScriptOpcode.GiveItem, 1)]
	[InlineData(ScriptOpcode.GiveGold, 2)]
	[InlineData(ScriptOpcode.Warp, 4)]
	[InlineData(ScriptOpcode.StartBattle, 2)]
	[InlineData(ScriptOpcode.AddPartyMember, 1)]
	public void GetParameterCount_ReturnsCorrectCount(ScriptOpcode opcode, int expectedCount) {
		Assert.Equal(expectedCount, ScriptCommand.GetParameterCount(opcode));
	}

	[Fact]
	public void ScriptCommand_Description_ContainsOpcodeInfo() {
		var cmd = new ScriptCommand {
			Opcode = ScriptOpcode.GiveItem,
			Parameters = [0x15]
		};

		Assert.Contains("item", cmd.Description.ToLower());
		Assert.Contains("21", cmd.Description); // 0x15 = 21
	}

	// ============================================================
	// EventScriptBuilder Tests
	// ============================================================

	[Fact]
	public void EventScriptBuilder_SimpleScript_BuildsCorrectly() {
		var script = new EventScriptBuilder(0x0001)
			.WithName("Test Script")
			.WithCategory(ScriptCategory.Dialog)
			.ShowDialog(0x0001)
			.End()
			.Build();

		Assert.Equal((ushort)0x0001, script.Id);
		Assert.Equal("Test Script", script.Name);
		Assert.Equal(ScriptCategory.Dialog, script.Category);
		Assert.Equal(2, script.Commands.Count);
	}

	[Fact]
	public void EventScriptBuilder_ForChapter_SetsChapterId() {
		var script = new EventScriptBuilder(0x0001)
			.ForChapter(0)
			.End()
			.Build();

		Assert.Equal(0, script.ChapterId);
	}

	[Fact]
	public void EventScriptBuilder_FlowControl_AddsCommands() {
		var script = new EventScriptBuilder(0x0001)
			.Nop()
			.Jump(0x0010)
			.JumpSubroutine(0x0020)
			.Return()
			.End()
			.Build();

		Assert.Equal(5, script.Commands.Count);
		Assert.Equal(ScriptOpcode.Nop, script.Commands[0].Opcode);
		Assert.Equal(ScriptOpcode.Jump, script.Commands[1].Opcode);
		Assert.Equal(ScriptOpcode.JumpSubroutine, script.Commands[2].Opcode);
		Assert.Equal(ScriptOpcode.Return, script.Commands[3].Opcode);
		Assert.Equal(ScriptOpcode.End, script.Commands[4].Opcode);
	}

	[Fact]
	public void EventScriptBuilder_Dialog_AddsCommands() {
		var script = new EventScriptBuilder(0x0001)
			.ShowDialog(0x1234)
			.ShowChoice(0x5678, 3)
			.End()
			.Build();

		Assert.Equal(3, script.Commands.Count);
		Assert.Equal(ScriptOpcode.ShowDialog, script.Commands[0].Opcode);
		Assert.Equal(ScriptOpcode.ShowChoice, script.Commands[1].Opcode);
	}

	[Fact]
	public void EventScriptBuilder_Flags_AddsCommands() {
		var script = new EventScriptBuilder(0x0001)
			.SetFlag(0x0005)
			.ClearFlag(0x0006)
			.CheckFlag(0x0007, 0x0010)
			.End()
			.Build();

		Assert.Equal(4, script.Commands.Count);
		Assert.Equal(ScriptOpcode.SetFlag, script.Commands[0].Opcode);
		Assert.Equal(ScriptOpcode.ClearFlag, script.Commands[1].Opcode);
		Assert.Equal(ScriptOpcode.CheckFlag, script.Commands[2].Opcode);
	}

	[Fact]
	public void EventScriptBuilder_Items_AddsCommands() {
		var script = new EventScriptBuilder(0x0001)
			.GiveItem(0x15)
			.TakeItem(0x20)
			.CheckItem(0x25, 0x0010)
			.End()
			.Build();

		Assert.Equal(4, script.Commands.Count);
		Assert.Equal(ScriptOpcode.GiveItem, script.Commands[0].Opcode);
		Assert.Equal(ScriptOpcode.TakeItem, script.Commands[1].Opcode);
		Assert.Equal(ScriptOpcode.CheckItem, script.Commands[2].Opcode);
	}

	[Fact]
	public void EventScriptBuilder_Gold_AddsCommands() {
		var script = new EventScriptBuilder(0x0001)
			.GiveGold(100)
			.TakeGold(50)
			.End()
			.Build();

		Assert.Equal(3, script.Commands.Count);
		Assert.Equal(ScriptOpcode.GiveGold, script.Commands[0].Opcode);
		Assert.Equal(ScriptOpcode.TakeGold, script.Commands[1].Opcode);
	}

	[Fact]
	public void EventScriptBuilder_Party_AddsCommands() {
		var script = new EventScriptBuilder(0x0001)
			.HealParty()
			.HealMember(1)
			.AddPartyMember(0x06)
			.RemovePartyMember(0x06)
			.GiveExp(500)
			.End()
			.Build();

		Assert.Equal(6, script.Commands.Count);
	}

	[Fact]
	public void EventScriptBuilder_Movement_AddsCommands() {
		var script = new EventScriptBuilder(0x0001)
			.Warp(0x10, 5, 5, 0)
			.FaceDirection(0, 2)
			.ShowNpc(0xC5)
			.HideNpc(0xC5)
			.End()
			.Build();

		Assert.Equal(5, script.Commands.Count);
		Assert.Equal(ScriptOpcode.Warp, script.Commands[0].Opcode);
	}

	[Fact]
	public void EventScriptBuilder_AudioVisual_AddsCommands() {
		var script = new EventScriptBuilder(0x0001)
			.FadeOut(4)
			.Wait(30)
			.PlayMusic(0x10)
			.PlaySound(0x05)
			.FadeIn(4)
			.StopMusic()
			.End()
			.Build();

		Assert.Equal(7, script.Commands.Count);
	}

	[Fact]
	public void EventScriptBuilder_Services_AddsCommands() {
		var script = new EventScriptBuilder(0x0001)
			.OpenShop(0x01)
			.OpenInn(0x01, 8)
			.OpenChurch()
			.OpenVault()
			.End()
			.Build();

		Assert.Equal(5, script.Commands.Count);
	}

	[Fact]
	public void EventScriptBuilder_Battle_AddsCommands() {
		var script = new EventScriptBuilder(0x0001)
			.StartBattle(0x0001)
			.End()
			.Build();

		Assert.Equal(2, script.Commands.Count);
		Assert.Equal(ScriptOpcode.StartBattle, script.Commands[0].Opcode);
	}

	// ============================================================
	// Opcode Enum Tests
	// ============================================================

	[Theory]
	[InlineData(ScriptOpcode.End, 0x00)]
	[InlineData(ScriptOpcode.Return, 0x01)]
	[InlineData(ScriptOpcode.ShowDialog, 0x10)]
	[InlineData(ScriptOpcode.SetFlag, 0x20)]
	[InlineData(ScriptOpcode.GiveItem, 0x30)]
	[InlineData(ScriptOpcode.GiveGold, 0x40)]
	[InlineData(ScriptOpcode.Heal, 0x50)]
	[InlineData(ScriptOpcode.Warp, 0x60)]
	[InlineData(ScriptOpcode.StartBattle, 0x70)]
	[InlineData(ScriptOpcode.FadeOut, 0x80)]
	[InlineData(ScriptOpcode.OpenShop, 0x90)]
	[InlineData(ScriptOpcode.SetVar, 0xa0)]
	[InlineData(ScriptOpcode.SetTimer, 0xb0)]
	public void ScriptOpcode_HasCorrectByteValue(ScriptOpcode opcode, byte expectedValue) {
		Assert.Equal(expectedValue, (byte)opcode);
	}

	// ============================================================
	// Category Tests
	// ============================================================

	[Theory]
	[InlineData(ScriptCategory.Dialog)]
	[InlineData(ScriptCategory.Cutscene)]
	[InlineData(ScriptCategory.Battle)]
	[InlineData(ScriptCategory.Story)]
	[InlineData(ScriptCategory.Shop)]
	[InlineData(ScriptCategory.Inn)]
	[InlineData(ScriptCategory.Item)]
	[InlineData(ScriptCategory.NPC)]
	[InlineData(ScriptCategory.Trigger)]
	[InlineData(ScriptCategory.System)]
	public void ScriptCategory_AllValues_AreDefined(ScriptCategory category) {
		Assert.True(Enum.IsDefined(typeof(ScriptCategory), category));
	}
}

/// <summary>
/// Unit tests for EventScriptConverter.
/// </summary>
public class EventScriptConverterTests {
	[Fact]
	public void Convert_AppliesScriptIdOffset() {
		var dw4Script = new EventScript { Id = 0x0001 };

		var dq3rScript = EventScriptConverter.Convert(dw4Script);

		Assert.Equal((ushort)0x1001, dq3rScript.Id); // 0x0001 + 0x1000
		Assert.Equal((ushort)0x0001, dq3rScript.SourceScriptId);
	}

	[Fact]
	public void Convert_PreservesMetadata() {
		var dw4Script = new EventScript {
			Id = 0x0001,
			Name = "Test Script",
			Category = ScriptCategory.Dialog,
			ChapterId = 0
		};

		var dq3rScript = EventScriptConverter.Convert(dw4Script);

		Assert.Equal("Test Script", dq3rScript.Name);
		Assert.Equal(DQ3rScriptCategory.Message, dq3rScript.Category);
		Assert.Equal(0, dq3rScript.ChapterId);
	}

	[Fact]
	public void Convert_ConvertsAllCommands() {
		var dw4Script = new EventScript {
			Commands = [
				new ScriptCommand { Opcode = ScriptOpcode.ShowDialog, Parameters = [0x01, 0x00] },
				new ScriptCommand { Opcode = ScriptOpcode.SetFlag, Parameters = [0x05, 0x00] },
				new ScriptCommand { Opcode = ScriptOpcode.End, Parameters = [] }
			]
		};

		var dq3rScript = EventScriptConverter.Convert(dw4Script);

		Assert.Equal(3, dq3rScript.Commands.Count);
	}

	[Theory]
	[InlineData(ScriptOpcode.End, DQ3rScriptOpcode.End)]
	[InlineData(ScriptOpcode.Return, DQ3rScriptOpcode.Return)]
	[InlineData(ScriptOpcode.ShowDialog, DQ3rScriptOpcode.Message)]
	[InlineData(ScriptOpcode.ShowChoice, DQ3rScriptOpcode.Choice)]
	[InlineData(ScriptOpcode.SetFlag, DQ3rScriptOpcode.SetFlag)]
	[InlineData(ScriptOpcode.GiveItem, DQ3rScriptOpcode.AddItem)]
	[InlineData(ScriptOpcode.GiveGold, DQ3rScriptOpcode.AddGold)]
	[InlineData(ScriptOpcode.Heal, DQ3rScriptOpcode.Heal)]
	[InlineData(ScriptOpcode.Warp, DQ3rScriptOpcode.Warp)]
	[InlineData(ScriptOpcode.StartBattle, DQ3rScriptOpcode.StartBattle)]
	[InlineData(ScriptOpcode.OpenShop, DQ3rScriptOpcode.OpenShop)]
	public void ConvertOpcode_MapsCorrectly(ScriptOpcode dw4Opcode, DQ3rScriptOpcode expectedDq3rOpcode) {
		Assert.Equal(expectedDq3rOpcode, EventScriptConverter.ConvertOpcode(dw4Opcode));
	}

	[Fact]
	public void ConvertParameters_DialogId_AppliesOffset() {
		var dw4Params = new byte[] { 0x01, 0x00 }; // Dialog ID 0x0001

		var dq3rParams = EventScriptConverter.ConvertParameters(ScriptOpcode.ShowDialog, dw4Params);

		Assert.Single(dq3rParams);
		Assert.Equal((ushort)0x1001, dq3rParams[0]); // 0x0001 + 0x1000
	}

	[Fact]
	public void ConvertParameters_FlagId_AppliesOffset() {
		var dw4Params = new byte[] { 0x05, 0x00 }; // Flag ID 0x0005

		var dq3rParams = EventScriptConverter.ConvertParameters(ScriptOpcode.SetFlag, dw4Params);

		Assert.Single(dq3rParams);
		Assert.Equal((ushort)0x0205, dq3rParams[0]); // 0x0005 + 0x0200
	}

	[Fact]
	public void ConvertParameters_Gold_ScalesValue() {
		var dw4Params = new byte[] { 0x64, 0x00 }; // 100 gold

		var dq3rParams = EventScriptConverter.ConvertParameters(ScriptOpcode.GiveGold, dw4Params);

		Assert.Single(dq3rParams);
		Assert.Equal((ushort)150, dq3rParams[0]); // 100 * 1.5
	}

	[Fact]
	public void ConvertParameters_Exp_ScalesValue() {
		var dw4Params = new byte[] { 0xF4, 0x01 }; // 500 exp

		var dq3rParams = EventScriptConverter.ConvertParameters(ScriptOpcode.GiveExp, dw4Params);

		Assert.Single(dq3rParams);
		Assert.Equal((ushort)600, dq3rParams[0]); // 500 * 1.2
	}

	[Fact]
	public void ConvertParameters_BattleId_AppliesOffset() {
		var dw4Params = new byte[] { 0x01, 0x00 }; // Battle ID 0x0001

		var dq3rParams = EventScriptConverter.ConvertParameters(ScriptOpcode.StartBattle, dw4Params);

		Assert.Single(dq3rParams);
		Assert.Equal((ushort)0x0101, dq3rParams[0]); // 0x0001 + 0x0100
	}

	[Fact]
	public void ConvertParameters_Warp_AppliesMapOffset() {
		var dw4Params = new byte[] { 0x10, 5, 5, 0 }; // Map 0x10, coords (5,5)

		var dq3rParams = EventScriptConverter.ConvertParameters(ScriptOpcode.Warp, dw4Params);

		Assert.Equal(4, dq3rParams.Length);
		Assert.Equal((ushort)0x210, dq3rParams[0]); // Map 0x10 + 0x200
		Assert.Equal((ushort)5, dq3rParams[1]);
		Assert.Equal((ushort)5, dq3rParams[2]);
	}

	[Theory]
	[InlineData(ScriptCategory.Dialog, DQ3rScriptCategory.Message)]
	[InlineData(ScriptCategory.Cutscene, DQ3rScriptCategory.Cutscene)]
	[InlineData(ScriptCategory.Battle, DQ3rScriptCategory.Battle)]
	[InlineData(ScriptCategory.Story, DQ3rScriptCategory.Story)]
	[InlineData(ScriptCategory.Shop, DQ3rScriptCategory.Service)]
	[InlineData(ScriptCategory.Inn, DQ3rScriptCategory.Service)]
	public void ConvertCategory_MapsCorrectly(ScriptCategory dw4Category, DQ3rScriptCategory expectedDq3rCategory) {
		Assert.Equal(expectedDq3rCategory, EventScriptConverter.ConvertCategory(dw4Category));
	}

	// ============================================================
	// DQ3rScriptCommand Tests
	// ============================================================

	[Fact]
	public void DQ3rScriptCommand_Size_Includes16BitOpcodeAndParams() {
		var cmd = new DQ3rScriptCommand {
			Opcode = DQ3rScriptOpcode.Warp,
			Parameters = [0x0210, 5, 5, 0]
		};

		Assert.Equal(10, cmd.Size); // 2 (opcode) + 4*2 (params)
	}

	[Fact]
	public void DQ3rScriptCommand_ToBytes_Serializes16Bit() {
		var cmd = new DQ3rScriptCommand {
			Opcode = DQ3rScriptOpcode.Message, // 0x0100
			Parameters = [0x1001] // Dialog ID
		};

		var bytes = cmd.ToBytes();

		Assert.Equal(4, bytes.Length);
		Assert.Equal(0x00, bytes[0]); // Opcode low
		Assert.Equal(0x01, bytes[1]); // Opcode high
		Assert.Equal(0x01, bytes[2]); // Param low
		Assert.Equal(0x10, bytes[3]); // Param high
	}

	// ============================================================
	// DQ3rEventScript Tests
	// ============================================================

	[Fact]
	public void DQ3rEventScript_ToBytes_SerializesAllCommands() {
		var script = new DQ3rEventScript {
			Commands = [
				new DQ3rScriptCommand { Opcode = DQ3rScriptOpcode.Message, Parameters = [0x1001] },
				new DQ3rScriptCommand { Opcode = DQ3rScriptOpcode.End, Parameters = [] }
			]
		};

		var bytes = script.ToBytes();

		Assert.Equal(6, bytes.Length); // 4 (Message) + 2 (End)
	}
}

/// <summary>
/// Unit tests for Chapter 1 Events.
/// </summary>
public class Chapter1EventsTests {
	[Fact]
	public void GetAllScripts_ReturnsAllChapter1Scripts() {
		var scripts = Chapter1Events.GetAllScripts();

		Assert.Equal(10, scripts.Length);
	}

	[Fact]
	public void IntroScript_HasCorrectId() {
		var scripts = Chapter1Events.GetAllScripts();
		var intro = scripts.First(s => s.Name == "Chapter 1 Intro");

		Assert.Equal(Chapter1Events.IntroScript, intro.Id);
		Assert.Equal(ScriptCategory.Cutscene, intro.Category);
		Assert.Equal(0, intro.ChapterId);
	}

	[Fact]
	public void IntroScript_ContainsRequiredCommands() {
		var intro = Chapter1Events.BuildIntroScript();

		Assert.Contains(intro.Commands, c => c.Opcode == ScriptOpcode.FadeOut);
		Assert.Contains(intro.Commands, c => c.Opcode == ScriptOpcode.PlayMusic);
		Assert.Contains(intro.Commands, c => c.Opcode == ScriptOpcode.FadeIn);
		Assert.Contains(intro.Commands, c => c.Opcode == ScriptOpcode.ShowDialog);
		Assert.Contains(intro.Commands, c => c.Opcode == ScriptOpcode.SetFlag);
		Assert.Contains(intro.Commands, c => c.Opcode == ScriptOpcode.End);
	}

	[Fact]
	public void MeetHealieScript_SetsHealieFlag() {
		var script = Chapter1Events.BuildMeetHealieScript();

		Assert.Contains(script.Commands, c =>
			c.Opcode == ScriptOpcode.SetFlag &&
			c.Parameters.Length >= 2 &&
			(c.Parameters[0] | (c.Parameters[1] << 8)) == Chapter1Events.FlagMetHealie);
	}

	[Fact]
	public void HealieJoinsScript_AddsPartyMember() {
		var script = Chapter1Events.BuildHealieJoinsScript();

		Assert.Contains(script.Commands, c =>
			c.Opcode == ScriptOpcode.AddPartyMember &&
			c.Parameters.Length >= 1 &&
			c.Parameters[0] == 0xC5); // Healie ID
	}

	[Fact]
	public void FlyingShoesScript_GivesItem() {
		var script = Chapter1Events.BuildFlyingShoesScript();

		Assert.Contains(script.Commands, c =>
			c.Opcode == ScriptOpcode.GiveItem &&
			c.Parameters.Length >= 1 &&
			c.Parameters[0] == 0x2A); // Flying Shoes
	}

	[Fact]
	public void SaroShadowBattleScript_StartsBattle() {
		var script = Chapter1Events.BuildSaroShadowBattleScript();

		Assert.Contains(script.Commands, c =>
			c.Opcode == ScriptOpcode.StartBattle);
	}

	[Fact]
	public void ReturnToKingScript_GivesRewards() {
		var script = Chapter1Events.BuildReturnToKingScript();

		Assert.Contains(script.Commands, c => c.Opcode == ScriptOpcode.GiveGold);
		Assert.Contains(script.Commands, c => c.Opcode == ScriptOpcode.GiveItem);
	}

	[Fact]
	public void ChapterCompleteScript_AdvancesChapter() {
		var script = Chapter1Events.BuildChapterCompleteScript();

		Assert.Contains(script.Commands, c =>
			c.Opcode == ScriptOpcode.SetChapter &&
			c.Parameters.Length >= 1 &&
			c.Parameters[0] == 1); // Advance to Chapter 2
	}

	[Fact]
	public void ShopScripts_OpenCorrectShops() {
		var weaponShop = Chapter1Events.BuildWeaponShopScript();
		var armorShop = Chapter1Events.BuildArmorShopScript();
		var itemShop = Chapter1Events.BuildItemShopScript();

		Assert.Contains(weaponShop.Commands, c => c.Opcode == ScriptOpcode.OpenShop);
		Assert.Contains(armorShop.Commands, c => c.Opcode == ScriptOpcode.OpenShop);
		Assert.Contains(itemShop.Commands, c => c.Opcode == ScriptOpcode.OpenShop);
	}

	[Fact]
	public void InnScript_OpensInnWithPrice() {
		var inn = Chapter1Events.BuildInnScript();

		var innCmd = inn.Commands.First(c => c.Opcode == ScriptOpcode.OpenInn);
		Assert.Equal(0x01, innCmd.Parameters[0]); // Inn ID
		Assert.Equal(8, innCmd.Parameters[1]); // Price
	}

	[Fact]
	public void ChurchScript_OpensChurch() {
		var church = Chapter1Events.BuildChurchScript();

		Assert.Contains(church.Commands, c => c.Opcode == ScriptOpcode.OpenChurch);
	}

	// ============================================================
	// Script Constants Tests
	// ============================================================

	[Fact]
	public void ScriptBase_IsCorrect() {
		Assert.Equal((ushort)0x0100, Chapter1Events.ScriptBase);
	}

	[Fact]
	public void FlagIds_AreSequential() {
		Assert.Equal((ushort)0x0001, Chapter1Events.FlagKingMission);
		Assert.Equal((ushort)0x0002, Chapter1Events.FlagChildrenInfo);
		Assert.Equal((ushort)0x0003, Chapter1Events.FlagMetHealie);
		Assert.Equal((ushort)0x0004, Chapter1Events.FlagHealieJoined);
		Assert.Equal((ushort)0x0005, Chapter1Events.FlagFlyingShoes);
		Assert.Equal((ushort)0x0006, Chapter1Events.FlagLochTower);
		Assert.Equal((ushort)0x0007, Chapter1Events.FlagSaroDefeated);
		Assert.Equal((ushort)0x0008, Chapter1Events.FlagChildrenRescued);
		Assert.Equal((ushort)0x0009, Chapter1Events.FlagReportedKing);
		Assert.Equal((ushort)0x000A, Chapter1Events.FlagChapterComplete);
	}

	[Fact]
	public void BattleSaroShadow_HasCorrectId() {
		Assert.Equal((ushort)0x0001, Chapter1Events.BattleSaroShadow);
	}

	// ============================================================
	// Full Conversion Test
	// ============================================================

	[Fact]
	public void FullScriptConversion_ProducesValidDQ3rScript() {
		// Build DW4 script
		var dw4Script = Chapter1Events.BuildIntroScript();

		// Convert to DQ3r
		var dq3rScript = EventScriptConverter.Convert(dw4Script);

		// Verify
		Assert.Equal((ushort)(Chapter1Events.IntroScript + 0x1000), dq3rScript.Id);
		Assert.Equal(dw4Script.Commands.Count, dq3rScript.Commands.Count);

		// Verify bytes can be generated
		var bytes = dq3rScript.ToBytes();
		Assert.NotEmpty(bytes);
	}
}
