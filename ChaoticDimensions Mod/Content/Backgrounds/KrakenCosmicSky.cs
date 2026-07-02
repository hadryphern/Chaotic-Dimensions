// Desenha e atualiza os elementos visuais de Kraken Cosmic Sky.

using ChaoticDimensions.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Backgrounds
{
	public class KrakenCosmicSky : CustomSky
	{
		public const string EffectKey = "ChaoticDimensions:KrakenCosmicSky";

		private bool active;
		private float intensity;
		private float timer;
		private float rageIntensity;

		public override void Update(GameTime gameTime) {
			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			intensity = active ? 1f : MathHelper.Max(0f, intensity - 0.04f);
			float rageTarget = active ? KrakenEventSystem.Instance.Phase2Rage : 0f;
			rageIntensity = MathHelper.Lerp(rageIntensity, rageTarget, 0.04f);
		}

		public override Color OnTileColor(Color inColor) {
			Color target = Color.Lerp(new Color(24, 42, 92), new Color(74, 22, 34), rageIntensity);
			float amount = MathHelper.Lerp(0.2f, 0.25f, rageIntensity) * intensity;
			return Color.Lerp(inColor, target, amount);
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth) {
			if (intensity <= 0f || maxDepth < 0f || minDepth >= 0f) {
				return;
			}

			Texture2D pixel = Terraria.GameContent.TextureAssets.MagicPixel.Value;
			Color background = Color.Lerp(new Color(1, 3, 14), new Color(15, 2, 7), rageIntensity);
			spriteBatch.Draw(pixel, new Rectangle(-8, -8, Main.screenWidth + 16, Main.screenHeight + 16), background * intensity);

			Texture2D far = ModContent.Request<Texture2D>("ChaoticDimensions/Content/Backgrounds/KrakenGalaxyFar").Value;
			Texture2D middle = ModContent.Request<Texture2D>("ChaoticDimensions/Content/Backgrounds/KrakenGalaxyMiddle").Value;
			Texture2D close = ModContent.Request<Texture2D>("ChaoticDimensions/Content/Backgrounds/KrakenGalaxyClose").Value;
			Texture2D glow = ModContent.Request<Texture2D>("ChaoticDimensions/Content/Backgrounds/KrakenGalaxyGlow").Value;
			Vector2 center = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f);
			Color farColor = Color.Lerp(Color.White, new Color(220, 126, 138), rageIntensity * 0.28f);
			Color middleColor = Color.Lerp(Color.White, new Color(218, 104, 120), rageIntensity * 0.36f);
			Color closeColor = Color.Lerp(Color.White, new Color(190, 74, 94), rageIntensity * 0.42f);
			Color glowColor = Color.Lerp(Color.White, new Color(225, 94, 112), rageIntensity * 0.48f);

			DrawCover(spriteBatch, far, center, Vector2.Zero, 1.08f, farColor * intensity, SpriteEffects.None);
			DrawGlow(spriteBatch, glow, center + new Vector2(Main.screenWidth * -0.12f, Main.screenHeight * -0.08f), 0.92f, intensity, glowColor);
			DrawCover(
				spriteBatch,
				middle,
				center,
				new Vector2((float)System.Math.Sin(timer * 0.055f) * 54f, (float)System.Math.Cos(timer * 0.038f) * 24f),
				1.24f,
				middleColor * (0.88f * intensity),
				SpriteEffects.None
			);
			DrawCover(
				spriteBatch,
				close,
				center,
				new Vector2((float)System.Math.Sin(timer * 0.09f + 1.6f) * -72f, (float)System.Math.Sin(timer * 0.047f) * 30f),
				1.34f,
				closeColor * (0.62f * intensity),
				SpriteEffects.FlipHorizontally
			);
		}

		private static void DrawGlow(SpriteBatch spriteBatch, Texture2D texture, Vector2 center, float size, float opacity, Color color) {
			float scale = System.Math.Min(Main.screenWidth / (float)texture.Width, Main.screenHeight / (float)texture.Height) * size;
			spriteBatch.Draw(texture, center, null, color * (0.82f * opacity), 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
		}

		private static void DrawCover(
			SpriteBatch spriteBatch,
			Texture2D texture,
			Vector2 center,
			Vector2 offset,
			float overscan,
			Color color,
			SpriteEffects effects
		) {
			float scale = System.Math.Max(Main.screenWidth / (float)texture.Width, Main.screenHeight / (float)texture.Height) * overscan;
			spriteBatch.Draw(texture, center + offset, null, color, 0f, texture.Size() * 0.5f, scale, effects, 0f);
		}

		public override float GetCloudAlpha() {
			return 0f;
		}

		public override void Activate(Vector2 position, params object[] args) {
			active = true;
			intensity = 1f;
		}

		public override void Deactivate(params object[] args) {
			active = false;
		}

		public override void Reset() {
			active = false;
			intensity = 0f;
			timer = 0f;
			rageIntensity = 0f;
		}

		public override bool IsActive() {
			return active || intensity > 0f;
		}
	}
}
