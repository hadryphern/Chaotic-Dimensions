using Terraria;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Players;

namespace ChaoticDimensions.Content.Items.Armor.CrystalineDevour
{
	[AutoloadEquip(EquipType.Head)]
	public sealed class CrystalineDevourMeleeHelm : CrystalineDevourHelmetBase
	{
		protected override CrystalineDevourSetType SetType => CrystalineDevourSetType.Melee;
		protected override int BonusDefense => 151;
		protected override string SetBonusText => "Massive melee strength, faster movement, and 10 seconds of crystal immortality after taking damage (3 minute cooldown).";

		protected override void ApplyClassBonuses(Player player, CrystalinePlayer modPlayer) {
			player.GetDamage(DamageClass.Melee) += 0.85f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.35f;
			player.moveSpeed += 0.22f;
			player.maxRunSpeed += 0.9f;
		}
	}
}
