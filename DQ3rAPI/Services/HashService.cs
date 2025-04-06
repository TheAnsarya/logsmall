using DQ3rAPI.Domain.Hash;
using DQ3rAPI.Extensions;
using DQ3rAPI.Services.Interfaces;
using Force.Crc32;
using System.Buffers;
using System.Security.Cryptography;

namespace DQ3rAPI.Services;

public class HashService : IHashService {
	// Read stream in 32k chunks
	private const int ChunkLength = 32 * 1024;

	public async Task<Hashes> GetAll(Stream stream) {
		ArgumentNullException.ThrowIfNull(stream, nameof(stream));

		using var crcHasher = new Crc32Algorithm();
		using var md5Hasher = MD5.Create();
		using var sha1Hasher = SHA1.Create();

		var buffer = ArrayPool<byte>.Shared.Rent(ChunkLength);
		int read;

		while ((read = await stream.ReadAsync(buffer.AsMemory(0, ChunkLength))) > 0) {
			// TODO: check to make sure return values match what we expect 
			crcHasher.TransformBlock(buffer, 0, read, buffer, 0);
			md5Hasher.TransformBlock(buffer, 0, read, buffer, 0);
			sha1Hasher.TransformBlock(buffer, 0, read, buffer, 0);
		}

		var crc = crcHasher.TransformFinalBlock(buffer, 0, read).ToHexString();
		var md5 = md5Hasher.TransformFinalBlock(buffer, 0, read).ToHexString();
		var sha1 = sha1Hasher.TransformFinalBlock(buffer, 0, read).ToHexString();

		ArrayPool<byte>.Shared.Return(buffer);

		return new Hashes {
			Crc32 = Crc.From(crc),
			Md5 = Md5.From(md5),
			Sha1 = Sha1.From(sha1),
		};
	}
}
