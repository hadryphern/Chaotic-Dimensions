using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.NPCs.OreSpawn;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Summons.OreSpawn
{
	public sealed class CephadromeCaller : OreSpawnPrototypeSummonItem
	{
		protected override int TargetNpcType => ModContent.NPCType<Cephadrome>();
		protected override ChaoticProgressionGate RequiredGate => ChaoticProgressionGate.PostWallOfFlesh;

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Ruby, 8)
				.AddIngredient(ItemID.SoulofLight, 5)
				.AddIngredient(ItemID.IceFeather)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
