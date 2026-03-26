using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.Items.Weapons.Melee;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.OreSpawn
{
	public sealed class Hercules : OreSpawnPrototypeNpc
	{
		protected override bool IsMiniBoss => true;
		protected override int PrototypeWidth => 42;
		protected override int PrototypeHeight => 48;
		protected override int PrototypeDamage => 38;
		protected override int PrototypeDefense => 14;
		protected override int PrototypeLifeMax => 960;
		protected override float PrototypeKnockBackResist => 0.1f;
		protected override float PrototypeValue => Item.buyPrice(gold: 1);
		protected override float PrototypeSpawnChance => DaySurfaceChance() * 0.028f;
		protected override ChaoticProgressionGate ProgressionGate => ChaoticProgressionGate.PostEvilBoss;
		protected override int VanillaAIType => NPCID.Mimic;

		protected override bool CanSpawnHere(NPCSpawnInfo spawnInfo) {
			return IsForestOrJungleSurface(spawnInfo.Player);
		}

		public override void SetDefaults() {
			base.SetDefaults();
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath6;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BigHammer>(), 1, 1, 1));
		}
	}
}
