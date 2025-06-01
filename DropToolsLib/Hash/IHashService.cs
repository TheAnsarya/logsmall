namespace DropToolsLib.Hash {
	public interface IHashService {
		Task<Hashes> GetAll(Stream stream);
	}
}
