using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Summon
{
	public sealed class MonthraButterflyMinion : ModProjectile
	{
		private const float IdleSpacing = 46f;
		private const float FollowDistance = 720f;

		public override string Texture => "ChaoticDimensions/Content/NPCs/Critters/MonthraButterfly";

		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 5;
			Main.projPet[Type] = true;
			ProjectileID.Sets.MinionSacrificable[Type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 40;
			Projectile.height = 36;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 0.5f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 18000;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 16;
		}

		public override bool? CanCutTiles() => false;

		public override bool MinionContactDamage() => true;

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (!player.active || player.dead) {
				player.ClearBuff(ModContent.BuffType<Buffs.MonthraButterflyBuff>());
				return;
			}

			if (player.HasBuff(ModContent.BuffType<Buffs.MonthraButterflyBuff>())) {
				Projectile.timeLeft = 2;
			}

			Animate();

			Vector2 idleOffset = new Vector2((Projectile.minionPos + 1) * IdleSpacing * -player.direction, -64f - ((Projectile.minionPos % 2) * 14f));
			Vector2 idlePosition = player.Center + idleOffset;
			Vector2 vectorToIdle = idlePosition - Projectile.Center;
			if (vectorToIdle.Length() > 1400f) {
				Projectile.Center = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}

			NPC target = FindTarget(player);
			if (target is not null) {
				Vector2 desiredVelocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 11f;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.1f);
			}
			else {
				float speed = vectorToIdle.Length() > FollowDistance ? 12f : 7f;
				Vector2 desiredVelocity = vectorToIdle == Vector2.Zero ? Vector2.Zero : vectorToIdle.SafeNormalize(Vector2.Zero) * speed;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.08f);
				if (vectorToIdle.Length() < 40f) {
					Projectile.velocity *= 0.95f;
				}
			}

			Projectile.rotation = Projectile.velocity.X * 0.06f;
			Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;
		}

		private void Animate() {
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
			}
		}

		private NPC FindTarget(Player player) {
			NPC target = null;
			float closestDistance = 620f;

			if (player.HasMinionAttackTargetNPC) {
				NPC focus = Main.npc[player.MinionAttackTargetNPC];
				if (focus.CanBeChasedBy(Projectile)) {
					return focus;
				}
			}

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

			return target;
		}
	}
}
