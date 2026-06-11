from __future__ import annotations

import math
import unittest
from pathlib import Path

import numpy as np

from tools.KrakenAnimation.kraken_animation import (
    deform_centerline,
    distance,
    generate_atlas,
    generate_frames,
    load_rig,
)


PROJECT_ROOT = Path(__file__).resolve().parents[3]
RIG_PATH = PROJECT_ROOT / "tools" / "KrakenAnimation" / "kraken_rig.json"
SOURCE_DIR = PROJECT_ROOT / "Assets" / "Source" / "Kraken"


class RigTests(unittest.TestCase):
    def test_rig_has_fourteen_unique_layers(self) -> None:
        rig = load_rig(RIG_PATH)

        self.assertEqual(14, len(rig.layers))
        self.assertEqual(14, len({layer.index for layer in rig.layers}))

    def test_every_tentacle_has_fixed_root_inside_source_canvas(self) -> None:
        rig = load_rig(RIG_PATH)

        for layer in rig.tentacles:
            self.assertGreaterEqual(layer.root[0], 0)
            self.assertLess(layer.root[0], rig.source_width)
            self.assertGreaterEqual(layer.root[1], 0)
            self.assertLess(layer.root[1], rig.source_height)

    def test_motion_profiles_cover_all_production_atlases(self) -> None:
        rig = load_rig(RIG_PATH)

        self.assertEqual(
            {"idle", "loop_back", "horizontal", "up"},
            set(rig.motion_profiles),
        )


class GeometryTests(unittest.TestCase):
    def setUp(self) -> None:
        self.root = (250.0, 305.0)
        self.length = 620.0
        self.profile = {
            "amplitude": 42.0,
            "frequency": 1.75,
            "tip_lag": 0.35,
            "vertical_amplitude": 8.0,
        }

    def assert_points_close(
        self,
        first: list[tuple[float, float]],
        second: list[tuple[float, float]],
        tolerance: float,
    ) -> None:
        self.assertEqual(len(first), len(second))
        for first_point, second_point in zip(first, second):
            self.assertLessEqual(distance(first_point, second_point), tolerance)

    def test_root_is_unchanged_by_deformation(self) -> None:
        points = deform_centerline(
            self.root,
            self.length,
            phase=1.2,
            profile=self.profile,
        )

        self.assertAlmostEqual(self.root[0], points[0][0], places=5)
        self.assertAlmostEqual(self.root[1], points[0][1], places=5)

    def test_tip_moves_more_than_root(self) -> None:
        points = deform_centerline(
            self.root,
            self.length,
            phase=1.2,
            profile=self.profile,
        )

        neutral_tip = (self.root[0], self.root[1] + self.length)
        self.assertLess(
            distance(points[0], self.root),
            distance(points[-1], neutral_tip),
        )

    def test_loop_closes_exactly(self) -> None:
        first = deform_centerline(
            self.root,
            self.length,
            phase=0.0,
            profile=self.profile,
        )
        last = deform_centerline(
            self.root,
            self.length,
            phase=math.tau,
            profile=self.profile,
        )

        self.assert_points_close(first, last, tolerance=1e-6)


class AtlasTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls) -> None:
        cls.rig = load_rig(RIG_PATH)
        cls.frames = generate_frames("idle", cls.rig, SOURCE_DIR)

    def test_idle_has_thirty_six_normalized_frames(self) -> None:
        self.assertEqual(36, len(self.frames))
        self.assertEqual(
            {(self.rig.frame_width, self.rig.frame_height)},
            {frame.size for frame in self.frames},
        )

    def test_generated_atlas_is_six_by_six_frames(self) -> None:
        atlas = generate_atlas(self.frames, self.rig)

        self.assertEqual((3840, 6144), atlas.size)

    def test_all_tentacle_roots_remain_opaque(self) -> None:
        placement_x, placement_y = 5, 43
        for frame in self.frames:
            alpha = np.asarray(frame.getchannel("A"))
            for layer in self.rig.tentacles:
                root_x = round(layer.root[0] + placement_x)
                root_y = round(layer.root[1] + placement_y)
                neighbourhood = alpha[
                    root_y - 3 : root_y + 4,
                    root_x - 3 : root_x + 4,
                ]
                self.assertGreater(
                    int(neighbourhood.max()),
                    0,
                    f"{layer.name} detached at ({root_x}, {root_y})",
                )

    def test_transparent_pixels_have_zero_rgb(self) -> None:
        for frame in self.frames:
            pixels = np.asarray(frame)
            transparent_rgb = pixels[pixels[:, :, 3] == 0, :3]
            self.assertTrue(np.all(transparent_rgb == 0))

    def test_low_alpha_edges_do_not_contain_green_fringe(self) -> None:
        for frame in self.frames:
            pixels = np.asarray(frame)
            alpha = pixels[:, :, 3]
            edge = pixels[(alpha > 0) & (alpha < 24), :3].astype(np.int16)
            green_fringe = (
                (edge[:, 1] > edge[:, 2])
                & (edge[:, 1] > edge[:, 0] + 12)
            )
            self.assertFalse(np.any(green_fringe))

    def test_head_region_does_not_drift(self) -> None:
        reference = np.asarray(self.frames[0].getchannel("A"))[:360]
        for frame in self.frames[1:]:
            current = np.asarray(frame.getchannel("A"))[:360]
            changed = np.count_nonzero(reference != current) / reference.size
            self.assertLess(changed, 0.02)

    def test_loop_boundary_is_no_larger_than_internal_steps(self) -> None:
        alphas = [
            np.asarray(frame.getchannel("A"), dtype=np.int16)
            for frame in self.frames
        ]

        def delta(first: np.ndarray, second: np.ndarray) -> float:
            return float(np.abs(first - second).mean() / 255.0)

        internal = [
            delta(alphas[index], alphas[index + 1])
            for index in range(len(alphas) - 1)
        ]
        boundary = delta(alphas[-1], alphas[0])
        self.assertLessEqual(boundary, max(internal) * 1.15)


if __name__ == "__main__":
    unittest.main()
