using Terraria;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public class KrakenAbyssBreathBuff : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.breath = player.breathMax;
			player.gills = true;
			player.accFlipper = true;
			player.moveSpeed += 0.35f;
			player.maxRunSpeed += 2.4f;
			player.runAcceleration *= 1.25f;
		}
	}
}
