using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Magic
{
	public sealed class RosalitaMagicBolt : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Magic/RosalitaMagicBolt";

		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 1;
		}

		public override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 300;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			Projectile.light = 0.5f;
		}

		public override void AI() {
			Projectile.rotation += 0.35f * Projectile.direction;
			if (Main.rand.NextBool(3)) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkTorch, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 140, default, 1.1f);
			}
		}

		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 8; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, Main.rand.NextFloat(-1.8f, 1.8f), Main.rand.NextFloat(-1.8f, 1.8f), 120, default, 1.05f);
			}
		}
	}
}
