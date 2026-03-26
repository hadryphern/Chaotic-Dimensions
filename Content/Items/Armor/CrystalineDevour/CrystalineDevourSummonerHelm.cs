using Terraria;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Players;

namespace ChaoticDimensions.Content.Items.Armor.CrystalineDevour
{
	[AutoloadEquip(EquipType.Head)]
	public sealed class CrystalineDevourSummonerHelm : CrystalineDevourHelmetBase
	{
		protected override CrystalineDevourSetType SetType => CrystalineDevourSetType.Summoner;
		protected override int BonusDefense => 0;
		protected override string SetBonusText => "Astronomical summon damage, 10 extra minions, and 10 seconds of crystal immortality after taking damage (3 minute cooldown).";

		protected override void ApplyClassBonuses(Player player, CrystalinePlayer modPlayer) {
			player.GetDamage(DamageClass.Summon) += 1.55f;
			player.maxMinions += 10;
			player.moveSpeed += 0.12f;
			player.maxRunSpeed += 0.45f;
			player.whipRangeMultiplier += 0.25f;
		}
	}
}
