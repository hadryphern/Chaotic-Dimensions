using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.Items.Materials.OreSpawn;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.OreSpawn
{
	public sealed class EmperorScorpion : OreSpawnPrototypeNpc
	{
		protected override bool IsMiniBoss => true;
		protected override int PrototypeWidth => 58;
		protected override int PrototypeHeight => 34;
		protected override int PrototypeDamage => 42;
		protected override int PrototypeDefense => 16;
		protected override int PrototypeLifeMax => 1200;
		protected override float PrototypeKnockBackResist => 0.08f;
		protected override float PrototypeValue => Item.buyPrice(gold: 1);
		protected override float PrototypeSpawnChance => DaySurfaceChance() * 0.03f;
		protected override ChaoticProgressionGate ProgressionGate => ChaoticProgressionGate.PostEvilBoss;
		protected override int VanillaAIType => NPCID.DesertGhoulCorruption;

		public override void SetDefaults() {
			base.SetDefaults();
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.HitSound = SoundID.NPCHit31;
			NPC.DeathSound = SoundID.NPCDeath22;
		}

		protected override bool CanSpawnHere(NPCSpawnInfo spawnInfo) {
			return spawnInfo.Player.ZoneDesert && spawnInfo.Player.ZoneOverworldHeight;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EmperorScorpionScale>(), 1, 6, 10));
			npcLoot.Add(ItemDropRule.Common(ItemID.Diamond, 1, 4, 8));
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
			target.AddBuff(BuffID.Poisoned, 240);
		}
	}
}
