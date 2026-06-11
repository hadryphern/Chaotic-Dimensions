using ChaoticDimensions.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.Kraken
{
	public class KrakenClone : ModNPC
	{
		private int OwnerIndex => (int)NPC.ai[0];
		private ref float Timer => ref NPC.localAI[0];
		private ref float Phase2LifeApplied => ref NPC.localAI[1];

		public override string Texture => "ChaoticDimensions/Content/NPCs/Kraken/KrakenBoss";

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}

		public override void SetDefaults() {
			NPC.width = 210;
			NPC.height = 420;
			NPC.damage = 280;
			NPC.defense = 8;
			NPC.lifeMax = 200000;
			NPC.knockBackResist = 0.05f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath13;
			NPC.value = 0f;
		}

		public override void AI() {
			Timer++;
			NPC.TargetClosest();
			Player player = Main.player[NPC.target];

			if (!OwnerAlive() || player.dead || !player.active) {
				NPC.velocity.Y -= 0.3f;
				NPC.EncourageDespawn(20);
				return;
			}

			NPC owner = Main.npc[OwnerIndex];
			ApplyPhase2Life(owner);
			float size = MathHelper.Clamp(NPC.ai[1], 0.42f, 0.82f);
			float angle = NPC.ai[2] + Timer * (0.012f + size * 0.004f);
			Vector2 orbit = new Vector2((float)System.Math.Cos(angle) * 620f, (float)System.Math.Sin(angle) * 340f);
			Vector2 desiredPosition = owner.Center + orbit;
			NPC.Center = Vector2.Lerp(NPC.Center, desiredPosition, 0.16f);
			NPC.velocity = Vector2.Zero;
			NPC.rotation = (float)System.Math.Sin(angle) * 0.08f;
			NPC.frameCounter = (NPC.frameCounter + 0.34f) % KrakenBoss.LoopAnimationFrames;

			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (Timer % 116f == 30f) {
					FireDropFan(player);
				}

				if (Timer % 280f == 116f) {
					Vector2 predicted = player.Center + player.velocity * 32f;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), predicted + new Vector2(Main.rand.NextFloat(-180f, 180f), 0f), Vector2.Zero, ModContent.ProjectileType<KrakenSkyBeam>(), 240, 0f, Main.myPlayer, 0f, Main.rand.NextFloat(-0.08f, 0.08f));
				}
			}

		}

		private void FireDropFan(Player player) {
			Vector2 origin = NPC.Center + new Vector2(0f, -140f);
			Vector2 aim = player.Center + player.velocity * 28f - origin;
			if (aim.LengthSquared() < 4f) {
				aim = Vector2.UnitY;
			}
			else {
				aim.Normalize();
			}

			for (int i = 0; i < 5; i++) {
				float spread = MathHelper.Lerp(-0.38f, 0.38f, i / 4f);
				Vector2 velocity = aim.RotatedBy(spread) * Main.rand.NextFloat(9.5f, 12.5f);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), origin + velocity * 5f, velocity, ModContent.ProjectileType<KrakenWaterDrop>(), 128, 0f, Main.myPlayer, 0f, 1f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Texture2D forwardTexture = TextureAssets.Npc[Type].Value;
			Texture2D loopBackTexture = ModContent.Request<Texture2D>(KrakenBoss.LoopBackTexturePath).Value;
			KrakenBoss.GetAnimationFrame(forwardTexture, loopBackTexture, (int)NPC.frameCounter, out Texture2D texture, out Rectangle source);
			Vector2 origin = source.Size() * 0.5f;
			float scale = KrakenBoss.BaseVisualScale * MathHelper.Clamp(NPC.ai[1], 0.42f, 0.82f);
			Color shade = new Color(22, 22, 30, 112);

			Vector2 center = NPC.Center + new Vector2(0f, KrakenBoss.VisualDrawOffsetY * 0.68f) - screenPos;
			spriteBatch.Draw(texture, center, source, shade, NPC.rotation, origin, scale, SpriteEffects.None, 0f);
			return false;
		}

		private bool OwnerAlive() {
			if (OwnerIndex < 0 || OwnerIndex >= Main.maxNPCs) {
				return false;
			}

			NPC owner = Main.npc[OwnerIndex];
			return owner.active && owner.type == ModContent.NPCType<KrakenBoss>();
		}

		private void ApplyPhase2Life(NPC owner) {
			if (Phase2LifeApplied == 1f || owner.lifeMax <= 0 || owner.life > owner.lifeMax * 0.5f || Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			NPC.lifeMax *= 2;
			NPC.life = System.Math.Min(NPC.lifeMax, NPC.life * 2);
			Phase2LifeApplied = 1f;
			NPC.netUpdate = true;
		}
	}
}
