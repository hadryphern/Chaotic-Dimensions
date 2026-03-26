using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChaoticDimensions.Content.NPCs.OreSpawn
{
	public abstract class OreSpawnTransportCritterBase : ModNPC
	{
		protected abstract int CloneNpcId { get; }
		protected abstract int CatchItemType { get; }
		protected abstract float SpawnChanceFactor { get; }

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[CloneNpcId];
			Main.npcCatchable[Type] = true;
			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
			NPCID.Sets.TownCritter[Type] = true;
		}

		public override void SetDefaults() {
			NPC.CloneDefaults(CloneNpcId);
			NPC.catchItem = CatchItemType;
			AIType = CloneNpcId;
			AnimationType = CloneNpcId;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.friendly = true;
			NPC.dontTakeDamageFromHostiles = false;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (!spawnInfo.Player.ZoneOverworldHeight || spawnInfo.PlayerSafe || !Main.dayTime) {
				return 0f;
			}

			return SpawnCondition.OverworldDaySlime.Chance * SpawnChanceFactor;
		}
	}
}
