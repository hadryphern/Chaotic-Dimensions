using ChaoticDimensions.Content.Bosses.CrystalineDevourer;
using ChaoticDimensions.Common.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChaoticDimensions.Content.Items.Materials;

namespace ChaoticDimensions.Content.Items.Summons
{
	public sealed class CrystalineSigil : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 13;
		}

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.UseSound = SoundID.Roar;
			Item.rare = ItemRarityID.Purple;
			Item.maxStack = 20;
			Item.consumable = false;
		}

		public override bool CanUseItem(Player player) {
			return NPC.downedMoonlord && !CrystalineDevourerIntroSystem.IsActive && !NPC.AnyNPCs(ModContent.NPCType<CrystalineDevourerHead>());
		}

		public override bool? UseItem(Player player) {
			if (Main.myPlayer == player.whoAmI) {
				CrystalineDevourerIntroSystem.StartIntro(player);
			}

			return true;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.CrystalShard, 250)
				.AddIngredient(ItemID.LunarBar, 250)
				.AddIngredient(ItemID.WormFood)
				.AddTile(TileID.LunarCraftingStation)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.CrystalShard, 250)
				.AddIngredient(ItemID.LunarBar, 250)
				.AddIngredient(ItemID.BloodySpine)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
