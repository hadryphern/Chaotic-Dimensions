using System.Collections.Generic;
using ChaoticDimensions.Common.Progression;

namespace ChaoticDimensions.Common.OreSpawn
{
	internal static class OreSpawnWorldCatalog
	{
		public const string AntDimensionTravelKey = "AntDimensionTravel";
		public const string TermiteCrystalTravelKey = "TermiteCrystalTravel";
		public const string BattleMobsTeamsKey = "BattleMobsTeams";
		public const string VillageNightSiegeKey = "VillageNightSiege";
		public const string RandomDungeonSpawnerKey = "RandomDungeonSpawner";
		public const string MiniDungeonPoolKey = "MiniDungeonPool";
		public const string AncientDriedEggHydrationKey = "AncientDriedEggHydration";
		public const string PrinceGrowthChainKey = "PrinceGrowthChain";
		public const string NightmareEscalationKey = "NightmareEscalation";
		public const string WormGrowthStagesKey = "WormGrowthStages";
		public const string QueenWrathStateKey = "QueenWrathState";

		public static IReadOnlyList<OreSpawnDimensionDefinition> AllDimensions { get; } = new[] {
			new OreSpawnDimensionDefinition("Utopia", "Utopia Dimension", ChaoticProgressionGate.PostMoonLord, "Late paradise dimension."),
			new OreSpawnDimensionDefinition("Village", "Village Dimension", ChaoticProgressionGate.PostAnyMech, "Robot-attacked village world."),
			new OreSpawnDimensionDefinition("Crystal", "Crystal Dimension", ChaoticProgressionGate.PostPlantera, "Crystal-heavy challenge world."),
			new OreSpawnDimensionDefinition("Mining", "Mining Dimension", ChaoticProgressionGate.PostWallOfFlesh, "Ore and dinosaur progression layer."),
			new OreSpawnDimensionDefinition("Danger", "Danger Dimension", ChaoticProgressionGate.PostGolem, "High-risk danger world."),
			new OreSpawnDimensionDefinition("Chaos", "Chaos Dimension", ChaoticProgressionGate.PostCultist, "Late chaos challenge world.")
		};

		public static IReadOnlyList<OreSpawnEventDefinition> AllEvents { get; } = new[] {
			new OreSpawnEventDefinition(AntDimensionTravelKey, "Ant Dimension Travel", ChaoticProgressionGate.Anytime, "Brown/rainbow/red ant travel network.", false, true),
			new OreSpawnEventDefinition(TermiteCrystalTravelKey, "Termite Crystal Travel", ChaoticProgressionGate.PostEvilBoss, "Termites open access to crystal progression.", false, true),
			new OreSpawnEventDefinition(BattleMobsTeamsKey, "Battle Mobs Teams", ChaoticProgressionGate.PostEyeOfCthulhu, "Enables battle-mob companion progression."),
			new OreSpawnEventDefinition(VillageNightSiegeKey, "Village Night Siege", ChaoticProgressionGate.PostAnyMech, "Nighttime robot assault event.", true),
			new OreSpawnEventDefinition(RandomDungeonSpawnerKey, "Random Dungeon Spawner", ChaoticProgressionGate.PostSkeletron, "Unlocks OreSpawn dungeon challenge flow."),
			new OreSpawnEventDefinition(MiniDungeonPoolKey, "Mini Dungeon Pool", ChaoticProgressionGate.PostSkeletron, "Enables mini-dungeon rotation."),
			new OreSpawnEventDefinition(AncientDriedEggHydrationKey, "Ancient Dried Egg Hydration", ChaoticProgressionGate.PostQueenBee, "Allows ancient dried eggs to become spawners.", false, true),
			new OreSpawnEventDefinition(PrinceGrowthChainKey, "Prince Growth Chain", ChaoticProgressionGate.PostChaoticApexTrio, "Royal growth progression unlock."),
			new OreSpawnEventDefinition(NightmareEscalationKey, "Nightmare Escalation", ChaoticProgressionGate.PostMoonLord, "Nightmare family scaling is enabled.", true),
			new OreSpawnEventDefinition(WormGrowthStagesKey, "Worm Growth Stages", ChaoticProgressionGate.PostWallOfFlesh, "Worm family multi-stage scaling unlock."),
			new OreSpawnEventDefinition(QueenWrathStateKey, "Queen Wrath State", ChaoticProgressionGate.PostChaoticApexTrio, "The Queen can enter a wrath phase.", true)
		};

		private static readonly Dictionary<string, OreSpawnEventDefinition> EventMap = new();
		private static readonly Dictionary<string, OreSpawnDimensionDefinition> DimensionMap = new();

		static OreSpawnWorldCatalog() {
			foreach (OreSpawnEventDefinition definition in AllEvents) {
				EventMap[definition.Key] = definition;
			}

			foreach (OreSpawnDimensionDefinition definition in AllDimensions) {
				DimensionMap[definition.Key] = definition;
			}
		}

		public static OreSpawnEventDefinition GetEvent(string key) => EventMap[key];

		public static OreSpawnDimensionDefinition GetDimension(string key) => DimensionMap[key];
	}
}
