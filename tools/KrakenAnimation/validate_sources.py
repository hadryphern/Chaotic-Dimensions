from __future__ import annotations

from pathlib import Path

from PIL import Image


PROJECT_ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = PROJECT_ROOT / "Assets" / "Source" / "Kraken"
EXPECTED_SIZE = (570, 990)
EXPECTED_COUNT = 14


def main() -> int:
    paths = sorted(
        SOURCE_DIR.glob("pixil-layer-*.png"),
        key=lambda path: int(path.stem.rsplit("-", 1)[1]),
    )
    if len(paths) != EXPECTED_COUNT:
        raise SystemExit(
            f"Expected {EXPECTED_COUNT} source layers, found {len(paths)}"
        )

    for expected_index, path in enumerate(paths):
        actual_index = int(path.stem.rsplit("-", 1)[1])
        if actual_index != expected_index:
            raise SystemExit(
                f"Expected layer {expected_index}, found {actual_index}"
            )

        with Image.open(path) as image:
            if image.size != EXPECTED_SIZE:
                raise SystemExit(
                    f"{path.name}: expected {EXPECTED_SIZE}, found {image.size}"
                )
            if image.mode != "RGBA":
                raise SystemExit(
                    f"{path.name}: expected RGBA, found {image.mode}"
                )
            if image.getchannel("A").getbbox() is None:
                raise SystemExit(f"{path.name}: alpha channel is empty")

    print(f"{EXPECTED_COUNT} source layers valid ({EXPECTED_SIZE[0]}x{EXPECTED_SIZE[1]} RGBA)")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
