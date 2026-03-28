using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Summon
{
	public sealed class HappyCreeperMinion : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Summon/HappyCreeperMinion";

		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
			Main.projPet[Type] = true;
			ProjectileID.Sets.MinionSacrificable[Type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 18000;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 18;
		}

		public override bool MinionContactDamage() => true;

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (!player.active || player.dead) {
				player.ClearBuff(ModContent.BuffType<Buffs.HappyCreeperMinionBuff>());
				return;
			}

			if (player.HasBuff(ModContent.BuffType<Buffs.HappyCreeperMinionBuff>())) {
				Projectile.timeLeft = 2;
			}

			Animate(6);

			NPC target = FindTarget(player, 760f);
			if (target is not null) {
				Vector2 desiredVelocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitY) * 13.5f;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.13f);
			}
			else {
				Vector2 idlePosition = player.Center + new Vector2((Projectile.minionPos + 1) * -42f * player.direction, -54f);
				Vector2 toIdle = idlePosition - Projectile.Center;
				Vector2 desiredVelocity = toIdle.SafeNormalize(Vector2.Zero) * (toIdle.Length() > 420f ? 10f : 6f);
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.08f);
			}

			Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;
			Projectile.rotation = Projectile.velocity.X * 0.04f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			SoundEngine.PlaySound(SoundID.Item14 with { Pitch = 0.1f, Volume = 0.65f }, Projectile.Center);
			for (int i = 0; i < 12; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
			}
		}

		private void Animate(int speed) {
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= speed) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
			}
		}

		internal static NPC FindTarget(Player player, float maxDistance) {
			if (player.HasMinionAttackTargetNPC) {
				NPC focus = Main.npc[player.MinionAttackTargetNPC];
				if (focus.CanBeChasedBy()) {
					return focus;
				}
			}

			NPC target = null;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (!npc.CanBeChasedBy()) {
					continue;
				}

				float distance = Vector2.Distance(player.Center, npc.Center);
				if (distance <= maxDistance && (target is null || distance < Vector2.Distance(player.Center, target.Center))) {
					target = npc;
				}
			}

			return target;
		}
	}

	public sealed class SquidKrakenMinion : ModProjectile
	{
		private int shotTimer;

		public override string Texture => "ChaoticDimensions/Content/Projectiles/Summon/SquidKrakenMinion";

		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 2;
			Main.projPet[Type] = true;
			ProjectileID.Sets.MinionSacrificable[Type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.width = 34;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 18000;
			Projectile.DamageType = DamageClass.Summon;
		}

		public override bool? CanCutTiles() => false;

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			if (!player.active || player.dead) {
				player.ClearBuff(ModContent.BuffType<Buffs.SquidKrakenMinionBuff>());
				return;
			}

			if (player.HasBuff(ModContent.BuffType<Buffs.SquidKrakenMinionBuff>())) {
				Projectile.timeLeft = 2;
			}

			Animate(10);

			NPC target = HappyCreeperMinion.FindTarget(player, 900f);
			Vector2 idlePosition = player.Center + new Vector2((Projectile.minionPos + 1) * -52f * player.direction, -38f);
			Vector2 desiredVelocity = target is not null
				? (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 9.5f
				: (idlePosition - Projectile.Center).SafeNormalize(Vector2.Zero) * 6.5f;

			Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.09f);
			Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;

			if (target is not null) {
				shotTimer++;
				if (shotTimer >= 18 && Projectile.owner == Main.myPlayer) {
					shotTimer = 0;
					Vector2 velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 13f;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<SquidKrakenMinionWaterBolt>(), Projectile.damage, 0f, Projectile.owner);
				}
			}
			else {
				shotTimer = 0;
			}
		}

		private void Animate(int speed) {
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= speed) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
			}
		}
	}

	public sealed class SquidKrakenMinionWaterBolt : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Hostile/SquidKrakenWaterBolt";

		public override void SetDefaults() {
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 90;
			Projectile.extraUpdates = 1;
		}

		public override void AI() {
			Projectile.rotation += 0.18f;
			if (Main.rand.NextBool(3)) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water);
			}
		}
	}
}
