# DW4Lib - Dragon Warrior IV Library

C# library for reading, converting, and manipulating Dragon Warrior IV (NES) game data.

## Purpose

DW4Lib provides data structures and converters for:
- Extracting DW4 ROM data into editable formats
- Converting DW4 data to DQ3 Remake (SNES) format
- Managing chapter, character, item, monster, and map data

## Project Structure

```
DW4Lib/
├── DataStructures/           # Game data models
│   ├── Chapter.cs            # Chapter system (5 chapters)
│   ├── Character.cs          # Character/party member data
│   ├── Item.cs               # Item definitions
│   ├── Monster.cs            # Monster stats
│   ├── Spell.cs              # Spell data
│   ├── Chapter1/             # Chapter 1 specific data
│   │   ├── Chapter1Data.cs   # Maps, events, treasures
│   │   └── Chapter1Dialog.cs # NPCs, dialog
│   ├── Chapter2/             # Chapter 2 data
│   ├── Chapter3/             # Chapter 3 data
│   ├── Chapter4/             # Chapter 4 data
│   └── Chapter5/             # Chapter 5 data
├── Converters/               # Data conversion utilities
│   ├── ChapterConverter.cs   # Chapter→Scenario conversion
│   ├── ItemIdConverter.cs    # Item ID mapping
│   ├── MonsterIdConverter.cs # Monster ID mapping
│   ├── ItemToDQ3r.cs         # Full item conversion
│   ├── MonsterToDQ3r.cs      # Full monster conversion
│   ├── GraphicsToDQ3r.cs     # 2bpp→4bpp graphics
│   ├── ExperienceTableConverter.cs
│   └── ...
├── ROM/                      # ROM reading utilities
└── Text/                     # Text encoding/decoding
```

## Chapter System

DW4's unique chapter structure is fully modeled:

| Chapter | Protagonist | Party Type | Special Mechanics |
|---------|-------------|------------|-------------------|
| 1 | Ragnar | Solo + NPC | Healie companion |
| 2 | Alena | AI party | Cristo, Brey AI-controlled |
| 3 | Taloon | Solo + hired | Merchant abilities |
| 4 | Nara | AI party | Mara AI-controlled |
| 5 | Hero | Full wagon | All characters, tactics |

## Converters

### ChapterConverter
Converts DW4 chapters to DQ3r "scenarios":
- Maps chapter IDs (0x00-0x04 → 0x100+)
- Converts events to quest steps
- Scales stats, prices, coordinates

### ItemIdConverter
Equipment-specific ID conversion:
- Weapons: base 0x000
- Armor: base 0x080
- Shields: base 0x0C0
- Helmets: base 0x0E0

### MonsterIdConverter
Monster and boss ID mapping:
- Regular monsters: 0x000+
- Boss monsters: 0x100+

## Usage

```csharp
using DW4Lib.DataStructures;
using DW4Lib.Converters;

// Get chapter data
var chapter1 = ChapterDatabase.GetChapter(0x00);
Console.WriteLine(chapter1.Name); // "Chapter 1: The Royal Soldiers"

// Convert Chapter 1 to DQ3r format
var dq3rData = ChapterConverter.ConvertChapter1();
Console.WriteLine(dq3rData.ProtagonistData.Name); // "Ragnar"
Console.WriteLine(dq3rData.Maps.Count);  // 5 maps
Console.WriteLine(dq3rData.QuestSteps.Count); // 10 events

// Convert item IDs
int dq3rWeapon = ItemIdConverter.ConvertWeaponId(0x03); // Copper Sword
```

## Testing

```bash
# Run all DW4Lib tests
dotnet test DW4Lib.Tests

# Run chapter tests only
dotnet test DW4Lib.Tests --filter "FullyQualifiedName~Chapter"
```

Current test count: **779 tests**

## Chapter Event & Map Coverage

| Chapter | Events | Maps | NPCs | Treasures | Tests |
|---------|--------|------|------|-----------|-------|
| Chapter 1 (Ragnar) | ✅ 17 scripts | ✅ 5 maps | ✅ 12 NPCs | ✅ 8 chests | ✅ 100+ |
| Chapter 2 (Alena) | ✅ 26 scripts | ✅ 10 maps | ✅ 10 NPCs | ✅ 10 chests | ✅ 43 |
| Chapter 3 (Torneko) | ✅ 24 scripts | ✅ 9 maps | ✅ 9 NPCs | ✅ 10 chests | ✅ 43 |
| Chapter 4 (Sisters) | ✅ 31 scripts | ✅ 12 maps | ✅ 13 NPCs | ✅ 13 chests | ✅ 43 |
| Chapter 5 (Hero) | ✅ 35 scripts | ✅ 16 maps | ✅ 15 NPCs | ✅ 15 chests | ✅ 46 |

## Related Projects

- **dragon-warrior-4-info**: DW4 disassembly and documentation
- **logsmall**: Parent solution containing DW4Lib
- **DQ3Lib**: Similar library for DQ3 Remake

## Version History

- **2026-01-01**: Added Chapter system with all 5 chapters, Chapter 1 complete data, converters
- **2025-12-31**: Added graphics, item, monster, experience converters
- **Initial**: Basic data structures

## License

MIT License - Part of the GameInfo project family.
