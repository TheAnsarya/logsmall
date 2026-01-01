namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 monster IDs to DQ3r monster IDs.
/// </summary>
public static class MonsterToDQ3r {
	/// <summary>
	/// DW4 to DQ3r monster ID mapping table.
	/// Index is DW4 monster ID, value is DQ3r monster ID.
	/// </summary>
	private static readonly ushort[] MonsterMapping = CreateMonsterMapping();

	/// <summary>
	/// Convert a DW4 monster ID to DQ3r monster ID.
	/// </summary>
	public static int ConvertMonsterId(int dw4MonsterId) {
		if (dw4MonsterId < 0 || dw4MonsterId >= MonsterMapping.Length) {
			return dw4MonsterId; // Return as-is if out of range
		}
		return MonsterMapping[dw4MonsterId];
	}

	/// <summary>
	/// Convert a DW4 monster group ID to DQ3r group ID.
	/// </summary>
	public static int ConvertMonsterGroupId(int dw4GroupId) {
		// Group IDs map differently than individual monster IDs
		// Groups reference formation patterns
		// For now, use identity mapping
		return dw4GroupId;
	}

	/// <summary>
	/// Create the monster ID mapping table.
	/// Maps monsters by name/type where both games share monsters.
	/// </summary>
	private static ushort[] CreateMonsterMapping() {
		// DW4 has ~200 monsters, DQ3r has ~256
		var mapping = new ushort[256];

		// Initialize with identity mapping
		for (int i = 0; i < 256; i++) {
			mapping[i] = (ushort)i;
		}

		// Common Dragon Quest monsters that appear in both games
		// These use similar IDs by convention in the series

		// Slimes (common across all DQ games)
		mapping[0x01] = 0x01; // Slime
		mapping[0x02] = 0x02; // Red Slime / She-Slime
		mapping[0x03] = 0x03; // Metal Slime
		mapping[0x04] = 0x04; // Healslime
		mapping[0x05] = 0x05; // Slime Knight
		mapping[0x06] = 0x06; // King Slime
		mapping[0x07] = 0x07; // Metal King Slime

		// Drackys/Bats
		mapping[0x08] = 0x08; // Dracky
		mapping[0x09] = 0x09; // Vampire Bat

		// Ghosts
		mapping[0x0A] = 0x0A; // Ghost
		mapping[0x0B] = 0x0B; // Phantom

		// Common humanoids
		mapping[0x10] = 0x10; // Imp
		mapping[0x11] = 0x11; // Demon

		// Drakes/Dragons
		mapping[0x20] = 0x20; // Dragonling
		mapping[0x21] = 0x21; // Dragon
		mapping[0x22] = 0x22; // Great Dragon

		// Note: Full mapping requires detailed comparison of both games' monster lists
		// Many DW4-unique monsters will need approximate matches

		return mapping;
	}

	/// <summary>
	/// Get DQ3r equivalent monster family.
	/// </summary>
	public static MonsterFamily ConvertMonsterFamily(DW4MonsterFamily dw4Family) => dw4Family switch {
		DW4MonsterFamily.Slime => MonsterFamily.Slime,
		DW4MonsterFamily.Beast => MonsterFamily.Beast,
		DW4MonsterFamily.Bird => MonsterFamily.Bird,
		DW4MonsterFamily.Dragon => MonsterFamily.Dragon,
		DW4MonsterFamily.Undead => MonsterFamily.Zombie,
		DW4MonsterFamily.Demon => MonsterFamily.Demon,
		DW4MonsterFamily.Material => MonsterFamily.Material,
		DW4MonsterFamily.Humanoid => MonsterFamily.Demon, // DQ3r doesn't have separate humanoid
		DW4MonsterFamily.Boss => MonsterFamily.Boss,
		_ => MonsterFamily.Material
	};
}

/// <summary>
/// DW4 monster families.
/// </summary>
public enum DW4MonsterFamily {
	Slime,
	Beast,
	Bird,
	Dragon,
	Undead,
	Demon,
	Material,
	Humanoid,
	Boss
}

/// <summary>
/// DQ3r monster families.
/// </summary>
public enum MonsterFamily {
	Slime,
	Beast,
	Bird,
	Dragon,
	Zombie,
	Demon,
	Material,
	Bug,
	Boss
}
