using DQ3rAPI.DataStructures.SNES;
using DQ3rAPI.Domain.Hash;
using DQ3rAPI.Options;
using DQ3rAPI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace DQ3rAPI.Services;

internal class RomService {
	private readonly ILogger<RomService> _logger;

	private readonly RomFileOptions _romFileOptions;

	private readonly IHashService _hashService;

	public Rom ROM { get; init; }

	public Hashes Hashes { get; init; }

	public bool VerifyRom() {
		return
			ROM.Data.Length == _romFileOptions.FileSize; // TODO: check CRC32, MD5, SHA1
	}

	public RomService(ILogger<RomService> logger, IOptions<RomFileOptions> options, IHashService hashService) {
		_logger = logger;
		_romFileOptions = options.Value;
		_hashService = hashService;
		var path = Path.Combine(_romFileOptions.Directory, _romFileOptions.FileName);
		ROM = new(path);
		Hashes = _hashService.GetAll(new MemoryStream(File.ReadAllBytes(path))).Result;
	}
}
