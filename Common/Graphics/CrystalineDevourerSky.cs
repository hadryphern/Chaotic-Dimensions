using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChaoticDimensions.Content.Bosses.CrystalineDevourer;
using ChaoticDimensions.Common.Systems;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace ChaoticDimensions.Common.Graphics
{
	public sealed class CrystalineDevourerSky : CustomSky
	{
		private static readonly Asset<Texture2D> BackdropTexture = ModContent.Request<Texture2D>("ChaoticDimensions/Assets/UI/CrystalineCosmosBackground");

		private struct SkyStar
		{
			public Vector2 Position;
			public float Depth;
			public float Scale;
			public float TwinkleOffset;
		}

		private struct ConstellationLine
		{
			public Vector2 Start;
			public Vector2 End;
			public float Depth;
		}

		private readonly List<SkyStar> stars = [];
		private readonly List<ConstellationLine> constellations = [];
		private bool isActive;
		private float intensity;
		private bool initialized;

		public override void Update(GameTime gameTime) {
			if (!initialized) {
				InitializeSky();
			}

			if (isActive && (!NPC.AnyNPCs(ModContent.NPCType<CrystalineDevourerHead>()) || !CrystalineDevourerArenaSystem.HasAnyLivingPlayers())) {
				isActive = false;
			}

			float step = Main.gamePaused ? 0f : 0.03f;
			intensity = MathHelper.Clamp(intensity + (isActive ? step : -step), 0f, 1f);
		}

		public override Color OnTileColor(Color inColor) {
			Color tint = new(138, 72, 184);
			return Color.Lerp(inColor, tint, intensity * 0.45f);
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth) {
			if (Main.gameMenu || intensity <= 0f) {
				return;
			}

			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Texture2D backdrop = BackdropTexture.Value;
			Rectangle screen = new(0, 0, Main.screenWidth, Main.screenHeight);

			float driftX = (Main.screenPosition.X * 0.0125f) % 150f;
			float driftY = (Main.screenPosition.Y * 0.0085f) % 90f;
			Rectangle backgroundDestination = new(-120 - (int)driftX, -90 - (int)driftY, screen.Width + 240, screen.Height + 180);
			spriteBatch.Draw(backdrop, backgroundDestination, Color.White * (0.78f * intensity));

			Color top = Color.Lerp(Color.Black, new Color(24, 4, 40), intensity);
			Color mid = Color.Lerp(Color.Black, new Color(58, 12, 83), intensity);
			Color bottom = Color.Lerp(Color.Black, new Color(16, 4, 28), intensity);

			spriteBatch.Draw(pixel, new Rectangle(0, 0, screen.Width, screen.Height / 2), top * 0.64f);
			spriteBatch.Draw(pixel, new Rectangle(0, screen.Height / 3, screen.Width, screen.Height / 2), mid * 0.58f);
			spriteBatch.Draw(pixel, new Rectangle(0, screen.Height / 2, screen.Width, screen.Height / 2), bottom * 0.78f);

			DrawWaveLayer(spriteBatch, pixel, 0.75f, 32f, 0.75f, new Color(117, 24, 158) * (0.1f * intensity));
			DrawWaveLayer(spriteBatch, pixel, 0.55f, 54f, -0.55f, new Color(89, 14, 122) * (0.14f * intensity));
			DrawWaveLayer(spriteBatch, pixel, 0.35f, 78f, 0.35f, new Color(161, 64, 199) * (0.08f * intensity));

			foreach (SkyStar star in stars) {
				float twinkle = 0.55f + 0.45f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2.4f + star.TwinkleOffset);
				Vector2 position = ApplyParallax(star.Position, star.Depth);
				float size = star.Scale * (0.6f + intensity * 0.8f);
				Rectangle starRect = new((int)position.X, (int)position.Y, Math.Max(1, (int)size), Math.Max(1, (int)size));
				spriteBatch.Draw(pixel, starRect, Color.Lerp(new Color(182, 90, 216), Color.White, 0.45f) * (twinkle * intensity * 0.75f));
			}

			foreach (ConstellationLine line in constellations) {
				Vector2 start = ApplyParallax(line.Start, line.Depth);
				Vector2 end = ApplyParallax(line.End, line.Depth);
				DrawLine(spriteBatch, pixel, start, end, new Color(190, 120, 232) * (0.1f * intensity), 2f);
			}
		}

		private void DrawWaveLayer(SpriteBatch spriteBatch, Texture2D pixel, float verticalFactor, float wavelength, float speed, Color color) {
			int baseY = (int)(Main.screenHeight * verticalFactor);
			float time = Main.GlobalTimeWrappedHourly * 80f * speed;
			for (int x = -32; x < Main.screenWidth + 32; x += 16) {
				float offset = (float)Math.Sin((x + time) / wavelength) * 18f;
				Rectangle waveSegment = new(x, baseY + (int)offset, 18, 6);
				spriteBatch.Draw(pixel, waveSegment, color);
			}
		}

		private Vector2 ApplyParallax(Vector2 worldAnchor, float depth) {
			Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
			Vector2 relative = worldAnchor - screenCenter;
			return new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f + relative / depth;
		}

		private static void DrawLine(SpriteBatch spriteBatch, Texture2D pixel, Vector2 start, Vector2 end, Color color, float thickness) {
			Vector2 edge = end - start;
			spriteBatch.Draw(pixel, start, null, color, edge.ToRotation(), Vector2.Zero, new Vector2(edge.Length(), thickness), SpriteEffects.None, 0f);
		}

		private void InitializeSky() {
			initialized = true;
			stars.Clear();
			constellations.Clear();

			UnifiedRandom random = new(642771);
			for (int i = 0; i < 165; i++) {
				stars.Add(new SkyStar {
					Position = new Vector2(random.NextFloat(0f, Main.maxTilesX * 16f), random.NextFloat(-2000f, (float)Main.worldSurface * 12f)),
					Depth = random.NextFloat(2.2f, 8.5f),
					Scale = random.NextFloat(1f, 3.5f),
					TwinkleOffset = random.NextFloat(MathHelper.TwoPi)
				});
			}

			for (int i = 0; i < 18; i++) {
				Vector2 origin = new(random.NextFloat(0f, Main.maxTilesX * 16f), random.NextFloat(-1200f, (float)Main.worldSurface * 10f));
				float depth = random.NextFloat(3.5f, 7f);
				Vector2 current = origin;
				for (int segment = 0; segment < 3; segment++) {
					Vector2 next = current + new Vector2(random.NextFloat(80f, 180f), random.NextFloat(-90f, 90f)).RotatedByRandom(0.8f);
					constellations.Add(new ConstellationLine {
						Start = current,
						End = next,
						Depth = depth
					});
					current = next;
				}
			}
		}

		public override float GetCloudAlpha() => 1f - intensity * 0.75f;

		public override void Activate(Vector2 position, params object[] args) {
			isActive = true;
		}

		public override void Deactivate(params object[] args) {
			isActive = false;
		}

		public override void Reset() {
			isActive = false;
			intensity = 0f;
		}

		public override bool IsActive() => isActive || intensity > 0.001f;
	}
}
