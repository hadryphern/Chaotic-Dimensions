using ChaoticDimensions.Common.Systems;
using Terraria.ModLoader;

namespace ChaoticDimensions.Common.Tiles
{
	public sealed class CrystalineDevourerArenaTileGuard : GlobalTile
	{
		public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged) {
			if (CrystalineDevourerArenaSystem.ProtectsTile(i, j, type)) {
				return false;
			}

			return base.CanKillTile(i, j, type, ref blockDamaged);
		}

		public override bool CanExplode(int i, int j, int type) {
			if (CrystalineDevourerArenaSystem.ProtectsTile(i, j, type)) {
				return false;
			}

			return base.CanExplode(i, j, type);
		}
	}
}
