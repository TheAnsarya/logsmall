namespace DW4Lib.Events;

/// <summary>
/// Converts DW4 event scripts to DQ3r format.
/// DQ3r uses a more complex 16-bit scripting system with extended opcodes.
/// </summary>
public static class EventScriptConverter {
	/// <summary>
	/// Offset applied to DW4 script IDs when converting to DQ3r.
	/// </summary>
	public const ushort ScriptIdOffset = 0x1000;

	/// <summary>
	/// Offset applied to DW4 dialog IDs when converting to DQ3r.
	/// </summary>
	public const ushort DialogIdOffset = 0x1000;

	/// <summary>
	/// Offset applied to DW4 flag IDs when converting to DQ3r.
	/// </summary>
	public const ushort FlagIdOffset = 0x0200;

	/// <summary>
	/// Offset applied to DW4 battle IDs when converting to DQ3r.
	/// </summary>
	public const ushort BattleIdOffset = 0x0100;

	/// <summary>
	/// Convert DW4 event script to DQ3r format.
	/// </summary>
	public static DQ3rEventScript Convert(EventScript dw4Script) {
		var dq3rScript = new DQ3rEventScript {
			Id = (ushort)(dw4Script.Id + ScriptIdOffset),
			Name = dw4Script.Name,
			SourceScriptId = dw4Script.Id,
			Category = ConvertCategory(dw4Script.Category),
			ChapterId = dw4Script.ChapterId
		};

		foreach (var cmd in dw4Script.Commands) {
			dq3rScript.Commands.Add(ConvertCommand(cmd));
		}

		return dq3rScript;
	}

	/// <summary>
	/// Convert DW4 script command to DQ3r format.
	/// </summary>
	public static DQ3rScriptCommand ConvertCommand(ScriptCommand dw4Cmd) {
		var opcode = ConvertOpcode(dw4Cmd.Opcode);
		var parameters = ConvertParameters(dw4Cmd.Opcode, dw4Cmd.Parameters);

		return new DQ3rScriptCommand {
			Opcode = opcode,
			Parameters = parameters
		};
	}

	/// <summary>
	/// Convert DW4 opcode to DQ3r opcode.
	/// </summary>
	public static DQ3rScriptOpcode ConvertOpcode(ScriptOpcode dw4Opcode) => dw4Opcode switch {
		// Flow control
		ScriptOpcode.End => DQ3rScriptOpcode.End,
		ScriptOpcode.Return => DQ3rScriptOpcode.Return,
		ScriptOpcode.Nop => DQ3rScriptOpcode.Nop,
		ScriptOpcode.Jump => DQ3rScriptOpcode.Jump,
		ScriptOpcode.JumpSubroutine => DQ3rScriptOpcode.Call,

		// Dialog
		ScriptOpcode.ShowDialog => DQ3rScriptOpcode.Message,
		ScriptOpcode.ShowChoice => DQ3rScriptOpcode.Choice,
		ScriptOpcode.ShowName => DQ3rScriptOpcode.ShowName,

		// Flags
		ScriptOpcode.SetFlag => DQ3rScriptOpcode.SetFlag,
		ScriptOpcode.ClearFlag => DQ3rScriptOpcode.ClearFlag,
		ScriptOpcode.CheckFlag => DQ3rScriptOpcode.BranchOnFlag,
		ScriptOpcode.ToggleFlag => DQ3rScriptOpcode.ToggleFlag,

		// Items
		ScriptOpcode.GiveItem => DQ3rScriptOpcode.AddItem,
		ScriptOpcode.TakeItem => DQ3rScriptOpcode.RemoveItem,
		ScriptOpcode.CheckItem => DQ3rScriptOpcode.BranchOnItem,
		ScriptOpcode.UseItem => DQ3rScriptOpcode.UseItem,

		// Gold
		ScriptOpcode.GiveGold => DQ3rScriptOpcode.AddGold,
		ScriptOpcode.TakeGold => DQ3rScriptOpcode.SubtractGold,
		ScriptOpcode.CheckGold => DQ3rScriptOpcode.BranchOnGold,

		// Party/Character
		ScriptOpcode.Heal => DQ3rScriptOpcode.Heal,
		ScriptOpcode.RestoreMP => DQ3rScriptOpcode.RestoreMP,
		ScriptOpcode.Revive => DQ3rScriptOpcode.Revive,
		ScriptOpcode.CurePoison => DQ3rScriptOpcode.CureStatus,
		ScriptOpcode.CureStatus => DQ3rScriptOpcode.CureStatus,
		ScriptOpcode.AddPartyMember => DQ3rScriptOpcode.AddPartyMember,
		ScriptOpcode.RemovePartyMember => DQ3rScriptOpcode.RemovePartyMember,
		ScriptOpcode.CheckPartyMember => DQ3rScriptOpcode.BranchOnPartyMember,
		ScriptOpcode.SetChapter => DQ3rScriptOpcode.SetGameState,
		ScriptOpcode.CheckChapter => DQ3rScriptOpcode.BranchOnGameState,
		ScriptOpcode.GiveExp => DQ3rScriptOpcode.AddExp,
		ScriptOpcode.LevelUp => DQ3rScriptOpcode.LevelUp,
		ScriptOpcode.LearnSpell => DQ3rScriptOpcode.LearnSpell,

		// Movement
		ScriptOpcode.Warp => DQ3rScriptOpcode.Warp,
		ScriptOpcode.MovePc => DQ3rScriptOpcode.MovePlayer,
		ScriptOpcode.MoveNpc => DQ3rScriptOpcode.MoveNpc,
		ScriptOpcode.FaceDirection => DQ3rScriptOpcode.Face,
		ScriptOpcode.ShowNpc => DQ3rScriptOpcode.ShowSprite,
		ScriptOpcode.HideNpc => DQ3rScriptOpcode.HideSprite,
		ScriptOpcode.TeleportParty => DQ3rScriptOpcode.Teleport,

		// Battle
		ScriptOpcode.StartBattle => DQ3rScriptOpcode.StartBattle,
		ScriptOpcode.CheckBattleWon => DQ3rScriptOpcode.BranchOnBattleResult,
		ScriptOpcode.ForceEscape => DQ3rScriptOpcode.EndBattle,
		ScriptOpcode.SetBattleBgm => DQ3rScriptOpcode.SetBattleMusic,

		// Audio/Visual
		ScriptOpcode.FadeOut => DQ3rScriptOpcode.FadeOut,
		ScriptOpcode.FadeIn => DQ3rScriptOpcode.FadeIn,
		ScriptOpcode.Wait => DQ3rScriptOpcode.Wait,
		ScriptOpcode.PlaySound => DQ3rScriptOpcode.PlaySfx,
		ScriptOpcode.PlayMusic => DQ3rScriptOpcode.PlayBgm,
		ScriptOpcode.StopMusic => DQ3rScriptOpcode.StopBgm,
		ScriptOpcode.ScreenFlash => DQ3rScriptOpcode.Flash,
		ScriptOpcode.ScreenShake => DQ3rScriptOpcode.Shake,
		ScriptOpcode.ShowSprite => DQ3rScriptOpcode.ShowSprite,
		ScriptOpcode.HideSprite => DQ3rScriptOpcode.HideSprite,
		ScriptOpcode.Animation => DQ3rScriptOpcode.PlayAnimation,

		// Services
		ScriptOpcode.OpenShop => DQ3rScriptOpcode.OpenShop,
		ScriptOpcode.OpenInn => DQ3rScriptOpcode.OpenInn,
		ScriptOpcode.OpenChurch => DQ3rScriptOpcode.OpenChurch,
		ScriptOpcode.OpenVault => DQ3rScriptOpcode.OpenVault,
		ScriptOpcode.OpenBank => DQ3rScriptOpcode.OpenBank,

		// Variables
		ScriptOpcode.SetVar => DQ3rScriptOpcode.SetVariable,
		ScriptOpcode.CheckVar => DQ3rScriptOpcode.BranchOnVariable,
		ScriptOpcode.AddVar => DQ3rScriptOpcode.AddVariable,
		ScriptOpcode.SubVar => DQ3rScriptOpcode.SubtractVariable,

		// Timer
		ScriptOpcode.SetTimer => DQ3rScriptOpcode.SetTimer,
		ScriptOpcode.CheckTimer => DQ3rScriptOpcode.BranchOnTimer,
		ScriptOpcode.ClearTimer => DQ3rScriptOpcode.ClearTimer,

		_ => DQ3rScriptOpcode.Nop
	};

	/// <summary>
	/// Convert command parameters, applying ID offsets as needed.
	/// </summary>
	public static ushort[] ConvertParameters(ScriptOpcode opcode, byte[] parameters) {
		return opcode switch {
			// Dialog commands need dialog ID offset
			ScriptOpcode.ShowDialog => ConvertDialogParams(parameters),
			ScriptOpcode.ShowChoice => ConvertChoiceParams(parameters),

			// Flag commands need flag ID offset
			ScriptOpcode.SetFlag => ConvertFlagParams(parameters),
			ScriptOpcode.ClearFlag => ConvertFlagParams(parameters),
			ScriptOpcode.CheckFlag => ConvertFlagCheckParams(parameters),

			// Item commands need item ID conversion
			ScriptOpcode.GiveItem => [Converters.ItemIdConverter.ConvertToDQ3r(parameters[0])],
			ScriptOpcode.TakeItem => [Converters.ItemIdConverter.ConvertToDQ3r(parameters[0])],
			ScriptOpcode.CheckItem => ConvertItemCheckParams(parameters),

			// Gold scaled 1.5x
			ScriptOpcode.GiveGold => [ScaleGold(GetWord(parameters, 0))],
			ScriptOpcode.TakeGold => [ScaleGold(GetWord(parameters, 0))],
			ScriptOpcode.CheckGold => [ScaleGold(GetWord(parameters, 0)), (ushort)(GetWord(parameters, 2) + ScriptIdOffset)],

			// Character IDs offset
			ScriptOpcode.AddPartyMember => [(ushort)(Converters.ItemIdConverter.CharacterIdOffset + parameters[0])],
			ScriptOpcode.RemovePartyMember => [(ushort)(Converters.ItemIdConverter.CharacterIdOffset + parameters[0])],

			// Battle ID offset
			ScriptOpcode.StartBattle => [(ushort)(GetWord(parameters, 0) + BattleIdOffset)],

			// EXP scaled 1.2x
			ScriptOpcode.GiveExp => [ScaleExp(GetWord(parameters, 0))],

			// Warp needs map ID conversion
			ScriptOpcode.Warp => ConvertWarpParams(parameters),

			// Default: just expand bytes to words
			_ => ExpandToWords(parameters)
		};
	}

	/// <summary>
	/// Convert script category.
	/// </summary>
	public static DQ3rScriptCategory ConvertCategory(ScriptCategory category) => category switch {
		ScriptCategory.Dialog => DQ3rScriptCategory.Message,
		ScriptCategory.Cutscene => DQ3rScriptCategory.Cutscene,
		ScriptCategory.Battle => DQ3rScriptCategory.Battle,
		ScriptCategory.Story => DQ3rScriptCategory.Story,
		ScriptCategory.Shop => DQ3rScriptCategory.Service,
		ScriptCategory.Inn => DQ3rScriptCategory.Service,
		ScriptCategory.Item => DQ3rScriptCategory.Item,
		ScriptCategory.NPC => DQ3rScriptCategory.NPC,
		ScriptCategory.Trigger => DQ3rScriptCategory.Trigger,
		ScriptCategory.System => DQ3rScriptCategory.System,
		_ => DQ3rScriptCategory.General
	};

	private static ushort[] ConvertDialogParams(byte[] parameters) {
		var dialogId = GetWord(parameters, 0);
		return [(ushort)(dialogId + DialogIdOffset)];
	}

	private static ushort[] ConvertChoiceParams(byte[] parameters) {
		var dialogId = GetWord(parameters, 0);
		return [(ushort)(dialogId + DialogIdOffset), parameters[2]];
	}

	private static ushort[] ConvertFlagParams(byte[] parameters) {
		var flagId = GetWord(parameters, 0);
		return [(ushort)(flagId + FlagIdOffset)];
	}

	private static ushort[] ConvertFlagCheckParams(byte[] parameters) {
		var flagId = GetWord(parameters, 0);
		var branchAddr = GetWord(parameters, 2);
		return [(ushort)(flagId + FlagIdOffset), (ushort)(branchAddr + ScriptIdOffset)];
	}

	private static ushort[] ConvertItemCheckParams(byte[] parameters) {
		var itemId = Converters.ItemIdConverter.ConvertToDQ3r(parameters[0]);
		var branchAddr = GetWord(parameters, 1);
		return [itemId, (ushort)(branchAddr + ScriptIdOffset)];
	}

	private static ushort[] ConvertWarpParams(byte[] parameters) {
		// Map ID + 0x200 offset, coordinates unchanged
		return [
			(ushort)(parameters[0] + 0x200),
			parameters[1],
			parameters[2],
			parameters[3]
		];
	}

	private static ushort ScaleGold(ushort gold) => (ushort)(gold * 1.5);
	private static ushort ScaleExp(ushort exp) => (ushort)(exp * 1.2);

	private static ushort GetWord(byte[] data, int offset) {
		if (offset + 1 < data.Length) {
			return (ushort)(data[offset] | (data[offset + 1] << 8));
		}
		return offset < data.Length ? data[offset] : (ushort)0;
	}

	private static ushort[] ExpandToWords(byte[] bytes) {
		var words = new ushort[bytes.Length];
		for (int i = 0; i < bytes.Length; i++) {
			words[i] = bytes[i];
		}
		return words;
	}
}

/// <summary>
/// DQ3r SNES event script structure.
/// Uses 16-bit opcodes and parameters for extended functionality.
/// </summary>
public class DQ3rEventScript {
	/// <summary>
	/// Script ID (DQ3r namespace: 0x1000+).
	/// </summary>
	public ushort Id { get; set; }

	/// <summary>
	/// Original DW4 script ID.
	/// </summary>
	public ushort SourceScriptId { get; set; }

	/// <summary>
	/// Script name/label.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Script category.
	/// </summary>
	public DQ3rScriptCategory Category { get; set; }

	/// <summary>
	/// Chapter association.
	/// </summary>
	public int? ChapterId { get; set; }

	/// <summary>
	/// Script commands.
	/// </summary>
	public List<DQ3rScriptCommand> Commands { get; set; } = [];

	/// <summary>
	/// Serialize script to bytes.
	/// </summary>
	public byte[] ToBytes() {
		var bytes = new List<byte>();
		foreach (var cmd in Commands) {
			bytes.AddRange(cmd.ToBytes());
		}
		return [.. bytes];
	}
}

/// <summary>
/// DQ3r script command with 16-bit opcode and parameters.
/// </summary>
public class DQ3rScriptCommand {
	/// <summary>
	/// Command opcode.
	/// </summary>
	public DQ3rScriptOpcode Opcode { get; set; }

	/// <summary>
	/// Command parameters (16-bit values).
	/// </summary>
	public ushort[] Parameters { get; set; } = [];

	/// <summary>
	/// Total size in bytes.
	/// </summary>
	public int Size => 2 + (Parameters.Length * 2);

	/// <summary>
	/// Serialize command to bytes.
	/// </summary>
	public byte[] ToBytes() {
		var bytes = new byte[Size];
		bytes[0] = (byte)((ushort)Opcode & 0xFF);
		bytes[1] = (byte)(((ushort)Opcode >> 8) & 0xFF);

		for (int i = 0; i < Parameters.Length; i++) {
			bytes[2 + i * 2] = (byte)(Parameters[i] & 0xFF);
			bytes[2 + i * 2 + 1] = (byte)((Parameters[i] >> 8) & 0xFF);
		}

		return bytes;
	}
}

/// <summary>
/// DQ3r script opcodes (16-bit).
/// </summary>
public enum DQ3rScriptOpcode : ushort {
	// Flow control (0x00xx)
	End = 0x0000,
	Return = 0x0001,
	Nop = 0x0002,
	Jump = 0x0010,
	Call = 0x0011,
	BranchIfTrue = 0x0020,
	BranchIfFalse = 0x0021,

	// Dialog (0x01xx)
	Message = 0x0100,
	Choice = 0x0101,
	ShowName = 0x0102,
	ClearMessage = 0x0103,
	SetMessageSpeed = 0x0104,

	// Flags (0x02xx)
	SetFlag = 0x0200,
	ClearFlag = 0x0201,
	ToggleFlag = 0x0202,
	BranchOnFlag = 0x0210,

	// Items (0x03xx)
	AddItem = 0x0300,
	RemoveItem = 0x0301,
	UseItem = 0x0302,
	BranchOnItem = 0x0310,
	CheckInventoryFull = 0x0311,

	// Gold (0x04xx)
	AddGold = 0x0400,
	SubtractGold = 0x0401,
	BranchOnGold = 0x0410,

	// Party (0x05xx)
	Heal = 0x0500,
	RestoreMP = 0x0501,
	Revive = 0x0502,
	CureStatus = 0x0503,
	AddPartyMember = 0x0510,
	RemovePartyMember = 0x0511,
	BranchOnPartyMember = 0x0512,
	AddExp = 0x0520,
	LevelUp = 0x0521,
	LearnSpell = 0x0522,

	// Map/Movement (0x06xx)
	Warp = 0x0600,
	Teleport = 0x0601,
	MovePlayer = 0x0610,
	MoveNpc = 0x0611,
	Face = 0x0612,
	ShowSprite = 0x0620,
	HideSprite = 0x0621,
	SetNpcPath = 0x0622,

	// Battle (0x07xx)
	StartBattle = 0x0700,
	EndBattle = 0x0701,
	BranchOnBattleResult = 0x0710,
	SetBattleMusic = 0x0720,

	// Audio/Visual (0x08xx)
	FadeOut = 0x0800,
	FadeIn = 0x0801,
	Wait = 0x0802,
	PlaySfx = 0x0810,
	PlayBgm = 0x0811,
	StopBgm = 0x0812,
	Flash = 0x0820,
	Shake = 0x0821,
	PlayAnimation = 0x0830,

	// Services (0x09xx)
	OpenShop = 0x0900,
	OpenInn = 0x0901,
	OpenChurch = 0x0902,
	OpenVault = 0x0903,
	OpenBank = 0x0904,

	// Variables (0x0Axx)
	SetVariable = 0x0a00,
	AddVariable = 0x0a01,
	SubtractVariable = 0x0a02,
	BranchOnVariable = 0x0a10,

	// Timer (0x0Bxx)
	SetTimer = 0x0b00,
	ClearTimer = 0x0b01,
	BranchOnTimer = 0x0b10,

	// Game state (0x0Cxx)
	SetGameState = 0x0c00,
	BranchOnGameState = 0x0c10
}

/// <summary>
/// DQ3r script categories.
/// </summary>
public enum DQ3rScriptCategory {
	General,
	Message,
	Cutscene,
	Battle,
	Story,
	Service,
	Item,
	NPC,
	Trigger,
	System
}
