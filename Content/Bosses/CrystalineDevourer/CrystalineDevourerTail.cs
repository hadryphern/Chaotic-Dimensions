using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChaoticDimensions.Content.Bosses.CrystalineDevourer
{
	public sealed class CrystalineDevourerTail : ModNPC
	{
		private const float SegmentFollowDistance = 108f;

		public override void SetStaticDefaults() {
			NPCID.Sets.MustAlwaysDraw[Type] = true;
			NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<CrystalineDevourerHead>();
		}

		public override void SetDefaults() {
			NPC.width = 56;
			NPC.height = 56;
			NPC.damage = 175;
			NPC.defense = 5;
			NPC.lifeMax = CrystalineDevourerHead.SharedLifeMax;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.dontCountMe = true;
		}

		public override bool PreAI() {
			if (NPC.ai[1] < 0 || NPC.ai[1] >= Main.maxNPCs) {
				NPC.active = false;
				return false;
			}

			NPC ahead = Main.npc[(int)NPC.ai[1]];
			NPC head = NPC.realLife >= 0 && NPC.realLife < Main.maxNPCs ? Main.npc[NPC.realLife] : null;
			if (!ahead.active || head is null || !head.active) {
				NPC.active = false;
				return false;
			}

			Vector2 direction = ahead.Center - NPC.Center;
			if (direction.LengthSquared() <= 0.01f) {
				direction = (ahead.rotation - MathHelper.PiOver2).ToRotationVector2();
			}

			direction = direction.SafeNormalize(Vector2.UnitY);
			NPC.rotation = direction.ToRotation() + MathHelper.PiOver2;
			NPC.Center = ahead.Center - direction * SegmentFollowDistance;

			NPC.velocity = Vector2.Zero;
			NPC.damage = head.damage;
			NPC.defense = head.defense;
			NPC.life = head.life;
			return false;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			return false;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			cooldownSlot = ImmunityCooldownID.Bosses;
			return true;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo) {
			target.AddBuff(BuffID.Slow, 90);
		}
	}
}
