using ChaoticDimensions.Common.Systems;
using Terraria;

namespace ChaoticDimensions.Common.Progression
{
	public static class ChaoticProgressionHelper
	{
		public static bool IsMet(ChaoticProgressionGate gate) {
			return gate switch {
				ChaoticProgressionGate.Anytime => true,
				ChaoticProgressionGate.PostKingSlime => NPC.downedSlimeKing,
				ChaoticProgressionGate.PostEyeOfCthulhu => NPC.downedBoss1,
				ChaoticProgressionGate.PostEvilBoss => NPC.downedBoss2,
				ChaoticProgressionGate.PostQueenBee => NPC.downedQueenBee,
				ChaoticProgressionGate.PostSkeletron => NPC.downedBoss3,
				ChaoticProgressionGate.PostWallOfFlesh => Main.hardMode,
				ChaoticProgressionGate.PostAnyMech => NPC.downedMechBossAny,
				ChaoticProgressionGate.PostAllMechs => NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3,
				ChaoticProgressionGate.PostPlantera => NPC.downedPlantBoss,
				ChaoticProgressionGate.PostGolem => NPC.downedGolemBoss,
				ChaoticProgressionGate.PostCultist => NPC.downedAncientCultist,
				ChaoticProgressionGate.PostMoonLord => NPC.downedMoonlord,
				ChaoticProgressionGate.PostCrystalineDevourer => ChaoticDownedBossSystem.downedCrystalineDevourer,
				ChaoticProgressionGate.PostChaoticApexOne => ChaoticDownedBossSystem.downedChaoticApexOne,
				ChaoticProgressionGate.PostChaoticApexTwo => ChaoticDownedBossSystem.downedChaoticApexTwo,
				ChaoticProgressionGate.PostChaoticApexThree => ChaoticDownedBossSystem.downedChaoticApexThree,
				ChaoticProgressionGate.PostChaoticApexTrio => ChaoticDownedBossSystem.downedChaoticApexOne && ChaoticDownedBossSystem.downedChaoticApexTwo && ChaoticDownedBossSystem.downedChaoticApexThree,
				_ => false
			};
		}

		public static string GetDebugLabel(ChaoticProgressionGate gate) {
			return gate switch {
				ChaoticProgressionGate.Anytime => "Anytime",
				ChaoticProgressionGate.PostKingSlime => "Post King Slime",
				ChaoticProgressionGate.PostEyeOfCthulhu => "Post Eye of Cthulhu",
				ChaoticProgressionGate.PostEvilBoss => "Post Eater of Worlds / Brain of Cthulhu",
				ChaoticProgressionGate.PostQueenBee => "Post Queen Bee",
				ChaoticProgressionGate.PostSkeletron => "Post Skeletron",
				ChaoticProgressionGate.PostWallOfFlesh => "Post Wall of Flesh",
				ChaoticProgressionGate.PostAnyMech => "Post Any Mechanical Boss",
				ChaoticProgressionGate.PostAllMechs => "Post All Mechanical Bosses",
				ChaoticProgressionGate.PostPlantera => "Post Plantera",
				ChaoticProgressionGate.PostGolem => "Post Golem",
				ChaoticProgressionGate.PostCultist => "Post Lunatic Cultist",
				ChaoticProgressionGate.PostMoonLord => "Post Moon Lord",
				ChaoticProgressionGate.PostCrystalineDevourer => "Post Crystaline Devourer",
				ChaoticProgressionGate.PostChaoticApexOne => "Post Future Chaotic Boss 1",
				ChaoticProgressionGate.PostChaoticApexTwo => "Post Future Chaotic Boss 2",
				ChaoticProgressionGate.PostChaoticApexThree => "Post Future Chaotic Boss 3",
				ChaoticProgressionGate.PostChaoticApexTrio => "Post Future Chaotic Boss Trio",
				_ => "Unknown"
			};
		}
	}
}
