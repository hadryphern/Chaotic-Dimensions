using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Bosses.CrystalineDevourer
{
	public sealed class CrystalineDevourerSkyBeam : ModProjectile
	{
		private const float BeamHalfLength = 1500f;

		private int TelegraphTime => (int)Projectile.ai[0];
		private int FireTime => (int)Projectile.ai[1];
		private bool IsFiring => Projectile.localAI[0] >= TelegraphTime;

		public override string Texture => "Terraria/Images/Projectile_466";

		public override void SetDefaults() {
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 900;
		}

		public override void AI() {
			Projectile.velocity = Projectile.velocity.LengthSquared() > 0.01f ? Projectile.velocity.SafeNormalize(Vector2.UnitY) : Vector2.UnitY;
			Projectile.localAI[0]++;

			if (Projectile.localAI[0] == TelegraphTime) {
				SoundEngine.PlaySound(SoundID.Item122 with { Pitch = -0.35f, Volume = 1.25f }, Projectile.Center);
			}

			if (Main.netMode != Terraria.ID.NetmodeID.Server && IsFiring && Projectile.localAI[0] % 2f == 0f && Vector2.Distance(Main.LocalPlayer.Center, Projectile.Center) < 880f) {
				PunchCameraModifier modifier = new(Projectile.Center, Main.rand.NextVector2Unit(), 22f, 8f, 6, 1100f, $"{nameof(CrystalineDevourerSkyBeam)}_{Projectile.identity}");
				Main.instance.CameraModifiers.Add(modifier);
			}

			if (Projectile.localAI[0] >= TelegraphTime + FireTime) {
				Projectile.Kill();
			}
		}

		public override bool CanHitPlayer(Player target) => IsFiring;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float collisionPoint = 0f;
			Vector2 axis = Projectile.velocity.SafeNormalize(Vector2.UnitY);
			Vector2 start = Projectile.Center - axis * BeamHalfLength;
			Vector2 end = Projectile.Center + axis * BeamHalfLength;
			float thickness = IsFiring ? 26f : 6f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, thickness, ref collisionPoint);
		}

		public override bool PreDraw(ref Color lightColor) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Vector2 axis = Projectile.velocity.SafeNormalize(Vector2.UnitY);
			Vector2 perpendicular = axis.RotatedBy(MathHelper.PiOver2);
			Vector2 start = Projectile.Center - axis * BeamHalfLength - Main.screenPosition;
			Vector2 end = Projectile.Center + axis * BeamHalfLength - Main.screenPosition;
			float pulse = 0.94f + (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 20f + Projectile.identity) * 0.08f;

			if (!IsFiring) {
				DrawBeam(Main.spriteBatch, pixel, start, end, new Color(255, 255, 255, 220), 4f);
				DrawBeam(Main.spriteBatch, pixel, start + perpendicular * 6f, end + perpendicular * 6f, new Color(232, 128, 255, 140), 2f);
				DrawBeam(Main.spriteBatch, pixel, start - perpendicular * 6f, end - perpendicular * 6f, new Color(232, 128, 255, 140), 2f);
				return false;
			}

			DrawBeam(Main.spriteBatch, pixel, start, end, new Color(255, 245, 255) * 0.95f, 26f * pulse);
			DrawBeam(Main.spriteBatch, pixel, start + perpendicular * 18f, end + perpendicular * 18f, new Color(239, 143, 255) * 0.82f, 8f);
			DrawBeam(Main.spriteBatch, pixel, start - perpendicular * 18f, end - perpendicular * 18f, new Color(239, 143, 255) * 0.82f, 8f);
			DrawBeam(Main.spriteBatch, pixel, start + perpendicular * 34f, end + perpendicular * 34f, new Color(180, 66, 219) * 0.68f, 4f);
			DrawBeam(Main.spriteBatch, pixel, start - perpendicular * 34f, end - perpendicular * 34f, new Color(180, 66, 219) * 0.68f, 4f);
			return false;
		}

		private static void DrawBeam(SpriteBatch spriteBatch, Texture2D pixel, Vector2 start, Vector2 end, Color color, float thickness) {
			Vector2 edge = end - start;
			spriteBatch.Draw(pixel, start, null, color, edge.ToRotation(), Vector2.Zero, new Vector2(edge.Length(), thickness), SpriteEffects.None, 0f);
		}
	}
}
