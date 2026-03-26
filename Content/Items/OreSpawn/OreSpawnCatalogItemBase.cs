using ChaoticDimensions.Common.OreSpawn;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Items.OreSpawn
{
	public abstract class OreSpawnCatalogItemBase : ModItem
	{
		protected abstract OreSpawnItemDefinition Definition { get; }

		public override void SetDefaults() {
			Item.width = Definition.RecommendedWidth;
			Item.height = Definition.RecommendedHeight;
			Item.damage = Definition.Damage;
			Item.useTime = Definition.UseTime;
			Item.useAnimation = Definition.UseAnimation;
			Item.useStyle = Definition.UseStyle;
			Item.knockBack = Definition.KnockBack;
			Item.rare = Definition.Rarity;
			Item.value = Definition.Value;
			Item.autoReuse = Definition.AutoReuse;
			Item.maxStack = Definition.MaxStack;
			Item.UseSound = Definition.Kind == OreSpawnItemKind.Magic ? SoundID.Item20 : SoundID.Item1;
			Item.hammer = Definition.Hammer;
			Item.axe = Definition.Axe;
			Item.pick = Definition.Pick;
			Item.shoot = Definition.Shoot;
			Item.shootSpeed = Definition.ShootSpeed;
			Item.useAmmo = Definition.UseAmmo;
			Item.mana = Definition.Mana;

			switch (Definition.Kind) {
				case OreSpawnItemKind.Magic:
					Item.DamageType = DamageClass.Magic;
					Item.noMelee = true;
					break;
				case OreSpawnItemKind.Ranged:
					Item.DamageType = DamageClass.Ranged;
					Item.noMelee = true;
					break;
				case OreSpawnItemKind.Tool:
				case OreSpawnItemKind.Melee:
					Item.DamageType = DamageClass.Melee;
					break;
				case OreSpawnItemKind.Material:
				case OreSpawnItemKind.Utility:
					Item.maxStack = 9999;
					Item.damage = 0;
					Item.knockBack = 0f;
					Item.useTime = 10;
					Item.useAnimation = 15;
					Item.useStyle = ItemUseStyleID.Swing;
					Item.consumable = false;
					break;
			}
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			if (Definition.AppliedBuffType > 0 && Definition.AppliedBuffTime > 0) {
				target.AddBuff(Definition.AppliedBuffType, Definition.AppliedBuffTime);
			}
		}
	}
}
