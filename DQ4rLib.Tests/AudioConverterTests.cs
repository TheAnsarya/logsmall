using DQ4rLib.Converters;

namespace DQ4rLib.Tests;

public class AudioConverterTests {
	[Fact]
	public void DpcmToBrr_ValidDpcm_ReturnsNonEmpty() {
		// Arrange - minimal DPCM data
		byte[] dpcm = [0x55, 0xAA, 0x55, 0xAA]; // Alternating pattern

		// Act
		byte[] brr = AudioConverter.DpcmToBrr(dpcm);

		// Assert
		Assert.NotEmpty(brr);
		// BRR is 9 bytes per 16 samples
		// 4 bytes DPCM = 32 bits = 32 samples (roughly)
		// Should produce at least one BRR block
		Assert.True(brr.Length >= 9);
	}

	[Fact]
	public void DpcmToBrr_EmptyInput_ReturnsEmpty() {
		// Arrange
		byte[] dpcm = [];

		// Act
		byte[] brr = AudioConverter.DpcmToBrr(dpcm);

		// Assert
		Assert.Empty(brr);
	}

	[Fact]
	public void DpcmToBrr_OutputLengthIsMultipleOf9() {
		// Arrange
		byte[] dpcm = new byte[16]; // 128 samples worth
		for (int i = 0; i < dpcm.Length; i++) {
			dpcm[i] = (byte)(i * 17); // Some pattern
		}

		// Act
		byte[] brr = AudioConverter.DpcmToBrr(dpcm);

		// Assert
		Assert.Equal(0, brr.Length % 9); // BRR blocks are always 9 bytes
	}

	[Fact]
	public void DpcmToBrr_LastBlockHasEndFlag() {
		// Arrange
		byte[] dpcm = [0xFF, 0x00, 0xFF, 0x00];

		// Act
		byte[] brr = AudioConverter.DpcmToBrr(dpcm);

		// Assert
		// Last block header should have bit 0 set (end flag)
		if (brr.Length >= 9) {
			int lastBlockHeader = brr.Length - 9;
			Assert.Equal(1, brr[lastBlockHeader] & 0x01);
		}
	}

	[Fact]
	public void CreateSpcAudio_EmptyInputs_ReturnsEmptyAudio() {
		// Arrange
		var samples = new Dictionary<string, byte[]>();
		var sequences = new Dictionary<string, byte[]>();

		// Act
		var audio = AudioConverter.CreateSpcAudio(samples, sequences);

		// Assert
		Assert.Empty(audio.SampleData);
		Assert.Empty(audio.SequenceData);
		Assert.Empty(audio.Samples);
		Assert.Empty(audio.TrackNames);
	}

	[Fact]
	public void CreateSpcAudio_WithSamples_PopulatesDirectory() {
		// Arrange
		var samples = new Dictionary<string, byte[]> {
			["kick"] = [0x55, 0xAA],
			["snare"] = [0xAA, 0x55, 0xAA, 0x55]
		};
		var sequences = new Dictionary<string, byte[]>();

		// Act
		var audio = AudioConverter.CreateSpcAudio(samples, sequences);

		// Assert
		Assert.Equal(2, audio.Samples.Count);
		Assert.Equal("kick", audio.Samples[0].Name);
		Assert.Equal("snare", audio.Samples[1].Name);
		Assert.Equal(0, audio.Samples[0].Offset);
		Assert.True(audio.Samples[1].Offset > 0);
	}
}
