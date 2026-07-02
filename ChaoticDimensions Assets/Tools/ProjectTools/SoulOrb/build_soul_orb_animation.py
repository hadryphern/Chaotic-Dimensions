from pathlib import Path
import math

from PIL import Image, ImageEnhance


ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / "assets_work/soul_orb/SoulOrb_Clean_Source_Transparent.png"
WORK = ROOT / "assets_work/soul_orb"
GAME = ROOT / "Assets/SoulOrb"
FRAME_SIZE = 256
FRAME_COUNT = 64
ATLAS_COLUMNS = 8
ORB_DIAMETER = 120
ORBIT_X = 54
ORBIT_Y = 24


def prepare_orb() -> Image.Image:
    source = Image.open(SOURCE).convert("RGBA")
    bounds = source.getchannel("A").point(lambda value: 255 if value >= 12 else 0).getbbox()
    if bounds is None:
        raise RuntimeError("Soul orb source has no visible pixels")

    orb = source.crop(bounds)
    return orb.resize((ORB_DIAMETER, ORB_DIAMETER), Image.Resampling.LANCZOS)


def transform_orb(orb: Image.Image, depth: float, phase: float) -> Image.Image:
    breath = (math.sin(phase * 2.0) + 1.0) * 0.5
    scale = 0.91 + depth * 0.15 + breath * 0.025
    diameter = round(ORB_DIAMETER * scale)
    animated = orb.resize((diameter, diameter), Image.Resampling.LANCZOS)
    animated = ImageEnhance.Brightness(animated).enhance(0.90 + depth * 0.12)
    alpha = 0.82 + depth * 0.18
    animated.putalpha(animated.getchannel("A").point(lambda value: round(value * alpha)))
    return animated


def build_frame(orb: Image.Image, index: int) -> Image.Image:
    theta = index / FRAME_COUNT * math.tau
    frame = Image.new("RGBA", (FRAME_SIZE, FRAME_SIZE), (0, 0, 0, 0))

    # O plano da órbita muda durante o ciclo para evitar uma rotação horizontal rígida.
    orientation = (
        math.sin(theta) * math.radians(58)
        + math.sin(theta * 2.0 + 0.7) * math.radians(24)
        + math.sin(theta * 3.0 - 0.4) * math.radians(10)
    )
    separation = 0.88 + (math.sin(theta * 3.0 + 0.9) + 1.0) * 0.10
    center_drift_x = math.sin(theta * 2.0 + 0.3) * 6.0
    center_drift_y = math.sin(theta * 3.0 - 0.5) * 7.5

    states = []
    for phase in (theta, theta + math.pi):
        local_x = math.cos(phase) * ORBIT_X * separation
        local_y = math.sin(phase) * ORBIT_Y
        x = local_x * math.cos(orientation) - local_y * math.sin(orientation)
        y = local_x * math.sin(orientation) + local_y * math.cos(orientation)
        depth = (math.sin(phase) + 1.0) * 0.5
        states.append((depth, x, y, phase))

    # A esfera mais distante é desenhada primeiro para a passagem parecer contínua.
    for depth, x, y, phase in sorted(states):
        animated = transform_orb(orb, depth, phase)
        position = (
            round(FRAME_SIZE / 2 + center_drift_x + x - animated.width / 2),
            round(FRAME_SIZE / 2 + center_drift_y + y - animated.height / 2),
        )
        frame.alpha_composite(animated, position)
    return frame


def on_dark_background(frame: Image.Image, scale: int = 2) -> Image.Image:
    size = (FRAME_SIZE * scale, FRAME_SIZE * scale)
    preview = Image.new("RGBA", size, (0, 0, 0, 255))
    preview.alpha_composite(frame.resize(size, Image.Resampling.LANCZOS))
    return preview


def main() -> None:
    WORK.mkdir(parents=True, exist_ok=True)
    GAME.mkdir(parents=True, exist_ok=True)
    orb = prepare_orb()
    frames = [build_frame(orb, index) for index in range(FRAME_COUNT)]

    atlas_rows = math.ceil(FRAME_COUNT / ATLAS_COLUMNS)
    atlas = Image.new(
        "RGBA",
        (FRAME_SIZE * ATLAS_COLUMNS, FRAME_SIZE * atlas_rows),
        (0, 0, 0, 0),
    )
    for index, frame in enumerate(frames):
        atlas.alpha_composite(
            frame,
            ((index % ATLAS_COLUMNS) * FRAME_SIZE, (index // ATLAS_COLUMNS) * FRAME_SIZE),
        )
    atlas.save(GAME / "SoulOrb_Atlas.png", optimize=True)

    previews = [on_dark_background(frame) for frame in frames]
    previews[0].save(
        WORK / "SoulOrb_Animated_Preview.gif",
        save_all=True,
        append_images=previews[1:],
        duration=80,
        loop=0,
        disposal=2,
    )

    columns = ATLAS_COLUMNS
    rows = math.ceil(FRAME_COUNT / columns)
    comparison = Image.new("RGBA", (FRAME_SIZE * columns, FRAME_SIZE * rows), (0, 0, 0, 255))
    for index, frame in enumerate(frames):
        comparison.alpha_composite(frame, ((index % columns) * FRAME_SIZE, (index // columns) * FRAME_SIZE))
    comparison.save(WORK / "SoulOrb_64_Frames.png", optimize=True)

    print(f"Atlas: {GAME / 'SoulOrb_Atlas.png'}")
    print(f"Preview: {WORK / 'SoulOrb_Animated_Preview.gif'}")


if __name__ == "__main__":
    main()
