from __future__ import annotations

import json
import math
from dataclasses import dataclass
from pathlib import Path
from typing import Any, Mapping, Sequence

import cv2
import numpy as np
from PIL import Image


SUPPORTED_ROLES = {
    "long_tentacle",
    "central_tentacle",
    "body",
    "ruby",
}


@dataclass(frozen=True)
class LayerSpec:
    index: int
    name: str
    role: str
    root: tuple[float, float] | None
    phase_offset: float
    direction: float
    z: int


@dataclass(frozen=True)
class Rig:
    source_width: int
    source_height: int
    frame_width: int
    frame_height: int
    atlas_columns: int
    atlas_rows: int
    frame_count: int
    working_scale: int
    outline_color: tuple[int, int, int, int]
    outline_radius: float
    source_placement: tuple[float, float]
    body_anchor: tuple[float, float]
    ruby_anchor: tuple[float, float]
    layers: tuple[LayerSpec, ...]
    motion_profiles: Mapping[str, Mapping[str, float]]

    @property
    def tentacles(self) -> tuple[LayerSpec, ...]:
        return tuple(
            layer
            for layer in self.layers
            if layer.role in {"long_tentacle", "central_tentacle"}
        )


def _pair(values: Sequence[Any], label: str) -> tuple[float, float]:
    if len(values) != 2:
        raise ValueError(f"{label} must contain exactly two values")
    return float(values[0]), float(values[1])


def load_rig(path: Path | str) -> Rig:
    path = Path(path)
    data = json.loads(path.read_text(encoding="utf-8"))

    source_width, source_height = map(int, data["source_size"])
    frame_width, frame_height = map(int, data["frame_size"])
    atlas_columns, atlas_rows = map(int, data["atlas_grid"])
    layers: list[LayerSpec] = []
    seen_indices: set[int] = set()

    for raw_layer in data["layers"]:
        index = int(raw_layer["index"])
        if index in seen_indices:
            raise ValueError(f"Duplicate layer index: {index}")
        seen_indices.add(index)

        role = str(raw_layer["role"])
        if role not in SUPPORTED_ROLES:
            raise ValueError(f"Unsupported role for layer {index}: {role}")

        raw_root = raw_layer.get("root")
        root = None if raw_root is None else _pair(raw_root, f"layer {index} root")
        if role in {"long_tentacle", "central_tentacle"}:
            if root is None:
                raise ValueError(f"Tentacle layer {index} requires a root")
            if not (0 <= root[0] < source_width and 0 <= root[1] < source_height):
                raise ValueError(f"Tentacle layer {index} root is outside source canvas")

        layers.append(
            LayerSpec(
                index=index,
                name=str(raw_layer["name"]),
                role=role,
                root=root,
                phase_offset=float(raw_layer.get("phase_offset", 0.0)),
                direction=float(raw_layer.get("direction", 1.0)),
                z=int(raw_layer["z"]),
            )
        )

    if len(layers) != 14:
        raise ValueError(f"Expected 14 layers, found {len(layers)}")

    motion_profiles = {
        str(name): {str(key): float(value) for key, value in profile.items()}
        for name, profile in data["motion_profiles"].items()
    }
    expected_profiles = {"idle", "loop_back", "horizontal", "up"}
    if set(motion_profiles) != expected_profiles:
        raise ValueError(
            f"Motion profiles must be {sorted(expected_profiles)}, "
            f"found {sorted(motion_profiles)}"
        )

    return Rig(
        source_width=source_width,
        source_height=source_height,
        frame_width=frame_width,
        frame_height=frame_height,
        atlas_columns=atlas_columns,
        atlas_rows=atlas_rows,
        frame_count=int(data["frame_count"]),
        working_scale=int(data["working_scale"]),
        outline_color=tuple(int(value) for value in data["outline"]["color"]),
        outline_radius=float(data["outline"]["radius"]),
        source_placement=_pair(data["source_placement"], "source placement"),
        body_anchor=_pair(data["body_anchor"], "body anchor"),
        ruby_anchor=_pair(data["ruby_anchor"], "ruby anchor"),
        layers=tuple(sorted(layers, key=lambda layer: layer.z)),
        motion_profiles=motion_profiles,
    )


def distance(
    first: tuple[float, float],
    second: tuple[float, float],
) -> float:
    return math.hypot(first[0] - second[0], first[1] - second[1])


def deform_centerline(
    root: tuple[float, float],
    length: float,
    phase: float,
    profile: Mapping[str, float],
    sample_count: int = 64,
) -> list[tuple[float, float]]:
    if sample_count < 2:
        raise ValueError("sample_count must be at least 2")
    if length <= 0:
        raise ValueError("length must be positive")

    amplitude = float(profile["amplitude"])
    frequency = float(profile["frequency"])
    tip_lag = float(profile["tip_lag"])
    vertical_amplitude = float(profile["vertical_amplitude"])
    points: list[tuple[float, float]] = []

    for index in range(sample_count):
        progress = index / (sample_count - 1)
        envelope = progress**1.45
        delayed_phase = phase - tip_lag * progress
        lateral = (
            amplitude
            * envelope
            * math.sin(delayed_phase + frequency * math.pi * progress)
        )
        vertical = (
            vertical_amplitude
            * envelope
            * math.sin(delayed_phase + math.pi * progress)
        )
        points.append(
            (
                root[0] + lateral,
                root[1] + length * progress + vertical,
            )
        )

    points[0] = root
    return points


def _load_layer(
    source_dir: Path,
    layer: LayerSpec,
    outline_color: tuple[int, int, int, int],
) -> Image.Image:
    path = source_dir / f"pixil-layer-{layer.index}.png"
    image = Image.open(path).convert("RGBA")
    pixels = np.asarray(image).copy()
    alpha = pixels[:, :, 3]
    dark = (
        (alpha > 0)
        & (pixels[:, :, 0] < 38)
        & (pixels[:, :, 1] < 38)
        & (pixels[:, :, 2] < 45)
    )
    pixels[dark, :3] = np.asarray(outline_color[:3], dtype=np.uint8)
    pixels[alpha == 0, :3] = 0
    return Image.fromarray(pixels, "RGBA")


def _remap_premultiplied(
    image: Image.Image,
    map_x: np.ndarray,
    map_y: np.ndarray,
) -> Image.Image:
    source = np.asarray(image, dtype=np.float32)
    source_alpha = source[:, :, 3:4] / 255.0
    premultiplied = source.copy()
    premultiplied[:, :, :3] *= source_alpha

    remapped = cv2.remap(
        premultiplied,
        map_x,
        map_y,
        interpolation=cv2.INTER_CUBIC,
        borderMode=cv2.BORDER_CONSTANT,
        borderValue=(0, 0, 0, 0),
    )
    alpha = np.clip(remapped[:, :, 3:4], 0.0, 255.0)
    rgb = np.zeros_like(remapped[:, :, :3])
    np.divide(
        remapped[:, :, :3] * 255.0,
        alpha,
        out=rgb,
        where=alpha > 0.5,
    )
    result = np.concatenate((np.clip(rgb, 0, 255), alpha), axis=2)
    result[alpha[:, :, 0] <= 0.5, :3] = 0
    return Image.fromarray(result.astype(np.uint8), "RGBA")


def _deform_layer(
    image: Image.Image,
    layer: LayerSpec,
    profile: Mapping[str, float],
    phase: float,
) -> Image.Image:
    if layer.root is None:
        return image.copy()

    width, height = image.size
    _, root_y = layer.root
    alpha_box = image.getchannel("A").getbbox()
    if alpha_box is None:
        return image.copy()

    tail_length = max(1.0, alpha_box[3] - root_y)
    grid_y, grid_x = np.indices((height, width), dtype=np.float32)
    progress = np.clip((grid_y - root_y) / tail_length, 0.0, 1.0)
    envelope = progress**1.45
    layer_phase = phase + layer.phase_offset
    delayed_phase = layer_phase - profile["tip_lag"] * progress
    amplitude_key = (
        "long_amplitude"
        if layer.role == "long_tentacle"
        else "central_amplitude"
    )
    amplitude = profile[amplitude_key] * layer.direction
    lateral = (
        amplitude
        * envelope
        * np.sin(
            delayed_phase
            + profile["frequency"] * math.pi * progress
        )
    )
    lateral += profile["drag_x"] * envelope
    vertical = (
        profile["vertical_amplitude"]
        * envelope
        * np.sin(delayed_phase + math.pi * progress)
    )
    vertical += profile["drag_y"] * envelope

    map_x = (grid_x - lateral).astype(np.float32)
    map_y = (grid_y - vertical).astype(np.float32)
    return _remap_premultiplied(image, map_x, map_y)


def _apply_unified_outline(
    image: Image.Image,
    color: tuple[int, int, int, int],
    radius: float,
    working_scale: int,
) -> Image.Image:
    if working_scale < 1:
        raise ValueError("working_scale must be positive")

    work_size = (
        image.width * working_scale,
        image.height * working_scale,
    )
    work = image.resize(work_size, Image.Resampling.LANCZOS)
    alpha = np.asarray(work.getchannel("A"))
    radius_pixels = max(1, round(radius * working_scale))
    kernel_size = radius_pixels * 2 + 1
    kernel = cv2.getStructuringElement(
        cv2.MORPH_ELLIPSE,
        (kernel_size, kernel_size),
    )
    expanded = cv2.dilate(alpha, kernel)
    outline_alpha = np.maximum(
        expanded.astype(np.int16) - alpha.astype(np.int16),
        0,
    ).astype(np.uint8)

    outlined = Image.new("RGBA", work_size, color)
    outlined.putalpha(Image.fromarray(outline_alpha, "L"))
    outlined.alpha_composite(work)
    return outlined


def _clean_transparent_rgb(
    image: Image.Image,
    edge_color: tuple[int, int, int] | None = None,
) -> Image.Image:
    pixels = np.asarray(image).copy()
    if edge_color is not None:
        low_alpha = (pixels[:, :, 3] > 0) & (pixels[:, :, 3] < 24)
        pixels[low_alpha, :3] = np.asarray(edge_color, dtype=np.uint8)
    pixels[pixels[:, :, 3] == 0, :3] = 0
    return Image.fromarray(pixels, "RGBA")


def _phase_for_frame(
    motion: str,
    frame_index: int,
    frame_count: int,
) -> float:
    if motion == "loop_back":
        progress = frame_index / max(1, frame_count - 1)
        return math.tau * (1.0 - progress)
    return math.tau * frame_index / frame_count


def generate_frames(
    motion: str,
    rig: Rig,
    source_dir: Path | str,
) -> list[Image.Image]:
    if motion not in rig.motion_profiles:
        raise ValueError(f"Unknown motion profile: {motion}")

    source_dir = Path(source_dir)
    loaded = {
        layer.index: _load_layer(source_dir, layer, rig.outline_color)
        for layer in rig.layers
        if layer.role != "ruby"
    }
    profile = rig.motion_profiles[motion]
    frames: list[Image.Image] = []

    for frame_index in range(rig.frame_count):
        phase = _phase_for_frame(motion, frame_index, rig.frame_count)
        composed = Image.new(
            "RGBA",
            (rig.source_width, rig.source_height),
            (0, 0, 0, 0),
        )

        for layer in rig.layers:
            if layer.role == "ruby":
                continue
            source = loaded[layer.index]
            rendered = (
                _deform_layer(source, layer, profile, phase)
                if layer.role in {"long_tentacle", "central_tentacle"}
                else source
            )
            composed.alpha_composite(rendered)

        outlined = _apply_unified_outline(
            composed,
            rig.outline_color,
            rig.outline_radius,
            rig.working_scale,
        )
        work_frame = Image.new(
            "RGBA",
            (
                rig.frame_width * rig.working_scale,
                rig.frame_height * rig.working_scale,
            ),
            (0, 0, 0, 0),
        )
        placement = (
            round(rig.source_placement[0] * rig.working_scale),
            round(rig.source_placement[1] * rig.working_scale),
        )
        work_frame.alpha_composite(outlined, placement)
        frame = work_frame.resize(
            (rig.frame_width, rig.frame_height),
            Image.Resampling.LANCZOS,
        )
        frames.append(
            _clean_transparent_rgb(frame, rig.outline_color[:3])
        )

    return frames


def generate_atlas(
    frames: Sequence[Image.Image],
    rig: Rig,
) -> Image.Image:
    if len(frames) != rig.frame_count:
        raise ValueError(
            f"Expected {rig.frame_count} frames, found {len(frames)}"
        )

    atlas = Image.new(
        "RGBA",
        (
            rig.frame_width * rig.atlas_columns,
            rig.frame_height * rig.atlas_rows,
        ),
        (0, 0, 0, 0),
    )
    for index, frame in enumerate(frames):
        if frame.size != (rig.frame_width, rig.frame_height):
            raise ValueError(
                f"Frame {index} has size {frame.size}, expected "
                f"{(rig.frame_width, rig.frame_height)}"
            )
        column = index % rig.atlas_columns
        row = index // rig.atlas_columns
        atlas.alpha_composite(
            frame,
            (
                column * rig.frame_width,
                row * rig.frame_height,
            ),
        )
    return _clean_transparent_rgb(atlas, rig.outline_color[:3])
