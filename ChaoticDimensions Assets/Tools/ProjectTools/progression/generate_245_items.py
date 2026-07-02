from pathlib import Path
from dataclasses import dataclass

ROOT = Path(__file__).resolve().parents[2]
OUT = ROOT / "Content" / "Items" / "Progression"
GEN = OUT / "Generated"
PROJ = ROOT / "Content" / "Projectiles" / "Progression"
BUFF = ROOT / "Content" / "Buffs"
for folder in (OUT, GEN, PROJ, BUFF):
    folder.mkdir(parents=True, exist_ok=True)

tiers = [
    "Timber", "Gelborn", "Demon", "Hive", "Dungeon", "Infernal", "Cobalt",
    "Hallowed", "Chlorophyte", "Temple", "Lunar", "Celestial", "Monthra",
    "Crystaline", "Krakenbane", "Abyssal", "Chaotic"
]

@dataclass
class Entry:
    name: str
    kind: str
    tier: int
    variant: int

entries = []

for tier, prefix in enumerate(tiers):
    for variant, suffix in enumerate(("Sabre", "Pike", "Chakram")):
        if tier == 0 and variant == 2:
            continue
        entries.append(Entry(prefix + suffix, "Melee", tier, variant))

for kind, suffixes in (
    ("Ranged", ("Longbow", "Carbine", "Launcher")),
    ("Magic", ("Scepter", "Grimoire", "Prism")),
    ("Summon", ("Idol", "Lash", "Beacon")),
):
    for tier, prefix in enumerate(tiers):
        for variant, suffix in enumerate(suffixes):
            if variant == 2 and tier <= 5:
                continue
            entries.append(Entry(prefix + suffix, kind, tier, variant))

accessories = [
    ("TimberCharm", 0, 0), ("GelbornBand", 1, 1), ("DemonEmblem", 2, 2),
    ("HivePendant", 3, 3), ("DungeonCompass", 4, 4), ("InfernalHeart", 5, 0),
    ("CobaltDrive", 6, 1), ("HallowedCrest", 7, 2), ("ChlorophyteBloom", 8, 3),
    ("TempleGear", 9, 4), ("LunarSeal", 10, 0), ("CelestialEngine", 11, 1),
    ("MonthraWingScale", 12, 2), ("CrystalineWard", 13, 3), ("KrakenLens", 14, 4),
    ("AbyssalAnchor", 15, 0), ("ChaoticHeart", 16, 1), ("WarriorSigil", 7, 0),
    ("RangerScope", 8, 1), ("MageSeal", 9, 2), ("SummonerCrest", 10, 3),
    ("MoonlitAegis", 11, 4), ("MonthraFury", 12, 0), ("CrystalRushCore", 13, 1),
    ("KrakenStormWings", 14, 2),
]
entries += [Entry(name, "Accessory", tier, variant) for name, tier, variant in accessories]

tools = [
    ("IronrootPickaxe", 1, 0), ("DemonDrill", 2, 3), ("HiveAxe", 3, 1),
    ("DungeonHammer", 4, 2), ("InfernalPickaxe", 5, 0), ("CobaltExcavator", 6, 3),
    ("HallowedMultitool", 7, 0), ("ChlorophytePickaxe", 8, 0),
    ("TempleBreaker", 9, 2), ("LunarDrill", 10, 3), ("CelestialPickaxe", 11, 0),
    ("MonthraAxe", 12, 1), ("CrystalineExcavator", 13, 3),
    ("KrakenbreakerPickaxe", 14, 0), ("ChaoticWorldshaper", 16, 2),
]
entries += [Entry(name, "Tool", tier, variant) for name, tier, variant in tools]

consumables = [
    ("SurfaceSurvivalTonic", 0, 0), ("GelbornRecoveryPotion", 1, 1),
    ("DemonicBattleDraught", 2, 2), ("HiveReflexElixir", 3, 3),
    ("DungeonSightPotion", 4, 4), ("InfernalGuardFlask", 5, 5),
    ("MechanicalOverdrive", 7, 6), ("LunarFocusSerum", 10, 7),
    ("CrystalineRestoration", 13, 8), ("ChaoticLimitBreak", 16, 9),
]
entries += [Entry(name, "Consumable", tier, variant) for name, tier, variant in consumables]

materials = [
    ("SurfaceFiber", 0, 0), ("GelatinousPearl", 1, 1), ("DemonicAlloy", 2, 2),
    ("HiveResin", 3, 3), ("DungeonRelic", 4, 4), ("MoltenCore", 5, 5),
    ("MechanicalMatrix", 7, 6), ("JungleHeart", 8, 7), ("LunarPrismShard", 10, 8),
    ("AbyssalKrakenCore", 15, 9),
]
entries += [Entry(name, "Material", tier, variant) for name, tier, variant in materials]

if len(entries) != 245:
    raise RuntimeError(f"Expected 245 entries, found {len(entries)}")
if len({entry.name for entry in entries}) != 245:
    from collections import Counter
    print("DUPLICATES", [name for name, count in Counter(entry.name for entry in entries).items() if count > 1])
    raise RuntimeError("Duplicate item names")

catalog_specs = "\n".join(
    f"\t\t\tnew(ProgressionItemKind.{entry.kind}, {entry.tier}, {entry.variant}),"
    for entry in entries
)

catalog = f'''// Centraliza os 245 itens de progressao e evita repetir formulas de balanceamento.

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
{{
\tpublic enum ProgressionItemKind
\t{{
\t\tMelee,
\t\tRanged,
\t\tMagic,
\t\tSummon,
\t\tAccessory,
\t\tTool,
\t\tConsumable,
\t\tMaterial
\t}}

\tpublic readonly record struct ProgressionItemSpec(ProgressionItemKind Kind, int Tier, int Variant);

\tpublic static class ProgressionItemCatalog
\t{{
\t\tprivate static readonly ProgressionItemSpec[] Specs = {{
{catalog_specs}
\t\t}};

\t\tprivate static readonly string[] TierNames = {{
\t\t\t"Surface", "King Slime", "Eye of Cthulhu", "Evil Boss", "Queen Bee",
\t\t\t"Skeletron", "Wall of Flesh", "Mechanical Boss", "All Mechanical Bosses",
\t\t\t"Plantera", "Golem", "Lunatic Cultist", "Moon Lord", "Monthra",
\t\t\t"Crystaline Devourer", "Alien Kraken", "Chaotic Endgame"
\t\t}};

\t\tprivate static readonly int[] DamageByTier = {{
\t\t\t9, 15, 24, 34, 46, 62, 86, 125, 180, 265, 390, 620, 1450,
\t\t\t160000, 12500000, 25000000, 50000000
\t\t}};

\t\tprivate static readonly int[] UseTimeByTier = {{
\t\t\t34, 32, 30, 28, 26, 24, 22, 20, 18, 16, 15, 14, 12, 10, 8, 7, 6
\t\t}};

\t\tpublic static int Count => Specs.Length;
\t\tpublic static ProgressionItemSpec Get(int index) => Specs[index];
\t\tpublic static string GetTierName(int tier) => TierNames[Math.Clamp(tier, 0, TierNames.Length - 1)];
\t\tpublic static int GetDamage(int tier) => DamageByTier[Math.Clamp(tier, 0, DamageByTier.Length - 1)];
\t\tpublic static int GetUseTime(int tier) => UseTimeByTier[Math.Clamp(tier, 0, UseTimeByTier.Length - 1)];

\t\tpublic static ChaoticProgressionGate GetGate(int tier) => tier switch {{
\t\t\t0 => ChaoticProgressionGate.Anytime,
\t\t\t1 => ChaoticProgressionGate.PostKingSlime,
\t\t\t2 => ChaoticProgressionGate.PostEyeOfCthulhu,
\t\t\t3 => ChaoticProgressionGate.PostEvilBoss,
\t\t\t4 => ChaoticProgressionGate.PostQueenBee,
\t\t\t5 => ChaoticProgressionGate.PostSkeletron,
\t\t\t6 => ChaoticProgressionGate.PostWallOfFlesh,
\t\t\t7 => ChaoticProgressionGate.PostAnyMech,
\t\t\t8 => ChaoticProgressionGate.PostAllMechs,
\t\t\t9 => ChaoticProgressionGate.PostPlantera,
\t\t\t10 => ChaoticProgressionGate.PostGolem,
\t\t\t11 => ChaoticProgressionGate.PostCultist,
\t\t\t12 => ChaoticProgressionGate.PostMoonLord,
\t\t\t13 => ChaoticProgressionGate.PostMonthra,
\t\t\t14 => ChaoticProgressionGate.PostCrystalineDevourer,
\t\t\t_ => ChaoticProgressionGate.PostKraken
\t\t}};

\t\tpublic static int GetRarity(int tier) => tier switch {{
\t\t\t<= 0 => ItemRarityID.White,
\t\t\t1 => ItemRarityID.Blue,
\t\t\t2 => ItemRarityID.Green,
\t\t\t3 => ItemRarityID.Orange,
\t\t\t4 => ItemRarityID.LightRed,
\t\t\t5 => ItemRarityID.Pink,
\t\t\t6 => ItemRarityID.LightPurple,
\t\t\t7 => ItemRarityID.Lime,
\t\t\t8 => ItemRarityID.Yellow,
\t\t\t9 => ItemRarityID.Cyan,
\t\t\t10 => ItemRarityID.Red,
\t\t\t_ => ItemRarityID.Purple
\t\t}};

\t\tpublic static int GetValue(int tier) {{
\t\t\tif (tier >= 13) return Item.buyPrice(platinum: Math.Min(99, 8 + tier * 3));
\t\t\treturn Item.buyPrice(gold: Math.Max(1, 1 + tier * tier));
\t\t}}

\t\tpublic static int GetTextureItemId(ProgressionItemSpec spec) => spec.Kind switch {{
\t\t\tProgressionItemKind.Melee => spec.Variant switch {{
\t\t\t\t0 => ItemID.CopperBroadsword,
\t\t\t\t1 => ItemID.Spear,
\t\t\t\t_ => ItemID.WoodenBoomerang
\t\t\t}},
\t\t\tProgressionItemKind.Ranged => spec.Variant switch {{
\t\t\t\t0 => ItemID.WoodenBow,
\t\t\t\t1 => ItemID.Musket,
\t\t\t\t_ => ItemID.GrenadeLauncher
\t\t\t}},
\t\t\tProgressionItemKind.Magic => spec.Variant switch {{
\t\t\t\t0 => ItemID.AmethystStaff,
\t\t\t\t1 => ItemID.WaterBolt,
\t\t\t\t_ => ItemID.DemonScythe
\t\t\t}},
\t\t\tProgressionItemKind.Summon => spec.Variant switch {{
\t\t\t\t0 => ItemID.SlimeStaff,
\t\t\t\t1 => ItemID.BlandWhip,
\t\t\t\t_ => ItemID.QueenSpiderStaff
\t\t\t}},
\t\t\tProgressionItemKind.Accessory => spec.Variant % 5 switch {{
\t\t\t\t0 => ItemID.WarriorEmblem,
\t\t\t\t1 => ItemID.RangerEmblem,
\t\t\t\t2 => ItemID.SorcererEmblem,
\t\t\t\t3 => ItemID.SummonerEmblem,
\t\t\t\t_ => ItemID.CelestialShell
\t\t\t}},
\t\t\tProgressionItemKind.Tool => spec.Variant switch {{
\t\t\t\t0 => ItemID.CopperPickaxe,
\t\t\t\t1 => ItemID.CopperAxe,
\t\t\t\t2 => ItemID.WoodenHammer,
\t\t\t\t_ => ItemID.CobaltDrill
\t\t\t}},
\t\t\tProgressionItemKind.Consumable => spec.Variant % 3 switch {{
\t\t\t\t0 => ItemID.IronskinPotion,
\t\t\t\t1 => ItemID.RegenerationPotion,
\t\t\t\t_ => ItemID.SwiftnessPotion
\t\t\t}},
\t\t\t_ => spec.Variant switch {{
\t\t\t\t0 => ItemID.Wood,
\t\t\t\t1 => ItemID.Gel,
\t\t\t\t2 => ItemID.DemoniteBar,
\t\t\t\t3 => ItemID.BeeWax,
\t\t\t\t4 => ItemID.Bone,
\t\t\t\t5 => ItemID.HellstoneBar,
\t\t\t\t6 => ItemID.HallowedBar,
\t\t\t\t7 => ItemID.ChlorophyteBar,
\t\t\t\t8 => ItemID.FragmentSolar,
\t\t\t\t_ => ItemID.LunarBar
\t\t\t}}
\t\t}};

\t\tpublic static int GetFragment(ProgressionItemKind kind) => kind switch {{
\t\t\tProgressionItemKind.Melee => ItemID.FragmentSolar,
\t\t\tProgressionItemKind.Ranged => ItemID.FragmentVortex,
\t\t\tProgressionItemKind.Magic => ItemID.FragmentNebula,
\t\t\tProgressionItemKind.Summon => ItemID.FragmentStardust,
\t\t\t_ => ItemID.FragmentSolar
\t\t}};

\t\tpublic static void AddTierIngredients(Recipe recipe, ProgressionItemSpec spec) {{
\t\t\tint amount = 8 + spec.Tier * 2;
\t\t\tswitch (spec.Tier) {{
\t\t\t\tcase 0: recipe.AddIngredient(ItemID.Wood, amount); break;
\t\t\t\tcase 1: recipe.AddIngredient(ItemID.IronBar, amount); break;
\t\t\t\tcase 2: recipe.AddIngredient(ItemID.DemoniteBar, amount); break;
\t\t\t\tcase 3: recipe.AddIngredient(ItemID.BeeWax, amount); break;
\t\t\t\tcase 4: recipe.AddIngredient(ItemID.Bone, amount); break;
\t\t\t\tcase 5: recipe.AddIngredient(ItemID.HellstoneBar, amount); break;
\t\t\t\tcase 6: recipe.AddIngredient(ItemID.CobaltBar, amount); break;
\t\t\t\tcase 7: recipe.AddIngredient(ItemID.HallowedBar, amount); break;
\t\t\t\tcase 8: recipe.AddIngredient(ItemID.ChlorophyteBar, amount); break;
\t\t\t\tcase 9: recipe.AddIngredient(ItemID.BeetleHusk, Math.Max(4, amount / 3)); break;
\t\t\t\tcase 10: recipe.AddIngredient(GetFragment(spec.Kind), amount); break;
\t\t\t\tcase 11: recipe.AddIngredient(ItemID.LunarBar, amount); break;
\t\t\t\tcase 12: recipe.AddIngredient<MonthraScale>(amount); break;
\t\t\t\tcase 13: recipe.AddIngredient<CrystalineTear>(amount); break;
\t\t\t\tcase 14:
\t\t\t\t\trecipe.AddIngredient<CrystalineTear>(amount * 2);
\t\t\t\t\trecipe.AddIngredient(ItemID.LunarBar, amount);
\t\t\t\t\tbreak;
\t\t\t\tcase 15: recipe.AddIngredient<Generated.AbyssalKrakenCore>(Math.Max(2, amount / 8)); break;
\t\t\t\tdefault:
\t\t\t\t\trecipe.AddIngredient<Generated.AbyssalKrakenCore>(Math.Max(4, amount / 6));
\t\t\t\t\trecipe.AddIngredient<ChaosCrystal>(12);
\t\t\t\t\tbreak;
\t\t\t}}
\t\t}}

\t\tpublic static int GetCraftingTile(int tier) => tier switch {{
\t\t\t0 => TileID.WorkBenches,
\t\t\t<= 5 => TileID.Anvils,
\t\t\t<= 9 => TileID.MythrilAnvil,
\t\t\t_ => TileID.LunarCraftingStation
\t\t}};
\t}}
}}
'''

bases = '''// Implementa o comportamento comum das oito familias de itens de progressao.

using ChaoticDimensions.Common.Progression;
using ChaoticDimensions.Content.Buffs;
using ChaoticDimensions.Content.Projectiles.Progression;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.Progression
{
\tpublic abstract class ProgressionCatalogItem : ModItem
\t{
\t\tprotected abstract int CatalogIndex { get; }
\t\tprotected ProgressionItemSpec Spec => ProgressionItemCatalog.Get(CatalogIndex);
\t\tpublic override string Texture => $"Terraria/Images/Item_{ProgressionItemCatalog.GetTextureItemId(Spec)}";

\t\tprotected void ApplyCommonDefaults() {
\t\t\tItem.width = 32;
\t\t\tItem.height = 32;
\t\t\tItem.rare = ProgressionItemCatalog.GetRarity(Spec.Tier);
\t\t\tItem.value = ProgressionItemCatalog.GetValue(Spec.Tier);
\t\t}

\t\tpublic override bool CanUseItem(Player player) {
\t\t\treturn ChaoticProgressionHelper.IsMet(ProgressionItemCatalog.GetGate(Spec.Tier));
\t\t}

\t\tpublic override void ModifyTooltips(List<TooltipLine> tooltips) {
\t\t\tstring gate = ChaoticProgressionHelper.GetDebugLabel(ProgressionItemCatalog.GetGate(Spec.Tier));
\t\t\ttooltips.Add(new TooltipLine(Mod, "ProgressionTier", $"Progressão: {ProgressionItemCatalog.GetTierName(Spec.Tier)} ({gate})") {
\t\t\t\tOverrideColor = new Color(110, 190, 255)
\t\t\t});
\t\t\ttooltips.Add(new TooltipLine(Mod, "PlaceholderArt", "Arte vanilla temporária; código e progressão já funcionais") {
\t\t\t\tOverrideColor = new Color(150, 150, 160)
\t\t\t});
\t\t\tif (ProgressionItemCatalog.GetDamage(Spec.Tier) >= 10000000 && Spec.Kind is ProgressionItemKind.Melee or ProgressionItemKind.Ranged or ProgressionItemKind.Magic or ProgressionItemKind.Summon) {
\t\t\t\ttooltips.Add(new TooltipLine(Mod, "KrakenBreaker", "Poder suficiente para derrotar o Alien Kraken num único golpe") {
\t\t\t\t\tOverrideColor = new Color(255, 80, 120)
\t\t\t\t});
\t\t\t}
\t\t}

\t\tpublic override void AddRecipes() {
\t\t\tif (Spec.Kind == ProgressionItemKind.Material && Spec.Variant == 9) return;
\t\t\tRecipe recipe = CreateRecipe();
\t\t\tProgressionItemCatalog.AddTierIngredients(recipe, Spec);
\t\t\trecipe.AddTile(ProgressionItemCatalog.GetCraftingTile(Spec.Tier));
\t\t\trecipe.Register();
\t\t}
\t}

\tpublic abstract class ProgressionMeleeItem : ProgressionCatalogItem
\t{
\t\tpublic override void SetDefaults() {
\t\t\tApplyCommonDefaults();
\t\t\tItem.DamageType = DamageClass.Melee;
\t\t\tItem.damage = ProgressionItemCatalog.GetDamage(Spec.Tier);
\t\t\tItem.knockBack = 4f + Spec.Tier * 0.28f;
\t\t\tItem.useTime = ProgressionItemCatalog.GetUseTime(Spec.Tier);
\t\t\tItem.useAnimation = Item.useTime;
\t\t\tItem.autoReuse = true;
\t\t\tItem.UseSound = SoundID.Item1;
\t\t\tif (Spec.Variant == 0) {
\t\t\t\tItem.useStyle = ItemUseStyleID.Swing;
\t\t\t}
\t\t\telse {
\t\t\t\tItem.useStyle = ItemUseStyleID.Shoot;
\t\t\t\tItem.noMelee = true;
\t\t\t\tItem.noUseGraphic = true;
\t\t\t\tItem.shoot = ModContent.ProjectileType<ProgressionWeaponProjectile>();
\t\t\t\tItem.shootSpeed = 10f + Spec.Tier * 0.65f;
\t\t\t}
\t\t}

\t\tpublic override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
\t\t\tif (Spec.Variant == 0) return false;
\t\t\tProjectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Spec.Variant, Spec.Tier);
\t\t\treturn false;
\t\t}
\t}

\tpublic abstract class ProgressionRangedItem : ProgressionCatalogItem
\t{
\t\tpublic override void SetDefaults() {
\t\t\tApplyCommonDefaults();
\t\t\tItem.DamageType = DamageClass.Ranged;
\t\t\tItem.damage = ProgressionItemCatalog.GetDamage(Spec.Tier);
\t\t\tItem.knockBack = 2.5f + Spec.Tier * 0.2f;
\t\t\tItem.useTime = ProgressionItemCatalog.GetUseTime(Spec.Tier);
\t\t\tItem.useAnimation = Item.useTime;
\t\t\tItem.useStyle = ItemUseStyleID.Shoot;
\t\t\tItem.noMelee = true;
\t\t\tItem.autoReuse = true;
\t\t\tItem.UseSound = Spec.Variant == 0 ? SoundID.Item5 : Spec.Variant == 1 ? SoundID.Item11 : SoundID.Item61;
\t\t\tItem.useAmmo = Spec.Variant == 0 ? AmmoID.Arrow : Spec.Variant == 1 ? AmmoID.Bullet : AmmoID.Rocket;
\t\t\tItem.shoot = Spec.Variant == 0 ? ProjectileID.WoodenArrowFriendly : Spec.Variant == 1 ? ProjectileID.Bullet : ProjectileID.Grenade;
\t\t\tItem.shootSpeed = 8f + Spec.Tier * 0.9f;
\t\t}
\t}

\tpublic abstract class ProgressionMagicItem : ProgressionCatalogItem
\t{
\t\tpublic override void SetDefaults() {
\t\t\tApplyCommonDefaults();
\t\t\tItem.DamageType = DamageClass.Magic;
\t\t\tItem.damage = ProgressionItemCatalog.GetDamage(Spec.Tier);
\t\t\tItem.knockBack = 3f + Spec.Tier * 0.2f;
\t\t\tItem.mana = System.Math.Max(2, 5 + Spec.Tier);
\t\t\tItem.useTime = ProgressionItemCatalog.GetUseTime(Spec.Tier);
\t\t\tItem.useAnimation = Item.useTime;
\t\t\tItem.useStyle = ItemUseStyleID.Shoot;
\t\t\tItem.noMelee = true;
\t\t\tItem.autoReuse = true;
\t\t\tItem.UseSound = SoundID.Item20;
\t\t\tItem.shoot = ModContent.ProjectileType<ProgressionWeaponProjectile>();
\t\t\tItem.shootSpeed = 9f + Spec.Tier * 0.7f;
\t\t}

\t\tpublic override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
\t\t\tProjectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 10 + Spec.Variant, Spec.Tier);
\t\t\treturn false;
\t\t}
\t}

\tpublic abstract class ProgressionSummonItem : ProgressionCatalogItem
\t{
\t\tpublic override void SetDefaults() {
\t\t\tApplyCommonDefaults();
\t\t\tint damage = ProgressionItemCatalog.GetDamage(Spec.Tier);
\t\t\tint useTime = ProgressionItemCatalog.GetUseTime(Spec.Tier);
\t\t\tif (Spec.Variant == 1) {
\t\t\t\tItem.DefaultToWhip(ModContent.ProjectileType<ProgressionWhipProjectile>(), damage, 2f + Spec.Tier * 0.18f, 4f, useTime);
\t\t\t\tItem.rare = ProgressionItemCatalog.GetRarity(Spec.Tier);
\t\t\t\tItem.value = ProgressionItemCatalog.GetValue(Spec.Tier);
\t\t\t\treturn;
\t\t\t}
\t\t\tItem.DamageType = DamageClass.Summon;
\t\t\tItem.damage = damage;
\t\t\tItem.knockBack = 2f + Spec.Tier * 0.16f;
\t\t\tItem.mana = System.Math.Max(3, 6 + Spec.Tier);
\t\t\tItem.useTime = useTime + 8;
\t\t\tItem.useAnimation = Item.useTime;
\t\t\tItem.useStyle = ItemUseStyleID.Swing;
\t\t\tItem.noMelee = true;
\t\t\tItem.UseSound = SoundID.Item44;
\t\t\tItem.buffType = ModContent.BuffType<ProgressionMinionBuff>();
\t\t\tItem.shoot = ModContent.ProjectileType<ProgressionMinionProjectile>();
\t\t\tItem.shootSpeed = 10f;
\t\t}

\t\tpublic override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
\t\t\tif (Spec.Variant == 1) return true;
\t\t\tplayer.AddBuff(Item.buffType, 2);
\t\t\tProjectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI, Spec.Variant, Spec.Tier);
\t\t\treturn false;
\t\t}
\t}

\tpublic abstract class ProgressionAccessoryItem : ProgressionCatalogItem
\t{
\t\tpublic override void SetDefaults() {
\t\t\tApplyCommonDefaults();
\t\t\tItem.accessory = true;
\t\t\tItem.defense = Spec.Tier * 3;
\t\t}

\t\tpublic override void UpdateAccessory(Player player, bool hideVisual) {
\t\t\tfloat power = 0.02f + Spec.Tier * 0.015f;
\t\t\tswitch (Spec.Variant % 5) {
\t\t\t\tcase 0:
\t\t\t\t\tplayer.GetDamage(DamageClass.Melee) += power;
\t\t\t\t\tplayer.GetAttackSpeed(DamageClass.Melee) += power * 0.6f;
\t\t\t\t\tbreak;
\t\t\t\tcase 1:
\t\t\t\t\tplayer.GetDamage(DamageClass.Ranged) += power;
\t\t\t\t\tplayer.GetCritChance(DamageClass.Ranged) += Spec.Tier * 0.8f;
\t\t\t\t\tbreak;
\t\t\t\tcase 2:
\t\t\t\t\tplayer.GetDamage(DamageClass.Magic) += power;
\t\t\t\t\tplayer.statManaMax2 += 10 + Spec.Tier * 8;
\t\t\t\t\tbreak;
\t\t\t\tcase 3:
\t\t\t\t\tplayer.GetDamage(DamageClass.Summon) += power;
\t\t\t\t\tplayer.maxMinions += 1 + Spec.Tier / 5;
\t\t\t\t\tbreak;
\t\t\t\tdefault:
\t\t\t\t\tplayer.moveSpeed += power;
\t\t\t\t\tplayer.endurance += System.Math.Min(0.22f, power * 0.45f);
\t\t\t\t\tplayer.noKnockback |= Spec.Tier >= 10;
\t\t\t\t\tbreak;
\t\t\t}
\t\t}
\t}

\tpublic abstract class ProgressionToolItem : ProgressionCatalogItem
\t{
\t\tpublic override void SetDefaults() {
\t\t\tApplyCommonDefaults();
\t\t\tItem.DamageType = DamageClass.Melee;
\t\t\tItem.damage = System.Math.Max(5, ProgressionItemCatalog.GetDamage(Spec.Tier) / 2);
\t\t\tItem.knockBack = 3f;
\t\t\tItem.useStyle = ItemUseStyleID.Swing;
\t\t\tItem.useTime = System.Math.Max(2, ProgressionItemCatalog.GetUseTime(Spec.Tier) / 2);
\t\t\tItem.useAnimation = Item.useTime * 2;
\t\t\tItem.useTurn = true;
\t\t\tItem.autoReuse = true;
\t\t\tItem.UseSound = SoundID.Item1;
\t\t\tint power = System.Math.Min(1000, 35 + Spec.Tier * 32);
\t\t\tif (Spec.Variant == 1) Item.axe = power / 5;
\t\t\telse if (Spec.Variant == 2) Item.hammer = power;
\t\t\telse Item.pick = power;
\t\t}
\t}

\tpublic abstract class ProgressionConsumableItem : ProgressionCatalogItem
\t{
\t\tpublic override void SetDefaults() {
\t\t\tApplyCommonDefaults();
\t\t\tItem.maxStack = Item.CommonMaxStack;
\t\t\tItem.consumable = true;
\t\t\tItem.useStyle = ItemUseStyleID.DrinkLiquid;
\t\t\tItem.useTime = 20;
\t\t\tItem.useAnimation = 20;
\t\t\tItem.UseSound = SoundID.Item3;
\t\t\tItem.healLife = 20 + Spec.Tier * 18;
\t\t\tItem.healMana = 10 + Spec.Tier * 12;
\t\t}

\t\tpublic override bool? UseItem(Player player) {
\t\t\tint duration = 60 * (45 + Spec.Tier * 5);
\t\t\tint buff = Spec.Variant % 5 switch {
\t\t\t\t0 => BuffID.Ironskin,
\t\t\t\t1 => BuffID.Regeneration,
\t\t\t\t2 => BuffID.Wrath,
\t\t\t\t3 => BuffID.Swiftness,
\t\t\t\t_ => BuffID.Endurance
\t\t\t};
\t\t\tplayer.AddBuff(buff, duration);
\t\t\treturn true;
\t\t}
\t}

\tpublic abstract class ProgressionMaterialItem : ProgressionCatalogItem
\t{
\t\tpublic override void SetDefaults() {
\t\t\tApplyCommonDefaults();
\t\t\tItem.maxStack = Item.CommonMaxStack;
\t\t}
\t}
}
'''

projectile = '''// Partilha a logica dos ataques de melee, magic e dos tiros dos minions.

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Progression
{
\tpublic sealed class ProgressionWeaponProjectile : ModProjectile
\t{
\t\tprivate int Mode => (int)Projectile.ai[0];
\t\tprivate int Tier => (int)Projectile.ai[1];

\t\tpublic override string Texture => $"Terraria/Images/Projectile_{ProjectileID.AmethystBolt}";

\t\tpublic override void SetDefaults() {
\t\t\tProjectile.width = 18;
\t\t\tProjectile.height = 18;
\t\t\tProjectile.friendly = true;
\t\t\tProjectile.penetrate = 3;
\t\t\tProjectile.timeLeft = 240;
\t\t\tProjectile.ignoreWater = true;
\t\t\tProjectile.tileCollide = true;
\t\t}

\t\tpublic override void AI() {
\t\t\tProjectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
\t\t\tif (Mode < 10) {
\t\t\t\tProjectile.DamageType = DamageClass.Melee;
\t\t\t\tif (Mode == 1) Projectile.velocity *= 1.012f;
\t\t\t\telse if (Projectile.timeLeft < 205) ReturnToOwner();
\t\t\t}
\t\t\telse if (Mode < 20) {
\t\t\t\tProjectile.DamageType = DamageClass.Magic;
\t\t\t\tif (Mode == 11) HomeTowardsTarget(0.065f, 12f + Tier * 0.8f);
\t\t\t\telse if (Mode == 12) Projectile.velocity = Projectile.velocity.RotatedBy(0.018f);
\t\t\t}
\t\t\telse {
\t\t\t\tProjectile.DamageType = DamageClass.Summon;
\t\t\t\tHomeTowardsTarget(0.08f, 10f + Tier * 0.7f);
\t\t\t}
\t\t\tLighting.AddLight(Projectile.Center, 0.08f, 0.12f, 0.2f);
\t\t}

\t\tprivate void ReturnToOwner() {
\t\t\tPlayer owner = Main.player[Projectile.owner];
\t\t\tVector2 desired = (owner.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * (13f + Tier);
\t\t\tProjectile.velocity = Vector2.Lerp(Projectile.velocity, desired, 0.12f);
\t\t\tProjectile.tileCollide = false;
\t\t\tif (Vector2.DistanceSquared(Projectile.Center, owner.Center) < 28f * 28f) Projectile.Kill();
\t\t}

\t\tprivate void HomeTowardsTarget(float turnRate, float speed) {
\t\t\tNPC target = null;
\t\t\tfloat distance = 900f;
\t\t\tfor (int i = 0; i < Main.maxNPCs; i++) {
\t\t\t\tNPC npc = Main.npc[i];
\t\t\t\tif (!npc.CanBeChasedBy(Projectile)) continue;
\t\t\t\tfloat current = Vector2.Distance(Projectile.Center, npc.Center);
\t\t\t\tif (current < distance) {
\t\t\t\t\tdistance = current;
\t\t\t\t\ttarget = npc;
\t\t\t\t}
\t\t\t}
\t\t\tif (target is null) return;
\t\t\tVector2 desired = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * speed;
\t\t\tProjectile.velocity = Vector2.Lerp(Projectile.velocity, desired, turnRate);
\t\t}
\t}
}
'''

minion = '''// Implementa tres comportamentos de summon sem exigir sprites definitivas.

using ChaoticDimensions.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Progression
{
\tpublic sealed class ProgressionMinionProjectile : ModProjectile
\t{
\t\tprivate int Mode => (int)Projectile.ai[0];
\t\tprivate int Tier => (int)Projectile.ai[1];

\t\tpublic override string Texture => $"Terraria/Images/Projectile_{ProjectileID.BabySlime}";

\t\tpublic override void SetStaticDefaults() {
\t\t\tMain.projPet[Type] = true;
\t\t\tProjectileID.Sets.MinionSacrificable[Type] = true;
\t\t\tProjectileID.Sets.MinionTargettingFeature[Type] = true;
\t\t}

\t\tpublic override void SetDefaults() {
\t\t\tProjectile.width = 30;
\t\t\tProjectile.height = 24;
\t\t\tProjectile.friendly = true;
\t\t\tProjectile.minion = true;
\t\t\tProjectile.minionSlots = 1f;
\t\t\tProjectile.penetrate = -1;
\t\t\tProjectile.tileCollide = false;
\t\t\tProjectile.ignoreWater = true;
\t\t\tProjectile.DamageType = DamageClass.Summon;
\t\t\tProjectile.usesLocalNPCImmunity = true;
\t\t\tProjectile.localNPCHitCooldown = 18;
\t\t}

\t\tpublic override bool MinionContactDamage() => Mode != 2;

\t\tpublic override void AI() {
\t\t\tPlayer player = Main.player[Projectile.owner];
\t\t\tif (!player.active || player.dead) {
\t\t\t\tplayer.ClearBuff(ModContent.BuffType<ProgressionMinionBuff>());
\t\t\t\treturn;
\t\t\t}
\t\t\tif (player.HasBuff(ModContent.BuffType<ProgressionMinionBuff>())) Projectile.timeLeft = 2;

\t\t\tNPC target = FindTarget(player);
\t\t\tVector2 idle = player.Center + new Vector2((Projectile.minionPos + 1) * -48f * player.direction, -70f - Mode * 18f);
\t\t\tif (target is null) {
\t\t\t\tMoveTowards(idle, 8f + Tier * 0.35f, 0.09f);
\t\t\t}
\t\t\telse if (Mode == 0) {
\t\t\t\tVector2 orbit = target.Center + new Vector2(110f + Tier * 2f, 0f).RotatedBy(Main.GameUpdateCount * 0.04f + Projectile.identity);
\t\t\t\tMoveTowards(orbit, 11f + Tier * 0.45f, 0.12f);
\t\t\t}
\t\t\telse if (Mode == 2) {
\t\t\t\tVector2 hover = target.Center + new Vector2(0f, -180f);
\t\t\t\tMoveTowards(hover, 10f + Tier * 0.35f, 0.1f);
\t\t\t\tProjectile.localAI[0]++;
\t\t\t\tif (Projectile.localAI[0] >= System.Math.Max(12, 48 - Tier * 2) && Main.myPlayer == Projectile.owner) {
\t\t\t\t\tProjectile.localAI[0] = 0f;
\t\t\t\t\tVector2 velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * (10f + Tier * 0.5f);
\t\t\t\t\tProjectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<ProgressionWeaponProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 20f, Tier);
\t\t\t\t}
\t\t\t}
\t\t\telse {
\t\t\t\tMoveTowards(target.Center, 15f + Tier * 0.55f, 0.15f);
\t\t\t}
\t\t\tProjectile.rotation = Projectile.velocity.X * 0.04f;
\t\t}

\t\tprivate void MoveTowards(Vector2 target, float speed, float turn) {
\t\t\tVector2 desired = (target - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;
\t\t\tProjectile.velocity = Vector2.Lerp(Projectile.velocity, desired, turn);
\t\t\tif (Vector2.DistanceSquared(Projectile.Center, target) > 1500f * 1500f) Projectile.Center = target;
\t\t}

\t\tprivate NPC FindTarget(Player player) {
\t\t\tif (player.HasMinionAttackTargetNPC) {
\t\t\t\tNPC focus = Main.npc[player.MinionAttackTargetNPC];
\t\t\t\tif (focus.CanBeChasedBy(Projectile)) return focus;
\t\t\t}
\t\t\tNPC target = null;
\t\t\tfloat distance = 800f + Tier * 25f;
\t\t\tfor (int i = 0; i < Main.maxNPCs; i++) {
\t\t\t\tNPC npc = Main.npc[i];
\t\t\t\tif (!npc.CanBeChasedBy(Projectile)) continue;
\t\t\t\tfloat current = Vector2.Distance(Projectile.Center, npc.Center);
\t\t\t\tif (current < distance) {
\t\t\t\t\tdistance = current;
\t\t\t\t\ttarget = npc;
\t\t\t\t}
\t\t\t}
\t\t\treturn target;
\t\t}
\t}
}
'''

whip = '''// Fornece um whip funcional comum aos quinze tiers de lashes.

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Projectiles.Progression
{
\tpublic sealed class ProgressionWhipProjectile : ModProjectile
\t{
\t\tpublic override string Texture => $"Terraria/Images/Projectile_{ProjectileID.BlandWhip}";

\t\tpublic override void SetStaticDefaults() {
\t\t\tProjectileID.Sets.IsAWhip[Type] = true;
\t\t}

\t\tpublic override void SetDefaults() {
\t\t\tProjectile.DefaultToWhip();
\t\t\tProjectile.WhipSettings.Segments = 24;
\t\t\tProjectile.WhipSettings.RangeMultiplier = 1.35f;
\t\t}

\t\tpublic override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
\t\t\tMain.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
\t\t\tProjectile.damage = (int)(Projectile.damage * 0.75f);
\t\t}
\t}
}
'''

buff = '''// Mantem os summons do catalogo ativos enquanto o jogador tiver pelo menos um minion.

using ChaoticDimensions.Content.Projectiles.Progression;
using Terraria;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
\tpublic sealed class ProgressionMinionBuff : ModBuff
\t{
\t\tpublic override string Texture => "Terraria/Images/Buff_30";

\t\tpublic override void SetStaticDefaults() {
\t\t\tMain.buffNoSave[Type] = true;
\t\t\tMain.buffNoTimeDisplay[Type] = true;
\t\t}

\t\tpublic override void Update(Player player, ref int buffIndex) {
\t\t\tif (player.ownedProjectileCounts[ModContent.ProjectileType<ProgressionMinionProjectile>()] > 0) {
\t\t\t\tplayer.buffTime[buffIndex] = 18000;
\t\t\t}
\t\t\telse {
\t\t\t\tplayer.DelBuff(buffIndex);
\t\t\t\tbuffIndex--;
\t\t\t}
\t\t}
\t}
}
'''

def generated_file(kind, base_type, subset):
    body = [
        f"// Declara os itens {kind.lower()} do catalogo; o comportamento fica concentrado na classe base.",
        "",
        "namespace ChaoticDimensions.Content.Items.Progression.Generated",
        "{",
    ]
    for index, entry in subset:
        body += [
            f"\tpublic sealed class {entry.name} : {base_type}",
            "\t{",
            f"\t\tprotected override int CatalogIndex => {index};",
            "\t}",
            "",
        ]
    body += ["}", ""]
    return "\n".join(body)

(OUT / "ProgressionItemCatalog.cs").write_text(catalog, encoding="utf-8")
(OUT / "ProgressionItemBases.cs").write_text(bases, encoding="utf-8")
(PROJ / "ProgressionWeaponProjectile.cs").write_text(projectile, encoding="utf-8")
(PROJ / "ProgressionMinionProjectile.cs").write_text(minion, encoding="utf-8")
(PROJ / "ProgressionWhipProjectile.cs").write_text(whip, encoding="utf-8")
(BUFF / "ProgressionMinionBuff.cs").write_text(buff, encoding="utf-8")

base_types = {
    "Melee": "ProgressionMeleeItem",
    "Ranged": "ProgressionRangedItem",
    "Magic": "ProgressionMagicItem",
    "Summon": "ProgressionSummonItem",
    "Accessory": "ProgressionAccessoryItem",
    "Tool": "ProgressionToolItem",
    "Consumable": "ProgressionConsumableItem",
    "Material": "ProgressionMaterialItem",
}
for kind, base_type in base_types.items():
    subset = [(index, entry) for index, entry in enumerate(entries) if entry.kind == kind]
    (GEN / f"{kind}Items.cs").write_text(generated_file(kind, base_type, subset), encoding="utf-8")

manifest = ["index\tname\tkind\ttier\tvariant"]
manifest += [f"{i}\t{e.name}\t{e.kind}\t{e.tier}\t{e.variant}" for i, e in enumerate(entries)]
(OUT / "ProgressionItems.tsv").write_text("\n".join(manifest) + "\n", encoding="utf-8")
print("Generated", len(entries), "items")
for kind in base_types:
    print(kind, sum(1 for e in entries if e.kind == kind))
