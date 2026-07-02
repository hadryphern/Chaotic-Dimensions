// Regista os buffs e debuffs agrupados em Crystaline Devour Aegis Buff.

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public sealed class CrystalineDevourAegisBuff : ModBuff
	{


		public override string Texture => $"Terraria/Images/Buff_{BuffID.Shine}";

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
	}
}
