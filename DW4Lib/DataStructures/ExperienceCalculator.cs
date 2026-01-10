namespace DW4Lib.DataStructures;

/// <summary>
/// Computes EXP thresholds for Dragon Warrior IV characters using the formula
/// reverse-engineered from Bank 18 ($9FB9-$A037).
/// 
/// Formula:
///   Level 2 EXP = initial_value
///   For each level N > 2:
///     - Check if level exceeds any threshold, advance rate index if so
///     - delta = (previous_delta * rate) >> 4
///     - total += delta
/// </summary>
public static class ExperienceCalculator {
	/// <summary>
	/// Maximum character level in DW4.
	/// </summary>
	public const int MaxLevel = 50;

	/// <summary>
	/// Growth rate table from Bank 18 at $A259.
	/// Rate / 16 gives the multiplier (e.g., 32 = 2.0x, 24 = 1.5x, 16 = 1.0x).
	/// </summary>
	private static readonly int[] GrowthRates = [32, 24, 20, 18, 16, 16, 30, 24, 20, 18, 16, 16];

	/// <summary>
	/// Character names by slot index.
	/// </summary>
	public static readonly string[] CharacterNames = ["Hero", "Ragnar", "Alena", "Cristo", "Brey", "Taloon", "Nara", "Mara"];

	/// <summary>
	/// Character EXP data from Bank 18 at $A123 (5 bytes per character).
	/// Format: [byte0, byte1, byte2, byte3, byte4]
	/// </summary>
	private static readonly byte[][] CharacterData = [
		[0x85, 0x8b, 0x0f, 0x2b, 0xe3], // Hero
		[0x24, 0x8c, 0x11, 0x2c, 0xe3], // Ragnar
		[0x04, 0x0c, 0x92, 0x2b, 0xe3], // Alena
		[0x84, 0x8c, 0x91, 0xad, 0x63], // Cristo
		[0xa4, 0x8c, 0x11, 0x2d, 0xe3], // Brey
		[0x04, 0x8c, 0x13, 0xac, 0x63], // Taloon
		[0x04, 0x0c, 0x94, 0xaa, 0x63], // Nara
		[0x24, 0x0c, 0x11, 0x2c, 0xe3], // Mara
	];

	/// <summary>
	/// Decode the initial EXP value (Level 2 base) from character data.
	/// Uses ROR algorithm: collect high bits, then shift right 3.
	/// </summary>
	private static int DecodeInitialValue(byte[] data) {
		// ASL A puts bit 7 in carry, ROR $7B rotates carry into bit 7
		int sevenB = 0;
		foreach (byte b in data) {
			int carry = (b >> 7) & 1;
			sevenB = (sevenB >> 1) | (carry << 7);
		}
		// Then 3 LSRs
		return sevenB >> 3;
	}

	/// <summary>
	/// Decode the starting rate index from character data.
	/// Stored in byte 0, bits 6-5, shifted right 3.
	/// </summary>
	private static int DecodeRateIndex(byte[] data) {
		return (data[0] & 0x60) >> 3;
	}

	/// <summary>
	/// Decode the 5 level thresholds from character data.
	/// Byte 0: bits 4-0 (5 bits), Bytes 1-4: bits 6-0 (7 bits).
	/// </summary>
	private static int[] DecodeThresholds(byte[] data) {
		return [
			data[0] & 0x1f,
			data[1] & 0x7f,
			data[2] & 0x7f,
			data[3] & 0x7f,
			data[4] & 0x7f
		];
	}

	/// <summary>
	/// Compute the total EXP required to reach a given level for a character.
	/// </summary>
	/// <param name="characterSlot">Character slot (0-7)</param>
	/// <param name="targetLevel">Target level (1-50)</param>
	/// <returns>Total EXP required to reach that level</returns>
	public static uint ComputeExp(int characterSlot, int targetLevel) {
		if (characterSlot < 0 || characterSlot >= CharacterData.Length) {
			throw new ArgumentOutOfRangeException(nameof(characterSlot), "Character slot must be 0-7");
		}
		if (targetLevel < 1 || targetLevel > MaxLevel) {
			throw new ArgumentOutOfRangeException(nameof(targetLevel), $"Level must be 1-{MaxLevel}");
		}

		byte[] data = CharacterData[characterSlot];
		int initial = DecodeInitialValue(data);
		int rateIdx = DecodeRateIndex(data);
		int[] thresholds = DecodeThresholds(data);

		if (targetLevel == 1) return 0;
		if (targetLevel == 2) return (uint)initial;

		uint delta = (uint)initial;
		uint total = (uint)initial;
		int threshIdx = 0;

		for (int level = 3; level <= targetLevel; level++) {
			// Check thresholds - advance rate if level > threshold
			while (threshIdx < 5 && level > thresholds[threshIdx]) {
				rateIdx++;
				threshIdx++;
			}

			// Apply growth formula: delta = (delta * rate) >> 4
			int rate = GrowthRates[Math.Min(rateIdx, GrowthRates.Length - 1)];
			delta = (uint)((delta * rate) >> 4);
			total += delta;
		}

		return total;
	}

	/// <summary>
	/// Compute EXP for a character by name.
	/// </summary>
	public static uint ComputeExp(string characterName, int targetLevel) {
		int slot = Array.FindIndex(CharacterNames, n =>
			string.Equals(n, characterName, StringComparison.OrdinalIgnoreCase));
		if (slot < 0) {
			throw new ArgumentException($"Unknown character: {characterName}", nameof(characterName));
		}
		return ComputeExp(slot, targetLevel);
	}

	/// <summary>
	/// Generate a complete experience table for a character.
	/// </summary>
	public static ExperienceTable GenerateTable(int characterSlot) {
		if (characterSlot < 0 || characterSlot >= CharacterData.Length) {
			throw new ArgumentOutOfRangeException(nameof(characterSlot));
		}

		var table = new ExperienceTable {
			CharacterId = characterSlot,
			CharacterName = CharacterNames[characterSlot]
		};

		for (int level = 1; level <= MaxLevel; level++) {
			table.ExpForLevel.Add(ComputeExp(characterSlot, level));
		}

		return table;
	}

	/// <summary>
	/// Generate experience tables for all characters.
	/// </summary>
	public static ExperienceTableCollection GenerateAllTables() {
		var collection = new ExperienceTableCollection();
		for (int slot = 0; slot < CharacterData.Length; slot++) {
			collection.Tables.Add(GenerateTable(slot));
		}
		return collection;
	}

	/// <summary>
	/// Get the raw character EXP data bytes for a slot.
	/// </summary>
	public static byte[] GetCharacterData(int characterSlot) {
		if (characterSlot < 0 || characterSlot >= CharacterData.Length) {
			throw new ArgumentOutOfRangeException(nameof(characterSlot));
		}
		return (byte[])CharacterData[characterSlot].Clone();
	}

	/// <summary>
	/// Get decoded parameters for a character.
	/// </summary>
	public static (int InitialValue, int RateIndex, int[] Thresholds) GetCharacterParams(int characterSlot) {
		if (characterSlot < 0 || characterSlot >= CharacterData.Length) {
			throw new ArgumentOutOfRangeException(nameof(characterSlot));
		}
		byte[] data = CharacterData[characterSlot];
		return (DecodeInitialValue(data), DecodeRateIndex(data), DecodeThresholds(data));
	}
}
