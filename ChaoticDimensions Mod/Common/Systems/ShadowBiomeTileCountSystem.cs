// Mantem o estado global e o ciclo de vida de Shadow Biome Tile Count System.

using System;
using ChaoticDimensions.Content.Tiles.ShadowBiome;
using Terraria.ModLoader;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class ShadowBiomeTileCountSystem : ModSystem
	{
		public int ShadowTileCount { get; private set; }

		// Conta os tiles usados para detetar o bioma.
		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
			ShadowTileCount =
				tileCounts[ModContent.TileType<ShadowGrassTile>()] +
				tileCounts[ModContent.TileType<ShadowDirtTile>()] +
				tileCounts[ModContent.TileType<ShadowStoneTile>()] +
				tileCounts[ModContent.TileType<ShadowWoodTile>()];
		}
	}
}
