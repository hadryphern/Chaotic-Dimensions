using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Melee
{
	public sealed class MonthraBladeProjectile : ModProjectile
	{
		private const int SpinDuration = 24;
		private const float SpinRadius = 92f;

		private ref float StartAngle => ref Projectile.ai[0];

		public override string Texture => "ChaoticDimensions/Content/Items/Weapons/Melee/MonthraBlade";

		public override void SetDefaults() {
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = SpinDuration;
			Projectile.ownerHitCheck = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override bool ShouldUpdatePosition() => false;

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (!player.active || player.dead) {
				Projectile.Kill();
				return;
			}

			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;

			float progress = 1f - (Projectile.timeLeft / (float)SpinDuration);
			float spinDirection = player.direction >= 0 ? 1f : -1f;
			float angle = StartAngle + (MathHelper.TwoPi * spinDirection * progress);
			Vector2 offset = angle.ToRotationVector2() * SpinRadius;

			Projectile.Center = player.MountedCenter + offset;
			Projectile.rotation = angle + MathHelper.PiOver4;
			Projectile.spriteDirection = offset.X >= 0f ? 1 : -1;
			Projectile.velocity = offset.SafeNormalize(Vector2.UnitX) * 0.1f;

			player.ChangeDir(Projectile.spriteDirection);
			player.itemRotation = (Projectile.Center - player.MountedCenter).ToRotation();

			Lighting.AddLight(Projectile.Center, 0.2f, 0.55f, 0.25f);
		}
	}
}
