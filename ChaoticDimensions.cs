using ChaoticDimensions.Common.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ChaoticDimensions
{
	public class ChaoticDimensions : Mod
	{
		internal const string CrystalineDevourerSkyKey = "ChaoticDimensions:CrystalineDevourerSky";

		public override void Load() {
			if (Main.dedServ) {
				return;
			}

			Filters.Scene[CrystalineDevourerSkyKey] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.45f, 0.1f, 0.55f), EffectPriority.VeryHigh);
			SkyManager.Instance[CrystalineDevourerSkyKey] = new CrystalineDevourerSky();
		}

		public override void Unload() {
			if (Main.dedServ) {
				return;
			}

			try {
				Filters.Scene?[CrystalineDevourerSkyKey]?.Deactivate();
			}
			catch {
			}

			try {
				SkyManager.Instance?.Deactivate(CrystalineDevourerSkyKey);
			}
			catch {
			}
		}
	}
}
