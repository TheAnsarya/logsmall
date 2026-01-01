# DQ4r GitHub Issues & Epics Structure

## Epic Overview

This document defines all GitHub issues organized into epics for the DQ4r project.
Issues should be created in the `logsmall` repository with appropriate labels.

## Labels to Create

### Type Labels
- `epic` - Major feature area (parent issues)
- `feature` - New functionality
- `bug` - Something broken
- `enhancement` - Improvement to existing
- `documentation` - Docs only
- `research` - Investigation needed

### Area Labels
- `area:graphics` - Graphics system
- `area:audio` - Audio system
- `area:text` - Text/dialog system
- `area:maps` - Map system
- `area:battle` - Battle system
- `area:engine` - Core engine
- `area:tools` - Development tools
- `area:testing` - Testing/QA

### Chapter Labels
- `chapter:1` - Ragnar chapter
- `chapter:2` - Alena chapter
- `chapter:3` - Torneko chapter
- `chapter:4` - Sisters chapter
- `chapter:5` - Hero chapter

### Priority Labels
- `priority:critical` - Blocking progress
- `priority:high` - Important
- `priority:medium` - Normal
- `priority:low` - Nice to have

### Status Labels
- `status:blocked` - Waiting on something
- `status:in-progress` - Being worked on
- `status:review` - Needs review

---

## Epic 1: Project Foundation
**Label:** `epic`, `priority:critical`

### Description
Set up the project infrastructure, documentation, and core tooling needed before development can begin.

### Sub-Issues

#### 1.1 Repository Setup
- [ ] Create `dq4r-info` repository for SNES project
- [ ] Set up directory structure
- [ ] Create initial README
- [ ] Set up build system (Asar/make)
- [ ] Configure CI/CD pipeline

#### 1.2 Documentation Foundation
- [ ] Create project overview document ✅
- [ ] Create technical architecture document ✅
- [ ] Create asset pipeline document ✅
- [ ] Create contributing guidelines
- [ ] Create development setup guide

#### 1.3 Tool Infrastructure
- [ ] Create DQ4rLib C# project
- [ ] Set up shared utilities with DW4Lib
- [ ] Create build automation scripts
- [ ] Set up test infrastructure

#### 1.4 DQ3r Engine Analysis
- [ ] Document DQ3r memory map
- [ ] Document DQ3r system calls
- [ ] Document DQ3r data formats
- [ ] Identify reusable components
- [ ] Document required modifications

---

## Epic 2: DW4 NES Documentation & Analysis
**Label:** `epic`, `priority:critical`

### Description
Complete documentation and reverse engineering of Dragon Warrior IV NES.

### Sub-Issues

#### 2.1 ROM Analysis
- [ ] Complete ROM map documentation
- [ ] Document all bank contents
- [ ] Identify all data tables
- [ ] Document compression schemes
- [ ] Create cross-reference documentation

#### 2.2 Graphics Documentation
- [ ] Document sprite formats
- [ ] Document tile formats
- [ ] Document palette system
- [ ] Map all graphics locations
- [ ] Document animation system

#### 2.3 Audio Documentation
- [ ] Document music engine
- [ ] Document sound effect system
- [ ] Map all music tracks
- [ ] Map all sound effects
- [ ] Document instrument data

#### 2.4 Text Documentation
- [ ] Complete character encoding table ✅
- [ ] Document DTE compression ✅
- [ ] Map all text pointers
- [ ] Document control codes ✅
- [ ] Create text extraction tools ✅

#### 2.5 Game Logic Documentation
- [ ] Document battle system formulas
- [ ] Document AI behavior
- [ ] Document experience/leveling
- [ ] Document item effects
- [ ] Document spell effects

#### 2.6 Map Documentation
- [ ] Document map format
- [ ] Document collision system
- [ ] Document event triggers
- [ ] Document NPC system
- [ ] Document warp system

---

## Epic 3: Graphics System
**Label:** `epic`, `area:graphics`

### Description
Extract, convert, and implement all graphics assets.

### Sub-Issues

#### 3.1 Character Sprites
- [ ] Extract all character sprites from NES
- [ ] Design SNES sprite specifications
- [ ] Convert/enhance Ragnar sprites
- [ ] Convert/enhance Alena sprites
- [ ] Convert/enhance Kiryl sprites
- [ ] Convert/enhance Borya sprites
- [ ] Convert/enhance Torneko sprites
- [ ] Convert/enhance Meena sprites
- [ ] Convert/enhance Maya sprites
- [ ] Convert/enhance Hero sprites
- [ ] Create walk animations
- [ ] Create battle sprites

#### 3.2 Monster Sprites
- [ ] Extract all monster sprites from NES
- [ ] Design SNES monster format
- [ ] Convert Chapter 1 monsters
- [ ] Convert Chapter 2 monsters
- [ ] Convert Chapter 3 monsters
- [ ] Convert Chapter 4 monsters
- [ ] Convert Chapter 5 monsters
- [ ] Convert boss sprites
- [ ] Create attack animations
- [ ] Implement sprite palettes

#### 3.3 Tilesets
- [ ] Extract all tilesets from NES
- [ ] Design SNES tileset format
- [ ] Convert overworld tileset
- [ ] Convert town tilesets
- [ ] Convert dungeon tilesets
- [ ] Convert indoor tilesets
- [ ] Create enhanced tile variations
- [ ] Implement tile animations

#### 3.4 Battle Backgrounds
- [ ] Design battle background system
- [ ] Create outdoor backgrounds
- [ ] Create indoor backgrounds
- [ ] Create dungeon backgrounds
- [ ] Create boss battle backgrounds
- [ ] Implement background effects

#### 3.5 UI Graphics
- [ ] Design UI style guide
- [ ] Create menu frame graphics
- [ ] Create font tiles ✅ (partial)
- [ ] Create status icons
- [ ] Create item icons
- [ ] Create spell icons
- [ ] Create cursor sprites
- [ ] Create dialog box graphics

#### 3.6 Graphics Engine
- [ ] Port/adapt DQ3r graphics engine
- [ ] Implement sprite manager
- [ ] Implement tile renderer
- [ ] Implement palette manager
- [ ] Implement animation system
- [ ] Implement special effects

---

## Epic 4: Audio System
**Label:** `epic`, `area:audio`

### Description
Extract, convert, and implement all audio assets.

### Sub-Issues

#### 4.1 Music Conversion
- [ ] Set up SPC700 driver
- [ ] Create instrument sample set
- [ ] Convert title theme
- [ ] Convert overworld theme
- [ ] Convert town theme
- [ ] Convert castle theme
- [ ] Convert dungeon theme
- [ ] Convert battle theme
- [ ] Convert boss theme
- [ ] Convert victory fanfare
- [ ] Convert chapter-specific themes
- [ ] Convert ending theme
- [ ] Implement music engine

#### 4.2 Sound Effects
- [ ] Design SFX specifications
- [ ] Create/convert menu sounds
- [ ] Create/convert battle sounds
- [ ] Create/convert spell sounds
- [ ] Create/convert world sounds
- [ ] Create/convert UI sounds
- [ ] Implement SFX playback

#### 4.3 Audio Engine
- [ ] Port/adapt DQ3r audio engine
- [ ] Implement music playback
- [ ] Implement SFX playback
- [ ] Implement volume control
- [ ] Implement stereo panning
- [ ] Implement fade effects

---

## Epic 5: Text & Dialog System
**Label:** `epic`, `area:text`

### Description
Extract, convert, and implement all text and dialog.

### Sub-Issues

#### 5.1 Text Extraction
- [ ] Complete dialog extraction tool ✅
- [ ] Extract Chapter 1 dialog
- [ ] Extract Chapter 2 dialog
- [ ] Extract Chapter 3 dialog
- [ ] Extract Chapter 4 dialog
- [ ] Extract Chapter 5 dialog
- [ ] Extract menu text
- [ ] Extract battle text
- [ ] Extract item names
- [ ] Extract spell names
- [ ] Extract monster names
- [ ] Extract location names

#### 5.2 Text Conversion
- [ ] Create batch export tool ✅
- [ ] Design SNES text format
- [ ] Convert all dialog to SNES format
- [ ] Implement VWF (variable width font)
- [ ] Implement text compression
- [ ] Create text import tool

#### 5.3 Dialog System
- [ ] Port/adapt DQ3r dialog engine
- [ ] Implement dialog boxes
- [ ] Implement text rendering ✅ (partial)
- [ ] Implement control codes ✅
- [ ] Implement name substitution
- [ ] Implement dialog choices
- [ ] Implement text speed control

---

## Epic 6: Battle System
**Label:** `epic`, `area:battle`

### Description
Implement the complete turn-based battle system.

### Sub-Issues

#### 6.1 Core Battle Engine
- [ ] Design battle architecture
- [ ] Implement turn order calculation
- [ ] Implement action selection
- [ ] Implement command execution
- [ ] Implement damage calculation
- [ ] Implement battle end conditions
- [ ] Implement escape mechanics

#### 6.2 AI & Tactics System
- [ ] Document DW4 AI patterns
- [ ] Implement monster AI
- [ ] Implement ally AI (Chapter 5)
- [ ] Implement tactics system
- [ ] Create tactics menu
- [ ] Balance AI behavior

#### 6.3 Spells & Abilities
- [ ] Document all spells
- [ ] Implement offensive spells
- [ ] Implement healing spells
- [ ] Implement buff/debuff spells
- [ ] Implement special abilities
- [ ] Create spell effects

#### 6.4 Battle UI
- [ ] Design battle menu layout
- [ ] Implement command menu
- [ ] Implement target selection
- [ ] Implement status display
- [ ] Implement battle messages
- [ ] Implement battle animations

#### 6.5 Wagon System (DW4-specific)
- [ ] Design wagon mechanics
- [ ] Implement party swapping
- [ ] Implement wagon interface
- [ ] Handle wagon in/out of combat
- [ ] Test 8-character management

---

## Epic 7: Map & World System
**Label:** `epic`, `area:maps`

### Description
Implement the complete world, map, and navigation systems.

### Sub-Issues

#### 7.1 Map Engine
- [ ] Design map architecture
- [ ] Implement tile rendering
- [ ] Implement collision detection
- [ ] Implement scrolling
- [ ] Implement layer management
- [ ] Implement map transitions

#### 7.2 Overworld Maps
- [ ] Convert overworld data
- [ ] Implement world map rendering
- [ ] Implement vehicle system (ship, balloon)
- [ ] Implement random encounters
- [ ] Implement day/night cycle
- [ ] Implement weather effects

#### 7.3 Town & Dungeon Maps
- [ ] Convert all town maps
- [ ] Convert all dungeon maps
- [ ] Convert all indoor maps
- [ ] Implement map events
- [ ] Implement treasure chests
- [ ] Implement doors/locks

#### 7.4 NPC System
- [ ] Design NPC architecture
- [ ] Implement NPC rendering
- [ ] Implement NPC movement
- [ ] Implement NPC interaction
- [ ] Implement NPC schedules (day/night)

#### 7.5 Event System
- [ ] Design event script format
- [ ] Create event compiler
- [ ] Implement event interpreter
- [ ] Convert all chapter events
- [ ] Implement cutscenes

---

## Epic 8: Menu System
**Label:** `epic`, `area:engine`

### Description
Implement all menu interfaces.

### Sub-Issues

#### 8.1 Main Menu
- [ ] Design menu layout
- [ ] Implement menu rendering
- [ ] Implement cursor navigation
- [ ] Implement menu transitions
- [ ] Implement sound feedback

#### 8.2 Status Screen
- [ ] Design status layout
- [ ] Display character stats
- [ ] Display equipment
- [ ] Display spells known
- [ ] Display party status

#### 8.3 Item Menu
- [ ] Design item layout
- [ ] Implement item list
- [ ] Implement item use
- [ ] Implement item transfer
- [ ] Implement item drop

#### 8.4 Equipment Menu
- [ ] Design equipment layout
- [ ] Implement equip/unequip
- [ ] Display stat changes
- [ ] Handle equipment restrictions

#### 8.5 Shop Menus
- [ ] Design shop layout
- [ ] Implement buy interface
- [ ] Implement sell interface
- [ ] Implement inn/church interfaces

#### 8.6 Save/Load Menu
- [ ] Design save layout
- [ ] Implement save slots
- [ ] Implement save data
- [ ] Implement load confirmation

---

## Epic 9: Chapter Implementation
**Label:** `epic`

### Description
Implement each chapter of the game.

### Sub-Issues

#### 9.1 Chapter 1: Ragnar
- [ ] Set up Chapter 1 data
- [ ] Implement Burland Castle
- [ ] Implement Strathbaile
- [ ] Implement Loch Tur cave
- [ ] Implement boss fight
- [ ] Test chapter completion

#### 9.2 Chapter 2: Alena
- [ ] Set up Chapter 2 data
- [ ] Implement Zamoksva Castle
- [ ] Implement tournament sequence
- [ ] Implement all chapter locations
- [ ] Test chapter completion

#### 9.3 Chapter 3: Torneko
- [ ] Set up Chapter 3 data
- [ ] Implement shop mechanics
- [ ] Implement merchant gameplay
- [ ] Implement all chapter locations
- [ ] Test chapter completion

#### 9.4 Chapter 4: Meena & Maya
- [ ] Set up Chapter 4 data
- [ ] Implement Monbaraba
- [ ] Implement chapter storyline
- [ ] Implement all chapter locations
- [ ] Test chapter completion

#### 9.5 Chapter 5: The Hero
- [ ] Set up Chapter 5 data
- [ ] Implement party reunion
- [ ] Implement all Chapter 5 locations
- [ ] Implement Zenithia
- [ ] Implement final dungeon
- [ ] Implement final boss
- [ ] Test full game completion

---

## Epic 10: Special Features
**Label:** `epic`

### Description
Implement DW4-specific special features.

### Sub-Issues

#### 10.1 Casino System
- [ ] Design casino implementation
- [ ] Implement slot machines
- [ ] Implement poker game
- [ ] Implement monster arena
- [ ] Implement prize exchange

#### 10.2 Medal Collection
- [ ] Design medal system
- [ ] Implement medal locations
- [ ] Implement medal tracking
- [ ] Implement medal rewards

#### 10.3 Post-Game Content
- [ ] Implement bonus dungeons (if any)
- [ ] Implement New Game+ (optional enhancement)
- [ ] Implement extras gallery (optional)

---

## Epic 11: Testing & Quality Assurance
**Label:** `epic`, `area:testing`

### Description
Comprehensive testing of all game systems.

### Sub-Issues

#### 11.1 Unit Testing
- [ ] Test asset conversion tools
- [ ] Test data serialization
- [ ] Test compression algorithms
- [ ] Test text encoding

#### 11.2 Integration Testing
- [ ] Test ROM build process
- [ ] Test emulator compatibility
- [ ] Test save/load system
- [ ] Test chapter transitions

#### 11.3 Gameplay Testing
- [ ] Playtest Chapter 1
- [ ] Playtest Chapter 2
- [ ] Playtest Chapter 3
- [ ] Playtest Chapter 4
- [ ] Playtest Chapter 5
- [ ] Playtest full game start to finish

#### 11.4 Regression Testing
- [ ] Compare text to original
- [ ] Compare battle calculations
- [ ] Compare item effects
- [ ] Verify all events trigger correctly

---

## Issue Templates

### Epic Template
```markdown
## Epic: [Name]

### Description
[High-level description of this epic]

### Goals
- [ ] Goal 1
- [ ] Goal 2
- [ ] Goal 3

### Sub-Issues
- #XX - Issue 1
- #XX - Issue 2
- #XX - Issue 3

### Dependencies
- Depends on: #XX
- Blocks: #XX

### Acceptance Criteria
- [ ] Criterion 1
- [ ] Criterion 2
```

### Feature Issue Template
```markdown
## Feature: [Name]

### Description
[Detailed description of the feature]

### Tasks
- [ ] Task 1
- [ ] Task 2
- [ ] Task 3

### Technical Details
[Implementation notes, data formats, etc.]

### Testing
- [ ] Test case 1
- [ ] Test case 2

### Parent Epic
- Part of: #XX [Epic Name]
```

### Bug Issue Template
```markdown
## Bug: [Short Description]

### Description
[What is happening]

### Expected Behavior
[What should happen]

### Steps to Reproduce
1. Step 1
2. Step 2
3. Step 3

### Environment
- ROM version:
- Emulator:
- Build date:

### Screenshots/Logs
[Attach if applicable]
```

---

## Project Board Columns

1. **Backlog** - Not yet started
2. **Ready** - Ready to work on
3. **In Progress** - Currently being worked on
4. **In Review** - Awaiting review
5. **Done** - Completed

---
*Document created: 2026-01-01*
*Last updated: 2026-01-01*
