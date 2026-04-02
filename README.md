

<div align="center">
  <img src=".github/readme/gallery/crystaline-devour-title.png" alt="Crystaline Devourer title card" width="100%" />
</div>

<p align="center">
  <img src=".github/readme/gallery/crystaline-devour-ring.png" alt="Crystaline Devourer arena showcase" width="49%" />
  <img src=".github/readme/gallery/crystaline-devour-blaster.png" alt="Crystaline Devourer sky beam showcase" width="49%" />
</p>

<p align="center">
  <strong>Chaotic Dimensions</strong> é um mod autoral para <strong>Terraria / tModLoader</strong>, desenvolvido como projeto de PAP e pensado como um universo próprio dentro do jogo, com bosses, biomas, progressão, sprites e documentação em crescimento constante.
  <br />
  <em>Chaotic Dimensions is an original Terraria / tModLoader mod project built as a long-term custom universe with its own bosses, biomes, progression, art pipeline and documentation.</em>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/tModLoader-Mod-7c3aed?style=for-the-badge" alt="tModLoader Mod" />
  <img src="https://img.shields.io/badge/Terraria-Original%20Content-a855f7?style=for-the-badge" alt="Original Content" />
  <img src="https://img.shields.io/badge/Status-Active%20Development-9333ea?style=for-the-badge" alt="Active Development" />
  <img src="https://img.shields.io/badge/Wiki-Live-7e22ce?style=for-the-badge" alt="Wiki Live" />
</p>

## Visão Geral

Chaotic Dimensions nasceu para ser mais do que um único boss ou um único bioma. A proposta do projeto é construir uma linha própria dentro do universo de Terraria, com encontros memoráveis, identidade visual forte, progressão autoral, documentação viva e espaço para expansão contínua.

O repositório já não representa apenas um protótipo. Hoje ele concentra:

- bosses jogáveis com apresentação própria
- biomas com worldgen dedicado
- tiers de progressão próprios
- armaduras, armas, acessórios, consumíveis, blocos e minérios originais
- pipeline de sprites e substituição de placeholders
- wiki pública sincronizada com o conteúdo do mod

## Conteúdo Atual

### Bosses principais

- `Monthra`
  - boss de progressão pré-hardmode
  - invocada através da `MonthraButterfly`
  - possui intro cinematográfica e música própria

- `Crystaline Devourer`
  - boss pós-`Moon Lord`
  - intro com title card, música própria, arena forçada e scene dedicada
  - abre o tier `Shadow`

### Progressão

- `Monthra`
  - introduz `MonthraScale`
  - abre a primeira linha de armas próprias do mod

- `Rosalita`
  - desbloqueada após os três mecânicos
  - adiciona minério, gema, arsenal e armadura acima do tier vanilla correspondente

- `Shadow`
  - ligada ao `ShadowBiome` e ao `Crystaline Devourer`
  - representa o tier mais forte atualmente implementado em código

### Biomas e worldgen

- `ShadowBiome`
  - substitui o segmento evil esquerdo do mundo
  - possui superfície e cavidades próprias
  - aplica trevas, drenagem de vida e redução de mobilidade sem o `ShadowTotem`
  - contém `ShadowOre`

- worldgen adicional para:
  - `RosalitaOre`
  - `ShadowOre`
  - blocos, tiles e estruturas ligadas ao conteúdo atual

### Itens e sistemas em destaque

- `CrystalineSigil`
- `ChaosCrystalPickaxe`
- `HeartOfTheGod`
- `ShadowTotem`
- `GodnessAnvil`
- linhas completas `Rosalita`, `Shadow` e `Eclipsed Monthra`
- scenes, músicas, title cards e auditoria de sprites

## Direção do Projeto

O foco atual do mod está em quatro pilares:

1. Criar bosses com presença forte, apresentação própria e leitura clara.
2. Expandir a progressão com tiers originais em vez de depender apenas de reskins.
3. Manter um pipeline visual consistente, mesmo quando parte do conteúdo ainda usa placeholder.
4. Documentar tudo publicamente na wiki para que o estado real do projeto seja sempre visível.

## Wiki Oficial

A wiki pública do projeto está aqui:

- [Chaotic Dimensions Wiki](https://hadryphern.github.io/Chaotic-Dimensions/?lang=pt-BR)

Ela faz parte do fluxo normal de desenvolvimento. Conteúdo novo no mod deve refletir-se também na wiki, mesmo quando a sprite final ainda não existe.

Documentação local importante:

- [ChaoticDimensionsWikiReference.md](docs/ChaoticDimensionsWikiReference.md)
- [WikiMaintenanceWorkflow.md](docs/WikiMaintenanceWorkflow.md)
- [SpriteAuditPendingAndReady.md](docs/SpriteAuditPendingAndReady.md)
- [ShadowBiomeSpriteRequirements.md](docs/ShadowBiomeSpriteRequirements.md)

## Estrutura do Repositório

```text
ChaoticDimensions/
|- Content/
|  |- BossBars/
|  |- Bosses/
|  |- Buffs/
|  |- Items/
|  |- NPCs/
|  |- Players/
|  |- Projectiles/
|  |- Scenes/
|  `- Tiles/
|- Common/
|  |- GlobalNPCs/
|  |- Graphics/
|  |- Menus/
|  |- Progression/
|  |- Systems/
|  `- Tiles/
|- Assets/
|- Sounds/
|- docs/
|- tools/
`- .github/readme/
```

## Build Local

```powershell
dotnet build ChaoticDimensions.csproj
```

O projeto foi pensado para desenvolvimento dentro de `tModLoader/ModSources`, por isso o fluxo normal é:

1. editar código e assets no repositório
2. compilar
3. abrir o mod no tModLoader
4. validar in-game
5. atualizar a wiki e a documentação auxiliar

## Fluxo de Assets e Wiki

Quando há conteúdo novo:

1. adicionar ou atualizar classes no mod
2. integrar sprites definitivas ou placeholders válidos
3. alinhar tamanho real dos sprites no código
4. atualizar documentação local
5. sincronizar imagens da wiki

Sincronização de assets da wiki:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\wiki\Sync-WikiAssets.ps1
```

## Estado Atual do Projeto

O projeto está em desenvolvimento ativo. Isso significa que:

- parte do conteúdo já está totalmente jogável
- parte já existe em código, mas ainda depende de sprite final ou balanceamento fino
- a wiki pode documentar conteúdo com placeholder, desde que isso fique claro
- sistemas experimentais que não se mostraram estáveis foram removidos em vez de forçados

Essa abordagem mantém o repositório mais honesto e mais útil para desenvolvimento contínuo.

## English Snapshot

Chaotic Dimensions is an original Terraria / tModLoader mod focused on building a custom universe rather than a one-off content pack. The current build already includes:

- the pre-hardmode boss `Monthra`
- the post-Moon Lord boss `Crystaline Devourer`
- the hostile `ShadowBiome`
- custom progression through `Rosalita` and `Shadow` tiers
- an active documentation workflow with a public wiki

The repository is meant to reflect the real state of the project, including placeholders, technical audits and ongoing content expansion.

## Créditos

- **Projeto:** `blueDev`
- **Plataforma:** `Terraria / tModLoader`
- **Documentação e wiki:** mantidas em paralelo com o desenvolvimento do mod
