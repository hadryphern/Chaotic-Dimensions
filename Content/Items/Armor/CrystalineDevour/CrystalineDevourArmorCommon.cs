using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.Players;

namespace ChaoticDimensions.Content.Items.Armor.CrystalineDevour
{
	internal static class CrystalineDevourArmorCommon
	{
		public const int SharedBreastplateDefense = 40;
		public const int SharedGreavesDefense = 30;
		public const int SharedHelmetDefense = 29;

		public static void AddArmorRecipe(ModItem item, int tearAmount, int lunarBarAmount) {
			item.CreateRecipe()
				.AddIngredient<CrystalineTear>(tearAmount)
				.AddIngredient(ItemID.LunarBar, lunarBarAmount)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}

	public abstract class CrystalineDevourHelmetBase : ModItem
	{
		protected abstract CrystalineDevourSetType SetType { get; }
		protected abstract int BonusDefense { get; }
		protected abstract string SetBonusText { get; }

		public sealed override void SetDefaults() {
			Item.width = 34;
			Item.height = 26;
			Item.value = Item.buyPrice(platinum: 3);
			Item.rare = ItemRarityID.Red;
			Item.defense = CrystalineDevourArmorCommon.SharedHelmetDefense;
			SetHelmetDefaults();
		}

		protected virtual void SetHelmetDefaults() {
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<CrystalineDevourBreastplate>() &&
				legs.type == ModContent.ItemType<CrystalineDevourGreaves>();
		}

		public override void UpdateArmorSet(Player player) {
			player.setBonus = SetBonusText;
			player.statDefense += BonusDefense;

			CrystalinePlayer modPlayer = player.GetModPlayer<CrystalinePlayer>();
			modPlayer.crystalineDevourSet = SetType;
			ApplyClassBonuses(player, modPlayer);
		}

		protected abstract void ApplyClassBonuses(Player player, CrystalinePlayer modPlayer);

		public override void AddRecipes() {
			CrystalineDevourArmorCommon.AddArmorRecipe(this, 24, 14);
		}
	}
}
