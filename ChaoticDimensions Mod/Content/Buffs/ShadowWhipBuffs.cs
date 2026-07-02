// Regista os buffs e debuffs agrupados em Shadow Whip Buffs.

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Buffs
{
	public sealed class RosalitaTagBuff : ModBuff
	{
		public const int TagDamage = 14;
		public override string Texture => "ChaoticDimensions/Content/Buffs/MonthraButterflyBuff";

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			BuffID.Sets.IsATagBuff[Type] = true;
		}
	}

	public sealed class EclipsedMonthraTagBuff : ModBuff
	{
		public const int TagDamage = 28;
		public override string Texture => "ChaoticDimensions/Content/Buffs/MonthraButterflyBuff";

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			BuffID.Sets.IsATagBuff[Type] = true;
		}
	}

	public sealed class ShadowTagBuff : ModBuff
	{
		public const int TagDamage = 56;
		public override string Texture => "ChaoticDimensions/Content/Buffs/ShadowTagBuff";

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			BuffID.Sets.IsATagBuff[Type] = true;
		}
	}

	public sealed class ShadowWhipGlobalNpc : GlobalNPC
	{
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
			if (projectile.npcProj || projectile.trap || !projectile.IsMinionOrSentryRelated) {
				return;
			}

			float tagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
			if (npc.HasBuff<RosalitaTagBuff>()) {
				modifiers.FlatBonusDamage += RosalitaTagBuff.TagDamage * tagMultiplier;
			}

			if (npc.HasBuff<EclipsedMonthraTagBuff>()) {
				modifiers.FlatBonusDamage += EclipsedMonthraTagBuff.TagDamage * tagMultiplier;
			}

			if (npc.HasBuff<ShadowTagBuff>()) {
				modifiers.FlatBonusDamage += ShadowTagBuff.TagDamage * tagMultiplier;
			}
		}
	}
}
