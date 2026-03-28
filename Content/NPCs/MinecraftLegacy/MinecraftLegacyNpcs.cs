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
		protected virtual int FrameCount => 1;
		protected virtual int AnimationSpeed => 8;
		protected virtual float SurfaceSpawnChance => 0f;
		protected virtual float CavernSpawnChance => 0f;
		protected virtual bool DayOnly => false;
		protected virtual bool NightOnly => false;
		protected virtual bool ExplodesNearPlayer => false;
		protected virtual int ExplosionDamage => BaseDamage * 2;
		protected virtual int ExplosionFuseTime => 120;
		protected virtual float ExplosionRadius => 96f;
		protected virtual bool ExplosionBreaksTiles => false;
		protected virtual int TileExplosionRadius => 2;
		protected virtual float WanderSpeed => 1.1f;
		protected virtual int OptionalDropItem => 0;
		protected virtual int OptionalDropMin => 1;
		protected virtual int OptionalDropMax => 1;

		protected bool IsHostile => MovementStyle is LegacyMobMovementStyle.GroundHostile or LegacyMobMovementStyle.FlyingHostile;
		protected bool IsFlying => MovementStyle is LegacyMobMovementStyle.FlyingHostile or LegacyMobMovementStyle.FlyingPassive;

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = FrameCount;
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

		public override void FindFrame(int frameHeight) {
			if (FrameCount <= 1) {
				NPC.frame.Y = 0;
				return;
			}

			NPC.frameCounter++;
			if (NPC.frameCounter < AnimationSpeed) {
				return;
			}

			NPC.frameCounter = 0;
			NPC.frame.Y += frameHeight;
			if (NPC.frame.Y >= frameHeight * FrameCount) {
				NPC.frame.Y = 0;
			}
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

				if (NPC.localAI[1] >= ExplosionFuseTime) {
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

					if (Vector2.Distance(player.Center, NPC.Center) <= ExplosionRadius) {
						player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), ExplosionDamage, player.Center.X >= NPC.Center.X ? 1 : -1);
					}
				}

				if (ExplosionBreaksTiles) {
					BreakTilesAroundCenter();
				}
			}

			SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
			for (int i = 0; i < 25; i++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Smoke, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
			}

			NPC.StrikeInstantKill();
		}

		private void BreakTilesAroundCenter() {
			Point centerTile = NPC.Center.ToTileCoordinates();
			for (int x = centerTile.X - TileExplosionRadius; x <= centerTile.X + TileExplosionRadius; x++) {
				for (int y = centerTile.Y - TileExplosionRadius; y <= centerTile.Y + TileExplosionRadius; y++) {
					if (!WorldGen.InWorld(x, y, 10)) {
						continue;
					}

					if (Vector2.Distance(new Vector2(x * 16f, y * 16f), NPC.Center) > (TileExplosionRadius + 0.5f) * 16f) {
						continue;
					}

					WorldGen.KillTile(x, y, false, false, true);
					if (Main.netMode == NetmodeID.Server) {
						NetMessage.SendTileSquare(-1, x, y, 1);
					}
				}
			}
		}

		protected static bool IsForest(Player player) {
			return (player.ZoneOverworldHeight || player.ZoneSkyHeight)
				&& !player.ZoneSnow
				&& !player.ZoneDesert
				&& !player.ZoneJungle
				&& !player.ZoneBeach
				&& !player.ZoneCorrupt
				&& !player.ZoneCrimson
				&& !player.ZoneHallow
				&& !player.ZoneDungeon
				&& !player.ZoneGlowshroom;
		}

		protected static bool IsSnow(Player player) => player.ZoneSnow && (player.ZoneOverworldHeight || player.ZoneSkyHeight || player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight);

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Texture2D texture = TextureAssets.Npc[Type].Value;
			int frameHeight = FrameCount <= 1 ? texture.Height : texture.Height / FrameCount;
			Rectangle source = new Rectangle(0, NPC.frame.Y, texture.Width, frameHeight);
			Vector2 origin = source.Size() * 0.5f;
			SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			spriteBatch.Draw(texture, NPC.Center - screenPos, source, NPC.GetAlpha(drawColor), NPC.rotation, origin, DrawScale, effects, 0f);
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

	public sealed class BigButterfly : LegacyFlyingPassiveNpcBase
	{
		protected override int BaseLifeMax => 42;
		protected override int HitboxWidth => 54;
		protected override int HitboxHeight => 54;
		protected override float DrawScale => 0.42f;
		protected override int FrameCount => 6;
	}

	public sealed class AlessandraNpc : LegacyGroundPassiveNpcBase
	{
		protected override int BaseLifeMax => 250;
		protected override int BaseDefense => 6;
		protected override float DrawScale => 0.9f;
		protected override int FrameCount => 4;
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
		protected override int FrameCount => 4;
	}

	public sealed class BlueButterfly : LegacyFlyingPassiveNpcBase
	{
		protected override int BaseLifeMax => 10;
		protected override int HitboxWidth => 20;
		protected override int HitboxHeight => 20;
		protected override float DrawScale => 0.72f;
		protected override int FrameCount => 2;

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return !spawnInfo.PlayerSafe && Main.dayTime && IsForest(spawnInfo.Player) ? 0.18f : 0f;
		}
	}

	public sealed class GreenButterfly : LegacyFlyingPassiveNpcBase
	{
		protected override int BaseLifeMax => 10;
		protected override int HitboxWidth => 20;
		protected override int HitboxHeight => 20;
		protected override float DrawScale => 0.8f;
		protected override int FrameCount => 5;
	}

	public sealed class RedButterfly : LegacyFlyingPassiveNpcBase
	{
		protected override int BaseLifeMax => 10;
		protected override int HitboxWidth => 20;
		protected override int HitboxHeight => 20;
		protected override float DrawScale => 0.72f;
		protected override int FrameCount => 2;

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return !spawnInfo.PlayerSafe && Main.dayTime && (IsForest(spawnInfo.Player) || IsSnow(spawnInfo.Player)) ? 0.055f : 0f;
		}
	}

	public sealed class YellowButterfly : LegacyFlyingPassiveNpcBase
	{
		protected override int BaseLifeMax => 10;
		protected override int HitboxWidth => 20;
		protected override int HitboxHeight => 20;
		protected override float DrawScale => 0.72f;
		protected override int FrameCount => 2;

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			return !spawnInfo.PlayerSafe && Main.dayTime && (IsForest(spawnInfo.Player) || IsSnow(spawnInfo.Player)) ? 0.055f : 0f;
		}
	}

	public sealed class AppleCow : LegacyGroundPassiveNpcBase
	{
		protected override int BaseLifeMax => 60;
		protected override int BaseDefense => 2;
		protected override float DrawScale => 1f;
		protected override int FrameCount => 4;
		protected override int OptionalDropItem => ItemID.Apple;
		protected override int OptionalDropMin => 1;
		protected override int OptionalDropMax => 3;
	}

	public sealed class GoldenAppleCow : LegacyGroundPassiveNpcBase
	{
		protected override int BaseLifeMax => 72;
		protected override int BaseDefense => 3;
		protected override float DrawScale => 1f;
		protected override int FrameCount => 4;
		protected override int OptionalDropItem => ItemID.Apple;
		protected override int OptionalDropMin => 2;
		protected override int OptionalDropMax => 4;
	}

	public sealed class CrystalAppleCow : LegacyGroundPassiveNpcBase
	{
		protected override int BaseLifeMax => 90;
		protected override int BaseDefense => 4;
		protected override float DrawScale => 1f;
		protected override int FrameCount => 4;

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
		protected override int FrameCount => 4;

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
		protected override int FrameCount => 4;
		protected override int OptionalDropItem => ItemID.Bacon;
		protected override int OptionalDropMin => 1;
		protected override int OptionalDropMax => 2;
	}
}
