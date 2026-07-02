// Controla movimento, dano e efeitos visuais dos projecteis de Monthra Fireball.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Hostile
{
	public sealed class MonthraFireball : ModProjectile
	{
		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
			ProjectileID.Sets.TrailCacheLength[Type] = 8;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.hostile = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 360;
			Projectile.penetrate = 1;
			Projectile.light = 0.75f;
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 4) {
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Main.rand.NextBool(2)) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkFairy, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 90, default, 1.35f);
				Main.dust[dust].noGravity = true;
			}
		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			int frameHeight = texture.Height / Main.projFrames[Type];
			Rectangle source = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
			Vector2 origin = source.Size() * 0.5f;

			for (int i = Projectile.oldPos.Length - 1; i >= 0; i--) {
				if (Projectile.oldPos[i] == Vector2.Zero) {
					continue;
				}

				float progress = 1f - (i / (float)Projectile.oldPos.Length);
				Color trailColor = new Color(255, 82, 214, 0) * (0.45f * progress);
				Vector2 drawPosition = Projectile.oldPos[i] + (Projectile.Size * 0.5f) - Main.screenPosition;
				Main.EntitySpriteDraw(texture, drawPosition, source, trailColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}

		// Executa a limpeza e atualiza a progressao ao terminar.
		public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			for (int i = 0; i < 18; i++) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkFairy, Main.rand.NextFloat(-3.8f, 3.8f), Main.rand.NextFloat(-3.8f, 3.8f), 90, default, 1.6f);
				Main.dust[dust].noGravity = true;
			}
		}
	}
}
