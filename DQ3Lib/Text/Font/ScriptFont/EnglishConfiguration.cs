namespace DQ3Lib.Text.Font.ScriptFont;

/// <summary>
/// Configuration for English script font in DQ3r.
/// Replaces the Japanese font configuration for localization.
/// </summary>
internal class EnglishConfiguration {
	/// <summary>
	/// Address of the font group table in the ROM.
	/// Same location as Japanese - we overwrite the existing data.
	/// </summary>
	public int GroupTableAddress = 0x151aa;

	/// <summary>
	/// Bank address where font tile data begins.
	/// </summary>
	public int BankAddress = 0x1bbd5;

	/// <summary>
	/// Size of each group structure in bytes.
	/// 5 bytes: size (12-bit), width (4-bit), height (4-bit), offset (16-bit).
	/// </summary>
	public int GroupStructureSize = 5;

	/// <summary>
	/// Number of font groups for English (fewer than Japanese).
	/// English: ~100 characters (A-Z, a-z, 0-9, punctuation, special).
	/// Japanese: 1000+ characters.
	/// </summary>
	public int Groups = 16;

	/// <summary>
	/// Maximum character width in pixels.
	/// </summary>
	public int MaxWidth = 8;

	/// <summary>
	/// Maximum character height in pixels.
	/// </summary>
	public int MaxHeight = 12;

	/// <summary>
	/// Characters per group for English font.
	/// We use 8 characters per group for efficient packing.
	/// </summary>
	public int CharsPerGroup = 8;

	/// <summary>
	/// Total English characters to generate.
	/// </summary>
	public int TotalCharacters = 104; // Space + digits + letters + punctuation + special

	/// <summary>
	/// English font character layout.
	/// Each group contains 8 characters.
	/// Group 0: Space + 0-6
	/// Group 1: 7-9 + A-D
	/// etc.
	/// </summary>
	public static readonly string[] CharacterGroups = [
		" 0123456",                 // Group 0
		"789ABCDE",                 // Group 1
		"FGHIJKLM",                 // Group 2
		"NOPQRSTU",                 // Group 3
		"VWXYZabc",                 // Group 4
		"defghijk",                 // Group 5
		"lmnopqrs",                 // Group 6
		"tuvwxyz.",                 // Group 7
		",!?':;-(",                 // Group 8
		")/\"*+=&@",               // Group 9
		"#%[]<>_^",                 // Group 10
		"~`\\|{}♪♥",               // Group 11
		"→←↑↓●○■□",               // Group 12
	];

	/// <summary>
	/// Get which group a character belongs to.
	/// </summary>
	public static int GetCharacterGroup(char c) {
		for (int i = 0; i < CharacterGroups.Length; i++) {
			if (CharacterGroups[i].Contains(c)) {
				return i;
			}
		}
		return 0; // Default to first group (contains space)
	}

	/// <summary>
	/// Get character index within its group.
	/// </summary>
	public static int GetCharacterIndex(char c) {
		for (int i = 0; i < CharacterGroups.Length; i++) {
			int index = CharacterGroups[i].IndexOf(c);
			if (index >= 0) {
				return index;
			}
		}
		return 0; // Default to space
	}
}
