using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Materials
{
	public sealed class CrystalineTear : ModItem
	{
		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 26;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = Item.buyPrice(gold: 2);
			Item.rare = ItemRarityID.Purple;
			Item.material = true;
		}
	}
}
