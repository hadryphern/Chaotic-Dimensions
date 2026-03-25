using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Bosses.CrystalineDevourer;
using ChaoticDimensions.Content.Tiles;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class CrystalineDevourerArenaSystem : ModSystem
	{
		private const int ArenaHalfWidthTiles = 164;
		private const int ArenaHalfHeightTiles = 104;
		private const int BorderThicknessTiles = 4;
		private const float PlayerKillPadding = (BorderThicknessTiles * 16f) - 8f;

		public const float ArenaHalfWidth = ArenaHalfWidthTiles * 16f;
		public const float ArenaHalfHeight = ArenaHalfHeightTiles * 16f;

		private static int ownerNpcIndex = -1;
		private static Vector2 arenaCenter;
		private static readonly Dictionary<Point, ArenaTileSnapshot> storedTiles = [];
		private static readonly HashSet<Point> barrierTiles = [];

		private readonly record struct ArenaTileSnapshot(
			bool HasTile,
			ushort TileType,
			short TileFrameX,
			short TileFrameY,
			byte TileColor,
			bool IsHalfBlock,
			SlopeType Slope,
			ushort WallType,
			byte WallColor,
			byte LiquidAmount,
			byte LiquidType)
		{
			public static ArenaTileSnapshot Capture(Tile tile) {
				return new ArenaTileSnapshot(
					tile.HasTile,
					tile.TileType,
					tile.TileFrameX,
					tile.TileFrameY,
					tile.TileColor,
					tile.IsHalfBlock,
					tile.Slope,
					tile.WallType,
					tile.WallColor,
					tile.LiquidAmount,
					(byte)tile.LiquidType);
			}

			public void Restore(Tile tile) {
				tile.HasTile = HasTile;
				tile.TileType = TileType;
				tile.TileFrameX = TileFrameX;
				tile.TileFrameY = TileFrameY;
				tile.TileColor = TileColor;
				tile.IsHalfBlock = IsHalfBlock;
				tile.Slope = Slope;
				tile.WallType = WallType;
				tile.WallColor = WallColor;
				tile.LiquidAmount = LiquidAmount;
				tile.LiquidType = LiquidType;
			}
		}

		public static bool IsActive => ownerNpcIndex >= 0 &&
			ownerNpcIndex < Main.maxNPCs &&
			Main.npc[ownerNpcIndex].active &&
			Main.npc[ownerNpcIndex].type == ModContent.NPCType<CrystalineDevourerHead>();

		public static Vector2 Center => arenaCenter;
		public static float HalfWidth => ArenaHalfWidth;
		public static float HalfHeight => ArenaHalfHeight;

		public override void OnWorldLoad() {
			ClearState();
		}

		public override void OnWorldUnload() {
			ClearState();
		}

		public override void Unload() {
			ClearState();
		}

		public static void EnsureArena(NPC owner, Player target) {
			if (!owner.active) {
				return;
			}

			if (ownerNpcIndex == -1) {
				ownerNpcIndex = owner.whoAmI;
				arenaCenter = target.Center;
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					CreateBarrier();
				}
				return;
			}

			ownerNpcIndex = owner.whoAmI;
		}

		public override void PostUpdateEverything() {
			if (!IsActive) {
				if (ownerNpcIndex != -1) {
					ResetArena(restoreTiles: true);
				}

				return;
			}

			if (Main.netMode != NetmodeID.MultiplayerClient) {
				KillPlayersOutsideArena();
			}
		}

		public static bool Contains(Vector2 worldPosition, float padding = 0f) {
			return Math.Abs(worldPosition.X - arenaCenter.X) <= ArenaHalfWidth + padding &&
				Math.Abs(worldPosition.Y - arenaCenter.Y) <= ArenaHalfHeight + padding;
		}

		public static bool ProtectsTile(int i, int j, int type) {
			if (!IsActive) {
				return false;
			}

			if (type != ModContent.TileType<CrystalineBarrierBlock>()) {
				return false;
			}

			return barrierTiles.Contains(new Point(i, j));
		}

		private static void KillPlayersOutsideArena() {
			for (int i = 0; i < Main.maxPlayers; i++) {
				Player player = Main.player[i];
				if (!player.active || player.dead) {
					continue;
				}

				if (Contains(player.Center, PlayerKillPadding)) {
					continue;
				}

				player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral($"{player.name} was annihilated beyond the Crystaline Arena.")), 999999, 0);
			}
		}

		private static void CreateBarrier() {
			if (storedTiles.Count > 0) {
				return;
			}

			int centerTileX = (int)Math.Round(arenaCenter.X / 16f);
			int centerTileY = (int)Math.Round(arenaCenter.Y / 16f);
			int minTileX = Math.Max(10, centerTileX - ArenaHalfWidthTiles);
			int maxTileX = Math.Min(Main.maxTilesX - 10, centerTileX + ArenaHalfWidthTiles);
			int minTileY = Math.Max(10, centerTileY - ArenaHalfHeightTiles);
			int maxTileY = Math.Min(Main.maxTilesY - 10, centerTileY + ArenaHalfHeightTiles);

			for (int offset = 0; offset < BorderThicknessTiles; offset++) {
				int topY = minTileY + offset;
				int bottomY = maxTileY - offset;
				for (int x = minTileX; x <= maxTileX; x++) {
					PlaceBarrierTile(x, topY);
					PlaceBarrierTile(x, bottomY);
				}
			}

			for (int offset = 0; offset < BorderThicknessTiles; offset++) {
				int leftX = minTileX + offset;
				int rightX = maxTileX - offset;
				for (int y = minTileY + BorderThicknessTiles; y <= maxTileY - BorderThicknessTiles; y++) {
					PlaceBarrierTile(leftX, y);
					PlaceBarrierTile(rightX, y);
				}
			}
		}

		private static void PlaceBarrierTile(int i, int j) {
			if (!WorldGen.InWorld(i, j, 10)) {
				return;
			}

			Point point = new(i, j);
			Tile tile = Framing.GetTileSafely(i, j);
			if (!storedTiles.ContainsKey(point)) {
				storedTiles[point] = ArenaTileSnapshot.Capture(tile);
			}

			barrierTiles.Add(point);

			tile.HasTile = true;
			tile.TileType = (ushort)ModContent.TileType<CrystalineBarrierBlock>();
			tile.TileFrameX = 0;
			tile.TileFrameY = 0;
			tile.TileColor = PaintID.None;
			tile.IsHalfBlock = false;
			tile.Slope = SlopeType.Solid;
			tile.LiquidAmount = 0;
			tile.LiquidType = LiquidID.Water;
			WorldGen.SquareTileFrame(i, j, true);

			if (Main.netMode == NetmodeID.Server) {
				NetMessage.SendTileSquare(-1, i, j);
			}
		}

		private static void RestoreBarrier() {
			foreach ((Point point, ArenaTileSnapshot snapshot) in storedTiles) {
				if (!WorldGen.InWorld(point.X, point.Y, 10)) {
					continue;
				}

				Tile tile = Framing.GetTileSafely(point.X, point.Y);
				snapshot.Restore(tile);
				WorldGen.SquareTileFrame(point.X, point.Y, true);

				if (Main.netMode == NetmodeID.Server) {
					NetMessage.SendTileSquare(-1, point.X, point.Y);
				}
			}
		}

		private static void ResetArena(bool restoreTiles) {
			if (restoreTiles && Main.netMode != NetmodeID.MultiplayerClient && storedTiles.Count > 0) {
				RestoreBarrier();
			}

			ClearState();
		}

		private static void ClearState() {
			ownerNpcIndex = -1;
			arenaCenter = Vector2.Zero;
			storedTiles.Clear();
			barrierTiles.Clear();
		}
	}
}
