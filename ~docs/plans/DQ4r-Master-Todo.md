# DQ4r Master Todo List

## Quick Reference - Current Status

**Project:** Dragon Quest IV Remix (DQ4r)
**Phase:** 1 - Foundation
**Current Focus:** Documentation & Planning

---

## Phase 1: Foundation

### ✅ Documentation & Planning (CURRENT)
- [x] Create project overview document
- [x] Create technical architecture document
- [x] Create asset pipeline document
- [x] Create GitHub issues structure document
- [x] Create development roadmap
- [x] Create testing & QA plan
- [ ] Create contributing guidelines
- [ ] Create development environment setup guide

### Repository Setup
- [ ] Create `dq4r-info` repository
- [ ] Set up directory structure
- [ ] Create initial README
- [ ] Configure `.editorconfig`
- [ ] Set up `.gitignore`
- [ ] Configure CI/CD (GitHub Actions)

### Tool Development
- [ ] Create DQ4rLib C# project
- [ ] Implement graphics converter (NES→SNES 4bpp)
- [ ] Implement palette converter
- [ ] Implement sprite generator
- [ ] Implement map compiler
- [ ] Implement audio converter (placeholder)
- [ ] Implement text compiler
- [ ] Create build automation scripts

### DW4 NES Analysis (Ongoing)
- [x] Text extraction tools (DialogExtractor)
- [x] Batch export tools (BatchScriptExporter)
- [x] Font conversion tools (FontToDQ3r)
- [x] Control codes documentation
- [ ] Complete ROM map documentation
- [ ] Complete graphics extraction
- [ ] Complete audio analysis
- [ ] Complete map format documentation
- [ ] Complete event script documentation

### DQ3r Engine Study
- [ ] Obtain DQ3r documentation/source
- [ ] Document DQ3r memory map
- [ ] Document DQ3r API/system calls
- [ ] Identify reusable components
- [ ] Plan required modifications
- [ ] Create adaptation strategy

---

## Phase 2: Core Systems

### Engine Foundation
- [ ] Set up base ROM structure
- [ ] Initialize SNES memory layout
- [ ] Implement main game loop
- [ ] Implement VBlank handler
- [ ] Implement DMA routines

### Graphics Engine
- [ ] Port/adapt tile renderer from DQ3r
- [ ] Implement sprite manager
- [ ] Implement palette manager
- [ ] Implement layer management
- [ ] Implement animation system

### Audio Engine
- [ ] Port/adapt SPC700 driver from DQ3r
- [ ] Implement music playback
- [ ] Implement SFX playback
- [ ] Implement volume/stereo control

### Text Engine
- [ ] Port/adapt VWF renderer from DQ3r
- [ ] Implement dialog box system
- [ ] Implement text decompression
- [ ] Implement control code processing
- [ ] Implement name substitution

### Menu System
- [ ] Implement menu framework
- [ ] Create main menu
- [ ] Create status screen
- [ ] Create item menu
- [ ] Create equipment menu
- [ ] Create shop interfaces
- [ ] Create save/load interface

### Map Engine
- [ ] Implement tile renderer
- [ ] Implement collision detection
- [ ] Implement scrolling
- [ ] Implement map transitions
- [ ] Implement event triggers

### Battle System
- [ ] Implement battle initialization
- [ ] Implement turn order calculation
- [ ] Implement action execution
- [ ] Implement damage calculation
- [ ] Implement status effects
- [ ] Implement battle AI
- [ ] Implement escape mechanics
- [ ] Implement battle UI

---

## Phase 3: Content Integration

### Graphics Content
- [ ] Convert all character sprites
- [ ] Convert all monster sprites
- [ ] Convert all tilesets
- [ ] Convert all battle backgrounds
- [ ] Convert all UI graphics
- [ ] Create/convert animations

### Audio Content
- [ ] Convert/rearrange all music tracks
- [ ] Create/convert all sound effects
- [ ] Set up music triggers
- [ ] Set up SFX triggers

### Text Content
- [ ] Convert all Chapter 1 dialog
- [ ] Convert all Chapter 2 dialog
- [ ] Convert all Chapter 3 dialog
- [ ] Convert all Chapter 4 dialog
- [ ] Convert all Chapter 5 dialog
- [ ] Convert all menu text
- [ ] Convert all battle text
- [ ] Convert all names (items, spells, monsters, etc.)

### Map Content
- [ ] Convert overworld map(s)
- [ ] Convert all Chapter 1 maps
- [ ] Convert all Chapter 2 maps
- [ ] Convert all Chapter 3 maps
- [ ] Convert all Chapter 4 maps
- [ ] Convert all Chapter 5 maps
- [ ] Implement all warps/transitions

### Data Content
- [ ] Convert experience tables
- [ ] Convert monster stats
- [ ] Convert item data
- [ ] Convert spell data
- [ ] Convert equipment data
- [ ] Convert shop inventories
- [ ] Convert encounter tables

---

## Phase 4: Chapter Implementation

### Chapter 1: Ragnar
- [ ] Implement starting sequence
- [ ] Implement Burland Castle
- [ ] Implement all chapter locations
- [ ] Implement all NPCs and events
- [ ] Implement boss fight
- [ ] Implement chapter ending
- [ ] Test full chapter completion

### Chapter 2: Alena
- [ ] Implement starting sequence
- [ ] Implement Zamoksva Castle
- [ ] Implement tournament
- [ ] Implement all chapter locations
- [ ] Implement all NPCs and events
- [ ] Implement chapter ending
- [ ] Test full chapter completion

### Chapter 3: Torneko
- [ ] Implement starting sequence
- [ ] Implement merchant mechanics
- [ ] Implement shop system
- [ ] Implement all chapter locations
- [ ] Implement all NPCs and events
- [ ] Implement chapter ending
- [ ] Test full chapter completion

### Chapter 4: Meena & Maya
- [ ] Implement starting sequence
- [ ] Implement Monbaraba
- [ ] Implement all chapter locations
- [ ] Implement all NPCs and events
- [ ] Implement chapter ending
- [ ] Test full chapter completion

### Chapter 5: The Hero
- [ ] Implement character creation
- [ ] Implement starting village
- [ ] Implement party reunions
- [ ] Implement wagon system
- [ ] Implement tactics AI
- [ ] Implement all chapter locations
- [ ] Implement Zenithia sequence
- [ ] Implement final dungeon
- [ ] Implement final boss
- [ ] Implement ending
- [ ] Test full chapter completion

### Special Features
- [ ] Implement casino (if included)
- [ ] Implement mini medal system (if included)
- [ ] Implement bonus content (if any)

---

## Phase 5: Polish & Release

### Bug Fixing
- [ ] Fix all critical bugs
- [ ] Fix all high-priority bugs
- [ ] Fix all medium-priority bugs
- [ ] Address low-priority bugs as time permits

### Balance & Testing
- [ ] Complete playthrough testing
- [ ] Verify battle balance
- [ ] Verify progression curve
- [ ] Verify all events trigger correctly

### QA
- [ ] Alpha testing
- [ ] Beta testing (external testers)
- [ ] Release candidate testing
- [ ] Final sign-off

### Release
- [ ] Create release build
- [ ] Write release notes
- [ ] Update documentation
- [ ] Publish release
- [ ] Announce release

---

## Ongoing Tasks

### Documentation
- [ ] Maintain wiki/docs as development progresses
- [ ] Document all discovered information
- [ ] Keep roadmap updated

### Testing
- [ ] Write unit tests for new code
- [ ] Run regression tests regularly
- [ ] Track and fix bugs promptly

### Version Control
- [ ] Commit regularly with good messages
- [ ] Use feature branches
- [ ] Review and merge PRs
- [ ] Tag milestones

---

## Quick Links

- [Project Overview](./DQ4r-Project-Overview.md)
- [Technical Architecture](./DQ4r-Technical-Architecture.md)
- [Asset Pipeline](./DQ4r-Asset-Pipeline.md)
- [GitHub Issues](./DQ4r-GitHub-Issues.md)
- [Roadmap](./DQ4r-Roadmap.md)
- [Testing Plan](./DQ4r-Testing-Plan.md)

---
*Last updated: 2026-01-01*
