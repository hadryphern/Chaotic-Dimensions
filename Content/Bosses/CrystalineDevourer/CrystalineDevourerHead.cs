using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChaoticDimensions.Content.BossBars;
using ChaoticDimensions.Content.Items.Accessories;
using ChaoticDimensions.Content.Items.Consumables;
using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.Items.Weapons.Magic;
using ChaoticDimensions.Content.Items.Weapons.Melee;
using ChaoticDimensions.Content.Items.Weapons.Ranged;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using ChaoticDimensions.Common.Systems;

namespace ChaoticDimensions.Content.Bosses.CrystalineDevourer
{
	internal enum CrystalineAttackState
	{
		Orbit,
		Dash,
		SupremeLaser
	}

	[AutoloadBossHead]
	public sealed class CrystalineDevourerHead : ModNPC
	{
		public const int SharedLifeMax = 5000000;
		public const int IndividualLifeMax = SharedLifeMax / 2;
		private const int MinSegments = 80;
		private const int MaxSegments = 92;
		private const float SpawnBodySpacing = 48f;
		private const float SpawnHeadSpacing = 66f;
		private const float SpawnTailSpacing = 72f;
		private const float HeadDrawForwardOffset = 12f;
		private const int OrbitDuration = 180;
		private const int DashDuration = 38;
		private const int DashCooldown = 30;
		private const int SupremeSkyBeamTelegraph = 45;
		private const int SupremeSkyBeamDuration = 150;

		private bool initialized;
		private int wormIndex;
		private int twinHeadIndex = -1;
		private bool hasSpawnedTwin;
		private int phaseTimer;
		private CrystalineAttackState attackState = CrystalineAttackState.Orbit;
		private float orbitAngle;
		private int laserActorIndex;
		private int sideSign = 1;
		private int dashTimer;
		private Vector2 dashVelocity;
		private Vector2 lastTargetPosition;
		private Vector2 cachedBeamDirection = Vector2.UnitX;
		private int shardBurstTimer;
		private int pressureBurstTimer;
		private int pressureRetargetTimer;
		private Vector2 pressureTargetPosition;
		private int skyBeamTimer;

		private bool IsLeader => wormIndex == 0;
		private bool ControlsState => IsLeader || !TwinIsAlive;
		private bool TwinIsAlive => twinHeadIndex >= 0 && twinHeadIndex < Main.maxNPCs && Main.npc[twinHeadIndex].active && Main.npc[twinHeadIndex].type == Type;
		private NPC TwinNPC => TwinIsAlive ? Main.npc[twinHeadIndex] : null;
		private CrystalineDevourerHead Leader => IsLeader ? this : (TwinIsAlive ? TwinNPC.ModNPC as CrystalineDevourerHead : this);
		private float SharedLifeRatio => GetCombinedLife() / (float)Math.Max(1, GetCombinedLifeMax());

		private int CurrentPhase {
			get {
				float ratio = SharedLifeRatio;
				if (ratio > 0.85f)
					return 1;
				if (ratio > 0.75f)
					return 2;
				if (ratio > 0.7f)
					return 3;
				if (ratio > 0.65f)
					return 4;
				if (ratio > 0.6f)
					return 5;
				return 6;
			}
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1;
			NPCID.Sets.MustAlwaysDraw[Type] = true;
		}

		public override void SetDefaults() {
			NPC.width = 96;
			NPC.height = 96;
			NPC.damage = 260;
			NPC.defense = 5;
			NPC.lifeMax = IndividualLifeMax;
			NPC.knockBackResist = 0f;
			NPC.value = Item.buyPrice(platinum: 1, gold: 50);
			NPC.npcSlots = 12f;
			NPC.boss = true;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.BossBar = ModContent.GetInstance<CrystalineDevourerBossBar>();
			Music = MusicID.Boss3;
		}

		public override bool PreAI() {
			InitializeIfNeeded();
			SpawnSegmentsIfNeeded();
			SpawnTwinIfNeeded();
			SyncSharedLife();
			TargetOrDespawn();
			if (!NPC.active) {
				return false;
			}

			Player target = Main.player[NPC.target];
			if (!target.active || target.dead) {
				return false;
			}

			if (ControlsState) {
				CrystalineDevourerArenaSystem.EnsureArena(NPC, target);
			}

			if (ControlsState) {
				RunLeaderState(target);
				UpdateProjectilePressure(target);
			}

			UpdateMovement(target);
			UpdateCombatStats();
			UpdateRotation();
			UpdateOpacity();
			return false;
		}

		private void InitializeIfNeeded() {
			if (initialized) {
				return;
			}

			initialized = true;
			NPC.life = NPC.lifeMax;
			if (NPC.ai[2] >= 0f) {
				wormIndex = (int)NPC.ai[2];
			}

			shardBurstTimer = wormIndex * 6;
			pressureRetargetTimer = wormIndex * 14;
			skyBeamTimer = 120 + (wormIndex * 20);
		}

		private void SpawnSegmentsIfNeeded() {
			if (NPC.ai[0] != 0f || Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			NPC.realLife = NPC.whoAmI;
			int latest = NPC.whoAmI;
			int totalSegments = Main.rand.Next(MinSegments, MaxSegments + 1);
			int bodySegments = totalSegments - 2;
			IEntitySource source = NPC.GetSource_FromAI();
			Vector2 spawnDirection = GetSpawnDirection();

			for (int i = 0; i < bodySegments; i++) {
				int segment = NPC.NewNPC(source, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CrystalineDevourerBody>(), NPC.whoAmI);
				NPC segmentNpc = Main.npc[segment];
				segmentNpc.realLife = NPC.whoAmI;
				segmentNpc.ai[1] = latest;
				segmentNpc.ai[2] = NPC.whoAmI;
				segmentNpc.ai[3] = 0f;
				PositionSpawnedSegment(Main.npc[latest], segmentNpc, spawnDirection, latest == NPC.whoAmI ? SpawnHeadSpacing : SpawnBodySpacing);
				Main.npc[latest].ai[0] = segment;
				latest = segment;
			}

			int tail = NPC.NewNPC(source, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CrystalineDevourerTail>(), NPC.whoAmI);
			NPC tailNpc = Main.npc[tail];
			tailNpc.realLife = NPC.whoAmI;
			tailNpc.ai[0] = -1f;
			tailNpc.ai[1] = latest;
			tailNpc.ai[2] = NPC.whoAmI;
			Main.npc[latest].ai[3] = 1f;
			PositionSpawnedSegment(Main.npc[latest], tailNpc, spawnDirection, SpawnTailSpacing);
			Main.npc[latest].ai[0] = tail;
			NPC.netUpdate = true;
		}

		private Vector2 GetSpawnDirection() {
			if (NPC.velocity.LengthSquared() > 0.01f) {
				return NPC.velocity.SafeNormalize(Vector2.UnitY);
			}

			if (NPC.HasValidTarget) {
				Vector2 targetDirection = Main.player[NPC.target].Center - NPC.Center;
				if (targetDirection.LengthSquared() > 0.01f) {
					return targetDirection.SafeNormalize(Vector2.UnitY);
				}
			}

			return Vector2.UnitY;
		}

		private static void PositionSpawnedSegment(NPC ahead, NPC segment, Vector2 spawnDirection, float spacing) {
			Vector2 direction = spawnDirection.LengthSquared() > 0.01f ? spawnDirection.SafeNormalize(Vector2.UnitY) : Vector2.UnitY;
			segment.Center = ahead.Center - direction * spacing;
			segment.rotation = direction.ToRotation() + MathHelper.PiOver2;
		}

		private void SpawnTwinIfNeeded() {
			if (!IsLeader || hasSpawnedTwin || Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int twin = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 120, Type, NPC.whoAmI);
			NPC twinNpc = Main.npc[twin];
			twinNpc.ai[2] = 1f;
			twinNpc.boss = false;
			twinNpc.netUpdate = true;

			var twinHead = (CrystalineDevourerHead)twinNpc.ModNPC;
			twinHead.wormIndex = 1;
			twinHead.twinHeadIndex = NPC.whoAmI;
			twinHead.orbitAngle = MathHelper.Pi;

			hasSpawnedTwin = true;
			twinHeadIndex = twin;
			NPC.netUpdate = true;
		}

		private void SyncSharedLife() {
			// Phase thresholds and boss bar use combined life, but each worm now keeps its own health pool.
		}

		private void TargetOrDespawn() {
			if (!CrystalineDevourerArenaSystem.HasAnyLivingPlayers()) {
				EndEncounterImmediately();
				return;
			}

			NPC.TargetClosest(false);
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead) {
				NPC.velocity.Y += 0.8f;
				NPC.EncourageDespawn(30);
				if (TwinIsAlive) {
					TwinNPC.EncourageDespawn(30);
				}
			}
		}

		private void RunLeaderState(Player target) {
			phaseTimer++;
			if (CurrentPhase <= 3) {
				attackState = CrystalineAttackState.Orbit;
				phaseTimer %= 10000;
				return;
			}

			if (CurrentPhase <= 5) {
				int cycle = OrbitDuration + DashDuration + DashCooldown;
				int cycleTimer = phaseTimer % cycle;
				attackState = cycleTimer < OrbitDuration ? CrystalineAttackState.Orbit : CrystalineAttackState.Dash;
				if (cycleTimer == OrbitDuration) {
					PrepareDash(target);
				}
				return;
			}

			skyBeamTimer--;
			if (skyBeamTimer <= 0) {
				SpawnSkyBeam(target);
				skyBeamTimer = GetNextSkyBeamDelay();
				NPC.netUpdate = true;
			}
			attackState = CrystalineAttackState.SupremeLaser;
		}

		private void PrepareDash(Player target) {
			Vector2 aim = target.Center - NPC.Center;
			if (aim == Vector2.Zero) {
				aim = Vector2.UnitY;
			}

			dashVelocity = aim.SafeNormalize(Vector2.UnitY) * GetDashSpeed();
			lastTargetPosition = target.Center;
			dashTimer = DashDuration;
			cachedBeamDirection = dashVelocity.SafeNormalize(Vector2.UnitX);
		}

		private void UpdateMovement(Player target) {
			var leader = Leader ?? this;
			CrystalineAttackState state = leader.attackState;
			if (!IsLeader && leader != this && !leader.NPC.active) {
				leader = this;
				state = attackState;
			}

			if (state == CrystalineAttackState.Dash && leader.dashTimer > 0) {
				DoDashMovement(leader);
				return;
			}

			if (state == CrystalineAttackState.SupremeLaser) {
				DoSupremeLaserMovement(target, leader);
				return;
			}

			DoOrbitMovement(target, leader);
		}

		private void DoOrbitMovement(Player target, CrystalineDevourerHead leader) {
			if (IsLeader) {
				orbitAngle += GetOrbitAngularSpeed();
			}

			float angle = leader.orbitAngle;
			Vector2 orbitDirection = Vector2.UnitX.RotatedBy(angle);
			Vector2 tangent = orbitDirection.RotatedBy(MathHelper.PiOver2);
			float radius = MathHelper.Lerp(1180f, 860f, 1f - leader.SharedLifeRatio);
			float signedRadiusOffset = wormIndex == 0 ? 180f : -180f;
			float signedLateralOffset = wormIndex == 0 ? GetOrbitSeparation() : -GetOrbitSeparation();
			Vector2 targetPosition = target.Center + orbitDirection * (radius + signedRadiusOffset) + tangent * signedLateralOffset;
			lastTargetPosition = targetPosition;
			SteerTowards(targetPosition, GetMoveSpeed(), GetTurnRate());
		}

		private void DoDashMovement(CrystalineDevourerHead leader) {
			Vector2 dashDirection = leader.dashVelocity.SafeNormalize(Vector2.UnitY);
			Vector2 lateral = dashDirection.RotatedBy(MathHelper.PiOver2) * (wormIndex == 0 ? GetDashSpread() : -GetDashSpread());
			Vector2 dashTarget = leader.lastTargetPosition + lateral;
			SteerTowards(dashTarget, leader.GetDashSpeed(), 0.45f);
			if (leader.IsLeader) {
				leader.dashTimer--;
			}
		}

		private void DoSupremeLaserMovement(Player target, CrystalineDevourerHead leader) {
			pressureRetargetTimer--;
			if (pressureRetargetTimer <= 0 || Vector2.Distance(NPC.Center, pressureTargetPosition) < 170f) {
				float phaseOffset = wormIndex == 0 ? 0f : MathHelper.Pi;
				float angle = leader.phaseTimer * 0.07f + phaseOffset + Main.rand.NextFloat(-0.95f, 0.95f);
				float radius = Main.rand.NextFloat(860f, 1460f);
				pressureTargetPosition = target.Center + Vector2.UnitX.RotatedBy(angle) * radius;
				dashVelocity = (target.Center - pressureTargetPosition).SafeNormalize(Vector2.UnitY) * (GetDashSpeed() + 12f);
				pressureRetargetTimer = Main.rand.Next(24, 40);
				NPC.netUpdate = true;
			}

			if (pressureRetargetTimer > 10) {
				SteerTowards(pressureTargetPosition, 62f, 0.36f);
			}
			else {
				NPC.velocity = Vector2.Lerp(NPC.velocity, dashVelocity, 0.24f);
			}

			if (Main.netMode != NetmodeID.MultiplayerClient) {
				shardBurstTimer++;
				int interval = wormIndex == 0 ? 10 : 13;
				if (shardBurstTimer >= interval) {
					SpawnShardBurst(target, 7, 6, 18.5f, 21.5f, 260f);
					shardBurstTimer = 0;
				}
			}
		}

		private void SteerTowards(Vector2 targetPosition, float moveSpeed, float turnRate) {
			Vector2 offset = targetPosition - NPC.Center;
			Vector2 desiredVelocity = (offset == Vector2.Zero ? Vector2.UnitY : Vector2.Normalize(offset)) * moveSpeed;
			NPC.velocity = Vector2.Lerp(NPC.velocity, desiredVelocity, turnRate);
			if (NPC.velocity.Length() > moveSpeed) {
				NPC.velocity = NPC.velocity.SafeNormalize(Vector2.Zero) * moveSpeed;
			}
		}

		private void EndEncounterImmediately() {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			CrystalineDevourerArenaSystem.ShutdownEncounter();
			DeactivateEncounterNPCs();
			DeactivateEncounterProjectiles();
		}

		private static void DeactivateEncounterNPCs() {
			int headType = ModContent.NPCType<CrystalineDevourerHead>();
			int bodyType = ModContent.NPCType<CrystalineDevourerBody>();
			int tailType = ModContent.NPCType<CrystalineDevourerTail>();
			int shardType = ModContent.NPCType<CrystalineShard>();

			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (!npc.active) {
					continue;
				}

				if (npc.type != headType && npc.type != bodyType && npc.type != tailType && npc.type != shardType) {
					continue;
				}

				npc.active = false;
				npc.netUpdate = true;
				if (Main.netMode == NetmodeID.Server) {
					NetMessage.SendData(MessageID.SyncNPC, number: i);
				}
			}
		}

		private static void DeactivateEncounterProjectiles() {
			int beamType = ModContent.ProjectileType<CrystalineDevourerBeam>();
			int skyBeamType = ModContent.ProjectileType<CrystalineDevourerSkyBeam>();
			int portalType = ModContent.ProjectileType<CrystalineDevourerPortal>();

			for (int i = 0; i < Main.maxProjectiles; i++) {
				Projectile projectile = Main.projectile[i];
				if (!projectile.active) {
					continue;
				}

				if (projectile.type != beamType && projectile.type != skyBeamType && projectile.type != portalType) {
					continue;
				}

				projectile.Kill();
			}
		}

		private void UpdateCombatStats() {
			NPC.defense = CurrentPhase >= 6 ? 10 : 5;
			NPC.damage = CurrentPhase switch {
				1 => 280,
				2 => 300,
				3 => 325,
				4 => 350,
				5 => 385,
				_ => 430
			};
		}

		private float GetMoveSpeed() {
			return CurrentPhase switch {
				1 => 10f,
				2 => 14f,
				3 => 18f,
				4 => 24f,
				5 => 30f,
				_ => 66f
			};
		}

		private float GetDashSpeed() {
			return CurrentPhase switch {
				4 => 34f,
				5 => 42f,
				_ => 82f
			};
		}

		private float GetTurnRate() {
			return CurrentPhase switch {
				1 => 0.085f,
				2 => 0.11f,
				3 => 0.14f,
				4 => 0.18f,
				5 => 0.23f,
				_ => 0.34f
			};
		}

		private float GetOrbitAngularSpeed() {
			return CurrentPhase switch {
				1 => 0.02f,
				2 => 0.028f,
				3 => 0.036f,
				4 => 0.045f,
				5 => 0.058f,
				_ => 0.09f
			};
		}

		private float GetOrbitSeparation() {
			return CurrentPhase switch {
				1 => 820f,
				2 => 900f,
				3 => 980f,
				4 => 1040f,
				5 => 1100f,
				_ => 1200f
			};
		}

		private float GetDashSpread() {
			return CurrentPhase switch {
				4 => 520f,
				5 => 620f,
				_ => 780f
			};
		}

		private void UpdateRotation() {
			if (NPC.ai[0] > 0f && NPC.ai[0] < Main.maxNPCs) {
				NPC nextSegment = Main.npc[(int)NPC.ai[0]];
				if (nextSegment.active) {
					Vector2 bodyDirection = nextSegment.Center - NPC.Center;
					if (bodyDirection.LengthSquared() > 0.1f) {
						float desiredRotation = bodyDirection.ToRotation() - MathHelper.PiOver2;
						NPC.rotation = desiredRotation;
						return;
					}
				}
			}

			if (NPC.velocity.LengthSquared() > 0.1f) {
				NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
			}
		}

		private void UpdateOpacity() {
			NPC.Opacity = 1f;
		}

		private void SpawnSkyBeam(Player target) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			float ratio = SharedLifeRatio;
			int beamCount = ratio <= 0.10f ? 6 : ratio <= 0.18f ? 5 : 4;
			Vector2 arenaCenter = CrystalineDevourerArenaSystem.IsActive ? CrystalineDevourerArenaSystem.Center : target.Center;
			float horizontalSpread = CrystalineDevourerArenaSystem.IsActive ? CrystalineDevourerArenaSystem.HalfWidth - 320f : 1100f;
			float verticalSpread = CrystalineDevourerArenaSystem.IsActive ? CrystalineDevourerArenaSystem.HalfHeight - 260f : 420f;

			for (int i = 0; i < beamCount; i++) {
				Vector2 beamCenter = arenaCenter + new Vector2(
					Main.rand.NextFloat(-horizontalSpread, horizontalSpread),
					Main.rand.NextFloat(-verticalSpread, verticalSpread));
				Vector2 beamDirection = Vector2.UnitY.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-72f, 72f)));
				Projectile.NewProjectile(NPC.GetSource_FromAI(), beamCenter, beamDirection, ModContent.ProjectileType<CrystalineDevourerSkyBeam>(), 580, 0f, Main.myPlayer, SupremeSkyBeamTelegraph, SupremeSkyBeamDuration);
			}

			SoundEngine.PlaySound(SoundID.Item122 with { Pitch = -0.2f, Volume = 1.2f }, target.Center);
		}

		private int GetNextSkyBeamDelay() {
			return SharedLifeRatio switch {
				<= 0.10f => Main.rand.Next(55, 90),
				<= 0.18f => Main.rand.Next(75, 110),
				_ => Main.rand.Next(95, 135)
			};
		}

		private void UpdateProjectilePressure(Player target) {
			if (Main.netMode == NetmodeID.MultiplayerClient || CurrentPhase <= 1 || CurrentPhase >= 6) {
				return;
			}

			pressureBurstTimer++;
			int interval = CurrentPhase switch {
				2 => 78,
				3 => 60,
				4 => 42,
				_ => 30
			};

			if (pressureBurstTimer < interval) {
				return;
			}

			pressureBurstTimer = 0;
			switch (CurrentPhase) {
				case 2:
					SpawnShardBurst(target, 3, 0, 12f, 0f, 180f);
					break;
				case 3:
					SpawnShardBurst(target, 4, 2, 13.5f, 15f, 185f);
					break;
				case 4:
					SpawnShardBurst(target, 5, 3, 15f, 16.5f, 195f);
					break;
				case 5:
					SpawnShardBurst(target, 6, 4, 16.5f, 18f, 210f);
					break;
			}
		}

		private void SpawnShardBurst(Player target, int primaryCount = 7, int secondaryCount = 5, float primarySpeed = 13.5f, float secondarySpeed = 15.5f, float lifetime = 210f) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			Vector2 offset = target.Center - NPC.Center;
			Vector2 direction = offset == Vector2.Zero ? Vector2.UnitY : Vector2.Normalize(offset);
			float stagger = (phaseTimer / 12 % 2 == 0) ? 5f : -5f;
			int primaryHalf = primaryCount / 2;
			for (int i = -primaryHalf; i <= primaryHalf; i++) {
				Vector2 velocity = direction.RotatedBy(MathHelper.ToRadians(11f * i + stagger)) * (primarySpeed + Math.Abs(i) * 0.6f);
				int shard = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CrystalineShard>(), Target: 0);
				Main.npc[shard].velocity = velocity;
				Main.npc[shard].ai[0] = target.whoAmI;
				Main.npc[shard].ai[1] = lifetime;
				Main.npc[shard].netUpdate = true;
			}

			int secondaryHalf = secondaryCount / 2;
			for (int i = -secondaryHalf; i <= secondaryHalf && secondaryCount > 0; i++) {
				Vector2 velocity = direction.RotatedBy(MathHelper.ToRadians((11f * i) + 5.5f - stagger)) * (secondarySpeed + Math.Abs(i) * 0.75f);
				int shard = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CrystalineShard>(), Target: 0);
				Main.npc[shard].velocity = velocity;
				Main.npc[shard].ai[0] = target.whoAmI;
				Main.npc[shard].ai[1] = Math.Max(30f, lifetime - 15f);
				Main.npc[shard].netUpdate = true;
			}
		}

		private int GetCombinedLife() {
			int total = NPC.active ? Math.Max(0, NPC.life) : 0;
			if (TwinIsAlive) {
				total += Math.Max(0, TwinNPC.life);
			}

			return total;
		}

		private int GetCombinedLifeMax() {
			return SharedLifeMax;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrystalineTear>(), 1, 250, 250));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrystalineSword>(), 40, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<global::ChaoticDimensions.Content.Items.Accessories.CrystalineEye>(), 20, 1, 1));
		}

		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(initialized);
			writer.Write(wormIndex);
			writer.Write(twinHeadIndex);
			writer.Write(hasSpawnedTwin);
			writer.Write(phaseTimer);
			writer.Write((int)attackState);
			writer.Write(orbitAngle);
			writer.Write(laserActorIndex);
			writer.Write(sideSign);
			writer.Write(dashTimer);
			writer.WriteVector2(dashVelocity);
			writer.WriteVector2(cachedBeamDirection);
			writer.Write(shardBurstTimer);
			writer.Write(pressureBurstTimer);
			writer.Write(pressureRetargetTimer);
			writer.WriteVector2(pressureTargetPosition);
			writer.Write(skyBeamTimer);
		}

		public override void ReceiveExtraAI(BinaryReader reader) {
			initialized = reader.ReadBoolean();
			wormIndex = reader.ReadInt32();
			twinHeadIndex = reader.ReadInt32();
			hasSpawnedTwin = reader.ReadBoolean();
			phaseTimer = reader.ReadInt32();
			attackState = (CrystalineAttackState)reader.ReadInt32();
			orbitAngle = reader.ReadSingle();
			laserActorIndex = reader.ReadInt32();
			sideSign = reader.ReadInt32();
			dashTimer = reader.ReadInt32();
			dashVelocity = reader.ReadVector2();
			cachedBeamDirection = reader.ReadVector2();
			shardBurstTimer = reader.ReadInt32();
			pressureBurstTimer = reader.ReadInt32();
			pressureRetargetTimer = reader.ReadInt32();
			pressureTargetPosition = reader.ReadVector2();
			skyBeamTimer = reader.ReadInt32();
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) {
			return ControlsState ? null : false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Texture2D texture = TextureAssets.Npc[Type].Value;
			Vector2 forward = CrystalineDevourerSegmentVisuals.GetHeadForward(NPC);
			float drawRotation = forward.ToRotation() + MathHelper.PiOver2;
			Vector2 drawPosition = NPC.Center - screenPos + forward * (HeadDrawForwardOffset + 2f);
			Vector2 origin = texture.Size() * 0.5f;
			float speedStretch = MathHelper.Clamp(NPC.velocity.Length() / 90f, 0f, 0.08f);
			Vector2 drawScale = new(1.04f, 1.04f + speedStretch);
			spriteBatch.Draw(texture, drawPosition, NPC.frame, NPC.GetAlpha(drawColor), drawRotation, origin, drawScale, SpriteEffects.None, 0f);
			return false;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			cooldownSlot = ImmunityCooldownID.Bosses;
			return NPC.Opacity > 0.5f;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
			target.AddBuff(BuffID.Slow, 120);
		}

		public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers) {
			modifiers.SetMaxDamage(Math.Max(1, NPC.life));
		}

		public override void HitEffect(NPC.HitInfo hit) {
			SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 1.15f, Pitch = -0.45f, PitchVariance = 0.08f }, NPC.Center);
		}

		public override void OnKill() {
			if (TwinIsAlive) {
				TwinNPC.boss = true;
			}
		}

		public override bool CheckActive() => false;
	}
}
