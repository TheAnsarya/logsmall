using DQ4rLib.Models;

namespace DQ4rLib.Converters;

/// <summary>
/// Converts NES audio data to SNES SPC700 format
/// </summary>
public static class AudioConverter {
	/// <summary>
	/// Convert NES DPCM sample to BRR format
	/// </summary>
	/// <param name="dpcmData">NES DPCM sample data</param>
	/// <returns>BRR encoded sample data</returns>
	public static byte[] DpcmToBrr(byte[] dpcmData) {
		// First decode DPCM to PCM
		short[] pcm = DecodeDpcm(dpcmData);

		// Then encode PCM to BRR
		return EncodeBrr(pcm);
	}

	/// <summary>
	/// Decode NES DPCM to 16-bit PCM
	/// </summary>
	private static short[] DecodeDpcm(byte[] dpcm) {
		var pcm = new List<short>();
		int output = 0;

		foreach (byte b in dpcm) {
			for (int bit = 0; bit < 8; bit++) {
				if ((b & (1 << bit)) != 0) {
					output += 2;
					if (output > 63) output = 63;
				} else {
					output -= 2;
					if (output < 0) output = 0;
				}

				// Scale to 16-bit
				pcm.Add((short)((output - 32) * 1024));
			}
		}

		return [.. pcm];
	}

	/// <summary>
	/// Encode 16-bit PCM to BRR format
	/// BRR = Bit Rate Reduction, 9 bytes encodes 16 samples
	/// </summary>
	private static byte[] EncodeBrr(short[] pcm) {
		var brr = new List<byte>();

		// Pad PCM to multiple of 16 samples
		int paddedLength = ((pcm.Length + 15) / 16) * 16;
		short[] padded = new short[paddedLength];
		Array.Copy(pcm, padded, pcm.Length);

		// Encode each block of 16 samples to 9 bytes
		for (int block = 0; block < paddedLength / 16; block++) {
			bool isLast = block == paddedLength / 16 - 1;
			byte[] brrBlock = EncodeBrrBlock(
				padded.AsSpan(block * 16, 16),
				isLast
			);
			brr.AddRange(brrBlock);
		}

		return [.. brr];
	}

	/// <summary>
	/// Encode a single BRR block (16 samples -> 9 bytes)
	/// </summary>
	private static byte[] EncodeBrrBlock(ReadOnlySpan<short> samples, bool isLast) {
		byte[] block = new byte[9];

		// Find optimal shift and filter
		int bestShift = FindOptimalShift(samples);
		int filter = 0; // Simple mode for now

		// Header byte
		block[0] = (byte)((bestShift << 4) | (filter << 2) | (isLast ? 1 : 0));

		// Encode 16 samples as 16 nibbles (8 bytes)
		for (int i = 0; i < 16; i += 2) {
			int s1 = QuantizeSample(samples[i], bestShift);
			int s2 = QuantizeSample(samples[i + 1], bestShift);
			block[1 + i / 2] = (byte)((s1 << 4) | (s2 & 0x0f));
		}

		return block;
	}

	private static int FindOptimalShift(ReadOnlySpan<short> samples) {
		int maxAbs = 0;
		foreach (short s in samples) {
			int abs = Math.Abs(s);
			if (abs > maxAbs) maxAbs = abs;
		}

		// Find shift that keeps samples in 4-bit signed range (-8 to 7)
		for (int shift = 0; shift <= 12; shift++) {
			if ((maxAbs >> shift) <= 7) {
				return shift;
			}
		}
		return 12;
	}

	private static int QuantizeSample(short sample, int shift) {
		int quantized = sample >> shift;
		return Math.Clamp(quantized, -8, 7) & 0x0f;
	}

	/// <summary>
	/// Convert NES music sequence to SNES format
	/// This is a stub - actual conversion requires understanding specific sequence format
	/// </summary>
	public static byte[] ConvertSequence(byte[] nesSequence) {
		// TODO: Implement based on DQ4 NES music format analysis
		return nesSequence; // Placeholder
	}

	/// <summary>
	/// Create complete SPC audio package
	/// </summary>
	public static SpcAudio CreateSpcAudio(
		Dictionary<string, byte[]> dpcmSamples,
		Dictionary<string, byte[]> sequences) {
		var audio = new SpcAudio();

		int offset = 0;
		foreach (var (name, dpcm) in dpcmSamples) {
			byte[] brr = DpcmToBrr(dpcm);
			audio.Samples.Add(new SampleEntry {
				Name = name,
				Offset = offset,
				Length = brr.Length
			});

			// Append to sample data
			byte[] newSampleData = new byte[audio.SampleData.Length + brr.Length];
			audio.SampleData.CopyTo(newSampleData, 0);
			brr.CopyTo(newSampleData, audio.SampleData.Length);
			audio.SampleData = newSampleData;

			offset += brr.Length;
		}

		// Process sequences
		foreach (var (name, seq) in sequences) {
			audio.TrackNames.Add(name);
			byte[] converted = ConvertSequence(seq);
			byte[] newSeqData = new byte[audio.SequenceData.Length + converted.Length];
			audio.SequenceData.CopyTo(newSeqData, 0);
			converted.CopyTo(newSeqData, audio.SequenceData.Length);
			audio.SequenceData = newSeqData;
		}

		return audio;
	}

	/// <summary>
	/// Export SPC audio data to binary files
	/// </summary>
	public static void ExportSpcAudio(SpcAudio audio, string outputDir) {
		Directory.CreateDirectory(outputDir);

		File.WriteAllBytes(Path.Combine(outputDir, "samples.brr"), audio.SampleData);
		File.WriteAllBytes(Path.Combine(outputDir, "sequences.bin"), audio.SequenceData);

		// Export sample directory
		using var dir = File.CreateText(Path.Combine(outputDir, "samples.inc"));
		dir.WriteLine("; BRR Sample Directory");
		foreach (var sample in audio.Samples) {
			dir.WriteLine($"; {sample.Name}: offset=${sample.Offset:x4}, len=${sample.Length:x4}");
		}
	}
}
