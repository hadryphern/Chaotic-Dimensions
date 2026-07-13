// Cria o muro de luz da Monthra que atravessa a arena pela lateral.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Hostile
{
	public sealed class MonthraSweepBeam : ModProjectile
	{
		private const int TelegraphTime = 72;
		private const int ActiveTime = 1500;
		private const int FadeOutTime = 38;
		private const float BeamHalfHeight = 14000f;

		private int Age => (int)Projectile.localAI[0];
		private float SpawnSide => Projectile.ai[0] == 0f ? 1f : Projectile.ai[0];
		private bool Active => Age >= TelegraphTime && Age <= TelegraphTime + ActiveTime;
		private float ActiveProgress => Utils.GetLerpValue(TelegraphTime, TelegraphTime + ActiveTime, Age, true);

		public override string Texture => "ChaoticDimensions/Content/Projectiles/Hostile/MonthraFireball";

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 120;
			Projectile.height = 16000;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = TelegraphTime + ActiveTime + FadeOutTime;
			Projectile.aiStyle = -1;
		}

		public override bool ShouldUpdatePosition() => false;

		// O dano so liga quando o aviso termina e o raio comeca a andar.
		public override bool? CanDamage() => Active;

		// Atualiza movimento, som e tremor do raio lateral.
		public override void AI() {
			Projectile.localAI[0]++;

			if (Age == TelegraphTime) {
				SoundEngine.PlaySound(SoundID.Item122 with { Pitch = 0.42f, Volume = 1.05f }, Projectile.Center);
				Projectile.netUpdate = true;
			}

			if (Active) {
				float speed = MathHelper.Lerp(2.4f, 16.5f, ActiveProgress * ActiveProgress);
				Vector2 center = Projectile.Center;
				center.X += -SpawnSide * speed;
				center.Y = MathHelper.Lerp(center.Y, GetTargetY(), 0.035f);
				Projectile.Center = center;

				if (!Main.dedServ && Age % 12 == 0) {
					float strength = MathHelper.Lerp(5f, 18f, ActiveProgress);
					PunchCameraModifier modifier = new(Projectile.Center, Main.rand.NextVector2Unit(), strength, 7f, 5, 2600f, $"{nameof(MonthraSweepBeam)}_{Projectile.identity}_{Age}");
					Main.instance.CameraModifiers.Add(modifier);
				}

				if (Age % 30 == 0) {
					Projectile.netUpdate = true;
				}
			}
		}

		private float GetTargetY() {
			int index = Player.FindClosest(Projectile.position, Projectile.width, Projectile.height);
			if (index >= 0 && index < Main.maxPlayers && Main.player[index].active && !Main.player[index].dead) {
				return Main.player[index].Center.Y;
			}

			return Main.screenPosition.Y + Main.screenHeight * 0.5f;
		}

		// Usa uma linha vertical enorme para impedir a passagem por cima ou por baixo.
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			if (!Active) {
				return false;
			}

			Vector2 start = Projectile.Center - Vector2.UnitY * BeamHalfHeight;
			Vector2 end = Projectile.Center + Vector2.UnitY * BeamHalfHeight;
			float collisionPoint = 0f;
			float width = MathHelper.Lerp(92f, 138f, ActiveProgress);
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, width, ref collisionPoint);
		}

		// Desenha o raio em camadas para evitar um retangulo rosa chapado.
		public override bool PreDraw(ref Color lightColor) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			float x = Projectile.Center.X - Main.screenPosition.X;
			Vector2 top = new(x, -760f);
			Vector2 bottom = new(x, Main.screenHeight + 760f);
			float telegraph = Utils.GetLerpValue(0f, TelegraphTime, Age, true);
			float fadeOut = Projectile.timeLeft < FadeOutTime ? Projectile.timeLeft / (float)FadeOutTime : 1f;
			float pulse = 0.82f + 0.18f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 15f + Projectile.identity);

			if (!Active) {
				Color warning = new Color(255, 42, 220) * (0.18f + telegraph * 0.28f);
				DrawLine(pixel, top, bottom, warning, 18f + telegraph * 18f);
				DrawLine(pixel, top + new Vector2(-46f, 0f), bottom + new Vector2(-46f, 0f), warning * 0.45f, 2f);
				DrawLine(pixel, top + new Vector2(46f, 0f), bottom + new Vector2(46f, 0f), warning * 0.45f, 2f);
				return false;
			}

			float width = MathHelper.Lerp(72f, 128f, ActiveProgress) * fadeOut;
			Color outer = new Color(255, 18, 205) * (0.32f * fadeOut * pulse);
			Color middle = new Color(255, 88, 230) * (0.58f * fadeOut);
			Color core = new Color(255, 236, 252) * (0.93f * fadeOut);
			DrawLine(pixel, top, bottom, outer, width * 1.55f);
			DrawLine(pixel, top, bottom, middle, width * 0.58f);
			DrawLine(pixel, top, bottom, core, 14f + 10f * pulse);

			for (int i = -3; i <= 3; i++) {
				if (i == 0) {
					continue;
				}

				float offset = i * 18f + (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 9f + i) * 7f;
				DrawLine(pixel, top + new Vector2(offset, 0f), bottom + new Vector2(offset, 0f), new Color(255, 160, 244) * (0.18f * fadeOut), 1.6f);
			}

			return false;
		}

		private static void DrawLine(Texture2D pixel, Vector2 start, Vector2 end, Color color, float width) {
			Vector2 edge = end - start;
			Main.EntitySpriteDraw(pixel, start, null, color, edge.ToRotation(), new Vector2(0f, 0.5f), new Vector2(edge.Length(), width), SpriteEffects.None, 0);
		}
	}
}
