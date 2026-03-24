using ChaoticDimensions.Content.Bosses.CrystalineDevourer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Summons
{
	public sealed class CrystalineSigil : ModItem
	{
		public override string Texture => "Terraria/Images/Item_520";

		public override void SetDefaults() {
			Item.width = 32;
			Item.height = 32;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.UseSound = SoundID.Roar;
			Item.rare = ItemRarityID.Red;
			Item.maxStack = 20;
			Item.consumable = false;
		}

		public override bool CanUseItem(Player player) {
			return !NPC.AnyNPCs(ModContent.NPCType<CrystalineDevourerHead>());
		}

		public override bool? UseItem(Player player) {
			if (Main.myPlayer == player.whoAmI) {
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<CrystalineDevourerHead>());
			}

			return true;
		}
	}
}
