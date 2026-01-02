using DQ4rLib.Models;

namespace DQ4rLib.Tests;

public class SpcAudioTests {
	[Fact]
	public void SpcAudio_DefaultConstructor_InitializesEmpty() {
		// Act
		var audio = new SpcAudio();

		// Assert
		Assert.Empty(audio.SampleData);
		Assert.Empty(audio.SequenceData);
		Assert.Empty(audio.Samples);
		Assert.Empty(audio.TrackNames);
	}

	[Fact]
	public void SampleEntry_DefaultValues_AreCorrect() {
		// Act
		var sample = new SampleEntry();

		// Assert
		Assert.Equal(string.Empty, sample.Name);
		Assert.Equal(0, sample.Offset);
		Assert.Equal(0, sample.Length);
		Assert.Equal(0, sample.LoopPoint);
		Assert.Equal(60, sample.BasePitch); // Middle C
	}

	[Fact]
	public void AdsrEnvelope_DefaultValues_AreReasonable() {
		// Act
		var envelope = new AdsrEnvelope();

		// Assert
		Assert.Equal(15, envelope.Attack);
		Assert.Equal(7, envelope.Decay);
		Assert.Equal(7, envelope.Sustain);
		Assert.Equal(31, envelope.Release);
	}

	[Fact]
	public void AdsrEnvelope_ToRegisters_FormatsCorrectly() {
		// Arrange
		var envelope = new AdsrEnvelope {
			Attack = 10,
			Decay = 5,
			Sustain = 3,
			Release = 20
		};

		// Act
		var (adsr1, adsr2) = envelope.ToRegisters();

		// Assert
		// ADSR1 = 0x80 | (decay << 4) | attack = 0x80 | 0x50 | 0x0A = 0xDA
		Assert.Equal(0xDA, adsr1);
		// ADSR2 = (sustain << 5) | release = 0x60 | 0x14 = 0x74
		Assert.Equal(0x74, adsr2);
	}

	[Fact]
	public void AdsrEnvelope_ToRegisters_MaxValues() {
		// Arrange
		var envelope = new AdsrEnvelope {
			Attack = 15,
			Decay = 7,
			Sustain = 7,
			Release = 31
		};

		// Act
		var (adsr1, adsr2) = envelope.ToRegisters();

		// Assert
		Assert.Equal(0xFF, adsr1); // 0x80 | 0x70 | 0x0F
		Assert.Equal(0xFF, adsr2); // 0xE0 | 0x1F
	}

	[Fact]
	public void SampleEntry_CanSetAllProperties() {
		// Arrange & Act
		var sample = new SampleEntry {
			Name = "TestSample",
			Offset = 0x1000,
			Length = 0x200,
			LoopPoint = 0x100,
			BasePitch = 72
		};

		// Assert
		Assert.Equal("TestSample", sample.Name);
		Assert.Equal(0x1000, sample.Offset);
		Assert.Equal(0x200, sample.Length);
		Assert.Equal(0x100, sample.LoopPoint);
		Assert.Equal(72, sample.BasePitch);
	}
}
