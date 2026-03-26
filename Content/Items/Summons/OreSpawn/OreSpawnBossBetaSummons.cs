using ChaoticDimensions.Content.Bosses.OreSpawn;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Summons.OreSpawn
{
	public sealed class KrakenBeacon : OreSpawnBossBetaSummon { protected override int TargetNpcType => ModContent.NPCType<Kraken>(); }
	public sealed class MobzillaSignal : OreSpawnBossBetaSummon { protected override int TargetNpcType => ModContent.NPCType<Mobzilla>(); }
	public sealed class MothraTotem : OreSpawnBossBetaSummon { protected override int TargetNpcType => ModContent.NPCType<Mothra>(); }
	public sealed class KingsEmblem : OreSpawnBossBetaSummon { protected override int TargetNpcType => ModContent.NPCType<TheKing>(); }
	public sealed class QueensBloom : OreSpawnBossBetaSummon { protected override int TargetNpcType => ModContent.NPCType<TheQueen>(); }
	public sealed class WtfSignal : OreSpawnBossBetaSummon { protected override int TargetNpcType => ModContent.NPCType<Wtf>(); }
}
