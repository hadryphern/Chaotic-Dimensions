# -*- coding: utf-8 -*-
"""
MagnetMageBoss — Idle Spritesheet Generator
Gera: Content/NPCs/MagnetMage/MagnetMageBoss.png
Atlas: ATLAS_COLS × ATLAS_ROWS = N_FRAMES frames
"""

from PIL import Image, ImageDraw, ImageFilter
import math, os, numpy as np

# ── Configuração ──────────────────────────────────────────────────────────────
FW, FH       = 220, 300      # tamanho de cada frame (píxeis finais)
S            = 4             # supersampling factor (4× → downscale suavizado)
N_FRAMES     = 120           # frames totais (2 s a 60 fps)
ATLAS_COLS   = 10
ATLAS_ROWS   = 12            # 10×12 = 120

WW, HH = FW * S, FH * S     # canvas de trabalho

OUTPUT_DIR = os.path.join(os.path.dirname(__file__),
                          "Content", "NPCs", "MagnetMage")
os.makedirs(OUTPUT_DIR, exist_ok=True)

# ── Paleta ────────────────────────────────────────────────────────────────────
MAG_RED_HI   = (240,  72,  60, 255)
MAG_RED_MID  = (198,  42,  38, 255)
MAG_RED_SHD  = (120,  18,  18, 255)
MAG_RED_AO   = ( 80,   8,   8, 255)
MAG_GRAY_HI  = (218, 220, 224, 255)
MAG_GRAY_MID = (155, 158, 168, 255)
MAG_GRAY_SHD = ( 88,  90, 100, 255)
MAG_GRAY_AO  = ( 48,  50,  60, 255)
MAG_STRIPE   = (240, 240, 252, 255)   # reflexo branco no ímã

ROBE_HI      = ( 72,  64,  98, 255)
ROBE_MID     = ( 34,  28,  52, 255)
ROBE_SHD     = ( 14,  10,  24, 255)
ROBE_FOLD    = (  8,   6,  16, 255)
ROBE_RIM     = ( 95,  82, 135, 200)

COLLAR_HI    = ( 88,  76, 120, 255)
COLLAR_MID   = ( 52,  44,  82, 255)
COLLAR_SHD   = ( 24,  18,  40, 255)

GLOVE_HI     = ( 82,  76,  90, 255)
GLOVE_MID    = ( 46,  42,  54, 255)
GLOVE_SHD    = ( 22,  19,  28, 255)
GLOVE_RIM    = (100,  92, 115, 220)

GLOW_PURPLE  = ( 90,  45, 160,  0)    # alpha per layer below
GLOW_RED     = (180,  22,  45,  0)


# ── Helpers ───────────────────────────────────────────────────────────────────
def s(v):     return int(round(v * S))
def sf(v):    return v * S

def lerp(a, b, t): return a + (b - a) * t
def lerp_c(c1, c2, t):
    return tuple(int(lerp(c1[i], c2[i], t)) for i in range(4))

def fill_vgrad(img, x0, y0, x1, y1, c_top, c_bot):
    """Preenche rectângulo com gradiente vertical."""
    d = ImageDraw.Draw(img)
    for y in range(max(0, y0), min(img.height, y1)):
        t = (y - y0) / max(1, y1 - y0)
        d.line([(max(0, x0), y), (min(img.width-1, x1), y)],
               fill=lerp_c(c_top, c_bot, t))

def fill_hgrad(img, x0, y0, x1, y1, c_left, c_right):
    """Preenche rectângulo com gradiente horizontal."""
    d = ImageDraw.Draw(img)
    for x in range(max(0, x0), min(img.width, x1)):
        t = (x - x0) / max(1, x1 - x0)
        d.line([(x, max(0, y0)), (x, min(img.height-1, y1))],
               fill=lerp_c(c_left, c_right, t))

def alpha_ellipse(canvas, x0, y0, x1, y1, color, blur=0):
    """Elipse em layer separada, composta sobre canvas."""
    lay = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
    ImageDraw.Draw(lay).ellipse([x0, y0, x1, y1], fill=color)
    if blur > 0:
        lay = lay.filter(ImageFilter.GaussianBlur(blur))
    canvas.alpha_composite(lay)

def half_ellipse_bottom(canvas, cx, cy, rx, ry, color):
    """Metade inferior de uma elipse (arco inferior do ímã)."""
    lay = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
    d   = ImageDraw.Draw(lay)
    d.ellipse([cx - rx, cy - ry, cx + rx, cy + ry], fill=color)
    # apaga metade superior
    d.rectangle([0, 0, WW, cy], fill=(0, 0, 0, 0))
    canvas.alpha_composite(lay)


# ── Desenho do boss ───────────────────────────────────────────────────────────

def draw_frame(frame_idx):
    t         = frame_idx / N_FRAMES
    # Flutuação do corpo
    bob       = math.sin(2 * math.pi * t) * 7.0
    # Delay das mãos: ~14% do ciclo atrás
    t_hand    = (frame_idx - N_FRAMES * 0.14) / N_FRAMES
    hand_bob  = math.sin(2 * math.pi * t_hand) * 7.0
    # Balanço do manto (freq ligeiramente diferente → movimento orgânico)
    sway_l    = math.sin(2 * math.pi * t * 1.4 + 0.3) * 3.5
    sway_r    = math.sin(2 * math.pi * t * 1.4 - 0.2) * 3.0
    sway_bot  = math.sin(2 * math.pi * t * 0.9)       * 2.5
    # Dedos: abre e fecha (dois ciclos por ciclo de bob)
    f_open    = (math.sin(2 * math.pi * t * 2) + 1) / 2.0  # 0=fechado 1=aberto

    canvas = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))

    # Centro horizontal (em coords 1×)
    CX = FW / 2

    # Posição do corpo com bob
    body_cy = FH * 0.60 + bob
    hand_cy = FH * 0.50 + hand_bob

    # ── 1. MANTO (robe) ──────────────────────────────────────────────────────
    ry0  = body_cy - 72    # topo do manto
    ry1  = body_cy + 80    # base do manto
    rw_t = 46              # meia-largura no topo
    rw_b = 76              # meia-largura na base

    # Camadas do manto: base escura → meio → reflexo nas bordas
    # Calculamos o polígono do manto com balanço nas bordas
    def robe_pts(w_top, w_bot, y0, y1, sl, sr, extra_bot=0):
        steps = 20
        pts = []
        # lado esquerdo (topo → base)
        for i in range(steps + 1):
            tt = i / steps
            y  = y0 + (y1 - y0) * tt
            w  = w_top + (w_bot - w_top) * tt
            x  = CX - w + sl * tt + extra_bot * math.sin(math.pi * tt) * (-1)
            pts.append((sf(x), sf(y)))
        # lado direito (base → topo)
        for i in range(steps, -1, -1):
            tt = i / steps
            y  = y0 + (y1 - y0) * tt
            w  = w_top + (w_bot - w_top) * tt
            x  = CX + w + sr * tt + extra_bot * math.sin(math.pi * tt)
            pts.append((sf(x), sf(y)))
        return pts

    # Camada base (sombra)
    lay_robe = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
    d = ImageDraw.Draw(lay_robe)
    d.polygon(robe_pts(rw_t, rw_b, ry0, ry1, sway_l, sway_r, sway_bot),
              fill=ROBE_SHD)
    canvas.alpha_composite(lay_robe)

    # Gradiente vertical sobre o manto (highlight no topo, sombra em baixo)
    lay_grad = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
    fill_vgrad(lay_grad,
               s(CX - rw_b), s(ry0), s(CX + rw_b), s(ry1),
               ROBE_MID, ROBE_SHD)
    # Aplica apenas dentro do polígono do manto
    poly_mask = Image.new('L', (WW, HH), 0)
    ImageDraw.Draw(poly_mask).polygon(
        robe_pts(rw_t, rw_b, ry0, ry1, sway_l, sway_r, sway_bot),
        fill=255)
    lay_robe2 = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
    lay_robe2.paste(lay_grad, mask=poly_mask)
    canvas.alpha_composite(lay_robe2)

    # Destaque lateral esquerdo (rim light)
    lay_rim = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
    d_rim = ImageDraw.Draw(lay_rim)
    rim_pts_l = robe_pts(rw_t - 1, rw_b - 1, ry0, ry1, sway_l, sway_r, sway_bot)
    rim_pts_l2 = robe_pts(rw_t - 10, rw_b - 10, ry0 + 5, ry1 - 5, sway_l * 0.8, sway_r * 0.8, sway_bot * 0.8)
    # Apenas lado esquerdo estreito
    rim_l = rim_pts_l[:21] + list(reversed(rim_pts_l2[:21]))
    d_rim.polygon(rim_l, fill=ROBE_HI)
    canvas.alpha_composite(lay_rim)

    # Prega central (linha escura ao centro)
    fold_lay = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
    d_fold = ImageDraw.Draw(fold_lay)
    fold_x_top = sf(CX + sway_l * 0.1)
    fold_x_bot = sf(CX + sway_bot * 0.5)
    for fi in range(4):
        off = fi - 1.5
        d_fold.line([
            (fold_x_top + s(off), s(ry0 + 8)),
            (fold_x_bot + s(off * 1.4), s(ry1 - 5))
        ], fill=(*ROBE_FOLD[:3], 180 - abs(fi-1)*50), width=s(1))
    canvas.alpha_composite(fold_lay)

    d = ImageDraw.Draw(canvas)

    # ── 2. COLARINHO ─────────────────────────────────────────────────────────
    col_cx = sf(CX)
    col_cy = sf(ry0)
    # Elipse base
    alpha_ellipse(canvas,
                  int(col_cx - s(32)), int(col_cy - s(9)),
                  int(col_cx + s(32)), int(col_cy + s(11)),
                  COLLAR_SHD)
    alpha_ellipse(canvas,
                  int(col_cx - s(26)), int(col_cy - s(6)),
                  int(col_cx + s(26)), int(col_cy + s(8)),
                  COLLAR_MID)
    alpha_ellipse(canvas,
                  int(col_cx - s(18)), int(col_cy - s(3)),
                  int(col_cx + s(18)), int(col_cy + s(5)),
                  COLLAR_HI)

    # ── 3. CABEÇA (ÍMÃO) ─────────────────────────────────────────────────────
    hx  = CX
    hy  = ry0 - 44              # centro vertical da cabeça
    AW  = 19                    # largura de cada braço
    AH  = 52                    # altura dos braços
    GAP = 22                    # metade do espaço interno

    ay0 = hy - AH / 2          # topo dos braços
    ay1 = hy + AH / 2          # base dos braços

    # Arco inferior (conexão entre braços) — metade inferior da elipse
    arc_rx = GAP + AW / 2 + 2
    arc_ry = 24
    arc_cx = s(hx)
    arc_cy = s(ay1)

    half_ellipse_bottom(canvas, arc_cx, arc_cy,
                        s(arc_rx), s(arc_ry), MAG_RED_SHD)
    # Highlight no arco
    half_ellipse_bottom(canvas, arc_cx, arc_cy,
                        s(arc_rx - 6), s(arc_ry - 6), MAG_RED_MID)

    # Braço esquerdo
    lax0 = hx - GAP - AW
    lax1 = hx - GAP
    lay_arm = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
    # Gradiente horizontal: sombra esquerda → highlight direita (face interna)
    fill_hgrad(lay_arm,
               s(lax0), s(ay0), s(lax1), s(ay1),
               MAG_RED_AO, MAG_RED_HI)
    canvas.alpha_composite(lay_arm)

    # Braço direito
    rax0 = hx + GAP
    rax1 = hx + GAP + AW
    lay_arm2 = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
    fill_hgrad(lay_arm2,
               s(rax0), s(ay0), s(rax1), s(ay1),
               MAG_RED_HI, MAG_RED_AO)
    canvas.alpha_composite(lay_arm2)

    # Reflexo metálico fino nos braços
    d = ImageDraw.Draw(canvas)
    d.line([(s(lax0 + 3), s(ay0 + 6)), (s(lax0 + 3), s(ay1 - 6))],
           fill=(*MAG_RED_MID[:3], 160), width=s(1))
    d.line([(s(rax1 - 3), s(ay0 + 6)), (s(rax1 - 3), s(ay1 - 6))],
           fill=(*MAG_RED_MID[:3], 160), width=s(1))

    # Pontas cinzentas (poles do ímão)
    for tip_x0, tip_x1 in [(lax0 - 3, lax1 + 3), (rax0 - 3, rax1 + 3)]:
        tip_y0 = ay0 - 20
        tip_y1 = ay0 + 6

        lay_tip = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
        # Fundo cinzento
        d_tip = ImageDraw.Draw(lay_tip)
        d_tip.rectangle([s(tip_x0), s(tip_y0 + 8), s(tip_x1), s(tip_y1)],
                         fill=MAG_GRAY_MID)
        d_tip.ellipse([s(tip_x0), s(tip_y0), s(tip_x1), s(tip_y0 + 16)],
                       fill=MAG_GRAY_MID)
        # Highlight topo
        tip_w = tip_x1 - tip_x0
        d_tip.ellipse([s(tip_x0 + 3), s(tip_y0 + 2),
                       s(tip_x1 - 3), s(tip_y0 + 9)],
                      fill=MAG_GRAY_HI)
        # Sombra lado esquerdo
        fill_hgrad(lay_tip,
                   s(tip_x0), s(tip_y0 + 4), s(tip_x0 + 5), s(tip_y1),
                   MAG_GRAY_SHD, (*MAG_GRAY_MID[:3], 0))
        # Reflexo branco (stripe)
        stripe_x = s(tip_x0 + tip_w * 0.35)
        d_tip.line([(stripe_x, s(tip_y0 + 3)), (stripe_x, s(tip_y1 - 2))],
                   fill=(*MAG_STRIPE[:3], 100), width=max(1, s(1)))

        canvas.alpha_composite(lay_tip)

    # ── 4. MÃOS ──────────────────────────────────────────────────────────────
    def draw_hand(hcx, hcy, mirror):
        """Desenha uma mão flutuante com dedos abertos/fechados."""
        pw = 32   # largura da palma
        ph = 22   # altura da palma
        px0, px1 = hcx - pw / 2, hcx + pw / 2
        py0, py1 = hcy - ph / 2, hcy + ph / 2

        lay_h = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
        dh    = ImageDraw.Draw(lay_h)

        # ── Palma ──
        # Elipse base
        dh.ellipse([s(px0), s(py0), s(px1), s(py1)], fill=GLOVE_SHD)
        # Gradiente sobre palma
        fill_vgrad(lay_h, s(px0 + 4), s(py0 + 3), s(px1 - 4), s(py1 - 3),
                   GLOVE_MID, GLOVE_SHD)
        # Highlight superior
        dh.ellipse([s(px0 + 5), s(py0 + 3), s(px1 - 5), s(py0 + 11)],
                    fill=GLOVE_HI)
        # Rim light (borda inferior)
        dh.arc([s(px0 + 2), s(py1 - 8), s(px1 - 2), s(py1 + 2)],
                180, 0, fill=(*GLOVE_RIM[:3], 120), width=s(1))

        # ── Dedos ──
        n_fingers = 4
        fsp  = pw / n_fingers                    # espaçamento
        # Altura do dedo: vai de 7 (fechado) a 16 (aberto)
        fh   = lerp(7, 18, f_open)
        fw   = 7.5

        for fi in range(n_fingers):
            fx = px0 + fi * fsp + fsp / 2
            # Dedo fecha curvando: topo do dedo baixa ao fechar
            fy0_d = py0 - fh
            fy1_d = py0 + 4

            # Corpo do dedo
            dh.rectangle([s(fx - fw/2 + 1), s(fy0_d + fw/2),
                           s(fx + fw/2 - 1), s(fy1_d)],
                          fill=GLOVE_MID)
            # Topo arredondado
            dh.ellipse([s(fx - fw/2), s(fy0_d),
                        s(fx + fw/2), s(fy0_d + fw)],
                        fill=GLOVE_MID)
            # Highlight topo
            dh.ellipse([s(fx - 2), s(fy0_d + 2),
                        s(fx + 2), s(fy0_d + 5)],
                        fill=GLOVE_HI)
            # Junta (knuckle) na base
            dh.ellipse([s(fx - fw/2 + 1), s(fy1_d - 5),
                        s(fx + fw/2 - 1), s(fy1_d + 1)],
                        fill=GLOVE_SHD)

        # ── Polegar ──
        thumb_sign = -1 if mirror else 1
        thumb_cx   = (px0 - 6) if mirror else (px1 + 6)
        thumb_open = f_open * 9
        thx = thumb_cx + thumb_sign * thumb_open
        thy = hcy - 2
        dh.ellipse([s(thx - 6), s(thy - 7),
                    s(thx + 6), s(thy + 7)],
                    fill=GLOVE_MID)
        dh.ellipse([s(thx - 3), s(thy - 4),
                    s(thx + 3), s(thy + 1)],
                    fill=GLOVE_HI)

        canvas.alpha_composite(lay_h)

    lhx = CX - 78
    rhx = CX + 78
    draw_hand(lhx, hand_cy + 12, mirror=False)
    draw_hand(rhx, hand_cy + 12, mirror=True)

    # ── 5. CONTORNO DO MANTO (rim escuro para separar do fundo) ───────────────
    outline_lay = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
    outline_mask = Image.new('L', (WW, HH), 0)
    ImageDraw.Draw(outline_mask).polygon(
        robe_pts(rw_t + 2, rw_b + 2, ry0 - 1, ry1 + 1, sway_l, sway_r, sway_bot),
        fill=255)
    inner_mask = Image.new('L', (WW, HH), 0)
    ImageDraw.Draw(inner_mask).polygon(
        robe_pts(rw_t, rw_b, ry0, ry1, sway_l, sway_r, sway_bot),
        fill=255)
    # Borda = outer - inner
    outer_arr = np.array(outline_mask)
    inner_arr = np.array(inner_mask)
    border_arr = np.clip(outer_arr.astype(int) - inner_arr.astype(int), 0, 255).astype(np.uint8)
    border_mask = Image.fromarray(border_arr, 'L')
    border_color = Image.new('RGBA', (WW, HH), (5, 3, 12, 200))
    outline_lay.paste(border_color, mask=border_mask)
    canvas.alpha_composite(outline_lay)

    # ── 6. EFEITOS DE BRILHO (subtis) ────────────────────────────────────────
    glow = Image.new('RGBA', (WW, HH), (0, 0, 0, 0))
    gd   = ImageDraw.Draw(glow)

    # Brilho roxo nas mãos — raios pequenos (px no canvas 4×: s(18)=72px → 18px final)
    for grx, gry, alpha in [(s(18), s(12), 60), (s(30), s(19), 30), (s(46), s(29), 12)]:
        for ghx in [s(lhx), s(rhx)]:
            gd.ellipse([ghx - grx, s(hand_cy + 12) - gry,
                        ghx + grx, s(hand_cy + 12) + gry],
                       fill=(90, 45, 165, alpha))

    # Brilho vermelho subtil no ímão (s(38)=152px → 38px final)
    for grx, gry, alpha in [(s(28), s(20), 22), (s(42), s(30), 12)]:
        gd.ellipse([s(hx) - grx, s(hy) - gry, s(hx) + grx, s(hy) + gry],
                   fill=(175, 20, 40, alpha))

    glow = glow.filter(ImageFilter.GaussianBlur(s(3)))
    canvas.alpha_composite(glow)

    # ── 6. DOWNSCALE (LANCZOS) ───────────────────────────────────────────────
    return canvas.resize((FW, FH), Image.LANCZOS)


# ── Geração da spritesheet ────────────────────────────────────────────────────
print(f"Gerando {N_FRAMES} frames ({ATLAS_COLS}×{ATLAS_ROWS} atlas) ...")

atlas  = Image.new('RGBA', (FW * ATLAS_COLS, FH * ATLAS_ROWS), (0, 0, 0, 0))
frames = []

for i in range(N_FRAMES):
    if i % 10 == 0:
        print(f"  Frame {i}/{N_FRAMES}")
    frame = draw_frame(i)
    frames.append(frame)
    col   = i % ATLAS_COLS
    row   = i // ATLAS_COLS
    atlas.paste(frame, (col * FW, row * FH))

out_path = os.path.join(OUTPUT_DIR, "MagnetMageBoss.png")
atlas.save(out_path, "PNG")
print(f"\nSpritesheet guardada: {out_path}")
print(f"Dimensões: {atlas.width} × {atlas.height} px")
print(f"Frames: {N_FRAMES}  |  Atlas: {ATLAS_COLS} cols × {ATLAS_ROWS} rows")
print(f"Por frame: {FW} × {FH} px")

# Guarda frame 0 aumentado (4×) para pré-visualização
preview = frames[0].resize((FW * 4, FH * 4), Image.NEAREST)
prev_path = os.path.join(OUTPUT_DIR, "MagnetMageBoss_preview.png")
preview.save(prev_path, "PNG")
print(f"Preview (frame 0, 4×): {prev_path}")
