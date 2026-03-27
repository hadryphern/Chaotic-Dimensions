# OreSpawn Dimension Design Notes

This file is now a source-analysis and design-target document.

It is not a live implementation guide anymore.

## Current project status

As of `2026-03-27`:

- the old "dimension as world region" prototype is removed
- the temporary `SubworldLibrary` attempt is removed
- OreSpawn dimensions are currently disabled in runtime
- this document should be used only to preserve source behavior and design goals until the next architecture is chosen

## Scope that still matters

The four core dimensions we still care about are:

- `Utopia`
- `Danger`
- `Mining`
- `Crystal`

These two are postponed:

- `Village`
- `Chaos`

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

## Source behavior by dimension

### Utopia

Source-aligned notes:

- peaceful late-game dimension
- strongly tied to gigantic trees
- tied to `The King`
- important trees include the `Goodness Tree`, `Queen's Tree`, and `OMG Magic Apple Tree`

Design target for Terraria:

- mostly open surface
- very few trees, but each tree is absurdly huge
- giant lateral branches with chest routes built into the branches
- vanilla `Living Wood` and `Leaf Block` are acceptable for the tree mass
- chest rewards should focus on royal materials, rare OreSpawn loot, and rich mining bundles

### Danger

Source-aligned notes:

- challenge-heavy dimension
- dungeon-heavy identity
- dangerous exposure and hostile density
- associated with nightmare-family enemies and high-risk exploration

Design target for Terraria:

- broad flat world feel
- sparse vanilla-style trees
- structures should dominate the map instead of foliage
- very high dungeon density
- use mixed structure heights:
  - surface
  - elevated
  - buried
- this should be the main natural miniboss farming dimension

Recommended mob pressure:

- `Creeping Horror`
- `Terrible Terror`
- `Triffid`
- `Lurking Terror`
- `Cloud Shark`
- later `Kyuubi`
- later `Spider Robot`
- later `Robo Jeffery`

### Mining

Source-aligned notes:

- ore farming dimension
- structurally close to a normal world
- overloaded with resources
- linked to dinos and underground danger content

Design target for Terraria:

- vanilla-like layer structure
- heavy ore generation
- only OreSpawn fauna by default
- keep the terrain readable and practical to farm

Recommended mob lane:

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

### Crystal

Source-aligned notes:

- one of the most visually distinct dimensions
- crystal-heavy terrain identity
- crystal materials and crystal-adjacent mobs

Design target for Terraria:

- vanilla-like world structure
- full crystal tileset
- replace normal terrain families with crystal variants:
  - crystal grass
  - crystal dirt
  - crystal stone
  - crystal sand
  - crystal wood
  - crystal leaves
- restrict enemy pool hard so the dimension feels authored

Recommended mob lane:

- `Vortex`
- `Rotator`
- `Dungeon Beast`
- `Crystal Urchin`
- `Rat`
- `Mantis`
- `Irukandji`
- `Skate`

## Multiplayer requirements

Any future dimension architecture must respect these rules:

- server-owned travel, not client-only teleport gimmicks
- safe multiplayer syncing from day one
- shared progression flags must stay consistent
- travel items must work the same in singleplayer and multiplayer

## World size rule

The project still assumes `Large World` as the supported base size for the main world.

Target warning text:

- `Chaotic Dimensions dimension content is designed for Large worlds only. Tiny and Medium worlds are unsupported.`

## Travel item identity

The source fantasy still stays the same:

- ants are the main dimension travel lane
- termites connect to crystal-themed travel
- travel should remain item-driven, not menu-driven

## Next implementation rule

Do not treat anything in this file as a dependency instruction anymore.

The next architecture should be chosen from scratch with these priorities:

1. stable multiplayer
2. faithful visual identity
3. clean save behavior
4. predictable world generation
5. easy content expansion later

