using System.Collections.Generic;
using Terraria;

namespace ChaoticDimensions.Common.OreSpawn
{
	internal static class OreSpawnDimensionSpawnRules
	{
		private static readonly HashSet<string> UtopiaDayMobs = new() {
			"Beaver", "Bee", "Cassowary", "CliffRacer", "EasterBunny", "Gazelle", "Peacock", "Stinky"
		};

		private static readonly HashSet<string> UtopiaNightMobs = new() {
			"Cricket", "Firefly"
		};

		private static readonly HashSet<string> VillageDayMobs = new() {
			"Beaver", "Cassowary", "Gazelle", "Peacock", "Girlfriend", "Boyfriend"
		};

		private static readonly HashSet<string> VillageNightMobs = new() {
			"Cricket", "Firefly", "Ghost", "GhostSkeleton"
		};

		private static readonly HashSet<string> MiningMobs = new() {
			"Alien", "Basilisc", "CaveFisher", "Dragon", "EnderKnight", "EnderReaper", "Kyuubi",
			"Alosaurus", "BabyDragon", "Baryonyx", "Camarasaurus", "Cryolophosaurus", "Leonopteryx",
			"Nastysaurus", "Pointysaurus", "TRex", "VelocityRaptor"
		};

		private static readonly HashSet<string> CrystalMobs = new() {
			"WaterDragon", "Mantis", "Cephadrome", "CrystalUrchin", "DungeonBeast", "Fairy", "Flounder",
			"Irukandji", "Peacock", "Rat", "Rotator", "Skate", "Termite", "Vortex", "Whale"
		};

		private static readonly HashSet<string> DangerDayMobs = new() {
			"CloudShark", "Dragon", "GoldFish", "WaterDragon"
		};

		private static readonly HashSet<string> DangerNightMobs = new() {
			"CreepingHorror", "TerribleTerror", "Triffid", "LurkingTerror", "CloudShark", "Dragon"
		};

		private static readonly HashSet<string> DedicatedDimensionMobs = new(UtopiaDayMobs);

		static OreSpawnDimensionSpawnRules() {
			DedicatedDimensionMobs.UnionWith(UtopiaNightMobs);
			DedicatedDimensionMobs.UnionWith(VillageDayMobs);
			DedicatedDimensionMobs.UnionWith(VillageNightMobs);
			DedicatedDimensionMobs.UnionWith(MiningMobs);
			DedicatedDimensionMobs.UnionWith(CrystalMobs);
			DedicatedDimensionMobs.UnionWith(DangerDayMobs);
			DedicatedDimensionMobs.UnionWith(DangerNightMobs);
		}

		public static bool HasDedicatedDimension(string mobKey) => DedicatedDimensionMobs.Contains(mobKey);

		public static bool CanSpawnInDimension(OreSpawnMobDefinition definition, Player player, OreSpawnDimensionId dimensionId) {
			return dimensionId switch {
				OreSpawnDimensionId.Utopia => CanSpawnInUtopia(definition.Key, player),
				OreSpawnDimensionId.Village => CanSpawnInVillage(definition.Key, player),
				OreSpawnDimensionId.Mining => CanSpawnInMining(definition.Key, player),
				OreSpawnDimensionId.Crystal => CanSpawnInCrystal(definition.Key, player),
				OreSpawnDimensionId.Danger => CanSpawnInDanger(definition.Key, player),
				OreSpawnDimensionId.Chaos => CanSpawnInChaos(definition, player),
				_ => false
			};
		}

		private static bool CanSpawnInUtopia(string mobKey, Player player) {
			if (UtopiaDayMobs.Contains(mobKey)) {
				return player.ZoneOverworldHeight && Main.dayTime;
			}

			if (UtopiaNightMobs.Contains(mobKey)) {
				return player.ZoneOverworldHeight && !Main.dayTime;
			}

			return false;
		}

		private static bool CanSpawnInVillage(string mobKey, Player player) {
			if (VillageDayMobs.Contains(mobKey)) {
				return player.ZoneOverworldHeight && Main.dayTime;
			}

			if (VillageNightMobs.Contains(mobKey)) {
				return player.ZoneOverworldHeight && !Main.dayTime;
			}

			return false;
		}

		private static bool CanSpawnInMining(string mobKey, Player player) {
			return MiningMobs.Contains(mobKey) && (player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight || player.ZoneUnderworldHeight);
		}

		private static bool CanSpawnInCrystal(string mobKey, Player player) {
			return CrystalMobs.Contains(mobKey) && (player.ZoneRockLayerHeight || player.ZoneUnderworldHeight);
		}

		private static bool CanSpawnInDanger(string mobKey, Player player) {
			if (!player.ZoneSkyHeight) {
				return false;
			}

			if (DangerDayMobs.Contains(mobKey)) {
				return Main.dayTime;
			}

			if (DangerNightMobs.Contains(mobKey)) {
				return !Main.dayTime;
			}

			return false;
		}

		private static bool CanSpawnInChaos(OreSpawnMobDefinition definition, Player player) {
			if (definition.RequiredEventKey is not null || definition.Archetype == OreSpawnNpcArchetype.Companion) {
				return false;
			}

			if (definition.SpawnKind == OreSpawnSpawnKind.None) {
				return false;
			}

			return player.ZoneSkyHeight;
		}
	}
}
