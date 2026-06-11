using ChaoticDimensions.Common.Systems;
using ChaoticDimensions.Content.Backgrounds;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.SceneEffects
{
	public class KrakenSceneEffect : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/KrakenTheme");

		public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

		public override bool IsSceneEffectActive(Player player) {
			KrakenEventSystem system = KrakenEventSystem.Instance;
			return system.Active;
		}

		public override void SpecialVisuals(Player player, bool isActive) {
			if (Main.dedServ || !global::ChaoticDimensions.ChaoticDimensions.KrakenCosmicSkyRegistered || SkyManager.Instance == null) {
				return;
			}

			CustomSky sky = SkyManager.Instance[KrakenCosmicSky.EffectKey];
			if (sky == null) {
				SkyManager.Instance[KrakenCosmicSky.EffectKey] = new KrakenCosmicSky();
				sky = SkyManager.Instance[KrakenCosmicSky.EffectKey];
			}

			if (isActive && !sky.IsActive()) {
				SkyManager.Instance.Activate(KrakenCosmicSky.EffectKey, player.Center);
			}
			else if (!isActive && sky.IsActive()) {
				SkyManager.Instance.Deactivate(KrakenCosmicSky.EffectKey);
			}
		}
	}
}
