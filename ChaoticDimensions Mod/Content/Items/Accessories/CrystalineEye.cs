// Reune valores, receitas e efeitos dos itens de Crystaline Eye.

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Players;

namespace ChaoticDimensions.Content.Items.Accessories
{
	public sealed class CrystalineEye : ModItem
	{
		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;
			Item.accessory = true;
			Item.defense = 25;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(platinum: 3);
		}

		// Aplica os bonus enquanto o acessorio esta equipado.
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<CrystalinePlayer>().crystalineEyeEquipped = true;
			player.moveSpeed += 0.22f;
			player.maxRunSpeed += 0.8f;
			player.runAcceleration *= 1.15f;
		}
	}
}
