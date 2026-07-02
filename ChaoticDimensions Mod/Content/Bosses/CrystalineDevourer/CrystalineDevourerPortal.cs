// Contem uma parte do combate e dos recursos do boss Crystaline Devourer Portal.

using Terraria;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Bosses.CrystalineDevourer
{
	public sealed class CrystalineDevourerPortal : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_687";

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Projectile.width = 96;
			Projectile.height = 96;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
		}

		// Atualiza o comportamento desta entidade a cada tick.
		public override void AI() {
			Projectile.rotation += 0.12f * (Projectile.ai[0] == 0f ? 1f : -1f);
			Projectile.scale = 0.9f + 0.2f * Utils.GetLerpValue(0f, 20f, Projectile.timeLeft, true);
			Lighting.AddLight(Projectile.Center, 0.65f, 0.1f, 0.7f);
		}
	}
}
