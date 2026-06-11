from __future__ import annotations

import math
import unittest
from pathlib import Path

from tools.KrakenAnimation.kraken_animation import (
    deform_centerline,
    distance,
    load_rig,
)


PROJECT_ROOT = Path(__file__).resolve().parents[3]
RIG_PATH = PROJECT_ROOT / "tools" / "KrakenAnimation" / "kraken_rig.json"


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


if __name__ == "__main__":
    unittest.main()
