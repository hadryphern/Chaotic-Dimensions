using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.NPCs.OreSpawn;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Summons.OreSpawn
{
	public sealed class CaterkillerBait : OreSpawnPrototypeSummonItem
	{
		protected override int TargetNpcType => ModContent.NPCType<Caterkiller>();
		protected override ChaoticProgressionGate RequiredGate => ChaoticProgressionGate.PostEvilBoss;

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Cobweb, 25)
				.AddIngredient(ItemID.Stinger, 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
