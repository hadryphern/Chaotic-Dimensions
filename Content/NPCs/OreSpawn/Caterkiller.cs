using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.Items.Materials.OreSpawn;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.OreSpawn
{
	public sealed class Caterkiller : OreSpawnPrototypeNpc
	{
		protected override bool IsMiniBoss => true;
		protected override int PrototypeWidth => 62;
		protected override int PrototypeHeight => 34;
		protected override int PrototypeDamage => 44;
		protected override int PrototypeDefense => 18;
		protected override int PrototypeLifeMax => 1425;
		protected override float PrototypeKnockBackResist => 0.06f;
		protected override float PrototypeValue => Item.buyPrice(gold: 1, silver: 20);
		protected override float PrototypeSpawnChance => DaySurfaceChance() * 0.02f;
		protected override ChaoticProgressionGate ProgressionGate => ChaoticProgressionGate.PostEvilBoss;
		protected override int VanillaAIType => NPCID.BlackRecluse;

		protected override bool CanSpawnHere(NPCSpawnInfo spawnInfo) {
			return IsForestOrJungleSurface(spawnInfo.Player);
		}

		public override void SetDefaults() {
			base.SetDefaults();
			NPC.HitSound = SoundID.NPCHit11;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CaterkillerJaw>(), 1, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ItemID.Ruby, 1, 3, 6));
		}
	}
}
