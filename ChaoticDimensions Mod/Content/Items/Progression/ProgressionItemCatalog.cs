// Centraliza os 245 itens de progressao e evita repetir formulas de balanceamento.

using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.Items.MinecraftLegacy;
using ChaoticDimensions.Content.Items.ShadowBiome;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Progression
{
	public enum ProgressionItemKind
	{
		Melee,
		Ranged,
		Magic,
		Summon,
		Accessory,
		Tool,
		Consumable,
		Material
	}

	public readonly record struct ProgressionItemSpec(ProgressionItemKind Kind, int Tier, int Variant);

	public static class ProgressionItemCatalog
	{
		private static readonly ProgressionItemSpec[] Specs = {
			new(ProgressionItemKind.Melee, 0, 0),
			new(ProgressionItemKind.Melee, 0, 1),
			new(ProgressionItemKind.Melee, 1, 0),
			new(ProgressionItemKind.Melee, 1, 1),
			new(ProgressionItemKind.Melee, 1, 2),
			new(ProgressionItemKind.Melee, 2, 0),
			new(ProgressionItemKind.Melee, 2, 1),
			new(ProgressionItemKind.Melee, 2, 2),
			new(ProgressionItemKind.Melee, 3, 0),
			new(ProgressionItemKind.Melee, 3, 1),
			new(ProgressionItemKind.Melee, 3, 2),
			new(ProgressionItemKind.Melee, 4, 0),
			new(ProgressionItemKind.Melee, 4, 1),
			new(ProgressionItemKind.Melee, 4, 2),
			new(ProgressionItemKind.Melee, 5, 0),
			new(ProgressionItemKind.Melee, 5, 1),
			new(ProgressionItemKind.Melee, 5, 2),
			new(ProgressionItemKind.Melee, 6, 0),
			new(ProgressionItemKind.Melee, 6, 1),
			new(ProgressionItemKind.Melee, 6, 2),
			new(ProgressionItemKind.Melee, 7, 0),
			new(ProgressionItemKind.Melee, 7, 1),
			new(ProgressionItemKind.Melee, 7, 2),
			new(ProgressionItemKind.Melee, 8, 0),
			new(ProgressionItemKind.Melee, 8, 1),
			new(ProgressionItemKind.Melee, 8, 2),
			new(ProgressionItemKind.Melee, 9, 0),
			new(ProgressionItemKind.Melee, 9, 1),
			new(ProgressionItemKind.Melee, 9, 2),
			new(ProgressionItemKind.Melee, 10, 0),
			new(ProgressionItemKind.Melee, 10, 1),
			new(ProgressionItemKind.Melee, 10, 2),
			new(ProgressionItemKind.Melee, 11, 0),
			new(ProgressionItemKind.Melee, 11, 1),
			new(ProgressionItemKind.Melee, 11, 2),
			new(ProgressionItemKind.Melee, 12, 0),
			new(ProgressionItemKind.Melee, 12, 1),
			new(ProgressionItemKind.Melee, 12, 2),
			new(ProgressionItemKind.Melee, 13, 0),
			new(ProgressionItemKind.Melee, 13, 1),
			new(ProgressionItemKind.Melee, 13, 2),
			new(ProgressionItemKind.Melee, 14, 0),
			new(ProgressionItemKind.Melee, 14, 1),
			new(ProgressionItemKind.Melee, 14, 2),
			new(ProgressionItemKind.Melee, 15, 0),
			new(ProgressionItemKind.Melee, 15, 1),
			new(ProgressionItemKind.Melee, 15, 2),
			new(ProgressionItemKind.Melee, 16, 0),
			new(ProgressionItemKind.Melee, 16, 1),
			new(ProgressionItemKind.Melee, 16, 2),
			new(ProgressionItemKind.Ranged, 0, 0),
			new(ProgressionItemKind.Ranged, 0, 1),
			new(ProgressionItemKind.Ranged, 1, 0),
			new(ProgressionItemKind.Ranged, 1, 1),
			new(ProgressionItemKind.Ranged, 2, 0),
			new(ProgressionItemKind.Ranged, 2, 1),
			new(ProgressionItemKind.Ranged, 3, 0),
			new(ProgressionItemKind.Ranged, 3, 1),
			new(ProgressionItemKind.Ranged, 4, 0),
			new(ProgressionItemKind.Ranged, 4, 1),
			new(ProgressionItemKind.Ranged, 5, 0),
			new(ProgressionItemKind.Ranged, 5, 1),
			new(ProgressionItemKind.Ranged, 6, 0),
			new(ProgressionItemKind.Ranged, 6, 1),
			new(ProgressionItemKind.Ranged, 6, 2),
			new(ProgressionItemKind.Ranged, 7, 0),
			new(ProgressionItemKind.Ranged, 7, 1),
			new(ProgressionItemKind.Ranged, 7, 2),
			new(ProgressionItemKind.Ranged, 8, 0),
			new(ProgressionItemKind.Ranged, 8, 1),
			new(ProgressionItemKind.Ranged, 8, 2),
			new(ProgressionItemKind.Ranged, 9, 0),
			new(ProgressionItemKind.Ranged, 9, 1),
			new(ProgressionItemKind.Ranged, 9, 2),
			new(ProgressionItemKind.Ranged, 10, 0),
			new(ProgressionItemKind.Ranged, 10, 1),
			new(ProgressionItemKind.Ranged, 10, 2),
			new(ProgressionItemKind.Ranged, 11, 0),
			new(ProgressionItemKind.Ranged, 11, 1),
			new(ProgressionItemKind.Ranged, 11, 2),
			new(ProgressionItemKind.Ranged, 12, 0),
			new(ProgressionItemKind.Ranged, 12, 1),
			new(ProgressionItemKind.Ranged, 12, 2),
			new(ProgressionItemKind.Ranged, 13, 0),
			new(ProgressionItemKind.Ranged, 13, 1),
			new(ProgressionItemKind.Ranged, 13, 2),
			new(ProgressionItemKind.Ranged, 14, 0),
			new(ProgressionItemKind.Ranged, 14, 1),
			new(ProgressionItemKind.Ranged, 14, 2),
			new(ProgressionItemKind.Ranged, 15, 0),
			new(ProgressionItemKind.Ranged, 15, 1),
			new(ProgressionItemKind.Ranged, 15, 2),
			new(ProgressionItemKind.Ranged, 16, 0),
			new(ProgressionItemKind.Ranged, 16, 1),
			new(ProgressionItemKind.Ranged, 16, 2),
			new(ProgressionItemKind.Magic, 0, 0),
			new(ProgressionItemKind.Magic, 0, 1),
			new(ProgressionItemKind.Magic, 1, 0),
			new(ProgressionItemKind.Magic, 1, 1),
			new(ProgressionItemKind.Magic, 2, 0),
			new(ProgressionItemKind.Magic, 2, 1),
			new(ProgressionItemKind.Magic, 3, 0),
			new(ProgressionItemKind.Magic, 3, 1),
			new(ProgressionItemKind.Magic, 4, 0),
			new(ProgressionItemKind.Magic, 4, 1),
			new(ProgressionItemKind.Magic, 5, 0),
			new(ProgressionItemKind.Magic, 5, 1),
			new(ProgressionItemKind.Magic, 6, 0),
			new(ProgressionItemKind.Magic, 6, 1),
			new(ProgressionItemKind.Magic, 6, 2),
			new(ProgressionItemKind.Magic, 7, 0),
			new(ProgressionItemKind.Magic, 7, 1),
			new(ProgressionItemKind.Magic, 7, 2),
			new(ProgressionItemKind.Magic, 8, 0),
			new(ProgressionItemKind.Magic, 8, 1),
			new(ProgressionItemKind.Magic, 8, 2),
			new(ProgressionItemKind.Magic, 9, 0),
			new(ProgressionItemKind.Magic, 9, 1),
			new(ProgressionItemKind.Magic, 9, 2),
			new(ProgressionItemKind.Magic, 10, 0),
			new(ProgressionItemKind.Magic, 10, 1),
			new(ProgressionItemKind.Magic, 10, 2),
			new(ProgressionItemKind.Magic, 11, 0),
			new(ProgressionItemKind.Magic, 11, 1),
			new(ProgressionItemKind.Magic, 11, 2),
			new(ProgressionItemKind.Magic, 12, 0),
			new(ProgressionItemKind.Magic, 12, 1),
			new(ProgressionItemKind.Magic, 12, 2),
			new(ProgressionItemKind.Magic, 13, 0),
			new(ProgressionItemKind.Magic, 13, 1),
			new(ProgressionItemKind.Magic, 13, 2),
			new(ProgressionItemKind.Magic, 14, 0),
			new(ProgressionItemKind.Magic, 14, 1),
			new(ProgressionItemKind.Magic, 14, 2),
			new(ProgressionItemKind.Magic, 15, 0),
			new(ProgressionItemKind.Magic, 15, 1),
			new(ProgressionItemKind.Magic, 15, 2),
			new(ProgressionItemKind.Magic, 16, 0),
			new(ProgressionItemKind.Magic, 16, 1),
			new(ProgressionItemKind.Magic, 16, 2),
			new(ProgressionItemKind.Summon, 0, 0),
			new(ProgressionItemKind.Summon, 0, 1),
			new(ProgressionItemKind.Summon, 1, 0),
			new(ProgressionItemKind.Summon, 1, 1),
			new(ProgressionItemKind.Summon, 2, 0),
			new(ProgressionItemKind.Summon, 2, 1),
			new(ProgressionItemKind.Summon, 3, 0),
			new(ProgressionItemKind.Summon, 3, 1),
			new(ProgressionItemKind.Summon, 4, 0),
			new(ProgressionItemKind.Summon, 4, 1),
			new(ProgressionItemKind.Summon, 5, 0),
			new(ProgressionItemKind.Summon, 5, 1),
			new(ProgressionItemKind.Summon, 6, 0),
			new(ProgressionItemKind.Summon, 6, 1),
			new(ProgressionItemKind.Summon, 6, 2),
			new(ProgressionItemKind.Summon, 7, 0),
			new(ProgressionItemKind.Summon, 7, 1),
			new(ProgressionItemKind.Summon, 7, 2),
			new(ProgressionItemKind.Summon, 8, 0),
			new(ProgressionItemKind.Summon, 8, 1),
			new(ProgressionItemKind.Summon, 8, 2),
			new(ProgressionItemKind.Summon, 9, 0),
			new(ProgressionItemKind.Summon, 9, 1),
			new(ProgressionItemKind.Summon, 9, 2),
			new(ProgressionItemKind.Summon, 10, 0),
			new(ProgressionItemKind.Summon, 10, 1),
			new(ProgressionItemKind.Summon, 10, 2),
			new(ProgressionItemKind.Summon, 11, 0),
			new(ProgressionItemKind.Summon, 11, 1),
			new(ProgressionItemKind.Summon, 11, 2),
			new(ProgressionItemKind.Summon, 12, 0),
			new(ProgressionItemKind.Summon, 12, 1),
			new(ProgressionItemKind.Summon, 12, 2),
			new(ProgressionItemKind.Summon, 13, 0),
			new(ProgressionItemKind.Summon, 13, 1),
			new(ProgressionItemKind.Summon, 13, 2),
			new(ProgressionItemKind.Summon, 14, 0),
			new(ProgressionItemKind.Summon, 14, 1),
			new(ProgressionItemKind.Summon, 14, 2),
			new(ProgressionItemKind.Summon, 15, 0),
			new(ProgressionItemKind.Summon, 15, 1),
			new(ProgressionItemKind.Summon, 15, 2),
			new(ProgressionItemKind.Summon, 16, 0),
			new(ProgressionItemKind.Summon, 16, 1),
			new(ProgressionItemKind.Summon, 16, 2),
			new(ProgressionItemKind.Accessory, 0, 0),
			new(ProgressionItemKind.Accessory, 1, 1),
			new(ProgressionItemKind.Accessory, 2, 2),
			new(ProgressionItemKind.Accessory, 3, 3),
			new(ProgressionItemKind.Accessory, 4, 4),
			new(ProgressionItemKind.Accessory, 5, 0),
			new(ProgressionItemKind.Accessory, 6, 1),
			new(ProgressionItemKind.Accessory, 7, 2),
			new(ProgressionItemKind.Accessory, 8, 3),
			new(ProgressionItemKind.Accessory, 9, 4),
			new(ProgressionItemKind.Accessory, 10, 0),
			new(ProgressionItemKind.Accessory, 11, 1),
			new(ProgressionItemKind.Accessory, 12, 2),
			new(ProgressionItemKind.Accessory, 13, 3),
			new(ProgressionItemKind.Accessory, 14, 4),
			new(ProgressionItemKind.Accessory, 15, 0),
			new(ProgressionItemKind.Accessory, 16, 1),
			new(ProgressionItemKind.Accessory, 7, 0),
			new(ProgressionItemKind.Accessory, 8, 1),
			new(ProgressionItemKind.Accessory, 9, 2),
			new(ProgressionItemKind.Accessory, 10, 3),
			new(ProgressionItemKind.Accessory, 11, 4),
			new(ProgressionItemKind.Accessory, 12, 0),
			new(ProgressionItemKind.Accessory, 13, 1),
			new(ProgressionItemKind.Accessory, 14, 2),
			new(ProgressionItemKind.Tool, 1, 0),
			new(ProgressionItemKind.Tool, 2, 3),
			new(ProgressionItemKind.Tool, 3, 1),
			new(ProgressionItemKind.Tool, 4, 2),
			new(ProgressionItemKind.Tool, 5, 0),
			new(ProgressionItemKind.Tool, 6, 3),
			new(ProgressionItemKind.Tool, 7, 0),
			new(ProgressionItemKind.Tool, 8, 0),
			new(ProgressionItemKind.Tool, 9, 2),
			new(ProgressionItemKind.Tool, 10, 3),
			new(ProgressionItemKind.Tool, 11, 0),
			new(ProgressionItemKind.Tool, 12, 1),
			new(ProgressionItemKind.Tool, 13, 3),
			new(ProgressionItemKind.Tool, 14, 0),
			new(ProgressionItemKind.Tool, 16, 2),
			new(ProgressionItemKind.Consumable, 0, 0),
			new(ProgressionItemKind.Consumable, 1, 1),
			new(ProgressionItemKind.Consumable, 2, 2),
			new(ProgressionItemKind.Consumable, 3, 3),
			new(ProgressionItemKind.Consumable, 4, 4),
			new(ProgressionItemKind.Consumable, 5, 5),
			new(ProgressionItemKind.Consumable, 7, 6),
			new(ProgressionItemKind.Consumable, 10, 7),
			new(ProgressionItemKind.Consumable, 13, 8),
			new(ProgressionItemKind.Consumable, 16, 9),
			new(ProgressionItemKind.Material, 0, 0),
			new(ProgressionItemKind.Material, 1, 1),
			new(ProgressionItemKind.Material, 2, 2),
			new(ProgressionItemKind.Material, 3, 3),
			new(ProgressionItemKind.Material, 4, 4),
			new(ProgressionItemKind.Material, 5, 5),
			new(ProgressionItemKind.Material, 7, 6),
			new(ProgressionItemKind.Material, 8, 7),
			new(ProgressionItemKind.Material, 10, 8),
			new(ProgressionItemKind.Material, 15, 9),
		};

		private static readonly string[] TierNames = {
			"Surface", "King Slime", "Eye of Cthulhu", "Evil Boss", "Queen Bee",
			"Skeletron", "Wall of Flesh", "Mechanical Boss", "All Mechanical Bosses",
			"Plantera", "Golem", "Lunatic Cultist", "Moon Lord", "Monthra",
			"Crystaline Devourer", "Alien Kraken", "Chaotic Endgame"
		};

		private static readonly int[] DamageByTier = {
			9, 15, 24, 34, 46, 62, 86, 125, 180, 265, 390, 620, 1450,
			160000, 12500000, 25000000, 50000000
		};

		private static readonly int[] UseTimeByTier = {
			34, 32, 30, 28, 26, 24, 22, 20, 18, 16, 15, 14, 12, 10, 8, 7, 6
		};

		public static int Count => Specs.Length;
		public static ProgressionItemSpec Get(int index) => Specs[index];
		public static string GetTierName(int tier) => TierNames[Math.Clamp(tier, 0, TierNames.Length - 1)];
		public static int GetDamage(int tier) => DamageByTier[Math.Clamp(tier, 0, DamageByTier.Length - 1)];
		public static int GetUseTime(int tier) => UseTimeByTier[Math.Clamp(tier, 0, UseTimeByTier.Length - 1)];

		public static ChaoticProgressionGate GetGate(int tier) => tier switch {
			0 => ChaoticProgressionGate.Anytime,
			1 => ChaoticProgressionGate.PostKingSlime,
			2 => ChaoticProgressionGate.PostEyeOfCthulhu,
			3 => ChaoticProgressionGate.PostEvilBoss,
			4 => ChaoticProgressionGate.PostQueenBee,
			5 => ChaoticProgressionGate.PostSkeletron,
			6 => ChaoticProgressionGate.PostWallOfFlesh,
			7 => ChaoticProgressionGate.PostAnyMech,
			8 => ChaoticProgressionGate.PostAllMechs,
			9 => ChaoticProgressionGate.PostPlantera,
			10 => ChaoticProgressionGate.PostGolem,
			11 => ChaoticProgressionGate.PostCultist,
			12 => ChaoticProgressionGate.PostMoonLord,
			13 => ChaoticProgressionGate.PostMonthra,
			14 => ChaoticProgressionGate.PostCrystalineDevourer,
			_ => ChaoticProgressionGate.PostKraken
		};

		public static int GetRarity(int tier) => tier switch {
			<= 0 => ItemRarityID.White,
			1 => ItemRarityID.Blue,
			2 => ItemRarityID.Green,
			3 => ItemRarityID.Orange,
			4 => ItemRarityID.LightRed,
			5 => ItemRarityID.Pink,
			6 => ItemRarityID.LightPurple,
			7 => ItemRarityID.Lime,
			8 => ItemRarityID.Yellow,
			9 => ItemRarityID.Cyan,
			10 => ItemRarityID.Red,
			_ => ItemRarityID.Purple
		};

		public static int GetValue(int tier) {
			if (tier >= 13) return Item.buyPrice(platinum: Math.Min(99, 8 + tier * 3));
			return Item.buyPrice(gold: Math.Max(1, 1 + tier * tier));
		}

		public static int GetTextureItemId(ProgressionItemSpec spec) => spec.Kind switch {
			ProgressionItemKind.Melee => spec.Variant switch {
				0 => ItemID.CopperBroadsword,
				1 => ItemID.Spear,
				_ => ItemID.WoodenBoomerang
			},
			ProgressionItemKind.Ranged => spec.Variant switch {
				0 => ItemID.WoodenBow,
				1 => ItemID.Musket,
				_ => ItemID.GrenadeLauncher
			},
			ProgressionItemKind.Magic => spec.Variant switch {
				0 => ItemID.AmethystStaff,
				1 => ItemID.WaterBolt,
				_ => ItemID.DemonScythe
			},
			ProgressionItemKind.Summon => spec.Variant switch {
				0 => ItemID.SlimeStaff,
				1 => ItemID.BlandWhip,
				_ => ItemID.QueenSpiderStaff
			},
			ProgressionItemKind.Accessory => spec.Variant % 5 switch {
				0 => ItemID.WarriorEmblem,
				1 => ItemID.RangerEmblem,
				2 => ItemID.SorcererEmblem,
				3 => ItemID.SummonerEmblem,
				_ => ItemID.CelestialShell
			},
			ProgressionItemKind.Tool => spec.Variant switch {
				0 => ItemID.CopperPickaxe,
				1 => ItemID.CopperAxe,
				2 => ItemID.WoodenHammer,
				_ => ItemID.CobaltDrill
			},
			ProgressionItemKind.Consumable => spec.Variant % 3 switch {
				0 => ItemID.IronskinPotion,
				1 => ItemID.RegenerationPotion,
				_ => ItemID.SwiftnessPotion
			},
			_ => spec.Variant switch {
				0 => ItemID.Wood,
				1 => ItemID.Gel,
				2 => ItemID.DemoniteBar,
				3 => ItemID.BeeWax,
				4 => ItemID.Bone,
				5 => ItemID.HellstoneBar,
				6 => ItemID.HallowedBar,
				7 => ItemID.ChlorophyteBar,
				8 => ItemID.FragmentSolar,
				_ => ItemID.LunarBar
			}
		};

		public static int GetFragment(ProgressionItemKind kind) => kind switch {
			ProgressionItemKind.Melee => ItemID.FragmentSolar,
			ProgressionItemKind.Ranged => ItemID.FragmentVortex,
			ProgressionItemKind.Magic => ItemID.FragmentNebula,
			ProgressionItemKind.Summon => ItemID.FragmentStardust,
			_ => ItemID.FragmentSolar
		};

		public static void AddTierIngredients(Recipe recipe, ProgressionItemSpec spec) {
			int amount = 8 + spec.Tier * 2;
			switch (spec.Tier) {
				case 0: recipe.AddIngredient(ItemID.Wood, amount); break;
				case 1: recipe.AddIngredient(ItemID.IronBar, amount); break;
				case 2: recipe.AddIngredient(ItemID.DemoniteBar, amount); break;
				case 3: recipe.AddIngredient(ItemID.BeeWax, amount); break;
				case 4: recipe.AddIngredient(ItemID.Bone, amount); break;
				case 5: recipe.AddIngredient(ItemID.HellstoneBar, amount); break;
				case 6: recipe.AddIngredient(ItemID.CobaltBar, amount); break;
				case 7: recipe.AddIngredient(ItemID.HallowedBar, amount); break;
				case 8: recipe.AddIngredient(ItemID.ChlorophyteBar, amount); break;
				case 9: recipe.AddIngredient(ItemID.BeetleHusk, Math.Max(4, amount / 3)); break;
				case 10: recipe.AddIngredient(GetFragment(spec.Kind), amount); break;
				case 11: recipe.AddIngredient(ItemID.LunarBar, amount); break;
				case 12: recipe.AddIngredient<MonthraScale>(amount); break;
				case 13: recipe.AddIngredient<CrystalineTear>(amount); break;
				case 14:
					recipe.AddIngredient<CrystalineTear>(amount * 2);
					recipe.AddIngredient(ItemID.LunarBar, amount);
					break;
				case 15: recipe.AddIngredient<Generated.AbyssalKrakenCore>(Math.Max(2, amount / 8)); break;
				default:
					recipe.AddIngredient<Generated.AbyssalKrakenCore>(Math.Max(4, amount / 6));
					recipe.AddIngredient<ChaosCrystal>(12);
					break;
			}
		}

		public static int GetCraftingTile(int tier) => tier switch {
			0 => TileID.WorkBenches,
			<= 5 => TileID.Anvils,
			<= 9 => TileID.MythrilAnvil,
			_ => TileID.LunarCraftingStation
		};
	}
}
