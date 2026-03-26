using ChaoticDimensions.Common.OreSpawn;

namespace ChaoticDimensions.Content.Items.OreSpawn
{
	public sealed class BrownAntItem : OreSpawnTransportCritterItemBase
	{
		protected override OreSpawnDimensionId TargetDimension => OreSpawnDimensionId.Utopia;
	}

	public sealed class RainbowAntItem : OreSpawnTransportCritterItemBase
	{
		protected override OreSpawnDimensionId TargetDimension => OreSpawnDimensionId.Village;
	}

	public sealed class RedAntItem : OreSpawnTransportCritterItemBase
	{
		protected override OreSpawnDimensionId TargetDimension => OreSpawnDimensionId.Mining;
	}

	public sealed class UnstableAntItem : OreSpawnTransportCritterItemBase
	{
		protected override OreSpawnDimensionId TargetDimension => OreSpawnDimensionId.Danger;
	}

	public sealed class TermiteItem : OreSpawnTransportCritterItemBase
	{
		protected override OreSpawnDimensionId TargetDimension => OreSpawnDimensionId.Crystal;
	}

	public sealed class ButterflyTransportItem : OreSpawnTransportCritterItemBase
	{
		protected override OreSpawnDimensionId TargetDimension => OreSpawnDimensionId.Chaos;
	}
}
