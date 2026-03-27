using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Weapons.Melee
{
	public sealed class MonthraBlade : ModItem
	{
		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 24;
			Item.useTime = 24;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 29;
			Item.knockBack = 5.75f;
			Item.shoot = ModContent.ProjectileType<MonthraBladeProjectile>();
			Item.shootSpeed = 1f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 1);
		}

		public override bool CanUseItem(Player player) {
			return player.ownedProjectileCounts[Item.shoot] == 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			float startAngle = velocity.SafeNormalize(Vector2.UnitX).ToRotation() - MathHelper.Pi;
			Projectile.NewProjectile(source, player.MountedCenter, Vector2.Zero, type, damage, knockback, player.whoAmI, startAngle);
			return false;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<MonthraScale>(12)
				.AddIngredient(ItemID.DemoniteBar, 14)
				.AddIngredient(ItemID.ShadowScale, 4)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient<MonthraScale>(12)
				.AddIngredient(ItemID.CrimtaneBar, 14)
				.AddIngredient(ItemID.TissueSample, 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
