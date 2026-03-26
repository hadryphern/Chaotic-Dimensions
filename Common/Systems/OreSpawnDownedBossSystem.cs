using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class OreSpawnDownedBossSystem : ModSystem
	{
		public static bool downedKraken;
		public static bool downedMobzilla;
		public static bool downedMothra;
		public static bool downedTheKing;
		public static bool downedTheQueen;
		public static bool downedWtf;

		public override void ClearWorld() {
			downedKraken = false;
			downedMobzilla = false;
			downedMothra = false;
			downedTheKing = false;
			downedTheQueen = false;
			downedWtf = false;
		}

		public override void SaveWorldData(TagCompound tag) {
			if (downedKraken) {
				tag["downedKraken"] = true;
			}

			if (downedMobzilla) {
				tag["downedMobzilla"] = true;
			}

			if (downedMothra) {
				tag["downedMothra"] = true;
			}

			if (downedTheKing) {
				tag["downedTheKing"] = true;
			}

			if (downedTheQueen) {
				tag["downedTheQueen"] = true;
			}

			if (downedWtf) {
				tag["downedWtf"] = true;
			}
		}

		public override void LoadWorldData(TagCompound tag) {
			downedKraken = tag.ContainsKey("downedKraken");
			downedMobzilla = tag.ContainsKey("downedMobzilla");
			downedMothra = tag.ContainsKey("downedMothra");
			downedTheKing = tag.ContainsKey("downedTheKing");
			downedTheQueen = tag.ContainsKey("downedTheQueen");
			downedWtf = tag.ContainsKey("downedWtf");
		}

		public override void NetSend(BinaryWriter writer) {
			writer.WriteFlags(
				downedKraken,
				downedMobzilla,
				downedMothra,
				downedTheKing,
				downedTheQueen,
				downedWtf);
		}

		public override void NetReceive(BinaryReader reader) {
			reader.ReadFlags(
				out downedKraken,
				out downedMobzilla,
				out downedMothra,
				out downedTheKing,
				out downedTheQueen,
				out downedWtf);
		}

		public static bool IsDowned(string key) {
			return key switch {
				"Kraken" => downedKraken,
				"Mobzilla" => downedMobzilla,
				"Mothra" => downedMothra,
				"TheKing" => downedTheKing,
				"TheQueen" => downedTheQueen,
				"Wtf" => downedWtf,
				_ => false
			};
		}

		public static void MarkDowned(string key) {
			switch (key) {
				case "Kraken":
					downedKraken = true;
					break;
				case "Mobzilla":
					downedMobzilla = true;
					break;
				case "Mothra":
					downedMothra = true;
					break;
				case "TheKing":
					downedTheKing = true;
					break;
				case "TheQueen":
					downedTheQueen = true;
					break;
				case "Wtf":
					downedWtf = true;
					break;
			}
		}
	}
}
