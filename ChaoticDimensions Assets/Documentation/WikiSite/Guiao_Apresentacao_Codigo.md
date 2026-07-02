# Guião de apresentação do código

Este ficheiro é um apoio oral. Os comentários no código explicam intenção e decisões importantes sem repetir literalmente cada instrução.

## Visão geral

- **ChaoticDimensions.cs** liga o mod ao tModLoader, regista efeitos gráficos e encaminha mensagens de rede.
- **Common/Systems** guarda apresentações, arenas, clima, eventos e bosses derrotados.
- **Content/Bosses e Content/NPCs** contêm as máquinas de estados e a inteligência dos inimigos.
- **Content/Projectiles** separa o comportamento dos ataques da classe principal de cada boss.
- **Content/Scenes** decide quando ativar música, filtros e céus especiais.
- O servidor cria NPCs e projéteis; o cliente trata principalmente de desenho, interface e áudio.

## Fluxo do Alien Kraken

1. KrakenSummonItem verifica a progressão e pede o início do evento.
2. KrakenEventSystem.StartEvent inicia a tempestade e sincroniza o encontro.
3. KrakenEventSystem.SpawnKraken cria o NPC apenas no servidor.
4. KrakenBoss.AI escolhe movimento, fase e ataque especial.
5. RunAttackPattern abre janelas de ataque; SpawnCoordinatedCombo escolhe apenas uma ameaça principal.
6. PreDraw seleciona os atlas de animação e desenha a Ruby separadamente.

## Fluxo da Monthra

1. A Monthra Butterfly pós-Moon Lord inicia MonthraIntroSystem.
2. MonthraBoss.AI calcula a raiva pela percentagem de vida perdida.
3. A máquina de estados alterna volley, dash, raios, grelha, espiral e enxame.
4. MonthraScene ativa a música, o filtro e MonthraGalaxySky.

## Fluxo do Crystaline Devourer

1. CrystalineSigil inicia a apresentação prolongada.
2. CrystalineDevourerIntroSystem cria o boss no final da apresentação.
3. CrystalineDevourerArenaSystem guarda os tiles e cria uma barreira temporária.
4. O verme líder cria o gémeo, sincroniza a vida e escolhe ataques.
5. Quando o encontro termina, os tiles originais são restaurados.

## Vocabulário rápido

- **SetStaticDefaults:** metadados registados uma vez.
- **SetDefaults:** tamanho, dano, defesa, raridade e valores iniciais.
- **AI:** lógica executada a cada tick, normalmente 60 vezes por segundo.
- **PreDraw:** desenho manual antes do Terraria desenhar a entidade.
- **ModSystem:** sistema global ligado ao mundo, interface ou eventos.
- **ModSceneEffect:** música e efeitos ativados por uma condição.
- **Main.netMode:** distingue cliente, servidor dedicado e jogo local.
- **NPC.ai e Projectile.ai:** campos pequenos e sincronizados usados por máquinas de estados.

## Pontos para abrir durante a apresentação

- **ChaoticDimensions.cs:62** — HandlePacket: entrada e distribuição dos pacotes de rede.
- **Common/Systems/KrakenEventSystem.cs:106** — StartEvent: início da tempestade e do evento.
- **Content/NPCs/Kraken/KrakenBoss.cs:447** — RunAttackPattern: agenda dos ataques.
- **Content/NPCs/Kraken/KrakenBoss.cs:597** — SpawnCoordinatedCombo: redução do spam.
- **Content/Bosses/Monthra/MonthraBoss.cs:76** — AI: máquina de estados e raiva.
- **Content/Bosses/Monthra/MonthraBoss.cs:197** — RunLightLattice: zona segura dos lasers.
- **Common/Systems/CrystalineDevourerArenaSystem.cs:190** — CreateBarrier: construção temporária da arena.
- **Content/Bosses/CrystalineDevourer/CrystalineDevourerHead.cs:248** — SyncSharedLife: vida partilhada entre vermes.

## Roteiro oral de oito minutos

1. **Objetivo:** desenvolver um mod jogável de Terraria com C# e tModLoader.
2. **Arquitetura:** separar sistemas, bosses, itens, projéteis e recursos visuais.
3. **Kraken:** evento, máquina de estados, animação por atlas e redução de spam.
4. **Monthra:** progressão de raiva e ataques com aviso visual.
5. **Crystaline:** dois NPCs com vida partilhada e arena restaurável.
6. **Multijogador:** criação no servidor e sincronização da AI.
7. **Testes:** compilação, carregamento e correção do filtro nulo da Monthra.
8. **Continuidade:** balanceamento, novos bosses e substituição de placeholders.

## Respostas curtas para perguntas prováveis

- **Porque existem vários sistemas?** Para não misturar clima, interface, arena e progressão dentro da classe de um boss.
- **Porque não comentar cada linha?** Comentários úteis explicam intenção; repetir o código piora a leitura e a manutenção.
- **Como se evita spam no Kraken?** Cada janela escolhe uma ameaça principal e as fases alteram precisão e tempo.
- **Como a Monthra fica mais difícil?** A vida perdida reduz durações e aumenta velocidade gradualmente.
- **Como a arena não destrói o mundo?** Cada tile é guardado antes da barreira e restaurado no final.
- **O que corre no servidor?** Spawn, dano, NPCs e projéteis. Céus, filtros e interface ficam no cliente.

## Mapa de todos os ficheiros C#

### Common/GlobalNPCs

- **Common/GlobalNPCs/ShadowBiomeGlobalNPC.cs** (41 linhas): Reune a implementacao e os tipos auxiliares de Shadow Biome Global NPC. Tipos: ShadowBiomeGlobalNPC.

### Common/Graphics

- **Common/Graphics/CrystalineDevourerSky.cs** (171 linhas): Desenha o ceu cosmico proprio da luta do Crystaline Devourer. Tipos: CrystalineDevourerSky, SkyStar, ConstellationLine.
- **Common/Graphics/MonthraGalaxySky.cs** (61 linhas): Desenha a galaxia rosa usada durante a luta da Monthra. Tipos: MonthraGalaxySky.

### Common/Menus

- **Common/Menus/ChaoticDimensionsMenu.cs** (81 linhas): Personaliza a apresentacao do menu atraves de Chaotic Dimensions Menu. Tipos: ChaoticDimensionsMenu.

### Common/Progression

- **Common/Progression/ChaoticProgressionGate.cs** (28 linhas): Define regras e etapas de progressao relacionadas com Chaotic Progression Gate. Tipos: ChaoticProgressionGate.
- **Common/Progression/ChaoticProgressionHelper.cs** (62 linhas): Centraliza as verificacoes de progressao usadas por itens, NPCs e receitas. Tipos: ChaoticProgressionHelper.

### Common/Systems

- **Common/Systems/ChaoticDownedBossSystem.cs** (87 linhas): Guarda no mundo quais bosses do mod ja foram derrotados. Tipos: ChaoticDownedBossSystem.
- **Common/Systems/CrystalineDevourerArenaSystem.cs** (323 linhas): Cria a arena temporaria do Crystaline e restaura os tiles no final. Tipos: CrystalineDevourerArenaSystem, struct.
- **Common/Systems/CrystalineDevourerIntroSystem.cs** (109 linhas): Controla a duracao e o desenho da apresentacao do Crystaline Devourer. Tipos: CrystalineDevourerIntroSystem.
- **Common/Systems/KrakenEventSystem.cs** (753 linhas): Coordena a entrada, tempestade, camera e efeitos gerais da luta do Kraken. Tipos: KrakenEventSystem.
- **Common/Systems/MinecraftLegacyWorldGenSystem.cs** (129 linhas): Mantem o estado global e o ciclo de vida de Minecraft Legacy World Gen System. Tipos: MinecraftLegacyWorldGenSystem.
- **Common/Systems/MonthraIntroSystem.cs** (121 linhas): Controla a apresentacao da Monthra antes de criar o NPC do boss. Tipos: MonthraIntroSystem.
- **Common/Systems/ShadowBiomeTileCountSystem.cs** (22 linhas): Mantem o estado global e o ciclo de vida de Shadow Biome Tile Count System. Tipos: ShadowBiomeTileCountSystem.
- **Common/Systems/ShadowBiomeWorldSystem.cs** (316 linhas): Mantem o estado global e o ciclo de vida de Shadow Biome World System. Tipos: ShadowBiomeWorldSystem.

### Common/Tiles

- **Common/Tiles/CrystalineDevourerArenaTileGuard.cs** (28 linhas): Define os blocos, colocacao e interacoes de Crystaline Devourer Arena Tile Guard. Tipos: CrystalineDevourerArenaTileGuard.

### Content/Backgrounds

- **Content/Backgrounds/KrakenCosmicSky.cs** (117 linhas): Desenha e atualiza os elementos visuais de Kraken Cosmic Sky. Tipos: KrakenCosmicSky.
- **Content/Backgrounds/KrakenSurfaceBackgroundStyle.cs** (29 linhas): Desenha e atualiza os elementos visuais de Kraken Surface Background Style. Tipos: KrakenSurfaceBackgroundStyle.

### Content/BossBars

- **Content/BossBars/CrystalineDevourerBossBar.cs** (55 linhas): Desenha a barra de vida especial de Crystaline Devourer Boss Bar. Tipos: CrystalineDevourerBossBar.

### Content/BossConcepts

- **Content/BossConcepts/BossDesignModels.cs** (22 linhas): Documenta em codigo o conceito futuro de Boss Design Models, ainda sem spawn no jogo. Tipos: struct.
- **Content/BossConcepts/MagnetMageDesign.cs** (76 linhas): Documenta em codigo o conceito futuro de Magnet Mage Design, ainda sem spawn no jogo. Tipos: contains, MagnetMageDesign.
- **Content/BossConcepts/MimicClownDesign.cs** (85 linhas): Documenta em codigo o conceito futuro de Mimic Clown Design, ainda sem spawn no jogo. Tipos: MimicClownDesign.

### Content/Bosses

- **Content/Bosses/CrystalineDevourer/CrystalineDevourerBeam.cs** (115 linhas): Contem uma parte do combate e dos recursos do boss Crystaline Devourer Beam. Tipos: CrystalineDevourerBeam.
- **Content/Bosses/CrystalineDevourer/CrystalineDevourerBody.cs** (113 linhas): Contem uma parte do combate e dos recursos do boss Crystaline Devourer Body. Tipos: CrystalineDevourerBody.
- **Content/Bosses/CrystalineDevourer/CrystalineDevourerHead.cs** (749 linhas): Controla os dois vermes, a vida partilhada e os ataques do Crystaline Devourer. Tipos: CrystalineAttackState, CrystalineDevourerHead.
- **Content/Bosses/CrystalineDevourer/CrystalineDevourerPortal.cs** (30 linhas): Contem uma parte do combate e dos recursos do boss Crystaline Devourer Portal. Tipos: CrystalineDevourerPortal.
- **Content/Bosses/CrystalineDevourer/CrystalineDevourerSegmentVisuals.cs** (124 linhas): Contem uma parte do combate e dos recursos do boss Crystaline Devourer Segment Visuals. Tipos: CrystalineDevourerSegmentVisuals.
- **Content/Bosses/CrystalineDevourer/CrystalineDevourerSkyBeam.cs** (101 linhas): Contem uma parte do combate e dos recursos do boss Crystaline Devourer Sky Beam. Tipos: CrystalineDevourerSkyBeam.
- **Content/Bosses/CrystalineDevourer/CrystalineDevourerTail.cs** (85 linhas): Contem uma parte do combate e dos recursos do boss Crystaline Devourer Tail. Tipos: CrystalineDevourerTail.
- **Content/Bosses/CrystalineDevourer/CrystalineShard.cs** (69 linhas): Contem uma parte do combate e dos recursos do boss Crystaline Shard. Tipos: CrystalineShard.
- **Content/Bosses/Monthra/MonthraBoss.cs** (410 linhas): Implementa os seis estados de combate e a progressao de raiva da Monthra. Tipos: MonthraAttackState, MonthraBoss.

### Content/Buffs

- **Content/Buffs/CrystalineDevourAegisBuff.cs** (19 linhas): Regista os buffs e debuffs agrupados em Crystaline Devour Aegis Buff. Tipos: CrystalineDevourAegisBuff.
- **Content/Buffs/CrystalinePotionFortitudeBuff.cs** (17 linhas): Regista os buffs e debuffs agrupados em Crystaline Potion Fortitude Buff. Tipos: CrystalinePotionFortitudeBuff.
- **Content/Buffs/CrystalinePotionRegenerationBuff.cs** (17 linhas): Regista os buffs e debuffs agrupados em Crystaline Potion Regeneration Buff. Tipos: CrystalinePotionRegenerationBuff.
- **Content/Buffs/CrystalineRushBuff.cs** (23 linhas): Regista os buffs e debuffs agrupados em Crystaline Rush Buff. Tipos: CrystalineRushBuff.
- **Content/Buffs/KrakenAbyssBreathBuff.cs** (25 linhas): Regista os buffs e debuffs agrupados em Kraken Abyss Breath Buff. Tipos: KrakenAbyssBreathBuff.
- **Content/Buffs/KrakenCrushingDepthDebuff.cs** (24 linhas): Regista os buffs e debuffs agrupados em Kraken Crushing Depth Debuff. Tipos: KrakenCrushingDepthDebuff.
- **Content/Buffs/MinecraftLegacySummonBuffs.cs** (50 linhas): Regista os buffs e debuffs agrupados em Minecraft Legacy Summon Buffs. Tipos: HappyCreeperMinionBuff, SquidKrakenMinionBuff.
- **Content/Buffs/MonthraButterflyBuff.cs** (27 linhas): Regista os buffs e debuffs agrupados em Monthra Butterfly Buff. Tipos: MonthraButterflyBuff.
- **Content/Buffs/ProgressionMinionBuff.cs** (28 linhas): Mantem os summons do catalogo ativos enquanto o jogador tiver pelo menos um minion. Tipos: ProgressionMinionBuff.
- **Content/Buffs/ShadowAscensionBuffs.cs** (103 linhas): Regista os buffs e debuffs agrupados em Shadow Ascension Buffs. Tipos: ShadowCombatHelper, ShadowRendDebuff, ShadowManaPotionBuff, ShadowMeleePotionBuff, ShadowCrystalMinionBuff, ShadowRendGlobalNPC.
- **Content/Buffs/ShadowWhipBuffs.cs** (63 linhas): Regista os buffs e debuffs agrupados em Shadow Whip Buffs. Tipos: RosalitaTagBuff, EclipsedMonthraTagBuff, ShadowTagBuff, ShadowWhipGlobalNpc.

### Content/Items

- **Content/Items/Accessories/CrystalineEye.cs** (30 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Eye. Tipos: CrystalineEye.
- **Content/Items/Armor/CrystalineDevour/CrystalineDevourArmorCommon.cs** (67 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Devour Armor Common. Tipos: CrystalineDevourArmorCommon, CrystalineDevourHelmetBase.
- **Content/Items/Armor/CrystalineDevour/CrystalineDevourBreastplate.cs** (26 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Devour Breastplate. Tipos: CrystalineDevourBreastplate.
- **Content/Items/Armor/CrystalineDevour/CrystalineDevourGreaves.cs** (26 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Devour Greaves. Tipos: CrystalineDevourGreaves.
- **Content/Items/Armor/CrystalineDevour/CrystalineDevourMagicHelm.cs** (23 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Devour Magic Helm. Tipos: CrystalineDevourMagicHelm.
- **Content/Items/Armor/CrystalineDevour/CrystalineDevourMeleeHelm.cs** (23 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Devour Melee Helm. Tipos: CrystalineDevourMeleeHelm.
- **Content/Items/Armor/CrystalineDevour/CrystalineDevourRangedHelm.cs** (27 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Devour Ranged Helm. Tipos: CrystalineDevourRangedHelm.
- **Content/Items/Armor/CrystalineDevour/CrystalineDevourSummonerHelm.cs** (24 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Devour Summoner Helm. Tipos: CrystalineDevourSummonerHelm.
- **Content/Items/Armor/Rosalita/RosalitaArmor.cs** (125 linhas): Reune valores, receitas e efeitos dos itens de Rosalita Armor. Tipos: RosalitaArmorCommon, RosalitaHelmet, at, RosalitaBreastplate, RosalitaGreaves.
- **Content/Items/Armor/Shadow/ShadowArmor.cs** (131 linhas): Reune valores, receitas e efeitos dos itens de Shadow Armor. Tipos: ShadowArmorCommon, ShadowHelmet, at, ShadowBreastplate, ShadowGreaves.
- **Content/Items/Consumables/CrystalinePotion.cs** (40 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Potion. Tipos: CrystalinePotion.
- **Content/Items/Materials/CrystalineTear.cs** (21 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Tear. Tipos: CrystalineTear.
- **Content/Items/Materials/MonthraScale.cs** (20 linhas): Reune valores, receitas e efeitos dos itens de Monthra Scale. Tipos: MonthraScale.
- **Content/Items/MinecraftLegacy/MinecraftLegacyCombatItems.cs** (150 linhas): Reune valores, receitas e efeitos dos itens de Minecraft Legacy Combat Items. Tipos: KrakenTear, FrozenFlame, ShadowCreeperHead, EndernmonScale, HappyCreeperStaff, SquidKrakenStaff.
- **Content/Items/MinecraftLegacy/MinecraftLegacyItems.cs** (363 linhas): Reune valores, receitas e efeitos dos itens de Minecraft Legacy Items. Tipos: MinecraftLegacyItemBase, MinecraftLegacyMaterialItemBase, MinecraftLegacyPlaceableItemBase, MinecraftLegacySwordBase, MinecraftLegacyPickaxeBase, MinecraftLegacyAxeBase.
- **Content/Items/Progression/Generated/AccessoryItems.cs** (130 linhas): Declara os itens accessory do catalogo; o comportamento fica concentrado na classe base. Tipos: TimberCharm, GelbornBand, DemonEmblem, HivePendant, DungeonCompass, InfernalHeart.
- **Content/Items/Progression/Generated/ConsumableItems.cs** (55 linhas): Declara os itens consumable do catalogo; o comportamento fica concentrado na classe base. Tipos: SurfaceSurvivalTonic, GelbornRecoveryPotion, DemonicBattleDraught, HiveReflexElixir, DungeonSightPotion, InfernalGuardFlask.
- **Content/Items/Progression/Generated/MagicItems.cs** (230 linhas): Declara os itens magic do catalogo; o comportamento fica concentrado na classe base. Tipos: TimberScepter, TimberGrimoire, GelbornScepter, GelbornGrimoire, DemonScepter, DemonGrimoire.
- **Content/Items/Progression/Generated/MaterialItems.cs** (55 linhas): Declara os itens material do catalogo; o comportamento fica concentrado na classe base. Tipos: SurfaceFiber, GelatinousPearl, DemonicAlloy, HiveResin, DungeonRelic, MoltenCore.
- **Content/Items/Progression/Generated/MeleeItems.cs** (255 linhas): Declara os itens melee do catalogo; o comportamento fica concentrado na classe base. Tipos: TimberSabre, TimberPike, GelbornSabre, GelbornPike, GelbornChakram, DemonSabre.
- **Content/Items/Progression/Generated/RangedItems.cs** (230 linhas): Declara os itens ranged do catalogo; o comportamento fica concentrado na classe base. Tipos: TimberLongbow, TimberCarbine, GelbornLongbow, GelbornCarbine, DemonLongbow, DemonCarbine.
- **Content/Items/Progression/Generated/SummonItems.cs** (230 linhas): Declara os itens summon do catalogo; o comportamento fica concentrado na classe base. Tipos: TimberIdol, TimberLash, GelbornIdol, GelbornLash, DemonIdol, DemonLash.
- **Content/Items/Progression/Generated/ToolItems.cs** (80 linhas): Declara os itens tool do catalogo; o comportamento fica concentrado na classe base. Tipos: IronrootPickaxe, DemonDrill, HiveAxe, DungeonHammer, InfernalPickaxe, CobaltExcavator.
- **Content/Items/Progression/ProgressionItemBases.cs** (263 linhas): Implementa o comportamento comum das oito familias de itens de progressao. Tipos: ProgressionCatalogItem, ProgressionMeleeItem, ProgressionRangedItem, ProgressionMagicItem, ProgressionSummonItem, ProgressionAccessoryItem.
- **Content/Items/Progression/ProgressionItemCatalog.cs** (437 linhas): Centraliza os 245 itens de progressao e evita repetir formulas de balanceamento. Tipos: ProgressionItemKind, struct, ProgressionItemCatalog.
- **Content/Items/ShadowBiome/RosalitaArsenal.cs** (448 linhas): Reune valores, receitas e efeitos dos itens de Rosalita Arsenal. Tipos: RosalitaRecipeCommon, RosalitaPickaxe, RosalitaAxe, RosalitaHammer, RosalitaBlade, RosalitaBow.
- **Content/Items/ShadowBiome/ShadowAscensionItems.cs** (522 linhas): Reune valores, receitas e efeitos dos itens de Shadow Ascension Items. Tipos: ShadowAscensionRecipeHelper, GodnessAnvil, HeartOfTheGod, HeartOfShadows, GloryBoots, ShadowManaPotion.
- **Content/Items/ShadowBiome/ShadowBiomeCoreItems.cs** (131 linhas): Reune valores, receitas e efeitos dos itens de Shadow Biome Core Items. Tipos: ShadowBiomeItemBase, ShadowBiomePlaceableItemBase, ShadowDirtBlock, ShadowStoneBlock, ShadowWood, ShadowScrap.
- **Content/Items/Summons/CrystalineSigil.cs** (62 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Sigil. Tipos: CrystalineSigil.
- **Content/Items/Summons/KrakenSummonItem.cs** (71 linhas): Reune valores, receitas e efeitos dos itens de Kraken Summon Item. Tipos: KrakenSummonItem.
- **Content/Items/Weapons/Magic/CrystalineStaff.cs** (61 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Staff. Tipos: CrystalineStaff.
- **Content/Items/Weapons/Magic/MonthraWand.cs** (55 linhas): Reune valores, receitas e efeitos dos itens de Monthra Wand. Tipos: MonthraWand.
- **Content/Items/Weapons/Melee/CrystalineSword.cs** (58 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Sword. Tipos: CrystalineSword.
- **Content/Items/Weapons/Melee/MonthraBlade.cs** (64 linhas): Reune valores, receitas e efeitos dos itens de Monthra Blade. Tipos: MonthraBlade.
- **Content/Items/Weapons/Ranged/CrystalineGun.cs** (57 linhas): Reune valores, receitas e efeitos dos itens de Crystaline Gun. Tipos: CrystalineGun.
- **Content/Items/Weapons/Ranged/MonthraBow.cs** (61 linhas): Reune valores, receitas e efeitos dos itens de Monthra Bow. Tipos: MonthraBow.
- **Content/Items/Weapons/Summon/MonthraButterflyStaff.cs** (61 linhas): Reune valores, receitas e efeitos dos itens de Monthra Butterfly Staff. Tipos: MonthraButterflyStaff.

### Content/NPCs

- **Content/NPCs/Critters/MonthraButterfly.cs** (104 linhas): Implementa a IA, animacao e regras dos NPCs de Monthra Butterfly. Tipos: MonthraButterfly.
- **Content/NPCs/Kraken/KrakenBoss.cs** (1388 linhas): Implementa as fases, movimentos, ataques e desenho do Alien Kraken. Tipos: KrakenBoss.
- **Content/NPCs/Kraken/KrakenClone.cs** (130 linhas): Implementa a IA, animacao e regras dos NPCs de Kraken Clone. Tipos: KrakenClone.
- **Content/NPCs/Kraken/KrakenCrystalTurret.cs** (141 linhas): Implementa a IA, animacao e regras dos NPCs de Kraken Crystal Turret. Tipos: KrakenCrystalTurret.
- **Content/NPCs/Kraken/KrakenMinion.cs** (110 linhas): Implementa a IA, animacao e regras dos NPCs de Kraken Minion. Tipos: KrakenMinion.
- **Content/NPCs/Kraken/KrakenTentacle.cs** (255 linhas): Implementa a IA, animacao e regras dos NPCs de Kraken Tentacle. Tipos: KrakenTentacle.
- **Content/NPCs/MinecraftLegacy/MinecraftLegacyNpcs.cs** (493 linhas): Implementa a IA, animacao e regras dos NPCs de Minecraft Legacy Npcs. Tipos: LegacyMobMovementStyle, MinecraftLegacyNpcBase, LegacyGroundHostileNpcBase, LegacyFlyingHostileNpcBase, LegacyGroundPassiveNpcBase, LegacyFlyingPassiveNpcBase.
- **Content/NPCs/Monthra/MonthraMothMinion.cs** (77 linhas): Implementa a IA, animacao e regras dos NPCs de Monthra Moth Minion. Tipos: MonthraMothMinion.
- **Content/NPCs/ShadowBiome/ShadowBiomeNpcs.cs** (180 linhas): Implementa a IA, animacao e regras dos NPCs de Shadow Biome Npcs. Tipos: ShadowBiomeNpcBase, ShadowGroundHostileBase, ShadowFlyingHostileBase, Phantasm, ShadowEye, ShadowSlime.

### Content/Players

- **Content/Players/CrystalinePlayer.cs** (163 linhas): Guarda e atualiza efeitos aplicados ao jogador em Crystaline Player. Tipos: CrystalineDevourSetType, CrystalinePlayer.
- **Content/Players/MinecraftLegacyEffectPlayer.cs** (37 linhas): Guarda e atualiza efeitos aplicados ao jogador em Minecraft Legacy Effect Player. Tipos: MinecraftLegacyEffectPlayer.
- **Content/Players/ShadowAscensionPlayer.cs** (57 linhas): Guarda e atualiza efeitos aplicados ao jogador em Shadow Ascension Player. Tipos: ShadowAscensionPlayer.
- **Content/Players/ShadowBiomePlayer.cs** (43 linhas): Guarda e atualiza efeitos aplicados ao jogador em Shadow Biome Player. Tipos: ShadowBiomePlayer.

### Content/Projectiles

- **Content/Projectiles/Hostile/MinecraftLegacyHostileProjectiles.cs** (104 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Minecraft Legacy Hostile Projectiles. Tipos: MinecraftLegacyHostileProjectileBase, SnowBlazeFrostFireball, SnowBlazeIceShard, SquidKrakenWaterBolt, KrakenLightningBolt.
- **Content/Projectiles/Hostile/MonthraFireball.cs** (80 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Monthra Fireball. Tipos: MonthraFireball.
- **Content/Projectiles/Hostile/MonthraFireballHoming.cs** (89 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Monthra Fireball Homing. Tipos: MonthraFireballHoming.
- **Content/Projectiles/Hostile/MonthraPrismaticLance.cs** (62 linhas): Implementa as lancas prismaticas finas e curvas da Monthra. Tipos: MonthraPrismaticLance.
- **Content/Projectiles/Hostile/MonthraPrismaticRay.cs** (111 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Monthra Prismatic Ray. Tipos: MonthraPrismaticRay.
- **Content/Projectiles/KrakenAbyssTether.cs** (118 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Abyss Tether. Tipos: KrakenAbyssTether.
- **Content/Projectiles/KrakenAbyssVortex.cs** (97 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Abyss Vortex. Tipos: KrakenAbyssVortex.
- **Content/Projectiles/KrakenHomingLightning.cs** (120 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Homing Lightning. Tipos: KrakenHomingLightning.
- **Content/Projectiles/KrakenHypnosisVortex.cs** (107 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Hypnosis Vortex. Tipos: KrakenHypnosisVortex.
- **Content/Projectiles/KrakenLightningStrike.cs** (116 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Lightning Strike. Tipos: KrakenLightningStrike.
- **Content/Projectiles/KrakenRedBolt.cs** (83 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Red Bolt. Tipos: KrakenRedBolt.
- **Content/Projectiles/KrakenRotatingLaser.cs** (156 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Rotating Laser. Tipos: KrakenRotatingLaser.
- **Content/Projectiles/KrakenSkyBeam.cs** (132 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Sky Beam. Tipos: KrakenSkyBeam.
- **Content/Projectiles/KrakenTrackingLaser.cs** (145 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Tracking Laser. Tipos: KrakenTrackingLaser.
- **Content/Projectiles/KrakenWaterDrop.cs** (94 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Water Drop. Tipos: KrakenWaterDrop.
- **Content/Projectiles/KrakenWaterJet.cs** (94 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Water Jet. Tipos: KrakenWaterJet.
- **Content/Projectiles/Magic/CrystalineBoltProjectile.cs** (65 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Crystaline Bolt Projectile. Tipos: CrystalineBoltProjectile.
- **Content/Projectiles/Magic/MonthraMagicFireball.cs** (90 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Monthra Magic Fireball. Tipos: MonthraMagicFireball.
- **Content/Projectiles/Magic/RosalitaMagicBolt.cs** (49 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Rosalita Magic Bolt. Tipos: RosalitaMagicBolt.
- **Content/Projectiles/Magic/ShadowBoltProjectile.cs** (70 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Shadow Bolt Projectile. Tipos: ShadowBoltProjectile.
- **Content/Projectiles/Melee/CrystalineSwordProjectile.cs** (158 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Crystaline Sword Projectile. Tipos: CrystalineSwordProjectile.
- **Content/Projectiles/Melee/KrakenGuardianBolt.cs** (41 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Kraken Guardian Bolt. Tipos: KrakenGuardianBolt.
- **Content/Projectiles/Melee/MonthraBladeProjectile.cs** (63 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Monthra Blade Projectile. Tipos: MonthraBladeProjectile.
- **Content/Projectiles/Melee/ShadowWhips.cs** (152 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Shadow Whips. Tipos: ShadowWhipProjectileBase, RosalitaWhipProjectile, EclipsedMonthraWhipProjectile, ShadowWhipProjectile.
- **Content/Projectiles/Melee/ShadowZenithProjectile.cs** (148 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Shadow Zenith Projectile. Tipos: ShadowZenithProjectile.
- **Content/Projectiles/Progression/ProgressionMinionProjectile.cs** (98 linhas): Implementa tres comportamentos de summon sem exigir sprites definitivas. Tipos: ProgressionMinionProjectile.
- **Content/Projectiles/Progression/ProgressionWeaponProjectile.cs** (71 linhas): Partilha a logica dos ataques de melee, magic e dos tiros dos minions. Tipos: ProgressionWeaponProjectile.
- **Content/Projectiles/Progression/ProgressionWhipProjectile.cs** (28 linhas): Fornece um whip funcional comum aos quinze tiers de lashes. Tipos: ProgressionWhipProjectile.
- **Content/Projectiles/Ranged/ShadowProjectiles.cs** (97 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Shadow Projectiles. Tipos: ShadowArrowProjectile, ShadowBulletProjectile.
- **Content/Projectiles/Summon/MinecraftLegacyMinions.cs** (213 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Minecraft Legacy Minions. Tipos: HappyCreeperMinion, SquidKrakenMinion, SquidKrakenMinionWaterBolt.
- **Content/Projectiles/Summon/MonthraButterflyMinion.cs** (123 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Monthra Butterfly Minion. Tipos: MonthraButterflyMinion.
- **Content/Projectiles/Summon/ShadowCrystalMinion.cs** (120 linhas): Controla movimento, dano e efeitos visuais dos projecteis de Shadow Crystal Minion. Tipos: ShadowCrystalMinion, ShadowCrystalBolt.

### Content/SceneEffects

- **Content/SceneEffects/KrakenSceneEffect.cs** (30 linhas): Liga a musica e o filtro espectral Moon Lord ao evento do Kraken. Tipos: KrakenSceneEffect.

### Content/Scenes

- **Content/Scenes/CrystalineDevourerScene.cs** (40 linhas): Ativa a musica e o ceu do Crystaline apenas durante o encontro. Tipos: CrystalineDevourerScene.
- **Content/Scenes/MonthraScene.cs** (41 linhas): Ativa a musica, o filtro e o ceu rosa durante a luta da Monthra. Tipos: MonthraScene.
- **Content/Scenes/ShadowBiome.cs** (23 linhas): Decide quando ativar a musica e os efeitos de cena de Shadow Biome. Tipos: ShadowBiome.

### Content/Tiles

- **Content/Tiles/CrystalineBarrierBlock.cs** (31 linhas): Define os blocos, colocacao e interacoes de Crystaline Barrier Block. Tipos: CrystalineBarrierBlock.
- **Content/Tiles/MinecraftLegacy/MinecraftLegacyTiles.cs** (108 linhas): Define os blocos, colocacao e interacoes de Minecraft Legacy Tiles. Tipos: MinecraftLegacyTileBase, RawAlexandriteBlockTile, GreystedWoodTile, ShadowBlockTile, ShadowOreTile, RosalitaOreTile.
- **Content/Tiles/ShadowBiome/GodnessAnvilTile.cs** (39 linhas): Define os blocos, colocacao e interacoes de Godness Anvil Tile. Tipos: GodnessAnvilTile.
- **Content/Tiles/ShadowBiome/ShadowBiomeTiles.cs** (112 linhas): Define os blocos, colocacao e interacoes de Shadow Biome Tiles. Tipos: ShadowBiomeSolidTileBase, ShadowDirtTile, ShadowGrassTile, ShadowStoneTile, ShadowWoodTile.

### Raiz

- **ChaoticDimensions.cs** (120 linhas): Liga o mod ao ciclo de vida do tModLoader e trata as mensagens de rede. Tipos: ChaoticDimensions, MessageType.
