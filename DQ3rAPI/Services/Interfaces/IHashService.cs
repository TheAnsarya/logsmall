using DQ3rAPI.Domain.Hash;

namespace DQ3rAPI.Services.Interfaces;

public interface IHashService {
	Task<Hashes> GetAll(Stream stream);
}
