from __future__ import annotations

import sys
from pathlib import Path

import numpy as np
from PIL import Image

PROJECT_ROOT = Path(__file__).resolve().parents[2]
if str(PROJECT_ROOT) not in sys.path:
    sys.path.insert(0, str(PROJECT_ROOT))

from tools.KrakenAnimation.kraken_animation import Rig, load_rig


RIG_PATH = PROJECT_ROOT / "tools" / "KrakenAnimation" / "kraken_rig.json"
CONTENT_DIR = PROJECT_ROOT / "Content" / "NPCs" / "Kraken"
ATLASES = {
    "idle": "KrakenBoss.png",
    "loop_back": "KrakenBossLoopBack.png",
    "horizontal": "KrakenBossMoveHorizontal.png",
    "up": "KrakenBossMoveUp.png",
}


def split_atlas(atlas: Image.Image, rig: Rig) -> list[Image.Image]:
    frames: list[Image.Image] = []
    for frame_index in range(rig.frame_count):
        column = frame_index % rig.atlas_columns
        row = frame_index // rig.atlas_columns
        left = column * rig.frame_width
        top = row * rig.frame_height
        frames.append(
            atlas.crop(
                (
                    left,
                    top,
                    left + rig.frame_width,
                    top + rig.frame_height,
                )
            ).convert("RGBA")
        )
    return frames


def alpha_delta(first: Image.Image, second: Image.Image) -> float:
    first_alpha = np.asarray(first.getchannel("A"), dtype=np.int16)
    second_alpha = np.asarray(second.getchannel("A"), dtype=np.int16)
    return float(np.abs(first_alpha - second_alpha).mean() / 255.0)


def validate_frame(
    frame: Image.Image,
    rig: Rig,
    label: str,
) -> list[str]:
    errors: list[str] = []
    pixels = np.asarray(frame)
    alpha = pixels[:, :, 3]
    if not np.all(pixels[alpha == 0, :3] == 0):
        errors.append(f"{label}: transparent pixels contain RGB data")

    low_alpha = pixels[(alpha > 0) & (alpha < 24), :3].astype(np.int16)
    if len(low_alpha):
        green_fringe = (
            (low_alpha[:, 1] > low_alpha[:, 2])
            & (low_alpha[:, 1] > low_alpha[:, 0] + 12)
        )
        if np.any(green_fringe):
            errors.append(f"{label}: green edge fringe detected")

    placement_x, placement_y = rig.source_placement
    for layer in rig.tentacles:
        root_x = round(layer.root[0] + placement_x)
        root_y = round(layer.root[1] + placement_y)
        neighbourhood = alpha[
            root_y - 3 : root_y + 4,
            root_x - 3 : root_x + 4,
        ]
        if neighbourhood.size == 0 or int(neighbourhood.max()) == 0:
            errors.append(f"{label}: {layer.name} root is detached")
    return errors


def validate_motion(
    motion: str,
    filename: str,
    rig: Rig,
) -> list[str]:
    path = CONTENT_DIR / filename
    errors: list[str] = []
    if not path.exists():
        return [f"{motion}: missing {path}"]

    atlas = Image.open(path).convert("RGBA")
    expected_size = (
        rig.frame_width * rig.atlas_columns,
        rig.frame_height * rig.atlas_rows,
    )
    if atlas.size != expected_size:
        return [
            f"{motion}: expected atlas {expected_size}, found {atlas.size}"
        ]

    frames = split_atlas(atlas, rig)
    for index, frame in enumerate(frames):
        errors.extend(
            validate_frame(frame, rig, f"{motion} frame {index + 1}")
        )

    internal = [
        alpha_delta(frames[index], frames[index + 1])
        for index in range(len(frames) - 1)
    ]
    boundary = alpha_delta(frames[-1], frames[0])
    if motion != "loop_back" and boundary > max(internal) * 1.2:
        errors.append(
            f"{motion}: loop boundary {boundary:.5f} exceeds "
            f"internal maximum {max(internal):.5f}"
        )

    if not errors:
        print(f"{motion}: {len(frames)} frames valid")
    return errors


def main() -> int:
    rig = load_rig(RIG_PATH)
    errors: list[str] = []
    for motion, filename in ATLASES.items():
        errors.extend(validate_motion(motion, filename, rig))

    if errors:
        for error in errors:
            print(f"ERROR: {error}")
        return 1

    print("All Kraken atlases passed")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
