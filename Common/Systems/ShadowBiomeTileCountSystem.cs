using System;
using ChaoticDimensions.Content.Tiles.ShadowBiome;
using Terraria.ModLoader;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class ShadowBiomeTileCountSystem : ModSystem
	{
		public int ShadowTileCount { get; private set; }

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
			ShadowTileCount =
				tileCounts[ModContent.TileType<ShadowGrassTile>()] +
				tileCounts[ModContent.TileType<ShadowDirtTile>()] +
				tileCounts[ModContent.TileType<ShadowStoneTile>()] +
				tileCounts[ModContent.TileType<ShadowWoodTile>()];
		}
	}
}
