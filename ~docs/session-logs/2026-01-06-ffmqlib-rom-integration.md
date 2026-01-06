# Session Log: 2026-01-06 - FFMQLib ROM Integration & DQ4rLib Text Decoder

## Session Overview

**Date:** 2026-01-06
**Focus:** Fix FFMQLib ROM addresses and add comprehensive text decoding for both FFMQ and DW4

## Work Completed

### 1. FFMQLib ROM Address Corrections

Fixed all FFMQLib readers to use correct PC file offsets based on analysis of the Python tools in ffmq-info:

#### FfmqMonsterReader
- **Stats Table:** `0x14275` (was `0x118000`)
  - SNES: Bank $02, ROM $C275 → PC: (0x02 × 0x8000) + (0xC275 - 0x8000) = 0x14275
- **Level Table:** `0x1417C` (new)
  - SNES: Bank $02, ROM $C17C → PC: 0x1417C
- **Monster Count:** 83 (was 60)
- **Stats Entry Size:** 14 bytes (was 16)
- **Level Entry Size:** 3 bytes (new)

#### FfmqSpellReader
- **Address:** `0x060F36` (was `0x150000`)
- **Spell Count:** 16
- **Entry Size:** 6 bytes (was 12)

#### FfmqItemReader
- **Weapons:** `0x066000` (15 weapons, 16 bytes each)
- **Armor:** `0x066100` (7 armor, 16 bytes each)
- **Helmets:** `0x066180` (7 helmets, 16 bytes each)
- **Shields:** `0x066200` (7 shields, 16 bytes each)
- **Accessories:** `0x066280` (11 accessories, 16 bytes each)
- **Consumables:** `0x066380` (20 items, 8 bytes each)

#### FfmqTextTables (Unchanged - Already Correct)
- Text tables were already using correct PC addresses:
- Monster Names: `0x064BA0` (256 × 16 bytes)
- Spell Names: `0x064210` (32 × 12 bytes)
- Weapon Names: `0x0642A0` (57 × 12 bytes)
- etc.

### 2. FFMQLib ROM Integration Tests

Created `FfmqRomIntegrationTests.cs` with 12 tests that:
- Verify ROM file exists and has correct size
- Read monsters, spells, weapons, armor, items
- Decode text from various ROM tables
- All tests use `[SkippableFact]` to skip gracefully if ROM not available

Added `Xunit.SkippableFact` package to support conditional test skipping.

### 3. DQ4rLib Text Decoder

Created `Dq4rTextDecoder.cs` with full Dragon Warrior IV character table:

#### Character Ranges
- `0x00`: Space
- `0x01-0x0A`: Digits (0-9)
- `0x0B-0x24`: Lowercase (a-z)
- `0x25-0x3E`: Uppercase (A-Z)
- `0x3F`: Em dash
- `0x65-0x79`: Punctuation
- `0x80-0x81`: UI symbols
- `0xF0-0xFF`: Control codes

#### Features
- `Decode(byte[], offset, length)` - Decode fixed-length text
- `Encode(string, fixedLength)` - Encode text to bytes
- `IsControlCode(byte)` - Check for control codes
- `Dq4rTextTables` - Placeholder ROM text table definitions
- `Dq4rTextExtensions` - ReadTable and ReadEntry extension methods

### 4. Code Quality

Simplified record types to only include fields actually read from ROM:
- `FfmqMonster` - Removed GraphicsPointer, AiScriptId, StatusImmunities
- `FfmqSpell` - Removed StatusEffectId, StatusChance, RequiredStoryFlag
- `FfmqWeapon` - Removed IconId, Slot, RequiredLevel, CategoryName
- `FfmqArmor` - Removed Slot

## Test Results

All tests passing:
- **FFMQLib.Tests:** 41 tests (including 12 new ROM integration tests)
- **DQ4rLib.Tests:** 247 tests
- **DW4Lib.Tests:** 840 tests
- **Total:** 1,128+ tests passing

## Key Learnings

1. **FFMQ Address Format:** The addresses in ffmq-info (e.g., `0x064BA0`) are already PC file offsets, not SNES addresses requiring conversion

2. **LoROM Address Conversion:** For addresses that ARE SNES format:
   ```
   PC = (bank × 0x8000) + (offset - 0x8000)
   ```

3. **ROM Size:** FFMQ ROM is 512KB (0x80000 bytes)

## Commits

```
903b91c feat: Fix FFMQLib ROM addresses and add DQ4rLib text decoder
```

## Files Changed

### New Files
- `DQ4rLib/Dq4rTextDecoder.cs` - Full DW4 text decoding
- `FFMQLib.Tests/FfmqRomIntegrationTests.cs` - ROM integration tests

### Modified Files
- `FFMQLib/FfmqMonster.cs` - Fixed addresses, updated structure
- `FFMQLib/FfmqSpell.cs` - Fixed addresses, updated structure
- `FFMQLib/FfmqItem.cs` - Fixed addresses, updated structure
- `FFMQLib/FfmqTextDecoder.cs` - Minor cleanups
- `FFMQLib.Tests/FFMQLib.Tests.csproj` - Added SkippableFact package

## What's Next

1. Research actual DW4 ROM text table addresses for Dq4rTextTables
2. Create character editor testing documentation
3. Add more FFMQLib reader integration tests (helmets, shields, accessories)
4. Consider adding DQ4rLib ROM integration tests when ROM available
