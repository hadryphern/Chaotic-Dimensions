using ChaoticDimensions.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.Kraken
{
	public class KrakenCrystalTurret : ModNPC
	{
		private const int AtlasColumns = 6;
		private const int AtlasRows = 3;
		private const int AnimationFrames = AtlasColumns * AtlasRows;
		private int OwnerIndex => (int)NPC.ai[0];
		private int Side => NPC.ai[1] < 0f ? -1 : 1;
		private ref float Timer => ref NPC.localAI[0];
		private ref float Phase2LifeApplied => ref NPC.localAI[1];

		public override string Texture => "ChaoticDimensions/Content/NPCs/Kraken/KrakenRubyShield";

		public override void SetDefaults() {
			NPC.width = 90;
			NPC.height = 90;
			NPC.damage = 205;
			NPC.defense = 26;
			NPC.lifeMax = 400000;
			NPC.knockBackResist = 0.04f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.Item122;
			NPC.value = 0f;
		}

		public override void AI() {
			Timer++;
			if (!TryGetOwner(out NPC owner)) {
				NPC.EncourageDespawn(20);
				return;
			}

			ApplyPhase2Life(owner);
			NPC.TargetClosest();
			Player player = Main.player[NPC.target];
			float lifeProgress = 1f - owner.life / (float)owner.lifeMax;
			Vector2 desiredPosition = owner.Center + new Vector2(Side * 520f, -90f + (float)System.Math.Sin(Timer * 0.035f + Side) * 92f);
			NPC.Center = Vector2.Lerp(NPC.Center, desiredPosition, 0.17f);
			NPC.velocity = Vector2.Zero;
			NPC.rotation += Side * 0.025f;
			NPC.frameCounter = (NPC.frameCounter + 0.48f) % AnimationFrames;

			int interval = lifeProgress >= 0.85f ? 24 : lifeProgress >= 0.5f ? 32 : 42;
			if (Main.netMode != NetmodeID.MultiplayerClient && Timer % interval == 0f) {
				Vector2 aim = player.Center + player.velocity * (18f + lifeProgress * 12f) - NPC.Center;
				if (aim.LengthSquared() < 4f) {
					aim = Vector2.UnitY;
				}
				else {
					aim.Normalize();
				}

				int amount = lifeProgress >= 0.85f ? 3 : 2;
				for (int i = 0; i < amount; i++) {
					float spread = amount == 1 ? 0f : MathHelper.Lerp(-0.16f, 0.16f, i / (float)(amount - 1));
					Vector2 velocity = aim.RotatedBy(spread) * (lifeProgress >= 0.85f ? 18f : 14.5f);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + velocity * 3f, velocity, ModContent.ProjectileType<KrakenRedBolt>(), 190, 0f, Main.myPlayer);
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			bool broken = NPC.lifeMax > 0 && NPC.life <= NPC.lifeMax * 0.2f;
			bool poweredOff = !broken && Timer < 45f;
			Texture2D texture = broken
				? ModContent.Request<Texture2D>(
					"ChaoticDimensions/Content/NPCs/Kraken/KrakenRubyShieldBroken"
				).Value
				: poweredOff
					? ModContent.Request<Texture2D>(
						"ChaoticDimensions/Content/NPCs/Kraken/KrakenRubyShieldOff"
					).Value
					: TextureAssets.Npc[Type].Value;
			Rectangle source = GetAtlasFrame(texture, (int)NPC.frameCounter);
			Vector2 origin = source.Size() * 0.5f;
			Vector2 position = NPC.Center - screenPos;
			float pulse = 1f + 0.08f * (float)System.Math.Sin(Timer * 0.12f);
			int auraCopies = poweredOff ? 1 : 5;
			for (int i = 0; i < auraCopies; i++) {
				float angle = MathHelper.TwoPi * i / 5f + Timer * 0.02f;
				Color aura = poweredOff
					? new Color(70, 0, 8, 22)
					: broken
						? new Color(255, 30, 42, 48)
						: new Color(255, 10, 52, 68);
				spriteBatch.Draw(texture, position + angle.ToRotationVector2() * 10f, source, aura, NPC.rotation, origin, 1.25f * pulse, SpriteEffects.None, 0f);
			}

			Color mainColor = poweredOff ? Color.White * 0.82f : Color.White;
			spriteBatch.Draw(texture, position, source, mainColor, NPC.rotation, origin, 1.05f * pulse, SpriteEffects.None, 0f);
			return false;
		}

		private static Rectangle GetAtlasFrame(Texture2D texture, int frame) {
			int frameWidth = texture.Width / AtlasColumns;
			int frameHeight = texture.Height / AtlasRows;
			int safeFrame = frame % AnimationFrames;
			int column = safeFrame % AtlasColumns;
			int row = safeFrame / AtlasColumns;
			return new Rectangle(column * frameWidth, row * frameHeight, frameWidth, frameHeight);
		}

		private bool TryGetOwner(out NPC owner) {
			owner = null;
			if (OwnerIndex < 0 || OwnerIndex >= Main.maxNPCs) {
				return false;
			}

			owner = Main.npc[OwnerIndex];
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
