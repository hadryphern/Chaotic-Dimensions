using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.NPCs.OreSpawn;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Summons.OreSpawn
{
	public sealed class HerculesTotem : OreSpawnPrototypeSummonItem
	{
		protected override int TargetNpcType => ModContent.NPCType<Hercules>();
		protected override ChaoticProgressionGate RequiredGate => ChaoticProgressionGate.PostEvilBoss;

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Vine, 2)
				.AddIngredient(ItemID.JungleSpores, 6)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
