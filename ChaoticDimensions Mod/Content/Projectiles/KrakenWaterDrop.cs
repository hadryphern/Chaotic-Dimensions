// Controla movimento, dano e efeitos visuais dos projecteis de Kraken Water Drop.

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
		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		// Define os valores iniciais usados pelo tModLoader.
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

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.tileCollide = false;
			Projectile.ai[0]++;
			Projectile.rotation += Projectile.ai[1] == 1f ? 0.18f : 0.13f;
			if (Projectile.ai[0] <= 34f) {
				Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
				if (target.active && !target.dead) {
					Vector2 predicted = target.Center + target.velocity * MathHelper.Lerp(10f, 3f, Projectile.ai[0] / 34f);
					Vector2 desired = (predicted - Projectile.Center).SafeNormalize(Projectile.velocity.SafeNormalize(Vector2.UnitY));
					float speed = MathHelper.Clamp(Projectile.velocity.Length() * 1.004f, 7f, 18f);
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired * speed, 0.016f);
				}
			}
			else {
				Projectile.velocity *= 1.002f;
			}

			if (Projectile.ai[0] < 18f) {
				Projectile.alpha = (int)MathHelper.Lerp(80f, 0f, Projectile.ai[0] / 18f);
			}

			if (Main.rand.NextBool(3)) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(8f, 8f), DustID.WaterCandle, -Projectile.velocity * 0.04f, 0, new Color(50, 120, 255), 0.8f);
				dust.noGravity = true;
			}
		}

		// Aplica o efeito adicional quando atinge um jogador.
		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			target.AddBuff(BuffID.Wet, 260);
			target.AddBuff(BuffID.Slow, 80);
		}

		// Controla o desenho antes da renderizacao padrao.
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
				Color trail = new Color(62, 105, 145, (byte)(48 * fade));
				Main.spriteBatch.Draw(texture, oldCenter, null, trail, Projectile.rotation, origin, Projectile.scale * pulse * (0.52f + fade * 0.42f), SpriteEffects.None, 0f);
			}

			Color color = new Color(172, 210, 232, (byte)(220 - Projectile.alpha));
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale * pulse, SpriteEffects.None, 0f);
			return false;
		}
	}
}
