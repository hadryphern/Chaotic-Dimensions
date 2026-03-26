using Terraria;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Players;

namespace ChaoticDimensions.Content.Items.Armor.CrystalineDevour
{
	[AutoloadEquip(EquipType.Head)]
	public sealed class CrystalineDevourRangedHelm : CrystalineDevourHelmetBase
	{
		protected override CrystalineDevourSetType SetType => CrystalineDevourSetType.Ranged;
		protected override int BonusDefense => 111;
		protected override string SetBonusText => "Crushing ranged damage, extreme firing speed, colossal ammo economy, and 10 seconds of crystal immortality after taking damage (3 minute cooldown).";

		protected override void ApplyClassBonuses(Player player, CrystalinePlayer modPlayer) {
			player.GetDamage(DamageClass.Ranged) += 0.72f;
			player.GetAttackSpeed(DamageClass.Ranged) += 0.55f;
			player.moveSpeed += 0.24f;
			player.maxRunSpeed += 1.15f;
			player.ammoBox = true;
			player.ammoPotion = true;
			player.ammoCost80 = true;
			modPlayer.crystalineDevourRangedEconomy = true;
		}
	}
}
