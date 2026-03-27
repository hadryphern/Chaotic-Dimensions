import fs from "fs";
import path from "path";
import { fileURLToPath } from "url";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const repoRoot = path.resolve(__dirname, "..", "..");
const docsRoot = path.join(repoRoot, "docs");
const legacyAssetRoot = path.join(docsRoot, "assets", "images", "orespawn-original");
const themeAssetRoot = path.join(docsRoot, "assets", "images", "orespawn-legacy-theme");
const outputFile = path.join(docsRoot, "generated-orespawn-legacy.js");

const sourceRoot = process.argv[2];

if (!sourceRoot) {
  console.error("Usage: node tools/wiki/import-orespawn-legacy-site.mjs <orespawn-site-root>");
  process.exit(1);
}

const siteRoot = path.resolve(sourceRoot);
const pageLimit = 3;
const themeFiles = [
  "clouds.jpg",
  "nav-top-logo.png",
  "main-bg.png",
  "top-bar-bg.jpg"
];

ensureDirectory(legacyAssetRoot);
ensureDirectory(themeAssetRoot);

for (const fileName of themeFiles) {
  const source = path.join(siteRoot, "files", "theme", fileName);
  if (fs.existsSync(source)) {
    fs.copyFileSync(source, path.join(themeAssetRoot, fileName));
  }
}

const pageFiles = fs.readdirSync(siteRoot)
  .filter((name) => /\.html?$/i.test(name))
  .sort((left, right) => left.localeCompare(right));

const manifest = [];

for (const fileName of pageFiles) {
  const filePath = path.join(siteRoot, fileName);
  const html = fs.readFileSync(filePath, "utf8");
  const slug = fileName.replace(/\.html?$/i, "");
  const titleMatch = html.match(/<title>([^<]+)<\/title>/i);
  const title = (titleMatch?.[1] ?? slug).trim();
  const imageMatches = [...html.matchAll(/<img[^>]+src\s*=\s*["']([^"']*uploads[^"']+)["']/gi)]
    .map((match) => decodePath(match[1]))
    .map(stripQuery)
    .filter((value, index, list) => value && list.indexOf(value) === index)
    .slice(0, pageLimit);

  if (imageMatches.length === 0) {
    continue;
  }

  const copiedImages = imageMatches
    .map((relativePath, index) => copyLegacyImage(siteRoot, slug, relativePath, index))
    .filter(Boolean);

  if (copiedImages.length === 0) {
    continue;
  }

  manifest.push({
    id: slug,
    title,
    heroImage: copiedImages[0],
    galleryImages: copiedImages.slice(1)
  });
}

const fileContents = `export const generatedOreSpawnLegacy = ${JSON.stringify(manifest, null, 2)};\n`;
fs.writeFileSync(outputFile, fileContents, "utf8");

console.log(`Imported ${manifest.length} OreSpawn legacy pages into ${path.relative(repoRoot, outputFile)}`);

function stripQuery(value) {
  return value.split("?")[0];
}

function decodePath(value) {
  return String(value ?? "").replace(/\\/g, path.sep).replace(/\//g, path.sep);
}

function ensureDirectory(directory) {
  fs.mkdirSync(directory, { recursive: true });
}

function copyLegacyImage(root, slug, relativePath, index) {
  const normalized = relativePath.split(path.sep).join(path.sep);
  const source = path.join(root, normalized);

  if (!fs.existsSync(source)) {
    return null;
  }

  const outputDirectory = path.join(legacyAssetRoot, slug);
  ensureDirectory(outputDirectory);

  const parsed = path.parse(source);
  const safeBaseName = sanitizeFileName(parsed.name || `${slug}-${index + 1}`);
  const outputName = `${String(index + 1).padStart(2, "0")}-${safeBaseName}${parsed.ext.toLowerCase()}`;
  const destination = path.join(outputDirectory, outputName);

  fs.copyFileSync(source, destination);

  return normalizeBrowserPath(path.relative(docsRoot, destination));
}

function sanitizeFileName(value) {
  return value.replace(/[^a-z0-9-_]+/gi, "-").replace(/-+/g, "-").replace(/^-|-$/g, "") || "asset";
}

function normalizeBrowserPath(value) {
  return `./${value.split(path.sep).join("/")}`;
}
