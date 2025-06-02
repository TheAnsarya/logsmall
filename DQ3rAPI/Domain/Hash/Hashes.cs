namespace DQ3rAPI.Domain.Hash;

public class Hashes {
	public required Md5 Md5 { get; set; }

	public required Sha1 Sha1 { get; set; }

	public required Crc Crc32 { get; set; }
}
