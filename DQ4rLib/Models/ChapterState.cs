namespace DQ4rLib.Models;

/// <summary>
/// Chapter state data for save files and runtime.
/// Tracks progress within a chapter and party state.
/// </summary>
public class ChapterState {
	/// <summary>
	/// Current chapter ID (0-4).
	/// </summary>
	public byte CurrentChapterId { get; set; }

	/// <summary>
	/// Current chapter (computed from ID).
	/// </summary>
	public int CurrentChapter => CurrentChapterId + 1;

	/// <summary>
	/// Flags indicating which chapters have been completed.
	/// Bit 0 = Ch1, Bit 1 = Ch2, etc.
	/// </summary>
	public byte ChaptersCompleted { get; set; }

	/// <summary>
	/// Whether a specific chapter has been completed.
	/// </summary>
	public bool IsChapterCompleted(int chapter) =>
		(ChaptersCompleted & (1 << (chapter - 1))) != 0;

	/// <summary>
	/// Mark a chapter as completed.
	/// </summary>
	public void SetChapterCompleted(int chapter) =>
		ChaptersCompleted |= (byte)(1 << (chapter - 1));

	/// <summary>
	/// Current map ID.
	/// </summary>
	public ushort CurrentMapId { get; set; }

	/// <summary>
	/// Current X position on map.
	/// </summary>
	public byte CurrentX { get; set; }

	/// <summary>
	/// Current Y position on map.
	/// </summary>
	public byte CurrentY { get; set; }

	/// <summary>
	/// Gold amount for current chapter.
	/// </summary>
	public uint Gold { get; set; }

	/// <summary>
	/// Step counter for encounters.
	/// </summary>
	public ushort StepCount { get; set; }

	/// <summary>
	/// Current AI tactic setting (Chapter 5 only).
	/// </summary>
	public BattleTactic CurrentTactic { get; set; }

	/// <summary>
	/// Active party member IDs (front row, up to 4).
	/// </summary>
	public byte[] ActiveParty { get; set; } = new byte[4];

	/// <summary>
	/// Wagon party member IDs (Chapter 5 only, up to 4).
	/// </summary>
	public byte[] WagonParty { get; set; } = new byte[4];

	/// <summary>
	/// Get list of active party member indices (excluding 0xFF/empty slots).
	/// </summary>
	public List<int> GetActiveParty() {
		return ActiveParty.Where(id => id != 0xFF && id != 0).Select(id => (int)id).ToList();
	}

	/// <summary>
	/// Get list of wagon party member indices (excluding 0xFF/empty slots).
	/// </summary>
	public List<int> GetWagonParty() {
		return WagonParty.Where(id => id != 0xFF && id != 0).Select(id => (int)id).ToList();
	}

	/// <summary>
	/// Event flags array (256 flags = 32 bytes).
	/// </summary>
	public byte[] EventFlags { get; set; } = new byte[32];

	/// <summary>
	/// Check if an event flag is set.
	/// </summary>
	public bool GetEventFlag(int flagId) {
		int byteIndex = flagId / 8;
		int bitIndex = flagId % 8;
		return byteIndex < EventFlags.Length && (EventFlags[byteIndex] & (1 << bitIndex)) != 0;
	}

	/// <summary>
	/// Set an event flag.
	/// </summary>
	public void SetEventFlag(int flagId, bool value = true) {
		int byteIndex = flagId / 8;
		int bitIndex = flagId % 8;
		if (byteIndex < EventFlags.Length) {
			if (value)
				EventFlags[byteIndex] |= (byte)(1 << bitIndex);
			else
				EventFlags[byteIndex] &= (byte)~(1 << bitIndex);
		}
	}

	/// <summary>
	/// Time played in frames (60 fps).
	/// </summary>
	public uint PlayTimeFrames { get; set; }

	/// <summary>
	/// Time played formatted as HH:MM.
	/// </summary>
	public string PlayTimeFormatted {
		get {
			uint seconds = PlayTimeFrames / 60;
			uint minutes = seconds / 60;
			uint hours = minutes / 60;
			return $"{hours:D2}:{minutes % 60:D2}";
		}
	}

	/// <summary>
	/// Serialize chapter state to SNES format (64 bytes).
	/// </summary>
	public byte[] ToSnesBytes() {
		byte[] data = new byte[64];

		data[0x00] = CurrentChapterId;
		data[0x01] = ChaptersCompleted;
		data[0x02] = (byte)(CurrentMapId & 0xff);
		data[0x03] = (byte)(CurrentMapId >> 8);
		data[0x04] = CurrentX;
		data[0x05] = CurrentY;
		data[0x06] = (byte)(Gold & 0xff);
		data[0x07] = (byte)((Gold >> 8) & 0xff);
		data[0x08] = (byte)((Gold >> 16) & 0xff);
		data[0x09] = (byte)(StepCount & 0xff);
		data[0x0a] = (byte)(StepCount >> 8);
		data[0x0b] = (byte)CurrentTactic;

		// Active party (4 bytes)
		Array.Copy(ActiveParty, 0, data, 0x0c, 4);

		// Wagon party (4 bytes)
		Array.Copy(WagonParty, 0, data, 0x10, 4);

		// Play time (4 bytes)
		data[0x14] = (byte)(PlayTimeFrames & 0xff);
		data[0x15] = (byte)((PlayTimeFrames >> 8) & 0xff);
		data[0x16] = (byte)((PlayTimeFrames >> 16) & 0xff);
		data[0x17] = (byte)((PlayTimeFrames >> 24) & 0xff);

		// Reserved (8 bytes)
		// 0x18-0x1f

		// Event flags (32 bytes)
		Array.Copy(EventFlags, 0, data, 0x20, 32);

		return data;
	}

	/// <summary>
	/// Deserialize chapter state from SNES format.
	/// </summary>
	public static ChapterState FromSnesBytes(byte[] data) {
		var state = new ChapterState {
			CurrentChapterId = data[0x00],
			ChaptersCompleted = data[0x01],
			CurrentMapId = (ushort)(data[0x02] | (data[0x03] << 8)),
			CurrentX = data[0x04],
			CurrentY = data[0x05],
			Gold = (uint)(data[0x06] | (data[0x07] << 8) | (data[0x08] << 16)),
			StepCount = (ushort)(data[0x09] | (data[0x0a] << 8)),
			CurrentTactic = (BattleTactic)data[0x0b]
		};

		Array.Copy(data, 0x0c, state.ActiveParty, 0, 4);
		Array.Copy(data, 0x10, state.WagonParty, 0, 4);

		state.PlayTimeFrames = (uint)(data[0x14] | (data[0x15] << 8) |
									 (data[0x16] << 16) | (data[0x17] << 24));

		Array.Copy(data, 0x20, state.EventFlags, 0, 32);

		return state;
	}
}
