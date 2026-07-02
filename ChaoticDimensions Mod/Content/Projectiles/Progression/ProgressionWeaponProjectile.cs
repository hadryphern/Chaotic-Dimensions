// Partilha a logica dos ataques de melee, magic e dos tiros dos minions.

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Progression
{
	public sealed class ProgressionWeaponProjectile : ModProjectile
	{
		private int Mode => (int)Projectile.ai[0];
		private int Tier => (int)Projectile.ai[1];

		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.AmethystBolt}";

		public override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 240;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
		}

		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Mode < 10) {
				Projectile.DamageType = DamageClass.Melee;
				if (Mode == 1) Projectile.velocity *= 1.012f;
				else if (Projectile.timeLeft < 205) ReturnToOwner();
			}
			else if (Mode < 20) {
				Projectile.DamageType = DamageClass.Magic;
				if (Mode == 11) HomeTowardsTarget(0.065f, 12f + Tier * 0.8f);
				else if (Mode == 12) Projectile.velocity = Projectile.velocity.RotatedBy(0.018f);
			}
			else {
				Projectile.DamageType = DamageClass.Summon;
				HomeTowardsTarget(0.08f, 10f + Tier * 0.7f);
			}
			Lighting.AddLight(Projectile.Center, 0.08f, 0.12f, 0.2f);
		}

		private void ReturnToOwner() {
			Player owner = Main.player[Projectile.owner];
			Vector2 desired = (owner.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * (13f + Tier);
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, 0.12f);
			Projectile.tileCollide = false;
			if (Vector2.DistanceSquared(Projectile.Center, owner.Center) < 28f * 28f) Projectile.Kill();
		}

		private void HomeTowardsTarget(float turnRate, float speed) {
			NPC target = null;
			float distance = 900f;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (!npc.CanBeChasedBy(Projectile)) continue;
				float current = Vector2.Distance(Projectile.Center, npc.Center);
				if (current < distance) {
					distance = current;
					target = npc;
				}
			}
			if (target is null) return;
			Vector2 desired = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * speed;
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, turnRate);
		}
	}
}
