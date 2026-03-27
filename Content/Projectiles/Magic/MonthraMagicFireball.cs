using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Magic
{
	public sealed class MonthraMagicFireball : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Hostile/MonthraFireball";

		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
			ProjectileID.Sets.TrailCacheLength[Type] = 8;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 200;
			Projectile.light = 0.75f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 12;
		}

		public override void AI() {
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 4) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Projectile.timeLeft < 140) {
				AdjustTowardsTarget();
			}

			if (Main.rand.NextBool(2)) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy, Projectile.velocity.X * 0.18f, Projectile.velocity.Y * 0.18f, 90, default, 1.15f);
				Main.dust[dust].noGravity = true;
			}
		}

		private void AdjustTowardsTarget() {
			NPC target = null;
			float closestDistance = 540f;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (!npc.CanBeChasedBy(Projectile)) {
					continue;
				}

				float distance = Vector2.Distance(Projectile.Center, npc.Center);
				if (distance < closestDistance && Collision.CanHitLine(Projectile.Center, 0, 0, npc.Center, 0, 0)) {
					closestDistance = distance;
					target = npc;
				}
			}

			if (target is null) {
				return;
			}

			Vector2 desiredVelocity = (target.Center - Projectile.Center).SafeNormalize(Projectile.velocity) * 10.5f;
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.08f);
		}

		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
			for (int i = 0; i < 12; i++) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 90, default, 1.35f);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
