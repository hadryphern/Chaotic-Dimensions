// Liga o mod ao ciclo de vida do tModLoader e trata as mensagens de rede.

using ChaoticDimensions.Common.Graphics;
using ChaoticDimensions.Common.Systems;
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
		internal const string MonthraGalaxySkyKey = "ChaoticDimensions:MonthraGalaxySky";

		// Regista os recursos criados ao carregar o mod.
		public override void Load() {
			if (Main.dedServ) {
				return;
			}

			Filters.Scene[CrystalineDevourerSkyKey] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.45f, 0.1f, 0.55f), EffectPriority.VeryHigh);
			Filters.Scene[MonthraGalaxySkyKey] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.55f, 0.08f, 0.35f), EffectPriority.VeryHigh);
			SkyManager.Instance[CrystalineDevourerSkyKey] = new CrystalineDevourerSky();
			SkyManager.Instance[MonthraGalaxySkyKey] = new MonthraGalaxySky();
		}

		// Liberta referencias para permitir recarregar o mod.
		public override void Unload() {
			if (Main.dedServ) {
				return;
			}

			try {
				Filters.Scene[CrystalineDevourerSkyKey]?.Deactivate();
				Filters.Scene[MonthraGalaxySkyKey]?.Deactivate();
				SkyManager.Instance[CrystalineDevourerSkyKey]?.Deactivate();
				SkyManager.Instance[MonthraGalaxySkyKey]?.Deactivate();
			}
			catch {
				// The graphics manager may already be shutting down.
			}
		}

		internal enum MessageType : byte
		{
			ShadowAscensionPlayerSync,
			SpawnMonthraAfterIntro,
			RequestKrakenEvent,
			StartKrakenEvent,
			StopKrakenEvent,
			CompleteMoonTalkIntro
		}

		// Distribui cada pacote pelo sistema certo e valida o remetente.
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

				case MessageType.CompleteMoonTalkIntro:
					if (Main.netMode == NetmodeID.Server) {
						MoonTalkWorldSystem.MarkIntroCompleted();
					}
					break;
			}
		}
	}
}
