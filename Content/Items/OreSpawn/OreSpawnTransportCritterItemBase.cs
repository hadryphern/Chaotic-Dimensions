using ChaoticDimensions.Common.OreSpawn;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.OreSpawn
{
	public abstract class OreSpawnTransportCritterItemBase : ModItem
	{
		protected abstract OreSpawnDimensionId TargetDimension { get; }

		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(silver: 50);
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item6;
			Item.noMelee = true;
			Item.consumable = false;
		}

		public override bool? UseItem(Player player) {
			OreSpawnDimensionTravel.RequestTransportFromClient(player, TargetDimension);
			return true;
		}

		public override bool CanUseItem(Player player) {
			return OreSpawnDimensionLayout.SupportsOreSpawnDimensions() && player.whoAmI == Main.myPlayer;
		}
	}
}
