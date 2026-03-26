using Microsoft.Xna.Framework;

namespace ChaoticDimensions.Common.OreSpawn
{
	public enum OreSpawnDimensionId : byte
	{
		Overworld,
		Utopia,
		Village,
		Crystal,
		Mining,
		Danger,
		Chaos
	}

	public readonly record struct OreSpawnDimensionRegion(
		OreSpawnDimensionId Id,
		Rectangle TileBounds,
		Point DefaultSpawnTile);
}
