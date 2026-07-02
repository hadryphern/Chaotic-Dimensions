// Desenha a galaxia rosa usada durante a luta da Monthra.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace ChaoticDimensions.Common.Graphics
{
	public sealed class MonthraGalaxySky : CustomSky
	{
		private static readonly Asset<Texture2D> Backdrop = ModContent.Request<Texture2D>("ChaoticDimensions/Assets/UI/CrystalineCosmosBackground");
		private bool active;
		private float intensity;

		public override void Update(GameTime gameTime) {
			float step = Main.gamePaused ? 0f : 0.025f;
			intensity = MathHelper.Clamp(intensity + (active ? step : -step), 0f, 1f);
		}

		public override Color OnTileColor(Color inColor) {
			return Color.Lerp(inColor, new Color(170, 62, 142), intensity * 0.38f);
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth) {
			if (Main.gameMenu || intensity <= 0f) {
				return;
			}

			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Texture2D backdrop = Backdrop.Value;
			float parallaxX = (Main.screenPosition.X * 0.009f) % 180f;
			float parallaxY = (Main.screenPosition.Y * 0.006f) % 110f;
			Rectangle destination = new(-(int)parallaxX - 120, -(int)parallaxY - 90, Main.screenWidth + 240, Main.screenHeight + 180);
			spriteBatch.Draw(backdrop, destination, new Color(255, 172, 238) * (0.82f * intensity));
			spriteBatch.Draw(pixel, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(44, 4, 46) * (0.58f * intensity));
			spriteBatch.Draw(pixel, new Rectangle(0, Main.screenHeight / 3, Main.screenWidth, Main.screenHeight * 2 / 3), new Color(112, 15, 94) * (0.18f * intensity));

			float time = Main.GlobalTimeWrappedHourly;
			for (int i = 0; i < 120; i++) {
				int seedX = (i * 977 + 173) % 1000;
				int seedY = (i * 613 + 419) % 1000;
				float x = seedX / 1000f * Main.screenWidth;
				float y = seedY / 1000f * Main.screenHeight;
				float twinkle = 0.35f + 0.65f * (float)System.Math.Sin(time * (1.4f + i % 5 * 0.17f) + i);
				int size = i % 17 == 0 ? 4 : i % 5 == 0 ? 2 : 1;
				Color star = Color.Lerp(new Color(255, 94, 218), new Color(190, 220, 255), (i % 7) / 6f);
				spriteBatch.Draw(pixel, new Rectangle((int)x, (int)y, size, size), star * (twinkle * intensity * 0.82f));
			}
		}

		public override float GetCloudAlpha() => 1f - intensity * 0.92f;
		public override void Activate(Vector2 position, params object[] args) => active = true;
		public override void Deactivate(params object[] args) => active = false;
		public override void Reset() { active = false; intensity = 0f; }
		public override bool IsActive() => active || intensity > 0.001f;
	}
}
