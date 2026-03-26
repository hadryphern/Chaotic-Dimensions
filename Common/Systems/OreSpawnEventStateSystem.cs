using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChaoticDimensions.Common.OreSpawn;
using ChaoticDimensions.Common.Progression;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class OreSpawnEventStateSystem : ModSystem
	{
		private static readonly HashSet<string> UnlockedEvents = new();

		public static bool VillageNightSiegeActive { get; private set; }
		private static int villageNightSiegeTimeLeft;

		public override void OnWorldLoad() {
			UnlockedEvents.Clear();
			VillageNightSiegeActive = false;
			villageNightSiegeTimeLeft = 0;
		}

		public override void OnWorldUnload() {
			UnlockedEvents.Clear();
			VillageNightSiegeActive = false;
			villageNightSiegeTimeLeft = 0;
		}

		public override void SaveWorldData(TagCompound tag) {
			tag["oreSpawnUnlockedEvents"] = UnlockedEvents.ToList();
		}

		public override void LoadWorldData(TagCompound tag) {
			UnlockedEvents.Clear();
			foreach (string key in tag.GetList<string>("oreSpawnUnlockedEvents")) {
				UnlockedEvents.Add(key);
			}

			VillageNightSiegeActive = false;
			villageNightSiegeTimeLeft = 0;
		}

		public override void NetSend(BinaryWriter writer) {
			writer.Write((ushort)UnlockedEvents.Count);
			foreach (string key in UnlockedEvents.OrderBy(static key => key)) {
				writer.Write(key);
			}

			writer.Write(VillageNightSiegeActive);
			writer.Write(villageNightSiegeTimeLeft);
		}

		public override void NetReceive(BinaryReader reader) {
			UnlockedEvents.Clear();
			int count = reader.ReadUInt16();
			for (int index = 0; index < count; index++) {
				UnlockedEvents.Add(reader.ReadString());
			}

			VillageNightSiegeActive = reader.ReadBoolean();
			villageNightSiegeTimeLeft = reader.ReadInt32();
		}

		public override void PreUpdateWorld() {
			foreach (OreSpawnEventDefinition definition in OreSpawnWorldCatalog.AllEvents) {
				if (ChaoticProgressionHelper.IsMet(definition.Gate)) {
					UnlockedEvents.Add(definition.Key);
				}
			}

			UpdateVillageNightSiege();
		}

		private static void UpdateVillageNightSiege() {
			if (!IsUnlocked(OreSpawnWorldCatalog.VillageNightSiegeKey)) {
				StopVillageNightSiege();
				return;
			}

			if (VillageNightSiegeActive) {
				villageNightSiegeTimeLeft--;

				if (Main.dayTime || villageNightSiegeTimeLeft <= 0 || !AnyVillageEligiblePlayer()) {
					StopVillageNightSiege();
				}

				return;
			}

			if (!Main.dayTime && AnyVillageEligiblePlayer() && Main.rand.NextBool(36000)) {
				StartVillageNightSiege();
			}
		}

		public static bool IsUnlocked(string key) => UnlockedEvents.Contains(key);

		internal static List<string> GetUnlockedEventKeys() {
			return UnlockedEvents.OrderBy(static key => key).ToList();
		}

		internal static void GetVillageNightSiegeState(out bool active, out int timeLeft) {
			active = VillageNightSiegeActive;
			timeLeft = villageNightSiegeTimeLeft;
		}

		internal static void RestoreTransferredState(IEnumerable<string> unlockedEvents, bool villageNightSiegeActiveState, int villageNightSiegeTimeLeftState) {
			UnlockedEvents.Clear();
			foreach (string key in unlockedEvents) {
				UnlockedEvents.Add(key);
			}

			VillageNightSiegeActive = villageNightSiegeActiveState && UnlockedEvents.Contains(OreSpawnWorldCatalog.VillageNightSiegeKey);
			villageNightSiegeTimeLeft = VillageNightSiegeActive ? villageNightSiegeTimeLeftState : 0;
		}

		public static bool IsEventActive(string key) {
			return key switch {
				OreSpawnWorldCatalog.VillageNightSiegeKey => VillageNightSiegeActive,
				_ => IsUnlocked(key)
			};
		}

		public static bool CanSpawnEventMob(string key, Player player) {
			return key switch {
				OreSpawnWorldCatalog.VillageNightSiegeKey => player.active &&
					!player.dead &&
					OreSpawnDimensionLayout.TryGetDimensionAtWorldPosition(player.Center, out OreSpawnDimensionId dimensionId) &&
					dimensionId == OreSpawnDimensionId.Village &&
					player.ZoneOverworldHeight,
				_ => false
			};
		}

		public static void StartVillageNightSiege(bool announce = true) {
			if (!IsUnlocked(OreSpawnWorldCatalog.VillageNightSiegeKey)) {
				return;
			}

			VillageNightSiegeActive = true;
			villageNightSiegeTimeLeft = 10800;

			if (announce && Main.netMode != NetmodeID.Server) {
				Main.NewText("Village Night Siege has begun!", 180, 80, 80);
			}
		}

		public static void StopVillageNightSiege(bool announce = false) {
			bool wasActive = VillageNightSiegeActive;
			VillageNightSiegeActive = false;
			villageNightSiegeTimeLeft = 0;

			if (announce && wasActive && Main.netMode != NetmodeID.Server) {
				Main.NewText("Village Night Siege has ended.", 140, 200, 140);
			}
		}

		private static bool AnyVillageEligiblePlayer() {
			for (int index = 0; index < Main.maxPlayers; index++) {
				Player player = Main.player[index];
				if (CanSpawnEventMob(OreSpawnWorldCatalog.VillageNightSiegeKey, player)) {
					return true;
				}
			}

			return false;
		}
	}
}
