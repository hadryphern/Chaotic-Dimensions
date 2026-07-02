// Implementa o comportamento comum das oito familias de itens de progressao.

using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.Buffs;
using ChaoticDimensions.Content.Projectiles.Progression;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Progression
{
	public abstract class ProgressionCatalogItem : ModItem
	{
		protected abstract int CatalogIndex { get; }
		protected ProgressionItemSpec Spec => ProgressionItemCatalog.Get(CatalogIndex);
		public override string Texture => $"Terraria/Images/Item_{ProgressionItemCatalog.GetTextureItemId(Spec)}";

		protected void ApplyCommonDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.rare = ProgressionItemCatalog.GetRarity(Spec.Tier);
			Item.value = ProgressionItemCatalog.GetValue(Spec.Tier);
		}

		public override bool CanUseItem(Player player) {
			return ChaoticProgressionHelper.IsMet(ProgressionItemCatalog.GetGate(Spec.Tier));
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			string gate = ChaoticProgressionHelper.GetDebugLabel(ProgressionItemCatalog.GetGate(Spec.Tier));
			tooltips.Add(new TooltipLine(Mod, "ProgressionTier", $"Progressão: {ProgressionItemCatalog.GetTierName(Spec.Tier)} ({gate})") {
				OverrideColor = new Color(110, 190, 255)
			});
			tooltips.Add(new TooltipLine(Mod, "PlaceholderArt", "Arte vanilla temporária; código e progressão já funcionais") {
				OverrideColor = new Color(150, 150, 160)
			});
			if (ProgressionItemCatalog.GetDamage(Spec.Tier) >= 10000000 && Spec.Kind is ProgressionItemKind.Melee or ProgressionItemKind.Ranged or ProgressionItemKind.Magic or ProgressionItemKind.Summon) {
				tooltips.Add(new TooltipLine(Mod, "KrakenBreaker", "Poder suficiente para derrotar o Alien Kraken num único golpe") {
					OverrideColor = new Color(255, 80, 120)
				});
			}
		}

		public override void AddRecipes() {
			if (Spec.Kind == ProgressionItemKind.Material && Spec.Variant == 9) return;
			Recipe recipe = CreateRecipe();
			ProgressionItemCatalog.AddTierIngredients(recipe, Spec);
			ChaoticProgressionGate gate = ProgressionItemCatalog.GetGate(Spec.Tier);
			if (gate != ChaoticProgressionGate.Anytime) {
				LocalizedText conditionText = Language.GetOrRegister(
					$"Mods.ChaoticDimensions.Conditions.{gate}",
					() => $"Requer {ChaoticProgressionHelper.GetDebugLabel(gate)}"
				);
				recipe.AddCondition(conditionText, () => ChaoticProgressionHelper.IsMet(gate));
			}
			recipe.AddTile(ProgressionItemCatalog.GetCraftingTile(Spec.Tier));
			recipe.Register();
		}
	}

	public abstract class ProgressionMeleeItem : ProgressionCatalogItem
	{
		public override void SetDefaults() {
			ApplyCommonDefaults();
			Item.DamageType = DamageClass.Melee;
			Item.damage = ProgressionItemCatalog.GetDamage(Spec.Tier);
			Item.knockBack = 4f + Spec.Tier * 0.28f;
			Item.useTime = ProgressionItemCatalog.GetUseTime(Spec.Tier);
			Item.useAnimation = Item.useTime;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			if (Spec.Variant == 0) {
				Item.useStyle = ItemUseStyleID.Swing;
			}
			else {
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.noMelee = true;
				Item.noUseGraphic = true;
				Item.shoot = ModContent.ProjectileType<ProgressionWeaponProjectile>();
				Item.shootSpeed = 10f + Spec.Tier * 0.65f;
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Spec.Variant == 0) return false;
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Spec.Variant, Spec.Tier);
			return false;
		}
	}

	public abstract class ProgressionRangedItem : ProgressionCatalogItem
	{
		public override void SetDefaults() {
			ApplyCommonDefaults();
			Item.DamageType = DamageClass.Ranged;
			Item.damage = ProgressionItemCatalog.GetDamage(Spec.Tier);
			Item.knockBack = 2.5f + Spec.Tier * 0.2f;
			Item.useTime = ProgressionItemCatalog.GetUseTime(Spec.Tier);
			Item.useAnimation = Item.useTime;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.UseSound = Spec.Variant == 0 ? SoundID.Item5 : Spec.Variant == 1 ? SoundID.Item11 : SoundID.Item61;
			Item.useAmmo = Spec.Variant == 0 ? AmmoID.Arrow : Spec.Variant == 1 ? AmmoID.Bullet : AmmoID.Rocket;
			Item.shoot = Spec.Variant == 0 ? ProjectileID.WoodenArrowFriendly : Spec.Variant == 1 ? ProjectileID.Bullet : ProjectileID.Grenade;
			Item.shootSpeed = 8f + Spec.Tier * 0.9f;
		}
	}

	public abstract class ProgressionMagicItem : ProgressionCatalogItem
	{
		public override void SetDefaults() {
			ApplyCommonDefaults();
			Item.DamageType = DamageClass.Magic;
			Item.damage = ProgressionItemCatalog.GetDamage(Spec.Tier);
			Item.knockBack = 3f + Spec.Tier * 0.2f;
			Item.mana = System.Math.Max(2, 5 + Spec.Tier);
			Item.useTime = ProgressionItemCatalog.GetUseTime(Spec.Tier);
			Item.useAnimation = Item.useTime;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item20;
			Item.shoot = ModContent.ProjectileType<ProgressionWeaponProjectile>();
			Item.shootSpeed = 9f + Spec.Tier * 0.7f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 10 + Spec.Variant, Spec.Tier);
			return false;
		}
	}

	public abstract class ProgressionSummonItem : ProgressionCatalogItem
	{
		public override void SetDefaults() {
			ApplyCommonDefaults();
			int damage = ProgressionItemCatalog.GetDamage(Spec.Tier);
			int useTime = ProgressionItemCatalog.GetUseTime(Spec.Tier);
			if (Spec.Variant == 1) {
				Item.DefaultToWhip(ModContent.ProjectileType<ProgressionWhipProjectile>(), damage, 2f + Spec.Tier * 0.18f, 4f, useTime);
				Item.rare = ProgressionItemCatalog.GetRarity(Spec.Tier);
				Item.value = ProgressionItemCatalog.GetValue(Spec.Tier);
				return;
			}
			Item.DamageType = DamageClass.Summon;
			Item.damage = damage;
			Item.knockBack = 2f + Spec.Tier * 0.16f;
			Item.mana = System.Math.Max(3, 6 + Spec.Tier);
			Item.useTime = useTime + 8;
			Item.useAnimation = Item.useTime;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item44;
			Item.buffType = ModContent.BuffType<ProgressionMinionBuff>();
			Item.shoot = ModContent.ProjectileType<ProgressionMinionProjectile>();
			Item.shootSpeed = 10f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (Spec.Variant == 1) return true;
			player.AddBuff(Item.buffType, 2);
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI, Spec.Variant, Spec.Tier);
			return false;
		}
	}

	public abstract class ProgressionAccessoryItem : ProgressionCatalogItem
	{
		public override void SetDefaults() {
			ApplyCommonDefaults();
			Item.accessory = true;
			Item.defense = Spec.Tier * 3;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			float power = 0.02f + Spec.Tier * 0.015f;
			switch (Spec.Variant % 5) {
				case 0:
					player.GetDamage(DamageClass.Melee) += power;
					player.GetAttackSpeed(DamageClass.Melee) += power * 0.6f;
					break;
				case 1:
					player.GetDamage(DamageClass.Ranged) += power;
					player.GetCritChance(DamageClass.Ranged) += Spec.Tier * 0.8f;
					break;
				case 2:
					player.GetDamage(DamageClass.Magic) += power;
					player.statManaMax2 += 10 + Spec.Tier * 8;
					break;
				case 3:
					player.GetDamage(DamageClass.Summon) += power;
					player.maxMinions += 1 + Spec.Tier / 5;
					break;
				default:
					player.moveSpeed += power;
					player.endurance += System.Math.Min(0.22f, power * 0.45f);
					player.noKnockback |= Spec.Tier >= 10;
					break;
			}
		}
	}

	public abstract class ProgressionToolItem : ProgressionCatalogItem
	{
		public override void SetDefaults() {
			ApplyCommonDefaults();
			Item.DamageType = DamageClass.Melee;
			Item.damage = System.Math.Max(5, ProgressionItemCatalog.GetDamage(Spec.Tier) / 2);
			Item.knockBack = 3f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = System.Math.Max(2, ProgressionItemCatalog.GetUseTime(Spec.Tier) / 2);
			Item.useAnimation = Item.useTime * 2;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			int power = System.Math.Min(1000, 35 + Spec.Tier * 32);
			if (Spec.Variant == 1) Item.axe = power / 5;
			else if (Spec.Variant == 2) Item.hammer = power;
			else Item.pick = power;
		}
	}

	public abstract class ProgressionConsumableItem : ProgressionCatalogItem
	{
		public override void SetDefaults() {
			ApplyCommonDefaults();
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.UseSound = SoundID.Item3;
			Item.healLife = 20 + Spec.Tier * 18;
			Item.healMana = 10 + Spec.Tier * 12;
		}

		public override bool? UseItem(Player player) {
			int duration = 60 * (45 + Spec.Tier * 5);
			int buff = Spec.Variant % 5 switch {
				0 => BuffID.Ironskin,
				1 => BuffID.Regeneration,
				2 => BuffID.Wrath,
				3 => BuffID.Swiftness,
				_ => BuffID.Endurance
			};
			player.AddBuff(buff, duration);
			return true;
		}
	}

	public abstract class ProgressionMaterialItem : ProgressionCatalogItem
	{
		public override void SetDefaults() {
			ApplyCommonDefaults();
			Item.maxStack = Item.CommonMaxStack;
		}
	}
}
