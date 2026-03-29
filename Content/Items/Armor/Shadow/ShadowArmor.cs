using ChaoticDimensions.Content.Items.MinecraftLegacy;
using ChaoticDimensions.Content.Items.ShadowBiome;
using ChaoticDimensions.Content.Tiles.ShadowBiome;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Armor.Shadow
{
	internal static class ShadowArmorCommon
	{
		public static void AddArmorRecipe(ModItem item, int shadowOreAmount, int shadowScrapAmount, int soulAmount, int rosalitaAmount) {
			item.CreateRecipe()
				.AddIngredient<ShadowOre>(shadowOreAmount)
				.AddIngredient<ShadowScrap>(shadowScrapAmount)
				.AddIngredient<SoulOfShadow>(soulAmount)
				.AddIngredient<RosalitaGem>(rosalitaAmount)
				.AddTile(ModContent.TileType<GodnessAnvilTile>())
				.Register();
		}

		public static void ApplySetBonuses(Player player) {
			player.statDefense += 70;
			player.GetDamage(DamageClass.Generic) += 0.38f;
			player.GetCritChance(DamageClass.Generic) += 22f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.25f;
			player.GetArmorPenetration(DamageClass.Generic) += 38;
			player.maxMinions += 5;
			player.whipRangeMultiplier += 0.28f;
			player.statManaMax2 += 220;
			player.moveSpeed += 0.18f;
			player.lifeRegen += 12;
			player.endurance += 0.12f;
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public sealed class ShadowHelmet : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/Armor/Shadow/ShadowHelmet";

		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.buyPrice(platinum: 1);
			Item.rare = ItemRarityID.Red;
			Item.defense = 52;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<ShadowBreastplate>() && legs.type == ModContent.ItemType<ShadowGreaves>();
		}

		public override void UpdateArmorSet(Player player) {
			player.setBonus = "The shadows crush every class at once";
			ShadowArmorCommon.ApplySetBonuses(player);
		}

		public override void UpdateEquip(Player player) {
			player.GetDamage(DamageClass.Generic) += 0.14f;
			player.GetCritChance(DamageClass.Generic) += 8f;
		}

		public override void AddRecipes() {
			ShadowArmorCommon.AddArmorRecipe(this, 48, 18, 8, 14);
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public sealed class ShadowBreastplate : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/Armor/Shadow/ShadowBreastplate";

		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 26;
			Item.value = Item.buyPrice(platinum: 1);
			Item.rare = ItemRarityID.Red;
			Item.defense = 70;
		}

		public override void UpdateEquip(Player player) {
			player.statLifeMax2 += 160;
			player.statManaMax2 += 80;
			player.maxMinions += 2;
			player.endurance += 0.08f;
		}

		public override void AddRecipes() {
			ShadowArmorCommon.AddArmorRecipe(this, 72, 24, 12, 20);
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public sealed class ShadowGreaves : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/Armor/Shadow/ShadowGreaves";

		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 18;
			Item.value = Item.buyPrice(platinum: 1);
			Item.rare = ItemRarityID.Red;
			Item.defense = 46;
		}

		public override void UpdateEquip(Player player) {
			player.moveSpeed += 0.18f;
			player.maxRunSpeed += 1.8f;
			player.runAcceleration *= 1.24f;
			player.lifeRegen += 4;
		}

		public override void AddRecipes() {
			ShadowArmorCommon.AddArmorRecipe(this, 56, 18, 8, 16);
		}
	}
}
