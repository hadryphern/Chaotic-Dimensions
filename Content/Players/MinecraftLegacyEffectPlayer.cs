using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Players
{
	public sealed class MinecraftLegacyEffectPlayer : ModPlayer
	{
		public float EndernmonIntensity { get; set; }

		public override void ResetEffects() {
			EndernmonIntensity = 0f;
		}

		public override void PostUpdateMiscEffects() {
			if (EndernmonIntensity <= 0f) {
				return;
			}

			float clamped = MathHelper.Clamp(EndernmonIntensity, 0f, 1f);
			Player.moveSpeed *= MathHelper.Lerp(1f, 0.28f, clamped);
			Player.maxRunSpeed *= MathHelper.Lerp(1f, 0.38f, clamped);
			Player.runAcceleration *= MathHelper.Lerp(1f, 0.45f, clamped);
			Player.lifeRegenTime = 0;
			Player.lifeRegen -= (int)MathHelper.Lerp(0f, 72f, clamped);

			Lighting.AddLight(Player.Center, 0.28f * clamped, 0f, 0.36f * clamped);
			if (Main.rand.NextFloat() < 0.2f * clamped) {
				Dust.NewDust(Player.position, Player.width, Player.height, DustID.Shadowflame, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
			}
		}
	}
}
