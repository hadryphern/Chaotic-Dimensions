using System.Collections.Generic;
using ChaoticDimensions.Content.Tiles.MinecraftLegacy;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class MinecraftLegacyWorldGenSystem : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
			int shiniesIndex = tasks.FindIndex(pass => pass.Name.Equals("Shinies"));
			if (shiniesIndex < 0) {
				return;
			}

			tasks.Insert(shiniesIndex + 1, new PassLegacy("Chaotic Dimensions Legacy Ores", GenerateLegacyOres));
		}

		private static void GenerateLegacyOres(GenerationProgress progress, GameConfiguration configuration) {
			progress.Message = "Embedding legacy Chaotic ores";

			GenerateOre(ModContent.TileType<RubyOreTile>(), 0.26, 0.58, 50, 4, 7);
			GenerateOre(ModContent.TileType<ShadowOreTile>(), 0.42, 0.82, 38, 4, 6);
			GenerateOre(ModContent.TileType<RosalitaOreTile>(), 0.52, 0.88, 28, 3, 5);
		}

		private static void GenerateOre(int tileType, double minDepth, double maxDepth, int frequencyScale, int minStrength, int maxStrength) {
			int attempts = (int)(Main.maxTilesX * Main.maxTilesY * 6E-05 * frequencyScale / 100f);
			int minY = (int)(Main.maxTilesY * minDepth);
			int maxY = (int)(Main.maxTilesY * maxDepth);

			for (int i = 0; i < attempts; i++) {
				int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				int y = WorldGen.genRand.Next(minY, maxY);
				double strength = WorldGen.genRand.Next(minStrength, maxStrength + 1);
				int steps = WorldGen.genRand.Next(3, 7);
				WorldGen.OreRunner(x, y, strength, steps, (ushort)tileType);
			}
		}
	}
}
