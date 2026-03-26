using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.NPCs.OreSpawn;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Summons.OreSpawn
{
	public sealed class EmperorScorpionIdol : OreSpawnPrototypeSummonItem
	{
		protected override int TargetNpcType => ModContent.NPCType<EmperorScorpion>();
		protected override ChaoticProgressionGate RequiredGate => ChaoticProgressionGate.PostEvilBoss;

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.AntlionMandible, 8)
				.AddIngredient(ItemID.Stinger, 5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
