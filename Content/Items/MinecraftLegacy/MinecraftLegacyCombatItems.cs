using ChaoticDimensions.Content.Buffs;
using ChaoticDimensions.Content.Projectiles.Melee;
using ChaoticDimensions.Content.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.MinecraftLegacy
{
	public sealed class KrakenTear : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemWidth => 20;
		protected override int ItemHeight => 20;
		protected override int ItemRarity => ItemRarityID.LightRed;
		protected override int ItemValue => Item.sellPrice(silver: 75);
	}

	public sealed class FrozenFlame : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemWidth => 20;
		protected override int ItemHeight => 20;
		protected override int ItemRarity => ItemRarityID.LightRed;
		protected override int ItemValue => Item.sellPrice(silver: 45);
	}

	public sealed class ShadowCreeperHead : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemWidth => 22;
		protected override int ItemHeight => 22;
		protected override int ItemRarity => ItemRarityID.Pink;
		protected override int ItemValue => Item.sellPrice(gold: 2);
	}

	public sealed class EndernmonScale : MinecraftLegacyMaterialItemBase
	{
		protected override int ItemWidth => 22;
		protected override int ItemHeight => 22;
		protected override int ItemRarity => ItemRarityID.LightPurple;
		protected override int ItemValue => Item.sellPrice(gold: 4);
	}

	public sealed class HappyCreeperStaff : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/MinecraftLegacy/HappyCreeperStaff";

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.damage = 125;
			Item.knockBack = 3f;
			Item.mana = 10;
			Item.buffType = ModContent.BuffType<HappyCreeperMinionBuff>();
			Item.shoot = ModContent.ProjectileType<HappyCreeperMinion>();
			Item.shootSpeed = 10f;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.buyPrice(gold: 8);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2);
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
			return false;
		}
	}

	public sealed class SquidKrakenStaff : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/MinecraftLegacy/SquidKrakenStaff";

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 24;
			Item.useTime = 24;
			Item.UseSound = SoundID.Item44;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.damage = 235;
			Item.knockBack = 2f;
			Item.mana = 10;
			Item.buffType = ModContent.BuffType<SquidKrakenMinionBuff>();
			Item.shoot = ModContent.ProjectileType<SquidKrakenMinion>();
			Item.shootSpeed = 10f;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(gold: 16);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2);
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
			return false;
		}
	}

	public sealed class KrakenBlade : ModItem
	{
		public override string Texture => "ChaoticDimensions/Content/Items/MinecraftLegacy/KrakenBlade";

		public override void SetDefaults() {
			Item.width = 60;
			Item.height = 60;
			Item.damage = 110;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 24;
			Item.useTime = 24;
			Item.knockBack = 7f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(gold: 14);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			return false;
		}

		public override bool? UseItem(Player player) {
			if (Main.myPlayer != player.whoAmI) {
				return true;
			}

			for (int i = 0; i < 3; i++) {
				float rotation = MathHelper.TwoPi * i / 3f;
				Vector2 spawnOffset = rotation.ToRotationVector2() * 56f;
				Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center + spawnOffset, spawnOffset.SafeNormalize(Vector2.UnitX) * 6.5f, ModContent.ProjectileType<KrakenGuardianBolt>(), Item.damage / 2, 1f, player.whoAmI, rotation);
			}

			return true;
		}
	}
}
