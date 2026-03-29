using ChaoticDimensions.Content.Bosses.Monthra;
using ChaoticDimensions.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.Critters
{
	public sealed class MonthraButterfly : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 6;
			NPCID.Sets.CountsAsCritter[Type] = true;
		}

		public override void SetDefaults() {
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 20;
			NPC.noGravity = true;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0.5f;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 0f;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (!NPC.downedBoss2 || spawnInfo.PlayerSafe || spawnInfo.Invasion || NPC.AnyNPCs(ModContent.NPCType<MonthraBoss>())) {
				return 0f;
			}

			if (!(spawnInfo.Player.ZoneOverworldHeight || spawnInfo.Player.ZoneSkyHeight)) {
				return 0f;
			}

			return Main.dayTime ? 0.012f : 0.018f;
		}

		public override void AI() {
			NPC.TargetClosest(false);

			NPC.localAI[0]++;
			if (NPC.localAI[0] >= NPC.ai[0] || NPC.ai[0] <= 0f) {
				NPC.ai[0] = Main.rand.Next(55, 130);
				NPC.ai[1] = Main.rand.NextFloat(-2.4f, 2.4f);
				NPC.ai[2] = Main.rand.NextFloat(-1.2f, 1.2f);
				NPC.netUpdate = true;
				NPC.localAI[0] = 0f;
			}

			float verticalBob = (float)System.Math.Sin(Main.GameUpdateCount * 0.075f + NPC.whoAmI) * 0.4f;
			Vector2 desiredVelocity = new Vector2(NPC.ai[1], NPC.ai[2] + verticalBob);
			NPC.velocity = Vector2.Lerp(NPC.velocity, desiredVelocity, 0.05f);

			if (NPC.collideX) {
				NPC.ai[1] *= -1f;
			}

			if (NPC.collideY) {
				NPC.ai[2] = -(float)System.Math.Abs(NPC.ai[2]);
			}

			NPC.spriteDirection = NPC.velocity.X >= 0f ? 1 : -1;
			NPC.rotation = NPC.velocity.X * 0.08f;
		}

		public override void FindFrame(int frameHeight) {
			NPC.frameCounter++;
			if (NPC.frameCounter >= 6) {
				NPC.frameCounter = 0;
				NPC.frame.Y += frameHeight;
				if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[Type]) {
					NPC.frame.Y = 0;
				}
			}
		}

		public override void OnKill() {
			if (NPC.AnyNPCs(ModContent.NPCType<MonthraBoss>()) || MonthraIntroSystem.IsActive) {
				return;
			}

			int targetPlayer = Player.FindClosest(NPC.position, NPC.width, NPC.height);
			if (Main.netMode == NetmodeID.Server) {
				return;
			}

			if (Main.myPlayer == targetPlayer) {
				MonthraIntroSystem.StartIntro(Main.player[targetPlayer]);
			}
		}
	}
}
