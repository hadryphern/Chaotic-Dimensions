using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public sealed class CrystalineDevourAegisBuff : ModBuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Shine}";

		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
	}
}
