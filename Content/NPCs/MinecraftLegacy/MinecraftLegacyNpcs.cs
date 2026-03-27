using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.MinecraftLegacy
{
	public enum LegacyMobMovementStyle
	{
		GroundPassive,
		GroundHostile,
		FlyingPassive,
		FlyingHostile
	}

	public abstract class MinecraftLegacyNpcBase : ModNPC
	{
		public override string Texture => $"ChaoticDimensions/Content/NPCs/MinecraftLegacy/{GetType().Name}";

		protected abstract LegacyMobMovementStyle MovementStyle { get; }
		protected abstract int BaseLifeMax { get; }
		protected virtual int BaseDamage => 0;
		protected virtual int BaseDefense => 0;
		protected virtual int HitboxWidth => 40;
		protected virtual int HitboxHeight => 40;
		protected virtual float KnockbackResistValue => 0.5f;
		protected virtual float DrawScale => 1f;
		protected virtual float SurfaceSpawnChance => 0f;
		protected virtual float CavernSpawnChance => 0f;
		protected virtual bool DayOnly => false;
		protected virtual bool NightOnly => false;
		protected virtual bool ExplodesNearPlayer => false;
		protected virtual int ExplosionDamage => BaseDamage * 2;
		protected virtual float WanderSpeed => 1.1f;
		protected virtual int OptionalDropItem => 0;
		protected virtual int OptionalDropMin => 1;
		protected virtual int OptionalDropMax => 1;

		protected bool IsHostile => MovementStyle is LegacyMobMovementStyle.GroundHostile or LegacyMobMovementStyle.FlyingHostile;
		protected bool IsFlying => MovementStyle is LegacyMobMovementStyle.FlyingHostile or LegacyMobMovementStyle.FlyingPassive;

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1;
		}

		public override void SetDefaults() {
			NPC.width = HitboxWidth;
			NPC.height = HitboxHeight;
			NPC.damage = BaseDamage;
			NPC.defense = BaseDefense;
			NPC.lifeMax = BaseLifeMax;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(silver: 2);
			NPC.knockBackResist = KnockbackResistValue;
			NPC.aiStyle = -1;
			NPC.noGravity = IsFlying;
			NPC.noTileCollide = IsFlying;
			NPC.friendly = !IsHostile;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.PlayerSafe || spawnInfo.Invasion) {
				return 0f;
			}

			if (DayOnly && !Main.dayTime) {
				return 0f;
			}

			if (NightOnly && Main.dayTime) {
				return 0f;
			}

			bool surface = spawnInfo.Player.ZoneOverworldHeight || spawnInfo.Player.ZoneSkyHeight;
			bool cavern = spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight;

			if (surface && SurfaceSpawnChance > 0f) {
				return SurfaceSpawnChance;
			}

			if (cavern && CavernSpawnChance > 0f) {
				return CavernSpawnChance;
			}

			return 0f;
		}

		public override void AI() {
			switch (MovementStyle) {
				case LegacyMobMovementStyle.GroundHostile:
					DoGroundHostileAI();
					break;
				case LegacyMobMovementStyle.FlyingHostile:
					DoFlyingHostileAI();
					break;
				case LegacyMobMovementStyle.FlyingPassive:
					DoFlyingPassiveAI();
					break;
				default:
					DoGroundPassiveAI();
					break;
			}

			if (ExplodesNearPlayer) {
				UpdateExplosionFuse();
			}

			NPC.spriteDirection = NPC.velocity.X >= 0f ? 1 : -1;
		}

		private void DoGroundPassiveAI() {
			Player closestPlayer = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)];
			float fleeDirection = closestPlayer.Center.X >= NPC.Center.X ? -1f : 1f;

			NPC.localAI[0]++;
			if (NPC.localAI[0] >= NPC.ai[0] || NPC.ai[0] <= 0f) {
				NPC.ai[0] = Main.rand.Next(80, 180);
				NPC.ai[1] = Main.rand.NextFloat(-1f, 1f);
				NPC.localAI[0] = 0f;
				NPC.netUpdate = true;
			}

			float horizontalSpeed = NPC.ai[1] * WanderSpeed;
			if (Vector2.Distance(NPC.Center, closestPlayer.Center) < 110f) {
				horizontalSpeed = fleeDirection * (WanderSpeed + 1.2f);
			}

			NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, horizontalSpeed, 0.08f);
			if (NPC.collideX) {
				NPC.velocity.Y = -4.6f;
			}
		}

		private void DoGroundHostileAI() {
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			if (!player.active || player.dead) {
				NPC.velocity.X *= 0.94f;
				return;
			}

			float direction = player.Center.X >= NPC.Center.X ? 1f : -1f;
			float targetSpeed = direction * (WanderSpeed + 1.25f);
			NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, targetSpeed, 0.12f);

			if (NPC.collideX || (player.Center.Y + 12f < NPC.Center.Y && NPC.velocity.Y == 0f)) {
				NPC.velocity.Y = -5.8f;
			}
		}

		private void DoFlyingPassiveAI() {
			Player closestPlayer = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)];
			NPC.localAI[0]++;
			if (NPC.localAI[0] >= NPC.ai[0] || NPC.ai[0] <= 0f) {
				NPC.ai[0] = Main.rand.Next(70, 140);
				NPC.ai[1] = Main.rand.NextFloat(-1.8f, 1.8f);
				NPC.ai[2] = Main.rand.NextFloat(-1.2f, 1.2f);
				NPC.localAI[0] = 0f;
				NPC.netUpdate = true;
			}

			Vector2 desiredVelocity = new Vector2(NPC.ai[1], NPC.ai[2]);
			if (Vector2.Distance(NPC.Center, closestPlayer.Center) < 120f) {
				desiredVelocity = (NPC.Center - closestPlayer.Center).SafeNormalize(Vector2.UnitY) * 2.8f;
			}

			NPC.velocity = Vector2.Lerp(NPC.velocity, desiredVelocity, 0.06f);
		}

		private void DoFlyingHostileAI() {
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			if (!player.active || player.dead) {
				NPC.velocity *= 0.96f;
				return;
			}

			Vector2 desiredVelocity = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY) * (WanderSpeed + 2.4f);
			NPC.velocity = Vector2.Lerp(NPC.velocity, desiredVelocity, 0.055f);
		}

		private void UpdateExplosionFuse() {
			if (!NPC.HasValidTarget) {
				NPC.localAI[1] = System.Math.Max(0f, NPC.localAI[1] - 1f);
				return;
			}

			Player player = Main.player[NPC.target];
			float distance = Vector2.Distance(NPC.Center, player.Center);
			if (distance <= 68f && Collision.CanHitLine(NPC.Center, 0, 0, player.Center, 0, 0)) {
				NPC.localAI[1]++;
				if (Main.rand.NextBool(6)) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke);
				}

				if (NPC.localAI[1] >= 75f) {
					Explode();
				}
			}
			else {
				NPC.localAI[1] = System.Math.Max(0f, NPC.localAI[1] - 2f);
			}
		}

		private void Explode() {
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				for (int i = 0; i < Main.maxPlayers; i++) {
					Player player = Main.player[i];
					if (!player.active || player.dead) {
						continue;
					}

					if (Vector2.Distance(player.Center, NPC.Center) <= 96f) {
						player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), ExplosionDamage, player.Center.X >= NPC.Center.X ? 1 : -1);
					}
				}
			}

			SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
			for (int i = 0; i < 25; i++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
			}

			NPC.StrikeInstantKill();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Texture2D texture = TextureAssets.Npc[Type].Value;
			Vector2 origin = texture.Size() * 0.5f;
			SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			spriteBatch.Draw(texture, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, origin, DrawScale, effects, 0f);
			return false;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			if (OptionalDropItem > 0) {
				npcLoot.Add(ItemDropRule.Common(OptionalDropItem, 1, OptionalDropMin, OptionalDropMax));
			}
		}
	}

	public abstract class LegacyGroundHostileNpcBase : MinecraftLegacyNpcBase
	{
		protected override LegacyMobMovementStyle MovementStyle => LegacyMobMovementStyle.GroundHostile;
		protected override float SurfaceSpawnChance => 0.035f;
		protected override float CavernSpawnChance => 0.028f;
		protected override bool NightOnly => true;
	}

	public abstract class LegacyFlyingHostileNpcBase : MinecraftLegacyNpcBase
	{
		protected override LegacyMobMovementStyle MovementStyle => LegacyMobMovementStyle.FlyingHostile;
		protected override float SurfaceSpawnChance => 0.02f;
		protected override float CavernSpawnChance => 0.018f;
		protected override bool NightOnly => true;
		protected override float WanderSpeed => 1.6f;
	}

	public abstract class LegacyGroundPassiveNpcBase : MinecraftLegacyNpcBase
	{
		protected override LegacyMobMovementStyle MovementStyle => LegacyMobMovementStyle.GroundPassive;
		protected override float SurfaceSpawnChance => 0.05f;
		protected override bool DayOnly => true;
	}

	public abstract class LegacyFlyingPassiveNpcBase : MinecraftLegacyNpcBase
	{
		protected override LegacyMobMovementStyle MovementStyle => LegacyMobMovementStyle.FlyingPassive;
		protected override float SurfaceSpawnChance => 0.045f;
		protected override bool DayOnly => true;
		protected override float WanderSpeed => 0.9f;
	}

	public sealed class HappyCreeper : LegacyGroundHostileNpcBase
	{
		protected override int BaseLifeMax => 70;
		protected override int BaseDamage => 16;
		protected override bool ExplodesNearPlayer => true;
		protected override float DrawScale => 0.85f;
	}

	public sealed class WhiteCreeper : LegacyGroundHostileNpcBase
	{
		protected override int BaseLifeMax => 82;
		protected override int BaseDamage => 18;
		protected override int BaseDefense => 3;
		protected override bool ExplodesNearPlayer => true;
		protected override float DrawScale => 0.85f;
	}

	public sealed class CrystalCreeper : LegacyGroundHostileNpcBase
	{
		protected override int BaseLifeMax => 96;
		protected override int BaseDamage => 20;
		protected override int BaseDefense => 5;
		protected override bool ExplodesNearPlayer => true;
		protected override int ExplosionDamage => 46;
		protected override float DrawScale => 0.85f;
		protected override int OptionalDropItem => ItemID.CrystalShard;
		protected override int OptionalDropMin => 1;
		protected override int OptionalDropMax => 3;
	}

	public sealed class ShadowCreeper : LegacyGroundHostileNpcBase
	{
		protected override int BaseLifeMax => 110;
		protected override int BaseDamage => 22;
		protected override int BaseDefense => 6;
		protected override bool ExplodesNearPlayer => true;
		protected override int ExplosionDamage => 54;
		protected override float DrawScale => 0.85f;
	}

	public sealed class Endernmon : LegacyFlyingHostileNpcBase
	{
		protected override int BaseLifeMax => 140;
		protected override int BaseDamage => 24;
		protected override int BaseDefense => 8;
		protected override float DrawScale => 0.9f;
	}

	public sealed class Fantasm : LegacyFlyingHostileNpcBase
	{
		protected override int BaseLifeMax => 120;
		protected override int BaseDamage => 19;
		protected override int BaseDefense => 5;
		protected override float DrawScale => 0.9f;
	}

	public sealed class FlyZombieSquid : LegacyFlyingHostileNpcBase
	{
		protected override int BaseLifeMax => 130;
		protected override int BaseDamage => 21;
		protected override int BaseDefense => 6;
		protected override float DrawScale => 0.9f;
	}

	public sealed class Formiga : LegacyGroundHostileNpcBase
	{
		protected override int BaseLifeMax => 46;
		protected override int BaseDamage => 13;
		protected override float DrawScale => 0.8f;
		protected override float SurfaceSpawnChance => 0.02f;
		protected override bool DayOnly => false;
		protected override bool NightOnly => false;
	}

	public sealed class ChaoticMimic : LegacyGroundHostileNpcBase
	{
		protected override int BaseLifeMax => 190;
		protected override int BaseDamage => 28;
		protected override int BaseDefense => 10;
		protected override float SurfaceSpawnChance => 0f;
		protected override float CavernSpawnChance => 0.01f;
		protected override float DrawScale => 0.95f;
		protected override int OptionalDropItem => ItemID.GoldCoin;
		protected override int OptionalDropMin => 1;
		protected override int OptionalDropMax => 3;
	}

	public sealed class RatMonster : LegacyGroundHostileNpcBase
	{
		protected override int BaseLifeMax => 38;
		protected override int BaseDamage => 10;
		protected override float DrawScale => 0.75f;
	}

	public sealed class SnowBlaze : LegacyFlyingHostileNpcBase
	{
		protected override int BaseLifeMax => 118;
		protected override int BaseDamage => 22;
		protected override int BaseDefense => 7;
		protected override float DrawScale => 0.85f;
	}

	public sealed class RedGiantSpider : LegacyGroundHostileNpcBase
	{
		protected override int BaseLifeMax => 200;
		protected override int BaseDamage => 26;
		protected override int BaseDefense => 8;
		protected override float DrawScale => 1.1f;
	}

	public sealed class GiantZombie : LegacyGroundHostileNpcBase
	{
		protected override int BaseLifeMax => 220;
		protected override int BaseDamage => 30;
		protected override int BaseDefense => 10;
		protected override float DrawScale => 1.15f;
	}

	public sealed class WhiteZombie : LegacyGroundHostileNpcBase
	{
		protected override int BaseLifeMax => 85;
		protected override int BaseDamage => 18;
		protected override int BaseDefense => 4;
		protected override float DrawScale => 0.9f;
	}

	public sealed class ZombieSombra : LegacyGroundHostileNpcBase
	{
		protected override int BaseLifeMax => 100;
		protected override int BaseDamage => 20;
		protected override int BaseDefense => 7;
		protected override float DrawScale => 0.9f;
	}

	public sealed class BigButterfly : LegacyFlyingPassiveNpcBase
	{
		protected override int BaseLifeMax => 42;
		protected override int HitboxWidth => 54;
		protected override int HitboxHeight => 54;
		protected override float DrawScale => 0.42f;
	}

	public sealed class AlessandraNpc : LegacyGroundPassiveNpcBase
	{
		protected override int BaseLifeMax => 250;
		protected override int BaseDefense => 6;
		protected override float DrawScale => 0.9f;
	}

	public sealed class FireflyCritter : LegacyFlyingPassiveNpcBase
	{
		protected override int BaseLifeMax => 8;
		protected override int HitboxWidth => 18;
		protected override int HitboxHeight => 18;
		protected override float DrawScale => 1f;
		protected override float SurfaceSpawnChance => 0.035f;
		protected override bool DayOnly => false;
		protected override bool NightOnly => true;
	}

	public sealed class BlueButterfly : LegacyFlyingPassiveNpcBase
	{
		protected override int BaseLifeMax => 10;
	}

	public sealed class GreenButterfly : LegacyFlyingPassiveNpcBase
	{
		protected override int BaseLifeMax => 10;
	}

	public sealed class RedButterfly : LegacyFlyingPassiveNpcBase
	{
		protected override int BaseLifeMax => 10;
	}

	public sealed class YellowButterfly : LegacyFlyingPassiveNpcBase
	{
		protected override int BaseLifeMax => 10;
	}

	public sealed class AppleCow : LegacyGroundPassiveNpcBase
	{
		protected override int BaseLifeMax => 60;
		protected override int BaseDefense => 2;
		protected override float DrawScale => 1f;
		protected override int OptionalDropItem => ItemID.Apple;
		protected override int OptionalDropMin => 1;
		protected override int OptionalDropMax => 3;
	}

	public sealed class GoldenAppleCow : LegacyGroundPassiveNpcBase
	{
		protected override int BaseLifeMax => 72;
		protected override int BaseDefense => 3;
		protected override float DrawScale => 1f;
		protected override int OptionalDropItem => ItemID.Apple;
		protected override int OptionalDropMin => 2;
		protected override int OptionalDropMax => 4;
	}

	public sealed class CrystalAppleCow : LegacyGroundPassiveNpcBase
	{
		protected override int BaseLifeMax => 90;
		protected override int BaseDefense => 4;
		protected override float DrawScale => 1f;

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			base.ModifyNPCLoot(npcLoot);
			npcLoot.Add(ItemDropRule.Common(ItemID.Apple, 1, 1, 3));
			npcLoot.Add(ItemDropRule.Common(ItemID.CrystalShard, 1, 1, 2));
		}
	}

	public sealed class CrystalGoldenAppleCow : LegacyGroundPassiveNpcBase
	{
		protected override int BaseLifeMax => 115;
		protected override int BaseDefense => 6;
		protected override float DrawScale => 1f;

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			base.ModifyNPCLoot(npcLoot);
			npcLoot.Add(ItemDropRule.Common(ItemID.Apple, 1, 2, 4));
			npcLoot.Add(ItemDropRule.Common(ItemID.CrystalShard, 1, 2, 4));
		}
	}

	public sealed class DimensionPig : LegacyGroundPassiveNpcBase
	{
		protected override int BaseLifeMax => 52;
		protected override int BaseDefense => 2;
		protected override float DrawScale => 1f;
		protected override int OptionalDropItem => ItemID.Bacon;
		protected override int OptionalDropMin => 1;
		protected override int OptionalDropMax => 2;
	}
}
