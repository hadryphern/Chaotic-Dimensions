// Fornece um whip funcional comum aos quinze tiers de lashes.

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Progression
{
	public sealed class ProgressionWhipProjectile : ModProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.BlandWhip}";

		public override void SetStaticDefaults() {
			ProjectileID.Sets.IsAWhip[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.DefaultToWhip();
			Projectile.WhipSettings.Segments = 24;
			Projectile.WhipSettings.RangeMultiplier = 1.35f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
			Projectile.damage = (int)(Projectile.damage * 0.75f);
		}
	}
}
