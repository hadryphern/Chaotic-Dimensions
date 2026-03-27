using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace ChaoticDimensions.Common.OreSpawn
{
	internal static class OreSpawnDimensionLayout
	{
		public const int LargeWorldWidthTiles = 8400;
		public const int LargeWorldHeightTiles = 2400;

		public static bool SupportsOreSpawnDimensions() {
			return false;
		}

		public static OreSpawnDimensionRegion GetRegion(OreSpawnDimensionId dimensionId) {
			int worldSurface = (int)Math.Max(240, Main.worldSurface);
			int rockLayer = (int)Math.Max(worldSurface + 120, Main.rockLayer);
			int centerX = Main.maxTilesX / 2;

			return dimensionId switch {
				OreSpawnDimensionId.Utopia => new OreSpawnDimensionRegion(
					dimensionId,
					new Rectangle(140, 120, 1150, Math.Min(1100, Main.maxTilesY - 360)),
					new Point(420, Math.Max(180, worldSurface - 48))),
				OreSpawnDimensionId.Village => new OreSpawnDimensionRegion(
					dimensionId,
					new Rectangle(Main.maxTilesX - 1290, 120, 1150, Math.Min(1100, Main.maxTilesY - 360)),
					new Point(Main.maxTilesX - 420, Math.Max(180, worldSurface - 40))),
				OreSpawnDimensionId.Mining => new OreSpawnDimensionRegion(
					dimensionId,
					new Rectangle(140, worldSurface + 80, 1650, Main.maxTilesY - worldSurface - 260),
					new Point(460, worldSurface + 220)),
				OreSpawnDimensionId.Crystal => new OreSpawnDimensionRegion(
					dimensionId,
					new Rectangle(centerX - 950, rockLayer + 80, 1900, Main.maxTilesY - rockLayer - 260),
					new Point(centerX, rockLayer + 180)),
				OreSpawnDimensionId.Danger => new OreSpawnDimensionRegion(
					dimensionId,
					new Rectangle(Main.maxTilesX - 2280, 70, 1900, 330),
					new Point(Main.maxTilesX - 1320, 150)),
				OreSpawnDimensionId.Chaos => new OreSpawnDimensionRegion(
					dimensionId,
					new Rectangle(centerX - 1500, 45, 3000, 280),
					new Point(centerX, 120)),
				_ => new OreSpawnDimensionRegion(OreSpawnDimensionId.Overworld, Rectangle.Empty, Point.Zero)
			};
		}

		public static bool TryGetRegion(OreSpawnDimensionId dimensionId, out OreSpawnDimensionRegion region) {
			region = GetRegion(dimensionId);
			return region.Id != OreSpawnDimensionId.Overworld && region.TileBounds != Rectangle.Empty;
		}

		public static bool TryGetDimensionAtWorldPosition(Vector2 worldPosition, out OreSpawnDimensionId dimensionId) {
			dimensionId = OreSpawnDimensionId.Overworld;
			if (!SupportsOreSpawnDimensions()) {
				return false;
			}

			Point tile = worldPosition.ToTileCoordinates();
			OreSpawnDimensionId[] orderedDimensions = [
				OreSpawnDimensionId.Chaos,
				OreSpawnDimensionId.Crystal,
				OreSpawnDimensionId.Mining,
				OreSpawnDimensionId.Utopia,
				OreSpawnDimensionId.Village
			];

			foreach (OreSpawnDimensionId candidate in orderedDimensions) {
				if (!TryGetRegion(candidate, out OreSpawnDimensionRegion region)) {
					continue;
				}

				if (region.TileBounds.Contains(tile)) {
					dimensionId = candidate;
					return true;
				}
			}

			return false;
		}

		public static bool IsOverworldPosition(Vector2 worldPosition) {
			return !TryGetDimensionAtWorldPosition(worldPosition, out _);
		}
	}
}
