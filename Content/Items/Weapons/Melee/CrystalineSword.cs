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
			for (int i = -1; i <= 1; i++) {
				Vector2 spawnOffset = baseDirection.RotatedBy(MathHelper.PiOver2) * (90f * i);
				Vector2 spawnPosition = player.MountedCenter - baseDirection * 80f + spawnOffset;
				Vector2 swordVelocity = baseDirection.RotatedBy(MathHelper.ToRadians(8f * i)) * Item.shootSpeed;
				Projectile.NewProjectile(source, spawnPosition, swordVelocity, type, damage, knockback, player.whoAmI);
			}

			return false;
		}

	}
}
