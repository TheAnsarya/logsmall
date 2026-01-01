namespace DW4Lib.Converters;

using DW4Lib.Text;

/// <summary>
/// Converts DW4 NES script text to DQ3r SNES format.
/// Handles character encoding transformation and control code mapping.
/// </summary>
public static class ScriptToDQ3r {
	/// <summary>
	/// DW4 NES control codes mapped to DQ3r SNES control codes.
	/// </summary>
	public static class DW4ControlCodes {
		/// <summary>End of string.</summary>
		public const byte EndString = 0xFF;

		/// <summary>New line.</summary>
		public const byte NewLine = 0xFE;

		/// <summary>Wait for button press (if present).</summary>
		public const byte Wait = 0xFD;

		/// <summary>Hero name placeholder.</summary>
		public const byte HeroName = 0xFC;

		/// <summary>Item name placeholder.</summary>
		public const byte ItemName = 0xFB;

		/// <summary>Number placeholder.</summary>
		public const byte Number = 0xFA;

		/// <summary>Monster name placeholder.</summary>
		public const byte MonsterName = 0xF9;

		/// <summary>Spell name placeholder.</summary>
		public const byte SpellName = 0xF8;
	}

	/// <summary>
	/// Convert DW4 string bytes to DQ3r encoded bytes.
	/// </summary>
	/// <param name="dw4Bytes">Raw DW4 NES text bytes.</param>
	/// <returns>DQ3r SNES encoded bytes (big-endian 16-bit codes).</returns>
	public static byte[] ConvertString(byte[] dw4Bytes) {
		var result = new List<byte>();

		foreach (byte b in dw4Bytes) {
			// Handle control codes first
			if (b >= 0xF8) {
				int dq3rCode = b switch {
					DW4ControlCodes.EndString => FontToDQ3r.ControlCodes.EndStringAC,
					DW4ControlCodes.NewLine => FontToDQ3r.ControlCodes.NewLine,
					DW4ControlCodes.Wait => FontToDQ3r.ControlCodes.Wait,
					DW4ControlCodes.HeroName => FontToDQ3r.ControlCodes.HeroName,
					DW4ControlCodes.ItemName => FontToDQ3r.ControlCodes.ItemName,
					DW4ControlCodes.Number => FontToDQ3r.ControlCodes.Number,
					DW4ControlCodes.MonsterName => FontToDQ3r.ControlCodes.MonsterName,
					DW4ControlCodes.SpellName => FontToDQ3r.ControlCodes.SpellName,
					_ => FontToDQ3r.ControlCodes.EndStringAC, // Unknown control = end
				};

				// Write as big-endian 16-bit
				result.Add((byte)(dq3rCode >> 8));
				result.Add((byte)(dq3rCode & 0xFF));
			}
			else {
				// Convert character byte to Unicode, then to DQ3r table code
				char c = DW4TextEncoder.DW4ToUnicode.TryGetValue(b, out char decoded) ? decoded : ' ';
				int tableCode = FontToDQ3r.GetTableCode(c);

				// Write as big-endian 16-bit
				result.Add((byte)(tableCode >> 8));
				result.Add((byte)(tableCode & 0xFF));
			}
		}

		return [.. result];
	}

	/// <summary>
	/// Convert Unicode string to DQ3r encoded bytes.
	/// </summary>
	public static byte[] EncodeString(string text) {
		var result = new List<byte>();

		foreach (char c in text) {
			int tableCode = FontToDQ3r.GetTableCode(c);

			// Write as big-endian 16-bit
			result.Add((byte)(tableCode >> 8));
			result.Add((byte)(tableCode & 0xFF));
		}

		// Add end marker
		result.Add((byte)(FontToDQ3r.ControlCodes.EndStringAC >> 8));
		result.Add((byte)(FontToDQ3r.ControlCodes.EndStringAC & 0xFF));

		return [.. result];
	}

	/// <summary>
	/// Decode DQ3r bytes back to Unicode string.
	/// </summary>
	public static string DecodeString(byte[] dq3rBytes) {
		var result = new System.Text.StringBuilder();

		for (int i = 0; i < dq3rBytes.Length - 1; i += 2) {
			int code = (dq3rBytes[i] << 8) | dq3rBytes[i + 1];

			// Check for control codes
			if (FontToDQ3r.IsControlCode(code)) {
				string controlText = code switch {
					FontToDQ3r.ControlCodes.EndStringAC => "",
					FontToDQ3r.ControlCodes.NewLine => "\n",
					FontToDQ3r.ControlCodes.Wait => "[WAIT]",
					FontToDQ3r.ControlCodes.HeroName => "[HERO]",
					FontToDQ3r.ControlCodes.Party1Name => "[PARTY1]",
					FontToDQ3r.ControlCodes.Party2Name => "[PARTY2]",
					FontToDQ3r.ControlCodes.Party3Name => "[PARTY3]",
					FontToDQ3r.ControlCodes.ItemName => "[ITEM]",
					FontToDQ3r.ControlCodes.Number => "[NUMBER]",
					FontToDQ3r.ControlCodes.MonsterName => "[MONSTER]",
					FontToDQ3r.ControlCodes.SpellName => "[SPELL]",
					_ => $"[{code:X4}]",
				};
				result.Append(controlText);

				if (code == FontToDQ3r.ControlCodes.EndStringAC ||
				    code == FontToDQ3r.ControlCodes.EndStringAE) {
					break;
				}
			}
			else {
				char c = FontToDQ3r.GetCharacter(code);
				result.Append(c);
			}
		}

		return result.ToString();
	}

	/// <summary>
	/// Batch convert all DW4 text entries to DQ3r format.
	/// </summary>
	public static Dictionary<string, List<DQ3rTextEntry>> ConvertAllText(
		Dictionary<string, List<TextEntry>> dw4Entries) {
		var result = new Dictionary<string, List<DQ3rTextEntry>>();

		foreach (var kvp in dw4Entries) {
			var dq3rList = new List<DQ3rTextEntry>();

			foreach (var entry in kvp.Value) {
				// Convert raw bytes to DQ3r format
				byte[] dq3rBytes = ConvertString(entry.RawBytes);

				dq3rList.Add(new DQ3rTextEntry {
					Index = entry.Index,
					OriginalText = entry.Text,
					DQ3rBytes = dq3rBytes,
					DQ3rText = DecodeString(dq3rBytes),
				});
			}

			result[kvp.Key] = dq3rList;
		}

		return result;
	}

	/// <summary>
	/// Represents a text entry in the original DW4 format.
	/// </summary>
	public class TextEntry {
		/// <summary>Entry index.</summary>
		public int Index { get; set; }

		/// <summary>Decoded text string.</summary>
		public string Text { get; set; } = "";

		/// <summary>Original raw bytes from DW4 ROM.</summary>
		public byte[] RawBytes { get; set; } = [];
	}

	/// <summary>
	/// Represents a converted text entry in DQ3r format.
	/// </summary>
	public class DQ3rTextEntry {
		/// <summary>Entry index.</summary>
		public int Index { get; set; }

		/// <summary>Original DW4 text.</summary>
		public string OriginalText { get; set; } = "";

		/// <summary>DQ3r encoded bytes.</summary>
		public byte[] DQ3rBytes { get; set; } = [];

		/// <summary>Decoded DQ3r text for verification.</summary>
		public string DQ3rText { get; set; } = "";
	}
}
