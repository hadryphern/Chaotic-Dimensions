using System;
using System.Collections.Generic;
using ChaoticDimensions.Common.OreSpawn;
using ChaoticDimensions.Content.Tiles.OreSpawn;
using Microsoft.Xna.Framework;
using SubworldLibrary;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ChaoticDimensions.Content.Subworlds.OreSpawn
{
	public sealed class DangerDimensionSubworld : Subworld
	{
		private const int WorldWidthTiles = 5600;
		private const int WorldHeightTiles = 1400;
		private const int SurfaceY = 360;
		private const int RockLayerY = 620;
		private const int SpawnTileX = 260;
		private const int SpawnTileY = SurfaceY - 3;

		public override int Width => WorldWidthTiles;
		public override int Height => WorldHeightTiles;
		public override bool ShouldSave => true;

		public override List<GenPass> Tasks => new() {
			new PassLegacy("Danger Terrain", GenerateTerrain),
			new PassLegacy("Danger Structures", GenerateStructures),
			new PassLegacy("Danger Ores", GenerateOres),
			new PassLegacy("Danger Polish", FinalizeWorld)
		};

		public override void CopyMainWorldData() => OreSpawnSubworldStateBridge.CopyCurrentWorldState();

		public override void ReadCopiedMainWorldData() => OreSpawnSubworldStateBridge.ReadCopiedState();

		public override void CopySubworldData() => OreSpawnSubworldStateBridge.CopyCurrentWorldState();

		public override void ReadCopiedSubworldData() => OreSpawnSubworldStateBridge.ReadCopiedState();

		public override void OnEnter() {
			if (Main.netMode != NetmodeID.Server) {
				Main.NewText("The Danger Dimension tears open around you.", 230, 105, 85);
			}
		}

		private static void GenerateTerrain(GenerationProgress progress, GameConfiguration configuration) {
			progress.Message = "Flattening the Danger Dimension";

			Main.spawnTileX = SpawnTileX;
			Main.spawnTileY = SpawnTileY;
			Main.worldSurface = SurfaceY;
			Main.rockLayer = RockLayerY;

			for (int x = 0; x < Main.maxTilesX; x++) {
				int surfaceOffset = (int)(Math.Sin(x * 0.009f) * 4f) + (int)(Math.Sin(x * 0.031f) * 2f);
				int surface = SurfaceY + surfaceOffset;

				for (int y = 0; y < Main.maxTilesY; y++) {
					if (y < surface) {
						ClearTile(x, y, true);
						continue;
					}

					if (y == surface) {
						SetSolidTile(x, y, TileID.Grass);
						continue;
					}

					if (y < RockLayerY) {
						SetSolidTile(x, y, TileID.Dirt);
						continue;
					}

					if (y < Main.maxTilesY - 140) {
						SetSolidTile(x, y, TileID.Stone);
						continue;
					}

					SetSolidTile(x, y, TileID.Obsidian);
				}
			}

			BuildSpawnShelf();
			PlaceSparseTrees();
		}

		private static void GenerateStructures(GenerationProgress progress, GameConfiguration configuration) {
			progress.Message = "Packing the Danger Dimension with dungeons";

			for (int index = 0; index < 22; index++) {
				int width = WorldGen.genRand.Next(28, 54);
				int height = WorldGen.genRand.Next(12, 22);
				int left = 340 + (index * 220) + WorldGen.genRand.Next(-36, 36);
				int top = SurfaceY - height - WorldGen.genRand.Next(4, 16);
				PlaceDungeonShell(left, top, width, height, GetDangerShellTile(index), GetDangerWall(index), true, true);

				if (WorldGen.genRand.NextBool(3)) {
					PlaceTower(left + WorldGen.genRand.Next(4, width - 8), top - WorldGen.genRand.Next(10, 22), WorldGen.genRand.Next(10, 18), GetDangerShellTile(index));
				}
			}

			for (int index = 0; index < 14; index++) {
				int width = WorldGen.genRand.Next(20, 38);
				int height = WorldGen.genRand.Next(10, 16);
				int left = 420 + (index * 330) + WorldGen.genRand.Next(-50, 50);
				int top = 130 + WorldGen.genRand.Next(-30, 120);
				PlaceFloatingDungeon(left, top, width, height, GetDangerShellTile(index + 50), GetDangerWall(index + 50));
			}

			for (int index = 0; index < 18; index++) {
				int width = WorldGen.genRand.Next(24, 44);
				int height = WorldGen.genRand.Next(12, 20);
				int left = 260 + (index * 270) + WorldGen.genRand.Next(-45, 45);
				int top = RockLayerY + 90 + WorldGen.genRand.Next(0, 430);
				PlaceDungeonShell(left, top, width, height, GetDangerShellTile(index + 100), GetDangerWall(index + 100), true, false);

				if (WorldGen.genRand.NextBool(2)) {
					CarveTunnel(left + (width / 2), top - 16, left + (width / 2) + WorldGen.genRand.Next(-24, 24), SurfaceY + 8, 3);
				}
			}
		}

		private static void GenerateOres(GenerationProgress progress, GameConfiguration configuration) {
			progress.Message = "Infusing danger ores";
			ScatterOre(ModContent.TileType<UraniumOreTile>(), 150, 5, 10, RockLayerY - 30, Main.maxTilesY - 150);
			ScatterOre(ModContent.TileType<TitaniumOreTile>(), 170, 5, 10, RockLayerY - 10, Main.maxTilesY - 150);
			ScatterOre(ModContent.TileType<RubyOreTile>(), 150, 5, 9, SurfaceY + 20, Main.maxTilesY - 180);
			ScatterOre(ModContent.TileType<TigersEyeOreTile>(), 160, 5, 9, SurfaceY + 20, Main.maxTilesY - 180);
		}

		private static void FinalizeWorld(GenerationProgress progress, GameConfiguration configuration) {
			progress.Message = "Settling the Danger Dimension";
			Main.spawnTileX = SpawnTileX;
			Main.spawnTileY = SpawnTileY;
		}

		private static void BuildSpawnShelf() {
			for (int x = SpawnTileX - 20; x <= SpawnTileX + 24; x++) {
				for (int y = SpawnTileY + 1; y <= SpawnTileY + 5; y++) {
					SetSolidTile(x, y, TileID.Grass);
				}
			}

			for (int x = SpawnTileX - 6; x <= SpawnTileX + 6; x++) {
				ClearTile(x, SpawnTileY - 1, true);
				ClearTile(x, SpawnTileY - 2, true);
			}
		}

		private static void PlaceSparseTrees() {
			for (int index = 0; index < 14; index++) {
				int x = 480 + (index * 360) + WorldGen.genRand.Next(-70, 70);
				int height = WorldGen.genRand.Next(18, 34);
				int canopy = WorldGen.genRand.Next(9, 15);
				PlaceLivingTree(x, SurfaceY, height, canopy);
			}
		}

		private static void PlaceLivingTree(int centerX, int groundY, int trunkHeight, int canopyRadius) {
			for (int y = groundY - trunkHeight; y < groundY; y++) {
				SetSolidTile(centerX, y, TileID.LivingWood);
				if (y > groundY - (trunkHeight / 2) && WorldGen.genRand.NextBool(4)) {
					int sideOffset = WorldGen.genRand.NextBool() ? 1 : -1;
					SetSolidTile(centerX + sideOffset, y, TileID.LivingWood);
				}
			}

			int canopyCenterY = groundY - trunkHeight;
			for (int x = centerX - canopyRadius; x <= centerX + canopyRadius; x++) {
				for (int y = canopyCenterY - canopyRadius; y <= canopyCenterY + canopyRadius; y++) {
					float dx = (x - centerX) / (float)canopyRadius;
					float dy = (y - canopyCenterY) / (float)Math.Max(4, canopyRadius - 2);
					if ((dx * dx) + (dy * dy) <= 1f) {
						SetSolidTile(x, y, TileID.LeafBlock);
					}
				}
			}

			for (int branchIndex = 0; branchIndex < 3; branchIndex++) {
				int direction = branchIndex % 2 == 0 ? -1 : 1;
				int startY = groundY - (trunkHeight / 3) - (branchIndex * 3);
				int length = WorldGen.genRand.Next(6, 12);
				for (int step = 0; step < length; step++) {
					int x = centerX + (direction * step);
					int y = startY - (step / 3);
					SetSolidTile(x, y, TileID.LivingWood);
					if (step > length / 2) {
						SetSolidTile(x, y - 1, TileID.LeafBlock);
						SetSolidTile(x, y + 1, TileID.LeafBlock);
					}
				}
			}
		}

		private static void PlaceFloatingDungeon(int left, int top, int width, int height, ushort shellTileType, ushort wallType) {
			PlaceDungeonShell(left, top, width, height, shellTileType, wallType, false, true);

			int centerX = left + (width / 2);
			for (int y = top + height; y < top + height + 16; y++) {
				SetSolidTile(centerX, y, TileID.Chain);
			}

			for (int platformX = left + 3; platformX < left + width - 3; platformX++) {
				SetSolidTile(platformX, top + height + 16, TileID.Cloud);
			}
		}

		private static void PlaceTower(int centerX, int top, int height, ushort shellTileType) {
			for (int y = top; y < top + height; y++) {
				SetSolidTile(centerX - 1, y, shellTileType);
				SetSolidTile(centerX + 1, y, shellTileType);
				ClearTile(centerX, y, false);
			}

			for (int x = centerX - 4; x <= centerX + 4; x++) {
				SetSolidTile(x, top, shellTileType);
			}
		}

		private static void PlaceDungeonShell(int left, int top, int width, int height, ushort shellTileType, ushort wallType, bool addFloors, bool carveSupport) {
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

			if (addFloors) {
				for (int y = top + 4; y < top + height - 2; y += 4) {
					for (int x = left + 2; x < left + width - 2; x++) {
						SetSolidTile(x, y, shellTileType, wallType);
					}
				}
			}

			int doorX = left + (width / 2);
			for (int y = top + height - 4; y < top + height; y++) {
				ClearTile(doorX, y, false);
			}

			if (!carveSupport) {
				return;
			}

			for (int x = left + 2; x < left + width - 2; x += 6) {
				for (int y = top + height; y < Math.Min(Main.maxTilesY - 5, top + height + 12); y++) {
					SetSolidTile(x, y, shellTileType);
				}
			}
		}

		private static void ScatterOre(int tileType, int attempts, int strengthMin, int strengthMax, int minY, int maxY) {
			for (int index = 0; index < attempts; index++) {
				int x = WorldGen.genRand.Next(80, Main.maxTilesX - 80);
				int y = WorldGen.genRand.Next(minY, maxY);
				WorldGen.TileRunner(x, y, WorldGen.genRand.Next(strengthMin, strengthMax), WorldGen.genRand.Next(4, 8), tileType);
			}
		}

		private static void CarveTunnel(int startX, int startY, int endX, int endY, int radius) {
			int steps = Math.Max(Math.Abs(endX - startX), Math.Abs(endY - startY));
			if (steps <= 0) {
				return;
			}

			for (int step = 0; step <= steps; step++) {
				float progress = step / (float)steps;
				int x = (int)MathHelper.Lerp(startX, endX, progress);
				int y = (int)MathHelper.Lerp(startY, endY, progress);
				for (int offsetX = -radius; offsetX <= radius; offsetX++) {
					for (int offsetY = -radius; offsetY <= radius; offsetY++) {
						if ((offsetX * offsetX) + (offsetY * offsetY) <= radius * radius) {
							ClearTile(x + offsetX, y + offsetY, true);
						}
					}
				}
			}
		}

		private static ushort GetDangerShellTile(int seed) {
			return seed % 4 switch {
				0 => (ushort)ModContent.TileType<TeleportBlockTile>(),
				1 => (ushort)ModContent.TileType<EnderPearlBlockTile>(),
				2 => (ushort)ModContent.TileType<EyeOfEnderBlockTile>(),
				_ => TileID.ObsidianBrick
			};
		}

		private static ushort GetDangerWall(int seed) {
			return seed % 3 switch {
				0 => WallID.ObsidianBrickUnsafe,
				1 => WallID.Stone,
				_ => WallID.DirtUnsafe
			};
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
			if (wallType > 0) {
				tile.WallType = wallType;
			}
		}

		private static void SetWall(int x, int y, ushort wallType) {
			if (WorldGen.InWorld(x, y, 10)) {
				Main.tile[x, y].WallType = wallType;
			}
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
