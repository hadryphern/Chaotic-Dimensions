from pathlib import Path

from PIL import Image, ImageDraw, ImageEnhance, ImageFilter


PROJECT = Path(__file__).resolve().parents[2]
SOURCE = Path("/home/hadrykkxd/Imagens/AseSprite-Sprites/MoonTalk")
OUTPUT = PROJECT / "assets_work/moontalk"
GAME_OUTPUT = PROJECT / "Assets/MoonTalk"
GAME_SCALE = 4


def paste_layer(canvas: Image.Image, layer: Image.Image, position: tuple[int, int]) -> None:
    canvas.alpha_composite(layer, position)


def tint_line_art(image: Image.Image) -> Image.Image:
    tinted = Image.new("RGBA", image.size, (30, 32, 39, 0))
    tinted.putalpha(image.getchannel("A"))
    return tinted


def make_game_sprite(assembled: Image.Image) -> Image.Image:
    size = (assembled.width * GAME_SCALE, assembled.height * GAME_SCALE)
    return assembled.resize(size, Image.Resampling.LANCZOS)


def assemble(raised: str = "none") -> Image.Image:
    head = Image.open(SOURCE / "MoonTalk_Head.png").convert("RGBA")
    head = head.resize((72, 72), Image.Resampling.LANCZOS)
    head = ImageEnhance.Brightness(head).enhance(0.62)
    body = tint_line_art(Image.open(SOURCE / "MoonTalk_Body.png").convert("RGBA"))
    right_arm = tint_line_art(Image.open(SOURCE / "MoonTalk_Braco.png").convert("RGBA"))
    left_arm = right_arm.transpose(Image.Transpose.FLIP_LEFT_RIGHT)

    assembled = Image.new("RGBA", (512, 512), (0, 0, 0, 0))

    attack_right = tint_line_art(Image.open(SOURCE / "MoonTalk_Braco2.png").convert("RGBA"))
    attack_left = attack_right.transpose(Image.Transpose.FLIP_LEFT_RIGHT)
    paste_layer(assembled, attack_left if raised in {"left", "both"} else left_arm, (89, 175))
    paste_layer(assembled, attack_right if raised in {"right", "both"} else right_arm, (288, 175))
    paste_layer(assembled, body, (192, 160))
    paste_layer(assembled, head, (220, 88))

    return assembled


def make_scene(assembled: Image.Image) -> Image.Image:
    scale = 2
    boss = assembled.resize((1024, 1024), Image.Resampling.NEAREST)
    scene = Image.new("RGBA", boss.size, (14, 15, 18, 255))
    scene.alpha_composite(boss)
    return scene


def main() -> None:
    OUTPUT.mkdir(parents=True, exist_ok=True)
    GAME_OUTPUT.mkdir(parents=True, exist_ok=True)

    normal = assemble("none")
    left_attack = assemble("left")
    right_attack = assemble("right")
    both_attack = assemble("both")
    normal.save(OUTPUT / "MoonTalk_Montagem_Normal_Transparente.png", optimize=True)
    left_attack.save(OUTPUT / "MoonTalk_Montagem_Ataque_Esquerdo_Transparente.png", optimize=True)
    right_attack.save(OUTPUT / "MoonTalk_Montagem_Ataque_Direito_Transparente.png", optimize=True)
    both_attack.save(OUTPUT / "MoonTalk_Montagem_Ataque_Duplo_Transparente.png", optimize=True)
    make_game_sprite(normal).save(GAME_OUTPUT / "MoonTalk_Normal.png", optimize=True)
    make_game_sprite(left_attack).save(GAME_OUTPUT / "MoonTalk_Ataque_Esquerdo.png", optimize=True)
    make_game_sprite(right_attack).save(GAME_OUTPUT / "MoonTalk_Ataque_Direito.png", optimize=True)
    make_game_sprite(both_attack).save(GAME_OUTPUT / "MoonTalk_Ataque_Duplo.png", optimize=True)
    make_scene(normal).save(OUTPUT / "MoonTalk_Preview_Normal.png", optimize=True)
    make_scene(left_attack).save(OUTPUT / "MoonTalk_Preview_Ataque_Esquerdo.png", optimize=True)
    make_scene(right_attack).save(OUTPUT / "MoonTalk_Preview_Ataque_Direito.png", optimize=True)
    make_scene(both_attack).save(OUTPUT / "MoonTalk_Preview_Ataque_Duplo.png", optimize=True)

    comparison = Image.new("RGBA", (2048, 2048), (2, 2, 7, 255))
    for image, position in zip(
        (normal, left_attack, right_attack, both_attack),
        ((0, 0), (1024, 0), (0, 1024), (1024, 1024)),
    ):
        comparison.alpha_composite(make_scene(image), position)
    comparison.save(OUTPUT / "MoonTalk_Preview_Quatro_Estados.png", optimize=True)


if __name__ == "__main__":
    main()
