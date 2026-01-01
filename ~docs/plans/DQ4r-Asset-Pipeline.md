# DQ4r Asset Pipeline

## Overview

This document describes the complete pipeline for extracting assets from Dragon Warrior IV (NES) and converting them for use in Dragon Quest IV Remix (SNES).

## Pipeline Architecture

```
┌─────────────────────────────────────────────────────────────────────────┐
│                         ASSET PIPELINE                                   │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                          │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐              │
│  │   DW4 NES    │───▶│   Extract    │───▶│    Raw       │              │
│  │     ROM      │    │   (DW4Lib)   │    │   Assets     │              │
│  └──────────────┘    └──────────────┘    └──────┬───────┘              │
│                                                  │                       │
│                                                  ▼                       │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐              │
│  │   Editable   │◀───│   Convert    │◀───│   Analyze    │              │
│  │   Format     │    │   to JSON    │    │   & Decode   │              │
│  └──────┬───────┘    └──────────────┘    └──────────────┘              │
│         │                                                                │
│         │  (Human editing / Translation / Enhancement)                   │
│         ▼                                                                │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐              │
│  │   Modified   │───▶│   Generate   │───▶│   SNES       │              │
│  │   JSON/PNG   │    │   (DQ4rLib)  │    │   Assets     │              │
│  └──────────────┘    └──────────────┘    └──────┬───────┘              │
│                                                  │                       │
│                                                  ▼                       │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐              │
│  │   DQ4r ROM   │◀───│   Assemble   │◀───│   ASM        │              │
│  │   (SNES)     │    │   (Asar)     │    │   Includes   │              │
│  └──────────────┘    └──────────────┘    └──────────────┘              │
│                                                                          │
└─────────────────────────────────────────────────────────────────────────┘
```

## Asset Categories

### 1. Graphics Assets

#### Character Sprites
| Source (NES) | Target (SNES) | Conversion |
|--------------|---------------|------------|
| 2bpp, 8×16 tiles | 4bpp, 16×24+ tiles | Upscale + repalette |
| ~16 colors total | 16 colors per palette | Enhanced palette |
| 4KB per character | 8-16KB per character | Higher detail |

**Files:**
- `characters/*.chr` → `characters/*.4bpp`
- `characters/palettes.json` → `characters/palettes.pal`

#### Monster Sprites
| Source (NES) | Target (SNES) | Conversion |
|--------------|---------------|------------|
| 2bpp, various sizes | 4bpp, up to 64×64 | Direct convert + enhance |
| Limited animation | Full animation frames | Add frames |
| ~100 monsters | ~100 monsters | 1:1 mapping |

**Files:**
- `monsters/*.chr` → `monsters/*.4bpp`
- `monsters/metadata.json` → `monsters/sprites.asm`

#### Tilesets
| Source (NES) | Target (SNES) | Conversion |
|--------------|---------------|------------|
| 2bpp, 8×8 tiles | 4bpp, 8×8 or 16×16 | Expand palette |
| 256 tiles per set | 512+ tiles per set | Add detail tiles |
| Single layer | Multi-layer | Add parallax |

**Tileset Categories:**
- Overworld (grass, water, mountains, etc.)
- Towns (buildings, streets, interiors)
- Dungeons (caves, castles, towers)
- Battle backgrounds

#### UI Graphics
- Menu frames/borders
- Font tiles
- Icons (items, status, etc.)
- Cursor sprites
- Dialog boxes

### 2. Audio Assets

#### Music Tracks
| Source (NES) | Target (SNES) | Conversion |
|--------------|---------------|------------|
| 2A03 (5 channels) | SPC700 (8 channels) | Rearrange + enhance |
| Square/Triangle/Noise | ADPCM samples | New instrument samples |
| ~30 tracks | ~30+ tracks | Add variations |

**Music List:**
```
- Title Theme
- Overworld (Day/Night)
- Town
- Castle
- Cave/Dungeon
- Battle (Normal)
- Battle (Boss)
- Victory Fanfare
- Chapter themes (per chapter)
- Sad/Dramatic
- Inn/Church
- Shop
- Ending
```

#### Sound Effects
| Source (NES) | Target (SNES) | Conversion |
|--------------|---------------|------------|
| 2A03 synthesis | BRR samples | Re-record or synthesize |
| ~50 effects | ~50+ effects | Add new effects |

**SFX Categories:**
- Menu sounds (cursor, confirm, cancel)
- Battle sounds (attack, spell, damage)
- World sounds (door, chest, stairs)
- UI sounds (level up, save, heal)

### 3. Text Assets

#### Dialog Text
| Source (NES) | Target (SNES) | Conversion |
|--------------|---------------|------------|
| DTE compressed | DTE or Huffman | Re-encode |
| Single-byte chars | Two-byte chars | Expand encoding |
| ~100KB text | ~200KB text | VWF support |

**Text Tables:**
- Chapter 1 Dialog
- Chapter 2 Dialog
- Chapter 3 Dialog
- Chapter 4 Dialog
- Chapter 5 Dialog
- Menu/System Text
- Battle Text
- Item Names
- Spell Names
- Monster Names
- Location Names
- Character Names

#### Text Encoding
```
DW4 NES Encoding:
  $00     = Space
  $01-$0A = 0-9
  $0B-$24 = a-z
  $25-$3E = A-Z
  $3F-$4A = Punctuation
  $80-$FE = DTE pairs
  $FD     = Newline
  $FF     = End

DQ3r SNES Encoding:
  $0200+  = Character tiles
  $00AB+  = Control codes
```

### 4. Map Assets

#### Map Data
| Source (NES) | Target (SNES) | Conversion |
|--------------|---------------|------------|
| Single layer | 2-4 layers | Add depth |
| 8×8 metatiles | 16×16 metatiles | Restructure |
| ~200 maps | ~200 maps | 1:1 mapping |

**Map Components:**
- Tile data (background layout)
- Collision data
- Event triggers
- NPC placement
- Warp points
- Treasure chests

#### Map Categories
- Overworld (large scrolling map)
- Towns (multiple areas each)
- Dungeons (multi-floor)
- Indoor locations
- Special areas (casino, arenas)

### 5. Data Assets

#### Game Data Tables
- Experience tables (per character class)
- Monster stats
- Item stats
- Spell effects
- Equipment bonuses
- Shop inventories
- Encounter tables
- Drop tables

#### Script Data
- Event scripts
- NPC behaviors
- Cutscene sequences
- Chapter transitions

## Conversion Tools

### DW4Lib (C# - Extraction)

```csharp
// Key classes for extraction
DW4Lib.Converters.GraphicsConverter   // NES CHR → PNG
DW4Lib.Converters.DialogExtractor     // Text extraction
DW4Lib.Converters.MapConverter        // Map data extraction
DW4Lib.Converters.MonsterConverter    // Monster data
DW4Lib.Converters.AudioExtractor      // Music/SFX
DW4Lib.Converters.TextConverter       // Names, menus
```

### DQ4rLib (C# - Generation)

```csharp
// Key classes for generation (TO BE CREATED)
DQ4rLib.Graphics.TileConverter        // PNG → 4bpp
DQ4rLib.Graphics.SpriteGenerator      // Sprite sheets
DQ4rLib.Graphics.PaletteGenerator     // SNES palettes
DQ4rLib.Audio.MusicConverter          // → SPC format
DQ4rLib.Audio.SfxConverter            // → BRR samples
DQ4rLib.Text.ScriptCompiler           // → SNES text format
DQ4rLib.Maps.MapCompiler              // → SNES map format
DQ4rLib.Data.TableCompiler            // → Binary tables
```

## File Format Specifications

### Editable Formats (Intermediate)

#### Graphics (PNG)
```
characters/
  ragnar_walk.png      # 64×64, 4 frames
  ragnar_battle.png    # 48×64, battle pose
  ragnar_palette.json  # 16 colors
```

#### Text (JSON)
```json
{
  "table": "Chapter1Dialog",
  "entries": [
    {
      "id": 0,
      "original": "Welcome to Burland!",
      "translated": "Welcome to Burland!",
      "notes": "First NPC in castle"
    }
  ]
}
```

#### Maps (JSON + PNG)
```json
{
  "map_id": 1,
  "name": "Burland Castle 1F",
  "width": 32,
  "height": 32,
  "tileset": "castle",
  "music": "castle_theme",
  "layers": ["background.png", "foreground.png"],
  "events": [...],
  "npcs": [...]
}
```

#### Audio (MIDI + JSON)
```json
{
  "track_id": 5,
  "name": "Battle Theme",
  "midi_file": "battle.mid",
  "instruments": {
    "0": "strings.brr",
    "1": "brass.brr"
  },
  "loop_point": 1234
}
```

### Output Formats (SNES)

#### Graphics (.4bpp / .pal)
- Raw 4bpp tile data
- SNES palette format (15-bit BGR)

#### Text (.bin / .ptr)
- Compressed text data
- Pointer tables

#### Maps (.map / .col)
- Tilemap data
- Collision data

#### Audio (.spc / .brr)
- SPC700 driver + data
- BRR sample data

## Build Commands

### Full Pipeline
```powershell
# Extract from NES ROM
dotnet run --project DW4Lib -- extract --rom dw4.nes --output assets/raw

# Convert to editable format
dotnet run --project DW4Lib -- convert --input assets/raw --output assets/editable

# (Human editing happens here)

# Generate SNES assets
dotnet run --project DQ4rLib -- generate --input assets/editable --output assets/snes

# Build ROM
asar src/main.asm build/dq4r.sfc
```

### Individual Asset Types
```powershell
# Graphics only
dotnet run --project DQ4rLib -- graphics --input assets/editable/graphics

# Text only  
dotnet run --project DQ4rLib -- text --input assets/editable/text

# Maps only
dotnet run --project DQ4rLib -- maps --input assets/editable/maps

# Audio only
dotnet run --project DQ4rLib -- audio --input assets/editable/audio
```

## Quality Assurance

### Extraction Verification
- Compare extracted text to known dumps
- Visual comparison of graphics
- Audio playback verification

### Conversion Verification
- Round-trip testing (convert → unconvert → compare)
- Visual inspection of SNES graphics
- Audio quality comparison

### Integration Verification
- ROM boots successfully
- Assets display correctly
- No corruption or glitches

## Asset Status Tracking

### Tracking Spreadsheet Format
| Asset | Category | Extracted | Converted | Verified | Notes |
|-------|----------|-----------|-----------|----------|-------|
| ragnar_sprite | Character | ✅ | ⬜ | ⬜ | Needs upscale |
| battle_music | Audio | ✅ | ⬜ | ⬜ | Rearranging |
| ch1_dialog | Text | ✅ | ✅ | ⬜ | In review |

### Status Codes
- ⬜ Not started
- 🔄 In progress
- ✅ Complete
- ❌ Blocked
- ⚠️ Needs review

---
*Document created: 2026-01-01*
*Last updated: 2026-01-01*
