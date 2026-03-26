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
			Item.defense = 25;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(platinum: 3);
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<CrystalinePlayer>().crystalineEyeEquipped = true;
			player.moveSpeed += 0.22f;
			player.maxRunSpeed += 0.8f;
			player.runAcceleration *= 1.15f;
		}
	}
}
