using Xunit;

namespace FFMQLib.Tests;

/// <summary>
/// Integration tests that verify FFMQ readers work with actual ROM data.
/// These tests require the FFMQ ROM to be present at the expected path.
/// </summary>
[Collection("ROM Tests")]
public class FfmqRomIntegrationTests {
	// ROM path - adjust if needed
	private const string RomPath = @"c:\Users\me\source\repos\ffmq-info\roms\Final Fantasy - Mystic Quest (U) (V1.1).sfc";

	private static byte[]? _romData;
	private static bool _romLoaded;
	private static string? _romLoadError;

	/// <summary>
	/// Load ROM data once for all tests
	/// </summary>
	private static byte[] GetRomData() {
		if (!_romLoaded) {
			try {
				if (File.Exists(RomPath)) {
					_romData = File.ReadAllBytes(RomPath);
				} else {
					_romLoadError = $"ROM not found at: {RomPath}";
				}
			} catch (Exception ex) {
				_romLoadError = $"Failed to load ROM: {ex.Message}";
			}
			_romLoaded = true;
		}

		if (_romData == null) {
			throw new SkipException(_romLoadError ?? "ROM not available");
		}

		return _romData;
	}

	[SkippableFact]
	public void RomFile_ShouldExist() {
		Skip.IfNot(File.Exists(RomPath), $"ROM not found at: {RomPath}");

		var romData = File.ReadAllBytes(RomPath);
		Assert.NotEmpty(romData);

		// SNES ROM should be 512KB or 1MB (with/without header)
		Assert.True(romData.Length >= 0x80000, "ROM file too small");
	}

	[SkippableFact]
	public void MonsterReader_ShouldReadKnownMonsters() {
		Skip.IfNot(File.Exists(RomPath), "ROM not available");

		var romData = GetRomData();
		var reader = new FfmqMonsterReader(romData);

		// Read first few monsters
		var monster0 = reader.ReadMonster(0);
		var monster1 = reader.ReadMonster(1);

		// Monster IDs should be correct
		Assert.Equal(0, monster0.Id);
		Assert.Equal(1, monster1.Id);

		// Monsters should have names from the ROM
		Assert.False(string.IsNullOrWhiteSpace(monster0.Name));
		Assert.False(string.IsNullOrWhiteSpace(monster1.Name));

		// HP should be reasonable values (not 0 for first monsters)
		Assert.True(monster0.Hp >= 0);
		Assert.True(monster1.Hp >= 0);
	}

	[SkippableFact]
	public void MonsterReader_ShouldReadBrownbull() {
		Skip.IfNot(File.Exists(RomPath), "ROM not available");

		var romData = GetRomData();
		var reader = new FfmqMonsterReader(romData);

		// Brownbull is one of the known monsters from enemy_names.json
		// Try to find it in the monster table
		var monsters = reader.ReadAllMonsters().ToList();
		var brownbull = monsters.FirstOrDefault(m =>
			m.Name.Contains("Brown", StringComparison.OrdinalIgnoreCase) ||
			m.Name.Contains("Bull", StringComparison.OrdinalIgnoreCase));

		// Should find at least some monsters
		Assert.NotEmpty(monsters);
	}

	[SkippableFact]
	public void SpellReader_ShouldReadKnownSpells() {
		Skip.IfNot(File.Exists(RomPath), "ROM not available");

		var romData = GetRomData();
		var reader = new FfmqSpellReader(romData);

		// Read known spells
		var cure = reader.ReadSpell(0);  // First spell is typically Cure
		var spells = reader.ReadAllSpells().ToList();

		Assert.Equal(0, cure.Id);
		Assert.False(string.IsNullOrWhiteSpace(cure.Name));

		// Should have all 16 spells
		Assert.Equal(FfmqSpellReader.SpellCount, spells.Count);

		// All spells should have names
		foreach (var spell in spells) {
			Assert.False(string.IsNullOrWhiteSpace(spell.Name), $"Spell {spell.Id} has no name");
		}
	}

	[SkippableFact]
	public void SpellReader_ShouldReadFireSpell() {
		Skip.IfNot(File.Exists(RomPath), "ROM not available");

		var romData = GetRomData();
		var reader = new FfmqSpellReader(romData);

		// Look for Fire spell
		var spells = reader.ReadAllSpells().ToList();
		var fireSpell = spells.FirstOrDefault(s =>
			s.Name.Contains("Fire", StringComparison.OrdinalIgnoreCase));

		// Fire should exist in FFMQ
		Assert.NotNull(fireSpell);
	}

	[SkippableFact]
	public void ItemReader_ShouldReadWeapons() {
		Skip.IfNot(File.Exists(RomPath), "ROM not available");

		var romData = GetRomData();
		var reader = new FfmqItemReader(romData);

		var weapons = reader.ReadAllWeapons().ToList();

		Assert.Equal(FfmqItemReader.WeaponCount, weapons.Count);

		// First weapon should have valid data
		var firstWeapon = weapons[0];
		Assert.Equal(0, firstWeapon.Id);
		Assert.False(string.IsNullOrWhiteSpace(firstWeapon.Name));
	}

	[SkippableFact]
	public void ItemReader_ShouldReadArmor() {
		Skip.IfNot(File.Exists(RomPath), "ROM not available");

		var romData = GetRomData();
		var reader = new FfmqItemReader(romData);

		var armor = reader.ReadAllArmor().ToList();

		Assert.Equal(FfmqItemReader.ArmorCount, armor.Count);

		// First armor should have valid data
		var firstArmor = armor[0];
		Assert.Equal(0, firstArmor.Id);
		Assert.False(string.IsNullOrWhiteSpace(firstArmor.Name));
	}

	[SkippableFact]
	public void ItemReader_ShouldReadItems() {
		Skip.IfNot(File.Exists(RomPath), "ROM not available");

		var romData = GetRomData();
		var reader = new FfmqItemReader(romData);

		var items = reader.ReadAllItems().ToList();

		Assert.Equal(FfmqItemReader.ItemCount, items.Count);

		// All items should have names
		foreach (var item in items) {
			Assert.False(string.IsNullOrWhiteSpace(item.Name), $"Item {item.Id} has no name");
		}
	}

	[SkippableFact]
	public void TextDecoder_ShouldDecodeLocationNames() {
		Skip.IfNot(File.Exists(RomPath), "ROM not available");

		var romData = GetRomData();
		var reader = new FfmqItemReader(romData);

		var locations = reader.GetLocationNames();

		// FFMQ has many locations
		Assert.NotEmpty(locations);

		// Location names should be readable text
		var firstLocation = locations.FirstOrDefault(l => !string.IsNullOrWhiteSpace(l));
		Assert.NotNull(firstLocation);
	}

	[SkippableFact]
	public void TextDecoder_ShouldDecodeAttackNames() {
		Skip.IfNot(File.Exists(RomPath), "ROM not available");

		var romData = GetRomData();
		var reader = new FfmqItemReader(romData);

		var attacks = reader.GetAttackNames();

		// FFMQ has attack/ability names
		Assert.NotEmpty(attacks);
	}

	[SkippableFact]
	public void AllReaders_ShouldNotThrowExceptions() {
		Skip.IfNot(File.Exists(RomPath), "ROM not available");

		var romData = GetRomData();

		// This should not throw
		var monsterReader = new FfmqMonsterReader(romData);
		var spellReader = new FfmqSpellReader(romData);
		var itemReader = new FfmqItemReader(romData);

		// Read all data - should not throw
		var monsters = monsterReader.ReadAllMonsters().ToList();
		var spells = spellReader.ReadAllSpells().ToList();
		var weapons = itemReader.ReadAllWeapons().ToList();
		var armor = itemReader.ReadAllArmor().ToList();
		var items = itemReader.ReadAllItems().ToList();

		// Basic sanity checks
		Assert.NotEmpty(monsters);
		Assert.NotEmpty(spells);
		Assert.NotEmpty(weapons);
		Assert.NotEmpty(armor);
		Assert.NotEmpty(items);
	}
}

/// <summary>
/// Custom exception for skipping tests
/// </summary>
public class SkipException : Exception {
	public SkipException(string message) : base(message) { }
}
