// Reune valores, receitas e efeitos dos itens de Crystaline Devour Greaves.

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Armor.CrystalineDevour
{
	[AutoloadEquip(EquipType.Legs)]
	public sealed class CrystalineDevourGreaves : ModItem
	{
		// Define os valores iniciais usados pelo tModLoader.
		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 24;
			Item.value = Item.buyPrice(platinum: 4);
			Item.rare = ItemRarityID.Red;
			Item.defense = CrystalineDevourArmorCommon.SharedGreavesDefense;
		}

		// Regista as receitas deste conteudo.
		public override void AddRecipes() {
			CrystalineDevourArmorCommon.AddArmorRecipe(this, 28, 16);
		}
	}
}
