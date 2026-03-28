# Chaotic Dimensions Sprite Audit

Atualizado em `2026-03-28`.

Este arquivo foi reorganizado para ficar mais util na producao.  
O foco principal agora e mostrar:

- o que ja esta pronto e integrado
- o que ainda esta em placeholder
- o que ainda nem tem sprite exclusiva e continua usando vanilla/compartilhada

## Legenda

- `✅` pronto e usavel no mod
- `⚠️` existe `.png`, mas ainda parece placeholder/base temporaria
- `❌` ainda nao tem sprite exclusiva; hoje usa vanilla, compartilhada ou desenho por codigo

## Como ler

- `Arquivo` = nome exato do asset
- `Total recomendado` = tamanho total do arquivo final
- `Frame recomendado` = tamanho de cada frame
- `Frames` = quantidade recomendada
- layout padrao recomendado para mobs e bosses = `vertical`

## Ja prontos e integrados

Esses ja entraram no mod e hoje nao sao prioridade visual.

| Status | Asset | Arquivo | Tamanho atual | Observacao |
|---|---|---|---:|---|
| `✅` | Monthra Boss | `Content/Bosses/Monthra/MonthraBoss.png` | `880x640` | 12 frames; sprite enviada por voce |
| `✅` | Monthra Butterfly | `Content/NPCs/Critters/MonthraButterfly.png` | `40x360` | Critter/summon atualizado; 6 frames |
| `✅` | Blue Butterfly | `Content/NPCs/MinecraftLegacy/BlueButterfly.png` | `40x120` | 2 frames |
| `✅` | Red Butterfly | `Content/NPCs/MinecraftLegacy/RedButterfly.png` | `40x120` | 2 frames |
| `✅` | Yellow Butterfly | `Content/NPCs/MinecraftLegacy/YellowButterfly.png` | `40x120` | 2 frames |
| `✅` | Crystaline Tear | `Content/Items/Materials/CrystalineTear.png` | `32x32` | Item novo integrado da `.archive` |
| `✅` | Crystaline Shard | `Content/Bosses/CrystalineDevourer/CrystalineShard.png` | `54x54` | Ja ligado no mod |
| `✅` | Crystaline Devourer Sky Beam | `Content/Bosses/CrystalineDevourer/CrystalineDevourerSkyBeam.png` | `16x64` | Ja ligado no codigo |
| `✅` | Rosalita Ore item | `Content/Items/MinecraftLegacy/RosalitaOre.png` | `16x16` | Item do inventario correto |
| `✅` | Shadow Ore item | `Content/Items/MinecraftLegacy/ShadowOre.png` | `16x16` | Item do inventario correto |
| `✅` | Rosalita Ore tile | `Content/Tiles/MinecraftLegacy/RosalitaOreTile.png` | `288x270` | Terrain sheet no padrao do Terraria |
| `✅` | Shadow Ore tile | `Content/Tiles/MinecraftLegacy/ShadowOreTile.png` | `288x270` | Terrain sheet no padrao do Terraria |
| `✅` | Shadow Dirt item | `Content/Items/ShadowBiome/ShadowDirtBlock.png` | `16x16` | Integrado |
| `✅` | Shadow Stone item | `Content/Items/ShadowBiome/ShadowStoneBlock.png` | `16x16` | Integrado |
| `✅` | Shadow Wood item | `Content/Items/ShadowBiome/ShadowWood.png` | `24x22` | Integrado |
| `✅` | Shadow Dirt tile | `Content/Tiles/ShadowBiome/ShadowDirtTile.png` | `288x270` | Terrain sheet completa |
| `✅` | Shadow Grass tile | `Content/Tiles/ShadowBiome/ShadowGrassTile.png` | `288x270` | Terrain sheet completa |
| `✅` | Shadow Stone tile | `Content/Tiles/ShadowBiome/ShadowStoneTile.png` | `288x270` | Terrain sheet completa |
| `✅` | Shadow Wood tile | `Content/Tiles/ShadowBiome/ShadowWoodTile.png` | `176x264` | Sheet pronta |
| `✅` | Godness Anvil tile | `Content/Tiles/ShadowBiome/GodnessAnvilTile.png` | `62x32` | Sprite pronta e tile ja ajustado no codigo |
| `✅` | Godness Anvil item | `Content/Items/ShadowBiome/GodnessAnvil.png` | `62x32` | Agora usa exatamente a mesma textura do tile |

## Bosses e visuais principais que ainda faltam

| Status | Asset | Arquivo | Total recomendado | Frame recomendado | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Monthra boss head icon | `Content/Bosses/Monthra/MonthraBoss_Head_Boss.png` | `80x80` | `80x80` | `1` | Funciona, mas ainda simples |
| `⚠️` | Crystaline Devourer boss head icon | `Content/Bosses/CrystalineDevourer/CrystalineDevourerHead_Head_Boss.png` | `80x80` | `80x80` | `1` | Usavel, mas pode ser refinado |
| `❌` | Crystaline Devourer Beam | `Content/Bosses/CrystalineDevourer/CrystalineDevourerBeam.png` | `32x32` ou `16x64` | `32x32` | `1` | Hoje usa `Terraria/Images/Projectile_466` |
| `❌` | Crystaline Devourer Portal | `Content/Bosses/CrystalineDevourer/CrystalineDevourerPortal.png` | `96x96` | `96x96` | `1` | Hoje usa `Terraria/Images/Projectile_687` |

## Mobs que ainda precisam de sprite final

### Minecraft Legacy

| Status | Asset | Arquivo | Total recomendado | Frame recomendado | Frames | Estado atual |
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

| Status | Asset | Arquivo | Total recomendado | Frame recomendado | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | KrakenSquid | `Content/NPCs/ShadowBiome/KrakenSquid.png` | `30x56` | `30x28` | `2` | Placeholder |
| `⚠️` | Phantasm | `Content/NPCs/ShadowBiome/Phantasm.png` | `40x224` | `40x56` | `4` | Placeholder |
| `⚠️` | Shadow Eye | `Content/NPCs/ShadowBiome/ShadowEye.png` | `42x168` | `42x42` | `4` | Placeholder |
| `⚠️` | Shadow Slime | `Content/NPCs/ShadowBiome/ShadowSlime.png` | `48x68` | `48x34` | `2` | Placeholder |
| `⚠️` | Shadow Worm | `Content/NPCs/ShadowBiome/ShadowWorm.png` | `64x128` | `64x32` | `4` | Placeholder |

## Itens, acessorios e materiais que ainda faltam

### Rosalita e linha Eclipsed Monthra

| Status | Asset | Arquivo | Total recomendado | Frame recomendado | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Rosalita Pickaxe | `Content/Items/ShadowBiome/RosalitaPickaxe.png` | `42x42` | `42x42` | `1` | Placeholder |
| `⚠️` | Rosalita Axe | `Content/Items/ShadowBiome/RosalitaAxe.png` | `42x42` | `42x42` | `1` | Placeholder |
| `⚠️` | Rosalita Hammer | `Content/Items/ShadowBiome/RosalitaHammer.png` | `44x44` | `44x44` | `1` | Placeholder |
| `⚠️` | Rosalita Blade | `Content/Items/ShadowBiome/RosalitaBlade.png` | `48x48` | `48x48` | `1` | Placeholder |
| `⚠️` | Rosalita Bow | `Content/Items/ShadowBiome/RosalitaBow.png` | `32x56` | `32x56` | `1` | Placeholder |
| `⚠️` | Rosalita Wand | `Content/Items/ShadowBiome/RosalitaWand.png` | `32x48` | `32x48` | `1` | Placeholder |
| `⚠️` | Rosalita Whip item | `Content/Items/ShadowBiome/RosalitaWhip.png` | `32x48` | `32x48` | `1` | Placeholder |
| `⚠️` | Eclipsed Monthra Pickaxe | `Content/Items/ShadowBiome/EclipsedMonthraPickaxe.png` | `46x46` | `46x46` | `1` | Placeholder |
| `⚠️` | Eclipsed Monthra Axe | `Content/Items/ShadowBiome/EclipsedMonthraAxe.png` | `46x46` | `46x46` | `1` | Placeholder |
| `⚠️` | Eclipsed Monthra Hammer | `Content/Items/ShadowBiome/EclipsedMonthraHammer.png` | `48x48` | `48x48` | `1` | Placeholder |
| `⚠️` | Eclipsed Monthra Blade | `Content/Items/ShadowBiome/EclipsedMonthraBlade.png` | `48x48` | `48x48` | `1` | Placeholder |
| `⚠️` | Eclipsed Monthra Bow | `Content/Items/ShadowBiome/EclipsedMonthraBow.png` | `34x58` | `34x58` | `1` | Placeholder |
| `⚠️` | Eclipsed Monthra Wand | `Content/Items/ShadowBiome/EclipsedMonthraWand.png` | `34x54` | `34x54` | `1` | Placeholder |
| `⚠️` | Eclipsed Monthra Whip item | `Content/Items/ShadowBiome/EclipsedMonthraWhip.png` | `34x54` | `34x54` | `1` | Placeholder |

### Shadow tier

| Status | Asset | Arquivo | Total recomendado | Frame recomendado | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Shadow Totem | `Content/Items/ShadowBiome/ShadowTotem.png` | `28x34` | `28x34` | `1` | Placeholder |
| `⚠️` | Chaos Crystal Pickaxe | `Content/Items/ShadowBiome/ChaosCrystalPickaxe.png` | `46x46` | `46x46` | `1` | Placeholder |
| `⚠️` | Shadow Scrap | `Content/Items/ShadowBiome/ShadowScrap.png` | `18x18` | `18x18` | `1` | Placeholder |
| `⚠️` | Soul of Shadow | `Content/Items/ShadowBiome/SoulOfShadow.png` | `22x22` | `22x22` | `1` | Placeholder |
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

| Status | Asset | Arquivo | Total recomendado | Frame recomendado | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Glass Stick | `Content/Items/MinecraftLegacy/GlassStick.png` | `20x20` | `20x20` | `1` | Funcional, mas simples |
| `⚠️` | Iron Stick | `Content/Items/MinecraftLegacy/IronStick.png` | `20x20` | `20x20` | `1` | Funcional, mas simples |
| `⚠️` | Ratrix Stick | `Content/Items/MinecraftLegacy/RatrixStick.png` | `20x20` | `20x20` | `1` | Funcional, mas simples |
| `⚠️` | Shadow Bar | `Content/Items/MinecraftLegacy/ShadowBar.png` | `24x16` | `24x16` | `1` | Funcional, mas simples |
| `⚠️` | Kraken Blade | `Content/Items/MinecraftLegacy/KrakenBlade.png` | `56x56` | `56x56` | `1` | Funcional, mas ainda simples |
| `⚠️` | Happy Creeper Staff | `Content/Items/MinecraftLegacy/HappyCreeperStaff.png` | `32x48` | `32x48` | `1` | Placeholder |
| `⚠️` | Squid Kraken Staff | `Content/Items/MinecraftLegacy/SquidKrakenStaff.png` | `32x48` | `32x48` | `1` | Placeholder |
| `⚠️` | Monthra Blade | `Content/Items/Weapons/Melee/MonthraBlade.png` | `48x48` | `48x48` | `1` | Base de teste |
| `⚠️` | Monthra Wand | `Content/Items/Weapons/Magic/MonthraWand.png` | `32x48` | `32x48` | `1` | Base de teste |
| `⚠️` | Monthra Bow | `Content/Items/Weapons/Ranged/MonthraBow.png` | `32x48` | `32x48` | `1` | Base de teste |
| `✅` | Monthra Butterfly Staff | `Content/Items/Weapons/Summon/MonthraButterflyStaff.png` | `40x60` | `40x60` | `1` | Adaptada da nova Month de 6 frames |

## Armaduras e equipaveis que ainda faltam

### Rosalita

| Status | Asset | Arquivo | Total recomendado | Frame recomendado | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Rosalita Helmet item | `Content/Items/Armor/Rosalita/RosalitaHelmet.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Rosalita Helmet equip | `Content/Items/Armor/Rosalita/RosalitaHelmet_Head.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Rosalita Breastplate item | `Content/Items/Armor/Rosalita/RosalitaBreastplate.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Rosalita Breastplate body | `Content/Items/Armor/Rosalita/RosalitaBreastplate_Body.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Rosalita Breastplate arms | `Content/Items/Armor/Rosalita/RosalitaBreastplate_Arms.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Rosalita Breastplate female | `Content/Items/Armor/Rosalita/RosalitaBreastplate_FemaleBody.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Rosalita Greaves item | `Content/Items/Armor/Rosalita/RosalitaGreaves.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Rosalita Greaves legs | `Content/Items/Armor/Rosalita/RosalitaGreaves_Legs.png` | `40x56` | `40x56` | `1` | Placeholder |

### Shadow

| Status | Asset | Arquivo | Total recomendado | Frame recomendado | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Shadow Helmet item | `Content/Items/Armor/Shadow/ShadowHelmet.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Shadow Helmet equip | `Content/Items/Armor/Shadow/ShadowHelmet_Head.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Shadow Breastplate item | `Content/Items/Armor/Shadow/ShadowBreastplate.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Shadow Breastplate body | `Content/Items/Armor/Shadow/ShadowBreastplate_Body.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Shadow Breastplate arms | `Content/Items/Armor/Shadow/ShadowBreastplate_Arms.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Shadow Breastplate female | `Content/Items/Armor/Shadow/ShadowBreastplate_FemaleBody.png` | `40x56` | `40x56` | `1` | Placeholder |
| `⚠️` | Shadow Greaves item | `Content/Items/Armor/Shadow/ShadowGreaves.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Shadow Greaves legs | `Content/Items/Armor/Shadow/ShadowGreaves_Legs.png` | `40x56` | `40x56` | `1` | Placeholder |

### Acessorio/equip extra

| Status | Asset | Arquivo | Total recomendado | Frame recomendado | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Glory Boots shoes equip | `Content/Items/ShadowBiome/GloryBoots_Shoes.png` | `40x56` | `40x56` | `1` | Equip texture ainda placeholder |

## Projeteis, buffs e efeitos que ainda faltam

| Status | Asset | Arquivo | Total recomendado | Frame recomendado | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `⚠️` | Rosalita Magic Bolt | `Content/Projectiles/Magic/RosalitaMagicBolt.png` | `18x18` | `18x18` | `1` | Placeholder |
| `⚠️` | Shadow Bolt Projectile | `Content/Projectiles/Magic/ShadowBoltProjectile.png` | `24x24` | `24x24` | `1` | Placeholder |
| `⚠️` | Shadow Arrow Projectile | `Content/Projectiles/Ranged/ShadowArrowProjectile.png` | `16x16` | `16x16` | `1` | Placeholder |
| `⚠️` | Shadow Bullet Projectile | `Content/Projectiles/Ranged/ShadowBulletProjectile.png` | `14x14` | `14x14` | `1` | Placeholder |
| `⚠️` | Shadow Crystal Minion | `Content/Projectiles/Summon/ShadowCrystalMinion.png` | `30x168` | `30x42` | `4` | Placeholder |
| `⚠️` | Shadow Crystal Bolt | `Content/Projectiles/Summon/ShadowCrystalBolt.png` | `18x18` | `18x18` | `1` | Placeholder |
| `⚠️` | Rosalita Whip Projectile | `Content/Projectiles/Melee/RosalitaWhipProjectile.png` | `14x66` | `14x66` | `1` | Placeholder |
| `⚠️` | Eclipsed Monthra Whip Projectile | `Content/Projectiles/Melee/EclipsedMonthraWhipProjectile.png` | `14x66` | `14x66` | `1` | Placeholder |
| `⚠️` | Shadow Whip Projectile | `Content/Projectiles/Melee/ShadowWhipProjectile.png` | `14x74` | `14x74` | `1` | Placeholder |
| `⚠️` | Shadow Zenith Projectile | `Content/Projectiles/Melee/ShadowZenithProjectile.png` | `52x52` | `52x52` | `1` | Placeholder |
| `⚠️` | Monthra Butterfly Buff | `Content/Buffs/MonthraButterflyBuff.png` | `32x32` | `32x32` | `1` | Usavel, mas ainda simples |
| `⚠️` | Shadow Crystal Minion Buff | `Content/Buffs/ShadowCrystalMinionBuff.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Shadow Mana Potion Buff | `Content/Buffs/ShadowManaPotionBuff.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Shadow Melee Potion Buff | `Content/Buffs/ShadowMeleePotionBuff.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Shadow Rend Debuff | `Content/Buffs/ShadowRendDebuff.png` | `32x32` | `32x32` | `1` | Placeholder |
| `⚠️` | Shadow Tag Buff | `Content/Buffs/ShadowTagBuff.png` | `32x32` | `32x32` | `1` | Placeholder |
| `❌` | Crystaline Devour Aegis Buff | `Content/Buffs/CrystalineDevourAegisBuff.png` | `32x32` | `32x32` | `1` | Hoje usa buff vanilla |
| `❌` | Crystaline Potion Fortitude Buff | `Content/Buffs/CrystalinePotionFortitudeBuff.png` | `32x32` | `32x32` | `1` | Hoje usa buff vanilla |
| `❌` | Crystaline Potion Regeneration Buff | `Content/Buffs/CrystalinePotionRegenerationBuff.png` | `32x32` | `32x32` | `1` | Hoje usa buff vanilla |
| `❌` | Crystaline Rush Buff | `Content/Buffs/CrystalineRushBuff.png` | `32x32` | `32x32` | `1` | Hoje usa buff vanilla |
| `❌` | Happy Creeper Minion Buff | `Content/Buffs/HappyCreeperMinionBuff.png` | `32x32` | `32x32` | `1` | Hoje reutiliza `MonthraButterflyBuff.png` |
| `❌` | Squid Kraken Minion Buff | `Content/Buffs/SquidKrakenMinionBuff.png` | `32x32` | `32x32` | `1` | Hoje reutiliza `MonthraButterflyBuff.png` |
| `❌` | Rosalita Tag Buff | `Content/Buffs/RosalitaTagBuff.png` | `32x32` | `32x32` | `1` | Hoje reutiliza `MonthraButterflyBuff.png` |
| `❌` | Eclipsed Monthra Tag Buff | `Content/Buffs/EclipsedMonthraTagBuff.png` | `32x32` | `32x32` | `1` | Hoje reutiliza `MonthraButterflyBuff.png` |
| `❌` | Crystaline Bolt Projectile | `Content/Projectiles/Magic/CrystalineBoltProjectile.png` | `20x20` | `20x20` | `1` | Hoje usa projétil vanilla |

## Visuais Crystaline ainda sem sprite exclusiva

| Status | Asset | Arquivo | Total recomendado | Frame recomendado | Frames | Estado atual |
|---|---|---|---:|---:|---:|---|
| `❌` | Crystaline Sword item icon exclusivo | `Content/Items/Weapons/Melee/CrystalineSword.png` | `64x64` | `64x64` | `1` | O arquivo existe, mas o item ainda usa sprite vanilla no codigo |

## Notas especiais importantes

### Ores do mundo

- `RosalitaOreTile.png` e `ShadowOreTile.png` devem continuar em `288x270`
- esse formato e o correto para terrain sheet de minerio no Terraria
- o item do inventario continua separado em `16x16`

### Godness Anvil

- `Content/Tiles/ShadowBiome/GodnessAnvilTile.png` atual = `62x32`
- o tile ja esta configurado para esse sprite custom
- layout atual no codigo:
  - `CoordinateWidth = 20`
  - `CoordinatePadding = 1`
  - `CoordinateHeights = [16, 15]`
  - `DrawYOffset = 4`

Se voce redesenhar o `GodnessAnvilTile.png` mantendo `62x32`, eu normalmente nao preciso mexer no codigo de novo.

### Armaduras equipaveis

Para armaduras do Terraria, mantenha exatamente estes nomes:

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

## Ordem recomendada de producao agora

Se quiser continuar pelo caminho mais eficiente, eu recomendo:

1. `ShadowBiome` mobs
2. `Rosalita` e `Shadow` armaduras
3. `Shadow` arsenal principal
4. `Whips`, projectiles e buffs
5. `Crystaline` assets que ainda usam vanilla
6. `Legacy` passivos/placeholders restantes

## Como me mandar a proxima leva

Quando voce terminar qualquer sprite nova, me manda:

- caminho do arquivo
- nome do asset
- tamanho total
- tamanho por frame
- quantidade de frames
- layout vertical ou horizontal

Ai eu substituo no mod e ajusto `frameCount`, `drawScale`, hitbox e animacao se precisar.
