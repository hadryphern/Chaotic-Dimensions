using ChaoticDimensions.Common.Progression;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Summons.OreSpawn
{
	public abstract class OreSpawnPrototypeSummonItem : ModItem
	{
		protected abstract int TargetNpcType { get; }
		protected abstract ChaoticProgressionGate RequiredGate { get; }

		public override void SetStaticDefaults() {
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 10;
		}

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 35;
			Item.useTime = 35;
			Item.UseSound = SoundID.Roar;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 1;
			Item.consumable = false;
		}

		public override bool CanUseItem(Player player) {
			return ChaoticProgressionHelper.IsMet(RequiredGate) && !NPC.AnyNPCs(TargetNpcType);
		}

		public override bool? UseItem(Player player) {
			if (Main.myPlayer == player.whoAmI) {
				NPC.SpawnOnPlayer(player.whoAmI, TargetNpcType);
			}

			return true;
		}
	}
}
