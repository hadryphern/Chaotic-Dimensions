import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const projectRoot = path.resolve(__dirname, "..", "..");
const docsRoot = path.join(projectRoot, "docs");
const docsAssetsRoot = path.join(docsRoot, "assets", "images", "content");
const outputPath = path.join(docsRoot, "generated-orespawn-data.js");

const assetMap = buildAssetMap(docsAssetsRoot);
const generatedEntries = [];
const entryById = new Map();
const recipesByResult = parseOreSpawnRecipes();

const gateOrder = {
  Anytime: 0,
  PostKingSlime: 10,
  PostEyeOfCthulhu: 20,
  PostEvilBoss: 30,
  PostQueenBee: 40,
  PostSkeletron: 50,
  PostWallOfFlesh: 60,
  PostAnyMech: 70,
  PostAllMechs: 80,
  PostPlantera: 90,
  PostGolem: 100,
  PostCultist: 110,
  PostMoonLord: 120,
  PostCrystalineDevourer: 130,
  PostChaoticApexOne: 140,
  PostChaoticApexTwo: 150,
  PostChaoticApexThree: 160,
  PostChaoticApexTrio: 170
};

const spawnLabels = {
  None: "Evento ou sistema",
  Ocean: "Oceano",
  ForestDay: "Floresta de dia",
  ForestNight: "Floresta a noite",
  ForestOrJungleDay: "Floresta ou Jungle de dia",
  JungleDay: "Jungle de dia",
  DesertDay: "Deserto de dia",
  DesertNight: "Deserto a noite",
  SurfaceNight: "Superficie a noite",
  Cavern: "Cavernas",
  Underground: "Subsolo",
  SnowSurface: "Neve de superficie",
  HallowNight: "Hallow a noite",
  CorruptNight: "Corrupcao a noite",
  CrimsonNight: "Crimson a noite",
  Sky: "Ceu",
  Dungeon: "Dungeon",
  Underworld: "Underworld"
};

const bossCategoryOverrides = new Map([
  ["mobzilla", "superbosses"],
  ["the-king", "superbosses"],
  ["the-queen", "superbosses"]
]);

const dimensionNotes = {
  Utopia: [
    "Estado atual: documentado no codigo e nas notas de design, mas a runtime de dimensoes ainda esta desligada.",
    "Foco de fantasia: paraiso tardio com arvores gigantescas e rota ligada ao The King.",
    "Alvo visual: superficie aberta com poucas arvores, mas cada uma absurdamente grande."
  ],
  Danger: [
    "Estado atual: documentado no codigo e nas notas de design, mas a runtime de dimensoes ainda esta desligada.",
    "Foco de fantasia: mapa de risco alto, densidade de dungeons e farme de minibosses.",
    "Alvo visual: mundo plano, estruturas dominando o mapa e pressao hostil constante."
  ],
  Mining: [
    "Estado atual: documentado no codigo e nas notas de design, mas a runtime de dimensoes ainda esta desligada.",
    "Foco de fantasia: dimensao de recursos com fauna OreSpawn e progressao de dinos.",
    "Alvo visual: camadas parecidas com um mundo vanilla, mas carregadas de minerios."
  ],
  Crystal: [
    "Estado atual: documentado no codigo e nas notas de design, mas a runtime de dimensoes ainda esta desligada.",
    "Foco de fantasia: mundo cristalino com tileset proprio e roster de mobs mais autoral.",
    "Alvo visual: grama, pedra, madeira, areia e folhas substituidas por variantes cristalinas."
  ],
  Village: [
    "Estado atual: dimensao adiada nas notas de rebuild, mas o evento de cerco noturno ja existe no catalogo.",
    "Foco de fantasia: mundo de vilarejo atacado por ondas roboticas."
  ],
  Chaos: [
    "Estado atual: dimensao adiada nas notas de rebuild.",
    "Foco de fantasia: desafio tardio com identidade imprevisivel e mais agressiva."
  ]
};

const workstationEntries = [
  createManualEntry({
    id: "anvil",
    category: "blocks",
    sortOrder: 3950,
    title: "Anvil",
    summary: "Bancada basica usada em crafts iniciais e intermediarios.",
    overview: "Entrada de referencia usada pela wiki para tornar as estacoes clicaveis dentro das receitas.",
    facts: ["Tipo: crafting station", "Faixa: progressao inicial"],
    meta: { vanillaAlias: "Anvil", wikiSource: "Anvil" }
  }),
  createManualEntry({
    id: "mythril-anvil",
    category: "blocks",
    sortOrder: 3951,
    title: "Mythril Anvil",
    summary: "Bancada avancada de Hardmode usada por varios crafts OreSpawn.",
    overview: "A wiki usa esta entrada para ligar receitas com Mythril Anvil ou Orichalcum Anvil para uma pagina dedicada.",
    facts: ["Tipo: hardmode crafting station", "Alias: Orichalcum Anvil"],
    meta: { vanillaAlias: "Mythril Anvil", wikiSource: "Hardmode Anvils" }
  }),
  createManualEntry({
    id: "adamantite-forge",
    category: "blocks",
    sortOrder: 3952,
    title: "Adamantite Forge",
    summary: "Forja de Hardmode usada em refinamento tardio de recursos.",
    overview: "Usada em cadeias de craft como Uranium Nugget dentro da linha OreSpawn atual.",
    facts: ["Tipo: hardmode crafting station"],
    meta: { vanillaAlias: "Adamantite Forge", wikiSource: "Adamantite Forge" }
  }),
  createManualEntry({
    id: "furnace",
    category: "blocks",
    sortOrder: 3953,
    title: "Furnace",
    summary: "Estacao basica de fusao usada em recursos iniciais.",
    overview: "Esta entrada serve para transformar a bancada em pagina clicavel e melhorar a leitura das receitas.",
    facts: ["Tipo: crafting station"],
    meta: { vanillaAlias: "Furnace", wikiSource: "Furnace" }
  }),
  createManualEntry({
    id: "ancient-manipulator",
    category: "blocks",
    sortOrder: 3954,
    title: "Ancient Manipulator",
    summary: "Bancada do final do jogo usada por crafts ligados a bossing e tecnologia lunar.",
    overview: "Alguns crafts OreSpawn e Crystaline usam esta estacao como ponto alto de progressao.",
    facts: ["Tipo: crafting station", "Faixa: endgame"],
    meta: { vanillaAlias: "Ancient Manipulator", wikiSource: "Ancient Manipulator" }
  }),
  createManualEntry({
    id: "lunar-crafting-station",
    category: "blocks",
    sortOrder: 3955,
    title: "Lunar Crafting Station",
    summary: "Alias de documentacao para a Ancient Manipulator em receitas mais descritivas.",
    overview: "Mantem compatibilidade com o jeito como a wiki descreve certos crafts de endgame.",
    facts: ["Tipo: alias de crafting station", "Referente a: Ancient Manipulator"],
    meta: { vanillaAlias: "Ancient Manipulator", wikiSource: "Ancient Manipulator" }
  })
];

buildGeneratedEntries();
writeOutput();

function buildGeneratedEntries() {
  workstationEntries.forEach(registerEntry);

  parseItemCatalog().forEach(registerEntry);
  parseResourceItems().forEach(registerEntry);
  parseTransportItems().forEach(registerEntry);
  parsePrototypeSummons().forEach(registerEntry);
  parseBossBetaSummons().forEach(registerEntry);
  parseMobCatalog().forEach(registerEntry);
  parseBossCatalog().forEach(registerEntry);
  parseDimensionCatalog().forEach(registerEntry);
  parseEventCatalog().forEach(registerEntry);
  parseFamilyPages().forEach(registerEntry);
  parseGroupPages().forEach(registerEntry);

  attachRecipesToEntries();
  attachDropSourcesToItems();
  wireRelatedEntries();
}

function registerEntry(entry) {
  if (!entry) {
    return;
  }

  const existing = entryById.get(entry.id);
  if (!existing) {
    entryById.set(entry.id, entry);
    generatedEntries.push(entry);
    return;
  }

  existing.related = [...new Set([...(existing.related ?? []), ...(entry.related ?? [])])];
  existing.sortOrder = Math.min(Number(existing.sortOrder ?? 0), Number(entry.sortOrder ?? 0));
  existing.image = existing.image || entry.image;
  existing.banner = existing.banner || entry.banner || "";
  existing.category = entry.category || existing.category;
  existing.isPublished = existing.isPublished ?? entry.isPublished ?? true;
  existing.content = mergeContentObjects(existing.content, entry.content);
}

function parseItemCatalog() {
  const text = readProjectFile("Common/OreSpawn/OreSpawnItemCatalog.cs");
  const regex = /public static readonly OreSpawnItemDefinition\s+(\w+)\s*=\s*(\w+)\(([\s\S]*?)\);/g;
  const results = [];

  for (const match of text.matchAll(regex)) {
    const definitionKey = match[1];
    const factory = match[2];
    const args = splitTopLevel(match[3]);
    const title = readQuoted(args[1]);
    const rarity = humanizeEnum(args[6]);
    const facts = [
      `Tipo: ${itemKindLabel(factory)}`,
      `Damage: ${cleanNumeric(args[4])}`,
      `Use time: ${cleanNumeric(args[5])}`,
      `Raridade: ${rarity}`,
      `Knockback: ${cleanNumeric(args[8])}`
    ];

    if (factory === "Magic") {
      facts.push(`Mana: ${cleanNumeric(args[9])}`);
      facts.push(`Velocidade do projetil: ${cleanNumeric(args[11])}`);
    }

    if (factory === "Ranged") {
      const ammoArg = args.find((arg) => arg.includes("AmmoID."));
      if (ammoArg) {
        facts.push(`Usa municao: ${humanizeEnum(ammoArg)}`);
      }
      facts.push(`Velocidade do projetil: ${cleanNumeric(args[10])}`);
    }

    if (factory === "Tool") {
      const hammer = extractNamedNumeric(args, "hammer");
      const axe = extractNamedNumeric(args, "axe");
      const pick = extractNamedNumeric(args, "pick");
      if (hammer) {
        facts.push(`Hammer power: ${hammer}`);
      }
      if (axe) {
        facts.push(`Axe power: ${axe}`);
      }
      if (pick) {
        facts.push(`Pick power: ${pick}`);
      }
    }

    results.push(createEntry({
      id: slugify(title),
      category: factory === "Tool" ? "tools" : "weapons",
      sortOrder: 500 + results.length,
      image: findLocalAsset(`${definitionKey}.png`),
      title,
      summary: `${title} ja existe no catalogo OreSpawn do mod e agora ganhou uma pagina propria na wiki.`,
      overview: `Entrada gerada automaticamente a partir de OreSpawnItemCatalog. ${extractNamedString(args, "notes") || "Os valores ainda podem mudar conforme o balanceamento do mod evolui."}`,
      facts,
      notes: compactList([
        extractNamedString(args, "notes"),
        "Os dados desta pagina foram puxados do catalogo OreSpawn implementado no proprio mod."
      ]),
      meta: {
        sourceCatalog: "OreSpawnItemCatalog",
        oreSpawnKey: definitionKey
      }
    }));
  }

  return results;
}

function parseResourceItems() {
  const text = readProjectFile("Content/Items/Materials/OreSpawn/OreSpawnResourceItems.cs");
  const blocks = extractClassBlocks(text, "OreSpawnResourceItemBase");

  return blocks.map((block, index) => {
    const className = block.name;
    const title = humanizeIdentifier(className);
    const rarityMatch = block.body.match(/ItemRarityID\.(\w+)/);
    const placeTile = block.body.match(/PlaceTileType\s*=>\s*ModContent\.TileType<(\w+)>/);
    const facts = compactList([
      rarityMatch ? `Raridade: ${humanizeEnum(rarityMatch[1])}` : "",
      placeTile ? `Coloca tile: ${humanizeIdentifier(placeTile[1].replace(/Tile$/, ""))}` : ""
    ]);

    return createEntry({
      id: slugify(title),
      category: /(Block|Nest|Log)$/i.test(className) ? "blocks" : "materials",
      sortOrder: 900 + index,
      image: findLocalAsset(`${className}.png`),
      title,
      summary: `${title} faz parte da linha de recursos OreSpawn ja presente no codigo do mod.`,
      overview: `Entrada gerada automaticamente a partir dos itens de recurso OreSpawn. ${placeTile ? "Tambem possui versao colocavel como bloco ou tile." : "Serve como material ou recurso utilitario."}`,
      facts,
      notes: [
        "As receitas e usos desta pagina sao atualizados a partir do proprio codigo C# do item."
      ],
      meta: {
        sourceCatalog: "OreSpawnResourceItems",
        oreSpawnKey: className
      }
    });
  });
}

function parseTransportItems() {
  const text = readProjectFile("Content/Items/OreSpawn/OreSpawnTransportCritterItems.cs");
  const regex = /public sealed class (\w+) : OreSpawnTransportCritterItemBase[\s\S]*?TargetDimension => OreSpawnDimensionId\.(\w+);/g;
  const results = [];

  for (const match of text.matchAll(regex)) {
    const className = match[1];
    const dimensionKey = match[2];
    const title = humanizeIdentifier(className.replace(/Item$/, ""));
    const dimensionSlug = slugify(dimensionKey === "Crystal" ? "Crystal Dimension" : `${dimensionKey} Dimension`);

    results.push(createEntry({
      id: slugify(title),
      category: "systems",
      sortOrder: 1200 + results.length,
      image: findLocalAsset(`${className}.png`),
      related: [dimensionSlug],
      title,
      summary: `${title} e um item de transporte OreSpawn ja ligado ao sistema de viagem entre dimensoes.`,
      overview: `Entrada gerada automaticamente a partir do sistema de transporte OreSpawn. Ao usar este item, o jogador tenta viajar para ${dimensionKey}.`,
      facts: [
        "Tipo: transport item",
        `Destino: ${dimensionKey} Dimension`,
        "Estado atual: depende do runtime de dimensoes OreSpawn estar ativo"
      ],
      notes: [
        "O item usa OreSpawnDimensionTravel.RequestTransportFromClient no codigo atual."
      ],
      meta: {
        sourceCatalog: "OreSpawnTransportCritterItems",
        oreSpawnKey: className
      }
    }));
  }

  return results;
}

function parsePrototypeSummons() {
  const files = [
    "Content/Items/Summons/OreSpawn/CaterkillerBait.cs",
    "Content/Items/Summons/OreSpawn/EmperorScorpionIdol.cs",
    "Content/Items/Summons/OreSpawn/HerculesTotem.cs",
    "Content/Items/Summons/OreSpawn/CephadromeCaller.cs"
  ];

  return files.map((filePath, index) => {
    const text = readProjectFile(filePath);
    const className = /public sealed class (\w+)/.exec(text)?.[1] ?? path.basename(filePath, ".cs");
    const targetNpc = /TargetNpcType => ModContent\.NPCType<(\w+)>\(\)/.exec(text)?.[1] ?? "";
    const gate = /RequiredGate => ChaoticProgressionGate\.(\w+)/.exec(text)?.[1] ?? "Anytime";
    const title = humanizeIdentifier(className);
    const targetTitle = humanizeIdentifier(targetNpc);

    return createEntry({
      id: slugify(title),
      category: "summons",
      sortOrder: gateSortOrder(gate, 1400 + index),
      image: findLocalAsset(`${className}.png`),
      related: targetNpc ? [slugify(targetTitle)] : [],
      title,
      summary: `${title} e o item atual usado para chamar ${targetTitle} na linha prototipo OreSpawn.`,
      overview: `Entrada gerada automaticamente a partir do summon item ja implementado no mod. Ele exige o gate ${humanizeIdentifier(gate)} para funcionar.`,
      facts: [
        "Tipo: summon item",
        `Gate: ${humanizeIdentifier(gate)}`,
        `Alvo: ${targetTitle}`
      ],
      notes: [
        "As receitas desta pagina sao lidas diretamente do metodo AddRecipes do item."
      ],
      meta: {
        sourceCatalog: "OreSpawnPrototypeSummonItem",
        progressionGate: gate,
        progressionGroup: gateToGroup(gate),
        oreSpawnKey: className
      }
    });
  });
}

function parseBossBetaSummons() {
  const text = readProjectFile("Content/Items/Summons/OreSpawn/OreSpawnBossBetaSummons.cs");
  const regex = /public sealed class (\w+) : OreSpawnBossBetaSummon \{ protected override int TargetNpcType => ModContent\.NPCType<(\w+)>\(\); \}/g;
  const results = [];

  for (const match of text.matchAll(regex)) {
    const className = match[1];
    const targetBoss = match[2];
    const title = humanizeIdentifier(className);
    const bossTitle = targetBoss === "Wtf" ? "WTF?" : humanizeIdentifier(targetBoss);
    results.push(createEntry({
      id: slugify(title),
      category: "summons",
      sortOrder: gateSortOrder("PostChaoticApexTrio", 1500 + results.length),
      image: findLocalAsset(`${className}.png`),
      related: [slugify(bossTitle)],
      title,
      summary: `${title} e o summon beta ja incluido no mod para iniciar o encontro de ${bossTitle}.`,
      overview: "Entrada gerada automaticamente a partir de OreSpawnBossBetaSummons. No codigo atual esses summons nao possuem receita publica e funcionam como itens de teste/beta para os encontros.",
      facts: [
        "Tipo: boss summon beta",
        "Gate: Post Chaotic Apex Trio",
        `Alvo: ${bossTitle}`
      ],
      notes: [
        "O item ja existe no jogo mesmo antes da etapa final de polish da wiki."
      ],
      meta: {
        sourceCatalog: "OreSpawnBossBetaSummons",
        progressionGate: "PostChaoticApexTrio",
        progressionGroup: "post_moonlord",
        oreSpawnKey: className
      }
    }));
  }

  return results;
}

function parseMobCatalog() {
  const text = readProjectFile("Common/OreSpawn/OreSpawnMobCatalog.cs");
  const regex = /public static readonly OreSpawnMobDefinition\s+(\w+)\s*=\s*(Ambient|Flyer|Walker|Hopper|Burrower|Companion)\(([\s\S]*?)\)(?:\s*with\s*\{([\s\S]*?)\})?;/g;
  const results = [];

  for (const match of text.matchAll(regex)) {
    const definitionKey = match[1];
    const factory = match[2];
    const args = splitTopLevel(match[3]);
    const withBlock = match[4] ?? "";
    const title = readQuoted(args[1]);
    const gate = extractMobGate(factory, args);
    const spawnKind = extractMobSpawn(factory, args);
    const damage = extractMobArg(factory, args, "damage");
    const defense = extractMobArg(factory, args, "defense");
    const life = extractMobArg(factory, args, "life");
    const miniBoss = factory !== "Ambient" && factory !== "Companion" && args.some((arg) => arg.trim() === "true");
    const eventKey = /RequiredEventKey\s*=\s*OreSpawnWorldCatalog\.(\w+)/.exec(withBlock)?.[1] ?? "";
    const drops = parseDropBlock(withBlock);
    const notes = extractLastString(args);
    const category = factory === "Companion"
      ? "npcs"
      : miniBoss
        ? "minibosses"
        : "mobs";

    const facts = compactList([
      `Gate: ${humanizeIdentifier(gate)}`,
      spawnKind ? `Spawn: ${spawnLabels[spawnKind] ?? humanizeIdentifier(spawnKind)}` : "",
      damage ? `Damage: ${damage}` : "",
      defense ? `Defense: ${defense}` : "",
      life ? `Life: ${life}` : "",
      factory ? `Arquetipo: ${factoryLabel(factory)}` : ""
    ]);

    const detailNotes = compactList([
      notes,
      eventKey ? `Aparece apenas quando o sistema ${humanizeIdentifier(eventKey.replace(/Key$/, ""))} estiver ativo.` : ""
    ]);

    results.push(createEntry({
      id: slugify(title),
      category,
      sortOrder: gateSortOrder(gate, 1600 + results.length),
      image: findLocalAsset(`${definitionKey}.png`),
      title,
      summary: buildMobSummary(factory, title, gate, spawnKind, miniBoss),
      overview: `Entrada gerada automaticamente a partir de OreSpawnMobCatalog. ${notes || "Os comportamentos ainda podem mudar conforme o beta evolui."}`,
      facts,
      drops,
      notes: detailNotes,
      meta: {
        sourceCatalog: "OreSpawnMobCatalog",
        progressionGate: gate,
        progressionGroup: gateToGroup(gate),
        featureInProgression: shouldFeatureInProgression(category, factory, spawnKind),
        spawnKind,
        oreSpawnKey: definitionKey,
        eventKey
      }
    }));
  }

  return results;
}

function parseBossCatalog() {
  const text = readProjectFile("Common/OreSpawn/OreSpawnMobCatalog.cs");
  const block = /public static IReadOnlyList<OreSpawnBossDefinition> AllBosses \{ get; \} = new\[\] \{([\s\S]*?)\};/.exec(text)?.[1] ?? "";
  const regex = /Boss\("([^"]+)",\s*"([^"]+)",\s*OreSpawnBossArchetype\.(\w+),\s*\d+,\s*\d+,\s*(\d+),\s*(\d+),\s*(\d+),\s*MusicID\.\w+,\s*[^,]+,\s*"([^"]*)"\)/g;
  const results = [];

  for (const match of block.matchAll(regex)) {
    const key = match[1];
    const title = match[2];
    const archetype = match[3];
    const life = match[4];
    const damage = match[5];
    const defense = match[6];
    const notes = match[7];
    const slug = slugify(title);
    const category = bossCategoryOverrides.get(slug) ?? "bosses";

    results.push(createEntry({
      id: slug,
      category,
      sortOrder: gateSortOrder("PostChaoticApexTrio", 2500 + results.length),
      image: findLocalAsset(`${key}.png`),
      title,
      summary: `${title} ja existe como encontro beta no mod e agora entrou na wiki com pagina propria.`,
      overview: `Entrada gerada automaticamente a partir do catalogo de bosses OreSpawn. ${notes}`,
      facts: [
        "Gate: Post Chaotic Apex Trio",
        `Arquetipo: ${humanizeIdentifier(archetype)}`,
        `Life: ${life}`,
        `Damage: ${damage}`,
        `Defense: ${defense}`
      ],
      notes: compactList([
        notes,
        "As sprites ainda podem ser placeholders enquanto o encounter continua em beta."
      ]),
      meta: {
        sourceCatalog: "OreSpawnBossCatalog",
        progressionGate: "PostChaoticApexTrio",
        progressionGroup: "post_moonlord",
        featureInProgression: true,
        oreSpawnKey: key
      }
    }));
  }

  return results;
}

function parseDimensionCatalog() {
  const text = readProjectFile("Common/OreSpawn/OreSpawnWorldCatalog.cs");
  const regex = /new OreSpawnDimensionDefinition\("([^"]+)",\s*"([^"]+)",\s*ChaoticProgressionGate\.(\w+),\s*"([^"]+)"\)/g;
  const results = [];

  for (const match of text.matchAll(regex)) {
    const key = match[1];
    const title = match[2];
    const gate = match[3];
    const summary = match[4];
    results.push(createEntry({
      id: slugify(title),
      category: "dimensions",
      sortOrder: gateSortOrder(gate, 3000 + results.length),
      title,
      summary,
      overview: `Entrada gerada automaticamente a partir de OreSpawnWorldCatalog para documentar a dimensao ${title}.`,
      facts: [
        `Gate: ${humanizeIdentifier(gate)}`,
        "Estado: documentada no codigo",
        "Runtime atual: dimensoes OreSpawn desativadas enquanto a arquitetura final nao volta"
      ],
      notes: dimensionNotes[key] ?? [
        "A dimensao existe no catalogo atual do mod, mesmo com a runtime ainda desativada."
      ],
      meta: {
        sourceCatalog: "OreSpawnWorldCatalog",
        progressionGate: gate,
        progressionGroup: gateToGroup(gate),
        oreSpawnKey: key
      }
    }));
  }

  return results;
}

function parseEventCatalog() {
  const text = readProjectFile("Common/OreSpawn/OreSpawnWorldCatalog.cs");
  const regex = /new OreSpawnEventDefinition\(([^,]+),\s*"([^"]+)",\s*ChaoticProgressionGate\.(\w+),\s*"([^"]+)"(?:,\s*(true|false))?(?:,\s*(true|false))?\)/g;
  const results = [];

  for (const match of text.matchAll(regex)) {
    const constantRef = match[1].trim();
    const title = match[2];
    const gate = match[3];
    const summary = match[4];

    results.push(createEntry({
      id: slugify(title),
      category: "events",
      sortOrder: gateSortOrder(gate, 3400 + results.length),
      title,
      summary,
      overview: `Entrada gerada automaticamente a partir de OreSpawnWorldCatalog para documentar o sistema ${title}.`,
      facts: [
        `Gate: ${humanizeIdentifier(gate)}`,
        `Chave interna: ${constantRef.replace(/Key$/, "")}`
      ],
      notes: [
        "Esta pagina representa um sistema real do catalogo OreSpawn usado para orientar a expansao do mod."
      ],
      meta: {
        sourceCatalog: "OreSpawnWorldCatalog",
        progressionGate: gate,
        progressionGroup: gateToGroup(gate),
        oreSpawnKey: constantRef.replace(/Key$/, "")
      }
    }));
  }

  return results;
}

function parseFamilyPages() {
  const text = readProjectFile("Common/OreSpawn/OreSpawnItemCatalog.cs");
  const block = /public static IReadOnlyList<OreSpawnFamilyDefinition> FamilyPages \{ get; \} = new\[\] \{([\s\S]*?)\};/.exec(text)?.[1] ?? "";
  const regex = /new OreSpawnFamilyDefinition\("([^"]+)",\s*"([^"]+)",\s*"([^"]+)"\)/g;
  const results = [];

  for (const match of block.matchAll(regex)) {
    const key = match[1];
    const title = match[2];
    const summary = match[3];
    results.push(createEntry({
      id: slugify(title),
      category: classifyFamilyCategory(title),
      sortOrder: 3800 + results.length,
      title,
      summary,
      overview: "Pagina de familia OreSpawn gerada a partir do catalogo do mod. Serve como hub organizado para agrupar esta linha de conteudo enquanto as entradas concretas continuam crescendo.",
      facts: [
        "Tipo: family page",
        `Chave interna: ${key}`
      ],
      notes: [
        "Estas paginas ajudam a wiki a ficar escalavel no estilo OreSpawn original, com hubs de familias e sets."
      ],
      meta: {
        sourceCatalog: "OreSpawnItemCatalog",
        oreSpawnKey: key
      }
    }));
  }

  return results;
}

function parseGroupPages() {
  const text = readProjectFile("Common/OreSpawn/OreSpawnMobCatalog.cs");
  const block = /public static IReadOnlyList<OreSpawnFamilyDefinition> GroupPages \{ get; \} = new\[\] \{([\s\S]*?)\};/.exec(text)?.[1] ?? "";
  const regex = /new OreSpawnFamilyDefinition\("([^"]+)",\s*"([^"]+)",\s*"([^"]+)"\)/g;
  const results = [];

  for (const match of block.matchAll(regex)) {
    const key = match[1];
    const title = match[2];
    const summary = match[3];
    results.push(createEntry({
      id: slugify(title),
      category: "mobs",
      sortOrder: 4100 + results.length,
      title,
      summary,
      overview: "Pagina de grupo OreSpawn gerada a partir do catalogo atual do mod para organizar variantes e familias inteiras.",
      facts: [
        "Tipo: group page",
        `Chave interna: ${key}`
      ],
      notes: [
        "Serve como ponto de expansao para quebrar variantes individuais depois sem perder a navegacao da wiki."
      ],
      meta: {
        sourceCatalog: "OreSpawnMobCatalog",
        oreSpawnKey: key
      }
    }));
  }

  return results;
}

function attachRecipesToEntries() {
  for (const [resultKey, recipeLines] of recipesByResult.entries()) {
    const entry = findEntryForResult(resultKey);
    if (!entry || recipeLines.length === 0) {
      continue;
    }

    for (const languageCode of Object.keys(entry.content)) {
      if (languageCode.startsWith("_")) {
        continue;
      }

      const languageContent = entry.content[languageCode];
      languageContent.crafting = [...new Set([...(languageContent.crafting ?? []), ...recipeLines])];
    }
  }
}

function attachDropSourcesToItems() {
  const reverseDrops = new Map();

  generatedEntries.forEach((entry) => {
    const content = entry.content?.["pt-BR"];
    const drops = content?.drops ?? [];
    const title = content?.title ?? entry.id;
    drops.forEach((line) => {
      const { label, amount } = parseDropLine(line);
      if (!label) {
        return;
      }
      const key = slugify(label);
      const sourceLine = `${title}${amount ? ` dropa ${label} ${amount}` : ` dropa ${label}`}`;
      if (!reverseDrops.has(key)) {
        reverseDrops.set(key, []);
      }
      reverseDrops.get(key).push(sourceLine);
    });
  });

  reverseDrops.forEach((sourceLines, key) => {
    const entry = entryById.get(key) ?? findEntryByTitle(key);
    if (!entry) {
      return;
    }

    for (const languageCode of Object.keys(entry.content)) {
      if (languageCode.startsWith("_")) {
        continue;
      }

      const content = entry.content[languageCode];
      content.obtain = [...new Set([...(content.obtain ?? []), ...sourceLines])];
    }
  });
}

function wireRelatedEntries() {
  generatedEntries.forEach((entry) => {
    (entry.related ?? []).forEach((relatedId) => {
      const relatedEntry = entryById.get(relatedId);
      if (relatedEntry) {
        pushRelated(relatedEntry, entry.id);
      }
    });

    const meta = entry.content?._meta ?? {};
    if (meta.eventKey) {
      const eventEntry = entryById.get(slugify(humanizeIdentifier(meta.eventKey.replace(/Key$/, ""))));
      if (eventEntry) {
        pushRelated(entry, eventEntry.id);
        pushRelated(eventEntry, entry.id);
      }
    }
  });
}

function pushRelated(entry, relatedId) {
  if (!entry || !relatedId) {
    return;
  }

  entry.related = [...new Set([...(entry.related ?? []), relatedId])];
}

function findEntryForResult(resultKey) {
  const normalizedResult = normalizeKey(resultKey);
  return generatedEntries.find((entry) => {
    const title = entry.content?.["pt-BR"]?.title ?? "";
    const meta = entry.content?._meta ?? {};
    return normalizeKey(entry.id) === normalizedResult
      || normalizeKey(title) === normalizedResult
      || normalizeKey(meta.oreSpawnKey) === normalizedResult;
  });
}

function findEntryByTitle(slug) {
  return generatedEntries.find((entry) => normalizeKey(entry.id) === normalizeKey(slug));
}

function parseOreSpawnRecipes() {
  const recipeMap = new Map();
  const sourceFiles = [
    ...walkFiles(path.join(projectRoot, "Content", "Items", "Materials", "OreSpawn"), ".cs"),
    ...walkFiles(path.join(projectRoot, "Content", "Items", "Summons", "OreSpawn"), ".cs")
  ];

  for (const filePath of sourceFiles) {
    const text = fs.readFileSync(filePath, "utf8");
    const lines = text.split(/\r?\n/);
    let currentClass = "";
    let activeRecipe = null;

    for (const line of lines) {
      const classMatch = line.match(/public sealed class (\w+)/);
      if (classMatch) {
        currentClass = classMatch[1];
      }

      const trimmed = line.trim();
      const createRecipeMatch = trimmed.match(/^CreateRecipe(?:\((\d+)\))?/);
      const staticCreateMatch = trimmed.match(/^Recipe\.Create\(ModContent\.ItemType<(\w+)>\(\)(?:,\s*(\d+))?\)/);
      if (createRecipeMatch) {
        activeRecipe = {
          result: currentClass,
          ingredients: [],
          tiles: []
        };
      } else if (staticCreateMatch) {
        activeRecipe = {
          result: staticCreateMatch[1],
          ingredients: [],
          tiles: []
        };
      }

      if (!activeRecipe) {
        continue;
      }

      const genericIngredient = trimmed.match(/\.AddIngredient<(\w+)>\((\d+)\)/);
      const genericIngredientSingle = trimmed.match(/\.AddIngredient<(\w+)>\(\)/);
      const itemIngredient = trimmed.match(/\.AddIngredient\(ItemID\.(\w+)(?:,\s*(\d+))?\)/);
      const modContentIngredient = trimmed.match(/\.AddIngredient\(ModContent\.ItemType<(\w+)>\(\)(?:,\s*(\d+))?\)/);
      const tileIngredient = trimmed.match(/\.AddTile\(TileID\.(\w+)\)/);

      if (genericIngredient) {
        activeRecipe.ingredients.push(`${humanizeIdentifier(genericIngredient[1])} x${genericIngredient[2]}`);
      } else if (genericIngredientSingle) {
        activeRecipe.ingredients.push(humanizeIdentifier(genericIngredientSingle[1]));
      } else if (itemIngredient) {
        const amount = itemIngredient[2] ? ` x${itemIngredient[2]}` : "";
        activeRecipe.ingredients.push(`${humanizeIdentifier(itemIngredient[1])}${amount}`);
      } else if (modContentIngredient) {
        const amount = modContentIngredient[2] ? ` x${modContentIngredient[2]}` : "";
        activeRecipe.ingredients.push(`${humanizeIdentifier(modContentIngredient[1])}${amount}`);
      } else if (tileIngredient) {
        activeRecipe.tiles.push(humanizeTileId(tileIngredient[1]));
      }

      if (trimmed.startsWith(".Register();")) {
        const recipeLine = buildRecipeLine(activeRecipe.ingredients, activeRecipe.tiles);
        if (recipeLine) {
          if (!recipeMap.has(activeRecipe.result)) {
            recipeMap.set(activeRecipe.result, []);
          }
          recipeMap.get(activeRecipe.result).push(recipeLine);
        }
        activeRecipe = null;
      }
    }
  }

  return recipeMap;
}

function buildRecipeLine(ingredients, tiles) {
  const cleanedIngredients = ingredients.filter(Boolean);
  if (cleanedIngredients.length === 0) {
    return "";
  }

  const line = cleanedIngredients.join(" + ");
  if (tiles.length === 0) {
    return line;
  }

  return `${line} at ${tiles.join(" / ")}`;
}

function parseDropBlock(block) {
  const regex = /new OreSpawnDropDefinition\(\(\) => (?:(?:ModContent\.ItemType<(\w+)>\(\))|(?:ItemID\.(\w+))),\s*[^,]+,\s*(\d+),\s*(\d+)\)/g;
  const drops = [];
  for (const match of block.matchAll(regex)) {
    const itemName = match[1] || match[2];
    const min = match[3];
    const max = match[4];
    drops.push(`${humanizeIdentifier(itemName)} x${min}${min !== max ? `-${max}` : ""}`);
  }
  return drops;
}

function parseDropLine(line) {
  const match = String(line ?? "").trim().match(/^(.+?)\s+x(\d+(?:-\d+)?)$/i);
  return {
    label: match?.[1]?.trim() ?? "",
    amount: match?.[2] ?? ""
  };
}

function extractClassBlocks(text, baseClass) {
  const regex = new RegExp(`public sealed class (\\w+) : ${baseClass}[\\s\\S]*?(?=\\n\\tpublic sealed class |\\n}\\s*$)`, "g");
  const results = [];
  for (const match of text.matchAll(regex)) {
    results.push({
      name: match[1],
      body: match[0]
    });
  }
  return results;
}

function extractMobGate(factory, args) {
  if (factory === "Ambient") {
    return "Anytime";
  }
  if (factory === "Companion") {
    return "PostChaoticApexTrio";
  }
  return args[3]?.trim().replace("ChaoticProgressionGate.", "") || "Anytime";
}

function extractMobSpawn(factory, args) {
  if (factory === "Companion") {
    return "None";
  }
  return args[2]?.trim().replace("OreSpawnSpawnKind.", "") || "None";
}

function extractMobArg(factory, args, field) {
  const indexMap = {
    Ambient: { damage: 5, defense: 6, life: 7 },
    Companion: { damage: 3, defense: 4, life: 5 },
    Flyer: { damage: 6, defense: 7, life: 8 },
    Walker: { damage: 6, defense: 7, life: 8 },
    Hopper: { damage: 6, defense: 7, life: 8 },
    Burrower: { damage: 6, defense: 7, life: 8 }
  };
  const index = indexMap[factory]?.[field];
  return index === undefined ? "" : cleanNumeric(args[index]);
}

function buildMobSummary(factory, title, gate, spawnKind, miniBoss) {
  if (factory === "Companion") {
    return `${title} ja existe no catalogo atual como companion ou battle mob ligado a sistemas OreSpawn.`;
  }
  const gateLabel = humanizeIdentifier(gate);
  const spawnLabel = spawnLabels[spawnKind] ?? humanizeIdentifier(spawnKind);
  if (miniBoss) {
    return `${title} ja esta no mod como miniboss OreSpawn com gate ${gateLabel} e rota de spawn em ${spawnLabel.toLowerCase()}.`;
  }
  return `${title} ja esta no mod como criatura OreSpawn com gate ${gateLabel} e spawn em ${spawnLabel.toLowerCase()}.`;
}

function shouldFeatureInProgression(category, factory, spawnKind) {
  if (category === "bosses" || category === "superbosses" || category === "minibosses") {
    return true;
  }
  if (category !== "mobs") {
    return false;
  }
  return factory !== "Ambient" && factory !== "Companion" && spawnKind !== "None";
}

function itemKindLabel(factory) {
  const labels = {
    Melee: "Melee weapon",
    Magic: "Magic weapon",
    Ranged: "Ranged weapon",
    Tool: "Tool"
  };
  return labels[factory] ?? factory;
}

function factoryLabel(factory) {
  const labels = {
    Ambient: "Criatura ambiente",
    Flyer: "Voador",
    Walker: "Terrestre",
    Hopper: "Saltador",
    Burrower: "Escavador",
    Companion: "Companion"
  };
  return labels[factory] ?? factory;
}

function classifyFamilyCategory(title) {
  if (/Armor/i.test(title)) {
    return "armor";
  }
  if (/Tools|Weapons/i.test(title)) {
    return "weapons";
  }
  if (/Food/i.test(title)) {
    return "consumables";
  }
  if (/Materials|Rocks|Crystals|Plants|Trees/i.test(title)) {
    return "materials";
  }
  return "systems";
}

function gateToGroup(gate) {
  const order = gateOrder[gate] ?? 0;
  if (order < gateOrder.PostWallOfFlesh) {
    return "pre_hardmode";
  }
  if (order < gateOrder.PostMoonLord) {
    return "pre_moonlord";
  }
  return "post_moonlord";
}

function gateSortOrder(gate, fallback) {
  return (gateOrder[gate] ?? 999) * 10 + fallback;
}

function humanizeTileId(value) {
  const labels = {
    Anvils: "Anvil",
    MythrilAnvil: "Mythril Anvil",
    Furnaces: "Furnace",
    AdamantiteForge: "Adamantite Forge"
  };
  return labels[value] ?? humanizeIdentifier(value);
}

function createManualEntry({ id, category, title, summary, overview, facts = [], notes = [], meta = {}, sortOrder = 0 }) {
  return createEntry({
    id,
    category,
    sortOrder,
    title,
    summary,
    overview,
    facts,
    notes,
    meta
  });
}

function createEntry({
  id,
  category,
  title,
  summary,
  overview,
  facts = [],
  obtain = [],
  crafting = [],
  drops = [],
  pieces = [],
  notes = [],
  tactics = [],
  related = [],
  image = "",
  banner = "",
  sortOrder = 0,
  meta = {}
}) {
  const pt = {
    title,
    subtitle: "",
    summary,
    overview,
    facts,
    obtain,
    crafting,
    drops,
    pieces,
    notes,
    tactics
  };

  const en = {
    title,
    subtitle: "",
    summary,
    overview,
    facts,
    obtain,
    crafting,
    drops,
    pieces,
    notes,
    tactics
  };

  return {
    id,
    category,
    image,
    banner,
    related,
    sortOrder,
    isPublished: true,
    content: {
      _meta: meta,
      "pt-BR": pt,
      en,
      es: en,
      ru: en
    }
  };
}

function mergeContentObjects(baseContent, nextContent) {
  const merged = { ...baseContent };
  for (const [languageCode, content] of Object.entries(nextContent ?? {})) {
    merged[languageCode] = {
      ...(baseContent?.[languageCode] ?? {}),
      ...(content ?? {})
    };
  }
  return merged;
}

function buildAssetMap(root) {
  const map = new Map();
  if (!fs.existsSync(root)) {
    return map;
  }

  walkFiles(root, ".png").forEach((filePath) => {
    const basename = path.basename(filePath).toLowerCase();
    const relative = `./${path.relative(docsRoot, filePath).replaceAll("\\", "/")}`;
    if (!map.has(basename)) {
      map.set(basename, relative);
    }
  });

  return map;
}

function findLocalAsset(fileName) {
  return assetMap.get(String(fileName).toLowerCase()) ?? "";
}

function walkFiles(root, extension) {
  if (!fs.existsSync(root)) {
    return [];
  }

  const results = [];
  const stack = [root];
  while (stack.length > 0) {
    const current = stack.pop();
    for (const entry of fs.readdirSync(current, { withFileTypes: true })) {
      const fullPath = path.join(current, entry.name);
      if (entry.isDirectory()) {
        stack.push(fullPath);
      } else if (!extension || fullPath.endsWith(extension)) {
        results.push(fullPath);
      }
    }
  }
  return results;
}

function splitTopLevel(value, delimiter = ",") {
  const parts = [];
  let current = "";
  let parenDepth = 0;
  let angleDepth = 0;
  let bracketDepth = 0;
  let braceDepth = 0;
  let inString = false;
  let previous = "";

  for (const character of value) {
    if (character === "\"" && previous !== "\\") {
      inString = !inString;
      current += character;
      previous = character;
      continue;
    }

    if (!inString) {
      if (character === "(") parenDepth += 1;
      if (character === ")") parenDepth -= 1;
      if (character === "<") angleDepth += 1;
      if (character === ">") angleDepth -= 1;
      if (character === "[") bracketDepth += 1;
      if (character === "]") bracketDepth -= 1;
      if (character === "{") braceDepth += 1;
      if (character === "}") braceDepth -= 1;

      if (character === delimiter && parenDepth === 0 && angleDepth === 0 && bracketDepth === 0 && braceDepth === 0) {
        parts.push(current.trim());
        current = "";
        previous = character;
        continue;
      }
    }

    current += character;
    previous = character;
  }

  if (current.trim()) {
    parts.push(current.trim());
  }

  return parts;
}

function readQuoted(token) {
  return String(token ?? "").trim().replace(/^"/, "").replace(/"$/, "");
}

function extractNamedString(args, name) {
  const token = args.find((arg) => arg.trim().startsWith(`${name}:`));
  if (!token) {
    return "";
  }
  return readQuoted(token.split(":").slice(1).join(":").trim());
}

function extractLastString(args) {
  const candidate = [...args].reverse().find((arg) => /"[^"]*"/.test(arg));
  return candidate ? readQuoted(candidate.split(":").pop().trim()) : "";
}

function extractNamedNumeric(args, name) {
  const token = args.find((arg) => arg.trim().startsWith(`${name}:`));
  return token ? cleanNumeric(token.split(":").slice(1).join(":")) : "";
}

function cleanNumeric(value) {
  return String(value ?? "").trim().replace(/f$/i, "");
}

function humanizeEnum(value) {
  return humanizeIdentifier(String(value ?? "").replace(/^.*\./, ""));
}

function humanizeIdentifier(value) {
  const overrides = {
    TRex: "T-Rex",
    Wtf: "WTF?",
    BombOmb: "Bomb-omb",
    SoulofLight: "Soul of Light",
    SoulofNight: "Soul of Night",
    ItemID: "Item",
    OreSpawn: "OreSpawn"
  };

  if (overrides[value]) {
    return overrides[value];
  }

  return String(value ?? "")
    .replace(/([A-Z]+)([A-Z][a-z])/g, "$1 $2")
    .replace(/([a-z0-9])([A-Z])/g, "$1 $2")
    .replace(/([A-Za-z])(\d)/g, "$1 $2")
    .replace(/(\d)([A-Za-z])/g, "$1 $2")
    .replace(/\bNpc\b/g, "NPC")
    .replace(/\bId\b/g, "ID")
    .replace(/\s+/g, " ")
    .trim();
}

function slugify(value) {
  return String(value ?? "")
    .normalize("NFKD")
    .replace(/[\u0300-\u036f]/g, "")
    .replace(/&/g, " and ")
    .replace(/[^a-zA-Z0-9]+/g, "-")
    .replace(/^-+|-+$/g, "")
    .replace(/-{2,}/g, "-")
    .toLowerCase();
}

function normalizeKey(value) {
  return slugify(String(value ?? "").replace(/\?+/g, ""));
}

function compactList(items) {
  return items.filter((item) => String(item ?? "").trim().length > 0);
}

function readProjectFile(relativePath) {
  return fs.readFileSync(path.join(projectRoot, relativePath), "utf8");
}

function writeOutput() {
  const sorted = [...generatedEntries].sort((left, right) => {
    const sortDifference = Number(left.sortOrder ?? 0) - Number(right.sortOrder ?? 0);
    if (sortDifference !== 0) {
      return sortDifference;
    }
    return String(left.id).localeCompare(String(right.id));
  });

  const banner = [
    "// Auto-generated by tools/wiki/generate-orespawn-generated-data.mjs",
    "// Do not hand-edit this file; regenerate it from the source catalogs instead.",
    ""
  ].join("\n");

  const body = `export const generatedOreSpawnEntries = ${JSON.stringify(sorted, null, 2)};\n`;
  fs.writeFileSync(outputPath, banner + body, "utf8");
  console.log(`Generated ${sorted.length} OreSpawn wiki entries at ${outputPath}`);
}
