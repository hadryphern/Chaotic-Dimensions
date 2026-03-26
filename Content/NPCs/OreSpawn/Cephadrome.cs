using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.Items.Materials.OreSpawn;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.OreSpawn
{
	public sealed class Cephadrome : OreSpawnPrototypeNpc
	{
		protected override bool IsFlying => true;
		protected override bool IsMiniBoss => true;
		protected override int PrototypeWidth => 58;
		protected override int PrototypeHeight => 46;
		protected override int PrototypeDamage => 60;
		protected override int PrototypeDefense => 20;
		protected override int PrototypeLifeMax => 2400;
		protected override float PrototypeKnockBackResist => 0.02f;
		protected override float PrototypeValue => Item.buyPrice(gold: 2);
		protected override float PrototypeSpawnChance => DaySurfaceChance() * 0.018f;
		protected override ChaoticProgressionGate ProgressionGate => ChaoticProgressionGate.PostWallOfFlesh;
		protected override int VanillaAIType => NPCID.GiantBat;

		protected override bool CanSpawnHere(NPCSpawnInfo spawnInfo) {
			return spawnInfo.Player.ZoneSnow && spawnInfo.Player.ZoneOverworldHeight;
		}

		public override void SetDefaults() {
			base.SetDefaults();
			NPC.HitSound = SoundID.NPCHit5;
			NPC.DeathSound = SoundID.NPCDeath14;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CephadromeHorn>(), 1, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ItemID.Ruby, 1, 6, 12));
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
			target.AddBuff(BuffID.Frostburn, 180);
		}
	}
}
