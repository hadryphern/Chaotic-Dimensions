using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ChaoticDimensions.Common.OreSpawn
{
	internal static class OreSpawnItemCatalog
	{
		private static OreSpawnItemDefinition Melee(string key, string displayName, int width, int height, int damage, int useTime, int rarity, int value, float knockBack, int appliedBuffType = 0, int appliedBuffTime = 0, string notes = "") =>
			new(key, displayName, OreSpawnItemKind.Melee, width, height, damage, useTime, useTime, rarity, value, ItemUseStyleID.Swing, knockBack, AppliedBuffType: appliedBuffType, AppliedBuffTime: appliedBuffTime, Notes: notes);

		private static OreSpawnItemDefinition Magic(string key, string displayName, int width, int height, int damage, int useTime, int rarity, int value, float knockBack, int mana, int shoot, float shootSpeed, string notes = "") =>
			new(key, displayName, OreSpawnItemKind.Magic, width, height, damage, useTime, useTime, rarity, value, ItemUseStyleID.Shoot, knockBack, Mana: mana, Shoot: shoot, ShootSpeed: shootSpeed, Notes: notes);

		private static OreSpawnItemDefinition Ranged(string key, string displayName, int width, int height, int damage, int useTime, int rarity, int value, float knockBack, int shoot, float shootSpeed, int useAmmo = 0, string notes = "") =>
			new(key, displayName, OreSpawnItemKind.Ranged, width, height, damage, useTime, useTime, rarity, value, ItemUseStyleID.Shoot, knockBack, Shoot: shoot, ShootSpeed: shootSpeed, UseAmmo: useAmmo, Notes: notes);

		private static OreSpawnItemDefinition Tool(string key, string displayName, int width, int height, int damage, int useTime, int rarity, int value, float knockBack, int hammer = 0, int axe = 0, int pick = 0, string notes = "") =>
			new(key, displayName, OreSpawnItemKind.Tool, width, height, damage, useTime, useTime, rarity, value, ItemUseStyleID.Swing, knockBack, Hammer: hammer, Axe: axe, Pick: pick, Notes: notes);

		public static readonly OreSpawnItemDefinition AttitudeAdjuster = Melee("AttitudeAdjuster", "Attitude Adjuster", 40, 40, 62, 28, ItemRarityID.Orange, Item.buyPrice(gold: 2), 7.5f, notes: "Heavy attitude-check sword.");
		public static readonly OreSpawnItemDefinition BattleAxe = Tool("BattleAxe", "Battle Axe", 42, 42, 68, 30, ItemRarityID.Orange, Item.buyPrice(gold: 2, silver: 20), 8f, axe: 22, notes: "Broad brutal axe inspired by OreSpawn.");
		public static readonly OreSpawnItemDefinition BigBerthaSword = Melee("BigBerthaSword", "Big Bertha Sword", 54, 54, 240, 24, ItemRarityID.Red, Item.buyPrice(platinum: 1), 10f, notes: "Endgame legendary blade.");
		public static readonly OreSpawnItemDefinition BigHammer = Tool("BigHammer", "Big Hammer", 40, 40, 48, 30, ItemRarityID.Orange, Item.buyPrice(gold: 1, silver: 50), 9f, hammer: 80, notes: "Hercules drop.");
		public static readonly OreSpawnItemDefinition Chainsaw = Tool("Chainsaw", "Chainsaw", 44, 44, 74, 18, ItemRarityID.LightRed, Item.buyPrice(gold: 4), 6f, axe: 30, notes: "Loud rush-down melee tool.");
		public static readonly OreSpawnItemDefinition ExperienceSword = Melee("ExperienceSword", "Experience Sword", 40, 40, 78, 24, ItemRarityID.Pink, Item.buyPrice(gold: 6), 7f, notes: "High-tier experience blade.");
		public static readonly OreSpawnItemDefinition FairySword = Melee("FairySword", "Fairy Sword", 34, 34, 46, 22, ItemRarityID.Green, Item.buyPrice(gold: 1), 5.5f, notes: "Crystal dimension fairy blade.");
		public static readonly OreSpawnItemDefinition MantisClaw = Melee("MantisClaw", "Mantis Claw", 34, 34, 56, 18, ItemRarityID.Green, Item.buyPrice(gold: 1), 4.5f, notes: "Fast lifesteal-ish claw, tuned as rapid melee.");
		public static readonly OreSpawnItemDefinition NightmareSword = Melee("NightmareSword", "Nightmare Sword", 46, 46, 94, 24, ItemRarityID.Yellow, Item.buyPrice(gold: 8), 7.5f, notes: "Nightmare trophy blade.");
		public static readonly OreSpawnItemDefinition PoisonSword = Melee("PoisonSword", "Poison Sword", 38, 38, 50, 22, ItemRarityID.Orange, Item.buyPrice(gold: 1, silver: 80), 5.5f, appliedBuffType: BuffID.Poisoned, appliedBuffTime: 300, notes: "Poisons struck enemies.");
		public static readonly OreSpawnItemDefinition QueenScaleBattleAxe = Tool("QueenScaleBattleAxe", "Queen Scale Battle Axe", 50, 50, 118, 26, ItemRarityID.Red, Item.buyPrice(platinum: 1), 9.5f, axe: 35, notes: "Royal queen-tier axe.");
		public static readonly OreSpawnItemDefinition RatSword = Melee("RatSword", "Rat Sword", 32, 32, 24, 16, ItemRarityID.Blue, Item.buyPrice(silver: 80), 3.5f, notes: "Early drop weapon.");
		public static readonly OreSpawnItemDefinition RayGun = Magic("RayGun", "Ray Gun", 38, 38, 64, 18, ItemRarityID.Pink, Item.buyPrice(gold: 6), 4f, 8, ProjectileID.MagicMissile, 14f, notes: "Alien-style magic blaster.");
		public static readonly OreSpawnItemDefinition RoyalGuardianSword = Melee("RoyalGuardianSword", "Royal Guardian Sword", 54, 54, 188, 20, ItemRarityID.Red, Item.buyPrice(platinum: 1), 9f, notes: "Royal Guardian signature sword.");
		public static readonly OreSpawnItemDefinition SkateBowAndIrukandjiArrows = Ranged("SkateBowAndIrukandjiArrows", "Skate Bow & Irukandji Arrows", 42, 42, 72, 22, ItemRarityID.Yellow, Item.buyPrice(gold: 8), 4f, ProjectileID.WoodenArrowFriendly, 12f, AmmoID.Arrow, "High-tier bow family page represented as one bow item.");
		public static readonly OreSpawnItemDefinition SliceSword = Melee("SliceSword", "Slice Sword", 40, 40, 58, 18, ItemRarityID.LightRed, Item.buyPrice(gold: 2, silver: 80), 5f, notes: "Fast slicing sword.");
		public static readonly OreSpawnItemDefinition Squidzooka = Ranged("Squidzooka", "Squidzooka", 44, 44, 86, 30, ItemRarityID.Yellow, Item.buyPrice(gold: 8), 7f, ProjectileID.Grenade, 10f, notes: "Explosive squid launcher beta.");
		public static readonly OreSpawnItemDefinition ThunderStaff = Magic("ThunderStaff", "Thunder Staff", 42, 42, 82, 24, ItemRarityID.Yellow, Item.buyPrice(gold: 9), 5f, 10, ProjectileID.MagicMissile, 15f, notes: "Lightning-theme staff beta.");

		public static IReadOnlyList<OreSpawnItemDefinition> AllItems { get; } = new[] {
			AttitudeAdjuster,
			BattleAxe,
			BigBerthaSword,
			BigHammer,
			Chainsaw,
			ExperienceSword,
			FairySword,
			MantisClaw,
			NightmareSword,
			PoisonSword,
			QueenScaleBattleAxe,
			RatSword,
			RayGun,
			RoyalGuardianSword,
			SkateBowAndIrukandjiArrows,
			SliceSword,
			Squidzooka,
			ThunderStaff
		};

		public static IReadOnlyList<OreSpawnFamilyDefinition> FamilyPages { get; } = new[] {
			new OreSpawnFamilyDefinition("AmethystArmor", "Amethyst Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("AmethystToolsWeapons", "Amethyst Tools and Weapons", "Tools and weapons tier page."),
			new OreSpawnFamilyDefinition("EmeraldArmor", "Emerald Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("EmeraldToolsWeapons", "Emerald Tools and Weapons", "Tools and weapons tier page."),
			new OreSpawnFamilyDefinition("ExperienceArmor", "Experience Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("GirlfriendBoyfriendWeapons", "Girlfriend/Boyfriend Weapons", "Companion drop weapon family."),
			new OreSpawnFamilyDefinition("KyaniteToolsWeapons", "Kyanite Tools and Weapons", "Tools and weapons tier page."),
			new OreSpawnFamilyDefinition("LapisArmor", "Lapis Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("LavaEelArmor", "Lava Eel Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("MobzillaArmor", "Mobzilla Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("MothScaleArmor", "Moth Scale Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("PeacockFeatherArmor", "Peacock Feather Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("PinkTourmalineArmor", "Pink Tourmaline Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("PinkTourmalineToolsWeapons", "Pink Tourmaline Tools and Weapons", "Tools and weapons tier page."),
			new OreSpawnFamilyDefinition("QueenScaleArmor", "Queen Scale Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("RocksAndCrystals", "Rocks and Crystals", "Mineral family page."),
			new OreSpawnFamilyDefinition("RoyalGuardianArmor", "Royal Guardian Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("RubyArmor", "Ruby Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("RubyToolsWeapons", "Ruby Tools and Weapons", "Tools and weapons tier page."),
			new OreSpawnFamilyDefinition("ThrowingWeapons", "Throwing Weapons", "Thrown weapon family page."),
			new OreSpawnFamilyDefinition("TigersEyeArmor", "Tigers Eye Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("TigersEyeToolsWeapons", "Tigers Eye Tools and Weapons", "Tools and weapons tier page."),
			new OreSpawnFamilyDefinition("UltimateArmor", "Ultimate Armor", "Armor tier page."),
			new OreSpawnFamilyDefinition("UltimateToolsWeapons", "Ultimate Tools and Weapons", "Tools and weapons tier page."),
			new OreSpawnFamilyDefinition("WoodCrystalToolsWeapons", "Wood Crystal Tools and Weapons", "Crystal wood tool family page."),
			new OreSpawnFamilyDefinition("AncientDriedEggs", "Ancient Dried Eggs", "Bulk registry page."),
			new OreSpawnFamilyDefinition("Food", "Food", "Bulk registry page."),
			new OreSpawnFamilyDefinition("Materials", "Materials", "Bulk registry page."),
			new OreSpawnFamilyDefinition("PlantsAndTrees", "Plants and Trees", "Bulk registry page.")
		};

		private static readonly Dictionary<string, OreSpawnItemDefinition> ItemMap = new()
		{
			[AttitudeAdjuster.Key] = AttitudeAdjuster,
			[BattleAxe.Key] = BattleAxe,
			[BigBerthaSword.Key] = BigBerthaSword,
			[BigHammer.Key] = BigHammer,
			[Chainsaw.Key] = Chainsaw,
			[ExperienceSword.Key] = ExperienceSword,
			[FairySword.Key] = FairySword,
			[MantisClaw.Key] = MantisClaw,
			[NightmareSword.Key] = NightmareSword,
			[PoisonSword.Key] = PoisonSword,
			[QueenScaleBattleAxe.Key] = QueenScaleBattleAxe,
			[RatSword.Key] = RatSword,
			[RayGun.Key] = RayGun,
			[RoyalGuardianSword.Key] = RoyalGuardianSword,
			[SkateBowAndIrukandjiArrows.Key] = SkateBowAndIrukandjiArrows,
			[SliceSword.Key] = SliceSword,
			[Squidzooka.Key] = Squidzooka,
			[ThunderStaff.Key] = ThunderStaff
		};

		public static OreSpawnItemDefinition GetItem(string key) => ItemMap[key];
	}
}
