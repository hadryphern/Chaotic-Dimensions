from __future__ import annotations

from datetime import datetime
from pathlib import Path
import shutil

import cv2
import numpy as np
from PIL import Image
from scipy.ndimage import distance_transform_edt


ATLAS_NAMES = (
    "KrakenBoss.png",
    "KrakenBossLoopBack.png",
    "KrakenBossMoveHorizontal.png",
    "KrakenBossMoveUp.png",
)
SOURCE_FRAME = (810, 1050)
TARGET_FRAME = (932, 1208)
GRID = (6, 6)


def remove_eye_residue(frame: np.ndarray) -> np.ndarray:
    """Remove the two old eye sockets while reconstructing the blue mantle texture."""
    rgb = frame[:, :, :3].copy()
    alpha = frame[:, :, 3]
    mask = np.zeros(alpha.shape, dtype=np.uint8)

    # The head is stable across all four motion atlases.
    cv2.ellipse(mask, (350, 334), (27, 39), 0, 0, 360, 255, -1)
    cv2.ellipse(mask, (460, 334), (27, 39), 0, 0, 360, 255, -1)
    mask[alpha < 24] = 0

    rgb = cv2.inpaint(cv2.cvtColor(rgb, cv2.COLOR_RGB2BGR), mask, 8, cv2.INPAINT_TELEA)
    rgb = cv2.cvtColor(rgb, cv2.COLOR_BGR2RGB)

    # Ruby is drawn separately in code, so red pixels embedded in the body are unwanted.
    red = (
        (rgb[:, :, 0] > 55)
        & (rgb[:, :, 0] > rgb[:, :, 1] * 1.30)
        & (rgb[:, :, 0] > rgb[:, :, 2] * 1.12)
        & (alpha > 12)
    )
    if np.any(red):
        blue = rgb[:, :, 2].astype(np.float32)
        rgb[red, 0] = np.clip(blue[red] * 0.32, 0, 255).astype(np.uint8)
        rgb[red, 1] = np.clip(blue[red] * 0.38, 0, 255).astype(np.uint8)

    result = frame.copy()
    result[:, :, :3] = rgb
    return result


def clean_edge_fringe(frame: np.ndarray) -> np.ndarray:
    """Replace saturated edge pixels with nearby interior texture without changing shape."""
    result = frame.copy()
    alpha = result[:, :, 3]
    solid = alpha > 12
    interior = cv2.erode(solid.astype(np.uint8), np.ones((9, 9), np.uint8), iterations=1) > 0
    edge = solid & ~interior

    if np.any(interior):
        _, nearest = distance_transform_edt(~interior, return_indices=True)
        nearest_rgb = result[nearest[0], nearest[1], :3]
        current = result[:, :, :3].astype(np.float32)
        current[edge] = current[edge] * 0.12 + nearest_rgb[edge].astype(np.float32) * 0.88
        result[:, :, :3] = np.clip(current, 0, 255).astype(np.uint8)

    result[alpha == 0, :3] = 0
    return result


def resize_rgba_premultiplied(frame: np.ndarray) -> np.ndarray:
    """Resize RGBA data without pulling transparent RGB values into the silhouette."""
    alpha = frame[:, :, 3].astype(np.float32) / 255.0
    premultiplied = frame[:, :, :3].astype(np.float32) * alpha[:, :, None]

    size = TARGET_FRAME
    resized_alpha = cv2.resize(alpha, size, interpolation=cv2.INTER_LANCZOS4)
    resized_rgb = cv2.resize(premultiplied, size, interpolation=cv2.INTER_LANCZOS4)

    resized_alpha = np.clip(resized_alpha, 0.0, 1.0)
    denominator = np.maximum(resized_alpha[:, :, None], 1.0 / 255.0)
    straight_rgb = np.clip(resized_rgb / denominator, 0, 255)

    output = np.zeros((TARGET_FRAME[1], TARGET_FRAME[0], 4), dtype=np.uint8)
    output[:, :, :3] = straight_rgb.astype(np.uint8)
    output[:, :, 3] = np.round(resized_alpha * 255).astype(np.uint8)
    output[output[:, :, 3] == 0, :3] = 0
    return output


def process_atlas(source: Path, destination: Path) -> None:
    image = Image.open(source).convert("RGBA")
    expected = (SOURCE_FRAME[0] * GRID[0], SOURCE_FRAME[1] * GRID[1])
    if image.size != expected:
        raise ValueError(f"{source} has {image.size}; expected {expected}")

    target = Image.new("RGBA", (TARGET_FRAME[0] * GRID[0], TARGET_FRAME[1] * GRID[1]))
    for index in range(GRID[0] * GRID[1]):
        column = index % GRID[0]
        row = index // GRID[0]
        box = (
            column * SOURCE_FRAME[0],
            row * SOURCE_FRAME[1],
            (column + 1) * SOURCE_FRAME[0],
            (row + 1) * SOURCE_FRAME[1],
        )
        frame = np.asarray(image.crop(box), dtype=np.uint8)
        frame = remove_eye_residue(frame)
        frame = clean_edge_fringe(frame)
        frame = resize_rgba_premultiplied(frame)
        target.alpha_composite(Image.fromarray(frame, "RGBA"), (column * TARGET_FRAME[0], row * TARGET_FRAME[1]))

    target.save(destination, optimize=True)


def main() -> None:
    root = Path(__file__).resolve().parents[2]
    npc_dir = root / "Content" / "NPCs" / "Kraken"
    output_dir = root / "assets_work" / "kraken_clean_hd"
    backup_dir = output_dir / "backups" / datetime.now().strftime("%Y%m%d-%H%M%S")
    output_dir.mkdir(parents=True, exist_ok=True)
    backup_dir.mkdir(parents=True, exist_ok=True)

    for name in ATLAS_NAMES:
        source = npc_dir / name
        shutil.copy2(source, backup_dir / name)
        processed = output_dir / name
        process_atlas(source, processed)
        shutil.copy2(processed, source)
        print(f"{name}: {SOURCE_FRAME[0]}x{SOURCE_FRAME[1]} -> {TARGET_FRAME[0]}x{TARGET_FRAME[1]} per frame")

    first = Image.open(output_dir / "KrakenBoss.png").crop((0, 0, TARGET_FRAME[0], TARGET_FRAME[1]))
    first.thumbnail((466, 604), Image.Resampling.LANCZOS)
    first.save(output_dir / "Kraken_clean_HD_preview.png")
    print(f"Backups: {backup_dir}")


if __name__ == "__main__":
    main()
