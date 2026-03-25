using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public sealed class CrystalinePotionFortitudeBuff : ModBuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Ironskin}";

		public override void Update(Player player, ref int buffIndex) {
			player.statDefense += 10;
		}
	}
}
