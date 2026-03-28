using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChaoticDimensions.Content.Players
{
	public sealed class ShadowAscensionPlayer : ModPlayer
	{
		public const int MaxHeartOfTheGodUses = 2;
		public const int LifePerHeartOfTheGod = 125;

		public int heartOfTheGodUses;

		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana) {
			health = StatModifier.Default;
			health.Base = heartOfTheGodUses * LifePerHeartOfTheGod;
			mana = StatModifier.Default;
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			ShadowAscensionPlayer clone = (ShadowAscensionPlayer)targetCopy;
			clone.heartOfTheGodUses = heartOfTheGodUses;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			ShadowAscensionPlayer clone = (ShadowAscensionPlayer)clientPlayer;
			if (clone.heartOfTheGodUses != heartOfTheGodUses) {
				SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
			}
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)ChaoticDimensions.MessageType.ShadowAscensionPlayerSync);
			packet.Write((byte)Player.whoAmI);
			packet.Write((byte)heartOfTheGodUses);
			packet.Send(toWho, fromWho);
		}

		public void ReceivePlayerSync(BinaryReader reader) {
			heartOfTheGodUses = reader.ReadByte();
		}

		public override void SaveData(TagCompound tag) {
			if (heartOfTheGodUses > 0) {
				tag["heartOfTheGodUses"] = heartOfTheGodUses;
			}
		}

		public override void LoadData(TagCompound tag) {
			heartOfTheGodUses = tag.GetInt("heartOfTheGodUses");
		}
	}
}
