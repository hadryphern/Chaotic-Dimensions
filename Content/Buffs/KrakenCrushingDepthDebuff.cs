using Terraria;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public class KrakenCrushingDepthDebuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.moveSpeed *= 0.72f;
			player.accRunSpeed *= 0.78f;
			player.statDefense -= 12;
			player.GetDamage(DamageClass.Generic) *= 0.9f;
		}
	}
}
