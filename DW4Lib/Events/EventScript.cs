namespace DW4Lib.Events;

/// <summary>
/// DW4 event scripting system.
/// Events control dialog, cutscenes, battles, and story progression.
/// Scripts are composed of opcodes with parameters.
/// </summary>
public class EventScript {
	/// <summary>
	/// Script ID (0x0000-0xFFFF).
	/// </summary>
	public ushort Id { get; set; }

	/// <summary>
	/// Script name/label for documentation.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Script category.
	/// </summary>
	public ScriptCategory Category { get; set; }

	/// <summary>
	/// Associated chapter (0-4, or null for global scripts).
	/// </summary>
	public int? ChapterId { get; set; }

	/// <summary>
	/// List of script commands.
	/// </summary>
	public List<ScriptCommand> Commands { get; set; } = [];

	/// <summary>
	/// Raw byte data.
	/// </summary>
	public byte[] RawData { get; set; } = [];

	/// <summary>
	/// ROM address where script is located.
	/// </summary>
	public int RomAddress { get; set; }

	/// <summary>
	/// Parse script from raw bytes.
	/// </summary>
	public static EventScript Parse(byte[] data, int offset, ushort scriptId) {
		var script = new EventScript { Id = scriptId, RomAddress = offset };
		var commands = new List<ScriptCommand>();

		int pos = offset;
		while (pos < data.Length) {
			var cmd = ScriptCommand.Parse(data, pos);
			commands.Add(cmd);
			pos += cmd.Size;

			// End of script
			if (cmd.Opcode == ScriptOpcode.End ||
				cmd.Opcode == ScriptOpcode.Return) {
				break;
			}
		}

		script.Commands = commands;
		script.RawData = data[offset..pos];
		return script;
	}

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
/// Script category.
/// </summary>
public enum ScriptCategory {
	Dialog,
	Cutscene,
	Battle,
	Story,
	Shop,
	Inn,
	Item,
	NPC,
	Trigger,
	System
}

/// <summary>
/// Individual script command with opcode and parameters.
/// </summary>
public class ScriptCommand {
	/// <summary>
	/// Command opcode.
	/// </summary>
	public ScriptOpcode Opcode { get; set; }

	/// <summary>
	/// Command parameters (opcode-dependent).
	/// </summary>
	public byte[] Parameters { get; set; } = [];

	/// <summary>
	/// Total size in bytes (opcode + parameters).
	/// </summary>
	public int Size => 1 + Parameters.Length;

	/// <summary>
	/// Human-readable description of this command.
	/// </summary>
	public string Description => GetDescription();

	/// <summary>
	/// Parse command from raw bytes.
	/// </summary>
	public static ScriptCommand Parse(byte[] data, int offset) {
		var opcode = (ScriptOpcode)data[offset];
		int paramCount = GetParameterCount(opcode);
		var parameters = new byte[paramCount];

		for (int i = 0; i < paramCount && offset + 1 + i < data.Length; i++) {
			parameters[i] = data[offset + 1 + i];
		}

		return new ScriptCommand { Opcode = opcode, Parameters = parameters };
	}

	/// <summary>
	/// Serialize command to bytes.
	/// </summary>
	public byte[] ToBytes() {
		var bytes = new byte[1 + Parameters.Length];
		bytes[0] = (byte)Opcode;
		Parameters.CopyTo(bytes, 1);
		return bytes;
	}

	/// <summary>
	/// Get parameter count for opcode.
	/// </summary>
	public static int GetParameterCount(ScriptOpcode opcode) => opcode switch {
		ScriptOpcode.End => 0,
		ScriptOpcode.Return => 0,
		ScriptOpcode.Nop => 0,
		ScriptOpcode.ShowDialog => 2,          // Dialog ID (16-bit)
		ScriptOpcode.ShowChoice => 3,          // Dialog ID + choice count
		ScriptOpcode.SetFlag => 2,             // Flag ID (16-bit)
		ScriptOpcode.ClearFlag => 2,           // Flag ID (16-bit)
		ScriptOpcode.CheckFlag => 4,           // Flag ID + branch address
		ScriptOpcode.GiveItem => 1,            // Item ID
		ScriptOpcode.TakeItem => 1,            // Item ID
		ScriptOpcode.CheckItem => 3,           // Item ID + branch address
		ScriptOpcode.GiveGold => 2,            // Gold amount (16-bit)
		ScriptOpcode.TakeGold => 2,            // Gold amount (16-bit)
		ScriptOpcode.CheckGold => 4,           // Amount + branch address
		ScriptOpcode.Heal => 1,                // Target (0=party, 1-8=member)
		ScriptOpcode.RestoreMP => 1,           // Target
		ScriptOpcode.Revive => 1,              // Target
		ScriptOpcode.CurePoison => 1,          // Target
		ScriptOpcode.Warp => 4,                // Map ID, X, Y, facing
		ScriptOpcode.FadeOut => 1,             // Speed
		ScriptOpcode.FadeIn => 1,              // Speed
		ScriptOpcode.Wait => 1,                // Frames
		ScriptOpcode.PlaySound => 1,           // Sound ID
		ScriptOpcode.PlayMusic => 1,           // Music ID
		ScriptOpcode.StopMusic => 0,
		ScriptOpcode.MovePc => 4,              // Direction, steps, speed, wait
		ScriptOpcode.MoveNpc => 5,             // NPC ID, direction, steps, speed, wait
		ScriptOpcode.FaceDirection => 2,       // Entity, direction
		ScriptOpcode.ShowNpc => 1,             // NPC ID
		ScriptOpcode.HideNpc => 1,             // NPC ID
		ScriptOpcode.StartBattle => 2,         // Battle ID (16-bit)
		ScriptOpcode.CheckBattleWon => 2,      // Branch address
		ScriptOpcode.AddPartyMember => 1,      // Character ID
		ScriptOpcode.RemovePartyMember => 1,   // Character ID
		ScriptOpcode.CheckPartyMember => 3,    // Char ID + branch address
		ScriptOpcode.SetChapter => 1,          // Chapter ID
		ScriptOpcode.CheckChapter => 3,        // Chapter ID + branch address
		ScriptOpcode.Jump => 2,                // Target address (16-bit)
		ScriptOpcode.JumpSubroutine => 2,      // Subroutine address
		ScriptOpcode.SetVar => 3,              // Var ID, value (16-bit)
		ScriptOpcode.CheckVar => 5,            // Var ID, value, branch address
		ScriptOpcode.AddVar => 3,              // Var ID, value
		ScriptOpcode.OpenShop => 1,            // Shop ID
		ScriptOpcode.OpenInn => 2,             // Inn ID, price
		ScriptOpcode.OpenChurch => 0,
		ScriptOpcode.OpenVault => 0,
		ScriptOpcode.OpenBank => 0,
		ScriptOpcode.GiveExp => 2,             // EXP amount (16-bit)
		ScriptOpcode.LevelUp => 1,             // Target member
		ScriptOpcode.LearnSpell => 2,          // Target, spell ID
		ScriptOpcode.ScreenFlash => 1,         // Color/style
		ScriptOpcode.ScreenShake => 1,         // Intensity
		ScriptOpcode.ShowSprite => 2,          // Sprite ID, position
		ScriptOpcode.HideSprite => 1,          // Sprite ID
		ScriptOpcode.Animation => 2,           // Animation ID, target
		ScriptOpcode.SetTimer => 2,            // Timer value (16-bit)
		ScriptOpcode.CheckTimer => 2,          // Branch address
		_ => 0
	};

	/// <summary>
	/// Get human-readable description.
	/// </summary>
	private string GetDescription() {
		return Opcode switch {
			ScriptOpcode.End => "End script",
			ScriptOpcode.Return => "Return from subroutine",
			ScriptOpcode.Nop => "No operation",
			ScriptOpcode.ShowDialog => $"Show dialog #{GetWord(0)}",
			ScriptOpcode.ShowChoice => $"Show choice dialog #{GetWord(0)} with {Parameters[2]} options",
			ScriptOpcode.SetFlag => $"Set flag ${GetWord(0):x4}",
			ScriptOpcode.ClearFlag => $"Clear flag ${GetWord(0):x4}",
			ScriptOpcode.CheckFlag => $"Check flag ${GetWord(0):x4}, branch to ${GetWord(2):x4}",
			ScriptOpcode.GiveItem => $"Give item #{Parameters[0]}",
			ScriptOpcode.TakeItem => $"Take item #{Parameters[0]}",
			ScriptOpcode.GiveGold => $"Give {GetWord(0)} gold",
			ScriptOpcode.TakeGold => $"Take {GetWord(0)} gold",
			ScriptOpcode.Heal => GetHealDescription(),
			ScriptOpcode.Warp => $"Warp to map ${Parameters[0]:x2} at ({Parameters[1]}, {Parameters[2]})",
			ScriptOpcode.StartBattle => $"Start battle #{GetWord(0)}",
			ScriptOpcode.AddPartyMember => $"Add character #{Parameters[0]} to party",
			ScriptOpcode.RemovePartyMember => $"Remove character #{Parameters[0]} from party",
			ScriptOpcode.OpenShop => $"Open shop #{Parameters[0]}",
			ScriptOpcode.OpenInn => $"Open inn #{Parameters[0]} (price: {Parameters[1]})",
			ScriptOpcode.PlayMusic => $"Play music #{Parameters[0]}",
			ScriptOpcode.PlaySound => $"Play sound #{Parameters[0]}",
			ScriptOpcode.FadeOut => $"Fade out (speed: {Parameters[0]})",
			ScriptOpcode.FadeIn => $"Fade in (speed: {Parameters[0]})",
			ScriptOpcode.Wait => $"Wait {Parameters[0]} frames",
			_ => $"{Opcode} ({string.Join(", ", Parameters.Select(p => $"${p:x2}"))})"
		};
	}

	private ushort GetWord(int paramIndex) {
		if (paramIndex + 1 < Parameters.Length) {
			return (ushort)(Parameters[paramIndex] | (Parameters[paramIndex + 1] << 8));
		}
		return 0;
	}

	private string GetHealDescription() {
		return Parameters[0] switch {
			0 => "Heal entire party",
			_ => $"Heal party member {Parameters[0]}"
		};
	}
}

/// <summary>
/// Script opcodes for DW4 event system.
/// Based on reverse engineering of NES ROM event data.
/// </summary>
public enum ScriptOpcode : byte {
	// Flow control
	End = 0x00,
	Return = 0x01,
	Nop = 0x02,
	Jump = 0x03,
	JumpSubroutine = 0x04,

	// Dialog
	ShowDialog = 0x10,
	ShowChoice = 0x11,
	ShowName = 0x12,

	// Flags
	SetFlag = 0x20,
	ClearFlag = 0x21,
	CheckFlag = 0x22,
	ToggleFlag = 0x23,

	// Items
	GiveItem = 0x30,
	TakeItem = 0x31,
	CheckItem = 0x32,
	UseItem = 0x33,

	// Gold
	GiveGold = 0x40,
	TakeGold = 0x41,
	CheckGold = 0x42,

	// Party/Character
	Heal = 0x50,
	RestoreMP = 0x51,
	Revive = 0x52,
	CurePoison = 0x53,
	CureStatus = 0x54,
	AddPartyMember = 0x58,
	RemovePartyMember = 0x59,
	CheckPartyMember = 0x5a,
	SetChapter = 0x5b,
	CheckChapter = 0x5c,
	GiveExp = 0x5d,
	LevelUp = 0x5e,
	LearnSpell = 0x5f,

	// Movement/Map
	Warp = 0x60,
	MovePc = 0x61,
	MoveNpc = 0x62,
	FaceDirection = 0x63,
	ShowNpc = 0x64,
	HideNpc = 0x65,
	TeleportParty = 0x66,

	// Battle
	StartBattle = 0x70,
	CheckBattleWon = 0x71,
	ForceEscape = 0x72,
	SetBattleBgm = 0x73,

	// Audio/Visual
	FadeOut = 0x80,
	FadeIn = 0x81,
	Wait = 0x82,
	PlaySound = 0x83,
	PlayMusic = 0x84,
	StopMusic = 0x85,
	ScreenFlash = 0x86,
	ScreenShake = 0x87,
	ShowSprite = 0x88,
	HideSprite = 0x89,
	Animation = 0x8a,

	// Services
	OpenShop = 0x90,
	OpenInn = 0x91,
	OpenChurch = 0x92,
	OpenVault = 0x93,
	OpenBank = 0x94,

	// Variables
	SetVar = 0xa0,
	CheckVar = 0xa1,
	AddVar = 0xa2,
	SubVar = 0xa3,

	// Timer
	SetTimer = 0xb0,
	CheckTimer = 0xb1,
	ClearTimer = 0xb2
}

/// <summary>
/// Script builder for creating event scripts programmatically.
/// </summary>
public class EventScriptBuilder {
	private readonly List<ScriptCommand> _commands = [];
	private ushort _scriptId;
	private string _name = string.Empty;
	private ScriptCategory _category;
	private int? _chapterId;

	public EventScriptBuilder(ushort scriptId) {
		_scriptId = scriptId;
	}

	public EventScriptBuilder WithName(string name) {
		_name = name;
		return this;
	}

	public EventScriptBuilder WithCategory(ScriptCategory category) {
		_category = category;
		return this;
	}

	public EventScriptBuilder ForChapter(int chapterId) {
		_chapterId = chapterId;
		return this;
	}

	// Flow control
	public EventScriptBuilder End() => AddCommand(ScriptOpcode.End);
	public EventScriptBuilder Return() => AddCommand(ScriptOpcode.Return);
	public EventScriptBuilder Nop() => AddCommand(ScriptOpcode.Nop);
	public EventScriptBuilder Jump(ushort address) => AddCommand(ScriptOpcode.Jump, LowByte(address), HighByte(address));
	public EventScriptBuilder JumpSubroutine(ushort address) => AddCommand(ScriptOpcode.JumpSubroutine, LowByte(address), HighByte(address));

	// Dialog
	public EventScriptBuilder ShowDialog(ushort dialogId) => AddCommand(ScriptOpcode.ShowDialog, LowByte(dialogId), HighByte(dialogId));
	public EventScriptBuilder ShowChoice(ushort dialogId, byte choiceCount) => AddCommand(ScriptOpcode.ShowChoice, LowByte(dialogId), HighByte(dialogId), choiceCount);

	// Flags
	public EventScriptBuilder SetFlag(ushort flagId) => AddCommand(ScriptOpcode.SetFlag, LowByte(flagId), HighByte(flagId));
	public EventScriptBuilder ClearFlag(ushort flagId) => AddCommand(ScriptOpcode.ClearFlag, LowByte(flagId), HighByte(flagId));
	public EventScriptBuilder CheckFlag(ushort flagId, ushort branchAddress) =>
		AddCommand(ScriptOpcode.CheckFlag, LowByte(flagId), HighByte(flagId), LowByte(branchAddress), HighByte(branchAddress));

	// Items
	public EventScriptBuilder GiveItem(byte itemId) => AddCommand(ScriptOpcode.GiveItem, itemId);
	public EventScriptBuilder TakeItem(byte itemId) => AddCommand(ScriptOpcode.TakeItem, itemId);
	public EventScriptBuilder CheckItem(byte itemId, ushort branchAddress) =>
		AddCommand(ScriptOpcode.CheckItem, itemId, LowByte(branchAddress), HighByte(branchAddress));

	// Gold
	public EventScriptBuilder GiveGold(ushort amount) => AddCommand(ScriptOpcode.GiveGold, LowByte(amount), HighByte(amount));
	public EventScriptBuilder TakeGold(ushort amount) => AddCommand(ScriptOpcode.TakeGold, LowByte(amount), HighByte(amount));

	// Party
	public EventScriptBuilder HealParty() => AddCommand(ScriptOpcode.Heal, 0);
	public EventScriptBuilder HealMember(byte memberId) => AddCommand(ScriptOpcode.Heal, memberId);
	public EventScriptBuilder AddPartyMember(byte charId) => AddCommand(ScriptOpcode.AddPartyMember, charId);
	public EventScriptBuilder RemovePartyMember(byte charId) => AddCommand(ScriptOpcode.RemovePartyMember, charId);
	public EventScriptBuilder GiveExp(ushort exp) => AddCommand(ScriptOpcode.GiveExp, LowByte(exp), HighByte(exp));

	// Movement
	public EventScriptBuilder Warp(byte mapId, byte x, byte y, byte facing = 0) => AddCommand(ScriptOpcode.Warp, mapId, x, y, facing);
	public EventScriptBuilder FaceDirection(byte entity, byte direction) => AddCommand(ScriptOpcode.FaceDirection, entity, direction);
	public EventScriptBuilder ShowNpc(byte npcId) => AddCommand(ScriptOpcode.ShowNpc, npcId);
	public EventScriptBuilder HideNpc(byte npcId) => AddCommand(ScriptOpcode.HideNpc, npcId);

	// Battle
	public EventScriptBuilder StartBattle(ushort battleId) => AddCommand(ScriptOpcode.StartBattle, LowByte(battleId), HighByte(battleId));

	// Audio/Visual
	public EventScriptBuilder FadeOut(byte speed = 4) => AddCommand(ScriptOpcode.FadeOut, speed);
	public EventScriptBuilder FadeIn(byte speed = 4) => AddCommand(ScriptOpcode.FadeIn, speed);
	public EventScriptBuilder Wait(byte frames) => AddCommand(ScriptOpcode.Wait, frames);
	public EventScriptBuilder PlaySound(byte soundId) => AddCommand(ScriptOpcode.PlaySound, soundId);
	public EventScriptBuilder PlayMusic(byte musicId) => AddCommand(ScriptOpcode.PlayMusic, musicId);
	public EventScriptBuilder StopMusic() => AddCommand(ScriptOpcode.StopMusic);

	// Services
	public EventScriptBuilder OpenShop(byte shopId) => AddCommand(ScriptOpcode.OpenShop, shopId);
	public EventScriptBuilder OpenInn(byte innId, byte price) => AddCommand(ScriptOpcode.OpenInn, innId, price);
	public EventScriptBuilder OpenChurch() => AddCommand(ScriptOpcode.OpenChurch);
	public EventScriptBuilder OpenVault() => AddCommand(ScriptOpcode.OpenVault);

	// Chapter control
	public EventScriptBuilder SetChapter(byte chapterId) => AddCommand(ScriptOpcode.SetChapter, chapterId);

	public EventScript Build() {
		return new EventScript {
			Id = _scriptId,
			Name = _name,
			Category = _category,
			ChapterId = _chapterId,
			Commands = [.. _commands]
		};
	}

	private EventScriptBuilder AddCommand(ScriptOpcode opcode, params byte[] parameters) {
		_commands.Add(new ScriptCommand { Opcode = opcode, Parameters = parameters });
		return this;
	}

	private static byte LowByte(ushort value) => (byte)(value & 0xFF);
	private static byte HighByte(ushort value) => (byte)((value >> 8) & 0xFF);
}
