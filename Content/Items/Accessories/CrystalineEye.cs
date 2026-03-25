using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Players;

namespace ChaoticDimensions.Content.Items.Accessories
{
	public sealed class CrystalineEye : ModItem
	{
		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
			Item.rare = ItemRarityID.Purple;
			Item.value = Item.buyPrice(platinum: 1);
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<CrystalinePlayer>().crystalineEyeEquipped = true;
			player.lifeRegen += 4;
		}
	}
}
