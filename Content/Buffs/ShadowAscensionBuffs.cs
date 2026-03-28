using ChaoticDimensions.Content.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public static class ShadowCombatHelper
	{
		public static void ApplyRend(NPC target, int owner, int duration = 60 * 30, int healAmount = 10) {
			target.AddBuff(ModContent.BuffType<ShadowRendDebuff>(), duration);
			target.AddBuff(BuffID.Confused, duration);

			if (owner < 0 || owner >= Main.maxPlayers) {
				return;
			}

			Player player = Main.player[owner];
			if (!player.active || player.dead) {
				return;
			}

			player.statLife = Utils.Clamp(player.statLife + healAmount, 0, player.statLifeMax2);
			player.HealEffect(healAmount, true);
		}
	}

	public sealed class ShadowRendDebuff : ModBuff
	{
		public override string Texture => "ChaoticDimensions/Content/Buffs/ShadowRendDebuff";
	}

	public sealed class ShadowManaPotionBuff : ModBuff
	{
		public override string Texture => "ChaoticDimensions/Content/Buffs/ShadowManaPotionBuff";

		public override void Update(Player player, ref int buffIndex) {
			player.statDefense += 40;
			player.manaRegenBonus += 60;
			player.manaCost -= 0.18f;
		}
	}

	public sealed class ShadowMeleePotionBuff : ModBuff
	{
		public override string Texture => "ChaoticDimensions/Content/Buffs/ShadowMeleePotionBuff";

		public override void Update(Player player, ref int buffIndex) {
			player.statDefense += 135;
			player.GetDamage(DamageClass.Melee) += 0.4f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.28f;
			player.moveSpeed += 0.12f;
		}
	}

	public sealed class ShadowCrystalMinionBuff : ModBuff
	{
		public override string Texture => "ChaoticDimensions/Content/Buffs/ShadowCrystalMinionBuff";

		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ShadowCrystalMinion>()] > 0) {
				player.buffTime[buffIndex] = 18000;
				return;
			}

			player.DelBuff(buffIndex);
			buffIndex--;
		}
	}

	public sealed class ShadowRendGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public override void UpdateLifeRegen(NPC npc, ref int damage) {
			if (!npc.HasBuff<ShadowRendDebuff>()) {
				return;
			}

			if (npc.lifeRegen > 0) {
				npc.lifeRegen = 0;
			}

			npc.lifeRegen -= 420;
			if (damage < 70) {
				damage = 70;
			}

			if (Main.rand.NextBool(4)) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame, Main.rand.NextFloat(-1.4f, 1.4f), Main.rand.NextFloat(-1.4f, 1.4f));
			}
		}
	}
}
