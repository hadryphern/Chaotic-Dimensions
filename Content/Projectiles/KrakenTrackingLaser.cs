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
	public class KrakenTrackingLaser : ModProjectile
	{
		private const int TelegraphTime = 28;
		private const int Lifetime = 360;
		private const float LaserLength = 14000f;
		private int OwnerIndex => (int)Projectile.ai[0];

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
			Projectile.timeLeft = Lifetime + 20;
			Projectile.aiStyle = -1;
		}

		public override bool ShouldUpdatePosition() {
			return false;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) {
			overPlayers.Add(index);
		}

		public override bool? CanDamage() {
			return Projectile.localAI[0] >= TelegraphTime && Projectile.localAI[0] <= Lifetime;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			if (CanDamage() != true) {
				return false;
			}

			Vector2 direction = Projectile.rotation.ToRotationVector2();
			Vector2 start = Projectile.Center + direction * 30f;
			Vector2 end = Projectile.Center + direction * LaserLength;
			float collisionPoint = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 82f, ref collisionPoint);
		}

		public override void AI() {
			Projectile.tileCollide = false;
			Projectile.localAI[0]++;
			if (!TryGetOwner(out NPC owner)) {
				Projectile.Kill();
				return;
			}

			Projectile.Center = KrakenRotatingLaser.GetKrakenHead(owner);
			if (owner.target < 0 || owner.target >= Main.maxPlayers || !Main.player[owner.target].active || Main.player[owner.target].dead) {
				owner.TargetClosest(false);
			}

			if (owner.target < 0 || owner.target >= Main.maxPlayers || !Main.player[owner.target].active || Main.player[owner.target].dead) {
				Projectile.Kill();
				return;
			}

			Player target = Main.player[owner.target];
			Vector2 aim = target.Center + target.velocity * 16f - Projectile.Center;
			if (aim.LengthSquared() < 4f) {
				aim = Vector2.UnitY;
			}

			float targetRotation = aim.ToRotation();
			if (Projectile.localAI[0] <= 2f) {
				Projectile.rotation = targetRotation;
			}
			else {
				float turn = MathHelper.WrapAngle(targetRotation - Projectile.rotation);
				Projectile.rotation += MathHelper.Clamp(turn, -0.16f, 0.16f);
			}

			if (Projectile.localAI[0] % 6f == 0f) {
				KrakenEventSystem.Instance.AddShake(6, 8f);
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
			Vector2 origin = new Vector2(0f, texture.Height * 0.5f);
			float fadeIn = Utils.GetLerpValue(0f, 36f, Projectile.localAI[0], true);
			float fadeOut = Utils.GetLerpValue(0f, 34f, Projectile.timeLeft, true);
			float opacity = fadeIn * fadeOut;
			float telegraph = Projectile.localAI[0] < TelegraphTime ? 0.35f : 1f;
			float lengthScale = LaserLength / texture.Width;
			float widthScale = MathHelper.Lerp(0.42f, 1.02f, Utils.GetLerpValue(TelegraphTime, Lifetime, Projectile.localAI[0], true));

			Vector2 position = Projectile.Center - Main.screenPosition;
			Color outer = new Color(20, 110, 255, (byte)(96 * opacity * telegraph));
			Color inner = new Color(190, 245, 255, (byte)(185 * opacity * telegraph));
			Main.spriteBatch.Draw(texture, position, null, outer, Projectile.rotation, origin, new Vector2(lengthScale, widthScale * 1.22f), SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture, position, null, inner, Projectile.rotation, origin, new Vector2(lengthScale, widthScale * 0.76f), SpriteEffects.None, 0f);
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
