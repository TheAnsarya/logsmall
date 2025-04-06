namespace DQ3rAPI.Options;

public class RomFileOptions {
	public const string RomFile = "RomFile";

	public string Directory { get; set; } = String.Empty;

	public string FileName { get; set; } = String.Empty;

	public int FileSize { get; set; }

	public string CRC32 { get; set; } = String.Empty;

	public string MD5 { get; set; } = String.Empty;

	public string SHA1 { get; set; } = String.Empty;
}
