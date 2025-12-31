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

	/// <summary>
	/// Read experience tables from the ROM.
	/// Based on research, Bank 8 contains experience table data.
	/// Tables appear to be 3-byte entries (24-bit EXP values).
	/// </summary>
	public ExperienceTableCollection ReadExperienceTables() {
		var collection = new ExperienceTableCollection();

		// Known character names for DW4
		var characterNames = new[] {
			"Hero (Ch5)", "Ragnar", "Alena", "Cristo", "Brey",
			"Taloon", "Nara", "Mara", "Panon", "Orin"
		};

		// Research suggests tables at Bank 8, starting around $A866
		// Each table appears to be 50 levels * 3 bytes = 150 bytes
		// But the exact layout needs verification

		const int expBank = 8;
		const int expTableStart = 0xA866; // First candidate from research
		const int levelsPerTable = 50;
		const int bytesPerEntry = 3; // 24-bit values
		const int tableSize = levelsPerTable * bytesPerEntry;

		for (int charId = 0; charId < characterNames.Length; charId++) {
			var table = new ExperienceTable {
				CharacterId = charId,
				CharacterName = characterNames[charId]
			};

			int tableAddress = expTableStart + (charId * tableSize);

			for (int level = 0; level < levelsPerTable; level++) {
				int address = tableAddress + (level * bytesPerEntry);
				var expData = ReadBytes(address, expBank, bytesPerEntry);

				// Read 24-bit value (little endian)
				uint exp = (uint)(expData[0] | (expData[1] << 8) | (expData[2] << 16));
				table.ExpForLevel.Add(exp);
			}

			collection.Tables.Add(table);
		}

		return collection;
	}

	/// <summary>
	/// Scan for potential experience table locations.
	/// Returns addresses where ascending 3-byte sequences are found.
	/// </summary>
	public List<(int bank, int address, List<uint> firstFive)> ScanForExpTables(int minBank = 0, int maxBank = 63) {
		var candidates = new List<(int bank, int address, List<uint> firstFive)>();

		for (int bank = minBank; bank <= maxBank; bank++) {
			// Scan through the bank
			for (int addr = 0x8000; addr < 0xBF00; addr++) {
				var values = new List<uint>();
				bool ascending = true;
				uint prevValue = 0;

				// Check for 5 ascending 3-byte values
				for (int i = 0; i < 5; i++) {
					var data = ReadBytes(addr + (i * 3), bank, 3);
					uint value = (uint)(data[0] | (data[1] << 8) | (data[2] << 16));

					if (value <= prevValue && i > 0) {
						ascending = false;
						break;
					}

					// Filter: reasonable EXP values (not too high, not too low)
					if (value > 0xFFFFFF || (i > 0 && value < 100)) {
						ascending = false;
						break;
					}

					values.Add(value);
					prevValue = value;
				}

				if (ascending && values.Count == 5 && values[0] > 0 && values[0] < 1000) {
					candidates.Add((bank, addr, values));
				}
			}
		}

		return candidates;
	}

	// ============================================================
	// Map Reading Methods
	// ============================================================

	/// <summary>
	/// Read map pointer table from Bank $17.
	/// Returns array of 73 pointers to map info structures.
	/// </summary>
	public ushort[] ReadMapPointerTable() {
		const int pointerTableBank = 0x17;
		const int pointerTableAddress = 0xB08D;
		const int mapCount = 73;

		var pointers = new ushort[mapCount];
		for (int i = 0; i < mapCount; i++) {
			pointers[i] = ReadWord(pointerTableAddress + (i * 2), pointerTableBank);
		}

		return pointers;
	}

	/// <summary>
	/// Read map info data for a specific map.
	/// Returns array of MapInfo for each submap.
	/// </summary>
	public DataStructures.Maps.MapInfo[] ReadMapInfo(int mapId) {
		var pointers = ReadMapPointerTable();
		if (mapId < 0 || mapId >= pointers.Length) {
			throw new ArgumentOutOfRangeException(nameof(mapId), $"Map ID must be 0-{pointers.Length - 1}");
		}

		// Get pointer for this map and next map (or end) to determine submap count
		ushort startPtr = pointers[mapId];
		ushort endPtr = mapId < pointers.Length - 1 ? pointers[mapId + 1] : (ushort)0xB4AE;

		int submapCount = (endPtr - startPtr) / 3;
		var submaps = new DataStructures.Maps.MapInfo[submapCount];

		const int infoBank = 0x17;
		for (int i = 0; i < submapCount; i++) {
			int address = startPtr + (i * 3);
			var data = ReadBytes(address, infoBank, 3);

			submaps[i] = new DataStructures.Maps.MapInfo {
				MapId = mapId,
				SubmapIndex = i,
				TilesetNumber = data[0],
				MapDataAddress = (ushort)(data[1] | (data[2] << 8)),
				DataBank = GetMapDataBank(mapId)
			};
		}

		return submaps;
	}

	/// <summary>
	/// Get the bank containing map data for a given map ID.
	/// Based on ROM research: Banks $09, $0A, $0B.
	/// </summary>
	public static int GetMapDataBank(int mapId) {
		// Maps $00-$2C are in Bank $09
		// Maps $2D-$45 are in Bank $0A
		// Maps $45-$48 are in Bank $0B
		// Note: Some maps span multiple banks

		if (mapId <= 0x2C) return 0x09;
		if (mapId <= 0x45) return 0x0A;
		return 0x0B;
	}

	/// <summary>
	/// Read main overworld map data.
	/// Returns 256x256 decompressed tilemap.
	/// </summary>
	public byte[,] ReadMainOverworld() {
		return ReadOverworld(
			DataStructures.Maps.OverworldMap.Bank,
			DataStructures.Maps.OverworldMap.MainOverworldAddress,
			DataStructures.Maps.OverworldMap.MainOverworldRowPointers
		);
	}

	/// <summary>
	/// Read Gottside overworld map data.
	/// </summary>
	public byte[,] ReadGottsideOverworld() {
		return ReadOverworld(
			DataStructures.Maps.OverworldMap.Bank,
			DataStructures.Maps.OverworldMap.GottsideOverworldAddress,
			DataStructures.Maps.OverworldMap.GottsideRowPointers
		);
	}

	/// <summary>
	/// Read underworld map data.
	/// </summary>
	public byte[,] ReadUnderworld() {
		return ReadOverworld(
			DataStructures.Maps.OverworldMap.Bank,
			DataStructures.Maps.OverworldMap.UnderworldAddress,
			DataStructures.Maps.OverworldMap.UnderworldRowPointers
		);
	}

	/// <summary>
	/// Read and decompress an overworld map.
	/// </summary>
	private byte[,] ReadOverworld(int bank, int mapDataAddress, int rowPointersAddress) {
		var map = new byte[256, 256];

		// Read row pointers (256 rows, 4 bytes each)
		for (int row = 0; row < 256; row++) {
			int pointerAddress = rowPointersAddress + (row * 4);
			var pointerData = ReadBytes(pointerAddress, bank, 4);

			ushort dataPtr = (ushort)(pointerData[0] | (pointerData[1] << 8));
			// byte sizeToX128 = pointerData[2]; // Not currently used
			// byte sizeToX256 = pointerData[3]; // Not currently used

			// Decompress row
			var rowTiles = DecompressOverworldRow(bank, dataPtr, 256);
			for (int col = 0; col < 256; col++) {
				map[row, col] = rowTiles[col];
			}
		}

		return map;
	}

	/// <summary>
	/// Decompress a single overworld row.
	/// Format: bits 0-4 = length+1, bits 5-7 = tile
	/// Special: if byte >= $E8, subtract $E0 for tile number
	/// </summary>
	private byte[] DecompressOverworldRow(int bank, int address, int targetWidth) {
		var result = new List<byte>();
		int pos = 0;

		while (result.Count < targetWidth) {
			byte b = ReadByte(address + pos, bank);
			pos++;

			byte tile;
			int length;

			if (b >= 0xE8) {
				tile = (byte)(b - 0xE0);
				length = 1;
			} else {
				length = (b & 0x1F) + 1;
				tile = (byte)((b >> 5) & 0x07);
			}

			for (int i = 0; i < length && result.Count < targetWidth; i++) {
				result.Add(tile);
			}
		}

		return result.ToArray();
	}

	/// <summary>
	/// Read all tilesets from the ROM.
	/// 51 tilesets at Bank $08, $8ADB-$979A.
	/// </summary>
	public DataStructures.Maps.Tileset[] ReadAllTilesets() {
		const int tilesetBank = 0x08;
		const int tilesetStartAddress = 0x8ADB;
		const int tilesetCount = 51;
		const int tilesetSize = 64;

		var tilesets = new DataStructures.Maps.Tileset[tilesetCount];

		for (int i = 0; i < tilesetCount; i++) {
			int address = tilesetStartAddress + (i * tilesetSize);
			var data = ReadBytes(address, tilesetBank, tilesetSize);
			tilesets[i] = DataStructures.Maps.Tileset.Parse(data, 0, i);
		}

		return tilesets;
	}
}
