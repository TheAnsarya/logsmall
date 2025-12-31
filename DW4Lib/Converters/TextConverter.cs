using System.Text.Json;
using DW4Lib.ROM;
using DW4Lib.Text;

namespace DW4Lib.Converters;

/// <summary>
/// Converts DW4 text to/from JSON format.
/// </summary>
public static class TextConverter {
	private static readonly JsonSerializerOptions JsonOptions = new() {
		WriteIndented = true,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};

	/// <summary>
	/// Text block definition for extraction.
	/// </summary>
	public class TextBlock {
		public string Name { get; set; } = "";
		public int Bank { get; set; }
		public int StartAddress { get; set; }
		public int EndAddress { get; set; }
		public bool UsePointers { get; set; }
		public int PointerTableAddress { get; set; }
		public int EntryCount { get; set; }
	}

	/// <summary>
	/// Extracted text entry.
	/// </summary>
	public class TextEntry {
		public int Index { get; set; }
		public int Offset { get; set; }
		public string Raw { get; set; } = "";
		public string Decoded { get; set; } = "";
	}

	/// <summary>
	/// Extracted text block with all entries.
	/// </summary>
	public class ExtractedTextBlock {
		public string Name { get; set; } = "";
		public int Bank { get; set; }
		public int StartAddress { get; set; }
		public List<TextEntry> Entries { get; set; } = new();
	}

	/// <summary>
	/// Known text blocks in DW4.
	/// </summary>
	public static readonly TextBlock[] KnownBlocks = new[] {
		new TextBlock {
			Name = "MonsterNames",
			Bank = 0x24,
			PointerTableAddress = 0x8000,
			UsePointers = true,
			EntryCount = 159
		},
		new TextBlock {
			Name = "ItemNames",
			Bank = 0x25,
			PointerTableAddress = 0x8000,
			UsePointers = true,
			EntryCount = 128
		},
		new TextBlock {
			Name = "SpellNames",
			Bank = 0x25,
			PointerTableAddress = 0x8100,
			UsePointers = true,
			EntryCount = 64
		},
		new TextBlock {
			Name = "CharacterNames",
			Bank = 0x25,
			PointerTableAddress = 0x8180,
			UsePointers = true,
			EntryCount = 8
		},
	};

	/// <summary>
	/// Extract a text block from ROM.
	/// </summary>
	public static ExtractedTextBlock Extract(DW4Rom rom, TextBlock block) {
		var result = new ExtractedTextBlock {
			Name = block.Name,
			Bank = block.Bank,
			StartAddress = block.UsePointers ? block.PointerTableAddress : block.StartAddress
		};

		if (block.UsePointers) {
			// Read pointer table
			for (int i = 0; i < block.EntryCount; i++) {
				int pointerOffset = rom.CpuToFileOffset(block.PointerTableAddress + i * 2, block.Bank);
				int textPtr = rom.ReadWordAtOffset(pointerOffset);

				// Read text until end marker
				int textOffset = rom.CpuToFileOffset(textPtr, block.Bank);
				var textBytes = new List<byte>();

				for (int j = 0; j < 256; j++) { // Max length safety
					byte b = rom.ReadByteAtOffset(textOffset + j);
					textBytes.Add(b);
					if (b == 0x10) break; // End marker
				}

				var entry = new TextEntry {
					Index = i,
					Offset = textPtr,
					Raw = BitConverter.ToString(textBytes.ToArray()).Replace("-", " "),
					Decoded = DW4Text.Decode(textBytes.ToArray())
				};

				result.Entries.Add(entry);
			}
		} else {
			// Sequential text entries
			int currentOffset = rom.CpuToFileOffset(block.StartAddress, block.Bank);
			int endOffset = rom.CpuToFileOffset(block.EndAddress, block.Bank);

			int index = 0;
			while (currentOffset < endOffset && (block.EntryCount == 0 || index < block.EntryCount)) {
				var textBytes = new List<byte>();
				int startPos = currentOffset;

				for (int j = 0; j < 256; j++) {
					byte b = rom.ReadByteAtOffset(currentOffset++);
					textBytes.Add(b);
					if (b == 0x10) break;
				}

				var entry = new TextEntry {
					Index = index++,
					Offset = startPos,
					Raw = BitConverter.ToString(textBytes.ToArray()).Replace("-", " "),
					Decoded = DW4Text.Decode(textBytes.ToArray())
				};

				result.Entries.Add(entry);
			}
		}

		return result;
	}

	/// <summary>
	/// Extract all known text blocks.
	/// </summary>
	public static List<ExtractedTextBlock> ExtractAll(DW4Rom rom) {
		var results = new List<ExtractedTextBlock>();

		foreach (var block in KnownBlocks) {
			try {
				results.Add(Extract(rom, block));
			} catch {
				// Skip blocks that fail to extract
			}
		}

		return results;
	}

	/// <summary>
	/// Convert text blocks to JSON.
	/// </summary>
	public static string ToJsonString(List<ExtractedTextBlock> blocks) {
		return JsonSerializer.Serialize(blocks, JsonOptions);
	}

	/// <summary>
	/// Save text blocks to JSON file.
	/// </summary>
	public static void SaveToFile(List<ExtractedTextBlock> blocks, string path) {
		string json = ToJsonString(blocks);
		File.WriteAllText(path, json);
	}

	/// <summary>
	/// Save a single block to JSON file.
	/// </summary>
	public static void SaveBlockToFile(ExtractedTextBlock block, string path) {
		string json = JsonSerializer.Serialize(block, JsonOptions);
		File.WriteAllText(path, json);
	}

	/// <summary>
	/// Load text blocks from JSON file.
	/// </summary>
	public static List<ExtractedTextBlock>? LoadFromFile(string path) {
		string json = File.ReadAllText(path);
		return JsonSerializer.Deserialize<List<ExtractedTextBlock>>(json, JsonOptions);
	}

	/// <summary>
	/// Generate a simple text dump for review.
	/// </summary>
	public static string GenerateTextDump(ExtractedTextBlock block) {
		var sb = new System.Text.StringBuilder();
		sb.AppendLine($"# {block.Name}");
		sb.AppendLine($"# Bank: ${block.Bank:X2}");
		sb.AppendLine();

		foreach (var entry in block.Entries) {
			sb.AppendLine($"{entry.Index:D3}: {entry.Decoded}");
		}

		return sb.ToString();
	}
}
