import fs from "node:fs";
import path from "node:path";
import { fileURLToPath, pathToFileURL } from "node:url";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const projectRoot = path.resolve(__dirname, "..", "..");
const docsRoot = path.join(projectRoot, "docs");
const itemsRoot = path.join(projectRoot, "Content", "Items");
const localizationPath = path.join(projectRoot, "Localization", "en-US_Mods.ChaoticDimensions.hjson");
const outputPath = path.join(docsRoot, "generated-code-entries.js");

const dataModule = await import(pathToFileURL(path.join(docsRoot, "data.js")).href);
const minecraftLegacyModule = await import(pathToFileURL(path.join(docsRoot, "generated-minecraft-legacy-data.js")).href);
const referenceModule = await import(pathToFileURL(path.join(docsRoot, "generated-reference-overrides.js")).href);
const overridesModule = await import(pathToFileURL(path.join(docsRoot, "wiki-overrides.js")).href);

const WORKSTATION_MAP = {
  "TileID.LunarCraftingStation": { id: "lunar-crafting-station", label: "Lunar Crafting Station" },
  "TileID.MythrilAnvil": { id: "mythril-anvil", label: "Mythril Anvil" },
  "TileID.OrichalcumAnvil": { id: "mythril-anvil", label: "Mythril Anvil" },
  "TileID.Anvils": { id: "anvil", label: "Anvil" },
  "TileID.Furnaces": { id: "furnace", label: "Furnace" },
  "TileID.Hellforge": { id: "furnace", label: "Hellforge" },
  "TileID.Bottles": { id: "bottles", label: "Bottles" },
  "ModContent.TileType<GodnessAnvilTile>()": { id: "godness-anvil", label: "Godness Anvil" }
};

const HELPER_RECIPES = {
  "CrystalineDevourArmorCommon.AddArmorRecipe": ([tearAmount, lunarBarAmount]) => ({
    ingredients: [
      buildModIngredient("CrystalineTear", tearAmount),
      buildVanillaIngredient("ItemID.LunarBar", lunarBarAmount)
    ],
    stations: [WORKSTATION_MAP["TileID.LunarCraftingStation"]]
  }),
  "RosalitaArmorCommon.AddArmorRecipe": ([rosalitaAmount, hallowedBarAmount, scrapAmount]) => ({
    ingredients: [
      buildModIngredient("RosalitaGem", rosalitaAmount),
      buildVanillaIngredient("ItemID.HallowedBar", hallowedBarAmount),
      buildModIngredient("ShadowScrap", scrapAmount)
    ],
    stations: [WORKSTATION_MAP["TileID.MythrilAnvil"]]
  }),
  "RosalitaRecipeCommon.CreateRosalitaWeaponRecipe": ([rosalitaAmount, scrapAmount, hallowedBarAmount]) => ({
    ingredients: [
      buildModIngredient("RosalitaGem", rosalitaAmount),
      buildModIngredient("ShadowScrap", scrapAmount),
      buildVanillaIngredient("ItemID.HallowedBar", hallowedBarAmount)
    ],
    stations: [WORKSTATION_MAP["TileID.MythrilAnvil"]]
  }),
  "RosalitaRecipeCommon.CreateEclipsedMonthraRecipe": ([rosalitaAmount, monthraAmount, scrapAmount]) => ({
    ingredients: [
      buildModIngredient("RosalitaGem", rosalitaAmount),
      buildModIngredient("MonthraScale", monthraAmount),
      buildModIngredient("ShadowScrap", scrapAmount)
    ],
    stations: [WORKSTATION_MAP["TileID.MythrilAnvil"]]
  }),
  "ShadowArmorCommon.AddArmorRecipe": ([shadowOreAmount, scrapAmount, soulAmount, rosalitaAmount]) => ({
    ingredients: [
      buildModIngredient("ShadowOre", shadowOreAmount),
      buildModIngredient("ShadowScrap", scrapAmount),
      buildModIngredient("SoulOfShadow", soulAmount),
      buildModIngredient("RosalitaGem", rosalitaAmount)
    ],
    stations: [WORKSTATION_MAP["ModContent.TileType<GodnessAnvilTile>()"]]
  }),
  "ShadowAscensionRecipeHelper.CreateShadowRecipe": ([shadowOreAmount, scrapAmount, soulAmount, rosalitaAmount]) => ({
    ingredients: [
      buildModIngredient("ShadowOre", shadowOreAmount),
      buildModIngredient("ShadowScrap", scrapAmount),
      buildModIngredient("SoulOfShadow", soulAmount),
      buildModIngredient("RosalitaGem", rosalitaAmount)
    ],
    stations: [WORKSTATION_MAP["ModContent.TileType<GodnessAnvilTile>()"]]
  })
};

const itemLocalization = readItemLocalization(localizationPath);
const existingEntries = mergeStaticSources(
  dataModule.entries,
  minecraftLegacyModule.generatedMinecraftLegacyEntries,
  referenceModule.generatedReferenceEntries,
  overridesModule.entryOverrides
);
const existingEntryMap = new Map(existingEntries.map((entry) => [entry.id, entry]));

const classInfos = collectItemClasses(itemsRoot);
const classInfoMap = new Map(classInfos.map((info) => [info.name, info]));
const concreteItemClasses = classInfos.filter((info) => !info.abstract && isModItemClass(info, classInfoMap));

const generatedEntries = concreteItemClasses
  .map((info) => buildCodeEntry(info, classInfoMap, existingEntryMap, itemLocalization))
  .sort((left, right) => left.id.localeCompare(right.id));

const codeIdSet = new Set(generatedEntries.map((entry) => entry.id));
const staleEntries = existingEntries
  .filter((entry) => isCodeTrackedCategory(entry.category))
  .filter((entry) => !codeIdSet.has(entry.id))
  .filter((entry) => !isAggregateArmorEntry(entry, codeIdSet))
  .map((entry) => entry.id)
  .sort();

const newEntries = generatedEntries
  .filter((entry) => !existingEntryMap.has(entry.id))
  .map((entry) => entry.id)
  .sort();

const output = `export const generatedCodeEntries = ${JSON.stringify(generatedEntries, null, 2)};\n`;
fs.writeFileSync(outputPath, output, "utf8");

console.log(
  `Generated ${generatedEntries.length} code-backed wiki entries (${newEntries.length} new).`
);
if (newEntries.length > 0) {
  console.log(`New code entries: ${newEntries.join(", ")}`);
}
if (staleEntries.length > 0) {
  console.log(`Entries without matching code classes: ${staleEntries.join(", ")}`);
}

function buildCodeEntry(info, classInfoMap, existingEntryMap, localizationMap) {
  const titleEn = localizationMap.get(info.name)?.DisplayName ?? humanizeIdentifier(info.name);
  const entryId = slugify(titleEn);
  const existingEntry = existingEntryMap.get(entryId);
  const inferredCategory = inferCategory(info, classInfoMap);
  const texturePath = info.texturePath || buildDefaultTexturePath(info);
  const imagePath = guessWikiImage(info);
  const recipes = buildRecipesForClass(info, classInfoMap, localizationMap);
  const titlePt = existingEntry?.content?.["pt-BR"]?.title
    ?? existingEntry?.content?.en?.title
    ?? titleEn;

  const content = {
    _meta: {
      ...(existingEntry?.content?._meta ?? {}),
        codePresent: true,
        codeClassName: info.name,
        codeSourcePath: info.filePath.replaceAll("\\", "/"),
        codeTexturePath: texturePath,
      codeTitles: {
        en: titleEn,
        "pt-BR": titlePt
      },
      codeAliases: uniqueList([info.name, titleEn, titlePt]),
      codeRecipes: recipes,
      codeCategory: inferredCategory
    }
  };

  if (!existingEntry) {
    const crafted = recipes.length > 0;
    content["pt-BR"] = {
      title: titlePt,
      subtitle: buildGeneratedSubtitle(inferredCategory, "pt-BR"),
      summary: crafted
        ? `${titlePt} foi sincronizado automaticamente a partir do codigo do mod e ja usa a receita real do projeto.`
        : `${titlePt} foi sincronizado automaticamente a partir do codigo atual do mod.`,
      overview: crafted
        ? `Esta pagina usa o codigo do Chaotic Dimensions como fonte de verdade para ingredientes, estacoes e quantidade da receita, evitando divergencia entre o jogo e a wiki.`
        : `A entrada foi criada automaticamente para manter a wiki alinhada com o codigo do mod, mesmo antes de receber uma descricao manual mais detalhada.`,
      obtain: crafted
        ? ["Criado pela receita real extraida do codigo do mod."]
        : ["Ainda sem receita publica no codigo atual; a entrada foi mantida para acompanhar o conteudo do mod."],
      crafting: buildCodeCraftingLines(recipes, "pt-BR")
    };
    content.en = {
      title: titleEn,
      subtitle: buildGeneratedSubtitle(inferredCategory, "en"),
      summary: crafted
        ? `${titleEn} was synced directly from the mod code and already uses the live in-game recipe.`
        : `${titleEn} was synced directly from the current mod code.`,
      overview: crafted
        ? `This page uses the Chaotic Dimensions source code as its single source of truth for ingredients, stations and recipe counts.`
        : `This entry was generated automatically so the wiki can stay aligned with the mod even before it receives a hand-written article.`,
      obtain: crafted
        ? ["Crafted using the live recipe extracted from the mod source."]
        : ["No public recipe is defined in the current code; the page is kept so the wiki can track the content."],
      crafting: buildCodeCraftingLines(recipes, "en")
    };
  }

  return {
    id: entryId,
    category: existingEntry?.category ?? inferredCategory,
    image: existingEntry ? undefined : imagePath,
    sortOrder: existingEntry?.sortOrder ?? 0,
    isPublished: existingEntry?.isPublished ?? true,
    content
  };
}

function buildGeneratedSubtitle(category, language) {
  const labels = {
    armor: { "pt-BR": "Armadura sincronizada do codigo", en: "Code-synced armor entry" },
    accessories: { "pt-BR": "Acessorio sincronizado do codigo", en: "Code-synced accessory entry" },
    consumables: { "pt-BR": "Consumivel sincronizado do codigo", en: "Code-synced consumable entry" },
    materials: { "pt-BR": "Material sincronizado do codigo", en: "Code-synced material entry" },
    blocks: { "pt-BR": "Bloco sincronizado do codigo", en: "Code-synced block entry" },
    tools: { "pt-BR": "Ferramenta sincronizada do codigo", en: "Code-synced tool entry" },
    summons: { "pt-BR": "Invocacao sincronizada do codigo", en: "Code-synced summon entry" },
    weapons: { "pt-BR": "Arma sincronizada do codigo", en: "Code-synced weapon entry" }
  };

  return labels[category]?.[language] ?? labels.materials[language];
}

function buildRecipesForClass(info, classInfoMap, localizationMap) {
  const addRecipesBody = resolveAddRecipesBody(info, classInfoMap);
  if (!addRecipesBody) {
    return [];
  }

  return splitStatements(addRecipesBody)
    .filter((statement) => statement.includes("Register()") || Object.keys(HELPER_RECIPES).some((helperName) => statement.includes(helperName)))
    .map((statement) => parseRecipeStatement(statement, localizationMap))
    .filter((recipe) => recipe.ingredients.length > 0 || recipe.stations.length > 0);
}

function resolveAddRecipesBody(info, classInfoMap, seen = new Set()) {
  if (!info || seen.has(info.name)) {
    return "";
  }

  seen.add(info.name);
  if (info.addRecipesBody) {
    return info.addRecipesBody;
  }

  const parentName = stripTypeQualifier(info.baseName);
  if (!parentName || parentName === "ModItem") {
    return "";
  }

  return resolveAddRecipesBody(classInfoMap.get(parentName), classInfoMap, seen);
}

function parseRecipeStatement(statement, localizationMap) {
  const normalized = statement.replace(/\s+/g, " ").trim();
  const recipe = {
    resultAmount: extractResultAmount(normalized),
    ingredients: [],
    stations: []
  };

  Object.entries(HELPER_RECIPES).forEach(([helperName, builder]) => {
    if (!normalized.includes(helperName)) {
      return;
    }

    const args = extractCallArguments(normalized, helperName)
      .slice(1)
      .map((value) => parseInteger(value))
      .filter((value) => value !== null);
    const baseRecipe = builder(args);
    baseRecipe.ingredients.forEach((ingredient) => {
      if (ingredient.amount > 0) {
        recipe.ingredients.push(ingredient);
      }
    });
    baseRecipe.stations.forEach((station) => pushUniqueStation(recipe.stations, station));
  });

  const ingredientRegex = /\.AddIngredient(?:<([^>]+)>)?\((.*?)\)\s*(?=\.|;)/g;
  for (const match of normalized.matchAll(ingredientRegex)) {
    const genericType = match[1]?.trim() ?? "";
    const args = splitArguments(match[2] ?? "");
    if (genericType) {
      recipe.ingredients.push(buildModIngredient(genericType, parseInteger(args[0]) ?? 1, localizationMap));
      continue;
    }

    const expression = args[0] ?? "";
    const amount = parseInteger(args[1]) ?? 1;
    const ingredient = buildIngredientFromExpression(expression, amount, localizationMap);
    if (ingredient) {
      recipe.ingredients.push(ingredient);
    }
  }

  const tileRegex = /\.AddTile\((.*?)\)\s*(?=\.|;)/g;
  for (const match of normalized.matchAll(tileRegex)) {
    const station = resolveStation(match[1] ?? "");
    if (station) {
      pushUniqueStation(recipe.stations, station);
    }
  }

  return {
    resultAmount: recipe.resultAmount,
    ingredients: mergeDuplicateIngredients(recipe.ingredients),
    stations: recipe.stations
  };
}

function mergeDuplicateIngredients(ingredients) {
  const merged = new Map();

  ingredients.forEach((ingredient) => {
    const key = ingredient.entryId
      ? `mod:${ingredient.entryId}`
      : `vanilla:${normalizeLabel(ingredient.label)}`;
    const existing = merged.get(key);
    if (!existing) {
      merged.set(key, { ...ingredient });
      return;
    }

    existing.amount += ingredient.amount;
  });

  return [...merged.values()];
}

function buildCodeCraftingLines(recipes, language) {
  return recipes.map((recipe) => {
    const ingredientText = recipe.ingredients
      .map((ingredient) => `${ingredient.label} x${ingredient.amount}`)
      .join(" + ");
    const stationText = recipe.stations
      .map((station) => station.label)
      .join(language === "pt-BR" ? " ou " : " or ");
    const resultPrefix = recipe.resultAmount > 1
      ? (language === "pt-BR" ? `Cria x${recipe.resultAmount}: ` : `Creates x${recipe.resultAmount}: `)
      : "";
    const joiner = language === "pt-BR" ? " em " : " at ";
    return `${resultPrefix}${ingredientText}${stationText ? `${joiner}${stationText}` : ""}`;
  });
}

function extractResultAmount(statement) {
  const match = statement.match(/CreateRecipe(?:<[^>]+>)?\(([^)]*)\)/);
  if (!match) {
    return 1;
  }

  const amount = parseInteger(match[1]);
  return amount ?? 1;
}

function buildIngredientFromExpression(expression, amount, localizationMap) {
  const clean = String(expression ?? "").trim();
  if (!clean) {
    return null;
  }

  const modTypeMatch = clean.match(/ModContent\.ItemType<([^>]+)>\(\)/);
  if (modTypeMatch) {
    return buildModIngredient(modTypeMatch[1], amount, localizationMap);
  }

  if (clean.startsWith("ItemID.")) {
    return buildVanillaIngredient(clean, amount);
  }

  return buildModIngredient(clean, amount, localizationMap);
}

function buildModIngredient(typeName, amount, localizationMap = itemLocalization) {
  const className = stripTypeQualifier(typeName);
  const title = localizationMap.get(className)?.DisplayName ?? humanizeIdentifier(className);
  return {
    label: title,
    amount: amount ?? 1,
    entryId: slugify(title)
  };
}

function buildVanillaIngredient(expression, amount) {
  return {
    label: buildVanillaLabel(expression),
    amount: amount ?? 1
  };
}

function buildVanillaLabel(expression) {
  const raw = String(expression ?? "").trim().replace(/^ItemID\./, "");
  if (!raw) {
    return "";
  }

  if (/^[A-Z0-9]+$/.test(raw)) {
    return raw;
  }

  const soulMatch = raw.match(/^Soulof(.+)$/);
  if (soulMatch) {
    return `Soul of ${humanizeIdentifier(soulMatch[1])}`;
  }

  return humanizeIdentifier(raw);
}

function resolveStation(expression) {
  const clean = String(expression ?? "").trim();
  return WORKSTATION_MAP[clean] ?? null;
}

function pushUniqueStation(stations, station) {
  if (!station || stations.some((candidate) => candidate.id === station.id)) {
    return;
  }

  stations.push({ ...station });
}

function extractCallArguments(statement, methodName) {
  const index = statement.indexOf(methodName);
  if (index === -1) {
    return [];
  }

  const openIndex = statement.indexOf("(", index + methodName.length);
  if (openIndex === -1) {
    return [];
  }

  let depth = 0;
  let closeIndex = -1;
  for (let cursor = openIndex; cursor < statement.length; cursor += 1) {
    const character = statement[cursor];
    if (character === "(") {
      depth += 1;
    }
    else if (character === ")") {
      depth -= 1;
      if (depth === 0) {
        closeIndex = cursor;
        break;
      }
    }
  }

  if (closeIndex === -1) {
    return [];
  }

  return splitArguments(statement.slice(openIndex + 1, closeIndex));
}

function splitArguments(value) {
  const output = [];
  let current = "";
  let parenDepth = 0;
  let angleDepth = 0;

  for (const character of String(value ?? "")) {
    if (character === "," && parenDepth === 0 && angleDepth === 0) {
      output.push(current.trim());
      current = "";
      continue;
    }

    if (character === "(") {
      parenDepth += 1;
    }
    else if (character === ")") {
      parenDepth = Math.max(0, parenDepth - 1);
    }
    else if (character === "<") {
      angleDepth += 1;
    }
    else if (character === ">") {
      angleDepth = Math.max(0, angleDepth - 1);
    }

    current += character;
  }

  if (current.trim()) {
    output.push(current.trim());
  }

  return output;
}

function parseInteger(value) {
  const match = String(value ?? "").match(/-?\d+/);
  return match ? Number(match[0]) : null;
}

function splitStatements(body) {
  return String(body ?? "")
    .split(";")
    .map((statement) => statement.trim())
    .filter(Boolean)
    .map((statement) => `${statement};`);
}

function inferCategory(info, classInfoMap) {
  const filePath = info.filePath.replaceAll("\\", "/");
  const inheritedChain = getInheritanceChain(info, classInfoMap);
  const body = info.body;

  if (filePath.includes("/Armor/")) {
    return "armor";
  }
  if (filePath.includes("/Summons/")) {
    return "summons";
  }
  if (filePath.includes("/Accessories/")) {
    return "accessories";
  }
  if (filePath.includes("/Consumables/")) {
    return "consumables";
  }
  if (filePath.includes("/Materials/")) {
    return "materials";
  }
  if (filePath.includes("/Weapons/")) {
    if (/Item\.pick\s*=|Item\.axe\s*=|Item\.hammer\s*=/.test(body)) {
      return "tools";
    }
    return "weapons";
  }

  if (inheritedChain.includes("MinecraftLegacyAccessoryBase") || /Item\.accessory\s*=/.test(body)) {
    return "accessories";
  }
  if (inheritedChain.includes("MinecraftLegacyPlaceableItemBase") || /Item\.createTile\s*=/.test(body)) {
    return "blocks";
  }
  if (inheritedChain.includes("MinecraftLegacyMaterialItemBase")) {
    return "materials";
  }
  if (inheritedChain.includes("MinecraftLegacyPickaxeBase") || inheritedChain.includes("MinecraftLegacyAxeBase") || /Item\.(pick|axe|hammer)\s*=/.test(body)) {
    return "tools";
  }
  if (inheritedChain.includes("MinecraftLegacySwordBase") || /DamageClass\.(Melee|Magic|Ranged|Summon)/.test(body)) {
    return "weapons";
  }
  if (/Item\.buffType\s*=/.test(body) || /Item\.healMana|Item\.healLife|ItemID\.Sets\.SortingPriorityBossSpawns/.test(body)) {
    return "consumables";
  }

  return "materials";
}

function getInheritanceChain(info, classInfoMap) {
  const output = [];
  let current = info;
  const seen = new Set();

  while (current && current.baseName && !seen.has(current.name)) {
    seen.add(current.name);
    const parentName = stripTypeQualifier(current.baseName);
    if (!parentName || parentName === "ModItem") {
      break;
    }
    output.push(parentName);
    current = classInfoMap.get(parentName);
  }

  return output;
}

function guessWikiImage(info) {
  const texturePath = info.texturePath
    || buildDefaultTexturePath(info);
  if (!texturePath) {
    return "";
  }

  const relativeTexturePath = texturePath
    .replace(/^ChaoticDimensions\/Content\//, "")
    .replace(/^ChaoticDimensions\//, "")
    .replace(/^Content\//, "");
  const candidate = `./assets/images/content/${relativeTexturePath}.png`;
  const candidateDiskPath = path.join(docsRoot, "assets", "images", "content", `${relativeTexturePath}.png`);
  return fs.existsSync(candidateDiskPath)
    ? candidate.replaceAll("\\", "/")
    : "";
}

function buildDefaultTexturePath(info) {
  const normalizedFilePath = info.filePath.replaceAll("\\", "/");
  const directory = normalizedFilePath.slice(0, normalizedFilePath.lastIndexOf("/"));
  return `ChaoticDimensions/${directory}/${info.name}`;
}

function collectItemClasses(rootPath) {
  const files = walkFiles(rootPath, ".cs");
  const output = [];

  files.forEach((filePath) => {
    const relativePath = path.relative(projectRoot, filePath);
    const source = fs.readFileSync(filePath, "utf8");
    output.push(...parseClassesFromFile(source, relativePath));
  });

  return output;
}

function parseClassesFromFile(source, filePath) {
  const output = [];
  const classRegex = /\b(public|internal)\s+(abstract\s+|sealed\s+)?class\s+([A-Za-z0-9_]+)\s*:\s*([A-Za-z0-9_<>.]+)/g;

  for (const match of source.matchAll(classRegex)) {
    const bodyStart = source.indexOf("{", match.index + match[0].length - 1);
    if (bodyStart === -1) {
      continue;
    }

    const bodyEnd = findMatchingBrace(source, bodyStart);
    if (bodyEnd === -1) {
      continue;
    }

    const body = source.slice(bodyStart + 1, bodyEnd);
    output.push({
      name: match[3],
      baseName: match[4],
      abstract: String(match[2] ?? "").includes("abstract"),
      filePath,
      body,
      texturePath: extractTexturePath(body),
      addRecipesBody: extractMethodBody(body, /public\s+(?:sealed\s+)?override\s+void\s+AddRecipes\s*\(\s*\)/)
    });
  }

  return output;
}

function extractTexturePath(body) {
  const match = body.match(/public\s+override\s+string\s+Texture\s*=>\s*"([^"]+)"/);
  return match?.[1] ?? "";
}

function extractMethodBody(body, signatureRegex) {
  const match = signatureRegex.exec(body);
  if (!match) {
    return "";
  }

  const braceIndex = body.indexOf("{", match.index + match[0].length);
  if (braceIndex === -1) {
    return "";
  }

  const endIndex = findMatchingBrace(body, braceIndex);
  if (endIndex === -1) {
    return "";
  }

  return body.slice(braceIndex + 1, endIndex);
}

function findMatchingBrace(source, openBraceIndex) {
  let depth = 0;
  for (let cursor = openBraceIndex; cursor < source.length; cursor += 1) {
    const character = source[cursor];
    if (character === "{") {
      depth += 1;
    }
    else if (character === "}") {
      depth -= 1;
      if (depth === 0) {
        return cursor;
      }
    }
  }

  return -1;
}

function isModItemClass(info, classInfoMap, seen = new Set()) {
  if (!info || seen.has(info.name)) {
    return false;
  }

  seen.add(info.name);
  const parentName = stripTypeQualifier(info.baseName);
  if (parentName === "ModItem") {
    return true;
  }

  const parent = classInfoMap.get(parentName);
  return Boolean(parent) && isModItemClass(parent, classInfoMap, seen);
}

function readItemLocalization(filePath) {
  const source = fs.readFileSync(filePath, "utf8");
  const output = new Map();
  const stack = [];
  let currentItem = "";

  source.split(/\r?\n/).forEach((line) => {
    const trimmed = line.trim();
    if (!trimmed || trimmed.startsWith("//")) {
      return;
    }

    const openMatch = trimmed.match(/^([A-Za-z0-9_.-]+):\s*\{$/);
    if (openMatch) {
      const key = openMatch[1];
      stack.push(key);
      if (stack.length === 2 && stack[0] === "Items") {
        currentItem = key;
        if (!output.has(currentItem)) {
          output.set(currentItem, {});
        }
      }
      return;
    }

    if (trimmed === "}") {
      const closed = stack.pop();
      if (closed === currentItem) {
        currentItem = "";
      }
      return;
    }

    if (stack.length === 2 && stack[0] === "Items" && currentItem) {
      const propertyMatch = trimmed.match(/^([A-Za-z0-9_.-]+):\s*(.+)$/);
      if (!propertyMatch) {
        return;
      }

      const [, propertyName, rawValue] = propertyMatch;
      output.get(currentItem)[propertyName] = rawValue.trim().replace(/^"|"$/g, "");
    }
  });

  return output;
}

function mergeStaticSources(...sourceLists) {
  const merged = new Map();

  sourceLists.flat().forEach((entry) => {
    if (!entry) {
      return;
    }

    const existing = merged.get(entry.id);
    if (!existing) {
      merged.set(entry.id, cloneEntry(entry));
      return;
    }

    merged.set(entry.id, {
      id: entry.id,
      category: entry.category ?? existing.category,
      image: entry.image ?? existing.image ?? "",
      banner: entry.banner || existing.banner || "",
      related: [...new Set([...(existing.related ?? []), ...(entry.related ?? [])])],
      sortOrder: Number(entry.sortOrder ?? existing.sortOrder ?? 0),
      isPublished: entry.isPublished ?? existing.isPublished ?? true,
      content: mergeContentObjects(existing.content, entry.content ?? {})
    });
  });

  return [...merged.values()];
}

function cloneEntry(entry) {
  return {
    id: entry.id,
    category: entry.category,
    image: entry.image,
    banner: entry.banner ?? "",
    related: [...(entry.related ?? [])],
    sortOrder: Number(entry.sortOrder ?? 0),
    isPublished: entry.isPublished ?? true,
    content: mergeContentObjects({}, entry.content ?? {})
  };
}

function mergeContentObjects(baseContent, nextContent) {
  const merged = { ...baseContent };

  Object.entries(nextContent ?? {}).forEach(([languageCode, content]) => {
    merged[languageCode] = {
      ...(baseContent?.[languageCode] ?? {}),
      ...(content ?? {})
    };
  });

  return merged;
}

function isCodeTrackedCategory(category) {
  return [
    "weapons",
    "armor",
    "accessories",
    "consumables",
    "materials",
    "blocks",
    "tools",
    "ammo",
    "summons"
  ].includes(category);
}

function isAggregateArmorEntry(entry, codeIdSet) {
  return entry.category === "armor"
    && Array.isArray(entry.related)
    && entry.related.some((relatedId) => codeIdSet.has(relatedId));
}

function walkFiles(rootPath, extension) {
  const output = [];

  fs.readdirSync(rootPath, { withFileTypes: true }).forEach((entry) => {
    const fullPath = path.join(rootPath, entry.name);
    if (entry.isDirectory()) {
      output.push(...walkFiles(fullPath, extension));
      return;
    }

    if (entry.isFile() && fullPath.endsWith(extension)) {
      output.push(fullPath);
    }
  });

  return output;
}

function uniqueList(values) {
  const seen = new Set();
  return values
    .map((value) => String(value ?? "").trim())
    .filter(Boolean)
    .filter((value) => {
      const key = normalizeLabel(value);
      if (seen.has(key)) {
        return false;
      }
      seen.add(key);
      return true;
    });
}

function humanizeIdentifier(value) {
  const raw = String(value ?? "").trim();
  if (!raw) {
    return "";
  }

  if (/^[A-Z0-9]+$/.test(raw) && raw.length <= 5) {
    return raw;
  }

  const title = raw
    .replace(/_/g, " ")
    .replace(/([a-z0-9])([A-Z])/g, "$1 $2")
    .replace(/([A-Z]+)([A-Z][a-z])/g, "$1 $2")
    .replace(/\s+/g, " ")
    .trim();

  return title.replace(/\b(Of|And|The|Or|At|In|On|For|To)\b/g, (match) => match.toLowerCase());
}

function slugify(value) {
  return humanizeIdentifier(value)
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/^-+|-+$/g, "");
}

function stripTypeQualifier(value) {
  return String(value ?? "")
    .trim()
    .replace(/^global::/, "")
    .split(".")
    .pop()
    .replace(/<.*$/, "");
}

function normalizeLabel(value) {
  return String(value ?? "").trim().toLowerCase();
}
