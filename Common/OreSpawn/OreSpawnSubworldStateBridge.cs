using System.Collections.Generic;
using ChaoticDimensions.Common.Systems;
using SubworldLibrary;

namespace ChaoticDimensions.Common.OreSpawn
{
	internal static class OreSpawnSubworldStateBridge
	{
		private const string UnlockedEventsKey = "OreSpawnUnlockedEvents";
		private const string VillageNightSiegeActiveKey = "OreSpawnVillageNightSiegeActive";
		private const string VillageNightSiegeTimeLeftKey = "OreSpawnVillageNightSiegeTimeLeft";

		public static void CopyCurrentWorldState() {
			SubworldSystem.CopyWorldData(nameof(ChaoticDownedBossSystem.downedChaoticApexOne), ChaoticDownedBossSystem.downedChaoticApexOne);
			SubworldSystem.CopyWorldData(nameof(ChaoticDownedBossSystem.downedChaoticApexTwo), ChaoticDownedBossSystem.downedChaoticApexTwo);
			SubworldSystem.CopyWorldData(nameof(ChaoticDownedBossSystem.downedChaoticApexThree), ChaoticDownedBossSystem.downedChaoticApexThree);
			SubworldSystem.CopyWorldData(nameof(ChaoticDownedBossSystem.downedCrystalineDevourer), ChaoticDownedBossSystem.downedCrystalineDevourer);

			SubworldSystem.CopyWorldData(nameof(OreSpawnDownedBossSystem.downedKraken), OreSpawnDownedBossSystem.downedKraken);
			SubworldSystem.CopyWorldData(nameof(OreSpawnDownedBossSystem.downedMobzilla), OreSpawnDownedBossSystem.downedMobzilla);
			SubworldSystem.CopyWorldData(nameof(OreSpawnDownedBossSystem.downedMothra), OreSpawnDownedBossSystem.downedMothra);
			SubworldSystem.CopyWorldData(nameof(OreSpawnDownedBossSystem.downedTheKing), OreSpawnDownedBossSystem.downedTheKing);
			SubworldSystem.CopyWorldData(nameof(OreSpawnDownedBossSystem.downedTheQueen), OreSpawnDownedBossSystem.downedTheQueen);
			SubworldSystem.CopyWorldData(nameof(OreSpawnDownedBossSystem.downedWtf), OreSpawnDownedBossSystem.downedWtf);

			List<string> unlockedEvents = OreSpawnEventStateSystem.GetUnlockedEventKeys();
			SubworldSystem.CopyWorldData(UnlockedEventsKey, unlockedEvents);

			OreSpawnEventStateSystem.GetVillageNightSiegeState(out bool villageNightSiegeActive, out int villageNightSiegeTimeLeft);
			SubworldSystem.CopyWorldData(VillageNightSiegeActiveKey, villageNightSiegeActive);
			SubworldSystem.CopyWorldData(VillageNightSiegeTimeLeftKey, villageNightSiegeTimeLeft);
		}

		public static void ReadCopiedState() {
			ChaoticDownedBossSystem.downedChaoticApexOne = SubworldSystem.ReadCopiedWorldData<bool>(nameof(ChaoticDownedBossSystem.downedChaoticApexOne));
			ChaoticDownedBossSystem.downedChaoticApexTwo = SubworldSystem.ReadCopiedWorldData<bool>(nameof(ChaoticDownedBossSystem.downedChaoticApexTwo));
			ChaoticDownedBossSystem.downedChaoticApexThree = SubworldSystem.ReadCopiedWorldData<bool>(nameof(ChaoticDownedBossSystem.downedChaoticApexThree));
			ChaoticDownedBossSystem.downedCrystalineDevourer = SubworldSystem.ReadCopiedWorldData<bool>(nameof(ChaoticDownedBossSystem.downedCrystalineDevourer));

			OreSpawnDownedBossSystem.downedKraken = SubworldSystem.ReadCopiedWorldData<bool>(nameof(OreSpawnDownedBossSystem.downedKraken));
			OreSpawnDownedBossSystem.downedMobzilla = SubworldSystem.ReadCopiedWorldData<bool>(nameof(OreSpawnDownedBossSystem.downedMobzilla));
			OreSpawnDownedBossSystem.downedMothra = SubworldSystem.ReadCopiedWorldData<bool>(nameof(OreSpawnDownedBossSystem.downedMothra));
			OreSpawnDownedBossSystem.downedTheKing = SubworldSystem.ReadCopiedWorldData<bool>(nameof(OreSpawnDownedBossSystem.downedTheKing));
			OreSpawnDownedBossSystem.downedTheQueen = SubworldSystem.ReadCopiedWorldData<bool>(nameof(OreSpawnDownedBossSystem.downedTheQueen));
			OreSpawnDownedBossSystem.downedWtf = SubworldSystem.ReadCopiedWorldData<bool>(nameof(OreSpawnDownedBossSystem.downedWtf));

			List<string> unlockedEvents = SubworldSystem.ReadCopiedWorldData<List<string>>(UnlockedEventsKey);
			bool villageNightSiegeActive = SubworldSystem.ReadCopiedWorldData<bool>(VillageNightSiegeActiveKey);
			int villageNightSiegeTimeLeft = SubworldSystem.ReadCopiedWorldData<int>(VillageNightSiegeTimeLeftKey);
			OreSpawnEventStateSystem.RestoreTransferredState(unlockedEvents, villageNightSiegeActive, villageNightSiegeTimeLeft);
		}
	}
}
