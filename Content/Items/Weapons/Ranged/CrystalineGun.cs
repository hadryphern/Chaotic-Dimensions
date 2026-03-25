using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Items.Materials;

namespace ChaoticDimensions.Content.Items.Weapons.Ranged
{
	public sealed class CrystalineGun : ModItem
	{
		public override void SetDefaults() {
			Item.width = 68;
			Item.height = 30;
			Item.useTime = 4;
			Item.useAnimation = 4;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item41;
			Item.DamageType = DamageClass.Ranged;
			Item.damage = 570;
			Item.knockBack = 4f;
			Item.noMelee = true;
			Item.crit = 150;
			Item.shoot = ProjectileID.Bullet;
			Item.shootSpeed = 28f;
			Item.useAmmo = AmmoID.Bullet;
			Item.consumeAmmoOnLastShotOnly = true;
			Item.rare = ItemRarityID.Purple;
			Item.value = Item.buyPrice(platinum: 2);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 56f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<CrystalineTear>(50)
				.AddIngredient(ItemID.SDMG)
				.AddIngredient(ItemID.LunarBar, 20)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}

		public override Vector2? HoldoutOffset() => new Vector2(-8f, 0f);
	}
}
