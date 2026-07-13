// Agulha fina de luz usada nos padrões novos da Monthra.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Hostile
{
	public sealed class MonthraHaloNeedle : ModProjectile
	{
		private const int TelegraphTime = 22;
		private const int FadeOutTime = 18;
		private int Age => (int)Projectile.localAI[0];
		private int TargetIndex => (int)Projectile.ai[0];
		private float Curve => Projectile.ai[1];
		private bool Active => Age >= TelegraphTime;

		public override string Texture => "ChaoticDimensions/Content/Projectiles/Hostile/MonthraFireball";

		// Mantem um rastro curto para a agulha parecer rapida sem sujar a tela.
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 150;
			Projectile.aiStyle = -1;
		}

		// Antes de disparar ela apenas mira e mostra o aviso.
		public override bool ShouldUpdatePosition() => Active;

		// A hitbox só liga depois do aviso.
		public override bool? CanDamage() => Active;

		// Atualiza a mira inicial e a curva final da agulha.
		public override void AI() {
			Projectile.localAI[0]++;

			if (Age < TelegraphTime && TryGetTarget(out Player target)) {
				Vector2 predicted = target.Center + target.velocity * 18f;
				Vector2 desired = (predicted - Projectile.Center).SafeNormalize(Projectile.velocity);
				float currentSpeed = Projectile.velocity.Length();
				Projectile.velocity = Vector2.Lerp(Projectile.velocity.SafeNormalize(Vector2.UnitY), desired, 0.12f) * currentSpeed;
			}
			else if (Active) {
				Projectile.velocity = Projectile.velocity.RotatedBy(Curve);
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Active && Main.rand.NextBool(3)) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(10f, 10f), DustID.PinkFairy, -Projectile.velocity * 0.04f, 80, new Color(255, 105, 230), 1.05f);
				dust.noGravity = true;
			}
		}

		private bool TryGetTarget(out Player target) {
			target = null;
			if (TargetIndex >= 0 && TargetIndex < Main.maxPlayers) {
				Player player = Main.player[TargetIndex];
				if (player.active && !player.dead) {
					target = player;
					return true;
				}
			}

			return false;
		}

		// Usa colisão em linha para a agulha parecer fina, mas confiável.
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			if (!Active) {
				return false;
			}

			Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitY);
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(
				targetHitbox.TopLeft(),
				targetHitbox.Size(),
				Projectile.Center - direction * 36f,
				Projectile.Center + direction * 46f,
				6f,
				ref collisionPoint
			);
		}

		// Desenha a agulha como um risco de luz fino, sem retângulo grande.
		public override bool PreDraw(ref Color lightColor) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitY);
			Vector2 center = Projectile.Center - Main.screenPosition;

			if (!Active) {
				float progress = Utils.GetLerpValue(0f, TelegraphTime, Age, true);
				Vector2 start = center - direction * 34f;
				Vector2 end = center + direction * 390f;
				DrawLine(pixel, start, end, new Color(255, 72, 226) * (0.16f + progress * 0.24f), 2.2f);
				DrawLine(pixel, start, end, new Color(255, 222, 250) * (0.1f + progress * 0.18f), 0.9f);
				return false;
			}

			float fadeOut = Projectile.timeLeft < FadeOutTime ? Projectile.timeLeft / (float)FadeOutTime : 1f;
			for (int i = Projectile.oldPos.Length - 1; i >= 1; i--) {
				if (Projectile.oldPos[i] == Vector2.Zero) {
					continue;
				}

				Vector2 oldCenter = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
				float trailOpacity = (1f - i / (float)Projectile.oldPos.Length) * 0.34f * fadeOut;
				DrawLine(pixel, oldCenter - direction * 24f, oldCenter + direction * 32f, new Color(255, 44, 214) * trailOpacity, 5f);
			}

			DrawLine(pixel, center - direction * 40f, center + direction * 48f, new Color(255, 58, 220) * (0.58f * fadeOut), 7f);
			DrawLine(pixel, center - direction * 34f, center + direction * 42f, new Color(255, 236, 252) * (0.94f * fadeOut), 1.8f);
			return false;
		}

		private static void DrawLine(Texture2D pixel, Vector2 start, Vector2 end, Color color, float width) {
			Vector2 edge = end - start;
			if (edge.LengthSquared() <= 0.01f) {
				return;
			}

			Main.EntitySpriteDraw(pixel, start, null, color, edge.ToRotation(), new Vector2(0f, 0.5f), new Vector2(edge.Length(), width), SpriteEffects.None, 0);
		}
	}
}
