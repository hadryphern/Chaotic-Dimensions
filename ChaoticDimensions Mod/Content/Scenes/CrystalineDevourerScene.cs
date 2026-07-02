// Ativa a musica e o ceu do Crystaline apenas durante o encontro.

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

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CrystalineDevour");

		// Confirma se esta cena deve ficar ativa para o jogador.
		public override bool IsSceneEffectActive(Player player) {
			if (!player.active || player.dead) {
				return false;
			}

			return CrystalineDevourerIntroSystem.IsActive ||
				(CrystalineDevourerArenaSystem.HasAnyLivingPlayers() && NPC.AnyNPCs(ModContent.NPCType<CrystalineDevourerHead>()));
		}

		// Liga ou desliga o filtro e o ceu desta cena.
		public override void SpecialVisuals(Player player, bool isActive) {
			if (!Main.dedServ) {
				player.ManageSpecialBiomeVisuals(ChaoticDimensions.CrystalineDevourerSkyKey, isActive);
				if (isActive) {
					SkyManager.Instance.Activate(ChaoticDimensions.CrystalineDevourerSkyKey, player.Center);
				}
				else {
					SkyManager.Instance.Deactivate(ChaoticDimensions.CrystalineDevourerSkyKey);
				}
			}
		}
	}
}
