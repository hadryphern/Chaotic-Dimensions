// Controla movimento, dano e efeitos visuais dos projecteis de Kraken Hypnosis Vortex.

using ChaoticDimensions.Common.Systems;
using ChaoticDimensions.Content.NPCs.Kraken;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles
{
	public class KrakenHypnosisVortex : ModProjectile
	{
		private int OwnerIndex => (int)Projectile.ai[0];

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 220;
			Projectile.height = 220;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 600;
			Projectile.aiStyle = -1;
		}

		public override bool ShouldUpdatePosition() {
			return false;
		}

		// Controla em que fase o projetil pode causar dano.
		public override bool? CanDamage() {
			return Projectile.localAI[0] > 50f;
		}

		// Calcula a colisao quando a forma nao e um retangulo simples.
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 closest = Vector2.Clamp(Projectile.Center, targetHitbox.TopLeft(), targetHitbox.BottomRight());
			return Vector2.Distance(closest, Projectile.Center) < 105f;
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.localAI[0]++;
			if (!TryGetOwner(out NPC owner)) {
				Projectile.Kill();
				return;
			}

			Projectile.Center = owner.Center + new Vector2(0f, KrakenBoss.VisualDrawOffsetY - 140f);
			Projectile.rotation += 0.034f + Projectile.localAI[0] * 0.00005f;

			for (int i = 0; i < Main.maxPlayers; i++) {
				Player player = Main.player[i];
				if (!player.active || player.dead) {
					continue;
				}

				Vector2 toCenter = Projectile.Center - player.Center;
				float distance = toCenter.Length();
				if (distance > 1240f || distance < 1f) {
					continue;
				}

				toCenter.Normalize();
				float pull = MathHelper.Lerp(0.72f, 0.1f, distance / 1240f);
				player.velocity += toCenter * pull;
				player.AddBuff(BuffID.Slow, 2);

			}

			if (Projectile.localAI[0] % 12f == 0f) {
				KrakenEventSystem.Instance.AddShake(10, 7f);
			}
		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 origin = texture.Size() * 0.5f;
			float fadeIn = Utils.GetLerpValue(0f, 70f, Projectile.localAI[0], true);
			float fadeOut = Utils.GetLerpValue(0f, 70f, Projectile.timeLeft, true);
			float opacity = fadeIn * fadeOut;
			float pulse = 1f + 0.06f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 5f);
			Vector2 center = Projectile.Center - Main.screenPosition;
			Main.spriteBatch.Draw(texture, center, null, new Color(22, 38, 60, (byte)(130 * opacity)), Projectile.rotation, origin, 1.72f * pulse, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture, center, null, new Color(138, 174, 198, (byte)(72 * opacity)), -Projectile.rotation * 0.72f, origin, 1.38f, SpriteEffects.FlipHorizontally, 0f);
			return false;
		}

		private bool TryGetOwner(out NPC owner) {
			owner = null;
			if (OwnerIndex < 0 || OwnerIndex >= Main.maxNPCs) {
				return false;
			}

			owner = Main.npc[OwnerIndex];
			return owner.active && owner.type == ModContent.NPCType<KrakenBoss>();
		}
	}
}
