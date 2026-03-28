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
		private const float BeamHalfLength = 2600f;
		private const float TelegraphHitThickness = 0.01f;
		private const float FireHitThickness = 0.05f;
		private const float TelegraphDrawThickness = 0.12f;
		private const float FireDrawThickness = 0.18f;

		private int TelegraphTime => (int)Projectile.ai[0];
		private int FireTime => (int)Projectile.ai[1];
		private bool IsFiring => Projectile.localAI[0] >= TelegraphTime;
		public override string Texture => "ChaoticDimensions/Content/Bosses/CrystalineDevourer/CrystalineDevourerSkyBeam";

		public override void SetDefaults() {
			Projectile.width = 1;
			Projectile.height = 1;
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
			float thickness = IsFiring ? FireHitThickness : TelegraphHitThickness;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, thickness, ref collisionPoint);
		}

		public override bool PreDraw(ref Color lightColor) {
			Texture2D beamTexture = TextureAssets.Projectile[Type].Value;
			Vector2 axis = Projectile.velocity.SafeNormalize(Vector2.UnitY);
			Vector2 start = Projectile.Center - axis * BeamHalfLength - Main.screenPosition;
			Vector2 end = Projectile.Center + axis * BeamHalfLength - Main.screenPosition;
			float pulse = 0.96f + (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 20f + Projectile.identity) * 0.04f;

			if (!IsFiring) {
				DrawBeam(Main.spriteBatch, beamTexture, start, end, new Color(255, 255, 255, 210), TelegraphDrawThickness * 5f);
				return false;
			}

			DrawBeam(Main.spriteBatch, beamTexture, start, end, new Color(255, 245, 255) * 0.95f, FireDrawThickness * pulse * 5f);
			return false;
		}

		private static void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end, Color color, float thickness) {
			Vector2 edge = end - start;
			spriteBatch.Draw(
				texture,
				start,
				null,
				color,
				edge.ToRotation() - MathHelper.PiOver2,
				new Vector2(texture.Width * 0.5f, 0f),
				new Vector2(thickness, edge.Length() / texture.Height),
				SpriteEffects.None,
				0f);
		}
	}
}
