using ChaoticDimensions.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Weapons.Ranged
{
	public sealed class MonthraBow : ModItem
	{
		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 54;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 21;
			Item.useTime = 21;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 24;
			Item.knockBack = 2.5f;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 10.25f;
			Item.useAmmo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item5;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 1);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 firstArrowVelocity = velocity.RotatedBy(MathHelper.ToRadians(-4f)) * 0.96f;
			Vector2 secondArrowVelocity = velocity.RotatedBy(MathHelper.ToRadians(4f)) * 1.04f;

			Projectile.NewProjectile(source, position, firstArrowVelocity, type, damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, secondArrowVelocity, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<MonthraScale>(10)
				.AddIngredient(ItemID.DemoniteBar, 12)
				.AddIngredient(ItemID.ShadowScale, 3)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient<MonthraScale>(10)
				.AddIngredient(ItemID.CrimtaneBar, 12)
				.AddIngredient(ItemID.TissueSample, 3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
