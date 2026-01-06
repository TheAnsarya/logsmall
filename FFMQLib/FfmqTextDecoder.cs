namespace FFMQLib;

/// <summary>
/// FFMQ text decoder using simple character table (simple.tbl format).
/// Handles fixed-length text for items, weapons, armor, spells, monsters.
/// </summary>
/// <remarks>
/// Character ranges:
///   0x00-0x8F: Control codes and placeholders
///   0x90-0x99: Digits (0-9)
///   0x9A-0xB3: Uppercase (A-Z)
///   0xB4-0xCD: Lowercase (a-z)
///   0xCE-0xFF: Punctuation and special characters
/// </remarks>
public class FfmqTextDecoder {
	/// <summary>Byte to character mapping</summary>
	private readonly Dictionary<byte, char> _charTable = [];

	/// <summary>Character to byte mapping (for encoding)</summary>
	private readonly Dictionary<char, byte> _reverseTable = [];

	/// <summary>End of string terminator</summary>
	public const byte Terminator = 0x00;

	/// <summary>Padding byte (used to fill fixed-length strings)</summary>
	public const byte PaddingByte = 0x03;

	/// <summary>Alternative padding byte</summary>
	public const byte AltPaddingByte = 0xFF;

	public FfmqTextDecoder() {
		InitializeDefaultTable();
	}

	/// <summary>
	/// Initialize with built-in FFMQ character table
	/// </summary>
	private void InitializeDefaultTable() {
		// Digits 0-9 (0x90-0x99)
		for (int i = 0; i < 10; i++) {
			_charTable[(byte)(0x90 + i)] = (char)('0' + i);
			_reverseTable[(char)('0' + i)] = (byte)(0x90 + i);
		}

		// Uppercase A-Z (0x9A-0xB3)
		for (int i = 0; i < 26; i++) {
			_charTable[(byte)(0x9A + i)] = (char)('A' + i);
			_reverseTable[(char)('A' + i)] = (byte)(0x9A + i);
		}

		// Lowercase a-z (0xB4-0xCD)
		for (int i = 0; i < 26; i++) {
			_charTable[(byte)(0xB4 + i)] = (char)('a' + i);
			_reverseTable[(char)('a' + i)] = (byte)(0xB4 + i);
		}

		// Special characters
		_charTable[0x06] = '_';  // Underscore/space
		_reverseTable['_'] = 0x06;

		_charTable[0x80] = '~';
		_reverseTable['~'] = 0x80;

		_charTable[0x81] = '…';  // Ellipsis
		_reverseTable['…'] = 0x81;

		_charTable[0x83] = 'é';
		_reverseTable['é'] = 0x83;

		_charTable[0x84] = 'è';
		_reverseTable['è'] = 0x84;

		_charTable[0x87] = 'à';
		_reverseTable['à'] = 0x87;

		_charTable[0x8A] = 'ü';
		_reverseTable['ü'] = 0x8A;

		_charTable[0x8B] = 'ö';
		_reverseTable['ö'] = 0x8B;

		_charTable[0x8C] = 'ä';
		_reverseTable['ä'] = 0x8C;

		_charTable[0xCE] = '\''; // Apostrophe
		_reverseTable['\''] = 0xCE;

		_charTable[0xD0] = '.';
		_reverseTable['.'] = 0xD0;

		_charTable[0xD1] = '\''; // Another apostrophe variant
		// Don't add to reverse - use 0xCE

		_charTable[0xD2] = ',';
		_reverseTable[','] = 0xD2;

		_charTable[0xDA] = '-';
		_reverseTable['-'] = 0xDA;

		_charTable[0xDB] = '&';
		_reverseTable['&'] = 0xDB;

		_charTable[0xDC] = ':';
		_reverseTable[':'] = 0xDC;

		_charTable[0xDE] = ';';
		_reverseTable[';'] = 0xDE;

		_charTable[0xE7] = '"';
		_reverseTable['"'] = 0xE7;

		_charTable[0xEB] = '?';
		_reverseTable['?'] = 0xEB;

		_charTable[0xF7] = '!';
		_reverseTable['!'] = 0xF7;

		_charTable[0xFF] = ' ';  // Space
		_reverseTable[' '] = 0xFF;
	}

	/// <summary>
	/// Load character table from .tbl file
	/// </summary>
	/// <param name="tblPath">Path to .tbl file</param>
	public void LoadTable(string tblPath) {
		_charTable.Clear();
		_reverseTable.Clear();

		foreach (var line in File.ReadAllLines(tblPath)) {
			var trimmed = line.Trim();
			if (string.IsNullOrEmpty(trimmed) || !trimmed.Contains('='))
				continue;

			var parts = trimmed.Split('=', 2);
			if (parts.Length != 2)
				continue;

			if (!byte.TryParse(parts[0], System.Globalization.NumberStyles.HexNumber, null, out byte byteVal))
				continue;

			var charStr = parts[1];
			if (string.IsNullOrEmpty(charStr) || charStr == "#")
				continue;

			// Handle special escape sequences
			char c = charStr switch {
				"[END]" => '\0',
				"{newline}" => '\n',
				"[WAIT]" => '\x02',
				"[NAME]" => '\x04',
				"[ITEM]" => '\x05',
				_ => charStr[0]
			};

			_charTable[byteVal] = c;
			if (!_reverseTable.ContainsKey(c)) {
				_reverseTable[c] = byteVal;
			}
		}
	}

	/// <summary>
	/// Decode fixed-length text from byte data
	/// </summary>
	/// <param name="data">Source byte array</param>
	/// <param name="offset">Starting offset</param>
	/// <param name="maxLength">Maximum bytes to read</param>
	/// <returns>Decoded string with padding/terminator stripped</returns>
	public string Decode(byte[] data, int offset, int maxLength) {
		var result = new System.Text.StringBuilder();

		int end = Math.Min(offset + maxLength, data.Length);
		for (int i = offset; i < end; i++) {
			byte b = data[i];

			// Terminator ends string
			if (b == Terminator)
				break;

			// Skip padding bytes
			if (b == PaddingByte || b == AltPaddingByte)
				continue;

			// Lookup character
			if (_charTable.TryGetValue(b, out char c)) {
				result.Append(c);
			} else {
				// Unknown byte - show as hex placeholder
				result.Append($"<{b:X2}>");
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

			if (b == PaddingByte || b == AltPaddingByte)
				continue;

			if (_charTable.TryGetValue(b, out char c)) {
				result.Append(c);
			} else {
				result.Append($"<{b:X2}>");
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

			if (_reverseTable.TryGetValue(c, out byte b)) {
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
}

/// <summary>
/// FFMQ text table definitions for ROM extraction.
/// Note: Addresses are PC file offsets (e.g., 0x064BA0).
/// </summary>
public static class FfmqTextTables {
	/// <summary>Item names: 232 entries × 12 bytes at 0x064120</summary>
	public static readonly FfmqTextTable ItemNames = new("item_names", 0x064120, 232, 12);

	/// <summary>Spell names: 32 entries × 12 bytes at 0x064210</summary>
	public static readonly FfmqTextTable SpellNames = new("spell_names", 0x064210, 32, 12);

	/// <summary>Weapon names: 57 entries × 12 bytes at 0x0642A0</summary>
	public static readonly FfmqTextTable WeaponNames = new("weapon_names", 0x0642A0, 57, 12);

	/// <summary>Helmet names: 10 entries × 12 bytes at 0x064354</summary>
	public static readonly FfmqTextTable HelmetNames = new("helmet_names", 0x064354, 10, 12);

	/// <summary>Armor names: 20 entries × 12 bytes at 0x064378</summary>
	public static readonly FfmqTextTable ArmorNames = new("armor_names", 0x064378, 20, 12);

	/// <summary>Shield names: 10 entries × 12 bytes at 0x0643CC</summary>
	public static readonly FfmqTextTable ShieldNames = new("shield_names", 0x0643CC, 10, 12);

	/// <summary>Accessory names: 24 entries × 12 bytes at 0x0643FC</summary>
	public static readonly FfmqTextTable AccessoryNames = new("accessory_names", 0x0643FC, 24, 12);

	/// <summary>Attack names: 128 entries × 12 bytes at 0x064420</summary>
	public static readonly FfmqTextTable AttackNames = new("attack_names", 0x064420, 128, 12);

	/// <summary>Monster names: 256 entries × 16 bytes at 0x064BA0</summary>
	public static readonly FfmqTextTable MonsterNames = new("monster_names", 0x064BA0, 256, 16);

	/// <summary>Location names: 37 entries × 16 bytes at 0x063ED0</summary>
	public static readonly FfmqTextTable LocationNames = new("location_names", 0x063ED0, 37, 16);

	/// <summary>All text tables in ROM order</summary>
	public static readonly FfmqTextTable[] All = [
		LocationNames, ItemNames, SpellNames, WeaponNames,
		HelmetNames, ArmorNames, ShieldNames, AccessoryNames,
		AttackNames, MonsterNames
	];
}

/// <summary>
/// Text table configuration
/// </summary>
/// <param name="Name">Table identifier</param>
/// <param name="Address">PC file offset in ROM</param>
/// <param name="Count">Number of entries</param>
/// <param name="EntryLength">Bytes per entry</param>
public record FfmqTextTable(string Name, int Address, int Count, int EntryLength) {
	/// <summary>Total bytes for this table</summary>
	public int TotalBytes => Count * EntryLength;

	/// <summary>End address (exclusive)</summary>
	public int EndAddress => Address + TotalBytes;
}

/// <summary>
/// Extension methods for reading text with decoder
/// </summary>
public static class FfmqTextExtensions {
	/// <summary>
	/// Read all entries from a text table
	/// </summary>
	public static string[] ReadTable(this FfmqTextDecoder decoder, byte[] rom, FfmqTextTable table) {
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
	public static string ReadEntry(this FfmqTextDecoder decoder, byte[] rom, FfmqTextTable table, int index) {
		if (index < 0 || index >= table.Count)
			throw new ArgumentOutOfRangeException(nameof(index));

		int offset = table.Address + (index * table.EntryLength);
		return decoder.Decode(rom, offset, table.EntryLength);
	}
}
