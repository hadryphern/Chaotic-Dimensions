using Microsoft.Xna.Framework;
using ChaoticDimensions.Content.Subworlds.OreSpawn;
using SubworldLibrary;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Players;

namespace ChaoticDimensions.Common.OreSpawn
{
	internal static class OreSpawnDimensionTravel
	{
		public static bool TryUseTransport(Player player, OreSpawnDimensionId targetDimension) {
			if (targetDimension != OreSpawnDimensionId.Danger && !OreSpawnDimensionLayout.SupportsOreSpawnDimensions()) {
				return false;
			}

			OreSpawnDimensionPlayer dimensionPlayer = player.GetModPlayer<OreSpawnDimensionPlayer>();
			return dimensionPlayer.TryUseTransport(targetDimension);
		}

		public static bool IsDangerSubworldActive() => SubworldSystem.IsActive<DangerDimensionSubworld>();

		public static bool TryTeleportPlayerToDimension(Player player, OreSpawnDimensionId targetDimension) {
			if (!OreSpawnDimensionLayout.TryGetRegion(targetDimension, out OreSpawnDimensionRegion region)) {
				return false;
			}

			if (!TryFindSafeDimensionSpawn(region, out Vector2 destinationWorld)) {
				return false;
			}

			TeleportPlayer(player, destinationWorld);
			return true;
		}

		public static void TeleportPlayer(Player player, Vector2 destinationWorld) {
			Vector2 destinationTopLeft = destinationWorld - (player.Size * 0.5f);
			player.Teleport(destinationTopLeft, TeleportationStyleID.RodOfDiscord);
			player.velocity = Vector2.Zero;
			player.fallStart = (int)(player.position.Y / 16f);

			if (Main.netMode == NetmodeID.Server) {
				NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, player.whoAmI, destinationTopLeft.X, destinationTopLeft.Y, TeleportationStyleID.RodOfDiscord);
			}
			else if (Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendData(MessageID.TeleportEntity, number: player.whoAmI, number2: destinationTopLeft.X, number3: destinationTopLeft.Y, number4: TeleportationStyleID.RodOfDiscord);
			}
		}

		public static bool TryFindSafeDimensionSpawn(OreSpawnDimensionRegion region, out Vector2 destinationWorld) {
			for (int x = region.DefaultSpawnTile.X - 120; x <= region.DefaultSpawnTile.X + 120; x += 8) {
				for (int y = region.DefaultSpawnTile.Y; y < region.TileBounds.Bottom - 20; y++) {
					if (!WorldGen.InWorld(x, y, 20)) {
						continue;
					}

					if (!WorldGen.SolidTile(x, y)) {
						continue;
					}

					if (Collision.SolidCollision(new Vector2((x - 1) * 16, (y - 4) * 16), 32, 48)) {
						continue;
					}

					destinationWorld = new Vector2((x * 16) + 8, ((y - 3) * 16) + 8);
					return true;
				}
			}

			destinationWorld = region.DefaultSpawnTile.ToWorldCoordinates(8f, 8f);
			return true;
		}

		public static void RequestTransportFromClient(Player player, OreSpawnDimensionId targetDimension) {
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				TryUseTransport(player, targetDimension);
				return;
			}

			Mod mod = ModContent.GetInstance<ChaoticDimensions>();
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)OreSpawnNetMessageType.RequestDimensionTeleport);
			packet.Write((byte)targetDimension);
			packet.Send();
		}
	}
}
