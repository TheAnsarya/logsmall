using DW4Lib.DataStructures;

namespace DW4Lib.DQ3r;

/// <summary>
/// Converts DW4 NES monsters to DQ3r SNES format.
/// Handles stat scaling and format differences.
/// </summary>
public static class MonsterToDQ3r {
	/// <summary>
	/// Scaling factors for 8-bit to 16-bit conversion.
	/// DW4 uses 8-bit stats, DQ3r uses 16-bit.
	/// </summary>
	public static class ScalingFactors {
		public const double HP = 4.0;        // HP scaling (255 → ~1000)
		public const double Attack = 2.5;    // Attack scaling
		public const double Defense = 2.5;   // Defense scaling
		public const double Agility = 2.0;   // Agility scaling
		public const double Experience = 3.0; // EXP scaling
		public const double Gold = 2.0;      // Gold scaling
	}

	/// <summary>
	/// Convert a single DW4 monster to DQ3r format.
	/// Note: Many DW4 monster bytes are still under research,
	/// so some DQ3r fields use defaults or raw byte values.
	/// </summary>
	public static DQ3rMonster Convert(Monster dw4Monster, int id, string name = "") {
		return new DQ3rMonster {
			Id = id,
			Name = string.IsNullOrEmpty(name) ? $"Monster_{id:D3}" : name,
			HP = ScaleStat(GetEstimatedHP(dw4Monster), ScalingFactors.HP),
			MP = 0, // DW4 doesn't track monster MP
			Attack = ScaleStat(dw4Monster.Attack, ScalingFactors.Attack),
			Defense = ScaleStat(dw4Monster.Defense, ScalingFactors.Defense),
			Agility = ScaleStat(dw4Monster.Agility, ScalingFactors.Agility),
			Experience = ScaleStat(dw4Monster.Experience, ScalingFactors.Experience),
			Gold = ScaleStat(dw4Monster.Gold, ScalingFactors.Gold),
			ItemDrop = dw4Monster.ItemDrop,
			DropRate = 32, // Default 12.5% (32/256)
			AIPattern = 0, // Unknown in 27-byte format
			Spells = new List<int>(), // Unknown in 27-byte format
			Resistances = new Dictionary<string, int>(), // Unknown in 27-byte format
			SpriteId = 0, // Unknown in 27-byte format
			PaletteId = 0,
			SourceDW4Id = id,
			Notes = BuildConversionNotes(dw4Monster, id)
		};
	}

	/// <summary>
	/// Estimate HP from unknown bytes. The exact HP byte location is still being researched.
	/// Using Byte15 as potential HP high byte combined with some pattern.
	/// </summary>
	private static int GetEstimatedHP(Monster monster) {
		// The exact HP format in 27-byte structure is unknown
		// For now, use a reasonable estimate based on Attack/Defense levels
		// This should be updated once the HP byte position is confirmed
		return Math.Max(monster.Attack, monster.Defense);
	}

	/// <summary>
	/// Build conversion notes with raw byte information for debugging.
	/// </summary>
	private static string BuildConversionNotes(Monster monster, int id) {
		var notes = new List<string> {
			$"Converted from DW4 monster {id}",
			$"IsMetal: {monster.IsMetal}"
		};

		if (monster.StatusFlags != 0) {
			notes.Add($"StatusFlags: 0x{monster.StatusFlags:X2}");
		}

		return string.Join("; ", notes);
	}

	/// <summary>
	/// Convert all monsters from DW4 ROM data.
	/// </summary>
	public static List<DQ3rMonster> ConvertAll(List<Monster> dw4Monsters, List<string>? names = null) {
		var result = new List<DQ3rMonster>();

		for (int i = 0; i < dw4Monsters.Count; i++) {
			string name = (names != null && i < names.Count) ? names[i] : "";
			result.Add(Convert(dw4Monsters[i], i, name));
		}

		return result;
	}

	/// <summary>
	/// Scale an 8-bit stat to 16-bit range.
	/// </summary>
	private static int ScaleStat(int value, double factor) {
		return (int)Math.Round(value * factor);
	}

	/// <summary>
	/// Apply custom scaling profile for specific monster types.
	/// </summary>
	public static DQ3rMonster ConvertWithProfile(Monster dw4Monster, int id, string name, MonsterProfile profile) {
		var monster = Convert(dw4Monster, id, name);

		// Apply profile adjustments
		monster.HP = (int)(monster.HP * profile.HPMultiplier);
		monster.Attack = (int)(monster.Attack * profile.AttackMultiplier);
		monster.Defense = (int)(monster.Defense * profile.DefenseMultiplier);
		monster.Experience = (int)(monster.Experience * profile.ExpMultiplier);
		monster.Gold = (int)(monster.Gold * profile.GoldMultiplier);

		return monster;
	}

	/// <summary>
	/// Profile for fine-tuning monster conversion.
	/// </summary>
	public class MonsterProfile {
		public double HPMultiplier { get; set; } = 1.0;
		public double AttackMultiplier { get; set; } = 1.0;
		public double DefenseMultiplier { get; set; } = 1.0;
		public double ExpMultiplier { get; set; } = 1.0;
		public double GoldMultiplier { get; set; } = 1.0;

		public static MonsterProfile Default => new();

		public static MonsterProfile Boss => new() {
			HPMultiplier = 1.5,
			AttackMultiplier = 1.2,
			DefenseMultiplier = 1.2,
			ExpMultiplier = 2.0,
			GoldMultiplier = 2.0
		};

		public static MonsterProfile EarlyGame => new() {
			HPMultiplier = 0.8,
			AttackMultiplier = 0.8,
			DefenseMultiplier = 0.8,
			ExpMultiplier = 1.2,
			GoldMultiplier = 1.5
		};
	}
}
