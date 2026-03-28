using ChaoticDimensions.Content.Players;
using ChaoticDimensions.Content.Tiles.ShadowBiome;
using ChaoticDimensions.Content.Items.MinecraftLegacy;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.ShadowBiome
{
	public abstract class ShadowBiomeItemBase : ModItem
	{
		public override string Texture => $"ChaoticDimensions/Content/Items/ShadowBiome/{GetType().Name}";

		protected virtual int ItemWidth => 20;
		protected virtual int ItemHeight => 20;
		protected virtual int ItemRarity => ItemRarityID.LightRed;
		protected virtual int ItemValue => Item.sellPrice(silver: 20);

		public override void SetDefaults() {
			Item.width = ItemWidth;
			Item.height = ItemHeight;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = ItemValue;
			Item.rare = ItemRarity;
		}
	}

	public abstract class ShadowBiomePlaceableItemBase : ShadowBiomeItemBase
	{
		protected abstract int PlaceTileType { get; }

		public override void SetDefaults() {
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = PlaceTileType;
		}
	}

	public sealed class ShadowDirtBlock : ShadowBiomePlaceableItemBase
	{
		protected override int PlaceTileType => ModContent.TileType<ShadowDirtTile>();
	}

	public sealed class ShadowStoneBlock : ShadowBiomePlaceableItemBase
	{
		protected override int PlaceTileType => ModContent.TileType<ShadowStoneTile>();
		protected override int ItemRarity => ItemRarityID.Orange;
	}

	public sealed class ShadowWood : ShadowBiomePlaceableItemBase
	{
		protected override int PlaceTileType => ModContent.TileType<ShadowWoodTile>();
		protected override int ItemRarity => ItemRarityID.Orange;
	}

	public sealed class ShadowScrap : ShadowBiomeItemBase
	{
		protected override int ItemWidth => 18;
		protected override int ItemHeight => 18;
		protected override int ItemRarity => ItemRarityID.Pink;
		protected override int ItemValue => Item.sellPrice(silver: 75);
	}

	public sealed class SoulOfShadow : ShadowBiomeItemBase
	{
		protected override int ItemWidth => 22;
		protected override int ItemHeight => 22;
		protected override int ItemRarity => ItemRarityID.Yellow;
		protected override int ItemValue => Item.sellPrice(gold: 1);
	}

	public sealed class ShadowTotem : ShadowBiomeItemBase
	{
		protected override int ItemWidth => 28;
		protected override int ItemHeight => 34;
		protected override int ItemRarity => ItemRarityID.Yellow;
		protected override int ItemValue => Item.sellPrice(gold: 6);

		public override void SetDefaults() {
			base.SetDefaults();
			Item.accessory = true;
			Item.defense = 12;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<ShadowBiomePlayer>().shadowBlessing = true;
			player.moveSpeed += 0.06f;
			player.endurance += 0.04f;
		}
	}

	public sealed class ChaosCrystalPickaxe : ShadowBiomeItemBase
	{
		protected override int ItemWidth => 46;
		protected override int ItemHeight => 46;
		protected override int ItemRarity => ItemRarityID.Red;
		protected override int ItemValue => Item.sellPrice(platinum: 1);

		public override void SetDefaults() {
			base.SetDefaults();
			Item.DamageType = DamageClass.Melee;
			Item.damage = 210;
			Item.knockBack = 4.5f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 10;
			Item.useTime = 5;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.pick = 300;
			Item.UseSound = SoundID.Item23;
		}
	}
}
