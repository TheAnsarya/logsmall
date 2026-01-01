# DQ4r GitHub Issue Bodies

This file contains the body text for GitHub issues to be created for the DQ4r project.
Copy and paste these into GitHub when creating issues.

---

## Epic: Project Foundation

**Title:** `[EPIC] Project Foundation - DQ4r Setup & Infrastructure`

**Labels:** `epic`, `priority:critical`

**Body:**
```markdown
## Epic: Project Foundation

Set up the project infrastructure, documentation, and core tooling needed before development can begin.

### Goals
- [ ] Create and configure repositories
- [ ] Complete all planning documentation
- [ ] Set up build and CI/CD infrastructure
- [ ] Create conversion tool framework

### Sub-Issues
_Create these issues and link them here_

- [ ] Repository setup and configuration
- [ ] Documentation foundation
- [ ] Tool infrastructure (DQ4rLib)
- [ ] DQ3r engine analysis and documentation

### Dependencies
- None (this is the first epic)

### Blocks
- All other epics depend on this foundation

### Acceptance Criteria
- [ ] `dq4r-info` repository exists with proper structure
- [ ] All planning documents complete and reviewed
- [ ] DQ4rLib project compiles and has basic structure
- [ ] Build pipeline produces valid output
- [ ] CI/CD runs tests automatically
```

---

## Epic: DW4 NES Analysis

**Title:** `[EPIC] DW4 NES Documentation & Complete Analysis`

**Labels:** `epic`, `priority:critical`

**Body:**
```markdown
## Epic: DW4 NES Documentation & Analysis

Complete reverse engineering and documentation of Dragon Warrior IV NES ROM.

### Goals
- [ ] Complete ROM map with all data locations
- [ ] Document all file formats and compression schemes
- [ ] Extract all assets to editable formats
- [ ] Document all game logic and formulas

### Sub-Issues
_Create these issues and link them here_

- [ ] ROM map and bank documentation
- [ ] Graphics format documentation & extraction
- [ ] Audio format documentation & extraction  
- [ ] Text system documentation (DONE - partial)
- [ ] Game logic/formula documentation
- [ ] Map format documentation

### Dependencies
- Depends on: #XX (Foundation epic)

### Current Progress
- ✅ Text encoding documented
- ✅ DialogExtractor created
- ✅ BatchScriptExporter created
- 🔄 ROM map in progress
- ⬜ Graphics extraction pending
- ⬜ Audio analysis pending

### Acceptance Criteria
- [ ] Complete ROM map published to wiki
- [ ] All assets extracted and verified
- [ ] All formulas documented and tested
- [ ] Documentation reviewed for accuracy
```

---

## Epic: Graphics System

**Title:** `[EPIC] Graphics System - Asset Conversion & Engine`

**Labels:** `epic`, `area:graphics`

**Body:**
```markdown
## Epic: Graphics System

Extract, convert, and implement all graphics assets for SNES.

### Goals
- [ ] Convert all NES graphics to SNES 4bpp format
- [ ] Enhance graphics where appropriate
- [ ] Implement graphics engine in ROM
- [ ] Achieve visual parity or better with original

### Sub-Issues
_Create these issues and link them here_

- [ ] Character sprite conversion (8 playable characters)
- [ ] Monster sprite conversion (~100 monsters)
- [ ] Tileset conversion (overworld, towns, dungeons)
- [ ] Battle background creation
- [ ] UI graphics creation
- [ ] Graphics engine implementation

### Dependencies
- Depends on: #XX (Foundation), #XX (DW4 Analysis)

### Technical Notes
- NES: 2bpp, 8x8/8x16 tiles
- SNES: 4bpp, 8x8 to 64x64 sprites
- Need palette expansion from 4 to 16 colors per palette

### Acceptance Criteria
- [ ] All character sprites converted and display correctly
- [ ] All monster sprites converted
- [ ] All tilesets converted and render properly
- [ ] UI is clear and readable
- [ ] No visual glitches in normal gameplay
```

---

## Epic: Audio System

**Title:** `[EPIC] Audio System - Music & SFX Conversion`

**Labels:** `epic`, `area:audio`

**Body:**
```markdown
## Epic: Audio System

Extract, convert/rearrange, and implement all audio for SNES.

### Goals
- [ ] Convert all NES music to SPC700 format
- [ ] Convert or recreate all sound effects
- [ ] Implement audio engine in ROM
- [ ] Achieve audio quality better than original

### Sub-Issues
_Create these issues and link them here_

- [ ] Music track conversion (~30 tracks)
- [ ] Sound effect conversion (~50 SFX)
- [ ] Audio engine implementation
- [ ] Instrument sample creation

### Dependencies
- Depends on: #XX (Foundation), #XX (DW4 Analysis)

### Technical Notes
- NES: 2A03 (5 channels - 2 pulse, triangle, noise, DPCM)
- SNES: SPC700 (8 ADPCM channels, custom samples)
- Music will need rearrangement to take advantage of SNES

### Acceptance Criteria
- [ ] All music tracks play correctly
- [ ] All sound effects trigger appropriately
- [ ] Audio quality is good on SNES
- [ ] No audio glitches or cutoffs
```

---

## Epic: Text & Dialog System

**Title:** `[EPIC] Text & Dialog System - Extraction & Engine`

**Labels:** `epic`, `area:text`

**Body:**
```markdown
## Epic: Text & Dialog System

Extract, convert, and implement all text and dialog.

### Goals
- [ ] Extract all text from NES ROM
- [ ] Convert to SNES format with VWF support
- [ ] Implement dialog engine in ROM
- [ ] Support all control codes and features

### Sub-Issues
_Create these issues and link them here_

- [ ] Text extraction tools (DONE ✅)
- [ ] Batch export/import tools (DONE ✅)
- [ ] SNES text format design
- [ ] VWF implementation
- [ ] Dialog engine implementation
- [ ] Control code processing

### Current Progress
- ✅ DialogExtractor.cs created
- ✅ BatchScriptExporter.cs created  
- ✅ FontToDQ3r.cs with 40+ control codes
- ✅ Font rendering preview tool
- 🔄 Text extraction testing

### Dependencies
- Depends on: #XX (Foundation), #XX (DW4 Analysis)

### Acceptance Criteria
- [ ] All dialog displays correctly
- [ ] VWF renders properly
- [ ] All control codes work
- [ ] No text overflow or truncation
```

---

## Epic: Battle System

**Title:** `[EPIC] Battle System - Combat Implementation`

**Labels:** `epic`, `area:battle`

**Body:**
```markdown
## Epic: Battle System

Implement the complete turn-based battle system.

### Goals
- [ ] Implement all battle mechanics from DW4
- [ ] Implement AI system (monsters + allies)
- [ ] Implement Tactics system (Chapter 5)
- [ ] Implement Wagon party swapping

### Sub-Issues
_Create these issues and link them here_

- [ ] Core battle engine (turns, actions, damage)
- [ ] AI & Tactics system
- [ ] Spells & abilities implementation
- [ ] Battle UI
- [ ] Wagon system (DW4-specific)
- [ ] Boss battles

### Dependencies
- Depends on: #XX (Foundation), #XX (Graphics), #XX (Text)

### Technical Notes
- DW4 has unique ally AI in Chapter 5
- Tactics system lets player guide AI behavior
- Wagon allows swapping 8 characters, 4 active

### Acceptance Criteria
- [ ] Battles can be fought and won/lost
- [ ] Damage calculations match original
- [ ] AI behaves correctly
- [ ] Tactics system works as expected
- [ ] Wagon swapping works
```

---

## Epic: Map & World System

**Title:** `[EPIC] Map & World System - Navigation & Events`

**Labels:** `epic`, `area:maps`

**Body:**
```markdown
## Epic: Map & World System

Implement the complete world, map, and navigation systems.

### Goals
- [ ] Implement map engine with all features
- [ ] Convert all maps from NES
- [ ] Implement NPC and event systems
- [ ] Implement vehicles and special movement

### Sub-Issues
_Create these issues and link them here_

- [ ] Map engine core
- [ ] Overworld implementation
- [ ] Town & dungeon maps
- [ ] NPC system
- [ ] Event/trigger system
- [ ] Vehicle system (ship, balloon)

### Dependencies
- Depends on: #XX (Foundation), #XX (Graphics)

### Map Count Estimates
- Overworld: 1 large map
- Towns: ~30 maps
- Dungeons: ~40 maps
- Indoor areas: ~50 maps
- Total: ~120+ maps

### Acceptance Criteria
- [ ] All maps load and display correctly
- [ ] Collision works properly
- [ ] All transitions work
- [ ] NPCs move and interact
- [ ] All events trigger correctly
```

---

## Epic: Menu System

**Title:** `[EPIC] Menu System - All Game Menus`

**Labels:** `epic`, `area:engine`

**Body:**
```markdown
## Epic: Menu System

Implement all menu interfaces.

### Goals
- [ ] Create reusable menu framework
- [ ] Implement all game menus
- [ ] Ensure smooth navigation
- [ ] Match DW4 functionality

### Sub-Issues
_Create these issues and link them here_

- [ ] Menu framework
- [ ] Main/field menu
- [ ] Status screen
- [ ] Item menu
- [ ] Equipment menu
- [ ] Shop menus (buy/sell/inn/church)
- [ ] Save/load menu
- [ ] Tactics menu (Chapter 5)

### Dependencies
- Depends on: #XX (Foundation), #XX (Graphics), #XX (Text)

### Acceptance Criteria
- [ ] All menus navigable
- [ ] All functions work correctly
- [ ] No navigation dead-ends
- [ ] Consistent look and feel
```

---

## Epic: Chapter Implementation

**Title:** `[EPIC] Chapter Implementation - All 5 Chapters`

**Labels:** `epic`

**Body:**
```markdown
## Epic: Chapter Implementation

Implement each chapter of the game completely.

### Goals
- [ ] All 5 chapters fully playable
- [ ] All events and storylines work
- [ ] Chapter transitions work correctly
- [ ] Game completable start to finish

### Sub-Issues
_Create these issues and link them here_

- [ ] Chapter 1: Ragnar McRyan
- [ ] Chapter 2: Alena, Kiryl, Borya
- [ ] Chapter 3: Torneko Taloon
- [ ] Chapter 4: Meena & Maya
- [ ] Chapter 5: The Hero (largest chapter)

### Dependencies
- Depends on: ALL previous epics

### Chapter 5 Special Requirements
- Party reunion events
- Full wagon mechanics
- Tactics AI for all allies
- Final dungeon and boss

### Acceptance Criteria
- [ ] Each chapter completable standalone
- [ ] Chapter transitions work correctly
- [ ] Full game completable start to finish
- [ ] All endings play correctly
```

---

## Epic: Testing & QA

**Title:** `[EPIC] Testing & Quality Assurance`

**Labels:** `epic`, `area:testing`

**Body:**
```markdown
## Epic: Testing & Quality Assurance

Comprehensive testing of all game systems.

### Goals
- [ ] Achieve high test coverage for tools
- [ ] Verify game completability
- [ ] Ensure emulator compatibility
- [ ] Fix all critical/high bugs before release

### Sub-Issues
_Create these issues and link them here_

- [ ] Unit test coverage (tools)
- [ ] Integration testing (build, boot)
- [ ] Gameplay testing (each chapter)
- [ ] Regression testing (compare to original)
- [ ] Emulator compatibility testing
- [ ] Final QA pass

### Test Coverage Goals
- Tools: 80%+ unit test coverage
- Game: Full playthrough without critical bugs

### Acceptance Criteria
- [ ] All unit tests pass
- [ ] ROM builds and boots
- [ ] All chapters completable
- [ ] Compatible with major emulators
- [ ] No known critical/high bugs
```

---

## Individual Issue Examples

### Issue: Character Sprite Conversion

**Title:** `Convert all character sprites to SNES 4bpp format`

**Labels:** `feature`, `area:graphics`, `priority:high`

**Body:**
```markdown
## Feature: Character Sprite Conversion

### Description
Convert all 8 playable character sprites from NES 2bpp to SNES 4bpp format.

### Characters to Convert
- [ ] Ragnar McRyan (Chapter 1 + 5)
- [ ] Alena (Chapter 2 + 5)
- [ ] Kiryl (Chapter 2 + 5)
- [ ] Borya (Chapter 2 + 5)
- [ ] Torneko (Chapter 3 + 5)
- [ ] Meena (Chapter 4 + 5)
- [ ] Maya (Chapter 4 + 5)
- [ ] Hero (Chapter 5)

### Tasks Per Character
- [ ] Extract NES sprite data
- [ ] Convert to SNES 4bpp format
- [ ] Create expanded palette
- [ ] Generate walk animations (4 directions)
- [ ] Generate battle sprite
- [ ] Test in-game

### Technical Details
- Input: NES 2bpp, 8x16 tiles, 4 colors
- Output: SNES 4bpp, 16x24 or larger, 16 colors
- Tool: DQ4rLib.Graphics.SpriteConverter

### Testing
- [ ] Sprites display correctly in overworld
- [ ] Walk animations work
- [ ] Battle sprites show properly
- [ ] No palette glitches

### Parent Epic
Part of: #XX [Graphics System]
```

---

### Issue: Implement Tactics AI

**Title:** `Implement Chapter 5 Tactics AI System`

**Labels:** `feature`, `area:battle`, `priority:high`, `chapter:5`

**Body:**
```markdown
## Feature: Tactics AI System

### Description
Implement the Chapter 5 AI system that controls ally behavior in battle.

### Tactics Options
- [ ] **Show No Mercy** - All-out offense
- [ ] **Fight Wisely** - Balanced approach
- [ ] **Watch My Back** - Defensive/healing priority
- [ ] **Don't Use MP** - Physical attacks only
- [ ] **Use No Spells** - Same as above
- [ ] **Follow Orders** - Manual control

### Tasks
- [ ] Design AI decision architecture
- [ ] Implement per-character AI routines
- [ ] Implement tactics menu
- [ ] Balance AI behavior
- [ ] Test all tactics options

### Technical Notes
- AI evaluates: HP levels, MP, enemy count, enemy weakness
- Different characters may have different AI tendencies
- Need to match original DW4 behavior

### Testing
- [ ] Each tactic produces expected behavior
- [ ] AI makes reasonable decisions
- [ ] "Follow Orders" gives full manual control
- [ ] Performance is acceptable

### Parent Epic
Part of: #XX [Battle System]
```

---
*This file is for reference when creating GitHub issues.*
*Copy the body content and create issues in the appropriate repository.*
