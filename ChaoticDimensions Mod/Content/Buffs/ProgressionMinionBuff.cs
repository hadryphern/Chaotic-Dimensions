// Mantem os summons do catalogo ativos enquanto o jogador tiver pelo menos um minion.

using ChaoticDimensions.Content.Projectiles.Progression;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public sealed class ProgressionMinionBuff : ModBuff
	{
		public override string Texture => "Terraria/Images/Buff_30";

		public override void SetStaticDefaults() {
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ProgressionMinionProjectile>()] > 0) {
				player.buffTime[buffIndex] = 18000;
			}
			else {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}
