// Regista os buffs e debuffs agrupados em Monthra Butterfly Buff.

using ChaoticDimensions.Content.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public sealed class MonthraButterflyBuff : ModBuff
	{
		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<MonthraButterflyMinion>()] > 0) {
				player.buffTime[buffIndex] = 18000;
				return;
			}

			player.DelBuff(buffIndex);
			buffIndex--;
		}
	}
}
