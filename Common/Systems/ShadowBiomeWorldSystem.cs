using System;
using System.IO;
using System.Collections.Generic;
using ChaoticDimensions.Content.Tiles.ShadowBiome;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class ShadowBiomeWorldSystem : ModSystem
	{
		public static int ShadowStartX { get; private set; }
		public static int ShadowEndX { get; private set; }

		public static bool HasShadowBiomeBounds => ShadowEndX > ShadowStartX;

		public static bool IsWithinShadowBiomeX(int tileX) {
			return HasShadowBiomeBounds && tileX >= ShadowStartX && tileX <= ShadowEndX;
		}

		public override void ClearWorld() {
			ShadowStartX = 0;
			ShadowEndX = 0;
		}

		public override void SaveWorldData(TagCompound tag) {
			if (HasShadowBiomeBounds) {
				tag["shadowStartX"] = ShadowStartX;
				tag["shadowEndX"] = ShadowEndX;
			}
		}

		public override void LoadWorldData(TagCompound tag) {
			ShadowStartX = tag.GetInt("shadowStartX");
			ShadowEndX = tag.GetInt("shadowEndX");
		}

		public override void NetSend(BinaryWriter writer) {
			writer.Write(ShadowStartX);
			writer.Write(ShadowEndX);
		}

		public override void NetReceive(BinaryReader reader) {
			ShadowStartX = reader.ReadInt32();
			ShadowEndX = reader.ReadInt32();
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
			int insertIndex = tasks.FindIndex(pass => pass.Name.Equals("Corruption") || pass.Name.Equals("Crimson"));
			if (insertIndex < 0) {
				insertIndex = tasks.FindIndex(pass => pass.Name.Equals("Micro Biomes"));
			}

			if (insertIndex < 0) {
				insertIndex = tasks.FindIndex(pass => pass.Name.Equals("Shinies"));
			}

			if (insertIndex < 0) {
				return;
			}

			tasks.Insert(insertIndex + 1, new PassLegacy("Chaotic Dimensions Shadow Biome", GenerateShadowBiome));
		}

		private static void GenerateShadowBiome(GenerationProgress progress, GameConfiguration configuration) {
			progress.Message = "Burying the western blight in shadow";

			(bool foundSegment, int regionStartX, int regionEndX) = FindLeftmostEvilBiomeSegment();
			if (foundSegment) {
				ShadowStartX = regionStartX;
				ShadowEndX = regionEndX;
			}
			else {
				int biomeWidth = Math.Max(110, Main.maxTilesX / 14);
				ShadowStartX = Math.Max(180, Main.maxTilesX / 10);
				ShadowEndX = Math.Min(Main.maxTilesX / 4, ShadowStartX + biomeWidth);
			}

			int shadowGrass = ModContent.TileType<ShadowGrassTile>();
			int shadowDirt = ModContent.TileType<ShadowDirtTile>();
			int shadowStone = ModContent.TileType<ShadowStoneTile>();
			int caveCenterX = (ShadowStartX + ShadowEndX) / 2;
			int caveHalfWidth = Math.Max(28, (ShadowEndX - ShadowStartX) / 4);
			int caveBandTop = (int)Main.rockLayer + 45;
			int caveBandBottom = Math.Min(Main.maxTilesY - 260, caveBandTop + 180);

			for (int x = ShadowStartX; x <= ShadowEndX; x++) {
				int surfaceY = FindSurfaceAtX(x);
				if (surfaceY <= 50) {
					surfaceY = (int)Main.worldSurface - 6;
				}

				int surfaceBandTop = Math.Max(40, surfaceY - 8);
				int surfaceBandBottom = Math.Min((int)Main.rockLayer + 40, surfaceY + 140);
				ConvertVerticalBand(x, surfaceBandTop, surfaceBandBottom, surfaceY, shadowGrass, shadowDirt, shadowStone);

				if (Math.Abs(x - caveCenterX) <= caveHalfWidth) {
					ConvertVerticalBand(x, caveBandTop, caveBandBottom, -1, shadowGrass, shadowDirt, shadowStone);
				}

				EnsureSurfaceCap(x, surfaceY, shadowGrass, shadowDirt);
			}

			GenerateDeadTrees();
		}

		private static bool ShouldConvertToShadowSoil(int tileType) {
			return tileType == TileID.Dirt ||
				tileType == TileID.CorruptGrass ||
				tileType == TileID.CrimsonGrass ||
				tileType == TileID.Ebonsand ||
				tileType == TileID.Crimsand;
		}

		private static bool ShouldConvertToShadowStone(int tileType) {
			return tileType == TileID.Stone ||
				tileType == TileID.Ebonstone ||
				tileType == TileID.Crimstone ||
				tileType == TileID.CorruptIce ||
				tileType == TileID.FleshIce;
		}

		private static (bool foundSegment, int regionStartX, int regionEndX) FindLeftmostEvilBiomeSegment() {
			int scanStartX = Math.Max(120, Main.maxTilesX / 20);
			int scanEndX = Math.Max(scanStartX + 120, Main.maxTilesX / 2);
			int segmentStart = -1;
			int lastSolidColumn = -1;

			for (int x = scanStartX; x <= scanEndX; x++) {
				bool evilColumn = CountEvilTilesInColumn(x) >= 8;
				if (evilColumn) {
					if (segmentStart < 0) {
						segmentStart = x;
					}

					lastSolidColumn = x;
				}
				else if (segmentStart >= 0) {
					int width = lastSolidColumn - segmentStart + 1;
					if (width >= 70) {
						return (true, segmentStart, lastSolidColumn);
					}

					segmentStart = -1;
					lastSolidColumn = -1;
				}
			}

			if (segmentStart >= 0 && lastSolidColumn >= segmentStart && (lastSolidColumn - segmentStart + 1) >= 70) {
				return (true, segmentStart, lastSolidColumn);
			}

			return (false, 0, 0);
		}

		private static int CountEvilTilesInColumn(int x) {
			int count = 0;
			int startY = 60;
			int endY = Math.Min(Main.maxTilesY - 260, (int)Main.rockLayer + 300);

			for (int y = startY; y <= endY; y += 2) {
				Tile tile = Framing.GetTileSafely(x, y);
				if (tile.HasTile && IsEvilBiomeTile(tile.TileType)) {
					count++;
				}
			}

			return count;
		}

		private static bool IsEvilBiomeTile(int tileType) {
			return tileType == TileID.CorruptGrass ||
				tileType == TileID.Ebonstone ||
				tileType == TileID.Ebonsand ||
				tileType == TileID.CorruptIce ||
				tileType == TileID.CrimsonGrass ||
				tileType == TileID.Crimstone ||
				tileType == TileID.Crimsand ||
				tileType == TileID.FleshIce;
		}

		private static int FindSurfaceAtX(int x) {
			for (int y = 80; y < Main.worldSurface + 80; y++) {
				Tile tile = Framing.GetTileSafely(x, y);
				Tile tileAbove = Framing.GetTileSafely(x, y - 1);
				if (tile.HasTile && Main.tileSolid[tile.TileType] && !tileAbove.HasTile) {
					return y;
				}
			}

			return (int)Main.worldSurface;
		}

		private static void ConvertVerticalBand(int x, int startY, int endY, int surfaceY, int shadowGrass, int shadowDirt, int shadowStone) {
			for (int y = startY; y <= endY; y++) {
				Tile tile = Framing.GetTileSafely(x, y);
				if (!tile.HasTile) {
					continue;
				}

				if (ShouldConvertToShadowSoil(tile.TileType)) {
					bool surfaceCap = surfaceY >= 0 && y <= surfaceY + 2;
					tile.TileType = (ushort)(surfaceCap ? shadowGrass : shadowDirt);
					tile.IsHalfBlock = false;
					tile.Slope = 0;
				}
				else if (ShouldConvertToShadowStone(tile.TileType)) {
					tile.TileType = (ushort)shadowStone;
					tile.IsHalfBlock = false;
					tile.Slope = 0;
				}
			}
		}

		private static void EnsureSurfaceCap(int x, int surfaceY, int grassTileType, int dirtTileType) {
			for (int y = surfaceY - 6; y <= surfaceY + 12; y++) {
				if (!WorldGen.InWorld(x, y, 20)) {
					continue;
				}

				Tile tile = Framing.GetTileSafely(x, y);
				Tile tileAbove = Framing.GetTileSafely(x, y - 1);

				if (tile.HasTile && tile.TileType == dirtTileType && !tileAbove.HasTile) {
					tile.TileType = (ushort)grassTileType;
					break;
				}
			}
		}

		private static void GenerateDeadTrees() {
			int woodTileType = ModContent.TileType<ShadowWoodTile>();
			int treeCount = Math.Max(8, (ShadowEndX - ShadowStartX) / 58);

			for (int i = 0; i < treeCount; i++) {
				int x = WorldGen.genRand.Next(ShadowStartX + 16, ShadowEndX - 16);
				int groundY = FindShadowGround(x);
				if (groundY <= 0) {
					continue;
				}

				int trunkHeight = WorldGen.genRand.Next(12, 24);
				int trunkWidth = WorldGen.genRand.NextBool(3) ? 2 : 1;

				for (int y = groundY - 1; y >= groundY - trunkHeight; y--) {
					for (int width = 0; width < trunkWidth; width++) {
						WorldGen.PlaceTile(x + width, y, woodTileType, mute: true, forced: true);
					}

					if (WorldGen.genRand.NextBool(7) && y < groundY - 5) {
						GenerateBranch(x + trunkWidth - 1, y, woodTileType, WorldGen.genRand.NextBool() ? -1 : 1);
					}
				}
			}
		}

		private static void GenerateBranch(int startX, int startY, int woodTileType, int direction) {
			int branchLength = WorldGen.genRand.Next(3, 7);
			for (int step = 1; step <= branchLength; step++) {
				int x = startX + (step * direction);
				int y = startY - (step / 3);
				if (!WorldGen.InWorld(x, y, 20)) {
					break;
				}

				WorldGen.PlaceTile(x, y, woodTileType, mute: true, forced: true);
			}
		}

		private static int FindShadowGround(int x) {
			for (int y = 90; y < Main.maxTilesY / 2; y++) {
				Tile tile = Framing.GetTileSafely(x, y);
				Tile tileAbove = Framing.GetTileSafely(x, y - 1);
				if (tile.HasTile && tile.TileType == ModContent.TileType<ShadowGrassTile>() && !tileAbove.HasTile) {
					return y;
				}
			}

			return -1;
		}

		public static void GenerateShadowOreInBiome(int tileType, bool syncTiles) {
			if (!HasShadowBiomeBounds) {
				return;
			}

			int attempts = Math.Max(120, (ShadowEndX - ShadowStartX));
			int minY = (int)Main.rockLayer + 55;
			int maxY = Math.Min(Main.maxTilesY - 250, minY + 220);

			for (int i = 0; i < attempts; i++) {
				int x = WorldGen.genRand.Next(ShadowStartX + 10, ShadowEndX - 10);
				int y = WorldGen.genRand.Next(minY, maxY);
				double strength = WorldGen.genRand.Next(5, 9);
				int steps = WorldGen.genRand.Next(4, 8);
				WorldGen.OreRunner(x, y, strength, steps, (ushort)tileType);

				if (syncTiles && Main.netMode == NetmodeID.Server) {
					NetMessage.SendTileSquare(-1, x, y, 16);
				}
			}
		}
	}
}
