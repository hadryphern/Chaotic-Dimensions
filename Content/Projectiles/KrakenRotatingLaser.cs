using System.Collections.Generic;
using ChaoticDimensions.Common.Systems;
using ChaoticDimensions.Content.NPCs.Kraken;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles
{
	public class KrakenRotatingLaser : ModProjectile
	{
		private const int Lifetime = 900;
		private const float NormalLength = 12000f;
		private const float EnragedLength = 14000f;
		private const int AtlasColumns = 6;
		private const int AtlasRows = 6;

		public override string Texture => "ChaoticDimensions/Content/Projectiles/KrakenLaserFrames";

		private int OwnerIndex => (int)Projectile.ai[0];
		private float AngleOffset => Projectile.ai[1];
		private bool Enraged => Projectile.ai[2] == 1f;

		public override void SetStaticDefaults() {
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 16000;
		}

		public override void SetDefaults() {
			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = Lifetime + 10;
			Projectile.aiStyle = -1;
		}

		public override bool ShouldUpdatePosition() {
			return false;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			overPlayers.Add(index);
		}

		public override bool? CanDamage() {
			return Projectile.localAI[0] >= 36f && Projectile.localAI[0] <= Lifetime - 30;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			if (CanDamage() != true) {
				return false;
			}

			GetEndpoints(out Vector2 start, out Vector2 end);
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Enraged ? 88f : 76f, ref collisionPoint);
		}

		public override void AI() {
			Projectile.tileCollide = false;
			Projectile.localAI[0]++;
			if (!TryGetOwner(out NPC owner)) {
				Projectile.Kill();
				return;
			}

			Projectile.Center = GetKrakenHead(owner);
			Projectile.rotation = AngleOffset + SpinForTime(Projectile.localAI[0], Enraged);

			if (Projectile.localAI[0] <= Lifetime && Projectile.localAI[0] % 10f == 0f) {
				KrakenEventSystem.Instance.AddShake(7, Enraged ? 16f : 13f);
			}

			if (Projectile.localAI[0] > Lifetime) {
				Projectile.Kill();
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info) {
			target.AddBuff(BuffID.Electrified, 180);
		}

		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			int frame = ((int)Projectile.localAI[0] / 3) % (AtlasColumns * AtlasRows);
			Rectangle source = GetAtlasFrame(texture, frame);
			Vector2 origin = source.Size() * 0.5f;
			GetEndpoints(out Vector2 start, out Vector2 end);

			Vector2 midpoint = (start + end) * 0.5f - Main.screenPosition;
			float lengthScale = Vector2.Distance(start, end) / source.Height;
			float fadeIn = Utils.GetLerpValue(0f, 42f, Projectile.localAI[0], true);
			float fadeOut = Utils.GetLerpValue(0f, 42f, Projectile.timeLeft, true);
			float opacity = fadeIn * fadeOut;
			float pulse = 0.95f + 0.09f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 34f + Projectile.identity);
			float rotation = Projectile.rotation - MathHelper.PiOver2;

			Color outer = new Color(30, 122, 255, (byte)(90 * opacity));
			Color inner = new Color(232, 252, 255, (byte)(170 * opacity));
			Vector2 scale = new Vector2(Enraged ? 0.54f : 0.48f, lengthScale);
			Main.spriteBatch.Draw(texture, midpoint, source, outer, rotation, origin, scale * new Vector2(1.48f, 1f), SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture, midpoint, source, inner, rotation, origin, scale * new Vector2(0.78f * pulse, 1f), SpriteEffects.None, 0f);
			return false;
		}

		public static float SpinForTime(float timer, bool enraged) {
			float speed = enraged ? 1.14f : 1f;
			return (0.0022f * timer + 0.000006f * timer * timer) * speed;
		}

		public static Vector2 GetKrakenHead(NPC owner) {
			float scale = KrakenBoss.GetVisualScaleForLife(owner);
			Vector2 localOffset = KrakenBoss.GetRubyFrameOffset((int)owner.frameCounter) * scale;
			return owner.Center + new Vector2(0f, KrakenBoss.VisualDrawOffsetY) + localOffset.RotatedBy(owner.rotation);
		}

		private bool TryGetOwner(out NPC owner) {
			owner = null;
			if (OwnerIndex < 0 || OwnerIndex >= Main.maxNPCs) {
				return false;
			}

			owner = Main.npc[OwnerIndex];
			return owner.active && owner.type == ModContent.NPCType<KrakenBoss>();
		}

		private void GetEndpoints(out Vector2 start, out Vector2 end) {
			Vector2 direction = Projectile.rotation.ToRotationVector2();
			start = Projectile.Center + direction * 42f;
			end = Projectile.Center + direction * (Enraged ? EnragedLength : NormalLength);
		}

		private static Rectangle GetAtlasFrame(Texture2D texture, int frame) {
			int frameWidth = texture.Width / AtlasColumns;
			int frameHeight = texture.Height / AtlasRows;
			int column = frame % AtlasColumns;
			int row = frame / AtlasColumns;
			return new Rectangle(column * frameWidth, row * frameHeight, frameWidth, frameHeight);
		}
	}
}
