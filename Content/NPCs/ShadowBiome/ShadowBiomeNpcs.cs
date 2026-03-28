using ChaoticDimensions.Content.Items.ShadowBiome;
using ChaoticDimensions.Content.NPCs.MinecraftLegacy;
using ShadowBiomeScene = ChaoticDimensions.Content.Scenes.ShadowBiome;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.ShadowBiome
{
	public abstract class ShadowBiomeNpcBase : MinecraftLegacyNpcBase
	{
		public override string Texture => $"ChaoticDimensions/Content/NPCs/ShadowBiome/{GetType().Name}";

		protected virtual bool UndergroundOnly => false;
		protected virtual bool SurfaceOnly => false;
		protected virtual float ShadowSurfaceChance => 0f;
		protected virtual float ShadowUndergroundChance => 0f;

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.PlayerSafe || spawnInfo.Invasion || !spawnInfo.Player.InModBiome<ShadowBiomeScene>()) {
				return 0f;
			}

			bool surface = spawnInfo.Player.ZoneOverworldHeight || spawnInfo.Player.ZoneSkyHeight;
			bool underground = spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight;

			if (SurfaceOnly && !surface) {
				return 0f;
			}

			if (UndergroundOnly && !underground) {
				return 0f;
			}

			if (surface && ShadowSurfaceChance > 0f) {
				return ShadowSurfaceChance;
			}

			if (underground && ShadowUndergroundChance > 0f) {
				return ShadowUndergroundChance;
			}

			return 0f;
		}

		public override void AI() {
			base.AI();
			if (!NPC.friendly && NPC.HasValidTarget && !Main.player[NPC.target].InModBiome<ShadowBiomeScene>()) {
				NPC.EncourageDespawn(15);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			base.ModifyNPCLoot(npcLoot);
			if (!NPC.friendly) {
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShadowScrap>(), 1, 1, 3));
				npcLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 4, 1, 2));
				npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ModContent.ItemType<SoulOfShadow>(), 2, 1, 2));
			}
		}

		public override bool CheckActive() => false;
	}

	public abstract class ShadowGroundHostileBase : ShadowBiomeNpcBase
	{
		protected override LegacyMobMovementStyle MovementStyle => LegacyMobMovementStyle.GroundHostile;
		protected override float WanderSpeed => 1.65f;
	}

	public abstract class ShadowFlyingHostileBase : ShadowBiomeNpcBase
	{
		protected override LegacyMobMovementStyle MovementStyle => LegacyMobMovementStyle.FlyingHostile;
		protected override float WanderSpeed => 2.15f;
	}

	public sealed class Phantasm : ShadowFlyingHostileBase
	{
		protected override int BaseLifeMax => 540;
		protected override int BaseDamage => 76;
		protected override int BaseDefense => 18;
		protected override int HitboxWidth => 40;
		protected override int HitboxHeight => 56;
		protected override int FrameCount => 4;
		protected override bool SurfaceOnly => true;
		protected override float ShadowSurfaceChance => 0.075f;

		public override void AI() {
			base.AI();
			Lighting.AddLight(NPC.Center, 0.08f, 0f, 0.12f);
		}
	}

	public sealed class ShadowEye : ShadowFlyingHostileBase
	{
		protected override int BaseLifeMax => 620;
		protected override int BaseDamage => 82;
		protected override int BaseDefense => 20;
		protected override int HitboxWidth => 42;
		protected override int HitboxHeight => 42;
		protected override int FrameCount => 4;
		protected override float ShadowSurfaceChance => 0.05f;
		protected override float ShadowUndergroundChance => 0.06f;
	}

	public sealed class ShadowSlime : ShadowGroundHostileBase
	{
		protected override int BaseLifeMax => 680;
		protected override int BaseDamage => 84;
		protected override int BaseDefense => 22;
		protected override int HitboxWidth => 48;
		protected override int HitboxHeight => 34;
		protected override int FrameCount => 2;
		protected override float ShadowSurfaceChance => 0.045f;
		protected override float ShadowUndergroundChance => 0.045f;
		protected override float WanderSpeed => 1.2f;
	}

	public sealed class ShadowWorm : ShadowFlyingHostileBase
	{
		protected override int BaseLifeMax => 840;
		protected override int BaseDamage => 104;
		protected override int BaseDefense => 26;
		protected override int HitboxWidth => 64;
		protected override int HitboxHeight => 32;
		protected override int FrameCount => 4;
		protected override bool UndergroundOnly => true;
		protected override float ShadowUndergroundChance => 0.072f;
		protected override float WanderSpeed => 2.7f;
	}

	public sealed class KrakenSquid : ShadowBiomeNpcBase
	{
		protected override LegacyMobMovementStyle MovementStyle => LegacyMobMovementStyle.FlyingPassive;
		protected override int BaseLifeMax => 260;
		protected override int HitboxWidth => 30;
		protected override int HitboxHeight => 28;
		protected override int FrameCount => 2;
		protected override float WanderSpeed => 0.4f;

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.PlayerSafe || spawnInfo.Invasion || !spawnInfo.Player.ZoneBeach || !spawnInfo.Water) {
				return 0f;
			}

			return 0.12f;
		}

		public override void SetDefaults() {
			base.SetDefaults();
			NPC.friendly = true;
			NPC.damage = 0;
			NPC.catchItem = (short)ItemID.None;
			NPC.noTileCollide = false;
		}

		public override void AI() {
			NPC.velocity.X *= 0.94f;
			if (NPC.wet) {
				NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, (float)System.Math.Sin(Main.GameUpdateCount * 0.12f) * 1.35f, 0.08f);
			}
			else {
				NPC.velocity.Y += 0.2f;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
		}
	}
}
