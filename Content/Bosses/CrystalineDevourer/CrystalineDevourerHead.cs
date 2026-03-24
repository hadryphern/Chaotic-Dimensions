using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Bosses.CrystalineDevourer
{
	internal enum CrystalineAttackState
	{
		Orbit,
		Dash,
		SupremeLaser,
		SupremePortals
	}

	public sealed class CrystalineDevourerHead : ModNPC
	{
		public override string Texture => "Terraria/Images/NPC_134";

		public const int SharedLifeMax = 987000;
		private const int MinSegments = 48;
		private const int MaxSegments = 54;
		private const int OrbitDuration = 180;
		private const int DashDuration = 34;
		private const int DashCooldown = 40;
		private const int SupremeLaserTelegraph = 60;
		private const int SupremeLaserDuration = 300;
		private const int SupremePortalDuration = 120;
		private const float OrbitSeparation = 210f;

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

		private bool IsLeader => wormIndex == 0;
		private bool TwinIsAlive => twinHeadIndex >= 0 && twinHeadIndex < Main.maxNPCs && Main.npc[twinHeadIndex].active && Main.npc[twinHeadIndex].type == Type;
		private NPC TwinNPC => TwinIsAlive ? Main.npc[twinHeadIndex] : null;
		private CrystalineDevourerHead Leader => IsLeader ? this : (TwinIsAlive ? TwinNPC.ModNPC as CrystalineDevourerHead : this);
		private float SharedLifeRatio => NPC.life / (float)Math.Max(1, NPC.lifeMax);

		private int CurrentPhase {
			get {
				float ratio = Leader.NPC.life / (float)Math.Max(1, Leader.NPC.lifeMax);
				if (ratio > 0.85f)
					return 1;
				if (ratio > 0.75f)
					return 2;
				if (ratio > 0.65f)
					return 3;
				if (ratio > 0.5f)
					return 4;
				if (ratio > 0.35f)
					return 5;
				return 6;
			}
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1;
			NPCID.Sets.MustAlwaysDraw[Type] = true;
		}

		public override void SetDefaults() {
			NPC.width = 112;
			NPC.height = 112;
			NPC.damage = 175;
			NPC.defense = 5;
			NPC.lifeMax = SharedLifeMax;
			NPC.knockBackResist = 0f;
			NPC.value = Item.buyPrice(platinum: 1, gold: 50);
			NPC.npcSlots = 12f;
			NPC.boss = true;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.BossBar = Main.BigBossProgressBar.NeverValid;
			Music = MusicID.Boss3;
		}

		public override bool PreAI() {
			InitializeIfNeeded();
			SpawnSegmentsIfNeeded();
			SpawnTwinIfNeeded();
			SyncSharedLife();
			TargetOrDespawn();

			Player target = Main.player[NPC.target];
			if (!target.active || target.dead) {
				return false;
			}

			if (IsLeader) {
				RunLeaderState(target);
			}

			UpdateMovement(target);
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

			for (int i = 0; i < bodySegments; i++) {
				int segment = NPC.NewNPC(source, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CrystalineDevourerBody>(), NPC.whoAmI);
				Main.npc[segment].realLife = NPC.whoAmI;
				Main.npc[segment].ai[1] = latest;
				Main.npc[segment].ai[2] = NPC.whoAmI;
				Main.npc[latest].ai[0] = segment;
				latest = segment;
			}

			int tail = NPC.NewNPC(source, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CrystalineDevourerTail>(), NPC.whoAmI);
			Main.npc[tail].realLife = NPC.whoAmI;
			Main.npc[tail].ai[1] = latest;
			Main.npc[tail].ai[2] = NPC.whoAmI;
			Main.npc[latest].ai[0] = tail;
			NPC.netUpdate = true;
		}

		private void SpawnTwinIfNeeded() {
			if (!IsLeader || hasSpawnedTwin || Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int twin = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 120, Type, NPC.whoAmI);
			NPC twinNpc = Main.npc[twin];
			twinNpc.ai[2] = 1f;
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
			if (!TwinIsAlive) {
				return;
			}

			NPC twin = TwinNPC;
			if (IsLeader) {
				int sharedLife = Math.Min(NPC.life, twin.life);
				NPC.life = sharedLife;
				twin.life = sharedLife;
				if (sharedLife <= 0) {
					NPC.active = false;
					twin.active = false;
				}
			}
			else if (Leader != null && Leader != this) {
				NPC.life = Leader.NPC.life;
			}
		}

		private void TargetOrDespawn() {
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

			int supremeCycle = SupremeLaserTelegraph + SupremeLaserDuration + SupremePortalDuration;
			int supremeTimer = phaseTimer % supremeCycle;
			if (supremeTimer == 0) {
				laserActorIndex = 1 - laserActorIndex;
				sideSign *= -1;
			}

			if (supremeTimer < SupremeLaserTelegraph + SupremeLaserDuration) {
				attackState = CrystalineAttackState.SupremeLaser;
				if (supremeTimer == 10) {
					SpawnBeam(target);
				}
			}
			else {
				if (attackState != CrystalineAttackState.SupremePortals) {
					PrepareDash(target);
				}

				attackState = CrystalineAttackState.SupremePortals;
				if (supremeTimer % 40 == 0) {
					SpawnPortalPair(target, supremeTimer);
				}
			}
		}

		private void PrepareDash(Player target) {
			Vector2 aim = target.Center - NPC.Center;
			if (aim == Vector2.Zero) {
				aim = Vector2.UnitY;
			}

			dashVelocity = aim.SafeNormalize(Vector2.UnitY) * GetDashSpeed();
			dashTimer = DashDuration;
			cachedBeamDirection = dashVelocity.SafeNormalize(Vector2.UnitX);
		}

		private void UpdateMovement(Player target) {
			var leader = Leader ?? this;
			CrystalineAttackState state = leader.attackState;
			if (!IsLeader && leader != this && !leader.NPC.active) {
				NPC.active = false;
				return;
			}

			if (state == CrystalineAttackState.Dash && leader.dashTimer > 0) {
				DoDashMovement(leader);
				return;
			}

			if (state == CrystalineAttackState.SupremeLaser) {
				DoSupremeLaserMovement(target, leader);
				return;
			}

			if (state == CrystalineAttackState.SupremePortals) {
				DoSupremePortalMovement(target, leader);
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
			float radius = MathHelper.Lerp(520f, 330f, 1f - leader.SharedLifeRatio);
			float signedOffset = wormIndex == 0 ? OrbitSeparation : -OrbitSeparation;
			Vector2 targetPosition = target.Center + orbitDirection * radius + tangent * signedOffset;
			lastTargetPosition = targetPosition;
			SteerTowards(targetPosition, GetMoveSpeed(), GetTurnRate());
		}

		private void DoDashMovement(CrystalineDevourerHead leader) {
			NPC.velocity = leader.dashVelocity;
			if (leader.IsLeader) {
				leader.dashTimer--;
			}
		}

		private void DoSupremeLaserMovement(Player target, CrystalineDevourerHead leader) {
			int timer = leader.phaseTimer % (SupremeLaserTelegraph + SupremeLaserDuration + SupremePortalDuration);
			bool isLaserActor = wormIndex == leader.laserActorIndex;
			if (isLaserActor) {
				Vector2 lockPosition = target.Center + new Vector2(leader.sideSign * 580f, -140f);
				lastTargetPosition = lockPosition;
				SteerTowards(lockPosition, 17f, 0.18f);
				if (timer >= SupremeLaserTelegraph) {
					NPC.velocity *= 0.9f;
				}
			}
			else {
				Vector2 orbitDirection = Vector2.UnitX.RotatedBy(leader.orbitAngle + MathHelper.PiOver2);
				Vector2 strikePoint = target.Center + orbitDirection * 280f;
				SteerTowards(strikePoint, 23f, 0.22f);
				if (Main.netMode != NetmodeID.MultiplayerClient) {
					shardBurstTimer++;
					if (shardBurstTimer >= 24) {
						SpawnShardBurst(target);
						shardBurstTimer = 0;
					}
				}
			}
		}

		private void DoSupremePortalMovement(Player target, CrystalineDevourerHead leader) {
			int timer = leader.phaseTimer % (SupremeLaserTelegraph + SupremeLaserDuration + SupremePortalDuration);
			int localTimer = timer - (SupremeLaserTelegraph + SupremeLaserDuration);
			if (localTimer < 0) {
				localTimer = 0;
			}

			float angleOffset = wormIndex == 0 ? 0f : MathHelper.Pi;
			Vector2 portalPoint = target.Center + Vector2.UnitX.RotatedBy(Main.GlobalTimeWrappedHourly * 2f + angleOffset) * 700f;
			if (localTimer < 30) {
				SteerTowards(portalPoint, 24f, 0.3f);
			}
			else {
				Vector2 dashDirection = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
				NPC.velocity = dashDirection * (GetDashSpeed() + 6f);
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

		private float GetMoveSpeed() {
			return CurrentPhase switch {
				1 => 8f,
				2 => 10.5f,
				3 => 13f,
				4 => 16f,
				5 => 18.5f,
				_ => 21.5f
			};
		}

		private float GetDashSpeed() {
			return CurrentPhase switch {
				4 => 24f,
				5 => 29f,
				_ => 34f
			};
		}

		private float GetTurnRate() {
			return CurrentPhase switch {
				1 => 0.07f,
				2 => 0.085f,
				3 => 0.105f,
				4 => 0.13f,
				5 => 0.155f,
				_ => 0.18f
			};
		}

		private float GetOrbitAngularSpeed() {
			return CurrentPhase switch {
				1 => 0.018f,
				2 => 0.023f,
				3 => 0.028f,
				4 => 0.032f,
				5 => 0.037f,
				_ => 0.045f
			};
		}

		private void UpdateRotation() {
			if (NPC.velocity.LengthSquared() > 0.1f) {
				NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
			}
		}

		private void UpdateOpacity() {
			NPC.Opacity = 1f;
		}

		private void SpawnBeam(Player target) {
			if (Main.netMode == NetmodeID.MultiplayerClient || !TwinIsAlive) {
				return;
			}

			NPC actor = laserActorIndex == wormIndex ? NPC : TwinNPC;
			Vector2 beamDirection = (target.Center - actor.Center).SafeNormalize(new Vector2(-sideSign, 0f));
			cachedBeamDirection = beamDirection;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), actor.Center, beamDirection, ModContent.ProjectileType<CrystalineDevourerBeam>(), 210, 0f, Main.myPlayer, actor.whoAmI, SupremeLaserTelegraph, SupremeLaserDuration);
			SoundEngine.PlaySound(SoundID.Item122, actor.Center);
		}

		private void SpawnPortalPair(Player target, int timer) {
			if (Main.netMode == NetmodeID.MultiplayerClient || !TwinIsAlive) {
				return;
			}

			Vector2 offsetA = new Vector2(560f * sideSign, -200f).RotatedBy(timer * 0.02f);
			Vector2 offsetB = new Vector2(-560f * sideSign, 200f).RotatedBy(-timer * 0.02f);
			Projectile.NewProjectile(NPC.GetSource_FromAI(), target.Center + offsetA, Vector2.Zero, ModContent.ProjectileType<CrystalineDevourerPortal>(), 0, 0f, Main.myPlayer, 0f, 0f);
			Projectile.NewProjectile(NPC.GetSource_FromAI(), target.Center + offsetB, Vector2.Zero, ModContent.ProjectileType<CrystalineDevourerPortal>(), 0, 0f, Main.myPlayer, 1f, 0f);
			SoundEngine.PlaySound(SoundID.Item8, target.Center);
		}

		private void SpawnShardBurst(Player target) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			Vector2 offset = target.Center - NPC.Center;
			Vector2 direction = offset == Vector2.Zero ? Vector2.UnitY : Vector2.Normalize(offset);
			for (int i = -2; i <= 2; i++) {
				Vector2 velocity = direction.RotatedBy(MathHelper.ToRadians(10f * i)) * 10f;
				int shard = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CrystalineShard>(), Target: 0);
				Main.npc[shard].velocity = velocity;
				Main.npc[shard].ai[0] = target.whoAmI;
				Main.npc[shard].ai[1] = 210f;
				Main.npc[shard].netUpdate = true;
			}
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
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) {
			return IsLeader ? null : false;
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
			if (NPC.life <= 0 && TwinIsAlive) {
				TwinNPC.active = false;
			}
		}

		public override void OnKill() {
			if (TwinIsAlive) {
				TwinNPC.active = false;
			}
		}

		public override bool CheckActive() => false;
	}
}
