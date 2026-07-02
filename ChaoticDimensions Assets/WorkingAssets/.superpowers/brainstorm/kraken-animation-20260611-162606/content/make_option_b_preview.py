from __future__ import annotations

import math
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter


SOURCE_DIR = Path(r"C:\Users\unknown\Downloads\novo-kraken")
OUTPUT_DIR = Path(__file__).resolve().parent
SCALE = 2
FRAME_COUNT = 24


def load_layer(index: int) -> Image.Image:
    image = Image.open(SOURCE_DIR / f"pixil-layer-{index}.png").convert("RGBA")
    image = image.resize(
        (image.width * SCALE, image.height * SCALE),
        Image.Resampling.NEAREST,
    )

    pixels = image.load()
    for y in range(image.height):
        for x in range(image.width):
            red, green, blue, alpha = pixels[x, y]
            if alpha and red < 48 and green < 48 and blue < 55:
                pixels[x, y] = (8, 18, 58, alpha)
    return image


def wave_layer(
    source: Image.Image,
    root_y: float,
    phase: float,
    amplitude: float,
    frequency: float,
    direction: float = 1.0,
) -> Image.Image:
    result = Image.new("RGBA", source.size)
    root = int(root_y * SCALE)
    amplitude *= SCALE

    for y in range(source.height):
        if y <= root:
            shift = 0
        else:
            progress = (y - root) / max(1, source.height - root)
            envelope = progress**1.45
            angle = phase + frequency * math.pi * progress
            shift = round(direction * amplitude * envelope * math.sin(angle))

        row = source.crop((0, y, source.width, y + 1))
        result.alpha_composite(row, (shift, y))

    return result


def breathe_layer(source: Image.Image, phase: float, strength: float) -> Image.Image:
    width_scale = 1.0 + math.sin(phase) * strength
    new_width = max(1, round(source.width * width_scale))
    resized = source.resize((new_width, source.height), Image.Resampling.BICUBIC)
    result = Image.new("RGBA", source.size)
    result.alpha_composite(resized, ((source.width - new_width) // 2, 0))
    return result


def add_unified_outline(sprite: Image.Image) -> Image.Image:
    alpha = sprite.getchannel("A")
    expanded = alpha.filter(ImageFilter.MaxFilter(11))
    outline_alpha = Image.eval(
        Image.fromarray(
            __import__("numpy").maximum(
                __import__("numpy").array(expanded, dtype="int16")
                - __import__("numpy").array(alpha, dtype="int16"),
                0,
            ).astype("uint8")
        ),
        lambda value: min(255, round(value * 0.92)),
    )
    outline = Image.new("RGBA", sprite.size, (7, 15, 52, 0))
    outline.putalpha(outline_alpha)
    outline.alpha_composite(sprite)
    return outline


def make_background(width: int, height: int) -> Image.Image:
    background = Image.new("RGBA", (width, height), (4, 10, 31, 255))
    draw = ImageDraw.Draw(background)
    for y in range(height):
        ratio = y / max(1, height - 1)
        color = (
            round(5 + ratio * 5),
            round(12 + ratio * 8),
            round(38 + ratio * 25),
            255,
        )
        draw.line((0, y, width, y), fill=color)

    stars = [
        (37, 61, 2),
        (91, 146, 1),
        (318, 86, 2),
        (410, 188, 1),
        (52, 376, 1),
        (389, 522, 2),
        (276, 640, 1),
        (128, 713, 2),
    ]
    for x, y, radius in stars:
        draw.ellipse(
            (x - radius, y - radius, x + radius, y + radius),
            fill=(116, 174, 238, 180),
        )
    return background


def render_frame(frame_index: int, layers: list[Image.Image]) -> Image.Image:
    phase = math.tau * frame_index / FRAME_COUNT
    canvas = Image.new("RGBA", layers[0].size)
    bob = round(math.sin(phase) * 5 * SCALE)

    long_specs = (
        (0, 307, 34, 2.35, 1.0, 0.00),
        (1, 298, 38, 2.15, -1.0, 1.15),
        (2, 304, 27, 2.55, 1.0, 2.35),
    )
    for index, root_y, amplitude, frequency, direction, offset in long_specs:
        animated = wave_layer(
            layers[index],
            root_y,
            phase + offset,
            amplitude,
            frequency,
            direction,
        )
        canvas.alpha_composite(animated, (0, bob))

    body = breathe_layer(layers[10], phase, 0.016)
    canvas.alpha_composite(body, (0, bob))

    small_specs = (
        (3, 305, 12, 2.2, 1.0, 0.2),
        (4, 304, 14, 2.4, -1.0, 1.0),
        (5, 304, 11, 2.7, 1.0, 1.8),
        (6, 305, 10, 2.5, -1.0, 2.6),
        (7, 304, 9, 2.8, 1.0, 3.4),
        (8, 305, 8, 2.6, -1.0, 4.2),
        (9, 305, 6, 2.9, 1.0, 5.0),
    )
    for index, root_y, amplitude, frequency, direction, offset in small_specs:
        animated = wave_layer(
            layers[index],
            root_y,
            phase + offset,
            amplitude,
            frequency,
            direction,
        )
        canvas.alpha_composite(animated, (0, bob))

    canvas.alpha_composite(layers[11], (0, bob))
    canvas = add_unified_outline(canvas)

    crop = canvas.crop((68 * SCALE, 0, 515 * SCALE, 990 * SCALE))
    crop = crop.resize((358, 792), Image.Resampling.LANCZOS)
    background = make_background(456, 792)
    background.alpha_composite(crop, ((456 - crop.width) // 2, 0))
    return background.convert("RGB")


def main() -> None:
    layers = [load_layer(index) for index in range(14)]
    frames = [render_frame(index, layers) for index in range(FRAME_COUNT)]

    frames[0].save(
        OUTPUT_DIR / "option-b-preview.gif",
        save_all=True,
        append_images=frames[1:],
        duration=70,
        loop=0,
        disposal=2,
        optimize=False,
    )

    selected = (0, 6, 12, 18)
    storyboard = Image.new("RGB", (456 * 4, 792), (5, 12, 38))
    for column, frame_index in enumerate(selected):
        storyboard.paste(frames[frame_index], (column * 456, 0))
    storyboard.save(OUTPUT_DIR / "option-b-storyboard.png")


if __name__ == "__main__":
    main()
