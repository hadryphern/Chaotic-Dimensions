using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public sealed class CrystalineRushBuff : ModBuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Regeneration}";

		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.lifeRegen += 8;
			player.moveSpeed += 0.12f;
		}
	}
}
