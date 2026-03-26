using Terraria;
using Terraria.ID;

namespace ChaoticDimensions.Content.Items.Materials.OreSpawn
{
	public abstract class OreSpawnResourceItemBase : OreSpawnMaterialItem
	{
		protected virtual int PrototypeWidth => 24;
		protected virtual int PrototypeHeight => 24;
		protected virtual int PlaceTileType => -1;
		protected virtual int PrototypeStack => 9999;

		public override void SetDefaults() {
			base.SetDefaults();
			Item.width = PrototypeWidth;
			Item.height = PrototypeHeight;
			Item.maxStack = PrototypeStack;

			if (PlaceTileType < 0) {
				return;
			}

			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.createTile = PlaceTileType;
		}
	}
}
