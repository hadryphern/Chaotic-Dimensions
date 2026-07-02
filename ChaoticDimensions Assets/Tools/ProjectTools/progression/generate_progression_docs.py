from pathlib import Path
from collections import defaultdict
from PIL import Image, ImageDraw, ImageFont
import csv
import math
import textwrap

ROOT = Path(__file__).resolve().parents[2]
TSV = ROOT / "Content" / "Items" / "Progression" / "ProgressionItems.tsv"
DOC = ROOT / "docs" / "Catalogo_245_Itens.md"
OUT = ROOT / "assets_work" / "concept_sketches"
ITEM_OUT = OUT / "items"
OUT.mkdir(parents=True, exist_ok=True)
ITEM_OUT.mkdir(parents=True, exist_ok=True)

tiers = [
    "Superfície", "King Slime", "Eye of Cthulhu", "Evil Boss", "Queen Bee",
    "Skeletron", "Wall of Flesh", "Mechanical Boss", "All Mechs", "Plantera",
    "Golem", "Lunatic Cultist", "Moon Lord", "Monthra", "Crystaline Devourer",
    "Alien Kraken", "Chaotic Endgame"
]
gates = [
    "Anytime", "PostKingSlime", "PostEyeOfCthulhu", "PostEvilBoss", "PostQueenBee",
    "PostSkeletron", "PostWallOfFlesh", "PostAnyMech", "PostAllMechs", "PostPlantera",
    "PostGolem", "PostCultist", "PostMoonLord", "PostMonthra",
    "PostCrystalineDevourer", "PostKraken", "PostKraken"
]
damage = [9, 15, 24, 34, 46, 62, 86, 125, 180, 265, 390, 620, 1450, 160000, 12500000, 25000000, 50000000]

rows = []
with TSV.open(encoding="utf-8") as handle:
    reader = csv.DictReader(handle, delimiter="\t")
    for row in reader:
        row["index"] = int(row["index"])
        row["tier"] = int(row["tier"])
        row["variant"] = int(row["variant"])
        rows.append(row)
if len(rows) != 245:
    raise RuntimeError(len(rows))

groups = defaultdict(list)
for row in rows:
    groups[row["kind"]].append(row)

doc = [
    "# Catálogo dos 245 itens de progressão",
    "",
    "Todos os itens abaixo são tipos reais de ModItem. Enquanto não recebem sprites próprias, usam ícones vanilla apenas como representação temporária.",
    "",
    "## Distribuição",
    "",
]
for kind in ("Melee", "Ranged", "Magic", "Summon", "Accessory", "Tool", "Consumable", "Material"):
    doc.append(f"- **{kind}:** {len(groups[kind])} itens")
doc += [
    "",
    "## Progressão de dano das armas",
    "",
    "| Tier | Requisito | Dano base |",
    "|---:|---|---:|",
]
for index, (tier, gate, value) in enumerate(zip(tiers, gates, damage)):
    doc.append(f"| {index} — {tier} | {gate} | {value:,} |".replace(",", " "))
doc += ["", "## Lista individual", ""]

for kind in ("Melee", "Ranged", "Magic", "Summon", "Accessory", "Tool", "Consumable", "Material"):
    doc += [f"### {kind}", "", "| # | Item | Tier | Requisito | Dano base |", "|---:|---|---|---|---:|"]
    for row in groups[kind]:
        value = damage[row["tier"]] if kind in {"Melee", "Ranged", "Magic", "Summon"} else "—"
        if isinstance(value, int):
            value = f"{value:,}".replace(",", " ")
        doc.append(f"| {row['index']} | {row['name']} | {row['tier']} — {tiers[row['tier']]} | {gates[row['tier']]} | {value} |")
    doc.append("")

DOC.write_text("\n".join(doc), encoding="utf-8")

font_candidates = [
    "/usr/share/fonts/dejavu-sans-fonts/DejaVuSans.ttf",
    "/usr/share/fonts/dejavu/DejaVuSans.ttf",
]
bold_candidates = [
    "/usr/share/fonts/dejavu-sans-fonts/DejaVuSans-Bold.ttf",
    "/usr/share/fonts/dejavu/DejaVuSans-Bold.ttf",
]
def font(paths, size):
    for value in paths:
        if Path(value).exists():
            return ImageFont.truetype(value, size)
    return ImageFont.load_default()

F16 = font(font_candidates, 16)
F18 = font(font_candidates, 18)
F22 = font(bold_candidates, 22)
F28 = font(bold_candidates, 28)
F36 = font(bold_candidates, 36)

colors = {
    "Melee": "#e45757", "Ranged": "#e1a83b", "Magic": "#547fe4", "Summon": "#9a62d5",
    "Accessory": "#42a981", "Tool": "#75808c", "Consumable": "#db67a0", "Material": "#7b6e62"
}
tier_colors = [
    "#8b98a5", "#4f8fd3", "#4aaa73", "#d7953d", "#d75d5d", "#df68a6",
    "#a06cdc", "#6bbd52", "#d6ba3f", "#4bc5c7", "#d94848", "#9f5cd1",
    "#e865a7", "#8b63d8", "#477bd9", "#304b79", "#a42152"
]

def draw_icon(draw, box, kind, color):
    x1, y1, x2, y2 = box
    cx, cy = (x1 + x2) // 2, (y1 + y2) // 2
    if kind == "Melee":
        draw.line((cx - 35, cy + 38, cx + 28, cy - 35), fill=color, width=11)
        draw.polygon([(cx + 20, cy - 43), (cx + 43, cy - 58), (cx + 35, cy - 27)], fill=color)
        draw.line((cx - 48, cy + 22, cx - 18, cy + 48), fill="#d9e0e8", width=7)
    elif kind == "Ranged":
        draw.arc((cx - 45, cy - 48, cx + 45, cy + 48), 70, 290, fill=color, width=10)
        draw.line((cx + 16, cy - 42, cx + 16, cy + 42), fill="#d9e0e8", width=5)
        draw.line((cx - 35, cy, cx + 50, cy), fill=color, width=6)
    elif kind == "Magic":
        draw.ellipse((cx - 38, cy - 38, cx + 38, cy + 38), outline=color, width=9)
        draw.polygon([(cx, cy - 52), (cx + 15, cy - 12), (cx + 52, cy), (cx + 15, cy + 12), (cx, cy + 52), (cx - 15, cy + 12), (cx - 52, cy), (cx - 15, cy - 12)], fill=color)
    elif kind == "Summon":
        draw.ellipse((cx - 42, cy - 25, cx + 42, cy + 25), outline=color, width=9)
        draw.ellipse((cx - 12, cy - 12, cx + 12, cy + 12), fill=color)
        draw.arc((cx - 52, cy - 52, cx + 52, cy + 52), 200, 340, fill="#d9e0e8", width=6)
    elif kind == "Accessory":
        draw.ellipse((cx - 38, cy - 38, cx + 38, cy + 38), outline=color, width=12)
        draw.ellipse((cx - 14, cy - 14, cx + 14, cy + 14), fill=color)
    elif kind == "Tool":
        draw.line((cx - 35, cy + 45, cx + 15, cy - 25), fill="#d9e0e8", width=12)
        draw.arc((cx - 2, cy - 48, cx + 55, cy + 2), 180, 340, fill=color, width=13)
    elif kind == "Consumable":
        draw.rounded_rectangle((cx - 30, cy - 38, cx + 30, cy + 42), radius=12, fill=color, outline="#e8edf3", width=6)
        draw.rectangle((cx - 14, cy - 54, cx + 14, cy - 35), fill="#d9e0e8")
    else:
        draw.polygon([(cx, cy - 48), (cx + 42, cy - 10), (cx + 26, cy + 42), (cx - 26, cy + 42), (cx - 42, cy - 10)], fill=color, outline="#e8edf3")

# Ten sheets, 25 item concepts per sheet.
for sheet_index, start in enumerate(range(0, len(rows), 25), start=1):
    batch = rows[start:start + 25]
    image = Image.new("RGB", (2000, 2000), "#111820")
    draw = ImageDraw.Draw(image)
    draw.rectangle((0, 0, 2000, 100), fill="#174f97")
    draw.text((45, 28), f"Chaotic Dimensions — itens {start + 1} a {start + len(batch)}", font=F36, fill="white")
    for index, row in enumerate(batch):
        grid_y, grid_x = divmod(index, 5)
        x = 30 + grid_x * 394
        y = 125 + grid_y * 365
        draw.rounded_rectangle((x, y, x + 370, y + 340), radius=7, fill="#1c2631", outline=tier_colors[row["tier"]], width=4)
        draw.rectangle((x, y, x + 370, y + 34), fill=tier_colors[row["tier"]])
        draw.text((x + 10, y + 7), f"Tier {row['tier']} — {tiers[row['tier']]}", font=F16, fill="white")
        draw_icon(draw, (x + 115, y + 55, x + 255, y + 195), row["kind"], colors[row["kind"]])
        name_lines = textwrap.wrap(row["name"], width=23)[:2]
        ty = y + 210
        for line in name_lines:
            bbox = draw.textbbox((0, 0), line, font=F22)
            draw.text((x + (370 - (bbox[2] - bbox[0])) / 2, ty), line, font=F22, fill="#f3f6fa")
            ty += 26
        draw.text((x + 12, y + 284), row["kind"], font=F18, fill=colors[row["kind"]])
        draw.text((x + 12, y + 309), "sprite pendente", font=F16, fill="#9aa8b6")
    image.save(ITEM_OUT / f"items_{sheet_index:02d}.png", optimize=True)

def card_board(path, title, cards, columns=3):
    rows_count = math.ceil(len(cards) / columns)
    width, height = 1800, 160 + rows_count * 300
    image = Image.new("RGB", (width, height), "#f5f7fa")
    draw = ImageDraw.Draw(image)
    draw.rectangle((0, 0, width, 110), fill="#174f97")
    draw.text((45, 28), title, font=F36, fill="white")
    cell_w = (width - 80) // columns
    for index, (name, status, summary, kind) in enumerate(cards):
        row, col = divmod(index, columns)
        x = 30 + col * cell_w
        y = 135 + row * 300
        draw.rounded_rectangle((x, y, x + cell_w - 20, y + 270), radius=7, fill="white", outline="#9fb0c2", width=3)
        color = colors.get(kind, "#7359b7")
        draw_icon(draw, (x + 20, y + 45, x + 150, y + 175), kind if kind in colors else "Magic", color)
        draw.text((x + 165, y + 25), name, font=F22, fill="#17202b")
        draw.text((x + 165, y + 58), status, font=F18, fill=color)
        wrapped = textwrap.wrap(summary, width=43)[:5]
        for line_index, line in enumerate(wrapped):
            draw.text((x + 165, y + 92 + line_index * 23), line, font=F18, fill="#475665")
    image.save(path, optimize=True)

bosses = [
    ("Monthra", "Implementada", "Boss pós-Moon Lord com dashes, lasers prismáticos, grelha segura e enxame.", "Summon"),
    ("Crystaline Devourer", "Implementado", "Dois vermes de vida partilhada dentro de uma arena temporária restaurável.", "Magic"),
    ("Alien Kraken", "Implementado", "Evento de tempestade com Ruby, clones, raios, água e defesa por fases.", "Magic"),
    ("Magnet Mage", "Conceito em código", "Controla impulsos, redes magnéticas, atração e reposicionamento do jogador.", "Magic"),
    ("Mimic Clown", "Conceito em código", "Boss final de ritmo que imita bosses antes de revelar a forma verdadeira.", "Melee"),
    ("Mimic Clown — True Form", "Planeada", "Forma final do Palhaço Mímico, com defesa rítmica e ataques de encerramento.", "Accessory"),
]
mobs = [
    ("Phantasm", "Existente", "Ameaça aérea do Shadow Biome.", "Magic"),
    ("Shadow Eye", "Existente", "Olho perseguidor com movimentação irregular.", "Magic"),
    ("Shadow Slime", "Existente", "Inimigo terrestre básico do bioma.", "Material"),
    ("Shadow Worm", "Existente", "Criatura segmentada subterrânea.", "Melee"),
    ("Kraken Squid", "Existente", "Criatura aquática ligada ao legado do Kraken.", "Summon"),
    ("Monthra Moth", "Existente", "Minion hostil invocado durante a luta.", "Summon"),
    ("Magnetized Wisp", "Planeado", "Orbe que altera levemente a trajetória do jogador.", "Magic"),
    ("Polar Sentinel", "Planeado", "Guardião que alterna polos de atração e repulsão.", "Tool"),
    ("Iron Net Caster", "Planeado", "Suporte do Magnet Mage que lança redes lentas.", "Ranged"),
    ("Circus Mimic", "Planeado", "Baú-armadilha ligado ao Mimic Clown.", "Melee"),
    ("Rhythm Marionette", "Planeado", "Ataca no pulso da música com janelas previsíveis.", "Summon"),
    ("Crystal Crawler", "Planeado", "Inimigo pós-Crystaline que deixa fragmentos.", "Material"),
    ("Abyssal Drone", "Planeado", "Unidade pós-Kraken que protege núcleos abissais.", "Ranged"),
    ("Chaotic Echo", "Planeado", "Cópia curta de ataques de bosses anteriores.", "Accessory"),
]
card_board(OUT / "Boss_Roster_Blueprint.png", "Esboço dos bosses e encontros principais", bosses, 3)
card_board(OUT / "Mob_Roster_Blueprint.png", "Esboço dos mobs existentes e planeados", mobs, 2)

# Tier map.
image = Image.new("RGB", (1900, 1700), "#f5f7fa")
draw = ImageDraw.Draw(image)
draw.rectangle((0, 0, 1900, 110), fill="#174f97")
draw.text((45, 28), "Mapa da progressão dos 245 itens", font=F36, fill="white")
for index, tier in enumerate(tiers):
    y = 140 + index * 88
    draw.rounded_rectangle((55, y, 1845, y + 70), radius=5, fill="white", outline=tier_colors[index], width=4)
    draw.rectangle((55, y, 255, y + 70), fill=tier_colors[index])
    draw.text((78, y + 20), f"Tier {index}", font=F22, fill="white")
    draw.text((285, y + 13), tier, font=F22, fill="#17202b")
    draw.text((700, y + 13), gates[index], font=F18, fill="#526171")
    draw.text((1260, y + 13), f"Dano: {damage[index]:,}".replace(",", " "), font=F18, fill="#9a2949" if damage[index] >= 10000000 else "#33495f")
    counts = defaultdict(int)
    for row in rows:
        if row["tier"] == index:
            counts[row["kind"]] += 1
    summary = " | ".join(f"{key} {value}" for key, value in counts.items())
    draw.text((285, y + 40), summary, font=F16, fill="#687787")
image.save(OUT / "Item_Progression_Blueprint.png", optimize=True)

print("Wrote catalog", DOC)
print("Wrote", math.ceil(len(rows) / 25), "item sheets and 3 roster/progression boards")
