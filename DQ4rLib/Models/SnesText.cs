namespace DQ4rLib.Models;

/// <summary>
/// Represents text/dialog data formatted for SNES
/// </summary>
public class SnesText {
	/// <summary>
	/// Dialog script entries indexed by ID
	/// </summary>
	public Dictionary<int, DialogEntry> Dialogs { get; set; } = [];

	/// <summary>
	/// Menu text strings
	/// </summary>
	public Dictionary<string, string> MenuStrings { get; set; } = [];

	/// <summary>
	/// Item names
	/// </summary>
	public List<string> ItemNames { get; set; } = [];

	/// <summary>
	/// Monster names
	/// </summary>
	public List<string> MonsterNames { get; set; } = [];

	/// <summary>
	/// Spell names
	/// </summary>
	public List<string> SpellNames { get; set; } = [];

	/// <summary>
	/// Character names (fixed characters)
	/// </summary>
	public List<string> CharacterNames { get; set; } = [];
}

/// <summary>
/// Individual dialog entry with SNES control codes
/// </summary>
public class DialogEntry {
	/// <summary>
	/// Dialog ID
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Raw text with control codes
	/// </summary>
	public string Text { get; set; } = string.Empty;

	/// <summary>
	/// Speaker name (if applicable)
	/// </summary>
	public string? Speaker { get; set; }

	/// <summary>
	/// Chapter where this dialog appears
	/// </summary>
	public int Chapter { get; set; }

	/// <summary>
	/// Location/context identifier
	/// </summary>
	public string? Location { get; set; }

	/// <summary>
	/// Convert to SNES binary format
	/// </summary>
	public byte[] ToBytes(TextEncoder encoder) {
		return encoder.Encode(Text);
	}
}

/// <summary>
/// Text encoder for SNES format using DQ3r font mapping
/// </summary>
public class TextEncoder {
	private readonly Dictionary<char, byte> _charMap = [];
	private readonly Dictionary<string, byte[]> _controlCodes = [];

	/// <summary>
	/// Load character mapping from table file
	/// </summary>
	public void LoadTable(string tablePath) {
		// Table format: HH=C (hex byte = character)
		foreach (var line in File.ReadAllLines(tablePath)) {
			if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
				continue;

			var parts = line.Split('=');
			if (parts.Length == 2 && parts[0].Length == 2) {
				if (byte.TryParse(parts[0], System.Globalization.NumberStyles.HexNumber, null, out byte value)) {
					if (parts[1].Length > 0) {
						_charMap[parts[1][0]] = value;
					}
				}
			}
		}
	}

	/// <summary>
	/// Register a control code
	/// </summary>
	public void RegisterControlCode(string code, params byte[] bytes) {
		_controlCodes[code] = bytes;
	}

	/// <summary>
	/// Encode text string to SNES bytes
	/// </summary>
	public byte[] Encode(string text) {
		var result = new List<byte>();
		int i = 0;

		while (i < text.Length) {
			// Check for control codes (format: [CODE])
			if (text[i] == '[') {
				int end = text.IndexOf(']', i);
				if (end > i) {
					string code = text[(i + 1)..end];
					if (_controlCodes.TryGetValue(code, out byte[]? bytes)) {
						result.AddRange(bytes);
						i = end + 1;
						continue;
					}
				}
			}

			// Regular character
			if (_charMap.TryGetValue(text[i], out byte value)) {
				result.Add(value);
			} else {
				// Unknown character - use space or placeholder
				result.Add(0x00);
			}
			i++;
		}

		return [.. result];
	}
}
