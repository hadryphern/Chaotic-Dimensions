// Implementa os seis estados de combate e a progressao de raiva da Monthra.

using System.IO;
using ChaoticDimensions.Common.Systems;
using ChaoticDimensions.Content.Items.Materials;
using ChaoticDimensions.Content.NPCs.Monthra;
using ChaoticDimensions.Content.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Bosses.Monthra
{
	internal enum MonthraAttackState
	{
		HoverVolley,
		DashChain,
		PrismaticPursuit,
		LightLattice,
		SolarSpiral,
		ButterflySwarm,
		PrismRain,
		ConvergingWings
	}

	[AutoloadBossHead]
	public sealed class MonthraBoss : ModNPC
	{
		private const float DrawScale = 0.82f;
		private ref float State => ref NPC.ai[0];
		private ref float StateTimer => ref NPC.ai[1];
		private ref float HoverSide => ref NPC.ai[2];

		private float LifeRatio => NPC.lifeMax <= 0 ? 1f : NPC.life / (float)NPC.lifeMax;
		private float Rage => 1f - LifeRatio;
		private bool PhaseTwo => LifeRatio < 0.5f;
		private bool FinalPhase => LifeRatio < 0.2f;

		// Regista metadados que nao mudam durante a execucao.
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 12;
			NPCID.Sets.BossBestiaryPriority.Add(Type);
			NPCID.Sets.MustAlwaysDraw[Type] = true;
		}

		// Define os valores usados quando esta entidade e criada.
		public override void SetDefaults() {
			NPC.width = 520;
			NPC.height = 400;
			NPC.damage = 270;
			NPC.defense = 80;
			NPC.lifeMax = 5000000;
			NPC.knockBackResist = 0f;
			NPC.value = Item.buyPrice(platinum: 1);
			NPC.npcSlots = 16f;
			NPC.boss = true;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			Music = 0;
		}

		// Ajusta vida e dano ao modo de dificuldade e ao numero de jogadores.
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment) {
			NPC.lifeMax = (int)(NPC.lifeMax * balance * 0.82f);
			NPC.damage = (int)(NPC.damage * 0.92f);
		}

		// Atualiza a raiva pela vida perdida e executa um dos seis estados.
		public override void AI() {
			if (!TargetOrDespawn()) {
				return;
			}

			if (HoverSide == 0f) {
				HoverSide = Main.rand.NextBool() ? 1f : -1f;
			}

			Player player = Main.player[NPC.target];
			StateTimer++;
			NPC.damage = (int)MathHelper.Lerp(270f, 350f, Rage);

			switch ((MonthraAttackState)(int)State) {
				case MonthraAttackState.DashChain:
					RunDashChain(player);
					break;
				case MonthraAttackState.PrismaticPursuit:
					RunPrismaticPursuit(player);
					break;
				case MonthraAttackState.LightLattice:
					RunLightLattice(player);
					break;
				case MonthraAttackState.SolarSpiral:
					RunSolarSpiral(player);
					break;
				case MonthraAttackState.ButterflySwarm:
					RunButterflySwarm(player);
					break;
				case MonthraAttackState.PrismRain:
					RunPrismRain(player);
					break;
				case MonthraAttackState.ConvergingWings:
					RunConvergingWings(player);
					break;
				default:
					RunHoverVolley(player);
					break;
			}

			NPC.spriteDirection = NPC.velocity.X >= 0f ? -1 : 1;
			NPC.rotation = MathHelper.Clamp(NPC.velocity.X * 0.009f, -0.36f, 0.36f);
			Lighting.AddLight(NPC.Center, 0.28f, 0.06f, 0.24f);
		}

		private bool TargetOrDespawn() {
			NPC.TargetClosest(false);
			if (NPC.target >= 0 && NPC.target < Main.maxPlayers) {
				Player player = Main.player[NPC.target];
				if (player.active && !player.dead) {
					return true;
				}
			}

			NPC.velocity.Y -= 0.4f;
			NPC.EncourageDespawn(10);
			return false;
		}

		// Encurta cada estado gradualmente quando a Monthra perde vida.
		private int Duration(int baseDuration) {
			return (int)MathHelper.Lerp(baseDuration, baseDuration * 0.62f, Rage);
		}

		// Mantem voo lateral e dispara pequenos leques de lanças curvas.
		private void RunHoverVolley(Player player) {
			Vector2 offset = new(330f * HoverSide, -245f + (float)System.Math.Sin(StateTimer * 0.095f) * 55f);
			SteerTowards(player.Center + offset, 20f + Rage * 13f, 0.1f + Rage * 0.045f);
			int interval = System.Math.Max(32, (int)MathHelper.Lerp(52f, 34f, Rage));
			if (Main.netMode != NetmodeID.MultiplayerClient && StateTimer > 16f && (int)StateTimer % interval == 0) {
				FirePrismaticLanceVolley(player, PhaseTwo ? 5 : 4, 19f + Rage * 5f, 19f + Rage * 7f, PhaseTwo ? 195 : 160, 0.65f);
			}

			if (StateTimer >= Duration(155)) {
				SwitchState(MonthraAttackState.DashChain);
			}
		}

		// Alterna preparação, dash e uma janela curta de recuperação.
		private void RunDashChain(Player player) {
			int cycle = System.Math.Max(23, (int)MathHelper.Lerp(38f, 24f, Rage));
			int tick = (int)StateTimer % cycle;
			if (tick < 11) {
				Vector2 staging = player.Center + new Vector2(-HoverSide * 560f, -120f + (float)System.Math.Sin(StateTimer * 0.12f) * 70f);
				SteerTowards(staging, 25f + Rage * 12f, 0.16f);
			}
			else if (tick == 11) {
				Vector2 predicted = player.Center + player.velocity * MathHelper.Lerp(10f, 18f, Rage);
				NPC.velocity = (predicted - NPC.Center).SafeNormalize(Vector2.UnitX) * MathHelper.Lerp(48f, 72f, Rage);
				SoundEngine.PlaySound(SoundID.Item74 with { Pitch = 0.18f, Volume = 0.9f }, NPC.Center);
				NPC.netUpdate = true;
			}
			else if (tick > cycle - 8) {
				NPC.velocity *= 0.86f;
			}

			if (Main.netMode != NetmodeID.MultiplayerClient && tick == cycle - 6) {
				FirePrismaticLanceVolley(player, PhaseTwo ? 4 : 3, 21f + Rage * 4f, 16f, PhaseTwo ? 205 : 170, 0.35f);
			}

			if (StateTimer >= Duration(210)) {
				HoverSide *= -1f;
				SwitchState(MonthraAttackState.PrismaticPursuit);
			}
		}

		// Os raios acompanham o jogador apenas durante o aviso e depois fixam a direção.
		private void RunPrismaticPursuit(Player player) {
			Vector2 offset = new(-HoverSide * 390f, -320f);
			SteerTowards(player.Center + offset, 22f + Rage * 12f, 0.11f);
			int interval = FinalPhase ? 72 : PhaseTwo ? 84 : 98;
			if (Main.netMode != NetmodeID.MultiplayerClient && (StateTimer == 24f || (StateTimer > 24f && (int)(StateTimer - 24f) % interval == 0))) {
				SpawnPrismaticFan(player);
			}

			if (StateTimer >= Duration(205)) {
				SwitchState(MonthraAttackState.LightLattice);
			}
		}

		// Cria dois corredores finos de luz com uma zona segura larga.
		private void RunLightLattice(Player player) {
			Vector2 watch = player.Center + new Vector2(HoverSide * 430f, -360f);
			SteerTowards(watch, 18f + Rage * 9f, 0.09f);
			if (Main.netMode != NetmodeID.MultiplayerClient && (StateTimer == 24f || StateTimer == 112f)) {
				SpawnLightLattice(player);
			}
			if (Main.netMode != NetmodeID.MultiplayerClient && (StateTimer == 76f || StateTimer == 164f)) {
				FireHomingShot(player, PhaseTwo ? 225 : 185);
			}
			if (StateTimer >= Duration(190)) {
				SwitchState(MonthraAttackState.SolarSpiral);
			}
		}

		// Espalha poucas lanças em espiral, com espaço constante para atravessar a onda.
		private void RunSolarSpiral(Player player) {
			Vector2 anchor = player.Center + new Vector2(0f, -330f);
			SteerTowards(anchor, 17f + Rage * 6f, 0.12f);
			NPC.velocity *= 0.94f;

			int interval = FinalPhase ? 18 : PhaseTwo ? 21 : 24;
			if (Main.netMode != NetmodeID.MultiplayerClient && StateTimer >= 32f && StateTimer <= 220f && (int)StateTimer % interval == 0) {
				int arms = FinalPhase ? 3 : 2;
				float baseAngle = StateTimer * (0.115f + Rage * 0.035f);
				for (int i = 0; i < arms; i++) {
					float angle = baseAngle + MathHelper.TwoPi * i / arms;
					Vector2 direction = angle.ToRotationVector2();
					Vector2 origin = NPC.Center + direction * 105f;
					Vector2 velocity = direction * (18f + Rage * 4f);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), origin, velocity, ModContent.ProjectileType<MonthraPrismaticLance>(), PhaseTwo ? 205 : 170, 0f, Main.myPlayer, 0.15f, HoverSide * 0.002f);
				}
			}

			if (StateTimer >= Duration(255)) {
				SwitchState(MonthraAttackState.ButterflySwarm);
			}
		}

		// Orbita o jogador sem acumular uma parede de minions e projéteis.
		private void RunButterflySwarm(Player player) {
			Vector2 orbit = new Vector2(410f, 0f).RotatedBy(StateTimer * (0.045f + Rage * 0.025f) * HoverSide);
			SteerTowards(player.Center + orbit, 23f + Rage * 14f, 0.12f);
			if (Main.netMode != NetmodeID.MultiplayerClient && StateTimer == 20f) {
				SpawnMothSwarm(FinalPhase ? 8 : PhaseTwo ? 7 : 6);
			}
			if (Main.netMode != NetmodeID.MultiplayerClient && StateTimer > 45f && (int)StateTimer % 58 == 0) {
				FireHomingShot(player, PhaseTwo ? 220 : 180);
			}
			if (StateTimer >= Duration(185)) {
				SwitchState(MonthraAttackState.PrismRain);
			}
		}

		// Faz chover lanças que corrigem a mira apenas no início da queda.
		private void RunPrismRain(Player player) {
			Vector2 anchor = player.Center + new Vector2(HoverSide * 260f, -520f);
			SteerTowards(anchor, 22f + Rage * 8f, 0.11f);
			int interval = FinalPhase ? 18 : PhaseTwo ? 22 : 26;
			if (Main.netMode != NetmodeID.MultiplayerClient && StateTimer >= 28f && StateTimer <= 176f && (int)StateTimer % interval == 0) {
				SpawnPrismRain(player, FinalPhase ? 3 : 2);
			}
			if (StateTimer >= Duration(215)) {
				SwitchState(MonthraAttackState.ConvergingWings);
			}
		}

		// Fecha pares de lanças pelos flancos, deixando uma pausa clara entre ondas.
		private void RunConvergingWings(Player player) {
			Vector2 watch = player.Center + new Vector2(-HoverSide * 430f, -280f);
			SteerTowards(watch, 20f + Rage * 10f, 0.1f);
			if (Main.netMode != NetmodeID.MultiplayerClient && (StateTimer == 34f || StateTimer == 108f)) {
				SpawnConvergingWingPair(player);
			}
			if (Main.netMode != NetmodeID.MultiplayerClient && (StateTimer == 70f || StateTimer == 144f)) {
				FirePrismaticLanceVolley(player, PhaseTwo ? 5 : 4, 22f, 14f, PhaseTwo ? 235 : 195, 0.45f);
			}
			if (StateTimer >= Duration(190)) {
				HoverSide *= -1f;
				SwitchState(MonthraAttackState.HoverVolley);
			}
		}

		private void SpawnPrismaticFan(Player player) {
			Vector2 predicted = player.Center + player.velocity * (14f + Rage * 8f);
			float centerAngle = (predicted - NPC.Center).ToRotation();
			int count = FinalPhase ? 5 : PhaseTwo ? 4 : 3;
			float spread = FinalPhase ? 0.56f : 0.42f;
			for (int i = 0; i < count; i++) {
				float t = count == 1 ? 0.5f : i / (float)(count - 1);
				float angle = centerAngle + MathHelper.Lerp(-spread, spread, t);
				float turn = 0.008f + Rage * 0.004f;
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(turn, 0f), ModContent.ProjectileType<MonthraPrismaticRay>(), PhaseTwo ? 245 : 205, 0f, Main.myPlayer, angle, 2200f, 1f);
			}
			SoundEngine.PlaySound(SoundID.Item29 with { Pitch = 0.34f, Volume = 1.05f }, NPC.Center);
		}

		private void SpawnLightLattice(Player player) {
			Vector2 safe = player.Center + player.velocity * 12f;
			const float safeWidth = 420f;
			const float safeHeight = 320f;
			for (int side = -1; side <= 1; side += 2) {
				SpawnLatticeRay(new Vector2(safe.X, safe.Y + side * safeHeight * 0.5f), 0f);
				SpawnLatticeRay(new Vector2(safe.X + side * safeWidth * 0.5f, safe.Y), MathHelper.PiOver2);
			}
			SoundEngine.PlaySound(SoundID.Item162 with { Pitch = 0.12f, Volume = 0.92f }, safe);
		}

		private void SpawnLatticeRay(Vector2 center, float rotation) {
			Projectile.NewProjectile(NPC.GetSource_FromAI(), center, Vector2.Zero, ModContent.ProjectileType<MonthraPrismaticRay>(), PhaseTwo ? 260 : 220, 0f, Main.myPlayer, rotation, -1500f, 0f);
		}

		private void SpawnPrismRain(Player player, int count) {
			Vector2 predicted = player.Center + player.velocity * 12f;
			for (int i = 0; i < count; i++) {
				float lane = count == 1 ? 0f : MathHelper.Lerp(-210f, 210f, i / (float)(count - 1));
				Vector2 origin = predicted + new Vector2(lane + Main.rand.NextFloat(-45f, 45f), -780f - i * 70f);
				Vector2 velocity = (predicted + new Vector2(lane * 0.2f, 0f) - origin).SafeNormalize(Vector2.UnitY) * (20f + Rage * 5f);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), origin, velocity, ModContent.ProjectileType<MonthraPrismaticLance>(), PhaseTwo ? 220 : 185, 0f, Main.myPlayer, 0.8f, HoverSide * 0.0015f);
			}
		}

		private void SpawnConvergingWingPair(Player player) {
			Vector2 predicted = player.Center + player.velocity * (14f + Rage * 6f);
			for (int side = -1; side <= 1; side += 2) {
				for (int lane = -1; lane <= 1; lane += 2) {
					Vector2 origin = predicted + new Vector2(side * 860f, lane * 310f);
					Vector2 velocity = (predicted + new Vector2(0f, lane * 85f) - origin).SafeNormalize(Vector2.UnitX) * (21f + Rage * 5f);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), origin, velocity, ModContent.ProjectileType<MonthraPrismaticLance>(), PhaseTwo ? 235 : 195, 0f, Main.myPlayer, 0.25f, -side * lane * 0.0018f);
				}
			}
			SoundEngine.PlaySound(SoundID.Item29 with { Pitch = 0.5f, Volume = 0.85f }, predicted);
		}

		private void SpawnMothSwarm(int count) {
			for (int i = 0; i < count; i++) {
				Vector2 spawn = NPC.Center + Main.rand.NextVector2CircularEdge(210f, 150f);
				int index = NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawn.X, (int)spawn.Y, ModContent.NPCType<MonthraMothMinion>(), ai0: NPC.whoAmI);
				if (index >= 0 && index < Main.maxNPCs) {
					Main.npc[index].netUpdate = true;
				}
			}
		}

		private void FirePrismaticLanceVolley(Player player, int count, float speed, float spreadDegrees, int damage, float homing) {
			Vector2 direction = (player.Center + player.velocity * 10f - NPC.Center).SafeNormalize(Vector2.UnitY);
			for (int i = 0; i < count; i++) {
				float t = count == 1 ? 0.5f : i / (float)(count - 1);
				Vector2 velocity = direction.RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(-spreadDegrees, spreadDegrees, t))) * speed;
				float curve = MathHelper.Lerp(-0.0018f, 0.0018f, t);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<MonthraPrismaticLance>(), damage, 0f, Main.myPlayer, homing, curve);
			}
			SoundEngine.PlaySound(SoundID.Item125 with { Pitch = 0.32f, Volume = 0.82f }, NPC.Center);
		}

		private void FireHomingShot(Player player, int damage) {
			Vector2 direction = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 11f, ModContent.ProjectileType<MonthraFireballHoming>(), damage, 0f, Main.myPlayer);
		}

		private void SteerTowards(Vector2 target, float speed, float turnRate) {
			Vector2 desired = (target - NPC.Center).SafeNormalize(Vector2.UnitY) * speed;
			NPC.velocity = Vector2.Lerp(NPC.velocity, desired, turnRate);
			if (NPC.velocity.Length() > speed) {
				NPC.velocity = NPC.velocity.SafeNormalize(Vector2.UnitY) * speed;
			}
		}

		// Muda de ataque e reinicia o temporizador sincronizado.
		private void SwitchState(MonthraAttackState next) {
			State = (float)next;
			StateTimer = 0f;
			NPC.netUpdate = true;
		}

		// Define as recompensas entregues ao derrotar o NPC.
		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MonthraScale>(), 1, 55, 75));
			npcLoot.Add(ItemDropRule.Common(ItemID.SuperHealingPotion, 1, 8, 14));
		}

		// Escolhe o frame da animacao apresentado neste tick.
		public override void FindFrame(int frameHeight) {
			NPC.frameCounter++;
			int frameDelay = FinalPhase ? 2 : PhaseTwo ? 3 : 4;
			if (NPC.frameCounter >= frameDelay) {
				NPC.frameCounter = 0;
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (frameHeight * Main.npcFrameCount[Type]);
			}
		}

		// Desenha o recurso manualmente quando o desenho padrao nao e suficiente.
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Texture2D texture = TextureAssets.Npc[Type].Value;
			int frameHeight = texture.Height / Main.npcFrameCount[Type];
			Rectangle source = new(0, NPC.frame.Y, texture.Width, frameHeight);
			Vector2 origin = source.Size() * 0.5f;
			SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(texture, NPC.Center - screenPos, source, NPC.GetAlpha(drawColor), NPC.rotation, origin, DrawScale, effects, 0f);
			return false;
		}

		// Envia o estado adicional necessario no modo multijogador.
		public override void SendExtraAI(BinaryWriter writer) => writer.Write(HoverSide);
		// Recebe o estado adicional enviado pela rede.
		public override void ReceiveExtraAI(BinaryReader reader) => HoverSide = reader.ReadSingle();

		// Atualiza o estado do mundo quando a entidade e derrotada.
		public override void OnKill() {
			NPC.SetEventFlagCleared(ref ChaoticDownedBossSystem.downedMonthra, -1);
		}

		public override bool CheckActive() => false;
	}
}
