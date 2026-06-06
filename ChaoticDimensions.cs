using ChaoticDimensions.Common.Graphics;
using ChaoticDimensions.Common.Systems;
using ChaoticDimensions.Content.Backgrounds;
using ChaoticDimensions.Content.Bosses.Monthra;
using ChaoticDimensions.Content.Items.Summons;
using ChaoticDimensions.Content.Players;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions
{
	public class ChaoticDimensions : Mod
	{
		internal const string CrystalineDevourerSkyKey = "ChaoticDimensions:CrystalineDevourerSky";
		public static bool KrakenCosmicSkyRegistered { get; private set; }

		internal enum MessageType : byte
		{
			ShadowAscensionPlayerSync,
			SpawnMonthraAfterIntro,
			RequestKrakenEvent,
			StartKrakenEvent,
			StopKrakenEvent
		}

		public override void Load() {
			KrakenCosmicSkyRegistered = false;
			if (Main.dedServ) {
				return;
			}

			Filters.Scene[CrystalineDevourerSkyKey] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.45f, 0.1f, 0.55f), EffectPriority.VeryHigh);
			SkyManager.Instance[CrystalineDevourerSkyKey] = new CrystalineDevourerSky();
			SkyManager.Instance[KrakenCosmicSky.EffectKey] = new KrakenCosmicSky();
			KrakenCosmicSkyRegistered = true;
		}

		public override void Unload() {
			KrakenCosmicSkyRegistered = false;
			if (Main.dedServ) {
				return;
			}

			try {
				Filters.Scene?[CrystalineDevourerSkyKey]?.Deactivate();
			}
			catch {
			}

			try {
				SkyManager.Instance?.Deactivate(CrystalineDevourerSkyKey);
			}
			catch {
			}

			try {
				SkyManager.Instance?.Deactivate(KrakenCosmicSky.EffectKey);
			}
			catch {
			}
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI) {
			switch ((MessageType)reader.ReadByte()) {
				case MessageType.ShadowAscensionPlayerSync:
					byte playerNumber = reader.ReadByte();
					ShadowAscensionPlayer player = Main.player[playerNumber].GetModPlayer<ShadowAscensionPlayer>();
					player.ReceivePlayerSync(reader);

					if (Main.netMode == Terraria.ID.NetmodeID.Server) {
						player.SyncPlayer(-1, whoAmI, false);
					}
					break;

				case MessageType.SpawnMonthraAfterIntro:
					byte targetPlayer = reader.ReadByte();
					if (Main.netMode == NetmodeID.Server && targetPlayer < Main.maxPlayers && !NPC.AnyNPCs(ModContent.NPCType<MonthraBoss>())) {
						Player monthraTarget = Main.player[targetPlayer];
						if (monthraTarget.active && !monthraTarget.dead) {
							int monthraIndex = NPC.NewNPC(new EntitySource_Misc("MonthraIntro"), (int)monthraTarget.Center.X, (int)monthraTarget.Center.Y - 320, ModContent.NPCType<MonthraBoss>(), Target: targetPlayer);
							if (monthraIndex < Main.maxNPCs) {
								Main.npc[monthraIndex].TargetClosest();
								NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, monthraIndex);
							}
						}
					}
					break;

				case MessageType.RequestKrakenEvent:
					byte requestingPlayer = reader.ReadByte();
					if (Main.netMode == NetmodeID.Server
						&& requestingPlayer == whoAmI
						&& requestingPlayer < Main.maxPlayers) {
						Player krakenRequester = Main.player[requestingPlayer];
						if (krakenRequester.active
							&& !krakenRequester.dead
							&& KrakenSummonItem.CanStartEncounter(krakenRequester)) {
							KrakenEventSystem.Instance.StartEvent(krakenRequester);
						}
					}
					break;

				case MessageType.StartKrakenEvent:
					byte ownerPlayer = reader.ReadByte();
					if (Main.netMode == NetmodeID.MultiplayerClient && ownerPlayer < Main.maxPlayers) {
						Player krakenOwner = Main.player[ownerPlayer];
						if (krakenOwner.active) {
							KrakenEventSystem.Instance.StartEvent(krakenOwner, false);
						}
					}
					break;

				case MessageType.StopKrakenEvent:
					if (Main.netMode == NetmodeID.MultiplayerClient) {
						KrakenEventSystem.Instance.StopEvent(false);
					}
					break;
			}
		}
	}
}
