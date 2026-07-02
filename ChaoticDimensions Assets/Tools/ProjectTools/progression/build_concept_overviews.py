#!/usr/bin/env python3
"""Build compact overview boards for the PAP report."""

from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / "assets_work" / "concept_sketches" / "items"
OUTPUT = ROOT / "assets_work" / "concept_sketches"


def font(size: int, bold: bool = False) -> ImageFont.FreeTypeFont:
    name = "DejaVuSans-Bold.ttf" if bold else "DejaVuSans.ttf"
    return ImageFont.truetype(name, size)


def build_board(indices: range, output_name: str, subtitle: str) -> None:
    canvas = Image.new("RGB", (2200, 2700), "#f4f7fb")
    draw = ImageDraw.Draw(canvas)
    draw.rectangle((0, 0, 2200, 210), fill="#15243a")
    draw.text((90, 52), "CATALOGO VISUAL DE ITENS", fill="white", font=font(62, True))
    draw.text((92, 130), subtitle, fill="#a9d4ff", font=font(31))

    slots = [(70, 260), (1135, 260), (70, 1430), (1135, 1430)]
    for slot, index in zip(slots, indices):
        source = Image.open(SOURCE / f"items_{index:02d}.png").convert("RGB")
        source.thumbnail((995, 1080), Image.Resampling.LANCZOS)
        x, y = slot
        panel = Image.new("RGB", (995, 1080), "white")
        panel.paste(source, ((995 - source.width) // 2, (1080 - source.height) // 2))
        canvas.paste(panel, (x, y))
        draw.rectangle((x, y, x + 995, y + 1080), outline="#6684a3", width=4)

    canvas.save(OUTPUT / output_name, quality=93, subsampling=0)


def main() -> None:
    OUTPUT.mkdir(parents=True, exist_ok=True)
    build_board(range(1, 5), "Item_Sheets_Overview_A.jpg", "Itens 1-100: inicio, pre-Hardmode e Hardmode inicial")
    build_board(range(5, 9), "Item_Sheets_Overview_B.jpg", "Itens 101-200: Hardmode avancado e pos-Moon Lord")

    # The final two sheets deserve more space because they contain the endgame tiers.
    canvas = Image.new("RGB", (2200, 2700), "#f4f7fb")
    draw = ImageDraw.Draw(canvas)
    draw.rectangle((0, 0, 2200, 210), fill="#15243a")
    draw.text((90, 52), "CATALOGO VISUAL DE ITENS", fill="white", font=font(62, True))
    draw.text((92, 130), "Itens 201-245: Crystaline, Kraken e Chaotic", fill="#a9d4ff", font=font(31))
    for y, index in ((260, 9), (1450, 10)):
        source = Image.open(SOURCE / f"items_{index:02d}.png").convert("RGB")
        source.thumbnail((1980, 1120), Image.Resampling.LANCZOS)
        x = (2200 - source.width) // 2
        canvas.paste(source, (x, y))
        draw.rectangle((x, y, x + source.width, y + source.height), outline="#6684a3", width=4)
    canvas.save(OUTPUT / "Item_Sheets_Overview_C.jpg", quality=93, subsampling=0)


if __name__ == "__main__":
    main()
