// Controla movimento, dano e efeitos visuais dos projecteis de Kraken Water Jet.

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
		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 8;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		// Define os valores iniciais usados pelo tModLoader.
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

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.tileCollide = false;
			Projectile.ai[0]++;
			if (Projectile.ai[0] <= 42f) {
				Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
				if (target.active && !target.dead) {
					Vector2 predicted = target.Center + target.velocity * MathHelper.Lerp(18f, 5f, Projectile.ai[0] / 42f);
					Vector2 desired = (predicted - Projectile.Center).SafeNormalize(Projectile.velocity.SafeNormalize(Vector2.UnitX));
					float speed = MathHelper.Clamp(Projectile.velocity.Length() * 1.005f, 9f, 20f);
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired * speed, 0.035f);
				}
			}
			else {
				Projectile.velocity *= 1.003f;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Projectile.ai[0] < 24f) {
				Projectile.alpha = (int)MathHelper.Lerp(90f, 0f, Projectile.ai[0] / 24f);
			}

			if (Main.rand.NextBool(2)) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(18f, 10f), DustID.WaterCandle, -Projectile.velocity * 0.08f, 0, new Color(80, 170, 255), 1.05f);
				dust.noGravity = true;
			}
		}

		// Aplica o efeito adicional quando atinge um jogador.
		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			target.AddBuff(BuffID.Wet, 240);
			target.AddBuff(BuffID.Slow, 70);
		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 origin = texture.Size() * 0.5f;
			float pulse = 0.95f + 0.1f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 18f + Projectile.ai[1]);
			Color color = new Color(170, 212, 235, (byte)(215 - Projectile.alpha));

			for (int i = Projectile.oldPos.Length - 1; i >= 1; i--) {
				if (Projectile.oldPos[i] == Vector2.Zero) {
					continue;
				}

				float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
				Vector2 oldCenter = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
				Color trail = new Color(52, 98, 145, (byte)(46 * fade));
				Main.spriteBatch.Draw(texture, oldCenter, null, trail, Projectile.rotation, origin, Projectile.scale * pulse * (0.55f + fade * 0.35f), SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale * pulse, SpriteEffects.None, 0f);
			return false;
		}
	}
}
