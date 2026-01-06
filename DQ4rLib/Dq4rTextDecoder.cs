namespace DQ4rLib;

/// <summary>
/// Dragon Warrior IV / Dragon Quest 4 (NES) text decoder using the DW4 character table.
/// Handles text decoding for items, spells, monsters, and dialog.
/// </summary>
/// <remarks>
/// Character ranges:
///   0x00: Space
///   0x01-0x0A: Digits (0-9)
///   0x0B-0x24: Lowercase (a-z)
///   0x25-0x3E: Uppercase (A-Z)
///   0x3F: Em dash (—)
///   0x65-0x79: Punctuation
///   0x80-0x81: UI symbols
///   0xF0-0xFF: Control codes
/// </remarks>
public class Dq4rTextDecoder {
	/// <summary>Byte to character mapping</summary>
	private readonly Dictionary<byte, string> _charTable = [];

	/// <summary>Character to byte mapping (for encoding)</summary>
	private readonly Dictionary<string, byte> _reverseTable = [];

	/// <summary>End of string terminator</summary>
	public const byte Terminator = 0xFF;

	/// <summary>Padding/unused byte</summary>
	public const byte PaddingByte = 0x00;

	public Dq4rTextDecoder() {
		InitializeDefaultTable();
	}

	/// <summary>
	/// Initialize with built-in DW4 character table
	/// </summary>
	private void InitializeDefaultTable() {
		// Space
		AddMapping(0x00, " ");

		// Digits 0-9 (0x01-0x0A)
		for (int i = 0; i < 10; i++) {
			AddMapping((byte)(0x01 + i), ((char)('0' + i)).ToString());
		}

		// Lowercase a-z (0x0B-0x24)
		for (int i = 0; i < 26; i++) {
			AddMapping((byte)(0x0B + i), ((char)('a' + i)).ToString());
		}

		// Uppercase A-Z (0x25-0x3E)
		for (int i = 0; i < 26; i++) {
			AddMapping((byte)(0x25 + i), ((char)('A' + i)).ToString());
		}

		// Em dash
		AddMapping(0x3F, "\u2014"); // Em dash

		// Punctuation (0x65-0x79)
		AddMapping(0x65, "\u2014");  // Em dash
		AddMapping(0x66, "\u201C");  // Left double quote "
		AddMapping(0x67, "\u201D");  // Right double quote "
		AddMapping(0x68, "\u2018");  // Left single quote '
		AddMapping(0x69, "\u2019");  // Right single quote '
		AddMapping(0x6A, "'");  // Apostrophe
		AddMapping(0x6B, "'");  // Apostrophe variant
		AddMapping(0x6C, ".'"); // Period apostrophe
		AddMapping(0x6D, "?");  // Question mark
		AddMapping(0x6E, "!");  // Exclamation mark
		AddMapping(0x6F, "-");  // Hyphen
		AddMapping(0x70, "*");  // Asterisk
		AddMapping(0x71, ":");  // Colon
		AddMapping(0x72, "\u2026");  // Ellipsis …
		AddMapping(0x73, "\u2020");  // Tombstone (dagger as substitute)
		AddMapping(0x74, "\u2620");  // Skull ☠
		AddMapping(0x75, "(");  // Left parenthesis
		AddMapping(0x76, ")");  // Right parenthesis
		AddMapping(0x77, ",");  // Comma
		AddMapping(0x78, ".");  // Period
		AddMapping(0x79, "\u300C"); // Japanese left corner bracket 「

		// UI Symbols (0x80-0x81)
		AddMapping(0x80, "\u25BC");  // Down arrow ▼
		AddMapping(0x81, "\u25B6");  // Right arrow ▶

		// Control codes (0xF0-0xFF)
		AddMapping(0xF0, "[WAIT]");
		AddMapping(0xF1, "[LINE]");
		AddMapping(0xF2, "[NAME]");
		AddMapping(0xF3, "[ITEM]");
		AddMapping(0xF4, "[NUM]");
		AddMapping(0xFE, "[PAUSE]");
		AddMapping(0xFF, "[END]");
	}

	private void AddMapping(byte code, string text) {
		_charTable[code] = text;
		if (!_reverseTable.ContainsKey(text)) {
			_reverseTable[text] = code;
		}
	}

	/// <summary>
	/// Decode fixed-length text from byte data
	/// </summary>
	/// <param name="data">Source byte array</param>
	/// <param name="offset">Starting offset</param>
	/// <param name="maxLength">Maximum bytes to read</param>
	/// <returns>Decoded string with terminator/padding stripped</returns>
	public string Decode(byte[] data, int offset, int maxLength) {
		var result = new System.Text.StringBuilder();

		int end = Math.Min(offset + maxLength, data.Length);
		for (int i = offset; i < end; i++) {
			byte b = data[i];

			// Terminator ends string
			if (b == Terminator)
				break;

			// Lookup character
			if (_charTable.TryGetValue(b, out string? c)) {
				result.Append(c);
			} else {
				// Unknown byte - show as hex placeholder
				result.Append($"[${b:X2}]");
			}
		}

		return result.ToString();
	}

	/// <summary>
	/// Decode fixed-length text from byte span
	/// </summary>
	public string Decode(ReadOnlySpan<byte> data) {
		var result = new System.Text.StringBuilder();

		foreach (byte b in data) {
			if (b == Terminator)
				break;

			if (_charTable.TryGetValue(b, out string? c)) {
				result.Append(c);
			} else {
				result.Append($"[${b:X2}]");
			}
		}

		return result.ToString();
	}

	/// <summary>
	/// Encode text to fixed-length byte array
	/// </summary>
	/// <param name="text">Text to encode</param>
	/// <param name="fixedLength">Total length including padding</param>
	/// <param name="padding">Padding byte to use</param>
	/// <returns>Fixed-length byte array</returns>
	public byte[] Encode(string text, int fixedLength, byte padding = PaddingByte) {
		var result = new byte[fixedLength];
		Array.Fill(result, padding);

		int pos = 0;
		foreach (char c in text) {
			if (pos >= fixedLength - 1) // Leave room for terminator
				break;

			string charStr = c.ToString();
			if (_reverseTable.TryGetValue(charStr, out byte b)) {
				result[pos++] = b;
			}
			// Skip characters not in table
		}

		// Add terminator if there's room
		if (pos < fixedLength) {
			result[pos] = Terminator;
		}

		return result;
	}

	/// <summary>
	/// Check if a byte is a control code
	/// </summary>
	public static bool IsControlCode(byte b) => b >= 0xF0;

	/// <summary>
	/// Check if a byte is printable text
	/// </summary>
	public bool IsPrintable(byte b) => _charTable.ContainsKey(b) && !IsControlCode(b);
}

/// <summary>
/// DW4 text table definitions for ROM extraction
/// </summary>
public static class Dq4rTextTables {
	// Note: DW4 NES addresses need to be researched
	// These are placeholder values until proper ROM analysis is done

	/// <summary>Monster names table (estimated)</summary>
	public static readonly Dq4rTextTable MonsterNames = new("monster_names", 0x1C010, 192, 8);

	/// <summary>Item names table (estimated)</summary>
	public static readonly Dq4rTextTable ItemNames = new("item_names", 0x1C810, 128, 8);

	/// <summary>Spell names table (estimated)</summary>
	public static readonly Dq4rTextTable SpellNames = new("spell_names", 0x1CC10, 64, 8);

	/// <summary>Character names table (estimated)</summary>
	public static readonly Dq4rTextTable CharacterNames = new("character_names", 0x1CE10, 16, 8);

	/// <summary>All text tables</summary>
	public static readonly Dq4rTextTable[] All = [
		MonsterNames, ItemNames, SpellNames, CharacterNames
	];
}

/// <summary>
/// Text table configuration for DW4
/// </summary>
/// <param name="Name">Table identifier</param>
/// <param name="Address">PC file offset in ROM</param>
/// <param name="Count">Number of entries</param>
/// <param name="EntryLength">Bytes per entry</param>
public record Dq4rTextTable(string Name, int Address, int Count, int EntryLength) {
	/// <summary>Total bytes for this table</summary>
	public int TotalBytes => Count * EntryLength;

	/// <summary>End address (exclusive)</summary>
	public int EndAddress => Address + TotalBytes;
}

/// <summary>
/// Extension methods for reading DW4 text with decoder
/// </summary>
public static class Dq4rTextExtensions {
	/// <summary>
	/// Read all entries from a text table
	/// </summary>
	public static string[] ReadTable(this Dq4rTextDecoder decoder, byte[] rom, Dq4rTextTable table) {
		var results = new string[table.Count];
		for (int i = 0; i < table.Count; i++) {
			int offset = table.Address + (i * table.EntryLength);
			results[i] = decoder.Decode(rom, offset, table.EntryLength);
		}
		return results;
	}

	/// <summary>
	/// Read a single entry from a text table
	/// </summary>
	public static string ReadEntry(this Dq4rTextDecoder decoder, byte[] rom, Dq4rTextTable table, int index) {
		if (index < 0 || index >= table.Count)
			throw new ArgumentOutOfRangeException(nameof(index));

		int offset = table.Address + (index * table.EntryLength);
		return decoder.Decode(rom, offset, table.EntryLength);
	}
}
