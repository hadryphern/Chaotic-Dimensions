// Controla movimento, dano e efeitos visuais dos projecteis de Kraken Abyss Tether.

using ChaoticDimensions.Content.Buffs;
using ChaoticDimensions.Content.NPCs.Kraken;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles
{
	public class KrakenAbyssTether : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_0";

		private int OwnerIndex => (int)Projectile.ai[0];
		private int TargetIndex => (int)Projectile.ai[1];

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 2;
			Projectile.aiStyle = -1;
		}

		public override bool ShouldUpdatePosition() {
			return false;
		}

		// Controla em que fase o projetil pode causar dano.
		public override bool? CanDamage() {
			return false;
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			if (!TryGetOwner(out NPC owner) || !TryGetTarget(out Player target)) {
				Projectile.Kill();
				return;
			}

			Projectile.timeLeft = 2;
			Projectile.Center = target.Center;
			target.AddBuff(ModContent.BuffType<KrakenCrushingDepthDebuff>(), 10);

			Vector2 pull = owner.Center - target.Center;
			float distance = pull.Length();
			if (distance > 60f) {
				pull.Normalize();
				target.velocity += pull * (distance > 900f ? 0.18f : 0.075f);
			}

			if (Main.rand.NextBool(4)) {
				Dust dust = Dust.NewDustPerfect(target.Center + Main.rand.NextVector2Circular(26f, 26f), DustID.BlueTorch, Vector2.Zero, 0, new Color(60, 120, 255), 1.2f);
				dust.noGravity = true;
			}
		}

		// Controla o desenho antes da renderizacao padrao.
		public override bool PreDraw(ref Color lightColor) {
			if (!TryGetOwner(out NPC owner) || !TryGetTarget(out Player target)) {
				return false;
			}

			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Vector2 start = KrakenRotatingLaser.GetKrakenHead(owner) - Main.screenPosition;
			Vector2 end = target.Center - Main.screenPosition;
			Vector2 span = end - start;
			float distance = span.Length();
			if (distance <= 4f) {
				return false;
			}

			Vector2 direction = span / distance;
			Vector2 normal = new Vector2(-direction.Y, direction.X);
			int nodes = (int)MathHelper.Clamp(distance / 52f, 6f, 34f);
			for (int i = 0; i <= nodes; i++) {
				float t = i / (float)nodes;
				float wave = (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 7f + t * 14f) * 7f;
				Vector2 point = Vector2.Lerp(start, end, t) + normal * wave;
				float size = MathHelper.Lerp(5.5f, 3.2f, t) + 0.8f * (float)System.Math.Sin(t * MathHelper.TwoPi * 3f);
				Color color = Color.Lerp(new Color(18, 26, 38, 105), new Color(92, 116, 135, 125), 0.28f + 0.18f * (float)System.Math.Sin(t * 12f));
				Rectangle rect = new Rectangle((int)(point.X - size * 0.5f), (int)(point.Y - size * 0.5f), (int)size, (int)size);
				Main.spriteBatch.Draw(pixel, rect, color);
			}

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
