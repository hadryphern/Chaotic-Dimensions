# Kraken Animation Rebuild Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Rebuild the Alien Kraken's four 36-frame atlases from stable source layers so every tentacle remains attached, the entire silhouette shares one outline treatment, and movement reacts smoothly to idle, horizontal travel, and ascent.

**Architecture:** A deterministic Python/Pillow/NumPy offline pipeline reads the original transparent layers plus a JSON rig, warps each tentacle around a fixed root, composites the body and Ruby anchor, applies a unified navy outline, and writes normalized 6x6 atlases. Python tests validate topology, frame dimensions, alpha cleanliness, loop closure, and anchor stability before the generated assets replace the current tModLoader textures.

**Tech Stack:** Python 3.11+, Pillow, NumPy, JSON, `unittest`, C#/.NET 8, tModLoader `ModNPC.PreDraw`.

---

## File Structure

- `Assets/Source/Kraken/*.png`: canonical separated artwork copied from the user's Pixilart export.
- `tools/KrakenAnimation/kraken_rig.json`: layer order, fixed roots, motion profiles, canvas size, and Ruby anchor.
- `tools/KrakenAnimation/kraken_animation.py`: reusable geometry, mesh-warp, outline, composition, and atlas functions.
- `tools/KrakenAnimation/generate_kraken_atlases.py`: command-line entry point that writes production atlases and previews.
- `tools/KrakenAnimation/validate_kraken_atlases.py`: command-line quality gate used independently of generation.
- `tools/KrakenAnimation/tests/test_kraken_animation.py`: deterministic unit and integration tests.
- `Content/NPCs/Kraken/KrakenBoss*.png`: generated production atlases.
- `Content/NPCs/Kraken/KrakenBoss.cs`: atlas anchor and Ruby placement integration.
- `build.txt`: excludes source artwork and tools from the `.tmod` package.
- `.gitignore`: excludes generated preview files.

### Task 1: Establish Reproducible Source Assets

**Files:**
- Create: `Assets/Source/Kraken/pixil-layer-0.png` through `pixil-layer-13.png`
- Modify: `build.txt`
- Modify: `.gitignore`

- [ ] **Step 1: Copy the fourteen approved source layers**

Run:

```powershell
New-Item -ItemType Directory -Force Assets\Source\Kraken
Copy-Item C:\Users\unknown\Downloads\novo-kraken\pixil-layer-*.png Assets\Source\Kraken\
```

Expected: fourteen PNG files, each `570x990`, with transparent backgrounds.

- [ ] **Step 2: Exclude production sources and previews from packaging**

Set `build.txt`:

```text
buildIgnore = tmp/*, docs/*, Assets/Fonts/*, Assets/Source/*, tools/*
```

Append to `.gitignore`:

```text
tools/KrakenAnimation/output/
```

- [ ] **Step 3: Verify source dimensions and count**

Run:

```powershell
python tools\KrakenAnimation\validate_sources.py
```

Expected: `14 source layers valid (570x990 RGBA)`.

- [ ] **Step 4: Commit source setup**

```powershell
git add Assets/Source/Kraken build.txt .gitignore
git commit -m "chore: preserve Kraken animation sources"
```

### Task 2: Define the Rig with Tests First

**Files:**
- Create: `tools/KrakenAnimation/kraken_rig.json`
- Create: `tools/KrakenAnimation/tests/test_kraken_animation.py`
- Create: `tools/KrakenAnimation/kraken_animation.py`

- [ ] **Step 1: Write failing rig tests**

Create tests that load `kraken_rig.json` and assert:

```python
class RigTests(unittest.TestCase):
    def test_rig_has_fourteen_unique_layers(self):
        rig = load_rig(RIG_PATH)
        self.assertEqual(14, len(rig.layers))
        self.assertEqual(14, len({layer.index for layer in rig.layers}))

    def test_every_tentacle_has_fixed_root_inside_source_canvas(self):
        rig = load_rig(RIG_PATH)
        for layer in rig.tentacles:
            self.assertGreaterEqual(layer.root[0], 0)
            self.assertLess(layer.root[0], rig.source_width)
            self.assertGreaterEqual(layer.root[1], 0)
            self.assertLess(layer.root[1], rig.source_height)

    def test_motion_profiles_cover_all_production_atlases(self):
        rig = load_rig(RIG_PATH)
        self.assertEqual(
            {"idle", "loop_back", "horizontal", "up"},
            set(rig.motion_profiles),
        )
```

- [ ] **Step 2: Run tests and confirm RED**

Run:

```powershell
python -m unittest tools.KrakenAnimation.tests.test_kraken_animation -v
```

Expected: FAIL because `kraken_animation` and `kraken_rig.json` do not exist.

- [ ] **Step 3: Implement typed rig loading**

Implement `Rig`, `LayerSpec`, `MotionProfile`, and `load_rig()` in `kraken_animation.py`. Reject duplicate indices, missing files, roots outside `570x990`, and unsupported layer roles.

- [ ] **Step 4: Add the approved rig data**

Define:

- layers `0-2` as long tentacles;
- layers `3-9` as central tentacles;
- layer `10` as the stable head/torso;
- layers `11-13` as Ruby states;
- fixed roots taken from each source layer's top attachment point;
- production frame size `640x1024`;
- atlas grid `6x6`;
- outline colour `#071334`;
- Ruby anchor relative to the stable head/torso.

- [ ] **Step 5: Run tests and confirm GREEN**

Run:

```powershell
python -m unittest tools.KrakenAnimation.tests.test_kraken_animation -v
```

Expected: all rig tests pass.

- [ ] **Step 6: Commit the rig**

```powershell
git add tools/KrakenAnimation
git commit -m "test: define stable Kraken animation rig"
```

### Task 3: Implement Continuous Tentacle Deformation

**Files:**
- Modify: `tools/KrakenAnimation/tests/test_kraken_animation.py`
- Modify: `tools/KrakenAnimation/kraken_animation.py`

- [ ] **Step 1: Write failing geometry tests**

Add tests:

```python
def test_root_is_unchanged_by_deformation(self):
    points = deform_centerline(ROOT, LENGTH, phase=1.2, profile=IDLE_PROFILE)
    self.assertAlmostEqual(ROOT[0], points[0][0], places=5)
    self.assertAlmostEqual(ROOT[1], points[0][1], places=5)

def test_tip_moves_more_than_root(self):
    points = deform_centerline(ROOT, LENGTH, phase=1.2, profile=IDLE_PROFILE)
    self.assertLess(distance(points[0], ROOT), distance(points[-1], (ROOT[0], ROOT[1] + LENGTH)))

def test_loop_closes_exactly(self):
    first = deform_centerline(ROOT, LENGTH, phase=0.0, profile=IDLE_PROFILE)
    last = deform_centerline(ROOT, LENGTH, phase=math.tau, profile=IDLE_PROFILE)
    self.assert_points_close(first, last, tolerance=1e-6)
```

- [ ] **Step 2: Run geometry tests and confirm RED**

Expected: FAIL because deformation functions are absent.

- [ ] **Step 3: Implement centerline and inverse mesh warp**

Use a normalized source coordinate per tentacle. Preserve the root exactly, apply sinusoidal lateral displacement with a `progress ** 1.45` envelope, add delayed tip motion, and sample source pixels through bilinear interpolation into a supersampled canvas.

- [ ] **Step 4: Implement motion profiles**

Implement:

- idle: broad asynchronous waves plus light body breathing;
- loop-back: cyclic return to the idle seed;
- horizontal: opposite-direction drag and moderate body lean;
- up: downward drag and vertical body compression.

- [ ] **Step 5: Run geometry tests and confirm GREEN**

Expected: all geometry tests pass.

- [ ] **Step 6: Commit deformation engine**

```powershell
git add tools/KrakenAnimation
git commit -m "feat: add continuous Kraken tentacle deformation"
```

### Task 4: Compose, Outline, and Generate Atlases

**Files:**
- Modify: `tools/KrakenAnimation/tests/test_kraken_animation.py`
- Modify: `tools/KrakenAnimation/kraken_animation.py`
- Create: `tools/KrakenAnimation/generate_kraken_atlases.py`
- Create: `tools/KrakenAnimation/validate_kraken_atlases.py`
- Create: `Content/NPCs/Kraken/KrakenBoss.png`
- Create: `Content/NPCs/Kraken/KrakenBossLoopBack.png`
- Create: `Content/NPCs/Kraken/KrakenBossMoveHorizontal.png`
- Create: `Content/NPCs/Kraken/KrakenBossMoveUp.png`

- [ ] **Step 1: Write failing atlas tests**

Assert:

```python
def test_generated_atlas_is_six_by_six_frames(self):
    atlas = generate_atlas("idle", self.rig)
    self.assertEqual((3840, 6144), atlas.size)

def test_all_frames_share_one_anchor(self):
    frames = generate_frames("idle", self.rig)
    anchors = [measure_body_anchor(frame) for frame in frames]
    self.assertLess(max_distance(anchors), 1.5)

def test_alpha_has_no_light_fringe(self):
    frame = generate_frames("idle", self.rig)[0]
    self.assertEqual([], find_light_transparent_fringe(frame))

def test_adjacent_frame_change_is_bounded(self):
    frames = generate_frames("idle", self.rig)
    self.assertLess(max_alpha_delta(frames), 0.18)
```

- [ ] **Step 2: Run atlas tests and confirm RED**

Expected: FAIL because composition and generation functions are absent.

- [ ] **Step 3: Implement layer composition**

Composite long rear tentacles, stable torso, long front tentacles, and central tentacles in a fixed order. Draw Ruby separately at its anchor. Use a 2x working canvas and downsample with Lanczos.

- [ ] **Step 4: Implement unified outline**

Build a silhouette mask from the composed alpha channel, expand it by the rig-defined radius, subtract the original mask, fill with `#071334`, and place the outlined sprite over it. Do not add glow or aura.

- [ ] **Step 5: Implement generation and validation CLIs**

`generate_kraken_atlases.py` writes four production PNGs, four GIF previews, and one comparison sheet. `validate_kraken_atlases.py` exits non-zero on invalid dimensions, dirty alpha, detached roots, unstable anchors, or loop discontinuity.

- [ ] **Step 6: Generate all production assets**

Run:

```powershell
python tools\KrakenAnimation\generate_kraken_atlases.py
python tools\KrakenAnimation\validate_kraken_atlases.py
```

Expected:

```text
idle: 36 frames valid
loop_back: 36 frames valid
horizontal: 36 frames valid
up: 36 frames valid
All Kraken atlases passed
```

- [ ] **Step 7: Run the full Python suite**

```powershell
python -m unittest discover -s tools\KrakenAnimation\tests -v
```

Expected: all tests pass.

- [ ] **Step 8: Commit generated atlases**

```powershell
git add tools/KrakenAnimation Content/NPCs/Kraken/KrakenBoss*.png
git commit -m "feat: rebuild Kraken animation atlases"
```

### Task 5: Integrate Stable Anchors in tModLoader

**Files:**
- Modify: `Content/NPCs/Kraken/KrakenBoss.cs`
- Modify: `tools/KrakenAnimation/tests/test_kraken_animation.py`

- [ ] **Step 1: Write a failing metadata consistency test**

Parse `KrakenBoss.cs` and assert the runtime constants match generated metadata:

```python
def test_runtime_uses_generated_frame_layout(self):
    constants = parse_kraken_constants(KRAKEN_BOSS_CS)
    self.assertEqual(36, constants["AnimationFrames"])
    self.assertEqual(6, constants["AtlasColumns"])
    self.assertEqual(6, constants["AtlasRows"])
```

Also assert the obsolete 36-element Ruby offset arrays no longer exist.

- [ ] **Step 2: Run test and confirm RED**

Expected: FAIL because the old per-frame Ruby arrays remain.

- [ ] **Step 3: Replace per-frame Ruby offsets with one stable anchor**

Remove `RubyFrameOffsetX` and `RubyFrameOffsetY`. Add a single `RubyFrameAnchor` from generated metadata and retain horizontal flipping and NPC rotation.

- [ ] **Step 4: Keep smooth rendering without changing combat**

Retain `SamplerState.LinearClamp`, cross-fading between motion atlases, life-dependent scale, attack state rendering, and collision logic. Do not alter AI or attack timings.

- [ ] **Step 5: Run Python tests**

Expected: all tests pass.

- [ ] **Step 6: Build the mod**

Run:

```powershell
dotnet build ChaoticDimensions.csproj
```

Expected: build succeeds with zero C# errors.

- [ ] **Step 7: Commit runtime integration**

```powershell
git add Content/NPCs/Kraken/KrakenBoss.cs tools/KrakenAnimation
git commit -m "fix: stabilize Kraken animation anchors"
```

### Task 6: Visual Verification and PAP Documentation

**Files:**
- Create: `tools/KrakenAnimation/output/kraken-idle.gif`
- Create: `tools/KrakenAnimation/output/kraken-motion-comparison.png`
- Modify: `Relatorio_PAP_Chaotic_Dimensions.docx` in the user's active ModSources copy

- [ ] **Step 1: Render visual evidence**

Generate GIFs at the game's idle, phase-two, and phase-three frame rates. Render a comparison sheet with frames `1, 7, 13, 19, 25, 31, 36`.

- [ ] **Step 2: Inspect every preview**

Confirm:

- no tentacle changes root;
- central tentacles never jump upward;
- head, torso, and tentacles use the same outline family;
- the Ruby remains fixed;
- horizontal and upward drag face the correct direction;
- frame 36 returns smoothly to frame 1.

- [ ] **Step 3: Copy the branch into the active ModSources checkout for playtest**

Copy only committed Kraken assets, tools, and `KrakenBoss.cs` into:

```text
C:\Users\unknown\Documents\My Games\Terraria\tModLoader\ModSources\ChaoticDimensions
```

Do not overwrite unrelated uncommitted files.

- [ ] **Step 4: Build from the active ModSources path**

Run:

```powershell
dotnet build ChaoticDimensions.csproj
```

Expected: build succeeds and packages the mod.

- [ ] **Step 5: Update the PAP report**

Add a concise Portuguese (Portugal) section explaining:

- the inconsistent lineart and changing tentacle topology;
- the stable-layer rig and continuous deformation solution;
- the four movement atlases;
- automated continuity and alpha validation;
- compilation and in-game verification;
- before/after images.

- [ ] **Step 6: Run final verification**

```powershell
python -m unittest discover -s tools\KrakenAnimation\tests -v
python tools\KrakenAnimation\validate_kraken_atlases.py
dotnet build ChaoticDimensions.csproj
git diff --check
git status --short
```

Expected: tests and validator pass, build succeeds, and `git diff --check` reports no whitespace errors.

- [ ] **Step 7: Commit documentation**

```powershell
git add Relatorio_PAP_Chaotic_Dimensions.docx
git commit -m "docs: record Kraken animation reconstruction"
```
