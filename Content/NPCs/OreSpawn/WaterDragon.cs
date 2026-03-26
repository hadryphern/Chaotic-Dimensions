using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.Items.Materials.OreSpawn;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ChaoticDimensions.Content.NPCs.OreSpawn
{
	public sealed class WaterDragon : OreSpawnPrototypeNpc
	{
		protected override bool IsFlying => true;
		protected override int PrototypeWidth => 54;
		protected override int PrototypeHeight => 40;
		protected override int PrototypeDamage => 24;
		protected override int PrototypeDefense => 8;
		protected override int PrototypeLifeMax => 260;
		protected override float PrototypeKnockBackResist => 0.2f;
		protected override float PrototypeValue => Item.buyPrice(silver: 90);
		protected override float PrototypeSpawnChance => SpawnCondition.OceanMonster.Chance * 0.28f;
		protected override ChaoticProgressionGate ProgressionGate => ChaoticProgressionGate.PostEyeOfCthulhu;
		protected override int VanillaAIType => NPCID.CaveBat;

		protected override bool CanSpawnHere(NPCSpawnInfo spawnInfo) {
			return spawnInfo.Player.ZoneBeach;
		}

		public override void SetDefaults() {
			base.SetDefaults();
			NPC.HitSound = SoundID.NPCHit13;
			NPC.DeathSound = SoundID.NPCDeath19;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WaterDragonScale>(), 1, 1, 2));
			npcLoot.Add(ItemDropRule.Common(ItemID.IronBar, 2, 3, 5));
			npcLoot.Add(ItemDropRule.Common(ItemID.LeadBar, 2, 3, 5));
		}
	}
}
