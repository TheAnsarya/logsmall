using DW4Lib.DataStructures;

namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 experience tables to DQ3r format.
/// DW4 has character-specific tables; DQ3r has vocation-based tables.
/// </summary>
public static class ExperienceTableToDQ3r {
	/// <summary>
	/// DW4 character to DQ3r vocation mapping.
	/// </summary>
	public static readonly Dictionary<string, DQ3rVocation> CharacterToVocation = new(StringComparer.OrdinalIgnoreCase) {
		["Hero"] = DQ3rVocation.Hero,
		["Ragnar"] = DQ3rVocation.Soldier,  // Chapter 1 soldier
		["Alena"] = DQ3rVocation.MartialArtist,  // Chapter 2 fighter
		["Cristo"] = DQ3rVocation.Priest,  // Chapter 2 priest
		["Brey"] = DQ3rVocation.Mage,  // Chapter 2 mage
		["Taloon"] = DQ3rVocation.Merchant,  // Chapter 3 merchant
		["Mara"] = DQ3rVocation.Mage,  // Chapter 4 dancer/mage
		["Nara"] = DQ3rVocation.Priest,  // Chapter 4 fortune teller/priest
		// DW4 doesn't have all DQ3 classes; use closest approximations
	};

	/// <summary>
	/// DQ3r max level (99 vs DW4's 50).
	/// </summary>
	public const int DQ3rMaxLevel = 99;

	/// <summary>
	/// Convert a DW4 experience table to DQ3r format.
	/// Extrapolates curve to DQ3r's higher max level.
	/// </summary>
	public static DQ3rExpTable ConvertTable(ExperienceTable dw4Table) {
		var vocation = GetVocationForCharacter(dw4Table.CharacterName);

		return new DQ3rExpTable {
			Vocation = vocation,
			VocationName = vocation.ToString(),
			SourceCharacter = dw4Table.CharacterName,
			MaxLevel = DQ3rMaxLevel,
			ExpRequired = ExtrapolateExpCurve(dw4Table.ExpForLevel, DQ3rMaxLevel)
		};
	}

	/// <summary>
	/// Convert all DW4 tables to DQ3r format.
	/// </summary>
	public static List<DQ3rExpTable> ConvertAll(ExperienceTableCollection dw4Tables) {
		var result = new List<DQ3rExpTable>();

		foreach (var table in dw4Tables.Tables) {
			result.Add(ConvertTable(table));
		}

		// Add missing DQ3r vocations with interpolated curves
		AddMissingVocations(result);

		return result;
	}

	/// <summary>
	/// Get DQ3r vocation for DW4 character name.
	/// </summary>
	public static DQ3rVocation GetVocationForCharacter(string characterName) {
		if (CharacterToVocation.TryGetValue(characterName, out var vocation)) {
			return vocation;
		}
		return DQ3rVocation.Soldier; // Default fallback
	}

	/// <summary>
	/// Extrapolate DW4 50-level curve to DQ3r 99-level curve.
	/// Uses polynomial fitting to extend the curve smoothly.
	/// </summary>
	public static uint[] ExtrapolateExpCurve(List<uint> dw4Curve, int targetLevels) {
		var result = new uint[targetLevels];

		// Copy existing values
		int copyCount = Math.Min(dw4Curve.Count, targetLevels);
		for (int i = 0; i < copyCount; i++) {
			result[i] = dw4Curve[i];
		}

		if (dw4Curve.Count >= targetLevels) {
			return result;
		}

		// Calculate average growth rate from last 10 levels
		int sampleStart = Math.Max(0, dw4Curve.Count - 11);
		int sampleEnd = dw4Curve.Count - 1;

		// Use exponential growth model: exp(level) = base * multiplier^level
		// Calculate multiplier from existing curve
		double totalGrowth = 0;
		int samples = 0;

		for (int i = sampleStart + 1; i <= sampleEnd; i++) {
			if (dw4Curve[i] > dw4Curve[i - 1] && dw4Curve[i - 1] > 0) {
				double ratio = (double)dw4Curve[i] / dw4Curve[i - 1];
				totalGrowth += ratio;
				samples++;
			}
		}

		// Average growth multiplier per level
		double avgMultiplier = samples > 0 ? totalGrowth / samples : 1.1;

		// Also calculate linear growth component
		long linearGrowth = samples > 0
			? (long)(dw4Curve[sampleEnd] - dw4Curve[sampleStart]) / (sampleEnd - sampleStart)
			: 1000;

		// Extrapolate using combination of exponential and linear growth
		// This creates a smooth curve that doesn't explode too quickly
		for (int i = dw4Curve.Count; i < targetLevels; i++) {
			// Blend between linear and exponential
			// Higher levels use more exponential growth
			double blendFactor = Math.Min(1.0, (i - dw4Curve.Count) / 30.0);

			uint exponentialValue = (uint)(result[i - 1] * avgMultiplier);
			uint linearValue = (uint)(result[i - 1] + linearGrowth);

			// Weighted blend
			result[i] = (uint)(linearValue * (1 - blendFactor) + exponentialValue * blendFactor);

			// Cap at reasonable max (prevent overflow)
			result[i] = Math.Min(result[i], 999_999_999);
		}

		return result;
	}

	/// <summary>
	/// Add DQ3r vocations not present in DW4.
	/// </summary>
	private static void AddMissingVocations(List<DQ3rExpTable> tables) {
		var existingVocations = tables.Select(t => t.Vocation).ToHashSet();

		// DQ3r vocations not in DW4: Gadabout, Sage, Thief
		// Create interpolated tables from similar classes

		if (!existingVocations.Contains(DQ3rVocation.Gadabout)) {
			// Gadabout has low exp requirements (easy to level)
			var mageTable = tables.FirstOrDefault(t => t.Vocation == DQ3rVocation.Mage);
			if (mageTable != null) {
				tables.Add(CreateScaledTable(mageTable, DQ3rVocation.Gadabout, 0.8));
			}
		}

		if (!existingVocations.Contains(DQ3rVocation.Sage)) {
			// Sage has high exp requirements (hard to level)
			var priestTable = tables.FirstOrDefault(t => t.Vocation == DQ3rVocation.Priest);
			var mageTable = tables.FirstOrDefault(t => t.Vocation == DQ3rVocation.Mage);
			if (priestTable != null && mageTable != null) {
				tables.Add(CreateAverageTable(priestTable, mageTable, DQ3rVocation.Sage, 1.3));
			}
		}

		if (!existingVocations.Contains(DQ3rVocation.Thief)) {
			// Thief has medium-low exp requirements
			var soldierTable = tables.FirstOrDefault(t => t.Vocation == DQ3rVocation.Soldier);
			if (soldierTable != null) {
				tables.Add(CreateScaledTable(soldierTable, DQ3rVocation.Thief, 0.9));
			}
		}
	}

	/// <summary>
	/// Create a scaled copy of an exp table for a different vocation.
	/// </summary>
	private static DQ3rExpTable CreateScaledTable(DQ3rExpTable source, DQ3rVocation vocation, double scale) {
		return new DQ3rExpTable {
			Vocation = vocation,
			VocationName = vocation.ToString(),
			SourceCharacter = $"Generated from {source.SourceCharacter}",
			MaxLevel = source.MaxLevel,
			ExpRequired = source.ExpRequired.Select(e => (uint)(e * scale)).ToArray()
		};
	}

	/// <summary>
	/// Create an averaged table from two sources.
	/// </summary>
	private static DQ3rExpTable CreateAverageTable(DQ3rExpTable t1, DQ3rExpTable t2, DQ3rVocation vocation, double scale) {
		int maxLevel = Math.Min(t1.MaxLevel, t2.MaxLevel);
		var expRequired = new uint[maxLevel];

		for (int i = 0; i < maxLevel; i++) {
			expRequired[i] = (uint)(((t1.ExpRequired[i] + t2.ExpRequired[i]) / 2.0) * scale);
		}

		return new DQ3rExpTable {
			Vocation = vocation,
			VocationName = vocation.ToString(),
			SourceCharacter = $"Generated from {t1.SourceCharacter} & {t2.SourceCharacter}",
			MaxLevel = maxLevel,
			ExpRequired = expRequired
		};
	}

	/// <summary>
	/// Calculate difference per level for display.
	/// </summary>
	public static uint[] CalculateDifferences(uint[] totalExp) {
		var diffs = new uint[totalExp.Length];
		diffs[0] = totalExp[0];
		for (int i = 1; i < totalExp.Length; i++) {
			diffs[i] = totalExp[i] - totalExp[i - 1];
		}
		return diffs;
	}
}

/// <summary>
/// DQ3r vocation types.
/// </summary>
public enum DQ3rVocation {
	Hero,
	Soldier,
	MartialArtist,
	Mage,
	Priest,
	Merchant,
	Gadabout,
	Sage,
	Thief
}

/// <summary>
/// DQ3r experience table.
/// </summary>
public class DQ3rExpTable {
	/// <summary>
	/// Vocation this table is for.
	/// </summary>
	public DQ3rVocation Vocation { get; set; }

	/// <summary>
	/// Vocation name.
	/// </summary>
	public string VocationName { get; set; } = "";

	/// <summary>
	/// Original DW4 character this was derived from (if any).
	/// </summary>
	public string SourceCharacter { get; set; } = "";

	/// <summary>
	/// Maximum level.
	/// </summary>
	public int MaxLevel { get; set; }

	/// <summary>
	/// Total EXP required for each level (index 0 = level 1).
	/// </summary>
	public uint[] ExpRequired { get; set; } = [];

	/// <summary>
	/// Get EXP needed to reach a specific level from level 1.
	/// </summary>
	public uint GetTotalExpForLevel(int level) {
		if (level < 1 || level > ExpRequired.Length) return 0;
		return ExpRequired[level - 1];
	}

	/// <summary>
	/// Get EXP needed to go from current level to next.
	/// </summary>
	public uint GetExpToNextLevel(int currentLevel) {
		if (currentLevel < 1 || currentLevel >= ExpRequired.Length) return 0;
		return ExpRequired[currentLevel] - ExpRequired[currentLevel - 1];
	}
}
