namespace DW4Lib.DataStructures;

/// <summary>
/// Dragon Warrior IV experience table for character level progression.
/// Each character has their own experience table defining EXP needed per level.
/// </summary>
public class ExperienceTable {
	/// <summary>
	/// Maximum level in DW4.
	/// </summary>
	public const int MaxLevel = 50;

	/// <summary>
	/// Character name or ID this table belongs to.
	/// </summary>
	public string CharacterName { get; set; } = "";

	/// <summary>
	/// Character index (0-9 for party members).
	/// </summary>
	public int CharacterId { get; set; }

	/// <summary>
	/// EXP required for each level (index 0 = level 1, etc.).
	/// Values are cumulative totals needed to reach that level.
	/// </summary>
	public List<uint> ExpForLevel { get; set; } = new();

	/// <summary>
	/// Calculate EXP needed to go from one level to the next.
	/// </summary>
	public uint ExpToNextLevel(int currentLevel) {
		if (currentLevel < 1 || currentLevel >= ExpForLevel.Count) {
			return 0;
		}
		return ExpForLevel[currentLevel] - ExpForLevel[currentLevel - 1];
	}

	/// <summary>
	/// Get level for a given total EXP amount.
	/// </summary>
	public int GetLevelForExp(uint totalExp) {
		for (int i = ExpForLevel.Count - 1; i >= 0; i--) {
			if (totalExp >= ExpForLevel[i]) {
				return i + 1;
			}
		}
		return 1;
	}
}

/// <summary>
/// Collection of all character experience tables.
/// </summary>
public class ExperienceTableCollection {
	/// <summary>
	/// Experience tables indexed by character ID.
	/// </summary>
	public List<ExperienceTable> Tables { get; set; } = new();

	/// <summary>
	/// Get table for a specific character.
	/// </summary>
	public ExperienceTable? GetTable(int characterId) {
		return Tables.FirstOrDefault(t => t.CharacterId == characterId);
	}

	/// <summary>
	/// Get table by character name.
	/// </summary>
	public ExperienceTable? GetTable(string characterName) {
		return Tables.FirstOrDefault(t =>
			string.Equals(t.CharacterName, characterName, StringComparison.OrdinalIgnoreCase));
	}
}
