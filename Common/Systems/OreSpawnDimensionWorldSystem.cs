using System.Collections.Generic;
using ChaoticDimensions.Common.OreSpawn;
using ChaoticDimensions.Content.Tiles.OreSpawn;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class OreSpawnDimensionWorldSystem : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
			int insertIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
			if (insertIndex < 0) {
				insertIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
			}

			if (insertIndex < 0) {
				insertIndex = tasks.Count - 1;
			}

			tasks.Insert(insertIndex + 1, new PassLegacy("Chaotic Dimensions OreSpawn", GenerateOreSpawnWorld));
		}

		private static void GenerateOreSpawnWorld(GenerationProgress progress, GameConfiguration configuration) {
			progress.Message = "Carving OreSpawn dimensions";

			if (!OreSpawnDimensionLayout.SupportsOreSpawnDimensions()) {
				return;
			}

			GenerateUtopia(progress);
			GenerateVillage(progress);
			GenerateMining(progress);
			GenerateCrystal(progress);
			GenerateDanger(progress);
			GenerateChaos(progress);
			GenerateTransportNests(progress);
		}

		private static void GenerateUtopia(GenerationProgress progress) {
			progress.Message = "Shaping Utopia Dimension";
			OreSpawnDimensionRegion region = OreSpawnDimensionLayout.GetRegion(OreSpawnDimensionId.Utopia);
			int baseSurface = region.DefaultSpawnTile.Y + 10;

			SculptSurfaceBand(region, baseSurface, 90, TileID.Dirt);

			for (int offset = 120; offset < region.TileBounds.Width - 120; offset += 170) {
				int x = region.TileBounds.Left + offset;
				int surfaceY = FindTopSolidTile(x, region.TileBounds.Top, region.TileBounds.Bottom);
				PlaceBlockTree(x, surfaceY - 1, WorldGen.genRand.Next(24, 36), WorldGen.genRand.Next(14, 22), (ushort)ModContent.TileType<SkyTreeLogTile>());
			}

			int goodnessX = region.TileBounds.Center.X;
			int goodnessSurface = FindTopSolidTile(goodnessX, region.TileBounds.Top, region.TileBounds.Bottom);
			PlaceBlockTree(goodnessX, goodnessSurface - 1, 56, 28, (ushort)ModContent.TileType<DuplicatorLogTile>());
		}

		private static void GenerateVillage(GenerationProgress progress) {
			progress.Message = "Raising Village Dimension";
			OreSpawnDimensionRegion region = OreSpawnDimensionLayout.GetRegion(OreSpawnDimensionId.Village);
			int baseSurface = region.DefaultSpawnTile.Y + 12;

			SculptSurfaceBand(region, baseSurface, 96, TileID.Dirt);

			int houseSpacing = 138;
			for (int houseIndex = 0; houseIndex < 7; houseIndex++) {
				int houseX = region.TileBounds.Left + 90 + (houseIndex * houseSpacing);
				int groundY = FindTopSolidTile(houseX + 10, region.TileBounds.Top, region.TileBounds.Bottom);
				PlaceVillageHouse(houseX, groundY - 1, 24, 13, (ushort)ModContent.TileType<SkyTreeLogTile>(), WallID.Wood);
			}
		}

		private static void GenerateMining(GenerationProgress progress) {
			progress.Message = "Digging Mining Dimension";
			OreSpawnDimensionRegion region = OreSpawnDimensionLayout.GetRegion(OreSpawnDimensionId.Mining);

			PlaceDungeonRoom(region.DefaultSpawnTile.X - 18, region.DefaultSpawnTile.Y - 8, 36, 12, (ushort)ModContent.TileType<TitaniumBlockTile>(), WallID.Stone);

			for (int caveIndex = 0; caveIndex < 72; caveIndex++) {
				int x = WorldGen.genRand.Next(region.TileBounds.Left + 40, region.TileBounds.Right - 40);
				int y = WorldGen.genRand.Next(region.TileBounds.Top + 40, region.TileBounds.Bottom - 40);
				CarveEllipse(x, y, WorldGen.genRand.Next(10, 30), WorldGen.genRand.Next(6, 14));
			}

			for (int roomIndex = 0; roomIndex < 12; roomIndex++) {
				int roomX = WorldGen.genRand.Next(region.TileBounds.Left + 60, region.TileBounds.Right - 120);
				int roomY = WorldGen.genRand.Next(region.TileBounds.Top + 80, region.TileBounds.Bottom - 100);
				PlaceDungeonRoom(roomX, roomY, WorldGen.genRand.Next(28, 42), WorldGen.genRand.Next(14, 20), (ushort)ModContent.TileType<TitaniumBlockTile>(), WallID.Stone);
			}

			ScatterOre(region, ModContent.TileType<TitaniumOreTile>(), 165, 6, 11, 4, 8);
			ScatterOre(region, ModContent.TileType<UraniumOreTile>(), 110, 5, 9, 3, 7);
			ScatterOre(region, ModContent.TileType<RubyOreTile>(), 140, 5, 9, 3, 7);
			ScatterOre(region, ModContent.TileType<TigersEyeOreTile>(), 150, 5, 9, 3, 7);
		}

		private static void GenerateCrystal(GenerationProgress progress) {
			progress.Message = "Building Crystal Dimension";
			OreSpawnDimensionRegion region = OreSpawnDimensionLayout.GetRegion(OreSpawnDimensionId.Crystal);

			PlaceDungeonRoom(region.DefaultSpawnTile.X - 18, region.DefaultSpawnTile.Y - 8, 36, 12, (ushort)ModContent.TileType<KyaniteBlockTile>(), WallID.Glass);

			for (int x = region.TileBounds.Left + 60; x < region.TileBounds.Right - 100; x += 120) {
				int corridorY = region.TileBounds.Top + 120 + ((x / 120) % 4 * 55);
				PlaceDungeonRoom(x, corridorY, 34, 16, (ushort)ModContent.TileType<KyaniteBlockTile>(), WallID.Glass);
			}

			for (int pillarIndex = 0; pillarIndex < 18; pillarIndex++) {
				int x = WorldGen.genRand.Next(region.TileBounds.Left + 40, region.TileBounds.Right - 40);
				int y = WorldGen.genRand.Next(region.TileBounds.Top + 40, region.TileBounds.Bottom - 90);
				PlaceCrystalPillar(x, y, WorldGen.genRand.Next(14, 28));
			}

			ScatterOre(region, ModContent.TileType<AmethystOreTile>(), 160, 5, 10, 3, 7);
			ScatterOre(region, ModContent.TileType<KyaniteOreTile>(), 165, 5, 10, 3, 8);
			ScatterOre(region, ModContent.TileType<SaltOreTile>(), 120, 5, 10, 3, 6);
			ScatterOre(region, ModContent.TileType<PinkTourmalineOreTile>(), 130, 4, 9, 3, 6);
		}

		private static void GenerateDanger(GenerationProgress progress) {
			progress.Message = "Forging Danger Dimension";
			OreSpawnDimensionRegion region = OreSpawnDimensionLayout.GetRegion(OreSpawnDimensionId.Danger);
			ClearRegion(region.TileBounds, true);

			PlaceFloatingIsland(region.DefaultSpawnTile.X, region.DefaultSpawnTile.Y + 14, 62, 11, TileID.Cloud, TileID.RainCloud);
			PlaceDungeonRoom(region.DefaultSpawnTile.X - 18, region.DefaultSpawnTile.Y - 10, 36, 12, (ushort)ModContent.TileType<TeleportBlockTile>(), WallID.ObsidianBrickUnsafe);

			for (int islandIndex = 0; islandIndex < 14; islandIndex++) {
				int x = WorldGen.genRand.Next(region.TileBounds.Left + 90, region.TileBounds.Right - 90);
				int y = WorldGen.genRand.Next(region.TileBounds.Top + 50, region.TileBounds.Bottom - 20);
				int halfWidth = WorldGen.genRand.Next(26, 56);
				int halfHeight = WorldGen.genRand.Next(6, 14);
				PlaceFloatingIsland(x, y, halfWidth, halfHeight, TileID.Cloud, TileID.RainCloud);

				if (WorldGen.genRand.NextBool(3)) {
					PlaceDungeonRoom(x - (halfWidth / 2), y - halfHeight - 16, WorldGen.genRand.Next(24, 36), WorldGen.genRand.Next(12, 18), (ushort)ModContent.TileType<TeleportBlockTile>(), WallID.ObsidianBrickUnsafe);
				}
			}

			ScatterOre(region, ModContent.TileType<UraniumOreTile>(), 70, 4, 7, 3, 5);
			ScatterOre(region, ModContent.TileType<RubyOreTile>(), 80, 4, 7, 3, 5);
			ScatterOre(region, ModContent.TileType<TigersEyeOreTile>(), 60, 4, 7, 3, 5);
		}

		private static void GenerateChaos(GenerationProgress progress) {
			progress.Message = "Tearing open Chaos Dimension";
			OreSpawnDimensionRegion region = OreSpawnDimensionLayout.GetRegion(OreSpawnDimensionId.Chaos);
			ClearRegion(region.TileBounds, true);

			PlaceFloatingIsland(region.DefaultSpawnTile.X, region.DefaultSpawnTile.Y + 12, 54, 10, TileID.Ebonstone, TileID.Cloud);
			PlaceDungeonRoom(region.DefaultSpawnTile.X - 16, region.DefaultSpawnTile.Y - 8, 32, 10, (ushort)ModContent.TileType<UraniumBlockTile>(), WallID.ObsidianBrickUnsafe);

			for (int islandIndex = 0; islandIndex < 18; islandIndex++) {
				int x = WorldGen.genRand.Next(region.TileBounds.Left + 80, region.TileBounds.Right - 80);
				int y = WorldGen.genRand.Next(region.TileBounds.Top + 30, region.TileBounds.Bottom - 20);
				int halfWidth = WorldGen.genRand.Next(18, 42);
				int halfHeight = WorldGen.genRand.Next(5, 12);
				PlaceFloatingIsland(x, y, halfWidth, halfHeight, WorldGen.genRand.NextBool() ? TileID.Ebonstone : TileID.Crimstone, TileID.Cloud);

				if (WorldGen.genRand.NextBool(4)) {
					PlaceDungeonRoom(x - (halfWidth / 2), y - halfHeight - 14, WorldGen.genRand.Next(18, 30), WorldGen.genRand.Next(10, 14), (ushort)ModContent.TileType<UraniumBlockTile>(), WallID.ObsidianBrickUnsafe);
				}
			}

			ScatterOre(region, ModContent.TileType<UraniumOreTile>(), 80, 4, 8, 3, 6);
			ScatterOre(region, ModContent.TileType<PinkTourmalineOreTile>(), 70, 4, 8, 3, 6);
			ScatterOre(region, ModContent.TileType<KyaniteOreTile>(), 60, 4, 8, 3, 6);
		}

		private static void GenerateTransportNests(GenerationProgress progress) {
			progress.Message = "Seeding OreSpawn nests";

			for (int nestIndex = 0; nestIndex < 18; nestIndex++) {
				bool termite = nestIndex % 2 == 1;
				int x = WorldGen.genRand.Next(220, Main.maxTilesX - 220);
				int y = (int)Main.worldSurface + WorldGen.genRand.Next(-20, 35);
				PlaceOreBlob(x, y, termite ? ModContent.TileType<TermiteNestTile>() : ModContent.TileType<RedAntNestTile>(), 7, 11);
			}
		}

		private static void SculptSurfaceBand(OreSpawnDimensionRegion region, int baseSurfaceY, int thickness, ushort fillTileType) {
			for (int x = region.TileBounds.Left; x < region.TileBounds.Right; x++) {
				float wave = (float)System.Math.Sin((x - region.TileBounds.Left) * 0.015f) * 14f;
				float wave2 = (float)System.Math.Sin((x - region.TileBounds.Left) * 0.043f) * 5f;
				int surfaceY = baseSurfaceY + (int)wave + (int)wave2;

				for (int y = region.TileBounds.Top; y < surfaceY; y++) {
					ClearTile(x, y, true);
				}

				for (int y = surfaceY; y < System.Math.Min(region.TileBounds.Bottom, surfaceY + thickness); y++) {
					SetSolidTile(x, y, fillTileType);
				}
			}
		}

		private static void PlaceVillageHouse(int left, int groundY, int width, int height, ushort shellTileType, ushort wallType) {
			for (int x = left; x < left + width; x++) {
				for (int y = groundY - height; y <= groundY; y++) {
					bool border = x == left || x == left + width - 1 || y == groundY - height || y == groundY;
					if (border) {
						SetSolidTile(x, y, shellTileType, wallType);
					}
					else {
						ClearTile(x, y, false);
						SetWall(x, y, wallType);
					}
				}
			}

			int doorX = left + (width / 2);
			for (int y = groundY - 3; y < groundY; y++) {
				ClearTile(doorX, y, false);
			}

			for (int y = groundY - height - 1; y < groundY - height + 3; y++) {
				SetSolidTile(doorX, y, shellTileType);
			}
		}

		private static void PlaceDungeonRoom(int left, int top, int width, int height, ushort shellTileType, ushort wallType) {
			for (int x = left; x < left + width; x++) {
				for (int y = top; y < top + height; y++) {
					bool border = x == left || x == left + width - 1 || y == top || y == top + height - 1;
					if (border) {
						SetSolidTile(x, y, shellTileType, wallType);
					}
					else {
						ClearTile(x, y, false);
						SetWall(x, y, wallType);
					}
				}
			}

			for (int floorY = top + 4; floorY < top + height - 2; floorY += 4) {
				for (int x = left + 2; x < left + width - 2; x++) {
					SetSolidTile(x, floorY, shellTileType, wallType);
				}
			}
		}

		private static void PlaceCrystalPillar(int centerX, int baseY, int height) {
			ushort logTile = (ushort)ModContent.TileType<CrystalTreeLogTile>();
			for (int y = baseY; y < baseY + height; y++) {
				SetSolidTile(centerX, y, logTile);
				if (y % 5 == 0) {
					SetSolidTile(centerX - 1, y, (ushort)ModContent.TileType<KyaniteBlockTile>());
					SetSolidTile(centerX + 1, y, (ushort)ModContent.TileType<KyaniteBlockTile>());
				}
			}
		}

		private static void PlaceBlockTree(int centerX, int groundY, int trunkHeight, int canopyRadius, ushort logTileType) {
			for (int y = groundY - trunkHeight; y <= groundY; y++) {
				SetSolidTile(centerX, y, logTileType);
				if (y > groundY - (trunkHeight / 2) && WorldGen.genRand.NextBool(4)) {
					SetSolidTile(centerX - 1, y, logTileType);
				}
			}

			int canopyCenterY = groundY - trunkHeight;
			for (int x = centerX - canopyRadius; x <= centerX + canopyRadius; x++) {
				for (int y = canopyCenterY - canopyRadius; y <= canopyCenterY + canopyRadius; y++) {
					float dx = (x - centerX) / (float)canopyRadius;
					float dy = (y - canopyCenterY) / (float)System.Math.Max(5, canopyRadius - 4);
					if ((dx * dx) + (dy * dy) <= 1f) {
						SetSolidTile(x, y, TileID.LeafBlock);
					}
				}
			}
		}

		private static void PlaceFloatingIsland(int centerX, int centerY, int halfWidth, int halfHeight, ushort lowerTileType, ushort upperTileType) {
			for (int x = centerX - halfWidth; x <= centerX + halfWidth; x++) {
				for (int y = centerY - halfHeight; y <= centerY + halfHeight; y++) {
					float dx = (x - centerX) / (float)halfWidth;
					float dy = (y - centerY) / (float)halfHeight;
					if ((dx * dx) + (dy * dy) > 1f) {
						continue;
					}

					ushort tileType = y <= centerY ? upperTileType : lowerTileType;
					SetSolidTile(x, y, tileType);
				}
			}
		}

		private static void CarveEllipse(int centerX, int centerY, int halfWidth, int halfHeight) {
			for (int x = centerX - halfWidth; x <= centerX + halfWidth; x++) {
				for (int y = centerY - halfHeight; y <= centerY + halfHeight; y++) {
					float dx = (x - centerX) / (float)halfWidth;
					float dy = (y - centerY) / (float)halfHeight;
					if ((dx * dx) + (dy * dy) <= 1f) {
						ClearTile(x, y, true);
					}
				}
			}
		}

		private static void ScatterOre(OreSpawnDimensionRegion region, int tileType, int attempts, int strengthMin, int strengthMax, int stepsMin, int stepsMax) {
			for (int index = 0; index < attempts; index++) {
				int x = WorldGen.genRand.Next(region.TileBounds.Left + 15, region.TileBounds.Right - 15);
				int y = WorldGen.genRand.Next(region.TileBounds.Top + 15, region.TileBounds.Bottom - 15);
				WorldGen.TileRunner(x, y, WorldGen.genRand.Next(strengthMin, strengthMax), WorldGen.genRand.Next(stepsMin, stepsMax), tileType);
			}
		}

		private static void PlaceOreBlob(int centerX, int centerY, int tileType, int strengthMin, int strengthMax) {
			WorldGen.TileRunner(centerX, centerY, WorldGen.genRand.Next(strengthMin, strengthMax), WorldGen.genRand.Next(4, 8), tileType);
		}

		private static void ClearRegion(Rectangle region, bool clearWalls) {
			for (int x = region.Left; x < region.Right; x++) {
				for (int y = region.Top; y < region.Bottom; y++) {
					ClearTile(x, y, clearWalls);
				}
			}
		}

		private static int FindTopSolidTile(int x, int top, int bottom) {
			for (int y = top; y < bottom; y++) {
				if (WorldGen.SolidTile(x, y)) {
					return y;
				}
			}

			return bottom;
		}

		private static void SetSolidTile(int x, int y, ushort tileType, ushort wallType = 0) {
			if (!WorldGen.InWorld(x, y, 10)) {
				return;
			}

			Tile tile = Main.tile[x, y];
			tile.HasTile = true;
			tile.TileType = tileType;
			tile.LiquidAmount = 0;
			tile.IsActuated = false;
			tile.WallType = wallType;
		}

		private static void SetWall(int x, int y, ushort wallType) {
			if (!WorldGen.InWorld(x, y, 10)) {
				return;
			}

			Main.tile[x, y].WallType = wallType;
		}

		private static void ClearTile(int x, int y, bool clearWall) {
			if (!WorldGen.InWorld(x, y, 10)) {
				return;
			}

			Tile tile = Main.tile[x, y];
			tile.HasTile = false;
			tile.LiquidAmount = 0;
			if (clearWall) {
				tile.WallType = WallID.None;
			}
		}
	}
}
