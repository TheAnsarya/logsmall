namespace DQ4rLib.Models;

/// <summary>
/// Represents SNES SPC700 audio data
/// </summary>
public class SpcAudio {
	/// <summary>
	/// BRR (Bit Rate Reduction) sample data
	/// </summary>
	public byte[] SampleData { get; set; } = [];

	/// <summary>
	/// Instrument/sample directory entries
	/// </summary>
	public List<SampleEntry> Samples { get; set; } = [];

	/// <summary>
	/// Sequence data for music tracks
	/// </summary>
	public byte[] SequenceData { get; set; } = [];

	/// <summary>
	/// Track names (from NES source)
	/// </summary>
	public List<string> TrackNames { get; set; } = [];
}

/// <summary>
/// Individual BRR sample entry
/// </summary>
public class SampleEntry {
	/// <summary>
	/// Sample name/identifier
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Offset in sample data
	/// </summary>
	public int Offset { get; set; }

	/// <summary>
	/// Length in bytes
	/// </summary>
	public int Length { get; set; }

	/// <summary>
	/// Loop point (0 = no loop)
	/// </summary>
	public int LoopPoint { get; set; }

	/// <summary>
	/// Base pitch (MIDI note number)
	/// </summary>
	public int BasePitch { get; set; } = 60; // Middle C

	/// <summary>
	/// ADSR envelope settings
	/// </summary>
	public AdsrEnvelope Envelope { get; set; } = new();
}

/// <summary>
/// SPC700 ADSR envelope parameters
/// </summary>
public class AdsrEnvelope {
	public int Attack { get; set; } = 15;
	public int Decay { get; set; } = 7;
	public int Sustain { get; set; } = 7;
	public int Release { get; set; } = 31;

	/// <summary>
	/// Convert to SPC700 ADSR register values
	/// </summary>
	public (byte Adsr1, byte Adsr2) ToRegisters() {
		byte adsr1 = (byte)(0x80 | (Decay << 4) | Attack);
		byte adsr2 = (byte)((Sustain << 5) | Release);
		return (adsr1, adsr2);
	}
}
