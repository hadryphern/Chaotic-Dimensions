using ChaoticDimensions.Content.Items.ShadowBiome;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Tiles.ShadowBiome
{
	public abstract class ShadowBiomeSolidTileBase : ModTile
	{
		protected virtual Color MapColor => new(90, 50, 130);
		protected virtual int DustTypeOverride => DustID.Shadowflame;

		public override string Texture => $"ChaoticDimensions/Content/Tiles/ShadowBiome/{GetType().Name}";

		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMergeDirt[Type] = true;
			HitSound = SoundID.Tink;
			DustType = DustTypeOverride;
			AddMapEntry(MapColor);
		}
	}

	public sealed class ShadowDirtTile : ShadowBiomeSolidTileBase
	{
		protected override Color MapColor => new(92, 42, 124);

		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			HitSound = SoundID.Dig;
			DustType = DustID.Dirt;
			RegisterItemDrop(ModContent.ItemType<ShadowDirtBlock>());
		}
	}

	public sealed class ShadowGrassTile : ShadowBiomeSolidTileBase
	{
		protected override Color MapColor => new(116, 44, 152);

		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			HitSound = SoundID.Grass;
			DustType = DustID.CorruptGibs;
			TileID.Sets.Grass[Type] = true;
			RegisterItemDrop(ModContent.ItemType<ShadowDirtBlock>());
		}

		public override void RandomUpdate(int i, int j) {
			Tile above = Framing.GetTileSafely(i, j - 1);
			if (above.HasTile && Main.tileSolid[above.TileType]) {
				Framing.GetTileSafely(i, j).TileType = (ushort)ModContent.TileType<ShadowDirtTile>();
				WorldGen.SquareTileFrame(i, j);
				return;
			}

			TrySpreadTo(i + 1, j);
			TrySpreadTo(i - 1, j);
		}

		private static void TrySpreadTo(int i, int j) {
			if (!WorldGen.InWorld(i, j, 20)) {
				return;
			}

			Tile tile = Framing.GetTileSafely(i, j);
			Tile above = Framing.GetTileSafely(i, j - 1);
			if (tile.HasTile && tile.TileType == ModContent.TileType<ShadowDirtTile>() && !above.HasTile) {
				tile.TileType = (ushort)ModContent.TileType<ShadowGrassTile>();
				WorldGen.SquareTileFrame(i, j);
				if (Main.netMode == NetmodeID.Server) {
					NetMessage.SendTileSquare(-1, i, j, 1);
				}
			}
		}
	}

	public sealed class ShadowStoneTile : ShadowBiomeSolidTileBase
	{
		protected override Color MapColor => new(74, 32, 112);

		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			TileID.Sets.Ore[Type] = false;
			HitSound = SoundID.Tink;
			DustType = DustID.Stone;
			RegisterItemDrop(ModContent.ItemType<ShadowStoneBlock>());
		}
	}

	public sealed class ShadowWoodTile : ShadowBiomeSolidTileBase
	{
		protected override Color MapColor => new(88, 64, 112);
		protected override int DustTypeOverride => DustID.WoodFurniture;

		public override void SetStaticDefaults() {
			base.SetStaticDefaults();
			HitSound = SoundID.Dig;
			DustType = DustID.WoodFurniture;
			RegisterItemDrop(ModContent.ItemType<ShadowWood>());
		}
	}
}
