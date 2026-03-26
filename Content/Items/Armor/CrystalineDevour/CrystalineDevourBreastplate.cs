using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Armor.CrystalineDevour
{
	[AutoloadEquip(EquipType.Body)]
	public sealed class CrystalineDevourBreastplate : ModItem
	{
		public override void SetDefaults() {
			Item.width = 36;
			Item.height = 26;
			Item.value = Item.buyPrice(platinum: 4);
			Item.rare = ItemRarityID.Red;
			Item.defense = CrystalineDevourArmorCommon.SharedBreastplateDefense;
		}

		public override void AddRecipes() {
			CrystalineDevourArmorCommon.AddArmorRecipe(this, 34, 18);
		}
	}
}
