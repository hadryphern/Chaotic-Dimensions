using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Hostile
{
	public abstract class MinecraftLegacyHostileProjectileBase : ModProjectile
	{
		protected abstract int Width { get; }
		protected abstract int Height { get; }
		protected abstract int Lifetime { get; }
		protected virtual float LightStrength => 0.25f;
		protected virtual int DustType => DustID.MagicMirror;

		public override string Texture => $"ChaoticDimensions/Content/Projectiles/Hostile/{GetType().Name}";

		public override void SetDefaults() {
			Projectile.width = Width;
			Projectile.height = Height;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = Lifetime;
		}

		public override void AI() {
			Projectile.rotation += Projectile.velocity.X * 0.06f;
			Lighting.AddLight(Projectile.Center, LightStrength, LightStrength, LightStrength);
			if (Main.rand.NextBool(5)) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustType);
			}
		}
	}

	public sealed class SnowBlazeFrostFireball : MinecraftLegacyHostileProjectileBase
	{
		protected override int Width => 20;
		protected override int Height => 20;
		protected override int Lifetime => 180;
		protected override float LightStrength => 0.35f;
		protected override int DustType => DustID.IceTorch;
	}

	public sealed class SnowBlazeIceShard : MinecraftLegacyHostileProjectileBase
	{
		protected override int Width => 16;
		protected override int Height => 16;
		protected override int Lifetime => 140;
		protected override float LightStrength => 0.25f;
		protected override int DustType => DustID.Ice;

		public override void AI() {
			base.AI();
			Projectile.velocity *= 1.006f;
		}
	}

	public sealed class SquidKrakenWaterBolt : MinecraftLegacyHostileProjectileBase
	{
		protected override int Width => 14;
		protected override int Height => 14;
		protected override int Lifetime => 160;
		protected override float LightStrength => 0.18f;
		protected override int DustType => DustID.Water;

		public override void SetDefaults() {
			base.SetDefaults();
			Projectile.tileCollide = false;
		}
	}

	public sealed class KrakenLightningBolt : MinecraftLegacyHostileProjectileBase
	{
		protected override int Width => 20;
		protected override int Height => 20;
		protected override int Lifetime => 120;
		protected override float LightStrength => 0.42f;
		protected override int DustType => DustID.Electric;

		public override void SetDefaults() {
			base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
		}

		public override void AI() {
			base.AI();
			Player player = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)];
			Vector2 desiredVelocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.UnitY) * 11.5f;
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.06f);
		}
	}
}
