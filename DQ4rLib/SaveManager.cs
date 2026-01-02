using DQ4rLib.Models;

namespace DQ4rLib;

/// <summary>
/// Manages save/load operations for DQ4r.
/// Handles SRAM format, multiple save slots, and chapter state persistence.
/// </summary>
public class SaveManager {
	/// <summary>Number of save slots available.</summary>
	public const int MaxSaveSlots = 3;

	/// <summary>SRAM total size (8KB).</summary>
	public const int SramSize = 0x2000;

	/// <summary>Adventure log entries per slot.</summary>
	public const int MaxLogEntries = 8;

	/// <summary>All save slots.</summary>
	public SaveSlot[] Slots { get; } = new SaveSlot[MaxSaveSlots];

	/// <summary>Adventure log (shared across saves).</summary>
	public AdventureLogEntry[] AdventureLog { get; } = new AdventureLogEntry[MaxLogEntries];

	/// <summary>System settings (shared).</summary>
	public SystemSettings Settings { get; set; } = new();

	/// <summary>
	/// Create a new SaveManager.
	/// </summary>
	public SaveManager() {
		for (int i = 0; i < MaxSaveSlots; i++) {
			Slots[i] = new SaveSlot { SlotIndex = i };
		}
		for (int i = 0; i < MaxLogEntries; i++) {
			AdventureLog[i] = new AdventureLogEntry();
		}
	}

	/// <summary>
	/// Save game to a slot.
	/// </summary>
	public bool Save(int slotIndex, ChapterManager chapterManager) {
		if (slotIndex < 0 || slotIndex >= MaxSaveSlots)
			return false;

		var slot = Slots[slotIndex];
		slot.IsUsed = true;
		slot.SaveData = SaveData.CreateFromChapterManager(chapterManager);

		// Set timestamp BEFORE finalizing checksum
		slot.SaveData.SaveTimestamp = GetCurrentTimestamp();
		// Recalculate checksum after all modifications
		slot.SaveData.Checksum = slot.SaveData.CalculateChecksum();

		// Create preview info
		slot.Preview = new SavePreview {
			ChapterNumber = chapterManager.State.CurrentChapter,
			ChapterTitle = chapterManager.CurrentChapter.Title,
			PlayTime = chapterManager.State.PlayTimeFormatted,
			Location = GetLocationName(chapterManager.State.CurrentMapId),
			Level = GetPartyLeaderLevel(slot.SaveData),
			Gold = chapterManager.State.Gold
		};

		// Add to adventure log
		AddAdventureLogEntry(chapterManager);

		return true;
	}

	/// <summary>
	/// Load game from a slot.
	/// </summary>
	public SaveData? Load(int slotIndex) {
		if (slotIndex < 0 || slotIndex >= MaxSaveSlots)
			return null;

		var slot = Slots[slotIndex];
		if (!slot.IsUsed || slot.SaveData == null)
			return null;

		// Validate checksum
		if (!slot.SaveData.ValidateChecksum())
			return null;

		return slot.SaveData;
	}

	/// <summary>
	/// Delete a save slot.
	/// </summary>
	public bool Delete(int slotIndex) {
		if (slotIndex < 0 || slotIndex >= MaxSaveSlots)
			return false;

		Slots[slotIndex] = new SaveSlot { SlotIndex = slotIndex };
		return true;
	}

	/// <summary>
	/// Copy save from one slot to another.
	/// </summary>
	public bool Copy(int fromSlot, int toSlot) {
		if (fromSlot < 0 || fromSlot >= MaxSaveSlots ||
			toSlot < 0 || toSlot >= MaxSaveSlots ||
			fromSlot == toSlot)
			return false;

		if (!Slots[fromSlot].IsUsed)
			return false;

		// Deep copy by serialization
		byte[] data = Slots[fromSlot].SaveData!.ToSnesBytes();
		Slots[toSlot].SaveData = SaveData.FromSnesBytes(data);
		Slots[toSlot].IsUsed = true;
		Slots[toSlot].Preview = Slots[fromSlot].Preview?.Clone();

		return true;
	}

	/// <summary>
	/// Serialize entire SRAM to bytes.
	/// </summary>
	public byte[] ToSramBytes() {
		byte[] sram = new byte[SramSize];

		// Header (16 bytes)
		sram[0x00] = (byte)'D';
		sram[0x01] = (byte)'Q';
		sram[0x02] = (byte)'4';
		sram[0x03] = (byte)'R';
		sram[0x04] = SaveData.CurrentVersion;

		// Save slots (3 * 2KB = 6KB at 0x100)
		for (int i = 0; i < MaxSaveSlots; i++) {
			int offset = 0x100 + (i * SaveData.SaveSize);
			if (Slots[i].IsUsed && Slots[i].SaveData != null) {
				byte[] slotData = Slots[i].SaveData.ToSnesBytes();
				Array.Copy(slotData, 0, sram, offset, SaveData.SaveSize);
			}
		}

		// System settings (64 bytes at 0x1900)
		byte[] settingsData = Settings.ToSnesBytes();
		Array.Copy(settingsData, 0, sram, 0x1900, Math.Min(64, settingsData.Length));

		// Adventure log (256 bytes at 0x1940)
		for (int i = 0; i < MaxLogEntries; i++) {
			byte[] logData = AdventureLog[i].ToSnesBytes();
			Array.Copy(logData, 0, sram, 0x1940 + (i * 32), 32);
		}

		// Calculate global checksum
		ushort checksum = 0;
		for (int i = 0x100; i < SramSize; i++) {
			checksum += sram[i];
		}
		sram[0x06] = (byte)(checksum & 0xff);
		sram[0x07] = (byte)(checksum >> 8);

		return sram;
	}

	/// <summary>
	/// Deserialize SRAM from bytes.
	/// </summary>
	public static SaveManager FromSramBytes(byte[] sram) {
		var manager = new SaveManager();

		// Validate header
		if (sram.Length < SramSize ||
			sram[0x00] != 'D' || sram[0x01] != 'Q' ||
			sram[0x02] != '4' || sram[0x03] != 'R') {
			return manager; // Return empty manager
		}

		// Load save slots
		for (int i = 0; i < MaxSaveSlots; i++) {
			int offset = 0x100 + (i * SaveData.SaveSize);
			byte[] slotData = new byte[SaveData.SaveSize];
			Array.Copy(sram, offset, slotData, 0, SaveData.SaveSize);

			// Check if slot has data (non-zero checksum or version)
			if (slotData[0] != 0 || slotData[2] != 0 || slotData[3] != 0) {
				manager.Slots[i].SaveData = SaveData.FromSnesBytes(slotData);
				manager.Slots[i].IsUsed = true;
				manager.Slots[i].Preview = CreatePreviewFromSaveData(manager.Slots[i].SaveData);
			}
		}

		// Load settings
		byte[] settingsData = new byte[64];
		Array.Copy(sram, 0x1900, settingsData, 0, 64);
		manager.Settings = SystemSettings.FromSnesBytes(settingsData);

		// Load adventure log
		for (int i = 0; i < MaxLogEntries; i++) {
			byte[] logData = new byte[32];
			Array.Copy(sram, 0x1940 + (i * 32), logData, 0, 32);
			manager.AdventureLog[i] = AdventureLogEntry.FromSnesBytes(logData);
		}

		return manager;
	}

	/// <summary>
	/// Get quick save preview without full load.
	/// </summary>
	public SavePreview? GetSlotPreview(int slotIndex) {
		if (slotIndex < 0 || slotIndex >= MaxSaveSlots)
			return null;

		return Slots[slotIndex].IsUsed ? Slots[slotIndex].Preview : null;
	}

	private static SavePreview CreatePreviewFromSaveData(SaveData data) {
		return new SavePreview {
			ChapterNumber = data.ChapterState.CurrentChapter,
			PlayTime = data.ChapterState.PlayTimeFormatted,
			Gold = data.ChapterState.Gold,
			Level = data.Characters[0]?.Level ?? 1
		};
	}

	private void AddAdventureLogEntry(ChapterManager manager) {
		// Shift existing entries
		for (int i = MaxLogEntries - 1; i > 0; i--) {
			AdventureLog[i] = AdventureLog[i - 1];
		}

		// Add new entry
		AdventureLog[0] = new AdventureLogEntry {
			Timestamp = GetCurrentTimestamp(),
			ChapterId = manager.State.CurrentChapterId,
			MapId = manager.State.CurrentMapId,
			EventType = AdventureEventType.Save
		};
	}

	private static uint GetCurrentTimestamp() {
		// Simple timestamp: days since 2000 + time of day
		var now = DateTime.Now;
		var epoch = new DateTime(2000, 1, 1);
		uint days = (uint)(now - epoch).TotalDays;
		uint timeOfDay = (uint)(now.Hour * 3600 + now.Minute * 60 + now.Second);
		return (days << 17) | (timeOfDay & 0x1ffff);
	}

	private static string GetLocationName(ushort mapId) {
		// Placeholder - would look up from map database
		return $"Map {mapId:X4}";
	}

	private static byte GetPartyLeaderLevel(SaveData data) {
		return data.Characters[0]?.Level ?? 1;
	}
}

/// <summary>
/// Single save slot container.
/// </summary>
public class SaveSlot {
	/// <summary>Slot index (0-2).</summary>
	public int SlotIndex { get; set; }

	/// <summary>Whether this slot contains data.</summary>
	public bool IsUsed { get; set; }

	/// <summary>Full save data.</summary>
	public SaveData? SaveData { get; set; }

	/// <summary>Quick preview data.</summary>
	public SavePreview? Preview { get; set; }
}

/// <summary>
/// Quick preview of save slot for menu display.
/// </summary>
public class SavePreview {
	/// <summary>Current chapter number (1-5).</summary>
	public int ChapterNumber { get; set; }

	/// <summary>Chapter title.</summary>
	public string ChapterTitle { get; set; } = string.Empty;

	/// <summary>Play time formatted as HH:MM.</summary>
	public string PlayTime { get; set; } = "00:00";

	/// <summary>Current location name.</summary>
	public string Location { get; set; } = string.Empty;

	/// <summary>Party leader level.</summary>
	public byte Level { get; set; }

	/// <summary>Gold amount.</summary>
	public uint Gold { get; set; }

	/// <summary>Create a deep copy.</summary>
	public SavePreview Clone() => new() {
		ChapterNumber = ChapterNumber,
		ChapterTitle = ChapterTitle,
		PlayTime = PlayTime,
		Location = Location,
		Level = Level,
		Gold = Gold
	};
}

/// <summary>
/// Adventure log entry for tracking major events.
/// </summary>
public class AdventureLogEntry {
	/// <summary>Timestamp of event.</summary>
	public uint Timestamp { get; set; }

	/// <summary>Chapter when event occurred.</summary>
	public byte ChapterId { get; set; }

	/// <summary>Map where event occurred.</summary>
	public ushort MapId { get; set; }

	/// <summary>Type of event.</summary>
	public AdventureEventType EventType { get; set; }

	/// <summary>Event-specific data.</summary>
	public ushort EventData { get; set; }

	/// <summary>Serialize to 32 bytes.</summary>
	public byte[] ToSnesBytes() {
		byte[] data = new byte[32];
		data[0] = (byte)(Timestamp & 0xff);
		data[1] = (byte)((Timestamp >> 8) & 0xff);
		data[2] = (byte)((Timestamp >> 16) & 0xff);
		data[3] = (byte)((Timestamp >> 24) & 0xff);
		data[4] = ChapterId;
		data[5] = (byte)(MapId & 0xff);
		data[6] = (byte)(MapId >> 8);
		data[7] = (byte)EventType;
		data[8] = (byte)(EventData & 0xff);
		data[9] = (byte)(EventData >> 8);
		return data;
	}

	/// <summary>Deserialize from 32 bytes.</summary>
	public static AdventureLogEntry FromSnesBytes(byte[] data) {
		return new AdventureLogEntry {
			Timestamp = (uint)(data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24)),
			ChapterId = data[4],
			MapId = (ushort)(data[5] | (data[6] << 8)),
			EventType = (AdventureEventType)data[7],
			EventData = (ushort)(data[8] | (data[9] << 8))
		};
	}
}

/// <summary>
/// Types of adventure log events.
/// </summary>
public enum AdventureEventType : byte {
	None = 0,
	Save = 1,
	ChapterStart = 2,
	ChapterComplete = 3,
	BossDefeated = 4,
	CharacterJoined = 5,
	KeyItemObtained = 6,
	QuestComplete = 7
}

/// <summary>
/// System-wide settings (shared across saves).
/// </summary>
public class SystemSettings {
	/// <summary>Message speed (0=slow, 1=normal, 2=fast).</summary>
	public byte MessageSpeed { get; set; } = 1;

	/// <summary>Battle animation (0=on, 1=off).</summary>
	public byte BattleAnimation { get; set; } = 0;

	/// <summary>Sound mode (0=stereo, 1=mono).</summary>
	public byte SoundMode { get; set; } = 0;

	/// <summary>Music volume (0-7).</summary>
	public byte MusicVolume { get; set; } = 7;

	/// <summary>Sound effect volume (0-7).</summary>
	public byte SfxVolume { get; set; } = 7;

	/// <summary>Window color preset (0-7).</summary>
	public byte WindowColor { get; set; } = 0;

	/// <summary>Cursor memory (0=off, 1=on).</summary>
	public byte CursorMemory { get; set; } = 1;

	/// <summary>Serialize to 64 bytes.</summary>
	public byte[] ToSnesBytes() {
		byte[] data = new byte[64];
		data[0] = MessageSpeed;
		data[1] = BattleAnimation;
		data[2] = SoundMode;
		data[3] = MusicVolume;
		data[4] = SfxVolume;
		data[5] = WindowColor;
		data[6] = CursorMemory;
		return data;
	}

	/// <summary>Deserialize from 64 bytes.</summary>
	public static SystemSettings FromSnesBytes(byte[] data) {
		return new SystemSettings {
			MessageSpeed = data[0],
			BattleAnimation = data[1],
			SoundMode = data[2],
			MusicVolume = data[3],
			SfxVolume = data[4],
			WindowColor = data[5],
			CursorMemory = data[6]
		};
	}
}
