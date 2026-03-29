using ChaoticDimensions.Content.Items.MinecraftLegacy;
using ChaoticDimensions.Content.Items.ShadowBiome;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Armor.Rosalita
{
	internal static class RosalitaArmorCommon
	{
		public static void AddArmorRecipe(ModItem item, int rosalitaAmount, int hallowedBars, int scrapAmount) {
			item.CreateRecipe()
				.AddIngredient<RosalitaGem>(rosalitaAmount)
				.AddIngredient(ItemID.HallowedBar, hallowedBars)
				.AddIngredient<ShadowScrap>(scrapAmount)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public static void ApplySetBonuses(Player player) {
			player.GetDamage(DamageClass.Generic) += 0.18f;
			player.GetCritChance(DamageClass.Generic) += 15f;
			player.moveSpeed += 0.14f;
			player.whipRangeMultiplier += 0.12f;
			player.maxMinions += 1;
			player.statManaMax2 += 60;
			player.lifeRegen += 5;
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public sealed class RosalitaHelmet : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/Armor/Rosalita/RosalitaHelmet";

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.buyPrice(gold: 16);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 26;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<RosalitaBreastplate>() &&
				legs.type == ModContent.ItemType<RosalitaGreaves>();
		}

		public override void UpdateArmorSet(Player player) {
			player.setBonus = "Empowers every class at once with rosalita pressure";
			RosalitaArmorCommon.ApplySetBonuses(player);
			player.statDefense += 18;
		}

		public override void UpdateEquip(Player player) {
			player.GetDamage(DamageClass.Generic) += 0.08f;
			player.GetCritChance(DamageClass.Generic) += 6f;
		}

		public override void AddRecipes() {
			RosalitaArmorCommon.AddArmorRecipe(this, 16, 10, 8);
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public sealed class RosalitaBreastplate : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/Armor/Rosalita/RosalitaBreastplate";

		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 20;
			Item.value = Item.buyPrice(gold: 22);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 34;
		}

		public override void UpdateEquip(Player player) {
			player.GetDamage(DamageClass.Generic) += 0.1f;
			player.maxMinions += 1;
			player.statManaMax2 += 30;
		}

		public override void AddRecipes() {
			RosalitaArmorCommon.AddArmorRecipe(this, 24, 14, 12);
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public sealed class RosalitaGreaves : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/Armor/Rosalita/RosalitaGreaves";

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.buyPrice(gold: 18);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 24;
		}

		public override void UpdateEquip(Player player) {
			player.moveSpeed += 0.12f;
			player.maxRunSpeed += 0.8f;
			player.lifeRegen += 2;
		}

		public override void AddRecipes() {
			RosalitaArmorCommon.AddArmorRecipe(this, 18, 12, 10);
		}
	}
}
