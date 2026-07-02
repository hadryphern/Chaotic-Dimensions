// Controla movimento, dano e efeitos visuais dos projecteis de Monthra Prismatic Ray.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Hostile
{
	public sealed class MonthraPrismaticRay : ModProjectile
	{
		private const int TelegraphTime = 48;
		private const int ActiveTime = 22;
		private int Age => (int)Projectile.localAI[0];
		private float BeamLength => System.Math.Abs(Projectile.ai[1]);
		private bool Symmetric => Projectile.ai[1] < 0f;
		private bool TracksTarget => Projectile.ai[2] > 0.5f;
		private bool Active => Age >= TelegraphTime && Age < TelegraphTime + ActiveTime;

		public override string Texture => "ChaoticDimensions/Content/Projectiles/Hostile/MonthraFireball";

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 86;
		}

		public override bool ShouldUpdatePosition() => false;
		// Controla em que fase o projetil pode causar dano.
		public override bool? CanDamage() => Active;

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.localAI[0]++;
			if (TracksTarget && Age < TelegraphTime - 5) {
				Player target = FindClosestPlayer();
				if (target != null) {
					float desired = (target.Center + target.velocity * 9f - Projectile.Center).ToRotation();
					float maxTurn = System.Math.Abs(Projectile.velocity.X);
					Projectile.ai[0] += MathHelper.Clamp(MathHelper.WrapAngle(desired - Projectile.ai[0]), -maxTurn, maxTurn);
				}
			}
			else {
				Projectile.ai[0] += Projectile.velocity.X;
			}
			if (Projectile.localAI[0] == TelegraphTime) {
				Projectile.netUpdate = true;
			}
		}

		private Player FindClosestPlayer() {
			Player result = null;
			float best = 2600f * 2600f;
			for (int i = 0; i < Main.maxPlayers; i++) {
				Player player = Main.player[i];
				if (!player.active || player.dead) continue;
				float distance = Vector2.DistanceSquared(player.Center, Projectile.Center);
				if (distance < best) {
					best = distance;
					result = player;
				}
			}
			return result;
		}

		private void GetLine(out Vector2 start, out Vector2 end) {
			Vector2 direction = Projectile.ai[0].ToRotationVector2();
			if (Symmetric) {
				start = Projectile.Center - direction * BeamLength;
				end = Projectile.Center + direction * BeamLength;
			}
			else {
				start = Projectile.Center;
				end = Projectile.Center + direction * BeamLength;
			}
		}

		// Calcula a colisao quando a forma nao e um retangulo simples.
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			GetLine(out Vector2 start, out Vector2 end);
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Active ? 5f : 2f, ref collisionPoint);
		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(ref Color lightColor) {
			GetLine(out Vector2 worldStart, out Vector2 worldEnd);
			Vector2 start = worldStart - Main.screenPosition;
			Vector2 end = worldEnd - Main.screenPosition;
			float telegraph = Utils.GetLerpValue(0f, TelegraphTime, Age, true);
			float pulse = 0.68f + 0.32f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 13f + Projectile.identity);
			Color glow = Active ? new Color(255, 75, 222) * 0.42f : new Color(218, 62, 195) * (0.08f + telegraph * pulse * 0.1f);
			Color core = Active ? new Color(255, 240, 253) * 0.92f : new Color(255, 172, 239) * (0.18f + telegraph * 0.2f);
			DrawLine(start, end, glow, Active ? 8f : 3f);
			DrawLine(start, end, core, Active ? 2f : 1f);
			return false;
		}

		private static void DrawLine(Vector2 start, Vector2 end, Color color, float width) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Vector2 edge = end - start;
			Main.EntitySpriteDraw(pixel, start, null, color, edge.ToRotation(), new Vector2(0f, 0.5f), new Vector2(edge.Length(), width), SpriteEffects.None, 0);
		}
	}
}
