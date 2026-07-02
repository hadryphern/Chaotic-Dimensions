from pathlib import Path
from PIL import Image, ImageDraw, ImageFont, ImageOps
import csv
import math
import textwrap

ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / "assets_work" / "report_annex_sheets"
OUT.mkdir(parents=True, exist_ok=True)
for old in OUT.glob("sheet_*.jpg"):
    old.unlink()

EXTENSIONS = {".png", ".jpg", ".jpeg", ".gif"}
files = []
for root_name in ("Content", "Assets", "assets_work"):
    root = ROOT / root_name
    for path in root.rglob("*"):
        if not path.is_file() or path.suffix.lower() not in EXTENSIONS:
            continue
        if OUT in path.parents:
            continue
        if any(part.lower() in {"backups", "backup", "kraken_replaced_backup"} for part in path.parts):
            continue
        files.append(path)

def group_key(path):
    rel = path.relative_to(ROOT).as_posix()
    if rel.startswith("Content/NPCs/Kraken") or rel.startswith("Content/Projectiles/Kraken") or rel.startswith("Content/Effects/Kraken") or rel.startswith("Content/Backgrounds/Kraken"):
        return "Runtime - Alien Kraken"
    if rel.startswith("Content/Bosses") or rel.startswith("Content/NPCs"):
        return "Runtime - bosses e NPCs"
    if rel.startswith("Content/Projectiles"):
        return "Runtime - projeteis"
    if rel.startswith("Content/Items/Armor"):
        return "Runtime - armaduras"
    if rel.startswith("Content/Items"):
        return "Runtime - itens"
    if rel.startswith("Content/Tiles") or rel.startswith("Content/Buffs") or rel.startswith("Assets/UI"):
        return "Runtime - tiles, buffs e interface"
    if rel.startswith("Assets/Source"):
        return "Fontes editaveis"
    if rel.startswith("assets_work/kraken_new_source"):
        return "Trabalho - fonte inicial do Kraken"
    if rel.startswith("assets_work/kraken_new_output/production_hd"):
        return "Trabalho - Kraken production HD"
    if rel.startswith("assets_work/kraken_new_output"):
        return "Trabalho - variantes iniciais do Kraken"
    if rel.startswith("assets_work/kraken_projectile_rework"):
        return "Trabalho - projeteis do Kraken"
    if rel.startswith("assets_work/kraken_initial_dark_rework"):
        return "Trabalho - Kraken final escuro"
    if rel.startswith("assets_work/kraken_restored_eyes"):
        return "Trabalho - restauracao dos olhos"
    if rel.startswith("assets_work/kraken_clean_hd"):
        return "Trabalho - auditoria HD"
    if rel.startswith("assets_work/kraken_edge_audit"):
        return "Trabalho - auditoria das bordas"
    if rel.startswith("assets_work/kraken_rework_exaggerated"):
        return "Trabalho - animacao exagerada"
    if rel.startswith("assets_work/kraken_rework_zero"):
        return "Trabalho - rework a partir do zero"
    if rel.startswith("Content/Backgrounds") or rel.startswith("Content/Effects"):
        return "Runtime - ambientes e efeitos"
    return "Outros recursos visuais"

files.sort(key=lambda p: (group_key(p), p.relative_to(ROOT).as_posix().lower()))
groups = []
runtime_files = [p for p in files if not p.relative_to(ROOT).as_posix().startswith("assets_work/")]
work_files = [p for p in files if p.relative_to(ROOT).as_posix().startswith("assets_work/")]
for scope, scoped_files in (("Sprites e recursos integrados", runtime_files), ("Evolucao e materiais de trabalho", work_files)):
    total_parts = math.ceil(len(scoped_files) / 20)
    for part, start in enumerate(range(0, len(scoped_files), 20), start=1):
        groups.append((f"{scope} ({part}/{total_parts})", scoped_files[start:start + 20]))

W, H = 1600, 1800
MARGIN = 34
TITLE_H = 82
FOOT_H = 42
GAP = 12
COLS = 4
ROWS = 5
CELL_W = (W - MARGIN * 2 - GAP * (COLS - 1)) // COLS
CELL_H = (H - MARGIN * 2 - TITLE_H - FOOT_H - GAP * (ROWS - 1)) // ROWS

font_paths = [
    "/usr/share/fonts/dejavu-sans-fonts/DejaVuSans.ttf",
    "/usr/share/fonts/dejavu/DejaVuSans.ttf",
]
font_bold_paths = [
    "/usr/share/fonts/dejavu-sans-fonts/DejaVuSans-Bold.ttf",
    "/usr/share/fonts/dejavu/DejaVuSans-Bold.ttf",
]
def load_font(paths, size):
    for path in paths:
        if Path(path).exists():
            return ImageFont.truetype(path, size)
    return ImageFont.load_default()

TITLE_FONT = load_font(font_bold_paths, 30)
LABEL_FONT = load_font(font_paths, 16)
SMALL_FONT = load_font(font_paths, 14)
FOOT_FONT = load_font(font_paths, 15)

def representative(path):
    with Image.open(path) as opened:
        opened.seek(0)
        image = opened.convert("RGBA")
    w, h = image.size
    name = path.name.lower()
    rel = path.relative_to(ROOT).as_posix().lower()
    if ("krakenboss" in name or "spritesheet" in name) and w >= 1200 and h >= 1200 and w % 6 == 0 and h % 6 == 0:
        image = image.crop((0, 0, w // 6, h // 6))
    elif "monthraboss.png" in name and h >= 12:
        image = image.crop((0, 0, w, h // 12))
    elif w <= 80 and h >= w * 3:
        if "monthrabutterfly" in name:
            frame_h = h // 6
        elif h % 4 == 0 and h // 4 <= max(96, w * 2):
            frame_h = h // 4
        else:
            frame_h = min(h, max(56, w))
        image = image.crop((0, 0, w, frame_h))
    return image

def checker(size, block=14):
    bg = Image.new("RGB", size, "#eef1f5")
    draw = ImageDraw.Draw(bg)
    for y in range(0, size[1], block):
        for x in range(0, size[0], block):
            if (x // block + y // block) % 2:
                draw.rectangle((x, y, x + block - 1, y + block - 1), fill="#dfe4eb")
    return bg

manifest_rows = []
for sheet_number, (title, paths) in enumerate(groups, start=1):
    canvas = Image.new("RGB", (W, H), "#ffffff")
    draw = ImageDraw.Draw(canvas)
    draw.rectangle((0, 0, W, TITLE_H), fill="#174f97")
    draw.text((MARGIN, 22), f"Prancha D.{sheet_number:02d} - {title}", font=TITLE_FONT, fill="white")

    for index, path in enumerate(paths):
        row, col = divmod(index, COLS)
        x = MARGIN + col * (CELL_W + GAP)
        y = MARGIN + TITLE_H + row * (CELL_H + GAP)
        draw.rounded_rectangle((x, y, x + CELL_W, y + CELL_H), radius=5, fill="#f8fafc", outline="#b9c5d2", width=2)
        image_box = (x + 12, y + 12, x + CELL_W - 12, y + CELL_H - 76)
        box_w = image_box[2] - image_box[0]
        box_h = image_box[3] - image_box[1]
        sprite = representative(path)
        original_size = Image.open(path).size
        contained = ImageOps.contain(sprite, (box_w - 12, box_h - 12), method=Image.Resampling.LANCZOS if max(sprite.size) > 512 else Image.Resampling.NEAREST)
        bg = checker((box_w, box_h))
        px = (box_w - contained.width) // 2
        py = (box_h - contained.height) // 2
        bg.paste(contained, (px, py), contained)
        canvas.paste(bg, (image_box[0], image_box[1]))

        rel = path.relative_to(ROOT).as_posix()
        parent = str(Path(rel).parent)
        label = path.name
        lines = textwrap.wrap(label, width=35)[:2]
        label_y = y + CELL_H - 68
        for line in lines:
            bbox = draw.textbbox((0, 0), line, font=LABEL_FONT)
            draw.text((x + (CELL_W - (bbox[2] - bbox[0])) / 2, label_y), line, font=LABEL_FONT, fill="#17202b")
            label_y += 19
        detail = f"{parent} | {original_size[0]}x{original_size[1]}"
        if len(detail) > 48:
            detail = "..." + detail[-45:]
        bbox = draw.textbbox((0, 0), detail, font=SMALL_FONT)
        draw.text((x + (CELL_W - (bbox[2] - bbox[0])) / 2, y + CELL_H - 23), detail, font=SMALL_FONT, fill="#536273")
        manifest_rows.append((sheet_number, title, rel, original_size[0], original_size[1]))

    footer = f"Catalogo visual do projeto - {len(paths)} recursos nesta prancha - {len(files)} recursos inventariados"
    draw.text((MARGIN, H - FOOT_H + 10), footer, font=FOOT_FONT, fill="#465568")
    out = OUT / f"sheet_{sheet_number:02d}.jpg"
    canvas.save(out, "JPEG", quality=88, optimize=True, progressive=True)

with (OUT / "manifest.csv").open("w", encoding="utf-8", newline="") as handle:
    writer = csv.writer(handle)
    writer.writerow(("prancha", "categoria", "ficheiro", "largura", "altura"))
    writer.writerows(manifest_rows)

print(f"Generated {len(groups)} sheets for {len(files)} visual resources in {OUT}")
