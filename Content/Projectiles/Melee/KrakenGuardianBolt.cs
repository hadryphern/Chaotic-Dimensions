using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Melee
{
	public sealed class KrakenGuardianBolt : ModProjectile
	{
		public override string Texture => "ChaoticDimensions/Content/Projectiles/Hostile/KrakenLightningBolt";

		public override void SetDefaults() {
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 3;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 45;
			Projectile.extraUpdates = 1;
		}

		public override void AI() {
			Player owner = Main.player[Projectile.owner];
			float baseRotation = Projectile.ai[0] + Projectile.timeLeft * 0.22f;
			Vector2 desiredOffset = baseRotation.ToRotationVector2() * 72f;
			Projectile.Center = owner.Center + desiredOffset;
			Projectile.rotation += 0.4f;
			Lighting.AddLight(Projectile.Center, 0.3f, 0.32f, 0.5f);
			if (Main.rand.NextBool(3)) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric);
			}
		}
	}
}
