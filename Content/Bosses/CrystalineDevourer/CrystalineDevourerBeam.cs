using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Bosses.CrystalineDevourer
{
	public sealed class CrystalineDevourerBeam : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_466";

		private const float BeamLength = 2600f;

		private int OwnerIndex => (int)Projectile.ai[0];
		private int TelegraphTime => (int)Projectile.ai[1];
		private int FireTime => (int)Projectile.ai[2];
		private bool IsFiring => Projectile.localAI[0] >= TelegraphTime;

		public override void SetDefaults() {
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
		}

		public override void AI() {
			if (OwnerIndex < 0 || OwnerIndex >= Main.maxNPCs || !Main.npc[OwnerIndex].active) {
				Projectile.Kill();
				return;
			}

			NPC owner = Main.npc[OwnerIndex];
			Projectile.Center = owner.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * 28f;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.localAI[0]++;
			if (Projectile.localAI[0] >= TelegraphTime + FireTime) {
				Projectile.Kill();
			}
		}

		public override bool CanHitPlayer(Player target) => IsFiring;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float collisionPoint = 0f;
			Vector2 end = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * BeamLength;
			float thickness = IsFiring ? 34f : 18f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, end, thickness, ref collisionPoint);
		}

		public override bool PreDraw(ref Color lightColor) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Vector2 start = Projectile.Center - Main.screenPosition;
			Vector2 direction = Projectile.velocity.SafeNormalize(Vector2.UnitX);
			Vector2 end = start + direction * BeamLength;
			float thickness = IsFiring ? 34f : 12f;
			Color coreColor = IsFiring ? new Color(255, 180, 255) : new Color(255, 120, 255);
			Color outerColor = IsFiring ? new Color(161, 40, 194) : new Color(130, 48, 160);

			DrawBeam(Main.spriteBatch, pixel, start, end, outerColor * 0.65f, thickness * 1.9f);
			DrawBeam(Main.spriteBatch, pixel, start, end, coreColor * 0.8f, thickness);
			return false;
		}

		private static void DrawBeam(SpriteBatch spriteBatch, Texture2D pixel, Vector2 start, Vector2 end, Color color, float thickness) {
			Vector2 edge = end - start;
			spriteBatch.Draw(pixel, start, null, color, edge.ToRotation(), Vector2.Zero, new Vector2(edge.Length(), thickness), SpriteEffects.None, 0f);
		}
	}
}
