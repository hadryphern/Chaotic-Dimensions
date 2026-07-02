// Liga a musica e o filtro espectral Moon Lord ao evento do Kraken.

using ChaoticDimensions.Common.Systems;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.SceneEffects
{
	public class KrakenSceneEffect : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/KrakenTheme");

		public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

		// Confirma se esta cena deve ficar ativa para o jogador.
		public override bool IsSceneEffectActive(Player player) {
			KrakenEventSystem system = KrakenEventSystem.Instance;
			return system.Active;
		}

		// Liga ou desliga o filtro e o ceu desta cena.
		public override void SpecialVisuals(Player player, bool isActive) {
			if (Main.dedServ) {
				return;
			}

			player.ManageSpecialBiomeVisuals("MoonLord", isActive, player.Center);
		}
	}
}
