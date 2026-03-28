using ChaoticDimensions.Content.Scenes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Players
{
	public sealed class ShadowBiomePlayer : ModPlayer
	{
		public bool shadowBlessing;
		public bool ShadowBiomeActive => Player.InModBiome<ShadowBiome>();

		public override void ResetEffects() {
			shadowBlessing = false;
		}

		public override void UpdateDead() {
			shadowBlessing = false;
		}

		public override void PostUpdateMiscEffects() {
			if (!ShadowBiomeActive || shadowBlessing) {
				return;
			}

			Player.AddBuff(BuffID.Darkness, 2);
			Player.lifeRegenTime = 0;
			Player.lifeRegen -= 90;
			Player.moveSpeed *= 0.68f;
			Player.maxRunSpeed *= 0.72f;
			Player.runAcceleration *= 0.7f;
			Lighting.AddLight(Player.Center, 0.02f, 0f, 0.04f);

			if (Main.rand.NextBool(3)) {
				Dust.NewDust(Player.position, Player.width, Player.height, DustID.Shadowflame, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 160, new Color(80, 0, 100), 1.1f);
			}
		}
	}
}
