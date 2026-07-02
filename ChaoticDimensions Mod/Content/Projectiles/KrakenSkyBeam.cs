// Controla movimento, dano e efeitos visuais dos projecteis de Kraken Sky Beam.

using System.Collections.Generic;
using ChaoticDimensions.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles
{
	public class KrakenSkyBeam : ModProjectile
	{
		private const int TelegraphTime = 64;
		private const int StrikeTime = 18;
		private const float CollisionHalfLength = 12000f;

		public override string Texture => "ChaoticDimensions/Content/Projectiles/KrakenSkyBeam";

		private float BeamRotation => Projectile.ai[1];
		private bool Striking => Projectile.ai[0] >= TelegraphTime && Projectile.ai[0] <= TelegraphTime + StrikeTime;

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 16000;
		}

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 122;
			Projectile.height = 122;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = TelegraphTime + StrikeTime + 18;
			Projectile.aiStyle = -1;
		}

		public override bool ShouldUpdatePosition() {
			return false;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			overPlayers.Add(index);
		}

		// Controla em que fase o projetil pode causar dano.
		public override bool? CanDamage() {
			return Striking;
		}

		// Calcula a colisao quando a forma nao e um retangulo simples.
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			if (!Striking) {
				return false;
			}

			GetBeamEndpoints(out Vector2 start, out Vector2 end);
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(new Vector2(targetHitbox.Left, targetHitbox.Top), new Vector2(targetHitbox.Width, targetHitbox.Height), start, end, 58f, ref collisionPoint);
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.tileCollide = false;
			Projectile.ai[0]++;
			if (Projectile.ai[0] == TelegraphTime) {
				Projectile.netUpdate = true;
				KrakenEventSystem.Instance.AddShake(10, 7f);
			}

			if (Striking && Main.rand.NextBool(2)) {
				GetBeamEndpoints(out Vector2 start, out Vector2 end);
				Vector2 point = Vector2.Lerp(start, end, Main.rand.NextFloat());
				Dust dust = Dust.NewDustPerfect(point + Main.rand.NextVector2Circular(26f, 26f), DustID.Electric, Vector2.Zero, 0, new Color(160, 225, 255), 1.3f);
				dust.noGravity = true;
			}
		}

		// Aplica o efeito adicional quando atinge um jogador.
		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			target.AddBuff(BuffID.Electrified, 180);
		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Rectangle source = texture.Bounds;
			Vector2 origin = source.Size() * 0.5f;
			float telegraphProgress = Utils.GetLerpValue(0f, TelegraphTime, Projectile.ai[0], true);
			float strikeFade = Striking ? 1f : Utils.GetLerpValue(0f, 18f, Projectile.timeLeft, true);
			GetVisibleBeamEndpoints(out Vector2 start, out Vector2 end);
			Vector2 drawPosition = (start + end) * 0.5f - Main.screenPosition;
			float beamLength = Vector2.Distance(start, end);
			float lengthScale = beamLength / source.Height;

			if (!Striking) {
				Color warning = new Color(156, 190, 210, (byte)(36 + 54 * telegraphProgress));
				Main.spriteBatch.Draw(texture, drawPosition, source, warning, BeamRotation, origin, new Vector2(0.12f, lengthScale), SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture, drawPosition, source, new Color(58, 94, 130, (byte)(28 * telegraphProgress)), BeamRotation, origin, new Vector2(0.26f, lengthScale), SpriteEffects.None, 0f);
				return false;
			}

			float pulse = 0.94f + 0.12f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 28f);
			Color outer = new Color(92, 148, 185, (byte)(78 * strikeFade));
			Color inner = new Color(240, 248, 250, (byte)(150 * strikeFade));
			Main.spriteBatch.Draw(texture, drawPosition, source, outer, BeamRotation, origin, new Vector2(0.46f * pulse, lengthScale), SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture, drawPosition, source, inner, BeamRotation, origin, new Vector2(0.24f * pulse, lengthScale), SpriteEffects.None, 0f);
			return false;
		}

		private void GetBeamEndpoints(out Vector2 start, out Vector2 end) {
			Vector2 axis = new Vector2((float)System.Math.Sin(BeamRotation), -(float)System.Math.Cos(BeamRotation));
			start = Projectile.Center - axis * CollisionHalfLength;
			end = Projectile.Center + axis * CollisionHalfLength;
		}

		private void GetVisibleBeamEndpoints(out Vector2 start, out Vector2 end) {
			Vector2 axis = new Vector2((float)System.Math.Sin(BeamRotation), -(float)System.Math.Cos(BeamRotation));
			Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
			float alongBeam = Vector2.Dot(screenCenter - Projectile.Center, axis);
			Vector2 visibleCenter = Projectile.Center + axis * alongBeam;
			float halfLength = new Vector2(Main.screenWidth, Main.screenHeight).Length() * 0.5f + 900f;
			start = visibleCenter - axis * halfLength;
			end = visibleCenter + axis * halfLength;
		}
	}
}
