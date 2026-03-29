# Chaotic Dimensions Sprite Audit

Atualizado em `2026-03-29`.

Este arquivo mostra apenas o estado visual atual do mod:

- `✅` sprite pronta e integrada
- `⚠️` existe `.png`, mas ainda parece placeholder, base simples ou rascunho
- `❌` ainda nao tem sprite exclusiva; usa vanilla, compartilhada ou desenho por codigo

## Como ler

- `Arquivo` = caminho exato do asset no mod
- `Tamanho atual` = tamanho do arquivo que esta no projeto hoje
- `Frame` = tamanho sugerido por frame quando a sprite for animada
- `Frames` = quantidade recomendada

## Ja prontos e integrados

| Status | Asset | Arquivo | Tamanho atual | Observacao |
|---|---|---|---:|---|
| `✅` | Monthra Boss | `Content/Bosses/Monthra/MonthraBoss.png` | `880x640` | 12 frames; sprite enviada por voce |
| `✅` | Monthra Boss Head Icon | `Content/Bosses/Monthra/MonthraBoss_Head_Boss.png` | `80x80` | Icone funcional |
| `✅` | Monthra Butterfly | `Content/NPCs/Critters/MonthraButterfly.png` | `40x360` | 6 frames |
| `✅` | Monthra Butterfly Staff | `Content/Items/Weapons/Summon/MonthraButterflyStaff.png` | `26x34` | Sprite nova da staff integrada |
| `✅` | Blue Butterfly | `Content/NPCs/MinecraftLegacy/BlueButterfly.png` | `40x120` | 2 frames |
| `✅` | Red Butterfly | `Content/NPCs/MinecraftLegacy/RedButterfly.png` | `40x120` | 2 frames |
| `✅` | Yellow Butterfly | `Content/NPCs/MinecraftLegacy/YellowButterfly.png` | `40x120` | 2 frames |
| `✅` | Crystaline Tear | `Content/Items/Materials/CrystalineTear.png` | `32x32` | Integrado da `.archive` |
| `✅` | Crystaline Shard | `Content/Bosses/CrystalineDevourer/CrystalineShard.png` | `54x54` | Integrado |
| `✅` | Crystaline Devourer Sky Beam | `Content/Bosses/CrystalineDevourer/CrystalineDevourerSkyBeam.png` | `16x64` | Integrado |
| `✅` | Rosalita Ore item | `Content/Items/MinecraftLegacy/RosalitaOre.png` | `16x16` | Item do inventario correto da `.structure/items` |
| `✅` | Shadow Ore item | `Content/Items/MinecraftLegacy/ShadowOre.png` | `16x16` | Item do inventario correto da `.structure/items` |
| `✅` | Rosalita Ore tile | `Content/Tiles/MinecraftLegacy/RosalitaOreTile.png` | `288x270` | Terrain sheet no padrao do Terraria |
| `✅` | Shadow Ore tile | `Content/Tiles/MinecraftLegacy/ShadowOreTile.png` | `288x270` | Terrain sheet no padrao do Terraria |
| `✅` | Shadow Dirt item | `Content/Items/ShadowBiome/ShadowDirtBlock.png` | `16x16` | Integrado |
| `✅` | Shadow Stone item | `Content/Items/ShadowBiome/ShadowStoneBlock.png` | `16x16` | Integrado |
| `✅` | Shadow Wood item | `Content/Items/ShadowBiome/ShadowWood.png` | `24x22` | Integrado |
| `✅` | Shadow Dirt tile | `Content/Tiles/ShadowBiome/ShadowDirtTile.png` | `288x270` | Terrain sheet completa |
| `✅` | Shadow Grass tile | `Content/Tiles/ShadowBiome/ShadowGrassTile.png` | `288x270` | Terrain sheet completa |
| `✅` | Shadow Stone tile | `Content/Tiles/ShadowBiome/ShadowStoneTile.png` | `288x270` | Terrain sheet completa |
| `✅` | Shadow Wood tile | `Content/Tiles/ShadowBiome/ShadowWoodTile.png` | `176x264` | Sheet pronta |
| `✅` | Godness Anvil tile | `Content/Tiles/ShadowBiome/GodnessAnvilTile.png` | `62x32` | Tile ajustado no codigo |
| `✅` | Godness Anvil item | `Content/Items/ShadowBiome/GodnessAnvil.png` | `62x32` | Usa a mesma textura do tile |
| `✅` | Shadow Scrap | `Content/Items/ShadowBiome/ShadowScrap.png` | `18x18` | Sprite nova integrada da `.archive` |
| `✅` | Soul of Shadow | `Content/Items/ShadowBiome/SoulOfShadow.png` | `22x22` | Sprite nova integrada da `.archive` |
| `✅` | Rosalita Wand | `Content/Items/ShadowBiome/RosalitaWand.png` | `42x40` | Sprite nova integrada da `.archive` |
| `✅` | Eclipsed Monthra Bow | `Content/Items/ShadowBiome/EclipsedMonthraBow.png` | `18x40` | Sprite nova integrada da `.archive` |
| `✅` | Eclipsed Monthra Wand | `Content/Items/ShadowBiome/EclipsedMonthraWand.png` | `34x34` | Sprite nova integrada da `.archive` |

## Bosses e visuais principais que ainda faltam

| Status | Asset | Arquivo | Tamanho recomendado | Frame | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Crystaline Devourer Head Boss Icon | `Content/Bosses/CrystalineDevourer/CrystalineDevourerHead_Head_Boss.png` | `80x80` | `80x80` | `1` | Funciona, mas pode ser refinado |
| `❌` | Crystaline Devourer Beam | `Content/Bosses/CrystalineDevourer/CrystalineDevourerBeam.png` | `32x32` ou `16x64` | `32x32` | `1` | Hoje usa vanilla |
| `❌` | Crystaline Devourer Portal | `Content/Bosses/CrystalineDevourer/CrystalineDevourerPortal.png` | `96x96` | `96x96` | `1` | Hoje usa vanilla |

## Mobs que ainda precisam de sprite final

### MinecraftLegacy

| Status | Asset | Arquivo | Tamanho recomendado | Frame | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Alessandra NPC | `Content/NPCs/MinecraftLegacy/AlessandraNpc.png` | `48x192` | `48x48` | `4` | Placeholder |
| `⚠️` | Apple Cow | `Content/NPCs/MinecraftLegacy/AppleCow.png` | `48x192` | `48x48` | `4` | Placeholder |
| `⚠️` | Big Butterfly | `Content/NPCs/MinecraftLegacy/BigButterfly.png` | `64x384` | `64x64` | `6` | Placeholder |
| `⚠️` | Crystal Apple Cow | `Content/NPCs/MinecraftLegacy/CrystalAppleCow.png` | `48x192` | `48x48` | `4` | Placeholder |
| `⚠️` | Crystal Golden Apple Cow | `Content/NPCs/MinecraftLegacy/CrystalGoldenAppleCow.png` | `48x192` | `48x48` | `4` | Placeholder |
| `⚠️` | Dimension Pig | `Content/NPCs/MinecraftLegacy/DimensionPig.png` | `48x192` | `48x48` | `4` | Placeholder |
| `⚠️` | Firefly Critter | `Content/NPCs/MinecraftLegacy/FireflyCritter.png` | `16x64` | `16x16` | `4` | Muito simples; opcional refazer |
| `⚠️` | Golden Apple Cow | `Content/NPCs/MinecraftLegacy/GoldenAppleCow.png` | `48x192` | `48x48` | `4` | Placeholder |
| `⚠️` | Green Butterfly | `Content/NPCs/MinecraftLegacy/GreenButterfly.png` | `32x160` | `32x32` | `5` | Placeholder |

### ShadowBiome

| Status | Asset | Arquivo | Tamanho recomendado | Frame | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | KrakenSquid | `Content/NPCs/ShadowBiome/KrakenSquid.png` | `30x56` | `30x28` | `2` | Placeholder |
| `⚠️` | Phantasm | `Content/NPCs/ShadowBiome/Phantasm.png` | `40x224` | `40x56` | `4` | Placeholder |
| `⚠️` | Shadow Eye | `Content/NPCs/ShadowBiome/ShadowEye.png` | `42x168` | `42x42` | `4` | Placeholder |
| `⚠️` | Shadow Slime | `Content/NPCs/ShadowBiome/ShadowSlime.png` | `48x68` | `48x34` | `2` | Placeholder |
| `⚠️` | Shadow Worm | `Content/NPCs/ShadowBiome/ShadowWorm.png` | `64x128` | `64x32` | `4` | Placeholder |

## Itens, acessorios e materiais que ainda faltam

### Rosalita e linha Eclipsed Monthra

| Status | Asset | Arquivo | Tamanho atual/recomendado | Frame | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Rosalita Pickaxe | `Content/Items/ShadowBiome/RosalitaPickaxe.png` | `42x42` | `42x42` | `1` | Placeholder |
| `⚠️` | Rosalita Axe | `Content/Items/ShadowBiome/RosalitaAxe.png` | `42x42` | `42x42` | `1` | Placeholder |
| `⚠️` | Rosalita Hammer | `Content/Items/ShadowBiome/RosalitaHammer.png` | `44x44` | `44x44` | `1` | Placeholder |
| `⚠️` | Rosalita Blade | `Content/Items/ShadowBiome/RosalitaBlade.png` | `48x48` | `48x48` | `1` | Placeholder |
| `⚠️` | Rosalita Bow | `Content/Items/ShadowBiome/RosalitaBow.png` | `32x56` | `32x56` | `1` | Placeholder |
| `✅` | Rosalita Wand | `Content/Items/ShadowBiome/RosalitaWand.png` | `42x40` | `42x40` | `1` | Sprite nova integrada |
| `⚠️` | Rosalita Whip item | `Content/Items/ShadowBiome/RosalitaWhip.png` | `38x34` | `38x34` | `1` | Base temporaria |
| `⚠️` | Eclipsed Monthra Pickaxe | `Content/Items/ShadowBiome/EclipsedMonthraPickaxe.png` | `36x36` | `36x36` | `1` | Base temporaria |
| `⚠️` | Eclipsed Monthra Axe | `Content/Items/ShadowBiome/EclipsedMonthraAxe.png` | `64x64` | `64x64` | `1` | Base temporaria |
| `⚠️` | Eclipsed Monthra Hammer | `Content/Items/ShadowBiome/EclipsedMonthraHammer.png` | `38x38` | `38x38` | `1` | Base temporaria |
| `⚠️` | Eclipsed Monthra Blade | `Content/Items/ShadowBiome/EclipsedMonthraBlade.png` | `74x74` | `74x74` | `1` | Base temporaria |
| `✅` | Eclipsed Monthra Bow | `Content/Items/ShadowBiome/EclipsedMonthraBow.png` | `18x40` | `18x40` | `1` | Sprite nova integrada |
| `✅` | Eclipsed Monthra Wand | `Content/Items/ShadowBiome/EclipsedMonthraWand.png` | `34x34` | `34x34` | `1` | Sprite nova integrada |
| `⚠️` | Eclipsed Monthra Whip item | `Content/Items/ShadowBiome/EclipsedMonthraWhip.png` | `38x34` | `38x34` | `1` | Base temporaria |

### Shadow tier

| Status | Asset | Arquivo | Tamanho atual/recomendado | Frame | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Shadow Totem | `Content/Items/ShadowBiome/ShadowTotem.png` | `28x34` | `28x34` | `1` | Placeholder |
| `⚠️` | Chaos Crystal Pickaxe | `Content/Items/ShadowBiome/ChaosCrystalPickaxe.png` | `46x46` | `46x46` | `1` | Placeholder |
| `✅` | Shadow Scrap | `Content/Items/ShadowBiome/ShadowScrap.png` | `18x18` | `18x18` | `1` | Sprite nova integrada |
| `✅` | Soul of Shadow | `Content/Items/ShadowBiome/SoulOfShadow.png` | `22x22` | `22x22` | `1` | Sprite nova integrada |
| `⚠️` | Shadow Summon Staff | `Content/Items/ShadowBiome/ShadowSummonStaff.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Shadow Whip item | `Content/Items/ShadowBiome/ShadowWhip.png` | `36x56` | `36x56` | `1` | Placeholder |
| `⚠️` | Shadow Bow | `Content/Items/ShadowBiome/ShadowBow.png` | `42x64` | `42x64` | `1` | Placeholder |
| `⚠️` | Shadow Arrow item | `Content/Items/ShadowBiome/ShadowArrow.png` | `18x32` | `18x32` | `1` | Placeholder |
| `⚠️` | Shadow Bullet item | `Content/Items/ShadowBiome/ShadowBullet.png` | `16x16` | `16x16` | `1` | Placeholder |
| `⚠️` | Shadow Staff | `Content/Items/ShadowBiome/ShadowStaff.png` | `42x64` | `42x64` | `1` | Placeholder |
| `⚠️` | Shadow Mana Potion | `Content/Items/ShadowBiome/ShadowManaPotion.png` | `24x32` | `24x32` | `1` | Placeholder |
| `⚠️` | Shadow Melee Potion | `Content/Items/ShadowBiome/ShadowMeleePotion.png` | `24x32` | `24x32` | `1` | Placeholder |
| `⚠️` | Heart of Shadows | `Content/Items/ShadowBiome/HeartOfShadows.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Heart of the God | `Content/Items/ShadowBiome/HeartOfTheGod.png` | `28x28` | `28x28` | `1` | Placeholder |
| `⚠️` | Glory Boots item | `Content/Items/ShadowBiome/GloryBoots.png` | `32x32` | `32x32` | `1` | Placeholder |

### Minecraft Legacy e Monthra extras

| Status | Asset | Arquivo | Tamanho atual/recomendado | Frame | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Glass Stick | `Content/Items/MinecraftLegacy/GlassStick.png` | `20x20` | `20x20` | `1` | Funcional, mas simples |
| `⚠️` | Iron Stick | `Content/Items/MinecraftLegacy/IronStick.png` | `20x20` | `20x20` | `1` | Funcional, mas simples |
| `⚠️` | Ratrix Stick | `Content/Items/MinecraftLegacy/RatrixStick.png` | `20x20` | `20x20` | `1` | Funcional, mas simples |
| `⚠️` | Shadow Bar | `Content/Items/MinecraftLegacy/ShadowBar.png` | `24x16` | `24x16` | `1` | Funcional, mas simples |
| `⚠️` | Kraken Blade | `Content/Items/MinecraftLegacy/KrakenBlade.png` | `56x56` | `56x56` | `1` | Funcional, mas simples |
| `⚠️` | Happy Creeper Staff | `Content/Items/MinecraftLegacy/HappyCreeperStaff.png` | `32x48` | `32x48` | `1` | Placeholder |
| `⚠️` | Squid Kraken Staff | `Content/Items/MinecraftLegacy/SquidKrakenStaff.png` | `32x48` | `32x48` | `1` | Placeholder |
| `⚠️` | Monthra Blade | `Content/Items/Weapons/Melee/MonthraBlade.png` | `48x48` | `48x48` | `1` | Base de teste |
| `⚠️` | Monthra Wand | `Content/Items/Weapons/Magic/MonthraWand.png` | `32x48` | `32x48` | `1` | Base de teste |
| `⚠️` | Monthra Bow | `Content/Items/Weapons/Ranged/MonthraBow.png` | `32x48` | `32x48` | `1` | Base de teste |
| `✅` | Monthra Butterfly Staff | `Content/Items/Weapons/Summon/MonthraButterflyStaff.png` | `26x34` | `26x34` | `1` | Sprite nova integrada |

## Armaduras e equipaveis que ainda faltam

### Rosalita

| Status | Asset | Arquivo | Tamanho atual/recomendado | Frame | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `✅` | Rosalita Helmet item | `Content/Items/Armor/Rosalita/RosalitaHelmet.png` | `22x22` | `22x22` | `1` | Item de inventario pronto |
| `⚠️` | Rosalita Helmet equip | `Content/Items/Armor/Rosalita/RosalitaHelmet_Head.png` | `40x56` | `40x56` | `1` | Placeholder |
| `✅` | Rosalita Breastplate item | `Content/Items/Armor/Rosalita/RosalitaBreastplate.png` | `34x20` | `34x20` | `1` | Item de inventario pronto |
| `⚠️` | Rosalita Breastplate body | `Content/Items/Armor/Rosalita/RosalitaBreastplate_Body.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Rosalita Breastplate arms | `Content/Items/Armor/Rosalita/RosalitaBreastplate_Arms.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Rosalita Breastplate female | `Content/Items/Armor/Rosalita/RosalitaBreastplate_FemaleBody.png` | `40x56` | `40x56` | `1` | Placeholder |
| `✅` | Rosalita Greaves item | `Content/Items/Armor/Rosalita/RosalitaGreaves.png` | `22x18` | `22x18` | `1` | Item de inventario pronto |
| `⚠️` | Rosalita Greaves legs | `Content/Items/Armor/Rosalita/RosalitaGreaves_Legs.png` | `40x56` | `40x56` | `1` | Placeholder |

### Shadow

| Status | Asset | Arquivo | Tamanho atual/recomendado | Frame | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `✅` | Shadow Helmet item | `Content/Items/Armor/Shadow/ShadowHelmet.png` | `22x22` | `22x22` | `1` | Item de inventario pronto |
| `⚠️` | Shadow Helmet equip | `Content/Items/Armor/Shadow/ShadowHelmet_Head.png` | `40x56` | `40x56` | `1` | Placeholder |
| `✅` | Shadow Breastplate item | `Content/Items/Armor/Shadow/ShadowBreastplate.png` | `30x26` | `30x26` | `1` | Item de inventario pronto |
| `⚠️` | Shadow Breastplate body | `Content/Items/Armor/Shadow/ShadowBreastplate_Body.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Shadow Breastplate arms | `Content/Items/Armor/Shadow/ShadowBreastplate_Arms.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Shadow Breastplate female | `Content/Items/Armor/Shadow/ShadowBreastplate_FemaleBody.png` | `40x56` | `40x56` | `1` | Placeholder |
| `✅` | Shadow Greaves item | `Content/Items/Armor/Shadow/ShadowGreaves.png` | `26x18` | `26x18` | `1` | Item de inventario pronto |
| `⚠️` | Shadow Greaves legs | `Content/Items/Armor/Shadow/ShadowGreaves_Legs.png` | `40x56` | `40x56` | `1` | Placeholder |

### Acessorio e equip extra

| Status | Asset | Arquivo | Tamanho atual/recomendado | Frame | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Glory Boots shoes equip | `Content/Items/ShadowBiome/GloryBoots_Shoes.png` | `40x56` | `40x56` | `1` | Equip texture ainda placeholder |

## Projeteis, buffs e efeitos que ainda faltam

| Status | Asset | Arquivo | Tamanho recomendado | Frame | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Rosalita Magic Bolt | `Content/Projectiles/Magic/RosalitaMagicBolt.png` | `18x18` | `18x18` | `1` | Placeholder |
| `⚠️` | Shadow Bolt Projectile | `Content/Projectiles/Magic/ShadowBoltProjectile.png` | `24x24` | `24x24` | `1` | Placeholder |
| `⚠️` | Shadow Arrow Projectile | `Content/Projectiles/Ranged/ShadowArrowProjectile.png` | `16x16` | `16x16` | `1` | Placeholder |
| `⚠️` | Shadow Bullet Projectile | `Content/Projectiles/Ranged/ShadowBulletProjectile.png` | `14x14` | `14x14` | `1` | Placeholder |
| `⚠️` | Shadow Crystal Minion | `Content/Projectiles/Summon/ShadowCrystalMinion.png` | `30x168` | `30x42` | `4` | Placeholder |
| `⚠️` | Shadow Crystal Bolt | `Content/Projectiles/Summon/ShadowCrystalBolt.png` | `18x18` | `18x18` | `1` | Placeholder |
| `⚠️` | Rosalita Whip Projectile | `Content/Projectiles/Melee/RosalitaWhipProjectile.png` | `10x92` | `10x92` | `1` | Base temporaria |
| `⚠️` | Eclipsed Monthra Whip Projectile | `Content/Projectiles/Melee/EclipsedMonthraWhipProjectile.png` | `10x92` | `10x92` | `1` | Base temporaria |
| `⚠️` | Shadow Whip Projectile | `Content/Projectiles/Melee/ShadowWhipProjectile.png` | `14x74` | `14x74` | `1` | Placeholder |
| `⚠️` | Shadow Zenith Projectile | `Content/Projectiles/Melee/ShadowZenithProjectile.png` | `52x52` | `52x52` | `1` | Placeholder |
| `⚠️` | Monthra Butterfly Buff | `Content/Buffs/MonthraButterflyBuff.png` | `32x32` | `32x32` | `1` | Simples, mas usavel |
| `⚠️` | Shadow Crystal Minion Buff | `Content/Buffs/ShadowCrystalMinionBuff.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Shadow Mana Potion Buff | `Content/Buffs/ShadowManaPotionBuff.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Shadow Melee Potion Buff | `Content/Buffs/ShadowMeleePotionBuff.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Shadow Rend Debuff | `Content/Buffs/ShadowRendDebuff.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Shadow Tag Buff | `Content/Buffs/ShadowTagBuff.png` | `32x32` | `32x32` | `1` | Placeholder |
| `❌` | Crystaline Devour Aegis Buff | `Content/Buffs/CrystalineDevourAegisBuff.png` | `32x32` | `32x32` | `1` | Hoje usa buff vanilla |
| `❌` | Crystaline Potion Fortitude Buff | `Content/Buffs/CrystalinePotionFortitudeBuff.png` | `32x32` | `32x32` | `1` | Hoje usa buff vanilla |
| `❌` | Crystaline Potion Regeneration Buff | `Content/Buffs/CrystalinePotionRegenerationBuff.png` | `32x32` | `32x32` | `1` | Hoje usa buff vanilla |
| `❌` | Crystaline Rush Buff | `Content/Buffs/CrystalineRushBuff.png` | `32x32` | `32x32` | `1` | Hoje usa buff vanilla |
| `❌` | Happy Creeper Minion Buff | `Content/Buffs/HappyCreeperMinionBuff.png` | `32x32` | `32x32` | `1` | Reutiliza outro buff |
| `❌` | Squid Kraken Minion Buff | `Content/Buffs/SquidKrakenMinionBuff.png` | `32x32` | `32x32` | `1` | Reutiliza outro buff |
| `❌` | Rosalita Tag Buff | `Content/Buffs/RosalitaTagBuff.png` | `32x32` | `32x32` | `1` | Reutiliza outro buff |
| `❌` | Eclipsed Monthra Tag Buff | `Content/Buffs/EclipsedMonthraTagBuff.png` | `32x32` | `32x32` | `1` | Reutiliza outro buff |
| `❌` | Crystaline Bolt Projectile | `Content/Projectiles/Magic/CrystalineBoltProjectile.png` | `20x20` | `20x20` | `1` | Hoje usa projetil vanilla |

## Notas especiais

### Ores do mundo

- `RosalitaOreTile.png` e `ShadowOreTile.png` devem continuar em `288x270`
- `RosalitaOre.png` e `ShadowOre.png` do inventario devem continuar em `16x16`

### Godness Anvil

- `Content/Tiles/ShadowBiome/GodnessAnvilTile.png` atual = `62x32`
- Layout atual no codigo:
  - `CoordinateWidth = 20`
  - `CoordinatePadding = 1`
  - `CoordinateHeights = [16, 15]`
  - `DrawYOffset = 4`

### Armaduras equipaveis

Use exatamente estes nomes:

- `_Head`
- `_Body`
- `_Arms`
- `_FemaleBody`
- `_Legs`

### Whips

Os whips usam um projectile em tira unica.  
O ideal e desenhar:

- cabo
- segmentos
- ponta

em uma sprite vertical unica.

## Ordem recomendada de producao

1. Mobs do `ShadowBiome`
2. Equipaveis de armadura `Rosalita` e `Shadow`
3. Arsenal `Shadow`
4. Whips, projectiles e buffs
5. Assets `Crystaline` que ainda usam vanilla
6. Passivos/placeholder restantes do `MinecraftLegacy`

## Como me mandar a proxima leva

Quando voce terminar qualquer sprite nova, me manda:

- caminho do arquivo
- nome do asset
- tamanho total
- tamanho por frame
- quantidade de frames
- layout vertical ou horizontal

Ai eu substituo no mod e ajusto `frameCount`, `drawScale`, hitbox e animacao se precisar.
