using ChaoticDimensions.Content.Buffs;
using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.Items.MinecraftLegacy;
using ChaoticDimensions.Content.Projectiles.Magic;
using ChaoticDimensions.Content.Projectiles.Melee;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.ShadowBiome
{
	internal static class RosalitaRecipeCommon
	{
		public static Recipe CreateRosalitaWeaponRecipe(ModItem item, int rosalitaAmount, int scrapAmount, int hallowedBars) {
			return item.CreateRecipe()
				.AddIngredient<RosalitaGem>(rosalitaAmount)
				.AddIngredient<ShadowScrap>(scrapAmount)
				.AddIngredient(ItemID.HallowedBar, hallowedBars)
				.AddTile(TileID.MythrilAnvil);
		}

		public static Recipe CreateEclipsedMonthraRecipe(ModItem item, int rosalitaAmount, int monthraAmount, int scrapAmount) {
			return item.CreateRecipe()
				.AddIngredient<RosalitaGem>(rosalitaAmount)
				.AddIngredient<MonthraScale>(monthraAmount)
				.AddIngredient<ShadowScrap>(scrapAmount)
				.AddTile(TileID.MythrilAnvil);
		}
	}

	public sealed class RosalitaPickaxe : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/RosalitaPickaxe";

		public override void SetDefaults() {
			Item.width = 44;
			Item.height = 44;
			Item.damage = 66;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 18;
			Item.useTime = 9;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.pick = 245;
			Item.knockBack = 4f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(gold: 18);
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateRosalitaWeaponRecipe(this, 18, 10, 12).Register();
		}
	}

	public sealed class RosalitaAxe : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/RosalitaAxe";

		public override void SetDefaults() {
			Item.width = 44;
			Item.height = 44;
			Item.damage = 72;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 22;
			Item.useTime = 11;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.axe = 42;
			Item.knockBack = 5f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(gold: 18);
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateRosalitaWeaponRecipe(this, 16, 8, 10).Register();
		}
	}

	public sealed class RosalitaHammer : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/RosalitaHammer";

		public override void SetDefaults() {
			Item.width = 78;
			Item.height = 78;
			Item.damage = 84;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 24;
			Item.useTime = 12;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.hammer = 105;
			Item.knockBack = 6f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(gold: 20);
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateRosalitaWeaponRecipe(this, 18, 10, 12).Register();
		}
	}

	public sealed class RosalitaBlade : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/RosalitaBlade";

		public override void SetDefaults() {
			Item.width = 70;
			Item.height = 70;
			Item.damage = 108;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.knockBack = 7f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(gold: 24);
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateRosalitaWeaponRecipe(this, 22, 12, 14).Register();
		}
	}

	public sealed class RosalitaBow : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/RosalitaBow";

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 50;
			Item.damage = 92;
			Item.DamageType = DamageClass.Ranged;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 22;
			Item.useTime = 22;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Arrow;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 16f;
			Item.UseSound = SoundID.Item5;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(gold: 24);
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position, velocity.RotatedBy(-0.06f), type, damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity.RotatedBy(0.06f), type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateRosalitaWeaponRecipe(this, 20, 10, 12).Register();
		}
	}

	public sealed class RosalitaWand : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/RosalitaWand";

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 48;
			Item.damage = 118;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 18;
			Item.useTime = 18;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<RosalitaMagicBolt>();
			Item.shootSpeed = 14f;
			Item.UseSound = SoundID.Item20;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(gold: 24);
			Item.autoReuse = true;
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateRosalitaWeaponRecipe(this, 20, 12, 14).Register();
		}
	}

	public sealed class RosalitaWhip : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/RosalitaWhip";

		public override void SetDefaults() {
			Item.DefaultToWhip(ModContent.ProjectileType<RosalitaWhipProjectile>(), 72, 3f, 6);
			Item.width = 38;
			Item.height = 34;
			Item.rare = ItemRarityID.Yellow;
			Item.channel = true;
			Item.value = Item.buyPrice(gold: 24);
		}

		public override bool MeleePrefix() => true;

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateRosalitaWeaponRecipe(this, 18, 12, 12).Register();
		}
	}

	public sealed class EclipsedMonthraPickaxe : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/EclipsedMonthraPickaxe";

		public override void SetDefaults() {
			Item.width = 36;
			Item.height = 36;
			Item.damage = 220;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 16;
			Item.useTime = 8;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.pick = 275;
			Item.knockBack = 5f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.buyPrice(gold: 38);
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateEclipsedMonthraRecipe(this, 22, 14, 14).Register();
		}
	}

	public sealed class EclipsedMonthraAxe : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/EclipsedMonthraAxe";

		public override void SetDefaults() {
			Item.width = 64;
			Item.height = 64;
			Item.damage = 240;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 20;
			Item.useTime = 10;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.axe = 55;
			Item.knockBack = 5.4f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.buyPrice(gold: 38);
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateEclipsedMonthraRecipe(this, 20, 12, 14).Register();
		}
	}

	public sealed class EclipsedMonthraHammer : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/EclipsedMonthraHammer";

		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 38;
			Item.damage = 270;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 22;
			Item.useTime = 11;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.hammer = 145;
			Item.knockBack = 6.5f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.buyPrice(gold: 42);
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateEclipsedMonthraRecipe(this, 22, 14, 14).Register();
		}
	}

	public sealed class EclipsedMonthraBlade : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/EclipsedMonthraBlade";

		public override void SetDefaults() {
			Item.width = 74;
			Item.height = 74;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 18;
			Item.useTime = 18;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 580;
			Item.knockBack = 7.2f;
			Item.shoot = ModContent.ProjectileType<MonthraBladeProjectile>();
			Item.shootSpeed = 1f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.buyPrice(gold: 44);
		}

		public override bool CanUseItem(Player player) {
			return player.ownedProjectileCounts[Item.shoot] == 0;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity, int type, int damage, float knockback) {
			float startAngle = velocity.SafeNormalize(Microsoft.Xna.Framework.Vector2.UnitX).ToRotation() - Microsoft.Xna.Framework.MathHelper.Pi;
			Projectile.NewProjectile(source, player.MountedCenter, Microsoft.Xna.Framework.Vector2.Zero, type, damage, knockback, player.whoAmI, startAngle);
			return false;
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateEclipsedMonthraRecipe(this, 24, 16, 16).Register();
		}
	}

	public sealed class EclipsedMonthraBow : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/EclipsedMonthraBow";

		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 58;
			Item.damage = 480;
			Item.DamageType = DamageClass.Ranged;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 18;
			Item.useTime = 18;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Arrow;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 18f;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.buyPrice(gold: 44);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity, int type, int damage, float knockback) {
			for (int i = 0; i < 4; i++) {
				float spread = Microsoft.Xna.Framework.MathHelper.Lerp(-0.14f, 0.14f, i / 3f);
				Projectile.NewProjectile(source, position, velocity.RotatedBy(spread), type, damage, knockback, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateEclipsedMonthraRecipe(this, 22, 14, 16).Register();
		}
	}

	public sealed class EclipsedMonthraWand : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/EclipsedMonthraWand";

		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 54;
			Item.damage = 500;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 18;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 14;
			Item.useTime = 14;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<RosalitaMagicBolt>();
			Item.shootSpeed = 18f;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.buyPrice(gold: 44);
		}

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateEclipsedMonthraRecipe(this, 22, 16, 16).Register();
		}
	}

	public sealed class EclipsedMonthraWhip : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/EclipsedMonthraWhip";

		public override void SetDefaults() {
			Item.DefaultToWhip(ModContent.ProjectileType<EclipsedMonthraWhipProjectile>(), 360, 4f, 7);
			Item.width = 38;
			Item.height = 34;
			Item.rare = ItemRarityID.Cyan;
			Item.channel = true;
			Item.value = Item.buyPrice(gold: 44);
		}

		public override bool MeleePrefix() => true;

		public override void AddRecipes() {
			RosalitaRecipeCommon.CreateEclipsedMonthraRecipe(this, 20, 16, 16).Register();
		}
	}
}
