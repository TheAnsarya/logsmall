# DQ4r Development Roadmap

## Project Timeline Overview

**Project Start:** January 2026
**Estimated Duration:** 24-30 months
**Target Completion:** Mid-2028

```
2026 Q1 ████████░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ Phase 1: Foundation
2026 Q2 ░░░░░░░░████████░░░░░░░░░░░░░░░░░░░░░░░░ Phase 1 cont.
2026 Q3 ░░░░░░░░░░░░░░░░████████████░░░░░░░░░░░░ Phase 2: Core Systems
2026 Q4 ░░░░░░░░░░░░░░░░░░░░░░░░░░░░████████████ Phase 2 cont.
2027 Q1 ████████████████░░░░░░░░░░░░░░░░░░░░░░░░ Phase 3: Content
2027 Q2 ░░░░░░░░░░░░░░░░████████████████░░░░░░░░ Phase 3 cont.
2027 Q3 ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░████████ Phase 4: Chapters
2027 Q4 ████████████████████░░░░░░░░░░░░░░░░░░░░ Phase 4 cont.
2028 Q1 ░░░░░░░░░░░░░░░░░░░░████████████████████ Phase 5: Polish
2028 Q2 ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░████████ Release
```

---

## Phase 1: Foundation (Months 1-6)

### Milestone 1.1: Project Setup (Month 1)
**Target:** January 2026

| Task | Priority | Status | Owner |
|------|----------|--------|-------|
| Create dq4r-info repository | Critical | ⬜ | - |
| Set up build system | Critical | ⬜ | - |
| Create project documentation | High | ✅ | - |
| Set up CI/CD | Medium | ⬜ | - |
| Create GitHub issues | High | 🔄 | - |

**Deliverables:**
- [ ] Repository with basic structure
- [ ] Working build script
- [ ] All planning documents
- [ ] GitHub project board

### Milestone 1.2: DW4 Analysis Completion (Months 2-3)
**Target:** February-March 2026

| Task | Priority | Status |
|------|----------|--------|
| Complete ROM map | Critical | 🔄 |
| Document all data formats | Critical | 🔄 |
| Extract all assets | Critical | 🔄 |
| Complete text extraction | High | ✅ |
| Complete graphics extraction | High | 🔄 |
| Complete audio analysis | Medium | ⬜ |

**Deliverables:**
- [ ] Complete ROM documentation wiki
- [ ] All assets extracted to editable formats
- [ ] Data format specifications

### Milestone 1.3: DQ3r Engine Study (Months 3-4)
**Target:** March-April 2026

| Task | Priority | Status |
|------|----------|--------|
| Obtain DQ3r source/docs | Critical | ⬜ |
| Document memory map | Critical | ⬜ |
| Document system calls | High | ⬜ |
| Identify reusable code | High | ⬜ |
| Plan modifications needed | High | ⬜ |

**Deliverables:**
- [ ] DQ3r architecture document
- [ ] Modification plan
- [ ] Code reuse inventory

### Milestone 1.4: Tool Development (Months 4-6)
**Target:** April-June 2026

| Task | Priority | Status |
|------|----------|--------|
| Create DQ4rLib project | Critical | ⬜ |
| Graphics converter (NES→SNES) | Critical | ⬜ |
| Text converter | High | 🔄 |
| Map converter | High | ⬜ |
| Audio converter | Medium | ⬜ |
| Build pipeline automation | High | ⬜ |

**Deliverables:**
- [ ] Working conversion tools
- [ ] Automated build pipeline
- [ ] Tool documentation

---

## Phase 2: Core Systems (Months 7-12)

### Milestone 2.1: Engine Foundation (Months 7-8)
**Target:** July-August 2026

| Task | Priority | Status |
|------|----------|--------|
| Set up base ROM structure | Critical | ⬜ |
| Initialize memory layout | Critical | ⬜ |
| Implement core loops | Critical | ⬜ |
| Port/adapt graphics engine | High | ⬜ |
| Port/adapt audio engine | High | ⬜ |

**Deliverables:**
- [ ] Bootable ROM (shows title screen)
- [ ] Basic graphics rendering
- [ ] Basic audio playback

### Milestone 2.2: Menu System (Months 8-9)
**Target:** August-September 2026

| Task | Priority | Status |
|------|----------|--------|
| Implement menu framework | High | ⬜ |
| Create main menu | High | ⬜ |
| Create status screen | High | ⬜ |
| Create item menu | High | ⬜ |
| Create equipment menu | High | ⬜ |

**Deliverables:**
- [ ] Functional menu system
- [ ] All menu screens navigable

### Milestone 2.3: Map Engine (Months 9-10)
**Target:** September-October 2026

| Task | Priority | Status |
|------|----------|--------|
| Implement tile renderer | Critical | ⬜ |
| Implement scrolling | Critical | ⬜ |
| Implement collision | Critical | ⬜ |
| Implement map loading | High | ⬜ |
| Implement transitions | High | ⬜ |

**Deliverables:**
- [ ] Character can walk on maps
- [ ] Maps load and display correctly
- [ ] Collision works properly

### Milestone 2.4: Battle System (Months 10-12)
**Target:** October-December 2026

| Task | Priority | Status |
|------|----------|--------|
| Implement battle start/end | Critical | ⬜ |
| Implement turn system | Critical | ⬜ |
| Implement commands | Critical | ⬜ |
| Implement damage calc | Critical | ⬜ |
| Implement basic AI | High | ⬜ |
| Implement battle UI | High | ⬜ |

**Deliverables:**
- [ ] Battles can be fought
- [ ] Damage calculations work
- [ ] Enemies have basic AI

---

## Phase 3: Content Integration (Months 13-18)

### Milestone 3.1: Graphics Integration (Months 13-14)
**Target:** January-February 2027

| Task | Priority | Status |
|------|----------|--------|
| Convert all character sprites | High | ⬜ |
| Convert all monster sprites | High | ⬜ |
| Convert all tilesets | High | ⬜ |
| Convert all UI graphics | Medium | ⬜ |
| Implement animations | Medium | ⬜ |

**Deliverables:**
- [ ] All graphics in SNES format
- [ ] Graphics display correctly in-game

### Milestone 3.2: Audio Integration (Months 14-15)
**Target:** February-March 2027

| Task | Priority | Status |
|------|----------|--------|
| Convert all music tracks | High | ⬜ |
| Convert all sound effects | High | ⬜ |
| Implement music triggers | Medium | ⬜ |
| Implement SFX triggers | Medium | ⬜ |

**Deliverables:**
- [ ] All music converted and playing
- [ ] All sound effects working

### Milestone 3.3: Text Integration (Months 15-16)
**Target:** March-April 2027

| Task | Priority | Status |
|------|----------|--------|
| Convert all dialog | High | ⬜ |
| Implement VWF | High | ⬜ |
| Test all text display | Medium | ⬜ |
| Fix encoding issues | Medium | ⬜ |

**Deliverables:**
- [ ] All text displays correctly
- [ ] VWF working properly

### Milestone 3.4: Map Integration (Months 16-18)
**Target:** April-June 2027

| Task | Priority | Status |
|------|----------|--------|
| Convert all overworld maps | High | ⬜ |
| Convert all town maps | High | ⬜ |
| Convert all dungeon maps | High | ⬜ |
| Implement all warps | High | ⬜ |
| Test all map transitions | Medium | ⬜ |

**Deliverables:**
- [ ] All maps loadable
- [ ] All transitions working

---

## Phase 4: Chapter Implementation (Months 19-24)

### Milestone 4.1: Chapter 1 - Ragnar (Month 19)
**Target:** July 2027

| Task | Priority | Status |
|------|----------|--------|
| Implement Chapter 1 maps | Critical | ⬜ |
| Implement Chapter 1 events | Critical | ⬜ |
| Implement Chapter 1 NPCs | High | ⬜ |
| Implement Chapter 1 boss | High | ⬜ |
| Test chapter completion | High | ⬜ |

**Deliverables:**
- [ ] Chapter 1 playable start to finish

### Milestone 4.2: Chapter 2 - Alena (Month 20)
**Target:** August 2027

| Task | Priority | Status |
|------|----------|--------|
| Implement Chapter 2 maps | Critical | ⬜ |
| Implement Chapter 2 events | Critical | ⬜ |
| Implement tournament | High | ⬜ |
| Test chapter completion | High | ⬜ |

**Deliverables:**
- [ ] Chapter 2 playable start to finish

### Milestone 4.3: Chapter 3 - Torneko (Month 21)
**Target:** September 2027

| Task | Priority | Status |
|------|----------|--------|
| Implement Chapter 3 maps | Critical | ⬜ |
| Implement shop mechanics | Critical | ⬜ |
| Implement merchant gameplay | High | ⬜ |
| Test chapter completion | High | ⬜ |

**Deliverables:**
- [ ] Chapter 3 playable start to finish

### Milestone 4.4: Chapter 4 - Sisters (Month 22)
**Target:** October 2027

| Task | Priority | Status |
|------|----------|--------|
| Implement Chapter 4 maps | Critical | ⬜ |
| Implement Chapter 4 events | Critical | ⬜ |
| Test chapter completion | High | ⬜ |

**Deliverables:**
- [ ] Chapter 4 playable start to finish

### Milestone 4.5: Chapter 5 - Hero (Months 23-24)
**Target:** November-December 2027

| Task | Priority | Status |
|------|----------|--------|
| Implement Chapter 5 intro | Critical | ⬜ |
| Implement party reunions | Critical | ⬜ |
| Implement Wagon system | Critical | ⬜ |
| Implement Tactics AI | Critical | ⬜ |
| Implement all Ch5 locations | Critical | ⬜ |
| Implement Zenithia | Critical | ⬜ |
| Implement final dungeon | Critical | ⬜ |
| Implement final boss | Critical | ⬜ |

**Deliverables:**
- [ ] Chapter 5 playable start to finish
- [ ] Full game completable

---

## Phase 5: Polish & Release (Months 25-30)

### Milestone 5.1: Bug Fixing (Months 25-26)
**Target:** January-February 2028

| Task | Priority | Status |
|------|----------|--------|
| Fix critical bugs | Critical | ⬜ |
| Fix gameplay bugs | High | ⬜ |
| Fix visual glitches | Medium | ⬜ |
| Fix audio issues | Medium | ⬜ |

### Milestone 5.2: Balance Testing (Months 26-27)
**Target:** February-March 2028

| Task | Priority | Status |
|------|----------|--------|
| Test battle balance | High | ⬜ |
| Test progression curve | High | ⬜ |
| Adjust as needed | Medium | ⬜ |

### Milestone 5.3: QA Passes (Months 27-28)
**Target:** March-April 2028

| Task | Priority | Status |
|------|----------|--------|
| Full playthrough test | Critical | ⬜ |
| Edge case testing | High | ⬜ |
| Emulator compatibility | High | ⬜ |

### Milestone 5.4: Final Polish (Months 28-29)
**Target:** April-May 2028

| Task | Priority | Status |
|------|----------|--------|
| Final bug fixes | Critical | ⬜ |
| Performance optimization | Medium | ⬜ |
| Final documentation | Medium | ⬜ |

### Milestone 5.5: Release (Month 30)
**Target:** June 2028

| Task | Priority | Status |
|------|----------|--------|
| Create release build | Critical | ⬜ |
| Write release notes | High | ⬜ |
| Publish release | High | ⬜ |

---

## Risk Assessment

### High Risk Items
1. **DQ3r Engine Complexity** - May require more modification than expected
2. **Tactics AI** - DW4's AI system is complex
3. **Audio Conversion** - NES→SNES music rearrangement is time-consuming
4. **Chapter 5 Scope** - Largest chapter with most content

### Mitigation Strategies
1. Allow buffer time in Phase 2 for engine work
2. Start AI research early (Phase 1)
3. Consider outsourcing music arrangement
4. Break Chapter 5 into smaller milestones

### Dependencies
- DQ3r source code/documentation access
- Tool development completion before content work
- Each chapter depends on core systems

---

## Resource Requirements

### Time Commitment
- **Full-time equivalent:** ~20-30 hours/week
- **Calendar time:** 24-30 months

### Skills Needed
- 65816 assembly programming
- SNES hardware knowledge
- C# for tools development
- Music arrangement (or collaborator)
- Pixel art (or collaborator)

### Tools & Software
- Asar assembler
- .NET 10 SDK
- bsnes/Mesen-S emulators
- Graphics editing software
- Audio editing software

---

## Progress Tracking

### Status Legend
- ⬜ Not Started
- 🔄 In Progress
- ✅ Complete
- ❌ Blocked
- ⏸️ On Hold

### Weekly Check-ins
- Review milestone progress
- Identify blockers
- Adjust timeline as needed
- Update documentation

### Monthly Reviews
- Assess overall progress
- Evaluate risks
- Celebrate wins
- Plan next month

---
*Document created: 2026-01-01*
*Last updated: 2026-01-01*
