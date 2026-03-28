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
		HoverVolley,
		SweepingBurst
	}

	[AutoloadBossHead]
	public sealed class MonthraBoss : ModNPC
	{
		private const int HoverDuration = 180;
		private const int SweepDuration = 110;
		private const float DrawScale = 0.48f;

		private ref float State => ref NPC.ai[0];
		private ref float StateTimer => ref NPC.ai[1];
		private ref float HoverSide => ref NPC.ai[2];

		private bool PhaseTwo => NPC.life < NPC.lifeMax * 0.5f;

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 12;
			NPCID.Sets.BossBestiaryPriority.Add(Type);
			NPCID.Sets.MustAlwaysDraw[Type] = true;
		}

		public override void SetDefaults() {
			NPC.width = 340;
			NPC.height = 260;
			NPC.damage = 30;
			NPC.defense = 10;
			NPC.lifeMax = 22000;
			NPC.knockBackResist = 0f;
			NPC.value = Item.buyPrice(gold: 5);
			NPC.npcSlots = 10f;
			NPC.boss = true;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			Music = MusicID.Boss3;
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment) {
			NPC.lifeMax = (int)(NPC.lifeMax * balance * 0.88f);
			NPC.damage = (int)(NPC.damage * 0.95f);
		}

		public override void AI() {
			TargetOrDespawn();
			if (!NPC.active) {
				return;
			}

			if (HoverSide == 0f) {
				HoverSide = Main.rand.NextBool() ? 1f : -1f;
			}

			Player player = Main.player[NPC.target];
			StateTimer++;

			switch ((MonthraAttackState)(int)State) {
				case MonthraAttackState.SweepingBurst:
					RunSweepingBurst(player);
					break;
				default:
					RunHoverVolley(player);
					break;
			}

			NPC.spriteDirection = NPC.velocity.X >= 0f ? -1 : 1;
			NPC.rotation = NPC.velocity.X * 0.018f;

			Lighting.AddLight(NPC.Center, 0.06f, 0.09f, 0.22f);
		}

		private void TargetOrDespawn() {
			NPC.TargetClosest(false);
			if (NPC.target < 0 || NPC.target == 255) {
				NPC.velocity.Y -= 0.18f;
				NPC.EncourageDespawn(10);
				return;
			}

			Player player = Main.player[NPC.target];
			if (player.active && !player.dead) {
				return;
			}

			NPC.TargetClosest(false);
			player = Main.player[NPC.target];
			if (!player.active || player.dead) {
				NPC.velocity.Y -= 0.18f;
				NPC.EncourageDespawn(10);
			}
		}

		private void RunHoverVolley(Player player) {
			Vector2 hoverOffset = new Vector2(205f * HoverSide, -185f + (float)System.Math.Sin(StateTimer * 0.06f) * 24f);
			SteerTowards(player.Center + hoverOffset, PhaseTwo ? 8.6f : 7.35f, 0.062f);

			int volleyInterval = PhaseTwo ? 36 : 50;
			if (Main.netMode != NetmodeID.MultiplayerClient && StateTimer > 18f && StateTimer % volleyInterval == 0f) {
				FireRegularVolley(player, PhaseTwo ? 5 : 4, PhaseTwo ? 10.15f : 8.75f, PhaseTwo ? 16f : 13f, PhaseTwo ? 21 : 18);
			}

			if (StateTimer >= HoverDuration) {
				SwitchState(MonthraAttackState.SweepingBurst);
			}
		}

		private void RunSweepingBurst(Player player) {
			Vector2 sweepTarget = player.Center + new Vector2(-HoverSide * 310f, -95f);
			SteerTowards(sweepTarget, PhaseTwo ? 10.9f : 9.45f, 0.09f);

			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (StateTimer == 26f || (PhaseTwo && StateTimer == 54f)) {
					FireHomingShot(player, 19);
				}

				if (StateTimer == 70f) {
					FireRegularVolley(player, PhaseTwo ? 6 : 5, PhaseTwo ? 10.8f : 9.4f, 18f, PhaseTwo ? 20 : 17);
				}
			}

			if (StateTimer >= SweepDuration) {
				HoverSide *= -1f;
				SwitchState(MonthraAttackState.HoverVolley);
			}
		}

		private void FireRegularVolley(Player player, int count, float speed, float spreadDegrees, int damage) {
			Vector2 direction = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
			if (count == 1) {
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * speed, ModContent.ProjectileType<MonthraFireball>(), damage, 0f, Main.myPlayer);
				return;
			}

			for (int i = 0; i < count; i++) {
				float interpolation = count == 1 ? 0.5f : i / (float)(count - 1);
				float rotation = MathHelper.Lerp(-spreadDegrees, spreadDegrees, interpolation);
				Vector2 velocity = direction.RotatedBy(MathHelper.ToRadians(rotation)) * speed;
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<MonthraFireball>(), damage, 0f, Main.myPlayer);
			}

			SoundEngine.PlaySound(SoundID.Item20 with { Pitch = -0.15f, Volume = 1.05f }, NPC.Center);
		}

		private void FireHomingShot(Player player, int damage) {
			Vector2 direction = (player.Center - NPC.Center).SafeNormalize(Vector2.UnitY);
			Vector2 velocity = direction * 7.25f;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velocity, ModContent.ProjectileType<MonthraFireballHoming>(), damage, 0f, Main.myPlayer);
			SoundEngine.PlaySound(SoundID.Item33 with { Pitch = 0.2f, Volume = 1.05f }, NPC.Center);
		}

		private void SteerTowards(Vector2 targetPosition, float moveSpeed, float turnRate) {
			Vector2 desiredVelocity = (targetPosition - NPC.Center).SafeNormalize(Vector2.UnitY) * moveSpeed;
			NPC.velocity = Vector2.Lerp(NPC.velocity, desiredVelocity, turnRate);
			if (NPC.velocity.Length() > moveSpeed) {
				NPC.velocity = NPC.velocity.SafeNormalize(Vector2.UnitY) * moveSpeed;
			}
		}

		private void SwitchState(MonthraAttackState nextState) {
			State = (float)nextState;
			StateTimer = 0f;
			NPC.netUpdate = true;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MonthraScale>(), 1, 22, 30));
			npcLoot.Add(ItemDropRule.Common(ItemID.HealingPotion, 1, 5, 10));
		}

		public override void FindFrame(int frameHeight) {
			NPC.frameCounter++;
			if (NPC.frameCounter >= 5) {
				NPC.frameCounter = 0;
				NPC.frame.Y += frameHeight;
				if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[Type]) {
					NPC.frame.Y = 0;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			Texture2D texture = TextureAssets.Npc[Type].Value;
			int frameHeight = texture.Height / Main.npcFrameCount[Type];
			Rectangle source = new Rectangle(0, NPC.frame.Y, texture.Width, frameHeight);
			Vector2 origin = source.Size() * 0.5f;
			SpriteEffects effects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			spriteBatch.Draw(texture, NPC.Center - screenPos, source, NPC.GetAlpha(drawColor), NPC.rotation, origin, DrawScale, effects, 0f);
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(HoverSide);
		}

		public override void ReceiveExtraAI(BinaryReader reader) {
			HoverSide = reader.ReadSingle();
		}

		public override void OnKill() {
			NPC.SetEventFlagCleared(ref ChaoticDownedBossSystem.downedMonthra, -1);
		}

		public override bool CheckActive() => false;
	}
}
