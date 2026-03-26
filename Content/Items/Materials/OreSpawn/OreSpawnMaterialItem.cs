using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Materials.OreSpawn
{
	public abstract class OreSpawnMaterialItem : ModItem
	{
		protected virtual int PrototypeRare => ItemRarityID.Green;
		protected virtual int PrototypeValue => Item.buyPrice(silver: 60);

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.rare = PrototypeRare;
			Item.value = PrototypeValue;
		}
	}
}
