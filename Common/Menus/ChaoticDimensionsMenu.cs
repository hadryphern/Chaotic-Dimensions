using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Common.Menus
{
	public sealed class ChaoticDimensionsMenu : ModMenu
	{
		private static readonly Asset<Texture2D> BackgroundTexture = ModContent.Request<Texture2D>("ChaoticDimensions/Assets/UI/CrystalineCosmosBackground");
		private static readonly Asset<Texture2D> TitleCardTexture = ModContent.Request<Texture2D>("ChaoticDimensions/Assets/UI/ChaoticDimensionsMenuTitle");

		public override string DisplayName => "Chaotic Dimensions";

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CrystalineWorm");

		public override void OnSelected() {
			SoundEngine.PlaySound(SoundID.Item29 with { Pitch = -0.35f, Volume = 0.9f });
		}

		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor) {
			DrawBackground(spriteBatch);
			DrawTitle(spriteBatch);
			return false;
		}

		private static void DrawBackground(SpriteBatch spriteBatch) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Texture2D background = BackgroundTexture.Value;
			Rectangle screen = new(0, 0, Main.screenWidth, Main.screenHeight);
			float time = (float)Main.timeForVisualEffects * 0.012f;
			float driftX = (float)Math.Sin(time * 0.6f) * 42f;
			float driftY = (float)Math.Cos(time * 0.4f) * 26f;
			Rectangle backgroundDestination = new(-120 + (int)driftX, -90 + (int)driftY, screen.Width + 240, screen.Height + 180);

			spriteBatch.Draw(background, backgroundDestination, Color.White * 0.88f);
			spriteBatch.Draw(pixel, screen, new Color(14, 2, 26) * 0.42f);
			spriteBatch.Draw(pixel, new Rectangle(0, 0, screen.Width, screen.Height / 2), new Color(33, 6, 54) * 0.32f);
			spriteBatch.Draw(pixel, new Rectangle(0, screen.Height / 2, screen.Width, screen.Height / 2), new Color(18, 5, 32) * 0.5f);

			DrawWaveLayer(spriteBatch, pixel, 0.68f, 32f, 0.7f, new Color(117, 24, 158) * 0.16f);
			DrawWaveLayer(spriteBatch, pixel, 0.50f, 54f, -0.55f, new Color(89, 14, 122) * 0.18f);
			DrawWaveLayer(spriteBatch, pixel, 0.34f, 78f, 0.35f, new Color(161, 64, 199) * 0.1f);

			for (int i = 0; i < 64; i++) {
				float x = (i * 137f) % Main.screenWidth;
				float y = ((i * 263f) % (Main.screenHeight + 420)) - 220f;
				float twinkle = 0.55f + 0.45f * (float)Math.Sin(time * 3f + i * 0.8f);
				int size = 1 + (i % 3);
				spriteBatch.Draw(pixel, new Rectangle((int)x, (int)y, size, size), Color.Lerp(new Color(182, 90, 216), Color.White, 0.45f) * twinkle * 0.85f);
			}
		}

		private static void DrawTitle(SpriteBatch spriteBatch) {
			Texture2D titleCard = TitleCardTexture.Value;
			float maxWidth = Main.screenWidth * 0.84f;
			float maxHeight = Main.screenHeight * 0.34f;
			float scale = Math.Min(maxWidth / titleCard.Width, maxHeight / titleCard.Height);
			Vector2 drawPosition = new(Main.screenWidth * 0.5f, Main.screenHeight * 0.17f);
			spriteBatch.Draw(titleCard, drawPosition, null, Color.White, 0f, titleCard.Size() * 0.5f, scale, SpriteEffects.None, 0f);
		}

		private static void DrawWaveLayer(SpriteBatch spriteBatch, Texture2D pixel, float verticalFactor, float wavelength, float speed, Color color) {
			int baseY = (int)(Main.screenHeight * verticalFactor);
			float time = Main.GlobalTimeWrappedHourly * 80f * speed;
			for (int x = -32; x < Main.screenWidth + 32; x += 16) {
				float offset = (float)Math.Sin((x + time) / wavelength) * 18f;
				Rectangle waveSegment = new(x, baseY + (int)offset, 18, 6);
				spriteBatch.Draw(pixel, waveSegment, color);
			}
		}

	}
}
