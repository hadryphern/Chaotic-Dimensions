using ChaoticDimensions.Content.Tiles.OreSpawn;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Materials.OreSpawn
{
	public sealed class Amethyst : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Blue;
		protected override int PrototypeValue => Item.buyPrice(silver: 70);
		protected override int PlaceTileType => ModContent.TileType<AmethystOreTile>();
	}

	public sealed class Ruby : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Green;
		protected override int PrototypeValue => Item.buyPrice(silver: 90);
		protected override int PlaceTileType => ModContent.TileType<RubyOreTile>();
	}

	public sealed class PinkTourmaline : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Orange;
		protected override int PrototypeValue => Item.buyPrice(silver: 95);
		protected override int PlaceTileType => ModContent.TileType<PinkTourmalineOreTile>();
	}

	public sealed class TigersEye : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Orange;
		protected override int PrototypeValue => Item.buyPrice(silver: 95);
		protected override int PlaceTileType => ModContent.TileType<TigersEyeOreTile>();
	}

	public sealed class Kyanite : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.LightRed;
		protected override int PrototypeValue => Item.buyPrice(silver: 120);
		protected override int PlaceTileType => ModContent.TileType<KyaniteOreTile>();
	}

	public sealed class Salt : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.White;
		protected override int PrototypeValue => Item.buyPrice(silver: 20);
		protected override int PlaceTileType => ModContent.TileType<SaltOreTile>();
	}

	public sealed class TitaniumOre : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.LightRed;
		protected override int PrototypeValue => Item.buyPrice(silver: 140);
		protected override int PlaceTileType => ModContent.TileType<TitaniumOreTile>();
	}

	public sealed class UraniumOre : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Pink;
		protected override int PrototypeValue => Item.buyPrice(silver: 180);
		protected override int PlaceTileType => ModContent.TileType<UraniumOreTile>();
	}

	public sealed class AmethystBlock : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Blue;
		protected override int PrototypeValue => Item.buyPrice(silver: 60);
		protected override int PlaceTileType => ModContent.TileType<AmethystBlockTile>();

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Amethyst>(9)
				.Register();

			Recipe.Create(ModContent.ItemType<Amethyst>(), 9)
				.AddIngredient(ModContent.ItemType<AmethystBlock>())
				.Register();
		}
	}

	public sealed class RubyBlock : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Green;
		protected override int PrototypeValue => Item.buyPrice(silver: 75);
		protected override int PlaceTileType => ModContent.TileType<RubyBlockTile>();

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Ruby>(9)
				.Register();

			Recipe.Create(ModContent.ItemType<Ruby>(), 9)
				.AddIngredient(ModContent.ItemType<RubyBlock>())
				.Register();
		}
	}

	public sealed class PinkTourmalineBlock : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Orange;
		protected override int PrototypeValue => Item.buyPrice(silver: 80);
		protected override int PlaceTileType => ModContent.TileType<PinkTourmalineBlockTile>();

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<PinkTourmaline>(9)
				.Register();

			Recipe.Create(ModContent.ItemType<PinkTourmaline>(), 9)
				.AddIngredient(ModContent.ItemType<PinkTourmalineBlock>())
				.Register();
		}
	}

	public sealed class TigersEyeBlock : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Orange;
		protected override int PrototypeValue => Item.buyPrice(silver: 80);
		protected override int PlaceTileType => ModContent.TileType<TigersEyeBlockTile>();

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<TigersEye>(9)
				.Register();

			Recipe.Create(ModContent.ItemType<TigersEye>(), 9)
				.AddIngredient(ModContent.ItemType<TigersEyeBlock>())
				.Register();
		}
	}

	public sealed class KyaniteBlock : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.LightRed;
		protected override int PrototypeValue => Item.buyPrice(silver: 110);
		protected override int PlaceTileType => ModContent.TileType<KyaniteBlockTile>();

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Kyanite>(9)
				.Register();

			Recipe.Create(ModContent.ItemType<Kyanite>(), 9)
				.AddIngredient(ModContent.ItemType<KyaniteBlock>())
				.Register();
		}
	}

	public sealed class SaltBlock : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.White;
		protected override int PrototypeValue => Item.buyPrice(copper: 50);
		protected override int PlaceTileType => ModContent.TileType<SaltBlockTile>();

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Salt>(8)
				.Register();

			Recipe.Create(ModContent.ItemType<Salt>(), 8)
				.AddIngredient(ModContent.ItemType<SaltBlock>())
				.Register();
		}
	}

	public sealed class TitaniumBlock : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.LightRed;
		protected override int PrototypeValue => Item.buyPrice(silver: 250);
		protected override int PlaceTileType => ModContent.TileType<TitaniumBlockTile>();

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<TitaniumIngot>(9)
				.Register();

			Recipe.Create(ModContent.ItemType<TitaniumIngot>(), 9)
				.AddIngredient(ModContent.ItemType<TitaniumBlock>())
				.Register();
		}
	}

	public sealed class UraniumBlock : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Pink;
		protected override int PrototypeValue => Item.buyPrice(silver: 300);
		protected override int PlaceTileType => ModContent.TileType<UraniumBlockTile>();

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<UraniumIngot>(9)
				.Register();

			Recipe.Create(ModContent.ItemType<UraniumIngot>(), 9)
				.AddIngredient(ModContent.ItemType<UraniumBlock>())
				.Register();
		}
	}

	public sealed class MolenoidDirt : OreSpawnResourceItemBase
	{
		protected override int PrototypeValue => Item.buyPrice(copper: 10);
		protected override int PlaceTileType => ModContent.TileType<MolenoidDirtTile>();
	}

	public sealed class RedAntNest : OreSpawnResourceItemBase
	{
		protected override int PrototypeValue => Item.buyPrice(copper: 20);
		protected override int PlaceTileType => ModContent.TileType<RedAntNestTile>();
	}

	public sealed class TermiteNest : OreSpawnResourceItemBase
	{
		protected override int PrototypeValue => Item.buyPrice(copper: 20);
		protected override int PlaceTileType => ModContent.TileType<TermiteNestTile>();
	}

	public sealed class CrystalTreeLog : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Green;
		protected override int PrototypeValue => Item.buyPrice(copper: 80);
		protected override int PlaceTileType => ModContent.TileType<CrystalTreeLogTile>();
	}

	public sealed class SkyTreeLog : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.White;
		protected override int PrototypeValue => Item.buyPrice(copper: 70);
		protected override int PlaceTileType => ModContent.TileType<SkyTreeLogTile>();
	}

	public sealed class DuplicatorLog : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Orange;
		protected override int PrototypeValue => Item.buyPrice(copper: 90);
		protected override int PlaceTileType => ModContent.TileType<DuplicatorLogTile>();
	}

	public sealed class TeleportBlock : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.LightPurple;
		protected override int PrototypeValue => Item.buyPrice(silver: 250);
		protected override int PlaceTileType => ModContent.TileType<TeleportBlockTile>();

		public override void AddRecipes() {
			CreateRecipe(5)
				.AddIngredient<EnderPearlBlock>(2)
				.AddIngredient<EyeOfEnderBlock>(1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public sealed class EnderPearlBlock : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.LightPurple;
		protected override int PrototypeValue => Item.buyPrice(silver: 120);
		protected override int PlaceTileType => ModContent.TileType<EnderPearlBlockTile>();
	}

	public sealed class EyeOfEnderBlock : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.LightPurple;
		protected override int PrototypeValue => Item.buyPrice(silver: 150);
		protected override int PlaceTileType => ModContent.TileType<EyeOfEnderBlockTile>();
	}

	public sealed class FairyCrystal : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Green;
		protected override int PrototypeValue => Item.buyPrice(silver: 100);
	}

	public sealed class RatCrystal : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Green;
		protected override int PrototypeValue => Item.buyPrice(silver: 95);
	}

	public sealed class LavaFoam : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Orange;
		protected override int PrototypeValue => Item.buyPrice(silver: 110);
	}

	public sealed class MobzillaScale : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Red;
		protected override int PrototypeValue => Item.buyPrice(gold: 1);
	}

	public sealed class TitaniumNugget : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.LightRed;
		protected override int PrototypeValue => Item.buyPrice(silver: 60);

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<TitaniumOre>(3)
				.AddTile(TileID.Furnaces)
				.Register();
		}
	}

	public sealed class TitaniumIngot : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.LightRed;
		protected override int PrototypeValue => Item.buyPrice(silver: 240);

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<TitaniumNugget>(4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public sealed class UraniumNugget : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Pink;
		protected override int PrototypeValue => Item.buyPrice(silver: 90);

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<UraniumOre>(3)
				.AddTile(TileID.AdamantiteForge)
				.Register();
		}
	}

	public sealed class UraniumIngot : OreSpawnResourceItemBase
	{
		protected override int PrototypeRare => ItemRarityID.Pink;
		protected override int PrototypeValue => Item.buyPrice(silver: 360);

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<UraniumNugget>(4)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
