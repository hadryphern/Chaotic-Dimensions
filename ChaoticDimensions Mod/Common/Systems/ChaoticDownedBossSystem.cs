// Guarda no mundo quais bosses do mod ja foram derrotados.

using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ChaoticDimensions.Common.Systems
{
	public sealed class ChaoticDownedBossSystem : ModSystem
	{
		public static bool downedMonthra;
		public static bool downedChaoticApexOne;
		public static bool downedChaoticApexTwo;
		public static bool downedChaoticApexThree;
		public static bool downedCrystalineDevourer;
		public static bool downedKraken;

		public override void ClearWorld() {
			downedMonthra = false;
			downedChaoticApexOne = false;
			downedChaoticApexTwo = false;
			downedChaoticApexThree = false;
			downedCrystalineDevourer = false;
			downedKraken = false;
		}

		// Guarda a progressao especifica deste mundo.
		public override void SaveWorldData(TagCompound tag) {
			if (downedMonthra) {
				tag["downedMonthra"] = true;
			}

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

			if (downedKraken) {
				tag["downedKraken"] = true;
			}
		}

		// Recupera a progressao guardada no mundo.
		public override void LoadWorldData(TagCompound tag) {
			downedMonthra = tag.ContainsKey("downedMonthra");
			downedChaoticApexOne = tag.ContainsKey("downedChaoticApexOne");
			downedChaoticApexTwo = tag.ContainsKey("downedChaoticApexTwo");
			downedChaoticApexThree = tag.ContainsKey("downedChaoticApexThree");
			downedCrystalineDevourer = tag.ContainsKey("downedCrystalineDevourer");
			downedKraken = tag.ContainsKey("downedKraken");
		}

		// Envia o estado global necessario aos clientes.
		public override void NetSend(BinaryWriter writer) {
			writer.WriteFlags(
				downedMonthra,
				downedChaoticApexOne,
				downedChaoticApexTwo,
				downedChaoticApexThree,
				downedCrystalineDevourer,
				downedKraken);
		}

		// Le o estado global recebido do servidor.
		public override void NetReceive(BinaryReader reader) {
			reader.ReadFlags(
				out downedMonthra,
				out downedChaoticApexOne,
				out downedChaoticApexTwo,
				out downedChaoticApexThree,
				out downedCrystalineDevourer,
				out downedKraken);
		}
	}
}
