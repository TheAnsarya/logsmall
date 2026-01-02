namespace DQ4rLib.Models;

/// <summary>
/// DQ4r SNES Chapter - adapted from DW4 NES for SNES format.
/// Dragon Quest IV uses a unique 5-chapter narrative structure where
/// each chapter follows different protagonists before they unite.
/// </summary>
public class Chapter {
	/// <summary>
	/// Chapter ID (0x00-0x04 for chapters 1-5).
	/// </summary>
	public byte Id { get; set; }

	/// <summary>
	/// Chapter number (1-5 for display).
	/// </summary>
	public int Number => Id + 1;

	/// <summary>
	/// Chapter title for display.
	/// </summary>
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// Chapter description/subtitle.
	/// </summary>
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Primary protagonist character ID.
	/// </summary>
	public byte ProtagonistId { get; set; }

	/// <summary>
	/// IDs of all party members available in this chapter.
	/// </summary>
	public byte[] PartyMemberIds { get; set; } = [];

	/// <summary>
	/// IDs of NPC companions (like Healie in Ch1).
	/// </summary>
	public byte[] CompanionIds { get; set; } = [];

	/// <summary>
	/// Starting map ID for the chapter.
	/// </summary>
	public ushort StartMapId { get; set; }

	/// <summary>
	/// Starting X coordinate on the map.
	/// </summary>
	public byte StartX { get; set; }

	/// <summary>
	/// Starting Y coordinate on the map.
	/// </summary>
	public byte StartY { get; set; }

	/// <summary>
	/// Event flags required to have been set before this chapter starts.
	/// </summary>
	public ushort[] PrerequisiteFlags { get; set; } = [];

	/// <summary>
	/// Event flags set when chapter is completed.
	/// </summary>
	public ushort[] CompletionFlags { get; set; } = [];

	/// <summary>
	/// Event ID that triggers chapter completion.
	/// </summary>
	public ushort CompletionEventId { get; set; }

	/// <summary>
	/// Whether wagon system is available (Chapter 5 only).
	/// </summary>
	public bool WagonEnabled { get; set; }

	/// <summary>
	/// Whether AI tactics menu is available (Chapter 5 only).
	/// </summary>
	public bool TacticsEnabled { get; set; }

	/// <summary>
	/// Chapter-specific gameplay mechanics.
	/// </summary>
	public ChapterMechanics Mechanics { get; set; }

	/// <summary>
	/// Maximum active party size for this chapter.
	/// </summary>
	public byte MaxPartySize => WagonEnabled ? (byte)4 : (byte)Math.Min(4, PartyMemberIds.Length);

	/// <summary>
	/// Maps accessible in this chapter (0 = all maps).
	/// </summary>
	public ushort[] AccessibleMapIds { get; set; } = [];

	/// <summary>
	/// Background music track ID for chapter intro.
	/// </summary>
	public byte IntroMusicId { get; set; }

	/// <summary>
	/// Default overworld music track ID.
	/// </summary>
	public byte OverworldMusicId { get; set; }

	/// <summary>
	/// Serialize chapter to SNES binary format.
	/// </summary>
	public byte[] ToSnesBytes() {
		using var ms = new MemoryStream();
		using var bw = new BinaryWriter(ms);

		// Header: 16 bytes
		bw.Write(Id);                       // +$00: Chapter ID
		bw.Write(ProtagonistId);            // +$01: Protagonist ID
		bw.Write((byte)PartyMemberIds.Length); // +$02: Party count
		bw.Write((byte)CompanionIds.Length);   // +$03: Companion count
		bw.Write(StartMapId);               // +$04-05: Start map (16-bit)
		bw.Write(StartX);                   // +$06: Start X
		bw.Write(StartY);                   // +$07: Start Y
		bw.Write((byte)Mechanics);          // +$08: Mechanics flags
		bw.Write(MaxPartySize);             // +$09: Max party size
		bw.Write(IntroMusicId);             // +$0A: Intro music
		bw.Write(OverworldMusicId);         // +$0B: Overworld music
		bw.Write(CompletionEventId);        // +$0C-0D: Completion event
		bw.Write((byte)(WagonEnabled ? 1 : 0));  // +$0E: Wagon flag
		bw.Write((byte)(TacticsEnabled ? 1 : 0)); // +$0F: Tactics flag

		// Party member IDs (up to 8 bytes, padded)
		byte[] partyPadded = new byte[8];
		Array.Copy(PartyMemberIds, partyPadded, Math.Min(8, PartyMemberIds.Length));
		bw.Write(partyPadded);

		// Companion IDs (up to 4 bytes, padded)
		byte[] companionPadded = new byte[4];
		Array.Copy(CompanionIds, companionPadded, Math.Min(4, CompanionIds.Length));
		bw.Write(companionPadded);

		return ms.ToArray();
	}

	/// <summary>
	/// Deserialize chapter from SNES binary format.
	/// </summary>
	public static Chapter FromSnesBytes(byte[] data) {
		using var ms = new MemoryStream(data);
		using var br = new BinaryReader(ms);

		var chapter = new Chapter {
			Id = br.ReadByte(),
			ProtagonistId = br.ReadByte()
		};

		int partyCount = br.ReadByte();
		int companionCount = br.ReadByte();

		chapter.StartMapId = br.ReadUInt16();
		chapter.StartX = br.ReadByte();
		chapter.StartY = br.ReadByte();
		chapter.Mechanics = (ChapterMechanics)br.ReadByte();
		_ = br.ReadByte(); // Max party (computed)
		chapter.IntroMusicId = br.ReadByte();
		chapter.OverworldMusicId = br.ReadByte();
		chapter.CompletionEventId = br.ReadUInt16();
		chapter.WagonEnabled = br.ReadByte() != 0;
		chapter.TacticsEnabled = br.ReadByte() != 0;

		byte[] partyData = br.ReadBytes(8);
		chapter.PartyMemberIds = partyData[..partyCount];

		byte[] companionData = br.ReadBytes(4);
		chapter.CompanionIds = companionData[..companionCount];

		return chapter;
	}
}

/// <summary>
/// Chapter-specific gameplay mechanics flags.
/// </summary>
[Flags]
public enum ChapterMechanics : byte {
	/// <summary>No special mechanics.</summary>
	None = 0x00,

	/// <summary>Party members are AI-controlled (Chapters 2, 4).</summary>
	AiPartyMembers = 0x01,

	/// <summary>Merchant abilities available (Chapter 3 - Taloon).</summary>
	MerchantAbilities = 0x02,

	/// <summary>Solo protagonist, no other party members (Chapters 1, 3 start).</summary>
	SoloProtagonist = 0x04,

	/// <summary>Magic-focused party (Chapter 4 - Nara/Mara).</summary>
	MagicFocus = 0x08,

	/// <summary>Wagon system with 8 party members (Chapter 5).</summary>
	WagonParty = 0x10,

	/// <summary>Tactics menu for AI control (Chapter 5).</summary>
	TacticsMenu = 0x20,

	/// <summary>NPC companion follows party (Chapter 1 - Healie).</summary>
	NpcCompanion = 0x40,

	/// <summary>Full player control over all party members.</summary>
	FullControl = 0x80
}

/// <summary>
/// AI Battle tactics for Chapter 5.
/// </summary>
public enum BattleTactic : byte {
	/// <summary>Balanced AI behavior (default).</summary>
	ShowNoMercy = 0x00,

	/// <summary>Use less MP, focus on physical attacks.</summary>
	WatchMyMp = 0x01,

	/// <summary>Maximum offense, ignore defense.</summary>
	GoAllOut = 0x02,

	/// <summary>Focus on healing and defense.</summary>
	DontUseMagic = 0x03,

	/// <summary>Try different strategies.</summary>
	TryOut = 0x04,

	/// <summary>Follow orders - player controls this member.</summary>
	FollowOrders = 0x05
}
