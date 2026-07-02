# Minecraft Original Port Audit

This document tracks content imported from the original Minecraft `Chaotic Dimensions`
project at:

`C:\Users\happi\Documents\.modding\Minecraft\ChaoticDimensions\ChaoticDimensions`

## Ported in this pass

### Materials and placeables
- `Alexandrite`
- `RawAlexandrite`
- `GlassStick`
- `ChaosCrystal`
- `ShadowGem`
- `ShadowNugget`
- `RosalitaGem`
- `RubyGem`
- `RubyNugget`
- `VortexGem`
- `BedrockStick`
- `IronStick`
- `AlexandriteBlock`
- `RawAlexandriteBlock`
- `GreystedWood`
- `AlexandriteOre`
- `AlexandriteDeepslateOre`
- `ShadowBlock`
- `ShadowOre`
- `RubyOre`
- `RosalitaOre`
- `BlueBerryPlant`

### Weapons and accessories
- `RubySword`
- `RubyPickaxe`
- `RubyAxe`
- `ShadowSword`
- `ShadowPickaxe`
- `VortexBlade`
- `RosalitaShield`

### Tiles and worldgen
- `AlexandriteBlockTile`
- `RawAlexandriteBlockTile`
- `GreystedWoodTile`
- `AlexandriteOreTile`
- `AlexandriteDeepslateOreTile`
- `ShadowBlockTile`
- `ShadowOreTile`
- `RubyOreTile`
- `RosalitaOreTile`
- `BlueBerryPlantTile`
- `MinecraftLegacyWorldGenSystem`

### NPC code added
- `HappyCreeper`
- `WhiteCreeper`
- `CrystalCreeper`
- `ShadowCreeper`
- `Endernmon`
- `Fantasm`
- `FlyZombieSquid`
- `Formiga`
- `ChaoticMimic`
- `RatMonster`
- `SnowBlaze`
- `RedGiantSpider`
- `GiantZombie`
- `WhiteZombie`
- `ZombieSombra`
- `BigButterfly`
- `AlessandraNpc`
- `FireflyCritter`
- `BlueButterfly`
- `GreenButterfly`
- `RedButterfly`
- `YellowButterfly`
- `AppleCow`
- `GoldenAppleCow`
- `CrystalAppleCow`
- `CrystalGoldenAppleCow`
- `DimensionPig`

## Asset strategy

### Imported directly from Minecraft original
- item icons
- block textures
- ore textures
- blue berry plant texture
- firefly texture

### Placeholder strategy used
Most Minecraft mob assets in `.mod` are UV maps for 3D models instead of Terraria-ready
side-view sprite sheets. To keep the code testable without locking in bad visuals,
placeholder sprites were generated and copied per mob class under:

`Content/NPCs/MinecraftLegacy`

Each placeholder file already matches the final Terraria asset name, so replacing the art
later does not require code changes.

## Next recommended art pass

Highest value replacements first:
- `HappyCreeper`
- `CrystalCreeper`
- `ShadowCreeper`
- `GiantZombie`
- `RedGiantSpider`
- `ChaoticMimic`
- `AppleCow`
- `CrystalAppleCow`
- `BigButterfly`
- `AlessandraNpc`
