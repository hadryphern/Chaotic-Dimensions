using ChaoticDimensions.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Summon
{
	public sealed class ShadowCrystalMinion : ModProjectile
	{
		private int shotTimer;

		public override string Texture => "ChaoticDimensions/Content/Projectiles/Summon/ShadowCrystalMinion";

		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
			Main.projPet[Type] = true;
			ProjectileID.Sets.MinionSacrificable[Type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 30;
			Projectile.height = 42;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 18000;
			Projectile.DamageType = DamageClass.Summon;
		}

		public override bool MinionContactDamage() => false;

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (!player.active || player.dead) {
				player.ClearBuff(ModContent.BuffType<ShadowCrystalMinionBuff>());
				return;
			}

			if (player.HasBuff(ModContent.BuffType<ShadowCrystalMinionBuff>())) {
				Projectile.timeLeft = 2;
			}

			Animate(5);

			NPC target = HappyCreeperMinion.FindTarget(player, 1100f);
			Vector2 idlePosition = player.Center + new Vector2((Projectile.minionPos + 1) * -54f * player.direction, -84f + (Projectile.minionPos % 3) * 18f);
			Vector2 desiredVelocity = target is not null
				? (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY) * 12.5f
				: (idlePosition - Projectile.Center).SafeNormalize(Vector2.Zero) * 8.5f;

			Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, target is not null ? 0.12f : 0.08f);
			Projectile.rotation = Projectile.velocity.X * 0.03f;

			if (target is not null) {
				shotTimer++;
				if (shotTimer >= 16 && Projectile.owner == Main.myPlayer) {
					shotTimer = 0;
					Vector2 boltVelocity = Projectile.DirectionTo(target.Center) * 17f;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, boltVelocity, ModContent.ProjectileType<ShadowCrystalBolt>(), Projectile.damage, 2f, Projectile.owner);
				}
			}
			else {
				shotTimer = 0;
			}

			Lighting.AddLight(Projectile.Center, 0.34f, 0.08f, 0.46f);
		}

		private void Animate(int speed) {
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= speed) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
			}
		}
	}

	public sealed class ShadowCrystalBolt : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Summon/ShadowCrystalBolt";

		public override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
		}

		public override void AI() {
			Projectile.rotation += 0.32f;
			Lighting.AddLight(Projectile.Center, 0.45f, 0.1f, 0.55f);
			if (Main.rand.NextBool(2)) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.velocity.X * -0.15f, Projectile.velocity.Y * -0.15f);
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			ShadowCombatHelper.ApplyRend(target, Projectile.owner, healAmount: 14);
		}
	}
}
