using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public sealed class CrystalinePotionRegenerationBuff : ModBuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Regeneration}";

		public override void Update(Player player, ref int buffIndex) {
			player.lifeRegen += 18;
		}
	}
}
