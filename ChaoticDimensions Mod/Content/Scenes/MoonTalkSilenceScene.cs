using ChaoticDimensions.Common.Systems;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Scenes
{
	public sealed class MoonTalkSilenceScene : ModSceneEffect
	{
		public override int Music => 0;

		public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

		public override float GetWeight(Player player) {
			return 1f;
		}

		public override bool IsSceneEffectActive(Player player) {
			return MoonTalkIntroSystem.IsActiveFor(player);
		}
	}
}
