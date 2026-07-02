import fs from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

const repoRoot = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..", "..");
const docsRoot = path.join(repoRoot, "docs");
const referencePath = path.join(docsRoot, "ChaoticDimensionsWikiReference.md");
const outputPath = path.join(docsRoot, "generated-reference-overrides.js");
const dataPath = path.join(docsRoot, "data.js");
const generatedPath = path.join(docsRoot, "generated-minecraft-legacy-data.js");
const overridesPath = path.join(docsRoot, "wiki-overrides.js");

const topContainers = new Set([
  "Escopo Desta Referencia",
  "Visao Geral da Progressao",
  "Bosses",
  "Biomas e Geracao de Mundo",
  "Blocos, Tiles e Estacoes",
  "Materiais e Recursos",
  "Materiais core de bosses",
  "Materiais do ShadowBiome",
  "Legacy materials",
  "Armas, Ferramentas e Equipamentos",
  "Linha Monthra",
  "Linha Crystaline",
  "Linha Rosalita",
  "Linha Eclipsed Monthra",
  "Shadow Ascension",
  "Armaduras",
  "Crystaline Devour Armor",
  "Rosalita Armor",
  "Shadow Armor",
  "Legacy Arsenal",
  "Mobs e NPCs",
  "Critters e fauna passiva",
  "Hostis legacy",
  "Hostis do ShadowBiome",
  "Summons, Minions e Armas especiais",
  "ShadowBiome blocks",
  "Legacy blocks",
  "Estado Atual de Conteudo Bloqueado ou Reservado",
  "Recomendacoes para Uso na Wiki"
]);

const containersWithLevel3Entries = new Set([
  "Materiais core de bosses",
  "Materiais do ShadowBiome",
  "Legacy materials",
  "Linha Monthra",
  "Linha Rosalita",
  "Linha Eclipsed Monthra",
  "Shadow Ascension",
  "Critters e fauna passiva",
  "Hostis legacy",
  "Hostis do ShadowBiome",
  "ShadowBiome blocks",
  "Legacy blocks"
]);

const containersThatAlsoProducePages = new Set([
  "Shadow Ascension",
  "Crystaline Devour Armor",
  "Rosalita Armor",
  "Shadow Armor"
]);

const narrativeSubheadings = new Set([
  "1. Pre-Hardmode inicial",
  "2. Hardmode intermediario",
  "3. ShadowBiome",
  "4. Pos-Moon Lord",
  "5. Shadow Ascension",
  "Funcao na progressao",
  "Como invocar",
  "Stats",
  "Scaling",
  "Padrao de ataques",
  "Fase 2",
  "Projeteis",
  "Drops",
  "Drop",
  "Tatica recomendada",
  "Tatica",
  "Intro",
  "Estrutura do boss",
  "Arena",
  "Stats da cabeca",
  "Stats dos segmentos",
  "Fases por vida combinada",
  "Escalada ofensiva por fase",
  "Estados de combate",
  "Conceito",
  "Geracao",
  "O que ele converte",
  "Visual do bioma",
  "Ativacao do biome para efeitos",
  "Penalidade sem protecao",
  "Mobs do bioma",
  "ShadowDirtBlock / ShadowDirtTile",
  "ShadowGrassTile",
  "ShadowStoneBlock / ShadowStoneTile",
  "ShadowWood / ShadowWoodTile",
  "RawAlexandriteBlock",
  "GreystedWood",
  "ShadowBlock",
  "BlueBerryPlant",
  "Pecas comuns",
  "Recipes",
  "Todos os sets",
  "Durante a aegis",
  "Bonus por peca",
  "Set bonus",
  "Observacao importante de estado"
]);

const childEntryHints = [
  /helm$/i,
  /helmet$/i,
  /breastplate$/i,
  /greaves$/i,
  /pickaxe$/i,
  /axe$/i,
  /hammer$/i,
  /blade$/i,
  /bow$/i,
  /wand$/i,
  /whip$/i,
  /staff$/i,
  /potion$/i,
  /arrow$/i,
  /bullet$/i,
  /boots$/i,
  /totem$/i,
  /shield$/i,
  /anvil$/i,
  /ore$/i,
  /gem$/i,
  /bar$/i,
  /stick$/i,
  /scale$/i,
  /tear$/i,
  /cow$/i,
  /pig$/i,
  /butterfly$/i,
  /critter$/i,
  /creeper$/i,
  /blaze$/i,
  /endernmon$/i,
  /kraken$/i,
  /phantasm$/i,
  /slime$/i,
  /worm$/i,
  /eye$/i,
  /npc$/i
];

const aliasMap = new Map([
  ["crystaline devourer", "crystaline-devourer"],
  ["crystaline devour armor", "crystaline-devour-armor"],
  ["shadowbiome", "shadow-biome"],
  ["godnessanvil", "godness-anvil"],
  ["monthrabutterfly", "monthra-butterfly"],
  ["monthrabutterflystaff", "monthra-butterfly-staff"],
  ["crystalinesigil", "crystaline-sigil"],
  ["chaoscrystalpickaxe", "chaos-crystal-pickaxe"],
  ["heartofthegod", "heart-of-the-god"],
  ["heartofshadows", "heart-of-shadows"],
  ["rosalitagem", "rosalita-gem"],
  ["rosalitaore", "rosalita-ore"],
  ["shadowore", "shadow-ore"],
  ["shadowscrap", "shadow-scrap"],
  ["soulofshadow", "soul-of-shadow"],
  ["monthrascale", "monthra-scale"],
  ["crystalinetear", "crystaline-tear"],
  ["krakentear", "kraken-tear"],
  ["frozenflame", "frozen-flame"],
  ["shadowcreeperhead", "shadow-creeper-head"],
  ["endernmonscale", "endernmon-scale"],
  ["glasstick", "glass-stick"],
  ["glassstick", "glass-stick"],
  ["shadowgem", "shadow-gem"],
  ["shadownugget", "shadow-nugget"],
  ["vortexgem", "vortex-gem"],
  ["bedrockstick", "bedrock-stick"],
  ["ironstick", "iron-stick"],
  ["ratrixstick", "ratrix-stick"],
  ["shadowbar", "shadow-bar"],
  ["rosalitashield", "rosalita-shield"],
  ["happycreeper", "happy-creeper"],
  ["whitecreeper", "white-creeper"],
  ["crystalcreeper", "crystal-creeper"],
  ["shadowcreeper", "shadow-creeper"],
  ["snowblaze", "snow-blaze"],
  ["squidkraken", "squid-kraken"],
  ["krakensquid", "kraken-squid"],
  ["shadoweye", "shadow-eye"],
  ["shadowslime", "shadow-slime"],
  ["shadowworm", "shadow-worm"],
  ["happycleeperstaff", "happy-creeper-staff"],
  ["happycreeperstaff", "happy-creeper-staff"],
  ["squidkrakenstaff", "squid-kraken-staff"],
  ["krakenblade", "kraken-blade"],
  ["bluebutterfly", "blue-butterfly"],
  ["greenbutterfly", "green-butterfly"],
  ["redbutterfly", "red-butterfly"],
  ["yellowbutterfly", "yellow-butterfly"],
  ["fireflycritter", "firefly-critter"],
  ["bigbutterfly", "big-butterfly"],
  ["applecow", "apple-cow"],
  ["goldenapplecow", "golden-apple-cow"],
  ["crystalapplecow", "crystal-apple-cow"],
  ["crystalgoldenapplecow", "crystal-golden-apple-cow"],
  ["dimensionpig", "dimension-pig"],
  ["alessandranpc", "alessandra-npc"],
  ["shadowsummonstaff", "shadow-summon-staff"],
  ["shadowwhip", "shadow-whip"],
  ["shadowbow", "shadow-bow"],
  ["shadowarrow", "shadow-arrow"],
  ["shadowbullet", "shadow-bullet"],
  ["shadowstaff", "shadow-staff"],
  ["shadowzenith", "shadow-zenith"],
  ["shadowmanapotion", "shadow-mana-potion"],
  ["shadowmeleepotion", "shadow-melee-potion"],
  ["gloryboots", "glory-boots"],
  ["rawalexandriteblock", "raw-alexandrite-block"],
  ["greystedwood", "greysted-wood"],
  ["shadowblock", "shadow-block"],
  ["blueberryplant", "blue-berry-plant"],
  ["magic helm", "crystaline-devour-magic-helm"],
  ["magichelm", "crystaline-devour-magic-helm"],
  ["melee helm", "crystaline-devour-melee-helm"],
  ["meleehelm", "crystaline-devour-melee-helm"],
  ["ranged helm", "crystaline-devour-ranged-helm"],
  ["rangedhelm", "crystaline-devour-ranged-helm"],
  ["summoner helm", "crystaline-devour-summoner-helm"],
  ["summonerhelm", "crystaline-devour-summoner-helm"]
]);

const categoryByContainer = {
  "Bosses": "bosses",
  "Biomas e Geracao de Mundo": "biomes",
  "Blocos, Tiles e Estacoes": "blocks",
  "ShadowBiome blocks": "blocks",
  "Legacy blocks": "blocks",
  "Materiais e Recursos": "materials",
  "Materiais core de bosses": "materials",
  "Materiais do ShadowBiome": "materials",
  "Legacy materials": "materials",
  "Armas, Ferramentas e Equipamentos": "weapons",
  "Linha Monthra": "weapons",
  "Linha Crystaline": "weapons",
  "Linha Rosalita": "weapons",
  "Linha Eclipsed Monthra": "weapons",
  "Shadow Ascension": "systems",
  "Armaduras": "armor",
  "Crystaline Devour Armor": "armor",
  "Rosalita Armor": "armor",
  "Shadow Armor": "armor",
  "Legacy Arsenal": "weapons",
  "Mobs e NPCs": "mobs",
  "Critters e fauna passiva": "mobs",
  "Hostis legacy": "mobs",
  "Hostis do ShadowBiome": "mobs",
  "Summons, Minions e Armas especiais": "weapons"
};

const summaryTemplates = {
  bosses: (title) => `${title} e um encontro importante do build atual, com progressao, ataques, drops e taticas documentados na referencia oficial do mod.`,
  minibosses: (title) => `${title} e um encontro especial documentado na referencia atual, com stats, comportamento e recompensas claros para a wiki.`,
  mobs: (title) => `${title} ja existe em codigo e tem comportamento, stats, spawn e drops descritos na referencia oficial do mod.`,
  materials: (title) => `${title} e um material documentado na referencia atual, com papel claro em progressao, recipes ou drops.`,
  weapons: (title) => `${title} e um equipamento ja descrito na referencia oficial, com stats, uso e recipe ou drop relevantes para a wiki.`,
  armor: (title) => `${title} faz parte da camada de defesa atual do mod, com bonus, recipes e papel de progressao descritos na referencia.`,
  accessories: (title) => `${title} e um acessorio documentado na referencia atual, com efeitos, condicoes de uso e papel claro dentro do mod.`,
  consumables: (title) => `${title} e um consumivel documentado na referencia atual, com limites, efeitos e contexto de progressao importantes.`,
  tools: (title) => `${title} e uma ferramenta do build atual, com stats, utilidade e recipe documentados na referencia oficial.`,
  ammo: (title) => `${title} e uma municao descrita na referencia atual, com recipe, efeito de projeteis e papel de combate registrados.`,
  blocks: (title) => `${title} faz parte da estrutura de blocos, tiles ou estacoes ja descritas na referencia atual do mod.`,
  biomes: (title) => `${title} e uma pagina de sistema/bioma baseada na referencia oficial, cobrindo geracao, efeitos e conteudo conectado.`,
  systems: (title) => `${title} resume uma camada de sistema ou progressao que ja esta descrita em detalhe na referencia do mod.`,
  npcs: (title) => `${title} ja existe no build atual e sua funcao, stats e papel no mundo estao descritos na referencia oficial.`
};

function toAscii(value) {
  return String(value ?? "")
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "")
    .replace(/[“”]/g, "\"")
    .replace(/[‘’]/g, "'")
    .replace(/[–—]/g, "-")
    .replace(/…/g, "...")
    .replace(/\u00A0/g, " ");
}

function escapeTemplateString(value) {
  return value
    .replace(/\\/g, "\\\\")
    .replace(/"/g, "\\\"")
    .replace(/`/g, "\\`")
    .replace(/\$\{/g, "\\${");
}

function normalize(value) {
  return toAscii(value).toLowerCase().replace(/[^a-z0-9]+/g, " ").trim();
}

function slugify(value) {
  return toAscii(value)
    .replace(/([a-z0-9])([A-Z])/g, "$1 $2")
    .replace(/([A-Z]+)([A-Z][a-z])/g, "$1 $2")
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/^-+|-+$/g, "");
}

async function importTextModule(filePath) {
  const source = fs.readFileSync(filePath, "utf8");
  const encoded = Buffer.from(source, "utf8").toString("base64");
  return import(`data:text/javascript;base64,${encoded}`);
}

function mergeKnownEntries(...sourceLists) {
  const merged = new Map();
  sourceLists.flat().forEach((entry) => {
    if (!entry || !entry.id) {
      return;
    }
    const existing = merged.get(entry.id) ?? {
      id: entry.id,
      category: entry.category ?? "",
      titles: new Set()
    };
    existing.category = entry.category ?? existing.category;

    Object.values(entry.content ?? {}).forEach((content) => {
      if (content?.title) {
        existing.titles.add(toAscii(content.title));
      }
    });
    existing.titles.add(toAscii(entry.id));
    existing.titles.add(toAscii(entry.id.replace(/-/g, " ")));
    merged.set(entry.id, existing);
  });
  return [...merged.values()];
}

function buildKnownEntryLookup(knownEntries) {
  const lookup = new Map();
  knownEntries.forEach((entry) => {
    const register = (label) => {
      const key = normalize(label);
      if (!key) {
        return;
      }
      if (!lookup.has(key)) {
        lookup.set(key, entry);
      }
    };
    register(entry.id);
    register(entry.id.replace(/-/g, " "));
    [...entry.titles].forEach(register);
  });
  return lookup;
}

function createEntryRecord(id, title, container, parentId = "") {
  return {
    id,
    title,
    container,
    parentId,
    bodyLines: [],
    sections: new Map(),
    children: new Set()
  };
}

function ensureRecord(records, id, title, container, parentId = "") {
  if (!records.has(id)) {
    records.set(id, createEntryRecord(id, title, container, parentId));
  }
  const record = records.get(id);
  if (title && !record.title) {
    record.title = title;
  }
  if (container && !record.container) {
    record.container = container;
  }
  if (parentId && !record.parentId) {
    record.parentId = parentId;
  }
  return record;
}

function resolveEntryId(title, knownLookup) {
  const normalized = normalize(title);
  const direct = aliasMap.get(normalized.replace(/\s+/g, "")) ?? aliasMap.get(normalized);
  if (direct) {
    return direct;
  }

  const exact = knownLookup.get(normalized);
  if (exact) {
    return exact.id;
  }

  const slug = slugify(title);
  const aliased = aliasMap.get(slug.replace(/-/g, "")) ?? aliasMap.get(slug.replace(/-/g, " "));
  if (aliased) {
    return aliased;
  }

  const fromSlugWords = knownLookup.get(normalize(slug.replace(/-/g, " ")));
  if (fromSlugWords) {
    return fromSlugWords.id;
  }

  return slug;
}

function shouldOpenChildEntry(heading, currentContainer, knownLookup) {
  const normalizedHeading = toAscii(heading).trim();
  if (!normalizedHeading) {
    return false;
  }
  if (narrativeSubheadings.has(normalizedHeading)) {
    return false;
  }
  if (knownLookup.has(normalize(normalizedHeading))) {
    return true;
  }
  return childEntryHints.some((pattern) => pattern.test(normalizedHeading));
}

function appendLine(target, sectionName, line) {
  if (!target) {
    return;
  }
  if (sectionName) {
    if (!target.sections.has(sectionName)) {
      target.sections.set(sectionName, []);
    }
    target.sections.get(sectionName).push(line);
    return;
  }
  target.bodyLines.push(line);
}

function parseReference(referenceSource, knownLookup) {
  const records = new Map();
  const lines = referenceSource.split(/\r?\n/);

  let currentContainer = "";
  let currentPrimary = null;
  let currentChild = null;
  let currentSection = "";

  const openPrimary = (heading) => {
    const id = resolveEntryId(heading, knownLookup);
    currentPrimary = ensureRecord(records, id, heading, currentContainer);
    currentChild = null;
    currentSection = "";
  };

  const openChild = (heading) => {
    const id = resolveEntryId(heading, knownLookup);
    currentChild = ensureRecord(records, id, heading, currentContainer, currentPrimary?.id ?? "");
    if (currentPrimary) {
      currentPrimary.children.add(id);
    }
    currentSection = "";
  };

  lines.forEach((rawLine) => {
    const line = rawLine.replace(/\t/g, "  ");
    const trimmed = toAscii(line).trim();

    if (trimmed.startsWith("## ")) {
      const heading = trimmed.slice(3).trim();
      currentChild = null;
      currentSection = "";

      if (topContainers.has(heading)) {
        currentContainer = heading;
        currentPrimary = containersThatAlsoProducePages.has(heading) ? null : null;
        if (containersThatAlsoProducePages.has(heading)) {
          openPrimary(heading);
        }
        return;
      }

      currentContainer = heading;
      openPrimary(heading);
      return;
    }

    if (trimmed.startsWith("### ")) {
      const heading = trimmed.slice(4).trim();

      if (currentChild) {
        currentChild = null;
      }

      if (currentPrimary) {
        if (containersWithLevel3Entries.has(currentContainer) && currentPrimary.title !== currentContainer) {
          openPrimary(heading);
          return;
        }

        if (shouldOpenChildEntry(heading, currentContainer, knownLookup)) {
          openChild(heading);
        } else {
          currentSection = heading;
        }
        return;
      }

      if (containersWithLevel3Entries.has(currentContainer)) {
        openPrimary(heading);
      }
      return;
    }

    appendLine(currentChild ?? currentPrimary, currentSection, line);
  });

  return [...records.values()];
}

function splitPseudoSections(lines) {
  const blocks = [];
  let current = { name: "__body", lines: [] };

  const flush = () => {
    if (current.lines.some((line) => String(line ?? "").trim())) {
      blocks.push({ name: current.name, lines: current.lines.slice() });
    }
  };

  lines.forEach((line) => {
    const trimmed = toAscii(line).trim();
    if (!trimmed) {
      current.lines.push("");
      return;
    }

    if (!trimmed.startsWith("-") && !trimmed.startsWith("|") && trimmed.endsWith(":") && trimmed.length <= 54) {
      flush();
      current = {
        name: trimmed.slice(0, -1).trim(),
        lines: []
      };
      return;
    }

    current.lines.push(line);
  });

  flush();
  return blocks;
}

function parseContentLines(lines) {
  const paragraphs = [];
  const items = [];
  let paragraphBuffer = [];

  const flushParagraph = () => {
    const text = paragraphBuffer.join(" ").trim();
    if (text) {
      paragraphs.push(cleanInline(text));
    }
    paragraphBuffer = [];
  };

  lines.forEach((line) => {
    const trimmed = toAscii(line).trim();
    if (!trimmed) {
      flushParagraph();
      return;
    }

    if (trimmed.startsWith("|")) {
      flushParagraph();
      if (/^\|\s*-/.test(trimmed)) {
        return;
      }
      const cells = trimmed
        .split("|")
        .map((cell) => cleanInline(cell))
        .filter(Boolean);
      if (cells.length >= 2) {
        items.push(`${cells[0]}: ${cells[1]}`);
      }
      return;
    }

    if (trimmed.startsWith("- ")) {
      flushParagraph();
      items.push(cleanInline(trimmed.slice(2)));
      return;
    }

    paragraphBuffer.push(trimmed);
  });

  flushParagraph();
  return { paragraphs, items };
}

function cleanInline(value) {
  return toAscii(value)
    .replace(/^#+\s*/g, "")
    .replace(/`/g, "")
    .replace(/\*\*/g, "")
    .replace(/\*/g, "")
    .replace(/\s+/g, " ")
    .trim();
}

function uniqueLines(lines) {
  const output = [];
  const seen = new Set();
  lines
    .map((item) => cleanInline(item))
    .filter(Boolean)
    .forEach((item) => {
      const key = normalize(item);
      if (seen.has(key)) {
        return;
      }
      seen.add(key);
      output.push(item);
    });
  return output;
}

function categorizeRecord(record, knownLookup) {
  const known = knownLookup.get(normalize(record.title)) ?? knownLookup.get(normalize(record.id.replace(/-/g, " ")));
  if (record.container === "Critters e fauna passiva" && !/npc$/i.test(record.title)) {
    return "mobs";
  }
  if (known?.category) {
    return known.category;
  }

  const body = cleanInline(record.bodyLines.join(" "));
  const title = toAscii(record.title);
  const slug = record.id;

  if (/biome/i.test(title) || slug === "shadow-biome") {
    return "biomes";
  }
  if (/npc$/i.test(title)) {
    return "npcs";
  }
  if (/anvil|block|tile|wood|plant/i.test(title)) {
    return "blocks";
  }
  if (/potion/i.test(title) || /consumivel/i.test(body)) {
    return "consumables";
  }
  if (/arrow|bullet/i.test(title)) {
    return "ammo";
  }
  if (/pickaxe|axe|hammer/i.test(title)) {
    return "tools";
  }
  if (/armor|breastplate|greaves|helm|helmet/i.test(title)) {
    return "armor";
  }
  if (/totem|shield|boots|eye|heart/i.test(title)) {
    return /consumivel/i.test(body) ? "consumables" : "accessories";
  }
  if (/staff/i.test(title) && /summon|minion|invoca/i.test(body)) {
    return "summons";
  }
  if (/bow|blade|sword|gun|wand|whip|staff|zenith/i.test(title)) {
    return "weapons";
  }
  if (/scale|tear|flame|scrap|soul|alexandrite|stick|crystal|gem|nugget|bar|ore/i.test(title)) {
    return "materials";
  }
  if (/butterfly|cow|pig|critter|creeper|blaze|endernmon|kraken|phantasm|slime|worm|eye/i.test(title)) {
    return "mobs";
  }

  return categoryByContainer[record.container] ?? "materials";
}

function humanizeTitle(value) {
  return toAscii(value)
    .replace(/([a-z0-9])([A-Z])/g, "$1 $2")
    .replace(/([A-Z]+)([A-Z][a-z])/g, "$1 $2")
    .replace(/\s+/g, " ")
    .trim();
}

function getDisplayTitle(record, knownLookup) {
  const known = knownLookup.get(normalize(record.title)) ?? knownLookup.get(normalize(record.id.replace(/-/g, " ")));
  if (known?.titles?.size) {
    const titles = [...known.titles];
    const preferred = titles.find((title) => {
      const clean = title.trim();
      return clean && clean !== record.id && clean !== record.id.replace(/-/g, " ");
    });
    if (preferred) {
      return /\s/.test(preferred) ? preferred : humanizeTitle(preferred);
    }
  }
  return humanizeTitle(record.title);
}

function blockTarget(name, record) {
  const key = normalize(name);
  const entryTitle = toAscii(record.title);

  if (key === "__body") {
    return "body";
  }
  if (key.includes("tatica")) {
    return "tactics";
  }
  if (key === "como invocar" || key === "como obter" || key === "geracao" || key === "ativacao do biome para efeitos") {
    return "obtain";
  }
  if (key.includes("recipe") || key === "recipes" || key === "crafting" || key === "recipe base") {
    return "crafting";
  }
  if (key === "drops" || key === "drop") {
    return "drops";
  }
  if (key === "pecas comuns" || key === "pecas" || key === "bonus por peca" || key === "set bonus" || /helm|helmet/i.test(name)) {
    return /armor/i.test(entryTitle) || /helm|helmet/i.test(name) ? "pieces" : "facts";
  }
  if (key === "stats" || key === "stats da cabeca" || key === "stats dos segmentos" || key === "scaling" || key === "fase 2" || key === "fases por vida combinada" || key === "projeteis" || key === "ataques" || key === "efeito" || key === "efeitos" || key === "projetil" || key === "minion" || key === "limite" || key === "todos os sets" || key === "durante a aegis") {
    return "facts";
  }
  if (key === "funcao na progressao" || key === "conceito" || key === "intro" || key === "estrutura do boss" || key === "arena" || key === "escalada ofensiva por fase" || key === "estados de combate" || key === "visual do bioma" || key === "penalidade sem protecao" || key === "o que ele converte" || key === "observacao importante de estado" || key === "comportamento" || key === "evento especial" || key === "mecanica" || key === "mobs do bioma") {
    return "notes";
  }
  return "notes";
}

function summarizeText(paragraphs, items, category, title) {
  const firstParagraph = paragraphs[0];
  if (firstParagraph) {
    const sentence = firstParagraph.split(/(?<=[.!?])\s+/)[0]?.trim() ?? firstParagraph;
    if (sentence.length >= 36) {
      return sentence;
    }
  }

  const firstItem = items[0];
  if (firstItem) {
    return `${title} - ${firstItem}.`;
  }

  return (summaryTemplates[category] ?? summaryTemplates.materials)(title);
}

function buildOverview(summary, paragraphs, notes) {
  const parts = [];
  if (summary) {
    parts.push(summary);
  }
  paragraphs.forEach((paragraph) => {
    if (!parts.includes(paragraph) && !paragraph.startsWith(summary)) {
      parts.push(paragraph);
    }
  });
  notes.slice(0, 3).forEach((line) => {
    if (line.length > 42) {
      parts.push(line);
    }
  });

  return uniqueLines(parts).slice(0, 4).join(" ");
}

function deriveRelatedIds(record, entryMap, knownLookup, contentBuckets) {
  const obtain = Array.isArray(contentBuckets.obtain) ? contentBuckets.obtain : [];
  const crafting = Array.isArray(contentBuckets.crafting) ? contentBuckets.crafting : [];
  const drops = Array.isArray(contentBuckets.drops) ? contentBuckets.drops : [];
  const notes = Array.isArray(contentBuckets.notes) ? contentBuckets.notes : [];
  const pieces = Array.isArray(contentBuckets.pieces) ? contentBuckets.pieces : [];
  const text = [
    ...record.bodyLines,
    ...Array.from(record.sections.values()).flat(),
    ...obtain,
    ...crafting,
    ...drops,
    ...notes,
    ...pieces
  ].join(" ");

  const related = new Set(record.parentId ? [record.parentId] : []);
  record.children.forEach((childId) => related.add(childId));

  entryMap.forEach((candidate) => {
    if (candidate.id === record.id) {
      return;
    }
    const labels = [candidate.title, candidate.id.replace(/-/g, " ")].filter(Boolean);
    if (labels.some((label) => normalize(text).includes(normalize(label)))) {
      related.add(candidate.id);
    }
  });

  const currentKnown = knownLookup.get(normalize(record.id.replace(/-/g, " ")));
  if (currentKnown?.titles) {
    currentKnown.titles.forEach((label) => {
      const matched = knownLookup.get(normalize(label));
      if (matched && matched.id !== record.id) {
        related.add(matched.id);
      }
    });
  }

  return [...related].filter((id) => id && id !== record.id).slice(0, 14);
}

function buildEntryOverride(record, entryMap, knownLookup) {
  const category = categorizeRecord(record, knownLookup);
  const displayTitle = getDisplayTitle(record, knownLookup);
  const bucketMap = {
    body: [],
    facts: [],
    obtain: [],
    crafting: [],
    drops: [],
    notes: [],
    tactics: [],
    pieces: [],
    overviewParagraphs: []
  };

  const sectionSources = [];
  if (record.bodyLines.length > 0) {
    sectionSources.push({ name: "__body", lines: record.bodyLines });
  }
  record.sections.forEach((lines, name) => {
    sectionSources.push({ name, lines });
  });

  sectionSources.forEach(({ name, lines }) => {
    splitPseudoSections(lines).forEach((block) => {
      const effectiveName = name === "__body" && block.name !== "__body" ? block.name : name === "__body" ? "__body" : `${name}::${block.name}`;
      const target = blockTarget(block.name === "__body" ? name : block.name, record);
      const { paragraphs, items } = parseContentLines(block.lines);

      if (target === "body") {
        bucketMap.overviewParagraphs.push(...paragraphs);
        bucketMap.facts.push(...items);
        return;
      }

      if (target === "notes") {
        bucketMap.overviewParagraphs.push(...paragraphs);
        bucketMap.notes.push(...items.length > 0 ? items.map((item) => `${block.name !== "__body" ? `${block.name}: ` : ""}${item}`) : paragraphs);
        return;
      }

      if (target === "pieces") {
        const pieceLines = items.length > 0
          ? items.map((item) => `${block.name}: ${item}`)
          : paragraphs.map((paragraph) => `${block.name}: ${paragraph}`);
        bucketMap.pieces.push(...pieceLines);
        return;
      }

      if (target === "facts") {
        bucketMap.facts.push(...(items.length > 0 ? items.map((item) => `${block.name !== "__body" ? `${block.name}: ` : ""}${item}`) : paragraphs));
        return;
      }

      bucketMap[target].push(...(items.length > 0 ? items : paragraphs));
    });
  });

  if (record.parentId && !bucketMap.notes.some((line) => normalize(line).includes(normalize(record.parentId.replace(/-/g, " "))))) {
    const parentRecord = entryMap.get(record.parentId);
    if (parentRecord?.title) {
      bucketMap.notes.unshift(`Relacionado a: ${parentRecord.title}`);
    }
  }

  const summary = summarizeText(bucketMap.overviewParagraphs, bucketMap.facts, category, displayTitle);
  const overview = buildOverview(summary, bucketMap.overviewParagraphs, bucketMap.notes);

  const content = {
      "pt-BR": {
        title: displayTitle,
      summary,
      overview,
      facts: uniqueLines(bucketMap.facts).slice(0, 18),
      obtain: uniqueLines(bucketMap.obtain).slice(0, 12),
      crafting: uniqueLines(bucketMap.crafting).slice(0, 12),
      drops: uniqueLines(bucketMap.drops).slice(0, 12),
      pieces: uniqueLines(bucketMap.pieces).slice(0, 16),
      notes: uniqueLines(bucketMap.notes).slice(0, 16),
      tactics: uniqueLines(bucketMap.tactics).slice(0, 10)
    }
  };

  Object.keys(content["pt-BR"]).forEach((key) => {
    const value = content["pt-BR"][key];
    if (Array.isArray(value) && value.length === 0) {
      delete content["pt-BR"][key];
    }
    if (typeof value === "string" && !value.trim()) {
      delete content["pt-BR"][key];
    }
  });

  return {
    id: record.id,
    category,
    related: deriveRelatedIds(record, entryMap, knownLookup, content["pt-BR"]),
    content
  };
}

function formatEntry(entry, indent = "  ") {
  const lines = [];
  lines.push(`${indent}{`);
  lines.push(`${indent}  id: "${escapeTemplateString(entry.id)}",`);
  lines.push(`${indent}  category: "${escapeTemplateString(entry.category)}",`);
  if (entry.related?.length) {
    lines.push(`${indent}  related: [${entry.related.map((item) => `"${escapeTemplateString(item)}"`).join(", ")}],`);
  }
  lines.push(`${indent}  content: {`);
  Object.entries(entry.content).forEach(([language, content]) => {
    lines.push(`${indent}    "${language}": {`);
    Object.entries(content).forEach(([key, value]) => {
      if (Array.isArray(value)) {
        lines.push(`${indent}      ${key}: [`);
        value.forEach((item) => {
          lines.push(`${indent}        "${escapeTemplateString(item)}",`);
        });
        lines.push(`${indent}      ],`);
      } else {
        lines.push(`${indent}      ${key}: "${escapeTemplateString(value)}",`);
      }
    });
    lines.push(`${indent}    }`);
  });
  lines.push(`${indent}  }`);
  lines.push(`${indent}}`);
  return lines.join("\n");
}

const [{ entries }, { generatedMinecraftLegacyEntries }, { entryOverrides }] = await Promise.all([
  importTextModule(dataPath),
  importTextModule(generatedPath),
  importTextModule(overridesPath)
]);

const knownEntries = mergeKnownEntries(entries, generatedMinecraftLegacyEntries, entryOverrides);
const knownLookup = buildKnownEntryLookup(knownEntries);
const referenceSource = fs.readFileSync(referencePath, "utf8");
const parsedRecords = parseReference(referenceSource, knownLookup);
const parsedRecordMap = new Map(parsedRecords.map((record) => [record.id, record]));
const generatedEntries = parsedRecords
  .map((record) => buildEntryOverride(record, parsedRecordMap, knownLookup))
  .filter((entry) => entry?.content?.["pt-BR"]?.summary)
  .sort((left, right) => left.id.localeCompare(right.id));

const output = `export const generatedReferenceEntries = [\n${generatedEntries.map((entry) => formatEntry(entry)).join(",\n")}\n];\n`;

fs.writeFileSync(outputPath, output, "utf8");
console.log(`Generated ${generatedEntries.length} reference overrides at ${path.relative(repoRoot, outputPath)}.`);
