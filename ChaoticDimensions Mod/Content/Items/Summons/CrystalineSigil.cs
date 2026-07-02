// Reune valores, receitas e efeitos dos itens de Crystaline Sigil.

using ChaoticDimensions.Content.Bosses.CrystalineDevourer;
using ChaoticDimensions.Common.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Summons
{
	public sealed class CrystalineSigil : ModItem
	{
		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 13;
		}

		// Define os valores iniciais usados pelo tModLoader.
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

		// Confirma se o item pode ser usado neste momento.
		public override bool CanUseItem(Player player) {
			return NPC.downedMoonlord && !CrystalineDevourerIntroSystem.IsActive && !NPC.AnyNPCs(ModContent.NPCType<CrystalineDevourerHead>());
		}

		// Executa o efeito principal quando o item e usado.
		public override bool? UseItem(Player player) {
			if (Main.myPlayer == player.whoAmI) {
				CrystalineDevourerIntroSystem.StartIntro(player);
			}

			return true;
		}

		// Regista as receitas deste conteudo.
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
