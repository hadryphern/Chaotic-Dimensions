# OreSpawn Sprite Sheet, File Name, and Asset ID Guide

This is the current art production guide for OreSpawn and related Chaotic Dimensions assets.

Use this file when deciding:

- what the `.png` file must be called
- which folder it belongs in
- what pixel size it should use
- which armor suffixes are required

## Naming rule that matters most

For almost every asset in this mod:

- the file name must match the class name exactly
- use `PascalCase`
- do not use spaces
- do not use snake_case
- do not invent custom IDs like `crystaline_taltal`

Correct examples:

- `WaterDragon.png`
- `Kraken.png`
- `BigBerthaSword.png`
- `CrystalineDevourBreastplate_Body.png`

Wrong examples:

- `water_dragon.png`
- `kraken_sheet.png`
- `big bertha sword.png`
- `crystaline_taltal.png`

## Default asset path rules

- OreSpawn bosses: `Content/Bosses/OreSpawn/`
- OreSpawn mobs and critters: `Content/NPCs/OreSpawn/`
- OreSpawn unique items and transport items: `Content/Items/OreSpawn/`
- OreSpawn summon items: `Content/Items/Summons/OreSpawn/`
- OreSpawn materials and placeable item icons: `Content/Items/Materials/OreSpawn/`
- OreSpawn world tiles: `Content/Tiles/OreSpawn/`

## General size rules

- NPCs and bosses use vertical sheets
- default NPC sheet format:
  - frame size = `width x height`
  - total size = `width x (height * 4)`
- normal item icon:
  - usually `32x32`
- small material icon:
  - usually `24x24`
- normal tile texture:
  - `16x16`

## Armor naming and required files

For armor, the item icon and the equipped sheets are separate files.

Helmet:

- item icon: `YourHelmet.png`
- equipped sheet: `YourHelmet_Head.png`

Chestplate:

- item icon: `YourChestplate.png`
- body sheet: `YourChestplate_Body.png`
- arms sheet: `YourChestplate_Arms.png`
- female body sheet: `YourChestplate_FemaleBody.png`

Legs:

- item icon: `YourGreaves.png`
- leg sheet: `YourGreaves_Legs.png`

## Real working armor sizes already proven in this mod

- head equip sheet: `40x1120`
- legs equip sheet: `40x1120`
- body equip sheet: `360x224`
- arms equip sheet: `360x224`
- female body equip sheet: `360x224`
- armor item icon: `32x32`

## OreSpawn Bosses

Folder:

- `Content/Bosses/OreSpawn/`

| Display name | File / asset ID | Frame size | Total sheet |
| --- | --- | --- | --- |
| Kraken | `Kraken.png` | `120x120` | `120x480` |
| Mobzilla | `Mobzilla.png` | `154x154` | `154x616` |
| Mothra | `Mothra.png` | `144x104` | `144x416` |
| The King | `TheKing.png` | `160x128` | `160x512` |
| The Queen | `TheQueen.png` | `170x140` | `170x560` |
| WTF? | `Wtf.png` | `132x132` | `132x528` |

## OreSpawn Mobs

Folder:

- `Content/NPCs/OreSpawn/`

### Hostile and elite mobs

| Display name | File / asset ID | Frame size | Total sheet |
| --- | --- | --- | --- |
| Water Dragon | `WaterDragon.png` | `56x56` | `56x224` |
| Mantis | `Mantis.png` | `48x48` | `48x192` |
| Emperor Scorpion | `EmperorScorpion.png` | `64x64` | `64x256` |
| Hercules | `Hercules.png` | `56x56` | `56x224` |
| Caterkiller | `Caterkiller.png` | `64x48` | `64x192` |
| Cephadrome | `Cephadrome.png` | `56x56` | `56x224` |
| Alien | `Alien.png` | `54x60` | `54x240` |
| Attack Squid | `AttackSquid.png` | `46x46` | `46x184` |
| Basilisc | `Basilisc.png` | `52x40` | `52x160` |
| Bomb-omb | `BombOmb.png` | `32x32` | `32x128` |
| Brutalfly | `Brutalfly.png` | `44x44` | `44x176` |
| Cave Fisher | `CaveFisher.png` | `40x36` | `40x144` |
| Creeping Horror | `CreepingHorror.png` | `48x40` | `48x160` |
| Crystal Urchin | `CrystalUrchin.png` | `36x34` | `36x136` |
| Dragon | `Dragon.png` | `72x64` | `72x256` |
| Dungeon Beast | `DungeonBeast.png` | `60x52` | `60x208` |
| Ender Knight | `EnderKnight.png` | `44x52` | `44x208` |
| Ender Reaper | `EnderReaper.png` | `46x54` | `46x216` |
| Ghost | `Ghost.png` | `34x42` | `34x168` |
| Ghost Skeleton | `GhostSkeleton.png` | `38x48` | `38x192` |
| Hydrolisc | `Hydrolisc.png` | `54x42` | `54x168` |
| Jumpy Bug | `JumpyBug.png` | `30x28` | `30x112` |
| Kyuubi | `Kyuubi.png` | `64x56` | `64x224` |
| Leaf Monster | `LeafMonster.png` | `40x44` | `40x176` |
| Lurking Terror | `LurkingTerror.png` | `48x40` | `48x160` |
| Molenoid | `Molenoid.png` | `36x32` | `36x128` |
| Mosquito | `Mosquito.png` | `28x28` | `28x112` |
| Red Ant Robot | `RedAntRobot.png` | `50x56` | `50x224` |
| Robo Gunner | `RoboGunner.png` | `44x52` | `44x208` |
| Robo Jeffery | `RoboJeffery.png` | `44x52` | `44x208` |
| Robo Pounder | `RoboPounder.png` | `54x58` | `54x232` |
| Robo Sniper | `RoboSniper.png` | `42x50` | `42x200` |
| Robo Warrior | `RoboWarrior.png` | `56x60` | `56x240` |
| Rotator | `Rotator.png` | `42x42` | `42x168` |
| Scorpion | `Scorpion.png` | `38x30` | `38x120` |
| Sea Monster | `SeaMonster.png` | `60x52` | `60x208` |
| Sea Viper | `SeaViper.png` | `54x48` | `54x192` |
| Spider Driver | `SpiderDriver.png` | `44x48` | `44x192` |
| Spider Robot | `SpiderRobot.png` | `50x44` | `50x176` |
| Spit Bug | `SpitBug.png` | `30x28` | `30x112` |
| Stink Bug | `StinkBug.png` | `30x28` | `30x112` |
| Terrible Terror | `TerribleTerror.png` | `48x44` | `48x176` |
| Triffid | `Triffid.png` | `46x54` | `46x216` |
| Vortex | `Vortex.png` | `52x52` | `52x208` |

### Dinosaurs and large beasts

| Display name | File / asset ID | Frame size | Total sheet |
| --- | --- | --- | --- |
| Alosaurus | `Alosaurus.png` | `60x42` | `60x168` |
| Baby Dragon | `BabyDragon.png` | `40x40` | `40x160` |
| Baryonyx | `Baryonyx.png` | `56x40` | `56x160` |
| Camarasaurus | `Camarasaurus.png` | `70x54` | `70x216` |
| Cryolophosaurus | `Cryolophosaurus.png` | `60x44` | `60x176` |
| Leonopteryx | `Leonopteryx.png` | `64x56` | `64x224` |
| Nastysaurus | `Nastysaurus.png` | `64x48` | `64x192` |
| Pointysaurus | `Pointysaurus.png` | `60x44` | `60x176` |
| T-Rex | `TRex.png` | `74x58` | `74x232` |

### Ambient, social, and utility mobs

| Display name | File / asset ID | Frame size | Total sheet |
| --- | --- | --- | --- |
| Beaver | `Beaver.png` | `28x24` | `28x96` |
| Bee | `Bee.png` | `24x22` | `24x88` |
| Cassowary | `Cassowary.png` | `30x34` | `30x136` |
| Chipmunk | `Chipmunk.png` | `22x20` | `22x80` |
| Cliff Racer | `CliffRacer.png` | `30x26` | `30x104` |
| Cloud Shark | `CloudShark.png` | `44x32` | `44x128` |
| Crab | `Crab.png` | `24x18` | `24x72` |
| Cricket | `Cricket.png` | `22x18` | `22x72` |
| Easter Bunny | `EasterBunny.png` | `24x24` | `24x96` |
| Fairy | `Fairy.png` | `28x30` | `28x120` |
| Firefly | `Firefly.png` | `20x20` | `20x80` |
| Flounder | `Flounder.png` | `28x18` | `28x72` |
| Frog | `Frog.png` | `22x18` | `22x72` |
| Gazelle | `Gazelle.png` | `30x24` | `30x96` |
| Gold Fish | `GoldFish.png` | `20x16` | `20x64` |
| Hammerhead | `Hammerhead.png` | `40x24` | `40x96` |
| Irukandji | `Irukandji.png` | `22x22` | `22x88` |
| Lizard | `Lizard.png` | `26x18` | `26x72` |
| Ostrich | `Ostrich.png` | `34x42` | `34x168` |
| Peacock | `Peacock.png` | `28x34` | `28x136` |
| Rat | `Rat.png` | `20x14` | `20x56` |
| Rubber Ducky | `RubberDucky.png` | `20x18` | `20x72` |
| Skate | `Skate.png` | `28x16` | `28x64` |
| Stinky | `Stinky.png` | `20x20` | `20x80` |
| Termite | `Termite.png` | `16x16` | `16x64` |
| Velocity Raptor | `VelocityRaptor.png` | `34x30` | `34x120` |
| Whale | `Whale.png` | `72x34` | `72x136` |
| Girlfriend | `Girlfriend.png` | `28x48` | `28x192` |
| Boyfriend | `Boyfriend.png` | `28x48` | `28x192` |
| The Prince | `ThePrince.png` | `64x56` | `64x224` |
| Young Prince | `YoungPrince.png` | `72x64` | `72x256` |
| Young Adult Prince | `YoungAdultPrince.png` | `96x80` | `96x320` |
| The Princess | `ThePrincess.png` | `72x64` | `72x256` |

### Transport critters

| Display name | File / asset ID | Frame size | Total sheet |
| --- | --- | --- | --- |
| Brown Ant Critter | `BrownAntCritter.png` | `18x18` | `18x72` |
| Rainbow Ant Critter | `RainbowAntCritter.png` | `18x18` | `18x72` |
| Red Ant Critter | `RedAntCritter.png` | `18x18` | `18x72` |
| Unstable Ant Critter | `UnstableAntCritter.png` | `18x18` | `18x72` |
| Termite Critter | `TermiteCritter.png` | `18x18` | `18x72` |
| Butterfly Critter | `ButterflyCritter.png` | `20x20` | `20x80` |

## Unique OreSpawn item icons

Folder:

- `Content/Items/OreSpawn/`

| Display name | File / asset ID | Target size |
| --- | --- | --- |
| Attitude Adjuster | `AttitudeAdjuster.png` | `40x40` |
| Battle Axe | `BattleAxe.png` | `42x42` |
| Big Bertha Sword | `BigBerthaSword.png` | `54x54` |
| Chainsaw | `Chainsaw.png` | `44x44` |
| Experience Sword | `ExperienceSword.png` | `40x40` |
| Fairy Sword | `FairySword.png` | `34x34` |
| Nightmare Sword | `NightmareSword.png` | `46x46` |
| Poison Sword | `PoisonSword.png` | `38x38` |
| Queen Scale Battle Axe | `QueenScaleBattleAxe.png` | `50x50` |
| Rat Sword | `RatSword.png` | `32x32` |
| Ray Gun | `RayGun.png` | `38x38` |
| Royal Guardian Sword | `RoyalGuardianSword.png` | `54x54` |
| Skate Bow and Irukandji Arrows | `SkateBowAndIrukandjiArrows.png` | `42x42` |
| Slice Sword | `SliceSword.png` | `40x40` |
| Squidzooka | `Squidzooka.png` | `44x44` |
| Thunder Staff | `ThunderStaff.png` | `42x42` |

## Existing non-OreSpawn weapon file in this mod

This one is not in `Content/Items/OreSpawn/`, but it matters for naming examples:

| Display name | File / asset ID | Folder | Target size |
| --- | --- | --- | --- |
| Big Hammer | `BigHammer.png` | `Content/Items/Weapons/Melee/` | `40x40` |

## Summon and transport item icons

### OreSpawn summon items

Folder:

- `Content/Items/Summons/OreSpawn/`

All of these should use `32x32` unless a future redesign needs more room.

| Display name | File / asset ID | Target size |
| --- | --- | --- |
| Caterkiller Bait | `CaterkillerBait.png` | `32x32` |
| Cephadrome Caller | `CephadromeCaller.png` | `32x32` |
| Emperor Scorpion Idol | `EmperorScorpionIdol.png` | `32x32` |
| Hercules Totem | `HerculesTotem.png` | `32x32` |
| Kings Emblem | `KingsEmblem.png` | `32x32` |
| Kraken Beacon | `KrakenBeacon.png` | `32x32` |
| Mobzilla Signal | `MobzillaSignal.png` | `32x32` |
| Mothra Totem | `MothraTotem.png` | `32x32` |
| Queens Bloom | `QueensBloom.png` | `32x32` |
| Wtf Signal | `WtfSignal.png` | `32x32` |

### OreSpawn transport items

Folder:

- `Content/Items/OreSpawn/`

All of these should use `24x24`.

| Display name | File / asset ID | Target size |
| --- | --- | --- |
| Brown Ant Item | `BrownAntItem.png` | `24x24` |
| Rainbow Ant Item | `RainbowAntItem.png` | `24x24` |
| Red Ant Item | `RedAntItem.png` | `24x24` |
| Unstable Ant Item | `UnstableAntItem.png` | `24x24` |
| Termite Item | `TermiteItem.png` | `24x24` |
| Butterfly Transport Item | `ButterflyTransportItem.png` | `24x24` |

## Materials, drops, and placeable item icons

Folder:

- `Content/Items/Materials/OreSpawn/`

Normal rule:

- small material icon: `24x24`
- more complex placeable block icon: `32x32`

| Display name | File / asset ID | Target size |
| --- | --- | --- |
| Amethyst | `Amethyst.png` | `24x24` |
| Ruby | `Ruby.png` | `24x24` |
| Pink Tourmaline | `PinkTourmaline.png` | `24x24` |
| Tigers Eye | `TigersEye.png` | `24x24` |
| Kyanite | `Kyanite.png` | `24x24` |
| Salt | `Salt.png` | `24x24` |
| Titanium Ore | `TitaniumOre.png` | `24x24` |
| Uranium Ore | `UraniumOre.png` | `24x24` |
| Titanium Nugget | `TitaniumNugget.png` | `24x24` |
| Titanium Ingot | `TitaniumIngot.png` | `24x24` |
| Uranium Nugget | `UraniumNugget.png` | `24x24` |
| Uranium Ingot | `UraniumIngot.png` | `24x24` |
| Fairy Crystal | `FairyCrystal.png` | `24x24` |
| Rat Crystal | `RatCrystal.png` | `24x24` |
| Lava Foam | `LavaFoam.png` | `24x24` |
| Mobzilla Scale | `MobzillaScale.png` | `24x24` |
| Water Dragon Scale | `WaterDragonScale.png` | `24x24` |
| Emperor Scorpion Scale | `EmperorScorpionScale.png` | `24x24` |
| Caterkiller Jaw | `CaterkillerJaw.png` | `24x24` |
| Cephadrome Horn | `CephadromeHorn.png` | `24x24` |
| Mantis Claw | `MantisClaw.png` | `24x24` |
| Amethyst Block item | `AmethystBlock.png` | `32x32` |
| Ruby Block item | `RubyBlock.png` | `32x32` |
| Pink Tourmaline Block item | `PinkTourmalineBlock.png` | `32x32` |
| Tigers Eye Block item | `TigersEyeBlock.png` | `32x32` |
| Kyanite Block item | `KyaniteBlock.png` | `32x32` |
| Salt Block item | `SaltBlock.png` | `32x32` |
| Titanium Block item | `TitaniumBlock.png` | `32x32` |
| Uranium Block item | `UraniumBlock.png` | `32x32` |
| Molenoid Dirt item | `MolenoidDirt.png` | `24x24` |
| Red Ant Nest item | `RedAntNest.png` | `24x24` |
| Termite Nest item | `TermiteNest.png` | `24x24` |
| Crystal Tree Log item | `CrystalTreeLog.png` | `24x24` |
| Sky Tree Log item | `SkyTreeLog.png` | `24x24` |
| Duplicator Log item | `DuplicatorLog.png` | `24x24` |
| Teleport Block item | `TeleportBlock.png` | `32x32` |
| Ender Pearl Block item | `EnderPearlBlock.png` | `32x32` |
| Eye of Ender Block item | `EyeOfEnderBlock.png` | `32x32` |

## World tiles and block textures

Folder:

- `Content/Tiles/OreSpawn/`

All single-tile block textures should use `16x16`.

| Display name | File / asset ID | Target size |
| --- | --- | --- |
| Amethyst Block Tile | `AmethystBlockTile.png` | `16x16` |
| Amethyst Ore Tile | `AmethystOreTile.png` | `16x16` |
| Crystal Tree Log Tile | `CrystalTreeLogTile.png` | `16x16` |
| Duplicator Log Tile | `DuplicatorLogTile.png` | `16x16` |
| Ender Pearl Block Tile | `EnderPearlBlockTile.png` | `16x16` |
| Eye of Ender Block Tile | `EyeOfEnderBlockTile.png` | `16x16` |
| Kyanite Block Tile | `KyaniteBlockTile.png` | `16x16` |
| Kyanite Ore Tile | `KyaniteOreTile.png` | `16x16` |
| Molenoid Dirt Tile | `MolenoidDirtTile.png` | `16x16` |
| Pink Tourmaline Block Tile | `PinkTourmalineBlockTile.png` | `16x16` |
| Pink Tourmaline Ore Tile | `PinkTourmalineOreTile.png` | `16x16` |
| Red Ant Nest Tile | `RedAntNestTile.png` | `16x16` |
| Ruby Block Tile | `RubyBlockTile.png` | `16x16` |
| Ruby Ore Tile | `RubyOreTile.png` | `16x16` |
| Salt Block Tile | `SaltBlockTile.png` | `16x16` |
| Salt Ore Tile | `SaltOreTile.png` | `16x16` |
| Sky Tree Log Tile | `SkyTreeLogTile.png` | `16x16` |
| Teleport Block Tile | `TeleportBlockTile.png` | `16x16` |
| Termite Nest Tile | `TermiteNestTile.png` | `16x16` |
| Tigers Eye Block Tile | `TigersEyeBlockTile.png` | `16x16` |
| Tigers Eye Ore Tile | `TigersEyeOreTile.png` | `16x16` |
| Titanium Block Tile | `TitaniumBlockTile.png` | `16x16` |
| Titanium Ore Tile | `TitaniumOreTile.png` | `16x16` |
| Uranium Block Tile | `UraniumBlockTile.png` | `16x16` |
| Uranium Ore Tile | `UraniumOreTile.png` | `16x16` |

## Future armor production defaults

For future OreSpawn armor sets:

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

## Tools and weapon family defaults

- small tool icons: `36x36`
- standard weapon icons: `40x40`
- large late-game weapon icons: `42x42` to `54x54`
- giant endgame swords and axes: up to `64x64`

Use the same naming rule:

- class name = file name
- example:
  - `RubyPickaxe` -> `RubyPickaxe.png`
  - `UltimateSword` -> `UltimateSword.png`

## Food, plants, eggs, and accessories

- food icons: `26x26` to `32x32`
- seeds and small plants: `24x24`
- fruit icons: `32x32`
- special saplings: `32x32` to `40x40`
- ancient dried eggs: `32x32`
- normal accessory icon: `32x32`
- oversized relic-like accessory: `36x36`

## Existing Chaotic Dimensions special file names

These are not OreSpawn, but they are useful naming references because they show how special cases are named in this mod.

| Purpose | File name | Recommended size |
| --- | --- | --- |
| Crystaline boss head icon | `CrystalineDevourerHead_Head_Boss.png` | `80x80` |
| Crystaline helmet equip sheet | `CrystalineDevourMagicHelm_Head.png` | `40x1120` |
| Crystaline chest body sheet | `CrystalineDevourBreastplate_Body.png` | `360x224` |
| Crystaline chest arms sheet | `CrystalineDevourBreastplate_Arms.png` | `360x224` |
| Crystaline chest female body sheet | `CrystalineDevourBreastplate_FemaleBody.png` | `360x224` |
| Crystaline leg sheet | `CrystalineDevourGreaves_Legs.png` | `40x1120` |

## Current optional custom art that still uses vanilla or shared textures

These are not missing files required for the mod to run, but if you want bespoke art, these are the recommended sizes.

| Visual | Current status | Recommended custom size |
| --- | --- | --- |
| `CrystalineDevourAegisBuff` | uses vanilla buff texture | `32x32` |
| `CrystalinePotionFortitudeBuff` | uses vanilla buff texture | `32x32` |
| `CrystalinePotionRegenerationBuff` | uses vanilla buff texture | `32x32` |
| `CrystalineRushBuff` | uses vanilla buff texture | `32x32` |
| `CrystalineSword` | item currently points at vanilla art | `64x64` |
| `CrystalineShard` | uses vanilla projectile art | `40x40` |
| `CrystalineBoltProjectile` | uses vanilla projectile art | `20x20` |
| `CrystalineDevourerPortal` | uses vanilla projectile art | `96x96` |
| `CrystalineDevourerBeam` | beam drawn mostly by code | optional core `32x32` or strip `16x64` |
| `CrystalineDevourerSkyBeam` | beam drawn mostly by code | optional core `32x32` or strip `16x64` |
| `CrystalineSwordProjectile` | reuses `CrystalineSword` texture | same as sword item |

## Short version

If you only need the fastest answer:

- file name = class name
- use `PascalCase`
- mobs and bosses = vertical sheets with `4` frames
- normal items = `32x32`
- materials = `24x24`
- tiles = `16x16`
- helmets use `_Head`
- chestpieces use `_Body`, `_Arms`, `_FemaleBody`
- legs use `_Legs`

