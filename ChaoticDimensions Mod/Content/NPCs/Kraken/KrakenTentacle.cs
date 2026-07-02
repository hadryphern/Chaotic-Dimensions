// Implementa a IA, animacao e regras dos NPCs de Kraken Tentacle.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.Kraken
{
	public class KrakenTentacle : ModNPC
	{
		private const string WaterSpikeTexturePath = "ChaoticDimensions/Content/NPCs/Kraken/KrakenWaterSpike";
		private const string GrabSegmentTexturePath = "ChaoticDimensions/Content/NPCs/Kraken/KrakenGrabTentacleSegment";

		private int Mode => (int)NPC.ai[0];
		private int OwnerIndex => (int)NPC.ai[1];
		private int Side => NPC.ai[2] < 0f ? -1 : 1;
		private ref float Timer => ref NPC.localAI[0];

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1;
		}

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			NPC.width = 70;
			NPC.height = 70;
			NPC.damage = 120;
			NPC.defense = 18;
			NPC.lifeMax = 26000;
			NPC.knockBackResist = 0.45f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath11;
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Timer++;
			NPC.TargetClosest();
			Player player = Main.player[NPC.target];

			if (Mode == 0) {
				DoWaterSpike(player);
			}
			else if (Mode == 1) {
				DoSlam(player);
			}
			else {
				DoGrabTentacle(player);
			}

			NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
		}

		private void DoWaterSpike(Player player) {
			NPC.alpha = 0;
			NPC.timeLeft = 220;
			NPC.damage = 105;
			NPC.width = 52;
			NPC.height = 78;

			Vector2 direction = player.Center - NPC.Center;
			if (direction != Vector2.Zero) {
				direction.Normalize();
			}

			float speed = Timer < 24f ? 3.4f : MathHelper.Lerp(12f, 18f, Utils.GetLerpValue(24f, 120f, Timer, true));
			float turn = Timer < 24f ? 0.035f : 0.12f;
			NPC.velocity = Vector2.Lerp(NPC.velocity, direction * speed, turn);

			if (Timer > 220f) {
				NPC.active = false;
				NPC.netUpdate = true;
			}
		}

		private void DoSlam(Player player) {
			if (Timer < 40f) {
				Vector2 warningPoint = player.Center + new Vector2(0f, 420f);
				NPC.Center = Vector2.Lerp(NPC.Center, warningPoint, 0.08f);
				NPC.velocity *= 0.88f;
				NPC.alpha = 80;
				return;
			}

			NPC.alpha = 0;
			if (Timer == 40f) {
				Vector2 direction = player.Center - NPC.Center;
				if (direction == Vector2.Zero) {
					direction = -Vector2.UnitY;
				}

				direction.Normalize();
				NPC.velocity = direction * 24f;
			}

			if (Timer > 100f) {
				NPC.active = false;
				NPC.netUpdate = true;
			}
		}

		private void DoGrabTentacle(Player player) {
			NPC.alpha = 0;
			NPC.timeLeft = 10;
			NPC.damage = 170;
			NPC.width = 62;
			NPC.height = 62;

			NPC owner = GetOwner();
			if (owner == null) {
				NPC.active = false;
				NPC.netUpdate = true;
				return;
			}

			Vector2 anchor = GetAnchor(owner);
			if (Timer < 2f) {
				NPC.Center = anchor;
			}

			Vector2 target = player.Center;
			NPC.Center = Vector2.Lerp(NPC.Center, target, Timer < 35f ? 0.055f : 0.095f);
			Vector2 direction = target - NPC.Center;
			if (direction != Vector2.Zero) {
				direction.Normalize();
			}

			NPC.velocity = Vector2.Lerp(NPC.velocity, direction * 12f, 0.1f);
			Vector2 fromAnchor = NPC.Center - anchor;
			float maxLength = 1460f;
			if (fromAnchor.Length() > maxLength) {
				fromAnchor.Normalize();
				NPC.Center = anchor + fromAnchor * maxLength;
			}

			if (NPC.Hitbox.Intersects(player.Hitbox)) {
				Vector2 pull = owner.Center - player.Center;
				if (pull != Vector2.Zero) {
					pull.Normalize();
					player.velocity += pull * 1.7f;
				}
			}

			if (Timer > 270f || Vector2.Distance(player.Center, owner.Center) < 460f && Timer > 90f) {
				NPC.active = false;
				NPC.netUpdate = true;
			}
		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			if (Mode == 0) {
				DrawWaterSpike(spriteBatch, screenPos);
				return false;
			}

			if (Mode == 2) {
				DrawGrabTentacle(spriteBatch, screenPos);
				return false;
			}

			Texture2D texture = TextureAssets.Npc[Type].Value;
			Vector2 origin = texture.Size() * 0.5f;
			Color color = Mode == 0 ? Color.White : new Color(90, 170, 255, 220);
			spriteBatch.Draw(texture, NPC.Center - screenPos, null, color, NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
			return false;
		}

		private NPC GetOwner() {
			if (OwnerIndex < 0 || OwnerIndex >= Main.maxNPCs) {
				return null;
			}

			NPC owner = Main.npc[OwnerIndex];
			return owner.active && owner.type == ModContent.NPCType<KrakenBoss>() ? owner : null;
		}

		private Vector2 GetAnchor(NPC owner) {
			return owner.Center + new Vector2(Side * 105f, -128f);
		}

		private void DrawWaterSpike(SpriteBatch spriteBatch, Vector2 screenPos) {
			Texture2D texture = ModContent.Request<Texture2D>(WaterSpikeTexturePath).Value;
			Vector2 origin = texture.Size() * 0.5f;
			float pulse = 1f + 0.08f * (float)System.Math.Sin(Timer * 0.18f);
			Color color = new Color(115, 190, 255, 220);

			for (int i = 5; i >= 1; i--) {
				if (NPC.oldPos.Length <= i || NPC.oldPos[i] == Vector2.Zero) {
					continue;
				}

				float fade = (6 - i) / 6f;
				Vector2 oldCenter = NPC.oldPos[i] + NPC.Size * 0.5f - screenPos;
				spriteBatch.Draw(texture, oldCenter, null, new Color(20, 80, 190, (byte)(70 * fade)), NPC.rotation, origin, NPC.scale * pulse * (0.6f + fade * 0.3f), SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(texture, NPC.Center - screenPos, null, color, NPC.rotation, origin, NPC.scale * pulse, SpriteEffects.None, 0f);
		}

		private void DrawGrabTentacle(SpriteBatch spriteBatch, Vector2 screenPos) {
			NPC owner = GetOwner();
			if (owner == null) {
				return;
			}

			Texture2D segmentTexture = ModContent.Request<Texture2D>(GrabSegmentTexturePath).Value;
			Vector2 anchor = GetAnchor(owner) - screenPos;
			Vector2 tip = NPC.Center - screenPos;
			Vector2 toTip = tip - anchor;
			float distance = toTip.Length();
			if (distance < 2f) {
				return;
			}

			Vector2 normal = new Vector2(-toTip.Y, toTip.X);
			if (normal != Vector2.Zero) {
				normal.Normalize();
			}

			float wave = (float)System.Math.Sin(Timer * 0.045f) * 64f;
			float wave2 = (float)System.Math.Sin(Timer * 0.038f + 1.7f) * 46f;
			Vector2 controlA = anchor + toTip * 0.24f + normal * (Side * 84f + wave);
			Vector2 controlB = anchor + toTip * 0.73f - normal * (Side * 58f - wave2);

			const int segments = 30;
			Vector2 origin = segmentTexture.Size() * 0.5f;
			for (int i = 0; i <= segments; i++) {
				float t = i / (float)segments;
				Vector2 point = Cubic(anchor, controlA, controlB, tip, t);
				Vector2 next = Cubic(anchor, controlA, controlB, tip, MathHelper.Clamp(t + 1f / segments, 0f, 1f));
				float rotation = (next - point).ToRotation();
				float radius = MathHelper.Lerp(25f, 13f, t);
				float pulse = 0.92f + 0.07f * (float)System.Math.Sin(Timer * 0.06f + t * MathHelper.TwoPi);
				float scale = radius / (segmentTexture.Width * 0.5f) * pulse;
				Color color = Color.Lerp(new Color(8, 18, 74, 235), new Color(44, 72, 182, 220), 0.18f + 0.16f * (float)System.Math.Sin(Timer * 0.045f + t * 8.2f));

				spriteBatch.Draw(segmentTexture, point, null, color, rotation, origin, scale, SpriteEffects.None, 0f);
			}
		}

		private static Vector2 Cubic(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t) {
			float u = 1f - t;
			return u * u * u * a + 3f * u * u * t * b + 3f * u * t * t * c + t * t * t * d;
		}

	}
}
