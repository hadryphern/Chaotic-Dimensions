using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Materials
{
	public sealed class MonthraScale : ModItem
	{
		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = Item.sellPrice(silver: 60);
			Item.rare = ItemRarityID.Orange;
		}
	}
}
