using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.Projectiles.Melee;

namespace ChaoticDimensions.Content.Items.Weapons.Melee
{
	public sealed class CrystalineSword : ModItem
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.FinalFractal}";

		public override void SetDefaults() {
			Item.width = 60;
			Item.height = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 1312;
			Item.knockBack = 6f;
			Item.crit = 99;
			Item.shoot = ModContent.ProjectileType<CrystalineSwordProjectile>();
			Item.shootSpeed = 28f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Purple;
			Item.value = Item.buyPrice(platinum: 2);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 baseDirection = velocity.SafeNormalize(Vector2.UnitX);
			Vector2 normal = baseDirection.RotatedBy(MathHelper.PiOver2);
			Vector2 centerSpawnPosition = player.MountedCenter + (baseDirection * 34f);
			Vector2 swordVelocity = baseDirection * Item.shootSpeed;

			int centerProjectileIndex = Projectile.NewProjectile(source, centerSpawnPosition, swordVelocity, type, damage, knockback, player.whoAmI);
			if (centerProjectileIndex >= 0 && centerProjectileIndex < Main.maxProjectiles) {
				Projectile centerProjectile = Main.projectile[centerProjectileIndex];
				float centerIdentity = centerProjectile.identity;

				Projectile.NewProjectile(source, centerSpawnPosition + (normal * CrystalineSwordProjectile.OrbitRadius), swordVelocity, type, damage, knockback, player.whoAmI, centerIdentity, 1f);
				Projectile.NewProjectile(source, centerSpawnPosition - (normal * CrystalineSwordProjectile.OrbitRadius), swordVelocity, type, damage, knockback, player.whoAmI, centerIdentity, -1f);
			}

			return false;
		}

	}
}
