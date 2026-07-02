// Regista os buffs e debuffs agrupados em Crystaline Rush Buff.

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public sealed class CrystalineRushBuff : ModBuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Regeneration}";

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.lifeRegen += 8;
			player.moveSpeed += 0.12f;
		}
	}
}
