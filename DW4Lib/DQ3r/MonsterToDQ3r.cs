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
	/// DW4 resistance bit positions to DQ3r element names.
	/// </summary>
	private static readonly Dictionary<int, string> ResistanceBitToElement = new() {
		{ 0, "fire" },
		{ 1, "ice" },
		{ 2, "lightning" },
		{ 3, "wind" },
		{ 4, "sleep" },
		{ 5, "stopspell" },
		{ 6, "surround" },
		{ 7, "death" },
	};

	/// <summary>
	/// Convert a single DW4 monster to DQ3r format.
	/// </summary>
	public static DQ3rMonster Convert(Monster dw4Monster, int id, string name = "") {
		return new DQ3rMonster {
			Id = id,
			Name = string.IsNullOrEmpty(name) ? $"Monster_{id:D3}" : name,
			HP = ScaleStat(dw4Monster.HP, ScalingFactors.HP),
			MP = 0, // DW4 doesn't track monster MP
			Attack = ScaleStat(dw4Monster.Attack, ScalingFactors.Attack),
			Defense = ScaleStat(dw4Monster.Defense, ScalingFactors.Defense),
			Agility = ScaleStat(dw4Monster.Agility, ScalingFactors.Agility),
			Experience = ScaleStat(dw4Monster.Experience, ScalingFactors.Experience),
			Gold = ScaleStat(dw4Monster.Gold, ScalingFactors.Gold),
			ItemDrop = dw4Monster.ItemDrop,
			DropRate = 32, // Default 12.5% (32/256)
			AIPattern = dw4Monster.AIPattern,
			Spells = ConvertSpells(dw4Monster.Spells),
			Resistances = ConvertResistances(dw4Monster.Resistances),
			SpriteId = dw4Monster.SpriteID,
			PaletteId = 0,
			SourceDW4Id = id,
			Notes = $"Converted from DW4 monster {id}"
		};
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
	/// Convert DW4 spell byte to list of spell IDs.
	/// </summary>
	private static List<int> ConvertSpells(byte spellByte) {
		var spells = new List<int>();

		// DW4 stores up to 2 spell slots in one byte (4 bits each)
		int spell1 = spellByte & 0x0F;
		int spell2 = (spellByte >> 4) & 0x0F;

		if (spell1 > 0) spells.Add(MapSpellId(spell1));
		if (spell2 > 0) spells.Add(MapSpellId(spell2));

		return spells;
	}

	/// <summary>
	/// Map DW4 spell ID to DQ3r equivalent.
	/// </summary>
	private static int MapSpellId(int dw4SpellId) {
		// Basic mapping - expand as needed
		// DW4 and DQ3r share many spell names
		return dw4SpellId switch {
			1 => 1,   // Heal
			2 => 2,   // Hurt
			3 => 3,   // Sleep
			4 => 4,   // Radiant
			5 => 5,   // Stopspell
			6 => 6,   // Outside
			7 => 7,   // Return
			8 => 8,   // Repel
			9 => 9,   // Healmore
			10 => 10, // Hurtmore
			_ => dw4SpellId
		};
	}

	/// <summary>
	/// Convert DW4 resistance flags to DQ3r format.
	/// </summary>
	private static Dictionary<string, int> ConvertResistances(byte resistanceByte) {
		var resistances = new Dictionary<string, int>();

		foreach (var (bit, element) in ResistanceBitToElement) {
			bool isResistant = (resistanceByte & (1 << bit)) != 0;
			// DQ3r uses 0-100 resistance scale, DW4 uses simple on/off
			resistances[element] = isResistant ? 100 : 0;
		}

		return resistances;
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
