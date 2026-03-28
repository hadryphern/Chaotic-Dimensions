using ChaoticDimensions.Content.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public sealed class HappyCreeperMinionBuff : ModBuff
	{
		public override string Texture => "ChaoticDimensions/Content/Buffs/MonthraButterflyBuff";

		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<HappyCreeperMinion>()] > 0) {
				player.buffTime[buffIndex] = 18000;
				return;
			}

			player.DelBuff(buffIndex);
			buffIndex--;
		}
	}

	public sealed class SquidKrakenMinionBuff : ModBuff
	{
		public override string Texture => "ChaoticDimensions/Content/Buffs/MonthraButterflyBuff";

		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SquidKrakenMinion>()] > 0) {
				player.buffTime[buffIndex] = 18000;
				return;
			}

			player.DelBuff(buffIndex);
			buffIndex--;
		}
	}
}
