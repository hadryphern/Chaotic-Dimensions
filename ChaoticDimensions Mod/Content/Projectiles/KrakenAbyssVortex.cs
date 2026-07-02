// Controla movimento, dano e efeitos visuais dos projecteis de Kraken Abyss Vortex.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles
{
	public class KrakenAbyssVortex : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/KrakenAbyssVortex";

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 220;
			Projectile.height = 320;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 360;
			Projectile.aiStyle = -1;
		}

		// Controla em que fase o projetil pode causar dano.
		public override bool? CanDamage() {
			return Projectile.ai[2] > 28f;
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.ai[2]++;
			Projectile.rotation += 0.14f * (Projectile.ai[0] < 0f ? -1f : 1f);
			Player target = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
			if (target.active && !target.dead && Projectile.ai[2] < 190f) {
				Vector2 desired = target.Center + target.velocity * 12f - Projectile.Center;
				if (desired.LengthSquared() > 4f) {
					desired.Normalize();
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired * 3.1f, 0.012f);
				}
			}
			Projectile.velocity += new Vector2(0f, (float)System.Math.Sin(Projectile.ai[2] * 0.035f) * 0.025f);

			float scaleTarget = MathHelper.Lerp(1.0f, 1.7f, MathHelper.Clamp(Projectile.ai[1], 0f, 2f) / 2f);
			Projectile.scale = MathHelper.Lerp(Projectile.scale <= 0f ? 0.45f : Projectile.scale, scaleTarget, 0.045f);

			if (Main.rand.NextBool(3)) {
				Vector2 dustPos = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width * 0.42f, Projectile.height * 0.46f);
				Dust dust = Dust.NewDustPerfect(dustPos, DustID.WaterCandle, Projectile.velocity * 0.15f, 0, new Color(45, 90, 190), 1.35f);
				dust.noGravity = true;
			}

			for (int i = 0; i < Main.maxPlayers; i++) {
				Player player = Main.player[i];
				if (!player.active || player.dead) {
					continue;
				}

				Vector2 pull = Projectile.Center - player.Center;
				float distance = pull.Length();
				float pullRadius = 360f * Projectile.scale;
				if (distance < 1f || distance > pullRadius) {
					continue;
				}

				pull.Normalize();
				float strength = MathHelper.Lerp(0.62f, 0.08f, distance / pullRadius);
				player.velocity += pull * strength;
				if (distance < 150f * Projectile.scale) {
					player.AddBuff(BuffID.Slow, 2);
				}
			}
		}

		// Aplica o efeito adicional quando atinge um jogador.
		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			target.AddBuff(BuffID.Wet, 300);
			target.AddBuff(BuffID.Slow, 120);
		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 origin = texture.Size() * 0.5f;
			float fadeIn = Utils.GetLerpValue(0f, 30f, Projectile.ai[2], true);
			float fadeOut = Utils.GetLerpValue(0f, 40f, Projectile.timeLeft, true);
			Color color = new Color(24, 45, 72, (byte)(214 * fadeIn * fadeOut));
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(145, 180, 205, (byte)(95 * fadeIn * fadeOut)), -Projectile.rotation * 0.65f, origin, Projectile.scale * 0.86f, SpriteEffects.FlipHorizontally, 0f);
			return false;
		}
	}
}
