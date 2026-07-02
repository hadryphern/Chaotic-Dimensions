// Implementa as lancas prismaticas finas e curvas da Monthra.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Hostile
{
	public sealed class MonthraPrismaticLance : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Hostile/MonthraFireball";

		public override void SetDefaults() {
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 230;
		}

		public override void AI() {
			Projectile.localAI[0]++;
			int targetIndex = Player.FindClosest(Projectile.position, Projectile.width, Projectile.height);
			if (Projectile.localAI[0] <= 52f && Projectile.ai[0] > 0f && targetIndex >= 0) {
				Player target = Main.player[targetIndex];
				float desired = (target.Center + target.velocity * 7f - Projectile.Center).ToRotation();
				float current = Projectile.velocity.ToRotation();
				float maxTurn = 0.004f + Projectile.ai[0] * 0.009f;
				Projectile.velocity = Projectile.velocity.RotatedBy(
					MathHelper.Clamp(MathHelper.WrapAngle(desired - current), -maxTurn, maxTurn)
				);
			}
			Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[1]);
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitX);
			float point = 0f;
			return Collision.CheckAABBvLineCollision(
				targetHitbox.TopLeft(),
				targetHitbox.Size(),
				Projectile.Center - direction * 30f,
				Projectile.Center + direction * 32f,
				5f,
				ref point
			);
		}

		public override bool PreDraw(ref Color lightColor) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Vector2 center = Projectile.Center - Main.screenPosition;
			Main.EntitySpriteDraw(pixel, center, null, new Color(255, 70, 220) * 0.48f, Projectile.rotation, new Vector2(0.5f), new Vector2(42f, 7f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(pixel, center, null, new Color(255, 236, 252) * 0.96f, Projectile.rotation, new Vector2(0.5f), new Vector2(34f, 2f), SpriteEffects.None, 0);
			return false;
		}
	}
}
