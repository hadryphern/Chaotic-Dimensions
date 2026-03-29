using ChaoticDimensions.Common.Systems;
using ChaoticDimensions.Content.Bosses.Monthra;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Scenes
{
	public sealed class MonthraScene : ModSceneEffect
	{
		public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Monthra");

		public override bool IsSceneEffectActive(Player player) {
			if (!player.active || player.dead) {
				return false;
			}

			return MonthraIntroSystem.IsActive || NPC.AnyNPCs(ModContent.NPCType<MonthraBoss>());
		}
	}
}
