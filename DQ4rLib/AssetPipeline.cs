using DQ4rLib.Converters;
using DQ4rLib.Data;
using DQ4rLib.Models;

namespace DQ4rLib;

/// <summary>
/// Main asset pipeline for converting DW4 NES assets to SNES format
/// </summary>
public class AssetPipeline {
	private readonly string _dw4AssetsPath;
	private readonly string _outputPath;
	private readonly TextEncoder _encoder;

	/// <summary>
	/// Create asset pipeline with input/output paths
	/// </summary>
	/// <param name="dw4AssetsPath">Path to extracted DW4 NES assets</param>
	/// <param name="outputPath">Path for SNES output files</param>
	public AssetPipeline(string dw4AssetsPath, string outputPath) {
		_dw4AssetsPath = dw4AssetsPath;
		_outputPath = outputPath;
		_encoder = TextConverter.CreateDq3rEncoder();
	}

	/// <summary>
	/// Run the complete asset conversion pipeline
	/// </summary>
	public void RunPipeline() {
		Console.WriteLine("DQ4r Asset Pipeline");
		Console.WriteLine("==================");

		// Ensure output directories exist
		Directory.CreateDirectory(_outputPath);
		Directory.CreateDirectory(Path.Combine(_outputPath, "graphics"));
		Directory.CreateDirectory(Path.Combine(_outputPath, "audio"));
		Directory.CreateDirectory(Path.Combine(_outputPath, "text"));
		Directory.CreateDirectory(Path.Combine(_outputPath, "data"));

		// Convert graphics
		ConvertGraphics();

		// Convert audio
		ConvertAudio();

		// Convert text
		ConvertText();

		// Convert data tables
		ConvertData();

		Console.WriteLine();
		Console.WriteLine("Pipeline complete!");
	}

	private void ConvertGraphics() {
		Console.WriteLine();
		Console.WriteLine("Converting graphics...");

		string chrPath = Path.Combine(_dw4AssetsPath, "chr");
		if (!Directory.Exists(chrPath)) {
			Console.WriteLine("  No CHR directory found, skipping graphics");
			return;
		}

		string graphicsOut = Path.Combine(_outputPath, "graphics");

		foreach (string chrFile in Directory.GetFiles(chrPath, "*.chr")) {
			string name = Path.GetFileNameWithoutExtension(chrFile);
			Console.WriteLine($"  Converting {name}...");

			byte[] nesChr = File.ReadAllBytes(chrFile);
			var snesGraphic = GraphicsConverter.CreateSnesGraphic(nesChr);
			GraphicsConverter.ExportToBinary(
				snesGraphic,
				Path.Combine(graphicsOut, $"{name}.4bpp")
			);
		}
	}

	private void ConvertAudio() {
		Console.WriteLine();
		Console.WriteLine("Converting audio...");

		string audioPath = Path.Combine(_dw4AssetsPath, "audio");
		if (!Directory.Exists(audioPath)) {
			Console.WriteLine("  No audio directory found, skipping audio");
			return;
		}

		// Collect DPCM samples
		var samples = new Dictionary<string, byte[]>();
		foreach (string dpcmFile in Directory.GetFiles(audioPath, "*.dpcm")) {
			string name = Path.GetFileNameWithoutExtension(dpcmFile);
			samples[name] = File.ReadAllBytes(dpcmFile);
		}

		// Collect sequences
		var sequences = new Dictionary<string, byte[]>();
		foreach (string seqFile in Directory.GetFiles(audioPath, "*.seq")) {
			string name = Path.GetFileNameWithoutExtension(seqFile);
			sequences[name] = File.ReadAllBytes(seqFile);
		}

		if (samples.Count > 0 || sequences.Count > 0) {
			var spcAudio = AudioConverter.CreateSpcAudio(samples, sequences);
			AudioConverter.ExportSpcAudio(
				spcAudio,
				Path.Combine(_outputPath, "audio")
			);
			Console.WriteLine($"  Converted {samples.Count} samples, {sequences.Count} sequences");
		}
	}

	private void ConvertText() {
		Console.WriteLine();
		Console.WriteLine("Converting text...");

		string textPath = Path.Combine(_dw4AssetsPath, "text");
		string dialogsJson = Path.Combine(textPath, "dialogs.json");

		if (File.Exists(dialogsJson)) {
			var snesText = TextConverter.ConvertDialogs(dialogsJson, string.Empty);

			// Load additional name tables if present
			LoadNameTable(Path.Combine(textPath, "items.json"), snesText.ItemNames);
			LoadNameTable(Path.Combine(textPath, "monsters.json"), snesText.MonsterNames);
			LoadNameTable(Path.Combine(textPath, "spells.json"), snesText.SpellNames);

			TextConverter.ExportToAsm(
				snesText,
				Path.Combine(_outputPath, "text"),
				_encoder
			);
			Console.WriteLine($"  Converted {snesText.Dialogs.Count} dialogs");
		} else {
			Console.WriteLine("  No dialogs.json found, skipping text");
		}
	}

	private static void LoadNameTable(string jsonPath, List<string> target) {
		if (!File.Exists(jsonPath))
			return;

		try {
			string json = File.ReadAllText(jsonPath);
			var names = System.Text.Json.JsonSerializer.Deserialize<List<string>>(json);
			if (names != null) {
				target.AddRange(names);
			}
		} catch {
			// Ignore errors for optional files
		}
	}

	private void ConvertData() {
		Console.WriteLine();
		Console.WriteLine("Converting data tables...");

		string jsonPath = Path.Combine(_dw4AssetsPath, "json");
		if (!Directory.Exists(jsonPath)) {
			Console.WriteLine("  No JSON directory found, skipping data");
			return;
		}

		DataTableConverter.ConvertAllTables(
			jsonPath,
			Path.Combine(_outputPath, "data")
		);
	}

	/// <summary>
	/// Load character mapping table
	/// </summary>
	public void LoadCharacterTable(string tablePath) {
		_encoder.LoadTable(tablePath);
	}
}
