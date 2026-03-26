using Microsoft.Xna.Framework;
using System.IO;
using ChaoticDimensions.Common.OreSpawn;
using ChaoticDimensions.Content.Subworlds.OreSpawn;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChaoticDimensions.Content.Players
{
	public sealed class OreSpawnDimensionPlayer : ModPlayer
	{
		private const int TransportCooldownMax = 60;

		private Vector2 returnPositionWorld;
		private bool hasReturnPosition;
		private bool pendingDangerReturnTeleport;
		private int transportCooldown;
		private bool warnedLargeWorld;

		public OreSpawnDimensionId CurrentDimension { get; private set; } = OreSpawnDimensionId.Overworld;

		public override void Initialize() {
			returnPositionWorld = Vector2.Zero;
			hasReturnPosition = false;
			pendingDangerReturnTeleport = false;
			transportCooldown = 0;
			warnedLargeWorld = false;
			CurrentDimension = OreSpawnDimensionId.Overworld;
		}

		public override void ResetEffects() {
			if (transportCooldown > 0) {
				transportCooldown--;
			}
		}

		public override void PreUpdate() {
			if (SubworldSystem.IsActive<DangerDimensionSubworld>()) {
				CurrentDimension = OreSpawnDimensionId.Danger;
				return;
			}

			CurrentDimension = OreSpawnDimensionLayout.TryGetDimensionAtWorldPosition(Player.Center, out OreSpawnDimensionId dimensionId)
				? dimensionId
				: OreSpawnDimensionId.Overworld;
		}

		public override void PostUpdate() {
			if (!pendingDangerReturnTeleport || hasReturnPosition == false || Player.dead || !Player.active) {
				return;
			}

			if (SubworldSystem.AnyActive() || CurrentDimension != OreSpawnDimensionId.Overworld || Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			pendingDangerReturnTeleport = false;
			OreSpawnDimensionTravel.TeleportPlayer(Player, returnPositionWorld);
		}

		public override void OnEnterWorld() {
			if (!OreSpawnDimensionLayout.SupportsOreSpawnDimensions() && !warnedLargeWorld) {
				warnedLargeWorld = true;
				if (Main.netMode != NetmodeID.Server) {
					Main.NewText("Chaotic Dimensions requires a Large world to enable OreSpawn dimensions and structures.", 255, 120, 120);
				}
			}
		}

		public override void SaveData(TagCompound tag) {
			tag["oreSpawnHasReturnPosition"] = hasReturnPosition;
			if (hasReturnPosition) {
				tag["oreSpawnReturnPositionX"] = returnPositionWorld.X;
				tag["oreSpawnReturnPositionY"] = returnPositionWorld.Y;
			}

			tag["oreSpawnPendingDangerReturnTeleport"] = pendingDangerReturnTeleport;
		}

		public override void LoadData(TagCompound tag) {
			hasReturnPosition = tag.GetBool("oreSpawnHasReturnPosition");
			if (hasReturnPosition) {
				returnPositionWorld = new Vector2(tag.GetFloat("oreSpawnReturnPositionX"), tag.GetFloat("oreSpawnReturnPositionY"));
			}

			pendingDangerReturnTeleport = tag.GetBool("oreSpawnPendingDangerReturnTeleport");
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			if (targetCopy is not OreSpawnDimensionPlayer clone) {
				return;
			}

			clone.returnPositionWorld = returnPositionWorld;
			clone.hasReturnPosition = hasReturnPosition;
			clone.pendingDangerReturnTeleport = pendingDangerReturnTeleport;
			clone.transportCooldown = transportCooldown;
			clone.CurrentDimension = CurrentDimension;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
		}

		public bool TryUseTransport(OreSpawnDimensionId targetDimension) {
			if (transportCooldown > 0 || !Player.active || Player.dead) {
				return false;
			}

			if (targetDimension == OreSpawnDimensionId.Danger) {
				return TryUseDangerDimensionTransport();
			}

			if (CurrentDimension == targetDimension) {
				return TryReturnToSavedPosition();
			}

			if (CurrentDimension == OreSpawnDimensionId.Overworld) {
				returnPositionWorld = Player.Center;
				hasReturnPosition = true;
			}

			if (!OreSpawnDimensionTravel.TryTeleportPlayerToDimension(Player, targetDimension)) {
				return false;
			}

			transportCooldown = TransportCooldownMax;
			return true;
		}

		private bool TryUseDangerDimensionTransport() {
			if (CurrentDimension == OreSpawnDimensionId.Danger) {
				transportCooldown = TransportCooldownMax;
				pendingDangerReturnTeleport = hasReturnPosition;
				if (Main.netMode == NetmodeID.SinglePlayer) {
					SubworldSystem.Exit();
					return true;
				}

				if (Main.netMode == NetmodeID.Server) {
					SubworldSystem.MovePlayerToMainWorld(Player.whoAmI);
					return true;
				}

				return false;
			}

			if (CurrentDimension == OreSpawnDimensionId.Overworld) {
				returnPositionWorld = Player.Center;
				hasReturnPosition = true;
			}

			pendingDangerReturnTeleport = true;
			transportCooldown = TransportCooldownMax;

			if (Main.netMode == NetmodeID.SinglePlayer) {
				return SubworldSystem.Enter<DangerDimensionSubworld>();
			}

			if (Main.netMode == NetmodeID.Server) {
				SubworldSystem.MovePlayerToSubworld<DangerDimensionSubworld>(Player.whoAmI);
				return true;
			}

			return false;
		}

		private bool TryReturnToSavedPosition() {
			if (!hasReturnPosition) {
				return false;
			}

			OreSpawnDimensionTravel.TeleportPlayer(Player, returnPositionWorld);
			transportCooldown = TransportCooldownMax;
			return true;
		}
	}
}
