// Implementa tres comportamentos de summon sem exigir sprites definitivas.

using ChaoticDimensions.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Progression
{
	public sealed class ProgressionMinionProjectile : ModProjectile
	{
		private int Mode => (int)Projectile.ai[0];
		private int Tier => (int)Projectile.ai[1];

		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.BabySlime}";

		public override void SetStaticDefaults() {
			Main.projPet[Type] = true;
			ProjectileID.Sets.MinionSacrificable[Type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 30;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 18;
		}

		public override bool MinionContactDamage() => Mode != 2;

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (!player.active || player.dead) {
				player.ClearBuff(ModContent.BuffType<ProgressionMinionBuff>());
				return;
			}
			if (player.HasBuff(ModContent.BuffType<ProgressionMinionBuff>())) Projectile.timeLeft = 2;

			NPC target = FindTarget(player);
			Vector2 idle = player.Center + new Vector2((Projectile.minionPos + 1) * -48f * player.direction, -70f - Mode * 18f);
			if (target is null) {
				MoveTowards(idle, 8f + Tier * 0.35f, 0.09f);
			}
			else if (Mode == 0) {
				Vector2 orbit = target.Center + new Vector2(110f + Tier * 2f, 0f).RotatedBy(Main.GameUpdateCount * 0.04f + Projectile.identity);
				MoveTowards(orbit, 11f + Tier * 0.45f, 0.12f);
			}
			else if (Mode == 2) {
				Vector2 hover = target.Center + new Vector2(0f, -180f);
				MoveTowards(hover, 10f + Tier * 0.35f, 0.1f);
				Projectile.localAI[0]++;
				if (Projectile.localAI[0] >= System.Math.Max(12, 48 - Tier * 2) && Main.myPlayer == Projectile.owner) {
					Projectile.localAI[0] = 0f;
					Vector2 velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * (10f + Tier * 0.5f);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<ProgressionWeaponProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 20f, Tier);
				}
			}
			else {
				MoveTowards(target.Center, 15f + Tier * 0.55f, 0.15f);
			}
			Projectile.rotation = Projectile.velocity.X * 0.04f;
		}

		private void MoveTowards(Vector2 target, float speed, float turn) {
			Vector2 desired = (target - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, turn);
			if (Vector2.DistanceSquared(Projectile.Center, target) > 1500f * 1500f) Projectile.Center = target;
		}

		private NPC FindTarget(Player player) {
			if (player.HasMinionAttackTargetNPC) {
				NPC focus = Main.npc[player.MinionAttackTargetNPC];
				if (focus.CanBeChasedBy(Projectile)) return focus;
			}
			NPC target = null;
			float distance = 800f + Tier * 25f;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (!npc.CanBeChasedBy(Projectile)) continue;
				float current = Vector2.Distance(Projectile.Center, npc.Center);
				if (current < distance) {
					distance = current;
					target = npc;
				}
			}
			return target;
		}
	}
}
