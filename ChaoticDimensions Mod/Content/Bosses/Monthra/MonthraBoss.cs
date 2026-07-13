// Implementa os dois padrões principais de combate e a progressao de raiva da Monthra.

using System.IO;
using ChaoticDimensions.Common.Systems;
using ChaoticDimensions.Content.Items.Materials;
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
		ShieldSweep,
		HaloNeedleBarrage
	}

	[AutoloadBossHead]
	public sealed class MonthraBoss : ModNPC
	{
		private const float DrawScale = 0.82f;
		private const int ShieldSweepDuration = 1620;
		private const int HaloNeedleDuration = 520;
		private ref float State => ref NPC.ai[0];
		private ref float StateTimer => ref NPC.ai[1];
		private ref float HoverSide => ref NPC.ai[2];

		private float LifeRatio => NPC.lifeMax <= 0 ? 1f : NPC.life / (float)NPC.lifeMax;
		private float Rage => 1f - LifeRatio;
		private bool PhaseTwo => LifeRatio < 0.5f;
		private bool FinalPhase => LifeRatio < 0.2f;
		private bool LowerShieldActive => StateTimer >= 18f
			&& ((MonthraAttackState)(int)State == MonthraAttackState.ShieldSweep && StateTimer <= ShieldSweepDuration - 70f
				|| (MonthraAttackState)(int)State == MonthraAttackState.HaloNeedleBarrage && StateTimer <= HaloNeedleDuration - 45f);

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

		// Atualiza a raiva pela vida perdida e executa um dos dois estados novos.
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
				case MonthraAttackState.HaloNeedleBarrage:
					RunHaloNeedleBarrage(player);
					break;
				default:
					RunShieldSweep(player);
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

		// Mantem a boss protegida por baixo enquanto um raio lateral fecha a arena.
		private void RunShieldSweep(Player player) {
			Vector2 offset = new(HoverSide * 210f * (float)System.Math.Sin(StateTimer * 0.018f), -385f);
			SteerTowards(player.Center + offset, 16f + Rage * 8f, 0.08f);
			NPC.velocity *= 0.96f;

			if (Main.netMode != NetmodeID.MultiplayerClient && StateTimer == 48f) {
				SpawnShieldSweepBeam(player);
			}

			if (Main.netMode != NetmodeID.MultiplayerClient && StateTimer > 170f && StateTimer < ShieldSweepDuration - 160f && (int)StateTimer % GetNeedleInterval(92) == 0) {
				SpawnHaloNeedleWave(player, PhaseTwo ? 3 : 2, 0.38f);
			}

			if (StateTimer >= ShieldSweepDuration) {
				HoverSide *= -1f;
				SwitchState(MonthraAttackState.HaloNeedleBarrage);
			}
		}

		// Dispara ondas de agulhas finas a partir do halo inferior da boss.
		private void RunHaloNeedleBarrage(Player player) {
			Vector2 orbit = new Vector2(360f + Rage * 140f, -335f).RotatedBy(StateTimer * (0.022f + Rage * 0.012f) * HoverSide);
			SteerTowards(player.Center + orbit, 22f + Rage * 12f, 0.12f + Rage * 0.035f);

			int interval = GetNeedleInterval(56);
			if (Main.netMode != NetmodeID.MultiplayerClient && StateTimer >= 38f && StateTimer <= HaloNeedleDuration - 70f && (int)StateTimer % interval == 0) {
				SpawnHaloNeedleWave(player, FinalPhase ? 7 : PhaseTwo ? 6 : 5, 0.82f);
			}

			if (StateTimer >= HaloNeedleDuration) {
				HoverSide *= -1f;
				SwitchState(MonthraAttackState.ShieldSweep);
			}
		}

		private int GetNeedleInterval(int baseInterval) {
			return System.Math.Max(24, (int)MathHelper.Lerp(baseInterval, baseInterval * 0.56f, Rage));
		}

		private void SpawnHaloNeedleWave(Player player, int count, float spread) {
			Vector2 predicted = player.Center + player.velocity * MathHelper.Lerp(16f, 30f, Rage);
			float start = -spread;
			float end = spread;
			for (int i = 0; i < count; i++) {
				float t = count == 1 ? 0.5f : i / (float)(count - 1);
				float arc = MathHelper.Lerp(start, end, t);
				Vector2 origin = NPC.Center + new Vector2(arc * 360f, 175f + 42f * (float)System.Math.Sin(StateTimer * 0.07f + i));
				Vector2 aimPoint = predicted + new Vector2((t - 0.5f) * 180f, Main.rand.NextFloat(-80f, 80f));
				Vector2 velocity = (aimPoint - origin).SafeNormalize(Vector2.UnitY) * MathHelper.Lerp(19f, 27f, Rage);
				int damage = PhaseTwo ? 245 : 200;
				float curve = MathHelper.Lerp(-0.0028f, 0.0028f, t) * HoverSide;
				Projectile.NewProjectile(NPC.GetSource_FromAI(), origin, velocity, ModContent.ProjectileType<MonthraHaloNeedle>(), damage, 0f, Main.myPlayer, player.whoAmI, curve);
			}

			SoundEngine.PlaySound(SoundID.Item29 with { Pitch = 0.48f, Volume = 0.9f }, NPC.Center);
		}

		private void SpawnShieldSweepBeam(Player player) {
			float side = Main.rand.NextBool() ? -1f : 1f;
			float spawnDistance = System.Math.Max(1280f, Main.screenWidth) * 0.5f + 620f;
			Vector2 spawn = player.Center + new Vector2(side * spawnDistance, -40f);
			int damage = PhaseTwo ? 330 : 270;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), spawn, Vector2.Zero, ModContent.ProjectileType<MonthraSweepBeam>(), damage, 0f, Main.myPlayer, side, NPC.whoAmI);
			SoundEngine.PlaySound(SoundID.Item122 with { Pitch = 0.35f, Volume = 1.05f }, spawn);
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

		private bool IsProtectedByLowerShield(Vector2 source) {
			if (!LowerShieldActive || source.Y < NPC.Center.Y - 35f) {
				return false;
			}

			Vector2 offset = source - NPC.Center;
			float ellipse = offset.X * offset.X / (430f * 430f) + offset.Y * offset.Y / (330f * 330f);
			return ellipse <= 1.18f;
		}

		public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers) {
			if (IsProtectedByLowerShield(player.Center)) {
				modifiers.FinalDamage *= 0.22f;
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers) {
			if (projectile.hostile || projectile.owner < 0 || projectile.owner >= Main.maxPlayers || !Main.player[projectile.owner].active) {
				return;
			}

			if (IsProtectedByLowerShield(projectile.Center)) {
				modifiers.FinalDamage *= 0.22f;
			}
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
			float shieldOpacity = GetShieldOpacity();
			if (shieldOpacity > 0f) {
				DrawShieldAura(spriteBatch, NPC.Center - screenPos, shieldOpacity * 0.72f);
			}

			spriteBatch.Draw(texture, NPC.Center - screenPos, source, NPC.GetAlpha(drawColor), NPC.rotation, origin, DrawScale, effects, 0f);
			if (shieldOpacity > 0f) {
				DrawShieldAura(spriteBatch, NPC.Center - screenPos, shieldOpacity);
			}

			return false;
		}

		private float GetShieldOpacity() {
			if (!LowerShieldActive) {
				return 0f;
			}

			float fadeIn = Utils.GetLerpValue(0f, 70f, StateTimer, true);
			float duration = (MonthraAttackState)(int)State == MonthraAttackState.ShieldSweep ? ShieldSweepDuration : HaloNeedleDuration;
			float fadeOut = 1f - Utils.GetLerpValue(duration - 95f, duration, StateTimer, true);
			return fadeIn * fadeOut;
		}

		private static void DrawShieldAura(SpriteBatch spriteBatch, Vector2 center, float opacity) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Vector2 radius = new(360f, 290f);
			Vector2 auraCenter = center + new Vector2(0f, 38f);
			float pulse = 0.86f + 0.14f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 7.5f);
			Color arcColor = new Color(255, 54, 218) * (0.52f * opacity * pulse);
			Color coreColor = new Color(255, 214, 249) * (0.68f * opacity);
			const int segments = 34;

			Vector2 previous = Vector2.Zero;
			for (int i = 0; i <= segments; i++) {
				float t = i / (float)segments;
				float angle = MathHelper.Lerp(MathHelper.ToRadians(14f), MathHelper.ToRadians(166f), t);
				Vector2 normal = new((float)System.Math.Cos(angle), (float)System.Math.Sin(angle));
				Vector2 point = auraCenter + new Vector2(normal.X * radius.X, normal.Y * radius.Y);
				if (i > 0) {
					DrawPixelLine(spriteBatch, pixel, previous, point, arcColor, 5f);
					DrawPixelLine(spriteBatch, pixel, previous, point, coreColor, 1.4f);
				}

				if (i % 3 == 0) {
					Vector2 spike = point + normal * (28f + 10f * pulse);
					DrawPixelLine(spriteBatch, pixel, point, spike, new Color(255, 42, 226) * (0.42f * opacity), 2f);
				}

				previous = point;
			}
		}

		private static void DrawPixelLine(SpriteBatch spriteBatch, Texture2D pixel, Vector2 start, Vector2 end, Color color, float width) {
			Vector2 edge = end - start;
			if (edge.LengthSquared() <= 0.01f) {
				return;
			}

			spriteBatch.Draw(pixel, start, null, color, edge.ToRotation(), new Vector2(0f, 0.5f), new Vector2(edge.Length(), width), SpriteEffects.None, 0f);
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
