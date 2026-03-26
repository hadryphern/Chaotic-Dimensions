# OreSpawn Dimension Rebuild

This document replaces the old "dimension regions inside one large world" prototype as the long-term direction for OreSpawn inside Chaotic Dimensions.

## Current conclusion

- The current in-world-region prototype is useful only as a throwaway test bed.
- It is not faithful enough for `Utopia`, `Danger`, `Mining`, or `Crystal`.
- The correct long-term solution is to move those dimensions into real subworlds.
- `Village` and `Chaos` are postponed and do not need to block the first rebuild pass.

## Sources reviewed

- [Dimensions](https://shrekleaker.github.io/orespawn.com/dimensions.html)
- [Utopia Dimension](https://shrekleaker.github.io/orespawn.com/utopia-dimension.html)
- [Danger Dimension](https://shrekleaker.github.io/orespawn.com/danger-dimension.html)
- [Mining Dimension](https://shrekleaker.github.io/orespawn.com/mining-dimension.html)
- [Crystal Dimension](https://shrekleaker.github.io/orespawn.com/crystal-dimension.html)
- [Dungeons](https://shrekleaker.github.io/orespawn.com/dungeons.html)
- [Ants](https://shrekleaker.github.io/orespawn.com/ants.html)
- [Materials](https://shrekleaker.github.io/orespawn.com/materials.html)
- [Plants and Trees](https://shrekleaker.github.io/orespawn.com/plants-and-trees.html)
- [Subworld Library README](https://github.com/jjohnsnaill/SubworldLibrary)
- [Subworld Library Repository](https://github.com/jjohnsnaill/SubworldLibrary)

## Source behavior by dimension

### Utopia

Source-aligned notes:

- Utopia is a peaceful late-game dimension.
- It is strongly tied to gigantic trees and the royal content path.
- `The King` is the main apex presence there.
- The important landmarks are the `Goodness Tree`, `Queen's Tree`, and `OMG Magic Apple Tree`.

Terraria adaptation:

- Make Utopia a dedicated surface-heavy subworld.
- Use vanilla `Living Wood` and `Leaf Block` for the actual tree mass, exactly as requested.
- The world should be mostly open surface with only a handful of absurdly large trees.
- Every major tree needs thick lateral branch systems with chest pads built into branches.
- Rich chest loot should focus on high-tier OreSpawn materials, ore bundles, royal progression materials, and rare utility items.

Inference:

- The wiki does not define exact Terraria-style chest placement rules, so branch chest clusters are a Terraria adaptation chosen to preserve the "mega-tree treasure world" identity.

### Danger

Source-aligned notes:

- Danger is a challenge dimension.
- The wiki highlights nightmare-family threats and dungeon-heavy danger content.
- Named dungeon groups include mini dungeons, challenge dungeons, and cloud-shark dungeons.
- The dimension is associated with dangerous falls, open exposure, and concentrated hostile content.
- The page lists dangerous mobs such as `Nightmares`, `Terrible Terror`, `Creeping Horror`, `Triffid`, `Lurking Terror`, and `Cloud Shark`.

Terraria adaptation:

- Make Danger a dedicated flat subworld, not a sky strip inside the main world.
- Keep the ground mostly plain and open.
- Use vanilla wood and leaves for the sparse trees, with low density so structures dominate the map.
- Push dungeon spawn density extremely high.
- Mix surface structures, elevated structures, and underground inserts.
- Treat this as the main natural miniboss farming dimension.

Recommended hostile pool:

- `Creeping Horror`
- `Terrible Terror`
- `Triffid`
- `Lurking Terror`
- `Cloud Shark`
- later `Kyuubi`
- later `Spider Robot`
- later `Robo Jeffery`

Recommended structure pool:

- mini dungeons
- challenge dungeons
- cloud-shark towers or cloud-shark nests
- basilisc-style lairs
- miniboss arenas
- chest bunkers

Inference:

- The original page emphasizes dangerous sky-style geography and challenge density.
- Your requested flat version is an adaptation choice, not a direct one-to-one copy.
- That adaptation still fits OreSpawn better than the current "one shared large world" prototype.

### Mining

Source-aligned notes:

- Mining is the ore-heavy farming dimension.
- It is close to a normal survival world structurally, but overpacked with resources.
- The wiki associates it with dinos, underground threat mobs, and a large number of resource opportunities.
- Dungeon groups mentioned there include alien/WTF content, basilisc lairs, bee hives, ender knight outposts, and other specialty structures.

Terraria adaptation:

- Make Mining a vanilla-shaped subworld with normal surface, underground, cavern, and underworld layers.
- Spawn only OreSpawn fauna there, plus vanilla critters only if absolutely needed for gameplay stability.
- Increase ore generation hard compared to vanilla.
- Keep the terrain readable so it still feels like a "mining world", not a chaos world.

Recommended mob pool:

- `Alien`
- `Cave Fisher`
- `Alosaurus`
- `Baryonyx`
- `Camarasaurus`
- `Cryolophosaurus`
- `Leonopteryx`
- `Nastysaurus`
- `Pointysaurus`
- `T-Rex`
- `Baby Dragon`
- `Velocity Raptor`
- `Mothra`
- `WTF?`

Recommended structure pool:

- alien/WTF dungeon
- basilisc lair
- bee hive cluster
- ender knight outpost
- shadow dungeon
- kyuubi arena

### Crystal

Source-aligned notes:

- Crystal is one of the most visually distinct OreSpawn dimensions.
- The wiki presents it as a crystal-heavy world with dungeon depth and specific crystal-themed mobs.
- It also ties it to strong materials and late exploration value.
- The page lists mobs such as `Vortex`, `Skate`, `Irukandji`, `Rotator`, `Dungeon Beast`, `Crystal Urchin`, `Rat`, and `Mantis`.

Terraria adaptation:

- Make Crystal a vanilla-structured subworld, but with a full crystal tileset.
- Replace normal-looking terrain families with crystal variants:
  - crystal grass
  - crystal dirt
  - crystal stone
  - crystal sand
  - crystal ice if needed
  - crystal wood
  - crystal leaves
- Keep the world readable like vanilla, but make every major material visually crystal-themed.
- Restrict enemy pool hard so the dimension feels clean and authored.

Recommended hostile pool:

- `Vortex`
- `Rotator`
- `Dungeon Beast`
- `Crystal Urchin`
- `Rat`
- `Mantis`
- `Irukandji`
- `Skate`

Recommended friendly/ambient pool:

- `Fairy`
- `Whale`
- `Peacock`
- `Flounder`
- `Termite`

Recommended structure pool:

- crystal dungeon maze
- rotator charging station
- crystal cave halls
- crystal chest pockets

Inference:

- The wiki does not literally define every Terraria block family replacement.
- The "vanilla world but every terrain family is crystal-styled" direction is a faithful Terraria adaptation of the source theme and your requested art direction.

## Dimensions postponed for now

- `Village Dimension`
- `Chaos Dimension`

They can return later, but they should not block the rebuild of the four core dimensions above.

## Recommended subworld sizes

These are Terraria-facing design recommendations, not source facts.

- `Utopia`: `5200x1600`
- `Danger`: `5600x1400`
- `Mining`: `4200x2000`
- `Crystal`: `4200x1900`

Reasoning:

- `Utopia` needs horizontal room for branch spread and chest routing.
- `Danger` needs a long dungeon belt.
- `Mining` and `Crystal` need enough depth to feel like full worlds without wasting generation budget.

## Multiplayer rules

- Keep dimension travel item-driven.
- Never use client-only teleports for real subworld travel.
- Server should own the transfer request.
- Shared progression flags must be copied between worlds.
- Downed boss state, unlock state, and event unlocks should be copied from main world into the subworld before generation/load.

## Subworld Library setup

Confirmed from the library source:

- The base class is `Subworld`.
- Entry is handled by `SubworldSystem.Enter<T>()`.
- Exit is handled by `SubworldSystem.Exit()`.
- Shared world state can be passed with `SubworldSystem.CopyWorldData(...)` and `SubworldSystem.ReadCopiedWorldData<T>(...)`.
- The library handles multiplayer by spinning up subservers for occupied subworlds.

### Dependency steps

1. Install `Subworld Library` in tModLoader.
2. Add a hard dependency to `build.txt`:

```txt
modReferences = SubworldLibrary
```

3. In code, add:

```cs
using SubworldLibrary;
```

4. Create one class per real OreSpawn dimension:

```cs
using System.Collections.Generic;
using Terraria.WorldBuilding;
using SubworldLibrary;

public sealed class DangerSubworld : Subworld
{
	public override int Width => 5600;
	public override int Height => 1400;
	public override bool ShouldSave => true;
	public override List<GenPass> Tasks => new()
	{
		// worldgen passes here
	};
}
```

5. Travel item usage should call:

```cs
SubworldSystem.Enter<DangerSubworld>();
```

6. Returning should call:

```cs
SubworldSystem.Exit();
```

### Important note

- I did not wire `SubworldLibrary` into the mod yet in this pass because that would force a live dependency before we convert the current architecture.
- The next safe step is to install the library first, then migrate one dimension at a time starting with `Danger`.

## World size warning

The existing runtime warning when entering a non-large world can stay until subworld migration is complete.

For the exact "show warning while selecting world size" behavior:

- that should be implemented as a menu/UI hook, not as a worldgen rule.
- I have not finished that hook yet in this pass.
- once subworld migration starts, the warning text should become:
  - `Chaotic Dimensions dimension content is designed for Large worlds only. Tiny and Medium worlds are unsupported.`

## Recommended migration order

1. `Danger`
2. `Utopia`
3. `Mining`
4. `Crystal`
5. travel item rewrite
6. shared progression state copy
7. structure and loot polish

## Final direction

The faithful route is:

- real subworlds
- item-driven entry with ants and termites
- authored mob pools per dimension
- authored structure pools per dimension
- no more "shared main world regions" for final OreSpawn content
