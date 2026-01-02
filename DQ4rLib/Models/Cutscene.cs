namespace DQ4rLib.Models;

/// <summary>
/// Cutscene definition for chapter transitions and story events.
/// Defines a sequence of commands to execute for cinematic presentation.
/// </summary>
public class Cutscene {
	/// <summary>Unique cutscene ID.</summary>
	public ushort Id { get; set; }

	/// <summary>Cutscene name for debugging.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Chapter this cutscene belongs to (0xFF = any).</summary>
	public byte ChapterId { get; set; } = 0xff;

	/// <summary>Cutscene type.</summary>
	public CutsceneType Type { get; set; }

	/// <summary>Sequence of commands to execute.</summary>
	public List<CutsceneCommand> Commands { get; set; } = [];

	/// <summary>Total duration in frames (computed from commands).</summary>
	public int TotalDuration => Commands.Sum(c => c.Duration);

	/// <summary>Whether cutscene can be skipped.</summary>
	public bool Skippable { get; set; } = true;

	/// <summary>Music track to play during cutscene.</summary>
	public byte MusicId { get; set; }

	/// <summary>Serialize cutscene to binary format.</summary>
	public byte[] ToSnesBytes() {
		using var ms = new MemoryStream();
		using var bw = new BinaryWriter(ms);

		// Header (16 bytes)
		bw.Write(Id);                          // +$00: ID
		bw.Write(ChapterId);                   // +$02: Chapter
		bw.Write((byte)Type);                  // +$03: Type
		bw.Write((ushort)Commands.Count);      // +$04: Command count
		bw.Write(MusicId);                     // +$06: Music
		bw.Write((byte)(Skippable ? 1 : 0));   // +$07: Skippable
		bw.Write((uint)TotalDuration);         // +$08: Duration
		bw.Write(0);                           // +$0C: Reserved
		bw.Write(0);                           // +$0E: Reserved

		// Commands
		foreach (var cmd in Commands) {
			bw.Write(cmd.ToSnesBytes());
		}

		return ms.ToArray();
	}

	/// <summary>Deserialize cutscene from binary format.</summary>
	public static Cutscene FromSnesBytes(byte[] data) {
		using var ms = new MemoryStream(data);
		using var br = new BinaryReader(ms);

		var cutscene = new Cutscene {
			Id = br.ReadUInt16(),
			ChapterId = br.ReadByte(),
			Type = (CutsceneType)br.ReadByte()
		};

		int cmdCount = br.ReadUInt16();
		cutscene.MusicId = br.ReadByte();
		cutscene.Skippable = br.ReadByte() != 0;
		_ = br.ReadUInt32(); // Duration (computed)
		_ = br.ReadUInt32(); // Reserved

		// Read commands
		for (int i = 0; i < cmdCount; i++) {
			byte[] cmdData = br.ReadBytes(CutsceneCommand.CommandSize);
			cutscene.Commands.Add(CutsceneCommand.FromSnesBytes(cmdData));
		}

		return cutscene;
	}
}

/// <summary>
/// Types of cutscenes.
/// </summary>
public enum CutsceneType : byte {
	/// <summary>Chapter opening sequence.</summary>
	ChapterIntro = 0,

	/// <summary>Chapter ending sequence.</summary>
	ChapterOutro = 1,

	/// <summary>Story event cutscene.</summary>
	StoryEvent = 2,

	/// <summary>Boss introduction.</summary>
	BossIntro = 3,

	/// <summary>Character joining party.</summary>
	CharacterJoin = 4,

	/// <summary>Location reveal.</summary>
	LocationReveal = 5,

	/// <summary>Ending sequence.</summary>
	Ending = 6,

	/// <summary>Credits roll.</summary>
	Credits = 7
}

/// <summary>
/// Single command in a cutscene sequence.
/// </summary>
public class CutsceneCommand {
	/// <summary>Command size in bytes.</summary>
	public const int CommandSize = 16;

	/// <summary>Command opcode.</summary>
	public CutsceneOpcode Opcode { get; set; }

	/// <summary>Duration in frames.</summary>
	public ushort Duration { get; set; }

	/// <summary>Command parameters.</summary>
	public int[] Parameters { get; set; } = new int[4];

	/// <summary>Text/string parameter if applicable.</summary>
	public string? TextParam { get; set; }

	/// <summary>Serialize command to 16 bytes.</summary>
	public byte[] ToSnesBytes() {
		byte[] data = new byte[CommandSize];
		data[0] = (byte)Opcode;
		data[1] = 0; // Reserved
		data[2] = (byte)(Duration & 0xff);
		data[3] = (byte)(Duration >> 8);

		// Parameters (3 bytes each, up to 4)
		for (int i = 0; i < 4; i++) {
			data[4 + (i * 3)] = (byte)(Parameters[i] & 0xff);
			data[5 + (i * 3)] = (byte)((Parameters[i] >> 8) & 0xff);
			data[6 + (i * 3)] = (byte)((Parameters[i] >> 16) & 0xff);
		}

		return data;
	}

	/// <summary>Deserialize command from 16 bytes.</summary>
	public static CutsceneCommand FromSnesBytes(byte[] data) {
		var cmd = new CutsceneCommand {
			Opcode = (CutsceneOpcode)data[0],
			Duration = (ushort)(data[2] | (data[3] << 8))
		};

		for (int i = 0; i < 4; i++) {
			cmd.Parameters[i] = data[4 + (i * 3)] |
							   (data[5 + (i * 3)] << 8) |
							   (data[6 + (i * 3)] << 16);
		}

		return cmd;
	}

	/// <summary>Create a fade command.</summary>
	public static CutsceneCommand Fade(bool fadeIn, int duration) => new() {
		Opcode = fadeIn ? CutsceneOpcode.FadeIn : CutsceneOpcode.FadeOut,
		Duration = (ushort)duration
	};

	/// <summary>Create a wait command.</summary>
	public static CutsceneCommand Wait(int frames) => new() {
		Opcode = CutsceneOpcode.Wait,
		Duration = (ushort)frames
	};

	/// <summary>Create a text display command.</summary>
	public static CutsceneCommand ShowText(int textId, int duration = 120) => new() {
		Opcode = CutsceneOpcode.ShowText,
		Duration = (ushort)duration,
		Parameters = [textId, 0, 0, 0]
	};

	/// <summary>Create a sprite command.</summary>
	public static CutsceneCommand ShowSprite(int spriteId, int x, int y) => new() {
		Opcode = CutsceneOpcode.ShowSprite,
		Parameters = [spriteId, x, y, 0]
	};

	/// <summary>Create a music command.</summary>
	public static CutsceneCommand PlayMusic(int trackId) => new() {
		Opcode = CutsceneOpcode.PlayMusic,
		Parameters = [trackId, 0, 0, 0]
	};

	/// <summary>Create a sound effect command.</summary>
	public static CutsceneCommand PlaySound(int soundId) => new() {
		Opcode = CutsceneOpcode.PlaySound,
		Parameters = [soundId, 0, 0, 0]
	};

	/// <summary>Create a screen shake command.</summary>
	public static CutsceneCommand ScreenShake(int intensity, int duration) => new() {
		Opcode = CutsceneOpcode.ScreenShake,
		Duration = (ushort)duration,
		Parameters = [intensity, 0, 0, 0]
	};

	/// <summary>Create a map transition command.</summary>
	public static CutsceneCommand ChangeMap(int mapId, int x, int y) => new() {
		Opcode = CutsceneOpcode.ChangeMap,
		Parameters = [mapId, x, y, 0]
	};
}

/// <summary>
/// Cutscene command opcodes.
/// </summary>
public enum CutsceneOpcode : byte {
	/// <summary>No operation.</summary>
	Nop = 0x00,

	/// <summary>Wait for specified frames.</summary>
	Wait = 0x01,

	/// <summary>Fade screen to black.</summary>
	FadeOut = 0x02,

	/// <summary>Fade screen from black.</summary>
	FadeIn = 0x03,

	/// <summary>Fade to white.</summary>
	WhiteOut = 0x04,

	/// <summary>Fade from white.</summary>
	WhiteIn = 0x05,

	/// <summary>Show text box with message.</summary>
	ShowText = 0x10,

	/// <summary>Hide text box.</summary>
	HideText = 0x11,

	/// <summary>Show chapter title card.</summary>
	ShowTitle = 0x12,

	/// <summary>Show location name.</summary>
	ShowLocation = 0x13,

	/// <summary>Display sprite on screen.</summary>
	ShowSprite = 0x20,

	/// <summary>Hide sprite.</summary>
	HideSprite = 0x21,

	/// <summary>Move sprite to position.</summary>
	MoveSprite = 0x22,

	/// <summary>Animate sprite.</summary>
	AnimateSprite = 0x23,

	/// <summary>Set sprite facing direction.</summary>
	SetSpriteFacing = 0x24,

	/// <summary>Flash sprite.</summary>
	FlashSprite = 0x25,

	/// <summary>Show character portrait.</summary>
	ShowPortrait = 0x28,

	/// <summary>Hide character portrait.</summary>
	HidePortrait = 0x29,

	/// <summary>Play music track.</summary>
	PlayMusic = 0x30,

	/// <summary>Stop music.</summary>
	StopMusic = 0x31,

	/// <summary>Fade music out.</summary>
	FadeMusic = 0x32,

	/// <summary>Play sound effect.</summary>
	PlaySound = 0x33,

	/// <summary>Set screen position/scroll.</summary>
	SetCamera = 0x40,

	/// <summary>Pan camera to position.</summary>
	PanCamera = 0x41,

	/// <summary>Shake screen.</summary>
	ScreenShake = 0x42,

	/// <summary>Flash screen.</summary>
	ScreenFlash = 0x43,

	/// <summary>Set screen palette.</summary>
	SetPalette = 0x44,

	/// <summary>Cycle palette colors.</summary>
	CyclePalette = 0x45,

	/// <summary>Change to new map.</summary>
	ChangeMap = 0x50,

	/// <summary>Load background image.</summary>
	LoadBackground = 0x51,

	/// <summary>Show weather effect.</summary>
	SetWeather = 0x52,

	/// <summary>Set event flag.</summary>
	SetFlag = 0x60,

	/// <summary>Clear event flag.</summary>
	ClearFlag = 0x61,

	/// <summary>Branch if flag set.</summary>
	BranchIfSet = 0x62,

	/// <summary>Branch if flag clear.</summary>
	BranchIfClear = 0x63,

	/// <summary>Jump to command index.</summary>
	Jump = 0x64,

	/// <summary>Call subroutine cutscene.</summary>
	Call = 0x65,

	/// <summary>Return from subroutine.</summary>
	Return = 0x66,

	/// <summary>Add character to party.</summary>
	AddPartyMember = 0x70,

	/// <summary>Remove character from party.</summary>
	RemovePartyMember = 0x71,

	/// <summary>Give item to player.</summary>
	GiveItem = 0x72,

	/// <summary>Give gold to player.</summary>
	GiveGold = 0x73,

	/// <summary>Start battle.</summary>
	StartBattle = 0x80,

	/// <summary>End cutscene.</summary>
	End = 0xff
}
