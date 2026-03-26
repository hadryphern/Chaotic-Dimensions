using ChaoticDimensions.Content.Bosses.CrystalineDevourer;
using ChaoticDimensions.Common.Systems;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Scenes
{
	public sealed class CrystalineDevourerScene : ModSceneEffect
	{
		public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CrystalineWorm");

		public override bool IsSceneEffectActive(Player player) {
			if (!player.active || player.dead) {
				return false;
			}

			return CrystalineDevourerIntroSystem.IsActive ||
				(CrystalineDevourerArenaSystem.HasAnyLivingPlayers() && NPC.AnyNPCs(ModContent.NPCType<CrystalineDevourerHead>()));
		}

		public override void SpecialVisuals(Player player, bool isActive) {
			player.ManageSpecialBiomeVisuals(ChaoticDimensions.CrystalineDevourerSkyKey, isActive);
			if (isActive) {
				SkyManager.Instance.Activate(ChaoticDimensions.CrystalineDevourerSkyKey);
			}
			else {
				SkyManager.Instance.Deactivate(ChaoticDimensions.CrystalineDevourerSkyKey);
			}
		}
	}
}
