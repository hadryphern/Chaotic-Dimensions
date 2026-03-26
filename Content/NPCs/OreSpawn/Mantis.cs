using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.Items.Materials.OreSpawn;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.OreSpawn
{
	public sealed class Mantis : OreSpawnPrototypeNpc
	{
		protected override int PrototypeWidth => 42;
		protected override int PrototypeHeight => 42;
		protected override int PrototypeDamage => 28;
		protected override int PrototypeDefense => 10;
		protected override int PrototypeLifeMax => 220;
		protected override float PrototypeKnockBackResist => 0.25f;
		protected override float PrototypeValue => Item.buyPrice(silver: 95);
		protected override float PrototypeSpawnChance => DaySurfaceChance() * 0.09f;
		protected override ChaoticProgressionGate ProgressionGate => ChaoticProgressionGate.PostEyeOfCthulhu;
		protected override int VanillaAIType => NPCID.Hornet;

		protected override bool CanSpawnHere(NPCSpawnInfo spawnInfo) {
			return IsForestOrJungleSurface(spawnInfo.Player);
		}

		public override void SetDefaults() {
			base.SetDefaults();
			NPC.HitSound = SoundID.NPCHit11;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MantisClaw>(), 1, 1, 2));
		}
	}
}
