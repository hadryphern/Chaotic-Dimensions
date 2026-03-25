using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Buffs;

namespace ChaoticDimensions.Content.Items.Consumables
{
	public sealed class CrystalinePotion : ModItem
	{
		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 30;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.rare = ItemRarityID.Purple;
			Item.value = Item.buyPrice(gold: 5);
			Item.healLife = 250;
			Item.potion = true;
		}

		public override bool? UseItem(Player player) {
			player.AddBuff(ModContent.BuffType<CrystalinePotionRegenerationBuff>(), 60 * 10);
			player.AddBuff(ModContent.BuffType<CrystalinePotionFortitudeBuff>(), 60 * 60 * 5);
			return true;
		}

		public override void ModifyPotionDelay(Player player, ref int baseDelay) {
			baseDelay -= 10 * 60;
		}
	}
}
