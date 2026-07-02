using System.IO;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class MoonTalkWorldSystem : ModSystem
	{
		public static bool IntroCompleted { get; private set; }
		public static bool IntroPending { get; private set; }

		public override void ClearWorld() {
			IntroCompleted = false;
			IntroPending = false;
		}

		public override void PostWorldGen() {
			IntroCompleted = false;
			IntroPending = true;
		}

		public override void SaveWorldData(TagCompound tag) {
			if (IntroCompleted) {
				tag["moonTalkIntroCompleted"] = true;
			}

			if (IntroPending) {
				tag["moonTalkIntroPending"] = true;
			}
		}

		public override void LoadWorldData(TagCompound tag) {
			IntroCompleted = tag.ContainsKey("moonTalkIntroCompleted");
			IntroPending = tag.ContainsKey("moonTalkIntroPending");
		}

		public override void NetSend(BinaryWriter writer) {
			writer.Write(IntroCompleted);
			writer.Write(IntroPending);
		}

		public override void NetReceive(BinaryReader reader) {
			IntroCompleted = reader.ReadBoolean();
			IntroPending = reader.ReadBoolean();
		}

		public static void MarkIntroCompleted() {
			IntroCompleted = true;
			IntroPending = false;

			if (Terraria.Main.netMode == NetmodeID.Server) {
				Terraria.NetMessage.SendData(MessageID.WorldData);
			}
		}
	}
}
