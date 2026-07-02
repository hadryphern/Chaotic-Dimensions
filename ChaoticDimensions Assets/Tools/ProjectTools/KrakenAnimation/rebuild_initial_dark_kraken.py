from pathlib import Path
import cv2
import numpy as np
from PIL import Image

ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / "assets_work/kraken_new_source/SPRITESHEET.png"
OUT = ROOT / "assets_work/kraken_initial_dark_rework"
NPC = ROOT / "Content/NPCs/Kraken"
COLS = ROWS = 6
SW, SH = 270, 350
TW, TH = 540, 700

def premultiplied_resize(frame):
    a = frame[:, :, 3].astype(np.float32) / 255.0
    rgb = frame[:, :, :3].astype(np.float32) * a[:, :, None]
    aa = cv2.resize(a, (TW, TH), interpolation=cv2.INTER_LANCZOS4)
    rr = cv2.resize(rgb, (TW, TH), interpolation=cv2.INTER_LANCZOS4)
    out = np.zeros((TH, TW, 4), dtype=np.uint8)
    out[:, :, 3] = np.clip(aa * 255.0, 0, 255).astype(np.uint8)
    out[:, :, :3] = np.clip(rr / np.maximum(aa[:, :, None], 1.0 / 255.0), 0, 255).astype(np.uint8)
    out[out[:, :, 3] == 0, :3] = 0
    return out

def clean_and_dark(frame):
    out = frame.copy()
    alpha = out[:, :, 3]
    rgb = out[:, :, :3].astype(np.float32)
    red_eye = (rgb[:, :, 0] > rgb[:, :, 2] * 1.15) & (rgb[:, :, 0] > 75) & (alpha > 20)
    hsv = cv2.cvtColor(out[:, :, :3], cv2.COLOR_RGB2HSV).astype(np.float32)
    hsv[:, :, 1] = np.clip(hsv[:, :, 1] * 1.10, 0, 255)
    hsv[:, :, 2] = np.clip(hsv[:, :, 2] * 0.72 + 5, 0, 255)
    dark = cv2.cvtColor(hsv.astype(np.uint8), cv2.COLOR_HSV2RGB).astype(np.float32)
    dark[red_eye] = rgb[red_eye] * np.array([0.95, 0.78, 0.78], dtype=np.float32)
    blurred = cv2.GaussianBlur(dark, (0, 0), 1.0)
    out[:, :, :3] = np.clip(dark * 1.22 - blurred * 0.22, 0, 255).astype(np.uint8)
    out[alpha == 0, :3] = 0
    return out

def wave_frame(frame, index, mode):
    h, w = frame.shape[:2]
    yy, xx = np.indices((h, w), dtype=np.float32)
    phase = 2.0 * np.pi * index / 36.0
    cx = w * 0.5
    lower = np.clip((yy - 112.0) / 238.0, 0.0, 1.0)
    upper_side = np.clip((np.abs(xx - cx) - 28.0) / 105.0, 0.0, 1.0)
    upper_height = np.clip((155.0 - yy) / 95.0, 0.0, 1.0)
    upper = upper_side * upper_height
    amp = 7.0 if mode == "idle" else 11.0 if mode == "horizontal" else 8.5
    dx = lower * amp * np.sin(phase * 1.35 + yy * 0.062 + (xx - cx) * 0.010)
    dy = upper * 5.5 * np.sin(phase * 1.65 + np.abs(xx - cx) * 0.055)
    dy += lower * 2.8 * np.cos(phase * 1.20 + yy * 0.047)
    if mode == "horizontal":
        dx += (3.5 + lower * 7.0) * np.sin(phase + yy * 0.026)
        dy += upper * 4.0 * np.cos(phase * 1.4 + np.abs(xx - cx) * 0.04)
    elif mode == "up":
        dy -= lower * (3.0 + 6.0 * (0.5 + 0.5 * np.sin(phase + yy * 0.038)))
        dx += lower * 5.5 * np.sin(phase * 1.5 + yy * 0.055)
    warped = cv2.remap(frame, xx - dx, yy - dy, cv2.INTER_CUBIC, borderMode=cv2.BORDER_CONSTANT, borderValue=(0, 0, 0, 0))
    warped[warped[:, :, 3] == 0, :3] = 0
    return warped

def atlas(frames):
    result = Image.new("RGBA", (TW * COLS, TH * ROWS))
    for i, frame in enumerate(frames):
        result.alpha_composite(Image.fromarray(frame, "RGBA"), ((i % COLS) * TW, (i // COLS) * TH))
    return result

def main():
    OUT.mkdir(parents=True, exist_ok=True)
    source = Image.open(SOURCE).convert("RGBA")
    if source.size != (SW * COLS, SH * ROWS):
        raise ValueError(f"Unexpected source size: {source.size}")
    raw = []
    for i in range(36):
        x, y = (i % COLS) * SW, (i // COLS) * SH
        raw.append(clean_and_dark(np.array(source.crop((x, y, x + SW, y + SH)), dtype=np.uint8)))
    idle = [premultiplied_resize(wave_frame(frame, i, "idle")) for i, frame in enumerate(raw)]
    horizontal = [premultiplied_resize(wave_frame(frame, i, "horizontal")) for i, frame in enumerate(raw)]
    upward = [premultiplied_resize(wave_frame(frame, i, "up")) for i, frame in enumerate(raw)]
    products = {"KrakenBoss.png": idle, "KrakenBossLoopBack.png": list(reversed(idle)), "KrakenBossMoveHorizontal.png": horizontal, "KrakenBossMoveUp.png": upward}
    for name, frames in products.items():
        image = atlas(frames)
        image.save(OUT / name, compress_level=4)
        image.save(NPC / name, compress_level=4)
    sequence = idle + list(reversed(idle))
    previews = []
    for frame in sequence:
        im = Image.fromarray(frame, "RGBA")
        im.thumbnail((378, 490), Image.Resampling.LANCZOS)
        previews.append(im)
    previews[0].save(OUT / "Kraken_initial_dark_preview.gif", save_all=True, append_images=previews[1:], duration=48, loop=0, disposal=2)
    Image.fromarray(idle[0], "RGBA").save(OUT / "Kraken_initial_dark_frame.png")
    print(f"Generated 4 atlases at {TW}x{TH} per frame from {SOURCE}")

if __name__ == "__main__":
    main()
