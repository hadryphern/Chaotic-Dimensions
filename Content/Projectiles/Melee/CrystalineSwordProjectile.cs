using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Melee
{
	public sealed class CrystalineSwordProjectile : ModProjectile
	{
		public const float OrbitRadius = 60f;

		private const float OrbitAngularSpeed = 0.42f;
		private const int FadeOutTime = 14;

		private bool IsOrbitingSword => OrbitSide != 0f;
		private ref float LinkedCenterIdentity => ref Projectile.ai[0];
		private ref float OrbitSide => ref Projectile.ai[1];
		private ref float OrbitTimer => ref Projectile.localAI[0];

		public override string Texture => "ChaoticDimensions/Content/Items/Weapons/Melee/CrystalineSword";

		public override void SetStaticDefaults() {
			ProjectileID.Sets.NeedsUUID[Type] = true;
			ProjectileID.Sets.TrailCacheLength[Type] = 12;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		public override void SetDefaults() {
			Projectile.Size = new Vector2(36f);
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 8;
			Projectile.timeLeft = 54;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override bool ShouldUpdatePosition() => !IsOrbitingSword;

		public override void AI() {
			OrbitTimer++;

			if (IsOrbitingSword) {
				if (!TryGetCenterSword(out Projectile centerSword)) {
					Projectile.Kill();
					return;
				}

				Vector2 forward = centerSword.velocity.SafeNormalize(Vector2.UnitX);
				Vector2 normal = forward.RotatedBy(MathHelper.PiOver2);
				float orbitAngle = ((OrbitTimer - 1f) * OrbitAngularSpeed) + (OrbitSide > 0f ? 0f : MathHelper.Pi);
				Vector2 orbitOffset = ((normal * (float)Math.Cos(orbitAngle)) + (forward * (float)Math.Sin(orbitAngle))) * OrbitRadius;
				Vector2 orbitTangent = ((-normal * (float)Math.Sin(orbitAngle)) + (forward * (float)Math.Cos(orbitAngle))) * OrbitAngularSpeed * OrbitRadius;

				Projectile.Center = centerSword.Center + orbitOffset;
				Projectile.velocity = centerSword.velocity + orbitTangent;
				Projectile.timeLeft = Math.Min(Projectile.timeLeft, centerSword.timeLeft);
				ApplyVisualRotation(Projectile.velocity.SafeNormalize(forward));
			}
			else {
				ApplyVisualRotation(Projectile.velocity.SafeNormalize(Vector2.UnitX));
			}

			if (Projectile.timeLeft < FadeOutTime) {
				Projectile.alpha = Math.Min(Projectile.alpha + 18, 255);
			}

			Lighting.AddLight(Projectile.Center, 0.8f, 0.3f, 0.9f);
		}

		private void ApplyVisualRotation(Vector2 direction) {
			Projectile.rotation = direction.ToRotation() + MathHelper.PiOver4;
			Projectile.direction = direction.X >= 0f ? 1 : -1;
		}

		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 origin = texture.Size() * 0.5f;

			DrawTrail(texture, origin);
			DrawSword(texture, origin, Projectile.Center, Projectile.rotation, GetCurrentColor(lightColor), 1f);
			return false;
		}

		private void DrawTrail(Texture2D texture, Vector2 origin) {
			int trailLength = Projectile.oldPos.Length;
			for (int i = trailLength - 1; i >= 0; i--) {
				Vector2 oldPos = Projectile.oldPos[i];
				if (oldPos == Vector2.Zero) {
					continue;
				}

				float progress = (trailLength - i) / (float)trailLength;
				float alpha = IsOrbitingSword ? 0.42f * progress : 0.75f * progress;
				float scale = IsOrbitingSword ? 0.92f + (0.08f * progress) : 1f + (0.12f * progress);
				float rotation = Projectile.oldRot[i] == 0f ? Projectile.rotation : Projectile.oldRot[i];
				Vector2 drawCenter = oldPos + (Projectile.Size * 0.5f);
				Color color = GetTrailColor(alpha);

				DrawSword(texture, origin, drawCenter, rotation, color, scale);
			}
		}

		private void DrawSword(Texture2D texture, Vector2 origin, Vector2 center, float rotation, Color color, float scale) {
			Main.EntitySpriteDraw(
				texture,
				center - Main.screenPosition,
				null,
				color,
				rotation,
				origin,
				scale,
				SpriteEffects.None,
				0
			);
		}

		private Color GetCurrentColor(Color lightColor) {
			Color baseColor = IsOrbitingSword ? new Color(255, 210, 245, 220) : new Color(255, 255, 255, 255);
			return baseColor * Projectile.Opacity;
		}

		private Color GetTrailColor(float alpha) {
			Color baseColor = IsOrbitingSword ? new Color(255, 120, 220, 0) : new Color(210, 245, 255, 0);
			return baseColor * alpha * Projectile.Opacity;
		}

		private bool TryGetCenterSword(out Projectile centerSword) {
			int centerIdentity = (int)LinkedCenterIdentity;
			for (int i = 0; i < Main.maxProjectiles; i++) {
				Projectile candidate = Main.projectile[i];
				if (!candidate.active || candidate.owner != Projectile.owner || candidate.type != Type) {
					continue;
				}

				if (candidate.identity == centerIdentity && candidate.ai[1] == 0f) {
					centerSword = candidate;
					return true;
				}
			}

			centerSword = null;
			return false;
		}
	}
}
