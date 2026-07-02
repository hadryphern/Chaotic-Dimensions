// Define os blocos, colocacao e interacoes de Crystaline Barrier Block.

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Tiles
{
	public sealed class CrystalineBarrierBlock : ModTile
	{
		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileSpelunker[Type] = true;

			HitSound = SoundID.Shatter;
			DustType = DustID.GemAmethyst;
			MineResist = float.MaxValue;
			MinPick = int.MaxValue;

			AddMapEntry(new Color(208, 148, 255));
		}

		// Protege este tile contra explosoes quando necessario.
		public override bool CanExplode(int i, int j) => false;
	}
}
