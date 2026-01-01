# DQ4r Technical Architecture

## Overview

This document describes the technical architecture for porting Dragon Warrior IV (NES) to Dragon Quest IV Remix (SNES) using the DQ3r engine.

## System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                        DQ4r SNES ROM                            │
├─────────────────────────────────────────────────────────────────┤
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐        │
│  │ Graphics │  │  Audio   │  │   Text   │  │   Maps   │        │
│  │  Engine  │  │  Engine  │  │  Engine  │  │  Engine  │        │
│  └────┬─────┘  └────┬─────┘  └────┬─────┘  └────┬─────┘        │
│       │             │             │             │               │
│  ┌────┴─────────────┴─────────────┴─────────────┴────┐         │
│  │                   Core Engine                      │         │
│  │  ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐  │         │
│  │  │ Battle  │ │  Menu   │ │  Event  │ │Character│  │         │
│  │  │ System  │ │ System  │ │ Scripts │ │ System  │  │         │
│  │  └─────────┘ └─────────┘ └─────────┘ └─────────┘  │         │
│  └───────────────────────────────────────────────────┘         │
│                              │                                  │
│  ┌───────────────────────────┴───────────────────────┐         │
│  │                  SNES Hardware Layer               │         │
│  │  CPU (65816) │ PPU │ APU (SPC700) │ DMA │ SRAM    │         │
│  └───────────────────────────────────────────────────┘         │
└─────────────────────────────────────────────────────────────────┘
```

## Memory Map (SNES LoROM)

### ROM Banks (Target: 4MB = 64 banks)
```
Bank $00-$3F: Program code and data (mirrors in $80-$BF)
  $00: Core engine, initialization
  $01-$03: Battle system
  $04-$07: Menu system
  $08-$0B: Map engine
  $0C-$0F: Event scripts
  $10-$1F: Chapter 1-4 data
  $20-$2F: Chapter 5 data
  $30-$3F: Shared data

Bank $40-$7F: Asset data (no mirror needed)
  $40-$4F: Character/monster graphics
  $50-$5F: Tileset graphics
  $60-$6F: Background graphics
  $70-$7F: Audio data (SPC samples)
```

### WRAM Layout ($7E0000-$7FFFFF)
```
$7E0000-$7E00FF: Direct page (fast access)
$7E0100-$7E01FF: Stack
$7E0200-$7E0FFF: Core variables
$7E1000-$7E1FFF: Party data
$7E2000-$7E2FFF: Battle state
$7E3000-$7E3FFF: Map state
$7E4000-$7E7FFF: Scratch buffers
$7E8000-$7EFFFF: Decompression buffers
$7F0000-$7FFFFF: Extended RAM (maps, graphics cache)
```

### SRAM Layout ($70:0000-$70:7FFF = 32KB)
```
$70:0000-$70:0FFF: Save slot 1 (Chapter progress + party)
$70:1000-$70:1FFF: Save slot 2
$70:2000-$70:2FFF: Save slot 3
$70:3000-$70:3FFF: Quick save / suspend
$70:4000-$70:7FFF: Extended save data
```

## DQ3r Engine Adaptation

### Components to Reuse (from DQ3r)
1. **Graphics Engine**
   - Tile decompression (LZ77/RLE)
   - Sprite management
   - Palette handling
   - Mode 7 (if applicable)

2. **Audio Engine**
   - SPC700 driver
   - Music playback
   - Sound effect system

3. **Text Engine**
   - Variable-width font renderer
   - Dialog box system
   - Text speed control

4. **Menu Framework**
   - Window drawing
   - Cursor handling
   - Item lists

### Components to Modify
1. **Battle System** - DW4 has different mechanics
   - AI-controlled allies (Chapter 5 Tactics)
   - Different spell/ability set
   - Different damage formulas

2. **Chapter System** - DW4-specific
   - Chapter state management
   - Character roster per chapter
   - Chapter transitions

3. **Wagon System** - DW4-specific
   - Party swapping during battle
   - 8 characters, 4 active
   - Wagon mechanics

### Components to Create New
1. **Tactics System**
   - AI behavior patterns
   - Player-configurable strategies
   - Per-character tactics

2. **Day/Night Cycle**
   - Time progression
   - NPC schedule changes
   - Shop availability

3. **Casino System**
   - Slot machines
   - Poker
   - Monster arena

## Data Structures

### Character Data (per character)
```c
struct Character {
    uint8_t  id;              // Character identifier
    char     name[8];         // 8-char name
    uint8_t  class;           // Character class/type
    uint16_t current_hp;
    uint16_t max_hp;
    uint16_t current_mp;
    uint16_t max_mp;
    uint16_t strength;
    uint16_t agility;
    uint16_t vitality;
    uint16_t intelligence;
    uint16_t luck;
    uint16_t attack;
    uint16_t defense;
    uint32_t experience;
    uint8_t  level;
    uint8_t  equipment[6];    // Weapon, armor, shield, helmet, accessory1, accessory2
    uint8_t  spells[32];      // Known spells bitmap
    uint8_t  status;          // Status effects
    uint8_t  tactics;         // AI behavior setting
};
```

### Monster Data
```c
struct Monster {
    uint8_t  id;
    uint16_t hp;
    uint16_t mp;
    uint8_t  attack;
    uint8_t  defense;
    uint8_t  agility;
    uint8_t  exp_reward_low;
    uint8_t  exp_reward_high;
    uint16_t gold_reward;
    uint8_t  drop_item;
    uint8_t  drop_rate;
    uint8_t  actions[4];      // AI action table
    uint8_t  resistances;     // Elemental/status resistances
    uint8_t  sprite_id;
    uint8_t  palette_id;
};
```

### Map Header
```c
struct MapHeader {
    uint8_t  map_id;
    uint8_t  tileset_id;
    uint8_t  palette_id;
    uint8_t  music_id;
    uint8_t  width;
    uint8_t  height;
    uint16_t tile_data_ptr;
    uint16_t event_data_ptr;
    uint16_t npc_data_ptr;
    uint8_t  encounter_rate;
    uint8_t  encounter_table_id;
};
```

### Event Script Format
```
Events use a bytecode system:
  $00     - End script
  $01 XX  - Show message XX
  $02 XX  - Set flag XX
  $03 XX  - Check flag XX, skip if false
  $04 XX YY - Move NPC XX to position YY
  $05 XX  - Play sound XX
  $06 XX  - Change map to XX
  $07     - Heal party
  $08 XX  - Give item XX
  $09 XX  - Take item XX
  $0A XX  - Give gold XX
  $0B XX  - Start battle XX
  ...etc
```

## Build Pipeline

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│   DW4 NES ROM   │────▶│  DW4Lib C#      │────▶│  Extracted      │
│   (Original)    │     │  (Extraction)   │     │  Assets/Data    │
└─────────────────┘     └─────────────────┘     └────────┬────────┘
                                                         │
                                                         ▼
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│   DQ4r ROM      │◀────│  Asar/ca65      │◀────│  DQ4rLib C#     │
│   (Output)      │     │  (Assembly)     │     │  (Conversion)   │
└─────────────────┘     └─────────────────┘     └─────────────────┘
```

### Build Steps
1. **Extract** - DW4Lib extracts assets from NES ROM
2. **Convert** - DQ4rLib converts to SNES format
3. **Generate** - Create ASM include files
4. **Assemble** - Asar compiles SNES ROM
5. **Verify** - Run automated tests
6. **Package** - Create distributable ROM

## Compression Schemes

### Graphics Compression (SNES)
- **LZ77** - General purpose, good ratio
- **RLE** - Run-length for simple graphics
- **4bpp Pack** - Tile optimization

### Text Compression
- **DTE** - Dual Tile Encoding (reuse from DW4)
- **Huffman** - Optional for large scripts
- **Pointer tables** - Indexed access

### Audio Compression
- **BRR** - SNES native sample format
- **Sequenced music** - MIDI-style events

## Toolchain

### Required Tools
- **Asar** - SNES assembler
- **ca65/ld65** - Alternative assembler
- **.NET 10** - C# tools runtime
- **Python 3.12+** - Analysis scripts

### Custom Tools (C#)
- `DW4Lib` - NES ROM extraction
- `DQ4rLib` - SNES ROM generation
- `TileConverter` - Graphics conversion
- `MusicConverter` - Audio conversion
- `ScriptCompiler` - Event script compiler
- `MapConverter` - Map data conversion

### Reference Tools
- **bsnes** - Accurate emulator
- **Mesen-S** - Debugging emulator
- **YY-CHR** - Tile editor
- **EbMusEd** - SNES music editor

## Testing Strategy

### Unit Tests
- Asset conversion accuracy
- Data structure serialization
- Compression/decompression

### Integration Tests
- ROM builds successfully
- Game boots in emulator
- Save/load functions

### Playthrough Tests
- Each chapter completable
- All events trigger correctly
- No softlocks

### Regression Tests
- Compare against original DW4
- Verify all text matches
- Verify battle calculations

---
*Document created: 2026-01-01*
*Last updated: 2026-01-01*
