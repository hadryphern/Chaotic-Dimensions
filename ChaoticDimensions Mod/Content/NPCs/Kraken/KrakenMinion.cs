// Implementa a IA, animacao e regras dos NPCs de Kraken Minion.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.Kraken
{
	public class KrakenMinion : ModNPC
	{
		private const int AtlasColumns = 6;
		private const int AtlasRows = 6;
		private const int AnimationFrames = AtlasColumns * AtlasRows;
		private int OwnerIndex => (int)NPC.ai[0];
		private ref float Timer => ref NPC.localAI[0];
		private ref float Phase2LifeApplied => ref NPC.localAI[1];

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1;
		}

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			NPC.width = 21;
			NPC.height = 21;
			NPC.damage = 150;
			NPC.defense = 18;
			NPC.lifeMax = 22000;
			NPC.knockBackResist = 0.15f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath11;
			NPC.value = 0f;
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Timer++;
			NPC.TargetClosest();
			Player player = Main.player[NPC.target];

			if (!OwnerAlive()) {
				NPC.velocity.Y -= 0.25f;
				NPC.EncourageDespawn(30);
				return;
			}

			NPC owner = Main.npc[OwnerIndex];
			ApplyPhase2Life(owner);
			Vector2 desired = player.Center - NPC.Center;
			float distance = desired.Length();
			if (distance > 1f) {
				desired.Normalize();
			}

			float speed = Timer % 170f > 120f ? 19.5f : 9.4f;
			float inertia = Timer % 170f > 120f ? 10f : 22f;
			NPC.velocity = (NPC.velocity * (inertia - 1f) + desired * speed) / inertia;
			NPC.rotation = NPC.velocity.X * 0.035f;
			NPC.frameCounter = (NPC.frameCounter + 0.56f) % AnimationFrames;

		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Texture2D texture = TextureAssets.Npc[Type].Value;
			Rectangle source = GetAtlasFrame(texture, (int)NPC.frameCounter);
			Vector2 origin = source.Size() * 0.5f;
			float pulse = 1f + 0.08f * (float)System.Math.Sin(Timer * 0.12f);
			float drawScale = NPC.scale * 0.36f * pulse;

			spriteBatch.Draw(texture, NPC.Center - screenPos, source, Color.White, NPC.rotation, origin, drawScale, SpriteEffects.None, 0f);
			return false;
		}

		private static Rectangle GetAtlasFrame(Texture2D texture, int frame) {
			int frameWidth = texture.Width / AtlasColumns;
			int frameHeight = texture.Height / AtlasRows;
			int column = frame % AtlasColumns;
			int row = frame / AtlasColumns;
			return new Rectangle(column * frameWidth, row * frameHeight, frameWidth, frameHeight);
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
