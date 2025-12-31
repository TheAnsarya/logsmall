namespace DW4Lib.DQ3r;

/// <summary>
/// DQ3 Remake monster format (SNES 16-bit).
/// Target format for DW4 → DQ3r conversion.
/// </summary>
public class DQ3rMonster {
	/// <summary>ID in the DQ3r monster table.</summary>
	public int Id { get; set; }

	/// <summary>Monster name.</summary>
	public string Name { get; set; } = "";

	/// <summary>Maximum HP (16-bit).</summary>
	public int HP { get; set; }

	/// <summary>Maximum MP (16-bit).</summary>
	public int MP { get; set; }

	/// <summary>Attack power (16-bit).</summary>
	public int Attack { get; set; }

	/// <summary>Defense power (16-bit).</summary>
	public int Defense { get; set; }

	/// <summary>Agility (16-bit).</summary>
	public int Agility { get; set; }

	/// <summary>Experience reward (16-bit).</summary>
	public int Experience { get; set; }

	/// <summary>Gold reward (16-bit).</summary>
	public int Gold { get; set; }

	/// <summary>Item drop ID.</summary>
	public int ItemDrop { get; set; }

	/// <summary>Drop rate (0-255, higher = more common).</summary>
	public int DropRate { get; set; }

	/// <summary>AI behavior pattern.</summary>
	public int AIPattern { get; set; }

	/// <summary>Spell IDs this monster can use.</summary>
	public List<int> Spells { get; set; } = new();

	/// <summary>Resistances (element-indexed dictionary).</summary>
	public Dictionary<string, int> Resistances { get; set; } = new();

	/// <summary>Sprite/graphic ID.</summary>
	public int SpriteId { get; set; }

	/// <summary>Palette ID.</summary>
	public int PaletteId { get; set; }

	/// <summary>Source DW4 monster ID for tracking.</summary>
	public int SourceDW4Id { get; set; }

	/// <summary>Conversion notes.</summary>
	public string Notes { get; set; } = "";
}
