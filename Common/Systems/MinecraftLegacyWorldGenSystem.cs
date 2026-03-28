using System.Collections.Generic;
using ChaoticDimensions.Common.Systems;
using ChaoticDimensions.Content.Tiles.MinecraftLegacy;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class MinecraftLegacyWorldGenSystem : ModSystem
	{
		public static LocalizedText RosalitaBlessingMessage { get; private set; }
		public static LocalizedText ShadowBlessingMessage { get; private set; }

		private bool generatedRosalitaOre;
		private bool generatedShadowOre;

		public override void SetStaticDefaults() {
			RosalitaBlessingMessage = Mod.GetLocalization("WorldGen.RosalitaBlessingMessage");
			ShadowBlessingMessage = Mod.GetLocalization("WorldGen.ShadowBlessingMessage");
		}

		public override void ClearWorld() {
			generatedRosalitaOre = false;
			generatedShadowOre = false;
		}

		public override void SaveWorldData(TagCompound tag) {
			if (generatedRosalitaOre) {
				tag["generatedRosalitaOre"] = true;
			}

			if (generatedShadowOre) {
				tag["generatedShadowOre"] = true;
			}
		}

		public override void LoadWorldData(TagCompound tag) {
			generatedRosalitaOre = tag.ContainsKey("generatedRosalitaOre");
			generatedShadowOre = tag.ContainsKey("generatedShadowOre");
		}

		public override void NetSend(System.IO.BinaryWriter writer) {
			writer.WriteFlags(generatedRosalitaOre, generatedShadowOre);
		}

		public override void NetReceive(System.IO.BinaryReader reader) {
			reader.ReadFlags(out generatedRosalitaOre, out generatedShadowOre);
		}

		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
			int shiniesIndex = tasks.FindIndex(pass => pass.Name.Equals("Shinies"));
			if (shiniesIndex < 0) {
				return;
			}

			tasks.Insert(shiniesIndex + 1, new PassLegacy("Chaotic Dimensions Legacy Ores", GenerateLegacyOres));
		}

		public override void PostUpdateWorld() {
			if (Main.gameMenu || Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			if (!generatedRosalitaOre && Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) {
				GenerateProgressionOre(ModContent.TileType<RosalitaOreTile>(), 0.44, 0.88, 34, 4, 6);
				generatedRosalitaOre = true;
				BroadcastOreBlessing(RosalitaBlessingMessage, new Color(255, 110, 186));
				NetMessage.SendData(MessageID.WorldData);
			}

			if (!generatedShadowOre && ChaoticDownedBossSystem.downedCrystalineDevourer) {
				ShadowBiomeWorldSystem.GenerateShadowOreInBiome(ModContent.TileType<ShadowOreTile>(), Main.netMode == NetmodeID.Server);
				generatedShadowOre = true;
				BroadcastOreBlessing(ShadowBlessingMessage, new Color(180, 100, 255));
				NetMessage.SendData(MessageID.WorldData);
			}
		}

		private static void GenerateLegacyOres(GenerationProgress progress, GameConfiguration configuration) {
			progress.Message = "Embedding legacy Chaotic ores";
		}

		private static void GenerateProgressionOre(int tileType, double minDepth, double maxDepth, int frequencyScale, int minStrength, int maxStrength) {
			GenerateOre(tileType, minDepth, maxDepth, frequencyScale, minStrength, maxStrength, Main.netMode == NetmodeID.Server);
		}

		private static void GenerateOre(int tileType, double minDepth, double maxDepth, int frequencyScale, int minStrength, int maxStrength, bool syncTiles) {
			int attempts = (int)(Main.maxTilesX * Main.maxTilesY * 6E-05 * frequencyScale / 100f);
			int minY = (int)(Main.maxTilesY * minDepth);
			int maxY = (int)(Main.maxTilesY * maxDepth);

			for (int i = 0; i < attempts; i++) {
				int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
				int y = WorldGen.genRand.Next(minY, maxY);
				double strength = WorldGen.genRand.Next(minStrength, maxStrength + 1);
				int steps = WorldGen.genRand.Next(3, 7);
				WorldGen.OreRunner(x, y, strength, steps, (ushort)tileType);

				if (syncTiles && Main.netMode == NetmodeID.Server) {
					NetMessage.SendTileSquare(-1, x, y, maxStrength + 8);
				}
			}
		}

		private static void BroadcastOreBlessing(LocalizedText message, Color color) {
			if (Main.netMode == NetmodeID.SinglePlayer) {
				Main.NewText(message.Value, color);
			}
			else if (Main.netMode == NetmodeID.Server) {
				ChatHelper.BroadcastChatMessage(message.ToNetworkText(), color);
			}
		}
	}
}
