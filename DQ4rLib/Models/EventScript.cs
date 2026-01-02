namespace DQ4rLib.Models;

/// <summary>
/// Event script definition for story progression and triggers.
/// Scripts are executed when conditions are met to advance the story.
/// </summary>
public class EventScript {
	/// <summary>Unique script ID.</summary>
	public ushort Id { get; set; }

	/// <summary>Script name for debugging.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Chapter this script belongs to (0xFF = any).</summary>
	public byte ChapterId { get; set; } = 0xff;

	/// <summary>Script trigger type.</summary>
	public EventTriggerType TriggerType { get; set; }

	/// <summary>Map ID where script can trigger (0xFFFF = any).</summary>
	public ushort MapId { get; set; } = 0xffff;

	/// <summary>X coordinate for position triggers (-1 = any).</summary>
	public short TriggerX { get; set; } = -1;

	/// <summary>Y coordinate for position triggers (-1 = any).</summary>
	public short TriggerY { get; set; } = -1;

	/// <summary>Event flags that must be set for script to run.</summary>
	public List<ushort> RequiredFlags { get; set; } = [];

	/// <summary>Event flags that must NOT be set for script to run.</summary>
	public List<ushort> BlockingFlags { get; set; } = [];

	/// <summary>Script instructions.</summary>
	public List<ScriptInstruction> Instructions { get; set; } = [];

	/// <summary>Priority for multiple matching scripts (higher = first).</summary>
	public byte Priority { get; set; }

	/// <summary>Whether script can run multiple times.</summary>
	public bool Repeatable { get; set; }

	/// <summary>Check if script conditions are met.</summary>
	public bool CanTrigger(EventContext context) {
		// Check chapter
		if (ChapterId != 0xff && ChapterId != context.CurrentChapterId)
			return false;

		// Check map
		if (MapId != 0xffff && MapId != context.CurrentMapId)
			return false;

		// Check position
		if (TriggerX >= 0 && TriggerX != context.PlayerX)
			return false;
		if (TriggerY >= 0 && TriggerY != context.PlayerY)
			return false;

		// Check required flags
		foreach (var flag in RequiredFlags) {
			if (!context.GetFlag(flag))
				return false;
		}

		// Check blocking flags
		foreach (var flag in BlockingFlags) {
			if (context.GetFlag(flag))
				return false;
		}

		return true;
	}

	/// <summary>Serialize script to binary.</summary>
	public byte[] ToSnesBytes() {
		using var ms = new MemoryStream();
		using var bw = new BinaryWriter(ms);

		// Header (24 bytes)
		bw.Write(Id);
		bw.Write(ChapterId);
		bw.Write((byte)TriggerType);
		bw.Write(MapId);
		bw.Write(TriggerX);
		bw.Write(TriggerY);
		bw.Write((byte)RequiredFlags.Count);
		bw.Write((byte)BlockingFlags.Count);
		bw.Write((ushort)Instructions.Count);
		bw.Write(Priority);
		bw.Write((byte)(Repeatable ? 1 : 0));
		bw.Write((uint)0); // Reserved
		bw.Write((ushort)0); // Reserved

		// Required flags
		foreach (var flag in RequiredFlags) {
			bw.Write(flag);
		}

		// Blocking flags
		foreach (var flag in BlockingFlags) {
			bw.Write(flag);
		}

		// Instructions
		foreach (var inst in Instructions) {
			bw.Write(inst.ToSnesBytes());
		}

		return ms.ToArray();
	}

	/// <summary>Deserialize script from binary.</summary>
	public static EventScript FromSnesBytes(byte[] data) {
		using var ms = new MemoryStream(data);
		using var br = new BinaryReader(ms);

		var script = new EventScript {
			Id = br.ReadUInt16(),
			ChapterId = br.ReadByte(),
			TriggerType = (EventTriggerType)br.ReadByte(),
			MapId = br.ReadUInt16(),
			TriggerX = br.ReadInt16(),
			TriggerY = br.ReadInt16()
		};

		int reqCount = br.ReadByte();
		int blockCount = br.ReadByte();
		int instCount = br.ReadUInt16();
		script.Priority = br.ReadByte();
		script.Repeatable = br.ReadByte() != 0;
		br.ReadUInt32(); // Reserved
		br.ReadUInt16(); // Reserved

		for (int i = 0; i < reqCount; i++) {
			script.RequiredFlags.Add(br.ReadUInt16());
		}

		for (int i = 0; i < blockCount; i++) {
			script.BlockingFlags.Add(br.ReadUInt16());
		}

		for (int i = 0; i < instCount; i++) {
			byte[] instData = br.ReadBytes(ScriptInstruction.InstructionSize);
			script.Instructions.Add(ScriptInstruction.FromSnesBytes(instData));
		}

		return script;
	}
}

/// <summary>
/// Types of event triggers.
/// </summary>
public enum EventTriggerType : byte {
	/// <summary>Triggered when player steps on tile.</summary>
	StepOn = 0,

	/// <summary>Triggered when player interacts with tile/NPC.</summary>
	Interact = 1,

	/// <summary>Triggered when entering map.</summary>
	MapEnter = 2,

	/// <summary>Triggered when flag is set.</summary>
	FlagSet = 3,

	/// <summary>Triggered by item use.</summary>
	ItemUse = 4,

	/// <summary>Triggered after battle victory.</summary>
	BattleWin = 5,

	/// <summary>Triggered by chapter transition.</summary>
	ChapterStart = 6,

	/// <summary>Triggered on timer.</summary>
	Timer = 7,

	/// <summary>Triggered by other script.</summary>
	ScriptCall = 8,

	/// <summary>Auto-triggered when conditions met.</summary>
	Auto = 9
}

/// <summary>
/// Single instruction in an event script.
/// </summary>
public class ScriptInstruction {
	/// <summary>Instruction size in bytes.</summary>
	public const int InstructionSize = 12;

	/// <summary>Instruction opcode.</summary>
	public ScriptOpcode Opcode { get; set; }

	/// <summary>Instruction parameters.</summary>
	public int[] Parameters { get; set; } = new int[3];

	/// <summary>Serialize to bytes.</summary>
	public byte[] ToSnesBytes() {
		byte[] data = new byte[InstructionSize];
		data[0] = (byte)Opcode;
		data[1] = 0; // Reserved

		for (int i = 0; i < 3; i++) {
			data[2 + (i * 3)] = (byte)(Parameters[i] & 0xff);
			data[3 + (i * 3)] = (byte)((Parameters[i] >> 8) & 0xff);
			data[4 + (i * 3)] = (byte)((Parameters[i] >> 16) & 0xff);
		}

		data[11] = 0; // Padding

		return data;
	}

	/// <summary>Deserialize from bytes.</summary>
	public static ScriptInstruction FromSnesBytes(byte[] data) {
		var inst = new ScriptInstruction {
			Opcode = (ScriptOpcode)data[0]
		};

		for (int i = 0; i < 3; i++) {
			inst.Parameters[i] = data[2 + (i * 3)] |
								(data[3 + (i * 3)] << 8) |
								(data[4 + (i * 3)] << 16);
		}

		return inst;
	}

	// Factory methods for common instructions
	public static ScriptInstruction SetFlag(int flagId) => new() {
		Opcode = ScriptOpcode.SetFlag,
		Parameters = [flagId, 0, 0]
	};

	public static ScriptInstruction ClearFlag(int flagId) => new() {
		Opcode = ScriptOpcode.ClearFlag,
		Parameters = [flagId, 0, 0]
	};

	public static ScriptInstruction ShowDialog(int dialogId) => new() {
		Opcode = ScriptOpcode.ShowDialog,
		Parameters = [dialogId, 0, 0]
	};

	public static ScriptInstruction GiveItem(int itemId, int count = 1) => new() {
		Opcode = ScriptOpcode.GiveItem,
		Parameters = [itemId, count, 0]
	};

	public static ScriptInstruction GiveGold(int amount) => new() {
		Opcode = ScriptOpcode.GiveGold,
		Parameters = [amount, 0, 0]
	};

	public static ScriptInstruction AddPartyMember(int characterId) => new() {
		Opcode = ScriptOpcode.AddPartyMember,
		Parameters = [characterId, 0, 0]
	};

	public static ScriptInstruction Teleport(int mapId, int x, int y) => new() {
		Opcode = ScriptOpcode.Teleport,
		Parameters = [mapId, x, y]
	};

	public static ScriptInstruction StartBattle(int battleId) => new() {
		Opcode = ScriptOpcode.StartBattle,
		Parameters = [battleId, 0, 0]
	};

	public static ScriptInstruction PlayCutscene(int cutsceneId) => new() {
		Opcode = ScriptOpcode.PlayCutscene,
		Parameters = [cutsceneId, 0, 0]
	};

	public static ScriptInstruction Branch(ScriptOpcode condition, int targetLabel) => new() {
		Opcode = condition,
		Parameters = [targetLabel, 0, 0]
	};

	public static ScriptInstruction CallScript(int scriptId) => new() {
		Opcode = ScriptOpcode.CallScript,
		Parameters = [scriptId, 0, 0]
	};

	public static ScriptInstruction ChapterTransition(int chapterId) => new() {
		Opcode = ScriptOpcode.ChapterTransition,
		Parameters = [chapterId, 0, 0]
	};
}

/// <summary>
/// Script instruction opcodes.
/// </summary>
public enum ScriptOpcode : byte {
	/// <summary>No operation.</summary>
	Nop = 0x00,

	/// <summary>End script execution.</summary>
	End = 0x01,

	/// <summary>Wait for frames.</summary>
	Wait = 0x02,

	// Flag operations (0x10-0x1F)
	/// <summary>Set event flag.</summary>
	SetFlag = 0x10,

	/// <summary>Clear event flag.</summary>
	ClearFlag = 0x11,

	/// <summary>Toggle event flag.</summary>
	ToggleFlag = 0x12,

	/// <summary>Copy flag value.</summary>
	CopyFlag = 0x13,

	// Flow control (0x20-0x2F)
	/// <summary>Jump to instruction.</summary>
	Jump = 0x20,

	/// <summary>Jump if flag set.</summary>
	JumpIfSet = 0x21,

	/// <summary>Jump if flag clear.</summary>
	JumpIfClear = 0x22,

	/// <summary>Jump if variable equals.</summary>
	JumpIfEqual = 0x23,

	/// <summary>Jump if variable not equal.</summary>
	JumpIfNotEqual = 0x24,

	/// <summary>Jump if variable greater.</summary>
	JumpIfGreater = 0x25,

	/// <summary>Jump if variable less.</summary>
	JumpIfLess = 0x26,

	/// <summary>Call subroutine script.</summary>
	CallScript = 0x27,

	/// <summary>Return from script.</summary>
	Return = 0x28,

	// Dialog and text (0x30-0x3F)
	/// <summary>Show dialog box.</summary>
	ShowDialog = 0x30,

	/// <summary>Show yes/no choice.</summary>
	ShowChoice = 0x31,

	/// <summary>Show multi-choice menu.</summary>
	ShowMenu = 0x32,

	/// <summary>Set speaker name.</summary>
	SetSpeaker = 0x33,

	/// <summary>Show floating text.</summary>
	ShowFloatText = 0x34,

	// Item and gold (0x40-0x4F)
	/// <summary>Give item to player.</summary>
	GiveItem = 0x40,

	/// <summary>Take item from player.</summary>
	TakeItem = 0x41,

	/// <summary>Check if player has item.</summary>
	CheckItem = 0x42,

	/// <summary>Give gold to player.</summary>
	GiveGold = 0x43,

	/// <summary>Take gold from player.</summary>
	TakeGold = 0x44,

	/// <summary>Check gold amount.</summary>
	CheckGold = 0x45,

	// Party management (0x50-0x5F)
	/// <summary>Add character to party.</summary>
	AddPartyMember = 0x50,

	/// <summary>Remove character from party.</summary>
	RemovePartyMember = 0x51,

	/// <summary>Heal party.</summary>
	HealParty = 0x52,

	/// <summary>Damage party.</summary>
	DamageParty = 0x53,

	/// <summary>Set party formation.</summary>
	SetFormation = 0x54,

	// Movement and maps (0x60-0x6F)
	/// <summary>Teleport to location.</summary>
	Teleport = 0x60,

	/// <summary>Move player relative.</summary>
	MovePlayer = 0x61,

	/// <summary>Set player facing.</summary>
	SetFacing = 0x62,

	/// <summary>Move NPC.</summary>
	MoveNpc = 0x63,

	/// <summary>Show/hide NPC.</summary>
	SetNpcVisible = 0x64,

	/// <summary>Lock player movement.</summary>
	LockMovement = 0x65,

	/// <summary>Unlock player movement.</summary>
	UnlockMovement = 0x66,

	// Battle and combat (0x70-0x7F)
	/// <summary>Start battle encounter.</summary>
	StartBattle = 0x70,

	/// <summary>Start boss battle.</summary>
	StartBossBattle = 0x71,

	/// <summary>Set encounter rate.</summary>
	SetEncounterRate = 0x72,

	/// <summary>Disable encounters.</summary>
	DisableEncounters = 0x73,

	/// <summary>Enable encounters.</summary>
	EnableEncounters = 0x74,

	// Audio and visual (0x80-0x8F)
	/// <summary>Play music.</summary>
	PlayMusic = 0x80,

	/// <summary>Stop music.</summary>
	StopMusic = 0x81,

	/// <summary>Play sound effect.</summary>
	PlaySound = 0x82,

	/// <summary>Fade screen.</summary>
	FadeScreen = 0x83,

	/// <summary>Flash screen.</summary>
	FlashScreen = 0x84,

	/// <summary>Shake screen.</summary>
	ShakeScreen = 0x85,

	// Cutscene and chapter (0x90-0x9F)
	/// <summary>Play cutscene.</summary>
	PlayCutscene = 0x90,

	/// <summary>Transition to chapter.</summary>
	ChapterTransition = 0x91,

	/// <summary>Show chapter title.</summary>
	ShowChapterTitle = 0x92,

	/// <summary>Show location name.</summary>
	ShowLocationName = 0x93,

	// Variables (0xA0-0xAF)
	/// <summary>Set variable value.</summary>
	SetVariable = 0xA0,

	/// <summary>Add to variable.</summary>
	AddVariable = 0xA1,

	/// <summary>Subtract from variable.</summary>
	SubVariable = 0xA2,

	/// <summary>Multiply variable.</summary>
	MulVariable = 0xA3,

	/// <summary>Divide variable.</summary>
	DivVariable = 0xA4,

	/// <summary>Random value to variable.</summary>
	RandomVariable = 0xA5,

	// Shop and services (0xB0-0xBF)
	/// <summary>Open shop.</summary>
	OpenShop = 0xB0,

	/// <summary>Open inn.</summary>
	OpenInn = 0xB1,

	/// <summary>Open church.</summary>
	OpenChurch = 0xB2,

	/// <summary>Open bank.</summary>
	OpenBank = 0xB3,

	/// <summary>Open casino.</summary>
	OpenCasino = 0xB4,

	// Debug (0xF0-0xFF)
	/// <summary>Debug print.</summary>
	DebugPrint = 0xF0,

	/// <summary>Debug break.</summary>
	DebugBreak = 0xF1
}

/// <summary>
/// Context for script execution.
/// </summary>
public class EventContext {
	/// <summary>Current chapter ID.</summary>
	public byte CurrentChapterId { get; set; }

	/// <summary>Current map ID.</summary>
	public ushort CurrentMapId { get; set; }

	/// <summary>Player X position.</summary>
	public short PlayerX { get; set; }

	/// <summary>Player Y position.</summary>
	public short PlayerY { get; set; }

	/// <summary>Event flags (reference to ChapterState flags).</summary>
	public byte[] EventFlags { get; set; } = new byte[32];

	/// <summary>Script variables.</summary>
	public int[] Variables { get; set; } = new int[32];

	/// <summary>Get flag value.</summary>
	public bool GetFlag(int flagId) {
		int byteIndex = flagId / 8;
		int bitIndex = flagId % 8;
		return byteIndex < EventFlags.Length && (EventFlags[byteIndex] & (1 << bitIndex)) != 0;
	}

	/// <summary>Set flag value.</summary>
	public void SetFlag(int flagId, bool value = true) {
		int byteIndex = flagId / 8;
		int bitIndex = flagId % 8;
		if (byteIndex < EventFlags.Length) {
			if (value)
				EventFlags[byteIndex] |= (byte)(1 << bitIndex);
			else
				EventFlags[byteIndex] &= (byte)~(1 << bitIndex);
		}
	}

	/// <summary>Last choice result (for ShowChoice).</summary>
	public int LastChoiceResult { get; set; }

	/// <summary>Last battle result (0=loss, 1=win, 2=fled).</summary>
	public int LastBattleResult { get; set; }
}
