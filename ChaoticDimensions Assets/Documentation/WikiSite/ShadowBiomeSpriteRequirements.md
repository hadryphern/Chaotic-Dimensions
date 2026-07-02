# Shadow Biome Sprite Requirements

Todos os nomes abaixo sao os nomes exatos dos arquivos que o mod espera hoje.

## Ja implementado em codigo agora

### Nota tecnica importante

- Para `item icons`, variar alguns pixels quase nunca causa problema real.
- Para `equip textures`, vale manter o padrao do Terraria.
- Para `tiles`, o que importa e seguir o tipo de sheet certo:
- bloco simples pode ser `16x16`
- terrain sheet de bloco auto-conectavel pode ser `288x270`
- furniture/multitile precisa seguir o layout esperado pelo `TileObjectData`

### Tiles e blocos

| Arquivo | Pasta | Tamanho | Frames |
|---|---|---:|---:|
| `ShadowDirtTile.png` | `Content/Tiles/ShadowBiome` | `288x270` | `1 terrain sheet` |
| `ShadowGrassTile.png` | `Content/Tiles/ShadowBiome` | `288x270` | `1 terrain sheet` |
| `ShadowStoneTile.png` | `Content/Tiles/ShadowBiome` | `288x270` | `1 terrain sheet` |
| `ShadowWoodTile.png` | `Content/Tiles/ShadowBiome` | `176x264` | `1 terrain-style sheet` |
| `ShadowDirtBlock.png` | `Content/Items/ShadowBiome` | `16x16` | `1` |
| `ShadowStoneBlock.png` | `Content/Items/ShadowBiome` | `16x16` | `1` |
| `ShadowWood.png` | `Content/Items/ShadowBiome` | `24x22` | `1` |

### Materiais e acessorios

| Arquivo | Pasta | Tamanho | Frames |
|---|---|---:|---:|
| `ShadowScrap.png` | `Content/Items/ShadowBiome` | `18x18` | `1` |
| `SoulOfShadow.png` | `Content/Items/ShadowBiome` | `22x22` | `1` |
| `ShadowTotem.png` | `Content/Items/ShadowBiome` | `28x34` | `1` |
| `ChaosCrystalPickaxe.png` | `Content/Items/ShadowBiome` | `46x46` | `1` |

### Mobs do biome

Spritesheet vertical recomendada.

| Arquivo | Pasta | Tamanho total | Frame | Frames |
|---|---|---:|---:|---:|
| `Phantasm.png` | `Content/NPCs/ShadowBiome` | `40x224` | `40x56` | `4` |
| `ShadowEye.png` | `Content/NPCs/ShadowBiome` | `42x168` | `42x42` | `4` |
| `ShadowSlime.png` | `Content/NPCs/ShadowBiome` | `48x68` | `48x34` | `2` |
| `ShadowWorm.png` | `Content/NPCs/ShadowBiome` | `64x128` | `64x32` | `4` |
| `KrakenSquid.png` | `Content/NPCs/ShadowBiome` | `30x56` | `30x28` | `2` |

### Rosalita armor

| Arquivo | Pasta | Tamanho | Frames |
|---|---|---:|---:|
| `RosalitaHelmet.png` | `Content/Items/Armor/Rosalita` | `30x22` | `1` |
| `RosalitaHelmet_Head.png` | `Content/Items/Armor/Rosalita` | `40x56` | `1` |
| `RosalitaBreastplate.png` | `Content/Items/Armor/Rosalita` | `34x24` | `1` |
| `RosalitaBreastplate_Body.png` | `Content/Items/Armor/Rosalita` | `40x56` | `1` |
| `RosalitaBreastplate_Arms.png` | `Content/Items/Armor/Rosalita` | `40x56` | `1` |
| `RosalitaBreastplate_FemaleBody.png` | `Content/Items/Armor/Rosalita` | `40x56` | `1` |
| `RosalitaGreaves.png` | `Content/Items/Armor/Rosalita` | `30x20` | `1` |
| `RosalitaGreaves_Legs.png` | `Content/Items/Armor/Rosalita` | `40x56` | `1` |

### Rosalita tools e armas

| Arquivo | Pasta | Tamanho | Frames |
|---|---|---:|---:|
| `RosalitaPickaxe.png` | `Content/Items/ShadowBiome` | `42x42` | `1` |
| `RosalitaAxe.png` | `Content/Items/ShadowBiome` | `42x42` | `1` |
| `RosalitaHammer.png` | `Content/Items/ShadowBiome` | `44x44` | `1` |
| `RosalitaBlade.png` | `Content/Items/ShadowBiome` | `48x48` | `1` |
| `RosalitaBow.png` | `Content/Items/ShadowBiome` | `32x56` | `1` |
| `RosalitaWand.png` | `Content/Items/ShadowBiome` | `32x48` | `1` |
| `RosalitaWhip.png` | `Content/Items/ShadowBiome` | `32x48` | `1` |
| `RosalitaMagicBolt.png` | `Content/Projectiles/Magic` | `18x18` | `1` |
| `RosalitaWhipProjectile.png` | `Content/Projectiles/Melee` | `14x66` | `1` |

### Upgrades de Monthra em Rosalita

| Arquivo | Pasta | Tamanho | Frames |
|---|---|---:|---:|
| `EclipsedMonthraPickaxe.png` | `Content/Items/ShadowBiome` | `46x46` | `1` |
| `EclipsedMonthraAxe.png` | `Content/Items/ShadowBiome` | `46x46` | `1` |
| `EclipsedMonthraHammer.png` | `Content/Items/ShadowBiome` | `48x48` | `1` |
| `EclipsedMonthraBlade.png` | `Content/Items/ShadowBiome` | `48x48` | `1` |
| `EclipsedMonthraBow.png` | `Content/Items/ShadowBiome` | `34x58` | `1` |
| `EclipsedMonthraWand.png` | `Content/Items/ShadowBiome` | `34x54` | `1` |
| `EclipsedMonthraWhip.png` | `Content/Items/ShadowBiome` | `34x54` | `1` |
| `EclipsedMonthraWhipProjectile.png` | `Content/Projectiles/Melee` | `14x66` | `1` |

### Shadow tier completo

| Arquivo | Pasta | Tamanho | Frames |
|---|---|---:|---:|
| `ShadowHelmet.png` | `Content/Items/Armor/Shadow` | `32x24` | `1` |
| `ShadowHelmet_Head.png` | `Content/Items/Armor/Shadow` | `40x56` | `1` |
| `ShadowBreastplate.png` | `Content/Items/Armor/Shadow` | `34x26` | `1` |
| `ShadowBreastplate_Body.png` | `Content/Items/Armor/Shadow` | `40x56` | `1` |
| `ShadowBreastplate_Arms.png` | `Content/Items/Armor/Shadow` | `40x56` | `1` |
| `ShadowBreastplate_FemaleBody.png` | `Content/Items/Armor/Shadow` | `40x56` | `1` |
| `ShadowGreaves.png` | `Content/Items/Armor/Shadow` | `32x22` | `1` |
| `ShadowGreaves_Legs.png` | `Content/Items/Armor/Shadow` | `40x56` | `1` |
| `ShadowSummonStaff.png` | `Content/Items/ShadowBiome` | `40x56` | `1` |
| `ShadowWhip.png` | `Content/Items/ShadowBiome` | `36x56` | `1` |
| `ShadowWhipProjectile.png` | `Content/Projectiles/Melee` | `14x74` | `1` |
| `ShadowBow.png` | `Content/Items/ShadowBiome` | `42x64` | `1` |
| `ShadowArrow.png` | `Content/Items/ShadowBiome` | `18x32` | `1` |
| `ShadowBullet.png` | `Content/Items/ShadowBiome` | `16x16` | `1` |
| `ShadowStaff.png` | `Content/Items/ShadowBiome` | `42x64` | `1` |
| `ShadowManaPotion.png` | `Content/Items/ShadowBiome` | `24x32` | `1` |
| `ShadowMeleePotion.png` | `Content/Items/ShadowBiome` | `24x32` | `1` |
| `HeartOfShadows.png` | `Content/Items/ShadowBiome` | `32x32` | `1` |
| `GloryBoots.png` | `Content/Items/ShadowBiome` | `32x32` | `1` |
| `HeartOfTheGod.png` | `Content/Items/ShadowBiome` | `28x28` | `1` |
| `GodnessAnvil.png` | `Content/Items/ShadowBiome` | `32x32` | `1` |
| `GodnessAnvilTile.png` | `Content/Tiles/ShadowBiome` | `62x32` | `1 custom 3x2 placed sprite` |
| `ShadowCrystalMinion.png` | `Content/Projectiles/Summon` | `30x168` | `4` |
| `ShadowCrystalBolt.png` | `Content/Projectiles/Summon` | `18x18` | `1` |
| `ShadowArrowProjectile.png` | `Content/Projectiles/Ranged` | `16x16` | `1` |
| `ShadowBulletProjectile.png` | `Content/Projectiles/Ranged` | `14x14` | `1` |
| `ShadowBoltProjectile.png` | `Content/Projectiles/Magic` | `24x24` | `1` |
| `ShadowZenith.png` | `Content/Items/ShadowBiome` | `64x64` | `1` |
| `ShadowZenithProjectile.png` | `Content/Projectiles/Melee` | `52x52` | `1` |
| `ShadowTagBuff.png` | `Content/Buffs` | `32x32` | `1` |
| `ShadowRendDebuff.png` | `Content/Buffs` | `32x32` | `1` |
| `ShadowManaPotionBuff.png` | `Content/Buffs` | `32x32` | `1` |
| `ShadowMeleePotionBuff.png` | `Content/Buffs` | `32x32` | `1` |
| `ShadowCrystalMinionBuff.png` | `Content/Buffs` | `32x32` | `1` |

## Ja vale desenhar para a proxima leva

### Itens futuros do Shadow tier

| Arquivo recomendado | Pasta recomendada | Tamanho |
|---|---|---:|
| `KrakenPrinceSummon.png` | `Content/Items/ShadowBiome` | `32x32` |
| `KrakenKingRelicCore.png` | `Content/Items/ShadowBiome` | `32x32` |

### Bosses futuros

| Arquivo recomendado | Pasta recomendada | Tamanho total | Frame | Frames |
|---|---|---:|---:|---:|
| `KrakenPrince.png` | `Content/Bosses/ShadowBiome` | `160x960` | `160x160` | `6` |
| `KrakenPrince_Head_Boss.png` | `Content/Bosses/ShadowBiome` | `80x80` | `1` |
| `KrakenKing.png` | `Content/Bosses/ShadowBiome` | `192x1152` | `192x192` | `6` |
| `KrakenKing_Head_Boss.png` | `Content/Bosses/ShadowBiome` | `80x80` | `1` |

### Environment extra do biome

| Arquivo recomendado | Pasta recomendada | Tamanho |
|---|---|---:|
| `ShadowGrassFoliage.png` | `Content/Tiles/ShadowBiome` | `16x16` |
| `ShadowPlantTall.png` | `Content/Tiles/ShadowBiome` | `16x32` |
| `ShadowFruit.png` | `Content/Items/ShadowBiome` | `20x20` |
| `ShadowTreeBranch.png` | `Content/Tiles/ShadowBiome` | `16x16` |

## Observacoes

- Para mobs e bosses, use spritesheet vertical por padrao.
- Para whips, o projectile deve ser uma tira vertical com `handle`, `segment` e `tip`.
- Para equipaveis do Terraria, mantenha o padrao `_Head`, `_Body`, `_Arms`, `_FemaleBody`, `_Legs`.
- Os placeholders atuais sao apenas para teste. Quando voce trocar pelos finais, eu ajusto `frameCount`, `drawScale`, hitbox e qualquer detalhe fino se precisar.
- `ShadowDirtTile`, `ShadowGrassTile` e `ShadowStoneTile` agora estao em sheet grande `288x270`, nao em `16x16`.
- `ShadowWoodTile` atual esta em `176x264`.
- `GodnessAnvilTile` atual esta em `62x32` e o codigo agora trata ele como `3x2` custom com `CoordinateWidth = 20`, `CoordinatePadding = 1`, `CoordinateHeights = [16, 15]` e `DrawYOffset = 4`.
