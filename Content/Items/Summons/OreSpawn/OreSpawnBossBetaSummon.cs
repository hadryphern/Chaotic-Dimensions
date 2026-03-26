using ChaoticDimensions.Common.Progression;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Summons.OreSpawn
{
	public abstract class OreSpawnBossBetaSummon : ModItem
	{
		protected abstract int TargetNpcType { get; }

		public override void SetStaticDefaults() {
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
		}

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 35;
			Item.useTime = 35;
			Item.UseSound = SoundID.Roar;
			Item.rare = ItemRarityID.Yellow;
			Item.maxStack = 20;
			Item.consumable = false;
		}

		public override bool CanUseItem(Player player) {
			return ChaoticProgressionHelper.IsMet(ChaoticProgressionGate.PostChaoticApexTrio) && !NPC.AnyNPCs(TargetNpcType);
		}

		public override bool? UseItem(Player player) {
			if (Main.myPlayer == player.whoAmI) {
				NPC.SpawnOnPlayer(player.whoAmI, TargetNpcType);
			}

			return true;
		}
	}
}
