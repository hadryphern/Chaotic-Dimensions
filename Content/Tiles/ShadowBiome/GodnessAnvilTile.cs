using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ChaoticDimensions.Content.Tiles.ShadowBiome
{
	public sealed class GodnessAnvilTile : ModTile
	{
		public override string Texture => "ChaoticDimensions/Content/Tiles/ShadowBiome/GodnessAnvilTile";

		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileSolidTop[Type] = false;
			TileID.Sets.DisableSmartCursor[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateWidth = 20;
			TileObjectData.newTile.CoordinatePadding = 1;
			TileObjectData.newTile.CoordinateHeights = [16, 15];
			TileObjectData.newTile.DrawYOffset = 4;
			TileObjectData.addTile(Type);

			AdjTiles = [TileID.MythrilAnvil, TileID.LunarCraftingStation];
			HitSound = SoundID.Tink;
			DustType = DustID.Shadowflame;

			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(136, 84, 180), name);
		}
	}
}
