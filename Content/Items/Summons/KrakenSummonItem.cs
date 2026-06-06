using ChaoticDimensions.Common.Systems;
using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.Items.ShadowBiome;
using ChaoticDimensions.Content.NPCs.Kraken;
using ChaoticDimensions.Content.Tiles.ShadowBiome;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Summons
{
	public class KrakenSummonItem : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 14;
		}

		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 34;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.rare = ItemRarityID.Red;
			Item.value = Item.sellPrice(gold: 1);
			Item.noMelee = true;
			Item.consumable = false;
			Item.UseSound = SoundID.Roar;
		}

		public override bool CanUseItem(Player player) {
			return CanStartEncounter(player);
		}

		internal static bool CanStartEncounter(Player player) {
			return ChaoticDownedBossSystem.downedCrystalineDevourer
				&& !KrakenEventSystem.Instance.Active
				&& !NPC.AnyNPCs(ModContent.NPCType<KrakenBoss>());
		}

		public override bool? UseItem(Player player) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)global::ChaoticDimensions.ChaoticDimensions.MessageType.RequestKrakenEvent);
				packet.Write((byte)player.whoAmI);
				packet.Send();
			}
			else {
				KrakenEventSystem.Instance.StartEvent(player);
			}

			return true;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<CrystalineTear>(60)
				.AddIngredient<SoulOfShadow>(30)
				.AddIngredient(ItemID.LunarBar, 20)
				.AddTile(ModContent.TileType<GodnessAnvilTile>())
				.Register();
		}
	}
}
