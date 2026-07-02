// Controla movimento, dano e efeitos visuais dos projecteis de Monthra Fireball Homing.

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Hostile
{
	public sealed class MonthraFireballHoming : ModProjectile
	{
		private int targetWho = -1;

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
			ProjectileID.Sets.TrailCacheLength[Type] = 12;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 420;
			Projectile.penetrate = 1;
			Projectile.light = 0.85f;
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 5) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
			}

			AcquireTarget();
			if (targetWho >= 0) {
				Player player = Main.player[targetWho];
				Vector2 predictedPosition = player.Center + (player.velocity * 8f);
				Vector2 desiredVelocity = (predictedPosition - Projectile.Center).SafeNormalize(Projectile.velocity) * 8.25f;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.06f);
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Main.rand.NextBool(2)) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkFairy, 0f, 0f, 90, default, 1.4f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 0.2f;
			}
		}

		private void AcquireTarget() {
			if (targetWho >= 0 && Main.player[targetWho].active && !Main.player[targetWho].dead) {
				return;
			}

			float bestDistance = 1200f;
			targetWho = -1;
			for (int i = 0; i < Main.maxPlayers; i++) {
				Player player = Main.player[i];
				if (!player.active || player.dead) {
					continue;
				}

				float distance = Vector2.Distance(Projectile.Center, player.Center);
				if (distance < bestDistance) {
					bestDistance = distance;
					targetWho = i;
				}
			}
		}

		// Executa a limpeza e atualiza a progressao ao terminar.
		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			for (int i = 0; i < 16; i++) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkFairy, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f), 90, default, 1.9f);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
