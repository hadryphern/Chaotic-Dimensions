using Microsoft.Xna.Framework;
using System.IO;
using ChaoticDimensions.Common.OreSpawn;
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
		private int transportCooldown;

		public OreSpawnDimensionId CurrentDimension { get; private set; } = OreSpawnDimensionId.Overworld;

		public override void Initialize() {
			returnPositionWorld = Vector2.Zero;
			hasReturnPosition = false;
			transportCooldown = 0;
			CurrentDimension = OreSpawnDimensionId.Overworld;
		}

		public override void ResetEffects() {
			if (transportCooldown > 0) {
				transportCooldown--;
			}
		}

		public override void PreUpdate() {
			CurrentDimension = OreSpawnDimensionId.Overworld;
		}

		public override void SaveData(TagCompound tag) {
			tag["oreSpawnHasReturnPosition"] = hasReturnPosition;
			if (hasReturnPosition) {
				tag["oreSpawnReturnPositionX"] = returnPositionWorld.X;
				tag["oreSpawnReturnPositionY"] = returnPositionWorld.Y;
			}
		}

		public override void LoadData(TagCompound tag) {
			hasReturnPosition = tag.GetBool("oreSpawnHasReturnPosition");
			if (hasReturnPosition) {
				returnPositionWorld = new Vector2(tag.GetFloat("oreSpawnReturnPositionX"), tag.GetFloat("oreSpawnReturnPositionY"));
			}
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			if (targetCopy is not OreSpawnDimensionPlayer clone) {
				return;
			}

			clone.returnPositionWorld = returnPositionWorld;
			clone.hasReturnPosition = hasReturnPosition;
			clone.transportCooldown = transportCooldown;
			clone.CurrentDimension = CurrentDimension;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
		}

		public bool TryUseTransport(OreSpawnDimensionId targetDimension) {
			if (transportCooldown > 0 || !Player.active || Player.dead) {
				return false;
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
