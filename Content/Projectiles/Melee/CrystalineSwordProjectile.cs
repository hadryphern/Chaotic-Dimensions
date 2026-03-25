using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Melee
{
	public sealed class CrystalineSwordProjectile : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.FinalFractal}";

		public override void SetDefaults() {
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 40;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
		}

		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			NPC target = FindTarget(900f);
			if (target != null) {
				Vector2 desiredVelocity = Projectile.DirectionTo(target.Center) * 28f;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.14f);
			}

			if (Projectile.timeLeft < 16) {
				Projectile.alpha += 14;
			}

			Lighting.AddLight(Projectile.Center, 0.8f, 0.3f, 0.9f);
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
