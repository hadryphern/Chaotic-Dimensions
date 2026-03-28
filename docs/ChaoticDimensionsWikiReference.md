# Chaotic Dimensions Wiki Reference

Atualizado a partir do código do mod em `2026-03-28`.

Este documento foi escrito para servir como base de wiki/site. Ele descreve o conteúdo que já existe em código no mod atual, incluindo progressão, atributos, obtenção, drops, comportamento de mobs, lógica de worldgen e observações de balanceamento. Onde o conteúdo já existe em código mas ainda não possui rota natural de obtenção, isso é dito explicitamente.

## Escopo Desta Referência

Esta referência cobre:

- Bosses jogáveis do build atual.
- Mini-bosses e ameaças especiais.
- Mobs hostis, passivos e critters já implementados.
- Itens, materiais, armaduras, acessórios, consumíveis, armas, ferramentas e munições existentes.
- Biomas, blocos, minérios, geração de mundo e estações de crafting.
- Regras especiais como drenagem de vida, trevas, arena forçada e progressão destravada por boss.

Esta referência não finge que conteúdo futuro já existe. Se algo está reservado em código, placeholder, sem sprite final, sem boss de drop ou sem recipe ativa, isso aparece como nota de estado.

## Visão Geral da Progressão

### 1. Pré-Hardmode inicial

- O primeiro grande degrau novo do mod no pré-hardmode é a `Monthra`.
- A `Monthra` não é invocada por item craftável: ela aparece quando o jogador encontra e mata uma `MonthraButterfly`.
- A `MonthraButterfly` só começa a nascer depois de `Boss 2` do vanilla (`Eater of Worlds` ou `Brain of Cthulhu`, dependendo do mundo).
- O loot principal da `Monthra` é `MonthraScale`, usado para o primeiro conjunto de armas do mod pensado para esse estágio.

### 2. Hardmode intermediário

- Depois de derrotar os três mecânicos, o mundo é abençoado com `RosalitaOre`.
- `RosalitaOre` é o primeiro minério do mod com foco claro em power jump de hardmode avançado.
- A linha `Rosalita` mistura `RosalitaGem`, `ShadowScrap` e `Hallowed Bar`, entregando ferramentas e armas melhores que o kit hallowed padrão.

### 3. ShadowBiome

- O `ShadowBiome` nasce já na geração do mundo e substitui o segmento evil esquerdo.
- Ele tem superfície e um núcleo de cavernas próprios.
- Entrar nele sem proteção aplica `Darkness`, drenagem forte de vida e forte redução de mobilidade.
- A proteção é o acessório `ShadowTotem`.
- O minério `ShadowOre` só passa a nascer nesse bioma depois da morte do `Crystaline Devourer`.

### 4. Pós-Moon Lord

- O `Crystaline Devourer` é um boss pós-`Moon Lord`.
- Ele é invocado com `CrystalineSigil`.
- Depois de derrotá-lo, o jogador recebe acesso a:
- `ChaosCrystalPickaxe`
- `HeartOfTheGod`
- grande quantidade de `CrystalineTear`
- chance de `CrystalineEye`
- chance de `CrystalineSword`
- geração de `ShadowOre` dentro do `ShadowBiome`

### 5. Shadow Ascension

- O tier `Shadow` é o pico atual do mod.
- Ele usa `ShadowOre`, `ShadowScrap`, `SoulOfShadow` e `RosalitaGem`.
- Boa parte dessa linha já está completa em código, mas parte da progressão natural ainda depende do drop/entrada definitiva de `GodnessAnvil` e do fechamento dos bosses futuros.

## Bosses

## Monthra

### Função na progressão

`Monthra` é um boss de progressão pré-hardmode, feito para entrar depois do conteúdo evil do vanilla e antes de qualquer salto agressivo de poder. Ela serve como gate para as armas `Monthra`, que já começam a puxar o jogador para um estilo de dano mais largo e mais “fantasia”.

### Como invocar

- A `Monthra` nasce quando a `MonthraButterfly` morre.
- A `MonthraButterfly` só pode nascer depois de `NPC.downedBoss2`.
- Ela aparece no overworld ou no sky layer.
- Chance de spawn:
- dia: `0.012`
- noite: `0.018`
- Ela não nasce se houver `MonthraBoss` vivo.

### Stats

| Propriedade | Valor |
|---|---:|
| Vida | `22000` |
| Dano base | `30` |
| Defesa | `10` |
| Tamanho lógico | `340x260` |
| Escala visual | `0.48` |
| Frames | `12` |
| Música | `Boss 3` |

### Scaling

Em escalonamento de dificuldade/jogadores:

- vida é multiplicada por `balance * 0.88`
- dano é multiplicado por `0.95`

Isso segura a boss fight para coop sem explodir demais o power budget dela.

### Padrão de ataques

`Monthra` alterna entre dois estados:

#### Hover Volley

- ela se mantém lateralmente acima do jogador
- faz um movimento oscilante
- dispara rajadas de `MonthraFireball`
- intervalo:
- normal: a cada `50` ticks
- fase 2: a cada `36` ticks

Volleys:

- fase 1: `4` projéteis
- fase 2: `5` projéteis
- velocidade:
- fase 1: `8.75`
- fase 2: `10.15`
- spread:
- fase 1: `13 graus`
- fase 2: `16 graus`
- dano da volley:
- fase 1: `18`
- fase 2: `21`

#### Sweeping Burst

- ela cruza por cima/lado do jogador
- reposiciona para o lado oposto
- solta um projétil homing
- depois fecha o padrão com outra volley

Homing:

- aparece no tick `26`
- na fase 2 também aparece no tick `54`
- dano do homing: `19`

Volley final do sweep:

- fase 1: `5` projéteis
- fase 2: `6` projéteis
- velocidade:
- fase 1: `9.4`
- fase 2: `10.8`
- spread: `18 graus`
- dano:
- fase 1: `17`
- fase 2: `20`

### Fase 2

A fase 2 ativa abaixo de `50%` da vida.

Mudanças:

- movimento mais rápido
- mais projéteis por volley
- intervalos menores
- homing extra durante o sweep

### Projéteis

#### MonthraFireball

- projétil hostil
- tamanho: `24x24`
- colide com tile
- tempo de vida: `360`
- animação de `4` frames
- deixa trilha e poeira verde

#### MonthraFireballHoming

- projétil hostil
- tamanho: `24x24`
- não colide com tile
- tempo de vida: `420`
- animação de `4` frames
- busca o jogador ativo mais próximo
- corrige a trajetória com lerp leve

### Drops

Garantidos:

- `MonthraScale` x`22-30`
- `HealingPotion` x`5-10`

### Tática recomendada

- Lute em arena horizontal aberta com pelo menos dois andares.
- Não tente colar nela o tempo inteiro; a `Monthra` pune proximidade descuidada com spreads curtos.
- Na fase 2, a ameaça real é a sobreposição entre reposition e homing. Tenha espaço lateral suficiente.
- Arcos e armas de precisão com mobilidade são ideais.
- Se a luta estiver longa demais, o erro normalmente é falta de ritmo de desvio horizontal, não falta de dano bruto.

## Crystaline Devourer

### Função na progressão

`Crystaline Devourer` é o boss pós-`Moon Lord` que abre o tier `Shadow` e o late game próprio do mod. Ele é tratado como um encontro de arena forçada e como gate duro de progressão.

### Como invocar

Item: `CrystalineSigil`

Condições de uso:

- `Moon Lord` derrotado
- a intro do boss não pode estar ativa
- não pode haver `CrystalineDevourerHead` vivo

Recipe do `CrystalineSigil`:

- `CrystalShard` x`250`
- `LunarBar` x`250`
- `WormFood` ou `BloodySpine`
- `Lunar Crafting Station`

### Intro

Ao usar o sigilo:

- começa uma intro visual com title card
- a tela escurece
- a intro dura `600` ticks
- quando termina, o boss é spawnado sobre o jogador que ativou a sequência

### Estrutura do boss

O encontro real é composto por dois vermes completos:

- duas cabeças
- dezenas de segmentos de corpo
- duas caudas

Cada verme:

- tem vida individual de `2.500.000`

Vida combinada usada para thresholds de fase:

- `5.000.000`

### Arena

Quando o encontro começa:

- o sistema cria uma arena retangular fechada com `CrystalineBarrierBlock`
- semi-largura: `164 tiles`
- semi-altura: `104 tiles`
- sair da arena mata instantaneamente o jogador

Em outras palavras: esta luta não é feita para kite global pelo mapa. A arena é parte central do design.

### Stats da cabeça

| Propriedade | Valor |
|---|---:|
| Vida individual | `2.500.000` |
| Vida combinada do encontro | `5.000.000` |
| Dano base inicial | `260` |
| Defesa inicial | `5` |
| Tamanho | `96x96` |

### Stats dos segmentos

Corpo:

- dano inicial: `175`
- defesa inicial: `5`
- herda vida/estado da cabeça
- causa `Slow` ao acertar jogador

Cauda:

- dano inicial: `175`
- defesa inicial: `5`
- herda vida/estado da cabeça
- causa `Slow` ao acertar jogador

### Fases por vida combinada

| Fase | Threshold de vida combinada |
|---|---|
| 1 | acima de `85%` |
| 2 | `85%` a `75%` |
| 3 | `75%` a `70%` |
| 4 | `70%` a `65%` |
| 5 | `65%` a `60%` |
| 6 | abaixo de `60%` |

### Escalada ofensiva por fase

| Fase | Dano da cabeça | Defesa |
|---|---:|---:|
| 1 | `280` | `5` |
| 2 | `300` | `5` |
| 3 | `325` | `5` |
| 4 | `350` | `5` |
| 5 | `385` | `5` |
| 6 | `430` | `10` |

### Estados de combate

O boss alterna entre:

- `Orbit`
- `Dash`
- `SupremeLaser`

Além disso, pressiona com:

- bursts de shard
- pressão de projéteis
- sky beams nas fases mais altas

Detalhamento dessas pressões:

- `CrystalineShard` funciona como um estilhaço vivo auxiliar do encontro.
- ele nasce em leques disparados pela cabeça
- tem vida curta, mas corrige trajetória na direção do alvo
- aplica `Slow` ao acertar
- existe para negar linha reta, punir desvio preguiçoso e forçar microajustes entre um dash e outro
- `CrystalineDevourerSkyBeam` é um feixe massivo de arena
- ele nasce já com telegraph visível antes do dano real
- cruza a arena em linhas longas e pode aparecer inclinado, não apenas em eixo reto
- o número de beams sobe quando a vida combinada cai, então a luta fica mais sobre leitura de espaço do que sobre tank puro
- visualmente, ambos agora possuem sprite dedicada no projeto em vez de depender apenas de placeholder vanilla

### Drops

Garantidos:

- `CrystalineTear` x`250`
- `ChaosCrystalPickaxe` x`1`
- `HeartOfTheGod` x`1`

Chance:

- `CrystalineSword`: `1/40`
- `CrystalineEye`: `1/20`

### Tática recomendada

- Trate a arena como regra absoluta. Sair significa morte.
- O erro mais comum é lutar perto demais da borda.
- Quanto mais longa a luta, mais importante fica manter leitura de rota entre dash e pressão de projéteis.
- Como o boss é duplo, armas com alto uptime e mira estável costumam render melhor do que setups ultra burst com downtime alto.
- Defense e sustain continuam relevantes, mas o encontro é desenhado para punir falta de posicionamento mais do que falta de tank puro.
- Em multiplayer, divisão lateral e disciplina de espaço são mais importantes do que “stackar” todo mundo no mesmo ponto.

## Biomas e Geração de Mundo

## ShadowBiome

### Conceito

O `ShadowBiome` é um bioma secreto e agressivo que substitui o segmento evil esquerdo do mundo. Ele é um dos pilares do mid/late game do mod.

### Geração

- ele tenta localizar o primeiro segmento grande de `Corruption` ou `Crimson` no lado esquerdo do mapa
- se encontrar, substitui esse segmento
- se não encontrar, gera fallback numa faixa esquerda definida
- ele converte:
- chão de superfície
- uma banda vertical de solo/pedra
- um núcleo de cavernas

### O que ele converte

Solo convertido:

- `Dirt`
- `CorruptGrass`
- `CrimsonGrass`
- `Ebonsand`
- `Crimsand`

Pedra convertida:

- `Stone`
- `Ebonstone`
- `Crimstone`
- `CorruptIce`
- `FleshIce`

### Visual do bioma

- fundo roxo escuro
- música da `Corruption`
- árvores mortas simples de `ShadowWood`
- grama e terra próprias
- pedra própria

### Ativação do biome para efeitos

O jogo considera o bioma ativo quando:

- o jogador está entre `ShadowStartX` e `ShadowEndX`
- e existem pelo menos `140` shadow tiles contados ao redor

### Penalidade sem proteção

Sem `ShadowTotem`:

- aplica `Darkness`
- `lifeRegen` recebe penalidade de `-90`
- move speed multiplicada por `0.68`
- max run speed multiplicada por `0.72`
- run acceleration multiplicada por `0.7`

Na prática, o bioma drena vida muito rápido e reduz a capacidade de fuga. Ele é propositalmente letal sem o acessório certo.

### ShadowTotem

Tipo: acessório

Stats:

- defesa: `12`
- move speed: `+6%`
- endurance: `+4%`

Efeito principal:

- habilita `shadowBlessing`
- anula a maldição do `ShadowBiome`

### Mobs do bioma

Mobs exclusivos:

- `Phantasm`
- `ShadowEye`
- `ShadowSlime`
- `ShadowWorm`

Todos:

- despawnam se o alvo sair do bioma
- dropam `ShadowScrap`
- dropam bastante dinheiro
- dropam `SoulOfShadow` em hardmode

### ShadowOre

`ShadowOre` não nasce no mundo inicial.

Condição:

- matar `Crystaline Devourer`

Local:

- apenas dentro do `ShadowBiome`
- faixa subterrânea do bioma

Geração:

- número de tentativas proporcional à largura do bioma
- profundidade de `rockLayer + 55` até cerca de `+220`
- clusters fortes

Quebra:

- requer `300 pick power`
- pensado para `ChaosCrystalPickaxe`

Stats do tile:

- `MinPick = 300`
- `MineResist = 4.6`
- conta como minério real
- pode ser rastreado por Spelunker
- fadas podem puxar o jogador até ele

Observação visual e de uso:

- o minério possui item e presença de mundo separados
- o objetivo é fazer `ShadowOre` parecer um recurso de tier final do `ShadowBiome`, e não só um recolor de minério vanilla
- ele é tratado como o metal-base da ascensão `Shadow`

### RosalitaOre

Condição:

- hardmode ativo
- `Destroyer`, `Twins` e `Skeletron Prime` derrotados

Geração:

- abençoa o mundo uma única vez
- profundidade entre `44%` e `88%` da altura do mundo
- nasce no subterrâneo geral

Quebra:

- requer `180 pick power`
- `MineResist = 3.4`

Mensagem ao nascer:

- `Seu mundo foi abençoado com Minério de Rosalita!`

Leitura de progressão:

- `RosalitaOre` é o elo entre o hardmode vanilla alto e o late game próprio do mod
- ela prepara o jogador para receitas que já começam a conversar com materiais `Shadow`
- o minério também já tem sprite própria no projeto para reforçar a identidade do tier

Mensagem do `ShadowOre`:

- `Seu mundo foi abençoado com Minério de Sombra!`

## Blocos, Tiles e Estações

## ShadowBiome blocks

### ShadowDirtBlock / ShadowDirtTile

- bloco base do solo do `ShadowBiome`
- dropado ao minerar `ShadowGrassTile`
- hit sound de terra
- item e tile possuem identidade visual própria dentro do projeto atual

### ShadowGrassTile

- versão coberta de grama do `ShadowBiome`
- se houver bloco sólido acima, volta para `ShadowDirtTile`
- espalha horizontalmente para `ShadowDirtTile` exposto
- serve como leitura visual imediata de superfície corrompida pelo `ShadowBiome`

### ShadowStoneBlock / ShadowStoneTile

- pedra base do `ShadowBiome`
- dropa o item placeable correspondente
- foi pensada para vender a sensação de camada mineral morta/escurecida do bioma

### ShadowWood / ShadowWoodTile

- madeira roxa/morta do bioma
- usada para árvores do `ShadowBiome`
- funciona tanto como material de ambientação quanto como bloco utilitário placeable
- item e tile já possuem arte dedicada para manter coerência entre mundo e inventário

## Legacy blocks

### RawAlexandriteBlock

- bloco legacy existente no mod
- atualmente sem loop de obtenção claramente conectado ao worldgen atual

### GreystedWood

- madeira decorativa legacy

### ShadowBlock

- bloco legacy de sombra
- separado do `ShadowOre`

### BlueBerryPlant

- planta decorativa/colocável 2x2

## GodnessAnvil

`GodnessAnvil` é a estação de crafting do tier `Shadow`.

Ela funciona como:

- `Mythril Anvil`
- `Lunar Crafting Station`

Status atual:

- item e tile existem em código
- recipes que dependem dela já estão prontas
- a rota de aquisição natural ainda precisa do gancho final de boss/drop
- a estação agora já possui apresentação visual própria no projeto, tanto para o item quanto para a versão colocada no mundo

## Materiais e Recursos

## Materiais core de bosses

### MonthraScale

- rarity: `Orange`
- valor: `60 silver`
- drop principal da `Monthra`
- base do primeiro arsenal `Monthra`

### CrystalineTear

- rarity: `Purple`
- valor: `2 gold`
- material central do tier `Crystaline`
- dropado em grande quantidade pelo `Crystaline Devourer`
- visualmente é o recurso-símbolo do pós-`Moon Lord` atual do mod
- ele é o material que mais comunica ao jogador que o encontro `Crystaline Devourer` foi superado com sucesso

### KrakenTear

- rarity: `LightRed`
- valor: `75 silver`
- drop garantido do `Kraken`
- atualmente é um material solto, aguardando expansão de recipes

### FrozenFlame

- rarity: `LightRed`
- valor: `45 silver`
- drop do `SnowBlaze`
- atualmente funciona como material de progressão temática, ainda com pouco uso

### ShadowCreeperHead

- rarity: `Pink`
- valor: `2 gold`
- drop extremamente raro de `ShadowCreeper`
- material raro de sombra

### EndernmonScale

- rarity: `LightPurple`
- valor: `4 gold`
- drop de `Endernmon`
- material raro e valioso

## Materiais do ShadowBiome

### ShadowScrap

- rarity: `Pink`
- valor: `75 silver`
- drop padrão dos mobs hostis do `ShadowBiome`
- material-base do tier shadow/rosalita híbrido

### SoulOfShadow

- rarity: `Yellow`
- valor: `1 gold`
- só cai em hardmode
- drop adicional dos mobs do `ShadowBiome`
- usado no tier final `Shadow`

## Legacy materials

### Alexandrite

- item material legacy
- rarity: `Green`
- atualmente existe mais como recurso reservado do que como parte fechada da progressão ativa

### GlassStick

Recipe:

- `Glass` x`50`
- `Furnace`

Uso:

- componente de crafts legacy

### ChaosCrystal

- rarity: `Pink`
- valor: `2 gold`
- drop conhecido de `CrystalCreeper`
- observação importante: `CrystalCreeper` ainda não nasce naturalmente porque o bioma de cristal ainda não existe, então o material está codado, mas hoje não está naturalmente acessível sem spawn de teste

### ShadowGem

- rarity: `Orange`
- material legacy de sombra usado em receitas antigas

### ShadowNugget

- material legacy simples

### RosalitaGem

Como obter:

- fundir `RosalitaOre` x`4` na `Furnace`

É o refinamento principal do minério de Rosalita.

### VortexGem

- material de tier alto
- existe em código
- ainda sem rota natural de obtenção no loop atual

### BedrockStick

- haste legacy usada em armas shadow antigas

### IronStick

Recipe:

- `IronBar` x`50` ou `LeadBar` x`50`
- `Anvil`

### RatrixStick

Recipe:

- `RosalitaGem` x`10`
- `ShadowBar` x`10`
- `LunarBar` x`10`
- `ChaosCrystal` x`10`
- `Lunar Crafting Station`

### ShadowBar

Recipe:

- `RosalitaGem` x`1`
- `LunarBar` x`1`
- `HallowedBar` x`1`
- `Mythril Anvil`

## Armas, Ferramentas e Equipamentos

## Linha Monthra

As quatro armas `Monthra` são pensadas para o pós-evil pré-hardmode.

### MonthraBlade

- classe: melee
- dano: `29`
- knockback: `5.75`
- raridade: `Orange`
- valor: `1 gold`

Mecânica:

- não usa swing físico tradicional
- invoca `MonthraBladeProjectile`
- funciona como lâmina giratória em torno do jogador
- só pode existir uma ativa por vez

Recipes:

- `MonthraScale` x`12`
- `DemoniteBar` x`14`
- `ShadowScale` x`4`

ou

- `MonthraScale` x`12`
- `CrimtaneBar` x`14`
- `TissueSample` x`4`

### MonthraWand

- classe: magic
- dano: `25`
- mana: `6`
- use time: `18`
- shoot speed: `9.5`
- raridade: `Orange`

Dispara:

- `MonthraMagicFireball`

Recipes:

- `MonthraScale` x`11`
- `DemoniteBar` x`12`
- `ShadowScale` x`3`

ou versão crimson equivalente.

### MonthraBow

- classe: ranged
- dano: `24`
- use time: `21`
- ammo: arrows
- shoot speed: `10.25`
- raridade: `Orange`

Mecânica:

- dispara duas flechas por tiro
- spread leve de `-4` e `+4` graus

### MonthraButterflyStaff

- classe: summon
- dano: `18`
- mana: `10`
- raridade: `Orange`

Invoca:

- `MonthraButterflyMinion`

Características do minion:

- ocupa `0.5` slot
- contato direto
- procura alvos num raio de `620`
- fica orbitando próximo ao jogador quando não há alvo

## Linha Crystaline

## CrystalineEye

- acessório
- defesa: `25`
- raridade: `Red`
- valor: `3 platinum`

Efeito:

- habilita um teleporte por clique direito
- cooldown do blink: `36` frames
- busca área segura num raio de `8` tiles ao redor do ponto clicado
- aplica impulso forte após teleportar
- concede `CrystalineRushBuff` por `4` segundos

Buff de corrida:

- `+8 life regen`
- `+12% move speed`

## CrystalinePotion

- cura instantânea: `250 life`
- reduz potion sickness em `10` segundos
- raridade: `Purple`
- valor: `5 gold`

Ao beber:

- `CrystalinePotionRegenerationBuff` por `10` segundos
- `CrystalinePotionFortitudeBuff` por `5` minutos

Buffs:

- regen: `+18 life regen`
- fortitude: `+10 defense`

## CrystalineStaff

- classe: magic
- dano: `570`
- mana: `6`
- crit: `150`
- use time: `8`
- shoot speed: `21`
- raridade: `Purple`

Mecânica:

- dispara três bolts em leque curto
- ótimo DPS estável

Recipe:

- `CrystalineTear` x`50`
- `LastPrism`
- `LunarBar` x`20`
- `Lunar Crafting Station`

## CrystalineSword

- classe: melee
- dano: `1312`
- crit: `99`
- use time: `10`
- raridade: `Purple`

Mecânica:

- usa `CrystalineSwordProjectile`
- gera uma lâmina central e duas orbitais
- em acerto:
- cura `10 life` por hit

## CrystalineGun

- classe: ranged
- dano: `570`
- crit: `150`
- use time: `4`
- ammo: bullet
- consume ammo on last shot only
- raridade: `Purple`

Recipe:

- `CrystalineTear` x`50`
- `SDMG`
- `LunarBar` x`20`
- `Lunar Crafting Station`

## Linha Rosalita

`Rosalita` é um tier hardmode avançado híbrido, claramente acima de hallowed.

Recipe base das armas:

- `RosalitaGem`
- `ShadowScrap`
- `HallowedBar`
- `Mythril Anvil`

### RosalitaPickaxe

- dano: `66`
- pick power: `245`
- use time: `9`

### RosalitaAxe

- dano: `72`
- axe power: `42`
- use time: `11`

### RosalitaHammer

- dano: `84`
- hammer power: `105`
- use time: `12`

### RosalitaBlade

- dano: `108`
- melee swing tradicional

### RosalitaBow

- dano: `92`
- dispara duas flechas por uso
- spread leve

### RosalitaWand

- dano: `118`
- mana: `12`
- dispara `RosalitaMagicBolt`
- o projétil perfura `3` alvos e deixa poeira rosa/sombra

### RosalitaWhip

- dano base do item: `72`
- projectile whip com:
- `22` segmentos
- range multiplier `1.35`
- aplica `RosalitaTagBuff`

`RosalitaTagBuff`:

- dano adicional para minions: `14`

## Linha Eclipsed Monthra

Esta linha é a evolução exagerada do kit `Monthra`, usando `RosalitaGem`, `MonthraScale` e `ShadowScrap`.

### EclipsedMonthraPickaxe

- dano: `220`
- pick power: `275`

### EclipsedMonthraAxe

- dano: `240`
- axe power: `55`

### EclipsedMonthraHammer

- dano: `270`
- hammer power: `145`

### EclipsedMonthraBlade

- dano: `580`
- usa `MonthraBladeProjectile`
- só uma ativa por vez

### EclipsedMonthraBow

- dano: `480`
- dispara `4` flechas por uso

### EclipsedMonthraWand

- dano: `500`
- mana: `18`
- dispara `RosalitaMagicBolt` em versão muito mais forte

### EclipsedMonthraWhip

- dano base do whip: `360`
- `26` segmentos
- range multiplier `1.7`
- aplica `EclipsedMonthraTagBuff`

`EclipsedMonthraTagBuff`:

- dano adicional para minions: `28`

## Shadow Ascension

Recipe base:

- `ShadowOre`
- `ShadowScrap`
- `SoulOfShadow`
- `RosalitaGem`
- `GodnessAnvil`

### Observação importante de estado

Todos os itens abaixo existem em código, com stats e recipes definidos. Porém, como `GodnessAnvil` ainda não tem drop natural finalizado no loop atual, esta linha está funcional em código mas parcialmente bloqueada na progressão natural.

### ShadowSummonStaff

- classe: summon
- dano: `3600`
- mana: `20`
- raridade: `Red`

Invoca:

- `ShadowCrystalMinion`

Minion:

- ocupa `1` slot
- busca alvo a até `1100`
- dispara `ShadowCrystalBolt` a cada `16` ticks enquanto travado em alvo
- o bolt aplica `ShadowRend`

### ShadowWhip

- dano base: `750`
- whip de `30` segmentos
- range multiplier `2.05`
- aplica `ShadowTagBuff`
- também aplica `ShadowRend`

`ShadowTagBuff`:

- dano adicional para minions: `56`

### ShadowBow

- dano: `4500`
- use time: `18`
- dispara `10` flechas por uso
- usa `ShadowArrowProjectile`
- cada flecha aplica `ShadowRend` e `Confused`

### ShadowArrow

- dano do item: `512`
- craft: `200`
- recipe:
- `ShadowOre` x`3`
- `ShadowScrap` x`2`
- `GodnessAnvil`

Projétil:

- penetra `2`
- aplica `ShadowRend`

### ShadowBullet

- dano do item: `512`
- craft: `200`
- recipe igual à da flecha

Projétil:

- homing leve
- penetra `3`
- extra updates
- local immunity curta
- aplica `ShadowRend`

### ShadowStaff

- classe: magic
- dano: `3999`
- crit: `200`
- mana: `18`
- use time: `10`
- dispara `3` bolts com spread curto
- usa `ShadowBoltProjectile`

O `ShadowBoltProjectile`:

- penetra `6`
- faz homing
- aplica `ShadowRend`

### ShadowZenith

- classe: melee
- dano: `4300`
- crit: `200`
- use time: `10`
- usa `ShadowZenithProjectile`

Mecânica:

- cria uma espada central e duas orbitais
- o projétil aplica `ShadowRend`
- feito para ser o upgrade bruto da `Zenith`

### ShadowManaPotion

- cura `450 mana`
- duração do buff: `5 minutos`
- buff:
- `+40 defense`
- `+60 mana regen bonus`
- `-18% mana cost`

### ShadowMeleePotion

- duração do buff: `8 minutos`
- buff:
- `+135 defense`
- `+40% melee damage`
- `+28% melee attack speed`
- `+12% move speed`

### HeartOfShadows

- acessório
- defesa própria: `245`
- raridade: `Red`
- valor: `6 platinum`

Efeitos:

- ativa o teleporte do `CrystalineEye`
- `+260 max life`
- `+280 max mana`
- `+245 defense`
- `+115% generic damage`
- `+22% generic crit`
- `+28% melee attack speed`
- `+42% move speed`
- `+3.5 max run speed`
- `runAcceleration * 1.35`
- `+8 max minions`
- `+28% whip range`
- `+18% endurance`
- no knockback

Recipe:

- `ShadowOre` x`50`
- `ShadowScrap` x`23`
- `RosalitaGem` x`25`
- `HallowedBar` x`35`
- `MonthraScale` x`10`
- `RosalitaShield`
- `CrystalineEye`
- `ShadowMeleePotion`
- `GodnessAnvil`

### GloryBoots

- upgrade de `TerrasparkBoots`
- dá automaticamente a benção do `ShadowBiome`

Efeitos:

- `shadowBlessing`
- `+32% move speed`
- `accRunSpeed = 12.5`
- `rocketBoots = 4`
- water walk
- ice skate
- fire walk
- no fall damage
- lava rose
- `+8 segundos` de lava

Recipe:

- `TerrasparkBoots`
- `ShadowOre` x`150`
- `ShadowScrap` x`50`
- `RosalitaGem` x`25`
- `Soul of Night` x`36`
- `ShadowTotem`
- `GodnessAnvil`

### HeartOfTheGod

- consumível de vida permanente
- só pode ser usado depois de:
- consumir todos os `Life Crystals`
- consumir todos os `Life Fruits`

Limite:

- `2` usos
- cada uso: `+125 max life`
- total extra: `+250`

### GodnessAnvil

- estação placeable
- raridade: `Red`
- valor: `2 platinum`
- atualmente é item pronto em código, mas aguardando hook final de drop/aquisição

## Armaduras

## Crystaline Devour Armor

Peças comuns:

- `CrystalineDevourBreastplate`: defesa `40`
- `CrystalineDevourGreaves`: defesa `30`
- todos os helms: defesa base `29`

Recipes:

- peitoral: `CrystalineTear` x`34` + `LunarBar` x`18`
- greaves: `CrystalineTear` x`28` + `LunarBar` x`16`
- helms: `CrystalineTear` x`24` + `LunarBar` x`14`

Todos os sets:

- usam o mesmo peitoral e greaves
- ativam um estado no `CrystalinePlayer`
- ao tomar dano, concedem `CrystalineDevourAegisBuff` por `10` segundos
- cooldown interno da aegis: `3` minutos

Durante a aegis:

- imunidade quase total
- sem knockback
- mobilidade ampliada

### Magic Helm

Set bonus:

- `+78% magic damage`
- `-95% mana cost`
- `+20% move speed`
- `+0.75 max run speed`
- bonus defense do set: `51`

### Melee Helm

Set bonus:

- `+85% melee damage`
- `+35% melee attack speed`
- `+22% move speed`
- `+0.9 max run speed`
- bonus defense do set: `151`

### Ranged Helm

Set bonus:

- `+72% ranged damage`
- `+55% ranged attack speed`
- `+24% move speed`
- `+1.15 max run speed`
- ammoBox
- ammoPotion
- ammoCost80
- economia de munição adicional interna
- bonus defense do set: `111`

### Summoner Helm

Set bonus:

- `+155% summon damage`
- `+10 max minions`
- `+12% move speed`
- `+0.45 max run speed`
- `+25% whip range`
- bonus defense do set: `0`

## Rosalita Armor

Peças:

- `RosalitaHelmet`: defesa `26`
- `RosalitaBreastplate`: defesa `34`
- `RosalitaGreaves`: defesa `24`

Bonus por peça:

Helmet:

- `+8% generic damage`
- `+6% generic crit`

Breastplate:

- `+10% generic damage`
- `+1 max minions`
- `+30 max mana`

Greaves:

- `+12% move speed`
- `+0.8 max run speed`
- `+2 life regen`

Set bonus:

- `+18% generic damage`
- `+15% generic crit`
- `+14% move speed`
- `+12% whip range`
- `+1 max minions`
- `+60 max mana`
- `+5 life regen`
- `+18 defense`

## Shadow Armor

Peças:

- `ShadowHelmet`: defesa `52`
- `ShadowBreastplate`: defesa `70`
- `ShadowGreaves`: defesa `46`

Bonus por peça:

Helmet:

- `+14% generic damage`
- `+8% generic crit`

Breastplate:

- `+160 max life`
- `+80 max mana`
- `+2 max minions`
- `+8% endurance`

Greaves:

- `+18% move speed`
- `+1.8 max run speed`
- `runAcceleration * 1.24`
- `+4 life regen`

Set bonus:

- `+70 defense`
- `+38% generic damage`
- `+22% generic crit`
- `+25% melee attack speed`
- `+38 armor penetration`
- `+5 max minions`
- `+28% whip range`
- `+220 max mana`
- `+18% move speed`
- `+12 life regen`
- `+12% endurance`

## Legacy Arsenal

## ShadowSword

- dano: `26`
- knockback: `6.25`
- recipe:
- `ShadowGem` x`10`
- `BedrockStick` x`3`
- `Anvil`

## ShadowPickaxe

- dano: `14`
- pick power: `105`
- recipe:
- `ShadowGem` x`12`
- `BedrockStick` x`3`
- `Anvil`

## VortexBlade

- dano: `34`
- knockback: `6.8`
- use time: `18`
- recipe:
- `VortexGem` x`12`
- `ChaosCrystal` x`4`
- `GlassStick` x`6`
- `Mythril Anvil`

## RosalitaShield

- acessório
- defesa: `8`
- `+5% endurance`
- no knockback
- recipe:
- `RosalitaGem` x`18`
- `Mythril Anvil`

## Mobs e NPCs

## Critters e fauna passiva

### MonthraButterfly

- vida: `20`
- critter voador passivo
- invoca `Monthra` ao morrer

### BlueButterfly

- vida: `10`
- forest daytime
- spawn chance alta para ambientação: `0.18`
- sem drops

### GreenButterfly

- vida: `10`
- passiva

### RedButterfly

- vida: `10`
- floresta e neve durante o dia
- spawn chance `0.055`

### YellowButterfly

- vida: `10`
- floresta e neve durante o dia
- spawn chance `0.055`

### FireflyCritter

- vida: `8`
- critter noturno

### BigButterfly

- vida: `42`
- grande borboleta passiva

### AppleCow

- vida: `60`
- defesa: `2`
- drops: `Apple` x`1-3`

### GoldenAppleCow

- vida: `72`
- defesa: `3`
- drops: `Apple` x`2-4`

### CrystalAppleCow

- vida: `90`
- defesa: `4`
- drops:
- `Apple` x`1-3`
- `CrystalShard` x`1-2`

### CrystalGoldenAppleCow

- vida: `115`
- defesa: `6`
- drops:
- `Apple` x`2-4`
- `CrystalShard` x`2-4`

### DimensionPig

- vida: `52`
- defesa: `2`
- drops: `Bacon` x`1-2`

### AlessandraNpc

- vida: `250`
- defesa: `6`
- NPC social/passivo

## Hostis legacy

### HappyCreeper

- vida: `92`
- dano de contato: `18`
- defesa: `4`
- floresta ou neve
- explode após se aproximar do jogador
- dano da explosão: `9999`
- não quebra blocos

Drops:

- `LifeCrystal` `1/18`
- `HappyCreeperStaff` `1/180`

### WhiteCreeper

- vida: `108`
- dano: `20`
- defesa: `4`
- spawna no `Hallow`
- explosão `9999`
- quebra blocos

Drop:

- `PixieDust` x`3-8`

### CrystalCreeper

- vida: `126`
- dano: `22`
- defesa: `6`
- explosão `9999`
- spawn natural atual: `desativado`

Drop:

- `ChaosCrystal` `1/28`

### ShadowCreeper

- vida: `150`
- dano: `24`
- defesa: `7`
- spawna na `Corruption`
- explosão `9999`
- quebra blocos

Drop:

- `ShadowCreeperHead` `1/220`

### Endernmon

- vida: `380`
- dano de contato: `0`
- defesa: `16`
- noite apenas
- no máximo um por vez
- chance de spawn `0.009`

Comportamento:

- teleporta ao redor do jogador
- tenta manter distância entre `160` e `320` pixels
- não quer tocar o jogador; ele quer pressionar presença
- intensifica um efeito roxo de lentidão e drenagem

Efeito:

- move speed pode cair até `28%` do normal
- max run speed até `38%`
- run acceleration até `45%`
- life regen pode chegar a `-72`

Drop:

- `EndernmonScale` x`8-14`

Tática:

- não persiga corpo a corpo cego
- use burst ranged/magic e mate rápido
- quanto mais tempo perto, pior a arena fica para você

### SnowBlaze

- vida: `132`
- dano: `18`
- defesa: `8`
- bioma de neve

Ataques:

- `SnowBlazeFrostFireball`
- `SnowBlazeIceShard`

Drops:

- `IceBlade` `1/18`
- `IceSkates` `1/6`
- `FrozenFlame` x`2-5`

### SquidKraken

- vida: `64`
- dano: `4`
- defesa: `2`
- oceano, água
- spawn chance `0.22`

Comportamento:

- fica pulando/bobbing na água
- quase não representa ameaça direta
- dispara `SquidKrakenWaterBolt` de dano muito baixo

Drop:

- `SquidKrakenStaff` `1/1400`

Evento especial:

- ao morrer, se não houver `Kraken` ativo, invoca `Kraken`

### Kraken

- vida: `3600`
- dano: `42`
- defesa: `14`
- mini-boss voador

Comportamento:

- força chuva enquanto vivo
- orbita o jogador
- lança `KrakenLightningBolt`

Drops:

- `KrakenTear` x`10` garantido
- `SquidKrakenStaff` `1/90`
- `KrakenBlade` `1/18`

Tática:

- mantenha movimento horizontal constante
- evite lutar em terreno cheio de teto
- a chuva é mais temática; o perigo real é a pressão de raio e dano bruto

## Hostis do ShadowBiome

Todos os hostis abaixo:

- só aparecem no `ShadowBiome`
- somem se o jogador sair do biome
- dropam `ShadowScrap`
- dropam ouro
- dropam `SoulOfShadow` em hardmode

### Phantasm

- vida: `540`
- dano: `76`
- defesa: `18`
- voador
- superfície apenas
- spawn chance `0.075`

### ShadowEye

- vida: `620`
- dano: `82`
- defesa: `20`
- voador
- superfície: `0.05`
- subterrâneo: `0.06`

### ShadowSlime

- vida: `680`
- dano: `84`
- defesa: `22`
- superfície e subterrâneo: `0.045`

### ShadowWorm

- vida: `840`
- dano: `104`
- defesa: `26`
- subterrâneo apenas
- spawn chance `0.072`

### KrakenSquid

Esta é uma entidade diferente do `SquidKraken`.

- vida: `260`
- passiva
- oceano/praia com água
- spawn chance `0.12`
- não ataca
- serve como fauna marítima temática

## Summons, Minions e Armas especiais

## HappyCreeperStaff

- summon
- dano: `125`
- mana: `10`
- raridade: `Pink`

É um drop extremamente raro, mas muito forte para o estágio em que aparece.

## SquidKrakenStaff

- summon
- dano: `235`
- mana: `10`
- raridade: `Yellow`

Invoca minions aquáticos agressivos.

## KrakenBlade

- melee
- dano: `110`
- use time: `24`
- knockback: `7`
- raridade: `Yellow`

Efeito especial:

- ao usar, spawna `3` `KrakenGuardianBolt` ao redor do jogador
- funciona como proteção ofensiva em órbita curta

## Estado Atual de Conteúdo Bloqueado ou Reservado

Os itens abaixo existem em código, mas hoje dependem de gancho futuro para ficarem naturalmente jogáveis:

- `GodnessAnvil` ainda sem drop final
- toda a linha `Shadow Ascension` depende da bancada
- `ShadowTotem` precisa do drop final definitivo do boss planejado
- `ChaosCrystal` existe, mas `CrystalCreeper` ainda não tem bioma final para spawn natural
- alguns materiais legacy como `Alexandrite` e `VortexGem` existem, mas ainda não possuem ciclo completo dentro do build atual

## Recomendações para Uso na Wiki

Se este documento for quebrado em páginas:

- uma página para `Progressão`
- uma página para `Monthra`
- uma página para `Crystaline Devourer`
- uma página para `ShadowBiome`
- uma página para `Rosalita`
- uma página para `Shadow tier`
- uma página para `Legacy mobs`

Se você quiser, a próxima etapa pode ser eu transformar este material em:

- um `.docx` formatado para edição manual
- vários `.md` separados por página de wiki
- ou uma versão em JSON/YAML para alimentar o site automaticamente
