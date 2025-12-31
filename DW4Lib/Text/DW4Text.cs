namespace DW4Lib.Text;

/// <summary>
/// Dragon Warrior IV NES text encoding/decoding.
/// Handles custom character encoding and DTE (Dual Tile Encoding) compression.
/// </summary>
public static class DW4Text {
	/// <summary>
	/// DW4 character to ASCII mapping.
	/// </summary>
	private static readonly Dictionary<byte, char> CharToAscii = new() {
		// Uppercase A-Z ($80-$99)
		{ 0x80, 'A' }, { 0x81, 'B' }, { 0x82, 'C' }, { 0x83, 'D' },
		{ 0x84, 'E' }, { 0x85, 'F' }, { 0x86, 'G' }, { 0x87, 'H' },
		{ 0x88, 'I' }, { 0x89, 'J' }, { 0x8A, 'K' }, { 0x8B, 'L' },
		{ 0x8C, 'M' }, { 0x8D, 'N' }, { 0x8E, 'O' }, { 0x8F, 'P' },
		{ 0x90, 'Q' }, { 0x91, 'R' }, { 0x92, 'S' }, { 0x93, 'T' },
		{ 0x94, 'U' }, { 0x95, 'V' }, { 0x96, 'W' }, { 0x97, 'X' },
		{ 0x98, 'Y' }, { 0x99, 'Z' },

		// Lowercase a-z ($9A-$B3)
		{ 0x9A, 'a' }, { 0x9B, 'b' }, { 0x9C, 'c' }, { 0x9D, 'd' },
		{ 0x9E, 'e' }, { 0x9F, 'f' }, { 0xA0, 'g' }, { 0xA1, 'h' },
		{ 0xA2, 'i' }, { 0xA3, 'j' }, { 0xA4, 'k' }, { 0xA5, 'l' },
		{ 0xA6, 'm' }, { 0xA7, 'n' }, { 0xA8, 'o' }, { 0xA9, 'p' },
		{ 0xAA, 'q' }, { 0xAB, 'r' }, { 0xAC, 's' }, { 0xAD, 't' },
		{ 0xAE, 'u' }, { 0xAF, 'v' }, { 0xB0, 'w' }, { 0xB1, 'x' },
		{ 0xB2, 'y' }, { 0xB3, 'z' },

		// Numbers 0-9 ($B4-$BD)
		{ 0xB4, '0' }, { 0xB5, '1' }, { 0xB6, '2' }, { 0xB7, '3' },
		{ 0xB8, '4' }, { 0xB9, '5' }, { 0xBA, '6' }, { 0xBB, '7' },
		{ 0xBC, '8' }, { 0xBD, '9' },

		// Punctuation
		{ 0xBE, '.' }, { 0xBF, ',' }, { 0xC0, '!' }, { 0xC1, '?' },
		{ 0xC2, '\'' }, { 0xC3, '"' }, { 0xC4, ':' }, { 0xC5, ';' },
		{ 0xC6, '-' }, { 0xC7, '/' }, { 0xC8, '(' }, { 0xC9, ')' },
		{ 0xCA, ' ' },
	};

	/// <summary>
	/// ASCII to DW4 character mapping (reverse of CharToAscii).
	/// </summary>
	private static readonly Dictionary<char, byte> AsciiToChar;

	/// <summary>
	/// DTE (Dual Tile Encoding) pairs ($E0-$FF).
	/// </summary>
	private static readonly Dictionary<byte, string> DtePairs = new() {
		{ 0xE0, "th" }, { 0xE1, "he" }, { 0xE2, "in" }, { 0xE3, "er" },
		{ 0xE4, "an" }, { 0xE5, "re" }, { 0xE6, "on" }, { 0xE7, "en" },
		{ 0xE8, "at" }, { 0xE9, "ed" }, { 0xEA, "ou" }, { 0xEB, "to" },
		{ 0xEC, "it" }, { 0xED, "es" }, { 0xEE, "or" }, { 0xEF, "nd" },
		{ 0xF0, "st" }, { 0xF1, "is" }, { 0xF2, "le" }, { 0xF3, "ng" },
		{ 0xF4, "te" }, { 0xF5, "al" }, { 0xF6, "ar" }, { 0xF7, "se" },
		{ 0xF8, "ve" }, { 0xF9, "me" }, { 0xFA, "ll" }, { 0xFB, " t" },
		{ 0xFC, "yo" }, { 0xFD, "of" }, { 0xFE, "ha" }, { 0xFF, "ne" },
	};

	/// <summary>
	/// Reverse DTE lookup (string to byte).
	/// </summary>
	private static readonly Dictionary<string, byte> ReverseDte;

	/// <summary>
	/// Control code names.
	/// </summary>
	private static readonly Dictionary<byte, string> ControlCodes = new() {
		{ 0x00, "[NOP]" },
		{ 0x01, "[DELAY]" },
		{ 0x02, "[CLEAR]" },
		{ 0x03, "[SCROLL]" },
		{ 0x10, "[END]" },
		{ 0x11, "[LINE]" },
		{ 0x12, "[WAIT]" },
		{ 0x13, "[NAME]" },
		{ 0x14, "[ITEM]" },
		{ 0x15, "[NUM]" },
		{ 0x16, "[MONSTER]" },
		{ 0x17, "[GOLD]" },
	};

	static DW4Text() {
		// Build reverse character mapping
		AsciiToChar = CharToAscii.ToDictionary(kv => kv.Value, kv => kv.Key);

		// Build reverse DTE mapping
		ReverseDte = DtePairs.ToDictionary(kv => kv.Value, kv => kv.Key);
	}

	/// <summary>
	/// Decode DW4 encoded text to ASCII string.
	/// </summary>
	public static string Decode(byte[] data, int offset = 0, int maxLength = -1) {
		var result = new System.Text.StringBuilder();
		int length = maxLength > 0 ? Math.Min(maxLength, data.Length - offset) : data.Length - offset;

		for (int i = 0; i < length; i++) {
			byte b = data[offset + i];

			// End marker
			if (b == 0x10) break;

			// Control codes
			if (ControlCodes.TryGetValue(b, out string? control)) {
				result.Append(control);
				if (b == 0x11) result.Append('\n'); // Add actual newline after [LINE]
				continue;
			}

			// DTE pairs
			if (DtePairs.TryGetValue(b, out string? dte)) {
				result.Append(dte);
				continue;
			}

			// Regular characters
			if (CharToAscii.TryGetValue(b, out char c)) {
				result.Append(c);
				continue;
			}

			// Unknown byte
			result.Append($"[${b:X2}]");
		}

		return result.ToString();
	}

	/// <summary>
	/// Encode ASCII string to DW4 format with optional DTE compression.
	/// </summary>
	public static byte[] Encode(string text, bool useDte = true) {
		var result = new List<byte>();
		int i = 0;

		while (i < text.Length) {
			// Check for control codes in brackets
			if (text[i] == '[') {
				int endBracket = text.IndexOf(']', i);
				if (endBracket > i) {
					string code = text.Substring(i, endBracket - i + 1);

					// Find matching control code
					var match = ControlCodes.FirstOrDefault(kv => kv.Value == code);
					if (match.Value != null) {
						result.Add(match.Key);
						i = endBracket + 1;
						continue;
					}

					// Check for hex byte [$XX]
					if (code.Length == 5 && code[1] == '$') {
						if (byte.TryParse(code.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out byte hex)) {
							result.Add(hex);
							i = endBracket + 1;
							continue;
						}
					}
				}
			}

			// Skip newlines (they're implied by [LINE])
			if (text[i] == '\n') {
				i++;
				continue;
			}

			// Try DTE compression (2-char pairs)
			if (useDte && i + 1 < text.Length) {
				string pair = text.Substring(i, 2).ToLower();
				if (ReverseDte.TryGetValue(pair, out byte dteByte)) {
					result.Add(dteByte);
					i += 2;
					continue;
				}
			}

			// Regular character
			char c = text[i];
			if (AsciiToChar.TryGetValue(c, out byte charByte)) {
				result.Add(charByte);
			} else if (AsciiToChar.TryGetValue(char.ToUpper(c), out byte upperByte)) {
				result.Add(upperByte);
			} else if (AsciiToChar.TryGetValue(char.ToLower(c), out byte lowerByte)) {
				result.Add(lowerByte);
			} else {
				// Unknown character - use space
				result.Add(0xCA);
			}

			i++;
		}

		// Add end marker
		result.Add(0x10);

		return result.ToArray();
	}

	/// <summary>
	/// Decode a fixed-length name (no DTE, padded with spaces).
	/// </summary>
	public static string DecodeName(byte[] data, int offset = 0, int length = 8) {
		var result = new System.Text.StringBuilder();

		for (int i = 0; i < length && offset + i < data.Length; i++) {
			byte b = data[offset + i];

			if (b == 0x00 || b == 0xCA) {
				// Null or space - end of name or padding
				if (result.Length > 0 && b == 0x00) break;
				continue;
			}

			if (CharToAscii.TryGetValue(b, out char c)) {
				result.Append(c);
			}
		}

		return result.ToString().Trim();
	}

	/// <summary>
	/// Encode a name to fixed-length format (no DTE, space padded).
	/// </summary>
	public static byte[] EncodeName(string name, int length = 8) {
		var result = new byte[length];
		Array.Fill(result, (byte)0xCA); // Fill with spaces

		for (int i = 0; i < Math.Min(name.Length, length); i++) {
			char c = name[i];
			if (AsciiToChar.TryGetValue(c, out byte b)) {
				result[i] = b;
			} else if (AsciiToChar.TryGetValue(char.ToUpper(c), out byte upper)) {
				result[i] = upper;
			}
		}

		return result;
	}
}
