using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class ChaoticDownedBossSystem : ModSystem
	{
		public static bool downedChaoticApexOne;
		public static bool downedChaoticApexTwo;
		public static bool downedChaoticApexThree;
		public static bool downedCrystalineDevourer;

		public override void ClearWorld() {
			downedChaoticApexOne = false;
			downedChaoticApexTwo = false;
			downedChaoticApexThree = false;
			downedCrystalineDevourer = false;
		}

		public override void SaveWorldData(TagCompound tag) {
			if (downedChaoticApexOne) {
				tag["downedChaoticApexOne"] = true;
			}

			if (downedChaoticApexTwo) {
				tag["downedChaoticApexTwo"] = true;
			}

			if (downedChaoticApexThree) {
				tag["downedChaoticApexThree"] = true;
			}

			if (downedCrystalineDevourer) {
				tag["downedCrystalineDevourer"] = true;
			}
		}

		public override void LoadWorldData(TagCompound tag) {
			downedChaoticApexOne = tag.ContainsKey("downedChaoticApexOne");
			downedChaoticApexTwo = tag.ContainsKey("downedChaoticApexTwo");
			downedChaoticApexThree = tag.ContainsKey("downedChaoticApexThree");
			downedCrystalineDevourer = tag.ContainsKey("downedCrystalineDevourer");
		}

		public override void NetSend(BinaryWriter writer) {
			writer.WriteFlags(
				downedChaoticApexOne,
				downedChaoticApexTwo,
				downedChaoticApexThree,
				downedCrystalineDevourer);
		}

		public override void NetReceive(BinaryReader reader) {
			reader.ReadFlags(
				out downedChaoticApexOne,
				out downedChaoticApexTwo,
				out downedChaoticApexThree,
				out downedCrystalineDevourer);
		}
	}
}
