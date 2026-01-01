namespace DW4Lib.Text;

/// <summary>
/// Dragon Warrior IV text encoding and dialog system.
/// Handles conversion between game text format and ASCII/Unicode.
/// </summary>
public static class DW4TextEncoder {
	/// <summary>
	/// DW4 character table mapping (based on standard DQ/DW encoding).
	/// </summary>
	private static readonly Dictionary<byte, char> DW4ToUnicode = new() {
		// Numbers
		[0x00] = '0', [0x01] = '1', [0x02] = '2', [0x03] = '3', [0x04] = '4',
		[0x05] = '5', [0x06] = '6', [0x07] = '7', [0x08] = '8', [0x09] = '9',

		// Uppercase letters
		[0x0A] = 'A', [0x0B] = 'B', [0x0C] = 'C', [0x0D] = 'D', [0x0E] = 'E',
		[0x0F] = 'F', [0x10] = 'G', [0x11] = 'H', [0x12] = 'I', [0x13] = 'J',
		[0x14] = 'K', [0x15] = 'L', [0x16] = 'M', [0x17] = 'N', [0x18] = 'O',
		[0x19] = 'P', [0x1A] = 'Q', [0x1B] = 'R', [0x1C] = 'S', [0x1D] = 'T',
		[0x1E] = 'U', [0x1F] = 'V', [0x20] = 'W', [0x21] = 'X', [0x22] = 'Y',
		[0x23] = 'Z',

		// Lowercase letters
		[0x24] = 'a', [0x25] = 'b', [0x26] = 'c', [0x27] = 'd', [0x28] = 'e',
		[0x29] = 'f', [0x2A] = 'g', [0x2B] = 'h', [0x2C] = 'i', [0x2D] = 'j',
		[0x2E] = 'k', [0x2F] = 'l', [0x30] = 'm', [0x31] = 'n', [0x32] = 'o',
		[0x33] = 'p', [0x34] = 'q', [0x35] = 'r', [0x36] = 's', [0x37] = 't',
		[0x38] = 'u', [0x39] = 'v', [0x3A] = 'w', [0x3B] = 'x', [0x3C] = 'y',
		[0x3D] = 'z',

		// Punctuation and symbols
		[0x3E] = ' ', [0x3F] = '.', [0x40] = ',', [0x41] = '-', [0x42] = '!',
		[0x43] = '?', [0x44] = '\'', [0x45] = '"', [0x46] = ':', [0x47] = ';',
		[0x48] = '(', [0x49] = ')', [0x4A] = '/', [0x4B] = '*',

		// Special characters
		[0x4C] = '♪', // Music note (used in jingles)
		[0x4D] = '♥', // Heart
		[0x4E] = '→', // Arrow
		[0x4F] = '·', // Middle dot

		// Control codes
		[0xFE] = '\n', // Newline
		[0xFF] = '\0'  // End of string
	};

	/// <summary>
	/// Reverse mapping for encoding.
	/// </summary>
	private static readonly Dictionary<char, byte> UnicodeToD4;

	static DW4TextEncoder() {
		UnicodeToD4 = DW4ToUnicode.ToDictionary(x => x.Value, x => x.Key);
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
				result.Add(0x3E);
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
