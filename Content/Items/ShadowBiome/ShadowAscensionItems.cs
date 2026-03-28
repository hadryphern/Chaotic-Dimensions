using ChaoticDimensions.Content.Buffs;
using ChaoticDimensions.Content.Items.Accessories;
using ChaoticDimensions.Content.Items.Consumables;
using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.Items.MinecraftLegacy;
using ChaoticDimensions.Content.Items.Weapons.Magic;
using ChaoticDimensions.Content.Items.Weapons.Summon;
using ChaoticDimensions.Content.Players;
using ChaoticDimensions.Content.Projectiles.Magic;
using ChaoticDimensions.Content.Projectiles.Melee;
using ChaoticDimensions.Content.Projectiles.Ranged;
using ChaoticDimensions.Content.Projectiles.Summon;
using ChaoticDimensions.Content.Tiles.ShadowBiome;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.ShadowBiome
{
	internal static class ShadowAscensionRecipeHelper
	{
		public static Recipe CreateShadowRecipe(ModItem item, int shadowOreAmount, int scrapAmount, int soulAmount, int rosalitaAmount) {
			return item.CreateRecipe()
				.AddIngredient<ShadowOre>(shadowOreAmount)
				.AddIngredient<ShadowScrap>(scrapAmount)
				.AddIngredient<SoulOfShadow>(soulAmount)
				.AddIngredient<RosalitaGem>(rosalitaAmount)
				.AddTile(ModContent.TileType<GodnessAnvilTile>());
		}
	}

	public sealed class GodnessAnvil : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/GodnessAnvil";

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = Item.CommonMaxStack;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<GodnessAnvilTile>();
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(platinum: 2);
		}
	}

	public sealed class HeartOfTheGod : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/HeartOfTheGod";

		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = Item.CommonMaxStack;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item4;
			Item.consumable = true;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(platinum: 1);
		}

		public override bool CanUseItem(Player player) {
			ShadowAscensionPlayer shadowPlayer = player.GetModPlayer<ShadowAscensionPlayer>();
			return player.ConsumedLifeCrystals == Player.LifeCrystalMax &&
				player.ConsumedLifeFruit == Player.LifeFruitMax &&
				shadowPlayer.heartOfTheGodUses < ShadowAscensionPlayer.MaxHeartOfTheGodUses;
		}

		public override bool? UseItem(Player player) {
			ShadowAscensionPlayer shadowPlayer = player.GetModPlayer<ShadowAscensionPlayer>();
			if (shadowPlayer.heartOfTheGodUses >= ShadowAscensionPlayer.MaxHeartOfTheGodUses) {
				return null;
			}

			player.UseHealthMaxIncreasingItem(ShadowAscensionPlayer.LifePerHeartOfTheGod);
			shadowPlayer.heartOfTheGodUses++;
			return true;
		}
	}

	public sealed class HeartOfShadows : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/HeartOfShadows";

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.accessory = true;
			Item.defense = 245;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(platinum: 6);
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<CrystalinePlayer>().crystalineEyeEquipped = true;
			player.statLifeMax2 += 260;
			player.statManaMax2 += 280;
			player.statDefense += 245;
			player.GetDamage(DamageClass.Generic) += 1.15f;
			player.GetCritChance(DamageClass.Generic) += 22f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.28f;
			player.moveSpeed += 0.42f;
			player.maxRunSpeed += 3.5f;
			player.runAcceleration *= 1.35f;
			player.maxMinions += 8;
			player.whipRangeMultiplier += 0.28f;
			player.endurance += 0.18f;
			player.noKnockback = true;
		}

		public override void AddRecipes() {
			ShadowAscensionRecipeHelper.CreateShadowRecipe(this, 50, 23, 0, 25)
				.AddIngredient(ItemID.HallowedBar, 35)
				.AddIngredient<MonthraScale>(10)
				.AddIngredient<RosalitaShield>(1)
				.AddIngredient<CrystalineEye>(1)
				.AddIngredient<ShadowMeleePotion>(1)
				.Register();
		}
	}

	[AutoloadEquip(EquipType.Shoes)]
	public sealed class GloryBoots : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/GloryBoots";

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.accessory = true;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(platinum: 3);
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<ShadowBiomePlayer>().shadowBlessing = true;
			player.moveSpeed += 0.32f;
			player.accRunSpeed = 12.5f;
			player.rocketBoots = 4;
			player.vanityRocketBoots = 4;
			player.waterWalk2 = true;
			player.waterWalk = true;
			player.iceSkate = true;
			player.fireWalk = true;
			player.noFallDmg = true;
			player.lavaRose = true;
			player.lavaMax += 8 * 60;

			if (!hideVisual) {
				player.CancelAllBootRunVisualEffects();
				player.hellfireTreads = true;
			}
		}

		public override void UpdateVanity(Player player) {
			player.CancelAllBootRunVisualEffects();
			player.vanityRocketBoots = 4;
			player.hellfireTreads = true;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.TerrasparkBoots)
				.AddIngredient<ShadowOre>(150)
				.AddIngredient<ShadowScrap>(50)
				.AddIngredient<RosalitaGem>(25)
				.AddIngredient(ItemID.SoulofNight, 36)
				.AddIngredient<ShadowTotem>(1)
				.AddTile(ModContent.TileType<GodnessAnvilTile>())
				.Register();
		}
	}

	public sealed class ShadowManaPotion : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/ShadowManaPotion";

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.SuperManaPotion);
			Item.width = 24;
			Item.height = 32;
			Item.healMana = 450;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(gold: 5);
			Item.buffType = ModContent.BuffType<ShadowManaPotionBuff>();
			Item.buffTime = 60 * 60 * 5;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<CrystalinePotion>(1)
				.AddIngredient(ItemID.SuperManaPotion, 5)
				.AddIngredient<ShadowOre>(20)
				.AddIngredient<SoulOfShadow>(8)
				.AddIngredient<ShadowScrap>(10)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}

	public sealed class ShadowMeleePotion : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/ShadowMeleePotion";

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.IronskinPotion);
			Item.width = 24;
			Item.height = 32;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(gold: 5);
			Item.buffType = ModContent.BuffType<ShadowMeleePotionBuff>();
			Item.buffTime = 60 * 60 * 8;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<CrystalinePotion>(1)
				.AddIngredient(ItemID.WrathPotion, 2)
				.AddIngredient(ItemID.IronskinPotion, 2)
				.AddIngredient<ShadowOre>(24)
				.AddIngredient<SoulOfShadow>(8)
				.AddIngredient<ShadowScrap>(10)
				.AddTile(TileID.Bottles)
				.Register();
		}
	}

	public sealed class ShadowSummonStaff : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/ShadowSummonStaff";

		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 56;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 22;
			Item.useTime = 22;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.damage = 3600;
			Item.knockBack = 5f;
			Item.mana = 20;
			Item.buffType = ModContent.BuffType<ShadowCrystalMinionBuff>();
			Item.shoot = ModContent.ProjectileType<ShadowCrystalMinion>();
			Item.shootSpeed = 10f;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(platinum: 2);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2);
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void AddRecipes() {
			ShadowAscensionRecipeHelper.CreateShadowRecipe(this, 80, 28, 12, 24)
				.AddIngredient<MonthraButterflyStaff>(1)
				.AddIngredient<RatrixStick>(2)
				.Register();
		}
	}

	public sealed class ShadowWhip : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/ShadowWhip";

		public override void SetDefaults() {
			Item.DefaultToWhip(ModContent.ProjectileType<ShadowWhipProjectile>(), 750, 4f, 8);
			Item.rare = ItemRarityID.Red;
			Item.channel = true;
			Item.value = Item.buyPrice(platinum: 2);
		}

		public override bool MeleePrefix() => true;

		public override void AddRecipes() {
			ShadowAscensionRecipeHelper.CreateShadowRecipe(this, 78, 26, 10, 24)
				.AddIngredient<EclipsedMonthraWhip>(1)
				.AddIngredient<RatrixStick>(2)
				.Register();
		}
	}

	public sealed class ShadowBow : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/ShadowBow";

		public override void SetDefaults() {
			Item.width = 42;
			Item.height = 64;
			Item.damage = 4500;
			Item.DamageType = DamageClass.Ranged;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 18;
			Item.useTime = 18;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Arrow;
			Item.shoot = ModContent.ProjectileType<ShadowArrowProjectile>();
			Item.shootSpeed = 24f;
			Item.UseSound = SoundID.Item5;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(platinum: 3);
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			for (int i = 0; i < 10; i++) {
				float spread = MathHelper.Lerp(-0.24f, 0.24f, i / 9f);
				Vector2 shotVelocity = velocity.RotatedBy(spread) * Main.rand.NextFloat(0.94f, 1.08f);
				Projectile.NewProjectile(source, position, shotVelocity, ModContent.ProjectileType<ShadowArrowProjectile>(), damage, knockback, player.whoAmI);
			}

			return false;
		}

		public override void AddRecipes() {
			ShadowAscensionRecipeHelper.CreateShadowRecipe(this, 90, 30, 12, 28)
				.AddIngredient<EclipsedMonthraBow>(1)
				.AddIngredient<RatrixStick>(2)
				.Register();
		}
	}

	public sealed class ShadowArrow : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/ShadowArrow";

		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 32;
			Item.damage = 512;
			Item.DamageType = DamageClass.Ranged;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(silver: 4);
			Item.rare = ItemRarityID.Red;
			Item.shoot = ModContent.ProjectileType<ShadowArrowProjectile>();
			Item.shootSpeed = 5f;
			Item.ammo = AmmoID.Arrow;
		}

		public override void AddRecipes() {
			CreateRecipe(200)
				.AddIngredient<ShadowOre>(3)
				.AddIngredient<ShadowScrap>(2)
				.AddTile(ModContent.TileType<GodnessAnvilTile>())
				.Register();
		}
	}

	public sealed class ShadowBullet : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/ShadowBullet";

		public override void SetDefaults() {
			Item.width = 16;
			Item.height = 16;
			Item.damage = 512;
			Item.DamageType = DamageClass.Ranged;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.knockBack = 3f;
			Item.value = Item.sellPrice(silver: 4);
			Item.rare = ItemRarityID.Red;
			Item.shoot = ModContent.ProjectileType<ShadowBulletProjectile>();
			Item.shootSpeed = 6f;
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes() {
			CreateRecipe(200)
				.AddIngredient<ShadowOre>(3)
				.AddIngredient<ShadowScrap>(2)
				.AddTile(ModContent.TileType<GodnessAnvilTile>())
				.Register();
		}
	}

	public sealed class ShadowStaff : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/ShadowStaff";

		public override void SetStaticDefaults() {
			Item.staff[Type] = true;
		}

		public override void SetDefaults() {
			Item.width = 42;
			Item.height = 64;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 3999;
			Item.knockBack = 5f;
			Item.crit = 200;
			Item.mana = 18;
			Item.shoot = ModContent.ProjectileType<ShadowBoltProjectile>();
			Item.shootSpeed = 26f;
			Item.UseSound = SoundID.Item43;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(platinum: 3);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			for (int i = -1; i <= 1; i++) {
				Vector2 perturbedVelocity = velocity.RotatedBy(MathHelper.ToRadians(5f * i)) * (1f + i * 0.05f);
				Projectile.NewProjectile(source, position, perturbedVelocity, type, damage, knockback, player.whoAmI, i * 0.2f);
			}

			return false;
		}

		public override void AddRecipes() {
			ShadowAscensionRecipeHelper.CreateShadowRecipe(this, 86, 30, 14, 26)
				.AddIngredient<CrystalineStaff>(1)
				.AddIngredient<RatrixStick>(2)
				.Register();
		}
	}

	public sealed class ShadowZenith : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/ShadowBiome/ShadowZenith";

		public override void SetDefaults() {
			Item.width = 64;
			Item.height = 64;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 4300;
			Item.knockBack = 8f;
			Item.crit = 200;
			Item.shoot = ModContent.ProjectileType<ShadowZenithProjectile>();
			Item.shootSpeed = 30f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.buyPrice(platinum: 4);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 baseDirection = velocity.SafeNormalize(Vector2.UnitX);
			Vector2 normal = baseDirection.RotatedBy(MathHelper.PiOver2);
			Vector2 centerSpawnPosition = player.MountedCenter + (baseDirection * 38f);
			Vector2 swordVelocity = baseDirection * Item.shootSpeed;

			int centerProjectileIndex = Projectile.NewProjectile(source, centerSpawnPosition, swordVelocity, type, damage, knockback, player.whoAmI);
			if (centerProjectileIndex >= 0 && centerProjectileIndex < Main.maxProjectiles) {
				Projectile centerProjectile = Main.projectile[centerProjectileIndex];
				float centerIdentity = centerProjectile.identity;

				Projectile.NewProjectile(source, centerSpawnPosition + (normal * ShadowZenithProjectile.OrbitRadius), swordVelocity, type, damage, knockback, player.whoAmI, centerIdentity, 1f);
				Projectile.NewProjectile(source, centerSpawnPosition - (normal * ShadowZenithProjectile.OrbitRadius), swordVelocity, type, damage, knockback, player.whoAmI, centerIdentity, -1f);
			}

			return false;
		}

		public override void AddRecipes() {
			ShadowAscensionRecipeHelper.CreateShadowRecipe(this, 110, 36, 16, 30)
				.AddIngredient(ItemID.Zenith)
				.AddIngredient<RatrixStick>(3)
				.Register();
		}
	}
}
