using ChaoticDimensions.Content.Buffs;
using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Weapons.Summon
{
	public sealed class MonthraButterflyStaff : ModItem
	{
		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 34;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.damage = 18;
			Item.knockBack = 2f;
			Item.mana = 10;
			Item.buffType = ModContent.BuffType<MonthraButterflyBuff>();
			Item.shoot = ModContent.ProjectileType<MonthraButterflyMinion>();
			Item.shootSpeed = 10f;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 1);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2);
			Vector2 spawnPosition = Main.MouseWorld;
			Projectile.NewProjectile(source, spawnPosition, Vector2.Zero, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<MonthraScale>(12)
				.AddIngredient(ItemID.DemoniteBar, 10)
				.AddIngredient(ItemID.ShadowScale, 5)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient<MonthraScale>(12)
				.AddIngredient(ItemID.CrimtaneBar, 10)
				.AddIngredient(ItemID.TissueSample, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
