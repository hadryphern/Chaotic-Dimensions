using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Magic
{
	public sealed class CrystalineBoltProjectile : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.MagicMissile}";

		public override void SetDefaults() {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 150;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
		}

		public override void AI() {
			Projectile.localAI[0]++;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.velocity = Projectile.velocity.RotatedBy(Math.Sin(Projectile.localAI[0] * 0.16f) * 0.018f);

			NPC target = FindTarget(900f);
			if (target != null) {
				Vector2 desiredVelocity = Projectile.DirectionTo(target.Center) * Projectile.velocity.Length();
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.05f);
			}

			Lighting.AddLight(Projectile.Center, 0.75f, 0.2f, 0.85f);
			Dust.NewDustPerfect(Projectile.Center, DustID.PinkTorch, Projectile.velocity * -0.08f, 150, default, 1.1f).noGravity = true;
		}

		private NPC FindTarget(float range) {
			NPC bestTarget = null;
			float bestDistance = range;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (!npc.CanBeChasedBy(Projectile)) {
					continue;
				}

				float distance = Projectile.Distance(npc.Center);
				if (distance < bestDistance) {
					bestDistance = distance;
					bestTarget = npc;
				}
			}

			return bestTarget;
		}
	}
}
