# Dragon Quest IV Remix (SNES) - Project Overview

## Project Summary

**Project Name:** Dragon Quest IV Remix (DQ4r)
**Target Platform:** Super Nintendo Entertainment System (SNES)
**Source Material:** Dragon Warrior IV (NES, 1992 US release)
**Base Engine:** Dragon Quest III Remix (DQ3r) SNES engine
**Goal:** Complete port/reimplementation of DW4 NES onto the SNES platform using the DQ3r engine

## Project Vision

Create a faithful SNES remake of Dragon Warrior IV that:
- Preserves the original gameplay, story, and mechanics
- Leverages the superior SNES hardware (better graphics, audio, larger ROM)
- Uses the proven DQ3r engine as the foundation
- Implements modern quality-of-life improvements where appropriate
- Maintains the classic Dragon Quest aesthetic

## Key Differences: NES → SNES

| Aspect | DW4 NES | DQ4r SNES |
|--------|---------|-----------|
| Resolution | 256×240 | 256×224 (can extend) |
| Colors | 25 on-screen | 256 on-screen |
| Sprites | 8×8 or 8×16, 64 max | 8×8 to 64×64, 128 max |
| Tile Layers | 1 background | 4 layers (2 BG + 2 mode 7) |
| Sound | 5 channels | 8 ADPCM channels |
| ROM Size | 512KB | 4-6MB typical |
| Save | Battery SRAM | Battery SRAM (larger) |
| CPU | 6502 @ 1.79MHz | 65816 @ 3.58MHz |

## Chapter Structure (DW4 Unique Feature)

DW4's defining feature is its chapter-based storytelling:

1. **Chapter 1: Ragnar McRyan** - The Royal Soldier
2. **Chapter 2: Alena, Kiryl, Borya** - The Tomboyish Princess
3. **Chapter 3: Torneko Taloon** - The Ambitious Merchant
4. **Chapter 4: Meena & Maya** - The Sisters of Fate
5. **Chapter 5: The Hero** - The Chosen One (all characters unite)

Each chapter has unique:
- Playable characters
- Starting locations
- Story events
- Available areas
- Party compositions

## Major System Components

### 1. Graphics System
- Character sprites (overworld, battle)
- Monster sprites
- Tile sets (towns, dungeons, overworld)
- Battle backgrounds
- UI elements
- Special effects
- Animations

### 2. Audio System
- Background music (all chapters)
- Sound effects
- Battle fanfares
- Event jingles

### 3. Text/Dialog System
- Dialog text (all chapters)
- Menu text
- Item/Spell/Monster names
- NPC conversations
- Story events
- Variable-width font support

### 4. Battle System
- Turn-based combat
- AI-controlled party members (Chapter 5)
- Tactics system
- Magic/abilities
- Item usage
- Escape mechanics
- Boss battles

### 5. Map/World System
- Overworld maps
- Town maps
- Dungeon maps
- Indoor areas
- Tile collision
- Warps/transitions
- Day/night cycle

### 6. Character System
- Stats (HP, MP, Str, Agi, etc.)
- Experience/leveling
- Equipment
- Spells learned
- Character-specific abilities

### 7. Event/Scripting System
- Story triggers
- NPC behavior
- Treasure chests
- Doors/locks
- Vehicles (ship, balloon)
- Chapter transitions

### 8. Menu System
- Main menu
- Battle menu
- Shop menus
- Equipment screen
- Status screen
- Tactics menu

### 9. Save System
- Adventure Log (save slots)
- Chapter progress
- Party data
- Inventory
- World state

## Development Phases

### Phase 1: Foundation (Months 1-3)
- Complete DW4 NES documentation
- Extract all assets
- Set up build pipeline
- Create conversion tools

### Phase 2: Core Systems (Months 4-8)
- Port character system
- Port battle system
- Port map system
- Port menu system

### Phase 3: Content (Months 9-14)
- Convert all graphics
- Convert all audio
- Convert all text
- Implement all maps

### Phase 4: Chapters (Months 15-20)
- Implement Chapter 1
- Implement Chapter 2
- Implement Chapter 3
- Implement Chapter 4
- Implement Chapter 5

### Phase 5: Polish (Months 21-24)
- Bug fixing
- Balance testing
- QA passes
- Final polish

## Repository Structure

```
logsmall/
├── DW4Lib/              # DW4 NES extraction/analysis library
│   ├── Converters/      # Asset conversion tools
│   ├── ROM/             # ROM reading utilities
│   ├── Text/            # Text encoding/decoding
│   └── Maps/            # Map data structures
├── DQ4rLib/             # DQ4r SNES generation library (NEW)
│   ├── Graphics/        # SNES graphics generation
│   ├── Audio/           # SNES audio conversion
│   ├── Maps/            # SNES map generation
│   └── Scripts/         # Event script compilation
└── DW4Lib.Tests/        # Unit tests

dragon-warrior-4-info/
├── docs/                # DW4 NES documentation
├── disasm/              # Disassembly files
├── data/                # Extracted data (JSON, etc.)
├── assets/              # Extracted assets
└── tools/               # Python analysis tools

dq4r-info/ (NEW)         # DQ4r SNES project
├── src/                 # Assembly source
├── assets/              # SNES-format assets
├── docs/                # DQ4r documentation
└── build/               # Build output
```

## Related Projects

- **DQ3r** - Base engine (SNES Dragon Quest III Remix)
- **DW4-info** - DW4 NES analysis and documentation
- **FFMQ** - Reference for SNES ROM hacking techniques
- **GameInfo** - Shared tools and utilities

## Success Criteria

1. **Playable**: Complete game from start to finish
2. **Faithful**: Story and gameplay match original
3. **Enhanced**: Takes advantage of SNES capabilities
4. **Stable**: No crashes or game-breaking bugs
5. **Complete**: All content present (no cut features)

## Team/Resources

- Primary Developer: TheAnsarya
- Tools: C# .NET, Python, ca65/cc65, Asar
- Emulators: bsnes, Mesen-S, Snes9x
- Reference: DQ3r source, DW4 disassembly

## Document Index

- [Technical Architecture](./DQ4r-Technical-Architecture.md)
- [Asset Pipeline](./DQ4r-Asset-Pipeline.md)
- [Development Roadmap](./DQ4r-Roadmap.md)
- [Testing Plan](./DQ4r-Testing-Plan.md)
- [Chapter Implementation Guide](./DQ4r-Chapter-Guide.md)

---
*Document created: 2026-01-01*
*Last updated: 2026-01-01*
