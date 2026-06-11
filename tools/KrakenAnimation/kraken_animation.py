from __future__ import annotations

import json
import math
from dataclasses import dataclass
from pathlib import Path
from typing import Any, Mapping, Sequence


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
