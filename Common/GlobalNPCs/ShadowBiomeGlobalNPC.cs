using System.Collections.Generic;
using ChaoticDimensions.Content.Scenes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Common.GlobalNPCs
{
	public sealed class ShadowBiomeGlobalNPC : GlobalNPC
	{
		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {
			if (!spawnInfo.Player.InModBiome<ShadowBiome>()) {
				return;
			}

			pool.Clear();
			if (spawnInfo.Player.ZoneOverworldHeight || spawnInfo.Player.ZoneSkyHeight) {
				pool[NPCID.CorruptSlime] = 0.26f;
				pool[NPCID.EaterofSouls] = 0.34f;
			}
			else if (spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight) {
				pool[NPCID.DevourerHead] = 0.3f;
				pool[NPCID.Corruptor] = 0.24f;
				if (Main.hardMode) {
					pool[NPCID.BigMimicCorruption] = 0.02f;
				}
			}
		}

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
			if (!player.InModBiome<ShadowBiome>()) {
				return;
			}

			spawnRate = (int)(spawnRate * 0.55f);
			maxSpawns += 7;
		}
	}
}
