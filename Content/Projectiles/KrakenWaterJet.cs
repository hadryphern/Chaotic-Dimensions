using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles
{
	public class KrakenWaterJet : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 8;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public override void SetDefaults() {
			Projectile.width = 46;
			Projectile.height = 28;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 170;
			Projectile.aiStyle = -1;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			overPlayers.Add(index);
		}

		public override void AI() {
			Projectile.tileCollide = false;
			Projectile.ai[0]++;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.velocity *= 1.006f;

			if (Projectile.ai[0] < 24f) {
				Projectile.alpha = (int)MathHelper.Lerp(90f, 0f, Projectile.ai[0] / 24f);
			}

			if (Main.rand.NextBool(2)) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(18f, 10f), DustID.WaterCandle, -Projectile.velocity * 0.08f, 0, new Color(80, 170, 255), 1.05f);
				dust.noGravity = true;
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			target.AddBuff(BuffID.Wet, 240);
			target.AddBuff(BuffID.Slow, 70);
		}

		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 origin = texture.Size() * 0.5f;
			float pulse = 0.95f + 0.1f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 18f + Projectile.ai[1]);
			Color color = new Color(105, 190, 255, (byte)(210 - Projectile.alpha));

			for (int i = Projectile.oldPos.Length - 1; i >= 1; i--) {
				if (Projectile.oldPos[i] == Vector2.Zero) {
					continue;
				}

				float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
				Vector2 oldCenter = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
				Color trail = new Color(30, 112, 220, (byte)(85 * fade));
				Main.spriteBatch.Draw(texture, oldCenter, null, trail, Projectile.rotation, origin, Projectile.scale * pulse * (0.55f + fade * 0.35f), SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale * pulse, SpriteEffects.None, 0f);
			return false;
		}
	}
}
