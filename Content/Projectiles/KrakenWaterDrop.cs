using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles
{
	public class KrakenWaterDrop : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public override void SetDefaults() {
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 210;
			Projectile.aiStyle = -1;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			overPlayers.Add(index);
		}

		public override void AI() {
			Projectile.tileCollide = false;
			Projectile.ai[0]++;
			Projectile.rotation += Projectile.ai[1] == 1f ? 0.24f : 0.16f;
			Projectile.velocity *= 1.003f;

			if (Projectile.ai[0] < 18f) {
				Projectile.alpha = (int)MathHelper.Lerp(80f, 0f, Projectile.ai[0] / 18f);
			}

			if (Main.rand.NextBool(3)) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(8f, 8f), DustID.WaterCandle, -Projectile.velocity * 0.04f, 0, new Color(50, 120, 255), 0.8f);
				dust.noGravity = true;
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			target.AddBuff(BuffID.Wet, 260);
			target.AddBuff(BuffID.Slow, 80);
		}

		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 origin = texture.Size() * 0.5f;
			float pulse = 0.92f + 0.1f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 16f + Projectile.identity);

			for (int i = Projectile.oldPos.Length - 1; i >= 1; i--) {
				if (Projectile.oldPos[i] == Vector2.Zero) {
					continue;
				}

				float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
				Vector2 oldCenter = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
				Color trail = new Color(22, 72, 190, (byte)(82 * fade));
				Main.spriteBatch.Draw(texture, oldCenter, null, trail, Projectile.rotation, origin, Projectile.scale * pulse * (0.52f + fade * 0.42f), SpriteEffects.None, 0f);
			}

			Color color = new Color(80, 160, 255, (byte)(230 - Projectile.alpha));
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale * pulse, SpriteEffects.None, 0f);
			return false;
		}
	}
}
