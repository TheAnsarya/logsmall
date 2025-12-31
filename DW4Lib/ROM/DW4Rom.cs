using DW4Lib.DataStructures;

namespace DW4Lib.ROM;

/// <summary>
/// Dragon Warrior IV NES ROM reader and extractor.
/// Handles MMC3 mapper bank switching calculations.
/// </summary>
public class DW4Rom {
	/// <summary>
	/// Expected ROM size for DW4 (512KB PRG + 16 byte header).
	/// </summary>
	public const int ExpectedSize = 0x80010; // 524,304 bytes

	/// <summary>
	/// PRG ROM size (512KB).
	/// </summary>
	public const int PrgRomSize = 0x80000; // 524,288 bytes

	/// <summary>
	/// iNES header size.
	/// </summary>
	public const int HeaderSize = 0x10; // 16 bytes

	/// <summary>
	/// Bank size for MMC3 mapper (8KB banks).
	/// </summary>
	public const int BankSize = 0x2000; // 8,192 bytes

	/// <summary>
	/// Number of banks in the ROM.
	/// </summary>
	public const int BankCount = 64;

	/// <summary>
	/// ROM data including header.
	/// </summary>
	private readonly byte[] _data;

	/// <summary>
	/// Create a new DW4Rom instance from file.
	/// </summary>
	public DW4Rom(string filePath) {
		_data = File.ReadAllBytes(filePath);
		ValidateRom();
	}

	/// <summary>
	/// Create a new DW4Rom instance from byte array.
	/// </summary>
	public DW4Rom(byte[] data) {
		_data = data;
		ValidateRom();
	}

	/// <summary>
	/// Validate the ROM is a valid DW4 ROM.
	/// </summary>
	private void ValidateRom() {
		if (_data.Length < HeaderSize + PrgRomSize) {
			throw new ArgumentException($"ROM is too small. Expected at least {ExpectedSize} bytes.");
		}

		// Check iNES header magic
		if (_data[0] != 'N' || _data[1] != 'E' || _data[2] != 'S' || _data[3] != 0x1A) {
			throw new ArgumentException("Invalid iNES header. Missing NES magic bytes.");
		}
	}

	/// <summary>
	/// Get a byte at the specified file address (includes header).
	/// </summary>
	public byte this[int address] => _data[address];

	/// <summary>
	/// Convert a CPU address and bank number to file offset.
	/// </summary>
	public int CpuToFileOffset(int cpuAddress, int bank) {
		// MMC3 banks are 8KB
		// $8000-$9FFF and $A000-$BFFF are switchable
		// $C000-$DFFF and $E000-$FFFF are switchable or fixed

		int bankOffset = bank * BankSize;
		int addressOffset = cpuAddress & 0x1FFF; // Offset within 8KB window

		return HeaderSize + bankOffset + addressOffset;
	}

	/// <summary>
	/// Read bytes from a specific bank and CPU address.
	/// </summary>
	public byte[] ReadBytes(int cpuAddress, int bank, int length) {
		int fileOffset = CpuToFileOffset(cpuAddress, bank);
		var result = new byte[length];
		Array.Copy(_data, fileOffset, result, 0, length);
		return result;
	}

	/// <summary>
	/// Read bytes from a file offset (header-relative).
	/// </summary>
	public byte[] ReadBytesAtOffset(int offset, int length) {
		var result = new byte[length];
		Array.Copy(_data, offset, result, 0, length);
		return result;
	}

	/// <summary>
	/// Read a single byte at file offset (header-relative).
	/// </summary>
	public byte ReadByteAtOffset(int offset) {
		return _data[offset];
	}

	/// <summary>
	/// Read a 16-bit word (little endian) at file offset (header-relative).
	/// </summary>
	public ushort ReadWordAtOffset(int offset) {
		return (ushort)(_data[offset] | (_data[offset + 1] << 8));
	}

	/// <summary>
	/// Read a single byte from a bank and CPU address.
	/// </summary>
	public byte ReadByte(int cpuAddress, int bank) {
		return _data[CpuToFileOffset(cpuAddress, bank)];
	}

	/// <summary>
	/// Read a 16-bit word (little endian) from a bank and CPU address.
	/// </summary>
	public ushort ReadWord(int cpuAddress, int bank) {
		int offset = CpuToFileOffset(cpuAddress, bank);
		return (ushort)(_data[offset] | (_data[offset + 1] << 8));
	}

	/// <summary>
	/// Read all monsters from the ROM.
	/// Monster data is in Bank 6 at $A2A2 (27 bytes per entry).
	/// </summary>
	public List<Monster> ReadAllMonsters() {
		var monsters = new List<Monster>();

		// Monster data at $A2A2 in Bank 6
		// Bank 6 = file offset $C010 (header) + $6000 (bank * 8KB) = $C010 + 6*$2000
		// Actually for MMC3, bank 6 is at file offset: header + (6 * 0x2000) = 0x10 + 0xC000 = 0xC010
		// But $A2A2 is a CPU address in the $A000-$BFFF range (second 8KB window)
		// So we need bank 6 mapped to $A000, meaning offset within bank = $A2A2 - $A000 = $2A2

		const int monsterCount = 200; // Approximate count

		for (int i = 0; i < monsterCount; i++) {
			int address = Monster.TableAddress + (i * Monster.Size);
			var data = ReadBytes(address, Monster.Bank, Monster.Size);
			monsters.Add(Monster.FromBytes(data));
		}

		return monsters;
	}

	/// <summary>
	/// Read all items from the ROM.
	/// </summary>
	public List<Item> ReadAllItems() {
		var items = new List<Item>();

		// Item data location (approximate)
		const int itemBank = 14; // Bank $07
		const int itemStartAddress = 0x8000;
		const int itemCount = 150; // Approximate

		for (int i = 0; i < itemCount; i++) {
			int address = itemStartAddress + (i * Item.Size);
			var data = ReadBytes(address, itemBank, Item.Size);
			items.Add(Item.FromBytes(data));
		}

		return items;
	}

	/// <summary>
	/// Read all spells from the ROM.
	/// </summary>
	public List<Spell> ReadAllSpells() {
		var spells = new List<Spell>();

		// Spell data location (approximate)
		const int spellBank = 13; // Bank $0D
		const int spellStartAddress = 0x9000;
		const int spellCount = 70; // Approximate

		for (int i = 0; i < spellCount; i++) {
			int address = spellStartAddress + (i * Spell.Size);
			var data = ReadBytes(address, spellBank, Spell.Size);
			spells.Add(Spell.FromBytes(data));
		}

		return spells;
	}

	/// <summary>
	/// Get the raw ROM data.
	/// </summary>
	public byte[] GetRawData() => _data;

	/// <summary>
	/// Get the PRG ROM data without header.
	/// </summary>
	public byte[] GetPrgData() {
		var prg = new byte[PrgRomSize];
		Array.Copy(_data, HeaderSize, prg, 0, PrgRomSize);
		return prg;
	}
}
