using ChaoticDimensions.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Scenes
{
	public sealed class ShadowBiome : ModBiome
	{
		public override int Music => MusicID.Corruption;
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;
		public override Color? BackgroundColor => new Color(36, 12, 56);

		public override bool IsBiomeActive(Player player) {
			return ShadowBiomeWorldSystem.IsWithinShadowBiomeX(player.Center.ToTileCoordinates().X) &&
				ModContent.GetInstance<ShadowBiomeTileCountSystem>().ShadowTileCount >= 140;
		}
	}
}
