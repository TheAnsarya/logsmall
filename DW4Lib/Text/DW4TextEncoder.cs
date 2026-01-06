namespace DW4Lib.Text;

/// <summary>
/// Dragon Warrior IV (NES) text encoding and dialog system.
/// Handles conversion between game text format and ASCII/Unicode.
/// </summary>
/// <remarks>
/// Character table verified from Dragon Warrior 4 (NES) - English.tbl
/// Ranges:
///   0x00: Space
///   0x01-0x0A: Digits (0-9)
///   0x0B-0x24: Lowercase (a-z)
///   0x25-0x3E: Uppercase (A-Z)
///   0x65-0x79: Punctuation
///   0xF0-0xFF: Control codes
/// </remarks>
public static class DW4TextEncoder {
	/// <summary>
	/// DW4 character table mapping (verified from GameInfo TBL file).
	/// </summary>
	public static readonly Dictionary<byte, char> DW4ToUnicode = new() {
		// Space
		[0x00] = ' ',

		// Numbers 0-9 (0x01-0x0A)
		[0x01] = '0', [0x02] = '1', [0x03] = '2', [0x04] = '3', [0x05] = '4',
		[0x06] = '5', [0x07] = '6', [0x08] = '7', [0x09] = '8', [0x0A] = '9',

		// Lowercase letters a-z (0x0B-0x24)
		[0x0B] = 'a', [0x0C] = 'b', [0x0D] = 'c', [0x0E] = 'd', [0x0F] = 'e',
		[0x10] = 'f', [0x11] = 'g', [0x12] = 'h', [0x13] = 'i', [0x14] = 'j',
		[0x15] = 'k', [0x16] = 'l', [0x17] = 'm', [0x18] = 'n', [0x19] = 'o',
		[0x1A] = 'p', [0x1B] = 'q', [0x1C] = 'r', [0x1D] = 's', [0x1E] = 't',
		[0x1F] = 'u', [0x20] = 'v', [0x21] = 'w', [0x22] = 'x', [0x23] = 'y',
		[0x24] = 'z',

		// Uppercase letters A-Z (0x25-0x3E)
		[0x25] = 'A', [0x26] = 'B', [0x27] = 'C', [0x28] = 'D', [0x29] = 'E',
		[0x2A] = 'F', [0x2B] = 'G', [0x2C] = 'H', [0x2D] = 'I', [0x2E] = 'J',
		[0x2F] = 'K', [0x30] = 'L', [0x31] = 'M', [0x32] = 'N', [0x33] = 'O',
		[0x34] = 'P', [0x35] = 'Q', [0x36] = 'R', [0x37] = 'S', [0x38] = 'T',
		[0x39] = 'U', [0x3A] = 'V', [0x3B] = 'W', [0x3C] = 'X', [0x3D] = 'Y',
		[0x3E] = 'Z',

		// Punctuation (0x65-0x79)
		[0x65] = '\u2014', // Em dash —
		[0x66] = '\u201C', // Left double quote "
		[0x67] = '\u201D', // Right double quote "
		[0x68] = '\u2018', // Left single quote '
		[0x69] = '\u2019', // Right single quote '
		[0x6A] = '\'',     // Apostrophe
		[0x6B] = '\'',     // Apostrophe variant
		[0x6D] = '?',      // Question mark
		[0x6E] = '!',      // Exclamation mark
		[0x6F] = '-',      // Hyphen
		[0x70] = '*',      // Asterisk
		[0x71] = ':',      // Colon
		[0x72] = '\u2026', // Ellipsis …
		[0x75] = '(',      // Left parenthesis
		[0x76] = ')',      // Right parenthesis
		[0x77] = ',',      // Comma
		[0x78] = '.',      // Period

		// Control codes
		[0xFE] = '\n', // Newline/pause
		[0xFF] = '\0'  // End of string
	};

	/// <summary>
	/// Reverse mapping for encoding.
	/// </summary>
	public static readonly Dictionary<char, byte> UnicodeToD4;

	static DW4TextEncoder() {
		// Build reverse mapping, keeping first occurrence for duplicate values
		UnicodeToD4 = [];
		foreach (var kvp in DW4ToUnicode) {
			if (!UnicodeToD4.ContainsKey(kvp.Value)) {
				UnicodeToD4[kvp.Value] = kvp.Key;
			}
		}
	}

	/// <summary>
	/// Decode DW4 text bytes to Unicode string.
	/// </summary>
	public static string Decode(byte[] data) {
		var result = new System.Text.StringBuilder();
		foreach (byte b in data) {
			if (b == 0xFF) break; // End of string
			if (DW4ToUnicode.TryGetValue(b, out char c)) {
				result.Append(c);
			} else {
				result.Append($"[{b:X2}]"); // Unknown byte
			}
		}
		return result.ToString();
	}

	/// <summary>
	/// Decode DW4 text from ROM at specified offset.
	/// </summary>
	public static string Decode(byte[] rom, int offset, int maxLength = 256) {
		var bytes = new List<byte>();
		for (int i = 0; i < maxLength && offset + i < rom.Length; i++) {
			byte b = rom[offset + i];
			if (b == 0xFF) break;
			bytes.Add(b);
		}
		return Decode(bytes.ToArray());
	}

	/// <summary>
	/// Encode Unicode string to DW4 text bytes.
	/// </summary>
	public static byte[] Encode(string text) {
		var result = new List<byte>();
		foreach (char c in text) {
			if (UnicodeToD4.TryGetValue(c, out byte b)) {
				result.Add(b);
			} else if (c == '\r') {
				// Skip carriage return
			} else {
				// Unknown character - use space
				result.Add(0x00); // Space at 0x00
			}
		}
		result.Add(0xFF); // End marker
		return result.ToArray();
	}

	/// <summary>
	/// Get the encoded length of a string (not including terminator).
	/// </summary>
	public static int GetEncodedLength(string text) {
		return text.Length; // 1 byte per character in DW4
	}
}

/// <summary>
/// Dialog box formatting for DW4.
/// </summary>
public static class DialogFormatter {
	/// <summary>
	/// Maximum characters per line in dialog box.
	/// </summary>
	public const int MaxLineLength = 18;

	/// <summary>
	/// Maximum lines visible at once in dialog box.
	/// </summary>
	public const int MaxVisibleLines = 4;

	/// <summary>
	/// Format text for dialog box display.
	/// </summary>
	public static string[] FormatForDialogBox(string text) {
		var lines = new List<string>();
		var words = text.Split(' ');
		var currentLine = "";

		foreach (var word in words) {
			if (currentLine.Length == 0) {
				currentLine = word;
			} else if (currentLine.Length + 1 + word.Length <= MaxLineLength) {
				currentLine += " " + word;
			} else {
				lines.Add(currentLine);
				currentLine = word;
			}
		}

		if (currentLine.Length > 0) {
			lines.Add(currentLine);
		}

		return lines.ToArray();
	}

	/// <summary>
	/// Split dialog into pages that fit the dialog box.
	/// </summary>
	public static string[][] SplitIntoPages(string[] lines) {
		var pages = new List<string[]>();

		for (int i = 0; i < lines.Length; i += MaxVisibleLines) {
			int count = Math.Min(MaxVisibleLines, lines.Length - i);
			var page = new string[count];
			Array.Copy(lines, i, page, 0, count);
			pages.Add(page);
		}

		return pages.ToArray();
	}

	/// <summary>
	/// Add control codes for dialog pagination.
	/// </summary>
	public static byte[] FormatWithControlCodes(string text) {
		var lines = FormatForDialogBox(text);
		var result = new List<byte>();

		for (int i = 0; i < lines.Length; i++) {
			result.AddRange(DW4TextEncoder.Encode(lines[i])[..^1]); // Remove terminator

			if (i < lines.Length - 1) {
				if ((i + 1) % MaxVisibleLines == 0) {
					result.Add(0xFD); // Wait for input, then clear
				} else {
					result.Add(0xFE); // Newline
				}
			}
		}

		result.Add(0xFF); // End marker
		return result.ToArray();
	}
}

/// <summary>
/// Dialog pointer table for DW4.
/// </summary>
public class DialogPointerTable {
	/// <summary>
	/// ROM bank containing dialog data.
	/// </summary>
	public int Bank { get; set; }

	/// <summary>
	/// Start address of pointer table in ROM.
	/// </summary>
	public int PointerTableAddress { get; set; }

	/// <summary>
	/// Number of dialog entries.
	/// </summary>
	public int EntryCount { get; set; }

	/// <summary>
	/// Pointers to each dialog entry.
	/// </summary>
	public ushort[] Pointers { get; set; } = [];

	/// <summary>
	/// Load pointer table from ROM.
	/// </summary>
	public static DialogPointerTable Load(byte[] rom, int address, int count) {
		var table = new DialogPointerTable {
			PointerTableAddress = address,
			EntryCount = count,
			Pointers = new ushort[count]
		};

		for (int i = 0; i < count; i++) {
			int offset = address + (i * 2);
			table.Pointers[i] = (ushort)(rom[offset] | (rom[offset + 1] << 8));
		}

		return table;
	}

	/// <summary>
	/// Get address of dialog entry.
	/// </summary>
	public int GetDialogAddress(int index) {
		if (index < 0 || index >= Pointers.Length) return -1;
		return Pointers[index];
	}
}

/// <summary>
/// Dialog script with branching and variables.
/// </summary>
public class DialogScript {
	public int Id { get; set; }
	public string Speaker { get; set; } = "";
	public DialogNode[] Nodes { get; set; } = [];

	/// <summary>
	/// Execute dialog script, returning text based on game state.
	/// </summary>
	public string Execute(Func<string, object?> getVariable) {
		var result = new System.Text.StringBuilder();

		foreach (var node in Nodes) {
			switch (node.Type) {
				case DialogNodeType.Text:
					result.Append(node.Text);
					break;

				case DialogNodeType.Variable:
					var value = getVariable(node.VariableName ?? "");
					result.Append(value?.ToString() ?? "???");
					break;

				case DialogNodeType.Conditional:
					var condValue = getVariable(node.ConditionVariable ?? "");
					bool condMet = node.ConditionValue?.Equals(condValue?.ToString()) ?? condValue != null;
					if (condMet) {
						result.Append(node.TrueText);
					} else if (node.FalseText != null) {
						result.Append(node.FalseText);
					}
					break;

				case DialogNodeType.LineBreak:
					result.Append('\n');
					break;
			}
		}

		return result.ToString();
	}

	/// <summary>
	/// Get raw text without variable expansion.
	/// </summary>
	public string GetRawText() {
		var result = new System.Text.StringBuilder();
		foreach (var node in Nodes) {
			if (node.Type == DialogNodeType.Text) {
				result.Append(node.Text);
			} else if (node.Type == DialogNodeType.Variable) {
				result.Append($"[{node.VariableName}]");
			} else if (node.Type == DialogNodeType.LineBreak) {
				result.Append('\n');
			}
		}
		return result.ToString();
	}
}

/// <summary>
/// Node in a dialog script.
/// </summary>
public class DialogNode {
	public DialogNodeType Type { get; set; }
	public string? Text { get; set; }
	public string? VariableName { get; set; }
	public string? ConditionVariable { get; set; }
	public string? ConditionValue { get; set; }
	public string? TrueText { get; set; }
	public string? FalseText { get; set; }
}

/// <summary>
/// Types of dialog nodes.
/// </summary>
public enum DialogNodeType {
	Text,
	Variable,
	Conditional,
	LineBreak
}

/// <summary>
/// Common dialog variables in DW4.
/// </summary>
public static class DialogVariables {
	public const string HeroName = "HERO_NAME";
	public const string PartyLeaderName = "LEADER_NAME";
	public const string CurrentGold = "GOLD";
	public const string CurrentLevel = "LEVEL";
	public const string CurrentHP = "HP";
	public const string CurrentMP = "MP";
	public const string ItemName = "ITEM_NAME";
	public const string MonsterName = "MONSTER_NAME";
	public const string LocationName = "LOCATION";
	public const string DayOrNight = "TIME_OF_DAY";
}
