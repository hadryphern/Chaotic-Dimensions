// Controla movimento, dano e efeitos visuais dos projecteis de Kraken Red Bolt.

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles
{
	public class KrakenRedBolt : ModProjectile
	{
		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 8;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 34;
			Projectile.height = 20;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 190;
			Projectile.aiStyle = -1;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			overPlayers.Add(index);
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.tileCollide = false;
			if (Projectile.ai[0] < 36f) {
				Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
				if (target.active && !target.dead) {
					Vector2 predicted = target.Center + target.velocity * MathHelper.Lerp(16f, 4f, Projectile.ai[0] / 36f);
					Vector2 desired = (predicted - Projectile.Center).SafeNormalize(Projectile.velocity.SafeNormalize(Vector2.UnitX));
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired * MathHelper.Clamp(Projectile.velocity.Length() * 1.003f, 10f, 19f), 0.03f);
				}
			}
			else {
				Projectile.velocity *= 1.002f;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.ai[0]++ < 14f) {
				Projectile.alpha = (int)MathHelper.Lerp(90f, 0f, Projectile.ai[0] / 14f);
			}

			if (Main.rand.NextBool(3)) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(8f, 5f), DustID.RedTorch, -Projectile.velocity * 0.08f, 0, new Color(255, 60, 95), 0.85f);
				dust.noGravity = true;
			}
		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 origin = texture.Size() * 0.5f;
			float opacity = 1f - Projectile.alpha / 255f;

			for (int i = Projectile.oldPos.Length - 1; i >= 1; i--) {
				if (Projectile.oldPos[i] == Vector2.Zero) {
					continue;
				}

				float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
				Vector2 oldCenter = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
				Main.spriteBatch.Draw(texture, oldCenter, null, new Color(105, 20, 42, (byte)(90 * fade * opacity)), Projectile.rotation, origin, Projectile.scale * (0.46f + fade * 0.38f), SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(232, 105, 130, (byte)(235 * opacity)), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}
