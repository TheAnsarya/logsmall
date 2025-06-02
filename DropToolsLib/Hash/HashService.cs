using DropToolsLib.Extensions;
using Force.Crc32;
using System.Buffers;
using System.Security.Cryptography;

namespace DropToolsLib.Hash;

public class HashService : IHashService {
	// Read stream in 32k chunks
	private const int ChunkLength = 32 * 1024;

	// TODO: check to make sure return values match what we expect 
	public async Task<Hashes> GetAll(Stream stream) {
		ArgumentNullException.ThrowIfNull(stream, nameof(stream));

		using var crcHasher = new Crc32Algorithm();
		using var md5Hasher = MD5.Create();
		using var sha1Hasher = SHA1.Create();

		var buffer = ArrayPool<byte>.Shared.Rent(ChunkLength);
		int read;

		while ((read = await stream.ReadAsync(buffer.AsMemory(0, ChunkLength))) > 0) {
			crcHasher.TransformBlock(buffer, 0, read, null, 0);
			md5Hasher.TransformBlock(buffer, 0, read, null, 0);
			sha1Hasher.TransformBlock(buffer, 0, read, null, 0);
		}

		ArrayPool<byte>.Shared.Return(buffer);

		// Finalize the hash computation
		crcHasher.TransformFinalBlock([], 0, 0);
		md5Hasher.TransformFinalBlock([], 0, 0);
		sha1Hasher.TransformFinalBlock([], 0, 0);

		// Finalize the hash computation
		var crc = crcHasher.Hash.ToHexString();
		var md5 = md5Hasher.Hash.ToHexString();
		var sha1 = sha1Hasher.Hash.ToHexString();

		return new Hashes {
			Crc32 = Crc.From(crc),
			Md5 = Md5.From(md5),
			Sha1 = Sha1.From(sha1),
		};
	}
}
