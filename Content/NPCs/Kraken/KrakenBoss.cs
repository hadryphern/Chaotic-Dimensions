using ChaoticDimensions.Common.Systems;
using ChaoticDimensions.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.NPCs.Kraken
{
	[AutoloadBossHead]
	public class KrakenBoss : ModNPC
	{
		private const int StateChase = 0;
		private const int StateEyeRush = 1;
		private const int StateLaserSpin = 2;
		private const int StateTrackingLaser = 3;
		private const int StateHypnosis = 4;
		private const int StateDashChain = 5;
		private const int StateInvisibleSwarm = 6;
		private const int VisualMotionIdle = 0;
		private const int VisualMotionHorizontal = 1;
		private const int VisualMotionUp = 2;
		public const int AnimationFrames = 36;
		public const int ForwardAnimationFrames = 36;
		public const int LoopAnimationFrames = 72;
		public const int AtlasColumns = 6;
		public const int AtlasRows = 6;
		public const string LoopBackTexturePath = "ChaoticDimensions/Content/NPCs/Kraken/KrakenBossLoopBack";
		public const float BaseVisualScale = 1.44f;
		public const float Phase2VisualScale = 1.54f;
		public const float Phase3VisualScale = 1.62f;
		public const float VisualDrawOffsetY = 82f;
		private const float RubySourceScale = 0.95f;
		private static readonly Vector2 RubyFrameOffset = new Vector2(-31f, -388f);

		private int visualMotion;
		private int previousVisualMotion;
		private float visualMotionBlend = 1f;
		private SpriteEffects visualEffects;
		private SpriteEffects previousVisualEffects;

		private ref float State => ref NPC.ai[0];
		private ref float StateTimer => ref NPC.ai[1];
		private ref float AttackPattern => ref NPC.ai[2];
		private ref float MinionGroupTimer => ref NPC.ai[3];
		private ref float AttackTimer => ref NPC.localAI[0];
		private ref float CloneCooldown => ref NPC.localAI[1];
		private ref float HadClones => ref NPC.localAI[2];
		private ref float InkTimer => ref NPC.localAI[3];
		public override string BossHeadTexture => "ChaoticDimensions/Content/NPCs/Kraken/KrakenBoss_Head_Boss";

		private float BattleRamp {
			get {
				float timeRamp = MathHelper.Clamp((KrakenEventSystem.Instance.Timer - KrakenEventSystem.BattleStart) / (240f * 60f), 0f, 1f);
				float lifeRamp = NPC.lifeMax <= 0 ? 0f : 1f - NPC.life / (float)NPC.lifeMax;
				return MathHelper.Clamp(System.Math.Max(timeRamp, lifeRamp), 0f, 1f);
			}
		}

		private int CurrentPhase {
			get {
				float lifeRatio = NPC.lifeMax <= 0 ? 1f : NPC.life / (float)NPC.lifeMax;
				if (lifeRatio <= 0.15f) {
					return 3;
				}

				return lifeRatio <= 0.5f ? 2 : 1;
			}
		}

		private bool Phase2 => CurrentPhase >= 2;
		private bool Phase3 => CurrentPhase >= 3;
		private bool RubyBroken => NPC.lifeMax > 0 && NPC.life <= NPC.lifeMax * 0.15f;
		private bool RubyOff => !RubyBroken
			&& KrakenEventSystem.Instance.Timer < KrakenEventSystem.BattleStart + 50;

		public static float GetVisualScaleForLife(NPC npc) {
			if (npc.lifeMax > 0 && npc.life <= npc.lifeMax * 0.15f) {
				return Phase3VisualScale;
			}

			return npc.lifeMax > 0 && npc.life <= npc.lifeMax * 0.5f ? Phase2VisualScale : BaseVisualScale;
		}

		public static Vector2 GetRubyFrameOffset() {
			return RubyFrameOffset;
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);
			NPCID.Sets.TrailCacheLength[Type] = 14;
			NPCID.Sets.TrailingMode[Type] = 0;
		}

		public override void SetDefaults() {
			NPC.width = 270;
			NPC.height = 640;
			NPC.damage = 235;
			NPC.defense = 60;
			NPC.lifeMax = 85000000;
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.boss = true;
			NPC.npcSlots = 20f;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.Roar;
			NPC.value = Item.buyPrice(platinum: 1);

			Music = 0;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			cooldownSlot = ImmunityCooldownID.Bosses;
			return true;
		}

		public override void OnKill() {
			ChaoticDownedBossSystem.downedKraken = true;
			if (Main.netMode == NetmodeID.Server) {
				NetMessage.SendData(MessageID.WorldData);
			}
		}

		public override void AI() {
			TargetPlayer();
			if (NPC.target < 0 || NPC.target >= Main.maxPlayers) {
				NPC.velocity.Y -= 0.3f;
				NPC.EncourageDespawn(60);
				return;
			}

			Player player = Main.player[NPC.target];
			if (!KrakenEventSystem.Instance.Active) {
				KrakenEventSystem.Instance.StartEvent(player);
				KrakenEventSystem.Instance.SpawnedKraken = true;
				KrakenEventSystem.Instance.Timer = KrakenEventSystem.BattleStart;
			}

			bool phase2 = Phase2;
			NPC.defense = Phase3 ? 117 : phase2 ? 90 : 60;
			NPC.damage = Phase3 ? 345 : phase2 ? 290 : 235;

			if (player.dead || !player.active) {
				NPC.TargetClosest(false);
				player = Main.player[NPC.target];
				if (player.dead || !player.active) {
					NPC.velocity.Y -= 0.3f;
					NPC.EncourageDespawn(60);
					return;
				}
			}

			if (State == StateEyeRush) {
				DoEyeRush(player, phase2);
			}
			else if (State == StateLaserSpin) {
				DoLaserSpin(player, phase2);
			}
			else if (State == StateTrackingLaser) {
				DoTrackingLaser(player);
			}
			else if (State == StateHypnosis) {
				DoHypnosis(player);
			}
			else if (State == StateDashChain) {
				DoDashChain(player);
			}
			else if (State == StateInvisibleSwarm) {
				DoInvisibleSwarm(player);
			}
			else {
				DoChase(player, phase2);
				RunAttackPattern(player, phase2);
				int passiveLightningInterval = Phase3 ? 120 : phase2 ? 180 : 360;
				if (Main.netMode != NetmodeID.MultiplayerClient && KrakenEventSystem.Instance.Timer % passiveLightningInterval == 0) {
					SpawnLightning(player, phase2);
				}
			}

			UpdateVisualMotion();
			NPC.rotation = NPC.velocity.X * 0.004f;
			NPC.alpha = State == StateEyeRush || State == StateDashChain ? 190 : State == StateInvisibleSwarm ? 225 : 0;
		}

		public override void FindFrame(int frameHeight) {
			if (State == StateEyeRush) {
				NPC.frame.Y = 0;
				return;
			}

			float frameStep = State == StateLaserSpin || State == StateTrackingLaser || State == StateHypnosis ? 0.19f : Phase3 ? 0.36f : Phase2 ? 0.29f : 0.24f;
			NPC.frameCounter += frameStep;
			if (NPC.frameCounter >= LoopAnimationFrames) {
				NPC.frameCounter -= LoopAnimationFrames;
			}

			NPC.frame.Y = 0;
		}

		private void UpdateVisualMotion() {
			float horizontalSpeed = System.Math.Abs(NPC.velocity.X);
			float verticalSpeed = System.Math.Abs(NPC.velocity.Y);
			int targetMotion = VisualMotionIdle;
			SpriteEffects targetEffects = SpriteEffects.None;

			if (NPC.velocity.Y < -5.5f && verticalSpeed > horizontalSpeed * 0.72f) {
				targetMotion = VisualMotionUp;
			}
			else if (horizontalSpeed > 5.5f) {
				targetMotion = VisualMotionHorizontal;
				targetEffects = NPC.velocity.X < 0f ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			}

			if (targetMotion != visualMotion || targetEffects != visualEffects) {
				previousVisualMotion = visualMotion;
				previousVisualEffects = visualEffects;
				visualMotion = targetMotion;
				visualEffects = targetEffects;
				visualMotionBlend = 0f;
			}

			visualMotionBlend = MathHelper.Clamp(visualMotionBlend + 0.08f, 0f, 1f);
		}

		private void TargetPlayer() {
			if (NPC.target < 0
				|| NPC.target >= Main.maxPlayers
				|| Main.player[NPC.target].dead
				|| !Main.player[NPC.target].active) {
				NPC.TargetClosest();
			}
		}

		private void DoChase(Player player, bool phase2) {
			float ramp = BattleRamp;
			float speed = Phase3 ? MathHelper.Lerp(60f, 78f, ramp) : phase2 ? MathHelper.Lerp(48f, 70f, ramp) : MathHelper.Lerp(27f, 42f, ramp);
			float inertia = Phase3 ? MathHelper.Lerp(7f, 4.8f, ramp) : phase2 ? MathHelper.Lerp(9.5f, 5.8f, ramp) : MathHelper.Lerp(18f, 11f, ramp);
			Vector2 desired = player.Center + new Vector2(player.Center.X < NPC.Center.X ? 610f : -610f, phase2 ? -20f : -100f);
			Vector2 direction = desired - NPC.Center;

			if (direction.Length() > speed) {
				direction.Normalize();
				direction *= speed;
			}

			NPC.velocity = (NPC.velocity * (inertia - 1f) + direction) / inertia;
		}

		private void DoEyeRush(Player player, bool phase2) {
			StateTimer++;

			if (StateTimer == 1) {
				NPC.velocity = Vector2.Zero;
			}

			if (StateTimer < 34) {
				Vector2 hover = player.Center + new Vector2(player.velocity.X * 18f, -330f);
				NPC.Center = Vector2.Lerp(NPC.Center, hover, 0.058f + BattleRamp * 0.018f);
				return;
			}

			if (StateTimer == 34) {
				Vector2 direction = player.Center - NPC.Center;
				if (direction == Vector2.Zero) {
					direction = Vector2.UnitY;
				}

				direction.Normalize();
				NPC.velocity = direction * (phase2 ? MathHelper.Lerp(100f, 128f, BattleRamp) : MathHelper.Lerp(72f, 96f, BattleRamp));
				SoundEngine.PlaySound(SoundID.Item122, NPC.Center);
				KrakenEventSystem.Instance.AddShake(12, phase2 ? 8f : 6f);
			}

			if (StateTimer > 82) {
				State = StateChase;
				StateTimer = 0;
				AdvanceAttackPattern(phase2);
				NPC.netUpdate = true;
			}
		}

		private void DoLaserSpin(Player player, bool phase2) {
			StateTimer++;
			NPC.velocity = Vector2.Zero;
			KrakenEventSystem.Instance.FocusCamera(NPC.Center + new Vector2(0f, VisualDrawOffsetY - 90f), 3, 1.06f);

			if (StateTimer == 1f) {
				SpawnRotatingLasers(phase2);
				SoundEngine.PlaySound(SoundID.Item122, NPC.Center);
			}

			if (StateTimer >= 60f && StateTimer <= 780f && StateTimer % (phase2 ? 20f : 24f) == 0f) {
				SpawnLaserDropBurst(phase2);
			}

			if (StateTimer % 10f == 0f) {
				KrakenEventSystem.Instance.AddShake(7, phase2 ? 16f : 13f);
			}

			if (StateTimer > 900f) {
				State = StateChase;
				StateTimer = 0f;
				AdvanceAttackPattern(phase2);
				NPC.netUpdate = true;
			}
		}

		private void DoTrackingLaser(Player player) {
			StateTimer++;
			NPC.velocity = Vector2.Zero;
			KrakenEventSystem.Instance.FocusCamera(NPC.Center + new Vector2(0f, VisualDrawOffsetY - 100f), 3, 1.05f);

			if (StateTimer == 1f && Main.netMode != NetmodeID.MultiplayerClient) {
				Projectile.NewProjectile(NPC.GetSource_FromAI(), KrakenRotatingLaser.GetKrakenHead(NPC), Vector2.Zero, ModContent.ProjectileType<KrakenTrackingLaser>(), Phase3 ? 380 : Phase2 ? 320 : 270, 0f, Main.myPlayer, NPC.whoAmI);
				SoundEngine.PlaySound(SoundID.Item122, NPC.Center);
			}

			if (StateTimer % 8f == 0f) {
				KrakenEventSystem.Instance.AddShake(7, Phase3 ? 11f : 8f);
			}

			if (StateTimer > 390f) {
				State = StateChase;
				StateTimer = 0f;
				AdvanceAttackPattern(Phase2);
				NPC.netUpdate = true;
			}
		}

		private void DoHypnosis(Player player) {
			StateTimer++;
			NPC.velocity = Vector2.Zero;
			KrakenEventSystem.Instance.FocusCamera(NPC.Center + new Vector2(0f, VisualDrawOffsetY - 110f), 3, 1.03f);

			if (StateTimer == 1f && Main.netMode != NetmodeID.MultiplayerClient) {
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<KrakenHypnosisVortex>(), 9999, 0f, Main.myPlayer, NPC.whoAmI);
				SoundEngine.PlaySound(SoundID.Item104, NPC.Center);
			}

			if (StateTimer > 620f) {
				State = StateChase;
				StateTimer = 0f;
				AdvanceAttackPattern(Phase2);
				NPC.netUpdate = true;
			}
		}

		private void DoDashChain(Player player) {
			StateTimer++;
			int totalDashes = Phase3 ? 20 : Phase2 ? 18 : 15;
			int dashCycle = Phase3 ? 20 : Phase2 ? 21 : 24;
			int dashIndex = (int)(StateTimer / dashCycle);
			int cycleTick = (int)StateTimer % dashCycle;

			if (dashIndex < totalDashes) {
				if (cycleTick == 1) {
					Vector2 aim = player.Center + player.velocity * (Phase3 ? 10f : 7f) - NPC.Center;
					if (aim.LengthSquared() < 4f) {
						aim = Vector2.UnitY;
					}
					else {
						aim.Normalize();
					}

					NPC.velocity = aim * (Phase3 ? 122f : Phase2 ? 104f : 88f);
					KrakenEventSystem.Instance.AddShake(9, Phase3 ? 10f : 7f);
					SoundEngine.PlaySound(SoundID.Item122, NPC.Center);
				}
				else if (cycleTick > 11) {
					NPC.velocity *= 0.84f;
				}

				return;
			}

			State = StateChase;
			StateTimer = 0f;
			AdvanceAttackPattern(Phase2);
			NPC.netUpdate = true;
		}

		private void DoInvisibleSwarm(Player player) {
			StateTimer++;
			NPC.velocity *= 0.9f;
			Vector2 observationPoint = player.Center + new Vector2(player.Center.X < NPC.Center.X ? 720f : -720f, -180f);
			NPC.Center = Vector2.Lerp(NPC.Center, observationPoint, 0.035f);

			int spawnInterval = Phase3 ? 58 : Phase2 ? 72 : 90;
			if (Main.netMode != NetmodeID.MultiplayerClient && StateTimer % spawnInterval == 1f) {
				SpawnMinions(Phase2, true);
			}

			if (StateTimer > 1800f) {
				State = StateChase;
				StateTimer = 0f;
				AdvanceAttackPattern(Phase2);
				NPC.netUpdate = true;
			}
		}

		private void RunAttackPattern(Player player, bool phase2) {
			AttackTimer++;
			InkTimer++;
			MinionGroupTimer++;
			UpdateCloneCooldown();

			float groupInterval = Phase3 ? 900f : phase2 ? 1200f : 2100f;
			if (MinionGroupTimer >= groupInterval) {
				MinionGroupTimer = 0f;
				SpawnMinions(phase2, false);
			}

			int pattern = (int)AttackPattern % 9;
			if (pattern < 0) {
				pattern += 9;
			}

			switch (pattern) {
				case 0:
					if (AtPatternTick(42, phase2) || AtPatternTick(132, phase2) || AtPatternTick(222, phase2)) {
						SpawnLightning(player, phase2);
						if (AtPatternTick(42, phase2)) {
							SpawnCoordinatedCombo(player, phase2, 0);
						}
					}
					if (AtPatternTick(286, phase2)) {
						SpawnWaterJetVolley(player, phase2);
					}
					if (AtPatternTick(326, phase2)) {
						TryReleaseInk(phase2);
					}
					if (AttackTimer >= PatternDuration(390, phase2)) {
						AdvanceAttackPattern(phase2);
					}
					break;
				case 1:
					if (AtPatternTick(58, phase2)) {
						SpawnDualDropWalls(player, phase2);
						SpawnCoordinatedCombo(player, phase2, 1);
					}
					if (AtPatternTick(154, phase2)) {
						SpawnDropWallFromSide(player, phase2, Main.rand.NextBool() ? -1 : 1);
					}
					if (AtPatternTick(236, phase2)) {
						SpawnVerticalDropWalls(player, phase2);
					}
					if (AtPatternTick(356, phase2)) {
						SpawnVortexWalls(player, phase2);
					}
					if (AttackTimer >= PatternDuration(470, phase2)) {
						AdvanceAttackPattern(phase2);
					}
					break;
				case 2:
					if (AtPatternTick(54, phase2)) {
						SpawnCoordinatedCombo(player, phase2, 2);
						StartTrackingLaser();
					}
					break;
				case 3:
					if (AtPatternTick(56, phase2)) {
						SpawnCoordinatedCombo(player, phase2, 3);
						StartLaserSpin();
					}
					break;
				case 4:
					if (AtPatternTick(58, phase2)) {
						TrySpawnClones(phase2);
						SpawnCoordinatedCombo(player, phase2, 4);
					}
					if (AtPatternTick(126, phase2)) {
						SpawnCrystalTurrets();
					}
					if (AtPatternTick(262, phase2)) {
						SpawnLightning(player, phase2);
					}
					if (AttackTimer >= PatternDuration(420, phase2)) {
						AdvanceAttackPattern(phase2);
					}
					break;
				case 5:
					if (AtPatternTick(70, phase2)) {
						SpawnSkyBeams(player, phase2);
						SpawnCoordinatedCombo(player, phase2, 5);
					}
					if (AtPatternTick(160, phase2)) {
						SpawnDropWallFromSky(player, phase2);
					}
					if (AtPatternTick(250, phase2)) {
						SpawnDualDropWalls(player, phase2);
					}
					if (AttackTimer >= PatternDuration(380, phase2)) {
						AdvanceAttackPattern(phase2);
					}
					break;
				case 6:
					if (AtPatternTick(58, phase2)) {
						SpawnCoordinatedCombo(player, phase2, 6);
						StartHypnosis();
					}
					break;
				case 7:
					if (AtPatternTick(48, phase2)) {
						SpawnCoordinatedCombo(player, phase2, 7);
						StartDashChain();
					}
					break;
				default:
					if (AtPatternTick(62, phase2)) {
						SpawnCoordinatedCombo(player, phase2, 8);
						StartInvisibleSwarm();
					}
					break;
			}
		}

		private bool AtPatternTick(int normalTick, bool phase2) {
			return AttackTimer == PatternTick(normalTick, phase2);
		}

		private int PatternTick(int normalTick, bool phase2) {
			float multiplier = Phase3 ? 0.64f : phase2 ? 0.76f : 1f;
			return System.Math.Max(1, (int)(normalTick * multiplier));
		}

		private int PatternDuration(int normalDuration, bool phase2) {
			float multiplier = Phase3 ? 0.68f : phase2 ? 0.8f : 1f;
			return System.Math.Max(128, (int)(normalDuration * multiplier));
		}

		private void SpawnCoordinatedCombo(Player player, bool phase2, int comboSeed) {
			int passes = phase2 ? 3 : 1;
			for (int pass = 0; pass < passes; pass++) {
				int combo = (comboSeed + pass * 2) % 6;
				switch (combo) {
					case 0:
						SpawnWaterJets(player, phase2);
						SpawnDropWallFromSide(player, phase2, pass % 2 == 0 ? -1 : 1);
						break;
					case 1:
						SpawnWaterJetVolley(player, phase2);
						SpawnSkyBeams(player, phase2);
						break;
					case 2:
						SpawnVortexWalls(player, phase2);
						SpawnWaterJets(player, phase2);
						break;
					case 3:
						SpawnDropWallFromSky(player, phase2);
						SpawnWaterJetVolley(player, phase2);
						break;
					case 4:
						SpawnLightning(player, phase2);
						SpawnWaterJets(player, phase2);
						break;
					default:
						SpawnSkyBeams(player, phase2);
						SpawnDropWallFromSide(player, phase2, pass % 2 == 0 ? 1 : -1);
						break;
				}
			}
		}

		private void AdvanceAttackPattern(bool phase2) {
			AttackTimer = 0f;
			AttackPattern = ((int)AttackPattern + 1) % 9;

			NPC.netUpdate = true;
		}

		private void StartEyeRush() {
			State = StateEyeRush;
			StateTimer = 0f;
			AttackTimer = 0f;
			NPC.netUpdate = true;
			SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
		}

		private void StartLaserSpin() {
			State = StateLaserSpin;
			StateTimer = 0f;
			AttackTimer = 0f;
			NPC.netUpdate = true;
			KrakenEventSystem.Instance.AddShake(18, 13f);
		}

		private void StartTrackingLaser() {
			State = StateTrackingLaser;
			StateTimer = 0f;
			AttackTimer = 0f;
			NPC.netUpdate = true;
			KrakenEventSystem.Instance.AddShake(16, 10f);
		}

		private void StartHypnosis() {
			State = StateHypnosis;
			StateTimer = 0f;
			AttackTimer = 0f;
			NPC.netUpdate = true;
			KrakenEventSystem.Instance.AddShake(24, 12f);
		}

		private void StartDashChain() {
			State = StateDashChain;
			StateTimer = 0f;
			AttackTimer = 0f;
			NPC.netUpdate = true;
			SoundEngine.PlaySound(SoundID.Item92, NPC.Center);
		}

		private void StartInvisibleSwarm() {
			State = StateInvisibleSwarm;
			StateTimer = 0f;
			AttackTimer = 0f;
			NPC.netUpdate = true;
			SoundEngine.PlaySound(SoundID.Item104, NPC.Center);
		}

		private void TryReleaseInk(bool phase2) {
			float cooldown = MathHelper.Lerp(390f, 230f, BattleRamp);
			if (InkTimer < (phase2 ? cooldown * 0.7f : cooldown)) {
				return;
			}

			InkTimer = 0f;
			KrakenEventSystem.Instance.AddInk(phase2 ? 320 : 260);
			SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
		}

		private void SpawnRotatingLasers(bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int amount = phase2 ? 9 : 8;
			int damage = phase2 ? 340 : 270;
			int type = ModContent.ProjectileType<KrakenRotatingLaser>();
			for (int i = 0; i < amount; i++) {
				float angle = MathHelper.TwoPi * i / amount;
				Projectile.NewProjectile(NPC.GetSource_FromAI(), KrakenRotatingLaser.GetKrakenHead(NPC), Vector2.Zero, type, damage, 0f, Main.myPlayer, NPC.whoAmI, angle, phase2 ? 1f : 0f);
			}
		}

		private void SpawnLaserDropBurst(bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int amount = phase2 ? 9 : 8;
			int damage = phase2 ? 166 : 126;
			Vector2 origin = KrakenRotatingLaser.GetKrakenHead(NPC);
			float spin = KrakenRotatingLaser.SpinForTime(StateTimer, phase2);
			float offset = MathHelper.Pi / amount;
			for (int i = 0; i < amount; i++) {
				float angle = spin + offset + MathHelper.TwoPi * i / amount;
				Vector2 velocity = angle.ToRotationVector2() * Main.rand.NextFloat(9.4f, phase2 ? 13.2f : 11.4f);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), origin + velocity * 6f, velocity, ModContent.ProjectileType<KrakenWaterDrop>(), damage, 0f, Main.myPlayer, 0f, 1f);
			}
		}

		private void SpawnDropWallFromSky(Player player, bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int lanes = Phase3 ? 31 : phase2 ? 27 : 23;
			float spacing = Phase3 ? 104f : phase2 ? 112f : 122f;
			int[] gaps = CreateThreeGapLanes(lanes);
			int damage = phase2 ? 185 : 145;
			Vector2 predicted = PredictPlayer(player, phase2 ? 38f : 30f);
			float startX = predicted.X - (lanes - 1) * spacing * 0.5f + Main.rand.NextFloat(-50f, 50f);
			int verticalSide = Main.rand.NextBool(4) ? 1 : -1;
			for (int i = 0; i < lanes; i++) {
				if (IsDropWallGap(i, gaps)) {
					continue;
				}

				Vector2 position = new Vector2(
					startX + i * spacing,
					predicted.Y + verticalSide * Main.rand.NextFloat(1380f, 1680f)
				);
				Vector2 velocity = new Vector2(
					Main.rand.NextFloat(-0.45f, 0.45f),
					-verticalSide * (Phase3 ? 16.4f : phase2 ? 13.8f : 11.4f)
				);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, ModContent.ProjectileType<KrakenWaterDrop>(), damage, 0f, Main.myPlayer, 0f, 1f);
			}
		}

		private void SpawnDropWallFromSide(Player player, bool phase2, int side) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int lanes = Phase3 ? 25 : phase2 ? 21 : 19;
			float spacing = Phase3 ? 98f : phase2 ? 108f : 118f;
			int[] gaps = CreateThreeGapLanes(lanes);
			int damage = phase2 ? 180 : 140;
			Vector2 predicted = PredictPlayer(player, phase2 ? 34f : 26f);
			float startY = predicted.Y - (lanes - 1) * spacing * 0.5f + Main.rand.NextFloat(-30f, 30f);
			float x = predicted.X + side * Main.rand.NextFloat(1500f, 1780f);
			for (int i = 0; i < lanes; i++) {
				if (IsDropWallGap(i, gaps)) {
					continue;
				}

				Vector2 position = new Vector2(x, startY + i * spacing);
				Vector2 velocity = new Vector2(-side * (phase2 ? 12.8f : 10.2f), Main.rand.NextFloat(-0.55f, 0.55f));
				Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, ModContent.ProjectileType<KrakenWaterDrop>(), damage, 0f, Main.myPlayer, 0f, 1f);
			}
		}

		private void SpawnDualDropWalls(Player player, bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int lanes = Phase3 ? 27 : phase2 ? 23 : 21;
			float spacing = Phase3 ? 92f : phase2 ? 102f : 112f;
			int[] gaps = CreateThreeGapLanes(lanes);
			int damage = Phase3 ? 230 : phase2 ? 190 : 150;
			Vector2 predicted = PredictPlayer(player, Phase3 ? 30f : 22f);
			float startY = predicted.Y - (lanes - 1) * spacing * 0.5f;

			for (int side = -1; side <= 1; side += 2) {
				float x = predicted.X + side * 1680f;
				for (int thickness = 0; thickness < 3; thickness++) {
					for (int i = 0; i < lanes; i++) {
						if (IsDropWallGap(i, gaps)) {
							continue;
						}

						Vector2 position = new Vector2(x + side * thickness * 56f, startY + i * spacing + thickness * spacing * 0.42f);
						Vector2 velocity = new Vector2(-side * (Phase3 ? 18f : phase2 ? 15.5f : 13f), 0f);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, ModContent.ProjectileType<KrakenWaterDrop>(), damage, 0f, Main.myPlayer, 0f, 1f);
					}
				}
			}
		}

		private void SpawnVerticalDropWalls(Player player, bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int lanes = Phase3 ? 29 : phase2 ? 25 : 23;
			float spacing = Phase3 ? 94f : phase2 ? 104f : 114f;
			int[] gaps = CreateThreeGapLanes(lanes);
			int damage = Phase3 ? 230 : phase2 ? 188 : 148;
			Vector2 predicted = PredictPlayer(player, Phase3 ? 28f : 20f);
			float startX = predicted.X - (lanes - 1) * spacing * 0.5f;

			for (int vertical = -1; vertical <= 1; vertical += 2) {
				float y = predicted.Y + vertical * 1520f;
				for (int thickness = 0; thickness < 3; thickness++) {
					for (int i = 0; i < lanes; i++) {
						if (IsDropWallGap(i, gaps)) {
							continue;
						}

						Vector2 position = new Vector2(startX + i * spacing + thickness * spacing * 0.38f, y + vertical * thickness * 58f);
						Vector2 velocity = new Vector2(0f, -vertical * (Phase3 ? 17.5f : phase2 ? 15f : 12.5f));
						Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, ModContent.ProjectileType<KrakenWaterDrop>(), damage, 0f, Main.myPlayer, 0f, 1f);
					}
				}
			}
		}

		private static int[] CreateThreeGapLanes(int lanes) {
			int third = lanes / 3;
			return new[] {
				Main.rand.Next(2, System.Math.Max(3, third - 1)),
				Main.rand.Next(System.Math.Max(3, third + 1), System.Math.Max(4, third * 2 - 1)),
				Main.rand.Next(System.Math.Max(5, third * 2 + 1), lanes - 2)
			};
		}

		private static bool IsDropWallGap(int lane, int[] gaps) {
			for (int i = 0; i < gaps.Length; i++) {
				if (System.Math.Abs(lane - gaps[i]) <= 2) {
					return true;
				}
			}

			return false;
		}

		private void SpawnCrystalTurrets() {
			if (Main.netMode == NetmodeID.MultiplayerClient || CountNPC(ModContent.NPCType<KrakenCrystalTurret>()) > 0) {
				return;
			}

			for (int side = -1; side <= 1; side += 2) {
				Vector2 position = NPC.Center + new Vector2(side * 440f, -80f);
				int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)position.X, (int)position.Y, ModContent.NPCType<KrakenCrystalTurret>(), ai0: NPC.whoAmI, ai1: side);
				if (index >= 0 && index < Main.maxNPCs) {
					Main.npc[index].netUpdate = true;
				}
			}
		}

		private void SpawnLightning(Player player, bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int lanes = Phase3 ? 31 : phase2 ? 27 : 9;
			int safeLane = Main.rand.Next(1, lanes - 1);
			float spacing = Phase3 ? 92f : phase2 ? 104f : 156f;
			Vector2 predicted = PredictPlayer(player, Phase3 ? 34f : phase2 ? 30f : 26f);
			float startX = predicted.X - (lanes - 1) * spacing * 0.5f;

			for (int i = 0; i < lanes; i++) {
				if (System.Math.Abs(i - safeLane) <= 1) {
					continue;
				}

				Vector2 position = new Vector2(startX + i * spacing + Main.rand.NextFloat(-24f, 24f), predicted.Y - Main.rand.NextFloat(620f, 820f));
				int damage = Phase3 ? 300 : phase2 ? 255 : 205;
				Projectile.NewProjectile(NPC.GetSource_FromAI(), position, Vector2.Zero, ModContent.ProjectileType<KrakenLightningStrike>(), damage, 0f, Main.myPlayer);
			}

			int backgroundAmount = Phase3 ? 14 : phase2 ? 10 : 5;
			for (int i = 0; i < backgroundAmount; i++) {
				Vector2 position = new Vector2(predicted.X + Main.rand.NextFloat(-1400f, 1400f), predicted.Y - Main.rand.NextFloat(580f, 900f));
				Projectile.NewProjectile(NPC.GetSource_FromAI(), position, Vector2.Zero, ModContent.ProjectileType<KrakenLightningStrike>(), 0, 0f, Main.myPlayer, 0f, 1f);
			}
		}

		private void SpawnHomingLightning(Player player, bool phase2) {
			int amount = phase2 ? 7 : 3;
			int damage = phase2 ? 175 : 130;
			int type = ModContent.ProjectileType<KrakenHomingLightning>();
			for (int i = 0; i < amount; i++) {
				float angle = Main.rand.NextFloat(MathHelper.TwoPi);
				Vector2 radius = new Vector2((float)System.Math.Cos(angle) * Main.rand.NextFloat(620f, 920f), (float)System.Math.Sin(angle) * Main.rand.NextFloat(360f, 620f));
				Vector2 position = PredictPlayer(player, 22f + i * 8f) + radius;
				Vector2 velocity = PredictPlayer(player, 38f + i * 10f) - position;
				if (velocity.LengthSquared() < 4f) {
					velocity = Vector2.UnitY;
				}
				else {
					velocity.Normalize();
				}

				velocity *= phase2 ? 7.8f : 6.6f;
				Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, type, damage, 0f, Main.myPlayer, player.whoAmI, i * 0.2f);
			}
		}

		private void SpawnWaterJets(Player player, bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int amount = phase2 ? 5 : 2;
			int damage = phase2 ? 120 : 92;
			int type = ModContent.ProjectileType<KrakenWaterJet>();
			for (int i = 0; i < amount; i++) {
				int side = Main.rand.NextBool() ? -1 : 1;
				Vector2 position = player.Center + new Vector2(side * Main.rand.NextFloat(620f, 860f), Main.rand.NextFloat(-360f, 260f));
				Vector2 velocity = PredictPlayer(player, 32f + i * 8f) - position;
				if (velocity.LengthSquared() < 4f) {
					velocity = new Vector2(-side, 0f);
				}
				else {
					velocity.Normalize();
				}

				velocity = velocity.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f)) * Main.rand.NextFloat(10.2f, phase2 ? 14.4f : 12.8f);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, type, damage, 0f, Main.myPlayer, 0f, i);
			}
		}

		private void SpawnWaterJetVolley(Player player, bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int amount = phase2 ? 12 : 6;
			int damage = phase2 ? 128 : 98;
			int type = ModContent.ProjectileType<KrakenWaterJet>();
			Vector2 origin = NPC.Center + new Vector2(0f, 40f);
			Vector2 predicted = PredictPlayer(player, phase2 ? 48f : 38f);
			for (int i = 0; i < amount; i++) {
				float spread = MathHelper.Lerp(-0.92f, 0.92f, amount <= 1 ? 0.5f : i / (float)(amount - 1));
				Vector2 direction = predicted - origin;
				if (direction.LengthSquared() < 4f) {
					direction = Vector2.UnitY;
				}
				else {
					direction.Normalize();
				}

				Vector2 velocity = direction.RotatedBy(spread) * Main.rand.NextFloat(9.2f, phase2 ? 14.8f : 12.8f);
				Vector2 position = origin + velocity * 6f;
				Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, type, damage, 0f, Main.myPlayer, 0f, i * 0.15f);
			}
		}

		private void SpawnSkyBeams(Player player, bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int amount = phase2 ? 3 : 1;
			int damage = phase2 ? 360 : 275;
			int type = ModContent.ProjectileType<KrakenSkyBeam>();
			Vector2 predicted = PredictPlayer(player, phase2 ? 56f : 44f);
			for (int i = 0; i < amount; i++) {
				float lane = amount == 1 ? 0f : MathHelper.Lerp(-370f, 370f, i / (float)(amount - 1));
				float x = predicted.X + lane + player.velocity.X * 18f;
				float y = predicted.Y;
				float rotation = MathHelper.Clamp(player.velocity.X * 0.005f, -0.36f, 0.36f) + Main.rand.NextFloat(-0.24f, 0.24f);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(x, y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, rotation);
			}
		}

		private void SpawnTentacleStorm(Player player, bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int amount = phase2 ? 4 : 2;
			for (int i = 0; i < amount; i++) {
				int side = i % 2 == 0 ? -1 : 1;
				Vector2 position = player.Center + new Vector2(side * Main.rand.NextFloat(240f, 620f), Main.rand.NextFloat(260f, 520f));
				int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)position.X, (int)position.Y, ModContent.NPCType<KrakenTentacle>(), ai0: 1, ai1: NPC.whoAmI, ai2: side);
				if (index >= 0 && index < Main.maxNPCs) {
					Main.npc[index].netUpdate = true;
				}
			}
		}

		private void SpawnTether(Player player) {
			if (Main.netMode == NetmodeID.MultiplayerClient || AnyProjectile(ModContent.ProjectileType<KrakenAbyssTether>()) || Vector2.Distance(player.Center, NPC.Center) < 700f) {
				return;
			}

			Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<KrakenAbyssTether>(), 0, 0f, Main.myPlayer, NPC.whoAmI, player.whoAmI);
		}

		private void SpawnVortexWalls(Player player, bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int type = ModContent.ProjectileType<KrakenAbyssVortex>();
			int damage = phase2 ? 220 : 170;
			float speed = phase2 ? 3.25f : 2.35f;
			int amount = phase2 ? 3 : 2;
			for (int i = 0; i < amount; i++) {
				int side = i == 0 ? -1 : 1;
				Vector2 position = PredictPlayer(player, 36f + i * 10f) + new Vector2(side * Main.rand.NextFloat(820f, 1080f), Main.rand.NextFloat(-310f, 210f));
				Vector2 velocity = new Vector2(-side * speed, -0.25f);
				float size = phase2 ? Main.rand.NextFloat(1f, 2f) : Main.rand.NextFloat(0f, 1.35f);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, type, damage, 0f, Main.myPlayer, side, size);
			}
		}

		private void SpawnMinions(bool phase2, bool largeGroup) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int current = CountNPC(ModContent.NPCType<KrakenMinion>());
			int max = Phase3 ? 48 : phase2 ? 36 : 22;
			int amount = largeGroup ? (Phase3 ? 16 : phase2 ? 14 : 8) : (Phase3 ? 9 : phase2 ? 7 : 4);
			for (int i = 0; i < amount && current < max; i++, current++) {
				Vector2 position = NPC.Center + Main.rand.NextVector2Circular(760f, 520f);
				int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)position.X, (int)position.Y, ModContent.NPCType<KrakenMinion>(), ai0: NPC.whoAmI);
				if (index >= 0 && index < Main.maxNPCs) {
					Main.npc[index].netUpdate = true;
				}
			}
		}

		private void TrySpawnClones(bool phase2) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int cloneType = ModContent.NPCType<KrakenClone>();
			int currentClones = CountNPC(cloneType);
			if (currentClones > 0 || CloneCooldown > 0f) {
				return;
			}

			float[] sizes = phase2
				? new[] { 0.44f, 0.52f, 0.6f, 0.68f, 0.76f, 0.56f }
				: new[] { 0.46f, 0.6f, 0.76f };
			int cloneAmount = sizes.Length;
			for (int i = 0; i < cloneAmount; i++) {
				float angle = MathHelper.TwoPi * i / cloneAmount;
				Vector2 position = NPC.Center + new Vector2((float)System.Math.Cos(angle) * 620f, (float)System.Math.Sin(angle) * 340f);
				int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)position.X, (int)position.Y, cloneType, ai0: NPC.whoAmI, ai1: sizes[i], ai2: angle);
				if (index >= 0 && index < Main.maxNPCs) {
					Main.npc[index].netUpdate = true;
				}
			}

			HadClones = 1f;
		}

		private void UpdateCloneCooldown() {
			if (CloneCooldown > 0f) {
				CloneCooldown--;
			}

			int currentClones = CountNPC(ModContent.NPCType<KrakenClone>());
			if (currentClones > 0) {
				HadClones = 1f;
				return;
			}

			if (HadClones == 1f) {
				HadClones = 0f;
				CloneCooldown = MathHelper.Lerp(2100f, 1500f, BattleRamp);
			}
		}

		private void SpawnWaterSpikesAroundPlayer(Player player, bool phase2, int amount) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int current = CountTentacleMode(0);
			int max = phase2 ? 6 : 4;
			for (int i = 0; i < amount && current < max; i++, current++) {
				int side = i % 2 == 0 ? -1 : 1;
				float angle = MathHelper.Lerp(-1.2f, 1.2f, amount <= 1 ? 0.5f : i / (float)(amount - 1));
				Vector2 predicted = PredictPlayer(player, 26f + i * 8f);
				Vector2 offset = new Vector2(side * Main.rand.NextFloat(420f, 680f), Main.rand.NextFloat(-260f, 320f)).RotatedBy(angle * 0.18f);
				int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(predicted.X + offset.X), (int)(predicted.Y + offset.Y), ModContent.NPCType<KrakenTentacle>(), ai0: 0, ai1: NPC.whoAmI, ai2: side);
				if (index >= 0 && index < Main.maxNPCs) {
					Main.npc[index].netUpdate = true;
				}
			}
		}

		private void SpawnTentacle(Player player, int mode) {
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				return;
			}

			int side = Main.rand.NextBool() ? -1 : 1;
			Vector2 position;
			if (mode == 0) {
				float angle = Main.rand.NextFloat(MathHelper.TwoPi);
				position = player.Center + new Vector2((float)System.Math.Cos(angle) * 680f, (float)System.Math.Sin(angle) * 430f);
			}
			else if (mode == 2) {
				position = NPC.Center + new Vector2(side * 105f, -128f);
			}
			else {
				Vector2 offset = new Vector2(Main.rand.NextFloat(-460f, 460f), 520f);
				position = player.Center + offset;
			}

			int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)position.X, (int)position.Y, ModContent.NPCType<KrakenTentacle>(), ai0: mode, ai1: NPC.whoAmI, ai2: side);
			if (index >= 0 && index < Main.maxNPCs) {
				Main.npc[index].netUpdate = true;
			}
		}

		private bool AnyTentacleMode(int mode) {
			int type = ModContent.NPCType<KrakenTentacle>();
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (npc.active && npc.type == type && (int)npc.ai[0] == mode) {
					return true;
				}
			}

			return false;
		}

		private bool AnyProjectile(int type) {
			for (int i = 0; i < Main.maxProjectiles; i++) {
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.type == type) {
					return true;
				}
			}

			return false;
		}

		private int CountNPC(int type) {
			int count = 0;
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (npc.active && npc.type == type) {
					count++;
				}
			}

			return count;
		}

		private int CountTentacleMode(int mode) {
			int count = 0;
			int type = ModContent.NPCType<KrakenTentacle>();
			for (int i = 0; i < Main.maxNPCs; i++) {
				NPC npc = Main.npc[i];
				if (npc.active && npc.type == type && (int)npc.ai[0] == mode) {
					count++;
				}
			}

			return count;
		}

		private static Vector2 PredictPlayer(Player player, float frames) {
			return player.Center + player.velocity * frames;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Vector2 center = NPC.Center + new Vector2(0f, VisualDrawOffsetY) - screenPos;

			if (State == StateEyeRush || State == StateDashChain) {
				DrawEyeRush(spriteBatch, NPC.Center - screenPos, screenPos);
				return false;
			}

			if (State == StateInvisibleSwarm) {
				DrawInvisibleCore(spriteBatch, KrakenRotatingLaser.GetKrakenHead(NPC) - screenPos);
				return false;
			}

			float scale = GetVisualScaleForLife(NPC);
			Color bodyColor = Color.White;
			GetVisualFrame(visualMotion, (int)NPC.frameCounter, out Texture2D texture, out Rectangle source);
			Vector2 origin = source.Size() * 0.5f;

			BeginSmoothKrakenBatch(spriteBatch);
			try {
				if (visualMotionBlend < 1f) {
					GetVisualFrame(previousVisualMotion, (int)NPC.frameCounter, out Texture2D previousTexture, out Rectangle previousSource);
					Vector2 previousOrigin = previousSource.Size() * 0.5f;
					spriteBatch.Draw(
						previousTexture,
						center,
						previousSource,
						bodyColor * (1f - visualMotionBlend),
						NPC.rotation,
						previousOrigin,
						scale,
						previousVisualEffects,
						0f
					);
				}

				spriteBatch.Draw(
					texture,
					center,
					source,
					bodyColor * visualMotionBlend,
					NPC.rotation,
					origin,
					scale,
					visualEffects,
					0f
				);
				DrawRubyOnBody(spriteBatch, center, scale);
			}
			finally {
				RestoreWorldBatch(spriteBatch);
			}
			return false;
		}

		private static void BeginSmoothKrakenBatch(SpriteBatch spriteBatch) {
			spriteBatch.End();
			spriteBatch.Begin(
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				SamplerState.LinearClamp,
				DepthStencilState.None,
				RasterizerState.CullCounterClockwise,
				null,
				Main.GameViewMatrix.TransformationMatrix
			);
		}

		private static void RestoreWorldBatch(SpriteBatch spriteBatch) {
			spriteBatch.End();
			spriteBatch.Begin(
				SpriteSortMode.Deferred,
				BlendState.AlphaBlend,
				Main.DefaultSamplerState,
				DepthStencilState.None,
				RasterizerState.CullCounterClockwise,
				null,
				Main.GameViewMatrix.TransformationMatrix
			);
		}

		private void GetVisualFrame(int motion, int frame, out Texture2D texture, out Rectangle source) {
			string texturePath = motion == VisualMotionHorizontal
				? "ChaoticDimensions/Content/NPCs/Kraken/KrakenBossMoveHorizontal"
				: motion == VisualMotionUp
					? "ChaoticDimensions/Content/NPCs/Kraken/KrakenBossMoveUp"
					: Texture;
			if (motion == VisualMotionIdle) {
				Texture2D forwardTexture = TextureAssets.Npc[Type].Value;
				Texture2D loopBackTexture = ModContent.Request<Texture2D>(LoopBackTexturePath).Value;
				GetAnimationFrame(forwardTexture, loopBackTexture, frame, out texture, out source);
				return;
			}

			texture = ModContent.Request<Texture2D>(texturePath).Value;
			source = GetAtlasFrameSource(texture, frame % AnimationFrames);
		}

		private Texture2D GetRubyHeadTexture() {
			string state = RubyBroken ? "Broken" : RubyOff ? "Off" : "Normal";
			return ModContent.Request<Texture2D>(
				$"ChaoticDimensions/Content/NPCs/Kraken/KrakenRubyHead{state}"
			).Value;
		}

		private void DrawRubyOnBody(SpriteBatch spriteBatch, Vector2 center, float scale) {
			Texture2D ruby = GetRubyHeadTexture();
			Vector2 origin = ruby.Size() * 0.5f;
			float phase = MathHelper.TwoPi * (float)NPC.frameCounter / LoopAnimationFrames;
			float pulse = RubyOff ? 1f : 1f + 0.045f * (float)System.Math.Sin(phase * 2f);
			Vector2 localOffset = GetRubyFrameOffset() * scale;
			if (visualEffects == SpriteEffects.FlipHorizontally) {
				localOffset.X *= -1f;
			}
			Vector2 position = center + localOffset.RotatedBy(NPC.rotation);
			float rubyScale = scale * RubySourceScale * pulse;

			if (!RubyOff) {
				Color aura = RubyBroken ? new Color(255, 20, 38, 46) : new Color(255, 0, 42, 58);
				for (int i = 0; i < 4; i++) {
					float angle = MathHelper.TwoPi * i / 4f + phase;
					spriteBatch.Draw(
						ruby,
						position + angle.ToRotationVector2() * 3.5f,
						null,
						aura,
						NPC.rotation,
						origin,
						rubyScale * 1.08f,
						SpriteEffects.None,
						0f
					);
				}
			}

			Color rubyColor = RubyOff ? Color.White * 0.82f : Color.White;
			spriteBatch.Draw(ruby, position, null, rubyColor, NPC.rotation, origin, rubyScale, SpriteEffects.None, 0f);
		}

		public static void GetAnimationFrame(Texture2D forwardTexture, Texture2D loopBackTexture, int frame, out Texture2D texture, out Rectangle source) {
			int safeFrame = frame % LoopAnimationFrames;
			if (safeFrame < 0) {
				safeFrame += LoopAnimationFrames;
			}

			if (safeFrame < ForwardAnimationFrames) {
				texture = forwardTexture;
				source = GetAtlasFrameSource(forwardTexture, safeFrame);
				return;
			}

			texture = loopBackTexture;
			source = GetAtlasFrameSource(loopBackTexture, safeFrame - ForwardAnimationFrames);
		}

		private static Rectangle GetAtlasFrameSource(Texture2D texture, int frame) {
			int frameWidth = texture.Width / AtlasColumns;
			int frameHeight = texture.Height / AtlasRows;
			int column = frame % AtlasColumns;
			int row = frame / AtlasColumns;
			return new Rectangle(column * frameWidth, row * frameHeight, frameWidth, frameHeight);
		}

		private void DrawEyes(SpriteBatch spriteBatch, Texture2D pixel, Vector2 center, float scale) {
			Color red = new Color(255, 0, 35, 230);
			spriteBatch.Draw(pixel, new Rectangle((int)(center.X - 78 * scale), (int)(center.Y - 24 * scale), (int)(22 * scale), (int)(38 * scale)), red);
			spriteBatch.Draw(pixel, new Rectangle((int)(center.X + 56 * scale), (int)(center.Y - 24 * scale), (int)(22 * scale), (int)(38 * scale)), red);
		}

		private void DrawEyeRush(SpriteBatch spriteBatch, Vector2 center, Vector2 screenPos) {
			Texture2D eye = GetRubyHeadTexture();
			float pulse = 1f + 0.18f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 18f);
			Vector2 origin = eye.Size() * 0.5f;

			Vector2 fallback = NPC.velocity;
			if (fallback.LengthSquared() > 1f) {
				fallback.Normalize();
			}
			else {
				fallback = Vector2.UnitY;
			}
			float dashRotation = fallback.ToRotation() + MathHelper.PiOver2;

			for (int i = 7; i >= 1; i--) {
				Vector2 oldCenter = NPC.oldPos.Length > i && NPC.oldPos[i] != Vector2.Zero ? NPC.oldPos[i] + NPC.Size * 0.5f - screenPos : center - fallback * i * 28f;
				float opacity = (8 - i) / 8f;
				float scale = MathHelper.Lerp(0.52f, 1.18f, opacity) * pulse;
				Color shadow = new Color(8, 18, 58, (byte)(118 * opacity));
				Color redGhost = new Color(190, 0, 42, (byte)(82 * opacity));
				spriteBatch.Draw(eye, oldCenter, null, shadow, dashRotation, origin, scale * 1.45f, SpriteEffects.None, 0f);
				spriteBatch.Draw(eye, oldCenter, null, redGhost, dashRotation, origin, scale, SpriteEffects.None, 0f);
			}

			for (int i = 0; i < 3; i++) {
				Vector2 smear = center - fallback * (18f + i * 22f);
				Color streak = new Color(18, 66, 150, (byte)(70 - i * 16));
				spriteBatch.Draw(eye, smear, null, streak, dashRotation, origin, new Vector2(1.9f + i * 0.28f, 0.7f) * pulse, SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(eye, center, null, new Color(255, 40, 62, 230), dashRotation, origin, 1.42f * pulse, SpriteEffects.None, 0f);
			spriteBatch.Draw(eye, center, null, new Color(255, 8, 28, 245), dashRotation, origin, 0.96f * pulse, SpriteEffects.None, 0f);
		}

		private void DrawInvisibleCore(SpriteBatch spriteBatch, Vector2 center) {
			Texture2D crystal = GetRubyHeadTexture();
			Vector2 origin = crystal.Size() * 0.5f;
			float pulse = 1f + 0.13f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 7f);
			for (int i = 0; i < 8; i++) {
				float angle = MathHelper.TwoPi * i / 8f + Main.GlobalTimeWrappedHourly * 0.4f;
				Vector2 offset = angle.ToRotationVector2() * 18f;
				spriteBatch.Draw(crystal, center + offset, null, new Color(255, 0, 46, 52), -angle, origin, 1.55f * pulse, SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(crystal, center, null, new Color(255, 52, 82, 245), Main.GlobalTimeWrappedHourly * 0.9f, origin, 1.28f * pulse, SpriteEffects.None, 0f);
		}
	}
}
