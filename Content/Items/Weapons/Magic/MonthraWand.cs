using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Weapons.Magic
{
	public sealed class MonthraWand : ModItem
	{
		public override void SetStaticDefaults() {
			Item.staff[Type] = true;
		}

		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 34;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 18;
			Item.useTime = 18;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 25;
			Item.knockBack = 3f;
			Item.mana = 6;
			Item.shoot = ModContent.ProjectileType<MonthraMagicFireball>();
			Item.shootSpeed = 9.5f;
			Item.UseSound = SoundID.Item20;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 1);
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<MonthraScale>(11)
				.AddIngredient(ItemID.DemoniteBar, 12)
				.AddIngredient(ItemID.ShadowScale, 3)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient<MonthraScale>(11)
				.AddIngredient(ItemID.CrimtaneBar, 12)
				.AddIngredient(ItemID.TissueSample, 3)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
