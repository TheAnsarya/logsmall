namespace FFMQLib;

/// <summary>
/// FFMQ element flags (shared between monsters, weapons, armor, spells)
/// </summary>
[Flags]
public enum FfmqElement : byte {
	None = 0x00,
	Fire = 0x01,
	Ice = 0x02,
	Thunder = 0x04,
	Earth = 0x08,
	Water = 0x10,
	Wind = 0x20,
	Cure = 0x40,
	Fatal = 0x80
}

/// <summary>
/// FFMQ status effect flags
/// </summary>
[Flags]
public enum FfmqStatus : byte {
	None = 0x00,
	Poison = 0x01,
	Paralysis = 0x02,
	Confusion = 0x04,
	Sleep = 0x08,
	Petrify = 0x10,
	Blind = 0x20,
	Mute = 0x40,
	Dead = 0x80
}

/// <summary>
/// Spell target types
/// </summary>
public enum FfmqTargetType : byte {
	SingleAlly = 0,
	AllAllies = 1,
	Self = 2,
	SingleEnemy = 3,
	AllEnemies = 4
}

/// <summary>
/// Weapon equipment slot
/// </summary>
public enum FfmqWeaponSlot : byte {
	Sword = 0,
	Axe = 1,
	Claw = 2,
	Bomb = 3
}

/// <summary>
/// Armor equipment slot
/// </summary>
public enum FfmqArmorSlot : byte {
	Helmet = 0,
	Armor = 1,
	Shield = 2,
	Accessory = 3
}

/// <summary>
/// FFMQ ROM address utilities
/// </summary>
public static class FfmqAddresses {
	// LoROM conversion: SNES address to PC file offset
	// Bank $00-$3F, $80-$BF: (bank & 0x7F) * 0x8000 + (addr & 0x7FFF)
	// Bank $40-$6F, $C0-$EF: (bank - 0x40) * 0x8000 + addr (full 64KB banks)

	/// <summary>Convert SNES LoROM address to PC file offset</summary>
	public static int SnesLoRomToPc(int snesAddress) {
		int bank = (snesAddress >> 16) & 0xFF;
		int offset = snesAddress & 0xFFFF;

		// HiROM-style banks $C0+
		if (bank >= 0xC0) {
			return ((bank - 0xC0) * 0x10000) + offset;
		}

		// LoROM banks $80-$BF mirror $00-$3F
		if (bank >= 0x80 && bank <= 0xBF) {
			bank -= 0x80;
		}

		// Standard LoROM
		return (bank * 0x8000) + (offset & 0x7FFF);
	}

	/// <summary>Convert PC file offset to SNES LoROM address</summary>
	public static int PcToSnesLoRom(int pcOffset) {
		int bank = pcOffset / 0x8000;
		int offset = (pcOffset % 0x8000) + 0x8000;
		return (bank << 16) | offset;
	}

	// Key ROM regions (SNES addresses)
	public const int MonsterStats = 0xD18000;
	public const int MonsterNames = 0xD20000;
	public const int ItemData = 0xD28000;
	public const int WeaponStats = 0xD2A000;
	public const int ArmorStats = 0xD2C000;
	public const int ItemNames = 0xD40000;
	public const int SpellData = 0xD50000;
	public const int SpellNames = 0xD52000;
}

