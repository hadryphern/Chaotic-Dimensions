using ChaoticDimensions.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Magic
{
	public sealed class ShadowBoltProjectile : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Magic/ShadowBoltProjectile";

		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 6;
			Projectile.timeLeft = 180;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
		}

		public override void AI() {
			Projectile.localAI[0]++;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.velocity = Projectile.velocity.RotatedBy(System.Math.Sin(Projectile.localAI[0] * 0.15f) * 0.024f);

			NPC target = FindTarget(1200f);
			if (target is not null) {
				Vector2 desiredVelocity = Projectile.DirectionTo(target.Center) * Projectile.velocity.Length();
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.09f);
			}

			Lighting.AddLight(Projectile.Center, 0.6f, 0.08f, 0.66f);
			Dust.NewDustPerfect(Projectile.Center, DustID.Shadowflame, Projectile.velocity * -0.08f, 150, default, 1.2f).noGravity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			ShadowCombatHelper.ApplyRend(target, Projectile.owner, healAmount: 18);
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
