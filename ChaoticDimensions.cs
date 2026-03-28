using ChaoticDimensions.Common.Graphics;
using ChaoticDimensions.Content.Players;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace ChaoticDimensions
{
	public class ChaoticDimensions : Mod
	{
		internal const string CrystalineDevourerSkyKey = "ChaoticDimensions:CrystalineDevourerSky";

		internal enum MessageType : byte
		{
			ShadowAscensionPlayerSync
		}

		public override void Load() {
			if (Main.dedServ) {
				return;
			}

			Filters.Scene[CrystalineDevourerSkyKey] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(0.45f, 0.1f, 0.55f), EffectPriority.VeryHigh);
			SkyManager.Instance[CrystalineDevourerSkyKey] = new CrystalineDevourerSky();
		}

		public override void Unload() {
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
			}
		}
	}
}
