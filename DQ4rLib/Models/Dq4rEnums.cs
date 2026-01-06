namespace DQ4rLib.Models;

/// <summary>
/// DQ4r element flags for spells and resistances
/// </summary>
[Flags]
public enum Dq4rElement : byte {
	None = 0x00,
	Fire = 0x01,
	Ice = 0x02,
	Thunder = 0x04,
	Wind = 0x08,
	Earth = 0x10,
	Zap = 0x20,
	Holy = 0x40,
	Dark = 0x80
}

/// <summary>
/// DQ4r status effect flags
/// </summary>
[Flags]
public enum Dq4rStatus : ushort {
	None = 0x0000,
	Poison = 0x0001,
	Paralysis = 0x0002,
	Sleep = 0x0004,
	Confusion = 0x0008,
	Blind = 0x0010,
	Silence = 0x0020,
	Curse = 0x0040,
	Death = 0x0080,
	Berserk = 0x0100,
	Stun = 0x0200,
	Slow = 0x0400,
	DefDown = 0x0800
}

/// <summary>
/// Target types for spells/skills
/// </summary>
public enum Dq4rTargetType : byte {
	Self = 0,
	SingleAlly = 1,
	AllAllies = 2,
	SingleEnemy = 3,
	AllEnemies = 4,
	RandomEnemy = 5,
	OneGroup = 6
}

/// <summary>
/// Item categories
/// </summary>
public enum Dq4rItemCategory : byte {
	Weapon = 0,
	Shield = 1,
	Armor = 2,
	Helmet = 3,
	Accessory = 4,
	Consumable = 5,
	KeyItem = 6,
	MiscItem = 7
}

/// <summary>
/// Playable character IDs (for equip flags)
/// </summary>
[Flags]
public enum Dq4rCharacter : byte {
	None = 0x00,
	Hero = 0x01,
	Ragnar = 0x02,
	Alena = 0x04,
	Kiryl = 0x08,
	Borya = 0x10,
	Torneko = 0x20,
	Meena = 0x40,
	Maya = 0x80,
	All = 0xFF
}
