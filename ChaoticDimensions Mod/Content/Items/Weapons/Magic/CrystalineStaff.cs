// Reune valores, receitas e efeitos dos itens de Crystaline Staff.

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.Projectiles.Magic;

namespace ChaoticDimensions.Content.Items.Weapons.Magic
{
	public sealed class CrystalineStaff : ModItem
	{
		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			Item.staff[Type] = true;
		}

		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Item.width = 60;
			Item.height = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 8;
			Item.useTime = 8;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 570;
			Item.knockBack = 4f;
			Item.crit = 150;
			Item.mana = 6;
			Item.shoot = ModContent.ProjectileType<CrystalineBoltProjectile>();
			Item.shootSpeed = 21f;
			Item.UseSound = SoundID.Item43;
			Item.rare = ItemRarityID.Purple;
			Item.value = Item.buyPrice(platinum: 2);
		}

		// Cria o projetil e ajusta os dados do disparo.
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			for (int i = -1; i <= 1; i++) {
				Vector2 perturbedVelocity = velocity.RotatedBy(MathHelper.ToRadians(5f * i)) * (1f + i * 0.04f);
				Projectile.NewProjectile(source, position, perturbedVelocity, type, damage, knockback, player.whoAmI, i * 0.25f);
			}

			return false;
		}

		// Regista as receitas deste conteudo.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<CrystalineTear>(50)
				.AddIngredient(ItemID.LastPrism)
				.AddIngredient(ItemID.LunarBar, 20)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
