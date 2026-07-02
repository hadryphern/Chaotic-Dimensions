// Ativa a musica, o filtro e o ceu rosa durante a luta da Monthra.

using ChaoticDimensions.Common.Systems;
using ChaoticDimensions.Content.Bosses.Monthra;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Scenes
{
	public sealed class MonthraScene : ModSceneEffect
	{
		public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Monthra");

		// Confirma se esta cena deve ficar ativa para o jogador.
		public override bool IsSceneEffectActive(Player player) {
			if (!player.active || player.dead) {
				return false;
			}

			return MonthraIntroSystem.IsActive || NPC.AnyNPCs(ModContent.NPCType<MonthraBoss>());
		}

		// Liga ou desliga o filtro e o ceu desta cena.
		public override void SpecialVisuals(Player player, bool isActive) {
			if (Main.dedServ) {
				return;
			}

			player.ManageSpecialBiomeVisuals(ChaoticDimensions.MonthraGalaxySkyKey, isActive);
			if (isActive) {
				SkyManager.Instance.Activate(ChaoticDimensions.MonthraGalaxySkyKey, player.Center);
			}
			else {
				SkyManager.Instance.Deactivate(ChaoticDimensions.MonthraGalaxySkyKey);
			}
		}
	}
}
