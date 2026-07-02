// Controla movimento, dano e efeitos visuais dos projecteis de Kraken Homing Lightning.

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles
{
	public class KrakenHomingLightning : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/KrakenLightningStrike";

		private int TargetIndex => (int)Projectile.ai[0];
		private ref float Timer => ref Projectile.localAI[0];

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 150;
			Projectile.aiStyle = -1;
		}

		// Controla em que fase o projetil pode causar dano.
		public override bool? CanDamage() {
			return Timer >= 34f;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			overPlayers.Add(index);
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.tileCollide = false;
			Timer++;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			if (TryGetTarget(out Player target)) {
				float leadFrames = MathHelper.Lerp(18f, 4f, Utils.GetLerpValue(0f, 110f, Timer, true));
				Vector2 predicted = target.Center + target.velocity * leadFrames;
				Vector2 direction = predicted - Projectile.Center;
				if (direction.LengthSquared() > 4f) {
					direction.Normalize();
					float speed = Timer < 34f ? 5.5f : MathHelper.Lerp(11f, 18f, Utils.GetLerpValue(34f, 112f, Timer, true));
					float turn = Timer < 34f ? 0.026f : MathHelper.Lerp(0.078f, 0.018f, Utils.GetLerpValue(34f, 125f, Timer, true));
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * speed, turn);
				}
			}

			if (Timer < 34f) {
				Projectile.alpha = (int)MathHelper.Lerp(150f, 70f, Timer / 34f);
			}
			else {
				Projectile.alpha = 0;
			}

			if (Main.rand.NextBool(Timer < 34f ? 5 : 3)) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(18f, 18f), DustID.Electric, -Projectile.velocity * 0.08f, 0, new Color(125, 205, 255), 1.05f);
				dust.noGravity = true;
			}
		}

		// Aplica o efeito adicional quando atinge um jogador.
		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			target.AddBuff(BuffID.Electrified, 150);
		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 origin = texture.Size() * 0.5f;
			float telegraph = Timer < 34f ? 0.38f : 1f;
			float pulse = 0.88f + 0.16f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 24f + Projectile.ai[1]);
			Vector2 scale = new Vector2(0.22f, 0.34f * pulse) * telegraph;
			Color outer = new Color(82, 135, 180, (byte)(120 * telegraph));
			Color inner = new Color(230, 250, 255, (byte)(225 * telegraph));

			for (int i = Projectile.oldPos.Length - 1; i >= 1; i--) {
				if (Projectile.oldPos[i] == Vector2.Zero) {
					continue;
				}

				float fade = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
				Vector2 oldCenter = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
				Color trail = new Color(50, 88, 130, (byte)(80 * fade * telegraph));
				Main.spriteBatch.Draw(texture, oldCenter, null, trail, Projectile.rotation, origin, scale * (0.55f + fade * 0.45f), SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, outer, Projectile.rotation, origin, scale * new Vector2(1.4f, 1f), SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, inner, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
			return false;
		}

		private bool TryGetTarget(out Player target) {
			target = null;
			if (TargetIndex < 0 || TargetIndex >= Main.maxPlayers) {
				return false;
			}

			target = Main.player[TargetIndex];
			return target.active && !target.dead;
		}
	}
}
