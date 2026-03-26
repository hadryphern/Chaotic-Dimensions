using Terraria;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Players;

namespace ChaoticDimensions.Content.Items.Armor.CrystalineDevour
{
	[AutoloadEquip(EquipType.Head)]
	public sealed class CrystalineDevourMagicHelm : CrystalineDevourHelmetBase
	{
		protected override CrystalineDevourSetType SetType => CrystalineDevourSetType.Magic;
		protected override int BonusDefense => 51;
		protected override string SetBonusText => "Overwhelming magic power, near-null mana cost, and 10 seconds of crystal immortality after taking damage (3 minute cooldown).";

		protected override void ApplyClassBonuses(Player player, CrystalinePlayer modPlayer) {
			player.GetDamage(DamageClass.Magic) += 0.78f;
			player.manaCost -= 0.95f;
			player.moveSpeed += 0.2f;
			player.maxRunSpeed += 0.75f;
		}
	}
}
