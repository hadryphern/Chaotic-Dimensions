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
			Rectangle screen = new(0, 0, Main.screenWidth, Main.screenHeight);

			spriteBatch.Draw(pixel, screen, new Color(17, 2, 30));
			spriteBatch.Draw(pixel, new Rectangle(0, 0, screen.Width, screen.Height / 2), new Color(33, 6, 54) * 0.96f);
			spriteBatch.Draw(pixel, new Rectangle(0, screen.Height / 2, screen.Width, screen.Height / 2), new Color(18, 5, 32) * 0.95f);

			DrawWaveLayer(spriteBatch, pixel, 0.68f, 32f, 0.7f, new Color(117, 24, 158) * 0.28f);
			DrawWaveLayer(spriteBatch, pixel, 0.50f, 54f, -0.55f, new Color(89, 14, 122) * 0.34f);
			DrawWaveLayer(spriteBatch, pixel, 0.34f, 78f, 0.35f, new Color(161, 64, 199) * 0.18f);

			float time = (float)Main.timeForVisualEffects * 0.012f;
			for (int i = 0; i < 140; i++) {
				float x = (i * 137f) % Main.screenWidth;
				float y = ((i * 263f) % (Main.screenHeight + 420)) - 220f;
				float twinkle = 0.55f + 0.45f * (float)Math.Sin(time * 3f + i * 0.8f);
				int size = 1 + (i % 3);
				spriteBatch.Draw(pixel, new Rectangle((int)x, (int)y, size, size), Color.Lerp(new Color(182, 90, 216), Color.White, 0.45f) * twinkle);
			}
		}

		private static void DrawTitle(SpriteBatch spriteBatch) {
			Texture2D titleCard = TitleCardTexture.Value;
			float maxWidth = Main.screenWidth * 0.76f;
			float maxHeight = Main.screenHeight * 0.28f;
			float scale = Math.Min(maxWidth / titleCard.Width, maxHeight / titleCard.Height);
			Vector2 drawPosition = new(Main.screenWidth * 0.5f, Main.screenHeight * 0.155f);
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
