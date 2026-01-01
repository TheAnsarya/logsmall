namespace DW4Lib.Audio;

/// <summary>
/// Converts DW4 NES audio to DQ3r SNES format.
/// NES uses 2A03 APU (8-bit, 5 channels).
/// SNES uses SPC700 (16-bit, 8 channels with sample-based synthesis).
/// </summary>
public static class AudioConverter {
	/// <summary>
	/// Offset applied to DW4 music track IDs for DQ3r.
	/// </summary>
	public const ushort MusicIdOffset = 0x0100;

	/// <summary>
	/// Offset applied to DW4 sound effect IDs for DQ3r.
	/// </summary>
	public const ushort SoundIdOffset = 0x0200;

	/// <summary>
	/// Convert DW4 music track to DQ3r format.
	/// </summary>
	public static DQ3rMusicTrack ConvertTrack(MusicTrack dw4Track) {
		return new DQ3rMusicTrack {
			Id = (ushort)(dw4Track.Id + MusicIdOffset),
			SourceId = dw4Track.Id,
			Name = dw4Track.Name,
			Category = ConvertCategory(dw4Track.Category),
			Chapters = dw4Track.Chapters,
			Tempo = ScaleTempo(dw4Track.Tempo),
			Loops = dw4Track.Loops,
			Channels = ConvertChannels(dw4Track.Channels)
		};
	}

	/// <summary>
	/// Convert DW4 sound effect to DQ3r format.
	/// </summary>
	public static DQ3rSoundEffect ConvertSoundEffect(SoundEffect dw4Sound) {
		return new DQ3rSoundEffect {
			Id = (ushort)(dw4Sound.Id + SoundIdOffset),
			SourceId = dw4Sound.Id,
			Name = dw4Sound.Name,
			Category = ConvertSoundCategory(dw4Sound.Category),
			Priority = dw4Sound.Priority,
			Duration = (ushort)(dw4Sound.Duration * 2) // Roughly double for 60fps SNES
		};
	}

	/// <summary>
	/// Convert all music tracks.
	/// </summary>
	public static DQ3rMusicTrack[] ConvertAllTracks() {
		return AudioDatabase.GetAllTracks()
			.Select(ConvertTrack)
			.ToArray();
	}

	/// <summary>
	/// Convert all sound effects.
	/// </summary>
	public static DQ3rSoundEffect[] ConvertAllSoundEffects() {
		return AudioDatabase.GetAllSoundEffects()
			.Select(ConvertSoundEffect)
			.ToArray();
	}

	/// <summary>
	/// Get DQ3r music ID for a DW4 music ID.
	/// </summary>
	public static ushort GetDQ3rMusicId(byte dw4MusicId) {
		return (ushort)(dw4MusicId + MusicIdOffset);
	}

	/// <summary>
	/// Get DQ3r sound ID for a DW4 sound ID.
	/// </summary>
	public static ushort GetDQ3rSoundId(byte dw4SoundId) {
		return (ushort)(dw4SoundId + SoundIdOffset);
	}

	/// <summary>
	/// Convert music category.
	/// </summary>
	public static DQ3rMusicCategory ConvertCategory(MusicCategory category) => category switch {
		MusicCategory.Title => DQ3rMusicCategory.Title,
		MusicCategory.Overworld => DQ3rMusicCategory.Field,
		MusicCategory.Town => DQ3rMusicCategory.Town,
		MusicCategory.Castle => DQ3rMusicCategory.Castle,
		MusicCategory.Dungeon => DQ3rMusicCategory.Dungeon,
		MusicCategory.Tower => DQ3rMusicCategory.Dungeon,
		MusicCategory.Battle => DQ3rMusicCategory.Battle,
		MusicCategory.BossBattle => DQ3rMusicCategory.Boss,
		MusicCategory.Victory => DQ3rMusicCategory.Fanfare,
		MusicCategory.Sad => DQ3rMusicCategory.Event,
		MusicCategory.Dramatic => DQ3rMusicCategory.Event,
		MusicCategory.Chapter => DQ3rMusicCategory.Field,
		MusicCategory.Ending => DQ3rMusicCategory.Ending,
		MusicCategory.Jingle => DQ3rMusicCategory.Fanfare,
		_ => DQ3rMusicCategory.Event
	};

	/// <summary>
	/// Convert sound category.
	/// </summary>
	public static DQ3rSoundCategory ConvertSoundCategory(SoundCategory category) => category switch {
		SoundCategory.Menu => DQ3rSoundCategory.System,
		SoundCategory.Battle => DQ3rSoundCategory.Battle,
		SoundCategory.Item => DQ3rSoundCategory.Item,
		SoundCategory.Status => DQ3rSoundCategory.Status,
		SoundCategory.Environment => DQ3rSoundCategory.Environment,
		SoundCategory.Character => DQ3rSoundCategory.Voice,
		SoundCategory.System => DQ3rSoundCategory.System,
		_ => DQ3rSoundCategory.System
	};

	/// <summary>
	/// Scale NES tempo to SNES tempo.
	/// NES runs at 60.0988 fps (NTSC), SNES at exactly 60 fps for gameplay.
	/// </summary>
	public static int ScaleTempo(int nesTempo) {
		if (nesTempo == 0) return 120; // Default tempo
		// Slight adjustment for APU timing differences
		return (int)(nesTempo * 1.0016);
	}

	/// <summary>
	/// Convert NES APU channel data to SNES SPC700 format.
	/// This is a simplified conversion - real conversion would need sample mapping.
	/// </summary>
	public static DQ3rChannelData[] ConvertChannels(ChannelData[] nesChannels) {
		if (nesChannels == null) return [];

		return nesChannels
			.Where(c => c != null)
			.Select(c => new DQ3rChannelData {
				Type = MapChannelType(c.Type),
				Instrument = MapInstrument(c.Type, c.Instrument),
				Volume = MapVolume(c.VolumeEnvelope),
				SequenceData = ConvertSequence(c.SequenceData)
			})
			.ToArray();
	}

	/// <summary>
	/// Map NES channel type to SNES channel.
	/// </summary>
	public static DQ3rChannelType MapChannelType(ChannelType nesType) => nesType switch {
		ChannelType.Pulse1 => DQ3rChannelType.Melody1,
		ChannelType.Pulse2 => DQ3rChannelType.Melody2,
		ChannelType.Triangle => DQ3rChannelType.Bass,
		ChannelType.Noise => DQ3rChannelType.Percussion,
		ChannelType.DPCM => DQ3rChannelType.Sample,
		_ => DQ3rChannelType.Melody1
	};

	/// <summary>
	/// Map NES instrument/duty to SNES sample instrument.
	/// </summary>
	public static byte MapInstrument(ChannelType channel, byte nesInstrument) {
		return channel switch {
			ChannelType.Pulse1 => (byte)(0x10 + (nesInstrument & 0x03)), // Lead instruments
			ChannelType.Pulse2 => (byte)(0x20 + (nesInstrument & 0x03)), // Harmony instruments
			ChannelType.Triangle => 0x01, // Bass instrument
			ChannelType.Noise => 0x40, // Drums
			ChannelType.DPCM => nesInstrument, // Direct sample mapping
			_ => 0x00
		};
	}

	/// <summary>
	/// Map NES volume envelope to SNES volume.
	/// </summary>
	public static byte MapVolume(byte nesEnvelope) {
		// NES envelope is 4-bit (0-15), SNES volume is 8-bit (0-127)
		int nesVolume = nesEnvelope & 0x0F;
		return (byte)((nesVolume * 127) / 15);
	}

	/// <summary>
	/// Convert NES sequence data to SNES format.
	/// Simplified - actual conversion needs note remapping.
	/// </summary>
	public static byte[] ConvertSequence(byte[] nesSequence) {
		if (nesSequence == null || nesSequence.Length == 0) {
			return [];
		}

		// Each NES note-duration pair becomes SNES note-velocity-duration
		var snesSequence = new List<byte>();

		for (int i = 0; i + 1 < nesSequence.Length; i += 2) {
			byte note = nesSequence[i];
			byte duration = nesSequence[i + 1];

			// Convert note (NES uses different pitch table)
			byte snesNote = ConvertNote(note);
			byte velocity = 0x7F; // Full velocity
			byte snesDuration = (byte)Math.Min(duration * 2, 255); // Scale duration

			snesSequence.Add(snesNote);
			snesSequence.Add(velocity);
			snesSequence.Add(snesDuration);
		}

		return [.. snesSequence];
	}

	/// <summary>
	/// Convert NES note value to SNES note value.
	/// </summary>
	public static byte ConvertNote(byte nesNote) {
		// NES and SNES use different note numbering
		// This is a simplified linear mapping
		// Real conversion needs frequency table lookup
		if (nesNote == 0) return 0; // Rest
		if (nesNote >= 0x80) return nesNote; // Control codes pass through

		// Map NES note range to SNES range
		// NES typically uses $01-$5F, SNES uses $01-$7F
		return (byte)Math.Min(nesNote + 12, 0x7F);
	}
}

/// <summary>
/// DQ3r SNES music track.
/// </summary>
public class DQ3rMusicTrack {
	/// <summary>DQ3r music ID.</summary>
	public ushort Id { get; set; }

	/// <summary>Original DW4 music ID.</summary>
	public byte SourceId { get; set; }

	/// <summary>Track name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Track category.</summary>
	public DQ3rMusicCategory Category { get; set; }

	/// <summary>Associated chapters.</summary>
	public int[]? Chapters { get; set; }

	/// <summary>Tempo (BPM).</summary>
	public int Tempo { get; set; }

	/// <summary>Whether track loops.</summary>
	public bool Loops { get; set; }

	/// <summary>Channel data.</summary>
	public DQ3rChannelData[] Channels { get; set; } = [];
}

/// <summary>
/// DQ3r music categories.
/// </summary>
public enum DQ3rMusicCategory {
	Title,
	Field,
	Town,
	Castle,
	Dungeon,
	Battle,
	Boss,
	Fanfare,
	Event,
	Ending
}

/// <summary>
/// DQ3r channel data.
/// </summary>
public class DQ3rChannelData {
	/// <summary>Channel type.</summary>
	public DQ3rChannelType Type { get; set; }

	/// <summary>Instrument sample ID.</summary>
	public byte Instrument { get; set; }

	/// <summary>Channel volume (0-127).</summary>
	public byte Volume { get; set; }

	/// <summary>Note sequence data.</summary>
	public byte[] SequenceData { get; set; } = [];
}

/// <summary>
/// DQ3r SNES channel types.
/// </summary>
public enum DQ3rChannelType {
	Melody1 = 0,
	Melody2 = 1,
	Harmony = 2,
	Bass = 3,
	Percussion = 4,
	Sample = 5,
	Effect1 = 6,
	Effect2 = 7
}

/// <summary>
/// DQ3r SNES sound effect.
/// </summary>
public class DQ3rSoundEffect {
	/// <summary>DQ3r sound ID.</summary>
	public ushort Id { get; set; }

	/// <summary>Original DW4 sound ID.</summary>
	public byte SourceId { get; set; }

	/// <summary>Sound name.</summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>Sound category.</summary>
	public DQ3rSoundCategory Category { get; set; }

	/// <summary>Priority level.</summary>
	public byte Priority { get; set; }

	/// <summary>Duration in frames.</summary>
	public ushort Duration { get; set; }
}

/// <summary>
/// DQ3r sound categories.
/// </summary>
public enum DQ3rSoundCategory {
	System,
	Battle,
	Item,
	Status,
	Environment,
	Voice
}
