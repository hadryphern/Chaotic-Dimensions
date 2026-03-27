using ChaoticDimensions.Content.Tiles.MinecraftLegacy;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.MinecraftLegacy
{
	public abstract class MinecraftLegacyItemBase : ModItem
	{
		public override string Texture => $"ChaoticDimensions/Content/Items/MinecraftLegacy/{GetType().Name}";

		protected virtual int ItemWidth => 16;
		protected virtual int ItemHeight => 16;
		protected virtual int MaxStackSize => Item.CommonMaxStack;
		protected virtual int ItemValue => Item.sellPrice(silver: 10);
		protected virtual int ItemRarity => ItemRarityID.White;

		public override void SetDefaults() {
			Item.width = ItemWidth;
			Item.height = ItemHeight;
			Item.maxStack = MaxStackSize;
			Item.value = ItemValue;
			Item.rare = ItemRarity;
		}
	}

	public abstract class MinecraftLegacyMaterialItemBase : MinecraftLegacyItemBase
	{
	}

	public abstract class MinecraftLegacyPlaceableItemBase : MinecraftLegacyItemBase
	{
		protected abstract int TileType { get; }

		public override void SetDefaults() {
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = TileType;
		}
	}

	public abstract class MinecraftLegacySwordBase : MinecraftLegacyItemBase
	{
		protected abstract int Damage { get; }
		protected virtual float Knockback => 5f;
		protected virtual int UseTime => 22;

		public override void SetDefaults() {
			base.SetDefaults();
			Item.DamageType = DamageClass.Melee;
			Item.damage = Damage;
			Item.knockBack = Knockback;
			Item.useAnimation = UseTime;
			Item.useTime = UseTime;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(silver: 60);
		}
	}

	public abstract class MinecraftLegacyPickaxeBase : MinecraftLegacyItemBase
	{
		protected abstract int PickPower { get; }
		protected virtual int Damage => 8;
		protected virtual int UseTime => 18;

		public override void SetDefaults() {
			base.SetDefaults();
			Item.DamageType = DamageClass.Melee;
			Item.damage = Damage;
			Item.knockBack = 2.4f;
			Item.useAnimation = UseTime;
			Item.useTime = UseTime;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.pick = PickPower;
			Item.value = Item.sellPrice(silver: 55);
		}
	}

	public abstract class MinecraftLegacyAxeBase : MinecraftLegacyItemBase
	{
		protected abstract int AxePower { get; }
		protected virtual int Damage => 9;
		protected virtual int UseTime => 24;

		public override void SetDefaults() {
			base.SetDefaults();
			Item.DamageType = DamageClass.Melee;
			Item.damage = Damage;
			Item.knockBack = 4.2f;
			Item.useAnimation = UseTime;
			Item.useTime = UseTime;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.axe = AxePower;
			Item.value = Item.sellPrice(silver: 65);
		}
	}

	public abstract class MinecraftLegacyAccessoryBase : MinecraftLegacyItemBase
	{
		public override void SetDefaults() {
			base.SetDefaults();
			Item.accessory = true;
			Item.value = Item.sellPrice(gold: 1);
		}
	}

	public sealed class Alexandrite : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.Green;
		protected override int ItemValue => Item.sellPrice(silver: 25);
	}

	public sealed class RawAlexandrite : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.Green;
		protected override int ItemValue => Item.sellPrice(silver: 12);
	}

	public sealed class GlassStick : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.White;
	}

	public sealed class ChaosCrystal : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.Pink;
		protected override int ItemValue => Item.sellPrice(gold: 2);
	}

	public sealed class ShadowGem : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.Orange;
		protected override int ItemValue => Item.sellPrice(silver: 30);
	}

	public sealed class ShadowNugget : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.Blue;
		protected override int ItemValue => Item.sellPrice(silver: 8);
	}

	public sealed class RosalitaGem : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.LightRed;
		protected override int ItemValue => Item.sellPrice(silver: 45);
	}

	public sealed class RubyGem : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.Orange;
		protected override int ItemValue => Item.sellPrice(silver: 22);
	}

	public sealed class RubyNugget : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.Blue;
		protected override int ItemValue => Item.sellPrice(silver: 6);
	}

	public sealed class VortexGem : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.Pink;
		protected override int ItemValue => Item.sellPrice(gold: 1);
	}

	public sealed class BedrockStick : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.LightRed;
		protected override int ItemValue => Item.sellPrice(silver: 25);
	}

	public sealed class IronStick : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemRarity => ItemRarityID.Blue;
	}

	public sealed class AlexandriteBlock : MinecraftLegacyPlaceableItemBase
	{
		protected override int TileType => ModContent.TileType<AlexandriteBlockTile>();
		protected override int ItemRarity => ItemRarityID.Green;
	}

	public sealed class RawAlexandriteBlock : MinecraftLegacyPlaceableItemBase
	{
		protected override int TileType => ModContent.TileType<RawAlexandriteBlockTile>();
		protected override int ItemRarity => ItemRarityID.Green;
	}

	public sealed class GreystedWood : MinecraftLegacyPlaceableItemBase
	{
		protected override int TileType => ModContent.TileType<GreystedWoodTile>();
		protected override int ItemRarity => ItemRarityID.White;
	}

	public sealed class AlexandriteOre : MinecraftLegacyPlaceableItemBase
	{
		protected override int TileType => ModContent.TileType<AlexandriteOreTile>();
		protected override int ItemRarity => ItemRarityID.Green;
	}

	public sealed class AlexandriteDeepslateOre : MinecraftLegacyPlaceableItemBase
	{
		protected override int TileType => ModContent.TileType<AlexandriteDeepslateOreTile>();
		protected override int ItemRarity => ItemRarityID.Green;
	}

	public sealed class ShadowBlock : MinecraftLegacyPlaceableItemBase
	{
		protected override int TileType => ModContent.TileType<ShadowBlockTile>();
		protected override int ItemRarity => ItemRarityID.Orange;
	}

	public sealed class ShadowOre : MinecraftLegacyPlaceableItemBase
	{
		protected override int TileType => ModContent.TileType<ShadowOreTile>();
		protected override int ItemRarity => ItemRarityID.Orange;
	}

	public sealed class RubyOre : MinecraftLegacyPlaceableItemBase
	{
		protected override int TileType => ModContent.TileType<RubyOreTile>();
		protected override int ItemRarity => ItemRarityID.Orange;
	}

	public sealed class RosalitaOre : MinecraftLegacyPlaceableItemBase
	{
		protected override int TileType => ModContent.TileType<RosalitaOreTile>();
		protected override int ItemRarity => ItemRarityID.LightRed;
	}

	public sealed class BlueBerryPlant : MinecraftLegacyPlaceableItemBase
	{
		protected override int TileType => ModContent.TileType<BlueBerryPlantTile>();
		protected override int ItemWidth => 24;
		protected override int ItemHeight => 24;
		protected override int ItemRarity => ItemRarityID.Blue;
	}

	public sealed class RubySword : MinecraftLegacySwordBase
	{
		protected override int Damage => 18;
		protected override int ItemRarity => ItemRarityID.Orange;

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<RubyGem>(10)
				.AddIngredient<GlassStick>(4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public sealed class RubyPickaxe : MinecraftLegacyPickaxeBase
	{
		protected override int PickPower => 75;
		protected override int Damage => 10;
		protected override int ItemRarity => ItemRarityID.Orange;

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<RubyGem>(12)
				.AddIngredient<GlassStick>(4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public sealed class RubyAxe : MinecraftLegacyAxeBase
	{
		protected override int AxePower => 12;
		protected override int Damage => 16;
		protected override int ItemRarity => ItemRarityID.Orange;

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<RubyGem>(12)
				.AddIngredient<GlassStick>(4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public sealed class ShadowSword : MinecraftLegacySwordBase
	{
		protected override int Damage => 26;
		protected override float Knockback => 6.25f;
		protected override int ItemRarity => ItemRarityID.LightRed;

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<ShadowGem>(10)
				.AddIngredient<BedrockStick>(3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public sealed class ShadowPickaxe : MinecraftLegacyPickaxeBase
	{
		protected override int PickPower => 105;
		protected override int Damage => 14;
		protected override int ItemRarity => ItemRarityID.LightRed;

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<ShadowGem>(12)
				.AddIngredient<BedrockStick>(3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public sealed class VortexBlade : MinecraftLegacySwordBase
	{
		protected override int Damage => 34;
		protected override float Knockback => 6.8f;
		protected override int UseTime => 18;
		protected override int ItemWidth => 32;
		protected override int ItemHeight => 32;
		protected override int ItemRarity => ItemRarityID.Pink;

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<VortexGem>(12)
				.AddIngredient<ChaosCrystal>(4)
				.AddIngredient<GlassStick>(6)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public sealed class RosalitaShield : MinecraftLegacyAccessoryBase
	{
		protected override int ItemWidth => 32;
		protected override int ItemHeight => 32;
		protected override int ItemRarity => ItemRarityID.LightRed;

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.statDefense += 5;
			player.noKnockback = true;
		}
	}
}
