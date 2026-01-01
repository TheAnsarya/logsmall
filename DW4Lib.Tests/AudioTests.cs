using DW4Lib.Audio;

namespace DW4Lib.Tests;

/// <summary>
/// Unit tests for Audio Data structures.
/// </summary>
public class AudioDataTests {
	// ============================================================
	// MusicTrack Tests
	// ============================================================

	[Fact]
	public void MusicTrack_DefaultValues_AreCorrect() {
		var track = new MusicTrack();

		Assert.Equal((byte)0, track.Id);
		Assert.Equal(string.Empty, track.Name);
		Assert.Equal(MusicCategory.Title, track.Category);
		Assert.Null(track.Chapters);
		Assert.True(track.Loops);
	}

	[Fact]
	public void MusicTrack_CanSetAllProperties() {
		var track = new MusicTrack {
			Id = 0x10,
			Name = "Test Track",
			Category = MusicCategory.Battle,
			Chapters = [0, 1],
			Bank = 0x0F,
			RomAddress = 0x8000,
			Tempo = 120,
			Loops = false
		};

		Assert.Equal(0x10, track.Id);
		Assert.Equal("Test Track", track.Name);
		Assert.Equal(MusicCategory.Battle, track.Category);
		Assert.Equal([0, 1], track.Chapters);
		Assert.False(track.Loops);
	}

	// ============================================================
	// SoundEffect Tests
	// ============================================================

	[Fact]
	public void SoundEffect_DefaultValues_AreCorrect() {
		var sound = new SoundEffect();

		Assert.Equal((byte)0, sound.Id);
		Assert.Equal(string.Empty, sound.Name);
		Assert.Equal((byte)0, sound.Priority);
	}

	[Fact]
	public void SoundEffect_CanSetAllProperties() {
		var sound = new SoundEffect {
			Id = 0x10,
			Name = "Test Sound",
			Category = SoundCategory.Battle,
			Priority = 5,
			Duration = 30,
			Channel = ChannelType.Pulse1,
			Data = [0x01, 0x02, 0x03]
		};

		Assert.Equal(0x10, sound.Id);
		Assert.Equal("Test Sound", sound.Name);
		Assert.Equal(SoundCategory.Battle, sound.Category);
		Assert.Equal(5, sound.Priority);
	}

	// ============================================================
	// ChannelData Tests
	// ============================================================

	[Fact]
	public void ChannelData_DefaultValues_AreCorrect() {
		var channel = new ChannelData();

		Assert.Equal(ChannelType.Pulse1, channel.Type);
		Assert.Empty(channel.SequenceData);
	}

	// ============================================================
	// AudioDatabase Tests
	// ============================================================

	[Fact]
	public void GetAllTracks_ReturnsAllDefinedTracks() {
		var tracks = AudioDatabase.GetAllTracks();

		Assert.NotEmpty(tracks);
		Assert.True(tracks.Length >= 20); // At least 20 tracks defined
	}

	[Fact]
	public void GetAllTracks_ContainsUniqueIds() {
		var tracks = AudioDatabase.GetAllTracks();
		var ids = tracks.Select(t => t.Id).ToList();

		Assert.Equal(ids.Count, ids.Distinct().Count());
	}

	[Fact]
	public void GetAllSoundEffects_ReturnsAllDefinedEffects() {
		var sounds = AudioDatabase.GetAllSoundEffects();

		Assert.NotEmpty(sounds);
		Assert.True(sounds.Length >= 20); // At least 20 sounds defined
	}

	[Fact]
	public void GetAllSoundEffects_ContainsUniqueIds() {
		var sounds = AudioDatabase.GetAllSoundEffects();
		var ids = sounds.Select(s => s.Id).ToList();

		Assert.Equal(ids.Count, ids.Distinct().Count());
	}

	[Fact]
	public void GetChapterOverworldMusic_ReturnsCorrectTrack() {
		Assert.Equal(AudioDatabase.MusicChapter1Overworld, AudioDatabase.GetChapterOverworldMusic(0));
		Assert.Equal(AudioDatabase.MusicChapter2Overworld, AudioDatabase.GetChapterOverworldMusic(1));
		Assert.Equal(AudioDatabase.MusicChapter3Overworld, AudioDatabase.GetChapterOverworldMusic(2));
		Assert.Equal(AudioDatabase.MusicChapter4Overworld, AudioDatabase.GetChapterOverworldMusic(3));
		Assert.Equal(AudioDatabase.MusicChapter5Overworld, AudioDatabase.GetChapterOverworldMusic(4));
	}

	[Fact]
	public void GetTrack_WithValidId_ReturnsTrack() {
		var track = AudioDatabase.GetTrack(AudioDatabase.MusicTitle);

		Assert.NotNull(track);
		Assert.Equal(AudioDatabase.MusicTitle, track.Id);
		Assert.Equal("Title Screen", track.Name);
	}

	[Fact]
	public void GetTrack_WithInvalidId_ReturnsNull() {
		var track = AudioDatabase.GetTrack(0xFF);

		Assert.Null(track);
	}

	[Fact]
	public void GetSoundEffect_WithValidId_ReturnsEffect() {
		var sound = AudioDatabase.GetSoundEffect(AudioDatabase.SfxConfirm);

		Assert.NotNull(sound);
		Assert.Equal(AudioDatabase.SfxConfirm, sound.Id);
		Assert.Equal("Confirm", sound.Name);
	}

	[Fact]
	public void GetSoundEffect_WithInvalidId_ReturnsNull() {
		var sound = AudioDatabase.GetSoundEffect(0xFF);

		Assert.Null(sound);
	}

	// ============================================================
	// Music Constants Tests
	// ============================================================

	[Fact]
	public void MusicConstants_HaveExpectedValues() {
		Assert.Equal(0x01, AudioDatabase.MusicTitle);
		Assert.Equal(0x10, AudioDatabase.MusicChapter1Overworld);
		Assert.Equal(0x20, AudioDatabase.MusicTown);
		Assert.Equal(0x30, AudioDatabase.MusicBattle);
		Assert.Equal(0x38, AudioDatabase.MusicVictory);
	}

	[Fact]
	public void SoundConstants_HaveExpectedValues() {
		Assert.Equal(0x01, AudioDatabase.SfxCursor);
		Assert.Equal(0x02, AudioDatabase.SfxConfirm);
		Assert.Equal(0x10, AudioDatabase.SfxHit);
		Assert.Equal(0x20, AudioDatabase.SfxHeal);
		Assert.Equal(0x30, AudioDatabase.SfxChest);
	}

	// ============================================================
	// Category Enum Tests
	// ============================================================

	[Theory]
	[InlineData(MusicCategory.Title)]
	[InlineData(MusicCategory.Overworld)]
	[InlineData(MusicCategory.Battle)]
	[InlineData(MusicCategory.BossBattle)]
	[InlineData(MusicCategory.Victory)]
	[InlineData(MusicCategory.Ending)]
	public void MusicCategory_AllValues_AreDefined(MusicCategory category) {
		Assert.True(Enum.IsDefined(typeof(MusicCategory), category));
	}

	[Theory]
	[InlineData(SoundCategory.Menu)]
	[InlineData(SoundCategory.Battle)]
	[InlineData(SoundCategory.Item)]
	[InlineData(SoundCategory.Status)]
	[InlineData(SoundCategory.System)]
	public void SoundCategory_AllValues_AreDefined(SoundCategory category) {
		Assert.True(Enum.IsDefined(typeof(SoundCategory), category));
	}

	[Theory]
	[InlineData(ChannelType.Pulse1)]
	[InlineData(ChannelType.Pulse2)]
	[InlineData(ChannelType.Triangle)]
	[InlineData(ChannelType.Noise)]
	[InlineData(ChannelType.DPCM)]
	public void ChannelType_AllValues_AreDefined(ChannelType channel) {
		Assert.True(Enum.IsDefined(typeof(ChannelType), channel));
	}

	// ============================================================
	// Track Category Tests
	// ============================================================

	[Fact]
	public void TitleTrack_HasTitleCategory() {
		var track = AudioDatabase.GetTrack(AudioDatabase.MusicTitle);
		Assert.Equal(MusicCategory.Title, track?.Category);
	}

	[Fact]
	public void BattleTrack_HasBattleCategory() {
		var track = AudioDatabase.GetTrack(AudioDatabase.MusicBattle);
		Assert.Equal(MusicCategory.Battle, track?.Category);
	}

	[Fact]
	public void VictoryTrack_DoesNotLoop() {
		var track = AudioDatabase.GetTrack(AudioDatabase.MusicVictory);
		Assert.False(track?.Loops);
	}

	[Fact]
	public void TownTrack_Loops() {
		var track = AudioDatabase.GetTrack(AudioDatabase.MusicTown);
		Assert.True(track?.Loops);
	}
}

/// <summary>
/// Unit tests for Audio Converter.
/// </summary>
public class AudioConverterTests {
	// ============================================================
	// ID Conversion Tests
	// ============================================================

	[Fact]
	public void GetDQ3rMusicId_AppliesOffset() {
		var dq3rId = AudioConverter.GetDQ3rMusicId(0x10);

		Assert.Equal((ushort)0x0110, dq3rId); // 0x10 + 0x100
	}

	[Fact]
	public void GetDQ3rSoundId_AppliesOffset() {
		var dq3rId = AudioConverter.GetDQ3rSoundId(0x10);

		Assert.Equal((ushort)0x0210, dq3rId); // 0x10 + 0x200
	}

	// ============================================================
	// Track Conversion Tests
	// ============================================================

	[Fact]
	public void ConvertTrack_AppliesIdOffset() {
		var dw4Track = new MusicTrack { Id = 0x10, Name = "Test" };

		var dq3rTrack = AudioConverter.ConvertTrack(dw4Track);

		Assert.Equal((ushort)0x0110, dq3rTrack.Id);
		Assert.Equal(0x10, dq3rTrack.SourceId);
	}

	[Fact]
	public void ConvertTrack_PreservesMetadata() {
		var dw4Track = new MusicTrack {
			Id = 0x10,
			Name = "Test Track",
			Category = MusicCategory.Battle,
			Chapters = [0, 1],
			Tempo = 120,
			Loops = false
		};

		var dq3rTrack = AudioConverter.ConvertTrack(dw4Track);

		Assert.Equal("Test Track", dq3rTrack.Name);
		Assert.Equal(DQ3rMusicCategory.Battle, dq3rTrack.Category);
		Assert.Equal([0, 1], dq3rTrack.Chapters);
		Assert.False(dq3rTrack.Loops);
	}

	[Fact]
	public void ConvertSoundEffect_AppliesIdOffset() {
		var dw4Sound = new SoundEffect { Id = 0x10, Name = "Test" };

		var dq3rSound = AudioConverter.ConvertSoundEffect(dw4Sound);

		Assert.Equal((ushort)0x0210, dq3rSound.Id);
		Assert.Equal(0x10, dq3rSound.SourceId);
	}

	[Fact]
	public void ConvertSoundEffect_ScalesDuration() {
		var dw4Sound = new SoundEffect { Id = 0x10, Duration = 30 };

		var dq3rSound = AudioConverter.ConvertSoundEffect(dw4Sound);

		Assert.Equal(60, dq3rSound.Duration); // 30 * 2
	}

	// ============================================================
	// Batch Conversion Tests
	// ============================================================

	[Fact]
	public void ConvertAllTracks_ConvertsAllDatabaseTracks() {
		var dq3rTracks = AudioConverter.ConvertAllTracks();

		Assert.NotEmpty(dq3rTracks);
		Assert.Equal(AudioDatabase.GetAllTracks().Length, dq3rTracks.Length);
	}

	[Fact]
	public void ConvertAllTracks_AllHaveValidIds() {
		var dq3rTracks = AudioConverter.ConvertAllTracks();

		foreach (var track in dq3rTracks) {
			Assert.True(track.Id >= AudioConverter.MusicIdOffset);
		}
	}

	[Fact]
	public void ConvertAllSoundEffects_ConvertsAllDatabaseSounds() {
		var dq3rSounds = AudioConverter.ConvertAllSoundEffects();

		Assert.NotEmpty(dq3rSounds);
		Assert.Equal(AudioDatabase.GetAllSoundEffects().Length, dq3rSounds.Length);
	}

	[Fact]
	public void ConvertAllSoundEffects_AllHaveValidIds() {
		var dq3rSounds = AudioConverter.ConvertAllSoundEffects();

		foreach (var sound in dq3rSounds) {
			Assert.True(sound.Id >= AudioConverter.SoundIdOffset);
		}
	}

	// ============================================================
	// Category Conversion Tests
	// ============================================================

	[Theory]
	[InlineData(MusicCategory.Title, DQ3rMusicCategory.Title)]
	[InlineData(MusicCategory.Overworld, DQ3rMusicCategory.Field)]
	[InlineData(MusicCategory.Town, DQ3rMusicCategory.Town)]
	[InlineData(MusicCategory.Castle, DQ3rMusicCategory.Castle)]
	[InlineData(MusicCategory.Dungeon, DQ3rMusicCategory.Dungeon)]
	[InlineData(MusicCategory.Battle, DQ3rMusicCategory.Battle)]
	[InlineData(MusicCategory.BossBattle, DQ3rMusicCategory.Boss)]
	[InlineData(MusicCategory.Victory, DQ3rMusicCategory.Fanfare)]
	[InlineData(MusicCategory.Ending, DQ3rMusicCategory.Ending)]
	public void ConvertCategory_MapsCorrectly(MusicCategory dw4Category, DQ3rMusicCategory expectedDq3r) {
		Assert.Equal(expectedDq3r, AudioConverter.ConvertCategory(dw4Category));
	}

	[Theory]
	[InlineData(SoundCategory.Menu, DQ3rSoundCategory.System)]
	[InlineData(SoundCategory.Battle, DQ3rSoundCategory.Battle)]
	[InlineData(SoundCategory.Item, DQ3rSoundCategory.Item)]
	[InlineData(SoundCategory.Status, DQ3rSoundCategory.Status)]
	[InlineData(SoundCategory.Environment, DQ3rSoundCategory.Environment)]
	[InlineData(SoundCategory.Character, DQ3rSoundCategory.Voice)]
	public void ConvertSoundCategory_MapsCorrectly(SoundCategory dw4Category, DQ3rSoundCategory expectedDq3r) {
		Assert.Equal(expectedDq3r, AudioConverter.ConvertSoundCategory(dw4Category));
	}

	// ============================================================
	// Channel Type Mapping Tests
	// ============================================================

	[Theory]
	[InlineData(ChannelType.Pulse1, DQ3rChannelType.Melody1)]
	[InlineData(ChannelType.Pulse2, DQ3rChannelType.Melody2)]
	[InlineData(ChannelType.Triangle, DQ3rChannelType.Bass)]
	[InlineData(ChannelType.Noise, DQ3rChannelType.Percussion)]
	[InlineData(ChannelType.DPCM, DQ3rChannelType.Sample)]
	public void MapChannelType_MapsCorrectly(ChannelType nesChannel, DQ3rChannelType expectedSnes) {
		Assert.Equal(expectedSnes, AudioConverter.MapChannelType(nesChannel));
	}

	// ============================================================
	// Tempo Scaling Tests
	// ============================================================

	[Fact]
	public void ScaleTempo_ZeroReturnsDefault() {
		var result = AudioConverter.ScaleTempo(0);
		Assert.Equal(120, result);
	}

	[Fact]
	public void ScaleTempo_PreservesApproximateValue() {
		var result = AudioConverter.ScaleTempo(120);
		// Should be approximately 120 (within 1%)
		Assert.InRange(result, 119, 121);
	}

	// ============================================================
	// Volume Mapping Tests
	// ============================================================

	[Fact]
	public void MapVolume_ZeroStaysZero() {
		Assert.Equal(0, AudioConverter.MapVolume(0));
	}

	[Fact]
	public void MapVolume_MaxMapsToSnesMax() {
		// NES max (15) maps to SNES max (127)
		Assert.Equal(127, AudioConverter.MapVolume(0x0F));
	}

	[Fact]
	public void MapVolume_MidValueMapsProportionally() {
		// NES 8 (about half) should map to approximately 67 (half of 127)
		var result = AudioConverter.MapVolume(8);
		Assert.InRange(result, 60, 70);
	}

	// ============================================================
	// Note Conversion Tests
	// ============================================================

	[Fact]
	public void ConvertNote_ZeroStaysZero() {
		Assert.Equal(0, AudioConverter.ConvertNote(0));
	}

	[Fact]
	public void ConvertNote_ControlCodesPassThrough() {
		Assert.Equal(0x80, AudioConverter.ConvertNote(0x80));
		Assert.Equal(0xFF, AudioConverter.ConvertNote(0xFF));
	}

	[Fact]
	public void ConvertNote_TransposesUp() {
		var result = AudioConverter.ConvertNote(0x20);
		Assert.Equal(0x2C, result); // 0x20 + 12
	}

	// ============================================================
	// Sequence Conversion Tests
	// ============================================================

	[Fact]
	public void ConvertSequence_EmptyReturnsEmpty() {
		var result = AudioConverter.ConvertSequence([]);
		Assert.Empty(result);
	}

	[Fact]
	public void ConvertSequence_NullReturnsEmpty() {
		var result = AudioConverter.ConvertSequence(null!);
		Assert.Empty(result);
	}

	[Fact]
	public void ConvertSequence_ConvertsPairs() {
		// NES: note, duration pairs
		// SNES: note, velocity, duration triples
		var nesSequence = new byte[] { 0x20, 0x10 }; // One note

		var snesSequence = AudioConverter.ConvertSequence(nesSequence);

		Assert.Equal(3, snesSequence.Length); // One triple
	}

	// ============================================================
	// Channel Conversion Tests
	// ============================================================

	[Fact]
	public void ConvertChannels_NullReturnsEmpty() {
		var result = AudioConverter.ConvertChannels(null!);
		Assert.Empty(result);
	}

	[Fact]
	public void ConvertChannels_ConvertsAllChannels() {
		var nesChannels = new ChannelData[] {
			new() { Type = ChannelType.Pulse1, Instrument = 0, VolumeEnvelope = 15 },
			new() { Type = ChannelType.Triangle, Instrument = 0, VolumeEnvelope = 15 }
		};

		var snesChannels = AudioConverter.ConvertChannels(nesChannels);

		Assert.Equal(2, snesChannels.Length);
		Assert.Equal(DQ3rChannelType.Melody1, snesChannels[0].Type);
		Assert.Equal(DQ3rChannelType.Bass, snesChannels[1].Type);
	}

	// ============================================================
	// Instrument Mapping Tests
	// ============================================================

	[Fact]
	public void MapInstrument_Pulse1_MapsToLeadRange() {
		var result = AudioConverter.MapInstrument(ChannelType.Pulse1, 0x02);
		Assert.InRange(result, 0x10, 0x1F);
	}

	[Fact]
	public void MapInstrument_Triangle_MapsToBass() {
		var result = AudioConverter.MapInstrument(ChannelType.Triangle, 0x00);
		Assert.Equal(0x01, result);
	}

	[Fact]
	public void MapInstrument_Noise_MapsToDrums() {
		var result = AudioConverter.MapInstrument(ChannelType.Noise, 0x00);
		Assert.Equal(0x40, result);
	}
}
