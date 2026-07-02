// Desenha e atualiza os elementos visuais de Kraken Surface Background Style.

using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Backgrounds
{
	public class KrakenSurfaceBackgroundStyle : ModSurfaceBackgroundStyle
	{
		public override void ModifyFarFades(float[] fades, float transitionSpeed) {
			for (int i = 0; i < fades.Length; i++) {
				fades[i] = i == Slot ? 1f : 0f;
			}
		}

		public override int ChooseFarTexture() {
			return BackgroundTextureLoader.GetBackgroundSlot("ChaoticDimensions/Content/Backgrounds/KrakenGalaxyFar");
		}

		public override int ChooseMiddleTexture() {
			return BackgroundTextureLoader.GetBackgroundSlot("ChaoticDimensions/Content/Backgrounds/KrakenGalaxyMiddle");
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) {
			scale = 1.05f;
			parallax = 0.35;
			return BackgroundTextureLoader.GetBackgroundSlot("ChaoticDimensions/Content/Backgrounds/KrakenGalaxyClose");
		}
	}
}
