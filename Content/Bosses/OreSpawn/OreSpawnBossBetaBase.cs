using ChaoticDimensions.Common.OreSpawn;
using ChaoticDimensions.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Bosses.OreSpawn
{
	public abstract class OreSpawnBossBetaBase : ModNPC
	{
		protected abstract string DefinitionKey { get; }
		protected OreSpawnBossDefinition Definition => OreSpawnMobCatalog.GetBoss(DefinitionKey);

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Definition.RecommendedFrameCount;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		public override void SetDefaults() {
			NPC.width = Definition.Width;
			NPC.height = Definition.Height;
			NPC.damage = Definition.Damage;
			NPC.defense = Definition.Defense;
			NPC.lifeMax = Definition.LifeMax;
			NPC.knockBackResist = 0f;
			NPC.value = Definition.Value;
			NPC.npcSlots = 12f;
			NPC.boss = true;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.rarity = Definition.Rarity;
			Music = Definition.MusicId;
		}

		public override void AI() {
			NPC.TargetClosest(faceTarget: true);
			Player player = Main.player[NPC.target];

			if (!player.active || player.dead) {
				NPC.velocity.Y -= 0.35f;
				if (NPC.timeLeft > 15) {
					NPC.timeLeft = 15;
				}
				return;
			}

			Vector2 toTarget = player.Center - NPC.Center;
			float hoverDistance = Definition.Archetype == OreSpawnBossArchetype.Bruiser ? 180f : 260f;
			Vector2 desiredPosition = player.Center + new Vector2(player.direction * -hoverDistance, -150f);
			Vector2 move = desiredPosition - NPC.Center;
			float speed = Definition.Archetype == OreSpawnBossArchetype.Bruiser ? 11f : 14f;
			if (Definition.Archetype == OreSpawnBossArchetype.AerialSweep) {
				speed = 16f;
			}

			if (NPC.ai[0]++ % 180f < 120f) {
				Vector2 desiredVelocity = move.SafeNormalize(Vector2.Zero) * speed;
				NPC.velocity = Vector2.Lerp(NPC.velocity, desiredVelocity, 0.08f);
			}
			else {
				Vector2 desiredVelocity = toTarget.SafeNormalize(Vector2.UnitY) * (speed + 5f);
				NPC.velocity = Vector2.Lerp(NPC.velocity, desiredVelocity, 0.18f);
			}

			if (Definition.Archetype == OreSpawnBossArchetype.RoyalCaster && NPC.ai[0] % 150f == 0f && Main.netMode != NetmodeID.MultiplayerClient) {
				Vector2 spawn = player.Center + new Vector2(Main.rand.NextFloat(-220f, 220f), -400f);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, Vector2.UnitY * 10f, ProjectileID.MagicMissile, Definition.Damage / 4, 1f, Main.myPlayer);
			}

			if (Definition.Archetype == OreSpawnBossArchetype.TentacleHover && NPC.ai[0] % 120f == 0f && Main.netMode != NetmodeID.MultiplayerClient) {
				Vector2 spawn = NPC.Center + new Vector2(Main.rand.NextFloat(-60f, 60f), 0f);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, Vector2.Normalize(toTarget) * 8f, ProjectileID.SharknadoBolt, Definition.Damage / 5, 1f, Main.myPlayer);
			}

			NPC.direction = player.Center.X > NPC.Center.X ? 1 : -1;
			NPC.spriteDirection = NPC.direction;
		}

		public override void OnKill() {
			OreSpawnDownedBossSystem.MarkDowned(DefinitionKey);
		}
	}
}
