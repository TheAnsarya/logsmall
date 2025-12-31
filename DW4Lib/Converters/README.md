# DW4 to DQ3r Map Conversion Pipeline

This document describes the conversion pipeline for transforming Dragon Warrior IV (NES) map data into Dragon Quest III Remix (SNES) format.

## Overview

The DW4→DQ3r map conversion involves several key transformations:

1. **Tile Translation** - Converting NES 1-byte tiles to SNES 2-byte tiles
2. **Format Conversion** - Adapting RLE-compressed NES data to LZSS/Ring400 SNES format
3. **Event Migration** - Converting NPCs, chests, warps, and other events
4. **Entrance Mapping** - Translating overworld entrance coordinates

## File Structure

```
DW4Lib/
├── DataStructures/
│   └── Maps/
│       ├── MapInfo.cs          # Map metadata and pointers
│       ├── TileData.cs         # Tile/tileset structures
│       ├── OverworldMap.cs     # Overworld map data
│       ├── MapEvents.cs        # NPCs, chests, warps
│       └── EncounterData.cs    # Monster encounters
├── DQ3r/
│   └── Maps/
│       ├── DQ3rMapData.cs      # DQ3r map structures
│       └── DQ3rMapEvents.cs    # DQ3r event structures
└── Converters/
    ├── MapToDQ3r.cs            # Main conversion logic
    ├── WorldMapToDQ3r.cs       # World map conversion
    └── EntranceToDQ3r.cs       # Entrance locations
```

## DW4 NES Map Format

### ROM Layout

| Bank | Offset | Content |
|------|--------|---------|
| $08 | $20000 | Tileset data (51 tilesets × 64 bytes) |
| $09 | $24000 | Map data (Maps $00-$2C) |
| $0A | $28000 | Map data (Maps $2D-$45) |
| $0B | $2C000 | Map data (Maps $45-$48) + Overworld |
| $17 | $5C000 | Map pointers and info |

### Map Info Format (3 bytes per submap)

```
Byte 0: Tileset number
Bytes 1-2: Map data pointer (little-endian)
```

### Overworld Compression

DW4 uses simple RLE for overworld maps:
- Bits 0-4: Length + 1 (1-32 tiles)
- Bits 5-7: Tile type (0-7)
- Special: If byte ≥ $E8, subtract $E0 for tile number

## DQ3r SNES Map Format

### ROM Layout (HiROM 6MB)

| Address | Content |
|---------|---------|
| $e54f38 | Metatile definitions (237 entries) |
| $ed8a00 | World map layout (Ring400 compressed) |
| $eda49c-$ee3e2f | Tilemap chunk data (16 streams) |
| $180000 | World map tile graphics |

### World Map Structure

1. **Layout Grid**: 64×64 entries (4096 total)
2. **Each Entry**: 2-byte chunk index
3. **Chunks**: 4×4 tile blocks
4. **Full Map**: 256×256 tiles

### Compression (Ring400/LZSS)

```
Command byte (8 bits, processed right to left):
- Bit 1: Read literal byte
- Bit 0: Read 2-byte reference (10-bit offset, 6-bit length+3)
```

## Conversion Process

### Step 1: Tile Translation

```csharp
// OverworldTileTranslation table maps DW4 tiles to DQ3r tiles
byte dq3rTile = MapToDQ3r.ConvertOverworldTile(dw4Tile);
```

### Step 2: Chunk Generation

```csharp
// Extract unique 4×4 chunks from translated tilemap
var chunks = WorldMapToDQ3r.GenerateChunks(translatedTilemap);
```

### Step 3: Layout Creation

```csharp
// Generate layout indices referencing chunks
var layout = WorldMapToDQ3r.GenerateLayout(tilemap, chunks);
```

### Step 4: Compression

```csharp
// Compress layout data for ROM insertion
var compressed = WorldMapToDQ3r.CompressLayout(layout);
```

## Event Conversion

### NPCs

| DW4 Field | DQ3r Field | Notes |
|-----------|------------|-------|
| X, Y | X, Y | Direct mapping |
| SpriteId | SpriteId | Needs sprite table |
| DialogId | DialogId | Needs dialog conversion |
| Movement | Movement | Similar patterns |
| Flags | Flags | Extended in DQ3r |

### Treasure Chests

| DW4 Type | DQ3r Type |
|----------|-----------|
| Item | Item (needs ID mapping) |
| Gold | Gold (direct) |
| SmallMedal | SmallMedal |
| Empty | Empty |
| Monster | Monster (needs ID mapping) |

### Warps

Warp conversion requires:
1. Map ID translation
2. Coordinate adjustment
3. Direction handling

## Entrance Locations

Default entrance database included in `EntranceToDQ3r.cs`:

- **Chapter 1**: Burland, Izmit
- **Chapter 2**: Santeem, Surene, Tempe, Frenor
- **Chapter 3**: Lakanaba, Endor, Bonmalmo
- **Chapter 4**: Monbaraba, Kievs, Haville, Aneaux, Hometown
- **Chapter 5**: All locations + Gottside/Underworld

## Usage Example

```csharp
using DW4Lib.Converters;
using DW4Lib.ROM;

// Load DW4 ROM
var rom = new DW4Rom("path/to/dw4.nes");

// Read overworld map
var dw4Map = rom.ReadOverworldMap();

// Convert to DQ3r format
var result = WorldMapToDQ3r.ConvertWorldMap(dw4Map);

// Check validity
if (result.IsValid) {
    Console.WriteLine($"Generated {result.UniqueChunkCount} chunks");
    Console.WriteLine($"Compression ratio: {result.CompressionRatio:P}");
}

// Export for ROM insertion
File.WriteAllBytes("layout.bin", result.CompressedLayout);
```

## Limitations

1. **Tile Mapping**: Not all DW4 tiles have DQ3r equivalents
2. **Chapter System**: DW4's chapter-based world changes need special handling
3. **NPC Scripts**: Dialog and scripts require separate conversion
4. **Graphics**: Tile graphics must be converted separately (4bpp SNES format)

## Future Work

- [ ] Complete tile translation table
- [ ] Dialog/script conversion
- [ ] Graphics conversion pipeline
- [ ] Chapter-aware map switching
- [ ] In-game testing framework

## References

- [DW4 ROM Map Reference](../../docs/rom-map-reference.md)
- [DW4 MAP_LIST](../../docs/reference/MAP_LIST.md)
- DQ3r research in `logsmall/DQ3/OverworldMap2.cs`
