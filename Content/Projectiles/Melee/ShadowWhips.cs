using System.Collections.Generic;
using ChaoticDimensions.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Melee
{
	public abstract class ShadowWhipProjectileBase : ModProjectile
	{
		protected abstract int SegmentCount { get; }
		protected abstract float RangeMultiplier { get; }
		protected abstract int TagBuffType { get; }
		protected abstract Color LineColor { get; }
		protected abstract Rectangle HandleFrame { get; }
		protected abstract Rectangle SegmentFrame { get; }
		protected virtual Rectangle EarlySegmentFrame => SegmentFrame;
		protected virtual Rectangle MidSegmentFrame => SegmentFrame;
		protected virtual Rectangle LateSegmentFrame => SegmentFrame;
		protected abstract Rectangle TipFrame { get; }

		public override void SetStaticDefaults() {
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.DefaultToWhip();
			Projectile.WhipSettings.Segments = SegmentCount;
			Projectile.WhipSettings.RangeMultiplier = RangeMultiplier;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(TagBuffType, 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Projectile.damage = (int)(Projectile.damage * 0.72f);
		}

		private void DrawLine(List<Vector2> points) {
			Texture2D lineTexture = TextureAssets.FishingLine.Value;
			Rectangle frame = lineTexture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2f, 2f);

			Vector2 current = points[0];
			for (int i = 0; i < points.Count - 1; i++) {
				Vector2 diff = points[i + 1] - points[i];
				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Color.Lerp(LineColor, Lighting.GetColor(points[i].ToTileCoordinates()), 0.5f);
				Vector2 scale = new Vector2(1f, (diff.Length() + 2f) / frame.Height);
				Main.EntitySpriteDraw(lineTexture, current - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);
				current += diff;
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			List<Vector2> points = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, points);
			DrawLine(points);

			Texture2D texture = TextureAssets.Projectile[Type].Value;
			SpriteEffects effects = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Vector2 drawPos = points[0];

			for (int i = 0; i < points.Count - 1; i++) {
				Rectangle frame = HandleFrame;
				Vector2 origin = new Vector2(frame.Width / 2f, 8f);
				float scale = 1f;

				if (i == points.Count - 2) {
					frame = TipFrame;
					origin = new Vector2(frame.Width / 2f, 10f);
				}
				else if (i > 10) {
					frame = LateSegmentFrame;
					origin = new Vector2(frame.Width / 2f, frame.Height / 2f);
				}
				else if (i > 5) {
					frame = MidSegmentFrame;
					origin = new Vector2(frame.Width / 2f, frame.Height / 2f);
				}
				else if (i > 0) {
					frame = EarlySegmentFrame;
					origin = new Vector2(frame.Width / 2f, frame.Height / 2f);
				}

				Vector2 diff = points[i + 1] - points[i];
				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(points[i].ToTileCoordinates());
				Main.EntitySpriteDraw(texture, drawPos - Main.screenPosition, frame, color, rotation, origin, scale, effects, 0);
				drawPos += diff;
			}

			return false;
		}
	}

	public sealed class RosalitaWhipProjectile : ShadowWhipProjectileBase
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Melee/RosalitaWhipProjectile";
		protected override int SegmentCount => 22;
		protected override float RangeMultiplier => 1.35f;
		protected override int TagBuffType => ModContent.BuffType<RosalitaTagBuff>();
		protected override Color LineColor => new Color(255, 120, 200);
		protected override Rectangle HandleFrame => new Rectangle(0, 0, 10, 26);
		protected override Rectangle SegmentFrame => new Rectangle(0, 42, 10, 16);
		protected override Rectangle EarlySegmentFrame => new Rectangle(0, 26, 10, 16);
		protected override Rectangle MidSegmentFrame => new Rectangle(0, 42, 10, 16);
		protected override Rectangle LateSegmentFrame => new Rectangle(0, 58, 10, 16);
		protected override Rectangle TipFrame => new Rectangle(0, 74, 10, 18);
	}

	public sealed class EclipsedMonthraWhipProjectile : ShadowWhipProjectileBase
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Melee/EclipsedMonthraWhipProjectile";
		protected override int SegmentCount => 26;
		protected override float RangeMultiplier => 1.7f;
		protected override int TagBuffType => ModContent.BuffType<EclipsedMonthraTagBuff>();
		protected override Color LineColor => new Color(170, 90, 255);
		protected override Rectangle HandleFrame => new Rectangle(0, 0, 10, 26);
		protected override Rectangle SegmentFrame => new Rectangle(0, 42, 10, 16);
		protected override Rectangle EarlySegmentFrame => new Rectangle(0, 26, 10, 16);
		protected override Rectangle MidSegmentFrame => new Rectangle(0, 42, 10, 16);
		protected override Rectangle LateSegmentFrame => new Rectangle(0, 58, 10, 16);
		protected override Rectangle TipFrame => new Rectangle(0, 74, 10, 18);
	}

	public sealed class ShadowWhipProjectile : ShadowWhipProjectileBase
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Melee/ShadowWhipProjectile";
		protected override int SegmentCount => 30;
		protected override float RangeMultiplier => 2.05f;
		protected override int TagBuffType => ModContent.BuffType<ShadowTagBuff>();
		protected override Color LineColor => new Color(150, 74, 220);
		protected override Rectangle HandleFrame => new Rectangle(0, 0, 14, 26);
		protected override Rectangle SegmentFrame => new Rectangle(0, 26, 14, 20);
		protected override Rectangle TipFrame => new Rectangle(0, 46, 14, 28);

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			base.OnHitNPC(target, hit, damageDone);
			ShadowCombatHelper.ApplyRend(target, Projectile.owner, 60 * 30, 8);
		}
	}
}
