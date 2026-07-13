// Controla movimento, dano e efeitos visuais dos projecteis de Kraken Lightning Strike.

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
		private const int TelegraphTime = 44;
		private const int StrikeTime = 12;
		private const int CollisionHeight = 16000;

		private bool VisualOnly => Projectile.ai[1] == 1f;

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 16000;
		}

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 16;
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

		// Controla em que fase o projetil pode causar dano.
		public override bool? CanDamage() {
			if (VisualOnly) {
				return false;
			}

			return Projectile.ai[0] >= TelegraphTime && Projectile.ai[0] <= TelegraphTime + StrikeTime;
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.tileCollide = false;
			Projectile.ai[0]++;

			if (Projectile.ai[0] == TelegraphTime) {
				Projectile.width = 32;
				Projectile.netUpdate = true;
			}

			if (Projectile.ai[0] > TelegraphTime + StrikeTime + 4) {
				Projectile.Kill();
			}

			if (Projectile.ai[0] >= TelegraphTime && Main.rand.NextBool(2)) {
				Dust.NewDustPerfect(new Vector2(Projectile.Center.X + Main.rand.NextFloat(-22f, 22f), Projectile.Center.Y + Main.rand.NextFloat(-520f, 520f)), DustID.Electric, Vector2.Zero).noGravity = true;
			}
		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(ref Color lightColor) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			float progress = Projectile.ai[0] / TelegraphTime;
			bool striking = Projectile.ai[0] >= TelegraphTime;
			float top = -600f;
			float visibleHeight = Main.screenHeight + 1200f;
			float centerX = Projectile.Center.X - Main.screenPosition.X;

			if (!striking) {
				byte alpha = (byte)(VisualOnly ? 34 + progress * 46 : 54 + progress * 112);
				Color warning = new Color(155, 188, 208, alpha);
				int telegraphWidth = VisualOnly ? 1 : 4;
				spriteBatchSafeDraw(pixel, new Rectangle((int)(centerX - telegraphWidth / 2f), (int)top, telegraphWidth, (int)visibleHeight), warning);
				if (!VisualOnly) {
					spriteBatchSafeDraw(pixel, new Rectangle((int)(centerX - 18f), (int)top, 1, (int)visibleHeight), new Color(78, 105, 138, 26));
					spriteBatchSafeDraw(pixel, new Rectangle((int)(centerX + 18f), (int)top, 1, (int)visibleHeight), new Color(78, 105, 138, 26));
				}
				return false;
			}

			float flicker = VisualOnly ? 0.42f : 0.82f + Main.rand.NextFloat(0.22f);
			Texture2D lightning = TextureAssets.Projectile[Type].Value;
			Vector2 origin = new Vector2(lightning.Width * 0.5f, 0f);
			float jitter = (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 46f + Projectile.identity * 0.61f) * (VisualOnly ? 2.2f : 5.5f);
			Vector2 position = new Vector2(centerX + jitter, top);
			Vector2 scale = new Vector2(VisualOnly ? 0.24f : 0.48f, visibleHeight / lightning.Height);
			Color outer = new Color(105, 160, 195, (byte)(VisualOnly ? 74 * flicker : 136 * flicker));
			Color inner = new Color(242, 249, 252, (byte)(VisualOnly ? 96 * flicker : 210 * flicker));
			Main.spriteBatch.Draw(lightning, position + new Vector2(-jitter * 0.75f, 0f), null, new Color(48, 90, 138, (byte)(58 * flicker)), 0f, origin, scale * new Vector2(1.7f, 1f), SpriteEffects.FlipHorizontally, 0f);
			Main.spriteBatch.Draw(lightning, position, null, outer, 0f, origin, scale * new Vector2(1.24f, 1f), SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(lightning, position, null, inner, 0f, origin, scale, SpriteEffects.None, 0f);

			return false;
		}

		private static void spriteBatchSafeDraw(Texture2D texture, Rectangle rectangle, Color color) {
			Main.spriteBatch.Draw(texture, rectangle, color);
		}

	}
}
