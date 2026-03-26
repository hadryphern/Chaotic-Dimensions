# OreSpawn Sprite and Texture Size Guide

This is the current production guide for OreSpawn art inside Chaotic Dimensions.

Use it as the default unless a specific class later overrides the size.

## General rules

- NPCs and bosses currently use vertical sprite sheets.
- Default NPC sheet format:
  - `frame width x frame height`
  - `4 frames total`
  - total sheet size = `frame width x (frame height * 4)`
- Standard item icon:
  - usually `32x32`
- Small material icon:
  - usually `24x24`
- Standard placeable block tile:
  - `16x16`
- Standard armor inventory icon:
  - `32x32`

## Real working armor texture sizes in this mod

These sizes are already proven by the current Crystaline armor assets and are the safest reference for future equipped armor art.

- Head equip sheet: `40x1120`
- Legs equip sheet: `40x1120`
- Body equip sheet: `360x224`
- Arms equip sheet: `360x224`
- Female body equip sheet: `360x224`
- Armor item icon: `32x32`

Use these exact sizes for OreSpawn wearable armor unless we later decide to make a custom equipment pipeline.

## Bosses

- `Kraken`: frame `120x120`, total `120x480`
- `Mobzilla`: frame `154x154`, total `154x616`
- `Mothra`: frame `144x104`, total `144x416`
- `The King`: frame `160x128`, total `160x512`
- `The Queen`: frame `170x140`, total `170x560`
- `WTF?`: frame `132x132`, total `132x528`

## Mobs

### Hostile and elite mobs

- `Water Dragon`: frame `56x56`, total `56x224`
- `Mantis`: frame `48x48`, total `48x192`
- `Emperor Scorpion`: frame `64x64`, total `64x256`
- `Hercules`: frame `56x56`, total `56x224`
- `Caterkiller`: frame `64x48`, total `64x192`
- `Cephadrome`: frame `56x56`, total `56x224`
- `Alien`: frame `54x60`, total `54x240`
- `Attack Squid`: frame `46x46`, total `46x184`
- `Basilisc`: frame `52x40`, total `52x160`
- `Bomb-omb`: frame `32x32`, total `32x128`
- `Brutalfly`: frame `44x44`, total `44x176`
- `Cave Fisher`: frame `40x36`, total `40x144`
- `Creeping Horror`: frame `48x40`, total `48x160`
- `Crystal Urchin`: frame `36x34`, total `36x136`
- `Dragon`: frame `72x64`, total `72x256`
- `Dungeon Beast`: frame `60x52`, total `60x208`
- `Ender Knight`: frame `44x52`, total `44x208`
- `Ender Reaper`: frame `46x54`, total `46x216`
- `Ghost`: frame `34x42`, total `34x168`
- `Ghost Skeleton`: frame `38x48`, total `38x192`
- `Hydrolisc`: frame `54x42`, total `54x168`
- `Jumpy Bug`: frame `30x28`, total `30x112`
- `Kyuubi`: frame `64x56`, total `64x224`
- `Leaf Monster`: frame `40x44`, total `40x176`
- `Lurking Terror`: frame `48x40`, total `48x160`
- `Molenoid`: frame `36x32`, total `36x128`
- `Mosquito`: frame `28x28`, total `28x112`
- `Red Ant Robot`: frame `50x56`, total `50x224`
- `Robo Gunner`: frame `44x52`, total `44x208`
- `Robo Jeffery`: frame `44x52`, total `44x208`
- `Robo Pounder`: frame `54x58`, total `54x232`
- `Robo Sniper`: frame `42x50`, total `42x200`
- `Robo Warrior`: frame `56x60`, total `56x240`
- `Rotator`: frame `42x42`, total `42x168`
- `Scorpion`: frame `38x30`, total `38x120`
- `Sea Monster`: frame `60x52`, total `60x208`
- `Sea Viper`: frame `54x48`, total `54x192`
- `Spider Driver`: frame `44x48`, total `44x192`
- `Spider Robot`: frame `50x44`, total `50x176`
- `Spit Bug`: frame `30x28`, total `30x112`
- `Stink Bug`: frame `30x28`, total `30x112`
- `Terrible Terror`: frame `48x44`, total `48x176`
- `Triffid`: frame `46x54`, total `46x216`
- `Vortex`: frame `52x52`, total `52x208`

### Dinosaurs and large beasts

- `Alosaurus`: frame `60x42`, total `60x168`
- `Baby Dragon`: frame `40x40`, total `40x160`
- `Baryonyx`: frame `56x40`, total `56x160`
- `Camarasaurus`: frame `70x54`, total `70x216`
- `Cryolophosaurus`: frame `60x44`, total `60x176`
- `Leonopteryx`: frame `64x56`, total `64x224`
- `Nastysaurus`: frame `64x48`, total `64x192`
- `Pointysaurus`: frame `60x44`, total `60x176`
- `T-Rex`: frame `74x58`, total `74x232`

### Ambient, utility, and social mobs

- `Beaver`: frame `28x24`, total `28x96`
- `Bee`: frame `24x22`, total `24x88`
- `Cassowary`: frame `30x34`, total `30x136`
- `Chipmunk`: frame `22x20`, total `22x80`
- `Cliff Racer`: frame `30x26`, total `30x104`
- `Cloud Shark`: frame `44x32`, total `44x128`
- `Crab`: frame `24x18`, total `24x72`
- `Cricket`: frame `22x18`, total `22x72`
- `Easter Bunny`: frame `24x24`, total `24x96`
- `Fairy`: frame `28x30`, total `28x120`
- `Firefly`: frame `20x20`, total `20x80`
- `Flounder`: frame `28x18`, total `28x72`
- `Frog`: frame `22x18`, total `22x72`
- `Gazelle`: frame `30x24`, total `30x96`
- `Gold Fish`: frame `20x16`, total `20x64`
- `Hammerhead`: frame `40x24`, total `40x96`
- `Irukandji`: frame `22x22`, total `22x88`
- `Lizard`: frame `26x18`, total `26x72`
- `Ostrich`: frame `34x42`, total `34x168`
- `Peacock`: frame `28x34`, total `28x136`
- `Rat`: frame `20x14`, total `20x56`
- `Rubber Ducky`: frame `20x18`, total `20x72`
- `Skate`: frame `28x16`, total `28x64`
- `Stinky`: frame `20x20`, total `20x80`
- `Termite`: frame `16x16`, total `16x64`
- `Velocity Raptor`: frame `34x30`, total `34x120`
- `Whale`: frame `72x34`, total `72x136`
- `Girlfriend`: frame `28x48`, total `28x192`
- `Boyfriend`: frame `28x48`, total `28x192`
- `The Prince`: frame `64x56`, total `64x224`
- `Young Prince`: frame `72x64`, total `72x256`
- `Young Adult Prince`: frame `96x80`, total `96x320`
- `The Princess`: frame `72x64`, total `72x256`

### Transport critters

- `Brown Ant Critter`: frame `18x18`, total `18x72`
- `Rainbow Ant Critter`: frame `18x18`, total `18x72`
- `Red Ant Critter`: frame `18x18`, total `18x72`
- `Unstable Ant Critter`: frame `18x18`, total `18x72`
- `Termite Critter`: frame `18x18`, total `18x72`
- `Butterfly Critter`: frame `20x20`, total `20x80`

## Unique item icons

- `Attitude Adjuster`: `40x40`
- `Battle Axe`: `42x42`
- `Big Bertha Sword`: `54x54`
- `Big Hammer`: `40x40`
- `Chainsaw`: `44x44`
- `Experience Sword`: `40x40`
- `Fairy Sword`: `34x34`
- `Mantis Claw`: `34x34`
- `Nightmare Sword`: `46x46`
- `Poison Sword`: `38x38`
- `Queen Scale Battle Axe`: `50x50`
- `Rat Sword`: `32x32`
- `Ray Gun`: `38x38`
- `Royal Guardian Sword`: `54x54`
- `Skate Bow & Irukandji Arrows`: `42x42`
- `Slice Sword`: `40x40`
- `Squidzooka`: `44x44`
- `Thunder Staff`: `42x42`

## Summon and transport item icons

- miniboss summon items: `32x32`
- boss summon items: `32x32`
- captured ant, termite, and butterfly transport items: `24x24`

## Material and resource item icons

Use these defaults unless a specific icon needs more room.

- mob drop materials: `24x24`
- gems and ore chunks: `24x24`
- ingots and nuggets: `24x24`
- special crystals: `24x24`
- log items: `24x24`
- nest block items: `24x24`
- placeable block items with complex silhouette: `32x32`

Current OreSpawn resource item sizes to follow:

- `Amethyst`, `Ruby`, `PinkTourmaline`, `TigersEye`, `Kyanite`, `Salt`: `24x24`
- `TitaniumOre`, `UraniumOre`: `24x24`
- `TitaniumNugget`, `TitaniumIngot`, `UraniumNugget`, `UraniumIngot`: `24x24`
- `FairyCrystal`, `RatCrystal`, `LavaFoam`, `MobzillaScale`: `24x24`
- `WaterDragonScale`, `EmperorScorpionScale`, `CaterkillerJaw`, `CephadromeHorn`: `24x24`

## Armor item icons

Use:

- helmet item icon: `32x32`
- chestplate item icon: `32x32`
- greaves item icon: `32x32`

For oversized endgame sets, `40x40` is acceptable, but keep all three pieces of the same set consistent.

## Accessory item icons

Use:

- normal accessory icon: `32x32`
- oversized relic-like accessory: `36x36`

If the accessory does not visibly render on the player, no extra wearable sheet is needed.

## Block, tile, and world texture sizes

These are the most important additions for the dimension rebuild.

### Single-tile world blocks

Each block texture should be drawn as:

- `16x16`

This applies to:

- crystal dirt
- crystal grass
- crystal stone
- crystal sand
- crystal hardened sand
- crystal ice
- crystal wood
- crystal leaves
- crystal ore blocks
- ore blocks for `Ruby`, `Pink Tourmaline`, `Tigers Eye`, `Kyanite`, `Titanium`, `Uranium`, `Salt`
- `Molenoid Dirt`
- `Red Ant Nest`
- `Termite Nest`
- `Teleport Block`
- `Ender Pearl Block`
- `Eye of Ender Block`

### Trees using vanilla materials

For `Utopia`, `Danger`, and `Mining`:

- no custom tree tile sprites are required if we stay on vanilla `Living Wood` and `Leaf Block`
- the important work there is structure generation, not new tile art

### Crystal tree family

If we replace vanilla tree visuals in the Crystal dimension with custom crystal tiles:

- crystal trunk tile: `16x16`
- crystal leaf tile: `16x16`
- crystal branch tile variants: `16x16` each
- optional glow overlays: `16x16`

## Armor family production defaults

Use these as the starting sizes for the OreSpawn armor lanes that still need art.

- early and midgame armor item icons: `32x32`
- late and endgame armor item icons: `40x40`
- all head equip sheets: `40x1120`
- all leg equip sheets: `40x1120`
- all body equip sheets: `360x224`
- all arm equip sheets: `360x224`
- all female body equip sheets: `360x224`

Suggested icon scale by set:

- `Amethyst Armor`: `32x32`
- `Emerald Armor`: `32x32`
- `Lapis Armor`: `32x32`
- `Ruby Armor`: `32x32`
- `Pink Tourmaline Armor`: `32x32`
- `Tigers Eye Armor`: `32x32`
- `Moth Scale Armor`: `32x32`
- `Peacock Feather Armor`: `32x32`
- `Lava Eel Armor`: `32x32`
- `Experience Armor`: `32x32`
- `Mobzilla Armor`: `40x40`
- `Queen Scale Armor`: `40x40`
- `Royal Guardian Armor`: `40x40`
- `Ultimate Armor`: `40x40`

## Tool and weapon family production defaults

- small tool icons: `36x36`
- standard weapon icons: `40x40`
- large late-game weapon icons: `42x42` to `54x54`
- giant endgame swords and axes: up to `64x64` if needed

Apply this to:

- `Amethyst Tools and Weapons`
- `Emerald Tools and Weapons`
- `Ruby Tools and Weapons`
- `Pink Tourmaline Tools and Weapons`
- `Tigers Eye Tools and Weapons`
- `Kyanite Tools and Weapons`
- `Wood Crystal Tools and Weapons`
- `Ultimate Tools and Weapons`

## Food, plants, and eggs

- food icons: `26x26` to `32x32`
- seeds and small plants: `24x24`
- fruit icons: `32x32`
- special saplings: `32x32` to `40x40`
- ancient dried eggs: `32x32`

## Dimension production summary

If you only need the short version:

- NPC frame art: use the exact sizes listed above
- item icons: `24x24`, `32x32`, or `40x40` depending on class
- block textures: `16x16`
- armor item icons: `32x32`
- armor wearable sheets:
  - head `40x1120`
  - legs `40x1120`
  - body `360x224`
  - arms `360x224`
  - female body `360x224`
