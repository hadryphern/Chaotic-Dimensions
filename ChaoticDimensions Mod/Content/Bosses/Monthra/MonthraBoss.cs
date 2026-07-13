// Implementa o padrão principal de escudo e raio lateral da Monthra.

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
	[AutoloadBossHead]
	public sealed class MonthraBoss : ModNPC
	{
		private const float DrawScale = 0.82f;
		private const int ShieldSweepDuration = 1620;
		private const int SweepBeamCastTime = 72;
		private ref float StateTimer => ref NPC.ai[1];
		private ref float HoverSide => ref NPC.ai[2];

		private float LifeRatio => NPC.lifeMax <= 0 ? 1f : NPC.life / (float)NPC.lifeMax;
		private float Rage => 1f - LifeRatio;
		private bool PhaseTwo => LifeRatio < 0.5f;
		private bool FinalPhase => LifeRatio < 0.2f;
		private bool LowerShieldActive => StateTimer >= 18f && StateTimer <= ShieldSweepDuration - 70f;
		private int SweepCycleDuration => (int)MathHelper.Lerp(2220f, 1720f, Rage);

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

		// Atualiza a raiva pela vida perdida e executa o padrão novo.
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

			RunShieldSweep(player);

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
			bool resting = StateTimer > ShieldSweepDuration;
			Vector2 offset = resting
				? new Vector2(HoverSide * 520f, -420f + (float)System.Math.Sin(StateTimer * 0.035f) * 60f)
				: new Vector2(HoverSide * 180f * (float)System.Math.Sin(StateTimer * 0.018f), -385f);
			float speed = resting ? 19f + Rage * 8f : 15f + Rage * 7f;
			float turn = resting ? 0.12f : 0.075f;
			SteerTowards(player.Center + offset, speed, turn);
			NPC.velocity *= 0.96f;

			if (Main.netMode != NetmodeID.MultiplayerClient && StateTimer == SweepBeamCastTime) {
				SpawnShieldSweepBeam(player);
			}

			if (StateTimer >= SweepCycleDuration) {
				HoverSide *= -1f;
				StateTimer = 0f;
				NPC.netUpdate = true;
			}
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
			float fadeOut = 1f - Utils.GetLerpValue(ShieldSweepDuration - 95f, ShieldSweepDuration, StateTimer, true);
			return fadeIn * fadeOut;
		}

		private static void DrawShieldAura(SpriteBatch spriteBatch, Vector2 center, float opacity) {
			Texture2D pixel = TextureAssets.MagicPixel.Value;
			Vector2 radius = new(300f, 220f);
			Vector2 auraCenter = center + new Vector2(0f, 45f);
			float pulse = 0.82f + 0.18f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 7.5f);
			Color arcColor = new Color(255, 38, 218) * (0.42f * opacity * pulse);
			Color tickColor = new Color(255, 118, 236) * (0.34f * opacity);
			const int segments = 30;

			Vector2 previous = Vector2.Zero;
			for (int i = 0; i <= segments; i++) {
				float t = i / (float)segments;
				float angle = MathHelper.Lerp(MathHelper.ToRadians(18f), MathHelper.ToRadians(162f), t);
				Vector2 normal = new((float)System.Math.Cos(angle), (float)System.Math.Sin(angle));
				Vector2 point = auraCenter + new Vector2(normal.X * radius.X, normal.Y * radius.Y);
				if (i > 0) {
					DrawPixelLine(spriteBatch, pixel, previous, point, arcColor, 2.6f);
				}

				if (i % 3 == 0) {
					Vector2 spike = point + normal * (22f + 8f * pulse);
					DrawPixelLine(spriteBatch, pixel, point, spike, tickColor, 1.7f);
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
