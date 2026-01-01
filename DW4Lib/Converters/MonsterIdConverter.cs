namespace DW4Lib.Converters;

/// <summary>
/// Monster ID converter facade.
/// Wraps MonsterToDQ3r for use by other converters.
/// </summary>
public static class MonsterIdConverter {
	/// <summary>
	/// Base offset for DQ3r regular monster IDs.
	/// </summary>
	private const int DQ3R_MONSTER_BASE = 0x000;

	/// <summary>
	/// Base offset for DQ3r boss monster IDs.
	/// </summary>
	private const int DQ3R_BOSS_BASE = 0x100;

	/// <summary>
	/// Convert a DW4 monster ID to DQ3r format.
	/// </summary>
	public static int ConvertMonsterId(byte dw4MonsterId) {
		if (dw4MonsterId == 0) return 0;

		// Check if it's a boss monster (DW4 bosses are typically 0x80+)
		if (dw4MonsterId >= 0x80) {
			return ConvertBossId(dw4MonsterId);
		}

		// Regular monster conversion
		int baseId = MonsterToDQ3r.ConvertMonsterId(dw4MonsterId);
		return DQ3R_MONSTER_BASE + baseId;
	}

	/// <summary>
	/// Convert a DW4 boss monster ID to DQ3r format.
	/// </summary>
	public static int ConvertBossId(byte dw4BossId) {
		// DW4 boss IDs start at 0x80
		// Map to DQ3r boss range starting at 0x100
		int bossIndex = dw4BossId - 0x80;
		return DQ3R_BOSS_BASE + bossIndex;
	}

	/// <summary>
	/// Convert a DW4 monster group ID to DQ3r format.
	/// </summary>
	public static int ConvertGroupId(byte dw4GroupId) {
		return MonsterToDQ3r.ConvertMonsterGroupId(dw4GroupId);
	}

	/// <summary>
	/// Batch convert monster IDs.
	/// </summary>
	public static int[] ConvertMonsterIds(byte[] dw4MonsterIds) {
		return dw4MonsterIds.Select(ConvertMonsterId).ToArray();
	}

	/// <summary>
	/// Check if a DW4 monster ID represents a boss.
	/// </summary>
	public static bool IsBoss(byte dw4MonsterId) {
		return dw4MonsterId >= 0x80;
	}

	/// <summary>
	/// Get the encounter rate scaling factor.
	/// DQ3r uses a different encounter system.
	/// </summary>
	public static double GetEncounterRateScale() {
		// DW4 encounter rates are 0-31 (5 bits)
		// DQ3r encounter rates are 0-255 (8 bits)
		return 8.0;
	}
}
