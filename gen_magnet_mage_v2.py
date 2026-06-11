# -*- coding: utf-8 -*-
"""
MagnetMageBoss — Versão 2 do gerador de spritesheet idle.
Melhorias: magnet maior c/ relâmpago, manto com dobras e botões,
mãos mais detalhadas, partículas magnéticas orbitais, sombras AO.
"""

from PIL import Image, ImageDraw, ImageFilter
import math, os
import numpy as np

FW, FH       = 280, 340
S            = 4
N_FRAMES     = 120
ATLAS_COLS   = 10
ATLAS_ROWS   = 12
WW, HH       = FW * S, FH * S

OUTPUT_DIR = os.path.join(os.path.dirname(__file__), "Content", "NPCs", "MagnetMage")
os.makedirs(OUTPUT_DIR, exist_ok=True)


# ══════════════════════════════════════════════════════════════════════
#  Paleta
# ══════════════════════════════════════════════════════════════════════
# Magnet
MR0  = ( 80,   8,   8, 255)   # sombra externa profunda
MR1  = (145,  22,  22, 255)   # sombra
MR2  = (202,  45,  40, 255)   # base
MR3  = (235,  72,  58, 255)   # highlight
MR4  = (255, 140, 110, 255)   # reflexo metálico

MG0  = ( 60,  62,  72, 255)   # tip sombra
MG1  = (128, 133, 145, 255)   # tip base
MG2  = (195, 198, 208, 255)   # tip highlight
MG3  = (240, 243, 248, 255)   # brilho topo
MG_ORG = (255, 190, 100, 120) # reflexo laranja quente

# Relâmpago
BOLT_C   = (160, 120, 255, 255)
BOLT_HI  = (230, 210, 255, 255)
BOLT_GLO = ( 80,  50, 180,  60)

# Manto
RB0  = (  6,   4,  14, 255)   # sombra profunda
RB1  = ( 18,  14,  32, 255)   # base escura
RB2  = ( 34,  28,  58, 255)   # meio
RB3  = ( 62,  52,  96, 255)   # highlight interior
RB4  = ( 95,  80, 140, 255)   # rim light borda
RB_INNER = ( 14,  10,  24, 255) # forro interior (ligeiramente roxo)
BUTTON  = (210, 175,  60, 255)  # botão ouro

# Colarinho
COL0 = ( 24,  18,  46, 255)
COL1 = ( 52,  44,  88, 255)
COL2 = ( 88,  76, 130, 255)

# Luvas
GL0  = ( 15,  13,  20, 255)
GL1  = ( 35,  32,  44, 255)
GL2  = ( 62,  58,  78, 255)
GL3  = ( 95,  88, 112, 255)
GL_GLO = ( 90,  50, 180, 80)

# Partículas magnéticas
PART_RED  = (220,  55,  40, 200)
PART_PUR  = (120,  60, 220, 180)


# ══════════════════════════════════════════════════════════════════════
#  Helpers
# ══════════════════════════════════════════════════════════════════════
def s(v):    return int(round(v * S))
def sf(v):   return float(v * S)

def lerp(a, b, t):       return a + (b - a) * t
def lc(c1, c2, t):       return tuple(int(lerp(c1[i], c2[i], t)) for i in range(4))
def clamp(v, lo=0, hi=255): return max(lo, min(hi, int(v)))


def new(w=WW, h=HH):     return Image.new('RGBA', (w, h), (0, 0, 0, 0))


def vgrad_rect(img, x0, y0, x1, y1, c_top, c_bot):
    """Gradiente vertical num rectângulo (px absolutos)."""
    d = ImageDraw.Draw(img)
    x0, y0, x1, y1 = int(x0), int(y0), int(x1), int(y1)
    for y in range(max(0, y0), min(img.height, y1)):
        t = (y - y0) / max(1, y1 - y0)
        d.line([(max(0, x0), y), (min(img.width-1, x1), y)], fill=lc(c_top, c_bot, t))


def hgrad_rect(img, x0, y0, x1, y1, c_l, c_r):
    """Gradiente horizontal."""
    d = ImageDraw.Draw(img)
    x0, y0, x1, y1 = int(x0), int(y0), int(x1), int(y1)
    for x in range(max(0, x0), min(img.width, x1)):
        t = (x - x0) / max(1, x1 - x0)
        d.line([(x, max(0, y0)), (x, min(img.height-1, y1))], fill=lc(c_l, c_r, t))


def masked_paste(canvas, layer, mask_poly_pts):
    """Pasta layer sobre canvas apenas dentro do polígono mask."""
    mask = Image.new('L', (WW, HH), 0)
    ImageDraw.Draw(mask).polygon(mask_poly_pts, fill=255)
    tmp = new()
    tmp.paste(layer, mask=mask)
    canvas.alpha_composite(tmp)


def poly_pts(cx, pts_1x):
    """Converte lista de pontos 1× em escala S, com centro cx aplicado."""
    return [(s(x), s(y)) for x, y in pts_1x]


def ellipse_alpha(canvas, cx, cy, rx, ry, color, blur=0):
    lay = new()
    ImageDraw.Draw(lay).ellipse(
        [int(sf(cx)-s(rx)), int(sf(cy)-s(ry)),
         int(sf(cx)+s(rx)), int(sf(cy)+s(ry))], fill=color)
    if blur:
        lay = lay.filter(ImageFilter.GaussianBlur(blur))
    canvas.alpha_composite(lay)


def half_ellipse(canvas, cx, cy, rx, ry, color, top=False):
    """Metade inferior (top=False) ou superior (top=True) de uma elipse."""
    lay = new()
    ImageDraw.Draw(lay).ellipse(
        [int(sf(cx)-s(rx)), int(sf(cy)-s(ry)),
         int(sf(cx)+s(rx)), int(sf(cy)+s(ry))], fill=color)
    cut = new()
    if top:
        ImageDraw.Draw(cut).rectangle([0, s(cy), WW, HH], fill=(0,0,0,255))
    else:
        ImageDraw.Draw(cut).rectangle([0, 0, WW, s(cy)], fill=(0,0,0,255))
    lay_arr = np.array(lay)
    cut_arr = np.array(cut)
    lay_arr[cut_arr[:,:,3] > 0] = [0,0,0,0]
    canvas.alpha_composite(Image.fromarray(lay_arr))


def lightning_pts(x0, y0, x1, y1, rng, n=14, amp=12):
    """Caminho de relâmpago dentado entre dois pontos (1× coords)."""
    dx = x1 - x0; dy = y1 - y0
    length = max(1, math.sqrt(dx*dx + dy*dy))
    px = -dy / length; py = dx / length   # perpendicular unitário
    pts = [(x0, y0)]
    for i in range(1, n):
        t   = i / n
        bx  = x0 + dx * t
        by  = y0 + dy * t
        off = rng.uniform(-amp, amp) * math.sin(math.pi * t)
        pts.append((bx + px*off, by + py*off))
    pts.append((x1, y1))
    return pts


def draw_lightning(canvas, x0, y0, x1, y1, rng, frame_seed):
    """Relâmpago elétrico com glow."""
    child_rng = np.random.default_rng(frame_seed)
    pts = lightning_pts(x0, y0, x1, y1, child_rng, n=16, amp=14)
    spx = [(s(px), s(py)) for px, py in pts]

    # Glow layer
    glay = new()
    gd = ImageDraw.Draw(glay)
    for i in range(len(spx)-1):
        gd.line([spx[i], spx[i+1]], fill=BOLT_GLO, width=s(6))
    glay = glay.filter(ImageFilter.GaussianBlur(s(4)))
    canvas.alpha_composite(glay)

    # Core bolt
    blay = new()
    bd = ImageDraw.Draw(blay)
    for i in range(len(spx)-1):
        bd.line([spx[i], spx[i+1]], fill=BOLT_C, width=s(2))
    # Bright center
    for i in range(len(spx)-1):
        bd.line([spx[i], spx[i+1]], fill=BOLT_HI, width=max(1, s(1)))
    canvas.alpha_composite(blay)


def draw_magnet_arm(canvas, x0, y0, x1, y1, flip=False):
    """Braço metálico do ímão com shading de 5 tons."""
    lay = new()
    w = x1 - x0

    # 5 faixas de gradiente horizontal: sombra → bright → reflexo → bright → sombra
    zones = 5
    colors = [MR0, MR1, MR2, MR3, MR4, MR3, MR1, MR0]
    zw = w / (len(colors)-1)
    for zi in range(len(colors)-1):
        zx0 = s(x0 + zi * zw)
        zx1 = s(x0 + (zi+1) * zw)
        t0, t1 = (zi/(len(colors)-1)), ((zi+1)/(len(colors)-1))
        vgrad_rect(lay, zx0, s(y0), zx1, s(y1), colors[zi], colors[zi+1])

    # Borda escura superior e inferior
    d = ImageDraw.Draw(lay)
    d.rectangle([s(x0), s(y0), s(x1), s(y0)+s(2)], fill=MR0)
    d.rectangle([s(x0), s(y1)-s(2), s(x1), s(y1)], fill=MR0)
    # Borda lateral externa
    if not flip:
        d.rectangle([s(x0), s(y0), s(x0)+s(2), s(y1)], fill=MR0)
    else:
        d.rectangle([s(x1)-s(2), s(y0), s(x1), s(y1)], fill=MR0)

    canvas.alpha_composite(lay)


def draw_tip(canvas, x0, y0, x1, y1):
    """Ponta cilíndrica cinzenta do polo do ímão."""
    lay = new()
    ld = ImageDraw.Draw(lay)
    cx = (x0 + x1) / 2
    w  = x1 - x0

    # Base cinzenta (gradiente v)
    vgrad_rect(lay, s(x0), s(y0+6), s(x1), s(y1), MG2, MG1)
    # Borda inferior (junta com braço)
    ld.rectangle([s(x0), s(y1-3), s(x1), s(y1)], fill=MG0)

    # Topo domed (elipse)
    tip_h = 12  # dome height
    dome_lay = new()
    dl = ImageDraw.Draw(dome_lay)
    dl.ellipse([s(x0-2), s(y0), s(x1+2), s(y0 + tip_h*2)], fill=MG2)
    # Corta metade inferior da elipse (só fica dome)
    dl.rectangle([0, s(y0 + tip_h), WW, HH], fill=(0,0,0,0))
    # Highlight no topo
    hl_x = cx - w*0.2
    dl.ellipse([s(hl_x), s(y0+1), s(hl_x + w*0.4), s(y0 + tip_h*0.6)], fill=MG3)
    canvas.alpha_composite(dome_lay)
    canvas.alpha_composite(lay)

    # Reflexo laranja quente (rim light do ímão)
    rim_lay = new()
    ImageDraw.Draw(rim_lay).ellipse(
        [s(x0+2), s(y1-8), s(x1-2), s(y1+2)], fill=MG_ORG)
    rim_lay = rim_lay.filter(ImageFilter.GaussianBlur(s(2)))
    canvas.alpha_composite(rim_lay)

    # Sombra AO na base da ponta
    ao_lay = new()
    ImageDraw.Draw(ao_lay).rectangle(
        [s(x0), s(y0+6), s(x1), s(y0+12)], fill=(*MG0[:3], 120))
    ao_lay = ao_lay.filter(ImageFilter.GaussianBlur(s(1)))
    canvas.alpha_composite(ao_lay)


def robe_outline(cx, ry0, ry1, rw_t, rw_b, sl, sr, sbot, n=24):
    """Calcula os vértices do polígono do manto."""
    pts = []
    # lado esquerdo (topo → base)
    for i in range(n+1):
        tt = i / n
        y  = ry0 + (ry1 - ry0) * tt
        w  = rw_t + (rw_b - rw_t) * tt
        sx = sl * tt + sbot * 0.6 * math.sin(math.pi * tt) * (-1)
        pts.append((cx - w + sx, y))
    # lado direito (base → topo)
    for i in range(n, -1, -1):
        tt = i / n
        y  = ry0 + (ry1 - ry0) * tt
        w  = rw_t + (rw_b - rw_t) * tt
        sx = sr * tt + sbot * 0.4 * math.sin(math.pi * tt)
        pts.append((cx + w + sx, y))
    return [(s(x), s(y)) for x, y in pts]


def draw_robe(canvas, cx, ry0, ry1, rw_t, rw_b, sl, sr, sbot, rng):
    poly = robe_outline(cx, ry0, ry1, rw_t, rw_b, sl, sr, sbot)

    # ── Camada base (sombra profunda) ──
    base_lay = new()
    ImageDraw.Draw(base_lay).polygon(poly, fill=RB0)
    canvas.alpha_composite(base_lay)

    # ── Gradiente vertical por máscara ──
    grad_lay = new()
    vgrad_rect(grad_lay, s(cx-rw_b-10), s(ry0), s(cx+rw_b+10), s(ry1), RB2, RB1)
    masked_paste(canvas, grad_lay, poly)

    # ── Painel central (chest highlight) ──
    chest_w = rw_t * 0.45
    chest_pts = []
    for i in range(13):
        tt = i / 12
        y  = ry0 + (ry1 - ry0) * tt
        w  = chest_w * (1 - tt * 0.3)   # estreita ligeiramente ao descer
        chest_pts.append((cx - w + sl * tt * 0.3, y))
    for i in range(12, -1, -1):
        tt = i / 12
        y  = ry0 + (ry1 - ry0) * tt
        w  = chest_w * (1 - tt * 0.3)
        chest_pts.append((cx + w + sr * tt * 0.3, y))
    chest_pts = [(s(x), s(y)) for x, y in chest_pts]
    chest_lay = new()
    vgrad_rect(chest_lay, s(cx-chest_w), s(ry0), s(cx+chest_w), s(ry1), RB3, RB2)
    masked_paste(canvas, chest_lay, chest_pts)

    # ── Rim light esquerdo ──
    rim_l_lay = new()
    vgrad_rect(rim_l_lay, s(cx-rw_b-2), s(ry0), s(cx-rw_b+12), s(ry1), RB4, RB2)
    masked_paste(canvas, rim_l_lay, poly)

    # ── Dobras (fold lines) ──
    fold_lay = new()
    fd = ImageDraw.Draw(fold_lay)
    fold_x_top = sf(cx) + sf(sl * 0.1)
    fold_x_bot = sf(cx) + sf(sbot * 0.5)
    for fi in range(-2, 3):
        off = fi * s(10)
        fd.line(
            [(int(fold_x_top + off), s(ry0 + 8)),
             (int(fold_x_bot + off * 1.3), s(ry1 - 6))],
            fill=(*RB0[:3], 130 - abs(fi)*30), width=max(1, s(1)))
    canvas.alpha_composite(fold_lay)

    # ── Forro inferior (bainha) ──
    hem_h = 14
    hem_pts_top = robe_outline(cx, ry1-hem_h, ry1, rw_b-2, rw_b, sl*0.95, sr*0.95, sbot*0.95)
    hem_lay = new()
    vgrad_rect(hem_lay, s(cx-rw_b-2), s(ry1-hem_h), s(cx+rw_b+2), s(ry1), RB_INNER, RB0)
    masked_paste(canvas, hem_lay, hem_pts_top)

    # ── Botões dourados ──
    btn_lay = new()
    bd = ImageDraw.Draw(btn_lay)
    btn_x = cx + sl * 0.2
    for bi in range(5):
        by = ry0 + 22 + bi * 28
        btn_x_off = btn_x + sl * (by - ry0)/(ry1 - ry0)
        bd.ellipse([s(btn_x_off-4), s(by-4), s(btn_x_off+4), s(by+4)], fill=BUTTON)
        # Reflexo topo
        bd.ellipse([s(btn_x_off-2), s(by-3), s(btn_x_off+1), s(by-1)], fill=(255,240,150,220))
        # Sombra baixo
        bd.ellipse([s(btn_x_off-2), s(by+1), s(btn_x_off+2), s(by+4)], fill=(120, 90, 20, 200))
    canvas.alpha_composite(btn_lay)

    # ── Sombra AO no topo do manto ──
    ao_lay = new()
    vgrad_rect(ao_lay, s(cx-rw_t), s(ry0), s(cx+rw_t), s(ry0+18), RB0, (*RB0[:3], 0))
    masked_paste(canvas, ao_lay, poly)

    # ── Textura de tecido (ruído subtil) ──
    arr = np.array(canvas)
    # mask onde o manto existe
    msk = np.array(Image.new('L', (WW, HH), 0))
    ImageDraw.Draw(Image.fromarray(msk)).polygon(poly, fill=255)
    noise = rng.normal(0, 4, (HH, WW)).astype(np.int32)
    msk_arr = np.array(Image.new('L', (WW, HH), 0))
    tmp_img  = Image.new('L', (WW, HH), 0)
    ImageDraw.Draw(tmp_img).polygon(poly, fill=255)
    msk_arr  = np.array(tmp_img)
    for ch in range(3):
        arr[:,:,ch] = np.clip(
            arr[:,:,ch].astype(np.int32) + noise * (msk_arr > 0),
            0, 255).astype(np.uint8)
    return Image.fromarray(arr)


def draw_collar(canvas, cx, cy, rng):
    # Base oval
    lay = new()
    d   = ImageDraw.Draw(lay)
    d.ellipse([s(cx-38), s(cy-10), s(cx+38), s(cy+13)], fill=COL0)
    # Highlight superior
    d.ellipse([s(cx-30), s(cy-7),  s(cx+30), s(cy+3)],  fill=COL1)
    d.ellipse([s(cx-18), s(cy-5),  s(cx+18), s(cy+0)],  fill=COL2)
    canvas.alpha_composite(lay)


def draw_hand(canvas, hcx, hcy, mirror, f_open, rng):
    """Mão com palma, 4 dedos e polegar. f_open: 0=punho, 1=aberta."""
    pw, ph = 46, 28
    px0, px1 = hcx - pw/2, hcx + pw/2
    py0, py1 = hcy - ph/2, hcy + ph/2

    lay = new()
    dh  = ImageDraw.Draw(lay)

    # Pulso (cuff) — anel mais claro no topo
    cuff_h = 8
    cuff_lay = new()
    vgrad_rect(cuff_lay, s(px0+2), s(py0-cuff_h), s(px1-2), s(py0+2), GL3, GL1)
    dh2 = ImageDraw.Draw(cuff_lay)
    # Trim dourado no cuff
    dh2.rectangle([s(px0+2), s(py0-cuff_h-1), s(px1-2), s(py0-cuff_h+2)], fill=(*BUTTON[:3], 200))
    lay.alpha_composite(cuff_lay)

    # Palma (elipse base)
    dh.ellipse([s(px0), s(py0), s(px1), s(py1)], fill=GL0)
    # Gradiente vertical palma
    palm_grad = new()
    vgrad_rect(palm_grad, s(px0+3), s(py0+3), s(px1-3), s(py1-3), GL2, GL1)
    lay.alpha_composite(palm_grad)
    # Highlight
    dh.ellipse([s(px0+6), s(py0+4), s(px1-6), s(py0+14)], fill=GL3)
    # Rim inferior
    dh.arc([s(px0+3), s(py1-8), s(px1-3), s(py1+3)], 180, 0,
           fill=(*GL3[:3], 100), width=s(1))

    # Dedos — 4 dedos
    n_fing = 4
    fw = 9.0
    fsp = pw / n_fing
    fh_open  = 22.0
    fh_close = 9.0
    fh = fh_close + (fh_open - fh_close) * f_open

    for fi in range(n_fing):
        fx = px0 + fi * fsp + fsp / 2
        fy0_d = py0 - fh
        fy1_d = py0 + 5

        # Cilindro do dedo — hgrad
        fng_lay = new()
        hgrad_rect(fng_lay, s(fx-fw/2), s(fy0_d+fw/2), s(fx+fw/2), s(fy1_d), GL0, GL2)
        dh3 = ImageDraw.Draw(fng_lay)
        # Topo arredondado
        dh3.ellipse([s(fx-fw/2), s(fy0_d), s(fx+fw/2), s(fy0_d+fw)], fill=GL2)
        dh3.ellipse([s(fx-fw/2+2), s(fy0_d+1), s(fx+fw/2-2), s(fy0_d+fw/2)], fill=GL3)
        # Junta
        dh3.ellipse([s(fx-fw/2+1), s(fy1_d-6), s(fx+fw/2-1), s(fy1_d+1)], fill=GL0)
        # Linha de separação entre falanges
        mid_y = (fy0_d + fy1_d) / 2 + 3
        dh3.line([(s(fx-fw/2+1), s(mid_y)), (s(fx+fw/2-1), s(mid_y))],
                 fill=(*GL0[:3], 160), width=max(1, s(1)))
        lay.alpha_composite(fng_lay)

    # Polegar
    ts = -1 if mirror else 1
    tcx = (px0 - 7) if mirror else (px1 + 7)
    tcy = hcy + 2
    t_ext = f_open * 10
    thx = tcx + ts * t_ext
    thy = tcy
    dh.ellipse([s(thx-8), s(thy-9), s(thx+8), s(thy+9)], fill=GL1)
    dh.ellipse([s(thx-5), s(thy-7), s(thx+5), s(thy-2)], fill=GL3)

    # Aura roxa entre os dedos
    glo_lay = new()
    for fi in range(n_fing - 1):
        gx = px0 + (fi + 0.5) * fsp + fsp / 2
        gy = py0 - fh * 0.4
        ImageDraw.Draw(glo_lay).ellipse(
            [s(gx-5), s(gy-8), s(gx+5), s(gy+8)], fill=GL_GLO)
    glo_lay = glo_lay.filter(ImageFilter.GaussianBlur(s(3)))
    lay.alpha_composite(glo_lay)

    canvas.alpha_composite(lay)


# Seeds fixos para partículas (determinísticos)
_PART_SEEDS = np.random.default_rng(1337).integers(0, 10000, size=100)


def draw_particles(canvas, cx, body_cy, hand_cy, frame_idx, seeds):
    """Partículas magnéticas em órbita."""
    lay = new()
    ld  = ImageDraw.Draw(lay)
    n_parts = 18

    for pi in range(n_parts):
        seed    = int(seeds[pi])
        angle   = (2*math.pi * pi/n_parts) + frame_idx * 0.052 + seed * 0.001
        radius  = 80 + (seed % 25) + math.sin(frame_idx*0.08 + pi) * 8
        orb_x   = cx + math.cos(angle) * radius
        orb_y   = body_cy - 30 + math.sin(angle * 1.3) * 28

        col     = PART_RED if pi % 2 == 0 else PART_PUR
        size    = 2.5 + (seed % 3) * 0.7
        alpha_f = 0.4 + 0.6 * abs(math.sin(frame_idx*0.04 + pi))
        pcolor  = (*col[:3], int(col[3] * alpha_f))
        ld.ellipse([s(orb_x-size), s(orb_y-size),
                    s(orb_x+size), s(orb_y+size)], fill=pcolor)

    lay_b = lay.filter(ImageFilter.GaussianBlur(s(1)))
    canvas.alpha_composite(lay_b)
    canvas.alpha_composite(lay)


# ══════════════════════════════════════════════════════════════════════
#  Frame principal
# ══════════════════════════════════════════════════════════════════════


def draw_frame(fi):
    t         = fi / N_FRAMES
    bob       = math.sin(2*math.pi*t) * 8.0
    t_hand    = (fi - N_FRAMES*0.13) / N_FRAMES
    hand_bob  = math.sin(2*math.pi*t_hand) * 8.0
    sl        = math.sin(2*math.pi*t*1.35 + 0.2) * 4.0
    sr        = math.sin(2*math.pi*t*1.35 - 0.25) * 3.5
    sbot      = math.sin(2*math.pi*t*0.85) * 3.0
    f_open    = (math.sin(2*math.pi*t*2) + 1) / 2.0

    rng = np.random.default_rng(fi * 31 + 7)

    canvas = new()
    CX  = FW / 2.0
    CY_B = FH * 0.60 + bob
    CY_H = FH * 0.50 + hand_bob

    # ── 1. Partículas orbitais (abaixo de tudo) ───────────────────────
    draw_particles(canvas, CX, CY_B, CY_H, fi, _PART_SEEDS)

    # ── 2. Manto ──────────────────────────────────────────────────────
    ry0  = CY_B - 78
    ry1  = CY_B + 90
    rw_t = 50
    rw_b = 90
    canvas = draw_robe(canvas, CX, ry0, ry1, rw_t, rw_b, sl, sr, sbot, rng)

    # ── 3. Colarinho ──────────────────────────────────────────────────
    draw_collar(canvas, CX, ry0 - 4, rng)

    # ── 4. Ímão ───────────────────────────────────────────────────────
    HX  = CX
    HY  = ry0 - 52
    AW  = 24             # largura de cada braço
    AH  = 64             # altura dos braços
    IHW = 28             # metade do espaço interno

    ay0 = HY - AH/2
    ay1 = HY + AH/2

    # Braço esquerdo
    la0, la1 = HX - IHW - AW, HX - IHW
    draw_magnet_arm(canvas, la0, ay0, la1, ay1, flip=False)

    # Braço direito
    ra0, ra1 = HX + IHW, HX + IHW + AW
    draw_magnet_arm(canvas, ra0, ay0, ra1, ay1, flip=True)

    # Arco inferior (body do ímão)
    arc_rx = IHW + AW/2 + 4
    arc_ry = 28

    # Base escura do arco
    half_ellipse(canvas, HX, ay1, arc_rx+3, arc_ry+3, MR0)
    # Meio
    half_ellipse(canvas, HX, ay1, arc_rx, arc_ry, MR2)
    # Highlight interno
    half_ellipse(canvas, HX, ay1, arc_rx-8, arc_ry-8, MR3)

    # Reflexo metálico interior do arco (brilho "oco")
    arc_glow = new()
    ImageDraw.Draw(arc_glow).ellipse(
        [s(HX - arc_rx + 10), s(ay1 - arc_ry + 8),
         s(HX + arc_rx - 10), s(ay1 + arc_ry - 8)],
        fill=(*MR4[:3], 60))
    arc_glow = arc_glow.filter(ImageFilter.GaussianBlur(s(3)))
    canvas.alpha_composite(arc_glow)

    # Pontas cinzentas
    tp_ew = 4     # extra width das pontas
    for tx0, tx1 in [(la0-tp_ew, la1+tp_ew), (ra0-tp_ew, ra1+tp_ew)]:
        draw_tip(canvas, tx0, ay0-22, tx1, ay0+5)

    # ── 5. Relâmpago elétrico entre os polos ─────────────────────────
    # Da ponta esquerda à direita, no topo (y ~ ay0-10)
    bolt_y = ay0 - 8
    bolt_x0 = (la0+la1)/2
    bolt_x1 = (ra0+ra1)/2
    draw_lightning(canvas, bolt_x0, bolt_y, bolt_x1, bolt_y, rng,
                   frame_seed=fi * 997 + 13)

    # ── 6. Mãos ───────────────────────────────────────────────────────
    lhx, rhx = CX - 92, CX + 92
    draw_hand(canvas, lhx, CY_H + 14, mirror=False, f_open=f_open, rng=rng)
    draw_hand(canvas, rhx, CY_H + 14, mirror=True,  f_open=f_open, rng=rng)

    # ── 7. Glow final ─────────────────────────────────────────────────
    glow = new()
    gd   = ImageDraw.Draw(glow)
    # Aura magnética (vermelho, no ímão)
    for grx, gry, alp in [(s(42), s(32), 25), (s(28), s(22), 18), (s(16), s(13), 30)]:
        gd.ellipse([s(HX)-grx, s(HY)-gry, s(HX)+grx, s(HY)+gry],
                   fill=(175, 20, 40, alp))
    # Aura roxa (nas mãos)
    for grx, gry, alp in [(s(22), s(16), 55), (s(38), s(26), 28), (s(55), s(36), 12)]:
        for ghx in [s(lhx), s(rhx)]:
            gd.ellipse([ghx-grx, s(CY_H+14)-gry,
                        ghx+grx, s(CY_H+14)+gry],
                       fill=(90, 45, 165, alp))
    # Brilho do relâmpago
    gd.ellipse([s(bolt_x0-4), s(bolt_y-6), s(bolt_x1+4), s(bolt_y+6)],
               fill=(120, 80, 220, 35))
    glow = glow.filter(ImageFilter.GaussianBlur(s(3)))
    canvas.alpha_composite(glow)

    return canvas.resize((FW, FH), Image.LANCZOS)


# ══════════════════════════════════════════════════════════════════════
#  Geração
# ══════════════════════════════════════════════════════════════════════
print(f"Gerando v2 — {N_FRAMES} frames ({ATLAS_COLS}×{ATLAS_ROWS}) ...")

atlas  = Image.new('RGBA', (FW*ATLAS_COLS, FH*ATLAS_ROWS), (0,0,0,0))
frames = []

for i in range(N_FRAMES):
    if i % 10 == 0:
        print(f"  {i}/{N_FRAMES}")
    frame = draw_frame(i)
    frames.append(frame)
    atlas.paste(frame, ((i % ATLAS_COLS)*FW, (i // ATLAS_COLS)*FH))

out = os.path.join(OUTPUT_DIR, "MagnetMageBoss.png")
atlas.save(out, "PNG")
print(f"\nAtlas: {out}  ({atlas.width}×{atlas.height})")

# Preview frame 0 (4×) e frame 45 (dedos fechados)
for idx, name in [(0, "preview_f0"), (15, "preview_f15"), (45, "preview_f45")]:
    p = frames[idx].resize((FW*4, FH*4), Image.NEAREST)
    p.save(os.path.join(OUTPUT_DIR, f"MagnetMageBoss_{name}.png"))
print("Previews guardados.")
