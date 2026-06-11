using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles
{
	public class KrakenLightningStrike : ModProjectile
	{
		private const int TelegraphTime = 46;
		private const int StrikeTime = 7;
		private const int CollisionHeight = 16000;

		private bool VisualOnly => Projectile.ai[1] == 1f;

		public override void SetStaticDefaults() {
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 16000;
		}

		public override void SetDefaults() {
			Projectile.width = 12;
			Projectile.height = CollisionHeight;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = TelegraphTime + StrikeTime + 8;
			Projectile.aiStyle = -1;
		}

		public override bool ShouldUpdatePosition() {
			return false;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			overPlayers.Add(index);
		}

		public override bool? CanDamage() {
			if (VisualOnly) {
				return false;
			}

			return Projectile.ai[0] >= TelegraphTime && Projectile.ai[0] <= TelegraphTime + StrikeTime;
		}

		public override void AI() {
			Projectile.tileCollide = false;
			Projectile.ai[0]++;

			if (Projectile.ai[0] == TelegraphTime) {
				Projectile.width = 18;
				Projectile.netUpdate = true;
			}

			if (Projectile.ai[0] > TelegraphTime + StrikeTime + 4) {
				Projectile.Kill();
			}

			if (Projectile.ai[0] >= TelegraphTime && Main.rand.NextBool(3)) {
				Dust.NewDustPerfect(new Vector2(Projectile.Center.X + Main.rand.NextFloat(-14f, 14f), Projectile.Center.Y + Main.rand.NextFloat(-420f, 420f)), DustID.Electric, Vector2.Zero).noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			float progress = Projectile.ai[0] / TelegraphTime;
			bool striking = Projectile.ai[0] >= TelegraphTime;
			float top = -600f;
			float visibleHeight = Main.screenHeight + 1200f;
			float centerX = Projectile.Center.X - Main.screenPosition.X;

			if (!striking) {
				byte alpha = (byte)(VisualOnly ? 32 + progress * 42 : 42 + progress * 88);
				Color warning = new Color(120, 185, 255, alpha);
				int telegraphWidth = VisualOnly ? 1 : 2;
				spriteBatchSafeDraw(pixel, new Rectangle((int)(centerX - telegraphWidth / 2f), (int)top, telegraphWidth, (int)visibleHeight), warning);
				if (!VisualOnly) {
					spriteBatchSafeDraw(pixel, new Rectangle((int)(centerX - 12f), (int)top, 1, (int)visibleHeight), new Color(70, 125, 210, 20));
					spriteBatchSafeDraw(pixel, new Rectangle((int)(centerX + 12f), (int)top, 1, (int)visibleHeight), new Color(70, 125, 210, 20));
				}
				return false;
			}

			float flicker = VisualOnly ? 0.42f : 0.72f + Main.rand.NextFloat(0.2f);
			Texture2D lightning = TextureAssets.Projectile[Type].Value;
			Vector2 origin = new Vector2(lightning.Width * 0.5f, 0f);
			float jitter = (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 46f + Projectile.identity * 0.61f) * (VisualOnly ? 2.2f : 5.5f);
			Vector2 position = new Vector2(centerX + jitter, top);
			Vector2 scale = new Vector2(VisualOnly ? 0.24f : 0.34f, visibleHeight / lightning.Height);
			Color outer = new Color(95, 180, 255, (byte)(VisualOnly ? 74 * flicker : 112 * flicker));
			Color inner = new Color(235, 250, 255, (byte)(VisualOnly ? 96 * flicker : 176 * flicker));
			Main.spriteBatch.Draw(lightning, position + new Vector2(-jitter * 0.75f, 0f), null, new Color(25, 80, 210, (byte)(48 * flicker)), 0f, origin, scale * new Vector2(1.5f, 1f), SpriteEffects.FlipHorizontally, 0f);
			Main.spriteBatch.Draw(lightning, position, null, outer, 0f, origin, scale * new Vector2(1.15f, 1f), SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(lightning, position, null, inner, 0f, origin, scale, SpriteEffects.None, 0f);

			return false;
		}

		private static void spriteBatchSafeDraw(Texture2D texture, Rectangle rectangle, Color color) {
			Main.spriteBatch.Draw(texture, rectangle, color);
		}

	}
}
