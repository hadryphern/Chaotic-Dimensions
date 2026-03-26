using ChaoticDimensions.Content.Items.OreSpawn;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChaoticDimensions.Content.NPCs.OreSpawn
{
	public sealed class BrownAntCritter : OreSpawnTransportCritterBase
	{
		protected override int CloneNpcId => NPCID.Worm;
		protected override int CatchItemType => ModContent.ItemType<BrownAntItem>();
		protected override float SpawnChanceFactor => 0.05f;
	}

	public sealed class RainbowAntCritter : OreSpawnTransportCritterBase
	{
		protected override int CloneNpcId => NPCID.Grasshopper;
		protected override int CatchItemType => ModContent.ItemType<RainbowAntItem>();
		protected override float SpawnChanceFactor => 0.03f;
	}

	public sealed class RedAntCritter : OreSpawnTransportCritterBase
	{
		protected override int CloneNpcId => NPCID.Scorpion;
		protected override int CatchItemType => ModContent.ItemType<RedAntItem>();
		protected override float SpawnChanceFactor => 0.025f;
	}

	public sealed class UnstableAntCritter : OreSpawnTransportCritterBase
	{
		protected override int CloneNpcId => NPCID.Firefly;
		protected override int CatchItemType => ModContent.ItemType<UnstableAntItem>();
		protected override float SpawnChanceFactor => 0.018f;
	}

	public sealed class TermiteCritter : OreSpawnTransportCritterBase
	{
		protected override int CloneNpcId => NPCID.Buggy;
		protected override int CatchItemType => ModContent.ItemType<TermiteItem>();
		protected override float SpawnChanceFactor => 0.02f;
	}

	public sealed class ButterflyCritter : OreSpawnTransportCritterBase
	{
		protected override int CloneNpcId => NPCID.Butterfly;
		protected override int CatchItemType => ModContent.ItemType<ButterflyTransportItem>();
		protected override float SpawnChanceFactor => 0.025f;
	}
}
