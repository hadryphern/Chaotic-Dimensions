using ChaoticDimensions.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Ranged
{
	public sealed class ShadowArrowProjectile : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Ranged/ShadowArrowProjectile";

		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = ProjAIStyleID.Arrow;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override void AI() {
			Lighting.AddLight(Projectile.Center, 0.48f, 0.08f, 0.44f);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			ShadowCombatHelper.ApplyRend(target, Projectile.owner, healAmount: 12);
		}
	}

	public sealed class ShadowBulletProjectile : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Ranged/ShadowBulletProjectile";

		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 240;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
		}

		public override void AI() {
			NPC target = FindTarget(900f);
			if (target is not null) {
				Vector2 desiredVelocity = Projectile.DirectionTo(target.Center) * Projectile.velocity.Length();
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.08f);
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Lighting.AddLight(Projectile.Center, 0.4f, 0.08f, 0.52f);
			if (Main.rand.NextBool(2)) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 120, default, 1.05f);
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			ShadowCombatHelper.ApplyRend(target, Projectile.owner, healAmount: 12);
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
