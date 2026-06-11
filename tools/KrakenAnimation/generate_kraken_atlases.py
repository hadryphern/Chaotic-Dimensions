from __future__ import annotations

import json
import sys
from pathlib import Path

from PIL import Image, ImageDraw

PROJECT_ROOT = Path(__file__).resolve().parents[2]
if str(PROJECT_ROOT) not in sys.path:
    sys.path.insert(0, str(PROJECT_ROOT))

from tools.KrakenAnimation.kraken_animation import (
    generate_atlas,
    generate_frames,
    load_rig,
)


RIG_PATH = PROJECT_ROOT / "tools" / "KrakenAnimation" / "kraken_rig.json"
SOURCE_DIR = PROJECT_ROOT / "Assets" / "Source" / "Kraken"
CONTENT_DIR = PROJECT_ROOT / "Content" / "NPCs" / "Kraken"
OUTPUT_DIR = PROJECT_ROOT / "tools" / "KrakenAnimation" / "output"

MOTIONS = {
    "idle": "KrakenBoss.png",
    "loop_back": "KrakenBossLoopBack.png",
    "horizontal": "KrakenBossMoveHorizontal.png",
    "up": "KrakenBossMoveUp.png",
}
STORYBOARD_FRAMES = (0, 6, 12, 18, 24, 30, 35)


def _save_preview(
    motion: str,
    frames: list[Image.Image],
) -> None:
    background_frames: list[Image.Image] = []
    for frame in frames:
        background = Image.new("RGBA", frame.size, (4, 10, 31, 255))
        background.alpha_composite(frame)
        background_frames.append(background.convert("P", palette=Image.Palette.ADAPTIVE))

    background_frames[0].save(
        OUTPUT_DIR / f"kraken-{motion}.gif",
        save_all=True,
        append_images=background_frames[1:],
        duration=75,
        loop=0,
        disposal=2,
        optimize=False,
    )


def _save_storyboard(
    motion: str,
    frames: list[Image.Image],
) -> None:
    frame_width, frame_height = frames[0].size
    sheet = Image.new(
        "RGBA",
        (frame_width * len(STORYBOARD_FRAMES), frame_height),
        (4, 10, 31, 255),
    )
    draw = ImageDraw.Draw(sheet)
    for column, frame_index in enumerate(STORYBOARD_FRAMES):
        x = column * frame_width
        sheet.alpha_composite(frames[frame_index], (x, 0))
        draw.text(
            (x + 10, 10),
            f"{frame_index + 1:02d}",
            fill=(190, 216, 255, 255),
        )
    sheet.save(OUTPUT_DIR / f"kraken-{motion}-storyboard.png")


def main() -> int:
    rig = load_rig(RIG_PATH)
    CONTENT_DIR.mkdir(parents=True, exist_ok=True)
    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)

    metadata = {
        "frame_size": [rig.frame_width, rig.frame_height],
        "frame_count": rig.frame_count,
        "atlas_grid": [rig.atlas_columns, rig.atlas_rows],
        "ruby_offset": [
            rig.source_placement[0] + rig.ruby_anchor[0] - rig.frame_width / 2,
            rig.source_placement[1] + rig.ruby_anchor[1] - rig.frame_height / 2,
        ],
    }

    for motion, filename in MOTIONS.items():
        print(f"Generating {motion}...")
        frames = generate_frames(motion, rig, SOURCE_DIR)
        atlas = generate_atlas(frames, rig)
        atlas.save(CONTENT_DIR / filename, compress_level=7)
        _save_preview(motion, frames)
        _save_storyboard(motion, frames)
        print(f"{motion}: {len(frames)} frames written")

    (OUTPUT_DIR / "kraken-runtime-metadata.json").write_text(
        json.dumps(metadata, indent=2) + "\n",
        encoding="utf-8",
    )
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
