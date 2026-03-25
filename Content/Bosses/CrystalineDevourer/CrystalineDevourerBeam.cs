using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Bosses.CrystalineDevourer
{
	public sealed class CrystalineDevourerBeam : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_466";

		private const float BeamLength = 1600f;

		private int OwnerIndex => (int)Projectile.ai[0];
		private int TelegraphTime => (int)Projectile.ai[1];
		private int FireTime => (int)Projectile.ai[2];
		private bool IsFiring => Projectile.localAI[0] >= TelegraphTime;

		public override void SetDefaults() {
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
		}

		public override void AI() {
			if (OwnerIndex < 0 || OwnerIndex >= Main.maxNPCs || !Main.npc[OwnerIndex].active) {
				Projectile.Kill();
				return;
			}

			NPC owner = Main.npc[OwnerIndex];
			if (owner.target >= 0 && owner.target < Main.maxPlayers) {
				Player target = Main.player[owner.target];
				if (target.active && !target.dead) {
					float trackStrength = IsFiring ? 0.028f : 0.14f;
					Vector2 desiredDirection = (target.Center - owner.Center).SafeNormalize(Projectile.velocity.SafeNormalize(Vector2.UnitX));
					Projectile.velocity = Vector2.Lerp(Projectile.velocity.SafeNormalize(Vector2.UnitX), desiredDirection, trackStrength);
				}
			}

			Projectile.Center = owner.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * 18f;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.localAI[0]++;
			if (Main.netMode != Terraria.ID.NetmodeID.Server && IsFiring && Projectile.localAI[0] % 2f == 0f && Vector2.Distance(Main.LocalPlayer.Center, Projectile.Center) < BeamLength + 420f) {
				PunchCameraModifier modifier = new(owner.Center, Main.rand.NextVector2Unit(), 18f, 8f, 6, 980f, $"{nameof(CrystalineDevourerBeam)}_{Projectile.identity}");
				Main.instance.CameraModifiers.Add(modifier);
			}

			if (Projectile.localAI[0] >= TelegraphTime + FireTime) {
				Projectile.Kill();
			}
		}

		public override bool CanHitPlayer(Player target) => IsFiring;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float collisionPoint = 0f;
			Vector2 end = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * BeamLength;
			float thickness = IsFiring ? 16f : 8f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, end, thickness, ref collisionPoint);
		}

		public override bool PreDraw(ref Color lightColor) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Vector2 start = Projectile.Center - Main.screenPosition;
			Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitX);
			float time = Main.GlobalTimeWrappedHourly * 10f + Projectile.identity * 0.37f;

			if (!IsFiring) {
				DrawWavyBeam(Main.spriteBatch, pixel, start, direction, BeamLength, 0f, 4f, 6f, new Color(214, 118, 255) * 0.72f, time, 0f, 20);
				DrawWavyBeam(Main.spriteBatch, pixel, start, direction, BeamLength, -10f, 6f, 3.5f, new Color(183, 82, 231) * 0.42f, time, 1.7f, 20);
				DrawWavyBeam(Main.spriteBatch, pixel, start, direction, BeamLength, 10f, 6f, 3.5f, new Color(183, 82, 231) * 0.42f, time, -1.7f, 20);
				return false;
			}

			DrawWavyBeam(Main.spriteBatch, pixel, start, direction, BeamLength, 0f, 7f, 14f, new Color(255, 247, 255) * 0.95f, time, 0f, 28);
			DrawWavyBeam(Main.spriteBatch, pixel, start, direction, BeamLength, -18f, 12f, 5f, new Color(233, 132, 255) * 0.9f, time, 1.2f, 28);
			DrawWavyBeam(Main.spriteBatch, pixel, start, direction, BeamLength, 18f, 12f, 5f, new Color(233, 132, 255) * 0.9f, time, -1.2f, 28);
			DrawWavyBeam(Main.spriteBatch, pixel, start, direction, BeamLength, -34f, 16f, 3.5f, new Color(171, 58, 214) * 0.72f, time, 2.4f, 28);
			DrawWavyBeam(Main.spriteBatch, pixel, start, direction, BeamLength, 34f, 16f, 3.5f, new Color(171, 58, 214) * 0.72f, time, -2.4f, 28);
			return false;
		}

		private static void DrawBeam(SpriteBatch spriteBatch, Texture2D pixel, Vector2 start, Vector2 end, Color color, float thickness) {
			Vector2 edge = end - start;
			spriteBatch.Draw(pixel, start, null, color, edge.ToRotation(), Vector2.Zero, new Vector2(edge.Length(), thickness), SpriteEffects.None, 0f);
		}

		private static void DrawWavyBeam(SpriteBatch spriteBatch, Texture2D pixel, Vector2 start, Vector2 direction, float length, float baseOffset, float amplitude, float thickness, Color color, float time, float phase, int steps) {
			Vector2 perpendicular = direction.RotatedBy(MathHelper.PiOver2);
			Vector2 previousPoint = start + perpendicular * baseOffset;
			for (int i = 1; i <= steps; i++) {
				float progress = i / (float)steps;
				float waveStrength = 0.35f + 0.65f * progress;
				float waveOffset = (float)System.Math.Sin(progress * 8.5f + time + phase) * amplitude * waveStrength;
				Vector2 nextPoint = start + direction * (length * progress) + perpendicular * (baseOffset + waveOffset);
				DrawBeam(spriteBatch, pixel, previousPoint, nextPoint, color, thickness);
				previousPoint = nextPoint;
			}
		}
	}
}
