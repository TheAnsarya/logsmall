# DQ4r Testing & Quality Assurance Plan

## Overview

This document defines the testing strategy for Dragon Quest IV Remix (DQ4r) to ensure a high-quality, bug-free release.

## Testing Levels

```
┌─────────────────────────────────────────────────────────────────┐
│                    Testing Pyramid                               │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│                        ▲                                         │
│                       ╱ ╲                                        │
│                      ╱   ╲       Manual Playtesting              │
│                     ╱     ╲      (Full game runs)                │
│                    ╱───────╲                                     │
│                   ╱         ╲                                    │
│                  ╱           ╲    System/Integration Tests       │
│                 ╱             ╲   (ROM builds, chapter tests)    │
│                ╱───────────────╲                                 │
│               ╱                 ╲                                │
│              ╱                   ╲  Unit Tests                   │
│             ╱                     ╲ (C# tools, converters)       │
│            ╱───────────────────────╲                             │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

---

## Unit Testing

### Scope
Test individual components in isolation, primarily the C# tooling.

### Test Categories

#### 1. Asset Conversion Tests
```csharp
// DW4Lib.Tests & DQ4rLib.Tests

[Fact]
public void GraphicsConverter_ConvertsTile_Correctly()
[Fact]
public void TextEncoder_EncodesString_WithDTE()
[Fact]
public void MapConverter_PreservesCollision_Data()
[Fact]
public void AudioConverter_OutputsValidBRR_Sample()
```

**Test Files:**
- `DialogExtractorTests.cs` ✅
- `BatchScriptExporterTests.cs` ✅
- `FontRendererTests.cs` ✅
- `GraphicsConverterTests.cs`
- `MapConverterTests.cs`
- `AudioConverterTests.cs`
- `SpriteConverterTests.cs`
- `PaletteConverterTests.cs`

#### 2. Data Serialization Tests
```csharp
[Fact]
public void CharacterData_SerializesAndDeserializes_Correctly()
[Fact]
public void MonsterData_RoundTrips_WithoutLoss()
[Fact]
public void MapHeader_ParsesAllFields_FromROM()
```

#### 3. Compression Tests
```csharp
[Fact]
public void LZ77_Compresses_AndDecompresses_Losslessly()
[Fact]
public void RLE_HandlesEdgeCases_Correctly()
[Fact]
public void DTE_ExpandsAllPairs_Correctly()
```

#### 4. Calculation Tests
```csharp
[Fact]
public void DamageFormula_MatchesOriginal_DW4()
[Fact]
public void ExperienceTable_MatchesOriginal_Values()
[Fact]
public void StatCalculation_ProducesCorrect_Results()
```

### Running Unit Tests
```powershell
# Run all tests
dotnet test logsmall.sln

# Run specific project
dotnet test DW4Lib.Tests/DW4Lib.Tests.csproj

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test
dotnet test --filter "FullyQualifiedName~DialogExtractorTests"
```

### Coverage Goals
- **Tools (DW4Lib, DQ4rLib):** 80%+ coverage
- **Critical paths:** 100% coverage
- **Edge cases:** Documented and tested

---

## Integration Testing

### Scope
Test that components work together correctly.

### Test Categories

#### 1. Build Pipeline Tests
```yaml
# CI/CD test steps
- name: Build ROM
  run: |
    dotnet run --project DQ4rLib -- build
    asar src/main.asm build/dq4r.sfc

- name: Verify ROM
  run: |
    python tools/verify_rom.py build/dq4r.sfc
    
- name: Check ROM Size
  run: |
    test $(stat -f%z build/dq4r.sfc) -le 4194304  # 4MB max
```

#### 2. Emulator Boot Tests
```python
# Automated emulator testing
def test_rom_boots():
    """ROM boots without crashing"""
    result = run_emulator("dq4r.sfc", frames=300)
    assert result.exit_code == 0
    assert not result.crashed
    
def test_title_screen_appears():
    """Title screen displays correctly"""
    result = run_emulator("dq4r.sfc", frames=600)
    screenshot = result.screenshot
    assert compare_image(screenshot, "expected_title.png", threshold=0.95)
```

#### 3. Save/Load Tests
```python
def test_save_load_cycle():
    """Save and load preserves game state"""
    state1 = create_test_save()
    save_game(state1, slot=1)
    state2 = load_game(slot=1)
    assert state1 == state2
```

#### 4. Chapter Transition Tests
```python
def test_chapter1_to_chapter2():
    """Completing Chapter 1 transitions to Chapter 2"""
    play_until("chapter1_end")
    assert get_current_chapter() == 2
    assert get_party() == ["Alena", "Kiryl", "Borya"]
```

### Emulator Compatibility Matrix

| Emulator | Priority | Status | Notes |
|----------|----------|--------|-------|
| bsnes (accuracy) | Critical | ⬜ | Reference standard |
| Mesen-S | High | ⬜ | Good debugger |
| Snes9x 1.60+ | High | ⬜ | Popular choice |
| higan | Medium | ⬜ | Accuracy focused |
| ZSNES | Low | ⬜ | Legacy (may skip) |
| Hardware (SD2SNES) | Medium | ⬜ | Real hardware test |

---

## Gameplay Testing

### Scope
Full playthrough testing by humans.

### Test Protocols

#### 1. Chapter Completion Tests

**Chapter 1 Checklist:**
- [ ] Game starts correctly
- [ ] Ragnar can move and interact
- [ ] All Burland Castle NPCs work
- [ ] Can exit to overworld
- [ ] Random encounters work
- [ ] Battles function correctly
- [ ] Can save/load game
- [ ] Can complete Loch Tur
- [ ] Boss fight works
- [ ] Chapter ends correctly

**Chapter 2 Checklist:**
- [ ] Chapter 2 starts with correct party
- [ ] All locations accessible
- [ ] Tournament sequence works
- [ ] Chapter ends correctly

**Chapter 3 Checklist:**
- [ ] Merchant mechanics work
- [ ] Shop buying/selling works
- [ ] All side content accessible
- [ ] Chapter ends correctly

**Chapter 4 Checklist:**
- [ ] Sisters story progresses
- [ ] All events trigger
- [ ] Chapter ends correctly

**Chapter 5 Checklist:**
- [ ] Hero creation works
- [ ] Party reunions work
- [ ] Wagon system works
- [ ] Tactics AI works
- [ ] All locations accessible
- [ ] Final dungeon completable
- [ ] Final boss defeatable
- [ ] Ending plays correctly

#### 2. Feature Testing Matrices

**Battle System Matrix:**

| Feature | Ch1 | Ch2 | Ch3 | Ch4 | Ch5 |
|---------|-----|-----|-----|-----|-----|
| Basic attack | ⬜ | ⬜ | ⬜ | ⬜ | ⬜ |
| Defend | ⬜ | ⬜ | ⬜ | ⬜ | ⬜ |
| Flee | ⬜ | ⬜ | ⬜ | ⬜ | ⬜ |
| Spells | ⬜ | ⬜ | ⬜ | ⬜ | ⬜ |
| Items | ⬜ | ⬜ | ⬜ | ⬜ | ⬜ |
| Tactics | - | - | - | - | ⬜ |
| Wagon swap | - | - | - | - | ⬜ |

**Menu System Matrix:**

| Menu | Test Status | Notes |
|------|-------------|-------|
| Main menu | ⬜ | |
| Status | ⬜ | |
| Items | ⬜ | |
| Equip | ⬜ | |
| Spells | ⬜ | |
| Tactics | ⬜ | |
| Settings | ⬜ | |

#### 3. Regression Testing

**Text Comparison:**
```python
def compare_dialog_to_original():
    """All dialog matches original DW4"""
    for entry in all_dialog_entries:
        original = get_original_text(entry.id)
        converted = get_converted_text(entry.id)
        assert normalize(original) == normalize(converted)
```

**Numeric Verification:**
```python
def verify_damage_formulas():
    """Battle damage matches original DW4"""
    test_cases = load_test_cases("damage_tests.json")
    for case in test_cases:
        expected = case.expected_damage
        actual = calculate_damage(case.attacker, case.defender)
        assert abs(expected - actual) <= 1  # Allow rounding
```

---

## Bug Tracking

### Bug Severity Levels

| Level | Description | Example | Response |
|-------|-------------|---------|----------|
| **Critical** | Game-breaking | Crash, softlock, save corruption | Fix immediately |
| **High** | Major issue | Quest can't complete, wrong damage | Fix before milestone |
| **Medium** | Noticeable problem | Visual glitch, wrong text | Fix before release |
| **Low** | Minor issue | Slight misalignment, typo | Fix if time permits |

### Bug Report Template
```markdown
## Bug Report

**Severity:** [Critical/High/Medium/Low]
**Chapter:** [1/2/3/4/5/All]
**Area:** [Location or system]

### Description
[What happened]

### Expected
[What should happen]

### Steps to Reproduce
1. Step 1
2. Step 2
3. Step 3

### Save State
[Attach save state if applicable]

### Screenshots/Video
[Attach if applicable]

### Environment
- Build: [date/commit]
- Emulator: [name/version]
```

### Bug Database
All bugs tracked in GitHub Issues with labels:
- `bug`
- `severity:critical/high/medium/low`
- `chapter:1/2/3/4/5`
- `area:battle/map/menu/text/audio/graphics`

---

## QA Milestones

### Alpha Testing (End of Phase 4)
**Goal:** All content present, may have bugs

**Criteria:**
- [ ] All chapters playable
- [ ] All major features implemented
- [ ] Known bugs documented
- [ ] No critical bugs

### Beta Testing (Phase 5, Month 1)
**Goal:** Feature-complete, limited bugs

**Criteria:**
- [ ] All features working
- [ ] No high-severity bugs
- [ ] Medium bugs documented
- [ ] External testers invited

### Release Candidate (Phase 5, Month 2)
**Goal:** Release-ready, minimal bugs

**Criteria:**
- [ ] No critical or high bugs
- [ ] All medium bugs fixed or deferred
- [ ] Full playthrough completed
- [ ] Emulator compatibility verified

### Gold Master (Phase 5, Month 3)
**Goal:** Final release version

**Criteria:**
- [ ] All known bugs fixed or documented
- [ ] Final playthrough sign-off
- [ ] Documentation complete
- [ ] Release notes written

---

## Automated Testing Infrastructure

### CI/CD Pipeline
```yaml
name: DQ4r CI

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'
          
      - name: Run Unit Tests
        run: dotnet test --verbosity normal
        
      - name: Build Tools
        run: dotnet build DQ4rLib/DQ4rLib.csproj
        
      - name: Generate Assets
        run: dotnet run --project DQ4rLib -- generate
        
      - name: Build ROM
        run: asar src/main.asm build/dq4r.sfc
        
      - name: Verify ROM
        run: python tools/verify_rom.py build/dq4r.sfc
        
      - name: Upload Artifact
        uses: actions/upload-artifact@v4
        with:
          name: dq4r-build
          path: build/dq4r.sfc
```

### Nightly Builds
- Automated full build every night
- Run extended test suite
- Generate coverage reports
- Archive build artifacts

### Test Reporting
- Unit test results in CI
- Coverage reports published
- Bug trends tracked over time
- Milestone progress dashboards

---

## Test Data Management

### Test Save States
```
test-saves/
├── chapter1/
│   ├── start.sav
│   ├── midpoint.sav
│   └── before-boss.sav
├── chapter2/
│   ├── start.sav
│   └── tournament.sav
├── chapter3/
│   ├── start.sav
│   └── shop-full.sav
├── chapter4/
│   ├── start.sav
│   └── midpoint.sav
└── chapter5/
    ├── start.sav
    ├── full-party.sav
    ├── before-zenithia.sav
    └── before-final-boss.sav
```

### Test ROM Variants
- `dq4r-debug.sfc` - Debug build with logging
- `dq4r-release.sfc` - Release build
- `dq4r-test.sfc` - Test build with cheats

---

## Acceptance Criteria Summary

### Minimum Viable Product (MVP)
- [ ] Game boots and shows title screen
- [ ] All 5 chapters completable
- [ ] Save/load works
- [ ] No critical bugs
- [ ] Runs on bsnes

### Full Release
- [ ] All MVP criteria met
- [ ] All features match original DW4
- [ ] No high/medium bugs
- [ ] Emulator compatibility verified
- [ ] Documentation complete
- [ ] Performance acceptable

---
*Document created: 2026-01-01*
*Last updated: 2026-01-01*
